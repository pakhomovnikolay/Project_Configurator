using ClosedXML.Excel;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly IUserDialogService UserDialog;
        private readonly IDBService _DBService;
        public MessageWindowViewModel(IUserDialogService userDialog, IDBService dBService)
        {
            UserDialog = userDialog;
            _DBService = dBService;

            _DBService.RefreshDataViewModel(this, false);
        }
        #endregion

        #region Параметры

        #region Заголовок окна
        private string _Title = "Сообщения";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
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
        private int _WindowWidth = 1200;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

        #region Режим изменения размеров окна
        private ResizeMode _WindowResizeMode = ResizeMode.CanResizeWithGrip;
        /// <summary>
        /// Режим изменения размеров окна
        /// </summary>
        public ResizeMode WindowResizeMode
        {
            get => _WindowResizeMode;
            set => Set(ref _WindowResizeMode, value);
        }
        #endregion

        #region Текущее состояние окна
        private WindowState _WindowWindowState = WindowState.Maximized;
        /// <summary>
        /// Текущее состояние окна
        /// </summary>
        public WindowState WindowWindowState
        {
            get => _WindowWindowState;
            set
            {
                if (Set(ref _WindowWindowState, value))
                {
                    ButtonChangeStateWindowStyle = _WindowWindowState == WindowState.Normal ? (Style)Application.Current.FindResource("MaximizedButtonStyle") : (Style)Application.Current.FindResource("MinimizedButtonStyle");
                }
            }
        }
        #endregion

        #region Стиль кнопки изменения состояния окна
        private Style _ButtonChangeStateWindowStyle = (Style)Application.Current.FindResource("MaximizedButtonStyle");
        /// <summary>
        /// Стиль кнопки изменения состояния окна
        /// </summary>
        public Style ButtonChangeStateWindowStyle
        {
            get => _ButtonChangeStateWindowStyle;
            set => Set(ref _ButtonChangeStateWindowStyle, value);
        }
        #endregion

        #region Список сообщений
        private List<CollectionMessage> _CollectionMessages = new();
        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<CollectionMessage> CollectionMessages
        {
            get => _CollectionMessages;
            set => Set(ref _CollectionMessages, value);
        }
        #endregion

        #region Коллекция сообщений
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранная коллекция сообщений
        private CollectionMessage _SelectedMessages = new();
        /// <summary>
        /// Выбранная коллекция сообщений
        /// </summary>
        public CollectionMessage SelectedMessages
        {
            get => _SelectedMessages;
            set
            {
                if (Set(ref _SelectedMessages, value))
                    SelectedMessage = _SelectedMessages?.Messages[0];
            }
        }
        #endregion

        #region Выбранное сообщение
        private BaseMessage _SelectedMessage = new();
        /// <summary>
        /// Выбранное сообщение
        /// </summary>
        public BaseMessage SelectedMessage
        {
            get => _SelectedMessage;
            set => Set(ref _SelectedMessage, value);
        }
        #endregion

        #region Выбранная вкладка, из списка вкладок
        private CollectionMessage _SelectedTabItem;
        /// <summary>
        /// Выбранная вкладка, из списка вкладок
        /// </summary>
        public CollectionMessage SelectedTabItem
        {
            get => _SelectedTabItem;
            set => Set(ref _SelectedTabItem, value);
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

        #region Выбранная вкладка
        private int _SelectedTabIndex;
        /// <summary>
        /// Выбранная вкладка
        /// </summary>
        public int SelectedTabIndex
        {
            get => _SelectedTabIndex;
            set => Set(ref _SelectedTabIndex, value);
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

        #region Команда - Добавить коллекция сообщений
        private ICommand _CmdCreateCollectionMessages;
        /// <summary>
        /// Команда - Добавить коллекция сообщений
        /// </summary>
        public ICommand CmdCreateCollectionMessages => _CmdCreateCollectionMessages ??= new RelayCommand(OnCmdCreateCollectionMessagesExecuted);
        private void OnCmdCreateCollectionMessagesExecuted(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            if (CollectionMessages.Count > 0)
            {
                SelectedMessages = CollectionMessages[^2];
                SelectedTabItem = SelectedMessages;
                SelectedMessage = SelectedMessages.Messages[0];
            }

            var msg = Enumerable.Range(1, 4095).Select(i => new BaseMessage() { Index = i.ToString() });
            CollectionMessages.Add(new CollectionMessage { IndexSystem = "", Description = $"Сообщение {CollectionMessages.Count}", Messages = new List<BaseMessage>(msg) });

            SelectedMessages = CollectionMessages[^1];
            SelectedTabItem = SelectedMessages;
            SelectedMessage = SelectedMessages.Messages[0];

            OnCmdSelectedTabPanelItemExecuted(MyScrollViewer);
            _DataView.Source = CollectionMessages;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion

        #region Команда - Удалить коллекция сообщений
        private ICommand _CmdDeleteCollectionMessages;
        /// <summary>
        /// Команда - Удалить коллекция сообщений
        /// </summary>
        public ICommand CmdDeleteCollectionMessages => _CmdDeleteCollectionMessages ??= new RelayCommand(OnCmdDeleteCollectionMessagesExecuted);
        private void OnCmdDeleteCollectionMessagesExecuted()
        {
            var index = CollectionMessages.IndexOf(SelectedMessages);
            index = index == 0 ? index : index - 1;

            CollectionMessages.Remove(SelectedMessages);
            if (CollectionMessages.Count > 0)
            {
                SelectedMessages = CollectionMessages[index];
                _DataView.Source = CollectionMessages;
                _DataView.View?.Refresh();
            }
            else
            {
                _DataView.Source = CollectionMessages;
                _DataView.View?.Refresh();
                SelectedMessages = null;
            }
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
            SelectedMessage.PathSound = select_file;

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
                                     where _TabItem.Description == SelectedTabItem.Description
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
        public ICommand CmdImportMessages => _CmdImportMessages ??= new RelayCommand(OnCmdImportMessagesExecuted);
        private void OnCmdImportMessagesExecuted()
        {
            try
            {
                using var work_book = new XLWorkbook(SelectedPathImport);
                if (!string.IsNullOrWhiteSpace(ListTableImport))
                {
                    var _TabItems = ListTableImport.Split(",");
                    foreach (var Item in _TabItems)
                    {
                        try
                        {
                            var StartIndexRow = 6;
                            var worksheet = work_book.Worksheets.Worksheet(1);
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

                            foreach (var _Messages in CollectionMessages)
                            {
                                if (_Messages.Description == Item)
                                {
                                    _Messages.IndexSystem = worksheet.Cell(2, 1).Value.ToString();

                                    while (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, 1).Value.ToString()))
                                    {
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
                        }
                        catch (Exception e)
                        {
                            var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                            UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                        }
                    }
                }
                else
                {
                    foreach (var _Messages in CollectionMessages)
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

                            while (!string.IsNullOrWhiteSpace(worksheet.Cell(StartIndexRow, 1).Value.ToString()))
                            {
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
                        catch (Exception e)
                        {
                            var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                            UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                var desc = $"Импорт завершен с ошибкой:\nПроверьте указанный путь к файлу,\nконфигурацию проекта и настройки импорта\n{e}";
                UserDialog.SendMessage(Title, desc, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
            }

            _DataView.Source = CollectionMessages;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }


        #endregion

        #endregion

        #region Функции

        #region Генерируем данные
        public void GeneratedData()
        {
            _DataView.Source = CollectionMessages;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));

            if (CollectionMessages is null || CollectionMessages.Count <= 0)
                SelectedMessage = null;
            else if (SelectedMessage is null)
                SelectedMessages = CollectionMessages[0];
        }
        #endregion

        #endregion
    }
}
