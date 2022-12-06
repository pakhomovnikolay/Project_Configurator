using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseKGMPNA : IBaseKGMPNA
    {
        #region Параметры готовностей
        /// <summary>
        /// Параметры готовностей
        /// </summary>
        public BaseParam Param { get; set; }
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        public BaseSetpoints Setpoints { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        public string NoMasked { get; set; }
        #endregion
    }
}
