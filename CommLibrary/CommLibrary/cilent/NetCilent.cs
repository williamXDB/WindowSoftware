using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：seriportDemo
 *文件名：  NetCilent
 *版本号：  V1.0.0.0
 *唯一标识：bb88aa74-6bee-45a5-8f3e-e5df6800537f
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：10/27/2016 1:41:16 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：10/27/2016 1:41:16 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class NetCilent : Cilent
    {
        private Socket _socket;

        public static NetCilent netCilent = null;

        public event onByteReceive ReceiveByteEvent;
        public event onDisconnect onDisconnectEvent;
        public event onConnected onConnectedEvent;


        public NetCilent()
        {
            connectPort = DefaultConnetPort;
            initSocket();
        }

        public static NetCilent shareCilent()
        {

            if (netCilent == null)
            {
                netCilent = new NetCilent();
            }
            return netCilent;
        }

        /// <summary>
        /// 
        /// </summary>
        private int DefaultConnetPort = 5000;
        private byte[] m_recData = new byte[8192];
        public int connectPort { get; set; }


        public override bool isConnected()
        {
            return _socket.Connected;
        }

        #region disconnect socket define...........
        /// <summary>
        /// disconnect define....below..
        /// </summary>
        public override void disConnect()
        {
            //  base.disConnect();
            if (_socket != null && _socket.Connected)
            {
                _socket.BeginDisconnect(true, new AsyncCallback(disconnectCallBack), _socket);

            }

        }
        private void disconnectCallBack(IAsyncResult result)
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
            catch
            {
                /// Debug.WriteLine("endconnect error...............");

            }

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>

        public void initSocket()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    _socket.Disconnect(true);
                }

                // Debug.WriteLine("init socket here.....................................");
            }

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //|SocketOptionName.KeepAlive
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            _socket.Blocking = false;


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

                    String strRIP = ((System.Net.IPEndPoint)_socket.RemoteEndPoint).Address.ToString();
                    ReceiveByteEvent(strRIP, pData, rlen, new EventArgs());
                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }
        //----------------
        /// <summary>
        /// socket connected event
        /// </summary>
        private void OnSocketConnect(string strip)
        {
            try
            {
                if (onConnectedEvent != null)
                {
                    onConnectedEvent(strip, new EventArgs());
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
                if (onDisconnectEvent != null)
                {
                    String strRIP = ((System.Net.IPEndPoint)_socket.RemoteEndPoint).Address.ToString();
                    onDisconnectEvent(strRIP, new EventArgs());
                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }


        }




        public override void connect(string strip)
        {
            if (string.IsNullOrEmpty(strip)) return;
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(strip), connectPort);

            try
            {
                initSocket();
                _socket.BeginConnect(ipe, asyncResult =>
                {
                    _socket.EndConnect(asyncResult);
                    beginReceive();
                    OnSocketConnect(strip);

                }


              , null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        #region sendbyte define.......................

        /// <summary>
        /// send byte ov
        /// </summary>
        /// <param name="sdata"></param>
        public override void sendByte(byte[] sdata)
        {
            sendByte(_socket, sdata);
        }
        /// <summary>
        /// sendByte with socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="sdata"></param>
        protected void sendByte(Socket socket, Byte[] sdata)  //send Byte array
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
            catch
            {
                Debug.WriteLine("Send error catch...");
            }


        }
        #endregion

        //if app is closed suddenly 
        public override void clear()
        {
            if (_socket != null && _socket.Connected)
            {

                _socket.Shutdown(SocketShutdown.Both);
                _socket.Disconnect(false);
                _socket = null;
                GC.Collect();
            }

        }
        // private IAsyncResult oldAynchResult;
        private IAsyncResult currentAynchResult;


        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                //  if (ar == currentAynchResult) //if add this,it will mispackage sometimes.,so comment it.
                {
                    try
                    {
                        Socket remote = (Socket)ar.AsyncState;
                        if (remote != null)
                        {
                            SocketError ercode;
                            int bytesRead = remote.EndReceive(ar, out ercode);
                            if (bytesRead > 0)
                                OnReceiveEvent(m_recData, bytesRead);
                        }
                    }
                    catch
                    {
                        Debug.WriteLine("Onreceive error.....catch..");
                    }
                }
                // else
                /// {
                //Ignore
                // }
                beginReceive();
            }
            catch (Exception ex)
            {
                //Log exception. Don't throw exception. Most probably BeginReceive failed.
            }
        }
        protected override void beginReceive()
        {
            if (_socket == null || !_socket.Connected) return;
            Array.Clear(m_recData, 0, m_recData.Length);
            currentAynchResult = _socket.BeginReceive(m_recData, 0, m_recData.Length,
            SocketFlags.None, new AsyncCallback(OnReceive), _socket);
        }





    }
}
