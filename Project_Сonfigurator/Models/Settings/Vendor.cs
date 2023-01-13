using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings
{
    public class Vendor : IVendor
    {
        #region Название вендора
        /// <summary>
        /// Название вендора
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Выбранный вендор
        /// <summary>
        /// Выбранный вендор
        /// </summary>
        public bool IsSelected { get; set; }
        #endregion

        #region Список типов модулей
        /// <summary>
        /// Список типов модулей
        /// </summary>
        public ObservableCollection<VendorModuleType> ModuleTypes { get; set; }
        #endregion
    }
}