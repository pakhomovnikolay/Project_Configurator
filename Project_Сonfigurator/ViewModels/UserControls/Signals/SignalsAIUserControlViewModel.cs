using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
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
    public class SignalsAIUserControlViewModel : ViewModelUserControls
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
                    //if (SelectedSignalAI is not null)
                    //    _SignalService.RedefineSignal(SelectedSignalAI.Signal, _IsSelected, Title);
                    //DoSelection = _SignalService.DoSelection;
                    //if (_IsSelected)
                    //    _DataView.View?.Refresh();
                }
            }
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
            set => Set(ref _Params, value);
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
        private bool CanCmdChangeAddressSignalExecute(object p) => /*SelectedSignalAI is not null*/true;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedSignalAI is null) return;

            //var data_list = new ObservableCollection<SignalAI>();
            //foreach (SignalAI SignalAI in DataView)
            //{
            //    data_list.Add(SignalAI);
            //}

            //if (Index != SelectedSignalAI.Signal.Index)
            //    SelectedSignalAI = data_list[int.Parse(Index) - 1];

            //_SignalService.DoSelection = true;
            //_SignalService.ListName = Title;
            //_SignalService.Type = TypeModule.AI;

            //var NameListSelected = "";
            //if (string.IsNullOrWhiteSpace(SelectedSignalAI.Signal.Area) || int.Parse(SelectedSignalAI.Signal.Area) == 0)
            //    NameListSelected = "Таблица сигналов";
            //else if (int.Parse(SelectedSignalAI.Signal.Area) == 1)
            //    NameListSelected = "AI формируемые";

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
        private bool CanCmdSelectionSignalExecute(object p) => /*SelectedSignalAI is not null*/true;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedSignalAI is null) return;
            //if (App.FucusedTabControl == null) return;
            //if (!_SignalService.DoSelection) return;

            //var data_list = new ObservableCollection<SignalAI>();
            //foreach (SignalAI Signal in DataView)
            //{
            //    data_list.Add(Signal);
            //}

            //if (Index != SelectedSignalAI.Signal.Index)
            //    SelectedSignalAI = data_list[int.Parse(Index) - 1];

            //_SignalService.Address = SelectedSignalAI.Signal.Index;
            //_SignalService.Id = SelectedSignalAI.Signal.Id;
            //_SignalService.Description = SelectedSignalAI.Signal.Description;

            //foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
            //                         let _TabItem = _Item as TabItem
            //                         where _TabItem.Header.ToString() == _SignalService.ListName
            //                         select _TabItem)
            //    App.FucusedTabControl.SelectedItem = _TabItem;
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
            //if (p is not DataGrid _DataGrid) return;
            //_DataGrid.BeginEdit();
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
        //    if (e.Item is not SignalAI Signal || Signal is null) { e.Accepted = false; return; }
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
        //    SignalsAI = new();

        //    #region Генерируем сигналы AI, при отсутсвии данных во владке Таблица сигналов
        //    if (TableSignalsViewModel.LayotRackViewModel is null ||
        //        TableSignalsViewModel.LayotRackViewModel.Params is null ||
        //        TableSignalsViewModel.LayotRackViewModel.Params.Count < 0) 
        //    {
        //        while (SignalsAI.Count < 500)
        //        {
        //            var signal = new SignalAI()
        //            {
        //                Signal = new BaseSignal
        //                {
        //                    Index = $"{++index}",
        //                    Id = "",
        //                    Description = "",
        //                    VarName = $"ai_shared[{index}]",
        //                    Area = "",
        //                    Address = "",
        //                    LinkValue = ""
        //                }
        //            };
        //            SignalsAI.Add(signal);
        //        }

        //        SelectedSignalAI = SignalsAI[0];
        //        _DataView.Source = SignalsAI;
        //        _DataView.View.Refresh();
        //        OnPropertyChanged(nameof(DataView));
        //        return;
        //    }
        //    #endregion

        //    #region Генерируем сигналы AI, созданные во вкладке Таблица сигналов

        //    #region Генерируем данные из ТБ
        //    var uso_list = TableSignalsViewModel.LayotRackViewModel.Params;
        //    foreach (var _USO in uso_list)
        //    {
        //        foreach (var _Rack in _USO.Racks)
        //        {
        //            foreach (var _Module in _Rack.Modules)
        //            {
        //                if (_Module.Type == TypeModule.AI)
        //                {
        //                    foreach (var _Channel in _Module.Channels)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(_Channel.Id) && (_Channel.Description != "Резерв" || _Channel.Description != "резерв"))
        //                        {
        //                            var signal = new SignalAI()
        //                            {
        //                                Signal = new BaseSignal
        //                                {
        //                                    Index = $"{++index}",
        //                                    Id = _Channel.Id,
        //                                    Description = _Channel.Description,
        //                                    VarName = $"ai_shared[{index}]",
        //                                    Area = "",
        //                                    Address = $"{int.Parse(_Channel.Address)}",
        //                                }
        //                            };
        //                            SignalsAI.Add(signal);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Дозаполняем таблицу
        //    while (SignalsAI.Count < 500)
        //    {
        //        var signal = new SignalAI()
        //        {
        //            Signal = new BaseSignal
        //            {
        //                Index = $"{++index}",
        //                Id = "",
        //                Description = "",
        //                VarName = $"ai_shared[{index}]",
        //                Area = "",
        //                Address = "",
        //                LinkValue = ""
        //            }
        //        };
        //        SignalsAI.Add(signal);
        //    }
        //    #endregion

        //    #endregion

        //    SelectedSignalAI = SignalsAI[0];
        //    _DataView.Source = SignalsAI;
        //    _DataView.View.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion

        //#region Генерируем данные
        //public void GeneratedData()
        //{
        //    _DataView.Source = SignalsAI;
        //    _DataView.View?.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion

        #endregion
    }
}