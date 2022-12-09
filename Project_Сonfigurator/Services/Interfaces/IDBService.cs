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

        #region Обновление данных во ViewModels
        /// <summary>
        /// Обновление данных во ViewModels
        /// </summary>
        /// <param name="Item"></param>
        bool RefreshDataViewModel(object Item);
        #endregion
    }
}
