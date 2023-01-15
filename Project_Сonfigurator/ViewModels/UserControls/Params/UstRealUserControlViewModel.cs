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

        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public UstRealUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
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

        #region Список уставок типа Real
        private ObservableCollection<BaseSetpointsReal> _Params = new();
        /// <summary>
        /// Список уставок типа Real
        /// </summary>
        public ObservableCollection<BaseSetpointsReal> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
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

        //#region Генерация сигналов
        //public void GeneratedData()
        //{
        //    _DataView.Source = Setpoints;
        //    _DataView.View?.Refresh();
        //    OnPropertyChanged(nameof(DataView));
        //}
        //#endregion 

        #endregion
    }
}
