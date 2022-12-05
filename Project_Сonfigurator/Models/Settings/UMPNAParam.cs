using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Settings
{
    public class UMPNAParam : IUMPNAParam
    {
        #region Входые параметры
        /// <summary>
        /// Входые параметры
        /// </summary>
        public List<BaseText> InputParams { get; set; } = new();
        #endregion

        #region Выходые параметры
        /// <summary>
        /// Выходые параметры
        /// </summary>
        public List<BaseText> OutputParams { get; set; } = new();
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        public List<BaseText> Setpoints { get; set; } = new();
        #endregion
    }
}
