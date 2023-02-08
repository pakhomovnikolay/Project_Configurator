using Project_Сonfigurator.Services.Interfaces;

namespace Project_Сonfigurator.Services
{
    public class CyrillicSymbolService : ICyrillicSymbolService
    {
        #region Проверить наличие русских букв
        /// <summary>
        /// Проверить наличие русских букв
        /// </summary>
        /// <param name="_Text"></param>
        /// <returns></returns>
        public bool CheckAvailability(string _Text)
        {
            var u = _Text.ToLower();
            for (int i = 0; i < u.Length; i++)
            {
                var c = u[i];
                if (c >= 'а' && c <= 'я')
                    return true;
            }
            return false;
        }
        #endregion
    }
}
