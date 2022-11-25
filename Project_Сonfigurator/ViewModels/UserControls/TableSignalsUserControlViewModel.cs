using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class TableSignalsUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;
        ILayotRackService _LayotRackService;
        public LayotRackUserControlViewModel LayotRackViewModel { get; }

        public TableSignalsUserControlViewModel(
            IUserDialogService userDialog,
            ILogSerivece logSerivece,
            ILayotRackService iLayotRackService,
            LayotRackUserControlViewModel layotRackViewModel)
        {
            UserDialog = userDialog;
            Log = logSerivece;
            _LayotRackService = iLayotRackService;
            LayotRackViewModel = layotRackViewModel;
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Таблица сигналов";
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
        private string _Description = "Таблица сигналов НПС-1 \"Сызрань\"";
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

        #region Список модулей
        private List<RackModule> _Modules = new();
        /// <summary>
        /// Список модулей
        /// </summary>
        public List<RackModule> Modules
        {
            get => _Modules;
            set => Set(ref _Modules, value);
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

        #endregion

        #region Команды

        #region Команда - Сгенерировать таблицу
        /// <summary>
        /// Команда - Сгенерировать таблицу
        /// </summary>
        private ICommand _CmdGenerateTable;
        public ICommand CmdGenerateTable => _CmdGenerateTable ??= new RelayCommand(OnCmdGenerateTableExecuted);
        private void OnCmdGenerateTableExecuted()
        {
            //if (LayotRackViewModel is null) return;
            //if (LayotRackViewModel.Racks is null) return;

            //if (MessageBox.Show("Все данные по сигналам будут потеряны!\nПродолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            //    != MessageBoxResult.Yes) return;


            //var modules = new List<RackModule>();
            //foreach (var _Racks in LayotRackViewModel.Racks)
            //{
            //    foreach (var _Module in _Racks.Modules)
            //    {
            //        switch (_Module.Type)
            //        {
            //            case TypeModule.AI:
            //            case TypeModule.DI:
            //            case TypeModule.AO:
            //            case TypeModule.DO:
            //                modules.Add(_Module);
            //                break;

            //        }
            //    }
            //}
            //Modules = new(modules);

        }
        #endregion

        //#region Команда - Выбрать сигнал
        ///// <summary>
        ///// Команда - Выбрать сигнал
        ///// </summary>
        //private ICommand _CmdSelectionSignal;
        //public ICommand CmdSelectionSignal => _CmdSelectionSignal ??= new RelayCommand(OnCmdSelectionSignalExecuted, CanCmdSelectionSignalExecute);
        //private bool CanCmdSelectionSignalExecute(object p) => _SignalService.DoSelection;
        //private void OnCmdSelectionSignalExecuted(object p)
        //{
        //    var index = (int)p;
        //    var message = "";
        //    switch (_SignalService.Type)
        //    {
        //        case Models.Enum.TypeModule.AI:
        //            if (index >= 100000)
        //            {
        //                message =
        //                    "Выбор неверный!\n" +
        //                    "Вы выбрали не аналоговый входной сигнал.\n" +
        //                    "Запрашивается ссылка на аналоговый входной сигнал (0-99999)" +
        //                    "Повторить выбор?";
        //            }
        //            break;
        //        case Models.Enum.TypeModule.DI:
        //            if (index < 100000 || index >= 200000)
        //            {
        //                message =
        //                    "Выбор неверный!\n" +
        //                    "Вы выбрали не дискретный входной сигнал.\n" +
        //                    "Запрашивается ссылка на дискретный входной сигнал (100000-199999)" +
        //                    "Повторить выбор?";
        //            }
        //            break;
        //        case Models.Enum.TypeModule.AO:
        //            if (index < 300000 || index >= 400000)
        //            {
        //                message =
        //                    "Выбор неверный!\n" +
        //                    "Вы выбрали не аналоговый выходной сигнал.\n" +
        //                    "Запрашивается ссылка на аналоговый выходной сигнал (300000-399999)" +
        //                    "Повторить выбор?";
        //            }
        //            break;
        //        case Models.Enum.TypeModule.DO:
        //            if (index < 200000 || index >= 300000)
        //            {
        //                message =
        //                    "Выбор неверный!\n" +
        //                    "Вы выбрали не дискретный выходной сигнал.\n" +
        //                    "Запрашивается ссылка на аналоговый входной сигнал (200000-299999)" +
        //                    "Повторить выбор?";
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //    if (!string.IsNullOrWhiteSpace(message))
        //    {
        //        if (UserDialog.SendMessage("Выбор сигнала", message, MessageBoxButton.YesNo, ResultType: MessageBoxResult.Yes))
        //            return;
        //        else
        //        {
        //            _SignalService.ResetSignal();
        //            DoSelection = _SignalService.DoSelection;
        //        }

        //    }
        //    else
        //    {
        //        foreach (var _Module in Modules)
        //        {
        //            foreach (var _Signal in _Module.Signals)
        //            {
        //                if (_Signal.Index == index)
        //                {
        //                    _SignalService.Id = _Signal.Id;
        //                    _SignalService.Description = _Signal.Description;
        //                    break;
        //                }

        //            }
        //        }

        //        var Content = Application.Current.MainWindow.Content;
        //        var _Grid = Content as Grid;

        //        foreach (var Children in _Grid.Children)
        //        {
        //            if (Children is TabControl)
        //            {
        //                var _TabControl = Children as TabControl;
        //                foreach (var item in _TabControl.Items)
        //                {
        //                    var TabItem = item as TabItem;
        //                    if (TabItem.Header.ToString() == _SignalService.ListName)
        //                    {
        //                        switch (_SignalService.Type)
        //                        {
        //                            case Models.Enum.TypeModule.AI:
        //                                _SignalService.Address = $"{index}";
        //                                break;
        //                            case Models.Enum.TypeModule.DI:
        //                                _SignalService.Address = $"{index - 100000}";
        //                                break;
        //                            case Models.Enum.TypeModule.AO:
        //                                _SignalService.Address = $"{index - 300000}";
        //                                break;
        //                            case Models.Enum.TypeModule.DO:
        //                                _SignalService.Address = $"{index - 200000}";
        //                                break;
        //                            default:
        //                                break;
        //                        }

        //                        _TabControl.SelectedItem = TabItem;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion

        #endregion
    }
}
