using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;//

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.commom
 *文件名：  CUlitity
 *版本号：  V1.0.0.0
 *唯一标识：5fcb6885-57e7-41c6-98f8-97c2cbe27929
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/10/2016 11:15:04 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/10/2016 11:15:04 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class CUlitity
    {
        public const int LINE_I = 0x1000;
        public const int LINE_II = 0x2000;
        public const int LINE_III = 0x3000;
        public const int LINE_IV = 0x4000;

        public static void sortDataGridAscending(DataGrid grid, int colindex)
        {
            ListSortDirection d = ListSortDirection.Ascending; //using System.ComponentModel;
            ICollectionView v = CollectionViewSource.GetDefaultView(grid.DataContext);
            //判断并设置方向
            //if (oldIndex == combobox.SelectedIndex && v.SortDescriptions.Count > 0/*第一次该值长度为0*/)
            //    d = v.SortDescriptions[0].Direction == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            //else
            //    d = ListSortDirection.Ascending;
            if (v != null)
            {
                v.SortDescriptions.Clear();
                v.SortDescriptions.Add(new SortDescription(grid.Columns[colindex].Header.ToString(), d));
                v.Refresh();
                grid.ColumnFromDisplayIndex(colindex).SortDirection = d;
            }
        }

        /// <summary>
        /// 不涉及到四舍五入，只截成2位表示
        /// </summary>
        /// <param name="mvalue"></param>
        /// <param name="dig"></param>
        /// <returns></returns>
        public static double fRound(double mvalue, int dig)
        {
            double fbase = Math.Pow(10, dig);
            int itmp = (int)(mvalue * fbase);
            double id = (double)itmp / fbase;
            return id;
        }



        public static int lineindexOfDevID(int mdevid)
        {
            int index = 0;
            if (mdevid >= LINE_I && mdevid < LINE_II)
                index = 0;
            else if (mdevid >= LINE_II && mdevid < LINE_III)
                index = 1;
            else if (mdevid >= LINE_III && mdevid < LINE_IV)
                index = 2;
            else if (mdevid >= LINE_IV)
                index = 3;
            return index;
        }

        public static int limitFreq(int freq)
        {
            int tmpf = 0;
            const int freqMin = 0;
            const int freqMax = 300;
            tmpf = Math.Max(freqMin, Math.Min(freqMax, freq));
            return tmpf;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sch">just for is searched substring</param>
        /// <param name="strSrc">source string </param>
        /// <returns></returns>
        public static string rightStr(string sch, string strSrc)
        {
            string res = "";
            int t = strSrc.IndexOf(sch);
            if (t >= 0)
            {
                res = strSrc.Substring(t + 1, strSrc.Length - t - 1);
            }
            return res;
        }

        public static string defaultChName(int chindex)
        {
            string strRes = "";
            if (chindex > 8)
                strRes = string.Format("CH{0}", chindex + 1);
            else
                strRes = string.Format("CH0{0}", chindex + 1);
            return strRes;

        }

        public static int limitValue(int mvalue, int maxm, int minx = 0)
        {

            return Math.Max(minx, Math.Min(maxm, mvalue)); //very good
        }

        public static int convertByteToInt(byte[] mData)
        {
            int x = 0;
            for (int i = 11; i >= 0; i--)
            {
                x = (x | (mData[i] << i));
            }
            return x;
        }


        public static byte convertByte12ToBHigh4(byte[] mData)
        {
            byte x = 0;
            for (int i = 11; i >= 8; i--)
            {
                x = (byte)(x | (mData[i] << (i - 8)));
            }
            return x;
        }

        public static int catsWithByteHL(byte bH4, byte bL8)
        {
            int x = 0;
            x = ((bH4 << 8) | bL8);
            return x;
        }

        public static byte[] catsByteHLToByteAry12(byte bH, byte bL)
        {
            int tmp = catsWithByteHL(bH, bL);
            byte[] mData = convertIntToByte(tmp);
            return mData;
        }


        public static byte convertByte12ToBLow8(byte[] mData)
        {
            byte x = 0;
            for (int i = 7; i >= 0; i--)
            {
                x = (byte)(x | (mData[i] << i));
            }
            return x;
        }




        public static byte convertBytesToBLow8(byte[] mData)
        {
            int tmp = convertByteToInt(mData);
            byte bLow = (byte)(tmp & 0xFF);
            return bLow;
        }

        public static byte convertBytesToBHigh4(byte[] mData)
        {
            int tmp = convertByteToInt(mData);
            int ftmp = (tmp & 0xFFF);
            byte bHigh = (byte)((ftmp >> 8) & 0x0f);
            return bHigh;
        }

        public static byte[] convertIntToByte(int fx)
        {
            fx = (fx & 0xfff);
            byte[] tmp = new Byte[12];

            for (int i = 1; i < 13; i++)
            {
                tmp[i - 1] = (byte)(fx & 0x01);
                fx = (fx >> 1);
            }
            return tmp;

        }



    }
}
