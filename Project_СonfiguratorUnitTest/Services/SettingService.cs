﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_СonfiguratorUnitTest.Services
{
    [TestClass]
    public class SettingService
    {
        [TestMethod]
        public void Test_1()
        {
            bool expected = true;
            bool result = true;
            Assert.AreEqual(expected, result, "Test_1 - Тест не пройден");
        }
    }
}
