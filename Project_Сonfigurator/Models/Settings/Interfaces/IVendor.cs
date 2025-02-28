﻿using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface IVendor
    {
        #region Название вендора
        /// <summary>
        /// Название вендора
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Выбранный вендор
        /// <summary>
        /// Выбранный вендор
        /// </summary>
        bool IsSelected { get; set; }
        #endregion

        #region Список модулей
        /// <summary>
        /// Список модулей
        /// </summary>
        ObservableCollection<VendorModuleType> ModuleTypes { get; set; }
        #endregion
    }
}
