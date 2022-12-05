using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UMPNAUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private ISignalService _SignalService;

        TableSignalsUserControlViewModel TableSignalsViewModel { get; }
        public UMPNAUserControlViewModel(
            ISignalService signalService,
            IUserDialogService userDialog,
            TableSignalsUserControlViewModel tableSignalsViewModel)
        {
            UserDialog = userDialog;
            _SignalService = signalService;
            TableSignalsViewModel = tableSignalsViewModel;
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Настройки МПНА";
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Описание вкладки
        private string _Description = "Текущие массивы состояний МПНА";
        /// <summary>
        /// Описание вкладки
        /// </summary>
        public string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
        }
        #endregion

        #region Высота окна
        private int _WindowHeight = 800;
        /// <summary>
        /// Высота окна
        /// </summary>
        public int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }
        #endregion

        #region Ширина окна
        private int _WindowWidth = 1740;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

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
                    if (_IsSelected)
                    {
                        DoSelection = _SignalService.DoSelection;
                        if (_SignalService.DoSelection && !string.IsNullOrWhiteSpace(_SignalService.Address))
                        {
                            if (DoSelectionInputParam)
                            {
                                _SignalService.RedefineParam(SelectedInputParam, _IsSelected, Title);
                                _DataViewInputParam.View?.Refresh();
                            }

                            if (DoSelectionOutputParam)
                            {
                                _SignalService.RedefineParam(SelectedOutputParam, _IsSelected, Title);
                                _DataViewOutputParam.View?.Refresh();
                            }

                            DoSelectionInputParam = false;
                            DoSelectionOutputParam = false;
                            DoSelection = false;
                            _SignalService.ResetSignal();
                        }
                        else if (_SignalService.DoSelection && string.IsNullOrWhiteSpace(_SignalService.Address) && _SignalService.ListName == Title)
                        {
                            _SignalService.ResetSignal();
                            DoSelection = false;
                        }
                    }
                    else if (_SignalService.DoSelection && string.IsNullOrWhiteSpace(_SignalService.Address) && _SignalService.ListName != Title)
                    {
                        _SignalService.ResetSignal();
                        DoSelection = false;
                    }
                }
            }
        }
        #endregion

        #region Коллекция МПНА
        /// <summary>
        /// Коллекция МПНА
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный МПНА
        private BaseUMPNA _SelectedUMPNA = new();
        /// <summary>
        /// Выбранный МПНА
        /// </summary>
        public BaseUMPNA SelectedUMPNA
        {
            get => _SelectedUMPNA;
            set => Set(ref _SelectedUMPNA, value);
        }
        #endregion

        #region Коллекция входных параметров
        /// <summary>
        /// Коллекция входных параметров
        /// </summary>
        private readonly CollectionViewSource _DataViewInputParam = new();
        public ICollectionView DataViewInputParam => _DataViewInputParam?.View;
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

        #region Коллекция выходных параметров
        /// <summary>
        /// Коллекция выходных параметров
        /// </summary>
        private readonly CollectionViewSource _DataViewOutputParam = new();
        public ICollectionView DataViewOutputParam => _DataViewOutputParam?.View;
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

        #region Команда - Добавить МПНА
        private ICommand _CmdAddUMPNA;
        /// <summary>
        /// Команда - Добавить МПНА
        /// </summary>
        public ICommand CmdAddUMPNA => _CmdAddUMPNA ??= new RelayCommand(OnCmdAddUMPNAExecuted, CanCmdAddUMPNAExecute);
        private bool CanCmdAddUMPNAExecute() => true;

        private void OnCmdAddUMPNAExecuted()
        {
            CreateUMPNA();
            //var data_list = (List<BaseUMPNA>)_DataView.Source ?? new List<BaseUMPNA>();

            //var index = data_list.Count + 1;
            //var index_setpoints = (index - 1) * Program.Settings.Config.UMPNA.Setpoints.Count;
            //var index_input_param = (index - 1) * Program.Settings.Config.UMPNA.InputParams.Count;
            //var index_output_param = (index - 1) * Program.Settings.Config.UMPNA.OutputParams.Count;
            //var InputParam = new List<BaseParam>();
            //var OutputParam = new List<BaseParam>();
            //var Setpoints = new List<BaseSetpoints>();

            //#region Создаем задвижку

            //#region Входные параметры
            //for (int i = 0; i < Program.Settings.Config.UMPNA.InputParams.Count; i++)
            //{
            //    var Param = new BaseParam
            //    {
            //        Index = $"{i + 1}",
            //        VarName = $"NA_DI_P[{index_input_param + i + 1}]",
            //        Id = "",
            //        Inv = "",
            //        TypeSignal = "",
            //        Address = "",
            //        Description = Program.Settings.Config.UMPNA.InputParams[i].Text
            //    };
            //    InputParam.Add(Param);
            //}
            //#endregion

            //#region Выходные параметры
            //for (int i = 0; i < Program.Settings.Config.UMPNA.OutputParams.Count; i++)
            //{
            //    var Param = new BaseParam
            //    {
            //        Index = $"{i + 1}",
            //        VarName = $"NA_DO_P[{index_output_param + i + 1}]",
            //        Id = "",
            //        Inv = "",
            //        TypeSignal = "",
            //        Address = "",
            //        Description = Program.Settings.Config.UMPNA.OutputParams[i].Text
            //    };
            //    OutputParam.Add(Param);
            //}
            //#endregion

            //#region Уставки
            //for (int i = 0; i < Program.Settings.Config.UMPNA.Setpoints.Count; i++)
            //{
            //    var Param = new BaseSetpoints
            //    {
            //        Index = $"{i + 1}",
            //        VarName = $"SP_TM_NA[{index_setpoints + i + 1}]",
            //        Id = $"H{6000 + index_setpoints + i}",
            //        Unit = "",
            //        Value = "",
            //        Address = $"%MW{5000 + index_setpoints + i}",
            //        Description = Program.Settings.Config.UMPNA.Setpoints[i].Text
            //    };
            //    Setpoints.Add(Param);
            //}
            //#endregion

            //#region Генерируем задвижки
            //var signal = new BaseUMPNA
            //{
            //    Index = $"{index}",
            //    Description = $"МПНА №{index}",
            //    VarName = $"umpna_param[{index}]",
            //    ShortDescription = "",
            //    IndexPZ = "",
            //    IndexVZ = "",
            //    TypeUMPNA = "",
            //    IndexGroupMS = "",
            //    UsedMCP = "",
            //    UsedKPD = "",
            //    CountButtonStop = "",
            //    InputParam = new List<BaseParam>(InputParam),
            //    OutputParam = new List<BaseParam>(OutputParam),
            //    Setpoints = new List<BaseSetpoints>(Setpoints)
            //};
            //data_list.Add(signal);
            //#endregion

            //SelectedUMPNA = data_list[^1];
            //SelectedInputParam = SelectedUMPNA.InputParam[0];
            //SelectedOutputParam = SelectedUMPNA.OutputParam[0];

            //_DataViewInputParam.Source = SelectedUMPNA.InputParam;
            //_DataViewInputParam.View.Refresh();
            //OnPropertyChanged(nameof(DataViewInputParam));

            //_DataViewOutputParam.Source = SelectedUMPNA.OutputParam;
            //_DataViewOutputParam.View.Refresh();
            //OnPropertyChanged(nameof(DataViewOutputParam));

            //_DataView.Source = data_list;
            //_DataView.View.Refresh();
            //OnPropertyChanged(nameof(DataView));
            //return;
            //#endregion

        }
        #endregion

        #region Команда - удалить вспомсистему
        private ICommand _CmdDeleteUMPNA;
        /// <summary>
        /// Команда - удалить вспомсистему
        /// </summary>
        public ICommand CmdDeleteUMPNA => _CmdDeleteUMPNA ??= new RelayCommand(OnCmdDeleteUMPNAExecuted, CanCmdDeleteUMPNAExecute);
        private bool CanCmdDeleteUMPNAExecute() => SelectedUMPNA is not null && !string.IsNullOrWhiteSpace(SelectedUMPNA.Description);

        private void OnCmdDeleteUMPNAExecuted()
        {
            var data_list = (List<BaseUMPNA>)_DataView.Source ?? new List<BaseUMPNA>();
            var index = data_list.IndexOf(SelectedUMPNA);
            data_list.Remove(SelectedUMPNA);

            if (data_list.Count > 0)
            {
                if (index > 0)
                    SelectedUMPNA = data_list[index - 1];
                else
                    SelectedUMPNA = data_list[index];
            }
            else
            {
                SelectedUMPNA = null;
            }

            _DataViewInputParam.Source = SelectedUMPNA?.InputParam;
            _DataViewInputParam.View?.Refresh();
            OnPropertyChanged(nameof(DataViewInputParam));

            _DataViewOutputParam.Source = SelectedUMPNA?.OutputParam;
            _DataViewOutputParam.View?.Refresh();
            OnPropertyChanged(nameof(DataViewOutputParam));

            _DataView.Source = data_list;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion

        #region Команда - Импортировать вспомсистемы из таблицы сигналов
        private ICommand _CmdImportUMPNA;
        /// <summary>
        /// Команда - Импортировать вспомсистемы из таблицы сигналов
        /// </summary>
        public ICommand CmdImportUMPNA => _CmdImportUMPNA ??= new RelayCommand(OnCmdImportUMPNAExecuted, CanCmdImportUMPNAExecute);
        private bool CanCmdImportUMPNAExecute() => TableSignalsViewModel.DataView is not null;

        private void OnCmdImportUMPNAExecuted()
        {
            if (TableSignalsViewModel.DataView is null) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            var data_list = new List<BaseUMPNA>();
            foreach (var DataView in TableSignalsViewModel.DataView)
            {
                var uso = DataView as USO;
                foreach (var _Rack in uso.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        foreach (var Channel in _Module.Channels)
                        {
                            if (Channel.Description.Contains("мна", StringComparison.CurrentCultureIgnoreCase) ||
                                Channel.Description.Contains("пна", StringComparison.CurrentCultureIgnoreCase) ||
                                Channel.Description.Contains("магистраль", StringComparison.CurrentCultureIgnoreCase) ||
                                Channel.Description.Contains("подпор", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(Channel.Description))
                                {
                                    var index_dot = Channel.Description.IndexOf(".");
                                    var qty = Channel.Description.Length;
                                    var name = Channel.Description.Remove(index_dot, qty - index_dot);
                                    var fl_tmp = false;
                                    foreach (var item in data_list)
                                    {
                                        if (item.Description == name)
                                            fl_tmp = true;
                                    }
                                    if (!fl_tmp)
                                        ImportUMPNA(name, data_list);
                                }

                            }
                        }
                    }
                }
            }
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

            if (Index != SelectedInputParam.Index)
                SelectedInputParam = SelectedUMPNA.InputParam[int.Parse(Index) - 1];

            DoSelectionInputParam = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedInputParam.TypeSignal) || int.Parse(SelectedInputParam.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                _SignalService.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedInputParam.TypeSignal) > 0)
            {
                NameListSelected = "Сигналы AI";
                _SignalService.Type = TypeModule.DI;
            }

            if (App.FucusedTabControl == null) return;
            foreach (var _Item in App.FucusedTabControl.Items)
            {
                var _TabItem = _Item as TabItem;
                if (_TabItem.Header.ToString() == NameListSelected)
                {
                    App.FucusedTabControl.SelectedItem = _TabItem;
                    break;
                }
            }
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

            if (Index != SelectedOutputParam.Index)
                SelectedOutputParam = SelectedUMPNA.OutputParam[int.Parse(Index) - 1];

            DoSelectionOutputParam = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.DO;

            var NameListSelected = "Сигналы DO";
            if (App.FucusedTabControl == null) return;
            foreach (var _Item in App.FucusedTabControl.Items)
            {
                var _TabItem = _Item as TabItem;
                if (_TabItem.Header.ToString() == NameListSelected)
                {
                    App.FucusedTabControl.SelectedItem = _TabItem;
                    break;
                }
            }
        }
        #endregion

        #endregion

        #region Функции

        #region Создаем МПНА
        private void CreateUMPNA()
        {
            var data_list = (List<BaseUMPNA>)_DataView.Source ?? new List<BaseUMPNA>();

            var index = data_list.Count + 1;
            var index_setpoints = (index - 1) * Program.Settings.Config.UMPNA.Setpoints.Count;
            var index_input_param = (index - 1) * Program.Settings.Config.UMPNA.InputParams.Count;
            var index_output_param = (index - 1) * Program.Settings.Config.UMPNA.OutputParams.Count;
            var InputParam = new List<BaseParam>();
            var OutputParam = new List<BaseParam>();
            var Setpoints = new List<BaseSetpoints>();

            #region Создаем задвижку

            #region Входные параметры
            for (int i = 0; i < Program.Settings.Config.UMPNA.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UMPNA.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < Program.Settings.Config.UMPNA.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UMPNA.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < Program.Settings.Config.UMPNA.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_NA[{index_setpoints + i + 1}]",
                    Id = $"H{6000 + index_setpoints + i}",
                    Unit = "",
                    Value = "",
                    Address = $"%MW{5000 + index_setpoints + i}",
                    Description = Program.Settings.Config.UMPNA.Setpoints[i].Text
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Генерируем задвижки
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
                InputParam = new List<BaseParam>(InputParam),
                OutputParam = new List<BaseParam>(OutputParam),
                Setpoints = new List<BaseSetpoints>(Setpoints)
            };
            data_list.Add(signal);
            #endregion

            SelectedUMPNA = data_list[^1];
            SelectedInputParam = SelectedUMPNA.InputParam[0];
            SelectedOutputParam = SelectedUMPNA.OutputParam[0];

            _DataViewInputParam.Source = SelectedUMPNA.InputParam;
            _DataViewInputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewInputParam));

            _DataViewOutputParam.Source = SelectedUMPNA.OutputParam;
            _DataViewOutputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewOutputParam));

            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
            return;
            #endregion
        }
        #endregion 

        #region Импортируем МПНА
        private void ImportUMPNA(string Description, List<BaseUMPNA> data_list)
        {
            var index = data_list.Count + 1;
            var index_setpoints = (index - 1) * Program.Settings.Config.UMPNA.Setpoints.Count;
            var index_input_param = (index - 1) * Program.Settings.Config.UMPNA.InputParams.Count;
            var index_output_param = (index - 1) * Program.Settings.Config.UMPNA.OutputParams.Count;
            var InputParam = new List<BaseParam>();
            var OutputParam = new List<BaseParam>();
            var Setpoints = new List<BaseSetpoints>();

            #region Создаем задвижку

            #region Входные параметры
            for (int i = 0; i < Program.Settings.Config.UMPNA.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UMPNA.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < Program.Settings.Config.UMPNA.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"NA_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UMPNA.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < Program.Settings.Config.UMPNA.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_NA[{index_setpoints + i + 1}]",
                    Id = $"H{6000 + index_setpoints + i}",
                    Unit = "",
                    Value = "",
                    Address = $"%MW{5000 + index_setpoints + i}",
                    Description = Program.Settings.Config.UMPNA.Setpoints[i].Text
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Генерируем задвижки
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
                InputParam = new List<BaseParam>(InputParam),
                OutputParam = new List<BaseParam>(OutputParam),
                Setpoints = new List<BaseSetpoints>(Setpoints)
            };
            data_list.Add(signal);
            #endregion

            SelectedUMPNA = data_list[^1];
            SelectedInputParam = SelectedUMPNA.InputParam[0];
            SelectedOutputParam = SelectedUMPNA.OutputParam[0];

            _DataViewInputParam.Source = SelectedUMPNA.InputParam;
            _DataViewInputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewInputParam));

            _DataViewOutputParam.Source = SelectedUMPNA.OutputParam;
            _DataViewOutputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewOutputParam));

            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
            return;
            #endregion
        }
        #endregion 

        #endregion
    }
}
