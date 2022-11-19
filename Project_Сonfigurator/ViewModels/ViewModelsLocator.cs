using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.UserControls;

namespace Project_Сonfigurator.ViewModels
{
    public class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public LayotRackUserControlViewModel LayotRackUserControlViewModel => App.Services.GetRequiredService<LayotRackUserControlViewModel>();
    }
}
