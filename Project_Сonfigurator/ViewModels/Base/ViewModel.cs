using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Export.SU;
using Project_Сonfigurator.Services.Export.SU.Interfaces;
using Project_Сonfigurator.Services.Export.VU;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Project_Сonfigurator.ViewModels.Base
{
    public class ViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Фиксация изменения свойств данных
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Проверка изменения данных
        protected virtual bool Set<T>(ref T filed, T value, [CallerMemberName] string propertyName = null, bool DontСommitСhanges = false)
        {
            if (Equals(filed, value)) return false;
            filed = value;
            OnPropertyChanged(propertyName);
            if (App.LoadComplete && !DontСommitСhanges)
                App.ProjectChanged = true;

            return true;
        }
        #endregion

        #region Заголовок окна
        private string _Title;
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public virtual string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Высота окна
        private int _WindowHeight = 800;
        /// <summary>
        /// Высота окна
        /// </summary>
        public virtual int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }
        #endregion

        #region Ширина окна
        private int _WindowWidth = 1200;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public virtual int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

        #region Режим изменения размеров окна
        private ResizeMode _WindowResizeMode = ResizeMode.CanResizeWithGrip;
        /// <summary>
        /// Режим изменения размеров окна
        /// </summary>
        public virtual ResizeMode WindowResizeMode
        {
            get => _WindowResizeMode;
            set => Set(ref _WindowResizeMode, value);
        }
        #endregion

        #region Текущее состояние окна
        private WindowState _WindowWindowState;
        /// <summary>
        /// Текущее состояние окна
        /// </summary>
        public virtual WindowState WindowWindowState
        {
            get
            {
                SetButtonStyleStateWindow(_WindowWindowState);
                return _WindowWindowState;
            }
            set
            {
                if (Set(ref _WindowWindowState, value))
                    SetButtonStyleStateWindow(_WindowWindowState);
            }
        }
        #endregion

        #region Стиль кнопки изменения состояния окна
        private Style _ButtonChangeStateWindowStyle;
        /// <summary>
        /// Стиль кнопки изменения состояния окна
        /// </summary>
        public virtual Style ButtonChangeStateWindowStyle
        {
            get => _ButtonChangeStateWindowStyle;
            set => Set(ref _ButtonChangeStateWindowStyle, value);
        }
        #endregion

        #region Установка стиля кнопки состояния окна
        /// <summary>
        /// Установка стиля кнопки состояния окна
        /// </summary>
        /// <param name="State"></param>
        public virtual void SetButtonStyleStateWindow(WindowState State)
        {
            ButtonChangeStateWindowStyle =
                State == WindowState.Normal ?
                (Style)Application.Current.FindResource("MaximizedButtonStyle") :
                (Style)Application.Current.FindResource("MinimizedButtonStyle");
        }
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public virtual void GetParams<T>(out T _Params) => _Params = default;
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public virtual ObservableCollection<T> GetParams<T>() => default;
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public virtual void SetParams<T>(T _Params) { }
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public virtual void SetParams<T>(ObservableCollection<T> _Params) { }
        #endregion

        #region Коллекция параметров
        public readonly CollectionViewSource _ParamsDataView = new();
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        public virtual ICollectionView ParamsDataView => _ParamsDataView?.View;
        #endregion

        #region Коллекция дополнительных параметров
        public readonly CollectionViewSource _SubParamsDataView = new();
        /// <summary>
        /// Коллекция дополнительных параметров
        /// </summary>
        public virtual ICollectionView SubParamsDataView => _SubParamsDataView?.View;
        #endregion

        #region Сервис взаимодействия с пользователем
        /// <summary>
        /// Сервис взаимодействия с пользователем
        /// </summary>
        public virtual IUserDialogService UserDialog { get; set; } = new UserDialogService();
        #endregion

        #region Сервис логирования
        /// <summary>
        /// Сервис логирования
        /// </summary>
        public virtual ILogSerivece LogSeriveces { get; set; } = new LogSerivece();
        #endregion
    }
}
