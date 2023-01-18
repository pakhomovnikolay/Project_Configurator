using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class KTPRSUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public KTPRSUserControlViewModel()
        {
            Title = "Предельные параметры";
            Description = "Массив предельных параметров общестанционных защит";
            UsingUserControl = new KTPRSUserControl();
        }

        private ISignalService SignalServices;
        private readonly IDBService DBServices;
        public KTPRSUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
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
        public override bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (Set(ref _IsSelected, value))
                {
                    //_SignalService.RedefineParam(SelectedParam.Param, _IsSelected, Title);
                    //DoSelection = _SignalService.DoSelection;
                    //if (_IsSelected)
                    //    _DataView.View.Refresh();
                }
            }
        }
        #endregion

        #region Список параметров
        private ObservableCollection<BaseKTPRS> _Params = new();
        /// <summary>
        /// Список параметров
        /// </summary>
        public ObservableCollection<BaseKTPRS> Params
        {
            get => _Params;
            set => Set(ref _Params, value);
        }
        #endregion

        #region Выбранный параметр
        private BaseKTPRS _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseKTPRS SelectedParam
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

        #region Команды

        #region Команда - Сменить адрес сигнала
        private ICommand _CmdChangeAddressSignal;
        /// <summary>
        /// Команда - Сменить адрес сигнала
        /// </summary>
        public ICommand CmdChangeAddressSignal => _CmdChangeAddressSignal ??= new RelayCommand(OnCmdChangeAddressSignalExecuted, CanCmdChangeAddressSignalExecute);
        private bool CanCmdChangeAddressSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdChangeAddressSignalExecuted(object p)
        {
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedParam is null) return;

            //var data_list = (ObservableCollection<BaseKTPRS>)_DataView.Source ?? new ObservableCollection<BaseKTPRS>();
            //if (Index != SelectedParam.Param.Index)
            //    SelectedParam = data_list[int.Parse(Index) - 1];

            //_SignalService.DoSelection = true;
            //_SignalService.ListName = Title;
            //_SignalService.Type = TypeModule.Unknown;

            //var NameListSelected = "";
            //if (string.IsNullOrWhiteSpace(SelectedParam.Param.TypeSignal) || int.Parse(SelectedParam.Param.TypeSignal) == 0)
            //{
            //    NameListSelected = "Сигналы DI";
            //    _SignalService.Type = TypeModule.DI;
            //}
            //else if (int.Parse(SelectedParam.Param.TypeSignal) > 1)
            //{
            //    NameListSelected = "Сигналы AI";
            //    _SignalService.Type = TypeModule.AI;
            //}
            //else if (int.Parse(SelectedParam.Param.TypeSignal) > 0)
            //{
            //    NameListSelected = "Группы сигналов";
            //    _SignalService.Type = TypeModule.DI;
            //}

            //if (App.FucusedTabControl == null) return;
            //foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
            //                         let _TabItem = _Item as TabItem
            //                         where _TabItem.Header.ToString() == NameListSelected
            //                         select _TabItem)
            //    App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion
    }
}
