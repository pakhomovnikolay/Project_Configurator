using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System.Collections.Generic;

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

        #region Цвет УСО
        /// <summary>
        /// Цвет УСО
        /// </summary>
        public string Color { get; set; }
        #endregion

        #region Корзины УСО
        /// <summary>
        /// Корзины УСО
        /// </summary>
        public List<Rack> Racks { get; set; }
        #endregion
    }
}
