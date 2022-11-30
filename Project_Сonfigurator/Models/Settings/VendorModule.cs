using Project_Сonfigurator.Models.Settings.Interfaces;

namespace Project_Сonfigurator.Models.Settings
{
    public class VendorModule : IVendorModule
    {
        #region Имя модуля
        /// <summary>
        /// Имя модуля
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}