namespace Project_Сonfigurator.Models.Interfaces
{
    public interface IBaseMessage
    {
        #region Порядковый номер сообщений
        /// <summary>
        /// Порядковый номер сообщений
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Описание сообщения
        /// <summary>
        /// Описание сообщения
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Цвет сообщений
        /// <summary>
        /// Цвет сообщений
        /// </summary>
        string Color { get; set; }
        #endregion

        #region Необходимость квитирования
        /// <summary>
        /// Необходимость квитирования
        /// </summary>
        string NeedAck { get; set; }
        #endregion

        #region Путь к файлу воспроизведения звука
        /// <summary>
        /// Путь к файлу воспроизведения звука
        /// </summary>
        string PathSound { get; set; }
        #endregion

        #region Тип сообщения (циклическое\не циклическое)
        /// <summary>
        /// Тип сообщения (циклическое\не циклическое)
        /// </summary>
        string TypeSound { get; set; }
        #endregion

        #region Необходимлсть воспроизведения звука
        /// <summary>
        /// Необходимлсть воспроизведения звука
        /// </summary>
        string NeedPlay { get; set; }
        #endregion

        #region Скрытое сообщение
        /// <summary>
        /// Скрытое сообщение
        /// </summary>
        string Hide { get; set; }
        #endregion

        #region Уровень доступа
        /// <summary>
        /// Уровень доступа
        /// </summary>
        string LevelAccess { get; set; }
        #endregion
    }
}
