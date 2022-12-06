using Project_Сonfigurator.Infrastructures.Commands;
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
    public class KTPRUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;

        public KTPRUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;

            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Общестанционные защиты";
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
        private string _Description = "Массив общестанционных защит";
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
                    _SignalService.RedefineParam(SelectedParam.Param, _IsSelected, Title);
                    DoSelection = _SignalService.DoSelection;
                    if (_IsSelected)
                        _DataView.View.Refresh();
                }
            }
        }
        #endregion

        #region Коллекция парметров
        /// <summary>
        /// Коллекция парметров
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный параметр
        private BaseKTPR _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseKTPR SelectedParam
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

            var data_list = (List<BaseKTPR>)_DataView.Source ?? new List<BaseKTPR>();
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
            else if (int.Parse(SelectedParam.Param.TypeSignal) > 0)
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

        #endregion

        #region Функции

        #region Генерация сигналов
        private void GeneratedSignals()
        {
            var index = 0;
            var data_list = new List<BaseKTPR>();

            #region Создание параметров
            while (data_list.Count < 256)
            {
                var param = new BaseKTPR()
                {
                    Param = new BaseParam
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"ktpr_param[{index}]",
                        Inv = "",
                        TypeSignal = "",
                        Address = ""
                    },
                    StateStation = "",
                    Shoulder = "",
                    SubShoulder = "",
                    Autodeblok = "",
                    NoMasked = "",
                    StopTypeNS = "",
                    StopTypeUMPNA = "",
                    Type = "",
                    ControlUTS = new BaseControlUTS(),
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD(),
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{index}",
                        Value = "",
                        Unit = "",
                        Id = $"H{2000 + index - 1}",
                        VarName = $"SP_STAT_PROT[{index}]",
                        Address = $"%MW{4800 + index - 1}",
                        Description = ""
                    }
                };

                data_list.Add(param);
            }
            #endregion

            SelectedParam = data_list[0];
            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
            return;
        }
        #endregion 

        #endregion
    }
}
