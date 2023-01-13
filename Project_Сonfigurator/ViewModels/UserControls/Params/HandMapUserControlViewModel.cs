using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

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

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public HandMapUserControlViewModel(ISignalService signalService, IDBService dBService) : this()
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

        #region Список параметров карты ручного ввода
        private ObservableCollection<BaseParam> _BaseParams = new();
        /// <summary>
        /// Список параметров карты ручного ввода
        /// </summary>
        public ObservableCollection<BaseParam> BaseParams
        {
            get => _BaseParams;
            set => Set(ref _BaseParams, value);
        }
        #endregion

        #region Коллекция параметров карты ручного ввода
        /// <summary>
        /// Коллекция параметров карты ручного ввода
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный параметр карты ручного ввода
        private BaseParam _SelectedSignal = new();
        /// <summary>
        /// Выбранный параметр карты ручного ввода
        /// </summary>
        public BaseParam SelectedSignal
        {
            get => _SelectedSignal;
            set => Set(ref _SelectedSignal, value);
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
            _DataView.Source = BaseParams;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
