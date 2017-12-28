using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Collections;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：CommLibrary
 *文件名：  NoVisualScan
 *版本号：  V1.0.0.0
 *唯一标识：d6d739ae-84c9-43a5-aff1-5e73c4e02653
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：10/18/2016 4:51:09 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：10/18/2016 4:51:09 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：  scan in novisual like wscanForm ,but it is in noVisual 
 *
/************************************************************************************/
///support udp/comport to scan the device,
///the last updated date:2016.10.27
namespace CommLibrary
{
    public class DeviceScaner : Object
    {

        public delegate void onDeviceStopScan(Object sender, EventArgs e);//socket disconnect
        public event onDeviceStopScan onUDPStopScanEvent;
        //
        public delegate void onDeviceBeginScan(Object sender, EventArgs e);//socket disconnect
        public event onDeviceBeginScan onUDPBeinScanEvent;
        //
        public delegate void onDeviceScanChanged(Object sender, RouterInfo rpinfo, EventArgs e);//socket disconnect
        public event onDeviceScanChanged onScanChangedEvent;

        public bool hasStopScan { get; set; }
        /// <summary>
        /// udpSocket begin.........................................
        /// </summary>
        ///

        public bool isSupportUSB
        {
            get;
            set;
        }

        private ScanThreader thread_IP210Receive; //IP210
        private ScanThreader thread_IP210Sender; //IP210
        private Socket udpSock_IP210;
        private EndPoint remotePoint_IP210;

        private const int Port_IP210 = 8000;
        //HAF11 --module
        private ScanThreader thread_HAF11Receive;
        private ScanThreader thread_HAF11Sender;
        private Socket udpSock_HAF11;
        private const string Chek_CMD_HF11 = "HF-A11ASSISTHREAD";
        private const int Port_HAF11 = 48899;
        private EndPoint remotePoint_HAF11;
        //
        private const int Send_Delay = 50;
        private string localIP;
        public List<RouterInfo> scanedIpList = new List<RouterInfo>();

        public bool DoReceiveFlag = false;
        public int delayMills = 4;

        private DispatcherTimer timer;

        public int indexOfScanedIP(String strIP)
        {
            int result = -1;
            if (strIP.Trim().Length < 1 || scanedIpList.Count < 1)
                result = -1;
            result = scanedIpList.FindIndex(r => r.RouterAddr.Equals(strIP));
            return result;
        }

        public DeviceScaner()
        {
            try
            {
                initParameters();
                isLoadedFailure = false;
                isSupportUSB = false;
            }
            catch (Exception e)
            {
                isLoadedFailure = true;
                Debug.WriteLine("load socket failure....................");
            }

        }

        public bool isLoadedFailure = false;
        public int DelayMills
        {
            get;
            set;
        }
        public const int SCAN_DES = 3;

        private void initSocket()
        {
            initIP210();
            initHAF11();
        }
        public void initParameters()
        {
            hasStopScan = true;
            initSocket();
            localIP = IPProces.getIPAddress();//localIP;;
            //-------------------------------           
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, SCAN_DES);
            timer.Stop();
            //ip210            
            thread_IP210Receive.DoSomethingEvent += new ScanThreader.threadDosomething(scanThread_IP210Receive);
            thread_IP210Sender.DoSomethingEvent += new ScanThreader.threadDosomething(scanthread_ip210sender);
            //haf11
            thread_HAF11Receive.DoSomethingEvent += new ScanThreader.threadDosomething(scanThread_HFA11Receive);
            thread_HAF11Sender.DoSomethingEvent += new ScanThreader.threadDosomething(scanthread_HAF11sender);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("timer_tick gogogog1");
            stopScan();

        }
        public void stopScan()
        {
            if (!hasStopScan)
            {
                Debug.WriteLine("close socket and stopscan....................");

                DoReceiveFlag = false;
                if (thread_IP210Receive != null)
                    thread_IP210Receive.Stop();

                if (thread_IP210Sender != null)
                    thread_IP210Sender.Stop();

                if (thread_HAF11Sender != null)
                    thread_HAF11Sender.Stop();
                if (thread_HAF11Receive != null)
                    thread_HAF11Receive.Stop();

                hasStopScan = true;
                if (timer != null)
                    timer.Stop();
                resetRemotPoint();

                Thread.Sleep(100);
                if (onUDPStopScanEvent != null)
                {
                    onUDPStopScanEvent(this, new EventArgs());

                }
            }
            else
            {

                // Debug.WriteLine("stopscan is false so no need to close xxxx....................");
            }

        }

        /// <summary>
        /// 
        /// </summary>


