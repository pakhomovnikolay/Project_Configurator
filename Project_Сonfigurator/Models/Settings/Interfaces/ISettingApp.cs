using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        List<Vendor> Vendors { get; set; }
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        UZDParam UZD { get; set; }
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        UVSParam UVS { get; set; }
        #endregion

        #region Настройки агрегатов
        /// <summary>
        /// Настройки агрегатов
        /// </summary>
        UMPNAParam UMPNA { get; set; }
        #endregion

        #region Настройки импорта ТБ
        /// <summary>
        /// Настройки импорта ТБ
        /// </summary>
        SettingImport Import { get; set; }
        #endregion
    }
}
