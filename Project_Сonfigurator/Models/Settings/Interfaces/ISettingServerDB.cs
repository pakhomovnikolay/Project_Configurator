namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    interface ISettingServerDB
    {
        #region Выбранный сервер для разлива данных
        /// <summary>
        /// Выбранный сервер для разлива данных
        /// </summary>
        bool IsSelection { get; set; }
        #endregion

        #region Описание подлключения
        /// <summary>
        /// Описание подлключения
        /// </summary>
        string DescriptionConnect { get; set; } 
        #endregion

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

        #region Состояние успешного обновления БД
        /// <summary>
        /// Состояние успешного обновления БД
        /// </summary>
        bool SuccessUpdate { get; set; } 
        #endregion
    }
}
