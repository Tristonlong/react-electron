using Common;
using Common.TestrongSystem;
using System.Threading;
using Modle.BasicClass;
using System.Collections.Generic;
using Common.TestrongIO;
using GpibIO;
using Modle.SystemEnum;
using Modle.FlowModels;
using Testrong.Core.Common;
using System.Linq;
using KeithleyInstruments.KeithleyDMM6500.Interop; //@@rita
using System; //@@rita
using System.IO;
using System.Text;
using System.Runtime.Remoting.Channels;
using ExternalCalibration;
using VISAKeysightDMM3458A;
using Modle.BisicTable;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Data;


namespace Testrong.User.Program
{




    public class UserProgram : TestrongCode
    {
        //private CapTrueCode capture = new CapTrueCode();
        // private TrimPatternCode trim = new TrimPatternCode();
        public override string GetProjectMessage()
        {
            EquipmentFactory.Set(new DummyWaferProberImpl());
            //Data.equipmentIo = new P12WaferProberImpl();//外设
            ProjectInfo projectInfo = new ProjectInfo();
            projectInfo.ProjectAuthor = "cyk";
            projectInfo.ProjectTime = "2022/07/09";
            projectInfo.ProjectClient = "test";
            projectInfo.ProjectDesCription = "Test";
            return projectInfo.GetProjectInfo();
        }
        public static string[] StdfItemName = {  "API_PMU_FVMV_EXT_PmuPinNames1","API_PMU_FVMV_EXT_PmuPinNames2","API_PMU_FVMV_EXT_PmuPinNames3","API_PMU_FVMV_EXT_PmuPinNames4", //0,1,2,3
                                                "API_PMU_FVMV_2mA_PmuPinNames1","API_PMU_FVMV_2mA_PmuPinNames2","API_PMU_FVMV_2mA_PmuPinNames3","API_PMU_FVMV_2mA_PmuPinNames4", //4,5,6,7
                                                "API_PMU_FVMV_200uA_PmuPinNames1","API_PMU_FVMV_200uA_PmuPinNames2","API_PMU_FVMV_200uA_PmuPinNames3","API_PMU_FVMV_200uA_PmuPinNames4", //8,9,10,11
                                                "API_PMU_FVMV_20uA_PmuPinNames1", "API_PMU_FVMV_20uA_PmuPinNames2", "API_PMU_FVMV_20uA_PmuPinNames3", "API_PMU_FVMV_20uA_PmuPinNames4", //12,13,14,15
                                                "API_PMU_FVMV_5uA_PmuPinNames1", "API_PMU_FVMV_5uA_PmuPinNames2", "API_PMU_FVMV_5uA_PmuPinNames3", "API_PMU_FVMV_5uA_PmuPinNames4", //16,17,18,19
                                                "API_PMU_FIMV_EXT_PmuPinNames1", "API_PMU_FIMV_EXT_PmuPinNames2", "API_PMU_FIMV_EXT_PmuPinNames3", "API_PMU_FIMV_EXT_PmuPinNames4", //20,21,22,23
                                                "API_PMU_FIMV_2mA_PmuPinNames1", "API_PMU_FIMV_2mA_PmuPinNames2", "API_PMU_FIMV_2mA_PmuPinNames3", "API_PMU_FIMV_2mA_PmuPinNames4", //24,25,26,27
                                                "API_PMU_FIMV_200uA_PmuPinNames1", "API_PMU_FIMV_200uA_PmuPinNames2", "API_PMU_FIMV_200uA_PmuPinNames3", "API_PMU_FIMV_200uA_PmuPinNames4", //28,29,30,31
                                                "API_PMU_FIMV_20uA_PmuPinNames1","API_PMU_FIMV_20uA_PmuPinNames2","API_PMU_FIMV_20uA_PmuPinNames3","API_PMU_FIMV_20uA_PmuPinNames4", //32,33,34,35
                                                "API_PMU_FIMV_5uA_PmuPinNames1","API_PMU_FIMV_5uA_PmuPinNames2","API_PMU_FIMV_5uA_PmuPinNames3","API_PMU_FIMV_5uA_PmuPinNames4", //36,37,38,39
                                                "API_PMU_per_pin", //40
                                                "API_DPS_FVMV_1A", //41
                                                "API_DPS_FVMV_60mA", //42
                                                "API_DPS_FVMV_2mA", //43
                                                "API_DPS_FVMV_200uA", //44
                                                "API_DPS_FVMV_20uA", //45
                                                "API_DPS_FVMV_5uA", //46
                                                "API_DPS_per_pin", //47
                                                "API_DPS_0V_1", //48
                                                "API_DPS_0V_2", //49
                                                "API_os_test", //50
                                                "API_Leakage_test" , //51
                                                "API_dps", //52
                                                "API_daq" ,//53
                                                "API_PMU_FVMV_EXT_Minus_PmuPinNames1","API_PMU_FVMV_EXT_Minus_PmuPinNames2","API_PMU_FVMV_EXT_Minus_PmuPinNames3","API_PMU_FVMV_EXT_Minus_PmuPinNames4",//54,55,56,57
                                                "API_PMU_FVMV_2mA_Minus_PmuPinNames1","API_PMU_FVMV_2mA_Minus_PmuPinNames2","API_PMU_FVMV_2mA_Minus_PmuPinNames3","API_PMU_FVMV_2mA_Minus_PmuPinNames4",//58,59,60,61
                                                "API_PMU_FVMV_200uA_Minus_PmuPinNames1","API_PMU_FVMV_200uA_Minus_PmuPinNames2","API_PMU_FVMV_200uA_Minus_PmuPinNames3","API_PMU_FVMV_200uA_Minus_PmuPinNames4",//62,63,64,65
                                                "API_PMU_FVMV_20uA_Minus_PmuPinNames1","API_PMU_FVMV_20uA_Minus_PmuPinNames2","API_PMU_FVMV_20uA_Minus_PmuPinNames3","API_PMU_FVMV_20uA_Minus_PmuPinNames4",//66,67,68,69
                                                "API_PMU_FVMV_5uA_Minus_PmuPinNames1","API_PMU_FVMV_5uA_Minus_PmuPinNames2","API_PMU_FVMV_5uA_Minus_PmuPinNames3","API_PMU_FVMV_5uA_Minus_PmuPinNames4",//70,71,72,73
                                                "API_PMU_FIMV_EXT_Minus_PmuPinNames1","API_PMU_FIMV_EXT_Minus_PmuPinNames2","API_PMU_FIMV_EXT_Minus_PmuPinNames3","API_PMU_FIMV_EXT_Minus_PmuPinNames4",//74,75,76,77
                                                "API_PMU_FIMV_2mA_Minus_PmuPinNames1","API_PMU_FIMV_2mA_Minus_PmuPinNames2","API_PMU_FIMV_2mA_Minus_PmuPinNames3","API_PMU_FIMV_2mA_Minus_PmuPinNames4",//78,79,80,81
                                                "API_PMU_FIMV_200uA_Minus_PmuPinNames1","API_PMU_FIMV_200uA_Minus_PmuPinNames2","API_PMU_FIMV_200uA_Minus_PmuPinNames3","API_PMU_FIMV_200uA_Minus_PmuPinNames4",//82,83,84,85
                                                "API_PMU_FIMV_20uA_Minus_PmuPinNames1","API_PMU_FIMV_20uA_Minus_PmuPinNames2","API_PMU_FIMV_20uA_Minus_PmuPinNames3","API_PMU_FIMV_20uA_Minus_PmuPinNames4",//86,87,88,89
                                                "API_PMU_FIMV_5uA_Minus_PmuPinNames1","API_PMU_FIMV_5uA_Minus_PmuPinNames2","API_PMU_FIMV_5uA_Minus_PmuPinNames3","API_PMU_FIMV_5uA_Minus_PmuPinNames4",//90,91,92,93
                                                "API_PMU_FVMI_5uA_PmuPinNames1", "API_PMU_FVMI_5uA_PmuPinNames2", "API_PMU_FVMI_5uA_PmuPinNames3", "API_PMU_FVMI_5uA_PmuPinNames4",//94,95,96,97
                                                "API_PMU_FVMI_20uA_PmuPinNames1","API_PMU_FVMI_20uA_PmuPinNames2","API_PMU_FVMI_20uA_PmuPinNames3","API_PMU_FVMI_20uA_PmuPinNames4",//98,99,100,101
                                                "API_PMU_FVMI_20uA_PmuPinNames1","API_PMU_FVMI_20uA_PmuPinNames2","API_PMU_FVMI_20uA_PmuPinNames3","API_PMU_FVMI_20uA_PmuPinNames4",//102,103,104,105
                                                "API_PMU_FVMI_2mA_PmuPinNames1","API_PMU_FVMI_2mA_PmuPinNames2","API_PMU_FVMI_2mA_PmuPinNames3","API_PMU_FVMI_2mA_PmuPinNames4",//106,107,108,109
                                                "API_PMU_FVMI_60mA_PmuPinNames1","API_PMU_FVMI_60mA_PmuPinNames2","API_PMU_FVMI_60mA_PmuPinNames3","API_PMU_FVMI_60mA_PmuPinNames4",//110,111,112,113
                                                "API_DPS_FVMV_1A",//114
                                                "API_DPS_FVMI_60mA",//115
                                                "API_DPS_FVMI_2mA",//116
                                                "API_DPS_FVMI_200uA",//117
                                                "API_DPS_FVMI_20uA",//118
                                                "API_DPS_FVMI_5uA",//119
                                                "API_DPS_FVMV_60mA_Minus",//120
                                                "API_DPS_FVMV_2mA_Minus",//121
                                                "API_DPS_FVMV_200uA_Minus",//122
                                                "API_DPS_FVMV_20uA_Minus",//123
                                                "API_DPS_FVMV_5uA_Minus",//124
                                                "API_DPS_FIMV_60mA",//125
                                                "API_DPS_FIMV_2mA",//126
                                                "API_DPS_FIMV_200uA",//127
                                                "API_DPS_FIMV_20uA",//128
                                                "API_DPS_FIMV_5uA",//129
                                                "API_DPS_FIMV_60mA_Minus",//130
                                                "API_DPS_FIMV_2mA_Minus",//131
                                                "API_DPS_FIMV_200uA_Minus",//132
                                                "API_DPS_FIMV_20uA_Minus",//133
                                                "API_DPS_FIMV_5uA_Minus",//134
                                                "API_VT_voltage_0P5V_PmuPinNames1","API_VT_voltage_0P5V_PmuPinNames2","API_VT_voltage_0P5V_PmuPinNames3","API_VT_voltage_0P5V_PmuPinNames4",//135,136,137,138
                                                "API_VT_voltage_1V_PmuPinNames1","API_VT_voltage_1V_PmuPinNames2","API_VT_voltage_1V_PmuPinNames3","API_VT_voltage_1V_PmuPinNames4",//139,140,141,142
                                                "API_VT_voltage_1P5V_PmuPinNames1","API_VT_voltage_1P5V_PmuPinNames2","API_VT_voltage_1P5V_PmuPinNames3","API_VT_voltage_1P5V_PmuPinNames4",//143,144,145,146
                                                "API_VT_voltage_2V_PmuPinNames1","API_VT_voltage_2V_PmuPinNames2","API_VT_voltage_2V_PmuPinNames3","API_VT_voltage_2V_PmuPinNames4",//147,148,149,150
                                                "API_VT_voltage_2P5V_PmuPinNames1","API_VT_voltage_2P5V_PmuPinNames2","API_VT_voltage_2P5V_PmuPinNames3","API_VT_voltage_2P5V_PmuPinNames4",//151,152,153,154
                                                "API_VT_voltage_3V_PmuPinNames1", "API_VT_voltage_3V_PmuPinNames2", "API_VT_voltage_3V_PmuPinNames3", "API_VT_voltage_3V_PmuPinNames4",//155,156,157,158
                                                "API_VT_current_FVMI_2mA_PmuPinNames1","API_VT_current_FVMI_2mA_PmuPinNames2","API_VT_current_FVMI_2mA_PmuPinNames3","API_VT_current_FVMI_2mA_PmuPinNames4",//159,160,161,162
                                                "API_VT_current_FVMI_200uA_PmuPinNames1","API_VT_current_FVMI_200uA_PmuPinNames2","API_VT_current_FVMI_200uA_PmuPinNames3","API_VT_current_FVMI_200uA_PmuPinNames4",//163,164,165,166


                                        };


