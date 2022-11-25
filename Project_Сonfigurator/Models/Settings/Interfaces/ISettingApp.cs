namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingApp
    {
        #region Выбранное имя системы
        /// <summary>
        /// Выбранное имя системы
        /// </summary>
        string SelectedSystemName { get; set; }
        #endregion

        #region Выбранный тип системы
        /// <summary>
        /// Выбранный тип системы
        /// </summary>
        string SelectedTypeSystem { get; set; }
        #endregion

        #region Выбранный тип вендора
        /// <summary>
        /// Выбранный тип вендора
        /// </summary>
        string SelectedTypeVendor { get; set; }
        #endregion
    }
}
