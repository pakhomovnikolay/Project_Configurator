using Microsoft.Win32;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.Views.DialogControl;
using System;
using System.IO;
using System.Windows;

namespace Project_Сonfigurator.Services
{
    public class UserDialogService : IUserDialogService
    {
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
            if (string.IsNullOrWhiteSpace(Program.Settings.Config.PathProject))
            {
                var window = new WindowSelectPath()
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Title = Title,
                    SelectPath = $"По умолчанию - ...\\AppData\\Roaming\\{App.NameApp}"
                };
                if (!window.ShowDialog().Value) return false;
                var path = window.SelectPath == $"По умолчанию - ...\\AppData\\Roaming\\{App.NameApp}" ? Program.PathConfig : window.SelectPath;
                if (!Directory.Exists(path))
                {
                    SendMessage("Выбор пути для сохранения", "Указанный путь не существует.\nВыберите другой путь для сохранения.",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                Program.Settings.Config.PathProject = $"{path}\\ProjectData.xml";
            }

            _SettingService.Config = Program.Settings.Config;
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
        public bool SelectFolder(string Title, out string SelectedPath, string DefaulPath = null, string Filter = "Папки (*.Folder*)|*.Folder*")
        {
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
            path = dialog.FileName.Replace(filename, "");
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
        public bool SelectFile(string Title, out string SelectedPath, string DefaulPath = null, string Filter = "Книга Excel (*.xlsx*)|*.xlsx*")
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
    }
}
