using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
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

        #region Переописание сигналов
        /// <summary>
        /// Переописание сигналов
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="is_selected"></param>
        public void RedefineSignal(BaseSignal signal, bool is_selected, string title)
        {
            if (is_selected)
            {
                if (DoSelection && !string.IsNullOrWhiteSpace(Address))
                {
                    signal.Address = Address;
                    if (string.IsNullOrWhiteSpace(signal.Id))
                        signal.Id = Id;
                    if (string.IsNullOrWhiteSpace(signal.Description))
                        signal.Description = Description;

                    ResetSignal();
                }
                else if (DoSelection && string.IsNullOrWhiteSpace(Address) && ListName == title)
                    ResetSignal();
            }
            else if (DoSelection && string.IsNullOrWhiteSpace(Address) && ListName != title)
                ResetSignal();
        }
        #endregion

        #region Переописание параметров
        /// <summary>
        /// Переописание параметров
        /// </summary>
        public void RedefineParam(BaseParam param, bool is_selected, string title)
        {
            if (is_selected)
            {
                if (DoSelection && !string.IsNullOrWhiteSpace(Address))
                {
                    param.Address = Address;
                    if (string.IsNullOrWhiteSpace(param.Id))
                        param.Id = Id;
                    if (string.IsNullOrWhiteSpace(param.Description))
                        param.Description = Description;

                    ResetSignal();
                }
                else if (DoSelection && string.IsNullOrWhiteSpace(Address) && ListName == title)
                    ResetSignal();
            }
            else if (DoSelection && string.IsNullOrWhiteSpace(Address) && ListName != title)
                ResetSignal();
        }
        #endregion
    }
}