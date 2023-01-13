﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.DialogControl;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels
{
    public class SettingWindowViewModels : ViewModel
    {
        #region Конструктор
        public SettingWindowViewModels()
        {
            Title = "Настройки";
        }

        private ISettingService SettingServices;
        private IEditService EditServices;
        private IUserDialogService UserDialog;

        public SettingWindowViewModels(ISettingService _ISettingService, IEditService _EditServices, IUserDialogService _UserDialog) : this()
        {
            SettingServices = _ISettingService;
            EditServices = _EditServices;
            UserDialog = _UserDialog;
        }
        #endregion

        #region Параметры

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
        private SettingApp _Config = App.Settings.Config ?? new();
        /// <summary>
        /// Настройки приложения
        /// </summary>
        public SettingApp Config
        {
            get => _Config;
            set => Set(ref _Config, value);
        }
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

        #region Выбранный сервер подключения к БД
        private SettingServerDB _SelectedServerDB = new();
        /// <summary>
        /// Выбранный сервер подключения к БД
        /// </summary>
        public SettingServerDB SelectedServerDB
        {
            get => _SelectedServerDB;
            set => Set(ref _SelectedServerDB, value);
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
            var msg = "Для применения настроек\nнеобходимо перезапустить приложение.\nПродолжить?";
            if (!UserDialog.SendMessage(Title, msg, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes)) return;

            SettingServices.Config = _Config;
            if (!SettingServices.Save())
            {
                UserDialog.SendMessage(Title, "Ошибка сохранения конфигурации.\nсм. лог", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }



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
                ModuleTypes = new ObservableCollection<VendorModuleType>(),
            };

            Config.Vendors.Add(vendor);
            SelectedVendor = Config.Vendors[^1];
            //_DataViewVendors.Source = Config.Vendors;
            //_DataViewVendors.View.Refresh();
            //OnPropertyChanged(nameof(DataViewVendors));
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

            if (!UserDialog.SendMessage(Title, "Вы действительно хотите\nудалить выбранного вендора?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes,
                MessageBoxOptions.None))
                return;

            var index = Config.Vendors.IndexOf(SelectedVendor);
            index = index == 0 ? index : index - 1;

            Config.Vendors.Remove(SelectedVendor);
            if (Config.Vendors.Count > 0)
                SelectedVendor = Config.Vendors[index];
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
            if (!EditServices.Edit(SelectedVendor)) return;
        }
        #endregion

        #region Команда - Открыть окно настроек управления устройствами
        /// <summary>
        /// Команда - Открыть окно настроек управления устройствами
        /// </summary>
        private ICommand _CmdOpenWindowEditDevice;
        public ICommand CmdOpenWindowEditDevice => _CmdOpenWindowEditDevice ??= new RelayCommand(OnCmdOpenWindowEditDeviceExecuted, CanCmdOpenWindowEditDeviceExecute);
        private bool CanCmdOpenWindowEditDeviceExecute(object p) => SelectedVendor is not null;
        private void OnCmdOpenWindowEditDeviceExecuted(object p)
        {
            if (p is not string Content) return;
            if (string.IsNullOrWhiteSpace(Content)) return;
            var window = new Window();
            switch (Content)
            {

                case "Настройки агрегатов":
                    window = new WindowEditDevice()
                    {
                        _Title = Content,
                        InputParam = Config.UMPNA.InputParams,
                        OutputParam = Config.UMPNA.OutputParams,
                        Setpoints = Config.UMPNA.Setpoints
                    };
                    break;
                case "Настройки задвижек":
                    window = new WindowEditDevice()
                    {
                        _Title = Content,
                        InputParam = Config.UZD.InputParams,
                        OutputParam = Config.UZD.OutputParams,
                        Setpoints = Config.UZD.Setpoints
                    };
                    break;
                case "Настройки вспомсистем":
                    window = new WindowEditDevice()
                    {
                        _Title = Content,
                        InputParam = Config.UVS.InputParams,
                        OutputParam = Config.UVS.OutputParams,
                        Setpoints = Config.UVS.Setpoints
                    };
                    break;
                default:
                    break;
            }



            window.ShowDialog();
        }
        #endregion

        #region Команда - Открыть окно настроек параметров
        /// <summary>
        /// Команда - Открыть окно настроек параметров
        /// </summary>
        private ICommand _CmdOpenWindowEditDefaultMapDefence;
        public ICommand CmdOpenWindowEditDefaultMapDefence => _CmdOpenWindowEditDefaultMapDefence ??= new RelayCommand(OnCmdOpenWindowEditDefaultMapDefenceExecuted, CanCmdOpenWindowEditDefaultMapDefenceExecute);
        private bool CanCmdOpenWindowEditDefaultMapDefenceExecute(object p) => SelectedVendor is not null;
        private void OnCmdOpenWindowEditDefaultMapDefenceExecuted(object p)
        {
            if (p is not string Content) return;
            if (string.IsNullOrWhiteSpace(Content)) return;

            var window = new WindowEditDefaultMapDefense()
            {
                Title = Content,
                DefaultMap = Config.DefualtMapKGMPNA,
                Owner = App.ActiveWindow ?? App.FucusedWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            switch (Content)
            {
                case "Настройка карты агрегатных готовностей":
                    window.DefaultMap = Config.DefualtMapKGMPNA;
                    break;
                case "Настройка карты общестанционных защит":
                    window.DefaultMap = Config.DefualtMapKTPR;
                    break;
                case "Настройка карты агрегатных защит":
                    window.DefaultMap = Config.DefualtMapKTPRA;
                    break;
                case "Настройка карты предельных параметров агрегатных защит":
                    window.DefaultMap = Config.DefualtMapKTPRAS;
                    break;
                case "Настройка карты предельных параметров общестанционных защит":
                    window.DefaultMap = Config.DefualtMapKTPRS;
                    break;
                case "Настройка карты общесистемной сигнализации":
                    window.DefaultMap = Config.DefualtMapSignaling;
                    break;
                default:
                    break;
            }
            if (!window.ShowDialog().Value) return;
            switch (Content)
            {
                case "Настройка карты агрегатных готовностей":
                    Config.DefualtMapKGMPNA = window.DefaultMap;
                    break;
                case "Настройка карты общестанционных защит":
                    Config.DefualtMapKTPR = window.DefaultMap;
                    break;
                case "Настройка карты агрегатных защит":
                    Config.DefualtMapKTPRA = window.DefaultMap;
                    break;
                case "Настройка карты предельных параметров агрегатных защит":
                    Config.DefualtMapKTPRAS = window.DefaultMap;
                    break;
                case "Настройка карты предельных параметров общестанционных защит":
                    Config.DefualtMapKTPRS = window.DefaultMap;
                    break;
                case "Настройка карты общесистемной сигнализации":
                    Config.DefualtMapSignaling = window.DefaultMap;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Команда - Создать новый узел подключения
        /// <summary>
        /// Команда - Создать новый узел подключения
        /// </summary>
        private ICommand _CmdCreateNewServerDB;
        public ICommand CmdCreateNewServerDB => _CmdCreateNewServerDB ??= new RelayCommand(OnCmdCreateNewServerDBExecuted, CanCmdCreateNewServerDBExecute);
        private bool CanCmdCreateNewServerDBExecute(object p) => true;
        private void OnCmdCreateNewServerDBExecuted(object p)
        {
            Config.ServerDB.Add(new SettingServerDB());
            SelectedServerDB = Config.ServerDB[^1];
            EditServices.Edit(SelectedServerDB);
        }
        #endregion

        #region Команда - Редактировать выбранный узел
        /// <summary>
        /// Команда - Редактировать выбранный узел
        /// </summary>
        private ICommand _CmdEditSelectedServerDB;
        public ICommand CmdEditSelectedServerDB => _CmdEditSelectedServerDB ??= new RelayCommand(OnCmdEditSelectedServerDBExecuted, CanCmdEditSelectedServerDBExecute);
        private bool CanCmdEditSelectedServerDBExecute(object p) => true;
        private void OnCmdEditSelectedServerDBExecuted(object p)
        {
            EditServices.Edit(SelectedServerDB);
        }
        #endregion

        #region Команда - Удалить выбранный сервер
        /// <summary>
        /// Команда - Удалить выбранный сервер
        /// </summary>
        private ICommand _CmdDeleteSelectedServerDB;
        public ICommand CmdDeleteSelectedServerDB => _CmdDeleteSelectedServerDB ??= new RelayCommand(OnCmdDeleteSelectedServerDBExecuted, CanCmdDeleteSelectedServerDBExecute);
        private bool CanCmdDeleteSelectedServerDBExecute(object p) => true;
        private void OnCmdDeleteSelectedServerDBExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            if (!UserDialog.SendMessage(Title, "Вы действительно хотите\nудалить выбранный узел?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes,
                MessageBoxOptions.None))
                return;

            var index = Config.ServerDB.IndexOf(SelectedServerDB);
            index = index == 0 ? index : index - 1;

            Config.ServerDB.Remove(SelectedServerDB);
            if (Config.ServerDB.Count > 0)
                SelectedServerDB = Config.ServerDB[index];
        }
        #endregion

        #endregion
    }
}
