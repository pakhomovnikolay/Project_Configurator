using MySqlConnector;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Project_Сonfigurator.Services
{
    public class RequestToDataBaseService : BaseService, IRequestToDataBaseService
    {
        #region Модуль соединения с БД
        /// <summary>
        /// Модуль соединения с БД
        /// </summary>
        MySqlConnection SqlConnection = new();
        #endregion

        #region Модуль команд (запросов) для БД
        /// <summary>
        /// Модуль команд (запросов) для БД
        /// </summary>
        MySqlCommand SqlCommand = new();
        #endregion

        #region Формируем настройки подключения
        /// <summary>
        /// Формируем настройки подключения
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public string SetСonnectionSettins(SettingServerDB Setting, string IPAddress)
        {
            return
                $"SERVER={IPAddress};" +
                $"PORT={Setting.Port};" +
                $"UID={Setting.UserName};" +
                $"PASSWORD={Setting.Password};" +
                $"Connection Timeout = {Setting.TimeoutConnect}";
        }

        /// <summary>
        /// Формируем настройки подключения
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <param name="OpenConnectToDB"></param>
        /// <returns></returns>
        public string SetСonnectionSettins(SettingServerDB Setting, string IPAddress, bool OpenConnectToDB)
        {
            return
                $"SERVER={IPAddress};" +
                $"PORT={Setting.Port};" +
                $"DATABASE={Setting.NameDB};" +
                $"UID={Setting.UserName};" +
                $"PASSWORD={Setting.Password};" +
                $"Connection Timeout = {Setting.TimeoutConnect}";
        }
        #endregion

        #region Формируем запрос подключения к БД
        /// <summary>
        /// Формируем запрос подключения к БД
        /// </summary>
        /// <param name="NameDB"></param>
        /// <returns></returns>
        public string SetQuery(string NameDB)
        {
            return $"CREATE DATABASE IF NOT EXISTS `{NameDB}` COLLATE = 'utf8mb4_general_ci';";
        }
        #endregion

        #region Устанавливаем соединение к БД
        /// <summary>
        /// Устанавливаем соединение к БД
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public bool SetConnection(SettingServerDB Setting, string IPAddress)
        {
            try
            {
                if (SqlConnection.State == ConnectionState.Open)
                    SqlConnection.Close();

                SqlConnection = new(SetСonnectionSettins(Setting, IPAddress));
                SqlConnection.Open();

                SqlCommand = new MySqlCommand(SetQuery(Setting.NameDB), SqlConnection);
                SqlCommand.ExecuteNonQuery();

                SqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Открываем сессию с БД, для формирования данных
        /// <summary>
        /// Открываем сессию с БД, для формирования данных
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public bool OpenConnection(SettingServerDB Setting, string IPAddress)
        {
            try
            {
                SqlConnection = new(SetСonnectionSettins(Setting, IPAddress, true));
                SqlConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Запись данных
        /// <summary>
        /// Запись данных
        /// </summary>
        /// <param name="NameTable"></param>
        /// <param name="TableField"></param>
        /// <param name="Field"></param>
        /// <param name="FieldValue"></param>
        /// <returns></returns>
        public bool SetData(string NameTable, string TableField, string Field, List<string> FieldValue)
        {
            try
            {
                #region Удаляем таблицу
                var Query = $"DROP TABLE IF EXISTS `{NameTable}`";
                var _MySqlCommand = new MySqlCommand(Query, SqlConnection);
                _MySqlCommand.ExecuteNonQuery();
                #endregion

                #region Создаем таблицу
                Query = $"CREATE TABLE `{NameTable}`{TableField}";
                _MySqlCommand = new MySqlCommand(Query, SqlConnection);
                _MySqlCommand.ExecuteNonQuery();
                #endregion

                #region Наполняем таблицу данными
                var QueryPar = "";
                foreach (var item in FieldValue)
                {
                    QueryPar += $"{item}";
                }
                QueryPar = QueryPar.TrimEnd(',');

                Query = $"INSERT INTO {NameTable} {Field} {QueryPar}";

                _MySqlCommand = new MySqlCommand(Query, SqlConnection);
                _MySqlCommand.ExecuteNonQuery();
                #endregion

                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Поля для создания ТБ

        #region Таблица сигналов
        /// <summary>
        /// Поля таблицы для "Таблица сигналов"
        /// </summary>
        public string TableFieldTableSignals
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`USO_NAME` TEXT," +
                    "`RACK_NAME` TEXT," +
                    "`MODULE_TYPE` TEXT," +
                    "`MODULE_NAME` TEXT," +
                    "`CHANNEL_INDEX` TEXT," +
                    "`CHANNEL_ID` TEXT," +
                    "`CHANNEL_DESCRIPTION` TEXT," +
                    "`VAR_NAME_SU` TEXT," +
                    "`BIT` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`VAR_NAME_VU` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Таблица сигналов"
        /// </summary>
        public string FieldTableSignals
        {
            get
            {
                return
                    "(USO_NAME, RACK_NAME, MODULE_TYPE, MODULE_NAME, CHANNEL_INDEX, CHANNEL_ID," +
                    "CHANNEL_DESCRIPTION, VAR_NAME_SU, BIT, ADDRESS, VAR_NAME_VU) VALUES";
            }
        }

        #endregion

        #region Сигналы DI
        /// <summary>
        /// Поля таблицы для "Сигналы DI"
        /// </summary>
        public string TableFieldSignalsDI
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`AREA` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`LINK_VALUE` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сигналы DI"
        /// </summary>
        public string FieldSignalsDI
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
            }
        }
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Поля таблицы для "Сигналы AI"
        /// </summary>
        public string TableFieldSignalsAI
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`AREA` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`LINK_VALUE` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "`INDEX_UMPNA` TEXT," +
                    "`UNIT` TEXT," +
                    "`TYPE_VIBRATION` TEXT," +
                    "`INDEX_PZ` TEXT," +
                    "`TYPE_PI` TEXT," +
                    "`INDEX_BD` TEXT," +
                    "`LEVEL_RPP` TEXT," +
                    "`ADDRESS_UTS` TEXT," +
                    "`CONVERTER_KGS` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сигналы AI"
        /// </summary>
        public string FieldSignalsAI
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG," +
                    "INDEX_UMPNA, UNIT, TYPE_VIBRATION, INDEX_PZ, TYPE_PI, INDEX_BD, LEVEL_RPP, ADDRESS_UTS, CONVERTER_KGS) VALUES";
            }
        }
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Поля таблицы для "Сигналы DO"
        /// </summary>
        public string TableFieldSignalsDO
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`AREA` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`LINK_VALUE` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сигналы DO"
        /// </summary>
        public string FieldSignalsDO
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
            }
        }
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Поля таблицы для "Сигналы AO"
        /// </summary>
        public string TableFieldSignalsAO
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`AREA` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`LINK_VALUE` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сигналы AO"
        /// </summary>
        public string FieldSignalsAO
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
            }
        }
        #endregion

        #region Секции шин
        /// <summary>
        /// Поля таблицы для "Секции шин"
        /// </summary>
        public string TableFieldEC
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Секции шин"
        /// </summary>
        public string FieldEC
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS) VALUES";
            }
        }
        #endregion

        #region DI формируемые
        /// <summary>
        /// Поля таблицы для "DI формируемые"
        /// </summary>
        public string TableFieldUserDI
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`BIT` TEXT," +
                    "`ADDRESS` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "DI формируемые"
        /// </summary>
        public string FieldUserDI
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, BIT, ADDRESS) VALUES";
            }
        }
        #endregion

        #region AI формируемые
        /// <summary>
        /// Поля таблицы для "AI формируемые"
        /// </summary>
        public string TableFieldUserAI
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`ADDRESS` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "AI формируемые"
        /// </summary>
        public string FieldUserAI
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, ADDRESS) VALUES";
            }
        }
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Поля таблицы для "Регистры формируемые"
        /// </summary>
        public string TableFieldUserReg
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`ADDRESS_VU` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Регистры формируемые"
        /// </summary>
        public string FieldUserReg
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, ADDRESS_VU) VALUES";
            }
        }
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Поля таблицы для "Сигналы групп"
        /// </summary>
        public string TableFieldSignalsGroup
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сигналы групп"
        /// </summary>
        public string FieldSignalsGroup
        {
            get
            {
                return
                    "(ID, DESCRIPTION, INV, TYPE_SIGNAL, ADDRESS, ADDRESS_SIG) VALUES";
            }
        }
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Поля таблицы для "Группы сигналов"
        /// </summary>
        public string TableFieldGroupsSignal
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`QTY` TEXT," +
                    "`ADDRESS_START` TEXT," +
                    "`ADDRESS_END` TEXT," +
                    "`ADDRESS_SIG` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Группы сигналов"
        /// </summary>
        public string FieldGroupsSignal
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, QTY, ADDRESS_START, ADDRESS_END, ADDRESS_SIG) VALUES";
            }
        }
        #endregion

        #region Задвижки
        /// <summary>
        /// Поля таблицы для "Задвижки"
        /// </summary>
        public string TableFieldUZD
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`SHORT_NAME` TEXT," +
                    "`INDEX_EC` TEXT," +
                    "`INDEX_GROUP` TEXT," +
                    "`NAME_GROUP` TEXT," +
                    "`DIST` TEXT," +
                    "`DOUBLE_STOP` TEXT," +
                    "`BUR` TEXT," +
                    "`COz` TEXT," +
                    "`CZz` TEXT," +
                    "`EC` TEXT," +
                    "`CHECK_STATE` TEXT," +
                    "`RS_OFF` TEXT," +
                    "`TYPE` TEXT," +
                    "`INDEX_PZ` TEXT," +
                    "`INDEX_BD` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Задвижки"
        /// </summary>
        public string FieldUZD
        {
            get
            {
                return
                    "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_EC, INDEX_GROUP, NAME_GROUP, DIST," +
                    "DOUBLE_STOP, BUR, COz, CZz, EC, CHECK_STATE, RS_OFF, TYPE, INDEX_PZ, INDEX_BD) VALUES";
            }
        }
        #endregion

        #region Вспомсистемы
        /// <summary>
        /// Поля таблицы для "Вспомсистемы"
        /// </summary>
        public string TableFieldUVS
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`SHORT_NAME` TEXT," +
                    "`INDEX_EC` TEXT," +
                    "`INDEX_GROUP` TEXT," +
                    "`NAME_GROUP` TEXT," +
                    "`RESERVABLE` TEXT," +
                    "`TYPE_PRESSURE` TEXT," +
                    "`COz` TEXT," +
                    "`ONE_PRESSURE_SENSOR_GROUP` TEXT," +
                    "`TYPE` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Вспомсистемы"
        /// </summary>
        public string FieldUVS
        {
            get
            {
                return
                    "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_EC, INDEX_GROUP, NAME_GROUP, RESERVABLE," +
                    "TYPE_PRESSURE, COz, ONE_PRESSURE_SENSOR_GROUP, TYPE) VALUES";
            }
        }
        #endregion

        #region Насосные агрегаты
        /// <summary>
        /// Поля таблицы для "Насосные агрегаты"
        /// </summary>
        public string TableFieldUMPNA
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`SHORT_NAME` TEXT," +
                    "`INDEX_PZ` TEXT," +
                    "`INDEX_VZ` TEXT," +
                    "`TYPE` TEXT," +
                    "`INDEX_GROUP_MS` TEXT," +
                    "`USED_MCP` TEXT," +
                    "`USED_KPD` TEXT," +
                    "`QTY_BUTTON_STOP` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Насосные агрегаты"
        /// </summary>
        public string FieldUMPNA
        {
            get
            {
                return
                    "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_PZ, INDEX_VZ, TYPE, INDEX_GROUP_MS, USED_MCP, USED_KPD, QTY_BUTTON_STOP) VALUES";
            }
        }
        #endregion

        #region Карта агрегатных готовностей
        /// <summary>
        /// Поля таблицы для "Карта агрегатных готовностей"
        /// </summary>
        public string TableFieldKGMPNA
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`NO_MASKED` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Карта агрегатных готовностей"
        /// </summary>
        public string FieldKGMPNA
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, NO_MASKED) VALUES";
            }
        }
        #endregion

        #region Карты агрегатных защит
        /// <summary>
        /// Поля таблицы для "Карты агрегатных защит"
        /// </summary>
        public string TableFieldKTPRA
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`STATE` TEXT," +
                    "`NO_MASKED` TEXT," +
                    "`AVR` TEXT," +
                    "`TYPE` TEXT," +
                    "`STOP_TYPE` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Карты агрегатных защит"
        /// </summary>
        public string FieldKTPRA
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE, NO_MASKED, AVR, TYPE, STOP_TYPE) VALUES";
            }
        }
        #endregion

        #region Карта предельных параметров агрегатных защит
        /// <summary>
        /// Поля таблицы для "Карта предельных параметров агрегатных защит"
        /// </summary>
        public string TableFieldKTPRAS
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`STATE` TEXT," +
                    "`TYPE_WARNING` TEXT," +
                    "`TYPE` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Карта предельных параметров агрегатных защит"
        /// </summary>
        public string FieldKTPRAS
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE, TYPE_WARNING, TYPE) VALUES";
            }
        }
        #endregion

        #region Карта общестанционных защит
        /// <summary>
        /// Поля таблицы для "Карта общестанционных защит"
        /// </summary>
        public string TableFieldKTPR
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`STATE_STATION` TEXT," +
                    "`SHOULDER` TEXT," +
                    "`SUB_SHOULDER` TEXT," +
                    "`AUTODEBLOCK` TEXT," +
                    "`NO_MASKED` TEXT," +
                    "`STOP_TYPE_STATION` TEXT," +
                    "`STOP_TYPE_NA` TEXT," +
                    "`TYPE` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Карта общестанционных защит"
        /// </summary>
        public string FieldKTPR
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE_STATION, SHOULDER," +
                    "SUB_SHOULDER, AUTODEBLOCK, NO_MASKED, STOP_TYPE_STATION, STOP_TYPE_NA, TYPE) VALUES";
            }
        }
        #endregion

        #region Карта предельных параметров общестанционных защит
        /// <summary>
        /// Поля таблицы для "Карта предельных параметров общестанционных защит"
        /// </summary>
        public string TableFieldKTPRS
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`TYPE_WARNING` TEXT," +
                    "`TYPE` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Карта предельных параметров общестанционных защит"
        /// </summary>
        public string FieldKTPRS
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, TYPE_WARNING, TYPE) VALUES";
            }
        }
        #endregion

        #region LIST5
        /// <summary>
        /// Поля таблицы для "LIST5"
        /// </summary>
        public string TableFieldSignaling
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`INV` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`VAR_NAME_VU` TEXT," +
                    "`BIT` TEXT," +
                    "`COLOR` TEXT," +
                    "`USO_INDEX` TEXT," +
                    "`TYPE_WARNING` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "LIST5"
        /// </summary>
        public string FieldSignaling
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, VAR_NAME_VU, BIT, COLOR, USO_INDEX, TYPE_WARNING) VALUES";
            }
        }
        #endregion

        #region Табло и сирены
        /// <summary>
        /// Поля таблицы для "Табло и сирены"
        /// </summary>
        public string TableFieldUTS
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`TYPE_SIGNAL` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`LOCK_ENABLE` TEXT," +
                    "`INDEX_PZ` TEXT," +
                    "`APT_OFF` TEXT," +
                    "`INDEX_GROUP` TEXT," +
                    "`TYPE_KCO` TEXT," +
                    "`ADDRESS_KCO` TEXT," +
                    "`TYPE_SIGNAL_SOD` TEXT," +
                    "`ADDRESS_SOD` TEXT," +
                    "`TYPE_SIGNALERR_SOD` TEXT," +
                    "`ADDRESS_ERR_SOD` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Табло и сирены"
        /// </summary>
        public string FieldUTS
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, TYPE_SIGNAL, ADDRESS, LOCK_ENABLE, INDEX_PZ, " +
                    "APT_OFF, INDEX_GROUP, TYPE_KCO, ADDRESS_KCO, TYPE_SIGNAL_SOD, ADDRESS_SOD, TYPE_SIGNALERR_SOD, ADDRESS_ERR_SOD) VALUES";
            }
        }
        #endregion

        #region Уставки типа REAL
        /// <summary>
        /// Поля таблицы для "Уставки типа REAL"
        /// </summary>
        public string TableFieldUstReal
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`VALUE` TEXT," +
                    "`UNIT` TEXT," +
                    "`QTY_SIMBOLS_COMMA` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Уставки типа REAL"
        /// </summary>
        public string FieldUstReal
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, ADDRESS, VALUE, UNIT, QTY_SIMBOLS_COMMA) VALUES";
            }
        }
        #endregion

        #region Временные уставки общие
        /// <summary>
        /// Поля таблицы для "Временные уставки общие"
        /// </summary>
        public string TableFieldUstCommon
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`ADDRESS` TEXT," +
                    "`VALUE` TEXT," +
                    "`UNIT` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Временные уставки общие"
        /// </summary>
        public string FieldUstCommon
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, ADDRESS, VALUE, UNIT) VALUES";
            }
        }
        #endregion

        #region Карта ручного ввода
        /// <summary>
        /// Поля таблицы для "Карта ручного ввода"
        /// </summary>
        public string TableFieldHandMap
        {
            get
            {
                return
                    "(ID, DESCRIPTION, VAR_NAME, ADDRESS, VALUE, UNIT) VALUES";
            }
        }

        /// <summary>
        /// Поля для "Карта ручного ввода"
        /// </summary>
        public string FieldHandMap
        {
            get
            {
                return
                    "(DESCRIPTION, VAR_NAME, BIT) VALUES";
            }
        }
        #endregion

        #region Команды
        /// <summary>
        /// Поля таблицы для "Команды"
        /// </summary>
        public string TableFieldCommand
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`DESCRIPTION` TEXT," +
                    "`VAR_NAME` TEXT," +
                    "`BIT` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Команды"
        /// </summary>
        public string FieldCommand
        {
            get
            {
                return
                    "(DESCRIPTION, VAR_NAME, BIT) VALUES";
            }
        }
        #endregion

        #region Сообщения
        /// <summary>
        /// Поля таблицы для "Сообщения"
        /// </summary>
        public string TableFieldMessages
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`ID` TEXT," +
                    "`NAME_TAB` TEXT," +
                    "`SYSTEM` TEXT," +
                    "`SYSTEM_NAME` TEXT," +
                    "`DESCRIPTION` TEXT," +
                    "`COLOR` TEXT," +
                    "`NEED_ACK` TEXT," +
                    "`PATH_SOUND` TEXT," +
                    "`TYPE_SOUND` TEXT," +
                    "`NEED_PLAY` TEXT," +
                    "`HIDE` TEXT," +
                    "`LEVEL_ACCESS` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Сообщения"
        /// </summary>
        public string FieldMessages
        {
            get
            {
                return
                    "(ID, NAME_TAB, SYSTEM, SYSTEM_NAME, DESCRIPTION, COLOR, NEED_ACK, PATH_SOUND, TYPE_SOUND, NEED_PLAY, HIDE, LEVEL_ACCESS) VALUES";
            }
        }
        #endregion

        #region Системы сообщения
        /// <summary>
        /// Поля таблицы для "Системы сообщения"
        /// </summary>
        public string TableFieldSyatemMessages
        {
            get
            {
                return
                    "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "`SYSTEM` TEXT," +
                    "`DESCRIPTION_MESSAGE` TEXT," +
                    "`DESCRIPTION_SYSTEM` TEXT," +
                    "`NAME_TAB` TEXT," +
                    "PRIMARY KEY(`INDEX`));";
            }
        }

        /// <summary>
        /// Поля для "Системы сообщения"
        /// </summary>
        public string FieldSyatemMessages
        {
            get
            {
                return
                    "(SYSTEM, DESCRIPTION_MESSAGE, DESCRIPTION_SYSTEM, NAME_TAB) VALUES";
            }
        }
        #endregion

        #endregion
    }
}
