using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：CommLibrary
 *文件名：  ListenerApparatus
 *版本号：  V1.0.0.0
 *唯一标识：a38358e6-78de-43be-9ecd-addcc8a3935c
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：12/9/2016 2:30:29 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：12/9/2016 2:30:29 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace CommLibrary
{
    public class ListenerApparatus
    {
        //

        public delegate void listenDoEvent(object sender, EventArgs e);
        public event listenDoEvent onListenDoEvent;

        public bool hasStopListening { get; set; }
        /// <summary>
        /// udpSocket begin.........................................
        /// </summary>
        ///


        private ScanThreader thread_apparatus; //

        public ListenerApparatus(int milliDelay=5000)
        {                     
                DelayMills = milliDelay;
                hasStopListening = true;
                thread_apparatus = new ScanThreader();
                //task to do in future          
                thread_apparatus.DoSomethingEvent += new ScanThreader.threadDosomething(onlistennerEvent);
                isBeginCheck = false;
                caculateCounter = 0;

        }

        public bool isBeginCheck= false;
        public int DelayMills
        {
            get;
            set;
        }

        private int caculateCounter = 0;

        private void onlistennerEvent(object sender, EventArgs e)
        {
            if (onListenDoEvent != null && caculateCounter>0) //first time not do
            {
                
                onListenDoEvent(this, e);
                Debug.WriteLine("listenning check now.......");
            }
            caculateCounter++;
            Thread.Sleep(DelayMills);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("timer_tick gogogog1");
            //  stopListen();

        }
        public void stopListen()
        {
            if (!hasStopListening)
            {
                Debug.WriteLine("close socket and stopscan....................");
                caculateCounter = 0;
                thread_apparatus.Stop();


                hasStopListening = true;
                //  timer.Stop();             

                Thread.Sleep(100);

            }
            else
            {

                // Debug.WriteLine("stopscan is false so no need to close xxxx....................");
            }

        }


        //-------------------------------

        //-------------------------

        //-------------------------------------------------------------------------------------------------
        public void startListen()
        {
            caculateCounter = 0;
            hasStopListening = false;
            thread_apparatus.Start();  //        
            //  timer.Start();                 

        }

        //---------------------------         

        public void clear()  //when it close 
        {
            caculateCounter = 0;
            thread_apparatus.Stop();
            hasStopListening = true;
           
        }



    }



}
