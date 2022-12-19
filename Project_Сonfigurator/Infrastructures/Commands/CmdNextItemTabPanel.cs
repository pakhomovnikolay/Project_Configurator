using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdNextItemTabPanel : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            var _TabControl = App.FucusedTabControl;
            if (_TabControl == null) return;
            if (p is not ScrollViewer _ScrollViewer) return;

            var TabIndex = _TabControl.SelectedIndex;
            if (TabIndex >= (_TabControl.Items.Count - 1)) return;

            var _TabItem = _TabControl.Items[TabIndex] as TabItem;
            _ScrollViewer.ScrollToHorizontalOffset(_ScrollViewer.HorizontalOffset + _TabItem.ActualWidth);
            _TabControl.SelectedIndex = ++TabIndex;

            if (TabIndex >= (_TabControl.Items.Count - 1))
                _ScrollViewer.ScrollToRightEnd();
        }
    }
}
