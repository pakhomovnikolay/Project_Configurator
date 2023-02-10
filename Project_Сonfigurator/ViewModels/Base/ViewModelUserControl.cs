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
using System.Windows.Controls;
using System.Windows.Data;

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

        #region Сервис работы с БД
        /// <summary>
        /// Сервис работы с БД
        /// </summary>
        public virtual IDBService DBServices { get; set; } = new DBService();
        #endregion

        #region Сервис работы с настройками приложения
        /// <summary>
        /// Сервис работы с настройками приложения
        /// </summary>
        public virtual ISettingService SettingServices { get; set; } = new SettingService();
        #endregion

        #region Сервис экспорта данных приложения для ПЛК
        /// <summary>
        /// Сервис экспорта данных приложения для ПЛК
        /// </summary>
        public virtual ISUExportRedefineService SUExportRedefineServices { get; set; } = new SUExportRedefineService();
        #endregion

        #region Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по OPC
        /// <summary>
        /// Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по OPC
        /// </summary>
        public virtual IVUExportOPCMap VUExportOPCMaps { get; set; } = new VUExportOPCMap();
        #endregion

        #region Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по modbus
        /// <summary>
        /// Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по modbus
        /// </summary>
        public virtual IVUExportModbusMap IVUExportModbusMaps { get; set; } = new VUExportModbusMap();
        #endregion

        #region Сервис проверки наличия в строках русских символов
        /// <summary>
        /// Сервис проверки наличия в строках русских символов
        /// </summary>
        public virtual ICyrillicSymbolService CyrillicSymbolServices { get; set; } = new CyrillicSymbolService();
        #endregion

        #region Сервис редактирования объектов
        /// <summary>
        /// Сервис редактирования объектов
        /// </summary>
        public virtual IEditService EditServices { get; set; } = new EditService();
        #endregion

        #region Сервис шифрования данных
        /// <summary>
        /// Сервис шифрования данных
        /// </summary>
        public virtual IEncryptorService EncryptorServices { get; set; } = new EncryptorService();
        #endregion

        #region Сервис логирования
        /// <summary>
        /// Сервис логирования
        /// </summary>
        public virtual ILogSerivece LogSeriveces { get; set; } = new LogSerivece();
        #endregion

        #region Сервис работы с настройками корзин
        /// <summary>
        /// Сервис работы с настройками корзин
        /// </summary>
        public virtual ILayotRackService LayotRackServices { get; set; } = new LayotRackService();
        #endregion

        #region Сервис работы с параметрами
        /// <summary>
        /// Сервис работы с параметрами
        /// </summary>
        public virtual ISignalService SignalServices { get; set; } = new SignalService();
        #endregion
    }
}
