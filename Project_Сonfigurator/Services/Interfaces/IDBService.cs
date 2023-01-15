using Project_Сonfigurator.Models;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IDBService
    {
        #region Данные для сохранения
        /// <summary>
        /// Данные для сохранения
        /// </summary>
        ProjectDataToSave DataToSave { get; set; }
        #endregion

        #region Запрос на получение данных проекта
        /// <summary>
        /// Запрос на получение данных проекта
        /// </summary>
        void ProjectDataRequest();
        #endregion

        #region Запрос на запись данных проекта
        /// <summary>
        /// Запрос на запись данных проекта
        /// </summary>
        void RequestToWriteProjectData();
        #endregion

        #region Запрос на запись данных в БД
        /// <summary>
        /// Запрос на запись данных в БД
        /// </summary>
        void RequestToWriteDataToTheDataBase();
        #endregion
    }
}
