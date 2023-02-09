using System.Windows;

namespace Project_Сonfigurator.ViewModels.Base.Interfaces
{
    public interface IViewModel
    {
        #region Заголовок окна
        /// <summary>
        /// Заголовок окна
        /// </summary>
        string Title { get; set; }
        #endregion

        #region Высота окна
        /// <summary>
        /// Высота окна
        /// </summary>
        int WindowHeight { get; set; }
        #endregion

        #region Ширина окна
        /// <summary>
        /// Ширина окна
        /// </summary>
        int WindowWidth { get; set; }
        #endregion

        #region Режим изменения размеров окна
        /// <summary>
        /// Режим изменения размеров окна
        /// </summary>
        ResizeMode WindowResizeMode { get; set; }
        #endregion

        #region Текущее состояние окна
        /// <summary>
        /// Текущее состояние окна
        /// </summary>
        WindowState WindowWindowState { get; set; }
        #endregion

        #region Стиль кнопки изменения состояния окна
        /// <summary>
        /// Стиль кнопки изменения состояния окна
        /// </summary>
        Style ButtonChangeStateWindowStyle { get; set; }
        #endregion

        #region Установка стиля кнопки состояния окна
        /// <summary>
        /// Установка стиля кнопки состояния окна
        /// </summary>
        /// <param name="State"></param>
        void SetButtonStyleStateWindow(WindowState State);
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        object GetParam();
        #endregion
    }
}
