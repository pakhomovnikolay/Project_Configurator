using Project_Сonfigurator.Models.Settings;
using System.Collections.Generic;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IRequestToDataBaseService
    {
        #region Формируем настройки подключения
        /// <summary>
        /// Формируем настройки подключения
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        string SetСonnectionSettins(SettingServerDB Setting, string IPAddress);

        /// <summary>
        /// Формируем настройки подключения
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <param name="OpenConnectToDB"></param>
        /// <returns></returns>
        string SetСonnectionSettins(SettingServerDB Setting, string IPAddress, bool OpenConnectToDB);
        #endregion

        #region Формируем запрос подключения к БД
        /// <summary>
        /// Формируем запрос подключения к БД
        /// </summary>
        /// <param name="NameDB"></param>
        /// <returns></returns>
        string SetQuery(string NameDB);
        #endregion

        #region Устанавливаем соединение к БД
        /// <summary>
        /// Устанавливаем соединение к БД
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        bool SetConnection(SettingServerDB Setting, string IPAddress);
        #endregion

        #region Открываем сессию с БД, для формирования данных
        /// <summary>
        /// Открываем сессию с БД, для формирования данных
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        bool OpenConnection(SettingServerDB Setting, string IPAddress);
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
        bool SetData(string NameTable, string TableField, string Field, List<string> FieldValue);
        #endregion

        #region Поля для создания ТБ

        #region Таблица сигналов
        /// <summary>
        /// Поля таблицы для "Таблица сигналов"
        /// </summary>
        string TableFieldTableSignals { get; }

        /// <summary>
        /// Поля для "Таблица сигналов"
        /// </summary>
        string FieldTableSignals { get; }
        #endregion

        #region Сигналы DI
        /// <summary>
        /// Поля таблицы для "Сигналы DI"
        /// </summary>
        string TableFieldSignalsDI { get; }

        /// <summary>
        /// Поля для "Сигналы DI"
        /// </summary>
        string FieldSignalsDI { get; }
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Поля таблицы для "Сигналы AI"
        /// </summary>
        string TableFieldSignalsAI { get; }

        /// <summary>
        /// Поля для "Сигналы AI"
        /// </summary>
        string FieldSignalsAI { get; }
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Поля таблицы для "Сигналы DO"
        /// </summary>
        string TableFieldSignalsDO { get; }

        /// <summary>
        /// Поля для "Сигналы DO"
        /// </summary>
        string FieldSignalsDO { get; }
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Поля таблицы для "Сигналы AO"
        /// </summary>
        string TableFieldSignalsAO { get; }

        /// <summary>
        /// Поля для "Сигналы AO"
        /// </summary>
        string FieldSignalsAO { get; }
        #endregion

        #region Секции шин
        /// <summary>
        /// Поля таблицы для "Секции шин"
        /// </summary>
        string TableFieldEC { get; }

        /// <summary>
        /// Поля для "Секции шин"
        /// </summary>
        string FieldEC { get; }
        #endregion

        #region DI формируемые
        /// <summary>
        /// Поля таблицы для "DI формируемые"
        /// </summary>
        string TableFieldUserDI { get; }

        /// <summary>
        /// Поля для "DI формируемые"
        /// </summary>
        string FieldUserDI { get; }
        #endregion

        #region AI формируемые
        /// <summary>
        /// Поля таблицы для "AI формируемые"
        /// </summary>
        string TableFieldUserAI { get; }

        /// <summary>
        /// Поля для "AI формируемые"
        /// </summary>
        string FieldUserAI { get; }
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Поля таблицы для "Регистры формируемые"
        /// </summary>
        string TableFieldUserReg { get; }

        /// <summary>
        /// Поля для "Регистры формируемые"
        /// </summary>
        string FieldUserReg { get; }
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Поля таблицы для "Сигналы групп"
        /// </summary>
        string TableFieldSignalsGroup { get; }

        /// <summary>
        /// Поля для "Сигналы групп"
        /// </summary>
        string FieldSignalsGroup { get; }
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Поля таблицы для "Группы сигналов"
        /// </summary>
        string TableFieldGroupsSignal { get; }

        /// <summary>
        /// Поля для "Группы сигналов"
        /// </summary>
        string FieldGroupsSignal { get; }
        #endregion

        #region Задвижки
        /// <summary>
        /// Поля таблицы для "Задвижки"
        /// </summary>
        string TableFieldUZD { get; }

        /// <summary>
        /// Поля для "Задвижки"
        /// </summary>
        string FieldUZD { get; }
        #endregion

        #region Вспомсистемы
        /// <summary>
        /// Поля таблицы для "Вспомсистемы"
        /// </summary>
        string TableFieldUVS { get; }

        /// <summary>
        /// Поля для "Вспомсистемы"
        /// </summary>
        string FieldUVS { get; }
        #endregion

        #region Насосные агрегаты
        /// <summary>
        /// Поля таблицы для "Насосные агрегаты"
        /// </summary>
        string TableFieldUMPNA { get; }

        /// <summary>
        /// Поля для "Насосные агрегаты"
        /// </summary>
        string FieldUMPNA { get; }
        #endregion

        #region Карта агрегатных готовностей
        /// <summary>
        /// Поля таблицы для "Карта агрегатных готовностей"
        /// </summary>
        string TableFieldKGMPNA { get; }

        /// <summary>
        /// Поля для "Карта агрегатных готовностей"
        /// </summary>
        string FieldKGMPNA { get; }
        #endregion

        #region Карты агрегатных защит
        /// <summary>
        /// Поля таблицы для "Карты агрегатных защит"
        /// </summary>
        string TableFieldKTPRA { get; }

        /// <summary>
        /// Поля для "Карты агрегатных защит"
        /// </summary>
        string FieldKTPRA { get; }
        #endregion

        #region Карта предельных параметров агрегатных защит
        /// <summary>
        /// Поля таблицы для "Карта предельных параметров агрегатных защит"
        /// </summary>
        string TableFieldKTPRAS { get; }

        /// <summary>
        /// Поля для "Карта предельных параметров агрегатных защит"
        /// </summary>
        string FieldKTPRAS { get; }
        #endregion

        #region Карта общестанционных защит
        /// <summary>
        /// Поля таблицы для "Карта общестанционных защит"
        /// </summary>
        string TableFieldKTPR { get; }

        /// <summary>
        /// Поля для "Карта общестанционных защит"
        /// </summary>
        string FieldKTPR { get; }
        #endregion

        #region Карта предельных параметров общестанционных защит
        /// <summary>
        /// Поля таблицы для "Карта предельных параметров общестанционных защит"
        /// </summary>
        string TableFieldKTPRS { get; }

        /// <summary>
        /// Поля для "Карта предельных параметров общестанционных защит"
        /// </summary>
        string FieldKTPRS { get; }
        #endregion

        #region LIST5
        /// <summary>
        /// Поля таблицы для "LIST5"
        /// </summary>
        string TableFieldSignaling { get; }

        /// <summary>
        /// Поля для "LIST5"
        /// </summary>
        string FieldSignaling { get; }
        #endregion

        #region Табло и сирены
        /// <summary>
        /// Поля таблицы для "Табло и сирены"
        /// </summary>
        string TableFieldUTS { get; }

        /// <summary>
        /// Поля для "Табло и сирены"
        /// </summary>
        string FieldUTS { get; }
        #endregion

        #region Уставки типа REAL
        /// <summary>
        /// Поля таблицы для "Уставки типа REAL"
        /// </summary>
        string TableFieldUstReal { get; }

        /// <summary>
        /// Поля для "Уставки типа REAL"
        /// </summary>
        string FieldUstReal { get; }
        #endregion

        #region Временные уставки общие
        /// <summary>
        /// Поля таблицы для "Временные уставки общие"
        /// </summary>
        string TableFieldUstCommon { get; }

        /// <summary>
        /// Поля для "Временные уставки общие"
        /// </summary>
        string FieldUstCommon { get; }
        #endregion

        #region Карта ручного ввода
        /// <summary>
        /// Поля таблицы для "Карта ручного ввода"
        /// </summary>
        string TableFieldHandMap { get; }

        /// <summary>
        /// Поля для "Карта ручного ввода"
        /// </summary>
        string FieldHandMap { get; }
        #endregion

        #region Команды
        /// <summary>
        /// Поля таблицы для "Команды"
        /// </summary>
        string TableFieldCommand { get; }

        /// <summary>
        /// Поля для "Команды"
        /// </summary>
        string FieldCommand { get; }
        #endregion

        #region Сообщения
        /// <summary>
        /// Поля таблицы для "Сообщения"
        /// </summary>
        string TableFieldMessages { get; }

        /// <summary>
        /// Поля для "Сообщения"
        /// </summary>
        string FieldMessages { get; }
        #endregion

        #region Системы сообщения
        /// <summary>
        /// Поля таблицы для "Системы сообщения"
        /// </summary>
        string TableFieldSyatemMessages { get; }

        /// <summary>
        /// Поля для "Системы сообщения"
        /// </summary>
        string FieldSyatemMessages { get; }
        #endregion

        #region Уставки аналоговых параметров
        /// <summary>
        /// Поля таблицы для "SETPOINTS_AI"
        /// </summary>
        public string TableFieldSetpointsAI { get; }

        /// <summary>
        /// Поля для "SETPOINTS_AI"
        /// </summary>
        public string FieldSetpointsAI { get; }
        #endregion

        #endregion
    }
}
