﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class SignalingUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public SignalingUserControlViewModel()
        {
            Title = "Сигнализация";
            Description = "Сигнализация и общесистемная диагностика";
            UsingUserControl = new SignalingUserControl();
        }

        public LayotRackUserControlViewModel LayotRackViewModel { get; }

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        private readonly IUserDialogService UserDialog;
        public SignalingUserControlViewModel(
            ISignalService signalService,
            IDBService dBService,
            IUserDialogService userDialog,
            LayotRackUserControlViewModel layotRackViewModel
            ) : this()
        {
            _SignalService = signalService;
            _DBService = dBService;
            UserDialog = userDialog;
            LayotRackViewModel = layotRackViewModel;

            _DataView.Filter += OnSignalsFiltered;
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
                    if (SelectedParam is null) return;
                    _SignalService.RedefineParam(SelectedParam.Param, _IsSelected, Title);
                    DoSelection = _SignalService.DoSelection;
                    if (_IsSelected)
                        _DataView.View.Refresh();
                }
            }
        }
        #endregion

        #region Список параметров
        private ObservableCollection<BaseSignaling> _Signaling = new();
        /// <summary>
        /// Список параметров
        /// </summary>
        public ObservableCollection<BaseSignaling> Signaling
        {
            get => _Signaling;
            set => Set(ref _Signaling, value);
        }
        #endregion

        #region Коллекция параметров
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный параметр
        private BaseSignaling _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseSignaling SelectedParam
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

        #endregion

        #region Команды

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
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;

            var data_list = (ObservableCollection<BaseSignaling>)_DataView.Source ?? new ObservableCollection<BaseSignaling>();
            if (Index != SelectedParam.Param.Index)
                SelectedParam = data_list[int.Parse(Index) - 1];

            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedParam.Param.TypeSignal) || int.Parse(SelectedParam.Param.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                _SignalService.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedParam.Param.TypeSignal) > 1)
            {
                NameListSelected = "Сигналы AI";
                _SignalService.Type = TypeModule.AI;
            }
            else if (int.Parse(SelectedParam.Param.TypeSignal) > 0)
            {
                NameListSelected = "Группы сигналов";
                _SignalService.Type = TypeModule.DI;
            }

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать цвет сообщения
        private ICommand _CmdSelecteColor;
        /// <summary>
        /// Команда - Выбрать цвет сообщения
        /// </summary>
        public ICommand CmdSelecteColor => _CmdSelecteColor ??= new RelayCommand(OnCmdSelecteColorExecuted, CanCmdSelecteColorExecute);
        private bool CanCmdSelecteColorExecute(object p) => p is DataGrid;

        private void OnCmdSelecteColorExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #region Команда - Выбрать УСО
        private ICommand _CmdSelecteUSO;
        /// <summary>
        /// Команда - Выбрать УСО
        /// </summary>
        public ICommand CmdSelecteUSO => _CmdSelecteUSO ??= new RelayCommand(OnCmdSelecteUSOExecuted, CanCmdSelecteUSOExecute);
        private bool CanCmdSelecteUSOExecute(object p) => p is DataGrid;

        private void OnCmdSelecteUSOExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #region Команда - Выбрать УСО
        private ICommand _CmdImportServiceSignal;
        /// <summary>
        /// Команда - Выбрать УСО
        /// </summary>
        public ICommand CmdImportServiceSignal => _CmdImportServiceSignal ??= new RelayCommand(OnCmdImportServiceSignalExecuted, CanCmdImportServiceSignalExecute);
        private bool CanCmdImportServiceSignalExecute(object p) => LayotRackViewModel is not null;

        private void OnCmdImportServiceSignalExecuted(object p)
        {
            if (LayotRackViewModel.USOList is null) return;
            if (LayotRackViewModel.USOList.Count <= 0) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;


            #region Генерируем регистры формируемые
            Signaling = new();
            for (int i = 0; i < 58; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    var param = new BaseSignaling
                    {
                        Param = new BaseParam
                        {
                            Index = $"{Signaling.Count + 1}",
                            Id = "",
                            Description = "",
                            VarName = $"list5_param[{Signaling.Count + 1}]",
                            Inv = "",
                            TypeSignal = "",
                            Address = ""
                        },
                        Color = "",
                        IndexUSO = "",
                        TypeWarning = "1",
                        VarNameVU = $"LIST5[{i + 1}]"
                    };
                    Signaling.Add(param);
                }
            }
            #endregion

            #region Переописываем сигналы диагностики
            var USOList = LayotRackViewModel.USOList;
            var index = 0;
            bool flNeedShift;
            var USONameException = new ObservableCollection<string>();
            foreach (var _USO in USOList)
                if (_USO.Racks is null || _USO.Racks.Count <= 0)
                    USONameException.Add(_USO.Name);

            foreach (var _USO in USOList)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    flNeedShift = false;
                    foreach (var _Module in _Rack.Modules)
                    {
                        foreach (var _Channel in _Module.Channels)
                        {
                            if (_Channel.Id.Contains("CSC", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var flTmp = false;

                                foreach (var item in USONameException)
                                {
                                    if (_Channel.Description.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                                        flTmp = true;


                                }
                                if (!flTmp)
                                {
                                    flNeedShift = true;
                                    Signaling[index].Param.Id = _Channel.Id;
                                    Signaling[index].Param.Description = _Channel.Description;
                                    Signaling[index].IndexUSO = _USO.Index;

                                    Signaling[index].Color = "Красный";
                                    if (_Channel.Description.Contains("Двер", StringComparison.CurrentCultureIgnoreCase))
                                        Signaling[index].Color = "Желтый";

                                    index++;
                                }
                            }
                        }
                    }

                    if (flNeedShift)
                        index += 5;
                }
            }
            #endregion

            #region Ищем имена УСО исключенй
            if (USONameException is not null && USONameException.Count > 0)
            {
                foreach (var item in USONameException)
                {
                    foreach (var _USO in USOList)
                    {
                        foreach (var _Rack in _USO.Racks)
                        {
                            flNeedShift = false;
                            foreach (var _Module in _Rack.Modules)
                            {
                                foreach (var _Channel in _Module.Channels)
                                {

                                    if (_Channel.Id.Contains("CSC", StringComparison.CurrentCultureIgnoreCase))
                                    {

                                        if (_Channel.Description.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            flNeedShift = true;
                                            Signaling[index].Param.Id = _Channel.Id;
                                            Signaling[index].Param.Description = _Channel.Description;

                                            Signaling[index].Color = "Красный";
                                            if (_Channel.Description.Contains("Двер", StringComparison.CurrentCultureIgnoreCase))
                                                Signaling[index].Color = "Желтый";

                                            foreach (var __USO in USOList)
                                                if (__USO.Name == item)
                                                    Signaling[index].IndexUSO = __USO.Index;
                                            index++;
                                        }
                                    }
                                }
                            }

                            if (flNeedShift)
                                index += 5;
                        }
                    }
                }
            }
            #endregion

            GeneratedData();

        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        private void OnSignalsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseSignaling Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Param.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Param.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация параметров
        public void GeneratedData()
        {
            _DataView.Source = Signaling;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
