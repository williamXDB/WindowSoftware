using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor
 *文件名：  CommuteStatus
 *版本号：  V1.0.0.0
 *唯一标识：ae864e70-dc83-46d9-ba52-09b45a647624
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：1/3/2017 1:55:23 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：1/3/2017 1:55:23 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace MatrixSystemEditor
{
    public class CommunicateStatus
    {
        public CommunicateStatus()
        {
            commuteStatus = ACK_Status.M_Disconnected;
             responseAckCounter = 1;
        }
        public int commuteStatus = ACK_Status.M_Disconnected;
        public int responseAckCounter = 1;
    }
}
