﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Devices
{
    public class PTTest
    {
        const string Incorrect = "Incorrect";
        const string PT = "PT";
        const string PT_IOLINK = "PT_IOLINK";
        const string DEV_SPAE = "DEV_SPAE";
        const string PT_VIRT = "PT_VIRT";

        const string AI = Device.IODevice.IOChannel.AI;
        const string AO = Device.IODevice.IOChannel.AO;
        const string DI = Device.IODevice.IOChannel.DI;
        const string DO = Device.IODevice.IOChannel.DO;

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
        /// Тестирование диапазона измерения устройства
        /// </summary>
        /// <param name="expected">Ожидаемый диапазон</param>
        /// <param name="subType">Актуальный подтип</param>
        /// <param name="value1">Начало диапазона</param>
        /// <param name="value2">Конец диапазона</param>
        /// <param name="device">Тестируемое устройство</param>
        [TestCaseSource(nameof(GetRangeTestData))]
        public void GetRangeTest(string expected, string subType,
            double value1, double value2, Device.IODevice device)
        {
            device.SetSubType(subType);
            device.SetParameter(DeviceParameter.P_MIN_V, value1);
            device.SetParameter(DeviceParameter.P_MAX_V, value2);
            Assert.AreEqual(expected, device.GetRange());
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
            int actualAI = device.Channels.Where(x => x.Name == AI).Count();
            int actualAO = device.Channels.Where(x => x.Name == AO).Count();
            int actualDI = device.Channels.Where(x => x.Name == DI).Count();
            int actualDO = device.Channels.Where(x => x.Name == DO).Count();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedChannelsCount[AI], actualAI);
                Assert.AreEqual(expectedChannelsCount[AO], actualAO);
                Assert.AreEqual(expectedChannelsCount[DI], actualDI);
                Assert.AreEqual(expectedChannelsCount[DO], actualDO);
            });
        }

        /// <summary>
        /// 1 - Ожидаемое перечисление подтипа,
        /// 2 - Задаваемое значение подтипа,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] SetSubTypeTestData()
        {
            return new object[]
            {
                new object[] { Device.DeviceSubType.NONE, string.Empty,
                    GetRandomPTDevice() },
                new object[] { Device.DeviceSubType.PT, PT,
                    GetRandomPTDevice() },
                new object[] { Device.DeviceSubType.PT_IOLINK, PT_IOLINK,
                    GetRandomPTDevice() },
                new object[] { Device.DeviceSubType.DEV_SPAE, DEV_SPAE,
                    GetRandomPTDevice() },
                new object[] { Device.DeviceSubType.NONE, Incorrect,
                    GetRandomPTDevice() },
                new object[] { Device.DeviceSubType.PT_VIRT, PT_VIRT,
                    GetRandomPTDevice() },
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
                new object[] { string.Empty, string.Empty, GetRandomPTDevice() },
                new object[] { PT, PT, GetRandomPTDevice() },
                new object[] { PT_IOLINK, PT_IOLINK, GetRandomPTDevice() },
                new object[] { DEV_SPAE, DEV_SPAE, GetRandomPTDevice() },
                new object[] { string.Empty, Incorrect, GetRandomPTDevice() },
                new object[] { PT_VIRT, PT_VIRT, GetRandomPTDevice() },
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
            var exportForPT = new Dictionary<string, int>()
            {
                {DeviceTag.ST, 1},
                {DeviceTag.M, 1},
                {DeviceTag.V, 1},
                {DeviceTag.P_MIN_V, 1},
                {DeviceTag.P_MAX_V, 1},
                {DeviceTag.P_CZ, 1},
            };

            var exportForPTIOLink = new Dictionary<string, int>()
            {
                {DeviceTag.M, 1},
                {DeviceTag.V, 1},
                {DeviceTag.P_MIN_V, 1},
                {DeviceTag.P_MAX_V, 1},
                {DeviceTag.P_ERR, 1},
            };

            var exportForDevSpae = new Dictionary<string, int>()
            {
                {DeviceTag.M, 1},
                {DeviceTag.V, 1},
                {DeviceTag.P_ERR, 1},
            };

            return new object[]
            {
                new object[] {exportForPT, PT, GetRandomPTDevice()},
                new object[] {exportForPTIOLink, PT_IOLINK, 
                    GetRandomPTDevice()},
                new object[] {exportForDevSpae, DEV_SPAE, 
                    GetRandomPTDevice()},
                new object[] {null, string.Empty, GetRandomPTDevice()},
                new object[] {null, Incorrect, GetRandomPTDevice()},
                new object[] {null, PT_VIRT, GetRandomPTDevice()},
            };
        }

        /// <summary>
        /// 1 - Ожидаемое значение,
        /// 2 - Подтип устройства в виде строки,
        /// 3 - Значение параметра меньшее,
        /// 4 - Значение параметра большее,
        /// 5 - Устройство для теста
        /// </summary>
        /// <returns></returns>
        private static object[] GetRangeTestData()
        {
            return new object[]
            {
                new object[] { $"_{2.0}..{4.0}", PT, 2.0, 4.0,
                    GetRandomPTDevice()},
                new object[] { string.Empty, PT_IOLINK, 1.0, 3.0,
                    GetRandomPTDevice()},
                new object[] { string.Empty, DEV_SPAE, 1.0, 3.0,
                    GetRandomPTDevice()},
                new object[] { string.Empty, string.Empty, 4.0, 8.0,
                    GetRandomPTDevice()},
                new object[] { string.Empty, Incorrect, 7.0, 9.0,
                    GetRandomPTDevice()},
                new object[] { string.Empty, PT_VIRT, 7.0, 9.0,
                    GetRandomPTDevice()},
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
            var defaultParameters = new string[]
            {
                DeviceParameter.P_C0,
                DeviceParameter.P_MIN_V,
                DeviceParameter.P_MAX_V,
            };

            var iolinkParameters = new string[]
            {
                DeviceParameter.P_ERR
            };

            return new object[]
            {
                new object[]
                {
                    defaultParameters,
                    PT,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    iolinkParameters,
                    PT_IOLINK,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    iolinkParameters,
                    DEV_SPAE,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    new string[0],
                    PT_VIRT,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    new string[0],
                    string.Empty,
                    GetRandomPTDevice()
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
            var emptyChannels = new Dictionary<string, int>()
            {
                { AI, 0 },
                { AO, 0 },
                { DI, 0 },
                { DO, 0 },
            };

            var oneAnalogInputChannel = new Dictionary<string, int>()
            {
                { AI, 1 },
                { AO, 0 },
                { DI, 0 },
                { DO, 0 },
            };

            return new object[]
            {
                new object[]
                {
                    oneAnalogInputChannel,
                    PT_IOLINK,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    oneAnalogInputChannel,
                    PT,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    oneAnalogInputChannel,
                    DEV_SPAE,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    emptyChannels,
                    string.Empty,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    emptyChannels,
                    Incorrect,
                    GetRandomPTDevice()
                },
                new object[]
                {
                    emptyChannels,
                    PT_VIRT,
                    GetRandomPTDevice()
                },
            };
        }

        /// <summary>
        /// Генератор PT устройств
        /// </summary>
        /// <returns></returns>
        private static Device.IODevice GetRandomPTDevice()
        {
            var randomizer = new Random();
            int value = randomizer.Next(1, 3);
            switch (value)
            {
                case 1:
                    return new Device.PT("KOAG4PT1", "+KOAG4-PT1",
                        "Test device", 1, "KOAG", 4, "Test article");
                case 2:
                    return new Device.PT("LINE1PT2", "+LINE1-PT2",
                        "Test device", 2, "LINE", 1, "Test article");
                case 3:
                    return new Device.PT("TANK2PT1", "+TANK2-PT1",
                        "Test device", 1, "TANK", 2, "Test article");
                default:
                    return new Device.PT("CW_TANK3PT3", "+CW_TANK3-PT3",
                        "Test device", 3, "CW_TANK", 3, "Test article");
            }
        }
    }
}
