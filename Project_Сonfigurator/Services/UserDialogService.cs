using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.Views.DialogControl;
using Project_Сonfigurator.Views.Windows;
using System;
using System.IO;
using System.Windows;

namespace Project_Сonfigurator.Services
{
    public class UserDialogService : IUserDialogService
    {
        #region Конструктор
        private MainWindow? _MainWindow;
        private SettingWindow? _SettingWindow;
        private MessageWindow? _MessageWindow;
        private readonly IServiceProvider _Services;

        public UserDialogService() { }
        public UserDialogService(IServiceProvider Services) : this() => _Services = Services;
        #endregion

        #region Открытие файла
        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedFile"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public bool OpenFile(string Title, out string SelectedFile, string DefaulPath = null, string Filter = "Все файлы (*.*)|*.*")
        {
            SelectedFile = "";
            var path = string.IsNullOrWhiteSpace(DefaulPath) ? Environment.CurrentDirectory : DefaulPath;
            var dialog = new OpenFileDialog()
            {
                Title = Title,
                Filter = Filter,
                RestoreDirectory = true,
                InitialDirectory = path,
                FileName = SelectedFile
            };

            if (dialog.ShowDialog() != true) { return false; }
            if (!File.Exists(dialog.FileName))
            {
                SendMessage("Выбор пути для сохранения", "Указанный путь не существует.\nВыберите другой путь для сохранения.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            SelectedFile = dialog.FileName;
            return true;
        }
        #endregion

        #region Сохранение файла
        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedFile"></param>
        /// <param name="DefaulFileName"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public bool SaveFile(string Title, out string SelectedPath, string DefaulPath = null, string DefaulFileName = null, string Filter = "Все файлы (*.*)|*.*")
        {
            SelectedPath = "";
            return true;
        }
        #endregion

        #region Сохранение проекта
        /// <summary>
        /// Сохранение проекта
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaulPath"></param>
        /// <param name="DefaulFileName"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public bool SaveProject(string Title, string DefaulPath = null, string DefaulFileName = null, string Filter = "Все файлы (*.*)|*.*")
        {
            var _SettingService = new SettingService();
            if (string.IsNullOrWhiteSpace(App.Settings.Config.PathProject))
            {
                var window = new WindowSelectPath()
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Title = Title,
                    SelectPath = $"По умолчанию - ...\\AppData\\Roaming\\{App.NameApp}"
                };
                if (!window.ShowDialog().Value) return false;

                var SelectPath = window.SelectPath;
                var SelectFileName = window.SelectFileName;
                if (!string.IsNullOrWhiteSpace(SelectFileName))
                    SelectPath = SelectPath.Replace(SelectFileName, "");

                var path = SelectPath == $"По умолчанию - ...\\AppData\\Roaming\\{App.NameApp}" ? App.PathConfig : SelectPath;
                if (!Directory.Exists(path))
                {
                    SendMessage("Выбор пути для сохранения", "Указанный путь не существует.\nВыберите другой путь для сохранения.",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }


                App.Settings.Config.PathProject = string.IsNullOrWhiteSpace(SelectFileName) ? $"{path}\\ProjectData.xml" : $"{path}\\{SelectFileName}.xml";
            }

            _SettingService.Config = App.Settings.Config;
            _SettingService.Save();
            return true;
        }
        #endregion

        #region Выбор пути
        /// <summary>
        /// Выбор пути
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedPath"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public bool SelectFolder(string Title, out string SelectedPath, out string SelectedFile, string DefaulPath = null, string Filter = "Папки (*.Folder*)|*.Folder*")
        {
            SelectedFile = "";
            SelectedPath = "";
            var filename = "Выберите путь";
            var path = string.IsNullOrWhiteSpace(DefaulPath) ? Environment.CurrentDirectory : DefaulPath;
            var dialog = new SaveFileDialog()
            {
                Title = Title,
                Filter = Filter,
                RestoreDirectory = true,
                InitialDirectory = path,
                CheckPathExists = true,
                ValidateNames = false,
                CheckFileExists = true,
                FileName = filename
            };

            if (dialog.ShowDialog() != true) { return false; }
            SelectedFile = dialog.SafeFileName;
            path = dialog.FileName.Replace(SelectedFile, "");
            if (!Directory.Exists(path))
            {
                SendMessage("Выбор пути для сохранения", "Указанный путь не существует.\nВыберите другой путь для сохранения.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            SelectedPath = path;
            return true;

        }
        #endregion

        #region Выбрать файл для импорта ТБ
        /// <summary>
        /// Выбрать файл для импорта ТБ
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedPath"></param>
        /// <param name="DefaulPath"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public bool SelectFile(string Title, out string SelectedPath, string DefaulPath = null, string Filter = "Файлы (*.*)|*.*")
        {
            SelectedPath = "";
            var path = string.IsNullOrWhiteSpace(DefaulPath) ? Environment.CurrentDirectory : DefaulPath;
            var dialog = new OpenFileDialog()
            {
                Title = Title,
                Filter = Filter,
                RestoreDirectory = true,
                InitialDirectory = path.Replace(".xlsx", ""),
                CheckPathExists = true,
                ValidateNames = false,
                CheckFileExists = true,
                FileName = ""
            };

            if (dialog.ShowDialog() != true) { return false; }
            if (!File.Exists(dialog.FileName))
            {
                SendMessage("Выбор пути для сохранения", "Указанный путь не существует.\nВыберите другой путь для сохранения.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            SelectedPath = dialog.FileName;
            return true;
        }
        #endregion

        #region Отправка сообщений пользователю
        /// <summary>
        /// Отправка сообщений пользователю
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Message"></param>
        /// <param name="ButtonType"></param>
        /// <param name="ImageType"></param>
        /// <param name="ResultType"></param>
        /// <returns></returns>
        public bool SendMessage(string Title, string Message,
            MessageBoxButton ButtonType = MessageBoxButton.OK,
            MessageBoxImage ImageType = MessageBoxImage.Information,
            MessageBoxResult ResultType = MessageBoxResult.OK,
            MessageBoxOptions Options = MessageBoxOptions.DefaultDesktopOnly) => MessageBox.Show(Message, Title, ButtonType, ImageType, ResultType, Options) == ResultType;
        #endregion

        #region Удалить файл
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="SelectedFile"></param>
        /// <returns></returns>
        public bool DeleteFile(string SelectedFile)
        {
            if (!File.Exists(SelectedFile))
                return false;

            File.Delete(SelectedFile);
            return true;
        }
        #endregion

        #region Открыть главное окно приложения
        /// <summary>
        /// Открыть главное окно приложения
        /// </summary>
        public void OpenMainWindow()
        {
            if (_MainWindow is { } window) { window.Show(); return; }

            window = _Services.GetRequiredService<MainWindow>();
            window.Closed += (_, _) => _MainWindow = null;
            _MainWindow = window;
            window.Show();
        }
        #endregion

        #region Открыть окно настроек приложения
        /// <summary>
        /// Открыть окно настроек приложения
        /// </summary>
        public void OpenSettingsWindow()
        {
            if (_SettingWindow is { } window) { window.Show(); return; }

            window = _Services.GetRequiredService<SettingWindow>();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closed += (_, _) => _SettingWindow = null;


            _SettingWindow = window;
            window.Show();
        }
        #endregion

        #region Открыть окно сообщений
        /// <summary>
        /// Открыть окно сообщений
        /// </summary>
        public void OpenMessageWindow()
        {
            if (_MessageWindow is { } window) { window.Show(); return; }

            window = _Services.GetRequiredService<MessageWindow>();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closed += (_, _) => _MessageWindow = null;


            _MessageWindow = window;
            window.Show();
        }
        #endregion
    }
}
