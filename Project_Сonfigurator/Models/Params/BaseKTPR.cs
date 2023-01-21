using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseKTPR : IBaseKTPR
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

        #region Управления табло и сиренами
        /// <summary>
        /// Управления табло и сиренами
        /// </summary>
        public BaseControlUTS ControlUTS { get; set; } = new();
        #endregion

        #region Состояние станции
        /// <summary>
        /// Состояние станции
        /// </summary>
        public string StateStation { get; set; }
        #endregion

        #region Принадлежность защиты к группе
        /// <summary>
        /// Принадлежность защиты к группе
        /// </summary>
        public string Shoulder { get; set; }
        #endregion

        #region Принадлежность защиты к подгруппе
        /// <summary>
        /// Принадлежность защиты к подгруппе
        /// </summary>
        public string SubShoulder { get; set; }
        #endregion

        #region Автодеблок
        /// <summary>
        /// Автодеблок
        /// </summary>
        public string Autodeblok { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        public string NoMasked { get; set; }
        #endregion

        #region Тип остановки станции
        /// <summary>
        /// Тип остановки станции
        /// </summary>
        public string StopTypeNS { get; set; }
        #endregion

        #region Тип остановки МПНА
        /// <summary>
        /// Тип остановки МПНА
        /// </summary>
        public string StopTypeUMPNA { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string Type { get; set; }
        #endregion
    }
}
