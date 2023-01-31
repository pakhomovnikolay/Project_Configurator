using System.Windows;

namespace Project_Сonfigurator.Infrastructures.ToolTips
{
    public class ToolTipParam : Freezable
    {
        protected override Freezable CreateInstanceCore() => new ToolTipParam();

        #region Всплывающая подсказка "Тип сигнала"
        /// <summary>
        /// Всплывающая подсказка "Тип сигнала"
        /// </summary>
        public string TypeSignal
        {
            get => (string)GetValue(TypeSignalProperty);
            set => SetValue(TypeSignalProperty, value);
        }

        public static readonly DependencyProperty TypeSignalProperty = DependencyProperty.Register(
            nameof(TypeSignal),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Задайте тип сигнала на который ссылаетесь\n" +
                "0  - DI аппаратные (\"Сигналы DI\")\n" +
                "1  - Результат обраб. группы сигналов сложеных по И(ИЛИ) (\"Группы сигналов\")\n" +
                "2  - Результат = НПД (\"Аналоговые сигналы\")\n" +
                "3  - Результат = мин. 6 (\"Аналоговые сигналы\")\n" +
                "4  - Результат ≤ мин. 5 (\"Аналоговые сигналы\")\n" +
                "5  - Результат ≤ мин. 4 (\"Аналоговые сигналы\")\n" +
                "6  - Результат ≤ мин. 3 (\"Аналоговые сигналы\")\n" +
                "7  - Результат ≤ мин. 2 (\"Аналоговые сигналы\")\n" +
                "8  - Результат ≤ мин. 1 (\"Аналоговые сигналы\")\n" +
                "9  - Результат = норма (\"Аналоговые сигналы\")\n" +
                "10 - Результат ≥ макс. 1 (\"Аналоговые сигналы\")\n" +
                "11 - Результат ≥ макс. 2 (\"Аналоговые сигналы\")\n" +
                "12 - Результат ≥ макс. 3 (\"Аналоговые сигналы\")\n" +
                "13 - Результат ≥ макс. 4 (\"Аналоговые сигналы\")\n" +
                "14 - Результат ≥ макс. 5 (\"Аналоговые сигналы\")\n" +
                "15 - Результат ≥ макс. 6 (\"Аналоговые сигналы\")\n" +
                "16 - Результат ≥ макс. 7 (\"Аналоговые сигналы\")\n" +
                "17 - Результат ≥ макс. 8 (\"Аналоговые сигналы\")\n" +
                "18 - Результат = макс. 9 (\"Аналоговые сигналы\")\n" +
                "19 - Результат = ВПД (\"Аналоговые сигналы\")\n" +
                "20 - Результат = Общая недостоверность (\"Аналоговые сигналы\")\n" +
                "31 - Результат ≥ макс.1 или ≤ мин.1 (\"Аналоговые сигналы\")\n" +
                "32 - Результат ≥ макс.2 или ≤ мин.2 (\"Аналоговые сигналы\")\n" +
                "33 - Результат ≥ макс.3 или ≤ мин.3 (\"Аналоговые сигналы\")\n" +
                "34 - Результат ≥ макс.4 или ≤ мин.4 (\"Аналоговые сигналы\")\n" +
                "35 - Результат ≥ макс.5 или ≤ мин.5 (\"Аналоговые сигналы\")\n" +
                "36 - Результат ≥ макс.6 или ≤ мин.6 (\"Аналоговые сигналы\")"));
        #endregion

        #region Всплывающая подсказка "Область"
        /// <summary>
        /// Всплывающая подсказка "Область"
        /// </summary>
        public string Area
        {
            get => (string)GetValue(AreaProperty);
            set => SetValue(AreaProperty, value);
        }

