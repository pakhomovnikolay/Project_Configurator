using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class WindowDialogCloseCommand : Command
    {
        public bool? DialogResult { get; set; }

        protected override bool CanExecute(object parameter) =>
            (parameter as Window ?? App.FucusedWindow ?? App.ActiveWindow) != null;

        protected override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            var window = (Window)parameter;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
