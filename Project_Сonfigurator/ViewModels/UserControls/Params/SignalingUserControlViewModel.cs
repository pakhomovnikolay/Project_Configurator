using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class SignalingUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public SignalingUserControlViewModel()
        {
            Title = "Сигнализация";
            Description = "Сигнализация и общесистемная диагностика";
            UsingUserControl = new SignalingUserControl();
            _ParamsDataView.Filter += ParamsFiltered;
        }

        private readonly ISignalService SignalServices;
        public SignalingUserControlViewModel(ISignalService _ISignalService) : this()
        {
            SignalServices = _ISignalService;
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
                    if (DoSelection) DoSelection = SignalServices.RedefineAddress(SelectedParam.Param, _IsSelected, Title);
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

        #region Список параметров
        private ObservableCollection<BaseSignaling> _Params = new();
        /// <summary>
        /// Список параметров
        /// </summary>
        public ObservableCollection<BaseSignaling> Params
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

        #region Выбранный параметр
        private BaseSignaling _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseSignaling SelectedParam
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

            if (Index != SelectedParam.Param.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            DoSelection = true;
            var NameListSelected = SignalServices.ChangeAddress(Title, SelectedParam.Param.TypeSignal);
            if (UserDialog.SearchControlViewModel(NameListSelected) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #region Команда - Выбрать цвет сообщения
        private ICommand _CmdSelecteColor;
        /// <summary>
        /// Команда - Выбрать цвет сообщения
        /// </summary>
        public ICommand CmdSelecteColor => _CmdSelecteColor ??= new RelayCommand(OnCmdSelecteColorExecuted, CanCmdSelecteColorExecute);
        private bool CanCmdSelecteColorExecute(object p) => p is DataGrid;

        private void OnCmdSelecteColorExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #region Команда - Выбрать УСО
        private ICommand _CmdSelecteUSO;
        /// <summary>
        /// Команда - Выбрать УСО
        /// </summary>
        public ICommand CmdSelecteUSO => _CmdSelecteUSO ??= new RelayCommand(OnCmdSelecteUSOExecuted, CanCmdSelecteUSOExecute);
        private bool CanCmdSelecteUSOExecute(object p) => p is DataGrid;

        private void OnCmdSelecteUSOExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #region Команда - Импортировать служебные сигналы из ТБ
        private ICommand _CmdImportServiceSignal;
        /// <summary>
        /// Команда - Импортировать служебные сигналы из ТБ
        /// </summary>
        public ICommand CmdImportServiceSignal => _CmdImportServiceSignal ??= new RelayCommand(OnCmdImportServiceSignalExecuted, CanCmdImportServiceSignalExecute);
        private bool CanCmdImportServiceSignalExecute(object p) => true;

        private void OnCmdImportServiceSignalExecuted(object p)
        {
            #region Импорт сигналов из ТБ
            if (!UserDialog.SendMessage("Внимание!", "Перед продолжением убедитесь, что ТБ заполнена.\nВсе данные будут потеряны! Продолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            Params = new();
            if (UserDialog.SearchControlViewModel("Компоновка корзин") is not LayotRackUserControlViewModel _TabItem) return;
            LayotRackUserControlViewModel LayotRacksViewModel = _TabItem;

            if (UserDialog.SearchControlViewModel("Таблица сигналов") is not TableSignalsUserControlViewModel _ParTabItem) return;
            TableSignalsUserControlViewModel TableSignalsViewModel = _ParTabItem;

            if (TableSignalsViewModel.Params is null || TableSignalsViewModel.Params.Count <= 0)
                if (!UserDialog.SendMessage("Внимание!", "Пожалуйста, проверьте ТБ",
                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;


            CreateData();

            #region Создаем имена УСО без сигналов
            var USONameException = new ObservableCollection<string>();
            foreach (var _Param in LayotRacksViewModel.Params)
                if (_Param.Racks is null || _Param.Racks.Count <= 0)
                    USONameException.Add(_Param.Name);
            #endregion

            #region Переописываем сигналы диагностики
            var index = 0;
            bool flNeedShift;
            foreach (var _Param in TableSignalsViewModel.Params)
            {
                foreach (var _Rack in _Param.Racks)
                {
                    flNeedShift = false;
                    foreach (var _Module in _Rack.Modules)
                    {
                        if (_Module.Channels is null || _Module.Channels.Count <= 0) continue;
                        foreach (var _Channel in _Module.Channels)
                        {
                            if (_Channel.Id.Contains("CSC", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var flTmp = false;
                                foreach (var item in USONameException)
                                    if (_Channel.Description.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                                        flTmp = true;

                                if (!flTmp)
                                {
                                    flNeedShift = true;
                                    Params[index].Param.Id = _Channel.Id;
                                    Params[index].Param.Description = _Channel.Description;
                                    Params[index].IndexUSO = _Param.Index;
                                    _ = int.TryParse(_Channel.Address, out int address);
                                    if (address > 100000)
                                        Params[index].Param.Address = $"{address - 100000}";

                                    Params[index].Color = "Красный";
                                    if (_Channel.Description.Contains("Двер", StringComparison.CurrentCultureIgnoreCase))
                                        Params[index].Color = "Желтый";

                                    index++;
                                }
                            }
                        }
                    }
                    if (flNeedShift)
                        index += 5;
                }
            }
            #endregion

            #region Ищем имена УСО исключенй
            if (USONameException is not null && USONameException.Count > 0)
            {
                foreach (var _Name in USONameException)
                {
                    foreach (var _Param in TableSignalsViewModel.Params)
                    {
                        foreach (var _Rack in _Param.Racks)
                        {
                            flNeedShift = false;
                            foreach (var _Module in _Rack.Modules)
                            {
                                if (_Module.Channels is null || _Module.Channels.Count <= 0) continue;
                                foreach (var _Channel in _Module.Channels)
                                {
                                    if (_Channel.Id.Contains("CSC", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        if (_Channel.Description.Contains(_Name, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            flNeedShift = true;
                                            Params[index].Param.Id = _Channel.Id;
                                            Params[index].Param.Description = _Channel.Description;


                                            Params[index].Color = "Красный";
                                            if (_Channel.Description.Contains("Двер", StringComparison.CurrentCultureIgnoreCase))
                                                Params[index].Color = "Желтый";

                                            foreach (var _USO in LayotRacksViewModel.Params)
                                                if (_USO.Name == _Name)
                                                    Params[index].IndexUSO = _USO.Index;

                                            index++;
                                        }
                                    }
                                }
                            }
                            if (flNeedShift)
                                index += 5;
                        }
                    }
                }
            }
            #endregion

            RefreshDataView();

            #endregion
        }
        #endregion

        #endregion

        #region Функции

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public override ObservableCollection<T> GetParams<T>() => Params as ObservableCollection<T>;
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<BaseSignaling>;
        #endregion

        #region Фильтрация парметров
        /// <summary>
        /// Фильтрация парметров
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseSignaling _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Параметры
            if (_Param.Param.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                _Param.Param.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            while (Params.Count < 928)
            {
                #region Создаем данные параметра
                var param = new BaseParam
                {
                    Index = $"{Params.Count + 1}",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = $"list5_param[{Params.Count + 1}]"
                };
                #endregion

                #region Создаем параметр
                var _Param = new BaseSignaling
                {
                    Color = "",
                    IndexUSO = "",
                    TypeWarning = "1",
                    VarNameVU = $"LIST5[{Params.Count / 16 + 1}]",
                    Param = param
                };
                #endregion

                Params.Add(_Param);
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
