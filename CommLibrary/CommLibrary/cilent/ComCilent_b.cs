﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Management;
using System.IO;
using System.IO.Ports;
using System.Threading;    //used to perform WMI queries,and add to the reference of system.Management.dll

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：seriportDemo
 *文件名：  Comport
 *版本号：  V1.0.0.0
 *唯一标识：3094d7a7-8154-4fb8-b1a4-6c53fc733758
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：10/27/2016 11:15:50 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：10/27/2016 11:15:50 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class ComCilent : Cilent
    {


        public static ComCilent comCilent = null;


        private SerialPort _comport;
        public event onByteReceive ReceiveByteEvent;
        public event onDisconnect onDisconnectEvent;
        public event onConnected onConnectedEvent;



        public int conPort
        {

            get
            {
                return _comport.BaudRate;
            }
            set
            {
                _comport.BaudRate = value;
            }

        }

        public static ComCilent shareCilent()
        {
            if (comCilent == null)
            {
                comCilent = new ComCilent();
            }

            return comCilent;
        }

        public ComCilent()
        {

            initalProperty();
        }


        private void initalProperty()
        {
            if (_comport == null)
                _comport = new SerialPort();

            conPort = 57600;
            _comport.BaudRate = 57600;
            _comport.ReadBufferSize = 8192;
            _comport.WriteBufferSize = 8192;

            _comport.Parity = Parity.None;//无奇偶校验位
            _comport.StopBits = StopBits.One;//两个停止位8
            _comport.DataBits = 8;
            _comport.Handshake = Handshake.None;
            _comport.RtsEnable = true;
            _comport.DtrEnable = true;
            _comport.ReceivedBytesThreshold = 1;//设置 DataReceived 事件发生前内部输入缓冲区中的字节数
            _comport.WriteTimeout = 1000;
            _comport.ReadTimeout = 100;

        }

        public override bool isConnected()
        {
            return (_comport!=null &&_comport.IsOpen);
        }

        public override void disConnect()
        {
            //  base.disConnect();
            if (_comport != null)
            {
                _comport.Close();
                _comport.Dispose();

            }             
            
            // _comport.Close();

            if (onDisconnectEvent != null)
            {
                onDisconnectEvent(this, new EventArgs());
            }


        }

        public override void connect(string mparam)
        {
            //  base.connect();
            if (_comport!=null && _comport.IsOpen)
            {
                _comport.Close();
                _comport.Dispose();
            }
            Param = mparam;
            initalProperty();
            _comport.PortName = Param;
            _comport.Open();

            if (_comport.IsOpen && onConnectedEvent != null)
            {
                onConnectedEvent(Param, new EventArgs());
            }
            if (_comport.IsOpen)
                beginReceive();
        }

        /// <summary>
        /// onReceiveEvent
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="rlen"></param>
        private void OnReceiveEvent(byte[] pData, int rlen)
        {
            try
            {
                if (ReceiveByteEvent != null)
                {
                    ReceiveByteEvent(pData, rlen, new EventArgs());
                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }

        public override void sendByte(byte[] sdata)
        {
            if (_comport == null || sdata.Length < 1 || !_comport.IsOpen) return;
            _comport.DiscardInBuffer();
            _comport.DiscardOutBuffer();
            _comport.BaseStream.BeginWrite(sdata, 0, sdata.Length, asyncResult =>
            {
                _comport.BaseStream.EndWrite(asyncResult);                
                _comport.BaseStream.Flush();
            }, null);


        }

        private IAsyncResult currentAynchResult;
        private void onReceive(IAsyncResult ar)
        {

            if (ar == currentAynchResult)
            {

                SerialPort sport = (SerialPort)ar.AsyncState;
                if (sport != null && sport.IsOpen)
                {

                    int byteRead = sport.BaseStream.EndRead(ar);
                    if (byteRead > 0)
                    {
                        OnReceiveEvent(m_read, byteRead);
                    }

                }


            }
            beginReceive();

        }
        /// <summary>
        /// 
        /// </summary>
        private static byte[] m_read = new byte[8192];

        /// <summary>
        /// receive byte being
        /// </summary>
        protected override void beginReceive()
        {
            if (_comport == null || !_comport.IsOpen) return;
            Array.Clear(m_read, 0, m_read.Length);
            currentAynchResult = _comport.BaseStream.BeginRead(m_read, 0, m_read.Length
                , new AsyncCallback(onReceive), _comport);

        }


        public override void clear()
        {
            if (_comport.IsOpen)
            {
                _comport.DiscardInBuffer();
                _comport.DiscardOutBuffer();
                _comport.Close();

            }
            _comport = new SerialPort(Param);
            _comport.Dispose();
            _comport = null;
            GC.Collect();
        }
            
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] listUsbPort()
        {
            List<string> portList = new List<string>();
            try
            {

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSSerial_PortName");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //Console.WriteLine("-----------------------------------");
                    //Console.WriteLine("MSSerial_PortName instance");
                    //Console.WriteLine("-----------------------------------");
                    //Console.WriteLine("InstanceName: {0}", queryObj["InstanceName"]);

                    //Console.WriteLine("-----------------------------------");
                    //Console.WriteLine("MSSerial_PortName instance");
                    //Console.WriteLine("-----------------------------------");
                    //Console.WriteLine("PortName: {0}", queryObj["PortName"]);


                    //If the serial port's instance name contains USB it must be a USB to serial device
                    if (queryObj["InstanceName"].ToString().Contains("USB"))
                    {
                        //  Console.WriteLine(queryObj["PortName"] + " is a USB to SERIAL adapter/converter");
                        portList.Add(queryObj["PortName"] as string);

                    }
                }

            }
            catch (ManagementException e)
            {
                Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                // MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            return portList.ToArray();
        }

    }
}
