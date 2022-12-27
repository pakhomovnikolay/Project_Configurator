﻿using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Signals;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Interfaces
{
    public interface IDBData
    {
        #region Данные диагностики
        /// <summary>
        /// Данные диагностики
        /// </summary>
        List<USO> USOList { get; set; }
        #endregion

        #region DI формируемые
        /// <summary>
        /// DI формируемые
        /// </summary>
        List<BaseSignal> UserDI { get; set; }
        #endregion

        #region AI формируемые
        /// <summary>
        /// AI формируемые
        /// </summary>
        List<BaseSignal> UserAI { get; set; }
        #endregion

        #region Сигналы DI
        /// <summary>
        /// Сигналы DI
        /// </summary>
        List<SignalDI> SignalDI { get; set; }
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Сигналы AI
        /// </summary>
        List<SignalAI> SignalAI { get; set; }
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Сигналы DO
        /// </summary>
        List<SignalDO> SignalDO { get; set; }
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Сигналы AO
        /// </summary>
        List<SignalAO> SignalAO { get; set; }
        #endregion

        #region Секции шин
        /// <summary>
        /// Секции шин
        /// </summary>
        List<BaseParam> ECParam { get; set; }
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Регистры формируемые
        /// </summary>
        List<BaseParam> UserReg { get; set; }
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Сигналы групп
        /// </summary>
        List<BaseParam> SignalGroup { get; set; }
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Группы сигналов
        /// </summary>
        List<GroupSignal> GroupSignals { get; set; }
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        List<BaseUZD> UZD { get; set; }
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        List<BaseUVS> UVS { get; set; }
        #endregion

        #region Настройки МПНА
        /// <summary>
        /// Настройки МПНА
        /// </summary>
        List<BaseUMPNA> UMPNA { get; set; }
        #endregion

        #region Общестанционные защиты
        /// <summary>
        /// Общестанционные защиты
        /// </summary>
        List<BaseKTPR> KTPR { get; set; }
        #endregion

        #region Предельные параметры общестанционных защит
        /// <summary>
        /// Предельные параметры общестанционных защит
        /// </summary>
        List<BaseKTPRS> KTPRS { get; set; }
        #endregion

        #region Сигнализация и общая диагностики
        /// <summary>
        /// Сигнализация и общая диагностики
        /// </summary>
        List<BaseSignaling> Signaling { get; set; }
        #endregion

        #region Табло и сирены
        /// <summary>
        /// Табло и сирены
        /// </summary>
        List<BaseUTS> UTS { get; set; }
        #endregion

        #region Уставки Real
        /// <summary>
        /// Уставки Real
        /// </summary>
        List<BaseSetpointsReal> SetpointsReal { get; set; }
        #endregion

        #region Уставки общие
        /// <summary>
        /// Уставки общие
        /// </summary>
        List<BaseSetpoints> SetpointsCommon { get; set; }
        #endregion

        #region Карта ручного ввода
        /// <summary>
        /// Карта ручного ввода
        /// </summary>
        List<BaseParam> HandMap { get; set; }
        #endregion
    }
}