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
            PSModuleList = new()
            {
                "",
                "- - - Оконечные модули - - - -",
                "R500 ST 02 011",
                "R500 ST 02 021",
                "R500 ST 02 111",
                "R500 ST 02 121",
                "R500 ST 00 001",
                "- - - - - - - DI - - - - - - -",
                "R500 DI 32 011",
                "- - - - - - - DO - - - - - - -",
                "R500 DO 32 011",
                "R500 DO 32 012",
                "- - - - - - - AI - - - - - - -",
                "R500 AI 08 041",
                "R500 AI 08 051",
                "- - - - - - - AO - - - - - - -",
                "R500 AO 08 011",
                "- - - - - - - БП - - - - - - -",
                "R500 PP 00 011",
                "- - - - CPU и прочее - - - - -",
                "R500 CU 00 061",
                "R500 CU 00 051",
                "- - - - - - - CP - - - - - - -",
                "R500 CP 04 011",
                "R500 CP 02 021",
                "- - - - - - - DA - - - - - - -",
                "R500 DA 03 0"
            };

            SEModuleList = new()
            {
                "",
                "- - - - - - - БП - - - - - - -",
                "BMX CPS 2000",
                "BMX CPS 2010",
                "BMX CPS 3020",
                "BMX CPS 3500",
                "BMX CPS 3522",
                "BMX CPS 3540",
                "BMX CPS 4002",
                "BMX CPS 4022",
                "- - - CPU M340 и прочее - - -",
                "BMX P34 1000",
                "BMX P34 2000",
                "BMX P34 2010",
                "BMX P34 20102",
                "BMX P34 2020",
                "BMX P34 2030",
                "BMX P34 20302",
                "BMX PRA 0100",
                "- - - - - CPU M580 - - - - -",
                "BME H58 2040 HSBY",
                "BME H58 4040 HSBY",
                "BME H58 6040 HSBY",
                "BME H58 1020",
                "BME H58 2020",
                "BME H58 2040",
                "BME H58 3020",
                "BME H58 3040",
                "BME H58 4020",
                "BME H58 4040",
                "BME H58 5040",
                "BME H58 6040",
                "BME P58 4020",
                "- - - CPU M580 Safety - - -",
                "BME H58 2040S (Safety) HSBY",
                "BME H58 4040S (Safety) HSBY",
                "BME H58 6040S (Safety) HSBY",
                "BME H58 2040S (Safety)",
                "BME H58 4040S (Safety)",
                "- - - - = Аналоги = - - - -",
                "BME AHI 0812",
                "BME AHO 0412",
                "BMX AMI 0410",
                "BMX AMI 0800",
                "BMX AMI 0810",
                "BMX AMM 0600",
                "BMX AMO 0210",
                "BMX AMO 0410",
                "BMX AMO 0802",
                "BMX ART 0414.2",
                "BMX ART 0814.2",
                "- - - - - Дискреты - - - - -",
                "BMX DAI 0805",
                "BMX DAI 0814",
                "BMX DAI 1602",
                "BMX DAI 1603",
                "BMX DAI 1604",
                "BMX DAI 1614",
                "BMX DAI 1615",
                "BMX DAO 1605",
                "BMX DAO 1615",
                "BMX DDI 1602",
                "BMX DDI 1603",
                "BMX DDI 1604",
                "BMX DDI 3202K",
                "BMX DDI 6402K",
                "BMX DDM 16022",
                "BMX DDM 16025",
                "BMX DDM 3202K",
                "BMX DDO 1602",
                "BMX DDO 1612",
                "BMX DDO 3202K",
                "BMX DDO 6402K",
                "BMX DRA 0804",
                "BMX DRA 0805",
                "BMX DRA 0815",
                "BMX DRA 1605",
                "BMX DRC 0805",
                "BMX ERT 1604",
                "- - - - - - Сеть - - - - - -",
                "BME CXM 0100",
                "BME NOC 0301",
                "BME NOC 0301.2",
                "BME NOC 0301.3",
                "BME NOC 0301.4",
                "BME NOC 0311",
                "BME NOC 0311.2",
                "BME NOC 0311.3",
                "BME NOC 0311.4",
                "BME NOC 0321",
                "BME NOP 0300",
                "BME NOR 2200",
                "BME NOS 0300",
                "BMX EIA 0100",
                "BMX NGD 0100",
                "BMX NOC 0402",
                "BMX NOM 0200",
                "BMX NOM 0200.2",
                "BMX NOM 0200.3",
                "BMX NOM 0200.4",
                "BMX NOR 0200",
                "BMX NOR 0200H",
                "BMX NRP 0200",
                "BMX NRP 0201",
                "BME CRA 312 10",
                "BMX CRA 312 10",
                "BMX CRA 312 00",
                "- - - Защитная заглушка - - -",
                "BMX XEM 010"
            };

            StateStation = new()
            {
                "",
                "0 - Всегда",
                "1 - В работе",
                "2 - Остановленна",
            };

            StateNA = new()
            {
                "",
                "0 - Всегда",
                "1 - Остановлен",
                "3 - Пусковой режим ЭД",
                "4 - Пусковой режим насоса",
                "5 - Не пусковой режим ЭД",
                "6 - Не пусковой режим насоса",
                "7 - Не пусковой режим вне раб.",
                "8 - Идет плавный пуск"
            };

            ResultModuleList = new List<string>(PSModuleList);
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

        #region Список модулей для систем SE
        /// <summary>
        /// Список модулей для систем SE
        /// </summary>
        public List<string> SEModuleList { get; set; }
        #endregion

        #region Список модулей для систем PS
        /// <summary>
        /// Список модулей для систем PS
        /// </summary>
        public List<string> PSModuleList { get; set; }
        #endregion

        #region Результирующий список модулей
        /// <summary>
        /// Результирующий список модулей
        /// </summary>
        public List<string> ResultModuleList { get; set; }
        #endregion
    }
}
