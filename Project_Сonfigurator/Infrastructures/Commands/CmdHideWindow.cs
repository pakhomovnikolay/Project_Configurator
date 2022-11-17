using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdHideWindow : Command
    {
        protected override bool CanExecute(object parameter) =>
            (parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow) != null;

        protected override void Execute(object parameter)
        {
            var window = parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow;
            if (window == null) return;

            window.WindowState = WindowState.Minimized;
            window.Focus();
        }
    }
}
