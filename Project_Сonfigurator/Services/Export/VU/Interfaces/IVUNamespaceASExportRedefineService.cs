﻿namespace Project_Сonfigurator.Services.Export.VU.Interfaces
{
    public interface IVUNamespaceASExportRedefineService
    {
        #region Экспорт данных ВУ
        /// <summary>
        /// Экспорт данных ВУ
        /// </summary>
        /// <param name="TypeExport"></param>
        /// <returns></returns>
        bool Export(string TypeExport);
        #endregion
    }
}
