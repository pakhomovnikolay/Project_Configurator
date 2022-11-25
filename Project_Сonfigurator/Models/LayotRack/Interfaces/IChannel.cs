namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IChannel
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
        #endregion

        #region Описание канала
        /// <summary>
        /// Описание канала
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        string VarName { get; set; }
        #endregion

        #region Бит
        /// <summary>
        /// Бит
        /// </summary>
        string Bit { get; set; }
        #endregion

        #region Адрес
        /// <summary>
        /// Адрес
        /// </summary>
        string Address { get; set; }
        #endregion

        #region Имя переменной для ВУ
        /// <summary>
        /// Имя переменной для ВУ
        /// </summary>
        string VarNameVU { get; set; }
        #endregion
    }
}
