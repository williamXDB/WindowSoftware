using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Controls;
using CommLibrary;
using System.Diagnostics;
using System.Windows.Threading;
using MatrixSystemEditor.commom;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.Matrix
 *文件名：  CMatrixData
 *版本号：  V1.0.0.0
 *唯一标识：ef0e6863-8680-4232-b805-e719338e8ad9
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/9/2016 7:16:33 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/9/2016 7:16:33 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace MatrixSystemEditor.Matrix
{
    public class CMatrixData
    {

        public const byte StandMCUVer = 0x03;

        public enum DUPM
        {
            Duck_Thres = 0,
            Duck_Attack = 1,
            Duck_Relese = 2,
            Duck_Depth = 3,
            Duck_Hold = 4,
            Duck_Power = 5,

        };


        #region define for RPM200 define
        public IORPM200 m_RPMCover;
        public IORVC1000 m_RVCover;



        #endregion

        public const byte SHIFT = 4;
        public const byte HByte = 0xf0;
        public const byte LByte = 0x0f;
        public CAutoMixerParam autoMixerParam = new CAutoMixerParam();

        public class DevNamInfo
        {
            public int apID;
            public int devID;
            public String devName;

            public DevNamInfo(String sname, int sid)
            {
                apID = 0;
                devID = sid;
                devName = sname;
            }

            public String getDevNamID()
            {
                String sd = devName + " - " + devID.ToString("X2");
                return sd;
            }
            public String getStrHexID()
            {
                return devID.ToString("X2");
            }
        };
        public DeviceProvision activeDevProvision = new DeviceProvision();
        public List<DevNamInfo> m_A8DevList;

        public String getStrDevID(int index)
        {
            return m_A8DevList[index].getStrHexID();
        }
        public String[] gStrA8DevList()
        {
            int sz = m_A8DevList.Count;

            String[] str = new String[sz];

            for (int i = 0; i < sz; i++)
            {
                str[i] = m_A8DevList[i].getDevNamID();
            }
            return str;
        }

        public CommunicateStatus m_CommuStatus = new CommunicateStatus();

        public void resetCommunicateStatus()
        {
            m_CommuStatus.responseAckCounter = 2;
            m_CommuStatus.commuteStatus = ACK_Status.M_ConectedNormal;
        }
        public int getSpecialRecall(int fbNum)
        {
            int resAck = fbNum + 1;
            int k = 0;
            int mout;
            int fnum = devIDDiffNum(out mout);
            if (fnum >= 256)
            {
                k = mout;
                if (k < 9)
                    resAck = fbNum + (k + 1);
                else
                    resAck = fbNum + (k + 3);
            }
            Debug.WriteLine("----------------------------------------get special ack timer number is  " + resAck.ToString());
            return resAck;
        }




        public int getNormalAckTimeNum(int fbNum)
        {
            int resAck = fbNum;
            int k = 0;
            int mout;
            int fnum = devIDDiffNum(out mout);
            if (fnum >= 256)
            {
                k = mout;
                if (k < 9)
                    resAck = fbNum + 1;
                else
                    resAck = fbNum + 2;
            }
            Debug.WriteLine("----------------------------------------get normal ack timer number is  " + resAck.ToString());
            return resAck;
        }


        public void setupCommunicateACKStatus()
        {
            if (m_CommuStatus.commuteStatus == CDefine.M_ACK || m_CommuStatus.commuteStatus == ACK_Status.M_ConectedNormal)
            {
                m_CommuStatus.responseAckCounter = getNormalAckTimeNum(2);
                m_CommuStatus.commuteStatus = CDefine.M_ACK;
            }

        }

        public byte m_RVAEsStatus { get; set; }

        public bool isLowFirmware()
        {
            return (m_MCUVer < StandMCUVer);
        }
        /// <summary>
        /// channel data,input12+output12 channel
        /// </summary>
        public CHEdit[] m_ChanelEdit = new CHEdit[CFinal.ChanelMax * 2];



        public CorprationInfo corpInfo = new CorprationInfo();

        public void readAppInfo()
        {
            corpInfo.ProductName = CDefine.MatrixName;
            corpInfo.ProductVer = CDefine.MatrixVer;
        }

        public static CMatrixData matrixData = null;//initial instance




        public byte[] DeviceSerialNo = new byte[CFinal.MAX_SERIALNO];


        public byte[][] m_matrixAry = new byte[CDefine.Matrix_TNUM][];//32*32

        public byte[] m_autoMixerCHSelect = new byte[CFinal.ChanelMax * 2];

        public byte[][] m_copy = new byte[2][];
        public byte[] m_FBCClearFlag = new byte[2];//FBC dynamicFilterclearFlag,FBCAllFilterClearFlag

        #region Ducker source input part


        public byte[] m_DuckerSourch = new byte[CFinal.IO_MaxMatrixBus];
        public byte[] m_DuckerBgm = new byte[CFinal.IO_MaxMatrixBus];

        #endregion
        //ducker:threshold,depth,attack,hold,release,powerOn
        public int[] m_duckerParameters = new int[CFinal.Max_DuckerParms];
        //FBC
        #region FBC Data Define

        public EQEdit[] m_FbcEQData = new EQEdit[FFTConstaint.FBC_FFTNum];
        public const int FBCParamLen = 6;
        public byte[] m_FbcParam = new byte[FBCParamLen];
        //fbc_bypas,fbc_gain,fbc_staticFilterSetttingFlag,fbcqvalue(preserved),FBC_ModeFlag,FBCFilterReleaseTime



        public byte[] m_FbcFilterStatus = new byte[CDefine.MAX_FBCFilter]; //fbc status:24
        public byte m_fbcNextStep = 0;




        public int m_devRVA_ID { get; set; }


        private static DispatcherTimer scanIDtimer = new DispatcherTimer();

        public void resetFBCData()
        {
            Array.Clear(m_FbcFilterStatus, 0, CDefine.MAX_FBCFilter);
            setFBC_EQFlat();
            Array.Clear(m_FbcParam, 0, FBCParamLen);
        }
        //
        public void resetFBCWhenSetup_Pressed()
        {
            m_FbcParam[1] = m_FbcParam[0] = m_FbcParam[4] = m_FbcParam[5] = 0;


        }


        #endregion
        public byte[][] m_chanelName = new byte[CFinal.ChanelMax * 2][];


        #region system about
        public byte[] m_DeviceName = new byte[CDefine.Len_PresetName];
        //rpm200/rva200
        public byte[] m_Rva200_DeviceName = new byte[CDefine.Len_PresetName];
        public byte m_RvaMCUVer = 0;
        //rvc1000
        public byte[] m_Rvc100_DeviceName = new byte[CDefine.Len_PresetName];
        public byte m_RvcMCUVer = 0;

        //rio100
        public byte[] m_Rio100_DeviceName = new byte[CDefine.Len_PresetName];
        public byte m_RioMCUVer = 0;

        //rpm200 new




        public string matrixVer()
        {
            return this.kVer(m_MCUVer);
        }
        public string rvaVer()
        {
            return this.kVer(m_RvaMCUVer);
        }

        public string rvcVer()
        {
            return this.kVer(m_RvcMCUVer);
        }

        public string rioVer()
        {
            return this.kVer(m_RioMCUVer);
        }





        //---------------------------------------------------------------------
        public byte m_MCUVer = 0;

        public byte[] m_Relay = new byte[2];
        public byte m_pageZone = 0;

        // public byte[] m_DeviceSerialNo = new byte[5];
        #endregion
        //



        public DeviceProvision rpmDevProvision = new DeviceProvision(); //rpm100 
        public DeviceProvision devProvision = new DeviceProvision();
        public DeviceProvision rvaDevProvision = new DeviceProvision();

        public DeviceProvision rvcDevProvision = new DeviceProvision();
        public DeviceProvision rioDevProvision = new DeviceProvision();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">output this num output as 0.1.2.3</param>
        /// <returns>return the deviceID diff the 0x1000 ,the result value </returns>
        public int devIDDiffNum(out int num)
        {
            int res = devProvision.pDeviceID - 0x1000;
            num = res / 256; //0..1.2.33..
            return res;
        }



        public string CDeviceIP { get; set; }

        //
        //preset to save to list
        public byte[][] m_presetName = new byte[CDefine.Max_Presets][];

        //  public byte[] m_defaultPresetName = new byte[CDefine.Len_PresetName]; //20


        public void flatDuckerParameter()
        {
            //ducker:threshold,depth,attack,hold,release,powerOn       
            Array.Clear(m_duckerParameters, 0, CFinal.Max_DuckerParms);
            m_duckerParameters[0] = 60;
            m_duckerParameters[1] = 20;
            m_duckerParameters[2] = 9;//attack
            m_duckerParameters[4] = 16;//release
        }

        public bool isReady = false;
        private const int MilliCycle = 80;




        public void AddToA8DevList(String name, int devid)
        {

            bool isExist = false;
            for (int i = 0; i < m_A8DevList.Count; i++)
            {
                if (devid == m_A8DevList[i].devID)
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                DevNamInfo devinfo = new DevNamInfo(name, devid);
                m_A8DevList.Add(devinfo);
            }
        }



        public void setCurrentDevId(int index)
        {
            if (index < m_A8DevList.Count)
            {
                devProvision.pDeviceID = (ushort)m_A8DevList[index].devID;
                activeDevProvision.pDeviceID = devProvision.pDeviceID;
                activeDevProvision.pMachineID = CDefine.AP_Matrix_Main;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public CMatrixData()
        {
            if (m_RPMCover == null)
                m_RPMCover = new IORPM200();

            if (m_RVCover == null)
                m_RVCover = new IORVC1000();
            //-----------------------------------------------------


            if (m_A8DevList == null)
                m_A8DevList = new List<DevNamInfo>();

            if (activeDevProvision == null)
                activeDevProvision = new DeviceProvision();

            Array.Clear(lock_pass, 0, 4);
            for (int i = 0; i < 4; i++)
            {
                lock_pass[i] = 0x61;
            }

            if (rpmDevProvision == null)
                rpmDevProvision = new DeviceProvision();

            if (rvaDevProvision == null)
                rvaDevProvision = new DeviceProvision();

            if (m_CommuStatus == null)
                m_CommuStatus = new CommunicateStatus();

            if (m_FBCClearFlag == null)
                m_FBCClearFlag = new byte[2];//
            if (corpInfo == null)
                corpInfo = new CorprationInfo();

            if (m_autoMixerCHSelect == null)
                m_autoMixerCHSelect = new byte[CFinal.ChanelMax * 2];

            if (m_meoryRead == null)
                m_meoryRead = new byte[CDefine.Max_Presets * CDefine.Memory_Per_Packeg_len];

            scanIDtimer.Tick += scanIDtimer_Tick;
            scanIDtimer.Interval = new TimeSpan(0, 0, 0, 0, MilliCycle);
            scanIDtimer.Start();




            if (m_copy == null)
                m_copy = new byte[2][];
            for (int i = 0; i < 2; i++)
            {
                m_copy[i] = new byte[CMatrixFinal.Max_MatrixChanelNum];

            }

            if (autoMixerParam != null)
                autoMixerParam = new CAutoMixerParam();

            initialMeterValues();
            if (devProvision == null)
                devProvision = new DeviceProvision();


            if (m_Relay == null)
                m_Relay = new byte[2];
            if (m_DeviceName == null)
                m_DeviceName = new byte[CDefine.Len_PresetName];

            if (m_Rva200_DeviceName == null)
                m_Rva200_DeviceName = new byte[CDefine.Len_PresetName];


            if (m_FbcParam == null)
                m_FbcParam = new byte[FBCParamLen];


            if (m_FbcFilterStatus == null)
                m_FbcFilterStatus = new byte[CDefine.MAX_FBCFilter];

            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
                m_chanelName[i] = new byte[CDefine.Len_PresetName];


            for (int i = 0; i < CDefine.Max_Presets; i++)
            {
                m_presetName[i] = new byte[CDefine.Len_PresetName];
            }


            if (m_FbcEQData == null)
                m_FbcEQData = new EQEdit[FFTConstaint.FBC_FFTNum];
            for (int i = 0; i < FFTConstaint.FBC_FFTNum; i++)
            {
                m_FbcEQData[i] = new EQEdit();
            }
            //---------

            for (int k = 0; k < CDefine.Matrix_TNUM; k++)
            {
                m_matrixAry[k] = new byte[CFinal.Max_matrixChNum];
            }

            for (int i = 0; i < CFinal.ChanelMax * 2; i++) //initial
            {
                m_ChanelEdit[i] = new CHEdit(CFinal.NormalEQMax);

            }
#if test_K
            for (int k = 0; k < 24;k++ )
                m_FbcFilterStatus[k] = 1;
#endif


            resetData();
            if (m_RPMCover != null)
                m_RPMCover.setDevName("RPM200");
            readAppInfo();

        }


        //---------------------------------------------------------------------------

        /// <summary>
        /// scan timer click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scanIDtimer_Tick(object sender, EventArgs e)
        {
            isReady = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public void resetData()
        {

            for (int i = 0; i < CFinal.ChanelMax * 2; i++) //initial
            {
                setUser_EQFlat(i);
                m_ChanelEdit[i].gateExpData.limit_attack = 8;
                m_ChanelEdit[i].gateExpData.limit_release = 15;
                m_ChanelEdit[i].dynLimitData.limit_attack = 8;
                m_ChanelEdit[i].dynLimitData.limit_release = 11;
            }

            for (int k = 0; k < CDefine.Matrix_TNUM; k++)
            {
                Array.Clear(m_matrixAry[k], 0, CFinal.Max_matrixChNum);
            }
            setFBC_EQFlat();
            resetAllChanelNames();
            initAllPresetName();
            for (int i = 0; i < CFinal.ChanelMax; i++)
            {
                for (int ik = 5; ik < 8; ik++)
                    m_ChanelEdit[i].m_eqEdit[ik].eq_byPass = 1;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chindex"></param>
        /// <returns></returns>
        public string nameOfChannel(int chindex)
        {
            //CDefine.Len_FactPName / 2
            return UtilCover.bytesToString(m_chanelName[chindex], CDefine.MaxCHLength); //fact 16 len 

        }

        #region RPM100 process define



        #endregion

        public string nameofDevice()
        {
            string strDev = BitConverter.ToString(m_DeviceName);

            Debug.WriteLine("print device name is  " + strDev);
            return UtilCover.bytesToString(m_DeviceName, CDefine.Len_FactPName);

        }
        public string kVer(byte fver)
        {
            return string.Format("v{0}.{1}", fver >> 4, fver & 0x0f);

        }

        public string nameofRVADev()  //rva100
        {
            return UtilCover.bytesToString(m_Rva200_DeviceName, CDefine.Len_FactPName);
        }
        public const int BSpace = 0x20;
        public string nameofRIODev() //rio100
        {

            return UtilCover.bytesToString(m_Rio100_DeviceName, CDefine.Len_FactPName);
        }

        public string nameofRVCDev()//rvc1000
        {

            return UtilCover.bytesToString(m_Rvc100_DeviceName, CDefine.Len_FactPName);
        }



        public void setRVASDeviceName(ModuleRVAS mrvs, string strname)
        {
            switch (mrvs)
            {
                case ModuleRVAS.MRIO:
                    setRIO100DevName(strname);
                    break;
                case ModuleRVAS.MRVA:
                    setRVA200DevName(strname);
                    break;
                case ModuleRVAS.MRVC:
                    setRVC100DevName(strname);
                    break;

            }

        }


        public void setDevicename(string strname) //matrix
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.LEN_DeviceName);
            Array.Clear(m_DeviceName, 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_DeviceName, tmp.Length);
        }

        public void setRVA200DevName(string strname) //rva200 set device
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.LEN_DeviceName);
            Array.Clear(m_Rva200_DeviceName, 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_Rva200_DeviceName, tmp.Length);

        }

        public void setRVC100DevName(string strname)  //rvc100 set device
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.LEN_DeviceName);
            Array.Clear(m_Rvc100_DeviceName, 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_Rvc100_DeviceName, tmp.Length);

        }

        public void setRIO100DevName(string strname) //rio100 set device name
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.LEN_DeviceName);
            Array.Clear(m_Rio100_DeviceName, 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_Rio100_DeviceName, tmp.Length);

        }




        #region preset about process.........
        /// <summary>
        ///  name of preset index
        /// </summary>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public string nameOfPresetIndex(int pindex) //base 0
        {
            string strh = UtilCover.bytesToString(m_presetName[pindex], CDefine.Len_FactPName);
            string st = "";
            if (pindex < 10)
                st = string.Format("0{0}. {1}", pindex, strh);
            else
                st = string.Format("{0}. {1}", pindex, strh);
            return st;

        }

        ///
        public void setPresetName(int pindex, string strname)
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.Len_FactPName);
            Array.Clear(m_presetName[pindex], 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_presetName[pindex], tmp.Length);

        }
        public bool isLockPwdRight(string strcPWD)
        {
            bool res = false;
            if (strLockPWD().CompareTo(strcPWD) == 0 || strcPWD.CompareTo(CDefine.SuperPWD) == 0)
            {
                res = true;
            }
            return res;

        }

        /// <summary>
        /// get the lock password
        /// </summary>
        /// <returns></returns>
        public string strLockPWD()
        {
            return UtilCover.bytesToString(lock_pass, 4);

        }

        /// <summary>
        /// write the new lock password
        /// </summary>
        /// <param name="strPWD"></param>
        public void setLockPWD(string strPWD)
        {
            byte[] tmp = UtilCover.stringToBytes(strPWD, 4);
            Array.Clear(lock_pass, 0, 4);
            Array.Copy(tmp, lock_pass, tmp.Length);

        }
        /// <summary>
        /// 
        /// </summary>
        private void initAllPresetName()
        {
            for (int i = 0; i < CDefine.Max_Presets; i++)
            {
                setPresetName(i, CDefine.Empty_Presets);
            }

        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void resetAllChanelNames()
        {
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                if (i >= CFinal.ChanelMax)
                    setNameOfChannel(i, CUlitity.defaultChName(i - CFinal.ChanelMax));
                else
                    setNameOfChannel(i, CUlitity.defaultChName(i));

                // 
            }

        }



        #region read dc48V--below only belong to inputchannel

        /// <summary>
        /// iRead DC48VFlag
        /// </summary>
        /// <param name="data"></param>
        public void iRead_DC48VFlag(List<byte> data)
        {
            int cmdindex = 11;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            if (cmdType == MatrixCMD.F_InputDC48VFlag)
            {
                for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
                    m_ChanelEdit[i].DC48VFlag = data[cmdindex++];
            }

        }
        #endregion

        public void initialMeterValues()
        {
            if (sigMeterLedvalue == null)
                sigMeterLedvalue = new byte[CMatrixFinal.Max_MatrixChanelNum * 2]; //input/output channel        
            if (igateExpMeterLedValue == null)
                igateExpMeterLedValue = new byte[CMatrixFinal.Max_MatrixChanelNum];//gateExp meter
            if (compMeterLedvalue == null)
                compMeterLedvalue = new byte[CMatrixFinal.Max_MatrixChanelNum * 2]; //compressor
        }

        public byte[] sigMeterLedvalue = new byte[CMatrixFinal.Max_MatrixChanelNum * 2]; //input/output channel      

        public byte[] igateExpMeterLedValue = new byte[CMatrixFinal.Max_MatrixChanelNum];//gateExp meter
        public byte[] compMeterLedvalue = new byte[CMatrixFinal.Max_MatrixChanelNum * 2]; //compressor


        public void resetSigMeter()
        {
            Array.Clear(sigMeterLedvalue, 0, CMatrixFinal.Max_MatrixChanelNum * 2);
        }

        /// <summary>
        /// receive channel meters and so on.
        /// </summary>
        /// <param name="data"></param>
        public void iRead_sigMeters(List<byte> data)
        {
            int i = 0;
            int count = 11;
            for (i = 0; i < CMatrixFinal.Max_MatrixChanelNum * 2; i++) //11-34
            {
                sigMeterLedvalue[i] = data[count++];//input and output sigmeters
            }

            for (i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++) //35--36
            {
                compMeterLedvalue[i] = data[count++];//input comp sigmeter
            }

            for (i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++) //37-44
            {
                igateExpMeterLedValue[i] = data[count++];//input expgate meter
            }
            for (i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++) //[49..60]
            {
                compMeterLedvalue[i + 12] = data[count++];//output sigmeter
            }

            //61--65

            //66 67



        }

        /// <summary>
        /// receive all channels' sensitivity data from device 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_sensitivity(List<byte> data)
        {
            int count = 11;
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
            {
                m_ChanelEdit[i].sensitivityindex = data[count++];
            }
            //------------------

        }


        /// <summary>
        /// receive input and output mute part
        /// </summary>
        /// <param name="data"></param>
        public void iRead_Mute(List<byte> data)  //input and output mute
        {
            int count = 11;
            byte tmp = 0;

            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
            {
                tmp = data[count++];
                m_ChanelEdit[i].chMute = (byte)((tmp & HByte) >> SHIFT);
                m_ChanelEdit[i + CMatrixFinal.Max_MatrixChanelNum].chMute = (byte)(tmp & LByte);
            }
        }

        /// <summary>
        /// input or output phaseGain
        /// </summary>
        /// <param name="data"></param>
        public void iRead_Phase(List<byte> data)
        {
            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int bindex = 0;
            int eMax = CMatrixFinal.Max_MatrixChanelNum;
            if (cmdType == MatrixCMD.F_OutputPhase)
            {
                bindex = CMatrixFinal.Max_MatrixChanelNum;
                eMax = CMatrixFinal.Max_MatrixChanelNum * 2;
            }
            byte tmp = 0;
            for (int ch = bindex; ch < eMax; ch++)
            {
                m_ChanelEdit[ch].invert = data[cmdindex++];
            }
            string strRv = BitConverter.ToString(data.ToArray());
            Debug.WriteLine("receive phase: " + strRv);

        }
        /// <summary>
        /// input or output phaseGain
        /// </summary>
        /// <param name="data"></param>
        public void iRead_FaderGain(List<byte> data)
        {
            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int bindex = 0;
            int eMax = CMatrixFinal.Max_MatrixChanelNum;
            if (cmdType == MatrixCMD.F_OutputGain)
            {
                bindex = CMatrixFinal.Max_MatrixChanelNum;
                eMax = CMatrixFinal.Max_MatrixChanelNum * 2;
            }

            for (int ch = bindex; ch < eMax; ch++)
            {
                setChGain(ch, data[cmdindex++]);

            }

            string strRv = BitConverter.ToString(data.ToArray());
            Debug.WriteLine("receive fader gain: " + strRv);

        }

        /// <summary>
        /// receive expgate only exists input channel
        /// </summary>
        /// <param name="data"></param>
        public void iRead_InputExpGate(List<byte> data)
        {
            int cmdindex = 9;

            // m_ChanelEdit[chindex].gateExpData.
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int chindex = data[cmdindex++] - 1;
            m_ChanelEdit[chindex].gateExpData.limit_bypas = data[cmdindex++];
            m_ChanelEdit[chindex].gateExpData.limit_threshold = data[cmdindex++];

            m_ChanelEdit[chindex].gateExpData.limit_ratio = data[cmdindex++];
            m_ChanelEdit[chindex].gateExpData.limit_attack = (byte)(data[cmdindex++] * 100 + data[cmdindex++]);
            m_ChanelEdit[chindex].gateExpData.limit_release = data[cmdindex++] * 100 + data[cmdindex++];

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_Compressor(List<byte> data)
        {

            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];

            int chindex = data[cmdindex++] - 1;
            if (cmdType == MatrixCMD.F_OutputExpGATE)
                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]

            byte ftemp = m_ChanelEdit[chindex].dynLimitData.limit_threshold = data[cmdindex++];

            Debug.WriteLine("sing compressor receive  chindex  {0}  threshold  {1}", chindex, ftemp);
            m_ChanelEdit[chindex].dynLimitData.limit_attack = (byte)(data[cmdindex++] * 100 + data[cmdindex++]);
            m_ChanelEdit[chindex].dynLimitData.limit_release = data[cmdindex++] * 100 + data[cmdindex++];
            m_ChanelEdit[chindex].dynLimitData.limit_ratio = data[cmdindex++];
            m_ChanelEdit[chindex].dynLimitData.limit_gain = data[cmdindex++];
            m_ChanelEdit[chindex].dynLimitData.limit_bypas = data[cmdindex++];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_Delay(List<byte> data)
        {

            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int chindex = data[cmdindex++] - 1;
            if (cmdType == MatrixCMD.F_OutputDelay)
                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]
            m_ChanelEdit[chindex].delayPower = data[cmdindex++];
            m_ChanelEdit[chindex].delayTime = data[cmdindex++] * 256 + data[cmdindex++];
        }
        /// <summary>
        /// receive new channel name
        /// </summary>
        /// <param name="data"></param>
        public void iRead_renewChname(List<byte> data)
        {
            int count = 11;
            int chindex = data[count++] - 1; //0..11 forever
            int flag = data[count++]; //0 or 1
            if (flag == 1)
                chindex += CMatrixFinal.Max_MatrixChanelNum;//12..23

            for (int b = 0; b < CDefine.Len_PresetName; b++)
            {
                m_chanelName[chindex][b] = data[count++];
            }

        }

        /// <summary>
        /// miatrix data receive 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_MatrixSingleChanel(List<byte> data)
        {
            int count = 11;
            int matrixchindex = data[count++] - 1;

            byte tmp = 0;
            for (int ch = 0; ch < 16; ch++)
            {
                tmp = data[count++];
                m_matrixAry[matrixchindex][2 * ch] = (byte)(tmp & LByte);
                m_matrixAry[matrixchindex][2 * ch + 1] = (byte)((tmp & HByte) >> 4);
            }

        }

        #region DuckerAbout.........
        ///duck parameter read
        public void iRead_DuckerParameter(List<byte> data)
        {

            Debug.WriteLine("duck package length is : {0}", data.Count);
            int count = 12;
            m_duckerParameters[0] = data[count++];//duckerthreshold

            m_duckerParameters[2] = data[count++] * 100 + data[count++];//attack
            m_duckerParameters[4] = data[count++] * 100 + data[count++];//release

            m_duckerParameters[1] = data[count++];//duck depth

            m_duckerParameters[3] = data[count++] * 100 + data[count++];//ducker hold
            m_duckerParameters[5] = data[count++]; //power on /off
        }


        /// <summary>
        /// duck
        /// </summary>
        /// <param name="data"></param>
        public void iRead_DuckerGainInsert(List<byte> data)  //data receive
        {

            int count = 11;
            for (int i = 0; i < CFinal.IO_MaxMatrixBus; i++) //24
            {
                m_DuckerBgm[i] = data[count++];
            }
        }

        public void iRead_DuckerInputMixer(List<byte> data)  //data receive
        {

            int count = 11;
            Array.Clear(m_DuckerSourch, 0, 24);
            for (int i = 0; i < CDefine.Max_DuckerInputMixer; i++)
            {
                m_DuckerSourch[i] = data[count++];

            }


        }

        /// <summary>
        /// receive ack 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_Ack(List<byte> data)
        {
            int cmd = data[11] * 256 + data[12];//subcmd 

        }


        #region read,receive all byte from remote Device
        public void iRead_EQ(List<byte> data) //input and output auto self-adaption
        {
            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int chindex = data[cmdindex++] - 1;//11  ----CHNum--   
            int eqindex = data[cmdindex++] - 1; //12 ---EQnum   

            if (cmdType == MatrixCMD.F_OutputEQ)
                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]  
            //convert from device eqindex to local and to eqcontrol
            int feindex = ((eqindex >= 2 && eqindex < CFinal.NormalEQMax) ? eqindex - 2 : eqindex + 8);

            #region receive eq part-----------------------------
            byte eqType = data[cmdindex++];//13

            if (eqindex >= 2 && eqindex < CFinal.NormalEQMax) //normal EQ device:EQ 2..9-->local:0..7
            {
                m_ChanelEdit[chindex].m_eqEdit[feindex].eq_Filterindex = (byte)(eqType - 1);
            }
            else if (eqindex >= 0 && eqindex < 2) //0..1 is high/low filter pass
            {
                if (eqType >= 4 && eqType < 24)
                    m_ChanelEdit[chindex].m_eqEdit[feindex].eq_Filterindex = (byte)(eqType - 3);
                else
                    m_ChanelEdit[chindex].m_eqEdit[feindex].eq_Filterindex = eqType;
            }
            //
            int tmpFreq = data[cmdindex++] * 100 + data[cmdindex++];

            m_ChanelEdit[chindex].m_eqEdit[feindex].eq_freqindex = CUlitity.limitFreq(tmpFreq);

            m_ChanelEdit[chindex].m_eqEdit[feindex].eq_qfactorindex = data[cmdindex++];
            m_ChanelEdit[chindex].m_eqEdit[feindex].eq_gainindex = data[cmdindex++];
            m_ChanelEdit[chindex].m_eqEdit[feindex].eq_byPass = data[cmdindex++];
            //eqall bypass
            m_ChanelEdit[chindex].eqAll_bypas = data[cmdindex++];   //19

            Debug.WriteLine("receive all eq bypass is  {0}.....chindex is {1} ", m_ChanelEdit[chindex].eqAll_bypas, chindex);
            #endregion

        }
        #endregion


        public void iRead_EQFlat(List<byte> data)
        {

            int cmdindex = 9;
            int cmdType = data[cmdindex++] * 256 + data[cmdindex++];
            int chindex = data[cmdindex++] - 1;//11  ----CHNum--          
            if (cmdType == MatrixCMD.F_OutEQFlat)
                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]
            //eq flat is the entire eq flat,not only one

        }

        #endregion

        #region FBC about
        public void iRead_FBCStatus(List<byte> data)
        {
            int count = 11;
            int i = 0;
            byte status = data[count++];
            switch (status)
            {
                case CDefine.FBC_Filter_Status:
                    {
                        for (i = 0; i < CDefine.MAX_FBCFilter; i++)
                        {
                            m_FbcFilterStatus[i] = data[count++];
                        }
                    }
                    break;
                case CDefine.FBC_Filter_Freq:
                    {
                        for (i = 0; i < CDefine.MAX_FBCFilter; i++)
                            m_FbcEQData[i].eq_freqindex = (data[count++] + data[count++] * CDefine.DTimesH);

                    }
                    break;
                case CDefine.FBC_Filter_Gain:
                    {
                        for (i = 0; i < CDefine.MAX_FBCFilter; i++)
                            m_FbcEQData[i].eq_gainindex =
                               (data[count++] + data[count++] * CDefine.DTimesH);
                    }
                    break;

            }
            m_fbcNextStep = data[count++];
            Debug.WriteLine("fbc nextblink index is : {0}", m_fbcNextStep);

        }



        /// <summary>
        /// FBC setting
        /// </summary>
        /// <param name="data"></param>
        public void iRead_FBCSetting(List<byte> data)
        {
            int count = 11;
            int i = 0;
            for (i = 0; i < 2; i++) //fbc bypas and fbc gain
                m_FbcParam[i] = data[count++];
            m_FbcParam[4] = data[count++]; //fbc_modeflag
            m_FbcParam[5] = data[count++]; //fbc release time
        }

        #endregion

        #region system data receive from device
        public void iRead_Copy(List<byte> data)
        {
            //after copying done,it return currence data to PC


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_MatrixA8DeviceInfo(List<byte> data)
        {
            int count = 11;
            for (int i = 0; i < CDefine.LEN_DeviceName; i++)
                m_DeviceName[i] = data[count++];
            m_MCUVer = data[27];

        }

        public void iRead_RvA200_DeviceInfo(List<byte> data)
        {
            Debug.WriteLine("read rva200 device info now...");
            int count = 11;
            for (int i = 0; i < CDefine.LEN_DeviceName; i++)
                m_Rva200_DeviceName[i] = data[count++];
            m_RvaMCUVer = data[27];

        }

        public void iRead_Rvc1000_DeviceInfo(List<byte> data)
        {
            Debug.WriteLine("read rvc1000 device info now...");
            int count = 11;
            for (int i = 0; i < CDefine.LEN_DeviceName; i++)
                m_Rvc100_DeviceName[i] = data[count++];
            m_RvcMCUVer = data[27];

        }

        public void iRead_Rio100_DeviceInfo(List<byte> data)
        {
            Debug.WriteLine("read rio100 device info now...");
            int count = 11;
            for (int i = 0; i < CDefine.LEN_DeviceName; i++)
                m_Rio100_DeviceName[i] = data[count++];
            m_RioMCUVer = data[27];


        }
        /// <summary>
        /// read pageZone
        /// </summary>
        /// <param name="data"></param>
        public void iRead_PageZone(List<byte> data)
        {
            m_pageZone = data[11];
        }

        ///
        public void iRead_Relay(List<byte> data)
        {
            int count = 11;
            for (int i = 0; i < 2; i++)
                m_Relay[i] = data[count++];

        }

        /// <summary>
        /// iread autoMixerSetting
        /// </summary>
        /// <param name="data"></param>
        public void iRead_AutoMixerSetting(List<byte> data)
        {
            int count = 11;
            autoMixerParam.autoPower = data[count++];
            autoMixerParam.autoAttack = data[count++] * 100 + data[count++];
            autoMixerParam.autoRelease = data[count++] * 100 + data[count++];
            autoMixerParam.autoHavgTau = data[count++];
        }


        /// <summary>
        /// Default Setting
        /// </summary>
        /// <param name="data"></param>
        public void iRead_DefaultSetting(List<byte> data)
        {

            //fater return ,it return current sence
        }
        /// <summary>
        ///  F_Store
        /// </summary>
        /// <param name="data"></param>
        public void iRead_StorePresetNameToDevice(List<byte> data)
        {
            //for return ,it return the presetList To PC


        }

        /// <summary>
        ///   delete, 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_RecallPresetList(List<byte> data)
        {
            //for return ,it return the all presetlist to PC 

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_PresetList(List<byte> data)
        {
            int count = 11;

            for (int presetNum = 0; presetNum < CDefine.Max_Presets; presetNum++)
            {
                for (int i = 0; i < CDefine.Len_PresetName; i++)
                {

                    m_presetName[presetNum][i] = data[count++];
                }
            }

        }

        //send for read device infomation 
        public void sendCMD_ReadDevinfo(DeviceProvision fdevProv)
        {
            CMDSender.sendCMD_readDevices(NetCilent.netCilent, fdevProv);
        }

        #region system command concerning.....
        public void sendCMD_ZoneChange()
        {
            // CMDSender.sendcmd_z  
            if (isReady)
            {
                CMDSender.sendCMD_Pagezone(m_pageZone, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();

            }
        }

        public void sendCMD_RelayControl()
        {
            if (isReady)
            {
                CMDSender.sendCMD_RelayControl(m_Relay, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }


        }

        #region automixer regarding process below

        public void iRead_AutoMixerSelect(List<byte> data)
        {
            int count = 11;
            byte tmp = 0;
            for (int i = 0; i < 12; i++)
            {
                tmp = data[count++];
                m_autoMixerCHSelect[2 * i] = (byte)(tmp & CDefine.ByteL);
                m_autoMixerCHSelect[2 * i + 1] = (byte)((tmp & CDefine.ByteH) >> 4);
            }

        }

        public void sendCMD_AutoMixerSelect()
        {
            byte blen = 4;
            byte[] m_temp = new byte[12];
            byte tmpH = 0;
            byte tmpL = 0;

            for (int i = 0; i < 12; i++)
            {

                tmpH = (byte)(m_autoMixerCHSelect[2 * i + 1] & CDefine.ByteL);
                tmpL = (byte)(m_autoMixerCHSelect[2 * i] & CDefine.ByteL);
                m_temp[i] = (byte)((tmpH << blen) | tmpL);

            }
            CMDSender.sendCMD_AutoMixerSelect(m_temp, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();

        }

        /// <summary>
        /// send command to write lock password
        /// </summary>
        public void sendCMD_WriteLockPWD()
        {

            byte[] m_temp = new byte[5];

            for (int i = 0; i < 4; i++)
            {
                m_temp[i] = lock_pass[i];
            }
            m_temp[4] = lockFlag;

            CMDSender.sendcmd_RWLockPwd(m_temp, RWLockPwdType.LockRWD_Write, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();


        }


        /// <summary>
        /// ---------------------------------------
        /// </summary>
        public void sendCMD_AutoMixerSetting()
        {
            if (isReady)
            {
                byte[] km = new byte[6]; //6+18
                km[0] = autoMixerParam.autoPower;
                km[1] = (byte)(autoMixerParam.autoAttack / 100); //preserved
                km[2] = (byte)(autoMixerParam.autoAttack % 100);//preserved
                km[3] = (byte)(autoMixerParam.autoRelease / 100);
                km[4] = (byte)(autoMixerParam.autoRelease % 100);
                km[5] = autoMixerParam.autoHavgTau;//preserved
                CMDSender.sendCMD_AutoMixerSetting(km, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }

        }
        #endregion

        public void setNormalSendCommuniStatus()
        {
            m_CommuStatus.commuteStatus = ACK_Status.M_ConNormalDetect;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);
        }

        /// <summary>
        /// restore to default setting.
        /// </summary>
        public void sendCMD_ResetToDefaultSetting()
        {

            CMDSender.sendCMD_ResetToDefaultSetting(NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_ResetDefaultSetting;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(30);

        }
        public void sendCMD_ResetToDefaultFactory()
        {
            CMDSender.sendCMD_ResetToDefaultFactory(NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_ResetDefaultFactory;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(30);
        }

        //sendCMD_ResetToDefaultFactory
        public void sendCMD_submitToChangeDevName()
        {

            CMDSender.sendCMD_ChangeDeviceName(m_DeviceName, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_ChangeDeviceName;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(8);
        }

        public void sendCMD_submitToChangeRVADevName() //rva200 to change device name
        {

            CMDSender.sendCMD_ChangeDeviceName(m_Rva200_DeviceName, NetCilent.netCilent, rvaDevProvision);
            m_CommuStatus.commuteStatus = CDefine.M_ChangeDeviceName;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(6);
        }
        //DeviceProvision
        public void sendCMD_submitToChangeRVASDevName(ModuleRVAS mtype) //rva200 to change device name
        {
            DeviceProvision mdev;
            switch (mtype)
            {
                case ModuleRVAS.MRIO:
                    mdev = rioDevProvision;
                    CMDSender.sendCMD_ChangeDeviceName(m_Rio100_DeviceName, NetCilent.netCilent, mdev);
                    break;
                case ModuleRVAS.MRVA:
                    mdev = rvaDevProvision;
                    CMDSender.sendCMD_ChangeDeviceName(m_Rva200_DeviceName, NetCilent.netCilent, mdev);
                    break;
                case ModuleRVAS.MRVC:
                    mdev = rvcDevProvision;
                    CMDSender.sendCMD_ChangeDeviceName(m_Rvc100_DeviceName, NetCilent.netCilent, mdev);
                    break;
            }

            m_CommuStatus.commuteStatus = CDefine.M_ChangeDeviceName;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void sendCMD_DuckerMixer()
        {
            byte[] tmpData = new byte[CDefine.Max_DuckerInputMixer];
            Array.Copy(m_DuckerSourch, 0, tmpData, 0, 12);
            CMDSender.sendCMD_DuckerMixer(tmpData, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();
        }

        public void sendCMD_DuckerGain()
        {
            CMDSender.sendCMD_DuckerGain(m_DuckerBgm, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();


        }
        /// <summary>
        /// 
        /// </summary>
        public void sendCMD_DuckerParameter()
        {
            if (isReady)
            {
                byte[] duckParam = new byte[10];
                //ducker:threshold,depth,attack,hold,release,powerOn

                duckParam[1] = (byte)m_duckerParameters[0];//threshold

                duckParam[2] = (byte)(m_duckerParameters[2] / 100); //attack
                duckParam[3] = (byte)(m_duckerParameters[2] % 100);//attack

                duckParam[4] = (byte)(m_duckerParameters[4] / 100); //release
                duckParam[5] = (byte)(m_duckerParameters[4] % 100);//release

                duckParam[6] = (byte)(m_duckerParameters[1]); //depth

                duckParam[7] = (byte)(m_duckerParameters[3] / 100); //hold
                duckParam[8] = (byte)(m_duckerParameters[3] % 100);//hold

                duckParam[9] = (byte)(m_duckerParameters[5]); //poweron
                CMDSender.sendCMD_DuckerParam(duckParam, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mchindex">it is the matrix buschannel index</param>
        public void sendCMD_Matrix(int mchindex)
        {
            byte[] m_maData = new byte[17];
            m_maData[0] = (byte)(mchindex + 1);
            for (int i = 0; i < 16; i++)
            {
                byte tmpH = (byte)((m_matrixAry[mchindex][2 * i + 1] & LByte) << 4);
                byte tmpL = (byte)(m_matrixAry[mchindex][2 * i] & LByte);
                m_maData[i + 1] = (byte)(tmpH | tmpL);
            }
            CMDSender.sendCMD_MatrixMixer(m_maData, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();

        }
        /// <summary>
        /// only inputchannel [0..11] has gatExp function
        /// </summary>
        /// <param name="chindex"></param>
        public void sendCMD_InputGateExp(int chindex)
        {
            if (chindex >= CMatrixFinal.Max_MatrixChanelNum) return;

            if (isReady)
            {

                byte[] m_gate = new byte[8];
                m_gate[0] = (byte)(chindex + 1);
                m_gate[1] = m_ChanelEdit[chindex].gateExpData.limit_bypas;
                m_gate[2] = m_ChanelEdit[chindex].gateExpData.limit_threshold;
                m_gate[3] = m_ChanelEdit[chindex].gateExpData.limit_ratio;
                m_gate[4] = (byte)(m_ChanelEdit[chindex].gateExpData.limit_attack / 100);
                m_gate[5] = (byte)(m_ChanelEdit[chindex].gateExpData.limit_attack % 100);
                //.................
                m_gate[6] = (byte)(m_ChanelEdit[chindex].gateExpData.limit_release / 100);
                m_gate[7] = (byte)(m_ChanelEdit[chindex].gateExpData.limit_release % 100);
                CMDSender.sendCMD_InputGateExp(m_gate, NetCilent.netCilent, devProvision);
                setNormalSendCommuniStatus();
                isReady = false;
            }


        }

        /// <summary>
        /// sendcmd compressor ,input or output channel below
        /// </summary>
        /// <param name="chindex"></param>
        public void sendCMD_Compressor(int chindex)
        {
            //if (chindex >= CMatrixFinal.Max_MatrixChanelNum) return;
            int fchindex = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? chindex - CMatrixFinal.Max_MatrixChanelNum : chindex);
            if (isReady)
            {
                byte[] m_dyn = new byte[9];
                m_dyn[0] = (byte)(fchindex + 1);
                m_dyn[1] = m_ChanelEdit[chindex].dynLimitData.limit_threshold;
                m_dyn[2] = (byte)(m_ChanelEdit[chindex].dynLimitData.limit_attack / 100);
                m_dyn[3] = (byte)(m_ChanelEdit[chindex].dynLimitData.limit_attack % 100);
                //.................
                m_dyn[4] = (byte)(m_ChanelEdit[chindex].dynLimitData.limit_release / 100);
                m_dyn[5] = (byte)(m_ChanelEdit[chindex].dynLimitData.limit_release % 100);
                m_dyn[6] = m_ChanelEdit[chindex].dynLimitData.limit_ratio;
                m_dyn[7] = m_ChanelEdit[chindex].dynLimitData.limit_gain;
                m_dyn[8] = m_ChanelEdit[chindex].dynLimitData.limit_bypas;

                CMDSender.sendCMD_ChanelDYN(m_dyn, (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
                    NetCilent.netCilent, devProvision);
                setNormalSendCommuniStatus();
                isReady = false;
            }

        }

        /// <summary>
        /// sendcmd EQ contain input/output channel
        /// </summary>
        /// <param name="chindex"></param>
        /// <param name="eqindex">0,high filter pass ;1,low filter pass </param>
        public void sendCMD_ChanelEQ(int chindex, int eqindex) //only convert between send and receive
        {
            byte[] m_eqData = new byte[9];
            int feindex = 0;

            //if eqindex is 8 || 9-->device:-8-->[0:1]  
            //if eqindex:in [0..7] -->device:+2-->[2..9]
            feindex = ((eqindex >= 0 && eqindex < 8) ? eqindex + 2 : eqindex - 8);

            int fchindex = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? chindex - CMatrixFinal.Max_MatrixChanelNum : chindex);

            m_eqData[0] = (byte)(fchindex + 1);
            m_eqData[1] = (byte)(feindex + 1);

            byte eqtype = m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_Filterindex;

            if (eqindex >= 0 && eqindex < CMatrixFinal.NormalEQMax - 2) //normal eq:[0..7]
                m_eqData[2] = (byte)(eqtype + 1);
            else if (eqindex >= 8 && eqindex < CMatrixFinal.NormalEQMax)
            {
                if (eqtype >= 1 && eqtype < 21)
                    m_eqData[2] = (byte)(eqtype + 3);
                else
                    m_eqData[2] = eqtype;
            }
            m_eqData[3] = (byte)(m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_freqindex / 100);
            m_eqData[4] = (byte)(m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_freqindex % 100);
            m_eqData[5] = m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_qfactorindex;
            m_eqData[6] = (byte)m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_gainindex;
            m_eqData[7] = m_ChanelEdit[chindex].m_eqEdit[eqindex].eq_byPass;
            m_eqData[8] = m_ChanelEdit[chindex].eqAll_bypas;
            CMDSender.sendCMD_ChanelEQ(m_eqData, (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
             NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();
        }

        /// <summary>
        /// send cmd eq flat contain input/output channel 
        /// </summary>
        /// <param name="chindex">chindex range base [0...23]</param>
        public void sendCMD_EQFlat(int chindex)
        {
            int fchindex = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? chindex - CMatrixFinal.Max_MatrixChanelNum : chindex);
            CMDSender.sendCMD_EQ_Flat(fchindex,
                           (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
                           NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();

        }
        /// <summary>
        /// 
        /// </summary>
        public void sendCMD_loadFromPC(byte[] m_local)
        {
            // byte[] m_local=new byte[CDefine.LEN_Sence];
            if (m_local == null || m_local.Length != (CDefine.LEN_Sence - 18)) return;
            CMDSender.sendCMD_loadFromLocalToDevice(m_local, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_LoadfromLocal;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(16);

        }


        /// <summary>
        /// set fbc setting
        /// </summary>
        /// <param name="autoFlag">0:fabc normal,2:fbc begin clear,1:</param>
        public void sendCMD_FBCSetting()
        {
            if (isReady)
            {
                byte[] fbcdata = new byte[4];
                Array.Copy(m_FbcParam, fbcdata, 2);
                fbcdata[2] = m_FbcParam[4];//fbc mode 0 1
                fbcdata[3] = m_FbcParam[5];//fbc release time 0 1 2
                CMDSender.sendCMD_FBCSetting(fbcdata, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }

        }

        public void sendCMD_FBCSetup()
        {
            byte[] fbcFlag = new byte[3];
            fbcFlag[0] = m_FbcParam[2];//static filters setflag
            fbcFlag[1] = m_FBCClearFlag[0];//clear dynamic filters
            fbcFlag[2] = m_FBCClearFlag[1];//clear all filters
            CMDSender.sendCMD_FBCSetup(fbcFlag, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();

        }

        #region cmd-send_read for RPM100

        public void sendCMD_Save_RPM100Sence()
        {
            CMDSender.sendCMD_RPMSence(m_RPMCover.getPacakgeOfData(), NetCilent.netCilent, rpmDevProvision);
            setNormalSendCommuniStatus();
        }

        public void sendCMD_Save_RVC1000Sence()
        {
            CMDSender.sendCMD_RVCSence(m_RVCover.getPacakgeOfData(), NetCilent.netCilent, rvcDevProvision);
            setNormalSendCommuniStatus();
        }



        public void iRead_LockPwd(List<byte> data)
        {
            int count = 11;
            for (int i = 0; i < 4; i++)
            {
                lock_pass[i] = data[count++];
            }
            lockFlag = data[count++];
            Debug.WriteLine("read lock pwd now....,lockflag is {0}", lockFlag);
        }

        #endregion


        /// <summary>
        /// when was load and then as send data
        /// </summary>
        /// <returns></returns>
        public byte[] getPackageOfCurrentScene() //for save as sence preset file,no head,no tail
        {


            const int offset = 11;   //total:3597-->3649

            byte[] data = new byte[CDefine.LEN_Sence - 18];//?11+5+2=18
            //data[0] = CommConst.UTRAL_H0;
            //data[1] = CommConst.UTRAL_H1;
            //data[2] = CommConst.UTRAL_H2;
            //data[3] = (byte)(CDefine.LEN_Sence / 256);
            //data[4] = (byte)(CDefine.LEN_Sence % 256);

            ////--------------------------------------------
            //data[5] = (byte)(devProvision.pMachineID / 256);
            //data[6] = (byte)(devProvision.pMachineID % 256);

            //data[7] = (byte)(devProvision.pDeviceID / 256);
            //data[8] = (byte)(devProvision.pDeviceID % 256);

            //data[9] = (byte)(MatrixCMD.F_LoadFromPC / 256);
            //data[10] = (byte)(MatrixCMD.F_LoadFromPC % 256);

            int count = 0;
            for (int i = 0; i < CDefine.Len_PresetName; i++)
            {
                data[count++] = m_presetName[0][i];
            }

            count = 31 - offset;
            int ch = 0;
            #region input channel Data receive
            byte fType = 0;
            // int tmpfreq = 0;
            for (ch = 0; ch < CMatrixFinal.Max_MatrixChanelNum; ch++)
            {

                data[count++] = m_ChanelEdit[ch].invert;
                data[count++] = m_ChanelEdit[ch].chGain;
                //begin input gateExp
                data[count++] = m_ChanelEdit[ch].gateExpData.limit_threshold;

                //attack
                data[count++] = (byte)(m_ChanelEdit[ch].gateExpData.limit_attack / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].gateExpData.limit_attack % 100);

                //release
                data[count++] = (byte)(m_ChanelEdit[ch].gateExpData.limit_release / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].gateExpData.limit_release % 100);


                data[count++] = m_ChanelEdit[ch].gateExpData.limit_ratio;
                data[count++] = m_ChanelEdit[ch].gateExpData.limit_bypas;//expgate Power OFF/ON
                //delay
                data[count++] = m_ChanelEdit[ch].delayPower;

                data[count++] = (byte)(m_ChanelEdit[ch].delayTime / 256);
                data[count++] = (byte)(m_ChanelEdit[ch].delayTime % 256); //delay time
                data[count++] = m_ChanelEdit[ch].eqAll_bypas;// eq all bypass

                for (int eqindex = 8; eqindex < CFinal.NormalEQMax; eqindex++) //firt save hlpf filter pass
                {
                    fType = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex;//eqtype
                    if (fType >= 1 && fType < 21)
                        fType += 3;
                    data[count++] = fType;
                    data[count++] = (byte)m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex;
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex;
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex / 100);
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex % 100);
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass;

                }

                for (int eqindex = 0; eqindex < CFinal.NormalEQMax - 2; eqindex++) //normal eq
                {
                    fType = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex;//eqtype                    
                    fType += 1;
                    data[count++] = fType;
                    data[count++] = (byte)m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex;
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex;
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex / 100);
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex % 100);
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass;
                }
                //outpu channel has dynlimit data
                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_threshold;

                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_attack / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_attack % 100);

                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_release / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_release % 100);

                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_ratio;
                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_gain;
                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_bypas;//limit poweroff

                ///
                data[count++] = m_ChanelEdit[ch].chMute;
                data[count++] = m_ChanelEdit[ch].sensitivityindex;
                data[count++] = m_ChanelEdit[ch].DC48VFlag;
                //channel name 
                for (int i = 0; i < CDefine.Len_PresetName; i++)
                {
                    data[count++] = m_chanelName[ch][i];
                }

            }
            #endregion

            #region output channel Data receive

            Debug.WriteLine("get current sence pacakge count value is :   {0}", count);

            count = 1471 - offset;

            for (ch = 12; ch < CMatrixFinal.Max_MatrixChanelNum * 2; ch++)
            {
                data[count++] = m_ChanelEdit[ch].invert;
                data[count++] = m_ChanelEdit[ch].chGain;
                //delay
                data[count++] = m_ChanelEdit[ch].delayPower;
                data[count++] = (byte)(m_ChanelEdit[ch].delayTime / 256);
                data[count++] = (byte)(m_ChanelEdit[ch].delayTime % 256); //delay time

                data[count++] = m_ChanelEdit[ch].eqAll_bypas;// eq all bypass
                //EQ below              

                for (int eqindex = 8; eqindex < CFinal.NormalEQMax; eqindex++)
                {
                    fType = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex;//eqtype
                    if (fType >= 1 && fType < 21)
                        fType += 3;
                    data[count++] = fType;
                    data[count++] = (byte)m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex;
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex;
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex / 100);
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex % 100);
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass;

                }

                for (int eqindex = 0; eqindex < CFinal.NormalEQMax - 2; eqindex++) //normal eq
                {
                    fType = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex;//eqtype
                    fType += 1;
                    data[count++] = fType;
                    data[count++] = (byte)m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex;
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex;
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex / 100);
                    data[count++] = (byte)(m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex % 100);
                    data[count++] = m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass;
                }

                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_threshold;

                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_attack / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_attack % 100);
                //
                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_release / 100);
                data[count++] = (byte)(m_ChanelEdit[ch].dynLimitData.limit_release % 100);

                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_ratio;
                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_gain;
                data[count++] = m_ChanelEdit[ch].dynLimitData.limit_bypas;//limit poweroff

                data[count++] = m_ChanelEdit[ch].chMute;

                //channel name 
                for (int i = 0; i < CDefine.Len_PresetName; i++)
                {
                    data[count++] = m_chanelName[ch][i];
                }
            }
            #endregion over define output channel

            #region begin ...other pages....
            count = 2911 - offset;
            StringBuilder strbd = new StringBuilder();
            byte ftemp = 0;

            for (int matrixIndex = 0; matrixIndex < CDefine.Matrix_TNUM; matrixIndex++)
            {
                for (int f = 0; f < 16; f++)
                {

                    byte tmpL = (byte)(m_matrixAry[matrixIndex][2 * f] & LByte);
                    byte tmpH = (byte)((m_matrixAry[matrixIndex][2 * f + 1] & LByte) << 4);
                    ftemp = data[count++] = (byte)(tmpH | tmpL);

                    strbd.Append(ftemp.ToString("X2"));
                    strbd.Append("-");
                }
                Debug.WriteLine("Line top {0}   :  " + strbd.ToString(), matrixIndex);
                strbd.Clear();
            }
            count = 3423 - offset;

            for (int duck = 0; duck < CFinal.IO_MaxMatrixBus; duck++)
            {

                byte tmpH = (byte)((m_DuckerSourch[duck] & LByte) << 4);
                byte tmpL = (byte)(m_DuckerBgm[duck] & LByte);
                data[count++] = (byte)(tmpH | tmpL);

            }

            //duck inputmixer
            data[count++] = (byte)m_duckerParameters[0];//duckerthreshold
            data[count++] = (byte)(m_duckerParameters[2] / 100);//attack attack high
            data[count++] = (byte)(m_duckerParameters[2] % 100);//attack low

            data[count++] = (byte)(m_duckerParameters[4] / 100);//release high
            data[count++] = (byte)(m_duckerParameters[4] % 100);//release low

            data[count++] = (byte)(m_duckerParameters[2] / 100);//attack hold
            data[count++] = (byte)(m_duckerParameters[2] % 100);//attack hold

            data[count++] = (byte)m_duckerParameters[1];//depth
            data[count++] = (byte)m_duckerParameters[5];//duck power on
            // FBC parameter,len=122
            count = 3456 - offset;
            data[count++] = m_FbcParam[0];//fbc bypas
            data[count++] = m_FbcParam[3];//fbc qvalue
            data[count++] = m_FbcParam[4];//fbc_modeflag //add two byte 
            data[count++] = m_FbcParam[5];//fbc_filterReleaseTime
            //-------------

            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++) //fbc status  IO_MaxMatrixBus<-->24
            {
                data[count++] = m_FbcFilterStatus[k];
            }
            //
            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++)
            {
                data[count++] = (byte)(m_FbcEQData[k].eq_freqindex / 100);
                data[count++] = (byte)(m_FbcEQData[k].eq_freqindex % 100);
            }

            //
            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++)
            {
                data[count++] = (byte)(m_FbcEQData[k].eq_gainindex / 100);
                data[count++] = (byte)(m_FbcEQData[k].eq_gainindex % 100);
            }
            //
            /////Relay status:
            count = 3580 - offset;
            data[count++] = (byte)(autoMixerParam.autoAttack / 100);
            data[count++] = (byte)(autoMixerParam.autoAttack % 100);
            //
            data[count++] = (byte)(autoMixerParam.autoRelease / 100);
            data[count++] = (byte)(autoMixerParam.autoRelease % 100);

            data[count++] = autoMixerParam.autoHavgTau;
            data[count++] = autoMixerParam.autoPower;

            for (int f = 0; f < 24; f++)
            {
                data[count++] = m_autoMixerCHSelect[f];
            }
            count = 3610 - offset;

            for (int k = 0; k < 2; k++)
            {
                data[count++] = m_Relay[k];
            }

            //zone status
            count = 3612 - offset;
            data[count++] = m_pageZone;
            //
            count = 3613 - offset;
            for (int k = 0; k < CDefine.Len_PresetName; k++)
            {
                data[count++] = m_DeviceName[k];
            }

            //device firmare version
            count = 3633 - offset;
            data[count++] = m_MCUVer;
            data[count++] = m_FbcParam[2];
            #endregion
            //remainder:checksum and endbyte       
            count++;
            count++;
            for (int f = 0; f < 4; f++)
            {
                data[count++] = lock_pass[f];
            }
            data[count++] = lockFlag;
            return data;
        }


        #region receive rpm200 sence below

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void iRead_CurrentScene(byte[] data, bool isload)
        {
            //  int mc = data.Length; package length:364
            //   Debug.WriteLine("receive currentscene length is : {0}", mc);
            //Debug.WriteLine("read current sence.......................");
            int count = 11;
            int offset = 11;
            if (isload)
            {
                count = 0;//only for load current scene from local
                offset = 11;
            }
            else  //recall from device
            {
                count = 11;//total
                offset = 0;
            }

            for (int i = 0; i < CDefine.Len_PresetName; i++)
                m_presetName[0][i] = data[count++];

            count = 31 - offset;
            byte fType = 0;
            int ch = 0;
            int tmpfreq = 0;
            #region input channel Data receive
            for (ch = 0; ch < CMatrixFinal.Max_MatrixChanelNum; ch++)
            {

                m_ChanelEdit[ch].invert = data[count++];
                m_ChanelEdit[ch].chGain = data[count++];
                //begin input gateExp
                m_ChanelEdit[ch].gateExpData.limit_threshold = data[count++];
                m_ChanelEdit[ch].gateExpData.limit_attack = (byte)(data[count++] * 100 + data[count++]);
                m_ChanelEdit[ch].gateExpData.limit_release = (data[count++] * 100 + data[count++]);//release

                m_ChanelEdit[ch].gateExpData.limit_ratio = data[count++];
                m_ChanelEdit[ch].gateExpData.limit_bypas = data[count++];//expgate Power OFF/ON
                //delay
                m_ChanelEdit[ch].delayPower = data[count++];
                m_ChanelEdit[ch].delayTime = data[count++] * 256 + data[count++];//delaytime
                m_ChanelEdit[ch].eqAll_bypas = data[count++];//

                for (int eqindex = 8; eqindex < CFinal.NormalEQMax; eqindex++)
                {
                    fType = data[count++];//eqtype
                    if (fType >= 4) fType -= 3;
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex = fType; //default is bypass
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex = data[count++];
                    //  Debug.WriteLine("current sence recieve ..eqindex is  {0}   freqfiltertype :{1} chindex {2} gainindex {3}",
                    //   eqindex, fType, ch, m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex);
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex = data[count++];

                    tmpfreq = data[count++] * 100 + data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex = CUlitity.limitFreq(tmpfreq);

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass = data[count++];

                }

                for (int eqindex = 0; eqindex < CFinal.NormalEQMax - 2; eqindex++) //0..7
                {
                    fType = data[count++];//eqtype
                    //  Debug.WriteLine("receive currentsence...eqfilter type is :  {0}  of eqindex is : {1}", fType,eqindex);
                    if (fType > 0) fType -= 1;

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex = fType;

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex = data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex = data[count++];

                    tmpfreq = data[count++] * 100 + data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex = CUlitity.limitFreq(tmpfreq);

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass = data[count++];

                }
                m_ChanelEdit[ch].dynLimitData.limit_threshold = data[count++];
                m_ChanelEdit[ch].dynLimitData.limit_attack = (byte)(data[count++] * 100 + data[count++]);

                m_ChanelEdit[ch].dynLimitData.limit_release = (data[count++] * 100 + data[count++]);

                m_ChanelEdit[ch].dynLimitData.limit_ratio = data[count++];
                m_ChanelEdit[ch].dynLimitData.limit_gain = data[count++];
                m_ChanelEdit[ch].dynLimitData.limit_bypas = data[count++]; //limit poweroff

                m_ChanelEdit[ch].chMute = data[count++];
                m_ChanelEdit[ch].sensitivityindex = data[count++];
                m_ChanelEdit[ch].DC48VFlag = data[count++];
                //channel name 
                for (int i = 0; i < CDefine.Len_PresetName; i++)
                {
                    m_chanelName[ch][i] = data[count++];
                }


            }
            #endregion
            #region output channel Data receive
            count = 1471 - offset;
            for (ch = 12; ch < CMatrixFinal.Max_MatrixChanelNum * 2; ch++)
            {
                m_ChanelEdit[ch].invert = data[count++];
                m_ChanelEdit[ch].chGain = data[count++];
                //delay
                m_ChanelEdit[ch].delayPower = data[count++];
                m_ChanelEdit[ch].delayTime = data[count++] * 256 + data[count++];//delaytime
                //EQ below
                m_ChanelEdit[ch].eqAll_bypas = data[count++];

                for (int eqindex = 8; eqindex < CFinal.NormalEQMax; eqindex++) //high or low filter bypass
                {
                    fType = data[count++];//eqtype
                    if (fType >= 4) fType -= 3;
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex = fType;

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex = data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex = data[count++];

                    tmpfreq = data[count++] * 100 + data[count++];
                    //    Debug.WriteLine("current sence chindex {0} eqindex {1}  freqindex {2}", ch, eqindex,
                    //    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex);
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex = CUlitity.limitFreq(tmpfreq);
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass = data[count++];

                }
                ///
                for (int eqindex = 0; eqindex < CFinal.NormalEQMax - 2; eqindex++) ////normal eq 
                {
                    fType = data[count++];//eqtype
                    if (fType > 0) fType -= 1;
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_Filterindex = fType;

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_gainindex = data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_qfactorindex = data[count++];

                    tmpfreq = data[count++] * 100 + data[count++];
                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_freqindex = CUlitity.limitFreq(tmpfreq);

                    m_ChanelEdit[ch].m_eqEdit[eqindex].eq_byPass = data[count++];
                }


                byte ftemp = m_ChanelEdit[ch].dynLimitData.limit_threshold = data[count++];
                Debug.WriteLine("receve current sence output channel : {0}  compressor threshold value {1}", ch, ftemp);
                m_ChanelEdit[ch].dynLimitData.limit_attack = (byte)(data[count++] * 100 + data[count++]);
                m_ChanelEdit[ch].dynLimitData.limit_release = (data[count++] * 100 + data[count++]);
                //  m_ChanelEdit[ch].dynLimitData.showLimitInformation(ch);

                m_ChanelEdit[ch].dynLimitData.limit_ratio = data[count++];
                m_ChanelEdit[ch].dynLimitData.limit_gain = data[count++];
                m_ChanelEdit[ch].dynLimitData.limit_bypas = data[count++]; //limit poweroff
                m_ChanelEdit[ch].chMute = data[count++];
                //channel name 
                for (int i = 0; i < CDefine.Len_PresetName; i++)
                {
                    m_chanelName[ch][i] = data[count++];
                }
            }
            #endregion over define output channel

            #region begin ...other pages....
            count = 2911 - offset;
            byte tmp = 0;
            byte mH = 0;
            byte mL = 0;
            for (int matrixIndex = 0; matrixIndex < CDefine.Matrix_TNUM; matrixIndex++)
            {
                for (int f = 0; f < 16; f++)
                {
                    tmp = data[count++];
                    mH = m_matrixAry[matrixIndex][2 * f] = (byte)(tmp & LByte);
                    mL = m_matrixAry[matrixIndex][2 * f + 1] = (byte)((tmp & HByte) >> 4);
                    //  Debug.WriteLine("show matrix bus chindex: {0} value: {1} of index {2}, value:{3} of index {4}  --------",
                    // matrixIndex, mH, 2 * f, mL, 2 * f + 1);

                }



            }
            count = 3423 - offset;

            for (int duck = 0; duck < CFinal.IO_MaxMatrixBus; duck++)
            {
                tmp = data[count++];

                m_DuckerSourch[duck] = (byte)((tmp & HByte) >> 4);
                m_DuckerBgm[duck] = (byte)(tmp & LByte);
            }

            //duck inputmixer
            m_duckerParameters[0] = data[count++];//duckerthreshold
            m_duckerParameters[2] = data[count++] * 100 + data[count++];//attack
            m_duckerParameters[4] = data[count++] * 100 + data[count++];//release          

            m_duckerParameters[3] = data[count++] * 100 + data[count++];//ducker hold
            m_duckerParameters[1] = data[count++];//duck depth
            m_duckerParameters[5] = data[count++]; //duck powON

            //StringBuilder sb = new StringBuilder();
            //sb.Append("current scene receive ducker parameters :\t");
            //for(int gg=0;gg<6;gg++)
            //{
            //    sb.Append("ducker pamter index {0}  value {1}", gg, m_duckerParameters[gg]);
            //}
            //Debug.WriteLine(sb.ToString());


            // FBC parameter,len=122
            count = 3456 - offset;
            m_FbcParam[0] = data[count++]; //fbc bypas
            m_FbcParam[3] = data[count++]; //fbc qvalue for preserved
            //----
            m_FbcParam[4] = data[count++]; //fbc_modeflag
            m_FbcParam[5] = data[count++]; //fbc_filterReleaseTime   //

            byte tmpby = 0;
            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++) //fbc status  IO_MaxMatrixBus<-->24
            {
                tmpby = data[count++];
                m_FbcFilterStatus[k] = tmpby;
                //  Debug.WriteLine("fbc status {0}  with index : {1}", tmpby, k);
            }
            //
            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++)
            {
                int kofbcGain = m_FbcEQData[k].eq_freqindex = data[count++] * CDefine.DTimesH + data[count++];
                Debug.WriteLine("fbc freq {0}  with index : {1}", kofbcGain, k);
            }
            //
            for (int k = 0; k < CFinal.IO_MaxMatrixBus; k++)
            {
                int kofbcGain = m_FbcEQData[k].eq_gainindex = (data[count++] * CDefine.DTimesH + data[count++]);

                //Debug.WriteLine("fbc gain {0}  with index : {1}", kofbcGain, k);

            }
            //
            /////Relay status:
            count = 3580 - offset;
            autoMixerParam.autoAttack = data[count++] * 100 + data[count++];
            autoMixerParam.autoRelease = data[count++] * 100 + data[count++];
            //Debug.WriteLine("-------autoRelease is  {0}", autoMixerParam.autoRelease);
            autoMixerParam.autoHavgTau = data[count++];
            autoMixerParam.autoPower = data[count++];
            for (int f = 0; f < 24; f++) //3587
            {
                m_autoMixerCHSelect[f] = data[count++];
            }

            count = 3610 - offset;

            for (int k = 0; k < 2; k++)
            {
                m_Relay[k] = data[count++];
            }

            //zone status
            count = 3612 - offset;
            m_pageZone = data[count++];
            //
            count = 3613 - offset;
            for (int k = 0; k < CDefine.Len_PresetName; k++)
            {
                m_DeviceName[k] = data[count++];
            }

            //device firmare version
            count = 3633 - offset;
            m_MCUVer = data[count++];
            m_FbcParam[2] = data[count++]; //fbc static filter
            //  Debug.WriteLine("mcuver read from device is : {0}", m_MCUVer);
            count++; //fbc all filter clear Flag
            count++; //fbc dynamictFilterClear flag
            for (int f = 0; f < 4; f++)
            {
                lock_pass[f] = data[count++];
            }

            lockFlag = data[count++];
            //device serial No.
            if (isload) return;
            count = 3642;
            for (int k = 0; k < 5; k++)
            {
                devProvision.pDevSNONumber[k] = data[count++];
            }

            // Debug.WriteLine("lock pasword read  from current sence is " + strLockPWD());

            //..................................over 
            #endregion --------------------------over..all data of current scence
        }

        /// <summary>
        /// Memory import/Export
        /// </summary>
        /// <param name="data"></param>
        public void iRead_MemoryFromDevice(List<byte> data)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chindex"></param>
        /// <param name="sname"></param>
        public void setNameOfChannel(int chindex, string sname)
        {
            // sname.Trim().CopyTo()
            byte[] tmp = UtilCover.stringToBytes(sname.Trim(), CDefine.MaxCHLength);
            Array.Clear(m_chanelName[chindex], 0, CDefine.Len_PresetName);
            if (tmp.Length > CDefine.MaxCHLength)
            {

                Array.Copy(tmp, m_chanelName[chindex], CDefine.MaxCHLength);
            }
            else
            {
                Array.Copy(tmp, m_chanelName[chindex], tmp.Length);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CMatrixData shareData()
        {
            if (matrixData == null)
            {

                matrixData = new CMatrixData();
            }
            return matrixData;

        }

        public void setFBC_EQFlat()
        {
            int CEQMax = FFTConstaint.FBC_FFTNum;
            for (byte i = 0; i < CEQMax; i++)
            {
                m_FbcEQData[i].eq_byPass = 0;
                m_FbcEQData[i].eq_Filterindex = 0;
                m_FbcEQData[i].eq_freqindex = CFinal.initial_fbcfreqAry[i];
                m_FbcEQData[i].eq_gainindex = CFinal.Inital_EQGain_index;         //CFinal.Inital_Gain_index; //0dB
                m_FbcEQData[i].eq_qfactorindex = CFinal.Initial_FBCQV_index; //68-->20.0
            }

        }

        public void setUser_EQFlat(int chindex)
        {
            int CEQMax = CFinal.NormalEQMax;
            for (byte i = 0; i < CEQMax; i++)
            {

                m_ChanelEdit[chindex].m_eqEdit[i].eq_byPass = 0;
                m_ChanelEdit[chindex].m_eqEdit[i].eq_Filterindex = 0;
                m_ChanelEdit[chindex].m_eqEdit[i].eq_freqindex = CFinal.inital_freq_ary[i];
                m_ChanelEdit[chindex].m_eqEdit[i].eq_gainindex = CFinal.Inital_EQGain_index;
                m_ChanelEdit[chindex].m_eqEdit[i].eq_qfactorindex = CFinal.Inital_QV_index;
            }

            setChGain(chindex, CDefine.DefaultFaderGain);//CFinal.Initial_ChGainIndex;
        }

        public void setChGain(int chindex, byte chg)
        {
            m_ChanelEdit[chindex].chGain = chg;
            // Debug.WriteLine("write chgain :{0} in chindex {1}", chg, chindex);
        }


        public EQParam paramOfFlatEQ(int eindex, int chindex)
        {

            EQParam param = new EQParam();

            int freqindex = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_freqindex;
            int gaindex = CFinal.Inital_EQGain_index;
            int qindex = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_qfactorindex;
            int filter = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_Filterindex;
            byte bypas = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_byPass;
            //param.Freq = 
            param.Freq = FFTConstaint.FreqTable[freqindex];
            param.Gain = FFTConstaint.EqGain[gaindex];

            if (eindex >= FFTConstaint.CEQ_MAX - 2)
                param.QValue = CFinal.DefaultQfactor;
            else
                param.QValue = StrValueTables.QVFactorTable[qindex];

            if (eindex >= 0 && eindex < FFTConstaint.CEQ_MAX - 2)
                param.TypeFilter = filter + 1;
            else  //eq 8,eq9
            {
                FFTConstaint.XoverType type = (FFTConstaint.XoverType)filter;
                filter = FFTConstaint.getFilterWithType(type);
                param.TypeFilter = filter;
            }
            if (chindex < 12 && eindex > 4 && eindex < 8)
                bypas = 1;

            param.ByPass = bypas;
            return param;

        }

        /// <summary>
        /// parm of flat FBCEQ
        /// </summary>
        /// <param name="eindex"></param>
        /// <param name="chindex"></param>
        /// <returns></returns>
        public EQParam paramOfFBCFlatEQ(int eindex)
        {

            EQParam param = new EQParam();

            int freqindex = m_FbcEQData[eindex].eq_freqindex;
            int gaindex = CFinal.Inital_EQGain_index;
            int qindex = m_FbcEQData[eindex].eq_qfactorindex;
            int filter = m_FbcEQData[eindex].eq_Filterindex;
            byte bypas = m_FbcEQData[eindex].eq_byPass;

            //param.Freq = 
            param.Freq = FFTConstaint.FreqTable[freqindex];
            param.Gain = FFTConstaint.EqGain[gaindex];
            param.QValue = StrValueTables.QVFactorTable[qindex];
            param.TypeFilter = filter + 1;
            param.ByPass = bypas;
            return param;

        }

        public EQParam paramOfEQ(int eindex, int chindex)
        {

            EQParam param = new EQParam();

            int freqindex = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_freqindex;

            int gaindex = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_gainindex;


            int qindex = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_qfactorindex;
            int filter = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_Filterindex;
            byte bypas = m_ChanelEdit[chindex].m_eqEdit[eindex].eq_byPass;

            //param.Freq = 

            param.Freq = FFTConstaint.FreqTable[freqindex];
            param.Gain = FFTConstaint.EqGain[gaindex];
            //param.QValue=StrValueTables.QVFactorTable[qindex];

            if (eindex >= FFTConstaint.CEQ_MAX - 2)  //8  9 is high pass or low pass 
                param.QValue = CFinal.DefaultQfactor;
            else
                param.QValue = StrValueTables.QVFactorTable[qindex];

            if (eindex >= 0 && eindex < FFTConstaint.CEQ_MAX - 2)
                param.TypeFilter = filter + 1;
            else  //eq 8,eq9
            {
                FFTConstaint.XoverType type = (FFTConstaint.XoverType)filter;
                filter = FFTConstaint.getFilterWithType(type);
                param.TypeFilter = filter;
            }
            if (chindex < 12 && eindex > 4 && eindex < 8)
                bypas = 1;
            param.ByPass = bypas;
            return param;

        }

        public byte lockFlag = 0;
        public byte[] lock_pass = new byte[4];//Default pass:MAA8


        public const int FBCQL = 48;
        public const int FBCQH = 96;
        public EQParam paramOfFBCEQ(int eindex)
        {

            EQParam param = new EQParam();
            int freqindex = m_FbcEQData[eindex].eq_freqindex;
            int gaindex = m_FbcEQData[eindex].eq_gainindex;
            int qindex = m_FbcEQData[eindex].eq_qfactorindex;
            int filter = m_FbcEQData[eindex].eq_Filterindex;
            byte bypas = m_FbcEQData[eindex].eq_byPass;

            //param.Freq = 
            param.Freq = freqindex;           //FFTConstaint.FreqTable[freqindex];
            param.Gain = (double)gaindex * (-0.001);    //FFTConstaint.EqGain[gaindex];
            param.QValue = (m_FbcParam[4] == 0 ? FBCQL : FBCQH);
            param.TypeFilter = filter + 1;
            param.ByPass = bypas;
            return param;

        }

        //-------------------------------------
        //public void showEQOutFreqindex()
        //{
        //    int tmp = 0;
        //    for (int i = 12; i < 24; i++)
        //    {
        //        tmp = m_ChanelEdit[12+i].m_eqEdit[9].eq_freqindex;
        //        Debug.WriteLine("output chindex {0} freqindex {1}", i, tmp);
        //    }
        //}

        #endregion

        public void pushMemoryData_toExport(List<byte> mdata)
        {
            int seindex = mdata[11];

            if (mdata.Count == CDefine.Memory_Per_Packeg_len && seindex < CDefine.Memory_Max_Package)
            {
                mdata.CopyTo(0, m_meoryRead, seindex * CDefine.Memory_Per_Packeg_len, CDefine.Memory_Per_Packeg_len);

            }



        }

        public byte[] m_meoryRead = new byte[CDefine.Memory_Max_Package * CDefine.Memory_Per_Packeg_len];

        public void sendCMD_exportMemoryDataToLocal()
        {

            CMDSender.sendCMD_MemoryExportFromDevice(NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_MemoryExport;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(30);
        }

        public void sendCMD_ImportLocalMemoryDataToDevice(int index)
        {
            byte[] tmp = new byte[CDefine.Memory_Per_Packeg_len];

            Array.Copy(CMatrixData.matrixData.m_meoryRead, index * CDefine.Memory_Per_Packeg_len, tmp,
                0, CDefine.Memory_Per_Packeg_len);

            CMDSender.sendCMD_MemoryImportFromPC(tmp, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_MemoryImport;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(30);

        }

        public void clearMemoryRead()
        {

            Array.Clear(m_meoryRead, 0, CDefine.Memory_Max_Package * CDefine.Memory_Per_Packeg_len);

        }




        public void sendCMD_ChangeChName(int chindex)
        {

            int fchindex = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? chindex - CMatrixFinal.Max_MatrixChanelNum : chindex);
            int flag = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? 1 : 0);
            byte[] m_data = new byte[22];
            m_data[0] = (byte)(fchindex + 1); //11
            m_data[1] = (byte)flag;
            Array.Copy(m_chanelName[chindex], 0, m_data, 2, CDefine.Len_PresetName);
            CMDSender.sendCMD_ChangeChannelName(m_data, NetCilent.netCilent, devProvision);


        }


        /// <summary>
        /// sendcmd Channel mute.
        /// </summary>
        public void sendCMD_ChanelMute()
        {

            byte[] m_data = new byte[CMatrixFinal.Max_MatrixChanelNum];
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
            {
                byte tmpH = (byte)((m_ChanelEdit[i].chMute & LByte) << 4);
                byte tmpL = (byte)(m_ChanelEdit[i + 12].chMute & LByte);
                m_data[i] = (byte)(tmpH | tmpL);
            }
            CMDSender.sendCMD_ChanelMute(m_data, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();
        }
        /// <summary>d
        /// it contain input/output delay command
        /// </summary>
        /// <param name="chindex"></param>
        public void sendCMD_ChanelDelay(int chindex)
        {
            if (isReady)
            {
                int fchindex = (chindex >= CMatrixFinal.Max_MatrixChanelNum ? chindex - CMatrixFinal.Max_MatrixChanelNum : chindex);
                byte[] m_data = new byte[4];
                m_data[0] = (byte)(fchindex + 1); //11
                m_data[1] = m_ChanelEdit[chindex].delayPower; //12
                m_data[2] = (byte)(m_ChanelEdit[chindex].delayTime / 256);
                m_data[3] = (byte)(m_ChanelEdit[chindex].delayTime % 256);
                CMDSender.sendCMD_Delay(m_data, (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
                       NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public bool determinePresetEmptyInPos(int pindex)
        {
            bool isEmpty = true;
            if ((m_presetName[pindex][17] == 0xaa) &&
             (m_presetName[pindex][18] == 0x55) &&
             (m_presetName[pindex][19] == 0xaa))
                isEmpty = false;
            return isEmpty;

        }

        //public static void sendCMD_PhaseGain(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        public void sendCMD_Phase(int chindex)
        {
            if (isReady)
            {

                byte[] m_data = new byte[CMatrixFinal.Max_MatrixChanelNum];
                int t = (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : CMatrixFinal.Max_MatrixChanelNum);
                for (int i = t; i < t + CMatrixFinal.Max_MatrixChanelNum; i++)
                {
                    m_data[i - t] = (byte)(m_ChanelEdit[i].invert);
                }
                CMDSender.sendCMD_Phase(m_data, (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
                       NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }

        }



        public void printFaderGain()
        {

            byte[] mdata = new byte[12];
            for (int i = 0; i < 12; i++)
            {
                mdata[i] = m_ChanelEdit[i].chGain;
            }
            Debug.WriteLine("print faderGain  " + BitConverter.ToString(mdata));

        }

        //public static void sendCMD_PhaseGain(byte[] mData, int flag, Cilent mcilent, DeviceProvision devp)
        public void sendCMD_FaderGain(int chindex)
        {
            // if (isReady)
            {

                byte[] m_data = new byte[CMatrixFinal.Max_MatrixChanelNum];
                int t = (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : CMatrixFinal.Max_MatrixChanelNum);
                for (int i = t; i < t + CMatrixFinal.Max_MatrixChanelNum; i++)
                {
                    m_data[i - t] = (m_ChanelEdit[i].chGain);
                }

                CMDSender.sendCMD_FaderGain(m_data, (chindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1),
                      NetCilent.netCilent, devProvision);
                printFaderGain();

                isReady = false;
                setNormalSendCommuniStatus();
            }

        }
        /// <summary>
        /// send all channels' DC48VFlag,no output channel
        /// </summary>
        public void sendCMD_DC48V()
        {
            byte[] m_data = new byte[CMatrixFinal.Max_MatrixChanelNum];
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
                m_data[i] = (byte)m_ChanelEdit[i].DC48VFlag;
            CMDSender.sendCMD_InputDc48V(m_data, NetCilent.netCilent, devProvision);
            setNormalSendCommuniStatus();

        }


        #region sendCMD  Define below......
        public void sendCMD_sensitivity()
        {
            if (isReady)
            {
                byte[] m_data = new byte[CMatrixFinal.Max_MatrixChanelNum];

                for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
                    m_data[i] = (byte)m_ChanelEdit[i].sensitivityindex;

                CMDSender.sendCMD_InputSensitivity(m_data, NetCilent.netCilent, devProvision);
                isReady = false;
                setNormalSendCommuniStatus();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void sendCMD_recallCurrence(bool isSpecial = false)
        {
            CMDSender.sendCMD_RecallCurrentScene(NetCilent.netCilent, devProvision);
            if (isSpecial)
                procesRecallAckComStatus();
            else
            {
                m_CommuStatus.commuteStatus = CDefine.M_RecallCursence;
                m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);
            }
        }
        public void procesRecallAckComStatus()
        {
            m_CommuStatus.commuteStatus = CDefine.M_RecallCursence;
            m_CommuStatus.responseAckCounter = getSpecialRecall(10);
        }


        public void sendCMD_recallRPMCurrence()
        {
            CMDSender.sendCMD_RecallCurrentScene(NetCilent.netCilent, rpmDevProvision);
            m_CommuStatus.commuteStatus = CDefine.M_RecallCursence;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);

        }
        public void sendCMD_recallRPVCurrence()
        {
            CMDSender.sendCMD_RecallCurrentScene(NetCilent.netCilent, rvcDevProvision);
            m_CommuStatus.commuteStatus = CDefine.M_RecallCursence;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);

        }



        public void sendCMD_ACK()
        {
            CMDSender.sendCMD_Ack(NetCilent.netCilent, devProvision);
        }

        public void sendCMD_ACKRva200()
        {
            CMDSender.sendCMD_Ack(NetCilent.netCilent, devProvision);
        }

        /// <summary>        
        /// </summary>
        /// <param name="preNum"></param>
        public void sendCMD_deletePreset(int preNum)
        {
            CMDSender.sendCMD_DeleteSinglePreset(preNum, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_PresetListRefresh;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(8);
        }

        /// <summary>
        /// recall single preset from list
        /// </summary>
        /// <param name="preNum"></param>
        public void sendCMD_recallSinglePreset(int preNum)
        {
            CMDSender.sendCMD_RecallSinglePreset(preNum, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_PresetListRefresh;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(16);
        }

        //-------------------
        /// <summary>
        /// fchindex :copy from where chindex 
        /// base 0..23
        /// </summary>
        /// <param name="fchindex"></param>
        public void sendCMD_Copy(int fchindex)  //0..23
        {
            byte[] cpyData = new byte[14];
            int f = (fchindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1);
            int chindex = (f > 0 ? fchindex - CMatrixFinal.Max_MatrixChanelNum : fchindex);
            cpyData[0] = (byte)(chindex + 1);

            cpyData[1] = (byte)f;

            Array.Copy(m_copy[f], 0, cpyData, 2, CMatrixFinal.Max_MatrixChanelNum);
            CMDSender.sendCMD_Copy(cpyData, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_PresetListRefresh;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(8);
        }

        /// <summary>
        /// get all presetList(total 25)
        /// </summary>
        public void sendCMD_recallPresetList()
        {
            CMDSender.sendCMD_ReadPresetList(NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_PresetListRefresh;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(4);

        }
        /// <summary>
        /// submit a inputname to device to save 
        /// </summary>
        /// <param name="preNum"></param>
        public void sendCMD_savePresetName(int preNum)
        {
            byte[] m_data = new byte[17];
            m_data[0] = (byte)(preNum);

            Array.Copy(m_presetName[preNum], 0, m_data, 1, CDefine.Len_FactPName);
            CMDSender.sendCMD_SavePresetWithName(m_data, NetCilent.netCilent, devProvision);
            m_CommuStatus.commuteStatus = CDefine.M_PresetListRefresh;
            m_CommuStatus.responseAckCounter = getNormalAckTimeNum(9);
        }

        #endregion






    }
}












