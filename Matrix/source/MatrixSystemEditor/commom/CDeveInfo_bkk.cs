using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.commom
 *文件名：  CDeveInfo
 *版本号：  V1.0.0.0
 *唯一标识：1190d119-4df2-4209-b803-046d1c77476f
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/19/2016 10:44:01 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/19/2016 10:44:01 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/
using System.Diagnostics;
using CommLibrary;


namespace MatrixSystemEditor
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

    public class CDeveInfo
    {
        public Module_Type devModuleType;
        public String DeviceName { get; set; }
        public string noteTxt { get; set; }

        public DeviceProvision devProv;
        public Point DevPoint;//Device point      
        public int lineIndex { get; set; } //for CL-4 device 
        //
        public CDeveInfo()
        {
            devProv = new DeviceProvision();
            devProv.pDeviceID = 0x1000;          
            DeviceName = "DeviceID";

        }

        /// <summary>
        /// factory
        /// </summary>
        /// <returns></returns>
        public static CDeveInfo createDevInfo()
        {
            return new CDeveInfo();
        }

        public void copyDevinfo(CDeveInfo devf)
        {
            if (devf != null && devf.devModuleType == devModuleType)
            {
                DeviceName = devf.DeviceName;               
                devProv.devProvisionCopy(devf.devProv);
                DevPoint = devf.DevPoint;              
                noteTxt = devf.noteTxt;
                lineIndex = devf.lineIndex;
            }

        }

        public void printDevInfo()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendFormat("device moduletype is : {0}", devModuleType);
            strb.AppendFormat("  deviceName: {0}", DeviceName);
            strb.AppendFormat("  machineID: {0}", devProv.pMachineID);
            strb.AppendFormat("  deviceID: {0}", devProv.pDeviceID);            
            strb.AppendFormat("  module point: {0}", DevPoint);

            Debug.WriteLine(strb.ToString());
        }

    }
}
