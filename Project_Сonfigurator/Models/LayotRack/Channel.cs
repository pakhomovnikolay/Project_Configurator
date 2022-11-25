using Project_Сonfigurator.Models.LayotRack.Interfaces;

namespace Project_Сonfigurator.Models.LayotRack
{
    public class Channel : IChannel
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

        #region Описание канала
        /// <summary>
        /// Описание канала
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Бит
        /// <summary>
        /// Бит
        /// </summary>
        public string Bit { get; set; }
        #endregion

        #region Адрес
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Имя переменной для ВУ
        /// <summary>
        /// Имя переменной для ВУ
        /// </summary>
        public string VarNameVU { get; set; }
        #endregion
    }
}