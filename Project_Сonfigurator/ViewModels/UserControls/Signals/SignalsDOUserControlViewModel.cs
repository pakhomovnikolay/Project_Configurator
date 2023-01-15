using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsDOUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public SignalsDOUserControlViewModel()
        {
            Title = "Сигналы DO";
            Description = "Дискретные сигналы (DO)";
            UsingUserControl = new SignalsDOUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public SignalsDOUserControlViewModel(IUserDialogService _UserDialog, ISignalService _ISignalService, IDBService _IDBService) : this()
        {
            UserDialog = _UserDialog;
            SignalServices = _ISignalService;
            DBServices = _IDBService;
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
                    //if (SelectedSignalDO is not null)
                    //    _SignalService.RedefineSignal(SelectedSignalDO.Signal, _IsSelected, Title);
                    //DoSelection = _SignalService.DoSelection;
                    //if (_IsSelected)
                    //    _DataView.View?.Refresh();
                }
            }
        }
        #endregion

        #region Список сигналов DO
        private ObservableCollection<SignalDO> _Params = new();
        /// <summary>
        /// Список сигналов DO
        /// </summary>
        public ObservableCollection<SignalDO> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
        #endregion

        #region Выбранный сигнал DO
        private SignalDO _SelectedParam = new();
        /// <summary>
        /// Выбранный сигнал DO
        /// </summary>
        public SignalDO SelectedParam
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
            //if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
            //    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None)) return;

            //GeneratedSignals();

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
            //if (_DataView.Source is null) return;
            //_DataView.View.Refresh();

        }
        #endregion

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => /*SelectedSignalDO is not null*/true;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedSignalDO is null) return;

            //var data_list = new ObservableCollection<SignalDO>();
            //foreach (SignalDO SignalDO in DataView)
            //{
            //    data_list.Add(SignalDO);
            //}

            //if (Index != SelectedSignalDO.Signal.Index)
            //    SelectedSignalDO = data_list[int.Parse(Index) - 1];

            //_SignalService.DoSelection = true;
            //_SignalService.ListName = Title;
            //_SignalService.Type = TypeModule.DO;

            //var NameListSelected = "Таблица сигналов";

            //if (App.FucusedTabControl == null) return;
            //foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
            //                         let _TabItem = _Item as TabItem
            //                         where _TabItem.Header.ToString() == NameListSelected
            //                         select _TabItem)
            //    App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => /*SelectedSignalDO is not null*/true;

        private void OnCmdSelectionSignalExecuted(object p)
        {
        //    if (p is not string Index) return;
        //    if (string.IsNullOrWhiteSpace(Index)) return;
        //    if (SelectedSignalDO is null) return;
        //    if (App.FucusedTabControl == null) return;
        //    if (!_SignalService.DoSelection) return;

        //    var data_list = new ObservableCollection<SignalDO>();
        //    foreach (SignalDO Signal in DataView)
        //    {
        //        data_list.Add(Signal);
        //    }

        //    if (Index != SelectedSignalDO.Signal.Index)
        //        SelectedSignalDO = data_list[int.Parse(Index) - 1];

        //    _SignalService.Address = SelectedSignalDO.Signal.Index;
        //    _SignalService.Id = SelectedSignalDO.Signal.Id;
        //    _SignalService.Description = SelectedSignalDO.Signal.Description;

        //    if (App.FucusedTabControl == null) return;
        //    foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
        //                             let _TabItem = _Item as TabItem
        //                             where _TabItem.Header.ToString() == _SignalService.ListName
        //                             select _TabItem)
        //        App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        //#region Фильтрация модулей
        ///// <summary>
        ///// Фильтрация модулей
        ///// </summary>
        //private void OnSignalsDOFiltered(object sender, FilterEventArgs e)
        //{
        //    #region Проверки до начала фильтрации
        //    // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
        //    if (e.Item is not SignalDO Signal || Signal is null) { e.Accepted = false; return; }
        //    if (string.IsNullOrWhiteSpace(TextFilter)) return;
        //    #endregion

        //    if (Signal.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
        //            Signal.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

        //    e.Accepted = false;
        //}
        //#endregion

        //#region Генерация сигналов
        //private void GeneratedSignals()
        //{
        //    var index = 0;
        //    SignalsDO = new();

        //    #region Генерируем сигналы DO, при отсутсвии данных во владке Таблица сигналов
        //    if (TableSignalsViewModel is null || TableSignalsViewModel.DataViewModules is null)
        //    {
        //        while (SignalsDO.Count < 2500)
        //        {
        //            var signal = new SignalDO()
        //            {
        //                Signal = new BaseSignal
        //                {
        //                    Index = $"{++index}",
        //                    Id = "",
        //                    Description = "",
        //                    VarName = $"do_shared[{index}]",
        //                    Area = "",
        //                    Address = "",
        //                    LinkValue = ""
        //                }
        //            };
        //            SignalsDO.Add(signal);
        //        }

        //        SelectedSignalDO = SignalsDO[0];
        //        _DataView.Source = SignalsDO;
        //        _DataView.View.Refresh();
        //        OnPropertyChanged(nameof(DataView));
        //        return;
        //    }
        //    #endregion

        //    #region Генерируем сигналы DO, созданные во вкладке Таблица сигналов

        //    #region Генерируем данные из ТБ
        //    var uso_list = TableSignalsViewModel.LayotRackViewModel.Params;
        //    foreach (var _USO in uso_list)
        //    {
        //        foreach (var _Rack in _USO.Racks)
        //        {
        //            foreach (var _Module in _Rack.Modules)
        //            {
        //                if (_Module.Type == TypeModule.DO)
        //                {
        //                    foreach (var _Channel in _Module.Channels)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(_Channel.Id) && (_Channel.Description != "Резерв" || _Channel.Description != "резерв"))
        //                        {
        //                            var signal = new SignalDO()
        //                            {
        //                                Signal = new BaseSignal
        //                                {
        //                                    Index = $"{++index}",
        //                                    Id = _Channel.Id,
        //                                    Description = _Channel.Description,
        //                                    VarName = $"do_shared[{index}]",
        //                                    Area = "",
        //                                    Address = $"{int.Parse(_Channel.Address) - 200000}",
        //                                }
        //                            };
        //                            SignalsDO.Add(signal);
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Дозаполняем таблицу
        //    while (SignalsDO.Count < 2500)
        //    {
        //        var signal = new SignalDO()
        //        {
        //            Signal = new BaseSignal
        //            {
        //                Index = $"{++index}",
        //                Id = "",
        //                Description = "",
        //                VarName = $"do_shared[{index}]",
        //                Area = "",
        //                Address = "",
        //                LinkValue = ""
        //            }
        //        };
        //        SignalsDO.Add(signal);
        //    }
        //    #endregion

        //    #endregion

        //    SelectedSignalDO = SignalsDO[0];
        //    _DataView.Source = SignalsDO;
        //    _DataView.View.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion 

        //#region Генерируем данные
        //public void GeneratedData()
        //{
        //    _DataView.Source = SignalsDO;
        //    _DataView.View?.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion

        #endregion
    }
}
