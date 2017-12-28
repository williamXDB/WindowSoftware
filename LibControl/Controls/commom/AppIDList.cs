﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：ALP50SA
 *文件名：  AppIDList
 *版本号：  V1.0.0.0
 *唯一标识：78b2ade2-5a45-404f-be95-64583de53d9f
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/22/2016 9:47:08 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/22/2016 9:47:08 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{


    public enum ModuleRVAS
    {
        MRVA=0,MRVC,MRIO,RPM, //RPM100/200        
    };

   public class AppIDList
    {

        public const int AP_T20 = 1;
        public const int AP_DLM_A88 = 2;
        public const int AP_CLV = 4;//cl-4

        public const int AP_Matrix_A8 = 6;
        public const int AP_Matrix_D8 = 7;

        public const int AP_RPM_100 = 8;//rpm200
        public const int AP_RVC_100 = 9; //rvc1000
        public const int AP_RVA_100 = 10;//RVA200,RVC200
        public const int AP_RIO_100 = 11;//rio200
        public const int AP_DCS_100 = 16;   

        public const int AP_Router = 20;
        public const int AP_ALP50SA = 22;
        

    }
}
