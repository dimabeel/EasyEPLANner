﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Devices
{
    public class WTTest
    {
        const string Incorrect = "Incorrect";
        const string WT = "WT";
        const string WT_VIRT = "WT_VIRT";

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
        /// 1 - Ожидаемое значение подтипа,
        /// 2 - Задаваемое значение подтипа,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] SetSubTypeTestData()
        {
            return new object[]
            {
                new object[] { Device.DeviceSubType.WT, WT,
                    GetRandomWTDevice() },
                new object[] { Device.DeviceSubType.WT_VIRT, WT_VIRT,
                    GetRandomWTDevice() },
                new object[] { Device.DeviceSubType.WT, WT,
                    GetRandomWTDevice() },
                new object[] { Device.DeviceSubType.NONE, Incorrect,
                    GetRandomWTDevice() },
            };
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
        /// 1 - Ожидаемое значение подтипа,
        /// 2 - Задаваемое значение подтипа,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] GetDeviceSubTypeStrTestData()
        {
            return new object[]
            {
                new object[] { WT, string.Empty, GetRandomWTDevice() },
                new object[] { WT, WT, GetRandomWTDevice() },
                new object[] { string.Empty, Incorrect, GetRandomWTDevice() },
                new object[] { WT_VIRT, WT_VIRT, GetRandomWTDevice() },
            };
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
        /// 1 - Ожидаемый список свойств для экспорта,
        /// 2 - Задаваемый подтип устройства,
        /// 3 - Устройство для тестов
        /// </summary>
        /// <returns></returns>
        private static object[] GetDevicePropertiesTestData()
        {
            var exportForWT = new Dictionary<string, int>()
            {
                {DeviceTag.ST, 1},
                {DeviceTag.M, 1},
                {DeviceTag.V, 1},
                {DeviceTag.P_NOMINAL_W, 1},
                {DeviceTag.P_DT, 1},
                {DeviceTag.P_RKP, 1},
                {DeviceTag.P_CZ, 1},
            };

            return new object[]
            {
                new object[] {exportForWT, string.Empty, GetRandomWTDevice() },
                new object[] {exportForWT, WT, GetRandomWTDevice() },
                new object[] {null, WT_VIRT, GetRandomWTDevice() },
                new object[] {null, Incorrect, GetRandomWTDevice() },
            };
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
        /// 1 - Параметры в том порядке, который нужен
        /// 2 - Подтип устройства
        /// 3 - Устройство
        /// </summary>
        /// <returns></returns>
        private static object[] ParametersTestData()
        {
            var parameters = new string[]
            {
                DeviceParameter.P_NOMINAL_W,
                DeviceParameter.P_RKP,
                DeviceParameter.P_C0,
                DeviceParameter.P_DT
            };
            var emptyParameters = new string[0];

            return new object[]
            {
                new object[]
                {
                    parameters,
                    WT,
                    GetRandomWTDevice()
                },
                new object[]
                {
                    parameters,
                    string.Empty,
                    GetRandomWTDevice()
                },
                new object[]
                {
                    emptyParameters,
                    WT_VIRT,
                    GetRandomWTDevice(),
                },
                new object[]
                {
                    emptyParameters,
                    Incorrect,
                    GetRandomWTDevice(),
                }
            };
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
        /// Данные для тестирования каналов устройств по подтипам.
        /// 1. Словарь с количеством каналов и их типами
        /// 2. Подтип устройства
        /// 3. Устройство
        /// </summary>
        /// <returns></returns>
        private static object[] ChannelsTestData()
        {
            var defaultChannels = new Dictionary<string, int>()
            {
                { AI, 2 },
                { AO, 0 },
                { DI, 0 },
                { DO, 0 },
            };
            var emptyChannels = new Dictionary<string, int>()
            {
                { AI, 0 },
                { AO, 0 },
                { DI, 0 },
                { DO, 0 },
            };

            return new object[]
            {
                new object[]
                {
                    defaultChannels,
                    WT,
                    GetRandomWTDevice()
                },
                new object[]
                {
                    defaultChannels,
                    string.Empty,
                    GetRandomWTDevice()
                },
                new object[]
                {
                    emptyChannels,
                    Incorrect,
                    GetRandomWTDevice()
                },
                new object[]
                {
                    emptyChannels,
                    WT_VIRT,
                    GetRandomWTDevice()
                }
            };
        }

        /// <summary>
        /// Генератор WT устройств
        /// </summary>
        /// <returns></returns>
        private static Device.IODevice GetRandomWTDevice()
        {
            var randomizer = new Random();
            int value = randomizer.Next(1, 3);
            switch (value)
            {
                case 1:
                    return new Device.WT("KOAG4WT1", "+KOAG4-WT1",
                        "Test device", 1, "KOAG", 4, "DeviceArticle");
                case 2:
                    return new Device.WT("LINE1WT2", "+LINE1-WT2",
                        "Test device", 2, "LINE", 1, "DeviceArticle");
                case 3:
                    return new Device.WT("TANK2WT1", "+TANK2-WT1",
                        "Test device", 1, "TANK", 2, "DeviceArticle");
                default:
                    return new Device.WT("CW_TANK3WT3", "+CW_TANK3-WT3",
                        "Test device", 3, "CW_TANK", 3, "DeviceArticle");
            }
        }
    }
}
