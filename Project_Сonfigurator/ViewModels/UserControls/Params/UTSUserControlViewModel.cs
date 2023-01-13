using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UTSUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UTSUserControlViewModel()
        {
            Title = "DO остальные";
            Description = "Ссылки отдельных дискретных выходов";
            UsingUserControl = new UTSUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private ISignalService _SignalService;
        private readonly IDBService _DBService;

        TableSignalsUserControlViewModel TableSignalsViewModel { get; }
        public UTSUserControlViewModel(
            ISignalService signalService,
            IUserDialogService userDialog,
            IDBService dBService,
            TableSignalsUserControlViewModel tableSignalsViewModel) : this()
        {
            UserDialog = userDialog;
            _SignalService = signalService;
            _DBService = dBService;
            TableSignalsViewModel = tableSignalsViewModel;
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
                            if (DoSelectionKCOParam)
                                _SignalService.RedefineParam(SelectedUTS.KCO, _IsSelected, Title);
                            else if (DoSelectionSignalSOD)
                                _SignalService.RedefineParam(SelectedUTS.SignalSOD, _IsSelected, Title);
                            else if (DoSelectionSignalErrSOD)
                                _SignalService.RedefineParam(SelectedUTS.SignalErrSOD, _IsSelected, Title);
                            else
                                _SignalService.RedefineParam(SelectedUTS.Param, _IsSelected, Title);


                            DoSelectionKCOParam = false;
                            DoSelectionSignalSOD = false;
                            DoSelectionSignalErrSOD = false;
                            DoSelection = false;
                            _SignalService.ResetSignal();
                            _DataView.View?.Refresh();
                        }
                        else if (_SignalService.DoSelection && string.IsNullOrWhiteSpace(_SignalService.Address) && _SignalService.ListName == Title)
                        {
                            _SignalService.ResetSignal();
                            DoSelectionKCOParam = false;
                            DoSelectionSignalSOD = false;
                            DoSelectionSignalErrSOD = false;
                            DoSelection = false;
                        }
                    }
                    else if (_SignalService.DoSelection && string.IsNullOrWhiteSpace(_SignalService.Address) && _SignalService.ListName != Title)
                    {
                        _SignalService.ResetSignal();
                        DoSelectionKCOParam = false;
                        DoSelectionSignalSOD = false;
                        DoSelectionSignalErrSOD = false;
                        DoSelection = false;
                    }
                }
            }
        }
        #endregion

        #region Список табло и сирен
        private ObservableCollection<BaseUTS> _UTS = new();
        /// <summary>
        /// Список табло и сирен
        /// </summary>
        public ObservableCollection<BaseUTS> UTS
        {
            get => _UTS;
            set => Set(ref _UTS, value);
        }
        #endregion

        #region Коллекция табло и сирен
        /// <summary>
        /// Коллекция табло и сирен
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный параметр табло и сирены
        private BaseUTS _SelectedUTS = new();
        /// <summary>
        /// Выбранный параметр табло и сирены
        /// </summary>
        public BaseUTS SelectedUTS
        {
            get => _SelectedUTS;
            set => Set(ref _SelectedUTS, value);
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

        #region Состояние необходимости выбора параметра контроля целостности цепей
        private bool _DoSelectionKCOParam;
        /// <summary>
        /// Состояние необходимости выбора параметра контроля целостности цепей
        /// </summary>
        public bool DoSelectionKCOParam
        {
            get => _DoSelectionKCOParam;
            set => Set(ref _DoSelectionKCOParam, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра датчика прохождения СОД
        private bool _DoSelectionSignalSOD;
        /// <summary>
        /// Состояние необходимости выбора параметра датчика прохождения СОД
        /// </summary>
        public bool DoSelectionSignalSOD
        {
            get => _DoSelectionSignalSOD;
            set => Set(ref _DoSelectionSignalSOD, value);
        }
        #endregion

        #region Состояние необходимости выбора параметра датчика прохождения СОД (неисправность)
        private bool _DoSelectionSignalErrSOD;
        /// <summary>
        /// Состояние необходимости выбора параметра датчика прохождения СОД (неисправность)
        /// </summary>
        public bool DoSelectionSignalErrSOD
        {
            get => _DoSelectionSignalErrSOD;
            set => Set(ref _DoSelectionSignalErrSOD, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Сменить адрес
        private ICommand _CmdChangeAddress;
        /// <summary>
        /// Команда - Сменить адрес
        /// </summary>
        public ICommand CmdChangeAddress => _CmdChangeAddress ??= new RelayCommand(OnCmdChangeAddressExecuted, CanCmdChangeAddressExecute);
        private bool CanCmdChangeAddressExecute(object p) => SelectedUTS is not null;

        private void OnCmdChangeAddressExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedUTS is null) return;

            if (Index != SelectedUTS.Param.Index)
                SelectedUTS = UTS[int.Parse(Index) - 1];

            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.DO;
            var NameListSelected = "Сигналы DO";

            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра контроля целостности цепей
        private ICommand _CmdChangeAddressKCO;
        /// <summary>
        /// Команда - Сменить адрес параметра контроля целостности цепей
        /// </summary>
        public ICommand CmdChangeAddressKCO => _CmdChangeAddressKCO ??= new RelayCommand(OnCmdChangeAddressKCOExecuted, CanCmdChangeAddressKCOExecute);
        private bool CanCmdChangeAddressKCOExecute(object p) => SelectedUTS is not null;

        private void OnCmdChangeAddressKCOExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedUTS is null) return;

            if (Index != SelectedUTS.Param.Index)
                SelectedUTS = UTS[int.Parse(Index) - 1];

            DoSelectionKCOParam = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.AI;
            var NameListSelected = "Сигналы AI";

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра датчика прохождения СОД
        private ICommand _CmdChangeAddressSignalSOD;
        /// <summary>
        /// Команда - Сменить адрес параметра датчика прохождения СОД
        /// </summary>
        public ICommand CmdChangeAddressSignalSOD => _CmdChangeAddressSignalSOD ??= new RelayCommand(OnCmdChangeAddressSignalSODExecuted, CanCmdChangeAddressSignalSODExecute);
        private bool CanCmdChangeAddressSignalSODExecute(object p) => SelectedUTS is not null;

        private void OnCmdChangeAddressSignalSODExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedUTS is null) return;

            if (Index != SelectedUTS.Param.Index)
                SelectedUTS = UTS[int.Parse(Index) - 1];

            DoSelectionSignalSOD = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedUTS.SignalSOD.TypeSignal) || int.Parse(SelectedUTS.SignalSOD.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                _SignalService.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedUTS.SignalSOD.TypeSignal) > 1)
            {
                NameListSelected = "Сигналы AI";
                _SignalService.Type = TypeModule.AI;
            }
            else if (int.Parse(SelectedUTS.SignalSOD.TypeSignal) > 0)
            {
                NameListSelected = "Группы сигналов";
                _SignalService.Type = TypeModule.DI;
            }


            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Сменить адрес параметра датчика прохождения СОД (неисправность)
        private ICommand _CmdChangeAddressSignalErrSOD;
        /// <summary>
        /// Команда - Сменить адрес параметра датчика прохождения СОД (неисправность)
        /// </summary>
        public ICommand CmdChangeAddressSignalErrSOD => _CmdChangeAddressSignalErrSOD ??= new RelayCommand(OnCmdChangeAddressSignalErrSODExecuted, CanCmdChangeAddressSignalErrSODExecute);
        private bool CanCmdChangeAddressSignalErrSODExecute(object p) => SelectedUTS is not null;

        private void OnCmdChangeAddressSignalErrSODExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedUTS is null) return;

            if (Index != SelectedUTS.Param.Index)
                SelectedUTS = UTS[int.Parse(Index) - 1];

            DoSelectionSignalErrSOD = true;
            _SignalService.DoSelection = true;
            _SignalService.ListName = Title;
            _SignalService.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedUTS.SignalErrSOD.TypeSignal) || int.Parse(SelectedUTS.SignalErrSOD.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                _SignalService.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedUTS.SignalErrSOD.TypeSignal) > 1)
            {
                NameListSelected = "Сигналы AI";
                _SignalService.Type = TypeModule.AI;
            }
            else if (int.Parse(SelectedUTS.SignalErrSOD.TypeSignal) > 0)
            {
                NameListSelected = "Группы сигналов";
                _SignalService.Type = TypeModule.DI;
            }


            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Генерация сигналов
        public void GeneratedData()
        {
            _DataView.Source = UTS;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));

            if (UTS is null || UTS.Count <= 0)
                SelectedUTS = null;
        }
        #endregion 

        #endregion
    }
}
