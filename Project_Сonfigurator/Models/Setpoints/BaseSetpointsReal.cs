using Project_Сonfigurator.Models.Setpoints.Interfaces;

namespace Project_Сonfigurator.Models.Setpoints
{
    public class BaseSetpointsReal : IBaseSetpointsReal
    {
        #region Количество знаков после запятой
        /// <summary>
        /// Количество знаков после запятой
        /// </summary>
        public string QtySimbolsComma { get; set; }
        #endregion

        #region Данные уставки
        /// <summary>
        /// Данные уставки
        /// </summary>
        public BaseSetpoints Setpoints { get; set; }
        #endregion
    }
}
