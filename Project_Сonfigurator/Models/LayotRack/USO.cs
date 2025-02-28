﻿using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class USO : IUSO
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя УСО
        /// <summary>
        /// Имя УСО
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Состояние включенных всех корзин в УСО
        /// <summary>
        /// Состояние включенных всех корзин в УСО
        /// </summary>
        public bool IsAllEnable { get; set; } = true;
        #endregion

        #region Корзины УСО
        /// <summary>
        /// Корзины УСО
        /// </summary>
        public ObservableCollection<Rack> Racks { get; set; }
        #endregion
    }
}
