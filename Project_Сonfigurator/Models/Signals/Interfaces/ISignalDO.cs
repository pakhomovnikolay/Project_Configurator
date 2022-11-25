namespace Project_Сonfigurator.Models.Signals.Interfaces
{
    public interface ISignalDO
    {
        #region Параметры сигнала
        /// <summary>
        /// Параметры сигнала
        /// </summary>
        BaseSignal Signal { get; set; }
        #endregion
    }
}
