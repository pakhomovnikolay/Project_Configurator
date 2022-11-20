using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.UserControls
{
    public class LayotRackUserControlViewModel : ViewModel
    {
        #region Конструктор
        private readonly IUserDialogService UserDialog;
        private readonly ILogSerivece Log;
        ILayotRackService _LayotRackService;

        public LayotRackUserControlViewModel(IUserDialogService userDialog, ILogSerivece logSerivece, ILayotRackService iLayotRackService)
        {
            UserDialog = userDialog;
            Log = logSerivece;
            _LayotRackService = iLayotRackService;


            while (Racks.Count < 53)
            {
                var modules = new List<RackModule>();
                while (modules.Count < 26)
                {
                    var module = new RackModule()
                    {
                        Index = $"A{Racks.Count + 1}.{modules.Count + 1}",
                        Name = "",
                        StartAddress = "",
                        EndAddress = "",
                        Type = TypeModule.Unknown,
                        ColorUSO = "",
                        NameUSO = "",
                        Channels = new()
                    };
                    modules.Add(module);
                }

                var rack = new Rack()
                {
                    Index = Racks.Count + 1,
                    NameUSO = "",
                    Name = $"A{Racks.Count + 1}",
                    IndexUSO = 1,
                    Modules = modules
                };

                Racks.Add(rack);
            }

            _LayotRackService.RefreshRack(Racks);
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Компоновка корзин";
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
        private string _Description = "Компоновка корзин НПС-1 \"Сызрань\"";
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

        #region Список корзин
        private List<Rack> _Racks = new();
        /// <summary>
        /// Список корзин
        /// </summary>
        public List<Rack> Racks
        {
            get => _Racks;
            set => Set(ref _Racks, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - обновить индексы модулей
        private ICommand _CmdRefreshIndexModule;
        /// <summary>
        /// Команда - обновить индексы модулей
        /// </summary>
        public ICommand CmdRefreshIndexModule => _CmdRefreshIndexModule ??= new RelayCommand(OnCmdRefreshIndexModuleExecuted);

        private void OnCmdRefreshIndexModuleExecuted()
        {
            _LayotRackService.RefreshIndexModule(Racks);
        }
        #endregion

        #region Команда - обновить адреса модулей
        private ICommand _CmdRefreshAddressModule;
        /// <summary>
        /// Команда - обновить адреса модулей
        /// </summary>
        public ICommand CmdRefreshAddressModule => _CmdRefreshAddressModule ??= new RelayCommand(OnCmdRefreshAddressModuleExecuted, CanCmdRefreshAddressModuleExecute);
        private bool CanCmdRefreshAddressModuleExecute(object p) => true;
        private void OnCmdRefreshAddressModuleExecuted(object p)
        {
            _LayotRackService.RefreshAddressModule(Racks);
        }
        #endregion

        #endregion
    }
}
