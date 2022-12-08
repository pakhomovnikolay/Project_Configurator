using ClosedXML.Excel;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class TableSignalsUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private ISignalService _SignalService;
        public LayotRackUserControlViewModel LayotRackViewModel { get; }

        public TableSignalsUserControlViewModel(
            IUserDialogService userDialog,
            ISignalService signalService,
            LayotRackUserControlViewModel layotRackViewModel)
        {
            UserDialog = userDialog;
            _SignalService = signalService;
            _DataViewModules.Filter += OnModulesFiltered;
            _DataView.Filter += OnUSOListFiltered;


            LayotRackViewModel = layotRackViewModel;

            if (Program.Settings.AppData is not null && Program.Settings.AppData.USOList.Count > 0)
                GeneratedData();

        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Таблица сигналов";
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
        private string _Description = "Таблица сигналов НПС-1 \"Сызрань\"";
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
                    DoSelection = _SignalService.DoSelection;
                    if (!_IsSelected && string.IsNullOrWhiteSpace(_SignalService.Address))
                    {
                        DoSelection = false;
                        _SignalService.ResetSignal();
                    }
                }
            }
        }
        #endregion

        #region Коллекция УСО для отображения
        /// <summary>
        /// Коллекция УСО для отображения
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранное УСО
        private USO _SelectedUSO = new();
        /// <summary>
        /// Выбранное УСО
        /// </summary>
        public USO SelectedUSO
        {
            get => _SelectedUSO;
            set
            {
                if (Set(ref _SelectedUSO, value))
                {
                    if (_SelectedUSO is null) return;
                    if (_SelectedUSO.Racks is null) return;
                    if (_SelectedUSO.Racks.Count <= 0) return;

                    var modules = new List<RackModule>();
                    foreach (var Rack in value?.Racks)
                    {
                        foreach (var Module in Rack.Modules)
                        {
                            switch (Module.Type)
                            {
                                case TypeModule.AI:
                                case TypeModule.DI:
                                case TypeModule.AO:
                                case TypeModule.DO:
                                case TypeModule.DA:
                                    var module = new RackModule()
                                    {
                                        Index = Module.Index,
                                        Name = Module.Name,
                                        Type = Module.Type,
                                        Channels = Module.Channels,
                                        EndAddress = Module.EndAddress,
                                        StartAddress = Module.StartAddress
                                    };
                                    modules.Add(module);

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    _DataViewModules.Source = modules;
                    OnPropertyChanged(nameof(DataViewModules));
                }
            }
        }
        #endregion

        #region Коллекция модулей для отображения
        /// <summary>
        /// Коллекция модулей для отображения
        /// </summary>
        private readonly CollectionViewSource _DataViewModules = new();
        public ICollectionView DataViewModules => _DataViewModules?.View;
        #endregion

        #region Выбранный модуль
        /// <summary>
        /// Выбранный модуль
        /// </summary>
        private RackModule _SelectedModule = new();
        public RackModule SelectedModule
        {
            get => _SelectedModule;
            set => Set(ref _SelectedModule, value);
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

        #region Команда - Сгенерировать таблицу
        /// <summary>
        /// Команда - Сгенерировать таблицу
        /// </summary>
        private ICommand _CmdGenerateTable;
        public ICommand CmdGenerateTable => _CmdGenerateTable ??= new RelayCommand(OnCmdGenerateTableExecuted, CanCmdGenerateTableExecute);
        private bool CanCmdGenerateTableExecute() =>
            LayotRackViewModel is not null &&
            LayotRackViewModel.USOList is not null &&
            LayotRackViewModel.USOList.Count > 0;
        private void OnCmdGenerateTableExecuted()
        {
            if (LayotRackViewModel is null) return;
            if (LayotRackViewModel.USOList is null) return;
            if (!UserDialog.SendMessage("Внимание!", "Все данные по сигналам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            SelectedUSO = new USO();
            var uso_list = new List<USO>();
            foreach (var _USO in LayotRackViewModel.USOList)
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
                    uso_list.Add(_USO);
            }

            if (uso_list.Count <= 0)
                _DataView.Source = uso_list;
            else
            {
                SelectedUSO = uso_list[0];
                _DataView.Source = uso_list;
                _DataView.View?.Refresh();
            }
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
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
            if (_DataView.Source is null) return;
            if (_DataViewModules.Source is null) return;
            _DataView.View.Refresh();
            _DataViewModules.View.Refresh();
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
        private bool CanCmdImportTBExecute() => !string.IsNullOrWhiteSpace(PathImport);
        private void OnCmdImportTBExecuted()
        {
            try
            {
                using var work_book = new XLWorkbook(PathImport);
                var worksheet = work_book.Worksheets.Worksheet(1);

                var StartIndexRow = int.Parse(Program.Settings.Config.Import.StartIndexRow);
                var IndexColumnId = int.Parse(Program.Settings.Config.Import.IndexColumnId);
                var IndexColumnDescription = int.Parse(Program.Settings.Config.Import.IndexColumnDescription);
                var IndexColumnRack = int.Parse(Program.Settings.Config.Import.IndexColumnRack);
                var IndexColumnModule = int.Parse(Program.Settings.Config.Import.IndexColumnModule);

                var Id = new List<string>();
                var Description = new List<string>();

                #region Формируем листы Идентификаторов и Наименования параметров
                while (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, IndexColumnRack).Value.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, IndexColumnModule).Value.ToString()))
                    {
                        if (!worksheet.Cell(StartIndexRow, IndexColumnId).Value.ToString().Contains(SelectedUSO.Name, StringComparison.CurrentCultureIgnoreCase))
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
                    foreach (var _Rack in SelectedUSO.Racks)
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

                _DataViewModules.View?.Refresh();
                OnPropertyChanged(nameof(DataViewModules));
            }
            catch (Exception)
            {
                var desc = "Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу и настройки импорта";
                UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
            }
        }
        #endregion

        #region Команда - Выбрать сигнал
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        private ICommand _CmdSelectionSignal;
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => _SignalService.DoSelection;
        private void OnCmdSelectionSignalExecuted(object p)
        {
            var index = int.Parse((string)p);
            var message = "";

            #region Проверка корректного выбора сигнала
            switch (_SignalService.Type)
            {
                case TypeModule.AI:
                    if (index >= 100000)
                    {
                        message =
                            "Выбор неверный!\n" +
                            "Вы выбрали не аналоговый входной сигнал.\n" +
                            "Запрашивается ссылка на аналоговый входной сигнал (0-99999)" +
                            "Повторить выбор?";
                    }
                    break;
                case TypeModule.DI:
                    if (index < 100000 || index >= 200000)
                    {
                        message =
                            "Выбор неверный!\n" +
                            "Вы выбрали не дискретный входной сигнал.\n" +
                            "Запрашивается ссылка на дискретный входной сигнал (100000-199999)" +
                            "Повторить выбор?";
                    }
                    break;
                case TypeModule.AO:
                    if (index < 300000 || index >= 400000)
                    {
                        message =
                            "Выбор неверный!\n" +
                            "Вы выбрали не аналоговый выходной сигнал.\n" +
                            "Запрашивается ссылка на аналоговый выходной сигнал (300000-399999)" +
                            "Повторить выбор?";
                    }
                    break;
                case TypeModule.DO:
                    if (index < 200000 || index >= 300000)
                    {
                        message =
                            "Выбор неверный!\n" +
                            "Вы выбрали не дискретный выходной сигнал.\n" +
                            "Запрашивается ссылка на аналоговый входной сигнал (200000-299999)" +
                            "Повторить выбор?";
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region Оповещение пользователя о некореектном выборе сигнала
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (UserDialog.SendMessage("Выбор сигнала", message, MessageBoxButton.YesNo, ResultType: MessageBoxResult.Yes))
                    return;
                else
                {
                    _SignalService.ResetSignal();
                    DoSelection = _SignalService.DoSelection;
                }
            }
            #endregion

            #region Возврат на вкладку измененногоо сигнала
            else
            {
                foreach (var _Channel in SelectedModule.Channels)
                {
                    if (_Channel.Address == index.ToString())
                    {
                        _SignalService.Id = _Channel.Id;
                        _SignalService.Description = _Channel.Description;

                        switch (_SignalService.Type)
                        {
                            case TypeModule.AI:
                                _SignalService.Address = $"{index}";
                                break;
                            case TypeModule.DI:
                                _SignalService.Address = $"{index - 100000}";
                                break;
                            case TypeModule.AO:
                                _SignalService.Address = $"{index - 300000}";
                                break;
                            case TypeModule.DO:
                                _SignalService.Address = $"{index - 200000}";
                                break;
                            default:
                                break;
                        }

                        break;
                    }
                }
                if (App.FucusedTabControl == null) return;
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
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        public void OnModulesFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not RackModule _Module || _Module is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            foreach (var Channel in _Module.Channels)
            {
                if (Channel.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Channel.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }
            }
            e.Accepted = false;
        }
        #endregion

        #region Фильтрация УСО
        /// <summary>
        /// Фильтрация УСО
        /// </summary>
        public void OnUSOListFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not USO _USO || _USO is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter))
            {
                var _USOList = (List<USO>)_DataView.Source;
                SelectedUSO = _USOList[0];
                return;
            }
            #endregion

            #region Фильтр УСО
            foreach (var _Rack in _USO.Racks)
            {
                foreach (var _Module in _Rack.Modules)
                {
                    foreach (var _Channel in _Module.Channels)
                    {
                        if (_Channel.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                            _Channel.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase))
                        {
                            SelectedUSO = _USO;
                            return;
                        }
                    }
                }
            }
            #endregion

            e.Accepted = false;
        }
        #endregion

        #region Генерируем данные
        public void GeneratedData()
        {
            SelectedUSO = new USO();
            var uso_list = new List<USO>();
            foreach (var _USO in LayotRackViewModel.USOList)
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
                                need_add_uso = true;
                                break;
                        }
                    }
                }
                if (need_add_uso)
                    uso_list.Add(_USO);
            }

            if (uso_list.Count <= 0)
                _DataView.Source = uso_list;
            else
            {
                SelectedUSO = uso_list[0];
                _DataView.Source = uso_list;
                _DataView.View?.Refresh();
            }
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion

        #endregion
    }
}
