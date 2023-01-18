﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UVSUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UVSUserControlViewModel()
        {
            Title = "Настройки вспомсистем";
            Description = "Текущие массивы состояний вспомсистем";
            UsingUserControl = new UVSUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly IUserDialogService UserDialog;
        private readonly IDBService DBServices;
        public UVSUserControlViewModel(ISignalService _ISignalService, IUserDialogService _UserDialog, IDBService _IDBService) : this()
        {
            SignalServices = _ISignalService;
            UserDialog = _UserDialog;
            DBServices = _IDBService;
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
                    if (_IsSelected)
                    {
                        DoSelection = SignalServices.DoSelection;
                        if (SignalServices.DoSelection && !string.IsNullOrWhiteSpace(SignalServices.Address))
                        {
                            #region Сменить входной параметр
                            if (DoSelectionInputParam)
                            {
                                SignalServices.RedefineParam(SelectedInputParam, _IsSelected, Title);
                            }
                            #endregion

                            #region Сменить выходной параметр
                            if (DoSelectionInputParam)
                            {
                                SignalServices.RedefineParam(SelectedOutputParam, _IsSelected, Title);
                            }
                            #endregion

                            #region Обнуляем данные
                            SignalServices.ResetSignal();
                            DoSelectionInputParam = SignalServices.DoSelection;
                            DoSelectionOutputParam = SignalServices.DoSelection;
                            DoSelection = SignalServices.DoSelection;
                            #endregion
                        }
                        else if (SignalServices.DoSelection && string.IsNullOrWhiteSpace(SignalServices.Address) && SignalServices.ListName == Title)
                        {
                            #region Обнуляем данные
                            SignalServices.ResetSignal();
                            DoSelectionInputParam = SignalServices.DoSelection;
                            DoSelectionOutputParam = SignalServices.DoSelection;
                            DoSelection = SignalServices.DoSelection;
                            #endregion
                        }
                    }
                    else if (SignalServices.DoSelection && string.IsNullOrWhiteSpace(SignalServices.Address) && SignalServices.ListName != Title)
                    {
                        #region Обнуляем данные
                        SignalServices.ResetSignal();
                        DoSelectionInputParam = SignalServices.DoSelection;
                        DoSelectionOutputParam = SignalServices.DoSelection;
                        DoSelection = SignalServices.DoSelection;
                        #endregion
                    }
                }
            }
        }
        #endregion

        #region Список вспомсистем
        private ObservableCollection<BaseUVS> _Params = new();
        /// <summary>
        /// Список вспомсистем
        /// </summary>
        public ObservableCollection<BaseUVS> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
        #endregion

        #region Выбранная вспомсистема
        private BaseUVS _SelectedParam = new();
        /// <summary>
        /// Выбранная вспомсистема
        /// </summary>
        public BaseUVS SelectedParam
        {
            get => _SelectedParam;
            set
            {
                if (Set(ref _SelectedParam, value))
                {
                    //_DataViewInputParam.Source = value?.InputParam;
                    //_DataViewInputParam.View?.Refresh();
                    //OnPropertyChanged(nameof(DataViewInputParam));

                    //_DataViewOutputParam.Source = value?.OutputParam;
                    //_DataViewOutputParam.View?.Refresh();
                    //OnPropertyChanged(nameof(DataViewOutputParam));
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

        #endregion

        #region Команды

        #region Команда - Добавить вспомсистему
        private ICommand _CmdAddUVS;
        /// <summary>
        /// Команда - Добавить вспомсистему
        /// </summary>
        public ICommand CmdAddUVS => _CmdAddUVS ??= new RelayCommand(OnCmdAddUVSExecuted, CanCmdAddUVSExecute);
        private bool CanCmdAddUVSExecute() => true;

        private void OnCmdAddUVSExecuted()
        {
            CreateParam();
        }
        #endregion

        #region Команда - удалить вспомсистему
        private ICommand _CmdDeleteUVS;
        /// <summary>
        /// Команда - удалить вспомсистему
        /// </summary>
        public ICommand CmdDeleteUVS => _CmdDeleteUVS ??= new RelayCommand(OnCmdDeleteUVSExecuted, CanCmdDeleteUVSExecute);
        private bool CanCmdDeleteUVSExecute() => SelectedParam is not null;

        private void OnCmdDeleteUVSExecuted()
        {
            var index = Params.IndexOf(SelectedParam);
            index = index == 0 ? index : index - 1;

            Params.Remove(SelectedParam);
            if (Params.Count > 0)
                SelectedParam = Params[index];

            RefreshIndex(index);
        }
        #endregion

        #region Команда - Импортировать вспомсистемы из таблицы сигналов
        private ICommand _CmdImportUVS;
        /// <summary>
        /// Команда - Импортировать вспомсистемы из таблицы сигналов
        /// </summary>
        public ICommand CmdImportUVS => _CmdImportUVS ??= new RelayCommand(OnCmdImportUVSExecuted, CanCmdImportUVSExecute);
        private bool CanCmdImportUVSExecute() => /*TableSignalsViewModel.DataView is not null && TableSignalsViewModel.DataView.CurrentItem is not null*/true;

        private void OnCmdImportUVSExecuted()
        {
            //if (TableSignalsViewModel.DataView is null) return;
            //if (!UserDialog.SendMessage("Внимание!", "Все данные будут потеряны!\nПродолжить?",
            //    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            //UVS = new();
            //foreach (var DataView in TableSignalsViewModel.USOList)
            //{
            //    foreach (var _Rack in DataView.Racks)
            //    {
            //        foreach (var _Module in _Rack.Modules)
            //        {
            //            foreach (var Channel in _Module.Channels)
            //            {
            //                if ((Channel.Description.Contains("насос", StringComparison.Ordinal) ||
            //                    Channel.Description.Contains("вентилятор", StringComparison.CurrentCultureIgnoreCase)) &&
            //                    !Channel.Description.Contains("резерв", StringComparison.CurrentCultureIgnoreCase) &&
            //                    !Channel.Description.Contains("мна", StringComparison.CurrentCultureIgnoreCase) &&
            //                    !Channel.Description.Contains("пна", StringComparison.CurrentCultureIgnoreCase))
            //                {
            //                    var name = "";
            //                    var index_dot = Channel.Description.IndexOf(".");
            //                    var qty = Channel.Description.Length;
            //                    if (index_dot > 0)
            //                        name = Channel.Description.Remove(index_dot, qty - index_dot);
            //                    var fl_tmp = false;
            //                    foreach (var item in UVS)
            //                    {
            //                        if (item.Description.Contains(name, StringComparison.CurrentCultureIgnoreCase))
            //                            fl_tmp = true;
            //                    }
            //                    if (!fl_tmp && !string.IsNullOrWhiteSpace(name))
            //                        ImportUVS(name, UVS);
            //                }
            //            }
            //        }
            //    }
            //}

            //SelectedUVS = UVS[^1];
            //_DataView.Source = UVS;
            //_DataView.View.Refresh();
            //OnPropertyChanged(nameof(DataView));
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
            SignalServices.DoSelection = true;
            SignalServices.ListName = Title;
            SignalServices.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedInputParam.TypeSignal) || int.Parse(SelectedInputParam.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                SignalServices.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedInputParam.TypeSignal) > 0)
            {
                NameListSelected = "Группы сигналов";
                SignalServices.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedInputParam.TypeSignal) > 1)
            {
                NameListSelected = "Сигналы AI";
                SignalServices.Type = TypeModule.AI;
            }

            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == NameListSelected
                                     select _TabItem)
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
            SignalServices.DoSelection = true;
            SignalServices.ListName = Title;
            SignalServices.Type = TypeModule.DO;

            var NameListSelected = "Сигналы DO";
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Создание параметра
        private void CreateParam()
        {
            var index = Params.Count + 1;
            var index_setpoints = (index - 1) * App.Settings.Config.UVS.Setpoints.Count;
            var index_input_param = (index - 1) * App.Settings.Config.UVS.InputParams.Count;
            var index_output_param = (index - 1) * App.Settings.Config.UVS.OutputParams.Count;
            var InputParam = new ObservableCollection<BaseParam>();
            var OutputParam = new ObservableCollection<BaseParam>();
            var Setpoints = new ObservableCollection<BaseSetpoints>();

            #region Создаем вспомсистему

            #region Входные параметры
            for (int i = 0; i < App.Settings.Config.UVS.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"VS_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UVS.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < App.Settings.Config.UVS.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"VS_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UVS.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < App.Settings.Config.UVS.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_VS[{index_setpoints + i + 1}]",
                    Id = $"H{7000 + index_setpoints + i}",
                    Unit = App.Settings.Config.UVS.Setpoints[i].Unit,
                    Value = App.Settings.Config.UVS.Setpoints[i].Value,
                    Address = $"%MW{2000 + index_setpoints + i}",
                    Description = App.Settings.Config.UVS.Setpoints[i].Description
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Генерируем вспомсистему
            var signal = new BaseUVS
            {
                Index = $"{index}",
                Description = $"Вспомсистема №{index}",
                VarName = $"uvs_param[{index}]",
                ShortDescription = "",
                IndexEC = "",
                IndexGroup = "",
                DescriptionGroup = "",
                COz = "",
                OnePressureSensorGroup = "",
                Reservable = "",
                TypeGroup = "",
                TypePressure = "",
                InputParam = new ObservableCollection<BaseParam>(InputParam),
                OutputParam = new ObservableCollection<BaseParam>(OutputParam),
                Setpoints = new ObservableCollection<BaseSetpoints>(Setpoints)
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
                item.VarName = $"uvs_param[{index}]";
                var index_setpoints = (index - 1) * App.Settings.Config.UVS.Setpoints.Count;
                var index_input_param = (index - 1) * App.Settings.Config.UVS.InputParams.Count;
                var index_output_param = (index - 1) * App.Settings.Config.UVS.OutputParams.Count;

                #region Уставки
                var i = 0;
                foreach (var _Setpoint in item.Setpoints)
                {
                    _Setpoint.VarName = $"SP_TM_VS[{index_setpoints + i + 1}]";
                    _Setpoint.Id = $"H{7000 + index_setpoints + i}";
                    _Setpoint.Address = $"%MW{2000 + index_setpoints + i}";
                    i++;
                }
                #endregion

                #region Входные параметры
                i = 0;
                foreach (var _InputParam in item.InputParam)
                {
                    _InputParam.VarName = $"VS_DI_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion

                #region Выходные параметры
                i = 0;
                foreach (var _OutputParam in item.OutputParam)
                {
                    _OutputParam.VarName = $"VS_DO_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion
            }

            if (Params is not null && Params.Count > 0)
                SelectedParam = Params[SelectedParamIndex];
        }
        #endregion 

        //#region Импортируем вспомсистемы
        //private static void ImportUVS(string Description, ObservableCollection<BaseUVS> data_list)
        //{

        //    var index = data_list.Count + 1;
        //    var index_setpoints = (index - 1) * App.Settings.Config.UVS.Setpoints.Count;
        //    var index_input_param = (index - 1) * App.Settings.Config.UVS.InputParams.Count;
        //    var index_output_param = (index - 1) * App.Settings.Config.UVS.OutputParams.Count;
        //    var InputParam = new ObservableCollection<BaseParam>();
        //    var OutputParam = new ObservableCollection<BaseParam>();
        //    var Setpoints = new ObservableCollection<BaseSetpoints>();

        //    #region Создаем вспомсистему

        //    #region Входные параметры
        //    for (int i = 0; i < App.Settings.Config.UVS.InputParams.Count; i++)
        //    {
        //        var Param = new BaseParam
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"VS_DI_P[{index_input_param + i + 1}]",
        //            Id = "",
        //            Inv = "",
        //            TypeSignal = "",
        //            Address = "",
        //            Description = App.Settings.Config.UVS.InputParams[i].Text
        //        };
        //        InputParam.Add(Param);
        //    }
        //    #endregion

        //    #region Выходные параметры
        //    for (int i = 0; i < App.Settings.Config.UVS.OutputParams.Count; i++)
        //    {
        //        var Param = new BaseParam
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"VS_DO_P[{index_output_param + i + 1}]",
        //            Id = "",
        //            Inv = "",
        //            TypeSignal = "",
        //            Address = "",
        //            Description = App.Settings.Config.UVS.OutputParams[i].Text
        //        };
        //        OutputParam.Add(Param);
        //    }
        //    #endregion

        //    #region Уставки
        //    for (int i = 0; i < App.Settings.Config.UVS.Setpoints.Count; i++)
        //    {
        //        var Param = new BaseSetpoints
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"SP_TM_VS[{index_setpoints + i + 1}]",
        //            Id = $"H{7000 + index_setpoints + i}",
        //            Unit = App.Settings.Config.UVS.Setpoints[i].Unit,
        //            Value = App.Settings.Config.UVS.Setpoints[i].Value,
        //            Address = $"%MW{2000 + index_setpoints + i}",
        //            Description = App.Settings.Config.UVS.Setpoints[i].Description
        //        };
        //        Setpoints.Add(Param);
        //    }
        //    #endregion

        //    #region Генерируем вспомсистему
        //    var signal = new BaseUVS
        //    {
        //        Index = $"{index}",
        //        Description = Description,
        //        VarName = $"uvs_param[{index}]",
        //        ShortDescription = "",
        //        IndexEC = "",
        //        IndexGroup = "",
        //        DescriptionGroup = "",
        //        COz = "",
        //        OnePressureSensorGroup = "",
        //        Reservable = "",
        //        TypeGroup = "",
        //        TypePressure = "",
        //        InputParam = new ObservableCollection<BaseParam>(InputParam),
        //        OutputParam = new ObservableCollection<BaseParam>(OutputParam),
        //        Setpoints = new ObservableCollection<BaseSetpoints>(Setpoints)
        //    };
        //    data_list.Add(signal);
        //    #endregion

        //    #endregion
        //}
        //#endregion 

        #endregion
    }
}
