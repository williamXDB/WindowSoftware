using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Globalization;



/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：DrawDemo
 *文件名：  DrawClass
 *版本号：  V1.0.0.0
 *唯一标识：29f6e020-b88a-4994-97fb-a7287a658814
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/24/2016 4:29:57 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/24/2016 4:29:57 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class CMeter : CommControl
    {

        public static int DefaultMaximum = 16;
        public static double DefaultSegGap = 4.0;
        public static SolidColorBrush DefaultBrush = Brushes.Gray;
        public static SolidColorBrush DefaultLimitBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        public static SolidColorBrush DefaultLightBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        public static SolidColorBrush DefaultClipBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        public static SolidColorBrush DefaultBackBrush = Brushes.Black;

        #region about DependencyProperty define.................

        public CMeter()
        {

        }
        private static double DefaultDistanceGap = 2.0;
        public double topDistanceGap
        {
            get { return (double)GetValue(topDistanceGapProperty); }
            set { SetValue(topDistanceGapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for topDistanceGap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty topDistanceGapProperty =
            DependencyProperty.Register("topDistanceGap", typeof(double), typeof(CMeter), new PropertyMetadata(DefaultDistanceGap));




        public SolidColorBrush BackBroundBrush
        {
            get { return (SolidColorBrush)GetValue(BackBroundBrushProperty); }
            set { SetValue(BackBroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackBroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackBroundBrushProperty =
            DependencyProperty.Register("BackBroundBrush", typeof(SolidColorBrush),
            typeof(CMeter), new PropertyMetadata(DefaultBackBrush));

        public SolidColorBrush ClipBrush
        {
            get { return (SolidColorBrush)GetValue(ClipBrushProperty); }
            set { SetValue(ClipBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClipBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClipBrushProperty =
            DependencyProperty.Register("ClipBrush", typeof(SolidColorBrush), typeof(CMeter), new PropertyMetadata(DefaultClipBrush));



        public SolidColorBrush LimitBrush
        {
            get { return (SolidColorBrush)GetValue(LimitBrushProperty); }
            set { SetValue(LimitBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitBrushProperty =
            DependencyProperty.Register("LimitBrush", typeof(SolidColorBrush), typeof(CMeter), new PropertyMetadata(DefaultLimitBrush));


        public SolidColorBrush LightNormalBrush
        {
            get { return (SolidColorBrush)GetValue(LightNormalBrushProperty); }
            set { SetValue(LightNormalBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LightNormalBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightNormalBrushProperty =
            DependencyProperty.Register("LightNormalBrush", typeof(SolidColorBrush), typeof(CMeter), new PropertyMetadata(DefaultLightBrush));

        public SolidColorBrush UnlightBrush
        {
            get { return (SolidColorBrush)GetValue(UnlightBrushProperty); }
            set { SetValue(UnlightBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnlightBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnlightBrushProperty =
            DependencyProperty.Register("UnlightBrush", typeof(SolidColorBrush), typeof(CMeter), new PropertyMetadata(DefaultBrush));




        public static int DefaultLimitSegNum = 3;
        public static int DefaultClipSegNum = 2;

        public int LimitSegNum
        {
            get { return (int)GetValue(LimitSegNumProperty); }
            set { SetValue(LimitSegNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitSegNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitSegNumProperty =
            DependencyProperty.Register("LimitSegNum", typeof(int), typeof(CMeter), new PropertyMetadata(DefaultLimitSegNum));



        public int ClipSegNum
        {
            get { return (int)GetValue(ClipSegNumProperty); }
            set { SetValue(ClipSegNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClipSegNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClipSegNumProperty =
            DependencyProperty.Register("ClipSegNum", typeof(int), typeof(CMeter), new PropertyMetadata(DefaultClipSegNum));




        public double segGap
        {
            get { return (double)GetValue(segGapProperty); }
            set { SetValue(segGapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for segGap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty segGapProperty =
            DependencyProperty.Register("segGap", typeof(double), typeof(CMeter), new PropertyMetadata(DefaultSegGap));



        public int Maximum
        {
            get { return (int)GetValue(MaxminumProperty); }
            set { SetValue(MaxminumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxminum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxminumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(CMeter), new PropertyMetadata(DefaultMaximum));



        public int Minimum
        {
            get { return (int)GetValue(MinminumProperty); }
            set { SetValue(MinminumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minminum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinminumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(CMeter), new PropertyMetadata(0));


        private static SolidColorBrush DefaultLbBrush = Brushes.Black;


        public SolidColorBrush MarkBrush
        {
            get { return (SolidColorBrush)GetValue(MarkBrushProperty); }
            set { SetValue(MarkBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkBrushProperty =
            DependencyProperty.Register("MarkBrush", typeof(SolidColorBrush), typeof(CMeter),
         new FrameworkPropertyMetadata(DefaultLbBrush, FrameworkPropertyMetadataOptions.None, onMarkBrushChange
                ));

        private static void onMarkBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CMeter meter = obj as CMeter;
            if (meter != null)
            {
                meter.MarkBrush = (SolidColorBrush)args.NewValue;
                meter.redraw();
            }

        }


        private static int DefaultPosValue = 0;
        public int posvalue
        {
            get { return (int)GetValue(posvalueProperty); }
            set { SetValue(posvalueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for posvalue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty posvalueProperty =
            DependencyProperty.Register("posvalue", typeof(int), typeof(CMeter),
            new FrameworkPropertyMetadata(DefaultPosValue, FrameworkPropertyMetadataOptions.None, onPosValueChange
                ));


        private static void onPosValueChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CMeter meter = obj as CMeter;
            if (meter != null)
            {
                meter.posvalue = (int)args.NewValue;
                meter.redraw();
            }

        }


        private static bool DeafultLoopRevse = false;
        public bool isLoopReverse
        {
            get { return (bool)GetValue(isLoopReverseProperty); }
            set { SetValue(isLoopReverseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isLoopReverse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isLoopReverseProperty =
            DependencyProperty.Register("isLoopReverse", typeof(bool), typeof(CMeter),
            new FrameworkPropertyMetadata(DeafultLoopRevse, FrameworkPropertyMetadataOptions.None, onLoopReverse
                ));
        // new PropertyMetadata(DeafultLoopRevse));


        private static void onLoopReverse(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CMeter meter = obj as CMeter;
            if (meter != null)
            {
                meter.isLoopReverse = (bool)args.NewValue;
                meter.redraw();
            }

        }

        #endregion

        static CMeter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CMeter), new FrameworkPropertyMetadata(typeof(CMeter)));
        }




        private const bool DefaultMarked = false;
        public bool isEnableMark
        {
            get { return (bool)GetValue(isEnableMarkProperty); }
            set { SetValue(isEnableMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isEnableMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isEnableMarkProperty =
            DependencyProperty.Register("isEnableMark", typeof(bool), typeof(CMeter), new PropertyMetadata(DefaultMarked));
       
        //draw 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        ///
        public string[] meterStr = 
         { 
           "clip",
           "+14",
           "+10",
           " +6",
           " +2",
           "  0",
           " -2",
           " -4",
           " -5",
           "-15",
           "-25",
           "-40"          
         };

        private double fontSize = 9;
        private double markXoffset = -19;
        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;
        private void drawMark(DrawingContext dc)
        {
            double ht = this.ActualHeight;

            if (isEnableMark)
            {
                FormattedText text = null;
                double heightPerseg = (double)ht / Maximum - segGap;
                double FontH = 12;
                double ty = 0;
                int len = meterStr.Length;
                for (int i = 0; i < len; i++)
                {
                    ty = topDistanceGap + (double)ht / Maximum *2* i + (heightPerseg - FontH) / 2;

                    text = new FormattedText(meterStr[i], CIR, FlowDirection.LeftToRight,
                                           new Typeface("Verdana"), fontSize, MarkBrush);

                    dc.DrawText(text, new Point(markXoffset, ty));
                }
            }

        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double wd = ActualWidth;
            double ht = ActualHeight;
            dc.DrawRectangle(BackBroundBrush, new Pen(), new Rect(0, 0, wd, ht));
            //Pen pen = new Pen(Brushes.Red, 1);
            //dc.DrawLine(pen, new Point(15, 80), new Point(80, 150));
            // this.UpdateLayout() relayout       


           // Debug.WriteLine("draw now.....with ...width height is :{0} {1} ", wd, ht);

            double heightPerseg = (double)ht / Maximum - segGap;

            drawMark(dc);

            for (int i = 0; i < Maximum; i++)
            {
                Rect segRect = new Rect(1, topDistanceGap + (heightPerseg + segGap) * i, wd - 2, heightPerseg);
                dc.DrawRectangle(UnlightBrush, new Pen(), segRect);

            }
            if (isLoopReverse) //from up to down
            {

                for (int i = 0; i < posvalue; i++)  //first draw normal lighted led
                {

                    Rect segRect = new Rect(1, topDistanceGap + (heightPerseg + segGap) * i, wd - 2, heightPerseg);
                    dc.DrawRectangle(LightNormalBrush, new Pen(), segRect);

                }

                if (posvalue > Maximum - (LimitSegNum + ClipSegNum)) //> normal led seg
                {
                    for (int i = Maximum - (LimitSegNum + ClipSegNum); i < posvalue; i++)
                    {
                        Rect segRect = new Rect(1, topDistanceGap + (heightPerseg + segGap) * i, wd - 2, heightPerseg);
                        dc.DrawRectangle(LimitBrush, new Pen(), segRect);

                    }

                }

                if (posvalue >= Maximum - ClipSegNum)
                {

                    for (int i = Maximum - ClipSegNum; i < posvalue; i++)
                    {

                        Rect segRect = new Rect(1, topDistanceGap + (heightPerseg + segGap) * i, wd - 2, heightPerseg);
                        dc.DrawRectangle(ClipBrush, new Pen(), segRect);

                    }


                }

            }
            else
            {

                for (int i = 0; i < posvalue; i++)  //first draw normal lighted led
                {

                    Rect segRect = new Rect(1, ht + topDistanceGap - (heightPerseg + segGap) * (i + 1), wd - 2, heightPerseg);
                    dc.DrawRectangle(LightNormalBrush, new Pen(), segRect);

                }

                if (posvalue > Maximum - (LimitSegNum + ClipSegNum)) //> normal led seg
                {
                    for (int i = Maximum - (LimitSegNum + ClipSegNum); i < posvalue; i++)
                    {
                        Rect segRect = new Rect(1, ht + topDistanceGap - (heightPerseg + segGap) * (i + 1), wd - 2, heightPerseg);
                        dc.DrawRectangle(LimitBrush, new Pen(), segRect);

                    }

                }

                if (posvalue >= Maximum - ClipSegNum)
                {

                    for (int i = Maximum - ClipSegNum; i < posvalue; i++)
                    {
                        Rect segRect = new Rect(1, ht + topDistanceGap - (heightPerseg + segGap) * (i + 1), wd - 2, heightPerseg);
                        dc.DrawRectangle(ClipBrush, new Pen(), segRect);

                    }

                }

            }

        }
        public void redraw()
        {
            //  Debug.WriteLine("readraw.....");
            InvalidateVisual();
        }

    }
}
