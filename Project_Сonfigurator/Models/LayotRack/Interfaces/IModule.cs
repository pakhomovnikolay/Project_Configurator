using Project_Сonfigurator.Infrastructures.Enum;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IModule
    {
        #region Тип модуля
        /// <summary>
        /// Тип модуля
        /// </summary>
        TypeModule Type { get; set; }
        #endregion

        #region Индекс модуля
        /// <summary>
        /// Индекс модуля
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

        #region Наименование УСО, к котрому относится модуль
        /// <summary>
        /// Наименование УСО, к котрому относится модуль
        /// </summary>
        string NameUSO { get; set; }
        #endregion

        #region Цвет УСО
        /// <summary>
        /// Цвет УСО
        /// </summary>
        string ColorUSO { get; set; }
        #endregion

        #region Каналы модуля
        /// <summary>
        /// Каналы модуля
        /// </summary>
        List<Channel> Channels { get; set; }
        #endregion
    }
}
