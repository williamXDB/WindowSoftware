using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：SaveOpenDialog
 *文件名：  IOBinaryOperation
 *版本号：  V1.0.0.0
 *唯一标识：e8fa87f2-841a-42ec-848d-7a92dddb68fb
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：8/24/2016 3:11:42 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：8/24/2016 3:11:42 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class IOBinaryOperation
    {


        public const int FILE_MAX_LEN = 1024;
        public static int calcDataSegments_fromFileLen(int fileLen) 
        {
            int res = 0;
            int ch = fileLen / FILE_MAX_LEN;
            int cl = fileLen % FILE_MAX_LEN;

            if (cl == 0)
                res = ch;
            else
            {
                res = (ch + 1);
            }

            return res;

        }

        public static long fileLength(string filePath)
        {
            long mlen = 0;

            if(File.Exists(filePath))
            {

                FileInfo mfile = new FileInfo(filePath);
                mlen = mfile.Length;
            }
            else
            {
                return 0;
            }


            return mlen;
        }

        //----------------------------------------
        /// <summary>
        /// readBinar from file 
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="dest"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] readBinaryFile(string strPath, int len)
        {
            byte[] dest = new byte[len];


            if (string.IsNullOrEmpty(strPath.Trim()))
            {

                return dest;
            }

            if (!File.Exists(strPath))   //Directory.Exists(判断目录是否存在）
            {
                Console.WriteLine("file not exist");
                return dest;
            }

            try
            {
                FileStream mystream = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                BinaryReader myReader = new BinaryReader(mystream);
                dest = myReader.ReadBytes(len);
              //  string sk = BitConverter.ToString(dest);
               // Console.WriteLine("read binary : {0}", sk);
                myReader.Close();
                mystream.Close();
            }

            catch
            {
            }

            return dest;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static bool writeBinaryToFile(string strPath, byte[] wdata)
        {
            bool res = false;
            if (String.IsNullOrEmpty(strPath.Trim()))
            {

                System.Windows.MessageBox.Show("Please choose the file path");
                return res;
            }

            if (wdata == null || wdata.Length < 1)
            {
                System.Windows.MessageBox.Show("Empty data");
                return res;
            }

            try
            {
                FileStream myStream = new FileStream(strPath, FileMode.Create);
                BinaryWriter myWriter = new BinaryWriter(myStream);

                myWriter.Write(wdata);
                myWriter.Close();
                myStream.Close();
                res = true;
               // System.Windows.MessageBox.Show("Successful to write to file！");
            }
            catch (Exception ex)
            {
                res = false;
              //  System.Windows.MessageBox.Show(ex.Message);
            }
            return res;
        }


    }
}
