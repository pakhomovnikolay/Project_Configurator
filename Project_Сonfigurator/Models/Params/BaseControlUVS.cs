using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseControlUVS : IBaseControlUVS
    {
        #region Включить "Основные"
        /// <summary>
        /// Включить "Основные"
        /// </summary>
        public string OnPrimary { get; set; }
        #endregion

        #region Включить "Резервный"
        /// <summary>
        /// Включить "Резервный"
        /// </summary>
        public string OnSecondary { get; set; }
        #endregion

        #region Отключить "Основной"
        /// <summary>
        /// Отключить "Основной"
        /// </summary>
        public string OffPrimary { get; set; }
        #endregion

        #region Отключить "Резервный"
        /// <summary>
        /// Отключить "Резервный"
        /// </summary>
        public string OffSecondary { get; set; }
        #endregion

        #region Включить "Основной" с блокировкой
        /// <summary>
        /// Включить "Основной" с блокировкой
        /// </summary>
        public string NoOffPrimary { get; set; }
        #endregion

        #region Включить "Резервный" с блокировкой
        /// <summary>
        /// Включить "Резервный" с блокировкой
        /// </summary>
        public string NoOffSecondary { get; set; }
        #endregion

        #region Отключить "Основной" с блокировкой
        /// <summary>
        /// Отключить "Основной" с блокировкой
        /// </summary>
        public string NoOnPrimary { get; set; }
        #endregion

        #region Отключить "Резервный" с блокировкой
        /// <summary>
        /// Отключить "Резервный" с блокировкой
        /// </summary>
        public string NoOnSecondary { get; set; }
        #endregion

        #region Включить "Основной" после снятия защиты
        /// <summary>
        /// Включить "Основной" после снятия защиты
        /// </summary>
        public string OnPrimaryAfterProt { get; set; }
        #endregion

        #region Включить "Резервный" после снятия защиты
        /// <summary>
        /// Включить "Резервный" после снятия защиты
        /// </summary>
        public string OnSecondaryAfterProt { get; set; }
        #endregion

        #region Отключить "Основной" после снятия защиты
        /// <summary>
        /// Отключить "Основной" после снятия защиты
        /// </summary>
        public string OffPrimaryAfterProt { get; set; }
        #endregion

        #region Отключить "Резервный" после снятия защиты
        /// <summary>
        /// Отключить "Резервный" после снятия защиты
        /// </summary>
        public string OffSecondaryAfterProt { get; set; }
        #endregion

        #region Включить по таймеру "Основной" после снятия защиты 
        /// <summary>
        /// Включить по таймеру "Основной" после снятия защиты 
        /// </summary>
        public string OnPrimaryAfterProtTimer { get; set; }
        #endregion

        #region Включить по таймеру "Резервный" после снятия защиты
        /// <summary>
        /// Включить по таймеру "Резервный" после снятия защиты
        /// </summary>
        public string OnSecondaryAfterProtTimer { get; set; }
        #endregion

        #region Отключить по таймеру "Основной" после снятия защиты
        /// <summary>
        /// Отключить по таймеру "Основной" после снятия защиты
        /// </summary>
        public string OffPrimaryAfterProtTimer { get; set; }
        #endregion

        #region Отключить по таймеру "Резервный" после снятия защиты
        /// <summary>
        /// Отключить по таймеру "Резервный" после снятия защиты
        /// </summary>
        public string OffSecondaryAfterProtTimer { get; set; }
        #endregion
    }
}
