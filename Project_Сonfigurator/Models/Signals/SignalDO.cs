using Project_Сonfigurator.Models.Signals.Interfaces;

namespace Project_Сonfigurator.Models.Signals
{
    public class SignalDO : ISignalDO
    {
        #region Параметры сигнала
        /// <summary>
        /// Параметры сигнала
        /// </summary>
        public BaseSignal Signal { get; set; } = new();
        #endregion
    }
}
