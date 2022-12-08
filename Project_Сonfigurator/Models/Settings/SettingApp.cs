using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingApp : ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        public List<Vendor> Vendors { get; set; } = new();
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        public UZDParam UZD { get; set; } = new();
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        public UVSParam UVS { get; set; } = new();
        #endregion

        #region Настройки агрегатов
        /// <summary>
        /// Настройки агрегатов
        /// </summary>
        public UMPNAParam UMPNA { get; set; } = new();
        #endregion

        #region Настройки импорта ТБ
        /// <summary>
        /// Настройки импорта ТБ
        /// </summary>
        public SettingImport Import { get; set; } = new();
        #endregion
    }
}
