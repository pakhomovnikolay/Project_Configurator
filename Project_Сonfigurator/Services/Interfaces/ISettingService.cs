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
    }
}