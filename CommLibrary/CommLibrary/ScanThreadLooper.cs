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
 *命名空间：MatrixUpdate
 *文件名：  ScanThread
 *版本号：  V1.0.0.0
 *唯一标识：40af76b9-9cd2-49c5-8a4f-1e87eaf74e61
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：8/27/2016 11:36:37 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：8/27/2016 11:36:37 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace CommLibrary
{
    public class ScanThreadOnce
    {      
        bool _threadSwitch = false; //线程启动开关
        bool _pauseSwitch = false;//暂停开关
        AutoResetEvent _resetEvent = new AutoResetEvent(false); //自动重设事件
        Thread _worker = null;
        object _locker = new object();

        public int ErrorCounter;
 
        public delegate void  threadDosomething(object sender, EventArgs e);
 
        public event threadDosomething DoSomethingEvent;
 
        private void OnDoSomethingEvent()
        {
            try
            {
                if (DoSomethingEvent != null)
                {
                    DoSomethingEvent(this, new EventArgs());
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }
        public void Start()
        {
            lock(_locker)
            {
                ErrorCounter = 0;
                if (_worker == null)
                {
                    _threadSwitch = true;
                    _worker = new Thread(Run);
                    _worker.IsBackground = true;
                    _worker.Start();
                }
            }
 
        }
 
        void Run()
        {
            while (_threadSwitch)
            {
                try
                {                    
                    if (_pauseSwitch)
                    {
                        _resetEvent.WaitOne();
                    }
                    OnDoSomethingEvent();
                }
                catch (System.Exception ex)
                {

                    Debug.WriteLine(ex.Message); //MessageBox.Show(ex.Message);
                }
                Thread.Sleep(1);
            }
        }
 
        public void Continue()
        {
            lock (_locker)
            {
                if (!_pauseSwitch)
                {
                    return;
                }
                _pauseSwitch = false;
                _resetEvent.Set();
            }
        }
 
        public void Pause()
        {
            lock (_locker)
            {
                if (_pauseSwitch)
                {
                    return;
                }
                _pauseSwitch = true;
            }
        }
 
        public void Stop()
        {
            lock (_locker)
            {
                if (_worker == null)
                {
                    return;
                }
                _threadSwitch = false;
                _pauseSwitch = false;
                _resetEvent.Set();
                if (_worker.IsAlive)
                {
                    try
                    {
                        _worker.Abort();
                    }
                    catch(Exception ex)
                    {
                        ErrorCounter++;
                        Debug.WriteLine("scan number is  {0}", ErrorCounter);
  
                    }                  
                }
                _worker = null;
            }
        }    


    }
}
