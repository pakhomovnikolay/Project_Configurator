using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;

namespace Project_Сonfigurator.ViewModels
{
    public static class ViewModelsRegistration
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
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
            ;
    }
}
