using Project_Сonfigurator.Models.Interfaces;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Signals;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models
{
    public class ProjectDataToSave : IProjectDataToSave
    {
        #region Компоновка корзин
        /// <summary>
        /// Компоновка корзин
        /// </summary>
        public ObservableCollection<USO> LayotRack { get; set; }
        #endregion

        #region Таблица сигналов
        /// <summary>
        /// Таблица сигналов
        /// </summary>
        public ObservableCollection<USO> TableSignals { get; set; }
        #endregion

        #region DI формируемые
        /// <summary>
        /// DI формируемые
        /// </summary>
        public ObservableCollection<BaseSignal> UserDI { get; set; } = new();
        #endregion

        #region AI формируемые
        /// <summary>
        /// AI формируемые
        /// </summary>
        public ObservableCollection<BaseSignal> UserAI { get; set; } = new();
        #endregion

        #region Сигналы DI
        /// <summary>
        /// Сигналы DI
        /// </summary>
        public ObservableCollection<SignalDI> SignalDI { get; set; } = new();
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Сигналы AI
        /// </summary>
        public ObservableCollection<SignalAI> SignalAI { get; set; } = new();
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Сигналы DO
        /// </summary>
        public ObservableCollection<SignalDO> SignalDO { get; set; } = new();
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Сигналы AO
        /// </summary>
        public ObservableCollection<SignalAO> SignalAO { get; set; } = new();
        #endregion

        #region Секции шин
        /// <summary>
        /// Секции шин
        /// </summary>
        public ObservableCollection<BaseParam> ECParam { get; set; } = new();
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Регистры формируемые
        /// </summary>
        public ObservableCollection<BaseParam> UserReg { get; set; } = new();
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Сигналы групп
        /// </summary>
        public ObservableCollection<BaseParam> SignalGroup { get; set; } = new();
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Группы сигналов
        /// </summary>
        public ObservableCollection<GroupSignal> GroupSignals { get; set; } = new();
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        public ObservableCollection<BaseUZD> UZD { get; set; } = new();
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        public ObservableCollection<BaseUVS> UVS { get; set; } = new();
        #endregion

        #region Настройки МПНА
        /// <summary>
        /// Настройки МПНА
        /// </summary>
        public ObservableCollection<BaseUMPNA> UMPNA { get; set; } = new();
        #endregion

        #region Общестанционные защиты
        /// <summary>
        /// Общестанционные защиты
        /// </summary>
        public ObservableCollection<BaseKTPR> KTPR { get; set; } = new();
        #endregion

        #region Предельные параметры общестанционных защит
        /// <summary>
        /// Предельные параметры общестанционных защит
        /// </summary>
        public ObservableCollection<BaseKTPRS> KTPRS { get; set; } = new();
        #endregion

        #region Сигнализация и общая диагностики
        /// <summary>
        /// Сигнализация и общая диагностики
        /// </summary>
        public ObservableCollection<BaseSignaling> Signaling { get; set; } = new();
        #endregion

        #region Табло и сирены
        /// <summary>
        /// Табло и сирены
        /// </summary>
        public ObservableCollection<BaseUTS> UTS { get; set; } = new();
        #endregion

        #region Уставки Real
        /// <summary>
        /// Уставки Real
        /// </summary>
        public ObservableCollection<BaseSetpointsReal> SetpointsReal { get; set; } = new();
        #endregion

        #region Уставки общие
        /// <summary>
        /// Уставки общие
        /// </summary>
        public ObservableCollection<BaseSetpoints> SetpointsCommon { get; set; } = new();
        #endregion

        #region Карта ручного ввода
        /// <summary>
        /// Карта ручного ввода
        /// </summary>
        public ObservableCollection<BaseParam> HandMap { get; set; } = new();
        #endregion

        #region Сообщения
        /// <summary>
        /// Сообщения
        /// </summary>
        public ObservableCollection<CollectionMessage> Messages { get; set; } = new();
        #endregion

        #region Команды
        /// <summary>
        /// Команды
        /// </summary>
        public ObservableCollection<BaseParam> Commands { get; set; }
        #endregion
    }
}