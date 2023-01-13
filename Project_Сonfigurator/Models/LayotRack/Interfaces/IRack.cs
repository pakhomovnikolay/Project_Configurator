using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IRack
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Имя корзины
        /// <summary>
        /// Имя корзины
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Корзина активна
        /// <summary>
        /// Корзина активна
        /// </summary>
        bool IsEnable { get; set; }
        #endregion

        #region Модули корзины
        /// <summary>
        /// Модули корзины
        /// </summary>
        ObservableCollection<RackModule> Modules { get; set; }
        #endregion
    }
}
