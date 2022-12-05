using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsAIUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private ISignalService _SignalService;
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }

        public SignalsAIUserControlViewModel(IUserDialogService userDialog, TableSignalsUserControlViewModel tableSignalsViewModel, ISignalService signalService)
        {
            UserDialog = userDialog;
            TableSignalsViewModel = tableSignalsViewModel;
            _SignalService = signalService;

            _DataView.Filter += OnSignalsDOFiltered;
            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Сигналы AI";
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
        private string _Description = "Аналоговые сигналы (AI)";
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
                    _SignalService.RedefineSignal(SelectedSignalAI.Signal, _IsSelected, Title);
                    DoSelection = _SignalService.DoSelection;
                    if (_IsSelected)
                        _DataView.View.Refresh();

                }
            }
        }
        #endregion

        #region Коллекция сигналов AI
        /// <summary>
        /// Коллекция сигналов AI
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный сигнал AI
        private SignalAI _SelectedSignalAI = new();
        /// <summary>
        /// Выбранный сигнал AI
        /// </summary>
        public SignalAI SelectedSignalAI
        {
            get => _SelectedSignalAI;
            set => Set(ref _SelectedSignalAI, value);
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

        #region Команда - Генерировать таблицу
        private ICommand _CmdGeneratedTable;
        /// <summary>
        /// Команда - Генерировать таблицу
        /// </summary>
        public ICommand CmdGeneratedTable => _CmdGeneratedTable ??= new RelayCommand(OnCmdGeneratedTableExecuted, CanCmdGeneratedTableExecute);
        private bool CanCmdGeneratedTableExecute() => true;

        private void OnCmdGeneratedTableExecuted()
        {
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None)) return;

            GeneratedSignals();

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
            if (_DataView.Source is null) return;
            _DataView.View.Refresh();

        }
        #endregion

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedSignalAI is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignalAI is null) return;

            var data_list = new List<SignalAI>();
            foreach (SignalAI SignalAI in DataView)
            {
                data_list.Add(SignalAI);
            }

            if (Index != SelectedSignalAI.Signal.Index)
                SelectedSignalAI = data_list[int.Parse(Index) - 1];

            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.AI;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedSignalAI.Signal.Area) || int.Parse(SelectedSignalAI.Signal.Area) == 0)
                NameListSelected = "Таблица сигналов";
            else if (int.Parse(SelectedSignalAI.Signal.Area) == 1)
                NameListSelected = "AI формируемые";

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
        private bool CanCmdSelectionSignalExecute(object p) => SelectedSignalAI is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignalAI is null) return;
            if (App.FucusedTabControl == null) return;
            if (!_SignalService.DoSelection) return;

            var data_list = new List<SignalAI>();
            foreach (SignalAI Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedSignalAI.Signal.Index)
                SelectedSignalAI = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedSignalAI.Signal.Index;
            _SignalService.Id = SelectedSignalAI.Signal.Id;
            _SignalService.Description = SelectedSignalAI.Signal.Description;

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
        private void OnSignalsDOFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalAI Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        private void GeneratedSignals()
        {
            var index = 0;
            var data_list = new List<SignalAI>();

            #region Генерируем сигналы AI, при отсутсвии данных во владке Таблица сигналов
            if (TableSignalsViewModel is null || TableSignalsViewModel.DataViewModules is null)
            {
                while (data_list.Count < 500)
                {
                    var signal = new SignalAI()
                    {
                        Signal = new BaseSignal
                        {
                            Index = $"{++index}",
                            Id = "",
                            Description = "",
                            VarName = $"ai_shared[{index}]",
                            Area = "",
                            Address = "",
                            LinkValue = ""
                        }
                    };
                    data_list.Add(signal);
                }

                SelectedSignalAI = data_list[0];
                _DataView.Source = data_list;
                _DataView.View.Refresh();
                OnPropertyChanged(nameof(DataView));
                return;
            }
            #endregion

            #region Генерируем сигналы AI, созданные во вкладке Таблица сигналов
            var uso_list = TableSignalsViewModel.LayotRackViewModel.USOList;
            foreach (var _USO in uso_list)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Type == TypeModule.AI)
                        {
                            foreach (var _Channel in _Module.Channels)
                            {
                                var signal = new SignalAI()
                                {
                                    Signal = new BaseSignal
                                    {
                                        Index = $"{++index}",
                                        Id = _Channel.Id,
                                        Description = _Channel.Description,
                                        VarName = $"ai_shared[{index}]",
                                        Area = "",
                                        Address = $"{int.Parse(_Channel.Address)}",
                                    }
                                };
                                data_list.Add(signal);
                            }
                        }
                    }
                }
            }

            while (data_list.Count < 500)
            {
                var signal = new SignalAI()
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"ai_shared[{index}]",
                        Area = "",
                        Address = "",
                        LinkValue = ""
                    }
                };
                data_list.Add(signal);
            }
            #endregion

            SelectedSignalAI = data_list[0];
            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
