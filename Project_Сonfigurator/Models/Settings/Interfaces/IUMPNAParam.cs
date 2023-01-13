using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface IUMPNAParam
    {
        #region Входые параметры
        /// <summary>
        /// Входые параметры
        /// </summary>
        ObservableCollection<BaseText> InputParams { get; set; }
        #endregion

        #region Выходые параметры
        /// <summary>
        /// Выходые параметры
        /// </summary>
        ObservableCollection<BaseText> OutputParams { get; set; }
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        ObservableCollection<BaseSetpoints> Setpoints { get; set; }
        #endregion
    }
}
