using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using Project_Сonfigurator.Views.Windows;
using System.Windows;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;

        public LayotRackUserControlViewModel LayotRackViewModel { get; }
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }
        public SignalsDIUserControlViewModel SignalsDIViewModel { get; }
        public SignalsAIUserControlViewModel SignalsAIViewModel { get; }

        public MainWindowViewModel(
            IUserDialogService userDialog,
            ILogSerivece logSerivece,
            LayotRackUserControlViewModel layotRackViewModel,
            TableSignalsUserControlViewModel tableSignalsViewModel,
            SignalsDIUserControlViewModel signalsDIViewModel,
            SignalsAIUserControlViewModel signalsAIViewModel
            )
        {
            UserDialog = userDialog;
            Log = logSerivece;

            LayotRackViewModel = layotRackViewModel;
            TableSignalsViewModel = tableSignalsViewModel;
            SignalsDIViewModel = signalsDIViewModel;
            SignalsAIViewModel = signalsAIViewModel;
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

        #endregion

        #region Команды

        #region Команда - Открыть окно настроек
        private ICommand _CmdOpenSettingWindow;
        /// <summary>
        /// Команда - Открыть окно настроек
        /// </summary>
        public ICommand CmdOpenSettingWindow => _CmdOpenSettingWindow ??= new RelayCommand(OnCmdOpenSettingWindowExecuted);
        private void OnCmdOpenSettingWindowExecuted()
        {
            var window = new SettingWindow()
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }
        #endregion

        #endregion
    }
}
