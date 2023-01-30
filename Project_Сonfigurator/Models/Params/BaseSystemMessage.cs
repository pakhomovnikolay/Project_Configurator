using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseSystemMessage : IBaseSystemMessage
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Имя системы сообщения
        /// <summary>
        /// Имя системы сообщения
        /// </summary>
        public string SystemMessage { get; set; }
        #endregion

        #region Описание сообщения
        /// <summary>
        /// Описание сообщения
        /// </summary>
        public string DescriptionMessage { get; set; }
        #endregion

        #region Описание сиситемы
        /// <summary>
        /// Описание сиситемы
        /// </summary>
        public string DescriptionSystem { get; set; }
        #endregion

        #region Наименование листа подсистемы
        /// <summary>
        /// Наименование листа подсистемы
        /// </summary>
        public string NameTabList { get; set; }
        #endregion
    }
}
