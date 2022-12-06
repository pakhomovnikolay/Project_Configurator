using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseKGMPNA
    {
        #region Параметры готовностей
        /// <summary>
        /// Параметры готовностей
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        BaseSetpoints Setpoints { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        string NoMasked { get; set; } 
        #endregion
    }
}
