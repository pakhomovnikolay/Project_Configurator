namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IGroupSignal
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Количество сработок в группе
        /// <summary>
        /// Количество сработок в группе
        /// </summary>
        string QtyInGroup { get; set; }
        #endregion

        #region Сигналы в группе "От"
        /// <summary>
        /// Сигналы в группе "От"
        /// </summary>
        string AddressStart { get; set; }
        #endregion

        #region Сигналы в группе "До"
        /// <summary>
        /// Сигналы в группе "До"
        /// </summary>
        string AddressEnd { get; set; }
        #endregion
    }
}
