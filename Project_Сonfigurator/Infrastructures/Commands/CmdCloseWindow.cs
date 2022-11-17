using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdCloseWindow : Command
    {
        protected override bool CanExecute(object parameter) =>
            (parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow) != null;

        protected override void Execute(object parameter) =>
            (parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow)?.Close();
    }
}
