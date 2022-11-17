using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Windows;

namespace Project_Сonfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;

        public MainWindowViewModel(IUserDialogService userDialog, ILogSerivece logSerivece)
        {
            UserDialog = userDialog;
            Log = logSerivece;
        }
        #endregion

        #region Параметры

        #region Заголовок окна
        private string _Title = "Конфигуратор проекта";
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
        private int _WindowWidth = 800;
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
        private WindowState _WindowWindowState = WindowState.Normal;
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

        #endregion
    }
}
