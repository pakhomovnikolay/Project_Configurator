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
    }
}
