using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.Views.UserControls.Params;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls.Params
{
    public class MessagesUserControlViewModel : ViewModelUserControl
    {
        #region Конструктор
        public MessagesUserControlViewModel()
        {
            Title = "Сообщения";
            Description = "Таблица систем сообщений";
            UsingUserControl = new MessagesUserControl();
        }

        private readonly ISignalService SignalServices;
        public MessagesUserControlViewModel(ISignalService _ISignalService) : this()
        {
            SignalServices = _ISignalService;
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
                if (Set(ref _IsSelected, value, DontСommitСhanges: true))
                    if (_IsSelected) SignalServices.ResetSignal();
            }
        }
        #endregion

        #region Список парметров
        private ObservableCollection<BaseSystemMessage> _Params = new();
        /// <summary>
        /// Список парметров
        /// </summary>
        public ObservableCollection<BaseSystemMessage> Params
        {
            get => _Params;
            set
            {
                if (Set(ref _Params, value))
                {
                    if (_Params is null || _Params.Count <= 0)
                        CreateData();
                }
            }
        }
        #endregion

        #region Выбранный параметр
        private BaseSystemMessage _SelectedParam = new();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public BaseSystemMessage SelectedParam
        {
            get => _SelectedParam;
            set => Set(ref _SelectedParam, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Выбрать лист пренадлежности
        private ICommand _CmdSelecteTabList;
        /// <summary>
        /// Команда - Выбрать лист пренадлежности
        /// </summary>
        public ICommand CmdSelecteTabList => _CmdSelecteTabList ??= new RelayCommand(OnCmdSelecteTabListExecuted, CanCmdSelecteTabListExecute);
        private bool CanCmdSelecteTabListExecute(object p) => p is DataGrid;

        private void OnCmdSelecteTabListExecuted(object p)
        {
            if (p is not DataGrid _DataGrid) return;
            _DataGrid.BeginEdit();
        }
        #endregion

        #endregion

        #region Функции

        #region Получение параметров
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns></returns>
        public override ObservableCollection<T> GetParams<T>() => Params as ObservableCollection<T>;
        #endregion

        #region Запись параметров
        /// <summary>
        /// Запись параметров
        /// </summary>
        /// <returns></returns>
        public override void SetParams<T>(ObservableCollection<T> _Params) => Params = _Params as ObservableCollection<BaseSystemMessage>;
        #endregion

        #region Формирование данных при создании нового проекта
        private void CreateData()
        {
            #region Создаем данные параметра
            while (Params.Count < 127)
            {
                Params.Add(new BaseSystemMessage
                {
                    Index = $"{Params.Count + 1}"
                });
            }
            #endregion
            if (Params.Count > 0)
                SelectedParam = Params[0];
        }
        #endregion

        #endregion
    }
}
