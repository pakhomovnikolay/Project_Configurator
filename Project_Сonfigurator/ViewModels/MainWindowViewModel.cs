using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using Project_Сonfigurator.Views.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;
        private readonly IDBService _DBService;
        public ISettingService _SettingService;

        public LayotRackUserControlViewModel LayotRackViewModel { get; }
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }
        public SignalsDIUserControlViewModel SignalsDIViewModel { get; }
        public SignalsAIUserControlViewModel SignalsAIViewModel { get; }
        public SignalsDOUserControlViewModel SignalsDOViewModel { get; }
        public SignalsAOUserControlViewModel SignalsAOViewModel { get; }
        public UserDIUserControlViewModel UserDIViewModel { get; }
        public UserAIUserControlViewModel UserAIViewModel { get; }
        public UserRegUserControlViewModel UserRegUserModel { get; }
        public SignalsGroupUserControlViewModel SignalsGroupViewModel { get; }
        public GroupsSignalUserControlViewModel GroupsSignalViewModel { get; }
        public UZDUserControlViewModel UZDViewModel { get; }
        public UVSUserControlViewModel UVSViewModel { get; }
        public UMPNAUserControlViewModel UMPNAViewModel { get; }
        public KTPRUserControlViewModel KTPRViewModel { get; }
        public KTPRSUserControlViewModel KTPRSViewModel { get; }
        public SignalingUserControlViewModel SignalingViewModel { get; }

        public MainWindowViewModel(
            IUserDialogService userDialog,
            ILogSerivece logSerivece,
            IDBService dDBService,
            ISettingService settingService,
            LayotRackUserControlViewModel layotRackViewModel,
            TableSignalsUserControlViewModel tableSignalsViewModel,
            SignalsDIUserControlViewModel signalsDIViewModel,
            SignalsAIUserControlViewModel signalsAIViewModel,
            SignalsDOUserControlViewModel signalsDOViewModel,
            SignalsAOUserControlViewModel signalsAOViewModel,
            UserDIUserControlViewModel userDIViewModel,
            UserAIUserControlViewModel userAIViewModel,
            UserRegUserControlViewModel userRegUserModel,
            SignalsGroupUserControlViewModel signalsGroupViewModel,
            GroupsSignalUserControlViewModel groupsSignalViewModel,
            UZDUserControlViewModel uZDViewModel,
            UVSUserControlViewModel uVSViewModel,
            UMPNAUserControlViewModel uMPNAViewModel,
            KTPRUserControlViewModel kTPRViewModel,
            KTPRSUserControlViewModel kTPRSViewModel,
            SignalingUserControlViewModel signalingViewModel
            )
        {
            UserDialog = userDialog;
            Log = logSerivece;
            _DBService = dDBService;
            _SettingService = settingService;

            LayotRackViewModel = layotRackViewModel;
            TableSignalsViewModel = tableSignalsViewModel;
            SignalsDIViewModel = signalsDIViewModel;
            SignalsAIViewModel = signalsAIViewModel;
            SignalsDOViewModel = signalsDOViewModel;
            SignalsAOViewModel = signalsAOViewModel;
            UserDIViewModel = userDIViewModel;
            UserAIViewModel = userAIViewModel;
            UserRegUserModel = userRegUserModel;
            SignalsGroupViewModel = signalsGroupViewModel;
            GroupsSignalViewModel = groupsSignalViewModel;
            UZDViewModel = uZDViewModel;
            UVSViewModel = uVSViewModel;
            UMPNAViewModel = uMPNAViewModel;
            KTPRViewModel = kTPRViewModel;
            KTPRSViewModel = kTPRSViewModel;
            SignalingViewModel = signalingViewModel;


            SetNameProject();
            if (Program.Settings.AppData is null)
                VisibilityTabContol = Visibility.Hidden;
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

        #region Наименование открытого проекта
        private string _NameProject;
        /// <summary>
        /// наименование открытого проекта
        /// </summary>
        public string NameProject
        {
            get => _NameProject;
            set => Set(ref _NameProject, value);
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

        #region Видимость панели элементов
        private Visibility _VisibilityTabContol = Visibility.Visible;
        /// <summary>
        /// Видимость панели элементов
        /// </summary>
        public Visibility VisibilityTabContol
        {
            get => _VisibilityTabContol;
            set => Set(ref _VisibilityTabContol, value);
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

        #region Команда - Создать проект
        private ICommand _CmdCreateProject;
        /// <summary>
        /// Команда - Создать проект
        /// </summary>
        public ICommand CmdCreateProject => _CmdCreateProject ??= new RelayCommand(OnCmdCreateProjectExecuted);
        private void OnCmdCreateProjectExecuted()
        {
            Program.Settings.AppData = null;
            Program.Settings.Config.PathProject = "";
            _SettingService.Config = Program.Settings.Config;
            _SettingService.Save();
            SetNameProject();

            LayotRackViewModel.GeneratedData();
            TableSignalsViewModel.GeneratedData();
            UserDIViewModel.GeneratedSignals();
            UserAIViewModel.GeneratedSignals();
            SignalsDIViewModel.GeneratedData();
            SignalsAIViewModel.GeneratedData();
            SignalsDOViewModel.GeneratedData();
            SignalsAOViewModel.GeneratedData();
            UserRegUserModel.GeneratedSignals();
            SignalsGroupViewModel.GeneratedSignals();
            GroupsSignalViewModel.GeneratedSignals();
            UZDViewModel.GeneratedSignals();
            UVSViewModel.GeneratedSignals();
            UMPNAViewModel.GeneratedSignals();
            KTPRViewModel.GeneratedSignals();
            KTPRSViewModel.GeneratedSignals();
            SignalingViewModel.GeneratedSignals();

            VisibilityTabContol = Visibility.Visible;
        }
        #endregion

        #region Команда - Отркыть проект
        private ICommand _CmdOpenProject;
        /// <summary>
        /// Команда - Отркыть проект
        /// </summary>
        public ICommand CmdOpenProject => _CmdOpenProject ??= new RelayCommand(OnCmdOpenProjectExecuted);
        private void OnCmdOpenProjectExecuted()
        {
            if (!UserDialog.OpenFile(Title, out string path, Program.Settings.Config.PathProject)) return;

            Program.Settings.Config.PathProject = path;
            Program.Settings.LoadData(path);
            if (Program.Settings.AppData is not null)
            {
                LayotRackViewModel.GeneratedData();
                TableSignalsViewModel.GeneratedData();

                UserDIViewModel.GeneratedSignals();
                UserAIViewModel.GeneratedSignals();
                SignalsDIViewModel.GeneratedData();
                SignalsAIViewModel.GeneratedData();
                SignalsDOViewModel.GeneratedData();
                SignalsAOViewModel.GeneratedData();
                UserRegUserModel.GeneratedSignals();
                SignalsGroupViewModel.GeneratedSignals();
                GroupsSignalViewModel.GeneratedSignals();
                UZDViewModel.GeneratedSignals();
                UVSViewModel.GeneratedSignals();
                UMPNAViewModel.GeneratedSignals();
                KTPRViewModel.GeneratedSignals();
                KTPRSViewModel.GeneratedSignals();
                SignalingViewModel.GeneratedSignals();
            }

            SetNameProject();
            VisibilityTabContol = Visibility.Visible;
        }
        #endregion

        #region Команда - Сохранить данные
        private ICommand _CmdSaveData;
        /// <summary>
        /// Команда - Сохранить данные
        /// </summary>
        public ICommand CmdSaveData => _CmdSaveData ??= new RelayCommand(OnCmdSaveDataExecuted);
        private void OnCmdSaveDataExecuted()
        {
            if (!UserDialog.SaveProject(Title)) return;
            FormingAppDataBeforeSaving();
            _SettingService.SaveData();
            _DBService.GetData();
        }
        #endregion

        #region Команда - Сохранить данные как..
        private ICommand _CmdSaveAsData;
        /// <summary>
        /// Команда - Сохранить данные как..
        /// </summary>
        public ICommand CmdSaveAsData => _CmdSaveAsData ??= new RelayCommand(OnCmdSaveAsDataExecuted);
        private void OnCmdSaveAsDataExecuted()
        {
            Program.Settings.Config.PathProject = "";
            if (!UserDialog.SaveProject(Title)) return;
            FormingAppDataBeforeSaving();
            _SettingService.SaveData();
            _DBService.GetData();
        }
        #endregion

        #region Команда - Открыть папку с проектом
        private ICommand _OpenProjectFolder;
        /// <summary>
        /// Команда - Открыть папку с проектом
        /// </summary>
        public ICommand OpenProjectFolder => _OpenProjectFolder ??= new RelayCommand(OnOpenProjectFolderExecuted, CanOpenProjectFolderExecute);
        private bool CanOpenProjectFolderExecute() => !string.IsNullOrWhiteSpace(Program.Settings.Config.PathProject);
        private void OnOpenProjectFolderExecuted()
        {
            var name_project = Program.Settings.Config.PathProject.Split('\\');
            var name_folder = Program.Settings.Config.PathProject.Replace(name_project[^1], "");
            Process.Start("explorer.exe", name_folder);
        }
        #endregion

        #endregion

        #region Функции

        #region Формируем данные приложения перед сохранением
        /// <summary>
        /// Формируем данные приложения перед сохранением
        /// </summary>
        private void FormingAppDataBeforeSaving()
        {
            try
            {
                _SettingService.AppData.USOList = LayotRackViewModel.DataView is null ? new() : (List<USO>)LayotRackViewModel.DataView.SourceCollection;
                _SettingService.AppData.UserDI = UserDIViewModel.DataView is null ? new() : (List<BaseSignal>)UserDIViewModel.DataView.SourceCollection;
                _SettingService.AppData.UserAI = UserAIViewModel.DataView is null ? new() : (List<BaseSignal>)UserAIViewModel.DataView.SourceCollection;
                _SettingService.AppData.SignalDI = SignalsDIViewModel.DataView is null ? new() : (List<SignalDI>)SignalsDIViewModel.DataView.SourceCollection;
                _SettingService.AppData.SignalAI = SignalsAIViewModel.DataView is null ? new() : (List<SignalAI>)SignalsAIViewModel.DataView.SourceCollection;
                _SettingService.AppData.SignalDO = SignalsDOViewModel.DataView is null ? new() : (List<SignalDO>)SignalsDOViewModel.DataView.SourceCollection;
                _SettingService.AppData.SignalAO = SignalsAOViewModel.DataView is null ? new() : (List<SignalAO>)SignalsAOViewModel.DataView.SourceCollection;
                _SettingService.AppData.UserReg = UserRegUserModel.DataView is null ? new() : (List<BaseParam>)UserRegUserModel.DataView.SourceCollection;
                _SettingService.AppData.SignalGroup = SignalsGroupViewModel.DataView is null ? new() : (List<BaseParam>)SignalsGroupViewModel.DataView.SourceCollection;
                _SettingService.AppData.GroupSignals = GroupsSignalViewModel.DataView is null ? new() : (List<GroupSignal>)GroupsSignalViewModel.DataView.SourceCollection;
                _SettingService.AppData.UZD = UZDViewModel.DataView is null ? new() : (List<BaseUZD>)UZDViewModel.DataView.SourceCollection;
                _SettingService.AppData.UVS = UVSViewModel.DataView is null ? new() : (List<BaseUVS>)UVSViewModel.DataView.SourceCollection;
                _SettingService.AppData.UMPNA = UMPNAViewModel.DataView is null ? new() : (List<BaseUMPNA>)UMPNAViewModel.DataView.SourceCollection;
                _SettingService.AppData.KTPR = KTPRViewModel.DataView is null ? new() : (List<BaseKTPR>)KTPRViewModel.DataView.SourceCollection;
                _SettingService.AppData.KTPRS = KTPRSViewModel.DataView is null ? new() : (List<BaseKTPRS>)KTPRSViewModel.DataView.SourceCollection;
                _SettingService.AppData.Signaling = SignalingViewModel.DataView is null ? new() : (List<BaseSignaling>)SignalingViewModel.DataView.SourceCollection;
            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
            }

        }
        #endregion

        #region Задаем имя проекта
        /// <summary>
        /// Задаем имя проекта
        /// </summary>
        private void SetNameProject()
        {
            NameProject = "Новый проект";
            if (Program.Settings is not null && Program.Settings.Config is not null && !string.IsNullOrWhiteSpace(Program.Settings.Config.PathProject))
            {
                var name_project = Program.Settings.Config.PathProject.Split('\\');
                var name = name_project[^1].Split('.');
                NameProject = name[0];
            }
        }
        #endregion

        #endregion
    }
}
