using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UZDUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;
        public UZDUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;
            _DataView.Filter += OnSignalsFiltered;
            //GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Настройки задвижек";
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
        private string _Description = "Текущие массивы состояний задвижек";
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
                    _SignalService.ResetSignal();
                    DoSelection = false;
                }
            }
        }
        #endregion

        #region Коллекция задвижек
        /// <summary>
        /// Коллекция задвижек
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранная задвижка
        private BaseUZD _SelectedUZD = new();
        /// <summary>
        /// Выбранная задвижка
        /// </summary>
        public BaseUZD SelectedUZD
        {
            get => _SelectedUZD;
            set => Set(ref _SelectedUZD, value);
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

        #region Команда - Добавить задвижку
        private ICommand _CmdAddUZD;
        /// <summary>
        /// Команда - Добавить задвижку
        /// </summary>
        public ICommand CmdAddUZD => _CmdAddUZD ??= new RelayCommand(OnCmdAddUZDExecuted, CanCmdAddUZDExecute);
        private bool CanCmdAddUZDExecute() => true;

        private void OnCmdAddUZDExecuted()
        {
            var data_list = (List<BaseUZD>)_DataView.Source ?? new List<BaseUZD>();

            var index = data_list.Count + 1;
            var index_setpoints = (index - 1) * Program.Settings.Config.UZD.Setpoints.Count;
            var index_input_param = (index - 1) * Program.Settings.Config.UZD.InputParams.Count;
            var index_output_param = (index - 1) * Program.Settings.Config.UZD.OutputParams.Count;
            var InputParam = new List<BaseParam>();
            var OutputParam = new List<BaseParam>();
            var Setpoints = new List<BaseSetpoints>();

            #region Создаем задвижку

            #region Входные параметры
            for (int i = 0; i < Program.Settings.Config.UZD.InputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"ZD_DI_P[{index_input_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UZD.InputParams[i].Text
                };
                InputParam.Add(Param);
            }
            #endregion

            #region Выходные параметры
            for (int i = 0; i < Program.Settings.Config.UZD.OutputParams.Count; i++)
            {
                var Param = new BaseParam
                {
                    Index = $"{i + 1}",
                    VarName = $"ZD_DO_P[{index_output_param + i + 1}]",
                    Id = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    Description = Program.Settings.Config.UZD.OutputParams[i].Text
                };
                OutputParam.Add(Param);
            }
            #endregion

            #region Уставки
            for (int i = 0; i < Program.Settings.Config.UZD.Setpoints.Count; i++)
            {
                var Param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    VarName = $"SP_TM_ZD[{index_setpoints + i + 1}]",
                    Id = $"H800{index_setpoints + i}",
                    Unit = "",
                    Value = "",
                    Address = "",
                    Description = Program.Settings.Config.UZD.Setpoints[i].Text
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
                InputParam = new List<BaseParam>(InputParam),
                OutputParam = new List<BaseParam>(OutputParam),
                Setpoints = new List<BaseSetpoints>(Setpoints)
            };
            data_list.Add(signal);
            #endregion

            SelectedUZD = data_list[^1];
            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
            return;
            #endregion

        }
        #endregion

        #region Команда - удалить задвижку
        private ICommand _CmdDeleteUZD;
        /// <summary>
        /// Команда - удалить задвижку
        /// </summary>
        public ICommand CmdDeleteUZD => _CmdDeleteUZD ??= new RelayCommand(OnCmdDeleteUZDExecuted, CanCmdDeleteUZDExecute);
        private bool CanCmdDeleteUZDExecute() => SelectedUZD is not null && !string.IsNullOrWhiteSpace(SelectedUZD.Description);

        private void OnCmdDeleteUZDExecuted()
        {
            var data_list = (List<BaseUZD>)_DataView.Source ?? new List<BaseUZD>();
            var index = data_list.IndexOf(SelectedUZD);
            data_list.Remove(SelectedUZD);

            if (data_list.Count > 0)
            {
                if (index > 0)
                    SelectedUZD = data_list[index - 1];
                else
                    SelectedUZD = data_list[index];
            }
            else
            {
                SelectedUZD = null;
            }
            _DataView.Source = data_list;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
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
            return;

        }
        #endregion


        //#region Команда - Выбрать сигнал
        //private ICommand _CmdSelectionSignal;
        ///// <summary>
        ///// Команда - Выбрать сигнал
        ///// </summary>
        //public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        //private bool CanCmdSelectionSignalExecute(object p) => SelectedSignal is not null;

        //private void OnCmdSelectionSignalExecuted(object p)
        //{
        //    if (p is not string Index) return;
        //    if (string.IsNullOrWhiteSpace(Index)) return;
        //    if (SelectedSignal is null) return;
        //    if (App.FucusedTabControl == null) return;

        //    var data_list = new List<BaseUZD>();
        //    foreach (BaseUZD Signal in DataView)
        //    {
        //        data_list.Add(Signal);
        //    }

        //    if (Index != SelectedSignal.Index)
        //        SelectedSignal = data_list[int.Parse(Index) - 1];

        //    _SignalService.Address = SelectedSignal.Address;
        //    _SignalService.Id = SelectedSignal.Id;
        //    _SignalService.Description = SelectedSignal.Description;

        //    foreach (var _Item in App.FucusedTabControl.Items)
        //    {
        //        var _TabItem = _Item as TabItem;
        //        if (_TabItem.Header.ToString() == _SignalService.ListName)
        //        {
        //            App.FucusedTabControl.SelectedItem = _TabItem;
        //            break;
        //        }
        //    }
        //}
        //#endregion

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
            if (e.Item is not BaseUZD Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        private void GeneratedSignals()
        {
            var index = 0;
            var data_list = new List<BaseUZD>();
            var InputParam = new List<BaseParam>();
            var OutputParam = new List<BaseParam>();
            var Setpoints = new List<BaseSetpoints>();
            #region Создаем задвижку
            while (index < 100)
            {
                ++index;
                #region Входные параметры
                for (int i = 0; i < Program.Settings.Config.UZD.InputParams.Count; i++)
                {
                    var Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        VarName = $"ZD_DI_P{index}",
                        Id = "",
                        Inv = "",
                        TypeSignal = "",
                        Address = "",
                        Description = Program.Settings.Config.UZD.InputParams[i].Text
                    };
                    InputParam.Add(Param);
                }
                #endregion

                #region Выходные параметры
                for (int i = 0; i < Program.Settings.Config.UZD.OutputParams.Count; i++)
                {
                    var Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        VarName = $"ZD_DO_P{index}",
                        Id = "",
                        Inv = "",
                        TypeSignal = "",
                        Address = "",
                        Description = Program.Settings.Config.UZD.OutputParams[i].Text
                    };
                    OutputParam.Add(Param);
                }
                #endregion

                #region Уставки
                for (int i = 0; i < Program.Settings.Config.UZD.Setpoints.Count; i++)
                {
                    var Param = new BaseSetpoints
                    {
                        Index = $"{i + 1}",
                        VarName = $"H800{index - 1}",
                        Id = "",
                        Unit = "",
                        Value = "",
                        Address = "",
                        Description = Program.Settings.Config.UZD.Setpoints[i].Text
                    };
                    Setpoints.Add(Param);
                }
                #endregion

                #region Генерируем задвижки
                var signal = new BaseUZD
                {
                    Index = $"{index}",
                    Description = "",
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
                    InputParam = new List<BaseParam>(InputParam),
                    OutputParam = new List<BaseParam>(OutputParam),
                    Setpoints = new List<BaseSetpoints>(Setpoints)
                };
                data_list.Add(signal);
                #endregion
            }
            SelectedUZD = data_list[0];
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
