using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.UserControls;
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
    }
}
