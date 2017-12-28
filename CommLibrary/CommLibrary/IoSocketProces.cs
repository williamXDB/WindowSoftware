using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixUpdate
 *文件名：  IoSocketProces
 *版本号：  V1.0.0.0
 *唯一标识：b2ea57f1-026d-478b-9e90-81c2fb27a55c
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：8/30/2016 2:53:03 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：8/30/2016 2:53:03 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *   
//this is udp transport..........below for reference
private void OnReceive(IAsyncResult ar)
{
 try
 {
  if (ar == currentAynchResult)
  {
   IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
   EndPoint epSender = (EndPoint)ipeSender;

   //Error comes here if we didn't have (ar == currentAynchResult) check
   int bytesRead = udpSocket.EndReceiveFrom(ar, ref epSender); 
   //process further
  }
  else
  {
   //Ignore
  }
  BeginReceive();
 }
 catch (Exception ex)
 {
  //Log exception. Don't throw exception. Most probably BeginReceive failed.
 }
}

private IAsyncResult oldAynchResult;
private void BeginReceive()
{
IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
//The epSender identifies the incoming clients
EndPoint epSender = (EndPoint)ipeSender;

//Start receiving data
oldAynchResult = udpSocket.BeginReceiveFrom(DataBuffer, 0, DataBuffer.Length,
SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);
}
/************************************************************************************/

namespace CommLibrary
{
    public class IoSocketProces   //tcp process
    {
        public static IoSocketProces ioProc = null;

        public Socket netCilent;    //= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //---------------------------------
        public delegate void socketReceive(byte[] m_rData, int dlen, EventArgs e); //socketReceive
        public event socketReceive ReceiveByteEvent;

        public delegate void socketDisconnect(Object sender, EventArgs e);//socket disconnect
        public event socketDisconnect SockDisconnectEvent;


        public delegate void socketConnected(string conIP, EventArgs e);//socket connected
        public event socketConnected SockConnectEvent;



