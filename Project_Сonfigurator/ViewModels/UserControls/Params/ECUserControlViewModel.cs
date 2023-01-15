using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class ECUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public ECUserControlViewModel()
        {
            Title = "Секции шин";
            Description = "Секции шин";
            UsingUserControl = new ECUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public ECUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
        {
            SignalServices = _ISignalService;
            DBServices = _IDBService;
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
            set
            {
                if (Set(ref _IsSelected, value))
                {
                    SignalServices.RedefineParam(SelectedParam, _IsSelected, Title);
                    DoSelection = SignalServices.DoSelection;
                }
            }
        }
        #endregion

        #region Список регистров формируемых
        private ObservableCollection<BaseParam> _Params = new();
        /// <summary>
        /// Список регистров формируемых
        /// </summary>
        public ObservableCollection<BaseParam> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
        #endregion

        #region Выбранный регистр
        private BaseParam _SelectedParam = new();
        /// <summary>
        /// Выбранный регистр
        /// </summary>
        public BaseParam SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
        }
        #endregion

        #region Текст фильтрации
        private string _TextFilter;
        /// <summary>
        /// Текст фильтрации
        /// </summary>
        public string TextFilter
        {
            get => _TextFilter;
            set => Set(ref _TextFilter, value);
        }
        #endregion

        #region Состояние необходимости выбора сигнала
        private bool _DoSelection;
        /// <summary>
        /// Состояние необходимости выбора сигнала
        /// </summary>
        public bool DoSelection
        {
            get => _DoSelection;
            set => Set(ref _DoSelection, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Обновить фильтр
        private ICommand _CmdRefreshFilter;
        /// <summary>
        /// Команда - Обновить фильтр
        /// </summary>
        public ICommand CmdRefreshFilter => _CmdRefreshFilter ??= new RelayCommand(OnCmdRefreshFilterExecuted, CanCmdRefreshFilterExecute);
        private bool CanCmdRefreshFilterExecute() => true;

        private void OnCmdRefreshFilterExecuted()
        {
            //if (_DataView.Source is null) return;
            //_DataView.View.Refresh();

        }
        #endregion

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedParam is null) return;


            if (Index != SelectedParam.Index)
                SelectedParam = Params[int.Parse(Index) - 1];

            SignalServices.DoSelection = true;
            SignalServices.ListName = Title;
            SignalServices.Type = TypeModule.Unknown;

            var NameListSelected = "";
            if (string.IsNullOrWhiteSpace(SelectedParam.TypeSignal) || int.Parse(SelectedParam.TypeSignal) == 0)
            {
                NameListSelected = "Сигналы DI";
                SignalServices.Type = TypeModule.DI;
            }
            else if (int.Parse(SelectedParam.TypeSignal) > 1)
            {
                NameListSelected = "Сигналы AI";
                SignalServices.Type = TypeModule.AI;
            }
            else if (int.Parse(SelectedParam.TypeSignal) > 0)
            {
                NameListSelected = "Группы сигналов";
                SignalServices.Type = TypeModule.DI;
            }

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as IViewModelUserControls
                                     where _TabItem.Title == NameListSelected
                                     select _TabItem)
                App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        private void OnSignalsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseParam Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion

        #region Генерация сигналов
        public void GeneratedData()
        {
            //_DataView.Source = BaseParams;
            //_DataView.View?.Refresh();
            //OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
