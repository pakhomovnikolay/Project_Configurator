using ClosedXML.Excel;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class TableSignalsUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public TableSignalsUserControlViewModel()
        {
            Title = "Таблица сигналов";
            Description = $" Таблица распределения сигналов по модулям и шкафам {App.Settings.Config.NameProject}";
            UsingUserControl = new TableSignalsUserControl();
        }

        private readonly IUserDialogService UserDialog;
        private readonly ISignalService SignalServices;
        public TableSignalsUserControlViewModel(IUserDialogService _UserDialog, ISignalService _ISignalService) : this()
        {
            UserDialog = _UserDialog;
            SignalServices = _ISignalService;

            _ParamsDataView.Filter += ParamsFiltered;
            _SubParamsDataView.Filter += SubParamsFiltered;
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
                    if (string.IsNullOrWhiteSpace(SignalServices.Address) && !SignalServices.DoSelection)
                        SignalServices.ResetSignal();

                    DoSelection = SignalServices.DoSelection;
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

        #region Список УСО
        private ObservableCollection<USO> _Params = new();
        /// <summary>
        /// Список УСО
        /// </summary>
        public ObservableCollection<USO> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (Params is not null && _Params.Count > 0)
                    {
                        SelectedParam = _Params[0];
                        RefreshDataView();
                    }
                    else
                    {
                        SubParams = null;
                        SelectedParam = null;
                        RefreshDataView();
                    }
                }
            }
        }
        #endregion

        #region Выбранное УСО
        private USO _SelectedParam = new();
        /// <summary>
        /// Выбранное УСО
        /// </summary>
        public USO SelectedParam
        {
            get => _SelectedParam;
            set
            {
                if (Set(ref _SelectedParam, value))
                {
                    if (_SelectedParam is null) return;
                    GeneratedDataView();
                    RefreshSubDataView();
                }
            }
        }
        #endregion

        #region Список модулей
        private ObservableCollection<RackModule> _SubParams = new();
        /// <summary>
        /// Список модулей
        /// </summary>
        public ObservableCollection<RackModule> SubParams
        {
            get => _SubParams;
            set => Set(ref _SubParams, value);
        }
        #endregion

        #region Выбранный модуль
        /// <summary>
        /// Выбранный модуль
        /// </summary>
        private RackModule _SelectedSubParam = new();
        public RackModule SelectedSubParam
        {
            get => _SelectedSubParam;
            set => Set(ref _SelectedSubParam, value);
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

        #region Путь для импорта ТБ
        private string _PathImport;
        /// <summary>
        /// Путь для импорта ТБ
        /// </summary>
        public string PathImport
        {
            get => _PathImport;
            set => Set(ref _PathImport, value);
        }
        #endregion

        #region Коллекция УСО для отображения
        /// <summary>
        /// Коллекция УСО для отображения
        /// </summary>
        private readonly CollectionViewSource _ParamsDataView = new();
        public ICollectionView ParamsDataView => _ParamsDataView?.View;
        #endregion

        #region Коллекция модулей для отображения
        /// <summary>
        /// Коллекция модулей для отображения
        /// </summary>
        private readonly CollectionViewSource _SubParamsDataView = new();
        public ICollectionView SubParamsDataView => _SubParamsDataView?.View;
        #endregion

        #endregion

        #region Команды

        #region Команда - Сгенерировать таблицу
        /// <summary>
        /// Команда - Сгенерировать таблицу
        /// </summary>
        private ICommand _CmdGenerateTable;
        public ICommand CmdGenerateTable => _CmdGenerateTable ??= new RelayCommand(OnCmdGenerateTableExecuted, CanCmdGenerateTableExecute);
        private bool CanCmdGenerateTableExecute() => true;
        private void OnCmdGenerateTableExecuted()
        {
            if (UserDialog.SearchControlViewModel("Компоновка корзин") is not LayotRackUserControlViewModel LayotRackViewModel) return;
            if (LayotRackViewModel is null) return;
            if (LayotRackViewModel.Params is null) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            GenerateTable(LayotRackViewModel);
            if (Params.Count > 0) SelectedParam = Params[0];
        }
        #endregion

        #region Команда - Отфильтровать каналы
        /// <summary>
        /// Команда - Отфильтровать каналы
        /// </summary>
        private ICommand _CmdFilteringChannels;
        public ICommand CmdFilteringChannels => _CmdFilteringChannels ??= new RelayCommand(OnCmdFilteringChannelsExecuted);
        private void OnCmdFilteringChannelsExecuted()
        {
            RefreshDataView();
        }
        #endregion

        #region Команда - Выбрать путь для импорта ТБ
        /// <summary>
        /// Команда - Выбрать путь для импорта ТБ
        /// </summary>
        private ICommand _CmdSelectedPathImport;
        public ICommand CmdSelectedPathImport => _CmdSelectedPathImport ??= new RelayCommand(OnCmdSelectedPathImportExecuted);
        private void OnCmdSelectedPathImportExecuted()
        {
            if (UserDialog.SelectFile(Title, out string path, PathImport))
                PathImport = path;
        }
        #endregion

        #region Команда - Импортировать ТБ
        /// <summary>
        /// Команда - Импортировать ТБ
        /// </summary>
        private ICommand _CmdImportTB;
        public ICommand CmdImportTB => _CmdImportTB ??= new RelayCommand(OnCmdImportTBExecuted, CanCmdImportTBExecute);
        private bool CanCmdImportTBExecute(object p) => !string.IsNullOrWhiteSpace(PathImport);
        private void OnCmdImportTBExecuted(object p)
        {
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid is null) return;

            ImportTB(MyDataGrid);
        }
        #endregion

        #region Команда - Выбрать сигнал
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        private ICommand _CmdSelectionSignal;
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SignalServices.DoSelection;
        private void OnCmdSelectionSignalExecuted(object p)
        {
            var index = int.Parse((string)p);
            if (App.FucusedTabControl == null) return;

            #region Проверка корректного выбора сигнала
            if (!SignalServices.CheckCurrentSelectedSignal(index, out string message))
            {
                if (UserDialog.SendMessage("Выбор сигнала", message, MessageBoxButton.YesNo, ResultType: MessageBoxResult.Yes)) return;
                else { DoSelection = SignalServices.ResetSignal(); return; }
            }
            #endregion

            #region Возврат на вкладку измененного сигнала
            SignalServices.SelecteAddress(SelectedSubParam.Channels, index);
            if (UserDialog.SearchControlViewModel(SignalServices.FromName) is not IViewModelUserControls _TabItem) return;
            App.FucusedTabControl.SelectedItem = _TabItem;
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
        public override object GetParam()
        {
            return Params;
        }
        #endregion

        #region Фильтрация УСО
        /// <summary>
        /// Фильтрация УСО
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not USO _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Фильтруем УСО
            foreach (var Channel in _Param.Racks)
            {
                foreach (var _Rack in _Param.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        foreach (var _Channel in _Module.Channels)
                        {
                            if (_Channel.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                                _Channel.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;
                        }
                    }
                }
            }
            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        public void SubParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not RackModule _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Фильтруем модулей
            foreach (var _Channel in _Param.Channels)
            {
                if (_Channel.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                        _Channel.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;
            }
            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Обновляем данные для отображения
        /// <summary>
        /// Обновляем данные для отображения
        /// </summary>
        private void RefreshDataView()
        {
            _SubParamsDataView.Source = SubParams;
            _SubParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(SubParamsDataView));

            _ParamsDataView.Source = Params;
            _ParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(ParamsDataView));
        }
        #endregion

        #region Обновляем данные модулей для отображения
        /// <summary>
        /// Обновляем данные модулей для отображения
        /// </summary>
        private void RefreshSubDataView()
        {
            _SubParamsDataView.Source = SubParams;
            _SubParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(SubParamsDataView));
        }
        #endregion

        #region Генерируем таблицу сигналов
        /// <summary>
        /// Генерируем таблицу сигналов
        /// </summary>
        /// <param name="_LayotRackViewModel"></param>
        private void GenerateTable(LayotRackUserControlViewModel _LayotRackViewModel)
        {
            Params = new();
            foreach (var _USO in _LayotRackViewModel.Params)
            {
                var need_add_uso = false;
                foreach (var _Rack in _USO.Racks)
                {
                    foreach (var _Module in _Rack.Modules)
                    {
                        switch (_Module.Type)
                        {
                            case TypeModule.AI:
                            case TypeModule.DI:
                            case TypeModule.AO:
                            case TypeModule.DO:
                            case TypeModule.DA:
                                if (_Module.Channels is null || _Module.Channels.Count <= 0)
                                    if (UserDialog.SendMessage("Внимание!", "Неверные данные таблицы сигналов. Проверьте вкладку",
                                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)) return;

                                foreach (var Channel in _Module.Channels)
                                {
                                    Channel.Description = "";
                                    Channel.Id = "";
                                }
                                need_add_uso = true;
                                break;
                        }
                    }
                }

                if (need_add_uso)
                    Params.Add(_USO);
            }
        }
        #endregion

        #region Импорт сигналов из ТБ
        /// <summary>
        /// Импорт сигналов из ТБ
        /// </summary>
        /// <param name="MyDataGrid"></param>
        private void ImportTB(DataGrid MyDataGrid)
        {
            try
            {
                using var work_book = new XLWorkbook(PathImport);
                var worksheet = work_book.Worksheets.Worksheet(1);

                var StartIndexRow = int.Parse(App.Settings.Config.Import.StartIndexRow);
                var IndexColumnId = int.Parse(App.Settings.Config.Import.IndexColumnId);
                var IndexColumnDescription = int.Parse(App.Settings.Config.Import.IndexColumnDescription);
                var IndexColumnRack = int.Parse(App.Settings.Config.Import.IndexColumnRack);
                var IndexColumnModule = int.Parse(App.Settings.Config.Import.IndexColumnModule);

                var Id = new ObservableCollection<string>();
                var Description = new ObservableCollection<string>();

                #region Формируем листы Идентификаторов и Наименования параметров
                while (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, IndexColumnRack).Value.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, IndexColumnModule).Value.ToString()))
                    {
                        if (!worksheet.Cell(StartIndexRow, IndexColumnId).Value.ToString().Contains(SelectedParam.Name, StringComparison.CurrentCultureIgnoreCase))
                            Id.Add(worksheet.Cell(StartIndexRow, IndexColumnId).Value.ToString());
                        else
                            Id.Add("");

                        Description.Add(worksheet.Cell(StartIndexRow, IndexColumnDescription).Value.ToString());
                    }
                    StartIndexRow++;
                }
                #endregion

                #region Переописываем каналы модулей
                var jSh = 0;
                if (Id.Count > 0 && Description.Count > 0)
                {
                    foreach (var _Rack in SelectedParam.Racks)
                    {
                        foreach (var _Module in _Rack.Modules)
                        {
                            foreach (var _Channel in _Module.Channels)
                            {
                                _Channel.Id = Id[jSh];
                                _Channel.Description = Description[jSh++];
                            }
                        }
                    }
                }
                #endregion

                MyDataGrid.Items.Refresh();
            }
            catch (Exception)
            {
                var desc = "Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта";
                UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
            }
        }
        #endregion

        #region Генерируем модули, для выбранного УСО
        /// <summary>
        /// Генерируем модули, для выбранного УСО
        /// </summary>
        private void GeneratedDataView()
        {
            SubParams = new();
            SelectedSubParam = new();
            foreach (var Rack in _SelectedParam.Racks)
            {
                if (!Rack.IsEnable) continue;
                foreach (var Module in Rack.Modules)
                {
                    if (string.IsNullOrWhiteSpace(Module.Name)) continue;
                    if (Module.Type != TypeModule.AI &&
                        Module.Type != TypeModule.DI &&
                        Module.Type != TypeModule.AO &&
                        Module.Type != TypeModule.DO &&
                        Module.Type != TypeModule.DA) continue;

                    SubParams.Add(Module);
                    SubParams[^1].Name = $"{Module.Index} {Module.Name.Replace(Module.Index, "").Trim()}";
                    SelectedSubParam = SubParams[0];
                }
            }
        }
        #endregion

        #endregion
    }
}
