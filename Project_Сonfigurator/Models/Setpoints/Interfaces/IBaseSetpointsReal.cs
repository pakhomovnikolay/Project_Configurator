namespace Project_Сonfigurator.Models.Setpoints.Interfaces
{
    public interface IBaseSetpointsReal
    {
        #region Количество знаков после запятой
		/// <summary>
        /// Количество знаков после запятой
        /// </summary>
        string QtySimbolsComma { get; set; }
        #endregion

        #region Данные уставки
        /// <summary>
        /// Данные уставки
        /// </summary>
        BaseSetpoints Setpoints { get; set; } 
        #endregion
    }
}
