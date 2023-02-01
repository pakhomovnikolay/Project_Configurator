using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.Infrastructures.DataLists;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdSelectedTabPanelItem : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            IUserDialogService UserDialog = new UserDialogService();
            var _ViewModel = App.Services.GetRequiredService<MainWindowViewModel>();

            var _TabControl = App.FucusedTabControl;
            if (_TabControl == null) return;
            if (p is not ScrollViewer MyScrollViewer) return;

            if (UserDialog.SearchControlViewModel(_ViewModel.SelectedViewModel.Title) is not IViewModelUserControls _TabItem) return;

            var SelectedIndex = _TabControl.SelectedIndex;
            _TabControl.SelectedItem = _TabItem;

            if (_TabControl.SelectedIndex == (_TabControl.Items.Count - 1))
            {
                MyScrollViewer.ScrollToRightEnd();
                return;
            }
            else if (_TabControl.SelectedIndex == 0)
            {
                MyScrollViewer.ScrollToLeftEnd();
                return;
            }

            var Offset = 0d;
            if (_TabControl.SelectedIndex > SelectedIndex)
            {
                for (int i = SelectedIndex; i < _TabControl.SelectedIndex; i++)
                {
                    var _Item = _TabControl.Items[i] as IViewModelUserControls;
                    Offset += _Item.Title.Length * 6;

                }
                MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset + Offset);
            }

            else if (_TabControl.SelectedIndex < SelectedIndex)
            {
                for (int i = SelectedIndex - 1; i >= _TabControl.SelectedIndex; i--)
                {
                    var _Item = _TabControl.Items[i] as IViewModelUserControls;
                    Offset += _Item.Title.Length * 6;
                }
                MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - Offset);
            }


        }
    }
}
