using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseSignaling : IBaseSignaling
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        public BaseParam Param { get; set; }
        #endregion

        #region Цвет сообщения
        /// <summary>
        /// Цвет сообщения
        /// </summary>
        public string Color { get; set; }
        #endregion

        #region Номер шкафа, если это служебный сигнал
        /// <summary>
        /// Номер шкафа, если это служебный сигнал
        /// </summary>
        public string IndexUSO { get; set; }
        #endregion

        #region Настройка сигнализации
        /// <summary>
        /// Настройка сигнализации
        /// </summary>
        public string TypeWarning { get; set; }
        #endregion

        #region Имя переменной для ВУ
        /// <summary>
        /// Имя переменной для ВУ
        /// </summary>
        public string VarNameVU { get; set; }
        #endregion
    }
}