        public void resetRemotPoint()
        {
            //remote
            IPEndPoint ipep = new IPEndPoint(IPAddress.Broadcast, Port_IP210);
            remotePoint_IP210 = (EndPoint)(ipep);
            //remote
            ipep = new IPEndPoint(IPAddress.Broadcast, Port_HAF11);
            remotePoint_HAF11 = (EndPoint)(ipep);
        }

        #region IP210 module define
        /// <summary>
        /// initIP210 module about 
        /// </summary>
        public void initIP210()
        {

            udpSock_IP210 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // udpSock_IP210.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);         
            udpSock_IP210.Blocking = false;



            thread_IP210Sender = new ScanThreader();
            thread_IP210Receive = new ScanThreader();
            localIP = IPProces.getIPAddress();//localIP;


            IPEndPoint ipLocalPoint = new IPEndPoint(IPAddress.Any, Port_IP210 - 1);
            udpSock_IP210.Bind(ipLocalPoint);
            udpSock_IP210.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            //remote
            IPEndPoint ipep = new IPEndPoint(IPAddress.Broadcast, Port_IP210);
            remotePoint_IP210 = (EndPoint)(ipep);

        }
        public void sendIP210()
        {
            try
            {
                if (udpSock_IP210 != null)
                {
                    byte[] data_IP210 = { 0x01, 0x02, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xFF, 0xFF };
                    udpSock_IP210.SendTo(data_IP210, data_IP210.Length, SocketFlags.None, remotePoint_IP210);
                    //  string strIP = string.Format("send ip210  udp socket send ip：{0} ", udpSock_IP210.ToString());
                    //  Debug.WriteLine(strIP);
                }
            }
            catch
            {

            }

        }

        public void udpRecive_IP210()
        {

            byte[] data = new byte[1024];
            string strIP;
            while (DoReceiveFlag)
            {

                if (udpSock_IP210 == null || udpSock_IP210.Available < 1)
                {
                    Thread.Sleep(delayMills + 10);
                    continue;
                }


                //  Debug.WriteLine("IP 210 receiver   wake  now...............");
                int rlen = udpSock_IP210.ReceiveFrom(data, ref remotePoint_IP210);
                byte[] kb = new byte[rlen + 1];
                Array.Copy(data, kb, rlen);
                IPProces.printAryByte("IP210 data receive ", kb);
                strIP = remotePoint_IP210.ToString();
                // string strIPN = "IP210 reciev.... " + strIP + "  data: " + BitConverter.ToString(data);
                // IPProces.printAryByte("thread receive-- ", data, rlen);
                addToNewIP(strIP, Device_AccesType.Device_Lan);
            }

        }
        #endregion
        //-------------------------------
        #region HAF11 module
        public void initHAF11()
        {
            //

            udpSock_HAF11 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //  udpSock_HAF11.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpSock_HAF11.Blocking = false;


            thread_HAF11Sender = new ScanThreader();
            thread_HAF11Receive = new ScanThreader();


            IPEndPoint ipLocalPoint = new IPEndPoint(IPAddress.Any, Port_HAF11); //- 1
            udpSock_HAF11.Bind(ipLocalPoint);
            udpSock_HAF11.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            //remote
            IPEndPoint ipep = new IPEndPoint(IPAddress.Broadcast, Port_HAF11);
            remotePoint_HAF11 = (EndPoint)(ipep);

        }
        /// <summary>
        /// sendHAF11
        /// </summary>
        public void sendHAF11()
        {
            try
            {
                if (udpSock_HAF11 != null)
                {
                    //Chek_CMD_HF11
                    byte[] data_HF11 = System.Text.Encoding.ASCII.GetBytes(Chek_CMD_HF11);
                    udpSock_HAF11.SendTo(data_HF11, data_HF11.Length, SocketFlags.None, remotePoint_HAF11);
                    string strIP = string.Format("send haf11 udp socket send ip：{0} ", data_HF11);
                    Debug.WriteLine(strIP);
                }
                else
                {
                    Debug.WriteLine("send haf11 ,but is  udpsock haff11 is null........");

                }
            }
            catch
            {

            }
        }

        //receive HAF11
        public void udpRecive_HAF11()
        {

            byte[] data = new byte[1024];
            string strIP;
            //   Debug.WriteLine("Haf11 receive begin.........................{0}",DoReceiveFlag);
            while (DoReceiveFlag)
            {

                if (udpSock_HAF11 == null || udpSock_HAF11.Available < 1)
                {
                    Thread.Sleep(delayMills);
                    //   Debug.WriteLine("no condition in haf11.........................");
                    continue;
                }

                //Debug.WriteLine("haf11 wake  now...............");
                int rlen = udpSock_HAF11.ReceiveFrom(data, ref remotePoint_HAF11);
                byte[] kb = new byte[rlen + 1];
                Array.Copy(data, kb, rlen);
                IPProces.printAryByte("haf11 data receive ", kb);
                strIP = remotePoint_HAF11.ToString();
                addToNewIP(strIP, Device_AccesType.Device_AP);

                //  string strIPN = "HAF11 reciev.... " + strIP + "  data: " + BitConverter.ToString(data);
                //  Debug.WriteLine(strIPN);

            }




        }
        #endregion

