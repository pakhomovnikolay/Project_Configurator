namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseSignaling
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Цвет сообщения
        /// <summary>
        /// Цвет сообщения
        /// </summary>
        string Color { get; set; }
        #endregion

        #region Номер шкафа, если это служебный сигнал
        /// <summary>
        /// Номер шкафа, если это служебный сигнал
        /// </summary>
        string IndexUSO { get; set; }
        #endregion

        #region Настройка сигнализации
        /// <summary>
        /// Настройка сигнализации
        /// </summary>
        string TypeWarning { get; set; }
        #endregion

        #region Имя переменной для ВУ
        /// <summary>
        /// Имя переменной для ВУ
        /// </summary>
        string VarNameVU { get; set; } 
        #endregion
    }
}