using Project_Сonfigurator.Models.Settings;
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

        #region Настройки подклюяения к БД
        /// <summary>
        /// Настройки подклюяения к БД
        /// </summary>
        List<SettingServerDB> ServerDB { get; set; }
        #endregion

        #region Путь хранения проекта
        /// <summary>
        /// Путь хранения проекта
        /// </summary>
        string PathProject { get; set; }
        #endregion

        #region Путь хранения настроек проекта
        /// <summary>
        /// Путь хранения настроек проекта
        /// </summary>
        string PathConfig { get; set; }
        #endregion

        #region Карта агрегатных готовностей по умолчанию
        /// <summary>
        /// Карта агрегатных готовностей по умолчанию
        /// </summary>
        List<SettingDefualtDefenseMap> DefualtMapKGMPNA { get; set; }
        #endregion

        #region Карта общестанционных защит по умолчанию
        /// <summary>
        /// Карта общестанционных защит по умолчанию
        /// </summary>
        List<SettingDefualtDefenseMap> DefualtMapKTPR { get; set; }
        #endregion

        #region Карта агрегатных защит по умолчанию
        /// <summary>
        /// Карта агрегатных защит по умолчанию
        /// </summary>
        List<SettingDefualtDefenseMap> DefualtMapKTPRA { get; set; }
        #endregion

        #region Карта предельных параметров агрегатных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров агрегатных защит по умолчанию
        /// </summary>
        List <SettingDefualtDefenseMap> DefualtMapKTPRAS { get; set; }
        #endregion

        #region Карта предельных параметров общестанционных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров общестанционных защит по умолчанию
        /// </summary>
        List <SettingDefualtDefenseMap> DefualtMapKTPRS { get; set; }
        #endregion

        #region Карта общесистемных параметров по умолчанию
        /// <summary>
        /// Карта общесистемных параметров по умолчанию
        /// </summary>
        List <SettingDefualtDefenseMap> DefualtMapSignaling { get; set; }
        #endregion
    }
}
