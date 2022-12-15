using Microsoft.Extensions.Hosting;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Interfaces;
using System;

namespace Project_Сonfigurator
{
    public class Program
    {
        public static readonly SettingService Settings = new();
        public static readonly IDBService _DBService = new DBService();
        public static string PathConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{App.NameApp}";

        [STAThread]
        public static void Main(string[] args)
        {
            #region Конфигурация приложения
            bool exists = System.IO.Directory.Exists(PathConfig);
            if (!exists)
                System.IO.Directory.CreateDirectory(PathConfig);

            Settings.Load();
            _DBService.AppData = _DBService.LoadData(Settings.Config.PathProject);
            #endregion

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices(App.ConfigureServices);
    }
}
