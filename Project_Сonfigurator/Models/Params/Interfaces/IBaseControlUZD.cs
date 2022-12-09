namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseControlUZD
    {
        #region Закрыть
        /// <summary>
        /// Закрыть
        /// </summary>
        string Close { get; set; }
        #endregion

        #region Открыть
        /// <summary>
        /// Открыть
        /// </summary>
        string Open { get; set; }
        #endregion

        #region Закрыть с блокировкой
        /// <summary>
        /// Закрыть с блокировкой
        /// </summary>
        string NoOpen { get; set; }
        #endregion

        #region Открыть с блокировкой
        /// <summary>
        /// Открыть с блокировкой
        /// </summary>
        string NoClose { get; set; }
        #endregion

        #region Закрыть после снятия защиты
        /// <summary>
        /// Закрыть после снятия защиты
        /// </summary>
        string CloseAfterProt { get; set; }
        #endregion

        #region Открыть после снятия защиты
        /// <summary>
        /// Открыть после снятия защиты
        /// </summary>
        string OpenAfterProt { get; set; }
        #endregion

        #region Закрыть по таймеру после снятия защиты
        /// <summary>
        /// Закрыть по таймеру после снятия защиты
        /// </summary>
        string CloseAfterProtTimer { get; set; }
        #endregion

        #region Открыть по таймеру после снятия защиты
        /// <summary>
        /// Открыть по таймеру после снятия защиты
        /// </summary>
        string OpenAfterProtTimer { get; set; }
        #endregion
    }
}
