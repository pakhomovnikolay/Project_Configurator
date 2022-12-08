using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.Settings;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ISettingService
    {
        #region Параметры настроек
        /// <summary>
        /// Параметры настроек
        /// </summary>
        SettingApp Config { get; set; }
        #endregion

        #region Данные
        /// <summary>
        /// Данные
        /// </summary>
        DBData AppData { get; set; }
        #endregion

        #region Сохранение настроек
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// <returns></returns>
        bool Save();
        #endregion

        #region Загрузка настроек
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        /// <returns></returns>
        SettingApp Load();
        #endregion

        #region Сохранение данные приложения
        /// <summary>
        /// Сохранение данные приложения
        /// </summary>
        /// <returns></returns>
        bool SaveData();
        #endregion

        #region Загрузка данных приложения
        /// <summary>
        /// Загрузка данных приложения
        /// </summary>
        /// <returns></returns>
        DBData LoadData(string SelectedPath = "");
        #endregion
    }
}