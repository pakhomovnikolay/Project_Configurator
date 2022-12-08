using Project_Сonfigurator.Infrastructures.Commands;
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
    public class UserRegUserControlViewModel : ViewModel
    {
        #region Конструктор
        private ISignalService _SignalService;
        public UserRegUserControlViewModel(ISignalService signalService)
        {
            _SignalService = signalService;
            _DataView.Filter += OnSignalsFiltered;
            GeneratedSignals();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Регистры формируемые";
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
        private string _Description = "Регистры формируемые для ВУ";
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
                    _SignalService.ResetSignal();
                    DoSelection = false;
                }
            }
        }
        #endregion

        #region Коллекция регистров формируемых
        /// <summary>
        /// Коллекция регистров формируемых
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный регистр
        private BaseParam _SelectedSignal = new();
        /// <summary>
        /// Выбранный регистр
        /// </summary>
        public BaseParam SelectedSignal
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

            var data_list = new List<BaseParam>();
            foreach (BaseParam Signal in DataView)
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
            if (e.Item is not BaseParam Signal || Signal is null) { e.Accepted = false; return; }
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
            var data_list = new List<BaseParam>();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UserReg.Count > 0)
            {
                var signals = Program.Settings.AppData.UserReg;
                foreach (var signal in signals)
                {
                    data_list.Add(signal);
                }
            }
            #endregion

            #region Генерируем регистры формируемые
            while (index < 1000)
            {
                var signal = new BaseParam
                {
                    Index = $"{++index}",
                    Id = "",
                    Description = "",
                    VarName = $"user_reg[{index}]",
                    Address = $"{index}"
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
