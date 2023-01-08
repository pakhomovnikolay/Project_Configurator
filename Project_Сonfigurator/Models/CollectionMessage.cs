using Project_Сonfigurator.Models.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models
{
    public class CollectionMessage : ICollectionMessage
    {
        #region Описание коллекции сообщения
        /// <summary>
        /// Описание коллекции сообщения
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Номер подсистемы
        /// <summary>
        /// Номер подсистемы
        /// </summary>
        public string IndexSystem { get; set; }
        #endregion

        #region Коллекция сообщений
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        public List<BaseMessage> Messages { get; set; } = new();
        #endregion
    }
}
