using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Settings
{
    public class UZDParam : IUZDParam
    {
        #region Входые параметры
        /// <summary>
        /// Входые параметры
        /// </summary>
        public ObservableCollection<BaseText> InputParams { get; set; } = new();
        #endregion

        #region Выходые параметры
        /// <summary>
        /// Выходые параметры
        /// </summary>
        public ObservableCollection<BaseText> OutputParams { get; set; } = new();
        #endregion

        #region Уставки
        /// <summary>
        /// Уставки
        /// </summary>
        public ObservableCollection<BaseSetpoints> Setpoints { get; set; } = new();
        #endregion
    }
}
