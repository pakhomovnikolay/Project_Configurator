using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    /// <summary>
    /// Команда - сместь ScrollViewer в начало
    /// </summary>
    public class CmdScrollToLeftEnd : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            MyScrollViewer.ScrollToLeftEnd();
        }
    }
}
