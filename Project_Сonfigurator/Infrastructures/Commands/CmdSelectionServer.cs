using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands.Base;
using Project_Сonfigurator.ViewModels;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    internal class CmdSelectionServer : Command
    {
        protected override bool CanExecute(object p) =>
            App.Settings.Config is not null &&
            App.Settings.Config.ServerDB is not null &&
            App.Settings.Config.ServerDB.Count > 0;

        protected override void Execute(object p)
        {
            var Servers = App.Settings.Config.ServerDB;
            var _ViewModel = App.Services.GetRequiredService<MainWindowViewModel>();
            _ViewModel.IsCheckedAll = true;
            foreach (var _Server in Servers)
                _ViewModel.IsCheckedAll = _ViewModel.IsCheckedAll && _Server.IsSelection;

        }
    }
}
