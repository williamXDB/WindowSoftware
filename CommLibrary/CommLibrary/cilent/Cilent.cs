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
 *命名空间：seriportDemo
 *文件名：  comCilent
 *版本号：  V1.0.0.0
 *唯一标识：3b371b32-c1b8-48ae-a1a7-1a38c0189a22
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：10/27/2016 11:19:12 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：10/27/2016 11:19:12 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/



namespace CommLibrary
{

    //public enum ClientType
    //{
    //    COM_Type = 0x101,
    //    Net_Type = 0x102
    //};

    public abstract class Cilent : Object
    {
        ///
        //delegate define below:
        public delegate void onByteReceive(String addr, byte[] m_rData, int dlen, EventArgs e); //socketReceive      

        public delegate void onDisconnect(String addr, EventArgs e);//socket disconnect    

        public delegate void onConnected(string conIP, EventArgs e);//socket connected

        public string Param { get; set; }

        public int iTag { get; set; }

        public Cilent()
        {


        }

        public virtual void clearRlease()
        {

        }
        //
        public virtual void connect(string mparam)
        {

        }

        public virtual void disConnect()
        {

        }

        public abstract bool isConnected();


        protected virtual void beginReceive()
        {

        }

        public virtual void sendByte(byte[] sdata)
        {

        }

        public virtual void clear()
        {


        }



    }
}
