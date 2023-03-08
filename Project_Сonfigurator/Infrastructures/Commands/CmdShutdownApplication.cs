using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    /// <summary>
    /// Команда - закрыть приложение
    /// </summary>
    public class CmdShutdownApplication : Command
    {
        protected override bool CanExecute(object parameter) =>
            (parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow) != null;

        protected override void Execute(object parameter)
        {
            IUserDialogService UserDialog = new UserDialogService();
            var msg = "Вы действительно хотите выйти?";
            if (!UserDialog.SendMessage("Внимание!", msg, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            if (App.ProjectChanged)
            {
                msg = $"Вы хотите сохранить изменения в файле\n{App.Services.GetRequiredService<MainWindowViewModel>().NameProject}{App.__EncryptedProjectFileSuffix}";
                if (UserDialog.SendMessage("Управление приложением", msg, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes))
                {
                    IDBService DBServices = new DBService();
                    DBServices.RequestToWriteProjectData();
                }
            }
                
            

            Application.Current.Shutdown();
        }
    }
}
