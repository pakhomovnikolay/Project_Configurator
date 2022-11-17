using Project_Сonfigurator.Infrastructures.Commands.Base;
using System;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class RelayCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public RelayCommand(Action Execute, Func<bool> CanExecute = null) :
            this(p => Execute(), CanExecute is null ? (Func<object, bool>)null : p => CanExecute())
        {

        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _CanExecute = canExecute;
        }

        protected override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;

        protected override void Execute(object parameter) => _Execute(parameter);
    }
}
