using ClosedXML.Excel;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.Interfaces;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class MessageWindowViewModel : ViewModel
    {
        #region Конструктор
        public MessageWindowViewModel()
        {
            Title = "Сообщения";
            WindowWindowState = WindowState.Maximized;
            _SubParamsDataView.Filter += ParamsFiltered;
        }
        #endregion

        #region Параметры

        #region Список сообщений
        private ObservableCollection<CollectionMessage> _Params = new();
        /// <summary>
        /// Список сообщений
        /// </summary>
        public ObservableCollection<CollectionMessage> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (_Params is null && _Params.Count <= 0) return;
                    SelectedTabIndex = 0;
                }
            }
        }
        #endregion

        #region Выбранное сообщение
        private BaseMessage _SelectedSubParam;
        /// <summary>
        /// Выбранное сообщение
        /// </summary>
        public BaseMessage SelectedSubParam
        {
            get => _SelectedSubParam;
            set => Set(ref _SelectedSubParam, value);
        }
        #endregion

        #region Выбранная вкладка
        private CollectionMessage _SelectedParam;
        /// <summary>
        /// Выбранная вкладка
        /// </summary>
        public CollectionMessage SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
        }
        #endregion

        #region Индекс выбранной вкладки
        private int _SelectedTabIndex = -1;
        /// <summary>
        /// Индекс выбранной вкладки
        /// </summary>
        public int SelectedTabIndex
        {
            get => _SelectedTabIndex;
            set
            {
                if (Set(ref _SelectedTabIndex, value))
                {
                    SelectedParam = null;
                    if (_SelectedTabIndex < 0) return;
                    if (Params is null || Params.Count <= 0) return;

                    SelectedParam = Params[_SelectedTabIndex];
                    SelectedSubParam = SelectedParam.Messages[0];
                    RefreshDataView();
                }
            }
        }
        #endregion

        #region Состояние ToggleButton
        private bool _ToggleButtonIsChecked;
        /// <summary>
        /// Состояние ToggleButton
        /// </summary>
        public bool ToggleButtonIsChecked
        {
            get => _ToggleButtonIsChecked;
            set => Set(ref _ToggleButtonIsChecked, value);
        }
        #endregion

        #region Выбранный путь импорта сообщений
        private string _SelectedPathImport;
        /// <summary>
        /// Выбранный путь импорта сообщений
        /// </summary>
        public string SelectedPathImport
        {
            get => _SelectedPathImport;
            set => Set(ref _SelectedPathImport, value);
        }
        #endregion

        #region Список вкладок для экспорта
        private string _ListTableImport;
        /// <summary>
        /// Список вкладок для экспорта
        /// </summary>
        public string ListTableImport
        {
            get => _ListTableImport;
            set => Set(ref _ListTableImport, value);
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

        #region Команда - Добавить коллекция сообщений
        private ICommand _CmdCreateCollectionMessages;
        /// <summary>
        /// Команда - Добавить коллекция сообщений
        /// </summary>
        public ICommand CmdCreateCollectionMessages => _CmdCreateCollectionMessages ??= new RelayCommand(OnCmdCreateCollectionMessagesExecuted);
        private void OnCmdCreateCollectionMessagesExecuted(object p)
        {
            CreateCollectionMessages(Params);
            SelectedTabIndex = Params.Count - 1;
        }
        #endregion

        #region Команда - Удалить коллекция сообщений
        private ICommand _CmdDeleteCollectionMessages;
        /// <summary>
        /// Команда - Удалить коллекция сообщений
        /// </summary>
        public ICommand CmdDeleteCollectionMessages => _CmdDeleteCollectionMessages ??= new RelayCommand(OnCmdDeleteCollectionMessagesExecuted, CanCmdDeleteCollectionMessagesExecute);
        private bool CanCmdDeleteCollectionMessagesExecute() => SelectedTabIndex >= 0;
        private void OnCmdDeleteCollectionMessagesExecuted()
        {
            Params.RemoveAt(SelectedTabIndex);
        }
        #endregion

        #region Команда - Выбрать путь для воспроизведения звука
        private ICommand _CmdSelectionPathSound;
        /// <summary>
        /// Команда - Выбрать путь для воспроизведения звука
        /// </summary>
        public ICommand CmdSelectionPathSound => _CmdSelectionPathSound ??= new RelayCommand(OnCmdSelectionPathSoundExecuted, CanCmdSelectionPathSoundExecute);
        private bool CanCmdSelectionPathSoundExecute(object p) => p is DataGrid;
        private void OnCmdSelectionPathSoundExecuted(object p)
        {
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid is null) return;

            if (!UserDialog.SelectFile("Выбрать файл звука", out string select_file)) return;
            SelectedSubParam.PathSound = select_file;

            MyDataGrid.Items.Refresh();
        }
        #endregion

        #region Команда - Выбрать вкладку
        private ICommand _CmdSelectedTabPanelItem;
        /// <summary>
        /// Команда - Выбрать вкладку
        /// </summary>
        public ICommand CmdSelectedTabPanelItem => _CmdSelectedTabPanelItem ??= new RelayCommand(OnCmdSelectedTabPanelItemExecuted, CanCmdSelectedTabPanelItemExecute);
        private bool CanCmdSelectedTabPanelItemExecute(object p) => p is ScrollViewer;
        private void OnCmdSelectedTabPanelItemExecuted(object p)
        {
            ToggleButtonIsChecked = false;
            var _TabControl = App.FucusedTabControl;
            if (_TabControl == null) return;
            if (p is not ScrollViewer MyScrollViewer) return;

            foreach (var _TabItem in from object _Item in _TabControl.Items
                                     let _TabItem = _Item as CollectionMessage
                                     where _TabItem.Description == SelectedParam.Description
                                     select _TabItem)
            {
                var SelectedIndex = _TabControl.SelectedIndex;
                _TabControl.SelectedItem = _TabItem;
                if (_TabControl.SelectedIndex == (_TabControl.Items.Count - 1))
                {
                    MyScrollViewer.ScrollToRightEnd();
                    return;
                }
                else if (_TabControl.SelectedIndex == 0)
                {
                    MyScrollViewer.ScrollToLeftEnd();
                    return;
                }
                var Offset = 0d;
                if (_TabControl.SelectedIndex > SelectedIndex)
                {
                    for (int i = SelectedIndex; i < _TabControl.SelectedIndex; i++)
                    {
                        var _Item = _TabControl.Items[i] as CollectionMessage;
                        Offset += _Item.Description.Length * 6;

                    }
                    MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset + Offset);
                }

                else if (_TabControl.SelectedIndex < SelectedIndex)
                {
                    for (int i = SelectedIndex - 1; i >= _TabControl.SelectedIndex; i--)
                    {
                        var _Item = _TabControl.Items[i] as CollectionMessage;
                        Offset += _Item.Description.Length * 6;
                    }
                    MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - Offset);
                }
            }
        }
        #endregion

        #region Команда - Редактировать ячейкку
        private ICommand _CmdBeginEditCell;
        /// <summary>
        /// Команда - Редактировать ячейкку
        /// </summary>
        public ICommand CmdBeginEditCell => _CmdBeginEditCell ??= new RelayCommand(OnCmdBeginEditCellExecuted, CanCmdBeginEditCellExecute);
        private bool CanCmdBeginEditCellExecute(object p) => p is DataGrid;
        private void OnCmdBeginEditCellExecuted(object p)
        {
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid is null) return;

            MyDataGrid.BeginEdit();
        }
        #endregion

        #region Команда - Выбрать файл импорта сообщений
        private ICommand _CmdSelectedPathImport;
        /// <summary>
        /// Команда - Выбрать файл импорта сообщений
        /// </summary>
        public ICommand CmdSelectedPathImport => _CmdSelectedPathImport ??= new RelayCommand(OnCmdSelectedPathImportExecuted);
        private void OnCmdSelectedPathImportExecuted()
        {
            SelectedPathImport = "";
            if (UserDialog.SelectFile(Title, out string selected_path, Filter: "Книга Excel (*.xlsm*)|*.xlsm*"))
                SelectedPathImport = selected_path;
        }
        #endregion

        #region Команда - Импорт сообщений
        private ICommand _CmdImportMessages;
        /// <summary>
        /// Команда - Импорт сообщений
        /// </summary>
        public ICommand CmdImportMessages => _CmdImportMessages ??= new RelayCommand(OnCmdImportMessagesExecuted, CanCmdImportMessagesExecute);
        private bool CanCmdImportMessagesExecute() => !string.IsNullOrWhiteSpace(SelectedPathImport);
        private void OnCmdImportMessagesExecuted()
        {
            if (!UserDialog.SendMessage("Внимание!", "Все данные по параметрам будут потеряны!\nПродолжить?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            try
            {
                using var work_book = new XLWorkbook(SelectedPathImport);

                #region Экпорт сообщений для указанных вкладок
                if (!string.IsNullOrWhiteSpace(ListTableImport))
                {
                    var _TabItems = ListTableImport.Split(",");
                    foreach (var Item in _TabItems)
                    {
                        try
                        {
                            var StartIndexRow = 6;
                            var worksheet = work_book.Worksheets.First();
                            try
                            {
                                worksheet = work_book.Worksheets.Worksheet(Item);
                            }
                            catch (Exception)
                            {
                                var desc = $"Вкладка с именем \"{Item}\" не найдена, проверьте правльность названий вкладок. Продолжить импорт?";
                                if (!UserDialog.SendMessage(Title, desc, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None))
                                    return;

                                continue;
                            }

                            foreach (var _Messages in Params)
                            {
                                if (_Messages.Description == Item)
                                {
                                    _Messages.IndexSystem = worksheet.Cell(2, 1).Value.ToString();

                                    foreach (var _Message in _Messages.Messages)
                                    {
                                        _Message.Description = worksheet.Cell(StartIndexRow, 2).Value.ToString();
                                        _Message.Color = worksheet.Cell(StartIndexRow, 3).Value.ToString();
                                        _Message.NeedAck = worksheet.Cell(StartIndexRow, 4).Value.ToString();
                                        _Message.PathSound = worksheet.Cell(StartIndexRow, 5).Value.ToString();
                                        _Message.TypeSound = worksheet.Cell(StartIndexRow, 6).Value.ToString();
                                        _Message.NeedPlay = worksheet.Cell(StartIndexRow, 7).Value.ToString();
                                        _Message.Hide = worksheet.Cell(StartIndexRow, 8).Value.ToString();
                                        _Message.LevelAccess = worksheet.Cell(StartIndexRow, 9).Value.ToString();

                                        StartIndexRow++;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                            UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                        }
                    }
                }
                #endregion

                #region Экпорт сообщений для всех созданных вкладок
                else
                {
                    foreach (var _Messages in Params)
                    {
                        try
                        {
                            var StartIndexRow = 6;
                            var worksheet = work_book.Worksheets.Worksheet(1);
                            try
                            {
                                worksheet = work_book.Worksheets.Worksheet(_Messages.Description);
                            }
                            catch (Exception)
                            {
                                var desc = $"Вкладка с именем \"{_Messages.Description}\" не найдена, проверьте правльность названий вкладок. Продолжить импорт?";
                                if (!UserDialog.SendMessage(Title, desc, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None))
                                    return;

                                continue;
                            }

                            _Messages.IndexSystem = worksheet.Cell(2, 1).Value.ToString();
                            foreach (var _Message in _Messages.Messages)
                            {
                                _Message.Description = worksheet.Cell(StartIndexRow, 2).Value.ToString();
                                _Message.Color = worksheet.Cell(StartIndexRow, 3).Value.ToString();
                                _Message.NeedAck = worksheet.Cell(StartIndexRow, 4).Value.ToString();
                                _Message.PathSound = worksheet.Cell(StartIndexRow, 5).Value.ToString();
                                _Message.TypeSound = worksheet.Cell(StartIndexRow, 6).Value.ToString();
                                _Message.NeedPlay = worksheet.Cell(StartIndexRow, 7).Value.ToString();
                                _Message.Hide = worksheet.Cell(StartIndexRow, 8).Value.ToString();
                                _Message.LevelAccess = worksheet.Cell(StartIndexRow, 9).Value.ToString();

                                StartIndexRow++;
                            }
                        }
                        catch (Exception e)
                        {
                            var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                            UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                        }
                    }
                }
                #endregion

                var desc_msg = $"Импорт успешно завершен";
                UserDialog.SendMessage(Title, desc_msg);
                RefreshDataView();

            }
            catch (Exception e)
            {
                var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
            }
        }
        #endregion

        #region Команда - Генерировать вкладки сообщений, на основе созданных систем сообщений
        private ICommand _CmdGeneratedMessageTabList;
        /// <summary>
        /// Команда - Генерировать вкладки сообщений, на основе созданных систем сообщений
        /// </summary>
        public ICommand CmdGeneratedMessageTabList => _CmdGeneratedMessageTabList ??= new RelayCommand(OnCmdGeneratedMessageTabListExecuted);
        private void OnCmdGeneratedMessageTabListExecuted()
        {
            try
            {
                if (UserDialog.SearchControlViewModel("Сообщения") is not MessagesUserControlViewModel _TabItem) throw new NotSupportedException();
                ObservableCollection<BaseSystemMessage> ParParams = _TabItem.Params;

                if (ParParams is null || ParParams.Count <= 0) throw new NotSupportedException();
                if (!UserDialog.SendMessage("Внимание!", "Все данные по параметрам будут потеряны!\nПродолжить?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

                var _CollectionMessage = new ObservableCollection<CollectionMessage>();
                foreach (var _Param in ParParams)
                    CreateCollectionMessages(_CollectionMessage, true, _Param);

                Params = new ObservableCollection<CollectionMessage>(_CollectionMessage);
                if (Params.Count > 0)
                    SelectedParam = Params[0];

                UserDialog.SendMessage("Генерация вкладок", "Генерация успешно выполнена");
            }
            catch (Exception e)
            {
                UserDialog.SendMessage("Генерация вкладок", $"Во время генерации произошла ошибка.\n\r{e}", ImageType: MessageBoxImage.Error);
            }
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
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<CollectionMessage>;
        #endregion

        #region Фильтрация сообщений
        /// <summary>
        /// Фильтрация сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not IBaseMessage _Param || _Param is null) { e.Accepted = false; return; }
            if (SelectedParam is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(SelectedParam.TextFilter)) return;
            #endregion

            #region Сообщения
            if (_Param.Description.Contains(SelectedParam.TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

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
            _SubParamsDataView.Source = SelectedParam.Messages;
            _SubParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(SubParamsDataView));
        }
        #endregion

        #region Создание новой коллекции сообщений
        /// <summary>
        /// Создание новой коллекции сообщений
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="ImportMessage"></param>
        /// <param name="ImportData"></param>
        private static void CreateCollectionMessages(ObservableCollection<CollectionMessage> Params, bool ImportMessage = false, BaseSystemMessage ImportData = null)
        {
            #region Создаем новую коллекцию сообщений
            if (!ImportMessage)
            {
                var _Messages = Enumerable.Range(1, 4095).Select(
                i => new BaseMessage()
                {
                    Index = i.ToString(),
                    Color = "",
                    Description = "",
                    Hide = "",
                    LevelAccess = "",
                    NeedAck = "",
                    NeedPlay = "",
                    PathSound = "",
                    TypeSound = ""
                });
                Params.Add(new CollectionMessage
                {
                    IndexSystem = "",
                    Description = $"Сообщение {Params.Count}",
                    TextFilter = "",
                    Messages = new ObservableCollection<BaseMessage>(_Messages)
                });
            }
            #endregion

            #region Создаем новую коллекцию сообщений, при импорте данных
            else if (ImportMessage)
            {
                if (string.IsNullOrWhiteSpace(ImportData.DescriptionSystem)) return;
                var _Messages = Enumerable.Range(1, 4095).Select(
                i => new BaseMessage()
                {
                    Index = i.ToString(),
                    Color = "",
                    Description = "",
                    Hide = "",
                    LevelAccess = "",
                    NeedAck = "",
                    NeedPlay = "",
                    PathSound = "",
                    TypeSound = ""
                });
                Params.Add(new CollectionMessage
                {
                    IndexSystem = ImportData.Index,
                    Description = ImportData.DescriptionSystem,
                    NameSystem = ImportData.SystemMessage,
                    TextFilter = "",
                    Messages = new ObservableCollection<BaseMessage>(_Messages)
                });
            }
            #endregion
        }
        #endregion

        #endregion
    }
}
