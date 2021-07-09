﻿using System.Collections.Generic;

namespace Device
{
    /// <summary>
    /// Технологическое устройство - мотор.
    /// Параметры:
    /// 1. P_ON_TIME - время включения, мсек.
    /// </summary>
    public class M : IODevice
    {
        public M(string name, string eplanName, string description,
            int deviceNumber, string objectName, int objectNumber,
            string articleName) : base(name, eplanName, description,
                deviceNumber, objectName, objectNumber)
        {
            dSubType = DeviceSubType.NONE;
            dType = DeviceType.M;
            ArticleName = articleName;
        }

        public override string SetSubType(string subtype)
        {
            base.SetSubType(subtype);

            string errStr = string.Empty;
            switch (subtype)
            {
                case "M":
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_FREQ":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    AO.Add(new IOChannel("AO", -1, -1, -1, "Частота вращения"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_REV":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_REV_FREQ":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));
                    AO.Add(new IOChannel("AO", -1, -1, -1, "Частота вращения"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_REV_2":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_REV_FREQ_2":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));
                    AO.Add(new IOChannel("AO", -1, -1, -1, "Частота вращения"));

                    parameters.Add("P_ON_TIME", null);
                    break;


                case "M_REV_2_ERROR":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Авария"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_REV_FREQ_2_ERROR":
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Обратная связь"));
                    DI.Add(new IOChannel("DI", -1, -1, -1, "Авария"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Реверс"));
                    DO.Add(new IOChannel("DO", -1, -1, -1, "Пуск"));
                    AO.Add(new IOChannel("AO", -1, -1, -1, "Частота вращения"));

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_ATV":
                    properties.Add("IP", null);

                    parameters.Add("P_ON_TIME", null);
                    break;

                case "M_VIRT":
                    break;

                case "":
                    errStr = string.Format("\"{0}\" - не задан тип" +
                        " (M, M_FREQ, M_REV, M_REV_FREQ, M_REV_2," +
                        " M_REV_FREQ_2, M_REV_2_ERROR, M_REV_FREQ_2_ERROR, " +
                        "M_ATV, M_VIRT).\n", Name);
                    break;

                default:
                    errStr = string.Format("\"{0}\" - неверный тип" +
                        " (M, M_FREQ, M_REV, M_REV_FREQ, M_REV_2," +
                        " M_REV_FREQ_2, M_REV_2_ERROR, M_REV_FREQ_2_ERROR, " +
                        "M_ATV, M_VIRT).\n", Name);
                    break;
            }

            return errStr;
        }

        public override string GetDeviceSubTypeStr(DeviceType dt,
            DeviceSubType dst)
        {
            switch (dt)
            {
                case DeviceType.M:
                    switch (dst)
                    {
                        case DeviceSubType.M:
                            return "M";
                        case DeviceSubType.M_FREQ:
                            return "M_FREQ";
                        case DeviceSubType.M_REV:
                            return "M_REV";
                        case DeviceSubType.M_REV_FREQ:
                            return "M_REV_FREQ";
                        case DeviceSubType.M_REV_2:
                            return "M_REV_2";
                        case DeviceSubType.M_REV_FREQ_2:
                            return "M_REV_FREQ_2";
                        case DeviceSubType.M_REV_2_ERROR:
                            return "M_REV_2_ERROR";
                        case DeviceSubType.M_REV_FREQ_2_ERROR:
                            return "M_REV_FREQ_2_ERROR";
                        case DeviceSubType.M_ATV:
                            return "M_ATV";
                        case DeviceSubType.M_VIRT:
                            return "M_VIRT";
                    }
                    break;
            }

            return string.Empty;
        }

        public override Dictionary<string, int> GetDeviceProperties(
            DeviceType dt, DeviceSubType dst)
        {
            switch (dt)
            {
                case DeviceType.M:
                    switch (dst)
                    {
                        case DeviceSubType.M:
                            return new Dictionary<string, int>()
                            {
                                {"ST", 1},
                                {"M", 1},
                                {"P_ON_TIME", 1},
                            };

                        case DeviceSubType.M_FREQ:
                            return new Dictionary<string, int>()
                            {
                                {"ST", 1},
                                {"M", 1},
                                {"P_ON_TIME", 1},
                                {"V", 1},
                            };

                        case DeviceSubType.M_REV:
                        case DeviceSubType.M_REV_FREQ:
                        case DeviceSubType.M_REV_2:
                        case DeviceSubType.M_REV_FREQ_2:
                        case DeviceSubType.M_REV_2_ERROR:
                            return new Dictionary<string, int>()
                            {
                                {"ST", 1},
                                {"M", 1},
                                {"P_ON_TIME", 1},
                                {"V", 1},
                                {"R", 1},
                            };

                        case DeviceSubType.M_ATV:
                            return new Dictionary<string, int>()
                            {
                                {"M", 1},
                                {"ST", 1},
                                {"R", 1},
                                {"FRQ", 1},
                                {"RPM", 1},
                                {"EST", 1},
                                {"V", 1},
                                {"P_ON_TIME", 1},
                            };
                    }
                    break;
            }

            return null;
        }

        public override string Check()
        {
            string res = base.Check();

            if (ArticleName == string.Empty &&
                DeviceSubType == DeviceSubType.M_VIRT)
            {
                res += $"\"{name}\" - не задано изделие.\n";
            }

            return res;
        }
    }
}
