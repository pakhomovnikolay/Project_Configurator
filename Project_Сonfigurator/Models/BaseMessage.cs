using Project_Сonfigurator.Models.Interfaces;

namespace Project_Сonfigurator.Models
{
    public class BaseMessage : IBaseMessage
    {
        #region Порядковый номер сообщений
        /// <summary>
        /// Порядковый номер сообщений
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Описание сообщения
        /// <summary>
        /// Описание сообщения
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Цвет сообщений
        /// <summary>
        /// Цвет сообщений
        /// </summary>
        public string Color { get; set; }
        #endregion

        #region Необходимость квитирования
        /// <summary>
        /// Необходимость квитирования
        /// </summary>
        public string NeedAck { get; set; }
        #endregion

        #region Путь к файлу воспроизведения звука
        /// <summary>
        /// Путь к файлу воспроизведения звука
        /// </summary>
        public string PathSound { get; set; }
        #endregion

        #region Тип сообщения (циклическое\не циклическое)
        /// <summary>
        /// Тип сообщения (циклическое\не циклическое)
        /// </summary>
        public string TypeSound { get; set; }
        #endregion

        #region Необходимлсть воспроизведения звука
        /// <summary>
        /// Необходимлсть воспроизведения звука
        /// </summary>
        public string NeedPlay { get; set; }
        #endregion

        #region Скрытое сообщение
        /// <summary>
        /// Скрытое сообщение
        /// </summary>
        public string Hide { get; set; }
        #endregion

        #region Уровень доступа
        /// <summary>
        /// Уровень доступа
        /// </summary>
        public string LevelAccess { get; set; }
        #endregion
    }
}
