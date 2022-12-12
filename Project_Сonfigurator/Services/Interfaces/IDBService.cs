using Project_Сonfigurator.Models;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IDBService
    {
        #region Данные
        /// <summary>
        /// Данные
        /// </summary>
        DBData AppData { get; set; }
        #endregion

        #region Функция записи данных
        /// <summary>
        /// Функция записи данных
        /// </summary>
        /// <returns></returns>
        bool SetData();
        #endregion

        #region Сохранение файла приложения
        /// <summary>
        /// Сохранение файла приложения
        /// </summary>
        /// <returns></returns>
        bool SaveData();
        #endregion

        #region Загрузка файла приложения
        /// <summary>
        /// Загрузка файла приложения
        /// </summary>
        /// <returns></returns>
        DBData LoadData(string SelectedPath = "");
        #endregion

        #region Обновление данных во ViewModels
        /// <summary>
        /// Обновление данных во ViewModels
        /// </summary>
        /// <param name="Item"></param>
        bool RefreshDataViewModel(object Item);
        #endregion

        #region Обнуление данных, при созданиии нового проекта
        /// <summary>
        /// Обнуление данных, при созданиии нового проекта
        /// </summary>
        /// <returns></returns>
        bool ClearDataBase();
        #endregion
    }
}
