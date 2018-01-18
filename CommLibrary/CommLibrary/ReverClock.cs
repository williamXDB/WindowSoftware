using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：ReverseCount
 *文件名：  ReverClock
 *版本号：  V1.0.0.0
 *唯一标识：48bd31cb-682f-45b0-8f5d-e490227ea0d4
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/12/2016 1:44:11 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/12/2016 1:44:11 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 * 注意在dll中虽然不能直接引用DispatchTimer之类的组件，但是可以在msdn中找到该类所属的dll库
 * 然后添加这个dll库引用（找到dll库，一般地址在
 * C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework 选择相应的版本目录下找到windowsbase.dll
 * 即可
/************************************************************************************/

namespace CommLibrary
{
   public class ReverClock
    {
        public delegate void clockCountDown(int cDown, EventArgs e);
        public event clockCountDown countDownEvent;


        public delegate void clockOver(EventArgs e);
        public event clockOver clockOverEvent;



        public int reverseCount
        {
            get;
            set;
        }
       
        public int GameTime { get; set; }
        private static DispatcherTimer disTimer = new DispatcherTimer();

        public ReverClock(int delayT)
        {

            disTimer.Interval = new TimeSpan(0, 0, 0, 1);
            disTimer.Tick += new EventHandler(disTimer_Tick);
            disTimer.Stop();
            GameTime = reverseCount = delayT;

        }

        void disTimer_Tick(object sender, EventArgs e)
        {
            if (reverseCount >= 0)
            {

                // Debug.WriteLine("game over........");

                //判断lblSecond是否处于UI线程上
                if (countDownEvent != null)
                {
                    countDownEvent(reverseCount, e);

                }
                --reverseCount;

            }
            else
            {

                if (clockOverEvent != null)
                {
                    clockOverEvent(e);
                }
                disTimer.Stop();
                Debug.WriteLine("game over........");
            }

        }

        public void startClock()
        {
            reverseCount = GameTime;
            disTimer.Start();
        }
        public void StopClock()
        {
            disTimer.Stop();
        }

    }
}
