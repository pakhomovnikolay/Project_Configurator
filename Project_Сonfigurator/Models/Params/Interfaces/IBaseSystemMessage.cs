namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseSystemMessage
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Имя системы сообщения
        /// <summary>
        /// Имя системы сообщения
        /// </summary>
        string SystemMessage { get; set; }
        #endregion

        #region Описание сообщения
        /// <summary>
        /// Описание сообщения
        /// </summary>
        string DescriptionMessage { get; set; }
        #endregion

        #region Описание сиситемы
        /// <summary>
        /// Описание сиситемы
        /// </summary>
        string DescriptionSystem { get; set; }
        #endregion

        #region Наименование листа подсистемы
        /// <summary>
        /// Наименование листа подсистемы
        /// </summary>
        string NameTabList { get; set; } 
        #endregion
    }
}
