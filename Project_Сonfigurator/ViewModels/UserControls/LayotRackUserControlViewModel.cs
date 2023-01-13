using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class LayotRackUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public LayotRackUserControlViewModel()
        {
            Title = "Компоновка корзин";
            Description = "Компоновка корзин НПС-1 \"Сызрань\"";
            UsingUserControl = new LayotRackUserControl();
        }

        private readonly ILayotRackService LayotRackServices;
        private readonly IDBService DBServices;
        public LayotRackUserControlViewModel(ILayotRackService _ILayotRackService, IDBService _IDBService) : this()
        {
            LayotRackServices = _ILayotRackService;
            DBServices = _IDBService;

            if (App.DBServices is null || App.DBServices.AppData is null || App.DBServices.AppData.USOList.Count <= 0)
                OnCmdCreateNewUSOExecuted();
            else
                DBServices.RefreshDataViewModel(this, false);
        }
        #endregion

        #region Параметры

        #region Состояние активной вкладки
        private bool _IsSelected = false;
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public bool IsSelected
        {
            get => _IsSelected;
            set => Set(ref _IsSelected, value);
        }
        #endregion

        #region Список УСО
        private ObservableCollection<USO> _USOList = new();
        /// <summary>
        /// Список УСО
        /// </summary>
        public ObservableCollection<USO> USOList
        {
            get => _USOList;
            set => Set(ref _USOList, value);
        }
        #endregion

        #region Коллекция УСО для отображения
        /// <summary>
        /// Коллекция УСО для отображения
        /// </summary>
        //private readonly CollectionViewSource _DataView = new();
        //public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранное УСО
        private USO _SelectedUSO = new();
        /// <summary>
        /// Выбранное УСО
        /// </summary>
        public USO SelectedUSO
        {
            get => _SelectedUSO;
            set => Set(ref _SelectedUSO, value);
        }
        #endregion

        #region Коллекция корзин для отображения
        /// <summary>
        /// Коллекция корзин для отображения
        /// </summary>
        //private readonly CollectionViewSource _DataViewRacks = new();
        //public ICollectionView DataViewRacks => _DataViewRacks?.View;
        #endregion

        #region Выбранная корзина
        private Rack _SelectedRack = new();
        /// <summary>
        /// Выбранная корзина
        /// </summary>
        public Rack SelectedRack
        {
            get => _SelectedRack;
            set => Set(ref _SelectedRack, value);
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
            var modules = new ObservableCollection<RackModule>();
            var racks = new ObservableCollection<Rack>();

            #region Создаем корзины и модули
            // Первое УСО - это КЦ. Для него создаем сразу 6 корзин, для всех остальный, одна корзина по умолчанию
            if (USOList.Count == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    modules = new ObservableCollection<RackModule>();

                    #region Создаем модули
                    // Для каждой корзины 26 модулей
                    for (int j = 0; j < 26; j++)
                    {
                        var module = new RackModule()
                        {
                            Type = TypeModule.Unknown,
                            Index = $"A{i + 1}.{j + 1}",
                            Name = $"",
                            EndAddress = $"",
                            StartAddress = $"",
                            Channels = null
                        };
                        modules.Add(module);
                    }
                    #endregion

                    #region Создаем корзины
                    var rack = new Rack()
                    {
                        Index = $"{i + 1}",
                        Name = $"A{i + 1}",
                        IsEnable = true,
                        Modules = modules
                    };
                    racks.Add(rack);
                    #endregion
                }
            }
            else
            {
                #region Создаем модули
                // Для каждой корзины 26 модулей
                for (int j = 0; j < 26; j++)
                {
                    var module = new RackModule()
                    {
                        Type = TypeModule.Unknown,
                        Index = $"A1.{j + 1}",
                        Name = $"",
                        EndAddress = $"",
                        StartAddress = $"",
                        Channels = null
                    };
                    modules.Add(module);
                }
                #endregion

                #region Создаем корзину
                var rack = new Rack()
                {
                    Index = $"1",
                    Name = $"A1",
                    IsEnable = true,
                    Modules = modules
                };
                racks.Add(rack);
                #endregion
            }
            #endregion

            #region Создаем УСО
            var uso = new USO()
            {
                Index = $"{USOList.Count + 1}",
                Name = USOList.Count == 0 ? $"КЦ" : $"УСО {USOList.Count}",
                Racks = racks
            };
            USOList.Add(uso);
            #endregion

            SelectedUSO = USOList[^1];
            SelectedRack = USOList[^1].Racks[0];
            //_DataView.Source = USOList;
            //_DataView.View.Refresh();
        }
        #endregion

        #region Команда - Удалить выбранное УСО
        private ICommand _CmdDeleteSelectedUSO;
        /// <summary>
        /// Команда - Удалить выбранное УСО
        /// </summary>
        public ICommand CmdDeleteSelectedUSO => _CmdDeleteSelectedUSO ??= new RelayCommand(OnCmdDeleteSelectedUSOExecuted, CanCmdDeleteSelectedUSOExecute);
        private bool CanCmdDeleteSelectedUSOExecute() => SelectedUSO is not null;
        private void OnCmdDeleteSelectedUSOExecuted()
        {
            var index = USOList.IndexOf(SelectedUSO);
            index = index == 0 ? index : index - 1;

            USOList.Remove(SelectedUSO);
            if (USOList.Count > 0)
            {
                SelectedUSO = USOList[index];
            }
        }
        #endregion

        #region Команда - Создать новую корзину в выбранном УСО
        private ICommand _CmdCreateNewRack;
        /// <summary>
        /// Команда - Создать новую корзину в выбранном УСО
        /// </summary>
        public ICommand CmdCreateNewRack => _CmdCreateNewRack ??= new RelayCommand(OnCmdCreateNewRackExecuted, CanCmdCreateNewRackExecute);
        private bool CanCmdCreateNewRackExecute(object p) => SelectedUSO is not null;
        private void OnCmdCreateNewRackExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            var modules = new ObservableCollection<RackModule>();
            for (int i = 0; i < 26; i++)
            {
                var module = new RackModule()
                {
                    Type = TypeModule.Unknown,
                    Index = $"A{SelectedUSO.Racks.Count + 1}.{i + 1}",
                    Name = $"",
                    EndAddress = $"",
                    StartAddress = $"",
                    Channels = null
                };
                modules.Add(module);
            }

            var rack = new Rack()
            {
                Index = $"{SelectedUSO.Racks.Count + 1}",
                Name = $"A{SelectedUSO.Racks.Count + 1}",
                IsEnable = true,
                Modules = modules
            };
            SelectedUSO.Racks.Add(rack);

            SelectedRack = SelectedUSO.Racks[^1];
        }
        #endregion

        #region Команда - Удалить выбранную корзину в выбранном УСО
        private ICommand _CmdDeleteSelectedRack;
        /// <summary>
        /// Команда - Удалить выбранную корзину в выбранном УСО
        /// </summary>
        public ICommand CmdDeleteSelectedRack => _CmdDeleteSelectedRack ??= new RelayCommand(OnCmdDeleteSelectedRackExecuted, CanCmdDeleteSelectedRackExecute);
        private bool CanCmdDeleteSelectedRackExecute() => SelectedRack is not null && SelectedUSO is not null && SelectedUSO.Index != "1";
        private void OnCmdDeleteSelectedRackExecuted()
        {
            var index = SelectedUSO.Racks.IndexOf(SelectedRack);
            index = index == 0 ? index : index - 1;

            SelectedUSO.Racks.Remove(SelectedRack);
            if (SelectedUSO.Racks.Count > 0)
            {
                SelectedRack = SelectedUSO.Racks[index];
            }
        }
        #endregion

        #region Команда - Обновить индексы модулей
        private ICommand _CmdRefreshIndexModules;
        /// <summary>
        /// Команда - Обновить индексы модулей
        /// </summary>
        public ICommand CmdRefreshIndexModules => _CmdRefreshIndexModules ??= new RelayCommand(OnCmdRefreshIndexModulesExecuted, CanCmdRefreshIndexModulesExecute);
        private bool CanCmdRefreshIndexModulesExecute(object p) => SelectedRack is not null;
        private void OnCmdRefreshIndexModulesExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();
            if (!int.TryParse(SelectedRack.Name.Replace("A", ""), out int index))
                index = 1;

            SelectedRack.Name = $"A{index}";
            //_LayotRackService.RefreshIndexModule(SelectedRack.Modules, index);
            //_DataViewRacks.Source = SelectedUSO.Racks;
            //_DataViewRacks.View.Refresh();
        }
        #endregion

        #region Команда - Обновить адреса модулей
        private ICommand _CmdRefreshAddressModules;
        /// <summary>
        /// Команда - Обновить адреса модулей
        /// </summary>
        public ICommand CmdRefreshAddressModules => _CmdRefreshAddressModules ??= new RelayCommand(OnCmdRefreshAddressModulesExecuted, CanCmdRefreshAddressModulesExecute);
        private bool CanCmdRefreshAddressModulesExecute(object p) => USOList is not null && USOList.Count > 0;

        private void OnCmdRefreshAddressModulesExecuted(object p)
        {
            if (p is null) return;
            if (p is not DataGrid MyDataGrid) return;
            if (MyDataGrid.CommitEdit()) MyDataGrid.CancelEdit();

            //_LayotRackService.RefreshAddressModule(USOList);
            //_DataViewRacks.Source = SelectedUSO.Racks;
            //_DataViewRacks.View?.Refresh();
            //OnPropertyChanged(nameof(_DataView));
            //OnPropertyChanged(nameof(_DataViewRacks));

        }
        #endregion

        #endregion

        #region Функции

        #region Генерируем данные
        public void GeneratedData()
        {
            //_DataView.Source = USOList;
            //_DataView.View?.Refresh();
            //OnPropertyChanged(nameof(DataView));

            if (USOList is null || USOList.Count <= 0)
                SelectedUSO = null;
        }
        #endregion

        #endregion
    }
}
