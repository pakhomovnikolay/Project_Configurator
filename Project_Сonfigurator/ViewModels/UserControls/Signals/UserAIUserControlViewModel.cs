using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class UserAIUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;
        public UserAIUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;
            _DataView.Filter += OnSignalsFiltered;
            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "AI формируемые";
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
        private string _Description = "AI формируемые от ПЛК";
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
                    _SignalService.RedefineSignal(SelectedSignal, _IsSelected, Title);
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
        private BaseSignal _SelectedSignal = new();
        /// <summary>
        /// Выбранный сигнал AI
        /// </summary>
        public BaseSignal SelectedSignal
        {
            get => _SelectedSignal;
            set => Set(ref _SelectedSignal, value);
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

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SelectedSignal is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignal is null) return;
            if (App.FucusedTabControl == null) return;
            if (!_SignalService.DoSelection) return;

            var data_list = new List<BaseSignal>();
            foreach (BaseSignal Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedSignal.Index)
                SelectedSignal = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedSignal.Address;
            _SignalService.Id = SelectedSignal.Id;
            _SignalService.Description = SelectedSignal.Description;

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
        private void OnSignalsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseSignal Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        private void GeneratedSignals()
        {
            var index = 0;
            var data_list = new List<BaseSignal>();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UserAI.Count > 0)
            {
                var signals = Program.Settings.AppData.UserAI;
                foreach (var signal in signals)
                {
                    data_list.Add(signal);
                }
            }
            #endregion

            #region Генерируем сигналы AI
            while (index < 500)
            {
                var signal = new BaseSignal
                {
                    Index = $"{++index}",
                    Id = "",
                    Description = "",
                    VarName = $"user_ai[{index}]",
                    Area = "",
                    Address = $"{index}",
                    LinkValue = ""
                };
                data_list.Add(signal);
            }
            SelectedSignal = data_list[0];
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
