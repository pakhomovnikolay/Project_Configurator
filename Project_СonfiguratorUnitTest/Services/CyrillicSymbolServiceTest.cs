using Project_Сonfigurator.Services;

namespace Project_СonfiguratorUnitTest.Services
{
    [TestClass]
    public class CyrillicSymbolServiceTest
    {
        [TestMethod]
        public void IsCyrillicSymbolTest_1()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, !Service.CheckAvailability("123"), "CyrillicSymbolService - Тест не пройден");
        }

        [TestMethod]
        public void IsCyrillicSymbolTest_2()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, !Service.CheckAvailability("qwe"), "CyrillicSymbolService - Тест не пройден");
        }

        [TestMethod]
        public void IsCyrillicSymbolTest_3()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, Service.CheckAvailability("123йцу"), "CyrillicSymbolService - Тест не пройден");
        }

        [TestMethod]
        public void IsCyrillicSymbolTest_4()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, Service.CheckAvailability("йцу123"), "CyrillicSymbolService - Тест не пройден");
        }

        [TestMethod]
        public void IsCyrillicSymbolTest_5()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, Service.CheckAvailability("qweйцу"), "CyrillicSymbolService - Тест не пройден");
        }

        [TestMethod]
        public void IsCyrillicSymbolTest_6()
        {
            bool expected = true;
            CyrillicSymbolService Service = new();
            Assert.AreEqual(expected, Service.CheckAvailability("йцу"), "CyrillicSymbolService - Тест не пройден");
        }
    }
}