        public bool isExistAddr(string strIP)
        {
            bool isExist = false;
            foreach (RouterInfo rp in scanedIpList)
            {
                //  Debug.WriteLine("compare ip is : " + strTemp+"  with iP: "+strIP);
                if (string.Compare(rp.RouterAddr, strIP) == 0)
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;

        }
        //-------------------------
        public void addToNewIP(string strIP, Device_AccesType routerType)
        {

            string tmpIP = strIP.Trim();

            if (routerType != Device_AccesType.Device_Com)
                tmpIP = tmpIP.Substring(0, tmpIP.IndexOf(":"));

            Debug.WriteLine("now.........add to new IP is :" + tmpIP);

            if ((tmpIP.Length < 1) || (tmpIP == localIP) || isExistAddr(tmpIP)) return;

            RouterInfo iRP = new RouterInfo();
            iRP.RouterStyle = routerType;
            if (routerType == Device_AccesType.Device_Com)
                iRP.RouterMac = "FF-FF-FF-FF-FF-FF";
            else  //ip condition,state
                iRP.RouterMac = IPProces.getMac(tmpIP);

            iRP.RouterAddr = tmpIP;

            scanedIpList.Add(iRP);
            //   Debug.WriteLine("tedected ip is :   " + tmpIP);

            if (onScanChangedEvent != null)
            {
                onScanChangedEvent(this, iRP, new EventArgs());
            }


        }
        public static string ProficUSB = "Prolific USB";//PL2303
        //-------------------------------------------------------------------------------------------------
        public void startScan()
        {
            scanedIpList.Clear();

            hasStopScan = false;
            DoReceiveFlag = true;
            thread_IP210Receive.Start();
            thread_IP210Sender.Start();

            thread_HAF11Receive.Start();
            thread_HAF11Sender.Start();

            timer.Start();
            if (onUDPBeinScanEvent != null)
            {
                onUDPBeinScanEvent(this, new EventArgs());

            }
            if (isSupportUSB)
            {
                string comList = ComCilent.listUsbPort(ProficUSB);
                if (comList != null && comList.Trim().Length > 3)
                    addToNewIP(comList, Device_AccesType.Device_Com);
                ////serialport scan below
                //string[] comList = ComCilent.listUsbPort();
                //for (int i = 0; i < comList.Length; i++)
                //{
                //    addToNewIP(comList[i], Device_AccesType.Device_Com);

                //}
            }

        }



        /// <summary>
        /// event define..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scanThread_IP210Receive(object sender, EventArgs e)
        {
            udpRecive_IP210();
            Debug.WriteLine("scan thread IP210 receive now...");

        }

        private void scanthread_ip210sender(object sender, EventArgs e)
        {
            if (DoReceiveFlag)
                sendIP210();
            Debug.WriteLine("scan thread IP210 sender now...");

        }

        private void scanThread_HFA11Receive(object sender, EventArgs e)
        {
            Debug.WriteLine("scan thread hfa11 recevie now.....");
            udpRecive_HAF11();


        }

        private void scanthread_HAF11sender(object sender, EventArgs e)
        {
            if (DoReceiveFlag)
                sendHAF11();
            Debug.WriteLine("scan thread haff11 sender now...");

        }
        //---------------------------         

        public void clearSocket()  //when it close 
        {
            DoReceiveFlag = false;
            if (udpSock_IP210 != null)
                udpSock_IP210.Shutdown(SocketShutdown.Both);

            if (thread_IP210Receive != null)
                thread_IP210Receive.Stop();

            if (thread_IP210Sender != null)
                thread_IP210Sender.Stop();

            if (udpSock_IP210 != null)
                udpSock_IP210.Close();

            //
            if (udpSock_HAF11 != null)
                udpSock_HAF11.Shutdown(SocketShutdown.Both);


            if (thread_HAF11Sender != null)
                thread_HAF11Sender.Stop();

            if (thread_HAF11Receive != null)
                thread_HAF11Receive.Stop();

            if (udpSock_HAF11 != null)
                udpSock_HAF11.Close();
            hasStopScan = true;
            if (timer != null)
                timer.Stop();
            //  GC.Collect();
        }



    }
}
