using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    /// <summary>
    /// Команда - сместь ScrollViewer в конец
    /// </summary>
    public class CmdScrollToRightEnd : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            MyScrollViewer.ScrollToRightEnd();
        }
    }
}
