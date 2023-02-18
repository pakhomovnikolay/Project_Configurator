using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.ViewModels.AS;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            WindowWindowState = WindowState.Maximized;

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

        public IEnumerable<IViewModelUserControls> ViewModelUserControls { get; }
        public MainWindowViewModel(IEnumerable<IViewModelUserControls> viewModelUserControls) : this()
        {
            ViewModelUserControls = viewModelUserControls;
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
            set
            {
                if (Set(ref _SelectedTabIndex, value))
                {
                    var i = 0;
                    foreach (var _ViewModel in ViewModelUserControls)
                    {
                        _ViewModel.IsSelected = false;
                        if (_SelectedTabIndex == i)
                            _ViewModel.IsSelected = true;
                        i++;
                    }
                }
            }
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
            UserDialog.OpenSettingsWindow();
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
            DBServices.CreateNewProject();
            SetNameProject();
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
            string Filter = $"Все файлы (*{App.__EncryptedProjectFileSuffix}*)|*{App.__EncryptedProjectFileSuffix}*";
            if (!UserDialog.OpenFile(Title, out string path, App.Settings.Config.PathProject, Filter)) return;
            App.Settings.Config.PathProject = path;

            DBServices.ProjectDataRequest();
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
            if (string.IsNullOrWhiteSpace(App.Settings.Config.PathProject))
            {
                string Filter = $"Все файлы (*{App.__EncryptedProjectFileSuffix}*)|*{App.__EncryptedProjectFileSuffix}*";
                if (!UserDialog.SelectFolder(Title, out string path, out string file_name, App.Settings.Config.PathProject, Filter)) return;
                if (!file_name.Contains(App.__EncryptedProjectFileSuffix, StringComparison.CurrentCultureIgnoreCase))
                    file_name = $"{file_name}.{App.__EncryptedProjectFileSuffix}";
                App.Settings.Config.PathProject = path + file_name;
            }
            SettingServices.Config = App.Settings.Config;
            SettingServices.Save();
            DBServices.RequestToWriteProjectData();
            SetNameProject();
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
            string Filter = $"Все файлы (*{App.__EncryptedProjectFileSuffix}*)|*{App.__EncryptedProjectFileSuffix}*";
            if (!UserDialog.SelectFolder(Title, out string path, out string file_name, App.Settings.Config.PathProject, Filter)) return;
            if (!file_name.Contains(App.__EncryptedProjectFileSuffix, StringComparison.CurrentCultureIgnoreCase))
                file_name = $"{file_name}.{App.__EncryptedProjectFileSuffix}";
            App.Settings.Config.PathProject = path + file_name;

            SettingServices.Config = App.Settings.Config;
            SettingServices.Save();
            DBServices.RequestToWriteProjectData();
            SetNameProject();
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
            var name_folder = App.Settings.Config.PathProject.Replace(NameProject + App.__EncryptedProjectFileSuffix, "");
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
            DBServices.RequestToWriteDataToTheDataBase();
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
            SUExportRedefineServices.Export(TypeExport);
        }
        #endregion

        #region Команда - Экспорт ВУ
        private ICommand _CmdOpenExportNamespaceASWindow;
        /// <summary>
        /// Команда - Экспорт ВУ
        /// </summary>
        public ICommand CmdOpenExportNamespaceASWindow => _CmdOpenExportNamespaceASWindow ??= new RelayCommand(OnCmdOpenExportNamespaceASWindowExecuted, CanCmdOpenExportNamespaceASWindowExecute);
        private bool CanCmdOpenExportNamespaceASWindowExecute(object p) => p is not null && p is string;
        private void OnCmdOpenExportNamespaceASWindowExecuted(object p)
        {
            var type_cmd = p as string;
            switch (type_cmd)
            {
                case "Экспорт пространства имен":
                    UserDialog.OpenExportNamespaceASWindow(App.Services.GetRequiredService<ExportNamespaceASWindowViewModel>());
                    break;
                case "Экспорт приложение PLC":
                    UserDialog.OpenExportNamespaceASWindow(App.Services.GetRequiredService<PLCExportASWindowViewModel>());
                    break;
                case "Экспорт приложение IOS":
                    UserDialog.OpenExportNamespaceASWindow(App.Services.GetRequiredService<IOSExportASWindowViewModel>());
                    break;
            }
        }
        #endregion

        #region Команда - Экспорт карты адресов для данных ВУ
        private ICommand _CmdExportAddressMap;
        /// <summary>
        /// Команда - Экспорт карты адресов для данных ВУ
        /// </summary>
        public ICommand CmdExportAddressMap => _CmdExportAddressMap ??= new RelayCommand(OnCmdExportAddressMapExecuted, CanCmdExportAddressMapExecute);
        private bool CanCmdExportAddressMapExecute(object p) => true;
        private void OnCmdExportAddressMapExecuted(object p)
        {
            bool SuccessfulCompletion;
            if (Config.UseOPC)
                SuccessfulCompletion = VUExportOPCMaps.ASExprot();
            else
                SuccessfulCompletion = IVUExportModbusMaps.ASExprot();

            if (!SuccessfulCompletion)
                if (UserDialog.SendMessage("Внимание!", $"Экспорт выполнен c ошибками.\nСм. лог", ImageType: MessageBoxImage.Warning)) return;

            UserDialog.SendMessage(Title, $"Экпорт выполнен успешно.\nДанные сохранены - {App.Settings.Config.PathExportVU}");
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
            if (UserDialog.SearchControlViewModel(SelectedViewModel.Title) is not IViewModelUserControls _TabItem) return;

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
            UserDialog.OpenMessageWindow();
        }
        #endregion

        #region Команда - Проверить в выбранной вкладке наличие русскиз символов
        private ICommand _CmdCheckAvailability;
        /// <summary>
        /// Команда - Проверить в выбранной вкладке наличие русскиз символов
        /// </summary>
        public ICommand CmdCheckAvailability => _CmdCheckAvailability ??= new RelayCommand(OnCmdCheckAvailabilityExecuted, CanCmdCheckAvailabilityExecute);
        private bool CanCmdCheckAvailabilityExecute(object p) => SelectedTabIndex >= 0;
        private void OnCmdCheckAvailabilityExecuted(object p)
        {
            foreach (var _ViewModelUserControl in ViewModelUserControls)
            {
                if (_ViewModelUserControl.Title == "Сигнализация")
                {
                    var _ViewModel = _ViewModelUserControl as SignalingUserControlViewModel;
                    foreach (var _Param in _ViewModel.Params)
                    {
                        if (CyrillicSymbolServices.CheckAvailability(_Param.Param.Id))
                        {
                            _Param.Param.Id += "\tRus";
                        }
                    }
                }
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
        public override void GetParams<T>(out T _Params) => _Params = (T)ViewModelUserControls;
        #endregion

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
