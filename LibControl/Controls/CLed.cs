using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using CommLibrary;
using System.Windows.Threading;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：HelloMove
 *文件名：  CLed
 *版本号：  V1.0.0.0
 *唯一标识：807d453a-96f4-4a67-82cd-815c224de77a
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/30/2016 9:49:10 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/30/2016 9:49:10 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public enum LED_Status
    {
        LED_Normal = 0,
        LED_Red = 1,
        LED_Yellow = 2,
        LED_Green = 3
    };

    public class CLed : CommControl
    {
        private DispatcherTimer blinkApparatus = new DispatcherTimer();

        public CLed()
        {
            if (blinkApparatus == null)
                blinkApparatus = new DispatcherTimer();


            blinkApparatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            blinkApparatus.Tick += blinkApparatus_Tick;
            blinkApparatus.Stop();
            //  Debug.WriteLine("cled construct now....");
            string ko = "";


        }

        private const bool DefaultBlinking = false;
        public bool isBlinking
        {
            get { return (bool)GetValue(isBlinkingProperty); }
            set { SetValue(isBlinkingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isBlinking.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isBlinkingProperty =
            DependencyProperty.Register("isBlinking", typeof(bool), typeof(CLed), new PropertyMetadata(DefaultBlinking));


        private SolidColorBrush tempBrush = Brushes.Gray;


        private void blinkApparatus_Tick(object sender, EventArgs e)
        {
            //  this.Dispatcher.BeginInvoke(new Action(() =>
            //   {
            if (useBlink)
            {
                isBlinking = true;
                counter++;
                if (counter % 2 == 0)
                {
                    BackBrush = NormalBrush;
                }
                else
                {
                    BackBrush = tempBrush;
                }

            }
            //}));

        }

        /// <summary>
        /// on blink event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        public void setBlinkInterval(int smiliTime)
        {
            blinkApparatus.Interval = new TimeSpan(0, 0, 0, 0, smiliTime);
        }

        private int counter = 0;

        public void stopBlink()
        {
            BackBrush = NormalBrush;
            blinkApparatus.Stop();
            counter = 0;
            isBlinking = false;
        }

        public void startRedBlink()
        {
            tempBrush =BackBrush = new SolidColorBrush(Color.FromRgb(255,0,0));
            startBlink();
        }

        public void startGreenBlink()
        {
            tempBrush = BackBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            startBlink();
        }

        public void startBlink()
        {
            //  tempBrush = BackBrush;
            blinkApparatus.Start();
            counter = 0;
            isBlinking = true;
        }

        private const bool DefaultBlink = false;

        public bool useBlink
        {
            get { return (bool)GetValue(useBlinkProperty); }
            set { SetValue(useBlinkProperty, value); }
        }
        // Using a DependencyProperty as the backing store for useBlink.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty useBlinkProperty =
            DependencyProperty.Register("useBlink", typeof(bool), typeof(CLed), new PropertyMetadata(DefaultBlink));



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>




        private const LED_Status DefaultLedStatus = LED_Status.LED_Normal;

        private static SolidColorBrush DefaultNormalBrush = Brushes.Gray;
        public SolidColorBrush NormalBrush
        {
            get { return (SolidColorBrush)GetValue(NormalBrushProperty); }
            set { SetValue(NormalBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.Register("NormalBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultNormalBrush));


        private static SolidColorBrush DefaultRedBrush = Brushes.Red;
        public SolidColorBrush RedBrush
        {
            get { return (SolidColorBrush)GetValue(RedBrushProperty); }
            set { SetValue(RedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RedBrushProperty =
            DependencyProperty.Register("RedBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultRedBrush));




        private static SolidColorBrush DefaultBlinkBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        public SolidColorBrush BlinkBrush
        {
            get { return (SolidColorBrush)GetValue(BlinkBrushProperty); }
            set { SetValue(BlinkBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlinkBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlinkBrushProperty =
            DependencyProperty.Register("BlinkBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultBlinkBrush));




        private static SolidColorBrush DefaultYelloBrush = Brushes.Yellow;
        public SolidColorBrush YellowBrush
        {
            get { return (SolidColorBrush)GetValue(YellowBrushProperty); }
            set { SetValue(YellowBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YellowBrushProperty =
            DependencyProperty.Register("YellowBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultYelloBrush));


        private static SolidColorBrush DefaultGreenBrush = new SolidColorBrush(Color.FromRgb(0, 0xff, 0));
        public SolidColorBrush GreenBrush
        {
            get { return (SolidColorBrush)GetValue(GreenBrushProperty); }
            set { SetValue(GreenBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GreenBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GreenBrushProperty =
            DependencyProperty.Register("GreenBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultGreenBrush));


        public LED_Status LedStatus
        {
            get { return (LED_Status)GetValue(LedStatusProperty); }
            set { SetValue(LedStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LedStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LedStatusProperty =
            DependencyProperty.Register("LedStatus", typeof(LED_Status), typeof(CLed),
         new FrameworkPropertyMetadata(DefaultLedStatus, FrameworkPropertyMetadataOptions.None, onLedStatusChange
                ));


        private static SolidColorBrush DefaultPenBrush = Brushes.Black;
        public SolidColorBrush penBrush
        {
            get { return (SolidColorBrush)GetValue(penBrushProperty); }
            set { SetValue(penBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for penBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty penBrushProperty =
            DependencyProperty.Register("penBrush", typeof(SolidColorBrush), typeof(CLed), new PropertyMetadata(DefaultPenBrush));


        private const double DefaultBorderThick = 0.5;
        public double borderThick
        {
            get { return (double)GetValue(borderThickProperty); }
            set { SetValue(borderThickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for borderThick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty borderThickProperty =
            DependencyProperty.Register("borderThick", typeof(double), typeof(CLed),
              new FrameworkPropertyMetadata(DefaultBorderThick, FrameworkPropertyMetadataOptions.None, onBorderThickChange
            ));

        private static void onBorderThickChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLed;
            if (mcontrol != null)
            {

                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.borderThick = (double)args.NewValue;
                mcontrol.Redraw();
            }

        }

        private static void onLedStatusChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLed;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.LedStatus = (LED_Status)args.NewValue;
                mcontrol.UpdateLedStatus();
            }

        }


        public void UpdateLedStatus()
        {
            LED_Status lsts = LedStatus;
            switch (lsts)
            {
                case LED_Status.LED_Normal:
                    BackBrush = NormalBrush;
                    break;
                case LED_Status.LED_Green:
                    BackBrush = GreenBrush;
                    break;
                case LED_Status.LED_Red:
                    BackBrush = RedBrush;
                    break;
                case LED_Status.LED_Yellow:
                    BackBrush = YellowBrush;
                    break;
            }
            Redraw();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double wd = ActualWidth;
            double ht = ActualHeight;
            dc.DrawRectangle(BackBrush, new Pen(penBrush, borderThick), new Rect(0, 0, wd, ht));

        }


    }
}
