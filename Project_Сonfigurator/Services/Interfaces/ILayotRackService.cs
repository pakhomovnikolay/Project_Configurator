using Project_Сonfigurator.Models.LayotRack;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ILayotRackService
    {
        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="SelectedUSO"></param>
        void RefreshIndexModule(USO SelectedUSO);
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="USOList"></param>
        void RefreshAddressModule(ObservableCollection<USO> USOList);
        #endregion
    }
}
