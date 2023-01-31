using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Signals;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsAIUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public SignalsAIUserControlViewModel()
        {
            Title = "Сигналы AI";
            Description = "Аналоговые сигналы (AI)";
            UsingUserControl = new SignalsAIUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public SignalsAIUserControlViewModel(IUserDialogService _UserDialog, ISignalService _ISignalService, IDBService _IDBService) : this()
        {
            UserDialog = _UserDialog;
            SignalServices = _ISignalService;
            DBServices = _IDBService;
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
                    if (DoSelection) DoSelection = SignalServices.RedefineAddress(SelectedParam.Signal, _IsSelected, Title);
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

        #region Список сигналов AI
        private ObservableCollection<SignalAI> _Params = new();
        /// <summary>
        /// Список сигналов 
        /// </summary>
        public ObservableCollection<SignalAI> Params
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

        #region Выбранный сигнал AI
        private SignalAI _SelectedParam = new();
        /// <summary>
        /// Выбранный сигнал AI
        /// </summary>
        public SignalAI SelectedParam
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

        #region Коллекция сигналов AI для отображения
        /// <summary>
        /// Коллекция сигналов AI для отображения
        /// </summary>
        private readonly CollectionViewSource _ParamsDataView = new();
        public ICollectionView ParamsDataView => _ParamsDataView?.View;
        #endregion

        #endregion

        #region Команды

        #region Команда - Генерировать таблицу
        private ICommand _CmdGeneratedTable;
        /// <summary>
        /// Команда - Генерировать таблицу
        /// </summary>
        public ICommand CmdGeneratedTable => _CmdGeneratedTable ??= new RelayCommand(OnCmdGeneratedTableExecuted, CanCmdGeneratedTableExecute);
        private bool CanCmdGeneratedTableExecute() => true;

        private void OnCmdGeneratedTableExecuted()
        {
            #region Импорт сигналов из ТБ
            if (UserDialog.SearchControlViewModel("Таблица сигналов") is not TableSignalsUserControlViewModel _TabItem) return;
            TableSignalsUserControlViewModel MyViewModel = _TabItem;

            if (MyViewModel is null) return;
            if (MyViewModel.Params is null) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            var _Params = new ObservableCollection<SignalAI>();
            foreach (var _USO in MyViewModel.Params)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Type == TypeModule.AI)
                        {
                            if (_Module.Channels is null || _Module.Channels.Count <= 0)
                                if (UserDialog.SendMessage("Внимание!", "Неверные данные таблицы сигналов. Проверьте вкладку",
                                    MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;

                            foreach (var Channel in _Module.Channels)
                            {
                                if ((string.IsNullOrWhiteSpace(Channel.Id) && string.IsNullOrWhiteSpace(Channel.Description)) ||
                                    Channel.Description == "Резерв") continue;

                                var param = new SignalAI
                                {
                                    AddresUTS = "",
                                    ConverterKgs = "",
                                    IndexBD = "",
                                    IndexNA = "",
                                    IndexPZ = "",
                                    LevelRPP = "",
                                    TypePI = "",
                                    TypeVibration = "",
                                    Unit = "",
                                    Signal = new BaseSignal
                                    {
                                        Index = $"{_Params.Count + 1}",
                                        Id = Channel.Id,
                                        Description = Channel.Description,
                                        Area = "",
                                        Address = Channel.Address,
                                        VarName = $"ai_shared[{_Params.Count + 1}]",

                                    }
                                };
                                _Params.Add(param);
                            }
                        }
                    }
                }
            }
            Params = new ObservableCollection<SignalAI>(_Params);
            UserDialog.SendMessage("Импорт сигналов AI", "Сигналы AI успешно импортированы\nиз таблицы сигналов",
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            #endregion

            CreateData();
            RefreshDataView();
        }
        #endregion

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

            if (Index != SelectedParam.Signal.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelection = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, TypeAddress.AI, SelectedParam.Signal.Area);
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

            if (Index != SelectedParam.Signal.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            SignalServices.SelecteAddress(SelectedParam.Signal);
            if (UserDialog.SearchControlViewModel(SignalServices.FromName) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать МПНА
        private ICommand _CmdSelecteUMPNA;
        /// <summary>
        /// Команда - Выбрать МПНА
        /// </summary>
        public ICommand CmdSelecteUMPNA => _CmdSelecteUMPNA ??= new RelayCommand(OnCmdSelecteUMPNAExecuted, CanCmdSelecteUMPNAExecute);
        private bool CanCmdSelecteUMPNAExecute(object p) => p is DataGrid;

        private void OnCmdSelecteUMPNAExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация сигналов AI
        /// <summary>
        /// Фильтрация сигналов AI
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalAI _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Сигналы AI
            if (_Param.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                _Param.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            while (Params.Count < 500)
            {
                var param = new SignalAI
                {
                    AddresUTS = "",
                    ConverterKgs = "",
                    IndexBD = "",
                    IndexNA = "",
                    IndexPZ = "",
                    LevelRPP = "",
                    TypePI = "",
                    TypeVibration = "",
                    Unit = "",
                    Signal = new BaseSignal
                    {
                        Index = $"{Params.Count + 1}",
                        Id = "",
                        Description = "",
                        Area = "",
                        Address = "",
                        VarName = $"ai_shared[{Params.Count + 1}]",
                        LinkValue = ""
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