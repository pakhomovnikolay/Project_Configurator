using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseKTPRA : IBaseKTPRA
    {
        #region Параметры защит
        /// <summary>
        /// Параметры защит
        /// </summary>
        public BaseParam Param { get; set; } = new();
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        public BaseSetpoints Setpoints { get; set; } = new();
        #endregion

        #region Управления задвижками
        /// <summary>
        /// Управления задвижками
        /// </summary>
        public BaseControlUZD ControlUZD { get; set; } = new();
        #endregion

        #region Управления вспомсистемами
        /// <summary>
        /// Управления вспомсистемами
        /// </summary>
        public BaseControlUVS ControlUVS { get; set; } = new();
        #endregion

        #region Состояние НА
        /// <summary>
        /// Состояние НА
        /// </summary>
        public string StateUMPNA { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        public string NoMasked { get; set; }
        #endregion

        #region АВР
        /// <summary>
        /// АВР
        /// </summary>
        public string AVR { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string Type { get; set; }
        #endregion

        #region Тип остановки МПНА
        /// <summary>
        /// Тип остановки МПНА
        /// </summary>
        public string StopType { get; set; }
        #endregion
    }
}
