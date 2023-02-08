namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ICyrillicSymbolService
    {
        #region Проверить наличие русских букв
        /// <summary>
        /// Проверить наличие русских букв
        /// </summary>
        /// <param name="_Text"></param>
        /// <returns></returns>
        bool CheckAvailability(string _Text); 
        #endregion
    }
}
