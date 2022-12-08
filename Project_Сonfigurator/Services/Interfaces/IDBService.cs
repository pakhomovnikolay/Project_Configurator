using Project_Сonfigurator.Models;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IDBService
    {
        #region Функция получения данных
        /// <summary>
        /// Функция получения данных
        /// </summary>
        /// <returns></returns>
        DBData GetData();
        #endregion
    }
}
