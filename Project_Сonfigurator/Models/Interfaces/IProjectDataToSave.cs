using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Signals;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Interfaces
{
    public interface IProjectDataToSave
    {
        #region Компоновка корзин
        /// <summary>
        /// Компоновка корзин
        /// </summary>
        ObservableCollection<USO> LayotRack { get; set; }
        #endregion

        #region Таблица сигналов
        /// <summary>
        /// Таблица сигналов
        /// </summary>
        ObservableCollection<USO> TableSignals { get; set; }
        #endregion

        #region DI формируемые
        /// <summary>
        /// DI формируемые
        /// </summary>
        ObservableCollection<BaseSignal> UserDI { get; set; }
        #endregion

        #region AI формируемые
        /// <summary>
        /// AI формируемые
        /// </summary>
        ObservableCollection<BaseSignal> UserAI { get; set; }
        #endregion

        #region Сигналы DI
        /// <summary>
        /// Сигналы DI
        /// </summary>
        ObservableCollection<SignalDI> SignalDI { get; set; }
        #endregion

        #region Сигналы AI
        /// <summary>
        /// Сигналы AI
        /// </summary>
        ObservableCollection<SignalAI> SignalAI { get; set; }
        #endregion

        #region Сигналы DO
        /// <summary>
        /// Сигналы DO
        /// </summary>
        ObservableCollection<SignalDO> SignalDO { get; set; }
        #endregion

        #region Сигналы AO
        /// <summary>
        /// Сигналы AO
        /// </summary>
        ObservableCollection<SignalAO> SignalAO { get; set; }
        #endregion

        #region Секции шин
        /// <summary>
        /// Секции шин
        /// </summary>
        ObservableCollection<BaseParam> ECParam { get; set; }
        #endregion

        #region Регистры формируемые
        /// <summary>
        /// Регистры формируемые
        /// </summary>
        ObservableCollection<BaseParam> UserReg { get; set; }
        #endregion

        #region Сигналы групп
        /// <summary>
        /// Сигналы групп
        /// </summary>
        ObservableCollection<BaseParam> SignalGroup { get; set; }
        #endregion

        #region Группы сигналов
        /// <summary>
        /// Группы сигналов
        /// </summary>
        ObservableCollection<GroupSignal> GroupSignals { get; set; }
        #endregion

        #region Настройки задвижек
        /// <summary>
        /// Настройки задвижек
        /// </summary>
        ObservableCollection<BaseUZD> UZD { get; set; }
        #endregion

        #region Настройки вспомсистем
        /// <summary>
        /// Настройки вспомсистем
        /// </summary>
        ObservableCollection<BaseUVS> UVS { get; set; }
        #endregion

        #region Настройки МПНА
        /// <summary>
        /// Настройки МПНА
        /// </summary>
        ObservableCollection<BaseUMPNA> UMPNA { get; set; }
        #endregion

        #region Общестанционные защиты
        /// <summary>
        /// Общестанционные защиты
        /// </summary>
        ObservableCollection<BaseKTPR> KTPR { get; set; }
        #endregion

        #region Предельные параметры общестанционных защит
        /// <summary>
        /// Предельные параметры общестанционных защит
        /// </summary>
        ObservableCollection<BaseKTPRS> KTPRS { get; set; }
        #endregion

        #region Сигнализация и общая диагностики
        /// <summary>
        /// Сигнализация и общая диагностики
        /// </summary>
        ObservableCollection<BaseSignaling> Signaling { get; set; }
        #endregion

        #region Табло и сирены
        /// <summary>
        /// Табло и сирены
        /// </summary>
        ObservableCollection<BaseUTS> UTS { get; set; }
        #endregion

        #region Уставки Real
        /// <summary>
        /// Уставки Real
        /// </summary>
        ObservableCollection<BaseSetpointsReal> SetpointsReal { get; set; }
        #endregion

        #region Уставки общие
        /// <summary>
        /// Уставки общие
        /// </summary>
        ObservableCollection<BaseSetpoints> SetpointsCommon { get; set; }
        #endregion

        #region Карта ручного ввода
        /// <summary>
        /// Карта ручного ввода
        /// </summary>
        ObservableCollection<BaseParam> HandMap { get; set; }
        #endregion
    }
}