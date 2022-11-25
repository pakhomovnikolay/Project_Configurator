using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Project_Сonfigurator
{
    public partial class App
    {
        public static Window ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        public static Window FucusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

        public static SettingApp Settings = new();

        public static string NameApp => Assembly.GetEntryAssembly().GetName().Name;

        public static string VersionApp => Assembly.GetEntryAssembly().GetName().Version.ToString();
        private static IHost __Host;

        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddServices()
            .AddViewModels()
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using (Host) await Host.StopAsync();
        }
    }
}
