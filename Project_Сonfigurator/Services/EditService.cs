using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.Views.DialogControl;
using System;
using System.Windows;

namespace Project_Сonfigurator.Services
{
    public class EditService : IEditService
    {
        #region Редактирование объекта
        /// <summary>
        /// Редактирование объекта
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool Edit(object Item, string title = null)
        {
            if (Item is null) throw new ArgumentNullException(nameof(Item));
            return Item switch
            {
                Vendor vendor => EditVendor(vendor),
                _ => throw new NotSupportedException($"Редактирование объекта типа {Item.GetType().Name} не поддерживается"),
            };
        }
        #endregion

        #region Открыть окно редактирования данных вендора
        /// <summary>
        /// Открыть окно редактирования данных вендора
        /// </summary>
        /// <returns></returns>
        private static bool EditVendor(Vendor vendor)
        {
            var dlg = new WindowEditVendor
            {
                VendorData = vendor,
                Owner = App.FucusedWindow ?? App.ActiveWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (!dlg.ShowDialog().Value) return false;

            return true;
        }
        #endregion
    }
}
