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
    public class SignalsAOUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private ISignalService _SignalService;
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }

        public SignalsAOUserControlViewModel(IUserDialogService userDialog, TableSignalsUserControlViewModel tableSignalsViewModel, ISignalService signalService)
        {
            UserDialog = userDialog;
            TableSignalsViewModel = tableSignalsViewModel;
            _SignalService = signalService;

            _DataView.Filter += OnSignalsAOFiltered;
            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Сигналы AO";
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
        private string _Description = "Дискретные сигналы (AO)";
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
                            _SignalService.RedefineSignal(SelectedSignalAO.Signal);
                            _DataView.View?.Refresh();
                            DoSelection = false;


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

        #region Коллекция сигналов AO
        /// <summary>
        /// Коллекция сигналов AO
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный сигнал AO
        private SignalAO _SelectedSignalAO = new();
        /// <summary>
        /// Выбранный сигнал AO
        /// </summary>
        public SignalAO SelectedSignalAO
        {
            get => _SelectedSignalAO;
            set => Set(ref _SelectedSignalAO, value);
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
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedSignalAO is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignalAO is null) return;

            var data_list = new List<SignalAO>();
            foreach (SignalAO SignalAO in DataView)
            {
                data_list.Add(SignalAO);
            }

            if (Index != SelectedSignalAO.Signal.Index)
                SelectedSignalAO = data_list[int.Parse(Index) - 1];

            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.AO;

            var NameListSelected = "Таблица сигналов";
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

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        private void OnSignalsAOFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalAO Signal || Signal is null) { e.Accepted = false; return; }
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
            var data_list = new List<SignalAO>();

            #region Генерируем сигналы AO, при отсутсвии данных во владке Таблица сигналов
            if (TableSignalsViewModel is null || TableSignalsViewModel.DataViewModules is null)
            {
                while (data_list.Count < 500)
                {
                    var signal = new SignalAO()
                    {
                        Signal = new BaseSignal
                        {
                            Index = $"{++index}",
                            Id = "",
                            Description = "",
                            VarName = $"ao_shared[{index}]",
                            Area = "",
                            Address = "",
                            LinkValue = ""
                        }
                    };
                    data_list.Add(signal);
                }

                SelectedSignalAO = data_list[0];
                _DataView.Source = data_list;
                _DataView.View.Refresh();
                OnPropertyChanged(nameof(DataView));
                return;
            }
            #endregion

            #region Генерируем сигналы AO, созданные во вкладке Таблица сигналов
            var uso_list = TableSignalsViewModel.LayotRackViewModel.USOList;
            foreach (var _USO in uso_list)
            {
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Type == TypeModule.AO)
                        {
                            foreach (var _Channel in _Module.Channels)
                            {
                                var signal = new SignalAO()
                                {
                                    Signal = new BaseSignal
                                    {
                                        Index = $"{++index}",
                                        Id = _Channel.Id,
                                        Description = _Channel.Description,
                                        VarName = $"ao_shared[{index}]",
                                        Area = "",
                                        Address = $"{int.Parse(_Channel.Address) - 300000}",
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
                var signal = new SignalAO()
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"ao_shared[{index}]",
                        Area = "",
                        Address = "",
                        LinkValue = ""
                    }
                };
                data_list.Add(signal);
            }
            #endregion

            SelectedSignalAO = data_list[0];
            _DataView.Source = data_list;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
