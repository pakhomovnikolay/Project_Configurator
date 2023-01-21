using Project_Сonfigurator.Infrastructures.Commands;
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
    public class UZDUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UZDUserControlViewModel()
        {
            Title = "Настройки задвижек";
            Description = "Текущие массивы состояний задвижек";
            UsingUserControl = new UZDUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly IUserDialogService UserDialog;
        private readonly IDBService DBServices;
        public UZDUserControlViewModel(ISignalService _ISignalService, IUserDialogService _UserDialog, IDBService _IDBService) : this()
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

        #region Список задвижек
        private ObservableCollection<BaseUZD> _Params = new();
        /// <summary>
        /// Список задвижек
        /// </summary>
        public ObservableCollection<BaseUZD> Params
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

        #region Выбранная задвижка
        private BaseUZD _SelectedParam = new();
        /// <summary>
        /// Выбранная задвижка
        /// </summary>
        public BaseUZD SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
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

        #region Команда - Добавить задвижку
        private ICommand _CmdAddUZD;
        /// <summary>
        /// Команда - Добавить задвижку
        /// </summary>
        public ICommand CmdAddUZD => _CmdAddUZD ??= new RelayCommand(OnCmdAddUZDExecuted, CanCmdAddUZDExecute);
        private bool CanCmdAddUZDExecute() => true;

        private void OnCmdAddUZDExecuted()
        {
            CreateParam();
        }
        #endregion

        #region Команда - удалить задвижку
        private ICommand _CmdDeleteUZD;
        /// <summary>
        /// Команда - удалить задвижку
        /// </summary>
        public ICommand CmdDeleteUZD => _CmdDeleteUZD ??= new RelayCommand(OnCmdDeleteUZDExecuted, CanCmdDeleteUZDExecute);
        private bool CanCmdDeleteUZDExecute() => SelectedParam is not null;

        private void OnCmdDeleteUZDExecuted()
        {
            var index = Params.IndexOf(SelectedParam);
            index = index == 0 ? index : index - 1;

            Params.Remove(SelectedParam);
            if (Params.Count > 0)
                SelectedParam = Params[index];

            RefreshIndex(index);
        }
        #endregion

        #region Команда - Импортировать задвижки из таблицы сигналов
        private ICommand _CmdImportUZD;
        /// <summary>
        /// Команда - Импортировать задвижки из таблицы сигналов
        /// </summary>
        public ICommand CmdImportUZD => _CmdImportUZD ??= new RelayCommand(OnCmdImportUZDExecuted, CanCmdImportUZDExecute);
        private bool CanCmdImportUZDExecute() => true;

        private void OnCmdImportUZDExecuted()
        {
            #region Импорт сигналов из ТБ
            //IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            //SignalsDIUserControlViewModel MyViewModel = new();

            //foreach (var _TabItem in from object _Item in _ViewModels
            //                         let _TabItem = _Item as SignalsDIUserControlViewModel
            //                         where _TabItem is not null
            //                         select _TabItem)
            //    MyViewModel = _TabItem;


            //if (MyViewModel is null) return;
            //if (MyViewModel.Params is null) return;
            //if (MyViewModel.Params.Count <= 0) return;
            //if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
            //    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            //var _Params = new ObservableCollection<BaseUZD>();
            //foreach (var _Param in MyViewModel.Params)
            //{
            //    _Param.Signal.id
            //    foreach (var _Rack in _USO.Racks)
            //    {
            //        foreach (var _Module in _Rack.Modules)
            //        {
            //            if (_Module.Type == TypeModule.AI)
            //            {
            //                if (_Module.Channels is null || _Module.Channels.Count <= 0)
            //                    if (UserDialog.SendMessage("Внимание!", "Неверные данные таблицы сигналов. Проверьте вкладку",
            //                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;

            //                foreach (var Channel in _Module.Channels)
            //                {
            //                    if ((string.IsNullOrWhiteSpace(Channel.Id) && string.IsNullOrWhiteSpace(Channel.Description)) ||
            //                        Channel.Description == "Резерв") continue;

            //                    var param = new SignalAI
            //                    {
            //                        AddresUTS = "",
            //                        ConverterKgs = "",
            //                        IndexBD = "",
            //                        IndexNA = "",
            //                        IndexPZ = "",
            //                        LevelRPP = "",
            //                        TypePI = "",
            //                        TypeVibration = "",
            //                        Unit = "",
            //                        Signal = new BaseSignal
            //                        {
            //                            Index = $"{_Params.Count + 1}",
            //                            Id = Channel.Id,
            //                            Description = Channel.Description,
            //                            Area = "",
            //                            Address = Channel.Address,
            //                            VarName = $"ai_shared[{_Params.Count + 1}]",

            //                        }
            //                    };
            //                    _Params.Add(param);
            //                }
            //            }
            //        }
            //    }
            //}
            //Params = new ObservableCollection<SignalAI>(_Params);
            //UserDialog.SendMessage("Импорт сигналов AI", "Сигналы AI успешно импортированы\nиз таблицы сигналов",
            //    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            #endregion

            //if (TableSignalsViewModel.DataView is null) return;
            //if (!UserDialog.SendMessage("Внимание!", "Все данные будут потеряны!\nПродолжить?",
            //    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            //UZD = new();
            //foreach (var DataView in TableSignalsViewModel.DataView)
            //{
            //    var uso = DataView as USO;
            //    foreach (var _Rack in uso.Racks)
            //    {
            //        foreach (var _Module in _Rack.Modules)
            //        {
            //            foreach (var Channel in _Module.Channels)
            //            {
            //                if ((Channel.Description.Contains("задвижк", StringComparison.CurrentCultureIgnoreCase) ||
            //                    Channel.Description.Contains("клапан", StringComparison.CurrentCultureIgnoreCase) ||
            //                    Channel.Description.Contains("клоп", StringComparison.CurrentCultureIgnoreCase)) &&
            //                    !Channel.Description.Contains("резерв", StringComparison.CurrentCultureIgnoreCase))
            //                {
            //                    var name = "";
            //                    var index_dot = Channel.Description.IndexOf(".");
            //                    var qty = Channel.Description.Length;
            //                    if (index_dot > 0)
            //                        name = Channel.Description.Remove(index_dot, qty - index_dot);
            //                    var fl_tmp = false;
            //                    foreach (var item in UZD)
            //                    {
            //                        if (item.Description.Contains(name, StringComparison.CurrentCultureIgnoreCase))
            //                            fl_tmp = true;
            //                    }
            //                    if (!fl_tmp && !string.IsNullOrWhiteSpace(name))
            //                        ImportUZD(name, UZD);
            //                }
            //            }
            //        }
            //    }
            //}

            //SelectedUZD = UZD[0];
            //_DataView.Source = UZD;
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
            var index_setpoints = (index - 1) * App.Settings.Config.UZD.Setpoints.Count;
            var index_input_param = (index - 1) * App.Settings.Config.UZD.InputParams.Count;
            var index_output_param = (index - 1) * App.Settings.Config.UZD.OutputParams.Count;
            var InputParam = new ObservableCollection<BaseParam>();
            var OutputParam = new ObservableCollection<BaseParam>();
            var Setpoints = new ObservableCollection<BaseSetpoints>();

            #region Создаем задвижку

            #region Входные параметры
            for (int i = 0; i < App.Settings.Config.UZD.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"ZD_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UZD.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < App.Settings.Config.UZD.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"ZD_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = App.Settings.Config.UZD.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < App.Settings.Config.UZD.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_ZD[{index_setpoints + i + 1}]",
                    Id = $"H{8000 + index_setpoints + i}",
                    Unit = App.Settings.Config.UZD.Setpoints[i].Unit,
                    Value = App.Settings.Config.UZD.Setpoints[i].Value,
                    Address = $"%MW{3000 + index_setpoints + i}",
                    Description = App.Settings.Config.UZD.Setpoints[i].Description
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Генерируем задвижки
            var signal = new BaseUZD
            {
                Index = $"{index}",
                Description = $"Задвижка №{index}",
                VarName = $"uzd_param[{index}]",
                ShortDescription = "",
                IndexEC = "",
                IndexGroup = "",
                DescriptionGroup = "",
                Dist = "",
                DoubleStop = "",
                Bur = "",
                COz = "",
                CZz = "",
                EC = "",
                CheckState = "",
                RsOff = "",
                TypeZD = "",
                IndexPZ = "",
                IndexBD = "",
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
                item.VarName = $"uzd_param[{index}]";
                var index_setpoints = (index - 1) * App.Settings.Config.UZD.Setpoints.Count;
                var index_input_param = (index - 1) * App.Settings.Config.UZD.InputParams.Count;
                var index_output_param = (index - 1) * App.Settings.Config.UZD.OutputParams.Count;

                #region Уставки
                var i = 0;
                foreach (var _Setpoint in item.Setpoints)
                {
                    _Setpoint.VarName = $"SP_TM_ZD[{index_setpoints + i + 1}]";
                    _Setpoint.Id = $"H{8000 + index_setpoints + i}";
                    _Setpoint.Address = $"%MW{3000 + index_setpoints + i}";
                    i++;
                }
                #endregion

                #region Входные параметры
                i = 0;
                foreach (var _InputParam in item.InputParam)
                {
                    _InputParam.VarName = $"ZD_DI_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion

                #region Выходные параметры
                i = 0;
                foreach (var _OutputParam in item.OutputParam)
                {
                    _OutputParam.VarName = $"ZD_DO_P[{index_input_param + i + 1}]";
                    i++;
                }
                #endregion
            }
            if (Params is not null && Params.Count > 0)
                SelectedParam = Params[SelectedParamIndex];
        }
        #endregion 

        //#region Импортируем задвижки
        //private static void ImportUZD(string Description, ObservableCollection<BaseUZD> data_list)
        //{
        //    var index = data_list.Count + 1;
        //    var index_setpoints = (index - 1) * App.Settings.Config.UZD.Setpoints.Count;
        //    var index_input_param = (index - 1) * App.Settings.Config.UZD.InputParams.Count;
        //    var index_output_param = (index - 1) * App.Settings.Config.UZD.OutputParams.Count;
        //    var InputParam = new ObservableCollection<BaseParam>();
        //    var OutputParam = new ObservableCollection<BaseParam>();
        //    var Setpoints = new ObservableCollection<BaseSetpoints>();

        //    #region Создаем задвижку

        //    #region Входные параметры
        //    for (int i = 0; i < App.Settings.Config.UZD.InputParams.Count; i++)
        //    {
        //        var Param = new BaseParam
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"ZD_DI_P[{index_input_param + i + 1}]",
        //            Id = "",
        //            Inv = "",
        //            TypeSignal = "",
        //            Address = "",
        //            Description = App.Settings.Config.UZD.InputParams[i].Text
        //        };
        //        InputParam.Add(Param);
        //    }
        //    #endregion

        //    #region Выходные параметры
        //    for (int i = 0; i < App.Settings.Config.UZD.OutputParams.Count; i++)
        //    {
        //        var Param = new BaseParam
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"ZD_DO_P[{index_output_param + i + 1}]",
        //            Id = "",
        //            Inv = "",
        //            TypeSignal = "",
        //            Address = "",
        //            Description = App.Settings.Config.UZD.OutputParams[i].Text
        //        };
        //        OutputParam.Add(Param);
        //    }
        //    #endregion

        //    #region Уставки
        //    for (int i = 0; i < App.Settings.Config.UZD.Setpoints.Count; i++)
        //    {
        //        var Param = new BaseSetpoints
        //        {
        //            Index = $"{i + 1}",
        //            VarName = $"SP_TM_ZD[{index_setpoints + i + 1}]",
        //            Id = $"H{8000 + index_setpoints + i}",
        //            Unit = App.Settings.Config.UZD.Setpoints[i].Unit,
        //            Value = App.Settings.Config.UZD.Setpoints[i].Value,
        //            Address = $"%MW{3000 + index_setpoints + i}",
        //            Description = App.Settings.Config.UZD.Setpoints[i].Description
        //        };
        //        Setpoints.Add(Param);
        //    }
        //    #endregion

        //    #region Генерируем задвижки
        //    var signal = new BaseUZD
        //    {
        //        Index = $"{index}",
        //        Description = Description,
        //        VarName = $"uzd_param[{index}]",
        //        ShortDescription = "",
        //        IndexEC = "",
        //        IndexGroup = "",
        //        DescriptionGroup = "",
        //        Dist = "",
        //        DoubleStop = "",
        //        Bur = "",
        //        COz = "",
        //        CZz = "",
        //        EC = "",
        //        CheckState = "",
        //        RsOff = "",
        //        TypeZD = "",
        //        IndexPZ = "",
        //        IndexBD = "",
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