        public override string[] GetTestItemName()
        {
            string[] testFunctionNames = {  "PMU_FVMV_EXT", "PMU_FVMV_2mA", "PMU_FVMV_200uA", "PMU_FVMV_20uA", "PMU_FVMV_5uA","PMU_FIMV_EXT", "PMU_FIMV_2mA",
            "PMU_FIMV_200uA","PMU_FIMV_20uA","PMU_FIMV_5uA", "PMU_per_pin","PMU_FVMV_EXT_Minus","PMU_FVMV_2mA_Minus","PMU_FVMV_200uA_Minus", "PMU_FVMV_20uA_Minus",
            "PMU_FVMV_5uA_Minus","PMU_FIMV_EXT_Minus","PMU_FIMV_2mA_Minus","PMU_FIMV_200uA_Minus","PMU_FIMV_20uA_Minus","PMU_FIMV_5uA_Minus",
            "DPS_FVMV_1A", "DPS_FVMV_80mA", "DPS_FVMV_2mA", "DPS_FVMV_200uA", "DPS_FVMV_20uA", "DPS_FVMV_5uA", "DPS_per_pin", "DPS_0V","DPS_FVMV_60mA_Minus", 
            "DPS_FVMV_2mA_Minus","DPS_FVMV_200uA_Minus","DPS_FVMV_20uA_Minus","DPS_FVMV_5uA_Minus","DPS_FIMV_60mA","DPS_FIMV_2mA","DPS_FIMV_200uA",
            "DPS_FIMV_20uA","DPS_FIMV_5uA","DPS_FIMV_60mA_Minus","DPS_FIMV_2mA_Minus","DPS_FIMV_200uA_Minus","DPS_FIMV_20uA_Minus","DPS_FIMV_5uA_Minus",
            "ram_Test", "ram_Calibration", "setVohVol", "Calibration", "RAM_PAT", "NOR_PAT","os_test", "ram_Test2","Leakage_test","Capture","trimpattern","ORD","readCPLD",
            "PMU_FVMI_2mA","PMU_FVMI_200uA","PMU_FVMI_20uA","PMU_FVMI_5uA","DPS_FVMI_60mA","DPS_FVMI_2mA","DPS_FVMI_200uA","DPS_FVMI_20uA","DPS_FVMI_5uA","VT_voltage_0P5V",
            "VT_voltage_1V","VT_voltage_1P5V","VT_voltage_2V","VT_voltage_2P5V","VT_voltage_3V","VT_current_FVMI_2mA","VT_current_FVMI_200uA","DMM6500_CTRL","SyncMeasureFVMV",
            "InitDMM", "CloseDMM", "DPS_17_Overnight", "DPS_17_Avg", "MeasureDMM_ADR425", "PMU1_Offset_Test_DMM", "PMU_1_OFFSET", "DPS_1_OFFSET", "Collect_DPS", "Collect_PMU",
            "PMU_Offset_Post_Check"};
            return testFunctionNames;
        }
        private IKeithleyDMM6500 dmmDriver;
        private const string DMM_BUFFER_NAME = "DMMBuffer";
        private void InitDMM(string IPAddress)
        {
            dmmDriver = new KeithleyDMM6500Class();
            try
            {
                dmmDriver.Initialize(IPAddress, false, false);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            if (dmmDriver.Initialized == true)
            {
                dmmDriver.Trigger.Abort();
            }
            //dmmDriver.Buffer.Delete(DMM_BUFFER_NAME);
            dmmDriver.Buffer.Create(DMM_BUFFER_NAME, 100);
        }
        private void MeasureDMM(KeithleyDMM6500FunctionEnum funcMode, KeithleyDMM6500FunctionsWithRangeEnum rangeMode, double range, int count)
        {
            dmmDriver.Function = funcMode;
            dmmDriver.Buffer.Clear(DMM_BUFFER_NAME);
            //dmmDriver.Buffer.Create(bufferName, 1000);
            dmmDriver.Measurement.Configuration.set_Range(rangeMode, range);
            dmmDriver.Measurement.Configuration.set_NPLC(KeithleyDMM6500NPLCFunctionsEnum.KeithleyDMM6500NPLCFunctionsVoltageDC, 1);
            dmmDriver.Measurement.Configuration.Count = count;
            dmmDriver.Buffer.TraceTrigger(DMM_BUFFER_NAME);
            Thread.Sleep(25);
        }
        private void CloseDMM()
        {
            dmmDriver.Buffer.Delete(DMM_BUFFER_NAME);
            dmmDriver.Close();
        }
        // private static List<ChannelInfo> CalcCurrentFOREACH(int count, daq daq, string[] pinNames, MeasureMode measureMode,double min,double max)
        // {
        //     var siteInfos = daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
        //     var channelInfos = siteInfos.SelectMany(t => t.ChannelInfoList.Where(c => pinNames.Contains(c.SignalName))).ToList();

        //     //daq.PostTextLog($"Sites: {string.Join(",",siteInfos.Select(t=>t.Site))}");
        //     //daq.PostTextLog($"channels: {string.Join(",", channelInfos.Select(t => t.Channel))}");

        //     channelInfos.ForEach(t => t.daq = 0);
        //     for (int i = 0; i < count; i++)
        //     {
        //         double[] values = Measure.GetMeasureBackChannelValue(measureMode, daq);
        //         for (int j = 0; j < channelInfos.Count; j++)
        //         {
        //             var channelInfo = channelInfos[j];
        //             int index = channelInfos[j].Channel - 1;
        //             // daq.PostTextLog( "dps: "+index.ToString()+" value: "+channelInfos[j].daq.ToString());
        //             channelInfos[j].daq = channelInfos[j].daq + values[index];
        //            // daq.PostPtr(channelInfos[j].Site+1, "testdps"+j, 99, max, min, values[index], "A");
        //            // channelInfos[j].daq = channelInfos[j].daq + values[index];
        //             daq.PostTextLog("dps: " + index.ToString() + " site: " + channelInfo.Site + " value: " + values[index]);  // 在txt中打印 各site 每次测试的值
        //             daq.PostPtr(channelInfo.Site + 1, $"testdps_{channelInfo.SignalName}_{i}", 100, max, min, values[index], "A");// 在csv和stdf中打印 各site 每次测试的值
        //         }

        //     }
        //     foreach (var item in channelInfos)
        //     {
        //         item.daq = item.daq / count;
        //         daq.PostTextLog("dps: " + (item.Channel-1).ToString() + " avg_value: " + item.daq);  // 在txt中打印 各site 均值
        //         daq.PostPtr(item.Site+1,$"testdpsaverage_{item.SignalName}",100, max,min, item.daq,"A");// 在csv和stdf中打印各site 均值
        //         if (item.daq<min|| item.daq>max)
        //         {
        //             item.Parent.LastSiteState = false;
        //             item.Parent.CurrentSiteState = false;
        //         }
        //     }
        //     return channelInfos;
        // }


        public override void TestCase(FlowDAQ daq, string name, int binCode)
        {

            string[] allPmuChannels = new string[64];
            for (int i = 0; i < 64; i++)
            {
                if (i < 9)
                {
                    allPmuChannels[i] = ("A" + 0) + (i + 1);
                }
                else
                {
                    allPmuChannels[i] = "A" + (i + 1);
                }
            }
            string[] pinName = new string[128];
            for (int i = 0; i < 128; i++)
            {
                if (i < 9)
                {
                    pinName[i] = ("A" + 0) + (i + 1);
                }
                else
                {
                    pinName[i] = "A" + (i + 1);
                }
            }
            string[] signalName = { "A01" };
            string[] signalNames = { "A02" };
            string[] PmuPinNames1 = {"A01","A05","A09","A13","A17","A21","A25","A29","A33","A37","A41","A45","A49","A53","A57","A61"};
            string[] PmuPinNames2 = {"A02","A06","A10","A14","A18","A22","A26","A30","A34","A38","A42","A46","A50","A54","A58","A62"};
            string[] PmuPinNames3 = {"A03","A07","A11","A15","A19","A23","A27","A31","A35","A39","A43","A47","A51","A55","A59","A63"};
            string[] PmuPinNames4 = {"A04","A08","A12","A16","A20","A24","A28","A32","A36","A40","A44","A48","A52","A56","A60","A64"};
            string[] pmuChannel1 = new string[16];
            string[] pmuChannel2 = new string[16];
            string[] pmuChannel3 = new string[16];
            string[] pmuChannel4 = new string[16];
            for (int i = 0; i < 64; i++)
            {
                if (i < 16)
                {
                    if (i < 9)
                    {
                        pmuChannel1[i] = ("A" + 0) + (i + 1);
                    }
                    else
                    {
                        pmuChannel1[i] = "A" + (i + 1);
                    }
                }
                else if (i < 32)
                {
                    pmuChannel2[i - 16] = "A" + (i + 1);
                }
                else if (i < 48)
                {
                    pmuChannel3[i - 32] = "A" + (i + 1);
                }
                else if (i < 64)
                {
                    pmuChannel4[i - 48] = "A" + (i + 1);
                }
            }
            void DpsForce0V()
            {

                Dps.GetDpsAllForceCmd(DpsMode.FVMV_All, 0.0, DpsRange._5uA, true, daq);

            }

            /*
            int[] allDpsChannels = new int[64];
            
            for (int i = 0;i < 64; i++)
            {
                allDpsChannels[i] = i + 1;
            }
            */

            string[] allDpsPins = new string[8];

            for (int i = 0; i < 8; i++)
            {
                allDpsPins[i] = "DPS" + (i + 1);
            }

            string[] pinname = { "A01", "A02" };
            string[] pinnames = { "A02" };
            string[] pmuPins1 = new string[16];
            string[] pmuPins2 = new string[16];
            string[] pmuPins3 = new string[16];
            string[] pmuPins4 = new string[16];
            for (int i = 0; i < 64; i++)
            {
                if (i < 16)
                {
                    if (i < 9)
                    {
                        pmuPins1[i] = ("A" + 0) + (i + 1);
                    }
                    else
                    {
                        pmuPins1[i] = "A" + (i + 1);
                    }
                }
                else if (i < 32)
                {
                    pmuPins2[i - 16] = "A" + (i + 1);
                }
                else if (i < 48)
                {
                    pmuPins3[i - 32] = "A" + (i + 1);
                }
                else
                {
                    pmuPins4[i - 48] = "A" + (i + 1);
                }
            }

            // string[] norPattern = {  "NOR_22"/* "NOR_23", "NOR_30", "NOR_31", "NOR_32", "NOR_33","NOR_34", "NOR_40", "NOR_41", "NOR_42", "NOR_43",
            //"NOR_44", "NOR_45", "NOR_46", "NOR_47", "NOR_48", "NOR_49",/* "NOR_50", "NOR_51", "NOR_52", "NOR_401", "NOR_402", "NOR_403", "NOR_409", "NOR_410", 
            //"NOR_1010", "NOR_1011", "NOR_1012", "NOR_1024", "NOR_1025", "NOR_1026", "NOR_1027", "NOR_1031", "NOR_3000", "NOR_3001", "NOR_3002", "NOR_3003", 
            //"NOR_3006", "NOR_3007", "NOR_3008", "NOR_3010", "NOR_4011", "NOR_4013", "NOR_4096", "NOR_4097", "NOR_4098", "NOR_4099", "NOR_4100", "NOR_40101" */};

            string[] norPatterns = { "NOR_20_10M_50Ln","NOR_21_5M_28Ln","NOR_22_2M_24Ln","NOR_23_1M_24Ln","NOR_30_500K_71Ln","NOR_31_200K_90Ln","NOR_32_200K_70Ln",
            "NOR_33_3M_70Ln","NOR_34_300K_70Ln","NOR_40_15M_7WLn","NOR_41_20M_7WLn","NOR_42_30M_37WLn","NOR_43_40M_7WLn","NOR_44_45M_7WLn","NOR_45_30M_12WLn","NOR_46_40M_40WLn",
            "NOR_47_35M_21WLn","NOR_48_25M_7WLn","NOR_49_20M_37WLn","NOR_50_15M_7WLn","NOR_51_10M_7WLn","NOR_52_8M_37WLn","NOR_401_5M_4KLn","NOR_402_2M_4KLn","NOR_403_1M_4KLn",
            "NOR_410_500K_4KLn","NOR_1010_200K_7KLn","NOR_1011_40M_7KLn","NOR_1012_20M_7KLn","NOR_1025_10M_7KLn","NOR_1026_200K_7KLn","NOR_1027_200K_7KLn","NOR_1031_45M_7KLn","NOR_3000_40M_3KLn",
            "NOR_3001_200K_3KLn","NOR_3002_5M_3KLn","NOR_3003_10M_3KLn","NOR_3006_1M_3KLn","NOR_3007_20M_3KLn","NOR_3008_30M_3KLn","NOR_3010_15M_3KLn","NOR_4011_25M_3KLn","NOR_4013_200K_3KLn",
            "NOR_4096_20M_3KLn","NOR_4097_8M_3KLn","NOR_4098_500K_3KLn","NOR_4099_200K_3KLn","NOR_4100_5M_3KLn","NOR_40101_2M_3KLn"};
             double[] norPatternsAcTiming = {100,200,500,1000,2000,5000,5000,300,3000,66,50,100,125,200,500,1000,2000,5000,
             25,50,100,5000,5000,22,25,5000,200,100,1000,45,33,66,40,5000,50,125,2000,1000,200,500};

             //double[] norPatternsAcTiming = { 200, 200, 1000, 200 , 100, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200/*, 200, 200, 200, 200,
           // 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200 */};
            double[] VIH = { 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5/*,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2*/};
            double[] VIH2 = { 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3/*,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2,5,3,2*/};
            switch (name)
            {
                case "PMU_FVMV_EXT":
                    {
                        // daq.PostTextLog("PMU_FVMV_EXT\n");

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq);

                        // Thread.Sleep(10);
                        // /*
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // */
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        daq.PostTextLog("PMU_FVMV_EXT\n");
                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq,StdfItemName[0]);
                        
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq,StdfItemName[1]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq,StdfItemName[2]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 3.0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.03, true, daq,StdfItemName[3]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);



                    }
                    break;

