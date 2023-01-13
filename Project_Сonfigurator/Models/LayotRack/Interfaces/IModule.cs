using Project_Сonfigurator.Infrastructures.Enum;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IModule
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Имя модуля
        /// <summary>
        /// Имя модуля
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Стартовый адрес
        /// <summary>
        /// Стартовый адрес
        /// </summary>
        string StartAddress { get; set; }
        #endregion

        #region Конечный адрес
        /// <summary>
        /// Конечный адрес
        /// </summary>
        string EndAddress { get; set; }
        #endregion

        #region Тип модуля
        /// <summary>
        /// Тип модуля
        /// </summary>
        TypeModule Type { get; set; }
        #endregion

        #region Каналы модуля
        /// <summary>
        /// Каналы модуля
        /// </summary>
        ObservableCollection<Channel> Channels { get; set; }
        #endregion
    }
}
