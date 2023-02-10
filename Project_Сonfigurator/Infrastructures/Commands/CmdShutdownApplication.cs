using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
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

            Application.Current.Shutdown();
        }
    }
}
