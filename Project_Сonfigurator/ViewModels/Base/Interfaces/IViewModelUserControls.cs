using Project_Сonfigurator.Services.Export.SU.Interfaces;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Project_Сonfigurator.ViewModels.Base.Interfaces
{
    public interface IViewModelUserControls
    {
        #region Заголовок вкладки
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        string Title { get; set; }
        #endregion

        #region Описание вкладки
        /// <summary>
        /// Описание вкладки
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Пользовательский интерфейс
        /// <summary>
        /// Пользовательский интерфейс
        /// </summary>
        UserControl UsingUserControl { get; set; }
        #endregion

        #region Состояние активной вкладки
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        bool IsSelected { get; set; }
        #endregion

        #region Состояние необходимости выбора сигнала
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        bool DoSelection { get; set; }
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        void GetParams<T>(out T _Params);
        #endregion

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        ObservableCollection<T> GetParams<T>();
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        void SetParams<T>(T _Params);
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        void SetParams<T>(ObservableCollection<T> _Params);
        #endregion

        #region Коллекция параметров
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        ICollectionView ParamsDataView { get; }
        #endregion

        #region Коллекция дополнительных параметров
        /// <summary>
        /// Коллекция дополнительных параметров
        /// </summary>
        ICollectionView SubParamsDataView { get; }
        #endregion

        #region Сервис взаимодействия с пользователем
        /// <summary>
        /// Сервис взаимодействия с пользователем
        /// </summary>
        IUserDialogService UserDialog { get; set; }
        #endregion

        #region Сервис работы с БД
        /// <summary>
        /// Сервис работы с БД
        /// </summary>
        IDBService DBServices { get; set; }
        #endregion

        #region Сервис работы с настройками приложения
        /// <summary>
        /// Сервис работы с настройками приложения
        /// </summary>
        ISettingService SettingServices { get; set; }
        #endregion

        #region Сервис экспорта данных приложения для ПЛК
        /// <summary>
        /// Сервис экспорта данных приложения для ПЛК
        /// </summary>
        ISUExportRedefineService SUExportRedefineServices { get; set; }
        #endregion

        #region Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по OPC
        /// <summary>
        /// Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по OPC
        /// </summary>
        IVUExportOPCMap VUExportOPCMaps { get; set; }
        #endregion

        #region Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по modbus
        /// <summary>
        /// Сервис экспорта данных для SCADA, при взаимодествии с ПЛК по modbus
        /// </summary>
        IVUExportModbusMap IVUExportModbusMaps { get; set; }
        #endregion

        #region Сервис проверки наличия в строках русских символов
        /// <summary>
        /// Сервис проверки наличия в строках русских символов
        /// </summary>
        ICyrillicSymbolService CyrillicSymbolServices { get; set; }
        #endregion

        #region Сервис редактирования объектов
        /// <summary>
        /// Сервис редактирования объектов
        /// </summary>
        IEditService EditServices { get; set; }
        #endregion

        #region Сервис шифрования данных
        /// <summary>
        /// Сервис шифрования данных
        /// </summary>
        IEncryptorService EncryptorServices { get; set; }
        #endregion

        #region Сервис логирования
        /// <summary>
        /// Сервис логирования
        /// </summary>
        ILogSerivece LogSeriveces { get; set; }
        #endregion

        #region Сервис работы с настройками корзин
        /// <summary>
        /// Сервис работы с настройками корзин
        /// </summary>
        ILayotRackService LayotRackServices { get; set; }
        #endregion

        #region Сервис работы с параметрами
        /// <summary>
        /// Сервис работы с параметрами
        /// </summary>
        ISignalService SignalServices { get; set; }
        #endregion
    }
}
