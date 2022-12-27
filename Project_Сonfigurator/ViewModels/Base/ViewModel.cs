using Project_Сonfigurator.ViewModels.Base.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project_Сonfigurator.ViewModels.Base
{
    public class ViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual bool Set<T>(ref T filed, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(filed, value)) return false;
            filed = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
