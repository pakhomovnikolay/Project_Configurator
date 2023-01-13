using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class RackModule : IModule
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя модуля
        private string _Name = "";
        /// <summary>
        /// Имя модуля
        /// </summary>
        //public string Name { get; set; }
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                Type = TypeModule.Unknown;

                #region AI
                if (_Name.Contains("ai", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("ahi", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("ami", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("art", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = TypeModule.AI;
                }
                else if (_Name.Contains("di", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("dai", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("ddi", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("ert", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = TypeModule.DI;
                }
                else if (_Name.Contains("ao", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("aho", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("amo", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("amm", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = TypeModule.AO;
                }
                else if (_Name.Contains("do", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("dao", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("ddo", StringComparison.CurrentCultureIgnoreCase) ||
                     _Name.Contains("ddm", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = TypeModule.DO;
                }
                else if (_Name.Contains("da", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("drc", StringComparison.CurrentCultureIgnoreCase) ||
                    _Name.Contains("dra", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = TypeModule.DA;
                }
                #endregion

            }
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

        #region Тип модуля
        /// <summary>
        /// Тип модуля
        /// </summary>
        public TypeModule Type { get; set; }
        #endregion

        #region Каналы модуля
        /// <summary>
        /// Каналы модуля
        /// </summary>
        public ObservableCollection<Channel> Channels { get; set; } = new();
        #endregion
    }
}
