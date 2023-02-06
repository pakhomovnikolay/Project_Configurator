using MySqlConnector;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services;
using System;
using System.Collections.Generic;
using System.Data;

namespace Project_Сonfigurator.Infrastructures.RequestToDataBase
{
    public static class Request
    {
        static MySqlConnection SqlConnection = new();
        static MySqlCommand SqlCommand;
        static readonly LogSerivece Logger = new();

        #region Формируем настройки подключения
        /// <summary>
        /// Формируем настройки подключения
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="OpenConnectToDB"></param>
        /// <returns></returns>
        public static string SetСonnectionSettins(SettingServerDB Setting, string IPAddress, bool OpenConnectToDB = false)
        {
            string СonnectionSettins =
                $"SERVER={IPAddress};" +
                    $"PORT={Setting.Port};" +
                    $"UID={Setting.UserName};" +
                    $"PASSWORD={Setting.Password};" +
                    $"Connection Timeout = {Setting.TimeoutConnect}";

            if (OpenConnectToDB)
                СonnectionSettins =
                    $"SERVER={IPAddress};" +
                    $"PORT={Setting.Port};" +
                    $"DATABASE={Setting.NameDB};" +
                    $"UID={Setting.UserName};" +
                    $"PASSWORD={Setting.Password};" +
                    $"Connection Timeout = {Setting.TimeoutConnect}";

            return СonnectionSettins;
        }
        #endregion

        #region Формируем запрос подключения к БД
        /// <summary>
        /// Формируем запрос подключения к БД
        /// </summary>
        /// <param name="NameDB"></param>
        /// <returns></returns>
        public static string SetQuery(string NameDB)
        {
            string Query = $"CREATE DATABASE IF NOT EXISTS `{NameDB}` COLLATE = 'utf8mb4_general_ci';";
            return Query;
        }
        #endregion

        #region Устанавливаем соединение к БД
        /// <summary>
        /// Устанавливаем соединение к БД
        /// </summary>
        /// <param name="Setting"></param>
        /// <returns></returns>
        public static bool SetConnection(SettingServerDB Setting, string IPAddress)
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
        /// <returns></returns>
        public static bool OpenConnection(SettingServerDB Setting, string IPAddress)
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
        /// <param name="Field"></param>
        /// <param name="FieldValue"></param>
        /// <returns></returns>
        public static bool SetData(string NameTable, string TableField, string Field, List<string> FieldValue)
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
        public const string TableFieldTableSignals =
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

        public const string FieldTableSignals =
            "(USO_NAME, RACK_NAME, MODULE_TYPE, MODULE_NAME, CHANNEL_INDEX, CHANNEL_ID," +
            "CHANNEL_DESCRIPTION, VAR_NAME_SU, BIT, ADDRESS, VAR_NAME_VU) VALUES";
        #endregion

        #region Сигналы DI
        public const string TableFieldSignalsDI =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`AREA` TEXT," +
            "`ADDRESS` TEXT," +
            "`LINK_VALUE` TEXT," +
            "`ADDRESS_SIG` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSignalsDI =
            "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
        #endregion

        #region Сигналы AI
        public const string TableFieldSignalsAI =
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

        public const string FieldSignalsAI =
            "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG," +
            "INDEX_UMPNA, UNIT, TYPE_VIBRATION, INDEX_PZ, TYPE_PI, INDEX_BD, LEVEL_RPP, ADDRESS_UTS, CONVERTER_KGS) VALUES";
        #endregion

        #region Сигналы DO
        public const string TableFieldSignalsDO =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`AREA` TEXT," +
            "`ADDRESS` TEXT," +
            "`LINK_VALUE` TEXT," +
            "`ADDRESS_SIG` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSignalsDO =
            "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
        #endregion

        #region Сигналы AO
        public const string TableFieldSignalsAO =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`AREA` TEXT," +
            "`ADDRESS` TEXT," +
            "`LINK_VALUE` TEXT," +
            "`ADDRESS_SIG` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSignalsAO =
            "(ID, DESCRIPTION, VAR_NAME, AREA, ADDRESS, LINK_VALUE, ADDRESS_SIG) VALUES";
        #endregion

        #region Секции шин
        public const string TableFieldEC =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`INV` TEXT," +
            "`TYPE_SIGNAL` TEXT," +
            "`ADDRESS` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldEC =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS) VALUES";
        #endregion

        #region DI формируемые
        public const string TableFieldUserDI =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`BIT` TEXT," +
            "`ADDRESS` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldUserDI =
            "(ID, DESCRIPTION, VAR_NAME, BIT, ADDRESS) VALUES";
        #endregion

        #region AI формируемые
        public const string TableFieldUserAI =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`ADDRESS` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldUserAI =
            "(ID, DESCRIPTION, VAR_NAME, ADDRESS) VALUES";
        #endregion

        #region Регистры формируемые
        public const string TableFieldUserReg =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`ADDRESS_VU` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldUserReg =
            "(ID, DESCRIPTION, VAR_NAME, ADDRESS_VU) VALUES";
        #endregion

        #region Сигналы групп
        public const string TableFieldSignalsGroup =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`INV` TEXT," +
            "`TYPE_SIGNAL` TEXT," +
            "`ADDRESS` TEXT," +
            "`ADDRESS_SIG` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSignalsGroup =
            "(ID, DESCRIPTION, INV, TYPE_SIGNAL, ADDRESS, ADDRESS_SIG) VALUES";
        #endregion

        #region Группы сигналов
        public const string TableFieldGroupsSignal =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`QTY` TEXT," +
            "`ADDRESS_START` TEXT," +
            "`ADDRESS_END` TEXT," +
            "`ADDRESS_SIG` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldGroupsSignal =
            "(ID, DESCRIPTION, VAR_NAME, QTY, ADDRESS_START, ADDRESS_END, ADDRESS_SIG) VALUES";
        #endregion

