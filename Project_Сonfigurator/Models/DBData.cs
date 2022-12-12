using Project_Сonfigurator.Models.Interfaces;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models
{
    public class DBData : IDBData
    {
        #region Данные диагностики
        /// <summary>
        /// Данные диагностики
        /// </summary>
        public List<USO> USOList { get; set; } = new();
        #endregion

        #region DI формируемые
        /// <summary>
        /// DI формируемые
        /// </summary>
        public List<BaseSignal> UserDI { get; set; } = new();
        #endregion

        #region AI формируемые
        /// <summary>
        /// AI формируемые
        /// </summary>
        public List<BaseSignal> UserAI { get; set; } = new();
        #endregion

        #region Сигналы DI
        /// <summary>
        /// Сигналы DI
        /// </summary>
        public List<SignalDI> SignalDI { get; set; } = new();
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Сигналы AI
        /// </summary>
        public List<SignalAI> SignalAI { get; set; } = new();
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Сигналы DO
        /// </summary>
        public List<SignalDO> SignalDO { get; set; } = new();
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Сигналы AO
        /// </summary>
        public List<SignalAO> SignalAO { get; set; } = new();
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Регистры формируемые
        /// </summary>
        public List<BaseParam> UserReg { get; set; } = new();
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Сигналы групп
        /// </summary>
        public List<BaseParam> SignalGroup { get; set; } = new();
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Группы сигналов
        /// </summary>
        public List<GroupSignal> GroupSignals { get; set; } = new();
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        public List<BaseUZD> UZD { get; set; } = new();
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        public List<BaseUVS> UVS { get; set; } = new();
        #endregion

        #region Настройки МПНА
        /// <summary>
        /// Настройки МПНА
        /// </summary>
        public List<BaseUMPNA> UMPNA { get; set; } = new();
        #endregion

        #region Общестанционные защиты
        /// <summary>
        /// Общестанционные защиты
        /// </summary>
        public List<BaseKTPR> KTPR { get; set; } = new();
        #endregion

        #region Предельные параметры общестанционных защит
        /// <summary>
        /// Предельные параметры общестанционных защит
        /// </summary>
        public List<BaseKTPRS> KTPRS { get; set; } = new();
        #endregion

        #region Сигнализация и общая диагностики
        /// <summary>
        /// Сигнализация и общая диагностики
        /// </summary>
        public List<BaseSignaling> Signaling { get; set; } = new();
        #endregion
    }
}
