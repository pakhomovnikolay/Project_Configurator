namespace Project_Сonfigurator.Models.Signals.Interfaces
{
    public interface ISignalAI
    {
        #region Параметры сигнала
        /// <summary>
        /// Параметры сигнала
        /// </summary>
        BaseSignal Signal { get; set; }
        #endregion

        #region Номер агрегата
        /// <summary>
        /// Номер агрегата
        /// </summary>
        string IndexNA { get; set; }
        #endregion

        #region Обозначение для ВУ
        /// <summary>
        /// Обозначение для ВУ
        /// </summary>
        string Unit { get; set; }
        #endregion

        #region Тип  вибрации
        /// <summary>
        /// Тип  вибрации
        /// </summary>
        string TypeVibration { get; set; }
        #endregion

        #region  Номер ПЗ
        /// <summary>
        /// Номер ПЗ
        /// </summary>
        string IndexPZ { get; set; }
        #endregion

        #region Тип ПЗ
        /// <summary>
        /// Тип ПЗ
        /// </summary>
        string TypePZ { get; set; }
        #endregion

        #region Тип ПИ
        /// <summary>
        /// Тип ПИ
        /// </summary>
        string TypePI { get; set; }
        #endregion

        #region Номер БД  (для уровней)
        /// <summary>
        /// Номер БД  (для уровней)
        /// </summary>
        string IndexBD { get; set; }
        #endregion

        #region Уровень в резервуаре противопожарного запаса воды
        /// <summary>
        /// Уровень в резервуаре противопожарного запаса воды
        /// </summary>
        string LevelRPP { get; set; }
        #endregion

        #region Смещенеие для DO (табло/сирены)
        /// <summary>
        /// Смещенеие для DO (табло/сирены)
        /// </summary>
        string AddresUTS { get; set; }
        #endregion

        #region Отображать в кгс\см2
        /// <summary>
        /// Отображать в кгс\см2
        /// </summary>
        string ConverterKgs { get; set; }
        #endregion
    }
}
