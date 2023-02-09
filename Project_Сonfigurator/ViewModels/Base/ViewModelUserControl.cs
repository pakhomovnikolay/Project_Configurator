using Project_Сonfigurator.ViewModels.Base.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Project_Сonfigurator.ViewModels.Base
{
    public class ViewModelUserControl : INotifyPropertyChanged, IViewModelUserControls
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Фиксация изменения свойств данных
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Проверка изменения данных
        protected virtual bool Set<T>(ref T filed, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(filed, value)) return false;
            filed = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Заголовок вкладки
        private string _Title;
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        public virtual string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Описание вкладки
        private string _Description;
        /// <summary>
        /// Описание вкладки
        /// </summary>
        public virtual string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
        }
        #endregion

        #region Пользовательский интерфейс
        /// <summary>
        /// Пользовательский интерфейс
        /// </summary>
        public virtual UserControl UsingUserControl { get; set; } = new();
        #endregion

        #region Состояние активной вкладки
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public virtual bool IsSelected { get; set; }
        #endregion

        #region Состояние необходимости выбора сигнала
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        public virtual bool DoSelection { get; set; }
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public virtual object GetParam() => null;
        #endregion
    }
}
