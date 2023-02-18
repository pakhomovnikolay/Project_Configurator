using Project_Сonfigurator.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        void GetParams<T>(out T _Params);
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        ObservableCollection<T> GetParams<T>();
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        void SetParams<T>(T _Params);
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        void SetParams<T>(ObservableCollection<T> _Params);
        #endregion

        #region Коллекция параметров
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        ICollectionView ParamsDataView { get; }
        #endregion

        #region Коллекция дополнительных параметров
        /// <summary>
        /// Коллекция дополнительных параметров
        /// </summary>
        ICollectionView SubParamsDataView { get; }
        #endregion

        #region Сервис взаимодействия с пользователем
        /// <summary>
        /// Сервис взаимодействия с пользователем
        /// </summary>
        IUserDialogService UserDialog { get; set; }
        #endregion

        #region Сервис логирования
        /// <summary>
        /// Сервис логирования
        /// </summary>
        ILogSerivece LogSeriveces { get; set; }
        #endregion
    }
}
