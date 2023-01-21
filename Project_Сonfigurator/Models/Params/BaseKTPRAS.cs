using Project_Сonfigurator.Models.Params.Interfaces;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseKTPRAS : IBaseKTPRAS
    {
        #region Параметры защит
        /// <summary>
        /// Параметры защит
        /// </summary>
        public BaseParam Param { get; set; } = new();
        #endregion

        #region Состояние НА
        /// <summary>
        /// Состояние НА
        /// </summary>
        public string StateUMPNA { get; set; }
        #endregion

        #region Настройка сигнализации
        /// <summary>
        /// Настройка сигнализации
        /// </summary>
        public string TypeWarning { get; set; }
        #endregion

        #region Тип параметра
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string Type { get; set; }
        #endregion
    }
}