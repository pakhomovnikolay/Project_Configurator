using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Settings.Interfaces;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingDefualtDefenseMap : ISettingDefualtDefenseMap
    {
        #region Данные параметра
        /// <summary>
        /// Данные параметра
        /// </summary>
        public BaseParam Param { get; set; }
        #endregion

        #region Данные уставок
        /// <summary>
        /// Данные уставок
        /// </summary>
        public BaseSetpoints Setpoints { get; set; }
        #endregion
    }
}
