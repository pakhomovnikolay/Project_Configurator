using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class HandMapUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public HandMapUserControlViewModel()
        {
            Title = "Карта ручн. ввода";
            Description = "Карта ручного ввода состояния оборудования";
            UsingUserControl = new HandMapUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public HandMapUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
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
                    //_SignalService.ResetSignal();
                    //DoSelection = false;
                }
            }
        }
        #endregion

        #region Список параметров карты ручного ввода
        private ObservableCollection<BaseParam> _Params = new();
        /// <summary>
        /// Список параметров карты ручного ввода
        /// </summary>
        public ObservableCollection<BaseParam> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
        #endregion

        #region Выбранный параметр карты ручного ввода
        private BaseParam _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр карты ручного ввода
        /// </summary>
        public BaseParam SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
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

        #region Функции

        //#region Генерация сигналов
        //public void GeneratedData()
        //{
        //    _DataView.Source = BaseParams;
        //    _DataView.View?.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion 

        #endregion
    }
}
