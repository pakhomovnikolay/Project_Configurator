using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class KTPRUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public KTPRUserControlViewModel()
        {
            Title = "Общестанционные защиты";
            Description = "Массив общестанционных защит";
            UsingUserControl = new KTPRUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService SignalServices;
        public KTPRUserControlViewModel(IUserDialogService _UserDialog, ISignalService _ISignalService) : this()
        {
            UserDialog = _UserDialog;
            SignalServices = _ISignalService;
            _ParamsDataView.Filter += ParamsFiltered;
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
                    if (DoSelection) DoSelection = SignalServices.RedefineAddress(SelectedParam.Param, _IsSelected, Title);
                    if (_IsSelected) RefreshDataView();
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

        #region Список парметров
        private ObservableCollection<BaseKTPR> _Params = new();
        /// <summary>
        /// Список парметров
        /// </summary>
        public ObservableCollection<BaseKTPR> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (_Params is null || _Params.Count <= 0)
                    {
                        CreateData();
                        RefreshDataView();
                    }
                    else RefreshDataView();
                }
            }
        }
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

        #region Коллекция парметров для отображения
        /// <summary>
        /// Коллекция парметров для отображения
        /// </summary>
        private readonly CollectionViewSource _ParamsDataView = new();
        public ICollectionView ParamsDataView => _ParamsDataView?.View;
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
            RefreshDataView();
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
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelection = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedParam.Param.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать состояние сработки защиты
        private ICommand _CmdSelecteStateStation;
        /// <summary>
        /// Команда - Выбрать состояние сработки защиты
        /// </summary>
        public ICommand CmdSelecteStateStation => _CmdSelecteStateStation ??= new RelayCommand(OnCmdSelecteStateStationExecuted, CanCmdSelecteStateStationExecute);
        private bool CanCmdSelecteStateStationExecute(object p) => p is DataGrid;

        private void OnCmdSelecteStateStationExecuted(object p)
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
        public override object GetParam()
        {
            return Params;
        }
        #endregion

        #region Фильтрация парметров
        /// <summary>
        /// Фильтрация парметров
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseKTPR _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Параметры
            if (_Param.Param.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                _Param.Param.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            var DefaultParam = App.Settings.Config.DefualtMapKTPR;

            #region Считываем дпнные по умолчанию
            foreach (var item in DefaultParam)
            {
                if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;

                #region Создаем параметр
                var _Param = new BaseKTPR
                {
                    Type = "",
                    SubShoulder = "",
                    StopTypeUMPNA = "",
                    StopTypeNS = "",
                    StateStation = "",
                    Shoulder = "",
                    NoMasked = "",
                    Autodeblok = "",
                    ControlUTS = new BaseControlUTS(),
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD(),
                    Setpoints = item.Setpoints,
                    Param = item.Param
                };
                Params.Add(_Param);
                #endregion
            }
            #endregion

            while (Params.Count < 256)
            {
                #region Создаем данные параметра
                var param = new BaseParam
                {
                    Index = $"{Params.Count + 1}",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = $"ktpr_param[{Params.Count + 1}]"
                };
                #endregion

                #region Создаем уставки
                var setpoint = new BaseSetpoints
                {
                    Index = $"{Params.Count + 1}",
                    Id = $"H{2000 + Params.Count}",
                    Description = "",
                    VarName = $"SP_STAT_PROT[{Params.Count + 1}]",
                    Address = $"%MW{4800 + +Params.Count}",
                    Value = "",
                    Unit = "сек."
                };
                #endregion

                #region Создаем параметр
                var _Param = new BaseKTPR
                {
                    Type = "",
                    SubShoulder = "",
                    StopTypeUMPNA = "",
                    StopTypeNS = "",
                    StateStation = "",
                    Shoulder = "",
                    NoMasked = "",
                    Autodeblok = "",
                    ControlUTS = new BaseControlUTS(),
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD(),
                    Setpoints = setpoint,
                    Param = param
                };
                #endregion

                Params.Add(_Param);
            }
            if (Params.Count > 0)
                SelectedParam = Params[0];
        }
        #endregion

        #region Обновляем данные для отображения
        /// <summary>
        /// Обновляем данные для отображения
        /// </summary>
        private void RefreshDataView()
        {
            _ParamsDataView.Source = Params;
            _ParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(ParamsDataView));
        }
        #endregion

        #endregion
    }
}