        public static readonly DependencyProperty AreaProperty = DependencyProperty.Register(
            nameof(Area),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Выбор области источника\n" +
                "0 - Таблица сигналов\n" +
                "1 - DI формируемые\n"));
        #endregion

        #region Всплывающая подсказка "Выбор агрегата"
        /// <summary>
        /// Всплывающая подсказка "Выбор агрегата"
        /// </summary>
        public string SelectedUMPNA
        {
            get => (string)GetValue(SelectedUMPNAProperty);
            set => SetValue(SelectedUMPNAProperty, value);
        }

        public static readonly DependencyProperty SelectedUMPNAProperty = DependencyProperty.Register(
            nameof(SelectedUMPNA),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указываем номер агрегата к которому\nотносится сигнал вибрации.\n0 - не вибрация"));
        #endregion

        #region Всплывающая подсказка "Короткое описание параметра для ВУ"
        /// <summary>
        /// Всплывающая подсказка "Короткое описание параметра для ВУ"
        /// </summary>
        public string ShortDescription
        {
            get => (string)GetValue(ShortDescriptionProperty);
            set => SetValue(ShortDescriptionProperty, value);
        }

        public static readonly DependencyProperty ShortDescriptionProperty = DependencyProperty.Register(
            nameof(ShortDescription),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Короткое обозначение:\n" +
                "T - Температура\n" +
                "Tоп - Температура опорного\n" +
                "Tуп - Температура упорного\n" +
                "Tуп т.1 - Температура упорного (точка 1)\n" +
                "Tуп т.2 - Температура упорного (точка 2)\n" +
                "L - Уровень\n" +
                "P - Давление\n" +
                "dP - Перепад давления\n" +
                "I - Ток\n" +
                "H - Гистерезис\n" +
                "Q - Процент / Положение\n" +
                "U - Напряжение\n" +
                "V - Объем / Скорость\n" +
                "X - Вибрация\n" +
                "Xв - Вибрация вертикальная\n" +
                "Xг - Вибрация горизонтальная\n" +
                "Xо - Вибрация опорного\n" +
                "Газ - Газ\n" +
                "Газ. т.1 - Газ (точка 1)\n" +
                "Газ. т.2 - Газ (точка 2)\n" +
                "Газ. т.3 - Газ (точка 3)\n" +
                "Газ. т.4 - Газ (точка 4)\n" +
                "P - Давление \"Значение предуставки\""));
        #endregion

        #region Всплывающая подсказка "Тип вибрации"
        /// <summary>
        /// Всплывающая подсказка "Тип вибрации"
        /// </summary>
        public string TypeVibration
        {
            get => (string)GetValue(TypeVibrationProperty);
            set => SetValue(TypeVibrationProperty, value);
        }

        public static readonly DependencyProperty TypeVibrationProperty = DependencyProperty.Register(
            nameof(TypeVibration),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "0 - Насос\n" +
                "1 - Двигатель"));
        #endregion

        #region Всплывающая подсказка "Тип ПИ"
        /// <summary>
        /// Всплывающая подсказка "Тип ПИ"
        /// </summary>
        public string TypePI
        {
            get => (string)GetValue(TypePIProperty);
            set => SetValue(TypePIProperty, value);
        }

        public static readonly DependencyProperty TypePIProperty = DependencyProperty.Register(
            nameof(TypePI),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "0 - Аналоговый шлеф\n" +
                "1 - АПУ"));
        #endregion

        #region Всплывающая подсказка "Состояние станции"
        /// <summary>
        /// Всплывающая подсказка "Состояние станции"
        /// </summary>
        public string StateStation
        {
            get => (string)GetValue(StateStationProperty);
            set => SetValue(StateStationProperty, value);
        }

        public static readonly DependencyProperty StateStationProperty = DependencyProperty.Register(
            nameof(StateStation),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Условие сработки защиты в\nсоотвествии с состоянием станции"));
        #endregion

        #region Всплывающая подсказка "Состояние агрегата"
        /// <summary>
        /// Всплывающая подсказка "Состояние агрегата"
        /// </summary>
        public string StateNA
        {
            get => (string)GetValue(StateNAProperty);
            set => SetValue(StateNAProperty, value);
        }

        public static readonly DependencyProperty StateNAProperty = DependencyProperty.Register(
            nameof(StateNA),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Условие сработки защиты в\nсоотвествии с состоянием агрегата"));
        #endregion

        #region Всплывающая подсказка "Принадлежность защиты к группе"
        /// <summary>
        /// Всплывающая подсказка "Принадлежность защиты к группе"
        /// </summary>
        public string Shoulder
        {
            get => (string)GetValue(ShoulderProperty);
            set => SetValue(ShoulderProperty, value);
        }

        public static readonly DependencyProperty ShoulderProperty = DependencyProperty.Register(
            nameof(Shoulder),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указывам номер плеча, для\nкоторого должна сработать защита\n\n" +
                "1 - Плечо №1 или для всех плеч\n" +
                "2 - Плечо №2\n" +
                "3 - Плечо №3\n" +
                "4 - Плечо №4\n" +
                "5 - Плечо №5\n" +
                "6 - Плечо №6\n" +
                "7 - Плечо №7\n" +
                "8 - Плечо №8\n" +
                "9 - Плечо №9\n" +
                "10 - Плечо №10"));
        #endregion

        #region Всплывающая подсказка "Принадлежность защиты к подгруппе"
        /// <summary>
        /// Всплывающая подсказка "Принадлежность защиты к подгруппе"
        /// </summary>
        public string SubShoulder
        {
            get => (string)GetValue(SubShoulderProperty);
            set => SetValue(SubShoulderProperty, value);
        }

        public static readonly DependencyProperty SubShoulderProperty = DependencyProperty.Register(
            nameof(SubShoulder),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указывам номер плеча, для\nкоторого должна сработать защита\n\n" +
                "1 - Подплечо №1 или для всех подплеч\n" +
                "2 - Подплечо №2\n" +
                "3 - Подплечо №3\n" +
                "4 - Подплечо №4\n" +
                "5 - Подплечо №5\n" +
                "6 - Подплечо №6\n" +
                "7 - Подплечо №7\n" +
                "8 - Подплечо №8\n" +
                "9 - Подплечо №9\n" +
                "10 - Подплечо №10"));
        #endregion

        #region Всплывающая подсказка "Автодеблокировка"
        /// <summary>
        /// Всплывающая подсказка "Автодеблокировка"
        /// </summary>
        public string AutoDeblock
        {
            get => (string)GetValue(AutoDeblockProperty);
            set => SetValue(AutoDeblockProperty, value);
        }

        public static readonly DependencyProperty AutoDeblockProperty = DependencyProperty.Register(
            nameof(AutoDeblock),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "По РД п.6.4.1.2 автодеблок только у следующих защит:\n\n" +
                "- предельное минимальное давление на входе МНС;\n" +
                "- предельное максимальное давление на выходе МНС;\n" +
                "- предельное максимальное давление на выходе НПС;\n" +
                "- предельный максимальный перепад давления на УРД;\n" +
                "- аварийное минимальное давление на входе МНС;\n" +
                "- аварийная максимальная скорость опорожнения резервуара;\n" +
                "- аварийное максимальное давление в трубопроводе РП;\n" +
                "- аварийное максимальное давление на входе НПС с РП."));
        #endregion

        #region Всплывающая подсказка "Тип остановки станции"
        /// <summary>
        /// Всплывающая подсказка "Тип остановки станции"
        /// </summary>
        public string StopTypeStation
        {
            get => (string)GetValue(StopTypeStationProperty);
            set => SetValue(StopTypeStationProperty, value);
        }

        public static readonly DependencyProperty StopTypeStationProperty = DependencyProperty.Register(
            nameof(StopTypeStation),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указывам тип остановки станции:\n\n" +
                "1 - Одновременное отключение МНА\n" +
                "2 - Последовательное отключение МНА\n" +
                "3 - Отключение первого по потоку МНА, с продолжением\n" +
                "4 - Отключение первого по потоку МНА\n" +
                "5 - Необходимость отключения ПНС, после последовательного отключения МНА\n" +
                "6 - НЕ ТРОГАТЬ!!!\n" +
                "7 - Необходимость отключения ПНС, без ожидания последовательного отключения МНА\n"));
        #endregion

        #region Всплывающая подсказка "Тип остановки агрегатов"
        /// <summary>
        /// Всплывающая подсказка "Тип остановки агрегатов"
        /// </summary>
        public string StopTypeNA
        {
            get => (string)GetValue(StopTypeNAProperty);
            set => SetValue(StopTypeNAProperty, value);
        }

        public static readonly DependencyProperty StopTypeNAProperty = DependencyProperty.Register(
            nameof(StopTypeNA),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указывам тип остановки МНА\n\n" +
                "1 - Управляемый останов НА\n" +
                "2 - Отключение ВВ по эл.защите\n" +
                "3 - Управляемый останов НА, с отключением ВВ\n" +
                "4 - Отключение ВВ при АЧР(СОАН)\n" +
                "5 - Останов, либо неуправляемый останов(для ЧРП) НА\n" +
                "6 - Останов НА при невыполнении команды остановки\n" +
                "7 - Останов НА, при невыполении отключения ВВ"));
        #endregion

        #region Всплывающая подсказка "Тип защиты"
        /// <summary>
        /// Всплывающая подсказка "Тип защиты"
        /// </summary>
        public string TypeProt
        {
            get => (string)GetValue(TypeProtProperty);
            set => SetValue(TypeProtProperty, value);
        }

        public static readonly DependencyProperty TypeProtProperty = DependencyProperty.Register(
            nameof(TypeProt),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "1 - Защита по пожару\n" +
                "2 - Защита по пожару в ССВД\n" +
                "3 - Защита \"Останов КЦ\"\n" +
                "4 - Защита \"Отсутствие связи с УСО\"\n" +
                "5 - Защита по пожару МНС\n" +
                "6 - Защита по АЧР(САОН)\n" +
                "7 - Защита \"Отключение МНС кнопкой с АРМ оператора\"\n" +
                "8 - Защита \"Отключение ПНС кнопкой с АРМ оператора\"\n" +
                "9 - Защита \"Отключение МНС по команде ЦСПА\"\n" +
                "10 - Защита \"Отключение ПНС по команде ЦСПА\"\n" +
                "11 - Защита \"Отсутствие связи с ЦСПА\"\n"));
        #endregion

        #region Всплывающая подсказка "Управление задвижками"
        /// <summary>
        /// Всплывающая подсказка "Управление задвижками"
        /// </summary>
        public string ControlUZD
        {
            get => (string)GetValue(ControlUZDProperty);
            set => SetValue(ControlUZDProperty, value);
        }

        public static readonly DependencyProperty ControlUZDProperty = DependencyProperty.Register(
            nameof(ControlUZD),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указываем номера групп задвижек"));
        #endregion

        #region Всплывающая подсказка "Управление вспомсистемами"
        /// <summary>
        /// Всплывающая подсказка "Управление вспомсистемами"
        /// </summary>
        public string ControlUVS
        {
            get => (string)GetValue(ControlUVSProperty);
            set => SetValue(ControlUVSProperty, value);
        }

        public static readonly DependencyProperty ControlUVSProperty = DependencyProperty.Register(
            nameof(ControlUVS),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указываем номера групп вспомсистем"));
        #endregion

        #region Всплывающая подсказка "Управление табло и сиренами"
        /// <summary>
        /// Всплывающая подсказка "Управление табло и сиренами"
        /// </summary>
        public string ControlUTS
        {
            get => (string)GetValue(ControlUTSProperty);
            set => SetValue(ControlUTSProperty, value);
        }

        public static readonly DependencyProperty ControlUTSProperty = DependencyProperty.Register(
            nameof(ControlUTS),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Указываем номера групп табло и сирен"));
        #endregion

        #region Всплывающая подсказка "Тип параметра для агрегатных защит"
        /// <summary>
        /// Всплывающая подсказка "Тип параметра для агрегатных защит"
        /// </summary>
        public string TypeParamKTPRA
        {
            get => (string)GetValue(TypeParamKTPRAProperty);
            set => SetValue(TypeParamKTPRAProperty, value);
        }

        public static readonly DependencyProperty TypeParamKTPRAProperty = DependencyProperty.Register(
            nameof(TypeParamKTPRA),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "1 - Отключение ВВ внешними средствами"));
        #endregion

        #region Всплывающая подсказка "Тип параметра для предельных параметров общестанционных защит"
        /// <summary>
        /// Всплывающая подсказка "Тип параметра для предельных параметров общестанционных защит"
        /// </summary>
        public string TypeParamKTPRS
        {
            get => (string)GetValue(TypeParamKTPRSProperty);
            set => SetValue(TypeParamKTPRSProperty, value);
        }

        public static readonly DependencyProperty TypeParamKTPRSProperty = DependencyProperty.Register(
            nameof(TypeParamKTPRS),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "1 - Защита по загазованности\n" +
                "2 - Удаление избытка тепла\n" +
                "3 - Защита по откачкам утечек\n" +
                "4 - Защита по откачкам утечек (ССВД)"));
        #endregion

        #region Всплывающая подсказка "Настройка сигнализации"
        /// <summary>
        /// Всплывающая подсказка "Настройка сигнализации"
        /// </summary>
        public string SettingsWarning
        {
            get => (string)GetValue(SettingsWarningProperty);
            set => SetValue(SettingsWarningProperty, value);
        }

        public static readonly DependencyProperty SettingsWarningProperty = DependencyProperty.Register(
            nameof(SettingsWarning),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "0(пусто) - Отключено\n" +
                "1 - Сообщение и звук\n" +
                "2 - Сообщение"));
        #endregion

        #region Всплывающая подсказка "Тип параметра Табло и сирен"
        /// <summary>
        /// Всплывающая подсказка "Тип параметра Табло и сирен"
        /// </summary>
        public string TypeParamUTS
        {
            get => (string)GetValue(TypeParamUTSProperty);
            set => SetValue(TypeParamUTSProperty, value);
        }

        public static readonly DependencyProperty TypeParamUTSProperty = DependencyProperty.Register(
            nameof(TypeParamUTS),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "0(пусто) - DO остальные\n" +
                "1 - Табло\n" +
                "2 - Cирены\n" +
                "3 - Cирены с возможностью блокировки\n" +
                "4 - Датчик СОД c контролем\n" +
                "5 - Датчик СОД с контролем и деблокировкой\n" +
                "6 - Датчик СОД  с контролем и питанием\n" +
                "7 - Датчик СОД с контролем, деблокировкой и питанием"));
        #endregion

        #region Всплывающая подсказка "Флаг табло: АПТ Отключено"
        /// <summary>
        /// Всплывающая подсказка "Флаг табло: АПТ Отключено"
        /// </summary>
        public string UTS_AptOff
        {
            get => (string)GetValue(UTS_AptOffProperty);
            set => SetValue(UTS_AptOffProperty, value);
        }

        public static readonly DependencyProperty UTS_AptOffProperty = DependencyProperty.Register(
            nameof(UTS_AptOff),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Флаг табло:\n" +
                "АПТ Отключено\n"));
        #endregion

        #region Всплывающая подсказка "Тип цепей включения"
        /// <summary>
        /// Всплывающая подсказка "Тип цепей включения"
        /// </summary>
        public string UTS_CBType
        {
            get => (string)GetValue(UTS_CBTypeProperty);
            set => SetValue(UTS_CBTypeProperty, value);
        }

        public static readonly DependencyProperty UTS_CBTypeProperty = DependencyProperty.Register(
            nameof(UTS_CBType),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "Проверка исправности цепей включения\n" +
                "0 - Нет целостности\n" +
                "1 - Есть целостнотсь(диагностика КЗ и обрывы)\n" +
                "2 - есть целоснтность(нет диагностики КЗ и обрыва)\n"
                ));
        #endregion
    }
}
