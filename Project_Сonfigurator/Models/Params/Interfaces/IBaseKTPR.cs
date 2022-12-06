using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseKTPR
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

        #region Управления табло и сиренами
        /// <summary>
        /// Управления табло и сиренами
        /// </summary>
        BaseControlUTS ControlUTS { get; set; }
        #endregion

        #region Состояние станции
        /// <summary>
        /// Состояние станции
        /// </summary>
        string StateStation { get; set; }
        #endregion

        #region Принадлежность защиты к группе
        /// <summary>
        /// Принадлежность защиты к группе
        /// </summary>
        string Shoulder { get; set; }
        #endregion

        #region Принадлежность защиты к подгруппе
        /// <summary>
        /// Принадлежность защиты к подгруппе
        /// </summary>
        string SubShoulder { get; set; }
        #endregion

        #region Автодеблок
        /// <summary>
        /// Автодеблок
        /// </summary>
        string Autodeblok { get; set; }
        #endregion

        #region Запрет маскирования
        /// <summary>
        /// Запрет маскирования
        /// </summary>
        string NoMasked { get; set; }
        #endregion

        #region Тип остановки станции
        /// <summary>
        /// Тип остановки станции
        /// </summary>
        string StopTypeNS { get; set; }
        #endregion

        #region Тип остановки МПНА
        /// <summary>
        /// Тип остановки МПНА
        /// </summary>
        string StopTypeUMPNA { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        string Type { get; set; }
        #endregion
    }
}
