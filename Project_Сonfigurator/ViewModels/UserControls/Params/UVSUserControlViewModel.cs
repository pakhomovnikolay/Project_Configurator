using Project_Сonfigurator.Infrastructures.Commands;
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
    public class UVSUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public UVSUserControlViewModel()
        {
            Title = "Настройки вспомсистем";
            Description = "Текущие массивы состояний вспомсистем";
            UsingUserControl = new UVSUserControl();
        }

        private readonly ISignalService SignalServices;
        public UVSUserControlViewModel(ISignalService _ISignalService) : this()
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
                    DoSelection = DoSelectionInputParam || DoSelectionOutputParam;
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

        #region Список вспомсистем
        private ObservableCollection<BaseUVS> _Params = new();
        /// <summary>
        /// Список вспомсистем
        /// </summary>
        public ObservableCollection<BaseUVS> Params
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

        #region Выбранная вспомсистема
        private BaseUVS _SelectedParam = new();
        /// <summary>
        /// Выбранная вспомсистема
        /// </summary>
        public BaseUVS SelectedParam
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
        private bool CanCmdDeleteUVSExecute(object p) => SelectedParam is not null;

        private void OnCmdDeleteUVSExecuted(object p)
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

        #region Команда - Импортировать вспомсистемы из таблицы сигналов
        private ICommand _CmdImportUVS;
        /// <summary>
        /// Команда - Импортировать вспомсистемы из таблицы сигналов
        /// </summary>
        public ICommand CmdImportUVS => _CmdImportUVS ??= new RelayCommand(OnCmdImportUVSExecuted, CanCmdImportUVSExecute);
        private bool CanCmdImportUVSExecute() => true;

        private void OnCmdImportUVSExecuted()
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

            foreach (var _Params in TableSignalsViewModel.Params)
            {
                foreach (var _Rack in _Params.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Channels is null || _Module.Channels.Count <= 0) continue;
                        foreach (var Channel in _Module.Channels)
                        {
                            if ((Channel.Description.Contains("насос", StringComparison.Ordinal) ||
                                Channel.Description.Contains("вентилятор", StringComparison.CurrentCultureIgnoreCase)) &&
                                !Channel.Description.Contains("резерв", StringComparison.CurrentCultureIgnoreCase) &&
                                !Channel.Description.Contains("мна", StringComparison.CurrentCultureIgnoreCase) &&
                                !Channel.Description.Contains("пна", StringComparison.CurrentCultureIgnoreCase))
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
                                    ImportUVS(name);
                            }
                        }
                    }
                }
            }
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
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<BaseUVS>;
        #endregion

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

        #region Импортируем вспомсистемы
        private void ImportUVS(string Description)
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
                Description = Description,
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
            #endregion

            #endregion
        }
        #endregion 

        #endregion
    }
}
