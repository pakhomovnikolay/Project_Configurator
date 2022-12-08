namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    interface ISettingConnectDB
    {
        #region Адрес подключения
        /// <summary>
        /// Адрес подключения
        /// </summary>
        string IPAddress { get; set; }
        #endregion

        #region Имя базы данных
        /// <summary>
        /// Имя базы данных
        /// </summary>
        string NameDB { get; set; }
        #endregion

        #region Порт подключения
        /// <summary>
        /// Порт подключения
        /// </summary>
        string Port { get; set; }
        #endregion

        #region Таймаут подключения(сек.)
        /// <summary>
        /// Таймаут подключения(сек.)
        /// </summary>
        string TimeoutConnect { get; set; }
        #endregion

        #region Имя пользователя
        /// <summary>
        /// Имя пользователя
        /// </summary>
        string UserName { get; set; }
        #endregion

        #region Пароль
        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; set; }
        #endregion
    }
}
