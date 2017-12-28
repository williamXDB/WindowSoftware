using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：Lib.Controls
 *文件名：  CMatrixFinal
 *版本号：  V1.0.0.0
 *唯一标识：6bc86c19-d1fe-45b1-bdf5-dc4648396de4
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/16/2016 9:38:05 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/16/2016 9:38:05 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class CMatrixFinal
    {
        public const int Max_MatrixChanelNum = 12;
        public const byte BI = 0;
        public const byte BII = 1;
        public const int NormalEQMax = 10;


        /// <summary>
        /// 
        /// </summary>
        public static string[] strSensitivity = 
        { 
           "0dB","-12dB","-24dB","-36dB","-48dB",
        };

    }
}
