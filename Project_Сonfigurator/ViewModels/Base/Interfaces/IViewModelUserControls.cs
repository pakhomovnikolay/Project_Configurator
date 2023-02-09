using System.Windows.Controls;

namespace Project_Сonfigurator.ViewModels.Base.Interfaces
{
    public interface IViewModelUserControls
    {
        #region Заголовок вкладки
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        string Title { get; set; }
        #endregion

        #region Описание вкладки
        /// <summary>
        /// Описание вкладки
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Пользовательский интерфейс
        /// <summary>
        /// Пользовательский интерфейс
        /// </summary>
        UserControl UsingUserControl { get; set; }
        #endregion

        #region Состояние активной вкладки
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        bool IsSelected { get; set; }
        #endregion

        #region Состояние необходимости выбора сигнала
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        bool DoSelection { get; set; }
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
