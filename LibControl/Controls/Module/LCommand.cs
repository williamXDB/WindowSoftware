using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixUpdate
 *文件名：  LCommand
 *版本号：  V1.0.0.0
 *唯一标识：852961b3-ed58-4d8f-9c96-ad5c5163c1a6
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/1/2016 3:30:35 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/1/2016 3:30:35 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class LCommand
    {
        public const byte UTRAL_H0 = 0x01;
        public const byte UTRAL_H1 = 0x20;
        public const byte UTRAL_H2 = 0x03;
        public const byte UTRAL_End = 0x40;

        private int packLen;
        private int _cmdType;
        public int packMinLen = 14;

        public int ID_Machine
        {
            get;
            set;
        }
        public int Device_ID
        {
            get;
            set;
        }

        public LCommand(int length, int cmdType)
        {

            packLen = length;
            _cmdType = cmdType;
        }

        /// <summary>
        /// getpackage with added data:aData
        /// </summary>
        /// <param name="aData"></param>
        /// <returns></returns>
        public byte[] getPackage_withDataArray(byte[] aData)
        {

            byte[] package = new byte[packLen];
            Array.Clear(package, 0, packLen);
            if (aData == null)
            {
                Debug.WriteLine("package data added is error......");
                return package;
            }

            package[0] = UTRAL_H0;
            package[1] = UTRAL_H1;
            package[2] = UTRAL_H2;
            //
            package[3] = (byte)(packLen / 256);
            package[4] = (byte)(packLen % 256);

            package[5] = (byte)(ID_Machine / 256);
            package[6] = (byte)(ID_Machine % 256);

            package[7] = (byte)(Device_ID / 256);
            package[8] = (byte)(Device_ID % 256);

            package[9] = (byte)(_cmdType / 256);
            package[10] = (byte)(_cmdType % 256);
            //
            Array.Copy(aData, 0, package, 11, aData.Length);

            package[packLen - 1] = UTRAL_End;

            byte checkSum = package[0];
            for (int i = 1; i < packLen; i++)
            {
                if (i != packLen - 2)
                    checkSum ^= package[i];

            }
            package[packLen - 2] = checkSum;
            return package;

        }

        /// <summary>
        /// getpackage with added data:aData,clen is the segment index
        /// </summary>
        /// <param name="aData"></param>
        /// <returns></returns>
        public byte[] getPackage_withDataArray(byte[] aData, int clen)
        {

            byte[] package = new byte[packLen];
            Array.Clear(package, 0, packLen);
            if (aData == null)
            {
                Debug.WriteLine("package data added is error......");
                return package;
            }
            package[0] = UTRAL_H0;
            package[1] = UTRAL_H1;
            package[2] = UTRAL_H2;
            //
            package[3] = (byte)(packLen / 256);
            package[4] = (byte)(packLen % 256);

            package[5] = (byte)(ID_Machine / 256);
            package[6] = (byte)(ID_Machine % 256);

            package[7] = (byte)(Device_ID / 256);
            package[8] = (byte)(Device_ID % 256);

            package[9] = (byte)(_cmdType / 256);
            package[10] = (byte)(_cmdType % 256);

            package[11] = (byte)(clen / 100);
            package[12] = (byte)((clen) % 100);
            //
            Array.Copy(aData, 0, package, 13, aData.Length);

            //len-3,len-4 is reserved

            byte checkSum = package[0];
            for (int i = 1; i < packLen; i++)
            {
                checkSum ^= package[i];

            }

            package[packLen - 1] = UTRAL_End;
            for (int i = 1; i < packLen; i++)
            {
                if (i != packLen - 2)
                    checkSum ^= package[i];

            }
            package[packLen - 2] = checkSum;
            return package;

        }

        public byte[] getPackage_withoutDataArray(int item)
        {

            byte[] package = new byte[packLen];
            Array.Clear(package, 0, packLen);

            package[0] = UTRAL_H0;
            package[1] = UTRAL_H1;
            package[2] = UTRAL_H2;
            //
            package[3] = (byte)(packLen / 256);
            package[4] = (byte)(packLen % 256);

            package[5] = (byte)(ID_Machine / 256);
            package[6] = (byte)(ID_Machine % 256);

            package[7] = (byte)(Device_ID / 256);
            package[8] = (byte)(Device_ID % 256);

            package[9] = (byte)(_cmdType / 256);
            package[10] = (byte)(_cmdType % 256);
            package[11] = (byte)item;

            //            
            byte checkSum = package[0];

            package[packLen - 1] = UTRAL_End;

            for (int i = 1; i < packLen; i++)
            {
                if (i != packLen - 2)
                    checkSum ^= package[i];

            }
            package[packLen - 2] = checkSum;
            return package;

        }

        public byte[] getPackage_withoutDataArray()
        {

            byte[] package = new byte[packLen];
            Array.Clear(package, 0, packLen);

            package[0] = UTRAL_H0;
            package[1] = UTRAL_H1;
            package[2] = UTRAL_H2;
            //
            package[3] = (byte)(packLen / 256);
            package[4] = (byte)(packLen % 256);

            package[5] = (byte)(ID_Machine / 256);
            package[6] = (byte)(ID_Machine % 256);

            package[7] = (byte)(Device_ID / 256);
            package[8] = (byte)(Device_ID % 256);

            package[9] = (byte)(_cmdType / 256);
            package[10] = (byte)(_cmdType % 256);
            //            
            byte checkSum = package[0];

            package[packLen - 1] = UTRAL_End;

            for (int i = 1; i < packLen; i++)
            {
                if (i != packLen - 2)
                    checkSum ^= package[i];

            }
            package[packLen - 2] = checkSum;
            return package;

        }

    }
}
