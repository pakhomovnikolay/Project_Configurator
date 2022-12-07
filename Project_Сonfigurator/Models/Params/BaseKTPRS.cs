using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseKTPRS : IBaseKTPRS
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        public BaseParam Param { get; set; }
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        public BaseSetpoints Setpoints { get; set; }
        #endregion

        #region Управление задвижками
        /// <summary>
        /// Управление задвижками
        /// </summary>
        public BaseControlUZD ControlUZD { get; set; }
        #endregion

        #region Управление вспомсистемами
        /// <summary>
        /// Управление вспомсистемами
        /// </summary>
        public BaseControlUVS ControlUVS { get; set; }
        #endregion

        #region Управление табло и сиренами
        /// <summary>
        /// Управление табло и сиренами
        /// </summary>
        public BaseControlUTS ControlUTS { get; set; }
        #endregion

        #region Тип сигнализации
        /// <summary>
        /// Тип сигнализации
        /// </summary>
        public string TypeWarning { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string Type { get; set; }
        #endregion
    }
}