namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseControlUTS
    {
        #region Отключить
        /// <summary>
        /// Отключить
        /// </summary>
        string On { get; set; }
        #endregion

        #region Включить
        /// <summary>
        /// Включить
        /// </summary>
        string Off { get; set; }
        #endregion

        #region Отключить с блокировкой
        /// <summary>
        /// Отключить с блокировкой
        /// </summary>
        string NoOn { get; set; }
        #endregion

        #region Включить с блокировкой
        /// <summary>
        /// Включить с блокировкой
        /// </summary>
        string NoOff { get; set; }
        #endregion

        #region Отключить после снятия защиты
        /// <summary>
        /// Отключить после снятия защиты
        /// </summary>
        string OnAfterProt { get; set; }
        #endregion

        #region Включить после снятия защиты
        /// <summary>
        /// Включить после снятия защиты
        /// </summary>
        string OffAfterProt { get; set; }
        #endregion

        #region Отключить по таймеру после снятия защиты
        /// <summary>
        /// Отключить по таймеру после снятия защиты
        /// </summary>
        string OnAfterProtTimer { get; set; }
        #endregion

        #region Включить по таймеру после снятия защиты
        /// <summary>
        /// Включить по таймеру после снятия защиты
        /// </summary>
        string OffAfterProtTimer { get; set; }
        #endregion
    }
}
