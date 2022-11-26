using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Data;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsDIUserControlViewModel : ViewModel
    {
        #region Конструктор
        ILayotRackService _LayotRackService;

        public TableSignalsUserControlViewModel TableSignalsViewModel { get; }

        public SignalsDIUserControlViewModel(ILayotRackService iLayotRackService, TableSignalsUserControlViewModel tableSignalsViewModel)
        {
            _LayotRackService = iLayotRackService;
            TableSignalsViewModel = tableSignalsViewModel;

            var index = 0;
            var signals = new List<SignalDI>();
            while (signals.Count < 2500)
            {
                var signal = new SignalDI()
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"di_shared[{index}]",
                        Area = "",
                        Address = "",
                        LinkValue = ""
                    }
                };


                signals.Add(signal);
            }


            _DataView.Filter += OnSignalsDIFiltered;
            _DataView.Source = signals;
            _DataView.View.Refresh();
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Сигналы DI";
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
        private string _Description = "Дискретные сигналы (DI)";
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
            set => Set(ref _IsSelected, value);
        }
        #endregion

        #region Коллекция сигналов DI
        /// <summary>
        /// Коллекция сигналов DI
        /// </summary>
        private readonly CollectionViewSource _DataView = new();
        public ICollectionView DataView => _DataView?.View;
        #endregion

        #region Выбранный сигнал DI
        private SignalDI _SelectedSignalDI = new();
        /// <summary>
        /// Выбранный сигнал DI
        /// </summary>
        public SignalDI SelectedSignalDI
        {
            get => _SelectedSignalDI;
            set => Set(ref _SelectedSignalDI, value);
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

        #endregion

        #region Команды

        #region Команда - Обновить адреса модулей
        private ICommand _CmdGeneratedTable;
        /// <summary>
        /// Команда - Обновить адреса модулей
        /// </summary>
        public ICommand CmdGeneratedTable => _CmdGeneratedTable ??= new RelayCommand(OnCmdGeneratedTableExecuted, CanCmdGeneratedTableExecute);
        private bool CanCmdGeneratedTableExecute(object p) => true;

        private void OnCmdGeneratedTableExecuted(object p)
        {
            var index = 0;
            var signals = new List<SignalDI>();
            if (TableSignalsViewModel.DataViewModules is not null)
            {
                foreach (RackModule Modules in TableSignalsViewModel.DataViewModules)
                {

                    if (Modules.Type == TypeModule.DI)
                    {
                        foreach (var _Channel in Modules.Channels)
                        {
                            var signal = new SignalDI()
                            {
                                Signal = new BaseSignal
                                {
                                    Index = $"{++index}",
                                    Id = _Channel.Id,
                                    Description = _Channel.Description,
                                    VarName = $"di_shared[{index}]",
                                    Area = "",
                                    Address = $"{int.Parse(_Channel.Address) - 100000}",
                                    LinkValue = ""
                                }
                            };
                            signals.Add(signal);
                        }
                    }
                }
            }

            while (signals.Count < 100)
            {
                var signal = new SignalDI()
                {
                    Signal = new BaseSignal
                    {
                        Index = $"{++index}",
                        Id = "",
                        Description = "",
                        VarName = $"di_shared[{index}]",
                        Area = "",
                        Address = "",
                        LinkValue = ""
                    }
                };
                signals.Add(signal);
            }

            _DataView.Source = signals;
            _DataView.View.Refresh();
            OnPropertyChanged(nameof(DataView));
        }
        #endregion

        #endregion


        #region Фильтрация модулей
        /// <summary>
        /// Фильтрация модулей
        /// </summary>
        public void OnSignalsDIFiltered(object sender, FilterEventArgs e)
        {
            #region Проверки до начала фильтрации
            // Выходим, если источник события не имеет нужный нам тип фильтрации, фильтр не установлен
            if (e.Item is not SignalDI Signal || Signal is null) { e.Accepted = false; return; }
            if (string.IsNullOrWhiteSpace(TextFilter)) return;
            #endregion

            if (Signal.Signal.Description.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase) ||
                    Signal.Signal.Id.Contains(TextFilter, StringComparison.CurrentCultureIgnoreCase)) return;

            e.Accepted = false;
        }
        #endregion
    }
}
