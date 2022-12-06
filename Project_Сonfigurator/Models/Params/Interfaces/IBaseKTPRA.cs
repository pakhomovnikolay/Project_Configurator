using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseKTPRA
    {
        #region Параметры защит
        /// <summary>
        /// Параметры защит
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        BaseSetpoints Setpoints { get; set; }
        #endregion

        #region Управления задвижками
        /// <summary>
        /// Управления задвижками
        /// </summary>
        BaseControlUZD ControlUZD { get; set; }
        #endregion

        #region Управления вспомсистемами
        /// <summary>
        /// Управления вспомсистемами
        /// </summary>
        BaseControlUVS ControlUVS { get; set; } 
        #endregion

        #region Состояние НА
        /// <summary>
        /// Состояние НА
        /// </summary>
        string StateUMPNA { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        string NoMasked { get; set; }
        #endregion

        #region АВР
        /// <summary>
        /// АВР
        /// </summary>
        string AVR { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        string Type { get; set; }
        #endregion

        #region Тип остановки МПНА
        /// <summary>
        /// Тип остановки МПНА
        /// </summary>
        string StopType { get; set; } 
        #endregion
    }
}
