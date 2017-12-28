using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lib.Controls.FFTControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Lib.Controls.FFTControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Lib.Controls.FFTControl;assembly=Lib.Controls.FFTControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FBCFFTDraw/>
    ///
    /// </summary>
    public class FBCFFTDraw : CommControl
    {
        /// <summary>
        /// constructor
        /// </summary>
        public FBCFFTDraw()
        {
            initalProperty();
        }
        private const double DefaultMasterGain = 0;
        public double masterGain
        {
            get { return (double)GetValue(masterGainProperty); }
            set { SetValue(masterGainProperty, value); }
        }
        // Using a DependencyProperty as the backing store for masterGainI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty masterGainProperty =
            DependencyProperty.Register("masterGain", typeof(double), typeof(FBCFFTDraw), new PropertyMetadata(DefaultMasterGain));


        #region fftData define
        private double[] PointArray = new double[FFTConstaint.NFFT];
        private double[] FreqTemp = new double[FFTConstaint.FBC_FFTNum];
        private double[] GainTemp = new double[FFTConstaint.FBC_FFTNum];
        public byte[] m_FlatFlag = new byte[FFTConstaint.FBC_FFTNum];
        #endregion


        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;
        private const double DefaultBlockRadius = 10;
        public double fBlockRadius
        {
            get { return (double)GetValue(fBlockRadiusProperty); }
            set { SetValue(fBlockRadiusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BlockRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fBlockRadiusProperty =
            DependencyProperty.Register("fBlockRadius", typeof(double), typeof(FBCFFTDraw), new PropertyMetadata(DefaultBlockRadius));


        /// <summary>
        /// inital 
        /// </summary>
        protected void initalProperty()
        {
            if (m_FlatFlag == null)
                m_FlatFlag = new byte[FFTConstaint.FBC_FFTNum];

            if (PointArray == null)
                PointArray = new double[FFTConstaint.NFFT];
            if (FreqTemp == null)
                FreqTemp = new double[FFTConstaint.FBC_FFTNum];
            if (GainTemp == null)
                GainTemp = new double[FFTConstaint.FBC_FFTNum];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag">0,1,2</param>
        /// <param name="index"> base 0...23</param>
        public void setBlockFlat(int flag, int index)
        {
            if(flag<=0||index<=0||index>23)return;
            m_FlatFlag[index] = (byte)flag;
            this.Redraw();
        }

        private static SolidColorBrush DefaultTextBrush = Brushes.White;

        public SolidColorBrush ftextBlockBrush
        {
            get { return (SolidColorBrush)GetValue(ftextBlockBrushProperty); }
            set { SetValue(ftextBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for textBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ftextBlockBrushProperty =
            DependencyProperty.Register("ftextBlockBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultTextBrush));



        public SolidColorBrush fcircleBlockBrush
        {
            get { return (SolidColorBrush)GetValue(fcircleBlockBrushProperty); }
            set { SetValue(fcircleBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for circleBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fcircleBlockBrushProperty =
            DependencyProperty.Register("fcircleBlockBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultTextBrush));



        private static SolidColorBrush DefaultFilBrush = Brushes.Transparent;
        public SolidColorBrush ffillBlockBrush
        {
            get { return (SolidColorBrush)GetValue(ffillBlockBrushProperty); }
            set { SetValue(ffillBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fillBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ffillBlockBrushProperty =
            DependencyProperty.Register("ffillBlockBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultFilBrush));






        private static SolidColorBrush DefaultOutBrush = new SolidColorBrush(Color.FromRgb(0, 0xFF, 0));
        public SolidColorBrush OutBrush
        {
            get { return (SolidColorBrush)GetValue(OutBrushProperty); }
            set { SetValue(OutBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutBrushProperty =
            DependencyProperty.Register("OutBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultOutBrush));







        public void drawBlock(DrawingContext cvs, int eindex)
        {
            int eqindex = eindex;
            if (m_FlatFlag[eindex] <= 0) return;

            double px = getBlockPosX(eqindex);
            double py = getBlockPosY(eqindex);

            //   Debug.WriteLine("block pos x:{0}:y:{1}  eqindex is : {2}", px, py, eindex);
            string ftext = (eindex + 1).ToString();
            double fontsize = 10.0;

            double offsetX = (eindex > 10 ? 7.8 : 3.0);
            if (eindex == 10 || eindex == 9)
                offsetX = 6.0;

            double offsetY = 7.0;


            SolidColorBrush penbrush = fgLineBrush;
            penbrush = fcircleBlockBrush;

            FormattedText texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
             new Typeface("Verdana"), fontsize, ftextBlockBrush);
            if (py > this.ActualHeight - offsetY * 2) return;

            cvs.DrawText(texte, new Point(px - offsetX, py - DefaultBlockRadius - offsetY));
            //cvs.DrawRectangle(ffillBlockBrush, new Pen(penbrush, 1),new ret)
            //  cvs.DrawEllipse( new Point(px, py), circleRadiosX, circleRadiosY);

            Rect cRect = new Rect(px - DefaultBlockRadius / 2, py - DefaultBlockRadius / 2, DefaultBlockRadius, DefaultBlockRadius);
            if (py <= this.ActualHeight)
                cvs.DrawRectangle(ffillBlockBrush, new Pen(penbrush, 1), cRect);


        }
        #region output interface to invoke.........
        /// <summary>
        /// for output invoke interface
        /// </summary>
        /// <param name="gData"></param>

        public void setGlobalEQData(double[] gData)
        {
            Array.Clear(PointArray, 0, FFTConstaint.NFFT); //first clear
            Array.Copy(gData, PointArray, FFTConstaint.NFFT);
        }
        public void setBlockData(double freq, double gain, int eindex)
        {
            FreqTemp[eindex] = freq;
            GainTemp[eindex] = gain;
        }
        #endregion



        /// <summary>
        /// pen brush for the border
        /// </summary>
        private static SolidColorBrush DefaultPenBrush = Brushes.Black;
        public SolidColorBrush fpenBrush
        {
            get { return (SolidColorBrush)GetValue(fpenBrushProperty); }
            set { SetValue(fpenBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for penBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fpenBrushProperty =
            DependencyProperty.Register("fpenBrush", typeof(SolidColorBrush),
                typeof(FFTDrawer), new PropertyMetadata(DefaultPenBrush));



        private static SolidColorBrush DefaultGridBrush = Brushes.LightBlue;
        public SolidColorBrush fGridBrush
        {
            get { return (SolidColorBrush)GetValue(fGridBrushProperty); }
            set { SetValue(fGridBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fGridBrushProperty =
            DependencyProperty.Register("fGridBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw),
              new FrameworkPropertyMetadata(DefaultGridBrush, FrameworkPropertyMetadataOptions.None, onGridBrushChange
            ));

        private static void onGridBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as FFTDrawer;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.GridBrush = (SolidColorBrush)args.NewValue;
                mcontrol.Redraw();
            }

        }


        private static SolidColorBrush DefaultLineBrush = Brushes.Red;
        public SolidColorBrush fgLineBrush
        {
            get { return (SolidColorBrush)GetValue(fgLineBrushProperty); }
            set { SetValue(fgLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fgLineBrushProperty =
            DependencyProperty.Register("fgLineBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw),
              new FrameworkPropertyMetadata(DefaultLineBrush, FrameworkPropertyMetadataOptions.None, onGridBrushChange
            ));

        private static void onLineBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as FFTDrawer;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.gLineBrush = (SolidColorBrush)args.NewValue;
                mcontrol.Redraw();
            }
        }
        //new PropertyMetadata(0));

        /// <summary>
        /// borderThick
        /// </summary>
        private const double DefaultBorderThick = 0.5;
        public double fBorderThick
        {
            get { return (double)GetValue(fBorderThickProperty); }
            set { SetValue(fBorderThickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for borderThick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fBorderThickProperty =
            DependencyProperty.Register("fBorderThick", typeof(double), typeof(FBCFFTDraw),
              new FrameworkPropertyMetadata(DefaultBorderThick, FrameworkPropertyMetadataOptions.None, onBorderThickChange
            ));

        private static void onBorderThickChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as FFTDrawer;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.BorderThick = (double)args.NewValue;
                mcontrol.Redraw();
            }

        }
        //



        public static SolidColorBrush DefaultInterBrush = new SolidColorBrush(Color.FromRgb(0, 0x80, 0)); //light green
        public SolidColorBrush InterGridBrush
        {
            get { return (SolidColorBrush)GetValue(InterGridBrushProperty); }
            set { SetValue(InterGridBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InterGridBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InterGridBrushProperty =
            DependencyProperty.Register("InterGridBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultInterBrush));



        public static SolidColorBrush DefaultLabelBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0)); //yellow
        public SolidColorBrush LabelBrush
        {
            get { return (SolidColorBrush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.Register("LabelBrush", typeof(SolidColorBrush), typeof(FBCFFTDraw), new PropertyMetadata(DefaultLabelBrush));




        protected void drawGrid(DrawingContext cvs)
        {
            double anx = this.ActualWidth;
            double any = this.ActualHeight;
            double delta, yy;
            delta = (FFTConstaint.FYMax - FFTConstaint.FYMin) / (FFTConstaint.FNumHorzGrid + 1);

            FormattedText texte = new FormattedText("+30", CIR, FlowDirection.LeftToRight,
               new Typeface("Verdana"), 10, LabelBrush);
            cvs.DrawText(texte, new Point(0 - 22, 0));
            cvs.DrawLine(new Pen(OutBrush, 1), new Point(0, 0), new Point(anx, 0));
            for (int i = 0; i < FFTConstaint.FNumHorzGrid; i++)   //
            {

                yy = logValue_ToFFTScreenY(FFTConstaint.FYMin + delta * (i + 1));
                if (i != (FFTConstaint.FNumHorzGrid - 1) / 2)
                {
                    cvs.DrawLine(new Pen(InterGridBrush, 1), new Point(0, yy), new Point(anx, yy));
                }
                texte = new FormattedText(FFTConstaint.strFBCGridGTable[i], CIR, FlowDirection.LeftToRight,
               new Typeface("Verdana"), 10, LabelBrush);
                cvs.DrawText(texte, new Point(0 - 22, yy - 5));

            }
            //draw middle
            double midle = logValue_ToFFTScreenY(FFTConstaint.FYMin + delta * 3);
            cvs.DrawLine(new Pen(InterGridBrush, 1), new Point(0, midle), new Point(anx, midle));
            for (int i = 0; i < FFTConstaint.GridFreqTable.Length; i++) //for test the coordinate stand   FFTConstaint.BZPlotBNum
            {
                double xx = freqToLogPosX(FFTConstaint.GridFreqTable[i]);

                double kpos = logPosXToFFTScreenPos(xx);
                //   Debug.WriteLine("xx zuobiao is :  {0}   kpos is :{1} with  freqindex : {2}  freq {3}", xx, kpos, FFTConstaint.GridFreqTable[i], i);
                cvs.DrawLine(new Pen(InterGridBrush, 1), new Point(kpos, 0), new Point(kpos, any));

                texte = new FormattedText(FFTConstaint.strGridFreqTable[i], CIR, FlowDirection.LeftToRight,
                new Typeface("Verdana"), 10, LabelBrush);
                cvs.DrawText(texte, new Point(kpos - 6, any));


            }
            texte = new FormattedText("-30", CIR, FlowDirection.LeftToRight,
               new Typeface("Verdana"), 10, LabelBrush);
            cvs.DrawText(texte, new Point(0 - 22, ActualHeight - 5));
            cvs.DrawLine(new Pen(OutBrush, 1), new Point(0, any), new Point(anx, any));

        }



        private const double DefaultCaptureRadius = 6.0;
        public double fCaptureRadius
        {
            get { return (double)GetValue(fCaptureRadiusProperty); }
            set { SetValue(fCaptureRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptureRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fCaptureRadiusProperty =
            DependencyProperty.Register("fCaptureRadius", typeof(double), typeof(FBCFFTDraw), new PropertyMetadata(DefaultCaptureRadius));



        private const double DefaultGLineWidth = 2.8;
        public double fgLineWidth
        {
            get { return (double)GetValue(fgLineWidthProperty); }
            set { SetValue(fgLineWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gLineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fgLineWidthProperty =
            DependencyProperty.Register("fgLineWidth", typeof(double), typeof(FBCFFTDraw), new PropertyMetadata(DefaultGLineWidth));

        #region freq-->lgopos->fftscreenPos
        /// <summary>
        /// convert freq(double) to Log Pos
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        /// freq-->logpos->screenpos
        public static double freqToLogPosX(double freq)
        {

            return Math.Log10(freq) / Math.Log10(FFTConstaint.SAMPLERATE) * FFTConstaint.NFFT;

        }

        /// <summary>
        /// convert x log Pos to fftdraw sceen pos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double logPosXToFFTScreenPos(double x) //screenX
        {

            double fWidth = ActualWidth;
            return fWidth * (x - FFTConstaint.FXMin) / (FFTConstaint.FXMax - FFTConstaint.FXMin);

        }

        public double freqToFFTScreenPosX(double freq)
        {
            return logPosXToFFTScreenPos(freqToLogPosX(freq));
        }

        #endregion
        #region fftscreenpos--->logpos--->freqs
        public static double logPosX_ToFreq(double screen)
        {
            return Math.Pow(10, screen / FFTConstaint.NFFT * Math.Log10(FFTConstaint.SAMPLERATE));
        }

        /// <summary>
        /// convert screen x at point to freq
        /// </summary>
        /// <param name="x"></param>s
        /// <returns></returns>
        public double fftScreenPosX_ToLogPosX(double x)
        {
            double fWidth = this.ActualWidth;
            return x * (FFTConstaint.FXMax - FFTConstaint.FXMin) / fWidth + FFTConstaint.FXMin;//fftScreenPos_toLogPoss
        }
        public double fftScreenPosXToFreq(double x)
        {
            return logPosX_ToFreq(fftScreenPosX_ToLogPosX(x));
        }

        #endregion
        public double logValue_ToFFTScreenY(double y) //screenY
        {
            double fHeight = this.ActualHeight;
            return fHeight - fHeight * (y - FFTConstaint.FYMin) / (FFTConstaint.FYMax - FFTConstaint.FYMin);
        }

        public double fftScreenY_ToLogValueY(double y)
        {
            double fHeight = this.ActualHeight;
            return (fHeight - y) * (FFTConstaint.FYMax - FFTConstaint.FYMin) / fHeight + FFTConstaint.FYMin;

        }

        /// <summary>
        /// drawGlobalLine,note in wpf draw with DrawingContext can draw outside line (left or right end)
        /// </summary>
        /// <param name="dc"></param>
        private void drawGlobalLine(DrawingContext dc)
        {

            Point p1 = new Point();
            Point p2 = new Point();
            Pen mPen = null;
            double gMaster = 0;
            mPen = new Pen(fgLineBrush, fgLineWidth);
            gMaster = masterGain;

            int i = 0;
            for (i = 0; i < FFTConstaint.NFFT - 1; i++)
            {

                p1.X = logPosXToFFTScreenPos(i);
                if (p1.X >= 0)
                {
                    //   Debug.WriteLine("point 1 x is :{0}  i is :{1}", p1.X, i);

                    p1.Y = logValue_ToFFTScreenY(PointArray[i] + gMaster);
                    if (p1.Y > this.ActualHeight)
                        p1.Y = this.ActualHeight;

                    p2.X = logPosXToFFTScreenPos(i + 1);
                    if (p2.X <= ActualWidth)
                    {
                        p2.Y = logValue_ToFFTScreenY(PointArray[i + 1] + gMaster);
                        if (p2.Y > this.ActualHeight)
                            p2.Y = this.ActualHeight;
                    }
                    else
                        break;
                    dc.DrawLine(mPen, p1, p2);
                }

            }
            //dc.DrawLine()
        }

        public void drawAllPlots(DrawingContext dc)
        {
            drawGlobalLine(dc);

            for (int i = 0; i < FFTConstaint.FBC_FFTNum; i++) //FFTConstaint.CEQ_MAX/2                       
                drawBlock(dc, i);
        }

        /// <summary>
        /// onRender Main Method
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double wd = ActualWidth;
            double ht = ActualHeight;
            //  Debug.WriteLine("on render width ,height is : {0}  {1}", wd, ht);
            dc.DrawRectangle(BackBrush, new Pen(fpenBrush, fBorderThick), new Rect(0, 0, wd, ht));
            drawGrid(dc);
            drawAllPlots(dc);

        }

        #region function define below..........
        public double getBlockPosX(int eindex)
        {
            return freqToFFTScreenPosX(FreqTemp[eindex]);
        }
        //but only changed of the entire y position
        public double getBlockPosY(int eindex)
        {
            return logValue_ToFFTScreenY(GainTemp[eindex] + masterGain);
        }
        #endregion




        //  protected double screenx



    }
}
