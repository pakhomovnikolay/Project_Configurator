namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IEditService
    {
        #region Редактирование объекта
        /// <summary>
        /// Редактирование объекта
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        bool Edit(object Item, string title = null);
        #endregion
    }
}
