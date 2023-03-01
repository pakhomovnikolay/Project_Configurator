using Project_Сonfigurator.Models.Setpoints.Interfaces;

namespace Project_Сonfigurator.Models.Setpoints
{
    public class BaseSetpointsAI : IBaseSetpointsAI
    {
        #region Описание вкладки, при необходиомсти группировки уставок
        /// <summary>
        /// Описание вкладки, при необходиомсти группировки уставок
        /// </summary>
        public string DescriptionTab { get; set; }
        #endregion

        #region Технологический максимум
        /// <summary>
        /// Технологический максимум
        /// </summary>
        public string TMax { get; set; }
        #endregion

        #region Технологический минимум
        /// <summary>
        /// Технологический минимум
        /// </summary>
        public string TMin { get; set; }
        #endregion

        #region Технологический максимум. Порог 1
        /// <summary>
        /// Технологический максимум. Порог 1
        /// </summary>
        public string TMax_1 { get; set; }
        #endregion

        #region Технологический максимум. Порог 2
        /// <summary>
        /// Технологический максимум. Порог 2
        /// </summary>
        public string TMax_2 { get; set; }
        #endregion

        #region Технологический максимум. Порог 3
        /// <summary>
        /// Технологический максимум. Порог 3
        /// </summary>
        public string TMax_3 { get; set; }
        #endregion

        #region Технологический максимум. Порог 4
        /// <summary>
        /// Технологический максимум. Порог 4
        /// </summary>
        public string TMax_4 { get; set; }
        #endregion

        #region Технологический максимум. Порог 5
        /// <summary>
        /// Технологический максимум. Порог 5
        /// </summary>
        public string TMax_5 { get; set; }
        #endregion

        #region Технологический максимум. Порог 6
        /// <summary>
        /// Технологический максимум. Порог 6
        /// </summary>
        public string TMax_6 { get; set; }
        #endregion

        #region Технологический минимум. Порог 1
        /// <summary>
        /// Технологический минимум. Порог 1
        /// </summary>
        public string TMin_1 { get; set; }
        #endregion

        #region Технологический минимум. Порог 2
        /// <summary>
        /// Технологический минимум. Порог 2
        /// </summary>
        public string TMin_2 { get; set; }
        #endregion

        #region Технологический минимум. Порог 3
        /// <summary>
        /// Технологический минимум. Порог 3
        /// </summary>
        public string TMin_3 { get; set; }
        #endregion

        #region Технологический минимум. Порог 4
        /// <summary>
        /// Технологический минимум. Порог 4
        /// </summary>
        public string TMin_4 { get; set; }
        #endregion

        #region Технологический минимум. Порог 5
        /// <summary>
        /// Технологический минимум. Порог 5
        /// </summary>
        public string TMin_5 { get; set; }
        #endregion

        #region Технологический минимум. Порог 6
        /// <summary>
        /// Технологический минимум. Порог 6
        /// </summary>
        public string TMin_6 { get; set; }
        #endregion

        #region Коэффициент сглаживания
        /// <summary>
        /// Коэффициент сглаживания
        /// </summary>
        public string KS { get; set; }
        #endregion

        #region Зона нечувствительности
        /// <summary>
        /// Зона нечувствительности
        /// </summary>
        public string Delta { get; set; }
        #endregion

        #region Гистерезис
        /// <summary>
        /// Гистерезис
        /// </summary>
        public string Hyst { get; set; }
        #endregion

        #region Гистерезис нижнего предела достоверности
        /// <summary>
        /// Гистерезис нижнего предела достоверности
        /// </summary>
        public string HystNPD { get; set; }
        #endregion

        #region Гистерезис верхнего предела достоверности
        /// <summary>
        /// Гистерезис верхнего предела достоверности
        /// </summary>
        public string HystVPD { get; set; }
        #endregion

        #region Задержка после выхода сигнала из недостоверности
        /// <summary>
        /// Задержка после выхода сигнала из недостоверности
        /// </summary>
        public string PDDelay { get; set; }
        #endregion

        #region Инжененрный максимум
        /// <summary>
        /// Инжененрный максимум
        /// </summary>
        public string AMax { get; set; }
        #endregion

        #region Инжененрный минимум
        /// <summary>
        /// Инжененрный минимум
        /// </summary>
        public string AMin { get; set; }
        #endregion

        #region Скорость изменения сигнала
        /// <summary>
        /// Скорость изменения сигнала
        /// </summary>
        public string Speed { get; set; }
        #endregion

        #region Флаг записи сигнала в тренды
        /// <summary>
        /// Флаг записи сигнала в тренды
        /// </summary>
        public string Trending { get; set; }
        #endregion

        #region Количество знаков после запятой
        /// <summary>
        /// Количество знаков после запятой
        /// </summary>
        public string QtySimbolsComma { get; set; }
        #endregion

        #region Ед. измерения
        /// <summary>
        /// Ед. измерения
        /// </summary>
        public string Unit { get; set; }
        #endregion
    }
}
