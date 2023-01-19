﻿using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

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

        private readonly ISignalService SignalServices;
        private readonly IDBService DBServices;
        public UstCommonUserControlViewModel(ISignalService _ISignalService, IDBService _IDBService) : this()
        {
            SignalServices = _ISignalService;
            DBServices = _IDBService;
            _ParamsDataView.Filter += ParamsFiltered;
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
                    if (SelectedParam is not null)
                        SignalServices.ResetSignal();
                    DoSelection = SignalServices.DoSelection;
                }
            }
        }
        #endregion

        #region Список парметров
        private ObservableCollection<BaseSetpoints> _Params = new();
        /// <summary>
        /// Список парметров
        /// </summary>
        public ObservableCollection<BaseSetpoints> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (_Params is null || _Params.Count <= 0)
                    {
                        CreateData();
                        RefreshDataView();
                    }
                    else RefreshDataView();
                }
            }
        }
        #endregion

        #region Выбранный параметр
        private BaseSetpoints _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseSetpoints SelectedParam
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

        #region Коллекция парметров для отображения
        /// <summary>
        /// Коллекция парметров для отображения
        /// </summary>
        private readonly CollectionViewSource _ParamsDataView = new();
        public ICollectionView ParamsDataView => _ParamsDataView?.View;
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
            RefreshDataView();
        }
        #endregion 

        #endregion

        #region Функции

        #region Фильтрация парметров
        /// <summary>
        /// Фильтрация парметров
        /// </summary>
        public void ParamsFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not BaseSetpoints _Param || _Param is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            #region Параметры
            if (_Param.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                _Param.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
            #endregion
        }
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            while (Params.Count < 400)
            {
                #region Создаем параметр
                var _Param = new BaseSetpoints
                {
                    Index = $"{Params.Count + 1}",
                    Id = $"H{(Params.Count + 1):000}",
                    Description = "",
                    VarName = $"SP_TM_COMMON[{Params.Count + 1}]",
                    Address = $"{1000 + Params.Count}",
                    Value = "",
                    Unit = "сек.",
                };
                #endregion

                Params.Add(_Param);
            }
            if (Params.Count > 0)
                SelectedParam = Params[0];
        }
        #endregion

        #region Обновляем данные для отображения
        /// <summary>
        /// Обновляем данные для отображения
        /// </summary>
        private void RefreshDataView()
        {
            _ParamsDataView.Source = Params;
            _ParamsDataView.View?.Refresh();
            OnPropertyChanged(nameof(ParamsDataView));
        }
        #endregion

        #endregion

    }
}
