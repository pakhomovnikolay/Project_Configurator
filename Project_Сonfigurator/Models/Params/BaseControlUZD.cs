using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseControlUZD : IBaseControlUZD
    {
        #region Закрыть
        /// <summary>
        /// Закрыть
        /// </summary>
        public string Close { get; set; }
        #endregion

        #region Открыть
        /// <summary>
        /// Открыть
        /// </summary>
        public string Open { get; set; }
        #endregion

        #region Закрыть с блокировкой
        /// <summary>
        /// Закрыть с блокировкой
        /// </summary>
        public string NoOpen { get; set; }
        #endregion

        #region Открыть с блокировкой
        /// <summary>
        /// Открыть с блокировкой
        /// </summary>
        public string NoClose { get; set; }
        #endregion

        #region Закрыть после снятия защиты
        /// <summary>
        /// Закрыть после снятия защиты
        /// </summary>
        public string CloseAfterProt { get; set; }
        #endregion

        #region Открыть после снятия защиты
        /// <summary>
        /// Открыть после снятия защиты
        /// </summary>
        public string OpenAfterProt { get; set; }
        #endregion

        #region Закрыть по таймеру после снятия защиты
        /// <summary>
        /// Закрыть по таймеру после снятия защиты
        /// </summary>
        public string CloseAfterProtTimer { get; set; }
        #endregion

        #region Открыть по таймеру после снятия защиты
        /// <summary>
        /// Открыть по таймеру после снятия защиты
        /// </summary>
        public string OpenAfterProtTimer { get; set; }
        #endregion
    }
}
