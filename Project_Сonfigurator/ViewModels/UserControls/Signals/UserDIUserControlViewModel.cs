﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class UserDIUserControlViewModel : ViewModelUserControls
    {
        #region Конструктор
        public UserDIUserControlViewModel()
        {
            Title = "DI формируемые";
            Description = "DI формируемые от ПЛК";
            UsingUserControl = new UserDIUserControl();
        }

        private readonly ISignalService _SignalService;
        private readonly IDBService _DBService;
        public UserDIUserControlViewModel(ISignalService signalService, IDBService dBService) : this()
        {
            _SignalService = signalService;
            _DBService = dBService;

            _DataView.Filter += OnSignalsFiltered;
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
                    _SignalService.RedefineSignal(SelectedSignal, _IsSelected, Title);
                    DoSelection = _SignalService.DoSelection;
                    if (_IsSelected)
                        _DataView.View.Refresh();
                }
            }
        }
        #endregion

        #region Список сигналов DI формируемых
        private ObservableCollection<BaseSignal> _BaseSignals = new();
        /// <summary>
        /// Список сигналов DI формируемых
        /// </summary>
        public ObservableCollection<BaseSignal> BaseSignals
        {
            get => _BaseSignals;
            set => Set(ref _BaseSignals, value);
        }
        #endregion

        #region Коллекция сигналов DI формируемых
        /// <summary>
        /// Коллекция сигналов DI формируемых
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный сигнал DI формируемых
        private BaseSignal _SelectedSignal = new();
        /// <summary>
        /// Выбранный сигнал DI формируемых
        /// </summary>
        public BaseSignal SelectedSignal
        {
            get => _SelectedSignal;
            set => Set(ref _SelectedSignal, value);
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
            if (_DataView.Source is null) return;
            _DataView.View.Refresh();

        }
        #endregion

        #region Команда - Выбрать сигнал
        private ICommand _CmdSelectionSignal;
        /// <summary>
        /// Команда - Выбрать сигнал
        /// </summary>
        public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        private bool CanCmdSelectionSignalExecute(object p) => SelectedSignal is not null;

        private void OnCmdSelectionSignalExecuted(object p)
        {
            if (p is not string Index) return;
            if (string.IsNullOrWhiteSpace(Index)) return;
            if (SelectedSignal is null) return;
            if (App.FucusedTabControl == null) return;
            if (!_SignalService.DoSelection) return;

            var data_list = new ObservableCollection<BaseSignal>();
            foreach (BaseSignal Signal in DataView)
            {
                data_list.Add(Signal);
            }

            if (Index != SelectedSignal.Index)
                SelectedSignal = data_list[int.Parse(Index) - 1];

            _SignalService.Address = SelectedSignal.Address;
            _SignalService.Id = SelectedSignal.Id;
            _SignalService.Description = SelectedSignal.Description;

            if (App.FucusedTabControl == null) return;
            foreach (var _TabItem in from object _Item in App.FucusedTabControl.Items
                                     let _TabItem = _Item as TabItem
                                     where _TabItem.Header.ToString() == _SignalService.ListName
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
            if (e.Item is not BaseSignal Signal || Signal is null) { e.Accepted = false; return; }
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
            _DataView.Source = BaseSignals;
            _DataView.View?.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion 

        #endregion
    }
}
