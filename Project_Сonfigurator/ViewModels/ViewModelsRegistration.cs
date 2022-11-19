using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.ViewModels.UserControls;

namespace Project_Сonfigurator.ViewModels
{
    public static class ViewModelsRegistration
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<LayotRackUserControlViewModel>()
            ;
    }
}
