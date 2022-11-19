using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class RackModule : IModule
    {
        #region Тип модуля
        /// <summary>
        /// Тип модуля
        /// </summary>
        public TypeModule Type { get; set; } = TypeModule.Unknown;
        #endregion

        #region Индекс модуля
        /// <summary>
        /// Индекс модуля
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя модуля
        /// <summary>
        /// Имя модуля
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Стартовый адрес
        /// <summary>
        /// Стартовый адрес
        /// </summary>
        public string StartAddress { get; set; }
        #endregion

        #region Конечный адрес
        /// <summary>
        /// Конечный адрес
        /// </summary>
        public string EndAddress { get; set; }
        #endregion

        #region Наименование УСО, к котрому относится модуль
        /// <summary>
        /// Наименование УСО, к котрому относится модуль
        /// </summary>
        public string NameUSO { get; set; }
        #endregion

        #region Цвет УСО
        /// <summary>
        /// Цвет УСО
        /// </summary>
        public string ColorRack { get; set; }
        #endregion

        #region Сигналы модуля
        /// <summary>
        /// Сигналы модуля
        /// </summary>
        public List<Channel> Channel { get; set; } = new();
        #endregion
    }
}
