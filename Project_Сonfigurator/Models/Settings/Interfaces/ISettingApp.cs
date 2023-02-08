using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        ObservableCollection<Vendor> Vendors { get; set; }
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
        ObservableCollection<SettingServerDB> ServerDB { get; set; }
        #endregion

        #region Путь хранения проекта
        /// <summary>
        /// Путь хранения проекта
        /// </summary>
        string PathProject { get; set; }
        #endregion

        #region Путь экспорта данных для ВУ
        /// <summary>
        /// Путь экспорта данных для ВУ
        /// </summary>
        string PathExportVU { get; set; }
        #endregion

        #region Наименование проекта
        /// <summary>
        /// Наименование проекта
        /// </summary>
        string NameProject { get; set; }
        #endregion

        #region Тип системы
        /// <summary>
        /// Тип системы
        /// </summary>
        string TypeSystem { get; set; }
        #endregion

        #region Адресное пространство
        /// <summary>
        /// Адресное пространство
        /// </summary>
        string NamespaceOPC { get; set; }
        #endregion

        #region Идентификатор узла
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        string NodeID { get; set; }
        #endregion

        #region Для обмена использовать OPC
        /// <summary>
        /// Для обмена использовать OPC
        /// </summary>
        bool UseOPC { get; set; }
        #endregion

        #region Карта агрегатных готовностей по умолчанию
        /// <summary>
        /// Карта агрегатных готовностей по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapKGMPNA { get; set; }
        #endregion

        #region Карта общестанционных защит по умолчанию
        /// <summary>
        /// Карта общестанционных защит по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPR { get; set; }
        #endregion

        #region Карта агрегатных защит по умолчанию
        /// <summary>
        /// Карта агрегатных защит по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRA { get; set; }
        #endregion

        #region Карта предельных параметров агрегатных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров агрегатных защит по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRAS { get; set; }
        #endregion

        #region Карта предельных параметров общестанционных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров общестанционных защит по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRS { get; set; }
        #endregion

        #region Карта общесистемных параметров по умолчанию
        /// <summary>
        /// Карта общесистемных параметров по умолчанию
        /// </summary>
        ObservableCollection<SettingDefualtDefenseMap> DefualtMapSignaling { get; set; }
        #endregion

        #region Список ПЛК для диагностики
        /// <summary>
        /// Список ПЛК для диагностики
        /// </summary>
        ObservableCollection<BaseText> PLC_List { get; set; }
        #endregion

        #region Modbus TCP slave - Input Registers
        /// <summary>
        /// Modbus TCP slave - Input Registers
        /// </summary>
        ObservableCollection<BaseAddressMap> ModbusTCP_IR { get; set; }
        #endregion

        #region Modbus TCP slave - Holding Registers
        /// <summary>
        /// Modbus TCP slave - Holding Registers
        /// </summary>
        ObservableCollection<BaseAddressMap> ModbusTCP_HR { get; set; } 
        #endregion
    }
}
