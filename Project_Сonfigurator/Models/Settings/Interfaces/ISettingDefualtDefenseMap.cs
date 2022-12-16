using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;

namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingDefualtDefenseMap
    {
        #region Данные параметра
        /// <summary>
        /// Данные параметра
        /// </summary>
        BaseParam Param { get; set; }
        #endregion

        #region Данные уставок
        /// <summary>
        /// Данные уставок
        /// </summary>
        BaseSetpoints Setpoints { get; set; }
        #endregion
    }
}
