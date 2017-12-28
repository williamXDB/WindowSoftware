using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixUpdate
 *文件名：  CDeviceInfo
 *版本号：  V1.0.0.0
 *唯一标识：8ecf212b-17ad-43fd-946f-cf7fe131a1b2
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/8/2016 8:47:05 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/8/2016 8:47:05 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{

    public class cPortInfo  //connected port information
    {
        byte portindex;
        byte portType; //port in,port out,text port,rd port etc.
        bool isHasJoined;
        public cPortInfo()
        {

        }

    }


    public enum Module_Type
    {
        Mod_CLIV = 100, //cl-4
        Mod_LAN = 101, //lan interface
        Mod_DLM808 = 102,//dlm808
        Mod_MAVIII = 103,//matrxiA8
        Mod_RVA100 = 104,//RVA200
        Mod_RVC100 = 105,//rvc1000
        Mod_RIO100 = 106,//rio100

        Mod_RPM100 = 107,//rpm100
        Mod_TxtLft = 108,//textleft
        Mod_TxtRht = 109,//text right
    }


    public enum SPecial_DevID
    {
        DevID_LTXT = 0x5000,
        DevID_RTXT = 0x5001,
        DevID_LanRouter = 0x5002,
        DevID_CLIV = 0x5003,
        //--------------------
        DevID_Normal = 0x1000,
    }

    public class DeviceProvision
    {
        public const int DefaultSerialNumberLen = 5;
        public const int Len_DevName = 16;
        public const int Len_FirmVer = 2;
        public UInt16 pMachineID; //also name appID
        public UInt16 pDeviceID;
        public byte[] pDeviceName;
        public byte[] pFirmwareVer;//mcu,dsp ver each one byte
        public byte[] pDevSNONumber; //DeviceserialNumber


        public DeviceProvision()
        {
            pDeviceID = 0;
            pMachineID = 0;
            pDeviceName = new byte[Len_DevName];
            pDevSNONumber = new byte[DefaultSerialNumberLen];
            pFirmwareVer = new byte[Len_FirmVer];
        }

        public string strDevName
        {
            get
            {
                return getStrDevName();

            }
            set
            {
                setStrDevName(value);
            }
        }


        public bool isSameToAnotherDevice(DeviceProvision anotherDev)
        {
            return (pDeviceID == anotherDev.pDeviceID && pMachineID == anotherDev.pMachineID);
        }

        public string getStrDevName()
        {
            return UtilCover.bytesToString(pDeviceName, Len_DevName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strname"></param>
        public void setStrDevName(string strname)
        {
            Array.Clear(pDeviceName, 0, Len_DevName);
            byte[] mData = UtilCover.stringToBytes(strname, Len_DevName);
            Array.Copy(mData, pDeviceName, mData.Length);
        }

        public void devProvisionCopy(DeviceProvision devp)
        {
            pDeviceID = devp.pDeviceID;
            pMachineID = devp.pMachineID;
            Array.Copy(devp.pDeviceName, pDeviceName, Len_DevName);
            Array.Copy(devp.pFirmwareVer, pFirmwareVer, 2);
            //no copy the device name
            Array.Copy(devp.pDevSNONumber, pDevSNONumber, DefaultSerialNumberLen);
        }

    }

    public enum Device_AccesType
    {
        Device_Lan = 100,
        Device_AP = 101,
        Device_Com = 102
    };

    public class RouterInfo
    {
        public string RouterAddr { get; set; }  //IP
        public string RouterMac { get; set; }
        private Device_AccesType RouterType;

        public Device_AccesType RouterStyle
        {
            get { return RouterType; }
            set { RouterType = value; }
        }
        public RouterInfo()
        {
            RouterStyle = Device_AccesType.Device_Lan;
        }

    };

    public class CDeviceInfo
    {

        public Module_Type devModuleType;
        public string noteTxt { get; set; }

        public DeviceProvision devProv; //contain appID,devid
        public Point DevPoint;//Device point      
        public int lineIndex { get; set; } //for CL-4 device        

        
        public bool iIsReady { get; set; } //preserved
        public int workedHours { get; set; } //preserved
        public int burnedCount { get; set; } //preserved

        #region for preserved used...
        public UInt16 DeviceID { get; set; } //preserved
        public UInt16 AppID { get; set; } //preserved
        public string strDevName { get; set; } //preserved
        public string iFirmwareVersion { get; set; } //preserved
        #endregion




        /// <summary>
        /// device is comserial or lan interface
        /// </summary>
        public Device_AccesType routerStyle { get; set; }


        public CDeviceInfo()
        {
            routerStyle = Device_AccesType.Device_Lan;
            devProv = new DeviceProvision();
            devProv.pDeviceID = 0x1000;
            devProv.setStrDevName("Device");


        }

        public string getDevInfo()
        {
            string devstr = string.Format("App ID:{0}  Device ID:{1}  Device Name:{2}",
                devProv.pMachineID.ToString("X2"), devProv.pDeviceID.ToString("X2"), devProv.strDevName);
            return devstr;
        }

        public void copyDevinfo(CDeviceInfo devf)
        {
            if (devf != null && devf.devModuleType == devModuleType)
            {
                devProv.devProvisionCopy(devf.devProv);
                DevPoint = devf.DevPoint;//device position
                noteTxt = devf.noteTxt; //note text or not
                lineIndex = devf.lineIndex; //lineindex
                workedHours = devf.workedHours;
                burnedCount = devf.burnedCount;
                iFirmwareVersion = devf.iFirmwareVersion;

            }

        }

        public void printDevInfo()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendFormat("device moduletype is : {0}", devModuleType);
            strb.AppendFormat("  deviceName: {0}", devProv.pDeviceName);
            strb.AppendFormat("  machineID: {0}", devProv.pMachineID);
            strb.AppendFormat("  deviceID: {0}", devProv.pDeviceID);
            strb.AppendFormat("  module point: {0}", DevPoint);

            Debug.WriteLine(strb.ToString());
        }


    }
}
