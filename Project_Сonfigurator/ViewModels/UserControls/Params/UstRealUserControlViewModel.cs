using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UstRealUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UstRealUserControlViewModel()
        {
            Title = "Уставки Real";
            Description = "Уставки типа Real";
            UsingUserControl = new UstRealUserControl();
        }

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public UstRealUserControlViewModel(ISignalService signalService, IDBService dBService) : this()
        {
            _SignalService = signalService;
            _DBService = dBService;

            _DBService.RefreshDataViewModel(this, false);
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
                    _SignalService.ResetSignal();
                    DoSelection = false;
                }
            }
        }
        #endregion

        #region Список уставок типа Real
        private ObservableCollection<BaseSetpointsReal> _Setpoints = new();
        /// <summary>
        /// Список уставок типа Real
        /// </summary>
        public ObservableCollection<BaseSetpointsReal> Setpoints
        {
            get => _Setpoints;
            set => Set(ref _Setpoints, value);
        }
        #endregion

        #region Коллекция уставок типа Real
        /// <summary>
        /// Коллекция уставок типа Real
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранная уставка
        private BaseSetpointsReal _SelectedParam = new();
        /// <summary>
        /// Выбранная уставка
        /// </summary>
        public BaseSetpointsReal SelectedParam
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

        #region Генерация сигналов
        public void GeneratedData()
        {
            _DataView.Source = Setpoints;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
