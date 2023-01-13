using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface IVendorModuleType
    {
        #region Наименование типа модулей
        /// <summary>
        /// Наименование типа модулей
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Список модулей
        /// <summary>
        /// Список модулей
        /// </summary>
        ObservableCollection<VendorModule> Modules { get; set; }
        #endregion
    }
}