        /// <summary>
        /// socket connected event
        /// </summary>
        private void OnSocketConnect(string strip)
        {
            try
            {
                if (SockConnectEvent != null)
                {
                    SockConnectEvent(strip, new EventArgs());
                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }


        }



        /// <summary>
        /// socket disconnect event
        /// </summary>
        private void OnSocketDisconnect()
        {
            try
            {
                if (SockDisconnectEvent != null)
                {
                    SockDisconnectEvent(this, new EventArgs());
                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// onSocket Recieve
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="rlen"></param>
        private void OnSocketReceiveEvent(byte[] pData, int rlen)
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
        //  

        public const int TCP_PORT = 5000;

        public int connetPort = TCP_PORT;

        private byte[] m_recData = new byte[8192];

        public int ConnectPort
        {
            get
            {
                return connetPort;
            }
            set
            {
                connetPort = value;

            }
        }
        //     

        public bool IsConnected
        {
            get
            {
                return netCilent.Connected;
            }

        }

        /// <summary>
        /// static object---------
        /// </summary>
        /// <returns></returns>
        public static IoSocketProces shareIoSocket()
        {
            if (ioProc == null)
            {
                ioProc = new IoSocketProces();
            }

            return ioProc;
        }


        public void initSocket()
        {
            if (netCilent != null)
            {
                if (netCilent.Connected)
                {                   
                   netCilent.Disconnect(true);                   
                }       
       
               // Debug.WriteLine("init socket here.....................................");
            }           
          
            netCilent = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);                     
            //|SocketOptionName.KeepAlive
            netCilent.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            netCilent.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            netCilent.Blocking = false;


        }

        public IoSocketProces()
        {
            initParameter();
            initSocket();
        }

        private void initParameter()
        {
            if (m_recData == null)
                m_recData = new byte[8192];
        }
        //
        public bool isNULL()
        {
            bool isnul = false;
            if (netCilent == null)
                isnul = true;
            else
                isnul = false;
            return isnul;
        }


        private void DisconnectCallBack(IAsyncResult result)
        {           
                try
                {

                    Socket remotesock = (Socket)result.AsyncState;
                    if (remotesock != null)
                    {
                        remotesock.EndDisconnect(result);
                        OnSocketDisconnect();
                    }
                }
                catch (Exception ec)
                {
                    Debug.WriteLine("endconnect error.............{0}..",ec.Message);

                }         

        }
        //
        /// <summary>
        /// socket close for temp
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (netCilent != null && netCilent.Connected)
                {
                    // netCilent.Shutdown(SocketShutdown.Both);
                    netCilent.BeginDisconnect(true, new AsyncCallback(DisconnectCallBack), netCilent);

                }
            }
            catch(Exception ec)
            {
                Debug.WriteLine("error message {0}", ec.Message);

            }

        }
        //--------check the socket is ready or no
        public bool isReady()
        {
            return ((!isNULL() && IsConnected));
        }

        //private override void dipos

        /// <summary>
        /// connect to IP with tcp
        /// </summary>
        /// <param name="strip"></param>
        public void connectWithIP(string strip)
        {
            if (string.IsNullOrEmpty(strip)) return;
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(strip), connetPort);

            try
            {
                initSocket();
                netCilent.Connect(ipe);
                BeginReceive();
                OnSocketConnect(strip); 
              //  netCilent.BeginConnect(ipe, asyncResult =>
              // {
              //     netCilent.EndConnect(asyncResult);                
                                  

              // }

              //, null);
            }
            catch(SocketException e)
            {
                Debug.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }

        //
        /// <summary>
        /// sendByte
        /// </summary>
        /// <param name="sdata"></param>
        public void sendByte(byte[] sdata)
        {
            sendByte(netCilent, sdata);
        }

        public bool showSockLog
        {
            get;
            set;
        }

        /// <summary>
        /// sendByte with socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="sdata"></param>
        public void sendByte(Socket socket, Byte[] sdata)  //send Byte array
        {

            if (socket == null || sdata.Length < 1 || !socket.Connected) return;
            try
            {
                socket.BeginSend(sdata, 0, sdata.Length, SocketFlags.None, asyncResult =>
                {

                    int length = socket.EndSend(asyncResult);
                    //完成发送消息
                    // if (showSockLog)
                    //   IPProces.printAryByte("client send :", sdata);
                    //  Debug.WriteLine("socket send byte over....");

                }, null);
            }
            catch (Exception ec)
            {
                Debug.WriteLine("Send error catch...",ec.Message);
            }


        }

       // private IAsyncResult oldAynchResult;
        private IAsyncResult currentAynchResult;
        private void BeginReceive()
        {          
            Array.Clear(m_recData, 0, m_recData.Length);
            if (netCilent == null || !netCilent.Connected) return;
            currentAynchResult = netCilent.BeginReceive(m_recData, 0, m_recData.Length,
               
            SocketFlags.None, new AsyncCallback(OnReceive),netCilent);
           
        }
        //if app is closed suddenly 
        public void clearSocket()
        {
            try
            {
                if (netCilent != null && netCilent.Connected)
                {

                    netCilent.Shutdown(SocketShutdown.Both);
                    netCilent.Disconnect(false);
                    netCilent = null;
                    GC.Collect();
                }
            }
            catch(Exception ec)
            {
                Debug.WriteLine(ec.Message);
            }

        }
       
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
               // if (ar == currentAynchResult)
                {
                    try
                    {
                        Socket remote = (Socket)ar.AsyncState;
                        if (remote!= null)
                        {
                            SocketError ercode;
                            int bytesRead = remote.EndReceive(ar,out ercode);   //netCilent.EndReceive(ar);
                           
                            if (bytesRead > 0)
                                OnSocketReceiveEvent(m_recData, bytesRead);
                        }
                    }
                    catch (Exception ec)
                    {
                        Debug.WriteLine("Onreceive error.....catch..{0}",ec.Message);
                    }
                }
             //   else
               // {
                    //Ignore
              //  }
                BeginReceive();
            }
            catch (Exception ex)
            {
                //Log exception. Don't throw exception. Most probably BeginReceive failed.
            }
        }

        

    }



}
