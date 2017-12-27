using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor
 *文件名：  CDefine
 *版本号：  V1.0.0.0
 *唯一标识：c13fd415-c509-42d1-ae21-2ca8181a8fd9
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/22/2016 7:34:03 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/22/2016 7:34:03 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace MatrixSystemEditor
{

    public class MatrixCMD
    {

        public const int F_PCGetDeviceInfo = 12;
        public const int F_ScanDeviceID = 3;
        //public const int F_InputPhaseGain = 65;
        //public const int F_OutputPhaseGain = 66;
        public const int F_InputExpGATE = 67;
        public const int F_OutputExpGATE = 68;
        public const int F_InputEQ = 69;
        public const int F_OutputEQ = 70;
        public const int F_InputCOMP = 71;
        public const int F_OutputCOMP = 72;
        public const int F_InputDelay = 73;
        public const int F_OutputDelay = 74;
        public const int F_DuckerParameter = 75;
        public const int F_DuckerInputMixer = 76;
        public const int F_DuckerGainInsert = 100;


        public const int F_RDPortOutSourceSet = 77;
        public const int F_MatrixMixer = 78;
        public const int F_PagingMixer = 79;
        public const int F_ReadStatus = 80;
        public const int F_InputDG411Gain = 81; //sensitivity
        public const int F_Ack = 82;
        public const int F_InputEQFlat = 83;
        public const int F_OutEQFlat = 84;
        public const int F_Copy = 85;
        public const int F_InputDC48VFlag = 86;
        public const int F_AutoMixerSetting = 87;



        //Reserve 87
        public const int F_WrDevInfo = 88;
        public const int F_RdDevInfo = 89;
        public const int F_ReChName = 90;//rename channel name
        //Reserve 90
        public const int F_StoreSinglePreset = 91;
        public const int F_RecallSinglePreset = 92;
        //Matrix cmd define below:
        public const int F_LoadFromPC = 93;
        public const int F_RDDefaultSetting = 94;
        public const int F_ReturnDefaultSetting = 95;

        public const int F_RecallCurrentScene = 96;
        public const int F_GetPresetList = 97;

        public const int F_DeleteSinglePreset = 98;
        public const int F_SigMeters = 99;
        //Reserve 100
        public const int F_Mute = 101;

        public const int F_MemoryExport = 102;
        public const int F_MemoryImport = 105;
        public const int F_MemoryImportAck = 106;

        public const int F_WrDevSerialNO = 107;
        public const int F_SetPageZoneIndex = 108;
        public const int F_RelayControl = 109;

        public const int F_FBCSetting = 110;
        public const int F_GetFBCStatus = 111;
        public const int F_FBCSetup = 112;
        public const int MSG_TCP_ParaConnected = 700;

        

        

        //public const int F_FBCFilterStatus = 113;


        //public const int F_FBCFilterGain_F08 = 114;
        //public const int F_FBCFilterGain_F16 = 115;
        //public const int F_FBCFilterGain_F24 = 116;
        //public const int F_FBCFilterFreq_F08 = 117;
        //public const int F_FBCFilterFreq_F16 = 118;
        //public const int F_FBCFilterFreq_F24 = 119;

        public const int F_InpuGain = 120;
        //  public const int F_GetInpuGain = 121;

        public const int F_InpuPhase = 122;
        // public const int F_GetInpuPhase = 123;

        public const int F_OutputGain = 124;
        // public const int F_GetOutputGain = 125;

        public const int F_OutputPhase = 126;
        // public const int F_GetOutputPhase = 127;

        // public const int F_GetInpuDelay = 128;
        //  public const int F_GetOutputDelay = 129;

        //  public const int F_GetMatrixMixer = 130;

        // public const int F_GetInMeter = 131;
        // public const int F_GetOutMeter = 132;
        public const int F_BGMSelect = 133;

        public const int F_AutoMixerCHSelect = 134;

        public const int F_ResetToFactorySetting = 135;

        public const int F_ReadLockPWD = 137;
        public const int F_WriteLockPWD = 138;

        //PC Communication ,MCU IAP Command
        public const int IAP_MODE_EXIT = 0XE0;
        public const int IAP_PROGRAMING = 0xF0;
        public const int IAP_FINISH = 0xF1;

        public const int IAP_ENTER_IN_APP = 0xEE;		//Enter IAP Mode in application
        public const int IAP_MODE_ENTER_READY = 0xEF;    //IAP Mode Prepare ready, 

        //PC Communication ,DSP Firmware Update Command
        public const int F_DSPFirmwareUpdate = 0xf2;     //DSP Firmware update 
        public const int F_UpdateProgress = 0xf3;
        public const int F_ReadDSPCode = 0xf4;
        public const int F_SendMCUStart = 0xf5;

        #region Message Send CMD Define


        public static string MatrixGUIClass = "MatrixPage";
        public static string LoadLEDGUIClass = "LoadLEDForm";


        public const int MatrixA8_MSG_Transfer = 0x501;

        public const int LoadLed_MSG_Transfer = 0x707;

        public const int MainWindow_MSG_Transfer = 0x601;
        public const int WM_Msg_Exit = 0x602;

        public static string RVAGUIClass = "RVADevPage";
        public const int RVA200_MSG_Transfer = 0x500;


        public static string RPMGUIClass = "RPM200Page";
        public const int RPM200_MSG_Transfer = 0x504;


        //RVCSeriesWnd
        public static string RVCUIClass = "RVCSeriesWnd";
        public const int RVC1000_MSG_Transfer = 0x508;



        public static string CGNewDevNameGUIClass = "CNewDeveName";  //"CNewDeveName";
        public const int ChangeDevName_MSG_Transfer = 0x502;

        public static string CMainWindowGUIClass = "MainWindow";

        public const int MSG_NoticeDisconnect = 0xFFFD;//notice msg to send matrix to close the device





        #endregion
    }


    public enum RWLockPwdType
    {
        LockPWD_Read = 105,
        LockRWD_Write = 106,
    };


    public class CDefine
    {

        public const int Width_leftScan = -190;

        public const int Matrix_Width = 1060;

        public const int Matrix_Height = 930;

        public const int MaxCHLength = 12;
        public const string DefaultPWD = "MAA8";
        public const string SuperPWD = "MA88";

        #region RPM100 define
        public const int MaxZoneNum = 60;
        #endregion


        #region Ack_status define

        //saveload
        public const int M_LoadfromDevice = 1;
        public const int M_LoadfromLocal = 2;
        public const int M_SaveToDevice = 3;
        public const int M_SaveToLoad = 4;

        //memory import export
        public const int M_MemoryImport = 5;
        public const int M_MemoryExport = 6;

        //reset to defaultsetting
        public const int M_ResetDefaultSetting = 7;
        public const int M_ResetDefaultFactory = 8;

        public const int M_PresetListRefresh = 10;
        public const int M_RecallCursence = 9;

        public const int M_ChangeDeviceName = 12;


        public const int M_ACK = 11;


        #endregion


        public const int DTimesH = 256;
        public const int DTimesL = 100;


        public static string[] strFBCMode = { "Speech", "Music" };
        public static string[] strFBCFilterRelease = { "Fast", "Mid", "Slow" };

        public static string MatrixVer = "v2.4.0";
        public static string MatrixName="Matrix Editor System ";


        public const byte ByteH = 0xf0;
        public const byte ByteL = 0x0f;


        public static string Warning_LowMCUVer = "Has detected the device firmware version is old,\n" +
                 "We recommend that you upgrade your device firmware version.";

        public const int Max_DuckerInputMixer = 12;

        public const int All_Machine_APPID = 0xFFFF;
        public const int AutoMixerChanel = 29;

        public static string[] strAutoReleseTable =
    {
    "10mS",    //00
    "20mS",    //02
    "25mS",    //03
    "50mS",    //04
    "100mS",    //05
    "200mS",   //06
    "300mS",   //07
    "400mS",   //08
    "500mS",   //09
    "600mS",   //10
    "700mS",   //11
    "800mS",   //12
    "900mS",   //13
    "1000mS",   //14
    "1200mS",
    "1500mS",   //15
    "2000mS",   //16
    "2500mS",   //17
    "3000mS",   //18
    "3500mS",  //19
    "4000mS",  //20
    "4500mS",  //21
    "5000mS",  //22
    "5500mS",  //23
    "6000mS",  //24

};

        public const int AP_ScandID = 0xffff;
        public const int AP_Matrix_Main = 0x06;        


        public const int Max_FaderGain = 190;
        public const int DefaultFaderGain = 160;

        public static string MPFFilter = "MatrixPresets File|*.MCSP";

        public const int Max_ExpThreshold = 100;
        public const int MIN_IP_LEN = 8;

        /// <summary>
        /// 
        /// </summary>
        public const int FBC_Filter_Status = 0;
        public const int FBC_Filter_Freq = 1;
        public const int FBC_Filter_Gain = 2;
        //

        public const int Max_Presets = 25;
        public const int Memory_Per_Packeg_len = 4115;



        public const int Len_PresetName = 20;
        public const int Len_FactPName = 16;
        public const int Max_outBus = 24;
        public const int MAX_FBCFilter = 24;

        // public const int LEN_FactCHName = 16;
        public const int LEN_DeviceName = 16;

        public const int FBCChanel = 28;
        public const int FBCLeftChanel = 20;



        public const int Matrix_CTL_NUM = 20;

        public const int Matrix_TNUM = 32;
        //      
        public const int MIN_LEN_PACK = 18;
        public const int LEN_EQFlat_Pack = 19;
        public static string Empty_Presets = "------empty------";

        //package lenth for sendCommand
        public const int Len_SensitivityPack = 30;

        public const int Len_DC48VPack = 30;
        public const int LEN_ComPack = 30;
        public const int LEN_GateExpPack = 26;
        public const int LEN_DelayPack = 22;
        public const int LEN_ChanelEQPack = 27;
        public const int LEN_ChanelDYNPack = 27;
        public const int LEN_MatrixPack = 35;

        public const int LEN_WriteLockPack = 23;

        public static int  Max_Zones = 60;
        public static int Max_ZonDev = 16;
       // public static int Max_DevPort = 12;


        //
        public const int LEN_DuckerMixerPack = 30;
        public const int LEN_DuckerGainPack = 42;

        public const int LEN_DuckerParamPack = 28;

        public const int LEN_FBCStatusPack = 70;
        public const int LEN_FBCSettingPack = 22;
        public const int LEN_FBCSetupPack = 21;

        public const int LEN_ZoneSencePack = (3164); //3882 //prm200

        public const int LEN_RVCSencePack = 75;//rvc1000

        public const int LEN_ACKPack = 18;

        public const int LEN_RecallSinglePresetPack = 19;

        public const int LEN_DeletePack = 19;
        public const int LEN_CopyPack = 32;

        public const int LEN_StoreNamePresetPack = 35;

        public const int LEN_changeChNamePack = 40;
        public const int LEN_Sence = 3649;
        public const int LEN_changeDevNamePack = 38;
        public const int LEN_DelPresetPack = 19;
        public const int Memory_Max_Package = 25;
        public static string FILE_MEMORY_Filter = "Memory File|*.Memy";
        public const int LEN_AutoMixerPack = 24;
        public const int LEN_AutoMixerSelectPack = 30;

        public static int ACK_Loop_Interval = 6;

        //-----
        public static double checkAvalibleInput(double tmp, double MinInput, double MaxInput)
        {
            double tmpmv = 0;
            if (tmp < MinInput)
                tmpmv = MinInput;
            else if (tmp > MaxInput)
                tmpmv = MaxInput;
            else
                tmpmv = tmp;
            tmpmv = input2Value(tmpmv);

            return tmpmv;
        }

        public static double input2Value(double mv)
        {

            int t = 0;
            bool isPog = (mv < 0);

            double x = Math.Abs(Math.Round(mv, 1));
            int a = 0;
            double y = 0;

            a = (int)(x * 10 - (int)x * 10);
            if (a >= 0 && a < 5)
            {
                t = 0;
                a = 0;
            }
            else if (a > 5 && a <= 9)
            {
                t = 1;
                a = 0;
            }
            else  //==5
            {
                t = 0;
                a = 5;
            }
            y = (double)((int)x * 10 + t * 10 + a) / 10;
            if (isPog)
                y = y * -1;

            return y;


        }


    }

}
