namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ISUExportRedefineService
    {
        #region Экспорт данных СУ
        /// <summary>
        /// Экспорт данных СУ
        /// </summary>
        /// <param name="TypeExport"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Export(string TypeExport, object item);
        #endregion
    }
}
