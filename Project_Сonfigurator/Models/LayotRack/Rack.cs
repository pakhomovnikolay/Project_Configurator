using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class Rack : IRack
    {
        #region Индекс корзины
        /// <summary>
        /// Индекс корзины
        /// </summary>
        public int Index { get; set; }
        #endregion

        #region Имя корзины
        /// <summary>
        /// Имя корзины
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Индекс шкафа
        /// <summary>
        /// Индекс шкафа
        /// </summary>
        public int IndexUSO { get; set; }
        #endregion

        #region Имя шкафа
        /// <summary>
        /// Имя шкафа
        /// </summary>
        public string NameUSO { get; set; }
        #endregion

        #region Список модулей в корзине
        /// <summary>
        /// Список модулей в корзине
        /// </summary>
        public List<RackModule> Modules { get; set; } = new();
        #endregion
    }
}
