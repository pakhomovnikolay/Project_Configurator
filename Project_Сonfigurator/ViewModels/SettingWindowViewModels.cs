using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class SettingWindowViewModels : ViewModel
    {
        #region Конструктор
        public ISettingService _SettingService;
        public IVendorService _VendorService;
        public SettingWindowViewModels(ISettingService settingService, IVendorService vendorService)
        {
            _SettingService = settingService;
            _VendorService = vendorService;

            if (Config.Vendors is not null && Config.Vendors.Count > 0)
            {
                SelectedVendor = Config.Vendors[^1];
                _DataViewVendors.Source = Config.Vendors;
                _DataViewVendors.View.Refresh();
                OnPropertyChanged(nameof(DataViewVendors));
            }
            
        }
        #endregion

        #region Параметры

        #region Заголовок окна
        private string _Title = "Настройки";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Высота окна
        private int _WindowHeight = 1000;
        /// <summary>
        /// Высота окна
        /// </summary>
        public int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }
        #endregion

        #region Ширина окна
        private int _WindowWidth = 1500;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

        #region Режим изменения размеров окна
        private ResizeMode _WindowResizeMode = ResizeMode.NoResize;
        /// <summary>
        /// Режим изменения размеров окна
        /// </summary>
        public ResizeMode WindowResizeMode
        {
            get => _WindowResizeMode;
            set => Set(ref _WindowResizeMode, value);
        }
        #endregion

        #region Текущее состояние окна
        private WindowState _WindowWindowState = WindowState.Normal;
        /// <summary>
        /// Текущее состояние окна
        /// </summary>
        public WindowState WindowWindowState
        {
            get => _WindowWindowState;
            set => Set(ref _WindowWindowState, value);
        }
        #endregion

        #region Настройки приложения
        private SettingApp _Config = Program.Settings.Config ?? new();
        /// <summary>
        /// Настройки приложения
        /// </summary>
        public SettingApp Config
        {
            get => _Config;
            set => Set(ref _Config, value);
        }
        #endregion

        #region Коллекция вендоров
        /// <summary>
        /// Коллекция вендоров
        /// </summary>
        private readonly CollectionViewSource _DataViewVendors = new();
        public ICollectionView DataViewVendors => _DataViewVendors?.View;
        #endregion

        #region Выбранный вендор
        private Vendor _SelectedVendor = new();
        /// <summary>
        /// Выбранный вендор
        /// </summary>
        public Vendor SelectedVendor
        {
            get => _SelectedVendor;
            set => Set(ref _SelectedVendor, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Сохранить настройки
        /// <summary>
        /// Команда - Сохранить настройки
        /// </summary>
        private ICommand _CmdSaveSettings;
        public ICommand CmdSaveSettings => _CmdSaveSettings ??= new RelayCommand(OnCmdSaveSettingsExecuted, CanCmdSaveSettingsExecute);
        private bool CanCmdSaveSettingsExecute(object p) => true;
        private void OnCmdSaveSettingsExecuted(object p)
        {
            if (p is not Window window) return;
            _SettingService.Config.Vendors = Config.Vendors;
            if (_SettingService.Save())
                Application.Current.Shutdown();
        }
        #endregion

        #region Команда - Добавить вендора
        /// <summary>
        /// Команда - Добавить вендора
        /// </summary>
        private ICommand _CmdAddVendor;
        public ICommand CmdAddVendor => _CmdAddVendor ??= new RelayCommand(OnCmdAddVendorExecuted, CanCmdAddVendorExecute);
        private bool CanCmdAddVendorExecute() => true;
        private void OnCmdAddVendorExecuted()
        {
            var vendor = new Vendor()
            {
                Name = $"Новый вендор {Config.Vendors.Count + 1}",
                IsSelected = false,
                ModuleTypes = new List<VendorModuleType>(),
                SelectedModuleType = new()
            };

            Config.Vendors.Add(vendor);
            SelectedVendor = Config.Vendors[^1];
            _DataViewVendors.Source = Config.Vendors;
            _DataViewVendors.View.Refresh();
            OnPropertyChanged(nameof(DataViewVendors));
        }
        #endregion

        #region Команда - Удалить вендора
        /// <summary>
        /// Команда - Удалить вендора
        /// </summary>
        private ICommand _CmdRemoveVendor;
        public ICommand CmdRemoveVendor => _CmdRemoveVendor ??= new RelayCommand(OnCmdRemoveVendorExecuted, CanCmdRemoveVendorExecute);
        private bool CanCmdRemoveVendorExecute(object p) => SelectedVendor is not null && Config.Vendors.Count > 0;
        private void OnCmdRemoveVendorExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            Config.Vendors.Remove(SelectedVendor);
            _DataViewVendors.Source = Config.Vendors;
            _DataViewVendors.View.Refresh();

            OnPropertyChanged(nameof(DataViewVendors));
        }
        #endregion

        #region Команда - Изменить  данные выбранного вендора
        /// <summary>
        /// Команда - Изменить  данные выбранного вендора
        /// </summary>
        private ICommand _CmdEditSelectedVendor;
        public ICommand CmdEditSelectedVendor => _CmdEditSelectedVendor ??= new RelayCommand(OnCmdEditSelectedVendorExecuted, CanCmdEditSelectedVendorExecute);
        private bool CanCmdEditSelectedVendorExecute(object p) => SelectedVendor is not null;
        private void OnCmdEditSelectedVendorExecuted(object p)
        {
            if (!_VendorService.Edit(SelectedVendor)) return;



            //var show_dialog = new WindowEditVendor()
            //{
            //    VendorData = SelectedVendor,
            //    Owner = App.FucusedWindow ?? App.ActiveWindow,
            //    WindowStartupLocation = WindowStartupLocation.CenterOwner
            //};
            //if (!show_dialog.ShowDialog().Value) return;

            //SelectedVendor = show_dialog.VendorData;








            //if (p is not DataGrid ModulesDataGrid) return;

            //SelectedVendor.SelectedModuleType.Modules.Remove(SelectedVendor.SelectedModuleType.SelectedModule);
            //if (SelectedVendor.SelectedModuleType.Modules.Count > 0)
            //    SelectedVendor.SelectedModuleType.SelectedModule = SelectedVendor.SelectedModuleType.Modules[^1];
            //else
            //    SelectedVendor.SelectedModuleType.SelectedModule.Name = "";

            //OnPropertyChanged(nameof(DataViewVendors));
            //OnPropertyChanged(nameof(DataViewVendorModuleType));
            //OnPropertyChanged(nameof(SelectedVendor));
            //ModulesDataGrid.ItemsSource = SelectedVendor.SelectedModuleType.Modules;
            //ModulesDataGrid.Items.Refresh();
        }
        #endregion

        #endregion
    }
}
