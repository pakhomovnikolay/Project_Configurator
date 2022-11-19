using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IRack
    {
        #region Индекс корзины
        /// <summary>
        /// Индекс корзины
        /// </summary>
        int Index { get; set; }
        #endregion

        #region Имя корзины
        /// <summary>
        /// Имя корзины
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Индекс шкафа
        /// <summary>
        /// Индекс шкафа
        /// </summary>
        int IndexUSO { get; set; }
        #endregion

        #region Имя шкафа
        /// <summary>
        /// Имя шкафа
        /// </summary>
        string NameUSO { get; set; }
        #endregion

        #region Список модулей в корзине
        /// <summary>
        /// Список модулей в корзине
        /// </summary>
        List<RackModule> Modules { get; set; }
        #endregion
    }
}
