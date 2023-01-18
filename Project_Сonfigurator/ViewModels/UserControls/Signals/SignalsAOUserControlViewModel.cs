﻿using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsAOUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public SignalsAOUserControlViewModel()
        {
            Title = "Сигналы AO";
            Description = "Аналоговые сигналы (AO)";
            UsingUserControl = new SignalsAOUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly IDBService DBServices;
        private readonly ISignalService SignalServices;
        public SignalsAOUserControlViewModel(IUserDialogService _UserDialog, IDBService _IDBService, ISignalService _ISignalService) : this()
        {
            UserDialog = _UserDialog;
            DBServices = _IDBService;
            SignalServices = _ISignalService;
            _ParamsDataView.Filter += ParamsFiltered;
        }
        #endregion

        #region Параметры

        #region Состояние активной вкладки
        private bool _IsSelected = false;
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public override bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (Set(ref _IsSelected, value))
                {
                    if (Set(ref _IsSelected, value))
                    {
                        if (SelectedParam is not null)
                            SignalServices.RedefineSignal(SelectedParam.Signal, _IsSelected, Title);
                        DoSelection = SignalServices.DoSelection;
                        if (_IsSelected)
                            RefreshDataView();
                    }
                }
            }
        }
        #endregion

        #region Список сигналов AO
        private ObservableCollection<SignalAO> _Params = new();
        /// <summary>
        /// Список сигналов AO
        /// </summary>
        public ObservableCollection<SignalAO> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (_Params is null || _Params.Count <= 0)
                    {
                        CreateData();
                        RefreshDataView();
                    }
                    else RefreshDataView();
                }
            }
        }
        #endregion

        #region Выбранный сигнал AO
        private SignalAO _SelectedParam = new();
        /// <summary>
        /// Выбранный сигнал AO
        /// </summary>
        public SignalAO SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
        }
        #endregion

        #region Текст фильтрации
        private string _TextFilter;
        /// <summary>
        /// Текст фильтрации
        /// </summary>
        public string TextFilter
        {
            get => _TextFilter;
            set => Set(ref _TextFilter, value);
        }
        #endregion

        #region Состояние необходимости выбора сигнала
        private bool _DoSelection;
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        public bool DoSelection
        {
            get => _DoSelection;
            set => Set(ref _DoSelection, value);
        }
        #endregion

        #region Коллекция сигналов AO для отображения
        /// <summary>
        /// Коллекция сигналов AO для отображения
        /// </summary>
        private readonly CollectionViewSource _ParamsDataView = new();
        public ICollectionView ParamsDataView => _ParamsDataView?.View;
        #endregion

        #endregion

        #region Команды

        #region Команда - Генерировать таблицу
        private ICommand _CmdGeneratedTable;
        /// <summary>
        /// Команда - Генерировать таблицу
        /// </summary>
        public ICommand CmdGeneratedTable => _CmdGeneratedTable ??= new RelayCommand(OnCmdGeneratedTableExecuted, CanCmdGeneratedTableExecute);
        private bool CanCmdGeneratedTableExecute() => true;

        private void OnCmdGeneratedTableExecuted()
        {
            #region Импорт сигналов из ТБ
            IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            TableSignalsUserControlViewModel MyViewModel = new();

            foreach (var _TabItem in from object _Item in _ViewModels
                                     let _TabItem = _Item as TableSignalsUserControlViewModel
                                     where _TabItem is not null
                                     select _TabItem)
                MyViewModel = _TabItem;


            if (MyViewModel is null) return;
            if (MyViewModel.Params is null) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            var _Params = new ObservableCollection<SignalAO>();
            foreach (var _USO in MyViewModel.Params)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Type == TypeModule.AO)
                        {
                            if (_Module.Channels is null || _Module.Channels.Count <= 0)
                                if (UserDialog.SendMessage("Внимание!", "Неверные данные таблицы сигналов. Проверьте вкладку",
                                    MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;

                            foreach (var Channel in _Module.Channels)
                            {
                                if ((string.IsNullOrWhiteSpace(Channel.Id) && string.IsNullOrWhiteSpace(Channel.Description)) ||
                                    Channel.Description == "Резерв") continue;

                                var Address = 0;
                                if (int.TryParse(Channel.Address, out int address))
                                    Address = address - 300000;

                                var param = new SignalAO
                                {
                                    Signal = new BaseSignal
                                    {
                                        Index = $"{_Params.Count + 1}",
                                        Id = Channel.Id,
                                        Description = Channel.Description,
                                        Area = "",
                                        Address = $"{Address}",
                                        VarName = $"di_shared[{_Params.Count + 1}]",

                                    }
                                };
                                _Params.Add(param);
                            }
                        }
                    }
                }
            }
            Params = new ObservableCollection<SignalAO>(_Params);
            UserDialog.SendMessage("Импорт сигналов AO", "Сигналы AO успешно импортированы\nиз таблицы сигналов",
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            #endregion

            CreateData();
            RefreshDataView();
        }
        #endregion

        #region Команда - Обновить фильтр
        private ICommand _CmdRefreshFilter;
        /// <summary>
        /// Команда - Обновить фильтр
        /// </summary>
        public ICommand CmdRefreshFilter => _CmdRefreshFilter ??= new RelayCommand(OnCmdRefreshFilterExecuted, CanCmdRefreshFilterExecute);
        private bool CanCmdRefreshFilterExecute() => true;

        private void OnCmdRefreshFilterExecuted()
        {
            RefreshDataView();
        }
        #endregion

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Signal.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            SignalServices.DoSelection = true;
            SignalServices.ListName = Title;
            SignalServices.Type = TypeModule.AO;

            var NameListSelected = "Таблица сигналов";
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;
            if (!SignalServices.DoSelection) return;

            if (Index != SelectedParam.Signal.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            SignalServices.Address = SelectedParam.Signal.Index;
            SignalServices.Id = SelectedParam.Signal.Id;
            SignalServices.Description = SelectedParam.Signal.Description;

            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == SignalServices.ListName
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация сигналов AO
        /// <summary>
        /// Фильтрация сигналов AO
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalAO _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Сигналы AO
            if (_Param.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                _Param.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            while (Params.Count < 500)
            {
                var param = new SignalAO
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{Params.Count + 1}",
                        Id = "",
                        Description = "",
                        Area = "",
                        Address = "",
                        VarName = $"ao_shared[{Params.Count + 1}]",
                        LinkValue = ""
                    }
                };
                Params.Add(param);
            }
            if (Params.Count > 0)
                SelectedParam = Params[0];
        }
        #endregion

        #region Обновляем данные для отображения
        /// <summary>
        /// Обновляем данные для отображения
        /// </summary>
        private void RefreshDataView()
        {
            _ParamsDataView.Source = Params;
            _ParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(ParamsDataView));
        }
        #endregion

        #endregion
    }
}
