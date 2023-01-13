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
    public class UstCommonUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UstCommonUserControlViewModel()
        {
            Title = "Врем. уставки общие";
            Description = "Временные уставки общие";
            UsingUserControl = new UstCommonUserControl();
        }

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public UstCommonUserControlViewModel(ISignalService signalService, IDBService dBService) : this()
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

        #region Список уставок
        private ObservableCollection<BaseSetpoints> _Setpoints = new();
        /// <summary>
        /// Список уставок
        /// </summary>
        public ObservableCollection<BaseSetpoints> Setpoints
        {
            get => _Setpoints;
            set => Set(ref _Setpoints, value);
        }
        #endregion

        #region Коллекция уставок
        /// <summary>
        /// Коллекция уставок
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранная уставка
        private BaseSetpoints _SelectedParam = new();
        /// <summary>
        /// Выбранная уставка
        /// </summary>
        public BaseSetpoints SelectedParam
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
