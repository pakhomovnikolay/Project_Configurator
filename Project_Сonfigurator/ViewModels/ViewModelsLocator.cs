using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;

namespace Project_Сonfigurator.ViewModels
{
    public class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public LayotRackUserControlViewModel LayotRackUserControlViewModel => App.Services.GetRequiredService<LayotRackUserControlViewModel>();
        public TableSignalsUserControlViewModel TableSignalsUserControlViewModel => App.Services.GetRequiredService<TableSignalsUserControlViewModel>();
        public SignalsDIUserControlViewModel SignalsDIUserControlViewModel => App.Services.GetRequiredService<SignalsDIUserControlViewModel>();
        public SettingWindowViewModels SettingWindowViewModels => App.Services.GetRequiredService<SettingWindowViewModels>();
        public SignalsAIUserControlViewModel SignalsAIUserControlViewModel => App.Services.GetRequiredService<SignalsAIUserControlViewModel>();
        public SignalsDOUserControlViewModel SignalsDOUserControlViewModel => App.Services.GetRequiredService<SignalsDOUserControlViewModel>();
        public SignalsAOUserControlViewModel SignalsAOUserControlViewModel => App.Services.GetRequiredService<SignalsAOUserControlViewModel>();
        public UserDIUserControlViewModel UserDIUserControlViewModel => App.Services.GetRequiredService<UserDIUserControlViewModel>();
        public UserAIUserControlViewModel UserAIUserControlViewModel => App.Services.GetRequiredService<UserAIUserControlViewModel>();
        public UserRegUserControlViewModel UserRegUserControlViewModel => App.Services.GetRequiredService<UserRegUserControlViewModel>();
        public SignalsGroupUserControlViewModel SignalsGroupUserControlViewModel => App.Services.GetRequiredService<SignalsGroupUserControlViewModel>();
        public GroupsSignalUserControlViewModel GroupsSignalUserControlViewModel => App.Services.GetRequiredService<GroupsSignalUserControlViewModel>();
        public UZDUserControlViewModel UZDUserControlViewModel => App.Services.GetRequiredService<UZDUserControlViewModel>();
        public UVSUserControlViewModel UVSUserControlViewModel => App.Services.GetRequiredService<UVSUserControlViewModel>();
        public UMPNAUserControlViewModel UMPNAUserControlViewModel => App.Services.GetRequiredService<UMPNAUserControlViewModel>();
        public KTPRUserControlViewModel KTPRUserControlViewModel => App.Services.GetRequiredService<KTPRUserControlViewModel>();
        public KTPRSUserControlViewModel KTPRSUserControlViewModel => App.Services.GetRequiredService<KTPRSUserControlViewModel>();
        

    }
}
