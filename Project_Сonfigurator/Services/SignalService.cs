using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Services.Interfaces;

namespace Project_Сonfigurator.Services
{
    public class SignalService : ISignalService
    {
        #region Флаг необходимости выбора сигнала
        /// <summary>
        /// Флаг необходимости выбора сигнала
        /// </summary>
        public bool DoSelection { get; set; }
        #endregion

        #region Тип модуля, для выбора сигнала
        /// <summary>
        /// Тип модуля, для выбора сигнала
        /// </summary>
        public TypeModule Type { get; set; }
        #endregion

        #region Имя вкладки, с корой производится выбор сигнала
        /// <summary>
        /// Имя вкладки, с корой производится выбор сигнала
        /// </summary>
        public string ListName { get; set; }
        #endregion

        #region Адрес сигнала
        /// <summary>
        /// Адрес сигнала
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Идентификатор сигнала
        /// <summary>
        /// Идентификатор сигнала
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Наименование сигнала
        /// <summary>
        /// Наименование сигнала
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Сброс параметров
        /// <summary>
        /// Сброс параметров
        /// </summary>
        public void ResetSignal()
        {
            DoSelection = false;
            Type = TypeModule.Unknown;
            ListName = "";
            Address = "";
            Id = "";
            Description = "";
        }
        #endregion
    }
}
