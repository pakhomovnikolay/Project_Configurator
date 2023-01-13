using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.Views.DialogControl;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdOpenWindowEditText : Command
    {
        protected override bool CanExecute(object p) => p is string;

        protected override void Execute(object p)
        {
            if (p is not string TextEdit) return;
            var window = new WindowTextEditor()
            {
                TextView = TextEdit,
                Width= 500,
                Height= 200,
            };

            window.Show();
        }
    }
}
