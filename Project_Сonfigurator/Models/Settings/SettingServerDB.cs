using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Windows;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingServerDB : Freezable, ISettingServerDB
    {
        protected override Freezable CreateInstanceCore() => new SettingServerDB();

        #region Выбранный сервер для разлива данных
        /// <summary>
        /// Выбранный сервер для разлива данных
        /// </summary>
        public bool IsSelection
        {
            get => (bool)GetValue(IsSelectionProperty);
            set => SetValue(IsSelectionProperty, value);
        }

        public static readonly DependencyProperty IsSelectionProperty =
            DependencyProperty.Register(
                nameof(IsSelection),
                typeof(bool),
                typeof(SettingServerDB),
                new PropertyMetadata(default(bool)));
        #endregion

        #region Описание подлключения
        /// <summary>
        /// Описание подлключения
        /// </summary>
        public string DescriptionConnect { get; set; }
        #endregion

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

        #region Состояние успешного обновления БД
        /// <summary>
        /// Состояние успешного обновления БД
        /// </summary>
        public bool SuccessUpdate
        {
            get => (bool)GetValue(SuccessUpdateProperty);
            set => SetValue(SuccessUpdateProperty, value);
        }

        public static readonly DependencyProperty SuccessUpdateProperty =
            DependencyProperty.Register(
                nameof(SuccessUpdate),
                typeof(bool),
                typeof(SettingServerDB),
                new PropertyMetadata(default(bool)));
        #endregion
    }
}
