﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsDIUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public SignalsDIUserControlViewModel()
        {
            Title = "Сигналы DI";
            Description = "Дискретные сигналы (DI)";
            UsingUserControl = new SignalsDIUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }

        public SignalsDIUserControlViewModel(
            IUserDialogService userDialog,
            ISignalService signalService,
            IDBService dBService,
            TableSignalsUserControlViewModel tableSignalsViewModel
            ) : this()
        {
            UserDialog = userDialog;
            TableSignalsViewModel = tableSignalsViewModel;
            _SignalService = signalService;
            _DBService = dBService;

            _DataView.Filter += OnSignalsDIFiltered;
            _DBService.RefreshDataViewModel(this, false);
        }
        #endregion

        #region Параметры

        #region Состояние активной вкладки
        private bool _IsSelected = false;
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (Set(ref _IsSelected, value))
                {
                    if (SelectedSignalDI is not null)
                        _SignalService.RedefineSignal(SelectedSignalDI.Signal, _IsSelected, Title);
                    DoSelection = _SignalService.DoSelection;
                    if (_IsSelected)
                        _DataView.View?.Refresh();
                }
            }
        }
        #endregion

        #region Список сигналов DI
        private ObservableCollection<SignalDI> _SignalsDI = new();
        /// <summary>   
        /// Список сигналов DI
        /// </summary>
        public ObservableCollection<SignalDI> SignalsDI
        {
            get => _SignalsDI;
            set => Set(ref _SignalsDI, value);
        }
        #endregion

        #region Коллекция сигналов DI
        /// <summary>
        /// Коллекция сигналов DI
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный сигнал DI
        private SignalDI _SelectedSignalDI = new();
        /// <summary>
        /// Выбранный сигнал DI
        /// </summary>
        public SignalDI SelectedSignalDI
        {
            get => _SelectedSignalDI;
            set => Set(ref _SelectedSignalDI, value);
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
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None)) return;

            GeneratedSignals();

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
            if (_DataView.Source is null) return;
            _DataView.View.Refresh();

        }
        #endregion

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedSignalDI is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignalDI is null) return;

            var data_list = new ObservableCollection<SignalDI>();
            foreach (SignalDI SignalDI in DataView)
            {
                data_list.Add(SignalDI);
            }

            if (Index != SelectedSignalDI.Signal.Index)
                SelectedSignalDI = data_list[int.Parse(Index) - 1];

            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.DI;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedSignalDI.Signal.Area) || int.Parse(SelectedSignalDI.Signal.Area) == 0)
                NameListSelected = "Таблица сигналов";
            else if (int.Parse(SelectedSignalDI.Signal.Area) == 1)
                NameListSelected = "DI формируемые";

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
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
        private bool CanCmdSelectionSignalExecute(object p) => SelectedSignalDI is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignalDI is null) return;
            if (App.FucusedTabControl == null) return;
            if (!_SignalService.DoSelection) return;

            var data_list = new ObservableCollection<SignalDI>();
            foreach (SignalDI Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedSignalDI.Signal.Index)
                SelectedSignalDI = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedSignalDI.Signal.Index;
            _SignalService.Id = SelectedSignalDI.Signal.Id;
            _SignalService.Description = SelectedSignalDI.Signal.Description;

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == _SignalService.ListName
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        private void OnSignalsDIFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalDI Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        private void GeneratedSignals()
        {
            var index = 0;
            SignalsDI = new();

            #region Генерируем сигналы DI, при отсутсвии данных во владке Таблица сигналов
            if (TableSignalsViewModel is null || TableSignalsViewModel.DataViewModules is null)
            {
                while (SignalsDI.Count < 2500)
                {
                    var signal = new SignalDI()
                    {
                        Signal = new BaseSignal
                        {
                            Index = $"{++index}",
                            Id = "",
                            Description = "",
                            VarName = $"di_shared[{index}]",
                            Area = "",
                            Address = "",
                            LinkValue = ""
                        }
                    };
                    SignalsDI.Add(signal);
                }

                SelectedSignalDI = SignalsDI[0];
                _DataView.Source = SignalsDI;
                _DataView.View.Refresh();
                OnPropertyChanged(nameof(DataView));
                return;
            }
            #endregion

            #region Генерируем сигналы DI, созданные во вкладке Таблица сигналов

            #region Генерируем данные из ТБ
            var uso_list = TableSignalsViewModel.LayotRackViewModel.Params;
            foreach (var _USO in uso_list)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Type == TypeModule.DI)
                        {
                            foreach (var _Channel in _Module.Channels)
                            {
                                if (!string.IsNullOrWhiteSpace(_Channel.Id) && (_Channel.Description != "Резерв" || _Channel.Description != "резерв"))
                                {
                                    var signal = new SignalDI()
                                    {
                                        Signal = new BaseSignal
                                        {
                                            Index = $"{++index}",
                                            Id = _Channel.Id,
                                            Description = _Channel.Description,
                                            VarName = $"di_shared[{index}]",
                                            Area = "",
                                            Address = $"{int.Parse(_Channel.Address) - 100000}",
                                        }
                                    };
                                    SignalsDI.Add(signal);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Дозаполняем таблицу
            while (SignalsDI.Count < 2500)
            {
                var signal = new SignalDI()
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"di_shared[{index}]",
                        Area = "",
                        Address = "",
                        LinkValue = ""
                    }
                };
                SignalsDI.Add(signal);
            }
            #endregion

            #endregion

            SelectedSignalDI = SignalsDI[0];
            _DataView.Source = SignalsDI;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #region Генерируем данные
        public void GeneratedData()
        {
            _DataView.Source = SignalsDI;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion

        #endregion
    }
}
