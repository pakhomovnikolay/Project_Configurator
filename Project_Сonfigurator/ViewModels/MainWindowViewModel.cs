using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Settings;
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
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Threading;

namespace Project_Сonfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;
        private readonly IDBService _DBService;
        public readonly ISettingService _SettingService;


        private readonly ISUExportRedefineService _SUExportRedefineService;


        public LayotRackUserControlViewModel LayotRackViewModel { get; }
        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }
        public SignalsDIUserControlViewModel SignalsDIViewModel { get; }
        public SignalsAIUserControlViewModel SignalsAIViewModel { get; }
        public SignalsDOUserControlViewModel SignalsDOViewModel { get; }
        public SignalsAOUserControlViewModel SignalsAOViewModel { get; }
        public ECUserControlViewModel ECViewModel { get; }
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
            ISUExportRedefineService sUExportRedefineService,
            LayotRackUserControlViewModel layotRackViewModel,
            TableSignalsUserControlViewModel tableSignalsViewModel,
            SignalsDIUserControlViewModel signalsDIViewModel,
            SignalsAIUserControlViewModel signalsAIViewModel,
            SignalsDOUserControlViewModel signalsDOViewModel,
            SignalsAOUserControlViewModel signalsAOViewModel,
            ECUserControlViewModel eCViewModel,
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
            _SUExportRedefineService = sUExportRedefineService;

            LayotRackViewModel = layotRackViewModel;
            TableSignalsViewModel = tableSignalsViewModel;
            SignalsDIViewModel = signalsDIViewModel;
            SignalsAIViewModel = signalsAIViewModel;
            SignalsDOViewModel = signalsDOViewModel;
            SignalsAOViewModel = signalsAOViewModel;
            ECViewModel = eCViewModel;
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
            if (Program._DBService is null)
                VisibilityTabContol = Visibility.Hidden;


            if (Program.Settings is not null && Program.Settings.Config is not null && Program.Settings.Config.ServerDB is not null)
            {
                IsCheckedAll = true;
                foreach (var _ServerDB in Program.Settings.Config.ServerDB)
                {
                    IsCheckedAll = IsCheckedAll && _ServerDB.IsSelection;
                }
            }

            CreateViewModels();
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
        /// Наименование открытого проекта
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

        #region Настройки приложения
        private SettingApp _Config = Program.Settings.Config ?? new();
        /// <summary>
        /// Настройки приложения
        /// </summary>
        public SettingApp Config
        {
            get => _Config;
            set => Set(ref _Config, value);
        }
        #endregion

        #region Текущее состояние флажка "Выбрать все"
        private bool _IsCheckedAll;
        /// <summary>
        /// Текущее состояние флажка "Выбрать все"
        /// </summary>
        public bool IsCheckedAll
        {
            get => _IsCheckedAll;
            set => Set(ref _IsCheckedAll, value);
        }
        #endregion

        #region Коллекция ViewModels
        private List<object> _ViewModels = new();
        /// <summary>
        /// Коллекция ViewModels
        /// </summary>
        public List<object> ViewModels
        {
            get => _ViewModels;
            set => Set(ref _ViewModels, value);
        }
        #endregion

        #region Коллекция ViewModelsHeader
        private List<string> _ViewModelsHeader = new();
        /// <summary>
        /// Коллекция ViewModelsHeader
        /// </summary>
        public List<string> ViewModelsHeader
        {
            get => _ViewModelsHeader;
            set => Set(ref _ViewModelsHeader, value);
        }
        #endregion

        #region Выбранный ViewModelsHeader
        private string _SelectedViewModelsHeader;
        /// <summary>
        /// Выбранный ViewModelsHeader
        /// </summary>
        public string SelectedViewModelsHeader
        {
            get => _SelectedViewModelsHeader;
            set => Set(ref _SelectedViewModelsHeader, value);
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
            Program._DBService.ClearDataBase();
            _DBService.ClearDataBase();
            Program.Settings.Config.PathProject = "";
            _SettingService.Config = Program.Settings.Config;
            _SettingService.Save();
            SetNameProject();

            foreach (var _ViewModel in ViewModels)
            {
                _DBService.RefreshDataViewModel(_ViewModel, true);
            }

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
            Program._DBService.AppData = Program._DBService.LoadData(path);
            _DBService.LoadData(path);
            _DBService.AppData = Program._DBService.AppData;
            _SettingService.Config = Program.Settings.Config;
            _SettingService.Save();

            if (Program._DBService.AppData is not null)
            {
                foreach (var _ViewModel in ViewModels)
                {
                    _DBService.RefreshDataViewModel(_ViewModel, false);
                }
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
            _DBService.SaveData();
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

            _DBService.AppData = new();
            FormingAppDataBeforeSaving();
            _DBService.SaveData();
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

        #region Команда - Сохранить данные в БД
        private ICommand _CmdUploadDB;
        /// <summary>
        /// Команда - Сохранить данные в БД
        /// </summary>
        public ICommand CmdUploadDB => _CmdUploadDB ??= new RelayCommand(OnCmdUploadDBExecuted);
        private void OnCmdUploadDBExecuted()
        {
            FormingAppDataBeforeSaving();
            _DBService.SetData();
        }
        #endregion

        #region Команда - Экспорт СУ
        private ICommand _CmdExportSU;
        /// <summary>
        /// Команда - Экспорт СУ
        /// </summary>
        public ICommand CmdExportSU => _CmdExportSU ??= new RelayCommand(OnCmdExportSUExecuted, CanCmdExportSUExecute);
        private bool CanCmdExportSUExecute(object p) => p is not null && p is string;
        private void OnCmdExportSUExecuted(object p)
        {
            var TypeExport = (string)p;
            _SUExportRedefineService.Export(TypeExport, this);
        }
        #endregion

        #region Команда - Снять\Установить узел для применения конфигурации
        private ICommand _CmdSelectionServer;
        /// <summary>
        /// Команда - Снять\Установить узел для применения конфигурации
        /// </summary>
        public ICommand CmdSelectionServer => _CmdSelectionServer ??= new RelayCommand(OnCmdSelectionServerExecuted);
        private void OnCmdSelectionServerExecuted()
        {
            if (Config is not null && Config.ServerDB is not null)
            {
                IsCheckedAll = true;
                foreach (var _ServerDB in Program.Settings.Config.ServerDB)
                {
                    IsCheckedAll = IsCheckedAll && _ServerDB.IsSelection;
                }
            }
        }
        #endregion

        #region Команда - Снять\Установить узелы для применения конфигурации
        private ICommand _CmdSelectionAllServer;
        /// <summary>
        /// Команда - Снять\Установить узелы для применения конфигурации
        /// </summary>
        public ICommand CmdSelectionAllServer => _CmdSelectionAllServer ??= new RelayCommand(OnCmdSelectionAllServerExecuted);
        private void OnCmdSelectionAllServerExecuted()
        {
            if (Config is not null && Config.ServerDB is not null)
            {
                foreach (var _ServerDB in Program.Settings.Config.ServerDB)
                {
                    _ServerDB.IsSelection = IsCheckedAll;
                }
            }
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
            if (p is not ScrollViewer _ScrollViewer) return;
            var ScrollToEnd = false;

            foreach (var _TabItem in from object _Item in _TabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == SelectedViewModelsHeader
                                     select _TabItem)
            {
                var SelectedIndex = _TabControl.Items.IndexOf(_TabItem);



                //if (SelectedIndex >= _TabControl.SelectedIndex)
                //    _ScrollViewer.ScrollToHorizontalOffset(_ScrollViewer.HorizontalOffset + _TabItem.Header.ToString().Length * 5);
                //else
                //    _ScrollViewer.ScrollToHorizontalOffset(_ScrollViewer.HorizontalOffset - _TabItem.Header.ToString().Length * 5);

                //_TabControl.SelectedIndex = SelectedIndex;


                if (SelectedIndex >= (_TabControl.Items.Count - 1))
                {
                    _TabControl.SelectedIndex = SelectedIndex;
                    _ScrollViewer.ScrollToRightEnd();
                    ScrollToEnd = true;
                }
                else if (SelectedIndex <= 0)
                {
                    _TabControl.SelectedIndex = SelectedIndex;
                    _ScrollViewer.ScrollToLeftEnd();
                    ScrollToEnd = true;
                }

                if (!ScrollToEnd)
                {
                    if (SelectedIndex >= _TabControl.SelectedIndex)
                    {
                        _TabControl.SelectedIndex = SelectedIndex;
                        _ScrollViewer.LineRight();
                    }

                    else if (SelectedIndex <= _TabControl.SelectedIndex)
                    {
                        _TabControl.SelectedIndex = SelectedIndex;
                        _ScrollViewer.LineLeft();
                    }
                }
            }
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
                _DBService.AppData.USOList = LayotRackViewModel.DataView is null ? new() : (List<USO>)LayotRackViewModel.DataView.SourceCollection;
                _DBService.AppData.UserDI = UserDIViewModel.DataView is null ? new() : (List<BaseSignal>)UserDIViewModel.DataView.SourceCollection;
                _DBService.AppData.UserAI = UserAIViewModel.DataView is null ? new() : (List<BaseSignal>)UserAIViewModel.DataView.SourceCollection;
                _DBService.AppData.SignalDI = SignalsDIViewModel.DataView is null ? new() : (List<SignalDI>)SignalsDIViewModel.DataView.SourceCollection;
                _DBService.AppData.SignalAI = SignalsAIViewModel.DataView is null ? new() : (List<SignalAI>)SignalsAIViewModel.DataView.SourceCollection;
                _DBService.AppData.SignalDO = SignalsDOViewModel.DataView is null ? new() : (List<SignalDO>)SignalsDOViewModel.DataView.SourceCollection;
                _DBService.AppData.SignalAO = SignalsAOViewModel.DataView is null ? new() : (List<SignalAO>)SignalsAOViewModel.DataView.SourceCollection;
                _DBService.AppData.ECParam = ECViewModel.DataView is null ? new() : (List<BaseParam>)ECViewModel.DataView.SourceCollection;
                _DBService.AppData.UserReg = UserRegUserModel.DataView is null ? new() : (List<BaseParam>)UserRegUserModel.DataView.SourceCollection;
                _DBService.AppData.SignalGroup = SignalsGroupViewModel.DataView is null ? new() : (List<BaseParam>)SignalsGroupViewModel.DataView.SourceCollection;
                _DBService.AppData.GroupSignals = GroupsSignalViewModel.DataView is null ? new() : (List<GroupSignal>)GroupsSignalViewModel.DataView.SourceCollection;
                _DBService.AppData.UZD = UZDViewModel.DataView is null ? new() : (List<BaseUZD>)UZDViewModel.DataView.SourceCollection;
                _DBService.AppData.UVS = UVSViewModel.DataView is null ? new() : (List<BaseUVS>)UVSViewModel.DataView.SourceCollection;
                _DBService.AppData.UMPNA = UMPNAViewModel.DataView is null ? new() : (List<BaseUMPNA>)UMPNAViewModel.DataView.SourceCollection;
                _DBService.AppData.KTPR = KTPRViewModel.DataView is null ? new() : (List<BaseKTPR>)KTPRViewModel.DataView.SourceCollection;
                _DBService.AppData.KTPRS = KTPRSViewModel.DataView is null ? new() : (List<BaseKTPRS>)KTPRSViewModel.DataView.SourceCollection;
                _DBService.AppData.Signaling = SignalingViewModel.DataView is null ? new() : (List<BaseSignaling>)SignalingViewModel.DataView.SourceCollection;
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

        #region Создаем коллекцию ViewModels
        private void CreateViewModels()
        {
            ViewModels = new()
            {
                LayotRackViewModel,
                TableSignalsViewModel,
                SignalsDIViewModel,
                SignalsAIViewModel,
                SignalsDOViewModel,
                SignalsAOViewModel,
                ECViewModel,
                UserDIViewModel,
                UserAIViewModel,
                UserRegUserModel,
                SignalsGroupViewModel,
                GroupsSignalViewModel,
                UZDViewModel,
                UVSViewModel,
                UMPNAViewModel,
                KTPRViewModel,
                KTPRSViewModel,
                SignalingViewModel
            };

            ViewModelsHeader = new()
            {
                LayotRackViewModel.Title,
                TableSignalsViewModel.Title,
                SignalsDIViewModel.Title,
                SignalsAIViewModel.Title,
                SignalsDOViewModel.Title,
                SignalsAOViewModel.Title,
                ECViewModel.Title,
                UserDIViewModel.Title,
                UserAIViewModel.Title,
                UserRegUserModel.Title,
                SignalsGroupViewModel.Title,
                GroupsSignalViewModel.Title,
                UZDViewModel.Title,
                UVSViewModel.Title,
                UMPNAViewModel.Title,
                KTPRViewModel.Title,
                KTPRSViewModel.Title,
                SignalingViewModel.Title
            };
        }
        #endregion

        #endregion
    }
}