                case "PMU_FVMV_2mA":
                    {
                        // daq.PostTextLog("PMU_FVMV_2mA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 3.3, PpmuRange._2mA, true, daq);
                        // Thread.Sleep(10);
                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_2mA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_2mA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_2mA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_2mA, 3.27, 3.33, true, daq);
                        // Thread.Sleep(10);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        daq.PostTextLog("PMU_FVMV_2mA\n");
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._2mA,true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 3.0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_2mA, 2.97, 3.03, true, daq,StdfItemName[4]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 3.0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_2mA, 2.97, 3.03, true, daq,StdfItemName[5]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 3.0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_2mA, 2.97, 3.03, true, daq,StdfItemName[6]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 3.0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_2mA, 2.97, 3.03, true, daq,StdfItemName[7]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);
                        

                    }
                    break;
                case "PMU_FVMV_200uA":
                    {
                        daq.PostTextLog("PMU_FVMV_200uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 3.3, PpmuRange._200uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_200uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_200uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_200uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_200uA, 3.27, 3.33, true, daq);
                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);




                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 3.0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 2.97, 3.03, true, daq,StdfItemName[8]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 3.0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 2.97, 3.03, true, daq,StdfItemName[9]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 3.0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 2.97, 3.03, true, daq,StdfItemName[10]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 3.0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 2.97, 3.03, true, daq,StdfItemName[11]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FVMV_20uA":
                    {
                        daq.PostTextLog("PMU_FVMV_20uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 3.3, PpmuRange._20uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_20uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_20uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_20uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_20uA, 3.27, 3.33, true, daq);
                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);



                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 3.0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_20uA, 2.97, 3.03, true, daq,StdfItemName[12]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 3.0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_20uA, 2.97, 3.03, true, daq,StdfItemName[13]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 3.0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_20uA, 2.97, 3.03, true, daq,StdfItemName[14]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 3.0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_20uA, 2.97, 3.03, true, daq,StdfItemName[15]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                        



                    }
                    break;
                case "PMU_FVMV_5uA":
                    {
                        daq.PostTextLog("PMU_FVMV_5uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 3.3, PpmuRange._5uA, true, daq);

                        // Thread.Sleep(10);
                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_5uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_5uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_5uA, 3.27, 3.33, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_5uA, 3.27, 3.33, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);



                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 3.0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_5uA, 2.97, 3.03, true, daq,StdfItemName[16]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 3.0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_5uA, 2.97, 3.03, true, daq,StdfItemName[17]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                       
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 3.0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_5uA, 2.97, 3.03, true, daq,StdfItemName[18]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 3.0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_5uA, 2.97, 3.03, true, daq,StdfItemName[19]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true,daq);

                    }
                    break;
                case "PMU_FIMV_EXT":
                    {
                        daq.PostTextLog("PMU_FIMV_EXT\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);



                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq,StdfItemName[20]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq,StdfItemName[21]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq,StdfItemName[22]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq,StdfItemName[23]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FIMV_2mA":
                    {
                        daq.PostTextLog("PMU_FIMV_2mA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_2mA, 5.0, 5.2, true, daq,StdfItemName[24]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_2mA, 5.0, 5.2, true, daq,StdfItemName[25]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_2mA, 5.0, 5.2, true, daq,StdfItemName[26]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_2mA, 5.0, 5.2, true, daq,StdfItemName[27]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);


                    }
                    break;
                case "PMU_FIMV_200uA":
                    {
                        daq.PostTextLog("PMU_FIMV_200uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);


                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 5.0, 5.2, true, daq,StdfItemName[28]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 5.0, 5.2, true, daq,StdfItemName[29]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 5.0, 5.2, true, daq,StdfItemName[30]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 5.0, 5.2, true, daq,StdfItemName[31]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FIMV_20uA":
                    {
                        daq.PostTextLog("PMU_FIMV_20uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_20uA, 5.0, 5.2, true, daq,StdfItemName[32]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_20uA, 5.0, 5.2, true, daq,StdfItemName[33]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_20uA, 5.0, 5.2, true, daq,StdfItemName[34]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_20uA, 5.0, 5.2, true, daq,StdfItemName[35]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);


                    }
                    break;
                case "PMU_FIMV_5uA":
                    {
                        daq.PostTextLog("PMU_FIMV_5uA\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, 5.0, 5.2, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_5uA, 5.0, 5.2, true, daq,StdfItemName[36]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_5uA, 5.0, 5.2, true, daq,StdfItemName[37]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_5uA, 5.0, 5.2, true, daq,StdfItemName[38]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_5uA, 5.0, 5.2, true, daq,StdfItemName[39]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);


                    }
                    break;

                case "PMU_per_pin":
                    {
                        daq.PostTextLog("PMU_per_pin\n");
                        Ppmu.GetPpmuAllForceCmd(PpmuMode.FVMV_all, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        for (int i = 0; i < 64; i++)
                        {
                            Ppmu.GetPpmuAllForceCmd(PpmuMode.FVMV_all, 0, PpmuRange._2mA, true, daq);
                            Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                            string[] temp = { allPmuChannels[i] };
                            Ppmu.GetPpmuPinNameForceCmd(temp, PpmuMode.FVMV, 3, PpmuRange._60mA, true, daq);
                            string[] fpgasignal;
                            if (i != 63)
                            {
                                fpgasignal = new string[] { allPmuChannels[i + 1] };
                            }
                            else
                            {
                                fpgasignal = new string[] { allPmuChannels[i - 1] };

                            }
                            Relay.GetRelayCmdByPinName(temp, fpgasignal, true, daq);
                            Measure.GetMeasureValueByPinName(temp, MeasureMode.IO_CallBack_Voltage_60mA, 2.97, 3.05, true, daq,StdfItemName[40]);
                        }

                        Ppmu.GetPpmuAllForceCmd(PpmuMode.FVMV_all, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);
                    }
                    break;

                case "PMU_FVMV_EXT_Minus":
                    {
                        daq.PostTextLog("PMU_FVMV_EXT_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.27, true, daq);

                        // Thread.Sleep(10);
                        // /*
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        // */
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        daq.PostTextLog("PMU_FVMV_EXT_Minus\n");
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.25, true, daq,StdfItemName[54]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.25, true, daq,StdfItemName[55]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.25, true, daq,StdfItemName[56]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, -3.3, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_60mA, -3.33, -3.25, true, daq,StdfItemName[57]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;

                case "PMU_FVMV_2mA_Minus":
                    {
                        daq.PostTextLog("PMU_FVMV_2mA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        // Thread.Sleep(10);
                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.27, true, daq);
                        // Thread.Sleep(10);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.25, true, daq,StdfItemName[58]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_2mA,-3.33, -3.25, true, daq,StdfItemName[59]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.25, true, daq,StdfItemName[60]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, -3.3, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_2mA, -3.33, -3.25, true, daq,StdfItemName[61]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FVMV_200uA_Minus":
                    {
                        daq.PostTextLog("PMU_FVMV_200uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.27, true, daq);
                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.25, true, daq,StdfItemName[62]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV,-3.3, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA,-3.33, -3.25, true, daq,StdfItemName[63]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.25, true, daq,StdfItemName[64]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, -3.3, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, -3.33, -3.25, true, daq,StdfItemName[65]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FVMV_20uA_Minus":
                    {
                        daq.PostTextLog("PMU_FVMV_20uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_20uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_20uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_20uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_20uA, -3.33, -3.27, true, daq);
                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_20uA, -3.33, -3.25, true, daq,StdfItemName[66]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, -3.3, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_20uA,  -3.33, -3.25, true, daq,StdfItemName[67]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV,-3.3, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_20uA,  -3.33, -3.25, true, daq,StdfItemName[68]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV,-3.3, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_20uA,  -3.33, -3.25, true, daq,StdfItemName[69]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FVMV_5uA_Minus":
                    {
                        daq.PostTextLog("PMU_FVMV_5uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);

                        // Thread.Sleep(10);
                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.27, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.27, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);



                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.25, true, daq,StdfItemName[70]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.25, true, daq,StdfItemName[71]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                       
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_5uA,-3.33, -3.25, true, daq,StdfItemName[72]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, -3.3, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_5uA, -3.33, -3.25, true, daq,StdfItemName[73]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true,daq);
                    }
                    break;
                case "PMU_FIMV_EXT_Minus":
                    {
                        daq.PostTextLog("PMU_FIMV_EXT_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, -0.03,PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq,StdfItemName[74]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq,StdfItemName[75]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV,-0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq,StdfItemName[76]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, -0.03, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_60mA,-5.2, -4.8, true, daq,StdfItemName[77]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._60mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);


                    }
                    break;
                case "PMU_FIMV_2mA_Minus":
                    {
                        daq.PostTextLog("PMU_FIMV_2mA_Minus\n");
                    //     Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                    //     Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);

                    //     Thread.Sleep(10);

                    //     Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                    //     Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                    //     Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                    //     Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                    //     Thread.Sleep(10);

                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                    //     Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                    //     Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                    //     Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_2mA,-5.2, -4.8, true, daq,StdfItemName[78]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_2mA, -5.2, -4.8, true, daq,StdfItemName[79]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_2mA, -5.2, -4.8, true, daq,StdfItemName[80]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, -0.001, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_2mA,-5.2, -4.8,  true, daq,StdfItemName[81]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FIMV_200uA_Minus":
                    {
                            daq.PostTextLog("PMU_FIMV_200uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);


                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);




                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, -5.2, -4.8, true, daq,StdfItemName[82]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, -5.2, -4.8, true, daq,StdfItemName[83]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, -5.2, -4.8, true, daq,StdfItemName[84]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, -0.0001, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, -5.2, -4.8, true, daq,StdfItemName[85]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._200uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);


                    }
                    break;
                case "PMU_FIMV_20uA_Minus":
                    {
                        daq.PostTextLog("PMU_FIMV_20uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);

                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);




                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_20uA, -5.2, -4.8, true, daq,StdfItemName[86]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_20uA, -5.2, -4.8, true, daq,StdfItemName[87]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_20uA, -5.2, -4.8, true, daq,StdfItemName[88]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, -0.00001, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_20uA,-5.2, -4.8, true, daq,StdfItemName[89]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._20uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);

                    }
                    break;
                case "PMU_FIMV_5uA_Minus":
                    {
                        daq.PostTextLog("PMU_FIMV_5uA_Minus\n");
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        // Thread.Sleep(10);

                        // Measure.GetMeasureValueByPinName(pmuChannel1, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel2, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel3, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);
                        // Measure.GetMeasureValueByPinName(pmuChannel4, MeasureMode.IO_CallBack_Voltage_60mA, -5.2, -4.8, true, daq);

                        // Thread.Sleep(10);

                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel1, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel2, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel3, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);
                        // Ppmu.GetPpmuPinNameForceCmd(pmuChannel4, PpmuMode.FIMV, 0, PpmuRange._2mA, true, daq);

                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        // Ppmu.ReadPpmuTemp(true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_5uA, -5.2, -4.8, true, daq,StdfItemName[90]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_5uA,-5.2, -4.8, true, daq,StdfItemName[91]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_5uA, -5.2, -4.8, true, daq,StdfItemName[92]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, -0.000001, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_5uA, -5.2, -4.8, true, daq,StdfItemName[93]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FIMV, 0, PpmuRange._5uA, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true, daq);



                    }
                    break;

                    case "PMU_FVMI_5uA":
                    {
                        daq.PostTextLog("PMU_FVMI_5uA\n");
                    
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_5uA, -0.00000004, 0.00000004, true, daq,StdfItemName[94]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_5uA,  -0.00000004, 0.00000004, true, daq,StdfItemName[95]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                       
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_5uA,  -0.00000004, 0.00000004, true, daq,StdfItemName[96]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._5uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._5uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_5uA, -0.00000004, 0.00000004, true, daq,StdfItemName[97]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._5uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Ppmu.ReadPpmuTemp(true,daq);

                    }
                    break;

                    case "PMU_FVMI_20uA":
                    {

                        daq.PostTextLog("PMU_FVMI_20uA\n");
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_20uA, -0.00000016, 0.00000016, true, daq,StdfItemName[98]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_20uA, -0.00000016, 0.00000016, true, daq,StdfItemName[99]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_20uA, -0.00000016, 0.00000016, true, daq,StdfItemName[100]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._20uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._20uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_20uA, -0.00000016, 0.00000016, true, daq,StdfItemName[101]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._20uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                    }
                    break;


                case "PMU_FVMI_200uA": 
                {
                        daq.PostTextLog("PMU_FVMI_200uA\n");
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_200uA, -0.0000016, 0.0000016, true, daq,StdfItemName[102]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_200uA, -0.0000016, 0.0000016, true, daq,StdfItemName[103]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_200uA, -0.0000016, 0.0000016, true, daq,StdfItemName[104]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_200uA, -0.0000016, 0.0000016, true, daq,StdfItemName[105]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._200uA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);




                }    
                break;


                case "PMU_FVMI_2mA": 
                {
                        daq.PostTextLog("PMU_FVMI_2mA\n");

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_2mA, -0.000016, 0.000016, true, daq,StdfItemName[106]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);


                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_2mA, -0.000016, 0.000016, true, daq,StdfItemName[107]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_2mA, -0.000016, 0.000016, true, daq,StdfItemName[108]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_2mA, -0.000016, 0.000016, true, daq,StdfItemName[109]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._2mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);





                }
                break;

                
                case "PMU_FVMI_60mA": 
                {
                        daq.PostTextLog("PMU_FVMI_60mA\n");
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_60mA,-0.00048, 0.00048, true, daq,StdfItemName[110]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_60mA, -0.00048, 0.00048, true, daq,StdfItemName[111]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_60mA, -0.00048, 0.00048, true, daq,StdfItemName[112]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);

                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._60mA, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._60mA, true, daq);
                        Thread.Sleep(10);
                        Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_60mA, -0.00048, 0.00048, true, daq,StdfItemName[113]);
                        Thread.Sleep(10);
                        Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4,PpmuMode.FVMV,0,PpmuRange._60mA,true,daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);



                };
                break;


                case "DPS_FVMI_1A":
                {
                        daq.PostTextLog("DPS_FVMI_1A\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.EXT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._1A, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_1A, -0.006, 0.006, true, daq,StdfItemName[114]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);

                
                }
                break;

                case "DPS_FVMI_60mA":
                {
                        daq.PostTextLog("DPS_FVMI_60mA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState._60MA, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._60mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_60mA, -0.0006, 0.0006, true, daq,StdfItemName[115]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);


                }
                break;

                case "DPS_FVMI_2mA":
                {
                        daq.PostTextLog("DPS_FVMI_2mA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._2mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_2mA, -0.000016, 0.000016, true, daq,StdfItemName[116]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);


                }
                break;

                case "DPS_FVMI_200uA":
                {
                        daq.PostTextLog("DPS_FVMI_200uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._200uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_200uA, -0.0000016, 0.0000016, true, daq,StdfItemName[117]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);


                }
                break;


                case "DPS_FVMI_20uA":
                {
                        daq.PostTextLog("DPS_FVMI_20uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._20uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_20uA, -0.00000016, 0.00000016, true, daq,StdfItemName[118]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);


                }
                break;


                case "DPS_FVMI_5uA":
                {
                        daq.PostTextLog("DPS_FVMI_5uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0, DpsRange._5uA ,true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_5uA, -0.00000004, 0.00000004, true, daq,StdfItemName[119]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);


                }
                break;

                case "DPS_FVMV_1A":
                    {
                        daq.PostTextLog("DPS_FVMV_1A\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.EXT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 3.3, DpsRange._1A, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_1A, 3.27, 3.33, true, daq,StdfItemName[41]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_60mA":
                    {
                        daq.PostTextLog("DPS_FVMV_60mA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState._60MA, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 7.999, DpsRange._60mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_60mA, 7.97, 8.03, true, daq,StdfItemName[42]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_2mA":
                    {
                        daq.PostTextLog("DPS_FVMV_2mA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 3.3, DpsRange._2mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_2mA, 3.27, 3.33, true, daq,StdfItemName[43]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_200uA":
                    {
                        daq.PostTextLog("DPS_FVMV_200uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 3.3, DpsRange._200uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_200uA, 3.27, 3.33, true, daq,StdfItemName[44]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_20uA":
                    {
                        daq.PostTextLog("DPS_FVMV_20uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 3.3, DpsRange._20uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_20uA, 3.27, 3.33, true, daq,StdfItemName[45]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_5uA":
                    {
                        daq.PostTextLog("DPS_FVMV_5uA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, 3.3, DpsRange._5uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_5uA, 3.27, 3.33, true, daq,StdfItemName[46]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);

                    }
                    break;
                case "DPS_FVMV_60mA_Minus":
                    {
                        daq.PostTextLog("DPS_FVMV_60mA_Minus\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState._60MA, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, -7.999, DpsRange._60mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_60mA, -8.2, -7, true, daq,StdfItemName[120]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_2mA_Minus":
                    {
                        daq.PostTextLog("DPS_FVMV_2mA_Minus\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, -3.3, DpsRange._2mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_2mA, -3.33, -3.27, true, daq,StdfItemName[121]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_200uA_Minus":
                    {
                        daq.PostTextLog("DPS_FVMV_200uA_Minus\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, -3.3, DpsRange._200uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_200uA, -3.33, -3.23, true, daq,StdfItemName[122]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_20uA_Minus":
                    {
                        daq.PostTextLog("DPS_FVMV_20uA_Minus\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, -3.3, DpsRange._20uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_20uA, -3.33, -3.23, true, daq,StdfItemName[123]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);
                    }
                    break;

                case "DPS_FVMV_5uA_Minus":
                    {
                        daq.PostTextLog("DPS_FVMV_5uA_Minus\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMV, -3.3, DpsRange._5uA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_5uA, -3.33, -3.23, true, daq,StdfItemName[124]);
                        Thread.Sleep(1);
                        DpsForce0V();
                        Dps.ReadDpsTemp(true, daq);

                    }
                    break;

                case "DPS_per_pin":
                    {
                        daq.PostTextLog("DPS_per_pin\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);

                        for (int i = 0; i < 8; i++)
                        {
                            string[] temp = { allDpsPins[i] };
                            Dps.GetDpsPinNameForceCmd(temp, DpsMode.FVMV, 3.0, DpsRange._200uA, true, daq);
                            Thread.Sleep(10);
                            Measure.GetMeasureValueByPinName(temp, MeasureMode.Supply_Voltage_200uA, 2.97, 3.07, true, daq,StdfItemName[47]);
                            DpsForce0V();
                            Dps.ReadDpsTemp(true, daq);
                        }
                    }
                    break;

                case "DPS_0V":
                    {

                        daq.PostTextLog("DPS_0V\n");
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FVMI, 0.0, DpsRange._200uA, true, daq);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_200uA, -0.0001, 0.0001, true, daq,StdfItemName[48]);
                        DpsForce0V();
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Leakage_200uA, -0.02, 0.02, true, daq,StdfItemName[49]);
                        Dps.ReadDpsTemp(true, daq);

                    }

                    break;
                /*   case "DPS_FIMV_1A":
                    {
                        daq.PostTextLog("DPS_FIMV_1A\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.EXT, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FIMV, 0.08, DpsRange._1A, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_1A, 3.27, 3.33, true, daq);
                        Thread.Sleep(1);
                        DpsForce0V();
                    }
                    break;
                    */
                case "DPS_FIMV_60mA":
                    {
                        daq.PostTextLog("DPS_FIMV_60mA\n");
                        DpsForce0V();
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState._60MA, daq);
                        Thread.Sleep(1);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FIMV, 0.048, DpsRange._60mA, true, daq);
                        Thread.Sleep(1);
                        Measure.GetMeasureValueByPinName(allDpsPins, MeasureMode.Supply_Voltage_60mA, 9.5, 10.5, true, daq,StdfItemName[125]);   
                        }

                    break;
                //         Thread.Sleep(10);
                //         int[] siteValue = CaptureMethodClass.CatchNumberMethod("RAM_20", "A02", "Cap1", daq);
                //         // int[] siteValue1 = CaptureMethodClass.CatchNumberMethod("RAM_20", "A01", "Cap1", daq);
                //         List<SiteInfo> siteInfos = daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
                //         for (int siteIndex = 0; siteIndex < siteInfos.Count; siteIndex++)
                //         {
                //             int site = siteInfos[siteIndex].Site;
                //             daq.PostTextLog($"site:{site}:value{siteValue[siteIndex]}\n");//打印
                //         }


                //     }
                //     break;



                // case "trimpattern":
                    // {
                        // int[] blocknumberas = { 10, 11, 12, 13, 14 };
                        // //string[] pin = {"A01"};
                        // //int trimValue1;
                        // DcLevel.GetDcLevelCmd(5, 0, true, daq);
                        // AcTimming.GetAcTimmingCmd(300, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        // for (int block = 0; block <= 31; block++)
                        // {
                        //     List<SiteInfo> siteInfos = daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
                        //    // List<SiteInfo> siteInfos = daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
                        //     for (int siteIndex = 0; siteIndex < siteInfos.Count; siteIndex++)
                        //     {
                        //         ClearError.ClearErrorMemory(daq);
                        //         int site = siteInfos[siteIndex].Site;
                        //         //trimValue1 = block;                                    
                        //         TrimPatternCode.TrimChangeSingleColumn("ram_1", "A01", site, blocknumberas, block, daq);
                        //         TrimPatternCode.SendTrimInfo("ram_1", blocknumberas, daq);
                        //         TrimPatternCode.StartPattern("ram_1", daq);
                        //         PatternStatus.CheckPatternStatus(daq);
                        //         Thread.Sleep(10);
                        //         // TrimPatternCode.StartPattern("ram_1",daq);
                        //         // PatternStatus.CheckPatternStatus(daq);
                        //         ClearError.ClearErrorMemory(daq);





                                
                    //         }


                    //     }
                    // }
                    // break;
                // case "ORD":
                //     {
                //         string[] A01 = { "A01" };
                //         string[] A02 = { "A02" };
                //         TrimPatternCode.StartPattern("ORD", daq);
                //         System.Threading.Thread.Sleep(50);
                //         ReadPatterndaq readpattern = ReadPatternMethodClass.ReadErrorMemory("ORD", A01, daq, 1);
                //         daq.PostTextLog(readpattern.ErrorLog);
                //         ReadFrequency.GetFreqMeasureValueByPinName(A01, 20000, 10, 0, 1000000, daq);
                //         ClearError.ClearErrorMemory(daq);


                //     }
                //     break;
                //case "readCPLD":
                //    {
                //        ReadHardwareVersion.ReadCPLDVersion(daq, true);
                //        ReadHardwareVersion.ReadArmVersion(daq, true);

                //    }
                //    break;
                case "DPS13":
                    {
                        int[] channels = new int[] { };
                        DpsRangeRelay.SetDpsRangeRelayByChannels(channels, DpsRelayState._60MA, daq);
                        Dps.GetDpsChannelForceCmd(channels, DpsMode.FVMV, 3.3, DpsRange._60mA, true, daq);
                    }
                    break;

                case "VT_voltage_0P5V":
                {
                daq.PostTextLog("VT_voltage_0P5V\n");
              //  NewRelay.GetRelayCmdByPinNameNew(PmuPinNames1, StatusFlag.SHORT,true,daq,false);

                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT, true, daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 0.4, 0.6, true, daq,StdfItemName[135]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 0.4, 0.6, true, daq,StdfItemName[136]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);   


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 0.4, 0.6, true, daq,StdfItemName[137]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 0.4, 0.6, true, daq,StdfItemName[138]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;
                case "VT_voltage_1V":
                {
                daq.PostTextLog("VT_voltage_1V\n");
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 0.8, 1.1, true, daq,StdfItemName[139]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 0.8, 1.1, true, daq,StdfItemName[140]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 0.8, 1.1, true, daq,StdfItemName[141]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 0.8, 1.1, true, daq,StdfItemName[142]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;

                case "VT_voltage_1P5V":
                {
                daq.PostTextLog("VT_voltage_1P5V\n");
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 1.4, 1.6, true, daq,StdfItemName[143]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA,  1.4, 1.6, true, daq,StdfItemName[144]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA,  1.4, 1.6, true, daq,StdfItemName[145]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA,  1.4, 1.6, true, daq,StdfItemName[146]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;


                case "VT_voltage_2V":
                {
                daq.PostTextLog("VT_voltage_2V\n");    
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 1.9, 2.1, true, daq,StdfItemName[147]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA,  1.9, 2.1, true, daq,StdfItemName[148]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);   


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA,  1.9, 2.1, true, daq,StdfItemName[149]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 1.9, 2.1, true, daq,StdfItemName[150]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;

                case "VT_voltage_2P5V":
                {
                daq.PostTextLog("VT_voltage_2P5V\n");
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 2.4, 2.6, true, daq,StdfItemName[151]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 2.4, 2.6, true, daq,StdfItemName[152]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);   


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 2.4, 2.6, true, daq,StdfItemName[153]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 2.4, 2.6, true, daq,StdfItemName[154]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;


                case "VT_voltage_3V":
                {            
                daq.PostTextLog("VT_voltage_2V\n");
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Voltage_200uA, 2.9, 3.1, true, daq,StdfItemName[155]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Voltage_200uA, 2.9, 3.1, true, daq,StdfItemName[156]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);   


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Voltage_200uA, 2.9, 3.1, true, daq,StdfItemName[157]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FZMV, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Voltage_200uA, 2.9, 3.1, true, daq,StdfItemName[158]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;

                

                case "VT_current_FVMI_2mA":
                {
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_2mA, -0.0007 ,-0.0005, true, daq,StdfItemName[159]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_2mA, -0.0007 ,-0.0005, true, daq,StdfItemName[160]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_2mA, -0.0007 ,-0.0005, true, daq,StdfItemName[161]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._2mA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_2mA, -0.0007 ,-0.0005,true, daq,StdfItemName[162]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._2mA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                }
                break;


                case "VT_current_FVMI_200uA":
                {
                Relay.GetRelayCmdByPinName(PmuPinNames1, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames1, MeasureMode.IO_CallBack_Current_200uA, -0.000105 ,-0.000085, true, daq,StdfItemName[163]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames1, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);

                Relay.GetRelayCmdByPinName(PmuPinNames2, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames2, MeasureMode.IO_CallBack_Current_200uA, -0.000105 ,-0.000085, true, daq,StdfItemName[164]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames2, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames3, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames3, MeasureMode.IO_CallBack_Current_200uA,-0.000105 ,-0.000085, true, daq,StdfItemName[165]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames3, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);


                Relay.GetRelayCmdByPinName(PmuPinNames4, RelayState.SHORT,true,daq);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMI, 0, PpmuRange._200uA, true, daq);
                Thread.Sleep(10);
                Measure.GetMeasureValueByPinName(PmuPinNames4, MeasureMode.IO_CallBack_Current_200uA, -0.000105 ,-0.000085,true, daq,StdfItemName[166]);
                Thread.Sleep(10);
                Ppmu.GetPpmuPinNameForceCmd(PmuPinNames4, PpmuMode.FVMV, 0, PpmuRange._200uA, true, daq);
                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                Thread.Sleep(10);               

                }
                break;

                case "RAM_PAT":
                    {
                        ClearAllError.ClearErrorMemory(daq);
                        ClearError.ClearErrorMemory(daq);
                        //AcTimming.GetAcTimmingCmd(200.0, true, daq);
                        // DcLevel.GetDcLevelCmd(5.0, 0.0, true, daq);

                        AcTimming.GetAcTimmingCmd(200.0, true, daq);
                        //DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        TrimPatternCode.StartPattern("ram_5M_1KLn", daq);//启动 ram pattern ADC
                        daq.PostTextLog("Run ram_5M_1KLn End\n");//UI打印
                        PatternStatus.CheckPatternStatus(daq);
                       // ReadPatternresult readpattern = ReadPatternMethodClass.ReadErrorMemory("ram_5M_1KLn", signalNames, daq, 100);
                        ReadPatternResult readpattern = ReadPatternMethodClass.ReadErrorMemory("ram_5M_1KLn", signalNames, daq, 100);
                        daq.PostTextLog(readpattern.ErrorLog);
                        ClearError.ClearErrorMemory(daq);

                        Thread.Sleep(100);
                        AcTimming.GetAcTimmingCmd(50, true, daq);
                        TrimPatternCode.StartPattern("ram_20M_250Ln", daq);//启动 ram pattern ADC
                        Thread.Sleep(100);
                        daq.PostTextLog("Run ram_20M_250Ln End\n");//UI打印
                       // PatternStatus.CheckPatternStatus(daq);
                        ReadPatternResult readPattern2 = ReadPatternMethodClass.ReadErrorMemory("ram_20M_250Ln", signalNames, daq, 20);
                        // int[] failSites = { 0, 1, 2, 3 };
                        // SoftBinMapClass softBinMapClass = new SoftBinMapClass();
                        // softBinMapClass.HardBinNumber = 1;
                        // softBinMapClass.SoftBinNumber = 2;
                        // daq.BinHandle(failSites, softBinMapClass);
                        daq.PostTextLog(readpattern.ErrorLog);
                        ClearError.ClearErrorMemory(daq);

                        daq.PostTextLog(readPattern2.ErrorLog);
                        ClearError.ClearErrorMemory(daq);

                        // //AcTimming.GetAcTimmingCmd(200.0, true, daq);
                        // DcLevel.GetDcLevelCmd(5.0, 0.0, true, daq);
                        // DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                        // AcTimming.GetAcTimmingCmd(10000, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        // TrimPatternCode.StartPattern("ram_100K_1KLn", daq);//启动 ram pattern ADC
                        // daq.PostTextLog("Run ram_100K_1KLn End\n");//UI打印
                        // PatternStatus.CheckPatternStatus(daq);
                        // ReadPatterndaq readpattern3 = ReadPatternMethodClass.ReadErrorMemory("ram_100K_1KLn", signalNames, daq, 10);
                        // daq.PostTextLog(readpattern.ErrorLog);
                        // ClearError.ClearErrorMemory(daq);


                        // DcLevel.GetDcLevelCmd(5.0, 0.0, true, daq);
                        // DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                        // AcTimming.GetAcTimmingCmd(25, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        // TrimPatternCode.StartPattern("ram_40M_15KLn", daq);//启动 ram pattern ADC
                        // daq.PostTextLog("Run ram_40M_15KLn End\n");//UI打印
                        // PatternStatus.CheckPatternStatus(daq);
                        // ReadPatterndaq readpattern4 = ReadPatternMethodClass.ReadErrorMemory("ram_40M_15KLn", signalNames, daq, 10);
                        // daq.PostTextLog(readpattern.ErrorLog);
                        // ClearError.ClearErrorMemory(daq);


                        // DcLevel.GetDcLevelCmd(5.0, 0.0, true, daq);
                        // DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                        // AcTimming.GetAcTimmingCmd(100, true, daq);
                        // Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        // TrimPatternCode.StartPattern("ram_10M_15KLn", daq);//启动 ram pattern ADC
                        // daq.PostTextLog("Run ram_10M_15KLn End\n");//UI打印
                        // PatternStatus.CheckPatternStatus(daq);
                        // ReadPatterndaq readpattern5 = ReadPatternMethodClass.ReadErrorMemory("ram_10M_15KLn", signalNames, daq, 10);
                        // daq.PostTextLog(readpattern.ErrorLog);
                        // ClearError.ClearErrorMemory(daq);
                    }
                    break;

                case "NOR_PAT":
                    {
                        //Reset.DigitalCardReset(daq);
                        ClearAllError.ClearErrorMemory(daq);
                        ClearError.ClearErrorMemory(daq);
                        // DcLevel.GetDcLevelCmd(4, 0, true, daq);
                        ////  DcLevel.GetVIHVILByPinName(pinname,4.5, 0, true, daq);
                        //DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                        ////DcLevel.SetVIHVILByPinName(3, 0, pinnames, true, daq);

                        //Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                        //AcTimming.GetAcTimmingCmd(200.0, true, daq);
                        //Thread.Sleep(100);
                        for (int i = 0; i < norPatterns.Length; i++)
                        {
                            //DcLevel.GetDcLevelCmd(VIH[i], 0.0, true, daq);
                            //DcLevel.GetVIHVILByPinName( pinname, VIH[i], 0, true, daq);
                            //DcLevel.GetVIHVILByPinName( pinnames, VIH2[i], 0, true, daq);


                            Thread.Sleep(10);
                            //AcTimming.GetAcTimmingCmd(norPatternsAcTiming[i], true, daq);
                            TrimPatternCode.StartPattern(norPatterns[i], daq);
                            //daq.stringbuilder.Append(norPatterns[i] + "\n");
                            // PatternStatus.CheckPatternStatus(daq);
                            //  Readpatterndaq readpattern=ReadPatternMthodClass.ReadErrorMemory()
                            Thread.Sleep(100);
                            ReadPatternResult readpattern = ReadPatternMethodClass.ReadErrorMemory(norPatterns[i], signalName, daq, 10);
                            // int[] failSites = { 0, 1, 2, 3 };
                            // SoftBinMapClass softBinMapClass = new SoftBinMapClass();
                            // softBinMapClass.HardBinNumber = 1;
                            // softBinMapClass.SoftBinNumber = 2;
                            // daq.BinHandle(failSites, softBinMapClass);
                            daq.PostTextLog(readpattern.ErrorLog);
                            ClearError.ClearErrorMemory(daq);

                        }
                        //AcTimming.GetAcTimmingCmd(norPatternsAcTiming[0], true, daq);
                        //TrimPatternCode.StartPattern(norPatterns[0], daq);
                        //daq.stringbuilder.Append(norPatterns[i] + "\n");
                        //PatternStatus.CheckPatternStatus(daq);


                    }
                    break;
                
                case "DMM6500_CTRL":
                    {
                        //Initilize
                        IKeithleyDMM6500 driver;
                        driver = new KeithleyDMM6500Class();

                        string IPAddress = "TCPIP0::192.168.100.240::inst0::INSTR"; //@@Rita, Hardcode, 后期需修改
                        string MeasItem = "Current"; //"Voltage"
                        double MeasRange = 0.001; //@@Rita, Hardcode, 后期需修改
                        int SampleNum = 10; //@@Rita, Hardcode, 后期需修改


                        try
                        {
                            driver.Initialize(IPAddress, false, false, "");
                        }
                        catch (Exception ex) //@@Rita, 连接失败如何处理，后期需修改
                        {
                            Console.WriteLine(ex);

                        }


                        KeithleyDMM6500FunctionEnum MeasMode;
                        KeithleyDMM6500FunctionsWithRangeEnum RangeMode;

                        //Measurement Start, Set Current/Voltage Mode
                        if (MeasItem == "Current") //Measure Current
                        {
                            MeasMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionCurrentDC;
                            RangeMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeCurrentDC;
                        }
                        else if (MeasItem == "Voltage") //Measure Voltage
                        {
                            MeasMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                            RangeMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        }
                        else  //设置缺省值，后期需修改
                        {
                            MeasMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                            RangeMode = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        }

                        //Set Mode
                        //driver.Function = KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionCurrentDC; //测量电流
                        driver.Function = MeasMode; //测量电流
                        Thread.Sleep(500);
                        //Set Range                        
                        //driver.Measurement.Configuration.set_Range(KeithleyInstruments.KeithleyDMM6500.Interop.KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeCurrentDC, MeasRange);
                        driver.Measurement.Configuration.set_Range(RangeMode, MeasRange);

                        Thread.Sleep(500);
                        //Set 抓取数据量
                        driver.Measurement.Configuration.Count = SampleNum;
                        Thread.Sleep(500);

                        //Start trigger
                        driver.Buffer.TraceTrigger("defbuffer1");
                        Thread.Sleep(500);// depends on the count we have to increase the sleep for the buffer to get all the data 
                        double[] wfmReading = driver.Buffer.FetchDoubleData(1, SampleNum, "defbuffer1", KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        Thread.Sleep(500);
                        string wfmReadingUnits = driver.Buffer.FetchStringData(1, SampleNum, "defbuffer1", KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        Thread.Sleep(500);
                        double[] wfmReadingRelativeTimestamp = driver.Buffer.FetchDoubleData(1, SampleNum, "defbuffer1", KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementRelative);
                        Thread.Sleep(500);

                        //print to log
                        string[] formattedReading = wfmReadingUnits.Split(',');
                        if (wfmReadingRelativeTimestamp.Length == wfmReading.Length && formattedReading.Length == wfmReading.Length)
                        {
                            for (int i = 0; i < wfmReadingRelativeTimestamp.Length; i++)
                            {
                                formattedReading[i] = wfmReadingRelativeTimestamp[i].ToString("F8") + "\t" + wfmReading[i].ToString("0.#####E+0") + formattedReading[i];
                            }
                        }

                        //??? 如何打到datalog
                        //ResultTextBox.Text = "RelativeTime\t FormattedReading" + Environment.NewLine + String.Join(Environment.NewLine, formattedReading);

                    }
                    break;
                case "SyncMeasureFVMV":
                    {
                        string IPAddress = "TCPIP0::192.168.100.240::inst0::INSTR";
                        //string bufferName = "FVMV";
                        InitDMM( IPAddress );
                        KeithleyDMM6500FunctionEnum measFunc = KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                        KeithleyDMM6500FunctionsWithRangeEnum measRange = KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        MeasureDMM(measFunc, measRange, 10, 1);
                        //dmmDriver.Function = measFunc;
                        //dmmDriver.Buffer.Clear(bufferName);
                        //dmmDriver.Buffer.Create("FVMV", 1000);
                        //dmmDriver.Measurement.Configuration.set_Range(measRange, 10);
                        //dmmDriver.Measurement.Configuration.set_NPLC(KeithleyDMM6500NPLCFunctionsEnum.KeithleyDMM6500NPLCFunctionsVoltageDC, 1);
                        //dmmDriver.Measurement.Configuration.Count = 1;
                        Thread.Sleep(50);
                        int[] dps17 = { 17 };
                        DpsRangeRelay.SetDpsRangeRelayByChannels(dps17, DpsRelayState.INT, daq);
                        Dps.GetDpsChannelForceCmd(dps17, DpsMode.FVMV, 2.5, DpsRange._2mA, true, daq);
                        Thread.Sleep(10);
                        //dmmDriver.Buffer.TraceTrigger(bufferName);
                        Thread.Sleep(200);
                        double[] readData = dmmDriver.Buffer.FetchDoubleData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        string readUnit = dmmDriver.Buffer.FetchStringData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        daq.PostTextLog($"{readData[0]:F8} "+readUnit);
                    }
                    break;
                case "InitDMM":
                    {
                        string IPAddress = "TCPIP0::192.168.100.240::inst0::INSTR";
                        InitDMM(IPAddress);
                        StreamWriter sw = new StreamWriter("Data_Collection.csv", false, Encoding.UTF8);
                        sw.WriteLine("Channel,Single Measurement,Manual Average,DMM Measure,Temperature,Range");
                        sw.Flush();
                        sw.Close();
                    }
                    break;
                case "CloseDMM":
                    {
                        CloseDMM();
                    }
                    break;
                case "Collect_DPS":
                    {
                        string[] dps = { "DPS1" };
                        int repeat1 = 64;
                        int repeat2 = 64;
                        int channel = 0;
                        double forceVoltage = 5;
                        DpsRange forceRange = DpsRange._2mA;
                        MeasureMode measureRange = MeasureMode.Supply_Voltage_2mA;
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.OPEN, daq);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0, DpsRange._2mA, true, daq);
                        DpsRangeRelay.SetDpsRangeRelayBySignals(dps, DpsRelayState.INT, daq);
                        //Dps.GetDpsPinNameForceCmd(dps1, DpsMode.FVMV, 2.511827765, DpsRange._2mA, true, daq);
                        Dps.GetDpsPinNameForceCmd(dps, DpsMode.FVMV, forceVoltage, forceRange, true, daq, true);
                        Thread.Sleep(10);
                        StreamWriter sw = new StreamWriter("Data_Collection.csv", true, Encoding.UTF8);
                        sw.Write($"DPS{channel+1},");
                        double[] readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                        sw.Write($"{readVal[channel]:F8},");
                        //readVal = Measure.GetMeasureValue(measureRange, repeat2, 5000, daq);
                        //sw.Write($"{readVal[channel]:F8},");
                        double sum = 0;
                        for (int i = 0; i < repeat1; i++)
                        {
                            readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                            sum += readVal[channel];
                        }
                        sum /= repeat1;
                        sw.Write($"{sum:F8},");
                        KeithleyDMM6500FunctionEnum measFunc = KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                        KeithleyDMM6500FunctionsWithRangeEnum measRangeMode = KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        double measRange = 10;
                        MeasureDMM(measFunc, measRangeMode, measRange, 1);
                        double[] readData = dmmDriver.Buffer.FetchDoubleData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        string readUnit = dmmDriver.Buffer.FetchStringData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        sw.Write($"{readData[0]:F8},");
                        readVal = Dps.ReadDpsTemp(true, daq);
                        int tempIndex = channel / 4;
                        sw.Write($"{readVal[tempIndex]:F8},");
                        sw.WriteLine(Enum.GetName(typeof(MeasureMode), measureRange));
                        sw.Flush();
                        sw.Close();
                    }
                    break;
                case "Collect_PMU":
                    {
                        KeysightDMM3458A dmm = new KeysightDMM3458A();
                        ExternalCalibrationHandle extCaliHandle = new ExternalCalibrationHandle();
                        string ksightConnStr = "GPIB0::22::INSTR";
                        string extConnStr = "192.168.100.11";
                        const string EXT_CAL_PATH = "C:\\ProgramData\\Testrong\\ATE_Tester\\ExtCal\\";
                        const string EXT_CAL_MV_PATH = "C:\\ProgramData\\Testrong\\ATE_Tester\\ExtCal_bak\\";
                        int extPort = 8;
                        byte[] armDnaPortCmd = new byte[2] { 14, 57 }; // command to read arm dna
                        byte[] armDnaRaw = daq.Communicator.Send(armDnaPortCmd); //send command
                        armDnaRaw = armDnaRaw.Reverse().ToArray();
                        string arm_dna = BitConverter.ToUInt64(armDnaRaw, 0).ToString(); //convert hex dna to decimal
                        List<Core.Communicate.ICommunicate> tcpList = daq.FlowContext.TcpManager.GetConnectedTcpCliens(); //prepare to get ip address
                        string[] Range = { "5uA", "20uA", "200uA", "2mA", "60mA" };
                        string[] Mode = { "FVMV", "FVMI", "FIMV", "FZMV", "FZMI" };
                        string[] Accuracy = { "20V", "5V" };
                        string[] Rload = { "Hi-Z", "150", "5k", "50k", "400K", "2M" };
                        int[] channel = new int[256];
                        for (int n = 0; n < 256; n++)
                        {
                            channel[n] = n + 1;
                        }
                        double[] readVal;
                        double ForceInitial = -7.5;//电压初始值
                        double VoltageStep = 2.5;

                        //mode
                        int ModeCount = 5;
                        PpmuMode[] PMUModes = { PpmuMode.FVMV, PpmuMode.FZMV, PpmuMode.FIMV, PpmuMode.FVMI, PpmuMode.FZMI };
                        DpsMode[] DPSModes = { DpsMode.FVMV, DpsMode.FZMV, DpsMode.FIMV, DpsMode.FVMI, DpsMode.FZMI };
                        //range
                        int RangeCount = 5;
                        PpmuRange[] PMURanges = { PpmuRange._5uA, PpmuRange._20uA, PpmuRange._200uA, PpmuRange._2mA, PpmuRange._60mA };
                        DpsRange[] DPSRanges = { DpsRange._5uA, DpsRange._20uA, DpsRange._200uA, DpsRange._2mA, DpsRange._60mA };

                        //channelNO
                        int ChannelCount = 256;
                        int DpsCount = 32;
                        //forceValue
                        int ForceCount = 7;
                        double[] ForceValue = { -7.5, 5, -2.5, 0, 2.5, 5, 7.5 };
                        //MeasureMode
                        MeasureMode[] measureRanges = { MeasureMode.IO_CallBack_Voltage_5uA ,
                                                        MeasureMode.IO_CallBack_Voltage_20uA,
                                                        MeasureMode.IO_CallBack_Voltage_200uA,
                                                        MeasureMode.IO_CallBack_Voltage_2mA,
                                                        MeasureMode.IO_CallBack_Voltage_60mA,
                                                        MeasureMode.IO_CallBack_Current_5uA ,
                                                        MeasureMode.IO_CallBack_Current_20uA ,
                                                        MeasureMode.IO_CallBack_Current_200uA ,
                                                        MeasureMode.IO_CallBack_Current_2mA ,
                                                        MeasureMode.IO_CallBack_Current_60mA
                                                        };
                        MeasureMode[] DpsMeasureRanges =
                                                      {
                                                        MeasureMode.Supply_Voltage_5uA ,
                                                        MeasureMode.Supply_Voltage_20uA,
                                                        MeasureMode.Supply_Voltage_200uA,
                                                        MeasureMode.Supply_Voltage_2mA,
                                                        MeasureMode.Supply_Voltage_60mA,
                                                      };
                        if (Directory.Exists(EXT_CAL_MV_PATH))
                        {
                            Directory.Delete(EXT_CAL_MV_PATH, true);
                        }
                        if (Directory.Exists(EXT_CAL_PATH))
                        {
                            Directory.Move(EXT_CAL_PATH, EXT_CAL_MV_PATH);
                        }
                        Directory.CreateDirectory(EXT_CAL_PATH);
                        dmm.InitDMM(ksightConnStr);
                        //metadata.csv
                        StreamWriter swdata = new StreamWriter(EXT_CAL_PATH + "metadata" + ".csv", true, Encoding.UTF8);
                        swdata.WriteLine("Tester_Name,Foo");
                        swdata.WriteLine("Slot1_Dna," + arm_dna);
                        swdata.WriteLine("Slot2_Dna,4764477827680352");
                        swdata.WriteLine("Slot3_Dna,4764477827680356");
                        swdata.WriteLine("Slot4_Dna,4764477827680360");
                        tcpList = daq.FlowContext.TcpManager.GetConnectedTcpCliens();
                        swdata.WriteLine(arm_dna + "," + tcpList[0].ServerIP);
                        swdata.WriteLine("4764477827680352,192.168.100.12");
                        swdata.WriteLine("4764477827680356,192.168.100.13");
                        swdata.WriteLine("4764477827680360,192.168.100.14");
                        //@@rita, 写入空行，数据从11行开始打印
                        for (int i = 0; i < 1; i++)
                        {
                            swdata.WriteLine();
                        }

                        swdata.Write("Slot_Dna,Channel_Type,Channel_No,Mode,R_Load,I_Range,Meas_Range,File_Path\n");
                        //PMU Data Collection
                        //Range循环

                        for (int j = 0; j < RangeCount; j++)
                        {   //Channel循环
                            for (int k = 0; k < ChannelCount; k++)
                            {
                                string filename = EXT_CAL_PATH + "PMU_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
                                string filepath = "PMU_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                int slotNum = (k + 64) / 64;
                                double ForceVoltage = ForceInitial;
                                string range = Range[j];
                                //write file 
                                sw.Write("Tester_Name,Foo\n");
                                sw.Write($"Slot_No,{slotNum}\n");
                                sw.Write($"Board_Dna,{arm_dna}\n");
                                sw.Write("Channel_Type,PMU\n");
                                sw.Write($"Channel_No,{channel[k]}\n");
                                sw.Write("Mode,FVMV\n");
                                sw.Write("R_Load,Hi-Z\n"); //@@rita
                                sw.Write($"I_Range,{Range[j]}\n");
                                sw.Write("Meas_Range,20V\n");
                                sw.Write("\n");
                                sw.Write("Force Value,ATE Measure,DMM Measure,Temperature\n");
                                if (k <= 63)
                                {
                                    //forceValue 循环
                                    for (int p = 0; p < ForceCount; p++)
                                    {
                                        PpmuRange forceRange = PMURanges[j];
                                        MeasureMode measureRange = measureRanges[j];
                                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                                        Thread.Sleep(10);
                                        Ppmu.GetPpmuPinNameForceCmd(allPmuChannels, PMUModes[0], ForceVoltage, forceRange, true, daq);
                                        sw.Write($"{ForceVoltage:F8},");
                                        ForceVoltage += VoltageStep;
                                        Thread.Sleep(10);
                                        readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                        sw.Write($"{readVal[k]:F8},");
                                        //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                        double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                        sw.Write($"{dmmRead:F8},");
                                        readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                        sw.Write($"{readVal[slotNum - 1]:F8}\n");
                                    }
                                }
                                else
                                {
                                    //forceValue 循环
                                    for (int p = 0; p < ForceCount; p++)
                                    {
                                        PpmuRange forceRange = PMURanges[j];
                                        MeasureMode measureRange = measureRanges[j];
                                        //Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                                        //Thread.Sleep(10);
                                        //Ppmu.GetPpmuPinNameForceCmd(allPmuChannels, PpmuMode.FVMV, ForceVoltage, forceRange, true, daq/*, true*/);
                                        sw.Write($"{0:F8},");
                                        ForceVoltage += 0;
                                        //Thread.Sleep(10);
                                        //readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                        sw.Write($"{0:F8},");
                                        //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                        //double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                        sw.Write($"{0:F8},");
                                        //readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                        sw.Write($"{0:F8}\n");
                                    }
                                }
                                sw.Flush();
                                sw.Close();

                                swdata.Write($"{arm_dna},");
                                swdata.Write("PMU,");
                                swdata.Write($"{channel[k]},");
                                swdata.Write($"{Mode[0]},");
                                swdata.Write($"{Rload[0]},");//change
                                swdata.Write($"{Range[j]},");
                                swdata.Write("20V,");//change
                                swdata.Write($"{filepath}\n");
                                swdata.Flush();
                            }
                        }
                        swdata.Flush();

                        //DPS Data Collection
                        for (int j = 0; j < RangeCount; j++)
                        {   //Channel循环
                            for (int k = 0; k < DpsCount; k++)
                            {
                                string filename = EXT_CAL_PATH + "DPS_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
                                string filepath = "DPS_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                int slotNum = (k + 64) / 64;
                                double ForceVoltage = ForceInitial;
                                string range = Range[j];
                                //write file 
                                sw.Write("Tester_Name,Foo\n");
                                sw.Write($"Slot_No,{slotNum}\n");
                                sw.Write($"Board_Dna,{arm_dna}\n");
                                sw.Write("Channel_Type,DPS\n");
                                sw.Write($"Channel_No,{channel[k]}\n");
                                sw.Write("Mode,FVMV\n");
                                sw.Write("R_Load,Hi-Z\n"); //@@rita
                                sw.Write($"I_Range,{Range[j]}\n");
                                sw.Write("Meas_Range,20V\n");
                                sw.Write("\n");
                                sw.Write("Force Value,ATE Measure,DMM Measure,Temperature\n");
                                //forceValue 循环
                                for (int p = 0; p < ForceCount; p++)
                                {
                                    DpsRange forceRange = DPSRanges[j];
                                    MeasureMode measureRange = DpsMeasureRanges[j];
                                    DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                                    Dps.GetDpsPinNameForceCmd(allDpsPins, DPSModes[0], ForceVoltage, forceRange, true, daq);
                                    Thread.Sleep(10);
                                    sw.Write($"{ForceVoltage:F8},");
                                    ForceVoltage += VoltageStep;
                                    Thread.Sleep(10);
                                    readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                    sw.Write($"{readVal[k]:F8},");
                                    //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                    double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                    sw.Write($"{dmmRead:F8},");
                                    readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                    sw.Write($"{readVal[slotNum - 1]:F8}\n");
                                }

                                sw.Flush();
                                sw.Close();

                                swdata.Write($"{arm_dna},");
                                swdata.Write("DPS,");
                                swdata.Write($"PS{channel[k]},");
                                swdata.Write($"{Mode[0]},");
                                swdata.Write($"{Rload[0]},");//change
                                swdata.Write($"{Range[j]},");
                                swdata.Write("20V,");//change
                                swdata.Write($"{filepath}\n");
                                swdata.Flush();
                            }
                        }
                        swdata.Flush();

                        //Write Finish
                        swdata.Close();

                    }

                    break;
                case "PMU_Offset_Post_Check":
                    {
                        KeysightDMM3458A dmm = new KeysightDMM3458A();
                        ExternalCalibrationHandle extCaliHandle = new ExternalCalibrationHandle();
                        string ksightConnStr = "GPIB0::22::INSTR";
                        string extConnStr = "192.168.100.11";
                        const string EXT_CAL_PATH = "C:\\ProgramData\\Testrong\\ATE_Tester\\ExtCal\\";
                        const string EXT_CAL_MV_PATH = "C:\\ProgramData\\Testrong\\ATE_Tester\\ExtCal_bak\\";
                        int extPort = 8;
                        byte[] armDnaPortCmd = new byte[2] { 14, 57 }; // command to read arm dna
                        byte[] armDnaRaw = daq.Communicator.Send(armDnaPortCmd); //send command
                        armDnaRaw = armDnaRaw.Reverse().ToArray();
                        string arm_dna = BitConverter.ToUInt64(armDnaRaw, 0).ToString(); //convert hex dna to decimal
                        List<Core.Communicate.ICommunicate> tcpList = daq.FlowContext.TcpManager.GetConnectedTcpCliens(); //prepare to get ip address
                        string[] Range = { "5uA", "20uA", "200uA", "2mA", "60mA" };
                        string[] Mode = { "FVMV", "FVMI", "FIMV", "FZMV", "FZMI" };
                        string[] Accuracy = { "20V", "5V" };
                        string[] Rload = { "Hi-Z", "150", "5k", "50k", "400K", "2M" };
                        int[] channel = new int[256];
                        for (int n = 0; n < 256; n++)
                        {
                            channel[n] = n + 1;
                        }
                        double[] readVal;
                        double ForceInitial = -7.5;//电压初始值
                        double VoltageStep = 2.5;

                        //mode
                        int ModeCount = 5;
                        PpmuMode[] PMUModes = { PpmuMode.FVMV, PpmuMode.FZMV, PpmuMode.FIMV, PpmuMode.FVMI, PpmuMode.FZMI };
                        DpsMode[] DPSModes = { DpsMode.FVMV, DpsMode.FZMV, DpsMode.FIMV, DpsMode.FVMI, DpsMode.FZMI };
                        //range
                        int RangeCount = 5;
                        PpmuRange[] PMURanges = { PpmuRange._5uA, PpmuRange._20uA, PpmuRange._200uA, PpmuRange._2mA, PpmuRange._60mA };
                        DpsRange[] DPSRanges = { DpsRange._5uA, DpsRange._20uA, DpsRange._200uA, DpsRange._2mA, DpsRange._60mA };

                        //channelNO
                        int ChannelCount = 256;
                        int DpsCount = 32;
                        //forceValue
                        int ForceCount = 7;
                        double[] ForceValue = { -7.5, 5, -2.5, 0, 2.5, 5, 7.5 };
                        //MeasureMode
                        MeasureMode[] measureRanges = { MeasureMode.IO_CallBack_Voltage_5uA ,
                                                        MeasureMode.IO_CallBack_Voltage_20uA,
                                                        MeasureMode.IO_CallBack_Voltage_200uA,
                                                        MeasureMode.IO_CallBack_Voltage_2mA,
                                                        MeasureMode.IO_CallBack_Voltage_60mA,
                                                        MeasureMode.IO_CallBack_Current_5uA ,
                                                        MeasureMode.IO_CallBack_Current_20uA ,
                                                        MeasureMode.IO_CallBack_Current_200uA ,
                                                        MeasureMode.IO_CallBack_Current_2mA ,
                                                        MeasureMode.IO_CallBack_Current_60mA
                                                        };
                        MeasureMode[] DpsMeasureRanges =
                                                      {
                                                        MeasureMode.Supply_Voltage_5uA ,
                                                        MeasureMode.Supply_Voltage_20uA,
                                                        MeasureMode.Supply_Voltage_200uA,
                                                        MeasureMode.Supply_Voltage_2mA,
                                                        MeasureMode.Supply_Voltage_60mA,
                                                      };
                        if (Directory.Exists(EXT_CAL_MV_PATH))
                        {
                            Directory.Delete(EXT_CAL_MV_PATH, true);
                        }
                        if (Directory.Exists(EXT_CAL_PATH))
                        {
                            Directory.Move(EXT_CAL_PATH, EXT_CAL_MV_PATH);
                        }
                        Directory.CreateDirectory(EXT_CAL_PATH);
                        dmm.InitDMM(ksightConnStr);
                        //metadata.csv
                        StreamWriter swdata = new StreamWriter(EXT_CAL_PATH + "metadata" + ".csv", true, Encoding.UTF8);
                        swdata.WriteLine("Tester_Name,Foo");
                        swdata.WriteLine("Slot1_Dna," + arm_dna);
                        swdata.WriteLine("Slot2_Dna,4764477827680352");
                        swdata.WriteLine("Slot3_Dna,4764477827680356");
                        swdata.WriteLine("Slot4_Dna,4764477827680360");
                        tcpList = daq.FlowContext.TcpManager.GetConnectedTcpCliens();
                        swdata.WriteLine(arm_dna + "," + tcpList[0].ServerIP);
                        swdata.WriteLine("4764477827680352,192.168.100.12");
                        swdata.WriteLine("4764477827680356,192.168.100.13");
                        swdata.WriteLine("4764477827680360,192.168.100.14");
                        //@@rita, 写入空行，数据从11行开始打印
                        for (int i = 0; i < 1; i++)
                        {
                            swdata.WriteLine();
                        }

                        swdata.Write("Slot_Dna,Channel_Type,Channel_No,Mode,R_Load,I_Range,Meas_Range,File_Path\n");
                        //PMU Data Collection
                        //Range循环

                        for (int j = 0; j < RangeCount; j++)
                        {   //Channel循环
                            for (int k = 0; k < ChannelCount; k++)
                            {
                                string filename = EXT_CAL_PATH + "PMU_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
                                string filepath = "PMU_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                int slotNum = (k + 64) / 64;
                                double ForceVoltage = ForceInitial;
                                string range = Range[j];
                                //write file 
                                sw.Write("Tester_Name,Foo\n");
                                sw.Write($"Slot_No,{slotNum}\n");
                                sw.Write($"Board_Dna,{arm_dna}\n");
                                sw.Write("Channel_Type,PMU\n");
                                sw.Write($"Channel_No,{channel[k]}\n");
                                sw.Write("Mode,FVMV\n");
                                sw.Write("R_Load,Hi-Z\n"); //@@rita
                                sw.Write($"I_Range,{Range[j]}\n");
                                sw.Write("Meas_Range,20V\n");
                                sw.Write("\n");
                                sw.Write("Force Value,ATE Measure,DMM Measure,Temperature\n");
                                if (k <= 63)
                                {
                                    //forceValue 循环
                                    for (int p = 0; p < ForceCount; p++)
                                    {
                                        PpmuRange forceRange = PMURanges[j];
                                        MeasureMode measureRange = measureRanges[j];
                                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                                        Thread.Sleep(10);
                                        Ppmu.GetPpmuPinNameForceCmd(allPmuChannels, PMUModes[0], ForceVoltage, forceRange, true, daq, true);
                                        sw.Write($"{ForceVoltage:F8},");
                                        ForceVoltage += VoltageStep;
                                        Thread.Sleep(10);
                                        readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                        sw.Write($"{readVal[k]:F8},");
                                        //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                        double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                        sw.Write($"{dmmRead:F8},");
                                        readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                        sw.Write($"{readVal[slotNum - 1]:F8}\n");
                                    }
                                }
                                else
                                {
                                    //forceValue 循环
                                    for (int p = 0; p < ForceCount; p++)
                                    {
                                        PpmuRange forceRange = PMURanges[j];
                                        MeasureMode measureRange = measureRanges[j];
                                        //Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Ppmu, true, daq);
                                        //Thread.Sleep(10);
                                        //Ppmu.GetPpmuPinNameForceCmd(allPmuChannels, PpmuMode.FVMV, ForceVoltage, forceRange, true, daq/*, true*/);
                                        sw.Write($"{0:F8},");
                                        ForceVoltage += 0;
                                        //Thread.Sleep(10);
                                        //readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                        sw.Write($"{0:F8},");
                                        //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                        //double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                        sw.Write($"{0:F8},");
                                        //readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                        sw.Write($"{0:F8}\n");
                                    }
                                }
                                sw.Flush();
                                sw.Close();

                                swdata.Write($"{arm_dna},");
                                swdata.Write("PMU,");
                                swdata.Write($"{channel[k]},");
                                swdata.Write($"{Mode[0]},");
                                swdata.Write($"{Rload[0]},");//change
                                swdata.Write($"{Range[j]},");
                                swdata.Write("20V,");//change
                                swdata.Write($"{filepath}\n");
                                swdata.Flush();
                            }
                        }
                        swdata.Flush();

                        //DPS Data Collection
                        for (int j = 0; j < RangeCount; j++)
                        {   //Channel循环
                            for (int k = 0; k < DpsCount; k++)
                            {
                                string filename = EXT_CAL_PATH + "DPS_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
                                string filepath = "DPS_" + "ch" + channel[k] + "_FVMV" + "_" + Range[j] + ".csv";
                                int slotNum = (k + 64) / 64;
                                double ForceVoltage = ForceInitial;
                                string range = Range[j];
                                //write file 
                                sw.Write("Tester_Name,Foo\n");
                                sw.Write($"Slot_No,{slotNum}\n");
                                sw.Write($"Board_Dna,{arm_dna}\n");
                                sw.Write("Channel_Type,DPS\n");
                                sw.Write($"Channel_No,{channel[k]}\n");
                                sw.Write("Mode,FVMV\n");
                                sw.Write("R_Load,Hi-Z\n"); //@@rita
                                sw.Write($"I_Range,{Range[j]}\n");
                                sw.Write("Meas_Range,20V\n");
                                sw.Write("\n");
                                sw.Write("Force Value,ATE Measure,DMM Measure,Temperature\n");
                                //forceValue 循环
                                for (int p = 0; p < ForceCount; p++)
                                {
                                    DpsRange forceRange = DPSRanges[j];
                                    MeasureMode measureRange = DpsMeasureRanges[j];
                                    DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.INT, daq);
                                    Dps.GetDpsPinNameForceCmd(allDpsPins, DPSModes[0], ForceVoltage, forceRange, true, daq, true);
                                    Thread.Sleep(10);
                                    sw.Write($"{ForceVoltage:F8},");
                                    ForceVoltage += VoltageStep;
                                    Thread.Sleep(10);
                                    readVal = Measure.GetMeasureBackChannelValue(measureRange, daq);
                                    sw.Write($"{readVal[k]:F8},");
                                    //extCaliHandle.SetPpmuExternalCalibration(extConnStr, extPort, slotNum, ExternalCalibrationMode.Channel, k, 1);//切换通道
                                    double dmmRead = dmm.GetDmmMeasureData(KeysightDMM3458A.KeysightMeasMode.VOLT);//八位半读值
                                    sw.Write($"{dmmRead:F8},");
                                    readVal = Ppmu.ReadPpmuTemp(true, daq);//温度读取
                                    sw.Write($"{readVal[slotNum - 1]:F8}\n");
                                }

                                sw.Flush();
                                sw.Close();

                                swdata.Write($"{arm_dna},");
                                swdata.Write("DPS,");
                                swdata.Write($"PS{channel[k]},");
                                swdata.Write($"{Mode[0]},");
                                swdata.Write($"{Rload[0]},");//change
                                swdata.Write($"{Range[j]},");
                                swdata.Write("20V,");//change
                                swdata.Write($"{filepath}\n");
                                swdata.Flush();
                            }
                        }
                        swdata.Flush();

                        //Write Finish
                        swdata.Close();

                    }

                    break;

                case "DPS_17_Overnight":
                    {
                        string[] dps1 = { "DPS1" };
                        int repeat1 = 48;
                        int repeat2 = 16;
                        int channel = 16;
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.OPEN, daq);
                        Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0, DpsRange._2mA, true, daq);
                        DpsRangeRelay.SetDpsRangeRelayBySignals(dps1, DpsRelayState.INT, daq);
                        //Dps.GetDpsPinNameForceCmd(dps1, DpsMode.FVMV, 2.511827765, DpsRange._2mA, true, daq);
                        Dps.GetDpsPinNameForceHighPrecisionCmd(dps1, DpsMode.FVMV, 2.5, DpsRange._2mA, VoltageRange.VoltageHighPrecision, true, daq, true);
                        Thread.Sleep(10);
                        StreamWriter sw = new StreamWriter("DPS_Overnight.csv", true, Encoding.UTF8);
                        double[] readVal = Measure.GetMeasureBackChannelValue(MeasureMode.Supply_Voltage_2mA, daq);
                        sw.Write($"{readVal[channel]:F8},");
                        readVal = Measure.GetMeasureValue(MeasureMode.Supply_Voltage_2mA, repeat2, 5000, daq);
                        sw.Write($"{readVal[channel]:F8},");
                        //daq.PostTextLog($"Firmware Average: Channel:017 Value:{readVal[16]:F8}");
                        double sum = 0;
                        for (int i = 0; i < repeat1; i++)
                        {
                            readVal = Measure.GetMeasureBackChannelValue(MeasureMode.Supply_Voltage_2mA, daq);
                            sum += readVal[channel];
                        }
                        sum /= repeat1;
                        //daq.PostTextLog($"Manual Average: Channel:017 Value:{sum:F8}");
                        sw.Write($"{sum:F8},");
                        KeithleyDMM6500FunctionEnum measFunc = KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                        KeithleyDMM6500FunctionsWithRangeEnum measRangeMode = KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        double measRange = 10;
                        MeasureDMM(measFunc, measRangeMode, measRange, 1);
                        double[] readData = dmmDriver.Buffer.FetchDoubleData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        string readUnit = dmmDriver.Buffer.FetchStringData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        sw.Write($"{readData[0]:F8},");
                        readVal = Dps.ReadDpsTemp(true, daq);
                        sw.Write($"{readVal[4]:F8}\n");
                        sw.Flush();
                        sw.Close();
                    }
                    break;
                case "DPS_17_Avg":
                    {
                        double[] readVal = Measure.GetMeasureValue(MeasureMode.Supply_Voltage_2mA, 128, 1000, daq);
                        daq.PostTextLog($"Channel:017 Value:{readVal[16]:F8}");
                    }
                    break;
                case "MeasureDMM_ADR425":
                    {
                        KeithleyDMM6500FunctionEnum measFunc = KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                        KeithleyDMM6500FunctionsWithRangeEnum measRangeMode = KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        double measRange = 10;
                        MeasureDMM(measFunc, measRangeMode, measRange, 1);
                        double[] readData = dmmDriver.Buffer.FetchDoubleData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        string readUnit = dmmDriver.Buffer.FetchStringData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        StreamWriter sw = new StreamWriter("ADR425.csv", true);
                        sw.Write($"{readData[0]:F8},");
                        double[] readVal = Dps.ReadDpsTemp(true, daq);
                        sw.WriteLine($"{readVal[7]:F8}");
                        sw.Flush();
                        sw.Close();
                    }
                    break;
                case "PMU1_Offset_Test_DMM":
                    {
                        double[] readVal;
                        int repeat1 = 48;
                        int channel = 0;
                        string[] pmu1 = { "A01" };
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdByPinName(pmu1, RelayState.PPMU, true, daq);
                        Ppmu.GetPpmuPinNameForceHighPrecisionCmd(pmu1, PpmuMode.FVMV, 2.5, PpmuRange._2mA, VoltageRange.VoltageHighPrecision, true, daq, true); 
                        double sum = 0;
                        for (int i = 0; i < repeat1; i++)
                        {
                            readVal = Measure.GetMeasureBackChannelValue(MeasureMode.IO_CallBack_Voltage_2mA, daq);
                            sum += readVal[channel];
                        }
                        sum /= repeat1;
                        double[] readBack = Measure.GetMeasureBackChannelValue(MeasureMode.IO_CallBack_Voltage_2mA, daq);
                        KeithleyDMM6500FunctionEnum measFunc = KeithleyDMM6500FunctionEnum.KeithleyDMM6500FunctionVoltageDC;
                        KeithleyDMM6500FunctionsWithRangeEnum measRangeMode = KeithleyDMM6500FunctionsWithRangeEnum.KeithleyDMM6500FunctionsWithRangeVoltageDC;
                        double measRange = 10;
                        MeasureDMM(measFunc, measRangeMode, measRange, 1);
                        double[] readData = dmmDriver.Buffer.FetchDoubleData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementReading);
                        string readUnit = dmmDriver.Buffer.FetchStringData(1, 1, DMM_BUFFER_NAME, KeithleyDMM6500BufferFormatElementEnum.KeithleyDMM6500BufferFormatElementUnit);
                        StreamWriter sw = new StreamWriter("PMU1_Offset.csv", true);
                        double[] temp = Ppmu.ReadPpmuTemp(true, daq);
                        int tempNo = channel / 64;
                        sw.WriteLine($"{readBack[channel]:F8},{sum:F8},{readData[0]:F8},{temp[tempNo]:F8}");
                        sw.Flush();
                        sw.Close();
                    }
                    break;
                case "PMU_1_OFFSET":
                    {
                        string[] pmu1 = { "A01" };
                        Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                        Relay.GetRelayCmdByPinName(pmu1, RelayState.PPMU, true, daq);
                        Ppmu.GetPpmuPinNameForceCmd(pmu1, PpmuMode.FVMV, 2.5, PpmuRange._2mA, true, daq, true); // with offset
                        Measure.GetMeasureValueByPinName(pmu1, MeasureMode.IO_CallBack_Voltage_2mA, 0, 5, true, daq, "A01");
                        //Ppmu.GetPpmuPinNameForceCmd(pmu1, PpmuMode.FVMV, 2.5, PpmuRange._2mA, true, daq); //without offset
                    }
                    break;
                case "DPS_1_OFFSET":
                    {
                        string[] dps1 = { "DPS1" };
                        DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DpsRelayState.OPEN, daq);
                        DpsRangeRelay.SetDpsRangeRelayBySignals(dps1, DpsRelayState.INT, daq);
                        Dps.GetDpsPinNameForceCmd(dps1, DpsMode.FVMV, 2.5, DpsRange._2mA, true, daq, true); // with offset
                        //Dps.GetDpsPinNameForceCmd(dps1, DpsMode.FVMV, 2.5, DpsRange._2mA, true, daq); //without offset
                    }
                    break;

                /*      case "BT_OS_test":
                      {
                          String [] DPS_OS_0V = {"DPS2"};
                          string [] DPS_OS_PIN ={"GPIO18","GPIO17","DECAP_LPO","BOOT","SWD"};
                          String [] DECAP_VDD = {"DECAP_VDD"};
                          //DpsRangeRelay.SetDpsRangeRelayBySignals(DPS_OS_0V,DPSStatusFlag._60MA,daq);
                          DpsForce0V();
                          Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                          Thread.Sleep(10);
                          NewRelay.GetRelayCmdByPinNameNew(DPS_OS_PIN,StatusFlag.PPMU,true,daq,false);
                          Thread.Sleep(10);
                          PpmuSetting.GetPpmuSettingValueByPinName(PpmuSettingMeasureMode.OS, DPS_OS_PIN, PpmuMode.FIMV, 0.0002, PpmuRange._200uA, 0.4, 0.6, true, daq);
                          PpmuSetting.GetPpmuSettingValueByPinName(PpmuSettingMeasureMode.OS, DPS_OS_PIN, PpmuMode.FIMV, -0.0002, PpmuRange._200uA, -0.6, -0.4, true, daq);
                          Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, daq);
                          NewRelay.GetRelayCmdByPinNameNew(DECAP_VDD,StatusFlag.PPMU,true,daq,false);
                          //DpsRangeRelay.SetDpsRangeRelayBySignals(DPS_OS_0V,DPSStatusFlag._60MA,daq);
                          DpsForce0V();
                          Thread.Sleep(10);
                          Ppmu.GetPpmuPinNameForceCmd(DECAP_VDD,PpmuMode.FIMV,0.0002,PpmuRange._200uA,true,daq);
                          Measure.GetMeasureValueByPinName(DECAP_VDD,MeasureMode.Supply_Voltage_200uA,0.4,0.6,true,daq);
                          Ppmu.GetPpmuPinNameForceCmd(DECAP_VDD,PpmuMode.FIMV,-0.0002,PpmuRange._200uA,true,daq);
                          Measure.GetMeasureValueByPinName(DECAP_VDD,MeasureMode.Supply_Voltage_200uA,-0.6,-0.4,true,daq);
                          Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open,true,daq);






                      }
                      break;
                        case "BT_Capture":
                        {
                            string[] pinNames = { "GPIO0","G0IO25","GPIO24","GPIO23","GPIO22","GPIO1"};
                            string[] pin = {"GPIO1"};
                            String [] DPSPIN = {"DPS1","DPS2","DPS3","DPS4","DPS5","DPS6","DPS7","DPS8"};
                            String [] DPS_5V = {"DPS1"};
                            String [] DPS_3V3  ={"DPS2"};
                            String [] DPS_0V82 = {"DPS4"};
                            String [] DPS_5V1  ={"DPS5"};
                            String [] DPS_RLY3_0V  ={"DPS6"};
                            String [] DPS_RLY_5V  ={"DPS6"};
                            String [] DPS_0V31 ={"DPS7"};
                            String [] DPS_OS_0V = {"DPS2"};
                            String [] DPS_RLY1_0V = {"DPS8"};
                            String [] DPS_RLY2_0V = {"DPS3"};
                            String [] DPS_RLY2_5V = {"DPS3"};
                            String [] DPS_0V = {"DPS1","DPS2","DPS3","DPS4","DPS5","DPS6","DPS7","DPS8"};


                            DpsRangeRelay.SetDpsRangeRelayBySignals(DPSPIN,DPSStatusFlag.EXT,daq);
                            Thread.Sleep(10);
                            Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open,true,daq);
                            Thread.Sleep(10);
                            NewRelay.GetRelayCmdByPinNameNew(pinNames,StatusFlag.FPGA,true,daq,false);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_0V82,DpsMode.FVMI,0.82,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_5V,DpsMode.FVMI,5,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_3V3,DpsMode.FVMI,3.3,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_5V1,DpsMode.FVMI,5,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_RLY1_0V,DpsMode.FVMI,0,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_RLY2_0V,DpsMode.FVMI,0,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_RLY2_5V,DpsMode.FVMI,5,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            Dps.GetDpsPinNameForceCmd(DPS_RLY3_0V,DpsMode.FVMI,0,DpsRange._60mA,true,daq);
                            Thread.Sleep(10);
                            DcLevel.GetVIHVILByPinName(pinNames,3,0,true,daq);
                            DcLevel.GetVohVolCmd(1,1,true,daq);
                            AcTimming.GetAcTimmingCmd(10000,true,daq);



                            //DcLevel.GetDcLevelCmd(5.0, 0.0, true, daq);
                            //DcLevel.GetVohVolCmd(3.5, 1.5, true, daq);
                            //Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_DigitalCard, true, daq);
                            //NewRelay.GetRelayCmdByPinNameNew(pinNames,StatusFlag.FPGA,true,daq,false);
                            TrimPatternCode.StartPattern("BT_test", daq);
                            daq.PostTextLog("run BT_test end\n");//UI打印
                            PatternStatus.CheckPatternStatus(daq);
                            Thread.Sleep(10);
                            ReadPatterndaq readpattern = ReadPatternMethodClass.ReadErrorMemory("BT_test",pin, daq, 1);
                            daq.PostTextLog(readpattern.ErrorLog);
                            ClearError.ClearErrorMemory(daq);
                            //int[] siteValue  =  CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO1", "Cap", daq);
                            int[] siteValue1 = CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO0", "Cap", daq);
                            int[] siteValue2 = CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO25", "Cap1", daq);
                            int[] siteValue3 = CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO24", "Cap2", daq);
                            int[] siteValue4 = CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO23", "Cap3", daq);
                            int[] siteValue5 = CaptureMethodClass.CatchNumberMethod("BT_TEST", "GPIO22", "Cap4", daq);
                            // int[] siteValue1 = CaptureMethodClass.CatchNumberMethod("RAM_20", "A01", "Cap1", daq);
                            List<SiteInfo>siteInfos=daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
                            for (int siteIndex = 0; siteIndex < siteInfos.Count; siteIndex++)
                            {
                                int site = siteInfos[siteIndex].Site;
                               //daq.PostTextLog($"site:{site}:value{siteValue[siteIndex]}\n");//打印
                               daq.PostTextLog($"site:{site}:value{siteValue1[siteIndex]}\n");
                               daq.PostTextLog($"site:{site}:value{siteValue2[siteIndex]}\n");
                               daq.PostTextLog($"site:{site}:value{siteValue3[siteIndex]}\n");
                               daq.PostTextLog($"site:{site}:value{siteValue4[siteIndex]}\n");
                               daq.PostTextLog($"site:{site}:value{siteValue5[siteIndex]}\n");


                            } 
                            for (int siteIndex = 0; siteIndex < siteValue1.Length;siteIndex++)
                            {

                                string str ="";
                                str += siteValue1[siteIndex].ToString();
                                str += siteValue2[siteIndex].ToString();
                                str += siteValue3[siteIndex].ToString();
                                str += siteValue4[siteIndex].ToString();
                                str += siteValue5[siteIndex].ToString();
                                daq.PostTextLog(str);

                                int site = siteIndex;
                                int value = Convert.ToInt32(str,2);

                                daq.PostTextLog("site"+site+":"+"value" +value);
                                DpsForce0V();
                                Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open,true,daq);
                                NewRelay.GetRelayCmdByPinNameNew(DPS_3V3,StatusFlag.FPGA,true,daq,false);
                                Dps.GetDpsPinNameForceCmd(DPS_3V3,DpsMode.FVMI,3.3,DpsRange._60mA,true,daq);
                                Measure.GetMeasureValueByPinName(DPS_3V3,MeasureMode.Supply_Leakage_200uA,0,0.00003,true,daq);



                                // Console.WriteLine(Convert.ToInt32(str,2));
                          }  



                        }
                        break;*/



                //       private static List<ChannelInfo> CalcCurrentFOREACH(int count, daq daq, string[] pinNames, MeasureMode measureMode,double min,double max)

                //      {
                //      var siteInfos = daq.ChanneldaqHandler.GetSlotSiteInfo(daq.TcpNumber);
                //      var channelInfos = siteInfos.SelectMany(t => t.ChannelInfoList.Where(c => pinNames.Contains(c.SignalName))).ToList();

                //     //daq.PostTextLog($"Sites: {string.Join(",",siteInfos.Select(t=>t.Site))}");
                //    //daq.PostTextLog($"channels: {string.Join(",", channelInfos.Select(t => t.Channel))}");

                //     channelInfos.ForEach(t => t.daq = 0);
                //     for (int i = 0; i < count; i++)
                //      {
                //     double[] values = Measure.GetMeasureBackChannelValue(measureMode, daq);
                //     for (int j = 0; j < channelInfos.Count; j++)
                //     {
                //     var channelInfo = channelInfos[j];
                //     int index = channelInfos[j].Channel - 1;
                //     // daq.PostTextLog( "dps: "+index.ToString()+" value: "+channelInfos[j].daq.ToString());
                //     channelInfos[j].daq = channelInfos[j].daq + values[index];
                //     // daq.PostPtr(channelInfos[j].Site+1, "testdps"+j, 99, max, min, values[index], "A");
                //     // channelInfos[j].daq = channelInfos[j].daq + values[index];
                //     daq.PostTextLog("dps: " + index.ToString() + " site: " + channelInfo.Site + " value: " + values[index]);  // 在txt中打印 各site 每次测试的值
                //     daq.PostPtr(channelInfo.Site + 1, $"testdps_{channelInfo.SignalName}_{i}", 100, max, min, values[index], "A");// 在csv和stdf中打印 各site 每次测试的值
                //     }

                //     }
                //     foreach (var item in channelInfos)
                //      {
                //     item.daq = item.daq / count;
                //     daq.PostTextLog("dps: " + (item.Channel-1).ToString() + " avg_value: " + item.daq);  // 在txt中打印 各site 均值
                //     daq.PostPtr(item.Site+1,$"testdpsaverage_{item.SignalName}",100, max,min, item.daq,"A");// 在csv和stdf中打印各site 均值
                //     if (item.daq<min|| item.daq>max)
                //     {
                //     item.Parent.LastSiteState = false;
                //     item.Parent.CurrentSiteState = false;
                //     }
                //     }
                //     return channelInfos;
                //     }


                //        default: break;
                //    }
                //}
                /*      case "DPS_FV/MV_temp":
                  {
                      result.PostTextLog("DPS_FVMV_1V_ch1_temp\n");
                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);	
                      Thread.Sleep(10);
                      DpsRangeRelay.SetDpsRangeRelayBySignals(dps_ch1, DPSStatusFlag._60MA, result);
                      Thread.Sleep(10);
                      Dps.GetDpsPinNameForceCmd(dps_ch1, DpsMode.FVMV, 1, DpsRange._60mA, true, result);
                      Thread.Sleep(10);

                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);				
                      double[] temp = Dps.ReadDpsTemp(true,result);
                      result.PostTextLog($"DPS_FVNV_temp: {temp:F3} \n");


                      result.PostTextLog("DPS_FVMI_0p5V_ch1_temp\n");
                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);
                      DpsRangeRelay.SetDpsRangeRelayBySignals(dps_ch1, DPSStatusFlag._60MA, result);
                      Thread.Sleep(10);
                      Dps.GetDpsPinNameForceCmd(dps_ch1, DpsMode.FVMI, 0.5, DpsRange._60mA, true, result);
                      Thread.Sleep(10);

                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);

                      double[] temp2 =Dps.ReadDpsTemp(true,result);					
                      result.PostTextLog($"DPS_FVMI_0p5V_ch1_temp: {temp2:F3} \n");



                      result.PostTextLog("DPS_FIMV_50mA_ch1_temp\n");
                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);
                      DpsRangeRelay.SetDpsRangeRelayBySignals(dps_ch1, DPSStatusFlag._60MA, result);
                      Thread.Sleep(10);
                      Dps.GetDpsPinNameForceCmd(dps_ch1, DpsMode.FIMV, 0.05, DpsRange._60mA, true, result);
                      Thread.Sleep(10);

                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);
                      double[] temp3 =Dps.ReadDpsTemp(true,result);					
                      result.PostTextLog($"DPS_FIMV_50mA_ch1_temp: {temp3:F3} \n");



                      result.PostTextLog("DPS_FZMI_2p5V_ch1_temp\n");
                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMV, 0.0, DpsRange._5uA, true, result);
                      DpsRangeRelay.SetDpsRangeRelayBySignals(dps_ch1, DPSStatusFlag._60MA, result);
                      Thread.Sleep(10);
                      Dps.GetDpsPinNameForceCmd(dps_ch1, DpsMode.FZMI, 2.5, DpsRange._60mA, true, result);
                      Thread.Sleep(10);

                      DpsRangeRelay.SetDpsRangeRelayBySignals(allDpsPins, DPSStatusFlag.OPEN, result);
                      Dps.GetDpsPinNameForceCmd(allDpsPins, DpsMode.FZMI, 0.0, DpsRange._5uA, true, result);
                      double[] temp4 =Dps.ReadDpsTemp(true,result);					
                      result.PostTextLog($"DPS_FZMI_2p5v_ch1_temp: {temp4:F3} \n");
                  }
                  break;

                  case "PMU_temp":
                  {
                      result.PostTextLog("PMU_FVMV_1V_temp\n");
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);
                      Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, result);	
                      Thread.Sleep(10);
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FVMV, 1, PpmuRange._60mA, true, result);
                      Thread.Sleep(10);                  
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);				   			
                      double[] temp = Ppmu.ReadPpmuTemp(true,result);
                      result.PostTextLog($"PMU_FVMV_1V_ch1_temp: {temp:F3} \n");


                      result.PostTextLog("PMU_FVMI_0P5V_temp\n");
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);
                      Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, result);	
                      Thread.Sleep(10);
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FVMI, 0.5, PpmuRange._60mA, true, result);
                      Thread.Sleep(10);                  
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);


                      double[] temp2 = Ppmu.ReadPpmuTemp(true,result);
                      result.PostTextLog($"PMU_FVMI_0P5V_temp: {temp2:F3} \n");


                      result.PostTextLog("PMU_FIMV_50mA_temp\n");
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);
                      Relay.GetRelayCmdAllChannel(RelayStatusMode.All_CH_To_Open, true, result);	
                      Thread.Sleep(10);
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FIMV, 0.05, PpmuRange._60mA, true, result);
                      Thread.Sleep(10);                  
                      Ppmu.GetPpmuPinNameForceCmd(allPmuPins, PpmuMode.FZMV, 0.0, PpmuRange._5uA, true, result);


                      double[] temp3 = Ppmu.ReadPpmuTemp(true,result);
                      result.PostTextLog($"PMU_FIMV_50mA_temp: {temp3:F3} \n");
                  }
  */




                case "FlowJumpTest":
                    {
                        ItemNode nodeInfo_1 = new ItemNode();//定义结点
                        nodeInfo_1.NodeNo = 1;
                        nodeInfo_1.PassSiteNo = 2;
                        nodeInfo_1.FailSiteNo = 3;
                        nodeInfo_1.TestCase = TestAction_1;
                        nodeInfo_1.NodeName = "node_1";
                        nodeInfo_1.NodeType = NodeType.ForkPoint;

                        ItemNode nodeInfo_2 = new ItemNode();//定义结点
                        nodeInfo_2.NodeNo = 2;
                        nodeInfo_2.PassSiteNo = 4;
                        nodeInfo_2.FailSiteNo = 5;
                        nodeInfo_2.TestCase = TestAction_2;
                        nodeInfo_2.NodeName = "node_2";
                        nodeInfo_2.NodeType = NodeType.ForkPoint;

                        ItemNode nodeInfo_4 = new ItemNode(4, true, TestAction_4, NodeType.IsolatedPoint);//定义孤点

                        ItemNode nodeInfo_5 = new ItemNode();//定义结点
                        nodeInfo_5.NodeNo = 5;
                        nodeInfo_5.PassSiteNo = 7;
                        nodeInfo_5.FailSiteNo = 8;
                        nodeInfo_5.TestCase = TestAction_5;
                        nodeInfo_5.NodeName = "node_5";
                        nodeInfo_5.NodeType = NodeType.ForkPoint;

                        ItemNode nodeInfo_7 = new ItemNode(7, true, TestAction_7, NodeType.IsolatedPoint);//定义孤点

                        ItemNode nodeInfo_8 = new ItemNode(8, true, TestAction_8, NodeType.IsolatedPoint);//定义孤点

                        ItemNode nodeInfo_3 = new ItemNode();//定义结点
                        nodeInfo_3.NodeNo = 3;
                        nodeInfo_3.PassSiteNo = 6;
                        nodeInfo_3.FailSiteNo = 6;
                        nodeInfo_3.TestCase = TestAction_3;
                        nodeInfo_3.NodeName = "node_3";
                        nodeInfo_3.NodeType = NodeType.ForkPoint;

                        ItemNode nodeInfo_6 = new ItemNode(6, true, TestAction_6, NodeType.IsolatedPoint);//定义孤点

                        List<ItemNode> nodeInfos = new List<ItemNode>();//按照前序遍历的顺序存储每一个结点
                        nodeInfos.Add(nodeInfo_1);//结点一
                        nodeInfos.Add(nodeInfo_2);//结点二
                        nodeInfos.Add(nodeInfo_4);//结点四
                        nodeInfos.Add(nodeInfo_5);//结点五
                        nodeInfos.Add(nodeInfo_7);//结点七
                        nodeInfos.Add(nodeInfo_8);//结点八
                        nodeInfos.Add(nodeInfo_3);//结点三
                        nodeInfos.Add(nodeInfo_6);//结点六

                        List<SiteInfo> siteInfos = daq.ChannelMapProvider.GetSiteInfosBySlotNo(daq.TcpNumber);//查找Site
                        FlowNode flowNode = new FlowNode(daq, nodeInfos, siteInfos);
                        string msg = flowNode.Start();//启动跳转
                        string nodeStatus = flowNode.GetSiteNodeSiteNoMsg();//获取每个结点的Site状态信息
                        int[] failSiteArray = flowNode.GetFailSites();       //获取跳转执行结束后的 fail site
                        daq.UpdateSiteAndChannelState(failSiteArray, null);//查找Fail的site后更新到map
                        daq.PostTextLog(msg);
                    }
                    break;
                default: break;
            }
            //int[] failSites = daq.FindFailSite();
            //daq.BinHandle(failSites, StdfItemName[53]);
        }



        private void TestAction_1(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点一执行的方法
        {
            passSites.AddRange(new int[] { 0, 1, 2, 3 }.ToList());
            failSites.AddRange(new int[] { 4, 5, 6, 7 }.ToList());
        }

        private void TestAction_2(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点二执行的方法
        {
            passSites.AddRange(new int[] { 0 }.ToList());
            failSites.AddRange(new int[] { 1, 2, 3, }.ToList());
        }

        private void TestAction_4(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点四执行的方法
        {
            passSites.AddRange(new int[] { 0 }.ToList());
            failSites.AddRange(new int[] { }.ToList());
        }

        private void TestAction_5(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点五执行的方法
        {
            passSites.AddRange(new int[] { 1 }.ToList());
            failSites.AddRange(new int[] { 2, 3 }.ToList());
        }

        private void TestAction_7(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点七执行的方法
        {
            passSites.AddRange(new int[] { 1 }.ToList());
            failSites.AddRange(new int[] { 2, 3 }.ToList());
        }

        private void TestAction_8(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点八执行的方法
        {
            passSites.AddRange(new int[] { 3 }.ToList());
            failSites.AddRange(new int[] { 2 }.ToList());
        }

        private void TestAction_3(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点三执行的方法
        {
            passSites.AddRange(new int[] { 4, 5, 6, 7 }.ToList());
        }

        private void TestAction_6(FlowDAQ daq, List<int> passSites, List<int> failSites)//结点六执行的方法
        {
            passSites.AddRange(new int[] { 4, 5 }.ToList());
            failSites.AddRange(new int[] { 6, 7 }.ToList());
        }

        /*
        private static void LogPost（string testItem,string log, IResult result)
        {
            DataInfo dataInfo = new DataInfo();
            dataInfo.SiteNumber = 1;
            dataInfo.Result = "-----------------TEST_ITEM  :  " + testItem + "----------------------";
            dataInfo.TestValue = "\n"+log;
            result.PostTXTData(dataInfo);
        }
        */
    }
}






//public override string ToString()
//        {
//            return base.ToString();
//        }

//        public override bool Equals(object obj)
//        {
//            return base.Equals(obj);
//        }

//        public override int GetHashCode()
//        {
//            return base.GetHashCode();
//        }

//        // public override void TestCase(FlowDAQ daq, string name, int binCode)
//        public override void TestCase(FlowDAQ daq, string name, int binCode)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}


