using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class SignalsGroupUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public SignalsGroupUserControlViewModel()
        {
            Title = "Сигналы групп";
            Description = "Список сигналов состоящих в группе";
            UsingUserControl = new SignalsGroupUserControl();
        }

        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public SignalsGroupUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
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
                    //_SignalService.RedefineParam(SelectedParam, _IsSelected, Title);
                    //DoSelection = _SignalService.DoSelection;
                    //if (_IsSelected)
                    //    _DataView.View.Refresh();
                }
            }
        }
        #endregion

        #region Список параметр
        private ObservableCollection<BaseParam> _Params = new();
        /// <summary>
        /// Список параметр
        /// </summary>
        public ObservableCollection<BaseParam> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                    if (_Params is null || _Params.Count <= 0)
                        CreateData();
            }
        }
        #endregion

        #region Выбранный параметр
        private BaseParam _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
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
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedParam is null) return;

            //var data_list = new ObservableCollection<BaseParam>();
            //foreach (BaseParam Param in DataView)
            //{
            //    data_list.Add(Param);
            //}

            //if (Index != SelectedParam.Index)
            //    SelectedParam = data_list[int.Parse(Index) - 1];

            //_SignalService.DoSelection = true;
            //_SignalService.ListName = Title;
            //_SignalService.Type = TypeModule.Unknown;

            //var NameListSelected = "";
            //if (string.IsNullOrWhiteSpace(SelectedParam.TypeSignal) || int.Parse(SelectedParam.TypeSignal) == 0)
            //{
            //    NameListSelected = "Сигналы DI";
            //    _SignalService.Type = TypeModule.DI;
            //}
            //else if (int.Parse(SelectedParam.TypeSignal) > 1)
            //{
            //    NameListSelected = "Сигналы AI";
            //    _SignalService.Type = TypeModule.AI;
            //}
            //else if (int.Parse(SelectedParam.TypeSignal) > 0)
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

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SelectedParam is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            //if (p is not string Index) return;
            //if (string.IsNullOrWhiteSpace(Index)) return;
            //if (SelectedParam is null) return;
            //if (App.FucusedTabControl == null) return;
            //if (!_SignalService.DoSelection) return;

            //var data_list = new ObservableCollection<BaseParam>();
            //foreach (BaseParam Signal in DataView)
            //{
            //    data_list.Add(Signal);
            //}

            //if (Index != SelectedParam.Index)
            //    SelectedParam = data_list[int.Parse(Index) - 1];

            //_SignalService.Address = SelectedParam.Index;
            //_SignalService.Id = SelectedParam.Id;
            //_SignalService.Description = SelectedParam.Description;

            //if (App.FucusedTabControl == null) return;
            //foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
            //                         let _TabItem = _Item as TabItem
            //                         where _TabItem.Header.ToString() == _SignalService.ListName
            //                         select _TabItem)
            //    App.FucusedTabControl.SelectedItem = _TabItem;
        }
        #endregion

        #endregion

        #region Функции

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            while (Params.Count < 256)
            {
                var param = new BaseParam
                {
                    Index = $"{Params.Count + 1}",
                    Id = "",
                    Description = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = "",
                    VarName = ""
                };
                Params.Add(param);
            }
            if (Params.Count > 0)
                SelectedParam = Params[0];
        }
        #endregion

        #endregion
    }
}
