using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface IUZDParam
    {
        #region Входые параметры
        /// <summary>
        /// Входые параметры
        /// </summary>
        List<BaseText> InputParams { get; set; }
        #endregion

        #region Выходые параметры
        /// <summary>
        /// Выходые параметры
        /// </summary>
        List<BaseText> OutputParams { get; set; }
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        List<BaseSetpoints> Setpoints { get; set; }
        #endregion
    }
}