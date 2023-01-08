using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
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
        IUserDialogService UserDialog;
        IDBService _DBService;
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
        private List<CollectionMessage> _Messages = new();
        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<CollectionMessage> Messages
        {
            get => _Messages;
            set => Set(ref _Messages, value);
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
            set => Set(ref _SelectedMessages, value);
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

        #endregion

        #region Команды

        #region Команда - Добавить коллекция сообщений
        private ICommand _CmdCreateCollectionMessages;
        /// <summary>
        /// Команда - Добавить коллекция сообщений
        /// </summary>
        public ICommand CmdCreateCollectionMessages => _CmdCreateCollectionMessages ??= new RelayCommand(OnCmdCreateCollectionMessagesExecuted);
        private void OnCmdCreateCollectionMessagesExecuted()
        {
            var msg = Enumerable.Range(1, 4095).Select(i => new BaseMessage()
            {
                Index = i.ToString()
            });

            Messages.Add(new CollectionMessage
            {
                Description = $"Сообщение {Messages.Count}",
                Messages = new List<BaseMessage>(msg)
            });

            SelectedMessages = Messages[^1];
            SelectedMessage = Messages[^1].Messages[0];
            _DataView.Source = Messages;
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
            var index = Messages.IndexOf(SelectedMessages);
            index = index == 0 ? index : index - 1;

            Messages.Remove(SelectedMessages);
            if (Messages.Count > 0)
            {
                SelectedMessages = Messages[index];
                _DataView.Source = Messages;
                _DataView.View?.Refresh();
            }
            else
            {
                _DataView.Source = Messages;
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
        private bool CanCmdSelectionPathSoundExecute(object p) => p is not null;
        private void OnCmdSelectionPathSoundExecuted(object p)
        {
            if (p is not DataGrid MyDataGrid) return;

            if (!UserDialog.SelectFile("Выбрать файл звука", out string select_file)) return;
            SelectedMessage.PathSound = select_file;

            MyDataGrid.Items.Refresh();
        }
        #endregion

        #endregion

        #region Функции

        #region Генерируем данные
        public void GeneratedData()
        {
            _DataView.Source = Messages;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));

            if (Messages is null || Messages.Count <= 0)
                SelectedMessage = null;
        }
        #endregion

        #endregion
    }
}
