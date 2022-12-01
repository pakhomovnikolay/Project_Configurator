using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseParam : IBaseParam
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Описание параметра
        /// <summary>
        /// Описание параметра
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Инверсия
        /// <summary>
        /// Инверсия
        /// </summary>
        public string Inv { get; set; }
        #endregion

        #region Тип сигнала
        /// <summary>
        /// Тип сигнала
        /// </summary>
        public string TypeSignal { get; set; }
        #endregion

        #region Смещенеие
        /// <summary>
        /// Смещенеие
        /// </summary>
        public string Address { get; set; }
        #endregion
    }
}
