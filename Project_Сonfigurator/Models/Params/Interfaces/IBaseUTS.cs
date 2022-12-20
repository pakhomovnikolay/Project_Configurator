namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseUTS
    {
        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        string Type { get; set; }
        #endregion

        #region Возможность блокировки
        /// <summary>
        /// Возможность блокировки
        /// </summary>
        string LockEnable { get; set; }
        #endregion

        #region Тип контроля исправности цепей
        /// <summary>
        /// Тип контроля исправности цепей
        /// </summary>
        string TypeCOz { get; set; }
        #endregion

        #region Номер ПЗ
        /// <summary>
        /// Номер ПЗ
        /// </summary>
        string IndexPZ { get; set; }
        #endregion

        #region Отсутствие табло "АПТ отключен"
        /// <summary>
        /// Отсутствие табло "АПТ отключен"
        /// </summary>
        string AptOff { get; set; }
        #endregion

        #region Номер группы
        /// <summary>
        /// Номер группы
        /// </summary>
        string IndexGroup { get; set; }
        #endregion

        #region Данные параметра
        /// <summary>
        /// Данные параметра
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Данные для контроля исправности цепей
        /// <summary>
        /// Данные для контроля исправности цепей
        /// </summary>
        BaseParam KCO { get; set; }
        #endregion

        #region Данные для датчика прохождения СОД
        /// <summary>
        /// Данные для датчика прохождения СОД
        /// </summary>
        BaseParam SignalSOD { get; set; }
        #endregion

        #region Данные для датчика прохождения СОД (неисправность)
        /// <summary>
        /// Данные для датчика прохождения СОД (неисправность)
        /// </summary>
        BaseParam SignalErrSOD { get; set; } 
        #endregion
    }
}
