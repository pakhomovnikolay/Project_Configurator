using Project_Сonfigurator.Models.Settings.Interfaces;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingConnectDB : ISettingConnectDB
    {
        #region Адрес подключения
        /// <summary>
        /// Адрес подключения
        /// </summary>
        public string IPAddress { get; set; }
        #endregion

        #region Имя базы данных
        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string NameDB { get; set; }
        #endregion

        #region Порт подключения
        /// <summary>
        /// Порт подключения
        /// </summary>
        public string Port { get; set; }
        #endregion

        #region Таймаут подключения(сек.)
        /// <summary>
        /// Таймаут подключения(сек.)
        /// </summary>
        public string TimeoutConnect { get; set; }
        #endregion

        #region Имя пользователя
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
        #endregion

        #region Пароль
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        #endregion
    }
}
