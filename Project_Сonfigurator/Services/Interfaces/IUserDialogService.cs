using System.Windows;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IUserDialogService
    {
        #region Открытие файла
        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedFile"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        bool OpenFile(string Title, out string SelectedFile, string DefaulPath = null, string Filter = "Все файлы (*.*)|*.*");
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
        bool SaveFile(string Title, out string SelectedFile, string DefaulPath = null, string DefaulFileName = null, string Filter = "Все файлы (*.*)|*.*");
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
        bool SaveProject(string Title, string DefaulPath = null, string DefaulFileName = null, string Filter = "Все файлы (*.*)|*.*");
        #endregion

        #region Выбор пути
        /// <summary>
        /// Выбор пути
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="SelectedPath"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        bool SelectFolder(string Title, out string SelectedPath, string DefaulPath = null, string Filter = "Папки (*.Folder*)|*.Folder*");
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
        bool SelectFile(string Title, out string SelectedPath, string DefaulPath = null, string Filter = "Книга Excel (*.xlsx*)|*.xlsx*");
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
        bool SendMessage(string Title, string Message,
            MessageBoxButton ButtonType = MessageBoxButton.OK,
            MessageBoxImage ImageType = MessageBoxImage.Information,
            MessageBoxResult ResultType = MessageBoxResult.OK,
            MessageBoxOptions Options = MessageBoxOptions.DefaultDesktopOnly);
        #endregion
    }
}
