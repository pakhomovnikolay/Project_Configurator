using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingApp : ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        public ObservableCollection<Vendor> Vendors { get; set; } = new();
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

        #region Настройки подклюяения к БД
        /// <summary>
        /// Настройки подклюяения к БД
        /// </summary>
        public ObservableCollection<SettingServerDB> ServerDB { get; set; } = new();
        #endregion

        #region Путь хранения проекта
        /// <summary>
        /// Путь хранения проекта
        /// </summary>
        public string PathProject { get; set; }
        #endregion

        #region Путь хранения настроек проекта
        /// <summary>
        /// Путь хранения настроек проекта
        /// </summary>
        public string PathConfig { get; set; }
        #endregion

        #region Карта агрегатных готовностей по умолчанию
        /// <summary>
        /// Карта агрегатных готовностей по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKGMPNA { get; set; }
        #endregion

        #region Карта общестанционных защит по умолчанию
        /// <summary>
        /// Карта общестанционных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPR { get; set; }
        #endregion

        #region Карта агрегатных защит по умолчанию
        /// <summary>
        /// Карта агрегатных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRA { get; set; }
        #endregion

        #region Карта предельных параметров агрегатных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров агрегатных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRAS { get; set; }
        #endregion

        #region Карта предельных параметров общестанционных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров общестанционных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRS { get; set; }
        #endregion

        #region Карта общесистемных параметров по умолчанию
        /// <summary>
        /// Карта общесистемных параметров по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapSignaling { get; set; }
        #endregion
    }
}