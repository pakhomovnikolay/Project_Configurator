using Project_Сonfigurator.Models.Settings.Interfaces;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingApp : ISettingApp
    {
        #region Выбранное имя системы
        /// <summary>
        /// Выбранное имя системы
        /// </summary>
        public string SelectedSystemName { get; set; }
        #endregion

        #region Выбранный тип системы
        /// <summary>
        /// Выбранный тип системы
        /// </summary>
        public string SelectedTypeSystem { get; set; }
        #endregion

        #region Выбранный тип вендора
        /// <summary>
        /// Выбранный тип вендора
        /// </summary>
        public string SelectedTypeVendor { get; set; }
        #endregion
    }
}
