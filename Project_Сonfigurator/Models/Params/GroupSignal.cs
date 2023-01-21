using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class GroupSignal : IGroupSignal
    {
        #region Параметры
        /// <summary>
        /// Параметры
        /// </summary>
        public BaseParam Param { get; set; } = new();
        #endregion

        #region Количество сработок в группе
        /// <summary>
        /// Количество сработок в группе
        /// </summary>
        public string QtyInGroup { get; set; }
        #endregion

        #region Сигналы в группе "От"
        /// <summary>
        /// Сигналы в группе "От"
        /// </summary>
        public string AddressStart { get; set; }
        #endregion

        #region Сигналы в группе "До"
        /// <summary>
        /// Сигналы в группе "До"
        /// </summary>
        public string AddressEnd { get; set; }
        #endregion
    }
}
