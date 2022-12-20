using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdPrevItemTabPanel : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - 100);
            if (MyScrollViewer.HorizontalOffset <= 0) return;

            //var _TabControl = App.FucusedTabControl;
            //if (_TabControl == null) return;
            //if (p is not ScrollViewer MyScrollViewer) return;
            //if (_TabControl.SelectedIndex == 0) return;
            //var _TabItem = _TabControl.Items[_TabControl.SelectedIndex] as TabItem;
            //_TabControl.SelectedIndex--;

            //if (MyScrollViewer.HorizontalOffset <= 0) return;
            //MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - _TabItem.ActualWidth);
            //if (_TabControl.SelectedIndex <= 0)
            //    MyScrollViewer.ScrollToLeftEnd();
        }
    }
}
