namespace Project_Сonfigurator.Models.LayotRack.Interfaces
{
    public interface IChannel
    {
        #region Индекс канала
        /// <summary>
        /// Индекс канала
        /// </summary>
        int Index { get; set; }
        #endregion

        #region Идентификатор канала
        /// <summary>
        /// Идентификатор канала
        /// </summary>
        string Id { get; set; }
        #endregion

        #region Наименование канала
        /// <summary>
        /// Наименование канала
        /// </summary>
        string Name { get; set; }
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
