﻿using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Signals.Interfaces;

namespace Project_Сonfigurator.Models.Signals
{
    public class SignalAI : ISignalAI
    {
        #region Параметры сигнала
        /// <summary>
        /// Параметры сигнала
        /// </summary>
        public BaseSignal Signal { get; set; } = new();
        #endregion

        #region Номер агрегата
        /// <summary>
        /// Номер агрегата
        /// </summary>
        public string IndexNA { get; set; }
        #endregion

        #region Обозначение для ВУ
        /// <summary>
        /// Обозначение для ВУ
        /// </summary>
        public string Unit { get; set; }
        #endregion

        #region Тип вибрации
        /// <summary>
        /// Тип  вибрации
        /// </summary>
        public string TypeVibration { get; set; }
        #endregion

        #region  Номер ПЗ
        /// <summary>
        /// Номер ПЗ
        /// </summary>
        public string IndexPZ { get; set; }
        #endregion

        #region Тип ПИ
        /// <summary>
        /// Тип ПИ
        /// </summary>
        public string TypePI { get; set; }
        #endregion

        #region Номер БД  (для уровней)
        /// <summary>
        /// Номер БД  (для уровней)
        /// </summary>
        public string IndexBD { get; set; }
        #endregion

        #region Уровень в резервуаре противопожарного запаса воды
        /// <summary>
        /// Уровень в резервуаре противопожарного запаса воды
        /// </summary>
        public string LevelRPP { get; set; }
        #endregion

        #region Смещенеие для DO (табло/сирены)
        /// <summary>
        /// Смещенеие для DO (табло/сирены)
        /// </summary>
        public string AddresUTS { get; set; }
        #endregion

        #region Отображать в кгс\см2
        /// <summary>
        /// Отображать в кгс\см2
        /// </summary>
        public string ConverterKgs { get; set; }
        #endregion

        #region Настройки уставок
        /// <summary>
        /// Настройки уставок
        /// </summary>
        public BaseSetpointsAI Setpoints { get; set; } = new(); 
        #endregion
    }
}
