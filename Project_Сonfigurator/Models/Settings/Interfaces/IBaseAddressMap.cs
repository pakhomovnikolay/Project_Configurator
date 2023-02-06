namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface IBaseAddressMap
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Описание переменной
        /// <summary>
        /// Описание переменной
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Путь к переменной
        /// <summary>
        /// Путь к переменной
        /// </summary>
        string PathTag { get; set; }
        #endregion

        #region Длина (WORD)
        /// <summary>
        /// Длина (WORD)
        /// </summary>
        string LengthWord { get; set; }
        #endregion

        #region Длина (BYTE)
        /// <summary>
        /// Длина (BYTE)
        /// </summary>
        string LengthByte { get; set; }
        #endregion

        #region Стартовый адрес
        /// <summary>
        /// Стартовый адрес
        /// </summary>
        string AddressStart { get; set; }
        #endregion

        #region Конечный адрес
        /// <summary>
        /// Конечный адрес
        /// </summary>
        string AddressEnd { get; set; }
        #endregion

        #region Адрес в ПЛК
        /// <summary>
        /// Адрес в ПЛК
        /// </summary>
        string AddressInPLC { get; set; }
        #endregion
    }
}
