using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Input;
using System.Globalization;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：HelloMove
 *文件名：  SKillerView
 *版本号：  V1.0.0.0
 *唯一标识：55ec11d9-4d2c-41cb-92be-b93c9f5b555b
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/29/2016 8:52:14 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/29/2016 8:52:14 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：if the object is 
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class CSlider : Control
    {
        public static int DefaultMaximum = 100;
        public static ImageSource DefaultImgsrc = null;
        private static Color DefaultLineColor = Color.FromRgb(40, 40, 40);
        private const double DefaultLineWidth = 1.0;
        private static double DefaultTopGap = 15.0;
        private static double DefaultBotomGap = 14.0;


        private int _posvalue = 0;
        private bool isTouched = false;
        private const double offsetY = 3.0;
        private int shift = 0;
        private double step = 0;



        private TextBlock numberLabel;
        private double lastPosition = 0;  //the posvalue last at position
        private int lastPosValue = 0;  //the posvalue last


        private static Rect thumbRect = new Rect();

        public delegate void sliderMouseDown(Object sender, EventArgs e);
        public event sliderMouseDown onSliderMouseDownEvent;
        //
        public delegate void sliderMouseMove(Object sender, int newPos, EventArgs e);
        public event sliderMouseMove onSliderMouseMoveEvent;


        private const bool DefaultMark = false;
        public bool isSupportMark
        {
            get { return (bool)GetValue(isSupportMarkProperty); }
            set { SetValue(isSupportMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isSupportMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSupportMarkProperty =
            DependencyProperty.Register("isSupportMark", typeof(bool), typeof(CSlider), new PropertyMetadata(DefaultMark));



        private const double DefaultMarkStep = 6.0;
        public double MarkStep
        {
            get { return (double)GetValue(MarkStepProperty); }
            set { SetValue(MarkStepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkStepProperty =
            DependencyProperty.Register("MarkStep", typeof(double), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultMarkStep, FrameworkPropertyMetadataOptions.None, onMarkStepChange
            ));

        private static void onMarkStepChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.MarkStep = (double)args.NewValue;
                mcontrol.redraw();
            }
        }



        private const bool DefaultLineStyle = false;
        public bool lineStyle
        {
            get { return (bool)GetValue(lineStyleProperty); }
            set { SetValue(lineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lineStyleProperty =
            DependencyProperty.Register("lineStyle", typeof(bool), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultLineStyle, FrameworkPropertyMetadataOptions.None, onLineStyleChange));

        private static void onLineStyleChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                mcontrol.lineStyle = (bool)args.NewValue;
                mcontrol.redraw();
            }
        }



        static CSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CSlider), new FrameworkPropertyMetadata(typeof(CSlider)));

        }



        public ImageSource thumb
        {
            get { return (ImageSource)GetValue(thumbProperty); }
            set { SetValue(thumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty thumbProperty =
            DependencyProperty.Register("thumb", typeof(ImageSource), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultImgsrc, FrameworkPropertyMetadataOptions.None, onThumbChange
                ));

        private static void onThumbChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;

                mcontrol.thumb = (ImageSource)args.NewValue;
                mcontrol.recaculateThumb();
                mcontrol.recaculate();
                mcontrol.redraw();
            }
        }


        private const bool DefaultIsVertial = true;
        public bool isVerticalSlider
        {
            get { return (bool)GetValue(isVerticalSliderProperty); }
            set { SetValue(isVerticalSliderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isVerticalSlider.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isVerticalSliderProperty =
            DependencyProperty.Register("isVerticalSlider", typeof(bool), typeof(CSlider),
             new FrameworkPropertyMetadata(DefaultIsVertial, FrameworkPropertyMetadataOptions.None, onSliderOrietainChange
                ));
        private static void onSliderOrietainChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = 
                mcontrol.isVerticalSlider = (bool)args.NewValue;
                mcontrol.recaculateThumb();
                mcontrol.recaculate();
                mcontrol.redraw();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(Color), typeof(CSlider), new PropertyMetadata(DefaultLineColor));




        public double lineWidth
        {
            get { return (double)GetValue(lineWidthProperty); }
            set { SetValue(lineWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lineWidthProperty =
            DependencyProperty.Register("lineWidth", typeof(double), typeof(CSlider), new PropertyMetadata(DefaultLineWidth));



        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            //  e.GetPosition()           

            Point pt = e.GetPosition(this);

            if ((pt.Y >= (thumbRect.Top - offsetY)) && (pt.Y < (thumbRect.Top + thumbRect.Height + offsetY)))
            {
                isTouched = true;
                if (isVerticalSlider)
                {
                    lastPosition = pt.Y;
                }
                else
                {
                    lastPosition = pt.X;

                }
                lastPosValue = Posvalue;//record the posvalue as save

                redraw();
            }
            else
            {
                if (onSliderMouseDownEvent != null)
                {
                    onSliderMouseDownEvent(this, new EventArgs());
                }

            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isTouched) return;
            if (e.LeftButton == MouseButtonState.Released)
            {
                isTouched = false;
                shift = 0;
                return;
            }
            Point pt = e.GetPosition(this);
            double deltaV = 0;
            if (isVerticalSlider)
                deltaV = pt.Y - lastPosition;
            else
                deltaV = pt.X - lastPosition;
            shift = (int)(deltaV / step);

            //  int tmpValue=limitValue()
            int tmpValue = 0;
            if (isVerticalSlider)
                tmpValue = Maximum - (lastPosValue + shift);
            else
                tmpValue = (lastPosValue + shift);
            tmpValue = limitValue(tmpValue);
            //  Debug.WriteLine("shift is : {0}...step is : {1}..new value is.......", shift,step,tmpValue);
            if (onSliderMouseMoveEvent != null)
            {
                onSliderMouseMoveEvent(this, tmpValue, new EventArgs());
            }


        }



        //protected override void OnMouseLeave(MouseEventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //    Debug.WriteLine("  ");
        //    if (e.LeftButton == MouseButtonState.Released)
        //    {
        //        isTouched = false;
        //        shift = 0;
        //    }
        //}

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            // Debug.WriteLine("on mouse up...............");
            isTouched = false;
            shift = 0;
            redraw();


        }

        public int Maximum
        {
            get { return (int)GetValue(MaxminumProperty); }
            set { SetValue(MaxminumProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Maxminum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxminumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultMaximum, FrameworkPropertyMetadataOptions.None, onMaximumChange
                ));

        private static void onMaximumChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;

            if (mcontrol != null)
            {
                int tmax = (int)args.NewValue;
                if (tmax > 0)
                {
                    mcontrol.Maximum = tmax;
                    // Debug.WriteLine("maximum change.........................{0}", mcontrol.Maximum);
                    mcontrol.recaculate();
                    mcontrol.redraw();
                }

            }

        }

        public int Minimum
        {
            get { return (int)GetValue(MinminumProperty); }
            set { SetValue(MinminumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minminum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinminumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(CSlider), new PropertyMetadata(0));



        public int Posvalue
        {
            get
            {
                return _posvalue;

            }
            set
            {
                setPosValue(value);
            }
        }

        public void setPosValue(int mpos)
        {
            int mv = limitValue(mpos);
            if (isVerticalSlider)
                _posvalue = Maximum - mv;
            else
                _posvalue = mv;
            redraw();
        }


        public int limitValue(int mva)
        {
            return Math.Max(Minimum, Math.Min(Maximum, mva));
        }
        //       

        public double TopGap
        {
            get { return (double)GetValue(TopGapProperty); }
            set { SetValue(TopGapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopGap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopGapProperty =
            DependencyProperty.Register("TopGap", typeof(double), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultTopGap, FrameworkPropertyMetadataOptions.None, onTopGapChange
                ));

        private static void onTopGapChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                mcontrol.TopGap = (double)args.NewValue;
                mcontrol.recaculate();
                mcontrol.redraw();
            }
        }


        public double BottomGap
        {
            get { return (double)GetValue(BottomGapProperty); }
            set { SetValue(BottomGapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomGap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomGapProperty =
            DependencyProperty.Register("BottomGap", typeof(double), typeof(CSlider),
            new FrameworkPropertyMetadata(DefaultBotomGap, FrameworkPropertyMetadataOptions.None, onBotomGapChange
                ));

        private static void onBotomGapChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                mcontrol.BottomGap = (double)args.NewValue;
                mcontrol.recaculate();
                mcontrol.redraw();
            }


        }
        //
        public void recaculateThumb()
        {
            if (thumb != null)
            {
                thumbRect.Width = thumb.Width;
                thumbRect.Height = thumb.Height;
                if (isVerticalSlider)
                {
                    thumbRect.X = (this.ActualWidth - thumb.Width) / 2 + xoffset;
                    thumbRect.Y = TopGap + Posvalue * step;

                }
                else //horizontal
                {
                    thumbRect.X = TopGap + Posvalue * step;
                    thumbRect.Y = (this.ActualHeight - thumb.Height) / 2;

                }
            }
        }

        double precal(double k, int fbase)
        {
            int l;
            double m;
            int KG = (int)Math.Pow(10, fbase);
            l = (int)(k * KG);
            m = (double)l / KG;
            return m;

        }


        private const double DefaultXoffset = 0;
        public double xoffset
        {
            get { return (double)GetValue(xoffsetProperty); }
            set { SetValue(xoffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for xoffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty xoffsetProperty =
            DependencyProperty.Register("xoffset", typeof(double), typeof(CSlider),
              new FrameworkPropertyMetadata(DefaultXoffset, FrameworkPropertyMetadataOptions.None, onXoffsetChange
                ));

        private static void onXoffsetChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                mcontrol.xoffset = (double)args.NewValue;
                mcontrol.recaculateThumb();
                mcontrol.redraw();
            }


        }


        public void recaculate()
        {

            if (thumb != null && Maximum > 0)
            {
                double sliderRange = 0;
                if (isVerticalSlider)
                {
                    double ht = this.ActualHeight;
                    sliderRange = ht - TopGap - BottomGap - thumb.Height;
                }
                else
                {
                    double wd = this.ActualWidth;
                    sliderRange = wd - TopGap - BottomGap - thumb.Width;
                }

                step = (double)sliderRange / Maximum;
                //  Debug.WriteLine(".xxx...cliser isvertical ? {0}  step is {1}", isVerticalSlider, step);

            }

        }


        private static SolidColorBrush DefaultBackBrush = new SolidColorBrush();

        public SolidColorBrush BackBrush
        {
            get { return (SolidColorBrush)GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(SolidColorBrush), typeof(CSlider),
             new FrameworkPropertyMetadata(DefaultBackBrush, FrameworkPropertyMetadataOptions.None, onBackBrushChange
                ));

        private static void onBackBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CSlider;
            if (mcontrol != null)
            {
                mcontrol.BackBrush = (SolidColorBrush)args.NewValue;
                mcontrol.redraw();
            }


        }

        public static string[] strMarkHR
           = {    
                       
             "-∞", 
             "-50", 
             "-40",
             "-30",
             "-20",
             "-10",
             "-5",
             " 0",
             " +5",
             "dB",
             "+15"
  
              };

        public static string[] strMarkVR
            = {
            "+15",
            " dB",
            "  0",
            "-10",
            "-20",
            "-30",
            "-40",
            "-50",
            "-60",
            "-70",                    
            "-80",                                         
              };
        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;


        private const double DefaultmarkXoffset = 4.0;
        public double markXoffset
        {
            get { return (double)GetValue(markXoffsetProperty); }
            set { SetValue(markXoffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for markXoffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty markXoffsetProperty =
            DependencyProperty.Register("markXoffset", typeof(double), typeof(CSlider), new PropertyMetadata(DefaultmarkXoffset));


        private double fontSize = 10;
        private double markLineLengthVR = 8;
        private double markLineHeightHR = 4;

        private void drawMarkVR(DrawingContext dc)
        {
            int last = strMarkVR.Length - 1;
            FormattedText text = null;

            int len = strMarkVR.Length;
            double wd = this.ActualWidth;

            double markxoffsets = wd / 2 + xoffset - 5 - markLineLengthVR;
            double markShortGap = 4.0;
            double markYoffset = 2.0;
            Pen tmpPen = new Pen(Brushes.Red, 1);
            double tmpYset = TopGap + 8;
            if (len > 0 && isSupportMark)
            {
                for (int i = 0; i < len; i++)
                {
                    if (i < len - 1)
                    {
                        text = new FormattedText(strMarkVR[i], CIR, FlowDirection.LeftToRight,
                                           new Typeface("Verdana"), fontSize, Brushes.White);
                    }
                    else
                    {
                        text = new FormattedText(strMarkVR[last], CIR, FlowDirection.LeftToRight,
                                  new Typeface("Verdana"), fontSize + 2, Brushes.White);
                    }                   
                       
                     if(i==2 || i==3)
                        dc.DrawText(text, new Point(markXoffset, (tmpYset + (MarkStep + 0.35) * i - 4)));                  
                    else
                       dc.DrawText(text, new Point(markXoffset, tmpYset + (MarkStep + 0.35) * i));
#if _koc
                    if (i == 3)
                        tmpPen = new Pen(Brushes.Red, 1);
                    else
                        tmpPen = new Pen(Brushes.Black, 1);

                    dc.DrawLine(tmpPen, new Point(markxoffsets, MarkStep * i + tmpYset + markYoffset),
                           new Point(markxoffsets + markLineLengthVR - 2, MarkStep * i + tmpYset + markYoffset));

                    dc.DrawLine(tmpPen, new Point(markxoffsets, MarkStep * i + tmpYset + markShortGap + markYoffset),
                           new Point(markxoffsets + markLineLengthVR, MarkStep * i + tmpYset + markShortGap + markYoffset));
#endif

                }
            }


        }






        private void drawMarkHR(DrawingContext dc)
        {
            int last = strMarkHR.Length - 1;
            FormattedText text = null;

            int len = strMarkHR.Length;
            double ht = this.ActualHeight;
            double markxoffsets = TopGap + 1;
            double markYoffset = ht - markLineHeightHR;
            Pen tmpPen = new Pen(Brushes.Red, 1);
            if (len > 0 && isSupportMark)
            {
                //Debug.WriteLine("drawmark hr len is :{0}", len);
                for (int k = 0; k <= len; k++)
                {
#if KOV
                    if (i >0)
                    {
                        text = new FormattedText(strMarkHR[i], CIR, FlowDirection.LeftToRight,
                                           new Typeface("Verdana"), fontSize, Brushes.Black);



                    }
                    else
                    {
                        text = new FormattedText(strMarkHR[last], CIR, FlowDirection.LeftToRight,
                                  new Typeface("Verdana"), fontSize + 2, Brushes.Black);
                    }

                    dc.DrawText(text, new Point(markXoffset, (MarkStep) * i));
#endif
                    if (k == len - 3)
                        tmpPen = new Pen(Brushes.Red, 1);
                    else
                        tmpPen = new Pen(Brushes.Black, 1);

                    double tmpX = markxoffsets + MarkStep * k - 3;
                    if (tmpX >= this.ActualWidth)
                        tmpX = this.ActualWidth - 5;

                    if (k > 0)
                        dc.DrawLine(tmpPen, new Point(tmpX, markYoffset - 2),
                               new Point(tmpX, markYoffset - 8));

                    tmpX = markxoffsets + MarkStep * k;
                    if (tmpX >= this.ActualWidth)
                        tmpX = this.ActualWidth - 2;
                    dc.DrawLine(tmpPen, new Point(tmpX, markYoffset),
                           new Point(tmpX, markYoffset - 8));

                }
            }


        }

        private const double trackRectangleXoffset = 4.0;
        private const double trackRectangRadius = 3.0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            recaculate();
            double wd = ActualWidth;
            double ht = ActualHeight;
            recaculateThumb();
            if (isVerticalSlider)
                MarkStep = (int)((ht - TopGap - BottomGap - thumb.Height) / (strMarkVR.Length - 1));
            else   //hr
                MarkStep = (int)((wd - TopGap - BottomGap - thumb.Width) / (strMarkVR.Length - 1));

            dc.DrawRectangle(BackBrush, new Pen(), new Rect(0, 0, wd, ht));


            //  Debug.WriteLine("cslider width is : {0} ,ht is :{1}", wd, ht);
            Pen apen = new Pen(new SolidColorBrush(LineColor), lineWidth);
            if (isVerticalSlider)
            {
                drawMarkVR(dc);
                if (lineStyle)
                {
                    dc.DrawLine(apen, new Point(wd / 2 + xoffset, TopGap), new Point(wd / 2 + xoffset, ht - BottomGap));
                }
                else
                {

                  //  Rect tmprect = new Rect(thumbRect.Left + trackRectangleXoffset, TopGap, thumbRect.Width - trackRectangleXoffset * 2, ht - BottomGap * 2);
                   // dc.DrawRoundedRectangle(Brushes.Transparent, apen, tmprect, trackRectangRadius, trackRectangRadius);
                }

            }
            else
            {
                drawMarkHR(dc);
                if (lineStyle)
                    dc.DrawLine(apen, new Point(TopGap, ht / 2), new Point(wd - BottomGap, ht / 2));
                else
                {

                   // Rect tmprect = new Rect(TopGap, thumbRect.Top + trackRectangleXoffset, wd - 2 * BottomGap, thumbRect.Height - trackRectangleXoffset * 2);
                   // dc.DrawRoundedRectangle(Brushes.Transparent, apen, tmprect, trackRectangRadius, trackRectangRadius);
                }

            }
            dc.DrawImage(thumb, thumbRect);

        }

        /// <summary>
        /// slider Tag here
        /// </summary>
        private const int DefaultTag = 0;
        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(CSlider), new PropertyMetadata(DefaultTag));



        public void redraw()
        {
            InvalidateVisual();

        }

    }
}
