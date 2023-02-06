using Project_Сonfigurator.Models.Interfaces;
using System.Collections.ObjectModel;

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

        #region Номер системы
        /// <summary>
        /// Номер системы
        /// </summary>
        public string IndexSystem { get; set; }
        #endregion

        #region Имя системы
        /// <summary>
        /// Имя системы
        /// </summary>
        public string NameSystem { get; set; }
        #endregion

        #region Коллекция сообщений
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        public ObservableCollection<BaseMessage> Messages { get; set; } = new();
        #endregion

        #region Текст фильтрации
        /// <summary>
        /// Текст фильтрации
        /// </summary>
        public string TextFilter { get; set; }
        #endregion
    }
}
