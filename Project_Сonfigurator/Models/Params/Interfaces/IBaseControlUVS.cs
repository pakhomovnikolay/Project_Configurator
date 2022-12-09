namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseControlUVS
    {
        #region Включить "Основные"
        /// <summary>
        /// Включить "Основные"
        /// </summary>
        string OnPrimary { get; set; }
        #endregion

        #region Включить "Резервный"
        /// <summary>
        /// Включить "Резервный"
        /// </summary>
        string OnSecondary { get; set; }
        #endregion

        #region Отключить "Основной"
        /// <summary>
        /// Отключить "Основной"
        /// </summary>
        string OffPrimary { get; set; }
        #endregion

        #region Отключить "Резервный"
        /// <summary>
        /// Отключить "Резервный"
        /// </summary>
        string OffSecondary { get; set; }
        #endregion

        #region Включить "Основной" с блокировкой
        /// <summary>
        /// Включить "Основной" с блокировкой
        /// </summary>
        string NoOffPrimary { get; set; }
        #endregion

        #region Включить "Резервный" с блокировкой
        /// <summary>
        /// Включить "Резервный" с блокировкой
        /// </summary>
        string NoOffSecondary { get; set; }
        #endregion

        #region Отключить "Основной" с блокировкой
        /// <summary>
        /// Отключить "Основной" с блокировкой
        /// </summary>
        string NoOnPrimary { get; set; }
        #endregion

        #region Отключить "Резервный" с блокировкой
        /// <summary>
        /// Отключить "Резервный" с блокировкой
        /// </summary>
        string NoOnSecondary { get; set; }
        #endregion

        #region Включить "Основной" после снятия защиты
        /// <summary>
        /// Включить "Основной" после снятия защиты
        /// </summary>
        string OnPrimaryAfterProt { get; set; }
        #endregion

        #region Включить "Резервный" после снятия защиты
        /// <summary>
        /// Включить "Резервный" после снятия защиты
        /// </summary>
        string OnSecondaryAfterProt { get; set; }
        #endregion

        #region Отключить "Основной" после снятия защиты
        /// <summary>
        /// Отключить "Основной" после снятия защиты
        /// </summary>
        string OffPrimaryAfterProt { get; set; }
        #endregion

        #region Отключить "Резервный" после снятия защиты
        /// <summary>
        /// Отключить "Резервный" после снятия защиты
        /// </summary>
        string OffSecondaryAfterProt { get; set; }
        #endregion

        #region Включить по таймеру "Основной" после снятия защиты 
        /// <summary>
        /// Включить по таймеру "Основной" после снятия защиты 
        /// </summary>
        string OnPrimaryAfterProtTimer { get; set; }
        #endregion

        #region Включить по таймеру "Резервный" после снятия защиты
        /// <summary>
        /// Включить по таймеру "Резервный" после снятия защиты
        /// </summary>
        string OnSecondaryAfterProtTimer { get; set; }
        #endregion

        #region Отключить по таймеру "Основной" после снятия защиты
        /// <summary>
        /// Отключить по таймеру "Основной" после снятия защиты
        /// </summary>
        string OffPrimaryAfterProtTimer { get; set; }
        #endregion

        #region Отключить по таймеру "Резервный" после снятия защиты
        /// <summary>
        /// Отключить по таймеру "Резервный" после снятия защиты
        /// </summary>
        string OffSecondaryAfterProtTimer { get; set; }
        #endregion
    }
}
