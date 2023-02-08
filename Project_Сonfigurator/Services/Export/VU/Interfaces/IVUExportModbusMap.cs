namespace Project_Сonfigurator.Services.Export.VU.Interfaces
{
    public interface IVUExportModbusMap
    {
        #region Экспорт карты Modbus адресов, для проектов AS
        /// <summary>
        /// Экспорт карты Modbus адресов, для проектов AS
        /// </summary>
        /// <returns></returns>
        bool ASExprot(); 
        #endregion
    }
}
