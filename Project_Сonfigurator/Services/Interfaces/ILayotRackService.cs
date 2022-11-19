using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using System.Collections.Generic;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ILayotRackService
    {
        #region Обновление корзин
        /// <summary>
        /// Обновление корзин
        /// </summary>
        /// <param name="Racks"></param>
        void RefreshRack(List<Rack> Racks);
        #endregion

        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="Racks"></param>
        void RefreshIndexModule(List<Rack> Racks);
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="Racks"></param>
        void RefreshAddressModule(List<Rack> Racks);
        #endregion

        #region Наполнение списка модулей
        /// <summary>
        /// Наполнение списка модулей
        /// </summary>
        /// <param name="Racks"></param>
        List<string> GetListModules(TypePLC typePLC);
        #endregion
    }
}
