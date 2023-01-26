namespace Project_Сonfigurator.Services.Export.SU.Interfaces
{
    public interface ISUExportRedefineService
    {
        #region Экспорт данных СУ
        /// <summary>
        /// Экспорт данных СУ
        /// </summary>
        /// <param name="TypeExport"></param>
        /// <returns></returns>
        bool Export(string TypeExport);
        #endregion
    }
}
