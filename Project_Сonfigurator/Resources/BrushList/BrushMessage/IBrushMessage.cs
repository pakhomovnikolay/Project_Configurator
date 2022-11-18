using System.Collections.Generic;
using System.Windows.Media;

namespace Project_Сonfigurator.Resources.BrushList.BrushMessage
{
    public interface IBrushMessage
    {
        #region Список кистей для сообщения
        /// <summary>
        /// Список кистей для сообщения
        /// </summary>
        List<SolidColorBrush> BrushList { get; set; }
        #endregion

    }
}
