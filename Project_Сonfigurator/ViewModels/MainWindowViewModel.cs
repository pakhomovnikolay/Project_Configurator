using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Конструктор

        public MainWindowViewModel()
        {
            Title = "Конфигуратор проекта";
        }

        public IEnumerable<IViewModelUserControls> ViewModelUserControls { get; }
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece LogSeriveces;
        private readonly IDBService DBServices;
        public readonly ISettingService SettingServices;
        private readonly ISUExportRedefineService SUExportRedefineServices;

        public MainWindowViewModel(IUserDialogService _UserDialog, ILogSerivece _ILogSerivece, IDBService _IDBService,
            ISettingService _ISettingService, ISUExportRedefineService _ISUExportRedefineService, IEnumerable<IViewModelUserControls> viewModelUserControls) : this()
        {
            #region Сервисы
            ViewModelUserControls = viewModelUserControls;
            UserDialog = _UserDialog;
            LogSeriveces = _ILogSerivece;
            DBServices = _IDBService;
            SettingServices = _ISettingService;
            SUExportRedefineServices = _ISUExportRedefineService;
            #endregion

            #region Задаем имя проекта
            SetNameProject();
            #endregion

            #region Формируем состояния выбранных узлов для импорта данных в БД
            if (App.Settings is not null && App.Settings.Config is not null && App.Settings.Config.ServerDB is not null)
            {
                IsCheckedAll = true;
                foreach (var _ServerDB in App.Settings.Config.ServerDB)
                {
                    IsCheckedAll = IsCheckedAll && _ServerDB.IsSelection;
                }
            }
            #endregion
        }
        #endregion

        #region Параметры

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

        #region Настройки приложения
        private SettingApp _Config = App.Settings.Config ?? new();
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

        #region Текущий индекс вкладки
        private int _SelectedTabIndex;
        /// <summary>
        /// Текущий индекс вкладки
        /// </summary>
        public int SelectedTabIndex
        {
            get => _SelectedTabIndex;
            set => Set(ref _SelectedTabIndex, value);
        }
        #endregion

        #region Выбранная вкладка из списка вкладок
        private IViewModelUserControls _SelectedViewModel;
        /// <summary>
        /// Выбранная вкладка из списка вкладок
        /// </summary>
        public IViewModelUserControls SelectedViewModel
        {
            get => _SelectedViewModel;
            set => Set(ref _SelectedViewModel, value);
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
            App.Services.GetRequiredService<IUserDialogService>().OpenSettingsWindow();
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
            App.DBServices.ClearDataBase();
            DBServices.ClearDataBase();
            App.Settings.Config.PathProject = "";
            SettingServices.Config = App.Settings.Config;
            SettingServices.Save();
            SetNameProject();

            foreach (var _ViewModel in ViewModelUserControls)
            {
                DBServices.RefreshDataViewModel(_ViewModel, true);
            }
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
            if (!UserDialog.OpenFile(Title, out string path, App.Settings.Config.PathProject)) return;

            App.Settings.Config.PathProject = path;
            App.DBServices.AppData = App.DBServices.LoadData(path);
            DBServices.LoadData(path);
            DBServices.AppData = App.DBServices.AppData;
            SettingServices.Config = App.Settings.Config;
            SettingServices.Save();

            if (App.DBServices.AppData is not null)
            {
                foreach (var _ViewModel in ViewModelUserControls)
                {
                    DBServices.RefreshDataViewModel(_ViewModel, false);
                }
            }

            SetNameProject();
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
            //DBServices.FormingAppDataBeforeSaving(ViewModelUserControls);
            DBServices.SaveData();
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
            App.Settings.Config.PathProject = "";
            if (!UserDialog.SaveProject(Title)) return;

            DBServices.AppData = new();
            //DBServices.FormingAppDataBeforeSaving(ViewModels);
            DBServices.SaveData();
        }
        #endregion

        #region Команда - Открыть папку с проектом
        private ICommand _CmdOpenProjectFolder;
        /// <summary>
        /// Команда - Открыть папку с проектом
        /// </summary>
        public ICommand CmdOpenProjectFolder => _CmdOpenProjectFolder ??= new RelayCommand(OnCmdOpenProjectFolderExecuted, CanCmdOpenProjectFolderExecute);
        private bool CanCmdOpenProjectFolderExecute() => !string.IsNullOrWhiteSpace(App.Settings.Config.PathProject);
        private void OnCmdOpenProjectFolderExecuted()
        {
            var name_folder = App.Settings.Config.PathProject;
            var name_project = App.Settings.Config.PathProject.Split('\\');

            if (name_project.Length <= 3)
                name_folder = App.Settings.Config.PathProject.Replace(name_project[^1], "").TrimEnd('\\');

            Process.Start("explorer.exe", name_folder);
        }
        #endregion

        #region Команда - Открыть папку с настройками проекта
        private ICommand _CmdOpenSettingsProjectFolder;
        /// <summary>
        /// Команда - Открыть папку с настройками проекта
        /// </summary>
        public ICommand CmdOpenSettingsProjectFolder => _CmdOpenSettingsProjectFolder ??= new RelayCommand(OnCmdOpenSettingsProjectFolderExecuted, CanCmdOpenSettingsProjectFolderExecute);
        private bool CanCmdOpenSettingsProjectFolderExecute() => !string.IsNullOrWhiteSpace(App.PathConfig);
        private void OnCmdOpenSettingsProjectFolderExecuted()
        {
            var name_folder = App.PathConfig;
            var name_project = App.PathConfig.Split('\\');

            if (name_project.Length < 2)
                name_folder = App.PathConfig.Replace(name_project[^1], "").TrimEnd('\\');

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
            //DBServices.RequestSetData(ViewModels);
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
            SUExportRedefineServices.Export(TypeExport, this);
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
                foreach (var _ServerDB in App.Settings.Config.ServerDB)
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
                foreach (var _ServerDB in App.Settings.Config.ServerDB)
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
            if (p is not ScrollViewer MyScrollViewer) return;

            foreach (var _TabItem in from object _Item in _TabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == SelectedViewModel.Title
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
                        var _Item = _TabControl.Items[i] as IViewModelUserControls;
                        Offset += _Item.Title.Length * 6;

                    }
                    MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset + Offset);
                }

                else if (_TabControl.SelectedIndex < SelectedIndex)
                {
                    for (int i = SelectedIndex - 1; i >= _TabControl.SelectedIndex; i--)
                    {
                        var _Item = _TabControl.Items[i] as IViewModelUserControls;
                        Offset += _Item.Title.Length * 6;
                    }
                    MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - Offset);
                }
            }
        }
        #endregion

        #region Команда - Открыть окно сообщений
        private ICommand _CmdOpenMessageWindow;
        /// <summary>
        /// Команда - Открыть окно сообщений
        /// </summary>
        public ICommand CmdOpenMessageWindow => _CmdOpenMessageWindow ??= new RelayCommand(OnCmdOpenMessageWindowExecuted, CanCmdOpenMessageWindowExecute);
        private bool CanCmdOpenMessageWindowExecute(object p) => true;
        private void OnCmdOpenMessageWindowExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialogService>().OpenMessageWindow();
        }
        #endregion

        #endregion

        #region Функции

        #region Задаем имя проекта
        /// <summary>
        /// Задаем имя проекта
        /// </summary>
        private void SetNameProject()
        {
            NameProject = "Новый проект";
            if (App.Settings is not null && App.Settings.Config is not null && !string.IsNullOrWhiteSpace(App.Settings.Config.PathProject))
            {
                var name_project = App.Settings.Config.PathProject.Split('\\');
                var name = name_project[^1].Split('.');
                NameProject = name[0];
            }
        }
        #endregion

        #endregion
    }
}
