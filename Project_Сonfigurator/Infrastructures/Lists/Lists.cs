using Project_Сonfigurator.Infrastructures.ToolTips;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Lists
{
    public class Lists : Freezable
    {
        protected override Freezable CreateInstanceCore() => new Lists();

        #region Режимы работы станции для обшестанционных защит
        /// <summary>
        /// Режимы работы станции для обшестанционных защит
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
                "0 - Всегда\n" +
                "1 - В работе\n" +
                "2 - Остановленна\n"));
        #endregion

        #region Режимы работы агрегата для агрегатных защит
        /// <summary>
        /// Режимы работы агрегата для агрегатных защит
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
                "0 - Всегда\n" +
                "1 - Остановлен\n" +
                "3 - Пусковой режим ЭД\n" +
                "4 - Пусковой режим насоса\n" +
                "5 - Не пусковой режим ЭД\n" +
                "6 - Не пусковой режим насоса\n" +
                "7 - Не пусковой режим вне раб.\n" +
                "8 - Идет плавный пуск\n"));
        #endregion

        #region Список модулей для систем SE
        /// <summary>
        /// Список модулей для систем SE
        /// </summary>
        public string SEModuleList
        {
            get => (string)GetValue(SEModuleListProperty);
            set => SetValue(SEModuleListProperty, value);
        }

        public static readonly DependencyProperty SEModuleListProperty = DependencyProperty.Register(
            nameof(SEModuleList),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "\n" +
                "- - - - - - - БП - - - - - - -\n" +
                "BMX CPS 2000\n" +
                "BMX CPS 2010\n" +
                "BMX CPS 3020\n" +
                "BMX CPS 3500\n" +
                "BMX CPS 3522\n" +
                "BMX CPS 3540\n" +
                "BMX CPS 4002\n" +
                "BMX CPS 4022\n" +
                "- - - CPU M340 и прочее - - -\n" +
                "BMX P34 1000\n" +
                "BMX P34 2000\n" +
                "BMX P34 2010\n" +
                "BMX P34 20102\n" +
                "BMX P34 2020\n" +
                "BMX P34 2030\n" +
                "BMX P34 20302\n" +
                "BMX PRA 0100\n" +
                "- - - - - CPU M580 - - - - -\n" +
                "BME H58 2040 HSBY\n" +
                "BME H58 4040 HSBY\n" +
                "BME H58 6040 HSBY\n" +
                "BME H58 1020\n" +
                "BME H58 2020\n" +
                "BME H58 2040\n" +
                "BME H58 3020\n" +
                "BME H58 3040\n" +
                "BME H58 4020\n" +
                "BME H58 4040\n" +
                "BME H58 5040\n" +
                "BME H58 6040\n" +
                "BME P58 4020\n" +
                "- - - CPU M580 Safety - - -\n" +
                "BME H58 2040S HSBY\n" +
                "BME H58 4040S HSBY\n" +
                "BME H58 6040S HSBY\n" +
                "BME H58 2040S\n" +
                "BME H58 4040S\n" +
                "- - - - = Аналоги = - - - -\n" +
                "BME AHI 0812\n" +
                "BME AHO 0412\n" +
                "BMX AMI 0410\n" +
                "BMX AMI 0800\n" +
                "BMX AMI 0810\n" +
                "BMX AMM 0600\n" +
                "BMX AMO 0210\n" +
                "BMX AMO 0410\n" +
                "BMX AMO 0802\n" +
                "BMX ART 0414.2\n" +
                "BMX ART 0814.2\n" +
                "- - - - - Дискреты - - - - -\n" +
                "BMX DAI 0805\n" +
                "BMX DAI 0814\n" +
                "BMX DAI 1602\n" +
                "BMX DAI 1603\n" +
                "BMX DAI 1604\n" +
                "BMX DAI 1614\n" +
                "BMX DAI 1615\n" +
                "BMX DAO 1605\n" +
                "BMX DAO 1615\n" +
                "BMX DDI 1602\n" +
                "BMX DDI 1603\n" +
                "BMX DDI 1604\n" +
                "BMX DDI 3202K\n" +
                "BMX DDI 6402K\n" +
                "BMX DDM 16022\n" +
                "BMX DDM 16025\n" +
                "BMX DDM 3202K\n" +
                "BMX DDO 1602\n" +
                "BMX DDO 1612\n" +
                "BMX DDO 3202K\n" +
                "BMX DDO 6402K\n" +
                "BMX DRA 0804\n" +
                "BMX DRA 0805\n" +
                "BMX DRA 0815\n" +
                "BMX DRA 1605\n" +
                "BMX DRC 0805\n" +
                "BMX ERT 1604\n" +
                "- - - - - - Сеть - - - - - -\n" +
                "BME CXM 0100\n" +
                "BME NOC 0301\n" +
                "BME NOC 0301.2\n" +
                "BME NOC 0301.3\n" +
                "BME NOC 0301.4\n" +
                "BME NOC 0311\n" +
                "BME NOC 0311.2\n" +
                "BME NOC 0311.3\n" +
                "BME NOC 0311.4\n" +
                "BME NOC 0321\n" +
                "BME NOP 0300\n" +
                "BME NOR 2200\n" +
                "BME NOS 0300\n" +
                "BMX EIA 0100\n" +
                "BMX NGD 0100\n" +
                "BMX NOC 0402\n" +
                "BMX NOM 0200\n" +
                "BMX NOM 0200.2\n" +
                "BMX NOM 0200.3\n" +
                "BMX NOM 0200.4\n" +
                "BMX NOR 0200\n" +
                "BMX NOR 0200H\n" +
                "BMX NRP 0200\n" +
                "BMX NRP 0201\n" +
                "BME CRA 312 10\n" +
                "BMX CRA 312 10\n" +
                "BMX CRA 312 00\n" +
                "- - - Защитная заглушка - - -\n" +
                "BMX XEM 010"));
        #endregion

        #region Список модулей для систем PS
        /// <summary>
        /// Список модулей для систем PS
        /// </summary>
        public string PSModuleList
        {
            get => (string)GetValue(PSModuleListProperty);
            set => SetValue(PSModuleListProperty, value);
        }

        public static readonly DependencyProperty PSModuleListProperty = DependencyProperty.Register(
            nameof(PSModuleList),
            typeof(string),
            typeof(ToolTipParam),
            new PropertyMetadata(
                "\n" +
                "- - - Оконечные модули - - - -\n" +
                "R500 ST 02 011\n" +
                "R500 ST 02 021\n" +
                "R500 ST 02 111\n" +
                "R500 ST 02 121\n" +
                "R500 ST 00 001\n" +
                "- - - - - - - DI - - - - - - -\n" +
                "R500 DI 32 011\n" +
                "- - - - - - - DO - - - - - - -\n" +
                "R500 DO 32 011\n" +
                "R500 DO 32 012\n" +
                "- - - - - - - AI - - - - - - -\n" +
                "R500 AI 08 041\n" +
                "R500 AI 08 051\n" +
                "- - - - - - - AO - - - - - - -\n" +
                "R500 AO 08 011\n" +
                "- - - - - - - БП - - - - - - -\n" +
                "R500 PP 00 011\n" +
                "- - - - CPU и прочее - - - - -\n" +
                "R500 CU 00 061\n" +
                "R500 CU 00 051\n" +
                "- - - - - - - CP - - - - - - -\n" +
                "R500 CP 04 011\n" +
                "R500 CP 02 021\n" +
                "- - - - - - - DA - - - - - - -\n" +
                "R500 DA 03 011"));
        #endregion
    }
}
