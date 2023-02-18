using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class LayotRackUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public LayotRackUserControlViewModel()
        {
            Title = "Компоновка корзин";
            Description = $"Компоновка корзин {App.Settings.Config.NameProject}";
            UsingUserControl = new LayotRackUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly ILayotRackService LayotRackServices;
        public LayotRackUserControlViewModel(ISignalService _ISignalService, ILayotRackService _ILayotRackService) : this()
        {
            SignalServices = _ISignalService;
            LayotRackServices = _ILayotRackService;
        }
        #endregion

        #region Параметры

        #region Состояние активной вкладки
        private bool _IsSelected = false;
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public override bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (Set(ref _IsSelected, value))
                    if (_IsSelected) SignalServices.ResetSignal();
            }
        }
        #endregion

        #region Список УСО
        private ObservableCollection<USO> _Params = new();
        /// <summary>
        /// Список УСО
        /// </summary>
        public ObservableCollection<USO> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                    if (Params is not null && _Params.Count > 0)
                        SelectedParam = _Params[0];
            }
        }
        #endregion

        #region Выбранное УСО
        private USO _SelectedParam = new();
        /// <summary>
        /// Выбранное УСО
        /// </summary>
        public USO SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
        }
        #endregion

        #region Выбранная корзина
        private Rack _SelectedSubParam = new();
        /// <summary>
        /// Выбранная корзина
        /// </summary>
        public Rack SelectedSubParam
        {
            get => _SelectedSubParam;
            set => Set(ref _SelectedSubParam, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Создать новое УСО
        private ICommand _CmdCreateNewUSO;
        /// <summary>
        /// Команда - Создать новое УСО
        /// </summary>
        public ICommand CmdCreateNewUSO => _CmdCreateNewUSO ??= new RelayCommand(OnCmdCreateNewUSOExecuted);
        private void OnCmdCreateNewUSOExecuted()
        {
            CreateNewUSO(Params);
            SelectedParam = Params[^1];
        }
        #endregion

        #region Команда - Удалить выбранное УСО
        private ICommand _CmdDeleteSelectedUSO;
        /// <summary>
        /// Команда - Удалить выбранное УСО
        /// </summary>
        public ICommand CmdDeleteSelectedUSO => _CmdDeleteSelectedUSO ??= new RelayCommand(OnCmdDeleteSelectedUSOExecuted, CanCmdDeleteSelectedUSOExecute);
        private bool CanCmdDeleteSelectedUSOExecute() => SelectedParam is not null;
        private void OnCmdDeleteSelectedUSOExecuted()
        {
            var index = Params.IndexOf(SelectedParam);
            index = index == 0 ? index : index - 1;

            Params.Remove(SelectedParam);
            if (Params.Count > 0)
                SelectedParam = Params[index];
        }
        #endregion

        #region Команда - Создать новую корзину в выбранном УСО
        private ICommand _CmdCreateNewRack;
        /// <summary>
        /// Команда - Создать новую корзину в выбранном УСО
        /// </summary>
        public ICommand CmdCreateNewRack => _CmdCreateNewRack ??= new RelayCommand(OnCmdCreateNewRackExecuted, CanCmdCreateNewRackExecute);
        private bool CanCmdCreateNewRackExecute(object p) => SelectedParam is not null;
        private void OnCmdCreateNewRackExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            CreateNewRack(SelectedParam);
            SelectedSubParam = SelectedParam.Racks[^1];
        }
        #endregion

        #region Команда - Удалить выбранную корзину в выбранном УСО
        private ICommand _CmdDeleteSelectedRack;
        /// <summary>
        /// Команда - Удалить выбранную корзину в выбранном УСО
        /// </summary>
        public ICommand CmdDeleteSelectedRack => _CmdDeleteSelectedRack ??= new RelayCommand(OnCmdDeleteSelectedRackExecuted, CanCmdDeleteSelectedRackExecute);
        private bool CanCmdDeleteSelectedRackExecute() => SelectedSubParam is not null;
        private void OnCmdDeleteSelectedRackExecuted()
        {
            var index = SelectedParam.Racks.IndexOf(SelectedSubParam);
            index = index == 0 ? index : index - 1;

            SelectedParam.Racks.Remove(SelectedSubParam);
            if (SelectedParam.Racks.Count > 0)
                SelectedSubParam = SelectedParam.Racks[index];
        }
        #endregion

        #region Команда - Обновить индексы модулей
        private ICommand _CmdRefreshIndexModules;
        /// <summary>
        /// Команда - Обновить индексы модулей
        /// </summary>
        public ICommand CmdRefreshIndexModules => _CmdRefreshIndexModules ??= new RelayCommand(OnCmdRefreshIndexModulesExecuted, CanCmdRefreshIndexModulesExecute);
        private bool CanCmdRefreshIndexModulesExecute(object p) => SelectedParam is not null;
        private void OnCmdRefreshIndexModulesExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            LayotRackServices.RefreshIndexModule(SelectedParam);
            MyDataGrid.Items.Refresh();
        }
        #endregion

        #region Команда - Обновить адресов модулей
        private ICommand _CmdRefreshAddressModules;
        /// <summary>
        /// Команда - Обновить адресов модулей
        /// </summary>
        public ICommand CmdRefreshAddressModules => _CmdRefreshAddressModules ??= new RelayCommand(OnCmdRefreshAddressModulesExecuted, CanCmdRefreshAddressModulesExecute);
        private bool CanCmdRefreshAddressModulesExecute(object p) => Params is not null && Params.Count > 0;

        private void OnCmdRefreshAddressModulesExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            LayotRackServices.RefreshAddressModule(Params);
            MyDataGrid.Items.Refresh();
        }
        #endregion

        #region Команда - Включить\Исключить из обработки все корзины выбранного УСО
        private ICommand _CmdChangeStateRacks;
        /// <summary>
        /// Команда - Включить\Исключить из обработки все корзины выбранного УСО
        /// </summary>
        public ICommand CmdChangeStateRacks => _CmdChangeStateRacks ??= new RelayCommand(OnCmdChangeStateRacksExecuted, CanCmdChangeStateRacksExecute);
        private bool CanCmdChangeStateRacksExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeStateRacksExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            var AllIsEnable = true;
            foreach (var _Rack in SelectedParam.Racks)
                AllIsEnable = AllIsEnable && _Rack.IsEnable;
            SelectedParam.IsAllEnable = !AllIsEnable;

            foreach (var _Rack in SelectedParam.Racks)
                _Rack.IsEnable = SelectedParam.IsAllEnable;

            OnPropertyChanged(nameof(SelectedParam));

            MyDataGrid.Items.Refresh();
        }
        #endregion

        #region Команда - Включить\Исключить из обработки выбранную корзину
        private ICommand _CmdChangeStateSelectedRack;
        /// <summary>
        /// Команда - Включить\Исключить из обработки выбранную корзину
        /// </summary>
        public ICommand CmdChangeStateSelectedRack => _CmdChangeStateSelectedRack ??= new RelayCommand(OnCmdChangeStateSelectedRackExecuted, CanCmdChangeStateSelectedRackExecute);
        private bool CanCmdChangeStateSelectedRackExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeStateSelectedRackExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            SelectedParam.IsAllEnable = true;
            foreach (var _Rack in SelectedParam.Racks)
                SelectedParam.IsAllEnable = SelectedParam.IsAllEnable && _Rack.IsEnable;

            OnPropertyChanged(nameof(SelectedParam));
            MyDataGrid.Items.Refresh();
        }
        #endregion

        #endregion

        #region Функции

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public override ObservableCollection<T> GetParams<T>() => Params as ObservableCollection<T>;
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<USO>;
        #endregion

        #region Создаем новое УСО
        private static void CreateNewUSO(ObservableCollection<USO> _Params)
        {
            // Первое УСО - это КЦ. Для него создаем сразу 6 корзин, для всех остальных - одна корзина по умолчанию
            var racks = new ObservableCollection<Rack>();
            var RackCount = _Params.Count == 0 ? 6 : 1;                         // Кол-во корзин в УСО
            var NameUSO = _Params.Count == 0 ? $"КЦ" : $"УСО {_Params.Count}";  // Имя УСО
            var Index = $"{_Params.Count + 1}";                                 // Индекс создаваемого УСО

            for (int i = 0; i < RackCount; i++)
            {
                #region Создаем модули
                var modules = Enumerable.Range(1, 26).Select(
                    j => new RackModule()
                    {
                        Type = TypeModule.Unknown,
                        Index = $"{i + 1}.{j}",
                        Name = $"",
                        EndAddress = $"",
                        StartAddress = $"",
                        Channels = null
                    });
                #endregion

                #region Создаем корзины
                var rack = new Rack()
                {
                    Index = $"{i + 1}",
                    Name = $"A{i + 1}",
                    IsEnable = true,
                    Modules = new ObservableCollection<RackModule>(modules)
                };
                racks.Add(rack);
                #endregion
            }

            #region Создаем УСО
            var uso = new USO()
            {
                Index = Index,
                Name = NameUSO,
                Racks = new ObservableCollection<Rack>(racks)
            };
            _Params.Add(uso);
            #endregion
        }
        #endregion

        #region Создаем новую корзину
        private static void CreateNewRack(USO _SelectedParam)
        {
            var IndexRack = $"A{_SelectedParam.Racks.Count + 1}";

            #region Создаем модули
            var modules = Enumerable.Range(1, 26).Select(
                i => new RackModule()
                {
                    Type = TypeModule.Unknown,
                    Index = $"{IndexRack}.{i}",
                    Name = $"",
                    EndAddress = $"",
                    StartAddress = $"",
                    Channels = null
                });
            #endregion

            #region Создаем корзину
            var rack = new Rack()
            {
                Index = IndexRack,
                Name = IndexRack,
                IsEnable = true,
                Modules = new ObservableCollection<RackModule>(modules)
            };
            _SelectedParam.Racks.Add(rack);
            #endregion
        }
        #endregion

        #endregion
    }
}