        #region Задвижки
        public const string TableFieldUZD =
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

        public const string FieldUZD =
            "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_EC, INDEX_GROUP, NAME_GROUP, DIST," +
            "DOUBLE_STOP, BUR, COz, CZz, EC, CHECK_STATE, RS_OFF, TYPE, INDEX_PZ, INDEX_BD) VALUES";
        #endregion

        #region Вспомсистемы
        public const string TableFieldUVS =
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

        public const string FieldUVS =
            "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_EC, INDEX_GROUP, NAME_GROUP, RESERVABLE," +
            "TYPE_PRESSURE, COz, ONE_PRESSURE_SENSOR_GROUP, TYPE) VALUES";
        #endregion

        #region Насосные агрегаты
        public const string TableFieldUMPNA =
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

        public const string FieldUMPNA =
            "(DESCRIPTION, VAR_NAME, SHORT_NAME, INDEX_PZ, INDEX_VZ, TYPE, INDEX_GROUP_MS, USED_MCP, USED_KPD, QTY_BUTTON_STOP) VALUES";
        #endregion

        #region Карта агрегатных готовностей
        public const string TableFieldKGMPNA =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`INV` TEXT," +
            "`TYPE_SIGNAL` TEXT," +
            "`ADDRESS` TEXT," +
            "`NO_MASKED` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldKGMPNA =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, NO_MASKED) VALUES";
        #endregion

        #region Карты агрегатных защит
        public const string TableFieldKTPRA =
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

        public const string FieldKTPRA =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE, NO_MASKED, AVR, TYPE, STOP_TYPE) VALUES";
        #endregion

        #region Карта предельных параметров агрегатных защит
        public const string TableFieldKTPRAS =
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

        public const string FieldKTPRAS =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE, TYPE_WARNING, TYPE) VALUES";
        #endregion

        #region Карта общестанционных защит
        public const string TableFieldKTPR =
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

        public const string FieldKTPR =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, STATE_STATION, SHOULDER," +
            "SUB_SHOULDER, AUTODEBLOCK, NO_MASKED, STOP_TYPE_STATION, STOP_TYPE_NA, TYPE) VALUES";
        #endregion

        #region Карта предельных параметров общестанционных защит
        public const string TableFieldKTPRS =
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

        public const string FieldKTPRS =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, TYPE_WARNING, TYPE) VALUES";
        #endregion

        #region LIST5
        public const string TableFieldSignaling =
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
            "`BTYPE_WARNINGIT` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSignaling =
            "(ID, DESCRIPTION, VAR_NAME, INV, TYPE_SIGNAL, ADDRESS, VAR_NAME_VU, BIT, COLOR, USO_INDEX, TYPE_WARNING) VALUES";
        #endregion

        #region Табло и сирены
        public const string TableFieldUTS =
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

        public const string FieldUTS =
            "(ID, DESCRIPTION, VAR_NAME, TYPE_SIGNAL, ADDRESS, LOCK_ENABLE, INDEX_PZ, " +
            "APT_OFF, INDEX_GROUP, TYPE_KCO, ADDRESS_KCO, TYPE_SIGNAL_SOD, ADDRESS_SOD, TYPE_SIGNALERR_SOD, ADDRESS_ERR_SOD) VALUES";
        #endregion

        #region Устаки типа REAL
        public const string TableFieldUstReal =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`ADDRESS` TEXT," +
            "`VALUE` TEXT," +
            "`UNIT` TEXT," +
            "`QTY_SIMBOLS_COMMA` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldUstReal =
            "(ID, DESCRIPTION, VAR_NAME, ADDRESS, VALUE, UNIT, QTY_SIMBOLS_COMMA) VALUES";
        #endregion

        #region Временные уставки общие
        public const string TableFieldUstCommon =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`ADDRESS` TEXT," +
            "`VALUE` TEXT," +
            "`UNIT` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldUstCommon =
            "(ID, DESCRIPTION, VAR_NAME, ADDRESS, VALUE, UNIT) VALUES";
        #endregion

        #region Карта ручного ввода
        public const string TableFieldHandMap =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`DESCRIPTION` TEXT," +
            "`VAR_NAME` TEXT," +
            "`BIT` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldHandMap =
            "(DESCRIPTION, VAR_NAME, BIT) VALUES";
        #endregion

        #region Команды
        public const string TableFieldCommand =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`VAR_NAME` TEXT," +
            "`ID` TEXT," +
            "`DESCRIPTION` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldCommand =
            "(VAR_NAME, ID, DESCRIPTION) VALUES";
        #endregion

        #region Сообщения
        public const string TableFieldMessages =
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

        public const string FieldMessages =
            "(ID, NAME_TAB, SYSTEM, SYSTEM_NAME, DESCRIPTION, COLOR, NEED_ACK, PATH_SOUND, TYPE_SOUND, NEED_PLAY, HIDE, LEVEL_ACCESS) VALUES";
        #endregion

        #region Системы сообщения
        public const string TableFieldSyatemMessages =
            "(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
            "`SYSTEM` TEXT," +
            "`DESCRIPTION_MESSAGE` TEXT," +
            "`DESCRIPTION_SYSTEM` TEXT," +
            "`NAME_TAB` TEXT," +
            "PRIMARY KEY(`INDEX`));";

        public const string FieldSyatemMessages =
            "(SYSTEM, DESCRIPTION_MESSAGE, DESCRIPTION_SYSTEM, NAME_TAB) VALUES";
        #endregion

        #endregion
    }
}
