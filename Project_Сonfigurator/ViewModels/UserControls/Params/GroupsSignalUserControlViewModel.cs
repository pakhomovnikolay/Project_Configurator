using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class GroupsSignalUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public GroupsSignalUserControlViewModel()
        {
            Title = "Группы сигналов";
            Description = "Список групп сигналов";
            UsingUserControl = new GroupsSignalUserControl();
        }

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;

        public GroupsSignalUserControlViewModel(ISignalService signalService, IDBService dBService) : this()
        {
            _SignalService = signalService;
            _DBService = dBService;

            _DataView.Filter += OnParamFiltered;
            _DBService.RefreshDataViewModel(this, false);
        }
        #endregion

        #region Параметры

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
                            if (DoSelectionAddressStart)
                            {
                                SelectedParam.AddressStart = _SignalService.Address;
                                if (string.IsNullOrWhiteSpace(SelectedParam.Param.Id))
                                    SelectedParam.Param.Id = _SignalService.Id;
                                if (string.IsNullOrWhiteSpace(SelectedParam.Param.Description))
                                    SelectedParam.Param.Description = _SignalService.Description;
                            }

                            if (DoSelectionAddressEnd)
                            {
                                SelectedParam.AddressEnd = _SignalService.Address;
                                if (string.IsNullOrWhiteSpace(SelectedParam.Param.Id))
                                    SelectedParam.Param.Id = _SignalService.Id;
                                if (string.IsNullOrWhiteSpace(SelectedParam.Param.Description))
                                    SelectedParam.Param.Description = _SignalService.Description;
                            }

                            DoSelectionAddressStart = false;
                            DoSelectionAddressEnd = false;
                            DoSelection = false;
                            _SignalService.ResetSignal();
                            _DataView.View?.Refresh();


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

        #region Список параметров
        private ObservableCollection<GroupSignal> _GroupSignals = new();
        /// <summary>
        /// Список параметров
        /// </summary>
        public ObservableCollection<GroupSignal> GroupSignals
        {
            get => _GroupSignals;
            set => Set(ref _GroupSignals, value);
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
        public bool DoSelection
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

            var data_list = new ObservableCollection<GroupSignal>();
            foreach (GroupSignal Param in DataView)
            {
                data_list.Add(Param);
            }

            if (Index != SelectedParam.Param.Index)
                SelectedParam = data_list[int.Parse(Index) - 1];

            DoSelectionAddressStart = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.DI;

            var NameListSelected = "Сигналы групп";

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
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

            var data_list = new ObservableCollection<GroupSignal>();
            foreach (GroupSignal Param in DataView)
            {
                data_list.Add(Param);
            }

            if (Index != SelectedParam.Param.Index)
                SelectedParam = data_list[int.Parse(Index) - 1];

            DoSelectionAddressEnd = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.DI;

            var NameListSelected = "Сигналы групп";

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
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
            if (!_SignalService.DoSelection) return;
                
            var data_list = new ObservableCollection<GroupSignal>();
            foreach (GroupSignal Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedParam.Param.Index)
                SelectedParam = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedParam.Param.Index;
            _SignalService.Id = SelectedParam.Param.Id;
            _SignalService.Description = SelectedParam.Param.Description;

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == _SignalService.ListName
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        private void OnParamFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not GroupSignal Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Param.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Param.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        public void GeneratedData()
        {
            _DataView.Source = GroupSignals;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
