using System.Collections.ObjectModel;

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

        #region Номер системы
        /// <summary>
        /// Номер системы
        /// </summary>
        string IndexSystem { get; set; }
        #endregion

        #region Имя системы
        /// <summary>
        /// Имя системы
        /// </summary>
        string NameSystem { get; set; }
        #endregion

        #region Коллекция сообщений
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        ObservableCollection<BaseMessage> Messages { get; set; }
        #endregion

        #region Текст фильтрации
        /// <summary>
        /// Текст фильтрации
        /// </summary>
        string TextFilter { get; set; }
        #endregion
    }
}
