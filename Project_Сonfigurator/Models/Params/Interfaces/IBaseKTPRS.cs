using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseKTPRS
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        BaseSetpoints Setpoints { get; set; }
        #endregion

        #region Управление задвижками
        /// <summary>
        /// Управление задвижками
        /// </summary>
        BaseControlUZD ControlUZD { get; set; }
        #endregion

        #region Управление вспомсистемами
        /// <summary>
        /// Управление вспомсистемами
        /// </summary>
        BaseControlUVS ControlUVS { get; set; }
        #endregion

        #region Управление табло и сиренами
        /// <summary>
        /// Управление табло и сиренами
        /// </summary>
        BaseControlUTS ControlUTS { get; set; }
        #endregion

        #region Тип сигнализации
        /// <summary>
        /// Тип сигнализации
        /// </summary>
        string TypeWarning { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        string Type { get; set; } 
        #endregion
    }
}
