using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class GroupsSignalUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;

        public GroupsSignalUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;

            _DataView.Filter += OnParamFiltered;
            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Группы сигналов";
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
        private string _Description = "Список групп сигналов";
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

            var data_list = new List<GroupSignal>();
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

            var data_list = new List<GroupSignal>();
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

            var data_list = new List<GroupSignal>();
            foreach (GroupSignal Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedParam.Param.Index)
                SelectedParam = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedParam.Param.Index;
            _SignalService.Id = SelectedParam.Param.Id;
            _SignalService.Description = SelectedParam.Param.Description;

            foreach (var _Item in App.FucusedTabControl.Items)
            {
                var _TabItem = _Item as TabItem;
                if (_TabItem.Header.ToString() == _SignalService.ListName)
                {
                    App.FucusedTabControl.SelectedItem = _TabItem;
                    break;
                }
            }
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
        public void GeneratedSignals()
        {
            var index = 0;
            var data_list = new List<GroupSignal>();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.GroupSignals.Count > 0)
            {
                var signals = Program.Settings.AppData.GroupSignals;
                foreach (var signal in signals)
                {
                    data_list.Add(signal);
                }
            }
            #endregion

            #region Генерируем сигналы
            while (data_list.Count < 128)
            {
                var signal = new GroupSignal()
                {
                    AddressEnd = "",
                    AddressStart = "",
                    QtyInGroup = "",
                    Param = new BaseParam
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"sig_grp[{index}]",
                        Address = "",
                        Inv = "",
                        TypeSignal = ""
                    }
                };
                data_list.Add(signal);
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
