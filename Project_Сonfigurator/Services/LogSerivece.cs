using Project_Сonfigurator.Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Project_Сonfigurator.Services
{
    public class LogSerivece : ILogSerivece
    {
        #region Запись лога
        /// <summary>
        /// Запись лога
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="AppName"></param>
        /// <param name="DebugMode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string WriteLog(string Message, string AppName, bool DebugMode = false)
        {
            if (!DebugMode) return Message;
            var file_name = $"{DateTime.Now:yyyyMMdd}_{AppName}_Log.csv";
            var log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {Message}\n";

            try
            {
                using var sw = new StreamWriter(file_name, true, Encoding.UTF8);
                sw.Write(log);
                return Message;
            }
            catch (Exception e)
            {
                file_name = $"{DateTime.Now:yyyyMMdd_HH_mm_ss}_{AppName}_Log.csv";
                log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {e.Message}\n";

                using var sw = new StreamWriter(file_name, true, Encoding.UTF8);
                sw.Write(log);
                log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {Message}\n";
                sw.Write(log);
                return Message;
            }
        }
        #endregion

        #region Асинхронная запись лога
        /// <summary>
        /// Асинхронная запись лога
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="AppName"></param>
        /// <param name="DebugMode"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> WriteLogAsync(string Message, string AppName, bool DebugMode = false)
        {
            if (!DebugMode) return Message;
            var file_name = $"{DateTime.Now:yyyyMMdd}_{AppName}_Log.csv";
            var log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {Message}\n";

            try
            {
                await using var sw = new StreamWriter(file_name, true, Encoding.UTF8);
                await sw.WriteAsync(log).ConfigureAwait(false);
                return Message;
            }
            catch (Exception e)
            {
                file_name = $"{DateTime.Now:yyyyMMdd_HH_mm_ss}_{AppName}_Log.csv";
                log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {e.Message}\n";
                await using var sw = new StreamWriter(file_name, true, Encoding.UTF8);
                await sw.WriteAsync(log).ConfigureAwait(false);

                log = $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} - {Message}\n";
                await sw.WriteAsync(log).ConfigureAwait(false);
                return Message;
            }
        }
        #endregion
    }
}
