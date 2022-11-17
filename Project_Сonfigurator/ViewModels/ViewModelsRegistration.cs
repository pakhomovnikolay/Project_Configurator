using Microsoft.Extensions.DependencyInjection;

namespace Project_Сonfigurator.ViewModels
{
    public static class ViewModelsRegistration
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>();
    }
}
