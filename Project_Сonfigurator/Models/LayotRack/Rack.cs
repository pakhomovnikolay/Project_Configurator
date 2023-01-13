using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class Rack : IRack
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя корзины
        /// <summary>
        /// Имя корзины
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Корзина активна
        /// <summary>
        /// Корзина активна
        /// </summary>
        public bool IsEnable { get; set; }
        #endregion

        #region Модули корзины
        /// <summary>
        /// Модули корзины
        /// </summary>
        public ObservableCollection<RackModule> Modules { get; set; } = new();
        #endregion
    }
}
