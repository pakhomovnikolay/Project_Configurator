using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using System.Collections.ObjectModel;

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

        #region Тип адреса, для выбора сигнала
        /// <summary>
        /// Тип адреса, для выбора сигнала
        /// </summary>
        public TypeAddress Type { get; set; }
        #endregion

        #region Имя вкладки, с корой производится выбор сигнала
        /// <summary>
        /// Имя вкладки, с корой производится выбор сигнала
        /// </summary>
        public string FromName { get; set; }
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
        public bool ResetSignal()
        {
            DoSelection = false;
            Type = TypeAddress.Unknown;
            FromName = "";
            Address = "";
            Id = "";
            Description = "";
            return false;
        }
        #endregion

        #region Переописание сигналов
        /// <summary>
        /// Переописание сигналов
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        public bool RedefineAddress(BaseSignal signal, bool is_selected, string title)
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

                    return ResetSignal();
                }
                else if (DoSelection && string.IsNullOrWhiteSpace(Address) && FromName == title)
                    return ResetSignal();
            }
            else if (DoSelection && string.IsNullOrWhiteSpace(Address) && FromName != title)
                return ResetSignal();

            return DoSelection;
        }
        #endregion

        #region Переописание параметров
        /// <summary>
        /// Переописание параметров
        /// </summary>
        /// <param name="param"></param>
        /// <param name="is_selected"></param>
        /// <param name="title"></param>
        public bool RedefineAddress(BaseParam param, bool is_selected, string title)
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

                    return ResetSignal();
                }
                else if (DoSelection && string.IsNullOrWhiteSpace(Address) && FromName == title)
                    return ResetSignal();
            }
            else if (DoSelection && string.IsNullOrWhiteSpace(Address) && FromName != title)
                return ResetSignal();

            return DoSelection;
        }
        #endregion

        #region Смена адреса ввода
        /// <summary>
        /// Смена адреса ввода
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="TypeSignal"></param>
        /// <returns></returns>
        public string ChangeAddress(string FromTitle, string TypeSignal)
        {
            DoSelection = true;
            FromName = FromTitle;
            var ToName = "";

            if (string.IsNullOrWhiteSpace(TypeSignal) || int.Parse(TypeSignal) == 0) { ToName = "Сигналы DI"; Type = TypeAddress.DI; }
            else if (int.Parse(TypeSignal) > 1) { ToName = "Сигналы AI"; Type = TypeAddress.AI; }
            else if (int.Parse(TypeSignal) > 0) { ToName = "Группы сигналов"; Type = TypeAddress.GrpSig; }

            return ToName;
        }
        #endregion

        #region Смена адреса вывода
        /// <summary>
        ///  Смена адреса вывода
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="_TypeModule"></param>
        /// <returns></returns>
        public string ChangeAddress(string FromTitle, TypeAddress _TypeAddress)
        {
            DoSelection = true;
            FromName = FromTitle;
            Type = _TypeAddress;
            var ToName = "";

            if (Type == TypeAddress.DO) { ToName = "Сигналы DO"; }
            else if (Type == TypeAddress.AO) { ToName = "Сигналы AO"; }
            else if (Type == TypeAddress.GrpSig) { ToName = "Сигналы групп"; }

            return ToName;

        }
        #endregion

        #region Смена адреса сигнала В\В
        /// <summary>
        /// Смена адреса сигнала В\В
        /// </summary>
        /// <param name="FromTitle"></param>
        /// <param name="_TypeModule"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        public string ChangeAddress(string FromTitle, TypeAddress _TypeAddress, string Area)
        {
            DoSelection = true;
            FromName = FromTitle;
            Type = _TypeAddress;
            var ToName = "";

            if (string.IsNullOrWhiteSpace(Area) || int.Parse(Area) == 0) { ToName = "Таблица сигналов"; }
            else if (int.Parse(Area) > 0)
            {
                if (Type == TypeAddress.AI) { ToName = "AI формируемые"; }
                else if (Type == TypeAddress.DI) { ToName = "DI формируемые"; }
            }

            return ToName;
        }
        #endregion

        #region Выбор сигнала
        /// <summary>
        /// Выбор сигнала
        /// </summary>
        /// <param name="_Signal"></param>
        public void SelecteAddress(BaseSignal _Signal)
        {
            Address = _Signal.Index;
            Id = _Signal.Id;
            Description = _Signal.Description;
        }
        #endregion

        #region Выбор канала
        /// <summary>
        /// Выбор канала
        /// </summary>
        /// <param name="Channels"></param>
        /// <param name="Shift"></param>
        public void SelecteAddress(ObservableCollection<Channel> Channels, int _Address)
        {
            foreach (var _Channel in Channels)
            {
                if (_Channel.Address == _Address.ToString())
                {
                    Id = _Channel.Id;
                    Description = _Channel.Description;

                    if (Type == TypeAddress.AI) Address = $"{_Address}";
                    else if (Type == TypeAddress.DI) Address = $"{_Address - 100000}";
                    else if (Type == TypeAddress.AO) Address = $"{_Address - 300000}";
                    else if (Type == TypeAddress.DO) Address = $"{_Address - 200000}";
                }
            }
        }
        #endregion

        #region Выбор параметра
        /// <summary>
        /// Выбор параметра
        /// </summary>
        /// <param name="_Param"></param>
        public void SelecteAddress(BaseParam _Param)
        {
            Address = _Param.Index;
            Id = _Param.Id;
            Description = _Param.Description;
        }
        #endregion

        #region Проверка корректного выбора адреса
        /// <summary>
        /// Проверка корректного выбора адреса
        /// </summary>
        /// <param name="_TypeAddress"></param>
        /// <param name="Address"></param>
        /// <returns></returns>
        public bool CheckCurrentSelectedSignal(int Address, out string message)
        {

            message = "";
            if (Type == TypeAddress.AI)
            {
                if (Address >= 100000)
                {
                    message =
                            "Выбор неверный!\n" +
                            "Вы выбрали не аналоговый входной сигнал.\n" +
                            "Запрашивается ссылка на аналоговый входной сигнал (0-99999)" +
                            "Повторить выбор?";
                    return false;
                }
            }
            else if (Type == TypeAddress.DI)
            {
                if (Address < 100000 || Address >= 200000)
                {
                    message =
                        "Выбор неверный!\n" +
                        "Вы выбрали не дискретный входной сигнал.\n" +
                        "Запрашивается ссылка на дискретный входной сигнал (100000-199999)" +
                        "Повторить выбор?";
                    return false;
                }
            }
            else if (Type == TypeAddress.AO)
            {
                if (Address < 300000 || Address >= 400000)
                {
                    message =
                        "Выбор неверный!\n" +
                        "Вы выбрали не аналоговый выходной сигнал.\n" +
                        "Запрашивается ссылка на аналоговый выходной сигнал (300000-399999)" +
                        "Повторить выбор?";
                    return false;
                }
            }
            else if (Type == TypeAddress.DO)
            {
                if (Address < 200000 || Address >= 300000)
                {
                    message =
                        "Выбор неверный!\n" +
                        "Вы выбрали не дискретный выходной сигнал.\n" +
                        "Запрашивается ссылка на аналоговый входной сигнал (200000-299999)" +
                        "Повторить выбор?";
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}