using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class GroupsSignalUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public GroupsSignalUserControlViewModel()
        {
            Title = "Группы сигналов";
            Description = "Список групп сигналов";
            UsingUserControl = new GroupsSignalUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService SignalServices;
        public GroupsSignalUserControlViewModel(IUserDialogService _UserDialog, ISignalService _ISignalService) : this()
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
                    if (DoSelectionAddressStart)
                    {
                        DoSelectionAddressStart = SignalServices.RedefineAddress(SelectedParam.Param, _IsSelected, Title);
                        SelectedParam.AddressStart = SelectedParam.Param.Address;
                    }
                    if (DoSelectionAddressEnd)
                    {
                        DoSelectionAddressEnd = SignalServices.RedefineAddress(SelectedParam.Param, _IsSelected, Title);
                        SelectedParam.AddressEnd = SelectedParam.Param.Address;
                    }
                    DoSelection = DoSelectionAddressStart || DoSelectionAddressEnd;
                }
            }
        }
        #endregion

        #region Список параметров
        private ObservableCollection<GroupSignal> _Params = new();
        /// <summary>
        /// Список параметров
        /// </summary>
        public ObservableCollection<GroupSignal> Params
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
        private GroupSignal _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public GroupSignal SelectedParam
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
        public override bool DoSelection
        {
            get => _DoSelection;
            set => Set(ref _DoSelection, value);
        }
        #endregion

        #region Состояние необходимости выбора сигнала "От"
        private bool _DoSelectionAddressStart;
        /// <summary>
        /// Состояние необходимости выбора сигнала "От"
        /// </summary>
        public bool DoSelectionAddressStart
        {
            get => _DoSelectionAddressStart;
            set => Set(ref _DoSelectionAddressStart, value);
        }
        #endregion

        #region Состояние необходимости выбора сигнала "До"
        private bool _DoSelectionAddressEnd;
        /// <summary>
        /// Состояние необходимости выбора сигнала "До"
        /// </summary>
        public bool DoSelectionAddressEnd
        {
            get => _DoSelectionAddressEnd;
            set => Set(ref _DoSelectionAddressEnd, value);
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

        #region Команда - Сменить адрес сигнала "От"
        private ICommand _CmdChangeAddressStart;
        /// <summary>
        /// Команда - Сменить адрес сигнала "От"
        /// </summary>
        public ICommand CmdChangeAddressStart => _CmdChangeAddressStart ??= new RelayCommand(OnCmdChangeAddressStartExecuted, CanCmdChangeAddressStartExecute);
        private bool CanCmdChangeAddressStartExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressStartExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelectionAddressStart = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, TypeAddress.GrpSig);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес сигнала "До"
        private ICommand _CmdChangeAddressEnd;
        /// <summary>
        /// Команда - Сменить адрес сигнала "До"
        /// </summary>
        public ICommand CmdChangeAddressEnd => _CmdChangeAddressEnd ??= new RelayCommand(OnCmdChangeAddressEndExecuted, CanCmdChangeAddressEndExecute);
        private bool CanCmdChangeAddressEndExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressEndExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelectionAddressEnd = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, TypeAddress.GrpSig);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;
            if (App.FucusedTabControl == null) return;
            if (!SignalServices.DoSelection) return;

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            SignalServices.SelecteAddress(SelectedParam.Param);
            if (UserDialog.SearchControlViewModel(SignalServices.FromName) is not IViewModelUserControls _TabItem) return;
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
            if (e.Item is not GroupSignal _Param || _Param is null) { e.Accepted = false; return; }
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
            while (Params.Count < 128)
            {
                var param = new GroupSignal
                {
                    AddressEnd = "",
                    AddressStart = "",
                    QtyInGroup = "",
                    Param = new BaseParam
                    {
                        Index = $"{Params.Count + 1}",
                        Id = "",
                        Description = "",
                        Inv = "",
                        TypeSignal = "",
                        Address = "",
                        VarName = $"sig_grp[{Params.Count + 1}]",
                    }
                };
                Params.Add(param);
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
