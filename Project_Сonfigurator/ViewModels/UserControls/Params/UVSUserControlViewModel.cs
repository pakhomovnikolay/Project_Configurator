﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UVSUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;
        public UVSUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Настройки вспомсистем";
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
        private string _Description = "Текущие массивы состояний вспомсистем";
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

        #region Коллекция вспомсистем
        /// <summary>
        /// Коллекция вспомсистем
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранная вспомсистема
        private BaseUVS _SelectedUVS = new();
        /// <summary>
        /// Выбранная вспомсистема
        /// </summary>
        public BaseUVS SelectedUVS
        {
            get => _SelectedUVS;
            set => Set(ref _SelectedUVS, value);
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

        #region Команда - Добавить вспомсистему
        private ICommand _CmdAddUVS;
        /// <summary>
        /// Команда - Добавить вспомсистему
        /// </summary>
        public ICommand CmdAddUVS => _CmdAddUVS ??= new RelayCommand(OnCmdAddUVSExecuted, CanCmdAddUVSExecute);
        private bool CanCmdAddUVSExecute() => true;

        private void OnCmdAddUVSExecuted()
        {
            var data_list = (List<BaseUVS>)_DataView.Source ?? new List<BaseUVS>();

            var index = data_list.Count + 1;
            var index_setpoints = (index - 1) * Program.Settings.Config.UVS.Setpoints.Count;
            var index_input_param = (index - 1) * Program.Settings.Config.UVS.InputParams.Count;
            var index_output_param = (index - 1) * Program.Settings.Config.UVS.OutputParams.Count;
            var InputParam = new List<BaseParam>();
            var OutputParam = new List<BaseParam>();
            var Setpoints = new List<BaseSetpoints>();

            #region Создаем задвижку

            #region Входные параметры
            for (int i = 0; i < Program.Settings.Config.UVS.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"VS_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UVS.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < Program.Settings.Config.UVS.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"VS_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UVS.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < Program.Settings.Config.UVS.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_VS[{index_setpoints + i + 1}]",
                    Id = $"H{7000 + index_setpoints + i}",
                    Unit = "",
                    Value = "",
                    Address = $"%MW{2000 + index_setpoints + i}",
                    Description = Program.Settings.Config.UVS.Setpoints[i].Text
                };
                Setpoints.Add(Param);
            }
            #endregion

            #region Генерируем задвижки
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
                InputParam = new List<BaseParam>(InputParam),
                OutputParam = new List<BaseParam>(OutputParam),
                Setpoints = new List<BaseSetpoints>(Setpoints)
            };
            data_list.Add(signal);
            #endregion

            SelectedUVS = data_list[^1];
            SelectedInputParam = SelectedUVS.InputParam[0];
            SelectedOutputParam = SelectedUVS.OutputParam[0];

            _DataViewInputParam.Source = SelectedUVS.InputParam;
            _DataViewInputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewInputParam));

            _DataViewOutputParam.Source = SelectedUVS.OutputParam;
            _DataViewOutputParam.View.Refresh();
            OnPropertyChanged(nameof(DataViewOutputParam));

            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
            return;
            #endregion

        }
        #endregion

        #region Команда - удалить вспомсистему
        private ICommand _CmdDeleteUVS;
        /// <summary>
        /// Команда - удалить вспомсистему
        /// </summary>
        public ICommand CmdDeleteUVS => _CmdDeleteUVS ??= new RelayCommand(OnCmdDeleteUVSExecuted, CanCmdDeleteUVSExecute);
        private bool CanCmdDeleteUVSExecute() => SelectedUVS is not null && !string.IsNullOrWhiteSpace(SelectedUVS.Description);

        private void OnCmdDeleteUVSExecuted()
        {
            var data_list = (List<BaseUVS>)_DataView.Source ?? new List<BaseUVS>();
            var index = data_list.IndexOf(SelectedUVS);
            data_list.Remove(SelectedUVS);

            if (data_list.Count > 0)
            {
                if (index > 0)
                    SelectedUVS = data_list[index - 1];
                else
                    SelectedUVS = data_list[index];
            }
            else
            {
                SelectedUVS = null;
            }

            _DataViewInputParam.Source = SelectedUVS?.InputParam;
            _DataViewInputParam.View?.Refresh();
            OnPropertyChanged(nameof(DataViewInputParam));

            _DataViewOutputParam.Source = SelectedUVS?.OutputParam;
            _DataViewOutputParam.View?.Refresh();
            OnPropertyChanged(nameof(DataViewOutputParam));

            _DataView.Source = data_list;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
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
            return;

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
                SelectedInputParam = SelectedUVS.InputParam[int.Parse(Index) - 1];

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
                SelectedOutputParam = SelectedUVS.OutputParam[int.Parse(Index) - 1];

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
    }
}
