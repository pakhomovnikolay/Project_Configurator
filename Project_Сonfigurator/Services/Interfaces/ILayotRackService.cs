using Project_Сonfigurator.Models.LayotRack;
using System.Collections.Generic;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ILayotRackService
    {
        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="Modules"></param>
        /// <param name="IndexRack"></param>
        void RefreshIndexModule(List<RackModule> Modules, int IndexRack);
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="USOList"></param>
        /// <param name="IndexRack"></param>
        void RefreshAddressModule(List<USO> USOList, int IndexRack = 0);
        #endregion
    }
}
