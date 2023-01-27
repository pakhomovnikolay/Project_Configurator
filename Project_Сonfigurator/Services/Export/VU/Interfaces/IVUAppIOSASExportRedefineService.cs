using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Project_Сonfigurator.Services.Export.VU.Interfaces
{
    public interface IVUAppIOSASExportRedefineService
    {
        #region Экспорт данных ВУ
        /// <summary>
        /// Экспорт данных ВУ
        /// </summary>
        /// <param name="CheckBoxs"></param>
        /// <returns></returns>
        bool Export(ObservableCollection<CheckBox> CheckBoxs);
        #endregion
    }
}
