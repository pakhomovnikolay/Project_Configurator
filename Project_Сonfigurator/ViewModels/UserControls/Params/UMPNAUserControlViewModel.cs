﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UMPNAUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public UMPNAUserControlViewModel()
        {
            Title = "Настройки МПНА";
            Description = "Текущие массивы состояний МПНА. НЕ ЗАБУДЬТЕ ЗАПОЛНИТЬ НАСТРОЙКИ КАРТ";
            UsingUserControl = new UMPNAUserControl();
        }

        private readonly ISignalService SignalServices;
        public UMPNAUserControlViewModel(ISignalService _ISignalService) : this()
        {
            SignalServices = _ISignalService;
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
                if (Set(ref _IsSelected, value, DontСommitСhanges: true))
                {
                    if (DoSelectionInputParam) DoSelectionInputParam = SignalServices.RedefineAddress(SelectedInputParam, _IsSelected, Title);
                    if (DoSelectionOutputParam) DoSelectionOutputParam = SignalServices.RedefineAddress(SelectedOutputParam, _IsSelected, Title);
                    if (DoSelectionKGMPNA) DoSelectionKGMPNA = SignalServices.RedefineAddress(SelectedKGMPNA.Param, _IsSelected, Title);
                    if (DoSelectionKTPRA) DoSelectionKTPRA = SignalServices.RedefineAddress(SelectedKTPRA.Param, _IsSelected, Title);
                    if (DoSelectionKTPRAS) DoSelectionKTPRAS = SignalServices.RedefineAddress(SelectedKTPRAS.Param, _IsSelected, Title);
                    DoSelection = DoSelectionInputParam || DoSelectionOutputParam || DoSelectionKGMPNA || DoSelectionKTPRA || DoSelectionKTPRAS;
                }
            }
        }
        #endregion

        #region Состояние необходимости выбора сигнала
        private bool _DoSelection;
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        public override bool DoSelection
        {
            get => _DoSelection;
            set => Set(ref _DoSelection, value);
        }
        #endregion

        #region Список МПНА
        private ObservableCollection<BaseUMPNA> _Params = new();
        /// <summary>
        /// Список МПНА
        /// </summary>
        public ObservableCollection<BaseUMPNA> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                    if (_Params is null || _Params.Count <= 0)
                        SelectedParam = null;
            }
        }
        #endregion

        #region Выбранный МПНА
        private BaseUMPNA _SelectedParam;
        /// <summary>
        /// Выбранный МПНА
        /// </summary>
        public BaseUMPNA SelectedParam
        {
            get => _SelectedParam;
            set
            {
                if (Set(ref _SelectedParam, value))
                {
                    if (_SelectedParam?.KTPRA.Count > 0)
                    {
                        _ParamsDataView.Source = _SelectedParam.KTPRA;
                        _ParamsDataView.View?.Refresh();
                        OnPropertyChanged(nameof(ParamsDataView));
                    }
                }

            }
        }
        #endregion

        #region Выбранный входной параметр
        private BaseParam _SelectedInputParam;
        /// <summary>
        /// Выбранный входной параметр
        /// </summary>
        public BaseParam SelectedInputParam
        {
            get => _SelectedInputParam;
            set => Set(ref _SelectedInputParam, value);
        }
        #endregion

        #region Выбранный выходной параметр
        private BaseParam _SelectedOutputParam;
        /// <summary>
        /// Выбранный выходной параметр
        /// </summary>
        public BaseParam SelectedOutputParam
        {
            get => _SelectedOutputParam;
            set => Set(ref _SelectedOutputParam, value);
        }
        #endregion

        #region Выбранный параметр агрегатных готовностей
        private BaseKGMPNA _SelectedKGMPNA;
        /// <summary>
        /// Выбранный параметр агрегатных готовностей
        /// </summary>
        public BaseKGMPNA SelectedKGMPNA
        {
            get => _SelectedKGMPNA;
            set => Set(ref _SelectedKGMPNA, value);
        }
        #endregion

        #region Выбранный параметр агрегатных защит
        private BaseKTPRA _SelectedKTPRA;
        /// <summary>
        /// Выбранный параметр агрегатных защит
        /// </summary>
        public BaseKTPRA SelectedKTPRA
        {
            get => _SelectedKTPRA;
            set => Set(ref _SelectedKTPRA, value);
        }
        #endregion

        #region Выбранный параметр предельных параметров
        private BaseKTPRAS _SelectedKTPRAS;
        /// <summary>
        /// Выбранный параметр предельных параметров
        /// </summary>
        public BaseKTPRAS SelectedKTPRAS
        {
            get => _SelectedKTPRAS;
            set => Set(ref _SelectedKTPRAS, value);
        }
        #endregion

        #region Состояние необходимости выбора входного параметра
        private bool _DoSelectionInputParam;
        /// <summary>
        /// Состояние необходимости выбора входного параметра
        /// </summary>
        public bool DoSelectionInputParam
        {
            get => _DoSelectionInputParam;
            set => Set(ref _DoSelectionInputParam, value);
        }
        #endregion

        #region Состояние необходимости выбора выходного параметра
        private bool _DoSelectionOutputParam;
        /// <summary>
        /// Состояние необходимости выбора выходного параметра
        /// </summary>
        public bool DoSelectionOutputParam
        {
            get => _DoSelectionOutputParam;
            set => Set(ref _DoSelectionOutputParam, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра карты готовности
        private bool _DoSelectionKGMPNA;
        /// <summary>
        /// Состояние необходимости выбора параметра карты готовности
        /// </summary>
        public bool DoSelectionKGMPNA
        {
            get => _DoSelectionKGMPNA;
            set => Set(ref _DoSelectionKGMPNA, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра карты защит
        private bool _DoSelectionKTPRA;
        /// <summary>
        /// Состояние необходимости выбора параметра карты защит
        /// </summary>
        public bool DoSelectionKTPRA
        {
            get => _DoSelectionKTPRA;
            set => Set(ref _DoSelectionKTPRA, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра предельных параметров
        private bool _DoSelectionKTPRAS;
        /// <summary>
        /// Состояние необходимости выбора параметра предельных параметров
        /// </summary>
        public bool DoSelectionKTPRAS
        {
            get => _DoSelectionKTPRAS;
            set => Set(ref _DoSelectionKTPRAS, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Добавить МПНА
        private ICommand _CmdAddUMPNA;
        /// <summary>
        /// Команда - Добавить МПНА
        /// </summary>
        public ICommand CmdAddUMPNA => _CmdAddUMPNA ??= new RelayCommand(OnCmdAddUMPNAExecuted, CanCmdAddUMPNAExecute);
        private bool CanCmdAddUMPNAExecute() => true;

        private void OnCmdAddUMPNAExecuted()
        {
            CreateParam();
        }
        #endregion

        #region Команда - удалить МПНА
        private ICommand _CmdDeleteUMPNA;
        /// <summary>
        /// Команда - удалить МПНА
        /// </summary>
        public ICommand CmdDeleteUMPNA => _CmdDeleteUMPNA ??= new RelayCommand(OnCmdDeleteUMPNAExecuted, CanCmdDeleteUMPNAExecute);
        private bool CanCmdDeleteUMPNAExecute(object p) => SelectedParam is not null;
        private void OnCmdDeleteUMPNAExecuted(object p)
        {
            if (p is not DataGrid SettingDataGrid) return;

            var index = Params.IndexOf(SelectedParam);
            index = index == 0 ? index : index - 1;

            Params.Remove(SelectedParam);
            if (Params.Count > 0)
                SelectedParam = Params[index];

            RefreshIndex(index);
            SettingDataGrid.Items.Refresh();
        }
        #endregion

        #region Команда - Импортировать МПНА из таблицы сигналов
        private ICommand _CmdImportUMPNA;
        /// <summary>
        /// Команда - Импортировать МПНА из таблицы сигналов
        /// </summary>
        public ICommand CmdImportUMPNA => _CmdImportUMPNA ??= new RelayCommand(OnCmdImportUMPNAExecuted, CanCmdImportUMPNAExecute);
        private bool CanCmdImportUMPNAExecute() => true;

        private void OnCmdImportUMPNAExecuted()
        {
            #region Импорт сигналов из ТБ
            if (!UserDialog.SendMessage("Внимание!", "Перед продолжением убедитесь, что ТБ заполнена.\nВсе данные будут потеряны! Продолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            Params = new();
            if (UserDialog.SearchControlViewModel("Таблица сигналов") is not TableSignalsUserControlViewModel _TabItem) return;
            TableSignalsUserControlViewModel TableSignalsViewModel = _TabItem;

            if (TableSignalsViewModel.Params is null || TableSignalsViewModel.Params.Count <= 0)
                if (!UserDialog.SendMessage("Внимание!", "Пожалуйста, проверьте ТБ",
                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;

            #region Импортируем МНА
            foreach (var _Params in TableSignalsViewModel.Params)
            {
                foreach (var _Rack in _Params.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Channels is null || _Module.Channels.Count <= 0) continue;
                        foreach (var Channel in _Module.Channels)
                        {
                            if ((Channel.Description.Contains("мна", StringComparison.CurrentCultureIgnoreCase) ||
                                Channel.Description.Contains("магистраль", StringComparison.CurrentCultureIgnoreCase)) &&
                                !Channel.Description.Contains("резерв", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var name = "";
                                var index_dot = Channel.Description.IndexOf(".");
                                var qty = Channel.Description.Length;
                                if (index_dot > 0) name = Channel.Description.Remove(index_dot, qty - index_dot);

                                var fl_tmp = false;
                                foreach (var _Param in Params)
                                    if (name.Contains(_Param.Description, StringComparison.CurrentCultureIgnoreCase))
                                        fl_tmp = true;

                                if (!fl_tmp && !string.IsNullOrWhiteSpace(name))
                                    ImportUMPNA(name);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Импортируем ПНА
            foreach (var _Params in TableSignalsViewModel.Params)
            {
                foreach (var _Rack in _Params.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Channels is null || _Module.Channels.Count <= 0) continue;
                        foreach (var Channel in _Module.Channels)
                        {
                            if ((Channel.Description.Contains("пна", StringComparison.CurrentCultureIgnoreCase) ||
                                Channel.Description.Contains("подпор", StringComparison.CurrentCultureIgnoreCase)) &&
                                !Channel.Description.Contains("резерв", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var name = "";
                                var index_dot = Channel.Description.IndexOf(".");
                                var qty = Channel.Description.Length;
                                if (index_dot > 0) name = Channel.Description.Remove(index_dot, qty - index_dot);

                                var fl_tmp = false;
                                foreach (var _Param in Params)
                                    if (name.Contains(_Param.Description, StringComparison.CurrentCultureIgnoreCase))
                                        fl_tmp = true;

                                if (!fl_tmp && !string.IsNullOrWhiteSpace(name))
                                    ImportUMPNA(name);
                            }
                        }
                    }
                }
            }
            #endregion

            if (Params.Count > 0)
                SelectedParam = Params[0];
            #endregion
        }
        #endregion

        #region Команда - Сменить адрес входного параметра
        private ICommand _CmdChangeAddressInputParam;
        /// <summary>
        /// Команда - Сменить адрес входного параметра
        /// </summary>
        public ICommand CmdChangeAddressInputParam => _CmdChangeAddressInputParam ??= new RelayCommand(OnCmdChangeAddressInputParamExecuted, CanCmdChangeAddressInputParamExecute);
        private bool CanCmdChangeAddressInputParamExecute(object p) => SelectedInputParam is not null;

        private void OnCmdChangeAddressInputParamExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedInputParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedInputParam.Index)
                SelectedInputParam = SelectedParam.InputParam[int.Parse(Index) - 1];

            DoSelectionInputParam = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedInputParam.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес выходного параметра
        private ICommand _CmdChangeAddressOutputParam;
        /// <summary>
        /// Команда - Сменить адрес выходного параметра
        /// </summary>
        public ICommand CmdChangeAddressOutputParam => _CmdChangeAddressOutputParam ??= new RelayCommand(OnCmdChangeAddressOutputParamExecuted, CanCmdChangeAddressOutputParamExecute);
        private bool CanCmdChangeAddressOutputParamExecute(object p) => SelectedOutputParam is not null;

        private void OnCmdChangeAddressOutputParamExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedOutputParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedOutputParam.Index)
                SelectedOutputParam = SelectedParam.OutputParam[int.Parse(Index) - 1];

            DoSelectionOutputParam = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, TypeAddress.DO);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра карты готовностей
        private ICommand _CmdChangeAddressKGMPNA;
        /// <summary>
        /// Команда - Сменить адрес параметра карты готовностей
        /// </summary>
        public ICommand CmdChangeAddressKGMPNA => _CmdChangeAddressKGMPNA ??= new RelayCommand(OnCmdChangeAddressKGMPNAExecuted, CanCmdChangeAddressKGMPNAExecute);
        private bool CanCmdChangeAddressKGMPNAExecute(object p) => SelectedKGMPNA is not null;

        private void OnCmdChangeAddressKGMPNAExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedKGMPNA is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedKGMPNA.Param.Index)
                SelectedKGMPNA = SelectedParam.KGMPNA[int.Parse(Index) - 1];

            DoSelectionKGMPNA = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedKGMPNA.Param.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра карты защит
        private ICommand _CmdChangeAddressKTPRA;
        /// <summary>
        /// Команда - Сменить адрес параметра карты защит
        /// </summary>
        public ICommand CmdChangeAddressKTPRA => _CmdChangeAddressKTPRA ??= new RelayCommand(OnCmdChangeAddressKTPRAExecuted, CanCmdChangeAddressKTPRAExecute);
        private bool CanCmdChangeAddressKTPRAExecute(object p) => SelectedKTPRA is not null;

        private void OnCmdChangeAddressKTPRAExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedKTPRA is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedKTPRA.Param.Index)
                SelectedKTPRA = SelectedParam.KTPRA[int.Parse(Index) - 1];

            DoSelectionKTPRA = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedKTPRA.Param.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра предельных параметров
        private ICommand _CmdChangeAddressKTPRAS;
        /// <summary>
        /// Команда - Сменить адрес параметра предельных параметров
        /// </summary>
        public ICommand CmdChangeAddressKTPRAS => _CmdChangeAddressKTPRAS ??= new RelayCommand(OnCmdChangeAddressKTPRASExecuted, CanCmdChangeAddressKTPRASExecute);
        private bool CanCmdChangeAddressKTPRASExecute(object p) => SelectedKTPRAS is not null;

        private void OnCmdChangeAddressKTPRASExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedKTPRAS is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedKTPRAS.Param.Index)
                SelectedKTPRAS = SelectedParam.KTPRAS[int.Parse(Index) - 1];

            DoSelectionKTPRAS = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedKTPRAS.Param.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать состояние сработки защиты
        private ICommand _CmdSelecteStateUMPNA;
        /// <summary>
        /// Команда - Выбрать состояние сработки защиты
        /// </summary>
        public ICommand CmdSelecteStateUMPNA => _CmdSelecteStateUMPNA ??= new RelayCommand(OnCmdSelecteStateUMPNAExecuted, CanCmdSelecteStateUMPNAExecute);
        private bool CanCmdSelecteStateUMPNAExecute(object p) => p is DataGrid;

        private void OnCmdSelecteStateUMPNAExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #endregion

        #region Функции

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public override ObservableCollection<T> GetParams<T>() => Params as ObservableCollection<T>;
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<BaseUMPNA>;
        #endregion

        #region Создание параметра
        private void CreateParam()
        {
            var index = Params.Count + 1;
            var index_setpoints = (index - 1) * App.Settings.Config.UMPNA.Setpoints.Count;
            var index_input_param = (index - 1) * App.Settings.Config.UMPNA.InputParams.Count;
            var index_output_param = (index - 1) * App.Settings.Config.UMPNA.OutputParams.Count;

            var DefualtMapKGMPNA = App.Settings.Config.DefualtMapKGMPNA.Count > 0 ? App.Settings.Config.DefualtMapKGMPNA : null;
            var DefualtMapKTPRA = App.Settings.Config.DefualtMapKTPRA.Count > 0 ? App.Settings.Config.DefualtMapKTPRA : null;
            var DefualtMapKTPRAS = App.Settings.Config.DefualtMapKTPRAS.Count > 0 ? App.Settings.Config.DefualtMapKTPRAS : null;

            var InputParam = new ObservableCollection<BaseParam>();
            var OutputParam = new ObservableCollection<BaseParam>();
            var Setpoints = new ObservableCollection<BaseSetpoints>();
            var KGMPNA = new ObservableCollection<BaseKGMPNA>();
            var KTPRA = new ObservableCollection<BaseKTPRA>();
            var KTPRAS = new ObservableCollection<BaseKTPRAS>();

            #region Создаем МПНА

            #region Входные параметры
            for (int i = 0; i < App.Settings.Config.UMPNA.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UMPNA.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < App.Settings.Config.UMPNA.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UMPNA.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < App.Settings.Config.UMPNA.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_NA[{index_setpoints + i + 1}]",
                    Id = $"H{6000 + index_setpoints + i}",
                    Unit = App.Settings.Config.UMPNA.Setpoints[i].Unit,
                    Value = App.Settings.Config.UMPNA.Setpoints[i].Value,
                    Address = $"%MW{5000 + index_setpoints + i}",
                    Description = App.Settings.Config.UMPNA.Setpoints[i].Description
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Параметры готовностей
            for (int i = 0; i < 48; i++)
            {
                var j = (index - 1) * 48 + i;
                var Param = new BaseKGMPNA
                {
                    NoMasked = "",
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.Description,
                        VarName = $"kgmpna_param[{index},{i + 1}]",
                        Inv = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.Inv,
                        TypeSignal = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.TypeSignal,
                        Address = ""
                    },
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{i + 1}",
                        Value = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Setpoints.Value,
                        Unit = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Setpoints.Unit,
                        Id = $"H{1000 + j}",
                        Description = "",
                        VarName = $"SP_NA_READY[{j + 1}]",
                        Address = $"%MW{6500 + j}"
                    }
                };
                KGMPNA.Add(Param);
            }
            #endregion

            #region Параметры защит
            for (int i = 0; i < 128; i++)
            {
                var j = (index - 1) * 128 + i;
                var Param = new BaseKTPRA
                {
                    StateUMPNA = "",
                    NoMasked = "",
                    AVR = "",
                    Type = "",
                    StopType = "",
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.Description,
                        VarName = $"ktpra_param[{index},{i + 1}]",
                        Inv = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.Inv,
                        TypeSignal = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.TypeSignal,
                        Address = ""
                    },
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{i + 1}",
                        Value = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Setpoints.Value,
                        Unit = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Setpoints.Unit,
                        Id = $"H{3000 + j}",
                        VarName = $"SP_NA_PROT[{j + 1}]",
                        Address = $"%MW{5200 + j}",
                        Description = ""
                    },
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD()
                };
                KTPRA.Add(Param);
            }
            #endregion

            #region Параметры предельных параметров
            for (int i = 0; i < 128; i++)
            {
                var j = (index - 1) * 128 + i;
                var Param = new BaseKTPRAS
                {
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.Description,
                        VarName = $"ktpras_param[{index},{i + 1}]",
                        Inv = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.Inv,
                        TypeSignal = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.TypeSignal,
                        Address = ""
                    },
                    TypeWarning = "",
                    StateUMPNA = "",
                    Type = ""
                };
                KTPRAS.Add(Param);
            }
            #endregion

            #region Генерируем МПНА
            var signal = new BaseUMPNA
            {
                Index = $"{index}",
                Description = $"МПНА №{index}",
                VarName = $"umpna_param[{index}]",
                ShortDescription = "",
                IndexPZ = "",
                IndexVZ = "",
                TypeUMPNA = "",
                IndexGroupMS = "",
                UsedMCP = "",
                UsedKPD = "",
                CountButtonStop = "",
                InputParam = new ObservableCollection<BaseParam>(InputParam),
                OutputParam = new ObservableCollection<BaseParam>(OutputParam),
                Setpoints = new ObservableCollection<BaseSetpoints>(Setpoints),
                KGMPNA = new ObservableCollection<BaseKGMPNA>(KGMPNA),
                KTPRA = new ObservableCollection<BaseKTPRA>(KTPRA),
                KTPRAS = new ObservableCollection<BaseKTPRAS>(KTPRAS)
            };
            Params.Add(signal);
            SelectedParam = Params[^1];
            #endregion

            #endregion
        }
        #endregion 

        #region Обновление индексов
        public void RefreshIndex(int SelectedParamIndex)
        {
            var index = 0;
            foreach (var item in Params)
            {
                index++;
                item.VarName = $"umpna_param[{index}]";
                var index_setpoints = (index - 1) * App.Settings.Config.UMPNA.Setpoints.Count;
                var index_input_param = (index - 1) * App.Settings.Config.UMPNA.InputParams.Count;
                var index_output_param = (index - 1) * App.Settings.Config.UMPNA.OutputParams.Count;

                #region Уставки
                var i = 0;
                foreach (var _Setpoint in item.Setpoints)
                {
                    _Setpoint.VarName = $"SP_TM_NA[{index_setpoints + i + 1}]";
                    _Setpoint.Id = $"H{6000 + index_setpoints + i}";
                    _Setpoint.Address = $"%MW{5000 + index_setpoints + i}";
                    i++;
                }
                #endregion

                #region Входные параметры
                i = 0;
                foreach (var _InputParam in item.InputParam)
                {
                    _InputParam.VarName = $"NA_DI_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion

                #region Выходные параметры
                i = 0;
                foreach (var _OutputParam in item.OutputParam)
                {
                    _OutputParam.VarName = $"NA_DO_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion

                #region Параметры готовностей
                i = 0;
                foreach (var _KGMPNA in item.KGMPNA)
                {
                    var j = (index - 1) * 48 + i;
                    _KGMPNA.Param.VarName = $"kgmpna_param[{index},{i + 1}]";
                    _KGMPNA.Setpoints.Id = $"H{1000 + j}";
                    _KGMPNA.Setpoints.VarName = $"SP_NA_READY[{j + 1}]";
                    _KGMPNA.Setpoints.Address = $"%MW{6500 + j}";
                    i++;
                }
                #endregion

                #region Параметры защит
                i = 0;
                foreach (var _KTPRA in item.KTPRA)
                {
                    var j = (index - 1) * 128 + i;
                    _KTPRA.Param.VarName = $"ktpra_param[{index},{i + 1}]";
                    _KTPRA.Setpoints.Id = $"H{3000 + j}";
                    _KTPRA.Setpoints.VarName = $"SP_NA_PROT[{j + 1}]";
                    _KTPRA.Setpoints.Address = $"%MW{5200 + j}";
                    i++;
                }
                #endregion

                #region Параметры предельных параметров
                i = 0;
                foreach (var _KTPRAS in item.KTPRAS)
                {
                    var j = (index - 1) * 128 + i;
                    _KTPRAS.Param.VarName = $"ktpras_param[{index},{i + 1}]";
                    i++;
                }
                #endregion

                if (Params is not null && Params.Count > 0)
                    SelectedParam = Params[SelectedParamIndex];
            }
        }
        #endregion

        #region Импортируем МПНА
        private void ImportUMPNA(string Description)
        {
            var index = Params.Count + 1;
            var index_setpoints = (index - 1) * App.Settings.Config.UMPNA.Setpoints.Count;
            var index_input_param = (index - 1) * App.Settings.Config.UMPNA.InputParams.Count;
            var index_output_param = (index - 1) * App.Settings.Config.UMPNA.OutputParams.Count;

            var DefualtMapKGMPNA = App.Settings.Config.DefualtMapKGMPNA.Count > 0 ? App.Settings.Config.DefualtMapKGMPNA : null;
            var DefualtMapKTPRA = App.Settings.Config.DefualtMapKTPRA.Count > 0 ? App.Settings.Config.DefualtMapKTPRA : null;
            var DefualtMapKTPRAS = App.Settings.Config.DefualtMapKTPRAS.Count > 0 ? App.Settings.Config.DefualtMapKTPRAS : null;

            var InputParam = new ObservableCollection<BaseParam>();
            var OutputParam = new ObservableCollection<BaseParam>();
            var Setpoints = new ObservableCollection<BaseSetpoints>();
            var KGMPNA = new ObservableCollection<BaseKGMPNA>();
            var KTPRA = new ObservableCollection<BaseKTPRA>();
            var KTPRAS = new ObservableCollection<BaseKTPRAS>();

            #region Создаем МПНА

            #region Входные параметры
            for (int i = 0; i < App.Settings.Config.UMPNA.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UMPNA.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < App.Settings.Config.UMPNA.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UMPNA.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < App.Settings.Config.UMPNA.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_NA[{index_setpoints + i + 1}]",
                    Id = $"H{6000 + index_setpoints + i}",
                    Unit = App.Settings.Config.UMPNA.Setpoints[i].Unit,
                    Value = App.Settings.Config.UMPNA.Setpoints[i].Value,
                    Address = $"%MW{5000 + index_setpoints + i}",
                    Description = App.Settings.Config.UMPNA.Setpoints[i].Description
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Параметры готовностей
            for (int i = 0; i < 48; i++)
            {
                var j = (index - 1) * 48 + i;
                var Param = new BaseKGMPNA
                {
                    NoMasked = "",
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.Description,
                        VarName = $"kgmpna_param[{index},{i + 1}]",
                        Inv = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.Inv,
                        TypeSignal = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Param.TypeSignal,
                        Address = ""
                    },
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{i + 1}",
                        Value = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Setpoints.Value,
                        Unit = DefualtMapKGMPNA is null || i > DefualtMapKGMPNA.Count ? "" : DefualtMapKGMPNA[i].Setpoints.Unit,
                        Id = $"H{1000 + j}",
                        Description = "",
                        VarName = $"SP_NA_READY[{j + 1}]",
                        Address = $"%MW{6500 + j}"
                    }
                };
                KGMPNA.Add(Param);
            }
            #endregion

            #region Параметры защит
            for (int i = 0; i < 128; i++)
            {
                var j = (index - 1) * 128 + i;
                var Param = new BaseKTPRA
                {
                    StateUMPNA = "",
                    NoMasked = "",
                    AVR = "",
                    Type = "",
                    StopType = "",
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.Description,
                        VarName = $"ktpra_param[{index},{i + 1}]",
                        Inv = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.Inv,
                        TypeSignal = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Param.TypeSignal,
                        Address = ""
                    },
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{i + 1}",
                        Value = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Setpoints.Value,
                        Unit = DefualtMapKTPRA is null || i > DefualtMapKTPRA.Count ? "" : DefualtMapKTPRA[i].Setpoints.Unit,
                        Id = $"H{3000 + j}",
                        VarName = $"SP_NA_PROT[{j + 1}]",
                        Address = $"%MW{5200 + j}",
                        Description = ""
                    },
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD()
                };
                KTPRA.Add(Param);
            }
            #endregion

            #region Параметры предельных параметров
            for (int i = 0; i < 128; i++)
            {
                var j = (index - 1) * 128 + i;
                var Param = new BaseKTPRAS
                {
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.Description,
                        VarName = $"ktpras_param[{index},{i + 1}]",
                        Inv = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.Inv,
                        TypeSignal = DefualtMapKTPRAS is null || i > DefualtMapKTPRAS.Count ? "" : DefualtMapKTPRAS[i].Param.TypeSignal,
                        Address = ""
                    },
                    TypeWarning = "",
                    StateUMPNA = "",
                    Type = ""
                };
                KTPRAS.Add(Param);
            }
            #endregion

            #region Генерируем МПНА
            var signal = new BaseUMPNA
            {
                Index = $"{index}",
                Description = Description,
                VarName = $"umpna_param[{index}]",
                ShortDescription = "",
                IndexPZ = "",
                IndexVZ = "",
                TypeUMPNA = "",
                IndexGroupMS = "",
                UsedMCP = "",
                UsedKPD = "",
                CountButtonStop = "",
                InputParam = new ObservableCollection<BaseParam>(InputParam),
                OutputParam = new ObservableCollection<BaseParam>(OutputParam),
                Setpoints = new ObservableCollection<BaseSetpoints>(Setpoints),
                KGMPNA = new ObservableCollection<BaseKGMPNA>(KGMPNA),
                KTPRA = new ObservableCollection<BaseKTPRA>(KTPRA),
                KTPRAS = new ObservableCollection<BaseKTPRAS>(KTPRAS)
            };
            Params.Add(signal);
            #endregion

            #endregion
        }
        #endregion 

        #endregion
    }
}
