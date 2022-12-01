using Project_Сonfigurator.Models.Setpoints.Interfaces;

namespace Project_Сonfigurator.Models.Setpoints
{
    public class BaseSetpoints : IBaseSetpoints
    {
        #region Индекс уставки
        /// <summary>
        /// Индекс уставки
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Описание уставки
        /// <summary>
        /// Описание уставки
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Адрес
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Значение уставки
        /// <summary>
        /// Значение уставки
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Ед. измерения
        /// <summary>
        /// Ед. измерения
        /// </summary>
        public string Unit { get; set; }
        #endregion
    }
}
