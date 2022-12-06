namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseKTPRAS
    {
        #region Параметры защит
        /// <summary>
        /// Параметры защит
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Состояние НА
        /// <summary>
        /// Состояние НА
        /// </summary>
        string StateUMPNA { get; set; }
        #endregion

        #region Настройка сигнализации
        /// <summary>
        /// Настройка сигнализации
        /// </summary>
        string TypeWarning { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        string Type { get; set; }
        #endregion
    }
}
