using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：CommLibrary
 *文件名：  CorprationInfo
 *版本号：  V1.0.0.0
 *唯一标识：c5334f20-ab6b-4f14-ae7c-23e028359b92
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：12/29/2016 5:07:00 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：12/29/2016 5:07:00 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class CorprationInfo
    {

        //string ProductName=string.Empty; //such Matrix
        //string ProductVer; //such as v0.1.1
        //string corpENName; //such ass Seikaku Technical Group
        //string corpWebSite; //such as http://www.seikaku.hk
        public string ProductName { get; set; }
        public string ProductVer { get; set; }
        public string corpENName { get; set; }
        public string corpWebSite { get; set; }

        public CorprationInfo() //initial 
        {
            ProductName = "Editor system";
            ProductVer = "V0.1";
            corpENName = "Seikaku Technical Group";
            corpWebSite = "http://www.seikaku.hk";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getProcesTitle()
        {
            return string.Format("{0} {1}", ProductName, ProductVer);

        }

    }

}
