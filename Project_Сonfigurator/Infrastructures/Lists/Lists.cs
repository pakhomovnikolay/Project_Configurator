using Project_Сonfigurator.Models;
using System.Collections.Generic;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Lists
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
            if (Program.Settings is not null &&
                    Program.Settings.Config is not null &&
                    Program.Settings.Config.Vendors is not null &&
                    Program.Settings.Config.Vendors.Count > 0)
            {
                ResultModuleList = new();
                foreach (var Vendor in Program.Settings.Config.Vendors)
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
                _UMPNAList = new() { "0 - Не вибрация" };
                if (Program._DBService is null) return _UMPNAList;
                if (Program._DBService.AppData is null) return _UMPNAList;
                if (Program._DBService.AppData.UMPNA is null) return _UMPNAList;

                foreach (var _UMPNA in Program._DBService.AppData.UMPNA)
                    _UMPNAList.Add($"{_UMPNA.Index} - {_UMPNA.Description}");

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
                _USOList = new() { "0 - Не служебный" };
                if (Program._DBService is null) return _USOList;
                if (Program._DBService.AppData is null) return _USOList;
                if (Program._DBService.AppData.USOList is null) return _USOList;

                foreach (var _USO in Program._DBService.AppData.USOList)
                    _USOList.Add($"{_USO.Index} - {_USO.Name}");

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
    }
}
