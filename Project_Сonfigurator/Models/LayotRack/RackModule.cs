using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class RackModule : IModule
    {
        #region Тип модуля
        /// <summary>
        /// Тип модуля
        /// </summary>
        public TypeModule Type { get; set; }
        #endregion

        #region Индекс модуля
        /// <summary>
        /// Индекс модуля
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя модуля
        private string _Name = "";
        /// <summary>
        /// Имя модуля
        /// </summary>
        public string Name
        {
            get
            {
                #region Модули DI
                if (_Name.Contains("DI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DAI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DDI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DDM", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DRA", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DRC", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("ERT", StringComparison.CurrentCultureIgnoreCase)
                        )
                    Type = TypeModule.DI;
                #endregion

                #region Модули AI
                else if (_Name.Contains("AI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("AHI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("AMI", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("AMM", StringComparison.CurrentCultureIgnoreCase)
                        )
                    Type = TypeModule.AI;
                #endregion

                #region Модули DO
                else if (_Name.Contains("DO", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DAO", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("DDO", StringComparison.CurrentCultureIgnoreCase)
                        )
                    Type = TypeModule.DO;
                #endregion

                #region Модули AO
                else if (_Name.Contains("AO", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("AMO", StringComparison.CurrentCultureIgnoreCase) ||
                        _Name.Contains("AHO", StringComparison.CurrentCultureIgnoreCase)
                        )
                    Type = TypeModule.AO;
                #endregion

                return _Name;
            }
            set => _Name = value;
        }
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
        public string ColorUSO { get; set; }
        #endregion

        #region Каналы модуля
        /// <summary>
        /// Каналы модуля
        /// </summary>
        public List<Channel> Channels { get; set; } = new();
        #endregion
    }
}
