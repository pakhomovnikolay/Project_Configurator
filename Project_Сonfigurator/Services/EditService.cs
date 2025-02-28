﻿using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.Views.DialogControl;
using System;
using System.Windows;

namespace Project_Сonfigurator.Services
{
    public class EditService : BaseService, IEditService
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
                Vendor Data => EditVendor(Data),
                SettingServerDB Data => EditServerDB(Data),
                _ => throw new NotSupportedException($"Редактирование объекта типа {Item.GetType().Name} не поддерживается"),
            };
        }
        #endregion

        #region Открыть окно редактирования данных вендора
        /// <summary>
        /// Открыть окно редактирования данных вендора
        /// </summary>
        /// <returns></returns>
        private static bool EditVendor(Vendor Data)
        {
            var dlg = new WindowEditVendor
            {
                VendorData = Data,
                Owner = App.FucusedWindow ?? App.ActiveWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (!dlg.ShowDialog().Value) return false;

            return true;
        }
        #endregion

        #region Открыть окно редактирования данных сервера
        /// <summary>
        /// Открыть окно редактирования данных сервера
        /// </summary>
        /// <returns></returns>
        private static bool EditServerDB(SettingServerDB Data)
        {
            var dlg = new WindowEditServerDB
            {
                SelectedServer = Data,
                Owner = App.FucusedWindow ?? App.ActiveWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (!dlg.ShowDialog().Value) return false;

            return true;
        }
        #endregion
    }
}
