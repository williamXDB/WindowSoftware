using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixUpdate
 *文件名：  IPProces
 *版本号：  V1.0.0.0
 *唯一标识：82c86802-257a-4dda-9463-646c5e62f2d0
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：8/27/2016 2:53:39 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：8/27/2016 2:53:39 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    
    public class IPProces
    {

        public static string getIPAddress()
        {
            //get local address IP 
            IPAddress[] adressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            if (adressList.Length < 1)
            {
                return "";
            }
            return adressList[0].ToString();
        }
        public static bool isInSameSubnet(string sip1, string sip2)
        {
            var ip1 = IPAddress.Parse(sip1);
            var ip2 = IPAddress.Parse(sip2);
            return ip1.IsInSameSubnet(ip2, SubnetMask.ClassC); //invoke example

        }
      

        public static IPAddress getValidIP(String ip)
        {
            IPAddress lip = null;
            try
            {
                if (IPAddress.TryParse(ip, out lip))
                {
                    throw new ArgumentException("Invalid IP ,cannot startup");
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("invalid IP " + e.ToString());
                return null;

            }

            return lip;
        }

        /// <summary>
        /// send Arp and fetch MAC address 
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        /// 
        [DllImport("Iphlpapi.dll")]
        static extern int SendARP(Int32 destIP,Int32 scrcIP,ref Int64 macAddress,ref Int32 PhyAddrLen);



        public static string getMac(string p_id)
        {
            IPAddress _address;

            if (!IPAddress.TryParse(p_id, out _address)) return "";
            uint DestIP = BitConverter.ToUInt32(_address.GetAddressBytes(), 0);

            Int64 pMacAddr = 0;
            Int32 PhyAddLen = 6;
            int error_code = SendARP((int)DestIP, 0, ref pMacAddr, ref PhyAddLen);
            byte[] _bytes=BitConverter.GetBytes(pMacAddr);
            return BitConverter.ToString(_bytes,0,6);

        }

        public static int getValidPort(string port)
        {

            int lport = 0;

            try
            {
                if (port == "")
                {
                    throw new ArgumentException("Port is invalid,cannot startup");
                }
                lport = System.Convert.ToInt32(port.Trim());//string to int



            }
            catch (Exception e)
            {

                Console.WriteLine("invalid port " + e.ToString());

            }
            return lport;
        }

        /// <summary>
        /// print ary byte 
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="mdata"></param>
        public static void printAryByte(String prestr, byte[] mdata)
        {

            string strm = string.Format("{0} data length {1}", prestr, mdata.Length);
            strm += "  " + BitConverter.ToString(mdata);

            System.Console.WriteLine(strm);

        }

        /// <summary>
        /// printbyte 
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="mdata"></param>
        /// <param name="length"></param>
        public static void printAryByte(string prestr, byte[] mdata, int length)
        {
            if (length <= mdata.Length)
            {
                StringBuilder buder = new StringBuilder();
                buder.Append(prestr);
                for (int i = 0; i < length; i++)
                {
                    buder.Append(" " + mdata[i].ToString("X2"));
                }
                Console.WriteLine(buder.ToString());
            }
        }




    }
}
