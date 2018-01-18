using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：sysLib
 *文件名：  UtilCover
 *版本号：  V1.0.0.0
 *唯一标识：4b8301ff-75b6-451f-aef7-ca82b0746a16
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/13/2016 2:50:11 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/13/2016 2:50:11 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class UtilCover
    {
        //
        public const int MIN_IP_LEN = 11;//192.168.1.1
        public static bool IPCheck(string IP)
        {
            if (IP == null || IP.Trim().Length < MIN_IP_LEN) return false;

            string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";//创建正则表达式字符串
            return Regex.IsMatch(IP,//使用正则表达式判断是否匹配
                ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"));
        }

        public static bool isASCII(byte b)
        {
            bool res = false;
            if (
                (b >= 48 && b <= 57) ||
                (b >= 41 && b <= 90) ||
                (b >= 97 && b <= 122) ||
                (b == 46 || b == 32)
               )
            {
                res = true;
            }
            return res;

        }

        public const byte SPACE = 0x20;
        //----------------------
        public static string bytesToString(byte[] ktmp, int len)
        {
           
            int lt = (ktmp.Length >= len ? len : ktmp.Length);
            byte[] m_temp = new byte[lt];
            Array.Clear(m_temp, 0, lt);
            Array.Copy(ktmp, m_temp, lt);

            for (int i = 0; i < m_temp.Length;i++)
            {
                if (!isASCII(m_temp[i]))
                    m_temp[i] = SPACE;
                                
            }
            return System.Text.Encoding.Default.GetString(m_temp);//auto for different OS


        }
        /// <summary>
        /// decode to bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] stringToBytes(string str, int maxlen)
        {
            string tmp = str;
            if (str.Length > maxlen)
                tmp = str.Substring(0, maxlen);
            return System.Text.ASCIIEncoding.Default.GetBytes(tmp);

        }


        /// <summary>
        /// registry for search
        /// </summary>
        /// <returns></returns>
        public static bool Get45or451FromRegistry()
        {
            bool res = false;
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    Console.WriteLine("Version: " + CheckFor45DotVersion((int)ndpKey.GetValue("Release")));
                    res = true;
                }
                else
                {
                    Console.WriteLine("Version 4.5 or later is not detected.");
                    res = false;
                }
            }

            return res;

        }

        // Checking the version using >= will enable forward compatibility, 
        // however you should always compile your code on newer versions of
        // the framework to ensure your app works the same.
        private static string CheckFor45DotVersion(int releaseKey)
        {
            if (releaseKey >= 393295)
            {
                return "4.6 or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            // This line should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }


    }
}
