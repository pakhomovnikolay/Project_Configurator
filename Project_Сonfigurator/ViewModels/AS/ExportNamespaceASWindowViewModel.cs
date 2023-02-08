using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.DataLists;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.AS
{
    public class ExportNamespaceASWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IVUNamespaceASExportRedefineService VUSocketsASExportRedefineServices;
        private readonly IUserDialogService UserDialog;

        public ExportNamespaceASWindowViewModel()
        {
            Title = "Экспорт пространства имен";
        }

        public ExportNamespaceASWindowViewModel(IVUNamespaceASExportRedefineService _VUSocketsASExportRedefineService, IUserDialogService _UserDialog) : this()
        {
            VUSocketsASExportRedefineServices = _VUSocketsASExportRedefineService;
            UserDialog = _UserDialog;

            #region Создаем CheckBox'ы
            CheckBoxs = new ObservableCollection<CheckBox>(new Lists().CheckBoxs);
            foreach (var _CheckBox in CheckBoxs)
                _CheckBox.Command = CmdSelectParam;
            #endregion
        }
        #endregion

        #region Параметры

        #region Коллекция параметров
        private ObservableCollection<CheckBox> _CheckBoxs;
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        public ObservableCollection<CheckBox> CheckBoxs
        {
            get
            {
                SelectPermissionControlCheckBoxs(_CheckBoxs);
                return _CheckBoxs;
            }
            set => Set(ref _CheckBoxs, value);
        }
        #endregion

        #region Текущее состояние флажка "Выбрать все"
        private bool _IsSelectedAll;
        /// <summary>
        /// Текущее состояние флажка "Выбрать все"
        /// </summary>
        public bool IsSelectedAll
        {
            get => _IsSelectedAll;
            set => Set(ref _IsSelectedAll, value);
        }
        #endregion

        #region Описание состояния выбранных CheckBox'сов
        private string _DescriptionSelectedAll;
        /// <summary>
        /// Описание состояния выбранных CheckBox'сов
        /// </summary>
        public string DescriptionSelectedAll
        {
            get => _DescriptionSelectedAll;
            set => Set(ref _DescriptionSelectedAll, value);
        }
        #endregion

        #region Наличие выбранного хотя бы одного из параметров экспорта
        private bool _IsSelectedOne;
        /// <summary>
        /// Наличие выбранного хотя бы одного из параметров экспорта
        /// </summary>
        public bool IsSelectedOne
        {
            get => _IsSelectedOne;
            set => Set(ref _IsSelectedOne, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Выбрать\Снять все параметры
        private ICommand _CmdSelectAllParam;
        /// <summary>
        /// Команда - Выбрать\Снять все параметры
        /// </summary>
        public ICommand CmdSelectAllParam => _CmdSelectAllParam ??= new RelayCommand(OnCmdSelectAllParamExecuted);
        private void OnCmdSelectAllParamExecuted()
        {
            IsSelectedAll = !IsSelectedAll;
            foreach (var _CheckBox in CheckBoxs)
            {
                if (_CheckBox.IsEnabled)
                    _CheckBox.IsChecked = IsSelectedAll;

                IsSelectedOne = IsSelectedAll;
            }

        }
        #endregion

        #region Команда - Выбрать\Снять один из параметров
        private ICommand _CmdSelectParam;
        /// <summary>
        /// Команда - Выбрать\Снять один из параметров
        /// </summary>
        public ICommand CmdSelectParam => _CmdSelectParam ??= new RelayCommand(OnCmdSelectParamExecuted);

        private void OnCmdSelectParamExecuted()
        {
            IsSelectedAll = true;
            IsSelectedOne = false;
            foreach (var _CheckBox in CheckBoxs)
            {
                IsSelectedAll = IsSelectedAll && _CheckBox.IsChecked == true;
                IsSelectedOne = IsSelectedOne || _CheckBox.IsChecked == true;
            }

        }
        #endregion

        #region Команда - Экспорт параметров
        private ICommand _CmdExportParams;
        /// <summary>
        /// Команда - Экспорт параметров
        /// </summary>
        public ICommand CmdExportParams => _CmdExportParams ??= new RelayCommand(OnCmdExportParamsExecuted, CanCmdExportParamsExecute);
        private bool CanCmdExportParamsExecute(object p) => IsSelectedOne;
        private void OnCmdExportParamsExecuted(object p)
        {
            if (p is not Window window) return;

            var SuccessfulCompletion = true;
            foreach (var _CheckBox in CheckBoxs)
                if (_CheckBox.IsChecked == true)
                    if (!VUSocketsASExportRedefineServices.Export(_CheckBox.Content.ToString()))
                        SuccessfulCompletion = false;

            if (!SuccessfulCompletion)
                if (UserDialog.SendMessage("Внимание!", $"Экспорт выполнен c ошибками.\nСм. лог", ImageType: MessageBoxImage.Warning)) return;

            if (UserDialog.SendMessage(Title, $"Экпорт выполнен успешно.\nДанные сохранены - {App.Settings.Config.PathExportVU}"))
                window.Close();
        }
        #endregion

        #endregion

        #region Функции

        #region Контроль разрешения выбора параметров экспорта
        /// <summary>
        /// Контроль разрешения выбора параметров экспорта
        /// </summary>
        private void SelectPermissionControlCheckBoxs(ObservableCollection<CheckBox> CheckBoxs)
        {
            foreach (var _CheckBox in CheckBoxs)
            {
                _CheckBox.IsEnabled = false;

                #region Сообщения
                if (_CheckBox.Content.ToString() == "Сообщения")
                {
                    var _ViewModel = App.Services.GetRequiredService<MessageWindowViewModel>();
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        foreach (var _Message in _Param.Messages)
                        {
                            if (string.IsNullOrWhiteSpace(_Message.Description)) continue;
                            _CheckBox.IsEnabled = true;
                        }
                    }
                }
                #endregion

                #region Диагностика
                if (_CheckBox.Content.ToString() == "Диагностика")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Компоновка корзин") as LayotRackUserControlViewModel;
                    _CheckBox.IsEnabled = _ViewModel is not null && _ViewModel.Params.Count > 0;
                }
                #endregion

                #region Сигналы AI
                if (_CheckBox.Content.ToString() == "Сигналы AI")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Сигналы AI") as SignalsAIUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Регистры формируемые
                if (_CheckBox.Content.ToString() == "Регистры формируемые")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Регистры формируемые") as UserRegUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Карта готовностей агрегатов (Лист 1)
                if (_CheckBox.Content.ToString() == "Карта готовностей агрегатов (Лист 1)")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Настройки МПНА") as UMPNAUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        foreach (var item in _Param.KGMPNA)
                        {
                            if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                            _CheckBox.IsEnabled = true;
                        }
                    }
                }
                #endregion

                #region Общестанционные защиты (Лист 2)
                if (_CheckBox.Content.ToString() == "Общестанционные защиты (Лист 2)")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Общестанционные защиты") as KTPRUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Агрегатные защиты (Лист 3)
                if (_CheckBox.Content.ToString() == "Агрегатные защиты (Лист 3)")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Настройки МПНА") as UMPNAUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        foreach (var item in _Param.KTPRA)
                        {
                            if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                            _CheckBox.IsEnabled = true;
                        }
                    }
                }
                #endregion

                #region Предельные параметры (Лист 4)
                if (_CheckBox.Content.ToString() == "Предельные параметры (Лист 4)")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Предельные параметры") as KTPRSUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Сигнализация (Лист 5)
                if (_CheckBox.Content.ToString() == "Сигнализация (Лист 5)")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Сигнализация") as SignalingUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Состояние НА
                if (_CheckBox.Content.ToString() == "Состояние НА")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Настройки МПНА") as UMPNAUserControlViewModel;
                    _CheckBox.IsEnabled = _ViewModel is not null && _ViewModel.Params.Count > 0;
                }
                #endregion

                #region Состояние ЗД
                if (_CheckBox.Content.ToString() == "Состояние ЗД")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Настройки задвижек") as UZDUserControlViewModel;
                    _CheckBox.IsEnabled = _ViewModel is not null && _ViewModel.Params.Count > 0;
                }
                #endregion

                #region Состояние ВС
                if (_CheckBox.Content.ToString() == "Состояние ВС")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Настройки вспомсистем") as UVSUserControlViewModel;
                    _CheckBox.IsEnabled = _ViewModel is not null && _ViewModel.Params.Count > 0;
                }
                #endregion

                #region Состояние ТС
                if (_CheckBox.Content.ToString() == "Состояние ТС")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("DO остальные") as UTSUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Карта ручного ввода
                if (_CheckBox.Content.ToString() == "Карта ручного ввода")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Карта ручн. ввода") as HandMapUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                #region Команды
                if (_CheckBox.Content.ToString() == "Команды")
                {
                    var _ViewModel = UserDialog.SearchControlViewModel("Команды") as CommandUserControlViewModel;
                    var Params = _ViewModel is not null && _ViewModel.Params.Count > 0 ? _ViewModel.Params : new();
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                        _CheckBox.IsEnabled = true;
                    }
                }
                #endregion

                _CheckBox.IsChecked = _CheckBox.IsChecked == true && _CheckBox.IsEnabled;
            }
        }
        #endregion

        #endregion
    }
}
