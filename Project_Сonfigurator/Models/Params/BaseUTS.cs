using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseUTS : IBaseUTS
    {
        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string Type { get; set; }
        #endregion

        #region Возможность блокировки
        /// <summary>
        /// Возможность блокировки
        /// </summary>
        public string LockEnable { get; set; }
        #endregion

        #region Тип контроля исправности цепей
        /// <summary>
        /// Тип контроля исправности цепей
        /// </summary>
        public string TypeCOz { get; set; }
        #endregion

        #region Номер ПЗ
        /// <summary>
        /// Номер ПЗ
        /// </summary>
        public string IndexPZ { get; set; }
        #endregion

        #region Отсутствие табло "АПТ отключен"
        /// <summary>
        /// Отсутствие табло "АПТ отключен"
        /// </summary>
        public string AptOff { get; set; }
        #endregion

        #region Номер группы
        /// <summary>
        /// Номер группы
        /// </summary>
        public string IndexGroup { get; set; }
        #endregion

        #region Данные параметра
        /// <summary>
        /// Данные параметра
        /// </summary>
        public BaseParam Param { get; set; } = new();
        #endregion

        #region Данные для контроля исправности цепей
        /// <summary>
        /// Данные для контроля исправности цепей
        /// </summary>
        public BaseParam KCO { get; set; } = new();
        #endregion

        #region Данные для датчика прохождения СОД
        /// <summary>
        /// Данные для датчика прохождения СОД
        /// </summary>
        public BaseParam SignalSOD { get; set; } = new();
        #endregion

        #region Данные для датчика прохождения СОД (неисправность)
        /// <summary>
        /// Данные для датчика прохождения СОД (неисправность)
        /// </summary>
        public BaseParam SignalErrSOD { get; set; } = new();
        #endregion
    }
}