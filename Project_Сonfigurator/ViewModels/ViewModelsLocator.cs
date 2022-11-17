using Microsoft.Extensions.DependencyInjection;

namespace Project_Сonfigurator.ViewModels
{
    public class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
