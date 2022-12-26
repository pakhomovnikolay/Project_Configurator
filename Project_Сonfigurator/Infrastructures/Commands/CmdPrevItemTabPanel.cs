using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    /// <summary>
    /// Команда - сместь ScrollViewer в лево
    /// </summary>
    public class CmdPrevItemTabPanel : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            if (MyScrollViewer.HorizontalOffset <= 0) return;
            MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - 100);
        }
    }
}
