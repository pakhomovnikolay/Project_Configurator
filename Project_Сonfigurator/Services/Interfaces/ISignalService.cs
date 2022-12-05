using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ISignalService
    {
        #region Флаг необходимости выбора сигнала
        /// <summary>
        /// Флаг необходимости выбора сигнала
        /// </summary>
        bool DoSelection { get; set; }
        #endregion

        #region Тип модуля, для выбора сигнала
        /// <summary>
        /// Тип модуля, для выбора сигнала
        /// </summary>
        TypeModule Type { get; set; }
        #endregion

        #region Имя вкладки, с которой производится выбор сигнала
        /// <summary>
        /// Имя вкладки, с которой производится выбор сигнала
        /// </summary>
        string ListName { get; set; }
        #endregion

        #region Адрес сигнала
        /// <summary>
        /// Адрес сигнала
        /// </summary>
        string Address { get; set; }
        #endregion

        #region Идентификатор сигнала
        /// <summary>
        /// Идентификатор сигнала
        /// </summary>
        string Id { get; set; }
        #endregion

        #region Наименование сигнала
        /// <summary>
        /// Наименование сигнала
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Сброс параметров
        /// <summary>
        /// Сброс параметров
        /// </summary>
        void ResetSignal();
        #endregion

        #region Переописание сигналов
        /// <summary>
        /// Переописание сигналов
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        void RedefineSignal(BaseSignal signal, bool is_selected, string title);
        #endregion

        #region Переописание параметров
        /// <summary>
        /// Переописание параметров
        /// </summary>
        /// <param name="param"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        void RedefineParam(BaseParam param, bool is_selected, string title);
        #endregion
    }
}