using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using System.Collections.ObjectModel;

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

        #region Тип адреса, для выбора сигнала
        /// <summary>
        /// Тип адреса, для выбора сигнала
        /// </summary>
        TypeAddress Type { get; set; }
        #endregion

        #region Имя вкладки, с которой производится выбор сигнала
        /// <summary>
        /// Имя вкладки, с которой производится выбор сигнала
        /// </summary>
        string FromName { get; set; }
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
        bool ResetSignal();
        #endregion

        #region Переописание сигналов
        /// <summary>
        /// Переописание сигналов
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        bool RedefineAddress(BaseSignal signal, bool is_selected, string title);
        #endregion

        #region Переописание параметров
        /// <summary>
        /// Переописание параметров
        /// </summary>
        /// <param name="param"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        bool RedefineAddress(BaseParam param, bool is_selected, string title);
        #endregion

        #region Смена адреса ввода
        /// <summary>
        /// Смена адреса ввода
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="TypeSignal"></param>
        /// <returns></returns>
        string ChangeAddress(string FromTitle, string TypeSignal);
        #endregion

        #region Смена адреса вывода
        /// <summary>
        ///  Смена адреса вывода
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="_TypeModule"></param>
        /// <returns></returns>
        string ChangeAddress(string FromTitle, TypeAddress _TypeAddress);
        #endregion

        #region Смена адреса сигнала В\В
        /// <summary>
        /// Смена адреса сигнала В\В
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="_TypeModule"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        string ChangeAddress(string FromTitle, TypeAddress _TypeAddress, string Area);
        #endregion

        #region Выбор сигнала
        /// <summary>
        /// Выбор сигнала
        /// </summary>
        /// <param name="_Signal"></param>
        void SelecteAddress(BaseSignal _Signal);
        #endregion

        #region Выбор канала
        /// <summary>
        /// Выбор канала
        /// </summary>
        /// <param name="Channels"></param>
        /// <param name="Shift"></param>
        void SelecteAddress(ObservableCollection<Channel> Channels, int _Address);
        #endregion

        #region Выбор параметра
        /// <summary>
        /// Выбор параметра
        /// </summary>
        /// <param name="_Param"></param>
        void SelecteAddress(BaseParam _Param);
        #endregion

        #region Проверка корректного выбора адреса
        /// <summary>
        /// Проверка корректного выбора адреса
        /// </summary>
        /// <param name="_TypeAddress"></param>
        /// <param name="Address"></param>
        /// <returns></returns>
        bool CheckCurrentSelectedSignal(int Address, out string message); 
        #endregion
    }
}