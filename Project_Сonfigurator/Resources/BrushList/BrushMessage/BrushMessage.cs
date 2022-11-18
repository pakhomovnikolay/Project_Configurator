using System.Collections.Generic;
using System.Windows.Media;

namespace Project_Сonfigurator.Resources.BrushList.BrushMessage
{
    public class BrushMessage : IBrushMessage
    {
        #region Список кистей для сообщения
        /// <summary>
        /// Список кистей для сообщения
        /// </summary>
        public List<SolidColorBrush> BrushList { get; set; } = new() { Brushes.Transparent, Brushes.Red, Brushes.Yellow, Brushes.Green };
        #endregion
    }
}
