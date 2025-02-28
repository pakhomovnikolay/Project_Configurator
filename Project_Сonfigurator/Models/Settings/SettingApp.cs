﻿using Project_Сonfigurator.Models.Settings.Interfaces;
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

        #region Путь экспорта данных для ВУ
        /// <summary>
        /// Путь экспорта данных для ВУ
        /// </summary>
        public string PathExportVU { get; set; }
        #endregion

        #region Наименование проекта
        /// <summary>
        /// Наименование проекта
        /// </summary>
        public string NameProject { get; set; }
        #endregion

        #region Тип системы
        /// <summary>
        /// Тип системы
        /// </summary>
        public string TypeSystem { get; set; }
        #endregion

        #region Размер буфера сообщений
        /// <summary>
        /// Размер буфера сообщений
        /// </summary>
        public int BufferSize { get; set; } = 0;
        #endregion

        #region Адресное пространство
        /// <summary>
        /// Адресное пространство
        /// </summary>
        public string NamespaceOPC { get; set; }
        #endregion

        #region Идентификатор узла
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        public string NodeID { get; set; }
        #endregion

        #region Для обмена использовать OPC
        /// <summary>
        /// Для обмена использовать OPC
        /// </summary>
        public bool UseOPC { get; set; }
        #endregion

        #region Карта агрегатных готовностей по умолчанию
        /// <summary>
        /// Карта агрегатных готовностей по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKGMPNA { get; set; } = new();
        #endregion

        #region Карта общестанционных защит по умолчанию
        /// <summary>
        /// Карта общестанционных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPR { get; set; } = new();
        #endregion

        #region Карта агрегатных защит по умолчанию
        /// <summary>
        /// Карта агрегатных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRA { get; set; } = new();
        #endregion

        #region Карта предельных параметров агрегатных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров агрегатных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRAS { get; set; } = new();
        #endregion

        #region Карта предельных параметров общестанционных защит по умолчанию
        /// <summary>
        /// Карта предельных параметров общестанционных защит по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapKTPRS { get; set; } = new();
        #endregion

        #region Карта общесистемных параметров по умолчанию
        /// <summary>
        /// Карта общесистемных параметров по умолчанию
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefualtMapSignaling { get; set; } = new();
        #endregion

        #region Список ПЛК для диагностики
        /// <summary>
        /// Список ПЛК для диагностики
        /// </summary>
        public ObservableCollection<BaseText> PLC_List { get; set; } = new();
        #endregion

        #region Modbus TCP slave - Input Registers
        /// <summary>
        /// Modbus TCP slave - Input Registers
        /// </summary>
        public ObservableCollection<BaseAddressMap> ModbusTCP_IR { get; set; } = new();
        #endregion

        #region Modbus TCP slave - Holding Registers
        /// <summary>
        /// Modbus TCP slave - Holding Registers
        /// </summary>
        public ObservableCollection<BaseAddressMap> ModbusTCP_HR { get; set; } = new();
        #endregion
    }
}