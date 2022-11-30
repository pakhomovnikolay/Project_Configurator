using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingApp
    {
        #region Список вендоров
        /// <summary>
        /// Список вендоров
        /// </summary>
        List<Vendor> Vendors { get; set; } 
        #endregion
    }
}
