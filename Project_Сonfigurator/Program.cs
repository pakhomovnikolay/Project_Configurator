using Microsoft.Extensions.Hosting;
using Project_Сonfigurator.Services;
using System;

namespace Project_Сonfigurator
{
    public class Program
    {
        public static readonly SettingService Settings = new();
        public static string PathConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{App.NameApp}";

        [STAThread]
        public static void Main(string[] args)
        {
            #region Конфигурация приложения
            bool exists = System.IO.Directory.Exists(PathConfig);
            if (!exists)
                System.IO.Directory.CreateDirectory(PathConfig);

            Settings.Load();
            Settings.LoadData();
            #endregion

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices(App.ConfigureServices);
    }
}
