using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseControlUTS : IBaseControlUTS
    {
        #region Отключить
        /// <summary>
        /// Отключить
        /// </summary>
        public string On { get; set; }
        #endregion

        #region Включить
        /// <summary>
        /// Включить
        /// </summary>
        public string Off { get; set; }
        #endregion

        #region Отключить с блокировкой
        /// <summary>
        /// Отключить с блокировкой
        /// </summary>
        public string NoOn { get; set; }
        #endregion

        #region Включить с блокировкой
        /// <summary>
        /// Включить с блокировкой
        /// </summary>
        public string NoOff { get; set; }
        #endregion

        #region Отключить после снятия защиты
        /// <summary>
        /// Отключить после снятия защиты
        /// </summary>
        public string OnAfterProt { get; set; }
        #endregion

        #region Включить после снятия защиты
        /// <summary>
        /// Включить после снятия защиты
        /// </summary>
        public string OffAfterProt { get; set; }
        #endregion

        #region Отключить по таймеру после снятия защиты
        /// <summary>
        /// Отключить по таймеру после снятия защиты
        /// </summary>
        public string OnAfterProtTimer { get; set; }
        #endregion

        #region Включить по таймеру после снятия защиты
        /// <summary>
        /// Включить по таймеру после снятия защиты
        /// </summary>
        public string OffAfterProtTimer { get; set; }
        #endregion
    }
}
