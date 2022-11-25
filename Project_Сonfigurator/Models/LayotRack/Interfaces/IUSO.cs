﻿using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IUSO
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Имя УСО
        /// <summary>
        /// Имя УСО
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Корзины УСО
        /// <summary>
        /// Корзины УСО
        /// </summary>
        List<Rack> Racks { get; set; }
        #endregion
    }
}
