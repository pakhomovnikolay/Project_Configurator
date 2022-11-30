using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingApp : ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        public List<Vendor> Vendors { get; set; } = new();
        #endregion
    }
}
