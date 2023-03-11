using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UTSUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public UTSUserControlViewModel()
        {
            Title = "DO остальные";
            Description = "Ссылки отдельных дискретных выходов";
            UsingUserControl = new UTSUserControl();
            _ParamsDataView.Filter += ParamsFiltered;
        }

        private readonly ISignalService SignalServices;
        public UTSUserControlViewModel(ISignalService _ISignalService) : this()
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
                    if (DoSelectionKCOParam) DoSelectionKCOParam = SignalServices.RedefineAddress(SelectedParam.KCO, _IsSelected, Title);
                    else if (DoSelectionSignalSOD) DoSelectionSignalSOD = SignalServices.RedefineAddress(SelectedParam.SignalSOD, _IsSelected, Title);
                    else if (DoSelectionSignalErrSOD) DoSelectionSignalErrSOD = SignalServices.RedefineAddress(SelectedParam.SignalErrSOD, _IsSelected, Title);
                    else if (DoSelection) DoSelection = SignalServices.RedefineAddress(SelectedParam.Param, _IsSelected, Title);
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
        private ObservableCollection<BaseUTS> _Params = new();
        /// <summary>
        /// Список парметров
        /// </summary>
        public ObservableCollection<BaseUTS> Params
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
        private BaseUTS _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseUTS SelectedParam
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

        #region Состояние необходимости выбора параметра контроля целостности цепей
        private bool _DoSelectionKCOParam;
        /// <summary>
        /// Состояние необходимости выбора параметра контроля целостности цепей
        /// </summary>
        public bool DoSelectionKCOParam
        {
            get => _DoSelectionKCOParam;
            set => Set(ref _DoSelectionKCOParam, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра датчика прохождения СОД
        private bool _DoSelectionSignalSOD;
        /// <summary>
        /// Состояние необходимости выбора параметра датчика прохождения СОД
        /// </summary>
        public bool DoSelectionSignalSOD
        {
            get => _DoSelectionSignalSOD;
            set => Set(ref _DoSelectionSignalSOD, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра датчика прохождения СОД (неисправность)
        private bool _DoSelectionSignalErrSOD;
        /// <summary>
        /// Состояние необходимости выбора параметра датчика прохождения СОД (неисправность)
        /// </summary>
        public bool DoSelectionSignalErrSOD
        {
            get => _DoSelectionSignalErrSOD;
            set => Set(ref _DoSelectionSignalErrSOD, value);
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
            RefreshDataView();
        }
        #endregion

        #region Команда - Сменить адрес
        private ICommand _CmdChangeAddress;
        /// <summary>
        /// Команда - Сменить адрес
        /// </summary>
        public ICommand CmdChangeAddress => _CmdChangeAddress ??= new RelayCommand(OnCmdChangeAddressExecuted, CanCmdChangeAddressExecute);
        private bool CanCmdChangeAddressExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelection = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, TypeAddress.DO);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра контроля целостности цепей
        private ICommand _CmdChangeAddressKCO;
        /// <summary>
        /// Команда - Сменить адрес параметра контроля целостности цепей
        /// </summary>
        public ICommand CmdChangeAddressKCO => _CmdChangeAddressKCO ??= new RelayCommand(OnCmdChangeAddressKCOExecuted, CanCmdChangeAddressKCOExecute);
        private bool CanCmdChangeAddressKCOExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressKCOExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelectionKCOParam = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, "9");
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра датчика прохождения СОД
        private ICommand _CmdChangeAddressSignalSOD;
        /// <summary>
        /// Команда - Сменить адрес параметра датчика прохождения СОД
        /// </summary>
        public ICommand CmdChangeAddressSignalSOD => _CmdChangeAddressSignalSOD ??= new RelayCommand(OnCmdChangeAddressSignalSODExecuted, CanCmdChangeAddressSignalSODExecute);
        private bool CanCmdChangeAddressSignalSODExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalSODExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelectionSignalSOD = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedParam.SignalSOD.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра датчика прохождения СОД (неисправность)
        private ICommand _CmdChangeAddressSignalErrSOD;
        /// <summary>
        /// Команда - Сменить адрес параметра датчика прохождения СОД (неисправность)
        /// </summary>
        public ICommand CmdChangeAddressSignalErrSOD => _CmdChangeAddressSignalErrSOD ??= new RelayCommand(OnCmdChangeAddressSignalErrSODExecuted, CanCmdChangeAddressSignalErrSODExecute);
        private bool CanCmdChangeAddressSignalErrSODExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalErrSODExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelectionSignalErrSOD = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedParam.SignalErrSOD.TypeSignal);
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
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<BaseUTS>;
        #endregion

        #region Фильтрация парметров
        /// <summary>
        /// Фильтрация парметров
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseUTS _Param || _Param is null) { e.Accepted = false; return; }
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
                    VarName = $"uts_param[{Params.Count + 1}]"
                };
                #endregion

                #region Создаем данные для контроля исправности цепей
                var kco = new BaseParam
                {
                    Index = "",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = "",
                };
                #endregion

                #region Создаем данные для датчика прохождения СОД
                var sod = new BaseParam
                {
                    Index = "",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = "",
                };
                #endregion

                #region Создаем данные для датчика прохождения СОД (неисправность)
                var sod_err = new BaseParam
                {
                    Index = "",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = "",
                };
                #endregion

                #region Создаем параметр
                var _Param = new BaseUTS
                {
                    Type = "",
                    AptOff = "",
                    IndexGroup = "",
                    IndexPZ = "",
                    LockEnable = "",
                    TypeCOz = "",
                    KCO = kco,
                    SignalSOD = sod,
                    SignalErrSOD = sod_err,
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
