using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdSelectedTabPanelItem : Command
    {
        protected override bool CanExecute(object p) => p is string;

        protected override void Execute(object p)
        {
            var _TabControl = App.FucusedTabControl;
            if (_TabControl == null) return;
            if (p is not string _TabItemHeader) return;
            
            if (App.FucusedTabControl == null) return;
            foreach (var _Item in App.FucusedTabControl.Items)
            {
                var _TabItem = _Item as TabItem;
                if (_TabItem.Header.ToString() == _TabItemHeader)
                {
                    App.FucusedTabControl.SelectedItem = _TabItem;
                    break;
                }
            }
        }
    }
}
