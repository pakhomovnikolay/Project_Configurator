using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.AS;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using Project_Сonfigurator.Views.UserControls;
using Project_Сonfigurator.Views.UserControls.Params;
using Project_Сonfigurator.Views.UserControls.Settings;
using Project_Сonfigurator.Views.UserControls.Signals;
using Project_Сonfigurator.Views.Windows;
using Project_Сonfigurator.Views.Windows.AS;

namespace Project_Сonfigurator.ViewModels
{
    public static class ViewModelsRegistration
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services

        #region ViewModels
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<LayotRackUserControlViewModel>()
            .AddSingleton<TableSignalsUserControlViewModel>()
            .AddSingleton<SignalsDIUserControlViewModel>()
            .AddSingleton<SettingWindowViewModels>()
            .AddSingleton<SignalsAIUserControlViewModel>()
            .AddSingleton<SignalsDOUserControlViewModel>()
            .AddSingleton<SignalsAOUserControlViewModel>()
            .AddSingleton<UserDIUserControlViewModel>()
            .AddSingleton<UserAIUserControlViewModel>()
            .AddSingleton<UserRegUserControlViewModel>()
            .AddSingleton<SignalsGroupUserControlViewModel>()
            .AddSingleton<GroupsSignalUserControlViewModel>()
            .AddSingleton<UZDUserControlViewModel>()
            .AddSingleton<UVSUserControlViewModel>()
            .AddSingleton<UMPNAUserControlViewModel>()
            .AddSingleton<KTPRUserControlViewModel>()
            .AddSingleton<KTPRSUserControlViewModel>()
            .AddSingleton<SignalingUserControlViewModel>()
            .AddSingleton<ECUserControlViewModel>()
            .AddSingleton<UTSUserControlViewModel>()
            .AddSingleton<UstRealUserControlViewModel>()
            .AddSingleton<UstCommonUserControlViewModel>()
            .AddSingleton<HandMapUserControlViewModel>()
            .AddSingleton<MessageWindowViewModel>()
            .AddSingleton<CommandUserControlViewModel>()
            .AddSingleton<ExportNamespaceASWindowViewModel>()
            .AddSingleton<IOSExportASWindowViewModel>()
            .AddSingleton<PLCExportASWindowViewModel>()
            .AddSingleton<MessagesUserControlViewModel>()
        #endregion

        #region IViewModel, IViewModelUserControls
            .AddSingleton<IViewModel, MainWindowViewModel>()
            .AddSingleton<IViewModel, SettingWindowViewModels>()
            .AddSingleton<IViewModel, MessageWindowViewModel>()
            .AddSingleton<IViewModel, ExportNamespaceASWindowViewModel>()
            .AddSingleton<IViewModel, IOSExportASWindowViewModel>()
            .AddSingleton<IViewModel, PLCExportASWindowViewModel>()
            .AddSingleton<IViewModelUserControls, MessagesUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, CommandUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, LayotRackUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, TableSignalsUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UserDIUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UserAIUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalsDIUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalsAIUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalsDOUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalsAOUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalsGroupUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, GroupsSignalUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, ECUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UserRegUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UTSUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UZDUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UVSUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UMPNAUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, KTPRUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, KTPRSUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, SignalingUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UstRealUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, UstCommonUserControlViewModel>()
            .AddSingleton<IViewModelUserControls, HandMapUserControlViewModel>()
        #endregion

        #region Регистрация окон

        #region MainWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<MainWindowViewModel>();
                var window = new MainWindow { DataContext = model };
                return window;
            })
        #endregion

        #region SettingWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingWindow { DataContext = model, };
                return window;
            })
        #endregion

        #region MessageWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<MessageWindowViewModel>();
                var window = new MessageWindow { DataContext = model };
                return window;
            })
        #endregion

        #region ExportNamespaceASWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<ExportNamespaceASWindowViewModel>();
                var window = new ExportNamespaceASWindow { DataContext = model };
                return window;
            })
        #endregion

        #region PLCExportASWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<PLCExportASWindowViewModel>();
                var window = new PLCExportASWindow { DataContext = model };
                return window;
            })
        #endregion

        #region IOSExportASWindow
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<IOSExportASWindowViewModel>();
                var window = new IOSExportASWindow { DataContext = model };
                return window;
            })
        #endregion

        #endregion

        #region Регисрация пользовательских интерфейсов для главного окна

        #region CommandUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<CommandUserControlViewModel>();
                var window = new CommandUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region LayotRackUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<LayotRackUserControlViewModel>();
                var window = new LayotRackUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region TableSignalsUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<TableSignalsUserControlViewModel>();
                var window = new TableSignalsUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UserDIUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UserDIUserControlViewModel>();
                var window = new UserDIUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UserAIUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UserAIUserControlViewModel>();
                var window = new UserAIUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalsDIUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalsDIUserControlViewModel>();
                var window = new SignalsDIUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalsAIUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalsAIUserControlViewModel>();
                var window = new SignalsAIUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalsDOUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalsDOUserControlViewModel>();
                var window = new SignalsDOUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalsAOUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalsAOUserControlViewModel>();
                var window = new SignalsAOUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region ECUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<ECUserControlViewModel>();
                var window = new ECUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UserRegUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UserRegUserControlViewModel>();
                var window = new UserRegUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalsGroupUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalsGroupUserControlViewModel>();
                var window = new SignalsGroupUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region GroupsSignalUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<GroupsSignalUserControlViewModel>();
                var window = new GroupsSignalUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UZDUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UZDUserControlViewModel>();
                var window = new UZDUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UVSUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UVSUserControlViewModel>();
                var window = new UVSUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UMPNAUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UMPNAUserControlViewModel>();
                var window = new UMPNAUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region KTPRUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<KTPRUserControlViewModel>();
                var window = new KTPRUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region KTPRSUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<KTPRSUserControlViewModel>();
                var window = new KTPRSUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SignalingUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SignalingUserControlViewModel>();
                var window = new SignalingUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UTSUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UTSUserControlViewModel>();
                var window = new UTSUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UstRealUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UstRealUserControlViewModel>();
                var window = new UstRealUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region UstCommonUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<UstCommonUserControlViewModel>();
                var window = new UstCommonUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region HandMapUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<HandMapUserControlViewModel>();
                var window = new HandMapUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region MessagesUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<MessagesUserControlViewModel>();
                var window = new MessagesUserControl { DataContext = model };
                return window;
            })
        #endregion

        #endregion

        #region Регисрация пользовательских интерфейсов для окна настроек приложения

        #region SettingsCommonUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingsCommonUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SettingsVendorUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingsVendorUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SettingsServerConnectUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingsServerConnectUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SettingsImportTableSignalsUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingsImportTableSignalsUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SettingsDeviceControlsUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingsDeviceControlsUserControl { DataContext = model };
                return window;
            })
        #endregion

        #region SettingsDeviceControlsUserControl
            .AddTransient(s =>
            {
                var model = s.GetRequiredService<SettingWindowViewModels>();
                var window = new SettingAddressMapUserControl { DataContext = model };
                return window;
            })
        #endregion

        #endregion

            ;
    }
}
