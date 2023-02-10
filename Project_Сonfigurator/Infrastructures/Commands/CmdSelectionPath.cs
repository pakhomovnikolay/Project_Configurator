using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdSelectionPath : Command
    {
        protected override bool CanExecute(object p) => true;

        protected override void Execute(object p)
        {
            if (p is not TextBox Path) return;
            IUserDialogService UserDialog = new UserDialogService();
            Path.Text = "";
            if (UserDialog.SelectFile("Выбор файла", out string selected_path, Filter: "Все файлы (*.*)|*.*"))
                Path.Text = selected_path;
        }
    }
}
