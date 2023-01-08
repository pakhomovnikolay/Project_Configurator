using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Interfaces
{
    public interface ICollectionMessage
    {
        #region Описание коллекции сообщения
        /// <summary>
        /// Описание коллекции сообщения
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Номер подсистемы
        /// <summary>
        /// Номер подсистемы
        /// </summary>
        string IndexSystem { get; set; }
        #endregion

        #region Коллекция сообщений
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        List<BaseMessage> Messages { get; set; }
        #endregion
    }
}
