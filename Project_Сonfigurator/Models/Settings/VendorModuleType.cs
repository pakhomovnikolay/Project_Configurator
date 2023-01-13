using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings
{
    public class VendorModuleType : IVendorModuleType
    {
        #region Наименование типа модулей
        /// <summary>
        /// Наименование типа модулей
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Наименование типа модулей
        /// <summary>
        /// Наименование типа модулей
        /// </summary>
        public VendorModule SelectedModule { get; set; }
        #endregion

        #region Список модулей
        /// <summary>
        /// Список модулей
        /// </summary>
        public ObservableCollection<VendorModule> Modules { get; set; }
        #endregion
    }
}