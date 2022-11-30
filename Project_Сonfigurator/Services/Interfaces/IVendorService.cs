using Project_Сonfigurator.Models.Settings;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IVendorService
    {
        #region Редактирование объекта
        /// <summary>
        /// Редактирование объекта
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        bool Edit(object Item); 
        #endregion

        //#region Открыть окно редактирования данных вендора
        ///// <summary>
        ///// Открыть окно редактирования данных вендора
        ///// </summary>
        ///// <returns></returns>
        //bool EditVendor(Vendor vendor); 
        //#endregion
    }
}
