using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Project_Сonfigurator
{
    public partial class App
    {
        public const string __EncryptedConfigFileSuffix = ".configproject";
        public const string __EncryptedProjectFileSuffix = ".myprojectodb";
        public const string __SocketsExportFileSuffix = ".omx-export";
        public const string __XMLExportFileSuffix = ".xml";
        public const string __FilterSelectExcelWithMakros = "Книга Excel (*.xlsm*)|*.xlsm*";
        public const string __FilterSelectExcel = "Книга Excel (*.xlsm*)|*.xlsx*";
        public const string __FilterSelectProject = $"Файлы (*{__EncryptedProjectFileSuffix}*)|*{__EncryptedProjectFileSuffix}*";

        public static bool LoadComplite = false;
        public static bool ProjectChanged = false;
        public static readonly ISettingService Settings = new SettingService();
        public static readonly IDBService DBServices = new DBService();

        public static string PathConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{NameApp}";
        public static Window ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);
        public static Window FucusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);
        public static TabControl FucusedTabControl
        {
            get
            {
                var Content = ActiveWindow.Content ?? FucusedWindow.Content;
                var _Grid = Content as Grid;
                var _FucusedTabControl = new TabControl();
                foreach (var Children in _Grid.Children)
                    if (Children is TabControl)
                        return Children as TabControl;

                return _FucusedTabControl;
            }
        }

        public static string NameApp => Assembly.GetEntryAssembly().GetName().Name;
        public static string VersionApp => Assembly.GetEntryAssembly().GetName().Version.ToString();
        private static IHost __Host;
        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Host.Services;
        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddViewModels()
            .AddServices()
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();

            #region Получение места хранения файла конфигурации проекта
            bool exists = System.IO.Directory.Exists(PathConfig);
            if (!exists)
                System.IO.Directory.CreateDirectory(PathConfig);
            #endregion

            #region Конфигурация приложения
            Settings.Load();
            DBServices.ProjectDataRequest();
            #endregion

            Services.GetRequiredService<IUserDialogService>().OpenMainWindow();
            LoadComplite = true;
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using (Host) await Host.StopAsync();
        }
    }
}
