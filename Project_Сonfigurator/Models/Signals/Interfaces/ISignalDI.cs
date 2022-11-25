namespace Project_Сonfigurator.Models.Signals.Interfaces
{
    public interface ISignalDI
    {
        #region Параметры сигнала
        /// <summary>
        /// Параметры сигнала
        /// </summary>
        BaseSignal Signal { get; set; } 
        #endregion
    }
}
