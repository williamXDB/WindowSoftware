#define _DEBUG


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLibrary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MatrixSystemEditor.commom;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixUpdate
 *文件名：  CMDSender
 *版本号：  V1.0.0.0
 *唯一标识：c7d701fb-0813-4032-bd4f-102bdd96342e
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/1/2016 4:51:35 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/1/2016 4:51:35 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace MatrixSystemEditor
{
    class CMDSender
    {
        public static void sendCMD_scanDevicesID(Cilent mcilent)
        {
            byte[] cmd_scanDevice = new byte[] { 0x01, 0x20, 0x03, 0x00, 0x0D, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x03, 0x00, 0x40 };

            if (mcilent != null)
                mcilent.sendByte(cmd_scanDevice);
        }

        #region Page Channel:mute,eq,compressor,expgate,delay etc;
        /// <summary>
        /// sendcmd for sensitivity 
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_InputSensitivity(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.Len_SensitivityPack;

            LCommand comcmd = new LCommand(count, MatrixCMD.F_InputDG411Gain);//sensitivity define
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        ///
        /// <summary>
        /// sendcmd for sensitivity 
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_InputDc48V(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.Len_DC48VPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_InputDC48VFlag);//sensitivity define
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        ///
        /// <summary>
        /// sendcmd for phaseGain,input or output
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_Phase(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ComPack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InpuPhase);//sensitivity define
            else if (flag == 1)
                comcmd = new LCommand(count, MatrixCMD.F_OutputPhase);//sensitivity define

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }


        public static void sendcmd_RWLockPwd(byte[] mData, RWLockPwdType rwtype, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_WriteLockPack;
            LCommand comcmd = null;
            if (rwtype == RWLockPwdType.LockPWD_Read)
                comcmd = new LCommand(count, MatrixCMD.F_ReadLockPWD);//sensitivity define
            else if (rwtype == RWLockPwdType.LockRWD_Write)
                comcmd = new LCommand(count, MatrixCMD.F_WriteLockPWD);//sensitivity define

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        ///
        /// <summary>
        /// sendcmd for phaseGain,input or output
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_FaderGain(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ComPack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InpuGain);//sensitivity define
            else if (flag == 1)
                comcmd = new LCommand(count, MatrixCMD.F_OutputGain);//sensitivity define

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;

#if _DEBUG
            IPProces.printAryByte("send chanel fader gain cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        /// <summary>
        /// sendcmd for phaseGain,input or output
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_ChanelMute(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ComPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_Mute);//channel mute   


            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }


        /// <summary>
        /// sendcmd for input GateExp 
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_InputGateExp(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_GateExpPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_InputExpGATE);//channel mute 
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);


            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// sendcmd  input/output delay 
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_Delay(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_DelayPack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InputDelay);//
            else
                comcmd = new LCommand(count, MatrixCMD.F_OutputDelay);//

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// sendcmd for input/output channel EQ
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_ChanelEQ(byte[] meqData, int flag, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_ChanelEQPack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InputEQ);//
            else
                comcmd = new LCommand(count, MatrixCMD.F_OutputEQ);//

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(meqData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //--------------------------------------- 
        /// <summary>
        /// send cmd eq all bypas
        /// </summary>
        public static void sendCMD_EQ_Flat(int findex, int flag,
            Cilent mcilent, DeviceProvision devp)  //chindex :all eq will be setflat 
        {

            int count = CDefine.LEN_EQFlat_Pack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InputEQFlat);
            else
                comcmd = new LCommand(count, MatrixCMD.F_OutEQFlat);
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];

            m_Data = comcmd.getPackage_withoutDataArray(findex + 1);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //---------------------------------------  

        /// <summary>
        /// sendcmd for input/output channel compressor(DYN)
        /// </summary>
        /// <param name="mcilent"></param>
        public static void sendCMD_ChanelDYN(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_ChanelDYNPack;
            LCommand comcmd = null;
            if (flag == 0)
                comcmd = new LCommand(count, MatrixCMD.F_InputCOMP);//
            else
                comcmd = new LCommand(count, MatrixCMD.F_OutputCOMP);//
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;


#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        #endregion

        public static void sendCMD_AutoMixerSelect(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_AutoMixerSelectPack;

            LCommand comcmd = new LCommand(count, MatrixCMD.F_AutoMixerCHSelect);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("sendcmd automixer cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }


        }

        public static void sendCMD_AutoMixerSetting(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_AutoMixerPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_AutoMixerSetting);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }


        /// <summary>
        /// matrix mixer cmd send here....
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="flag"></param>
        /// <param name="mcilent"></param>
        /// <param name="apID"></param>
        /// <param name="devID"></param>
        public static void sendCMD_MatrixMixer(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_MatrixPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_MatrixMixer);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        #region Ducker mixer,ducker parameter about.....

        /// <summary>
        /// sendcmd for ducker mixer
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="apID"></param>
        /// <param name="devID"></param>
        public static void sendCMD_DuckerMixer(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_DuckerMixerPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_DuckerInputMixer);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_DuckerGain(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_DuckerGainPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_DuckerGainInsert);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        public static void sendCMD_DuckerParam(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_DuckerParamPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_DuckerParameter);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        #endregion ducker about cmd send over

        /// <summary>
        /// pagezone
        /// </summary>
        /// <param name="zonindex"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_Pagezone(int zonindex, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_DuckerParamPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_SetPageZoneIndex);//           

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray(zonindex);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// fbc setting cmd 
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_FBCSetting(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_FBCSettingPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_FBCSetting);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        
        public static void sendCMD_RVCSence(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_RVCSencePack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_LoadFromPC);//          

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];

            m_Data = comcmd.getPackage_withDataArray(mData);

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("send rvc100 sence command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_RPMSence(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ZoneSencePack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_LoadFromPC);//            

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];

            m_Data = comcmd.getPackage_withDataArray(mData);

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("send rpm100 sence command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// fbc reset clear
        /// </summary>
        /// <param name="mreset"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_FBCSetup(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_FBCSetupPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_FBCSetup);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];

            m_Data = comcmd.getPackage_withDataArray(mData);

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        /// <summary>
        /// ack cmd define............
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="apID"></param>
        /// <param name="devID"></param>
        public static void sendCMD_Ack(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_Ack);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("ack command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }
        }

        public static void sendCMD_Copy(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_CopyPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_Copy);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];

            ///---------------------------------------
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// delete preset from device presetlist....
        /// </summary>
        /// <param name="preindex">the preset index,but from 1 begin,becase the 0 is default not promised access</param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_delPrsetWithindex(int preindex, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_DelPresetPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_DeleteSinglePreset);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray(preindex);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// read Deivce info
        /// </summary>
        public static void sendCMD_readDevices(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_RdDevInfo);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("send read deive command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_RelayControl(byte[] mData, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_CopyPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_RelayControl);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mData);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }

            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdata"></param>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_ChangeChannelName(byte[] mdata, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_changeChNamePack;
            byte chnameCMD = MatrixCMD.F_ReChName;
            LCommand comcmd = new LCommand(count, chnameCMD);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mdata);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }

            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("change device name cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }


        public static void sendCMD_ChangeDeviceName(byte[] mdata, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_changeDevNamePack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_WrDevInfo);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(mdata);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("change device name cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //---------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mcilent"></param>
        /// <param name="apID"></param>
        /// <param name="devID"></param>
        public static void sendCMD_ResetToDefaultSetting(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_ReturnDefaultSetting);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// reset to default factory
        /// </summary>
        /// <param name="mcilent"></param>
        /// <param name="devp"></param>
        public static void sendCMD_ResetToDefaultFactory(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_ResetToFactorySetting);//  

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //---------------------------------------
        public static void sendCMD_ReadPresetList(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_GetPresetList);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        /// <summary>
        /// read Deivce info
        /// </summary>
        public static void sendCMD_RecallCurrentScene(Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.MIN_LEN_PACK; //18
            LCommand comcmd = new LCommand(count, MatrixCMD.F_RecallCurrentScene);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("send recall current sence command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //---------------------------------------
        public static void sendCMD_RecallSinglePreset(int preindex, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_RecallSinglePresetPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_RecallSinglePreset);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray(preindex);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        /// <summary>
        /// delte single preset
        /// </summary>
        /// <param name="preNum"></param>
        /// <param name="mcilent"></param>
        /// <param name="apID"></param>
        /// <param name="devID"></param>
        public static void sendCMD_DeleteSinglePreset(int preNum, Cilent mcilent, DeviceProvision devp)
        {

            int count = CDefine.LEN_DeletePack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_DeleteSinglePreset);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray(preNum);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);
            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }
        //---------------------------------------
        public static void sendCMD_LoadPresetFromPC(byte[] m_data, Cilent mcilent, DeviceProvision devp)
        {
            int count = CDefine.LEN_ACKPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_LoadFromPC);//  
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(m_data);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];
            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        //
        /// <summary>
        /// send cmd import memory from local
        /// </summary>
        public static void sendCMD_MemoryImportFromPC(byte[] mdata, Cilent mcilent,
            DeviceProvision devp)  //chindex :0..2
        {

            int count = CDefine.Memory_Per_Packeg_len;

            LCommand comcmd = new LCommand(count, MatrixCMD.F_MemoryImport);

            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataMemory(mdata);
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];
            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            //   IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        //
        public static void sendCMD_MemoryExportFromDevice(Cilent mcilent,
          DeviceProvision devp)  //chindex :0..2
        {

            int count = CDefine.MIN_LEN_PACK; //18
            LCommand comcmd = new LCommand(count, MatrixCMD.F_MemoryExport);
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withoutDataArray();
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];
            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }

        }

        //

        /// <summary>
        /// send cmd save user/factory preset with data(name)
        /// 0:user model,1:factory model
        /// </summary>
        public static void sendCMD_SavePresetWithName(byte[] prestDatas, Cilent mcilent,
            DeviceProvision devp)//
        {
            int count = CDefine.LEN_StoreNamePresetPack;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_StoreSinglePreset);
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(prestDatas);//preset name
            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("command cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }


        }

        /// <summary>
        /// send cmd save user/factory preset with data(name)
        /// 0:user model,1:factory model
        /// </summary>
        public static void sendCMD_loadFromLocalToDevice(byte[] localDatas, Cilent mcilent,
            DeviceProvision devp)//
        {
            int count = CDefine.LEN_Sence;
            LCommand comcmd = new LCommand(count, MatrixCMD.F_LoadFromPC);
            comcmd.ID_Machine = devp.pMachineID;
            comcmd.Device_ID = devp.pDeviceID;
            byte[] m_Data = new byte[count];
            m_Data = comcmd.getPackage_withDataArray(localDatas);//preset name

            Array.Copy(devp.pDevSNONumber, 0, m_Data, m_Data.Length - 7, 5);

            byte checkSum = m_Data[0];
            for (int i = 1; i < count; i++)
            {
                if (i != count - 2)
                    checkSum ^= m_Data[i];

            }
            m_Data[count - 2] = checkSum;
#if _DEBUG
            IPProces.printAryByte("send for load preset from local as cmd byte:-- ", m_Data);
#endif
            if (mcilent != null)
            {
                mcilent.sendByte(m_Data);
            }


        }

        #region Message Send
        //---------------------------------------Message sender below:
        /// <summary>
        /// sendMessage to translate data
        /// </summary>
        public static void sendMsgWithData(string strClass, int MSG, byte[] tdata, int hCMD, int lCMD = 0)
        {
            if (tdata == null)
            {
                Debug.WriteLine("data is null array ,so return");
                return;
            }

            IntPtr handle = MessageHelper.FindWindow(strClass, null);
            if (handle == IntPtr.Zero)
            {
                // MessageBox.Show("No find the window");
                Debug.WriteLine("No find the window,or the handle is null");
                return;
            }

            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            //byte[] amp = { 0x10, 0x02, 0x03, 0x33, 0x55, 0x78 };
            //cds.dLength = amp.Length;

            cds.dLength = tdata.Length;
            cds.preWph = hCMD;
            cds.preWpl = lCMD;

            IntPtr tmp = Marshal.AllocHGlobal(tdata.Length);
            cds.hData = tmp;
            Marshal.Copy(tdata, 0, cds.hData, tdata.Length);
            MessageHelper.SendMessage(handle, MSG, IntPtr.Zero, ref cds);
            Marshal.FreeHGlobal(tmp);

        }
        //
        /// <summary>
        /// sendMessage to translate ,without data
        /// </summary>


        public static void sendMsgWithoutData(string strClass, int MSG, int hCMD, int lCMD)
        {
            IntPtr handle = MessageHelper.FindWindow(strClass, null);
            if (handle == IntPtr.Zero)
            {
                // MessageBox.Show("No find the window");
                Debug.WriteLine("No find the window, " + strClass + "   the handle is null");
                return;
            }
            // Debug.WriteLine("window name " + strClass + "  not null ");
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.preWph = hCMD;
            cds.preWpl = lCMD;
            cds.hData = IntPtr.Zero;
            MessageHelper.SendMessage(handle, MSG, IntPtr.Zero, ref cds);
        }

        public static void sendMSG_note_ChangeDevName(int hcmd, int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.CGNewDevNameGUIClass, MatrixCMD.ChangeDevName_MSG_Transfer, hcmd, lcmd);

        }

        public static void sendMSG_note_exitMainWindow(int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.CMainWindowGUIClass, MatrixCMD.MainWindow_MSG_Transfer, MatrixCMD.WM_Msg_Exit, lcmd);
        }

        #endregion


    }


}

