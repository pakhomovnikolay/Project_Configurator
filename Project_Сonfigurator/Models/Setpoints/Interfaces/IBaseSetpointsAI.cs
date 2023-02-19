namespace Project_Сonfigurator.Models.Setpoints.Interfaces
{
    public interface IBaseSetpointsAI
    {
        #region Описание вкладки, при необходиомсти группировки уставок
        /// <summary>
        /// Описание вкладки, при необходиомсти группировки уставок
        /// </summary>
        string DescriptionTab { get; set; }
        #endregion

        #region Технологический максимум
        /// <summary>
        /// Технологический максимум
        /// </summary>
        string TMax { get; set; }
        #endregion

        #region Технологический минимум
        /// <summary>
        /// Технологический минимум
        /// </summary>
        string TMin { get; set; }
        #endregion

        #region Технологический максимум. Порог 1
        /// <summary>
        /// Технологический максимум. Порог 1
        /// </summary>
        string TMax_1 { get; set; }
        #endregion

        #region Технологический максимум. Порог 2
        /// <summary>
        /// Технологический максимум. Порог 2
        /// </summary>
        string TMax_2 { get; set; }
        #endregion

        #region Технологический максимум. Порог 3
        /// <summary>
        /// Технологический максимум. Порог 3
        /// </summary>
        string TMax_3 { get; set; }
        #endregion

        #region Технологический максимум. Порог 4
        /// <summary>
        /// Технологический максимум. Порог 4
        /// </summary>
        string TMax_4 { get; set; }
        #endregion

        #region Технологический максимум. Порог 5
        /// <summary>
        /// Технологический максимум. Порог 5
        /// </summary>
        string TMax_5 { get; set; }
        #endregion

        #region Технологический максимум. Порог 6
        /// <summary>
        /// Технологический максимум. Порог 6
        /// </summary>
        string TMax_6 { get; set; }
        #endregion

        #region Технологический минимум. Порог 1
        /// <summary>
        /// Технологический минимум. Порог 1
        /// </summary>
        string TMin_1 { get; set; }
        #endregion

        #region Технологический минимум. Порог 2
        /// <summary>
        /// Технологический минимум. Порог 2
        /// </summary>
        string TMin_2 { get; set; }
        #endregion

        #region Технологический минимум. Порог 3
        /// <summary>
        /// Технологический минимум. Порог 3
        /// </summary>
        string TMin_3 { get; set; }
        #endregion

        #region Технологический минимум. Порог 4
        /// <summary>
        /// Технологический минимум. Порог 4
        /// </summary>
        string TMin_4 { get; set; }
        #endregion

        #region Технологический минимум. Порог 5
        /// <summary>
        /// Технологический минимум. Порог 5
        /// </summary>
        string TMin_5 { get; set; }
        #endregion

        #region Технологический минимум. Порог 6
        /// <summary>
        /// Технологический минимум. Порог 6
        /// </summary>
        string TMin_6 { get; set; }
        #endregion

        #region Коэффициент сглаживания
        /// <summary>
        /// Коэффициент сглаживания
        /// </summary>
        string KS { get; set; }
        #endregion

        #region Зона нечувствительности
        /// <summary>
        /// Зона нечувствительности
        /// </summary>
        string Delta { get; set; }
        #endregion

        #region Гистерезис
        /// <summary>
        /// Гистерезис
        /// </summary>
        string Hyst { get; set; }
        #endregion

        #region Гистерезис нижнего предела достоверности
        /// <summary>
        /// Гистерезис нижнего предела достоверности
        /// </summary>
        string HystNPD { get; set; }
        #endregion

        #region Гистерезис верхнего предела достоверности
        /// <summary>
        /// Гистерезис верхнего предела достоверности
        /// </summary>
        string HystVPD { get; set; }
        #endregion

        #region Задержка после выхода сигнала из недостоверности
        /// <summary>
        /// Задержка после выхода сигнала из недостоверности
        /// </summary>
        string PDDelay { get; set; }
        #endregion

        #region Инжененрный максимум
        /// <summary>
        /// Инжененрный максимум
        /// </summary>
        string AMax { get; set; }
        #endregion

        #region Инжененрный минимум
        /// <summary>
        /// Инжененрный минимум
        /// </summary>
        string AMin { get; set; }
        #endregion

        #region Скорость изменения сигнала
        /// <summary>
        /// Скорость изменения сигнала
        /// </summary>
        string Speed { get; set; }
        #endregion

        #region Флаг записи сигнала в тренды
        /// <summary>
        /// Флаг записи сигнала в тренды
        /// </summary>
        string Trending { get; set; }
        #endregion

        #region Количество знаков после запятой
        /// <summary>
        /// Количество знаков после запятой
        /// </summary>
        string QtySimbolsComma { get; set; }
        #endregion
    }
}
