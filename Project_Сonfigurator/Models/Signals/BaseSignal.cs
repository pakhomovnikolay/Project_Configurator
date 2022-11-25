using Project_Сonfigurator.Models.Signals.Interfaces;

namespace Project_Сonfigurator.Models.Signals
{
    public class BaseSignal : IBaseSignal
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Описание сигнала
        /// <summary>
        ///  Описание сигнала
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Область
        /// <summary>
        /// Область (физический\формируемый сигнал) 
        /// </summary>
        public string Area { get; set; }
        #endregion

        #region Смещение
        /// <summary>
        /// Смещение. Ссылка на сигнал
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Значание ссылки
        /// <summary>
        /// Значание ссылки. Формируется с учетом области
        /// </summary>
        public string LinkValue { get; set; }
        #endregion
    }
}
