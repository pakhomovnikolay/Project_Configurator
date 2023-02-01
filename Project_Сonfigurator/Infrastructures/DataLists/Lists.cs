using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.ToolTips;
using Project_Сonfigurator.ViewModels;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.DataLists
{
    public class Lists : Freezable
    {
        protected override Freezable CreateInstanceCore() => new Lists();

        #region Конструктор
        public Lists()
        {
            StateStation = new()
            {
                "0 - Всегда",
                "1 - В работе",
                "2 - Остановленна",
            };

            StateNA = new()
            {
                "0 - Всегда",
                "1 - Остановлен",
                "3 - Пусковой режим ЭД",
                "4 - Пусковой режим насоса",
                "5 - Не пусковой режим ЭД",
                "6 - Не пусковой режим насоса",
                "7 - Не пусковой режим вне раб.",
                "8 - Идет плавный пуск"
            };

            #region Формируем список модулей
            if (App.Settings is not null &&
                    App.Settings.Config is not null &&
                    App.Settings.Config.Vendors is not null &&
                    App.Settings.Config.Vendors.Count > 0)
            {
                ResultModuleList = new();
                foreach (var Vendor in App.Settings.Config.Vendors)
                {
                    if (Vendor.IsSelected)
                    {
                        ResultModuleList.Add($"");
                        foreach (var ModuleType in Vendor.ModuleTypes)
                        {
                            ResultModuleList.Add($"- - - - - - - {ModuleType.Name} - - - - - - -");
                            foreach (var Module in ModuleType.Modules)
                            {
                                ResultModuleList.Add($"{Module.Name}");
                            }
                        }
                    }
                }
            }
            else
            {
                ResultModuleList = new();
            }
            #endregion

        }
        #endregion

        #region Режимы работы станции для обшестанционных защит
        /// <summary>
        /// Режимы работы станции для обшестанционных защит
        /// </summary>
        public List<string> StateStation { get; set; }
        #endregion

        #region Режимы работы агрегата для агрегатных защит
        /// <summary>
        /// Режимы работы агрегата для агрегатных защит
        /// </summary>
        public List<string> StateNA { get; set; }
        #endregion

        #region Результирующий список модулей
        /// <summary>
        /// Результирующий список модулей
        /// </summary>
        public List<string> ResultModuleList { get; set; }
        #endregion

        #region Список МПНА
        private List<string> _UMPNAList = new();
        /// <summary>
        /// Список МПНА
        /// </summary>
        public List<string> UMPNAList
        {
            get
            {
                _UMPNAList = new();
                UMPNAUserControlViewModel _ViewModel = new();
                IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
                foreach (var _TabItem in from object _Item in _ViewModels
                                         let _TabItem = _Item as UMPNAUserControlViewModel
                                         where _TabItem is UMPNAUserControlViewModel
                                         select _TabItem)
                    _ViewModel = _TabItem;

                if (_ViewModel.Params is null || _ViewModel.Params.Count <= 0)
                {
                    _UMPNAList.Add($" Сначала заполните список агрегатов ");
                    return _UMPNAList;
                }

                _UMPNAList.Add($" 0 - Не вибрация ");
                foreach (var _Params in _ViewModel.Params)
                    _UMPNAList.Add($" {_Params.Index} - {_Params.Description} ");

                return _UMPNAList;
            }
            set => _UMPNAList = value;
        }
        #endregion

        #region Список УСО
        private List<string> _USOList = new();
        /// <summary>
        /// Список УСО
        /// </summary>
        public List<string> USOList
        {
            get
            {
                _USOList = new();
                LayotRackUserControlViewModel _ViewModel = new();
                IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
                foreach (var _TabItem in from object _Item in _ViewModels
                                         let _TabItem = _Item as LayotRackUserControlViewModel
                                         where _TabItem is LayotRackUserControlViewModel
                                         select _TabItem)
                    _ViewModel = _TabItem;

                if (_ViewModel.Params is null || _ViewModel.Params.Count <= 0)
                {
                    _USOList.Add($" Сначала заполните компоновку корзин ");
                    return _USOList;
                }

                foreach (var _Params in _ViewModel.Params)
                    _USOList.Add($" {_Params.Index} - {_Params.Name} ");

                return _USOList;
            }
            set => _USOList = value;
        }
        #endregion

        #region Список цветов
        private List<string> _ColorList = new();
        /// <summary>
        /// Список цветов
        /// </summary>
        public List<string> ColorList
        {
            get
            {
                _ColorList = new List<string>()
                {
                    "",
                    "Красный",
                    "Желтый",
                    "Зеленый"
                };

                return _ColorList;
            }
            set => _ColorList = value;
        }


        #endregion

        #region Список наименований листов проекта
        private List<string> _TabList = new();
        /// <summary>
        /// Список наименований листов проекта
        /// </summary>
        public List<string> TabList
        {
            get
            {
                IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
                _TabList = new List<string> { "" };
                foreach (var _ViewModel in _ViewModels)
                    _TabList.Add(_ViewModel.Title);

                return _TabList;
            }
        }
        #endregion

        #region Список листов проекта
        /// <summary>
        /// Список листов проекта
        /// </summary>
        public static IEnumerable<IViewModelUserControls> ViewModelUserControls
        {
            get => App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
        }
        #endregion

        #region Выбранный лист
        /// <summary>
        /// Выбранный лист
        /// </summary>
        public static IViewModelUserControls SelectedViewModel
        {
            get => App.Services.GetRequiredService<MainWindowViewModel>().SelectedViewModel;
            set => App.Services.GetRequiredService<MainWindowViewModel>().SelectedViewModel = value;
        }
        #endregion
    }
}
