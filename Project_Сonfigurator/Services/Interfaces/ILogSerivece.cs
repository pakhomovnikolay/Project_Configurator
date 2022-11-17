using System.Threading.Tasks;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface ILogSerivece
    {
        #region Запись лога
        /// <summary>
        /// Запись лога
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="AppName"></param>
        /// <param name="DebugMode"></param>
        /// <returns></returns>
        string WriteLog(string Message, string AppName, bool DebugMode = false);
        #endregion

        #region Асинхронная запись лога
        /// <summary>
        /// Асинхронная запись лога
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="AppName"></param>
        /// <param name="DebugMode"></param>
        /// <returns></returns>
        Task<string> WriteLogAsync(string Message, string AppName, bool DebugMode = false);
        #endregion
    }
}
