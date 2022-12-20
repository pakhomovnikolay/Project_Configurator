using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class UstRealUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public UstRealUserControlViewModel(ISignalService signalService, IDBService dBService)
        {
            _SignalService = signalService;
            _DBService = dBService;

            _DBService.RefreshDataViewModel(this, false);
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Уставки Real";
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Описание вкладки
        private string _Description = "Уставки типа Real";
        /// <summary>
        /// Описание вкладки
        /// </summary>
        public string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
        }
        #endregion

        #region Высота окна
        private int _WindowHeight = 800;
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
        private int _WindowWidth = 1740;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

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
        private List<BaseSetpointsReal> _Setpoints = new();
        /// <summary>
        /// Список уставок типа Real
        /// </summary>
        public List<BaseSetpointsReal> Setpoints
        {
            get => _Setpoints;
            set => Set(ref _Setpoints, value);
        }
        #endregion

        #region Коллекция регистров формируемых
        /// <summary>
        /// Коллекция регистров формируемых
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
