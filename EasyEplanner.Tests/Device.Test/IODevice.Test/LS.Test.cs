﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public class LSTest
    {
        /// <summary>
        /// Тест установки подтипа устройства
        /// </summary>
        /// <param name="expectedSubType">Ожидаемый подтип</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(SetSubTypeTestData))]
        public void SetSubTypeTest(Device.DeviceSubType expectedSubType,
            string subType, Device.IODevice device)
        {
            device.SetSubType(subType);
            Assert.AreEqual(expectedSubType, device.DeviceSubType);
        }

        /// <summary>
        /// Тест получения подтипа устройства
        /// </summary>
        /// <param name="expectedType">Ожидаемый подтип</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(GetDeviceSubTypeStrTestData))]
        public void GetDeviceSubTypeStrTest(string expectedType,
            string subType, Device.IODevice device)
        {
            device.SetSubType(subType);
            Assert.AreEqual(expectedType, device.GetDeviceSubTypeStr(
                device.DeviceType, device.DeviceSubType));
        }

        /// <summary>
        /// Тест свойств устройств в зависимости от подтипа
        /// </summary>
        /// <param name="expectedProperties">Ожидаемый список свойств</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(GetDevicePropertiesTestData))]
        public void GetDevicePropertiesTest(
            Dictionary<string, int> expectedProperties, string subType,
            Device.IODevice device)
        {
            device.SetSubType(subType);
            Assert.AreEqual(expectedProperties, device.GetDeviceProperties(
                device.DeviceType, device.DeviceSubType));
        }

        /// <summary>
        /// Тестирование получения типа подключения датчика
        /// </summary>
        /// <param name="expected">Ожидаемый тип подключения</param>
        /// <param name="subType">Подтип устройства</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(GetConnectionTestData))]
        public void GetConnectionTest(string expected, string subType,
            Device.IODevice device)
        {
            device.SetSubType(subType);
            Assert.AreEqual(expected, device.GetConnectionType());
        }

        /// <summary>
        /// Тестирование параметров устройства
        /// </summary>
        /// <param name="parametersSequence">Ожидаемые параметры</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(ParametersTestData))]
        public void ParametersTest(string[] parametersSequence, string subType,
            Device.IODevice device)
        {
            device.SetSubType(subType);
            string[] actualParametersSequence = device.Parameters
                .Select(x => x.Key)
                .ToArray();
            Assert.AreEqual(parametersSequence, actualParametersSequence);
        }

        /// <summary>
        /// Тестирование каналов устройства
        /// </summary>
        /// <param name="expectedChannelsCount">Ожидаемое количество каналов
        /// в словаре с названием каналов</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(ChannelsTestData))]
        public void ChannelsTest(Dictionary<string, int> expectedChannelsCount,
            string subType, Device.IODevice device)
        {
            device.SetSubType(subType);
            int actualAI = device.Channels.Where(x => x.Name == "AI").Count();
            int actualAO = device.Channels.Where(x => x.Name == "AO").Count();
            int actualDI = device.Channels.Where(x => x.Name == "DI").Count();
            int actualDO = device.Channels.Where(x => x.Name == "DO").Count();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedChannelsCount["AI"], actualAI);
                Assert.AreEqual(expectedChannelsCount["AO"], actualAO);
                Assert.AreEqual(expectedChannelsCount["DI"], actualDI);
                Assert.AreEqual(expectedChannelsCount["DO"], actualDO);
            });
        }

        /// <summary>
        /// 1 - Ожидаемое значение подтипа,
        /// 2 - Задаваемое значение подтипа,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] SetSubTypeTestData()
        {
            return new object[]
            {
                new object[] { Device.DeviceSubType.LS_MIN, "LS_MIN",
                    GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.LS_MAX, "LS_MAX",
                    GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.LS_IOLINK_MIN, 
                    "LS_IOLINK_MIN", GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.LS_IOLINK_MAX, 
                    "LS_IOLINK_MAX", GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.LS_VIRT, "LS_VIRT",
                    GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.NONE, "",
                    GetRandomLSDevice() },
                new object[] { Device.DeviceSubType.NONE, "Incorrect",
                    GetRandomLSDevice() },
            };
        }

        /// <summary>
        /// 1 - Ожидаемое значение подтипа,
        /// 2 - Задаваемое значение подтипа,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] GetDeviceSubTypeStrTestData()
        {
            return new object[]
            {
                new object[] { "LS_MIN", "LS_MIN", GetRandomLSDevice() },
                new object[] { "LS_MAX", "LS_MAX", GetRandomLSDevice() },
                new object[] { "LS_IOLINK_MIN", "LS_IOLINK_MIN", 
                    GetRandomLSDevice() },
                new object[] { "LS_IOLINK_MAX", "LS_IOLINK_MAX", 
                    GetRandomLSDevice() },
                new object[] { "LS_VIRT", "LS_VIRT", GetRandomLSDevice() },
                new object[] { "", "", GetRandomLSDevice() },
                new object[] { "", "Incorrect", GetRandomLSDevice() },
            };
        }

        /// <summary>
        /// 1 - Ожидаемый список свойств для экспорта,
        /// 2 - Задаваемый подтип устройства,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] GetDevicePropertiesTestData()
        {
            var exportForIOLinkLS = new Dictionary<string, int>()
            {
                {"ST", 1},
                {"M", 1},
                {"V", 1},
                {"P_ERR", 1},
            };

            var exportForLS = new Dictionary<string, int>()
            {
                {"ST", 1},
                {"M", 1},
                {"P_DT", 1},
            };

            return new object[]
            {
                new object[] {exportForLS, "LS_MIN", GetRandomLSDevice()},
                new object[] {exportForLS, "LS_MAX", GetRandomLSDevice()},
                new object[] {exportForLS, "LS_VIRT", GetRandomLSDevice()},
                new object[] {exportForIOLinkLS, "LS_IOLINK_MIN", 
                    GetRandomLSDevice()},
                new object[] {exportForIOLinkLS, "LS_IOLINK_MAX", 
                    GetRandomLSDevice()},
                new object[] {null, "Incorrect", GetRandomLSDevice()},
                new object[] {null, "", GetRandomLSDevice()},
            };
        }

        /// <summary>
        /// 1 - Ожидаемое значение,
        /// 2 - Подтип устройства в виде строки,
        /// 3 - Устройство для теста
        /// </summary>
        /// <returns></returns>
        private static object[] GetConnectionTestData()
        {
            return new object[]
            {
                new object[] {$"_Min", "LS_MIN", GetRandomLSDevice()},
                new object[] {$"_Min", "LS_IOLINK_MIN", GetRandomLSDevice()},
                new object[] {$"_Max", "LS_MAX", GetRandomLSDevice()},
                new object[] {$"_Max", "LS_IOLINK_MAX", GetRandomLSDevice()},
                new object[] {$"", "", GetRandomLSDevice()},
                new object[] {$"", "Incorrect", GetRandomLSDevice()},
                new object[] {$"", "LS_VIRT", GetRandomLSDevice()},
            };
        }

        /// <summary>
        /// 1 - Параметры в том порядке, который нужен
        /// 2 - Подтип устройства
        /// 3 - Устройство
        /// </summary>
        /// <returns></returns>
        private static object[] ParametersTestData()
        {
            return new object[]
            {
                new object[]
                {
                    new string[] { "P_DT" },
                    "LS_MIN",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new string[] { "P_DT" },
                    "LS_MAX",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new string[0],
                    "LS_VIRT",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new string[] { "P_DT", "P_ERR" },
                    "LS_IOLINK_MIN",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new string[] { "P_DT", "P_ERR" },
                    "LS_IOLINK_MAX",
                    GetRandomLSDevice()
                },
            };
        }

        /// <summary>
        /// Данные для тестирования каналов устройств по подтипам.
        /// 1. Словарь с количеством каналов и их типами
        /// 2. Подтип устройства
        /// 3. Устройство
        /// </summary>
        /// <returns></returns>
        private static object[] ChannelsTestData()
        {
            return new object[]
            {
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 0 },
                        { "AO", 0 },
                        { "DI", 1 },
                        { "DO", 0 },
                    },
                    "LS_MIN",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 0 },
                        { "AO", 0 },
                        { "DI", 1 },
                        { "DO", 0 },
                    },
                    "LS_MAX",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 1 },
                        { "AO", 0 },
                        { "DI", 0 },
                        { "DO", 0 },
                    },
                    "LS_IOLINK_MIN",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 1 },
                        { "AO", 0 },
                        { "DI", 0 },
                        { "DO", 0 },
                    },
                    "LS_IOLINK_MAX",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 0 },
                        { "AO", 0 },
                        { "DI", 0 },
                        { "DO", 0 },
                    },
                    "LS_VIRT",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 0 },
                        { "AO", 0 },
                        { "DI", 0 },
                        { "DO", 0 },
                    },
                    "",
                    GetRandomLSDevice()
                },
                new object[]
                {
                    new Dictionary<string, int>()
                    {
                        { "AI", 0 },
                        { "AO", 0 },
                        { "DI", 0 },
                        { "DO", 0 },
                    },
                    "Incorrect",
                    GetRandomLSDevice()
                },
            };
        }

        /// <summary>
        /// Генератор LS устройств
        /// </summary>
        /// <returns></returns>
        private static Device.IODevice GetRandomLSDevice()
        {
            var randomizer = new Random();
            int value = randomizer.Next(1, 3);
            switch (value)
            {
                case 1:
                    return new Device.LS("KOAG4LS1", "Test device", 1,
                        "KOAG", 4, "DeviceArticle");
                case 2:
                    return new Device.LS("LINE1LS2", "Test device", 2,
                        "LINE", 1, "DeviceArticle");
                case 3:
                    return new Device.LS("TANK2LS1", "Test device", 1,
                        "TANK", 2, "DeviceArticle");
                default:
                    return new Device.LS("CW_TANK3LS3", "Test device", 3,
                        "CW_TANK", 3, "DeviceArticle");
            }
        }
    }
}
