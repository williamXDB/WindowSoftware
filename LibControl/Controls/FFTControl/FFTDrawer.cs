using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：HelloMove
 *文件名：  FFTDrawer
 *版本号：  V1.0.0.0
 *唯一标识：e625d99f-867a-4a20-a2a4-16c4befa45f4
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/30/2016 2:27:37 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/30/2016 2:27:37 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *suppose there are two channels to caculate.so channel is referecne now.
 *
/************************************************************************************/

namespace Lib.Controls
{

    public enum FFT_LineType
    {
        LineI = 0, LineII = 1
    };
    public class FFTDrawer : CommControl
    {



        private const double DefaultMasterGain = 0;
        public double masterGainI
        {
            get { return (double)GetValue(masterGainIProperty); }
            set { SetValue(masterGainIProperty, value); }
        }
        // Using a DependencyProperty as the backing store for masterGainI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty masterGainIProperty =
            DependencyProperty.Register("masterGainI", typeof(double), typeof(FFTDrawer), new PropertyMetadata(DefaultMasterGain));


        /// <summary>
        /// masterGainII change event popup
        /// </summary>

        public double masterGainII
        {
            get { return (double)GetValue(masterGainIIProperty); }
            set { SetValue(masterGainIIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for masterGainII.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty masterGainIIProperty =
            DependencyProperty.Register("masterGainII", typeof(double), typeof(FFTDrawer), new PropertyMetadata(DefaultMasterGain));

        private double[][] PointArray = new double[FFTConstaint.LINE_MAX][];
        private double[][] curvePointArray = new double[FFTConstaint.CEQ_MAX * 2][];

        private double[] FreqTemp = new double[FFTConstaint.CEQ_MAX * 2];
        private double[] GainTemp = new double[FFTConstaint.CEQ_MAX * 2];
        private int[] qIndexTemp = new int[FFTConstaint.CEQ_MAX * 2];

        private bool isTouched = false;
        // private bool isNeedDrawBlock = false;

        private const FFT_LineType DefaultLineType = FFT_LineType.LineI;
        public FFT_LineType activeLine
        {
            get { return (FFT_LineType)GetValue(activeLineProperty); }
            set { SetValue(activeLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeLineProperty =
            DependencyProperty.Register("activeLine", typeof(FFT_LineType), typeof(FFTDrawer), new PropertyMetadata(DefaultLineType));

        private const bool DefaultDrawWave = true;
        public bool isDrawWave
        {
            get { return (bool)GetValue(isDrawWaveProperty); }
            set { SetValue(isDrawWaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isDrawBlock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isDrawWaveProperty =
            DependencyProperty.Register("isDrawWave", typeof(bool), typeof(FFTDrawer),
            new FrameworkPropertyMetadata(DefaultDrawWave, FrameworkPropertyMetadataOptions.None, onIsNeedWaveChange
            ));

        private static void onIsNeedWaveChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as FFTDrawer;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.isDrawWave = (bool)args.NewValue;
                mcontrol.Redraw();
            }

        }

        private bool[] curveFlats = new bool[FFTConstaint.CEQ_MAX * 2]; //false as default 
        private Color[] curveColor = new Color[FFTConstaint.CEQ_MAX];

        private Color[] circumColor = new Color[FFTConstaint.CEQ_MAX];


        private const byte ColorAlpha = 134;


        private const bool DefaultSupportMutiLine = false;
        public bool isSupportMutiLine
        {
            get { return (bool)GetValue(isSupportMutiLineProperty); }
            set { SetValue(isSupportMutiLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isSupportMutiLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSupportMutiLineProperty =
            DependencyProperty.Register("isSupportMutiLine", typeof(bool), typeof(FFTDrawer), new PropertyMetadata(DefaultSupportMutiLine));

        private const bool DefaultHasHLPF = false;
        public bool isHasHLPF
        {
            get { return (bool)GetValue(isHasHLPFProperty); }
            set { SetValue(isHasHLPFProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isHasHLPF.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isHasHLPFProperty =
            DependencyProperty.Register("isHasHLPF", typeof(bool), typeof(FFTDrawer), new PropertyMetadata(DefaultHasHLPF));

        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;

        public void setCureFlat(bool isFlat, int eindex, bool isRedraw)
        {
            curveFlats[eindex] = isFlat;
            if (isRedraw)
                Redraw();
        }

        /// <summary>
        /// 
        /// </summary>
        private const double DefaultBlockDiameter = 10;

        public double BlockDiameter
        {
            get { return (double)GetValue(BlockDiameterProperty); }
            set { SetValue(BlockDiameterProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BlockDiameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockDiameterProperty =
            DependencyProperty.Register("BlockDiameter", typeof(double), typeof(FFTDrawer), new PropertyMetadata(DefaultBlockDiameter));



        private const int DefaultSelIndex = 0;


        /// <summary>
        /// is used for mark left/right q block
        /// </summary>
        public int activeSelectBlockIndex
        {
            get { return (int)GetValue(activeSelectBlockIndexProperty); }
            set { SetValue(activeSelectBlockIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeSelectBlockIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeSelectBlockIndexProperty =
            DependencyProperty.Register("activeSelectBlockIndex", typeof(int), typeof(FFTDrawer), new PropertyMetadata(DefaultSelIndex));







        private const int DefaultPKMouseIndex = -1;
        public int PKMouseIndex
        {
            get { return (int)GetValue(PKMouseIndexProperty); }
            set { SetValue(PKMouseIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PKMouseIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PKMouseIndexProperty =
            DependencyProperty.Register("PKMouseIndex", typeof(int), typeof(FFTDrawer), new PropertyMetadata(DefaultPKMouseIndex));

        public void setBlockData(double freq, double gain, double qvalue, int eindex)
        {
            FreqTemp[eindex] = freq;
            GainTemp[eindex] = gain;
            int qvindex = HalfSearcher.search(StrValueTables.QVFactorTable, StrValueTables.QVFactorTable.Length, qvalue);
            if (qvindex >= 0)
            {
                qIndexTemp[eindex] = qvindex;
                // Debug.WriteLine("fftdrawer listener writinto for save qindex {0}  eqindex {1}", qvindex, eindex);

            }


        }

        /// <summary>
        /// initial parameters
        /// </summary>
        protected void initalProperty()
        {
            if (PointArray == null)
                PointArray = new double[FFTConstaint.LINE_MAX][];
            if (curvePointArray == null)
                curvePointArray = new double[FFTConstaint.CEQ_MAX * 2][];

            if (FreqTemp == null)
                FreqTemp = new double[FFTConstaint.CEQ_MAX * 2];

            if (GainTemp == null)
                GainTemp = new double[FFTConstaint.CEQ_MAX * 2];


            for (int i = 0; i < FFTConstaint.LINE_MAX; i++)
            {
                PointArray[i] = new double[FFTConstaint.NFFT];
            }

            for (int i = 0; i < FFTConstaint.CEQ_MAX * 2; i++)
            {
                curvePointArray[i] = new double[FFTConstaint.NFFT];
            }
            curveColor[0] = Color.FromArgb(ColorAlpha, 233, 252, 227);
            //high ,low  for reserved
            curveColor[1] = Color.FromArgb(ColorAlpha, 0, 140, 245);
            curveColor[2] = Color.FromArgb(ColorAlpha, 166, 202, 240);
            curveColor[3] = Color.FromArgb(ColorAlpha, 255, 0, 255);
            curveColor[4] = Color.FromArgb(ColorAlpha, 28, 163, 97);

            curveColor[5] = Color.FromArgb(ColorAlpha, 58, 110, 165);
            curveColor[6] = Color.FromArgb(ColorAlpha, 255, 0, 0);
            curveColor[7] = Color.FromArgb(ColorAlpha, 255, 255, 0);

            curveColor[8] = Color.FromArgb(ColorAlpha, 28, 163, 97); //high filterPas
            curveColor[9] = Color.FromArgb(ColorAlpha, 0, 255, 0);//low filterPas


            //circumColor
            circumColor[0] = Color.FromRgb(233, 252, 227);
            //high ,low  for reserved
            circumColor[1] = Color.FromRgb(0xFF, 0xFF, 0xFF);
            circumColor[2] = Color.FromRgb(166, 202, 240);
            circumColor[3] = Color.FromRgb(255, 0, 255);
            circumColor[4] = Color.FromRgb(28, 163, 97);

            circumColor[5] = Color.FromRgb(0xFF, 0xFF, 0xFF);
            circumColor[6] = Color.FromRgb(255, 0, 0);
            circumColor[7] = Color.FromRgb(255, 255, 0);

            circumColor[8] = Color.FromRgb(0xFF, 0xFF, 0xFF); //high filterPas
            circumColor[9] = Color.FromRgb(0, 255, 0);//low filterPas




        }




        public void set_CurveEQData(int eindex, double[] eData)  //[0..19]
        {
            if (eData == null || eData.Length != FFTConstaint.NFFT) return;
            Array.Copy(eData, 0, curvePointArray[eindex], 0, FFTConstaint.NFFT);

        }



        private static SolidColorBrush DefaultTextBrush = Brushes.White;

        public SolidColorBrush textBlockBrush
        {
            get { return (SolidColorBrush)GetValue(textBlockBrushProperty); }
            set { SetValue(textBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for textBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty textBlockBrushProperty =
            DependencyProperty.Register("textBlockBrush", typeof(SolidColorBrush), typeof(FFTDrawer), new PropertyMetadata(DefaultTextBrush));



        public SolidColorBrush circleBlockBrush
        {
            get { return (SolidColorBrush)GetValue(circleBlockBrushProperty); }
            set { SetValue(circleBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for circleBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty circleBlockBrushProperty =
            DependencyProperty.Register("circleBlockBrush", typeof(SolidColorBrush), typeof(FFTDrawer), new PropertyMetadata(DefaultTextBrush));



        private static SolidColorBrush DefaultFilBrush = Brushes.Transparent;
        public SolidColorBrush fillBlockBrush
        {
            get { return (SolidColorBrush)GetValue(fillBlockBrushProperty); }
            set { SetValue(fillBlockBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fillBlockBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fillBlockBrushProperty =
            DependencyProperty.Register("fillBlockBrush", typeof(SolidColorBrush), typeof(FFTDrawer), new PropertyMetadata(DefaultFilBrush));


        public void drawCircleBlock_withInnerText(DrawingContext cvs, int eindex, int chindex)
        {
            int eqindex = (chindex == 0 ? eindex : eindex + FFTConstaint.CEQ_MAX);
            //double gMaster = (chindex == 0 ? masterGainI : masterGainII);          

            if (!curveFlats[eqindex])
            {
                double px = getBlockPosX(eqindex);
                double py = getBlockPosY(eqindex);

                //   Debug.WriteLine("block pos x:{0}:y:{1}  eqindex is : {2}", px, py, eindex);
                string ftext = (eindex + 1).ToString();
                double fontsize = 10.0;
                double offsetX = 3.0;
                double offsetY = 7.0;
                double circleRadiosX = DefaultBlockDiameter - 2;
                double circleRadiosY = DefaultBlockDiameter - 2;

                SolidColorBrush penbrush = (chindex == 0 ? gLineBrush : gLineBrushII);
                if (!isSupportMutiLine)
                    penbrush = circleBlockBrush;
                if (eindex == FFTConstaint.CEQ_MAX - 2)
                {
                    ftext = "HPF";
                    fontsize = 7;
                    offsetX = 6.5;
                    offsetY = 5;
                    circleRadiosX = DefaultBlockDiameter;
                    circleRadiosY = DefaultBlockDiameter - 2;
                }
                else if (eindex == FFTConstaint.CEQ_MAX - 1)
                {
                    ftext = "LPF";
                    fontsize = 7;
                    offsetX = 6.5;
                    offsetY = 5;
                    circleRadiosX = DefaultBlockDiameter;
                    circleRadiosY = DefaultBlockDiameter - 2;

                }
                FormattedText texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                 new Typeface("Verdana"), fontsize, textBlockBrush);
                if (py > this.ActualHeight - offsetY * 2) return;

                cvs.DrawEllipse(fillBlockBrush, new Pen(penbrush, 1), new Point(px, py), circleRadiosX, circleRadiosY);


            }

        }
        private const double Qdistance = 2.0;



        private const double QDefaultRatio = 0.40;
        public double QShiftRatio
        {
            get { return (double)GetValue(QShiftRatioProperty); }
            set { SetValue(QShiftRatioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QShiftRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QShiftRatioProperty =
            DependencyProperty.Register("QShiftRatio", typeof(double), typeof(FFTDrawer), new PropertyMetadata(QDefaultRatio));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cvs"></param>
        /// <param name="eindex"></param>
        /// <param name="chindex"></param>
        public void drawSquareQBlock_withLine(DrawingContext cvs, int eindex, int chindex)
        {
            if (eindex < 0 || eindex >= FFTConstaint.CEQ_MAX) return;
            int eqindex = (chindex == 0 ? eindex : eindex + FFTConstaint.CEQ_MAX);
            //double gMaster = (chindex == 0 ? masterGainI : masterGainII);         
            if (!curveFlats[eqindex])
            {
                double px = getBlockPosX(eqindex);
                double py = getBlockPosY(eqindex);

                //   Debug.WriteLine("block pos x:{0}:y:{1}  eqindex is : {2}", px, py, eindex);             

                double diameter = DefaultBlockDiameter;

                SolidColorBrush penbrush = Brushes.White;       //(chindex == 0 ? gLineBrush : gLineBrushII);
                //if (!isSupportMutiLine)
                //    penbrush = circleBlockBrush;
                if (eindex < FFTConstaint.CEQ_MAX - 2)
                {

                    double qvalue = QShiftRatio * qIndexTemp[eqindex];
                    Rect leftQblock = new Rect(px - 1.5 * diameter - Qdistance - qvalue, py - diameter / 2, diameter, diameter);
                    Rect rightQblock = new Rect(px + diameter / 2 + Qdistance + qvalue, py - diameter / 2, diameter, diameter);




                    //draw a line from left Qblock to right qblock with two center point
                    double leftQCenterX = px - diameter / 2 - Qdistance - diameter / 2 - qvalue;
                    double rightQCenterX = px + diameter / 2 + Qdistance + diameter / 2 + qvalue;
                    Pen linePen = new Pen(Brushes.Red, 1);
                    linePen.DashStyle = DashStyles.Dash;

                    if (py <= this.ActualHeight)
                    {
                        cvs.DrawLine(new Pen(Brushes.Red, 1), new Point(leftQCenterX, py), new Point(rightQCenterX, py));
                        //draw two left and right block
                        cvs.DrawRectangle(Brushes.Black, new Pen(penbrush, 2), leftQblock);
                        cvs.DrawRectangle(Brushes.Black, new Pen(penbrush, 2), rightQblock);
                    }


                    //   if (py <= this.ActualHeight)
                    ///   cvs.DrawRectangle(fillBlockBrush, new Pen(penbrush, 1), cRect);







                }




            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cvs"></param>
        /// <param name="eindex"></param>
        /// <param name="chindex"></param>
        public void drawSquareMainBlock_withUpText(DrawingContext cvs, int eindex, int chindex)
        {
            int eqindex = (chindex == 0 ? eindex : eindex + FFTConstaint.CEQ_MAX);
            //double gMaster = (chindex == 0 ? masterGainI : masterGainII);          

            if (!curveFlats[eqindex])
            {
                double px = getBlockPosX(eqindex);
                double py = getBlockPosY(eqindex);

                //   Debug.WriteLine("block pos x:{0}:y:{1}  eqindex is : {2}", px, py, eindex);
                string ftext = (eindex + 1).ToString();
                double fontsize = 10.0;
                double offsetX = 3.0; 
                double offsetY = 7.0 + DefaultBlockDiameter;
                double squalRadius = DefaultBlockDiameter;

                const double circleRadius = 7;
                SolidColorBrush penbrush = Brushes.White;     //(chindex == 0 ? gLineBrush : gLineBrushII);
                if (isSupportMutiLine)
                textBlockBrush = (chindex == 0 ? gLineBrush : gLineBrushII);

                //   if (!isSupportMutiLine)
                //   penbrush = circleBlockBrush;
                if (eindex == FFTConstaint.CEQ_MAX - 2)
                {
                    ftext = "HPF";
                    fontsize = 8;
                    offsetX = 6.5;
                    offsetY = 5 + DefaultBlockDiameter;
                    if (py <= this.ActualHeight)
                        cvs.DrawEllipse(fillBlockBrush, new Pen(penbrush, 1), new Point(px, py), circleRadius, circleRadius);
                }
                else if (eindex == FFTConstaint.CEQ_MAX - 1)
                {
                    ftext = "LPF";
                    fontsize = 8;
                    offsetX = 6.5;
                    offsetY = 5 + DefaultBlockDiameter;
                    if (py <= this.ActualHeight)
                        cvs.DrawEllipse(fillBlockBrush, new Pen(penbrush, 1), new Point(px, py), circleRadius, circleRadius);
                }
                else
                {
                    Rect cRect = new Rect(px - squalRadius / 2, py - squalRadius / 2, squalRadius, squalRadius);
                    if (py <= this.ActualHeight)
                        cvs.DrawRectangle(fillBlockBrush, new Pen(penbrush, 1), cRect);
                }
                FormattedText texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                 new Typeface("Verdana"), fontsize, textBlockBrush);


                if (py > this.ActualHeight) return;
                cvs.DrawText(texte, new Point(px - offsetX, py - offsetY));



            }

        }

        public void setGlobalEQData(double[] gData, int chindex)
        {
            if (gData == null || gData.Length != FFTConstaint.NFFT || chindex >= FFTConstaint.LINE_MAX || chindex < 0) return;
            Array.Clear(PointArray[chindex], 0, FFTConstaint.NFFT); //first clear
            Array.Copy(gData, PointArray[chindex], FFTConstaint.NFFT);
        }



        public FFTDrawer()
        {
            initalProperty();


        }

        /// <summary>
        /// pen brush for the border
        /// </summary>
        private static SolidColorBrush DefaultPenBrush = Brushes.Black;
        public SolidColorBrush penBrush
        {
            get { return (SolidColorBrush)GetValue(penBrushProperty); }
            set { SetValue(penBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for penBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty penBrushProperty =
            DependencyProperty.Register("penBrush", typeof(SolidColorBrush),
                typeof(FFTDrawer), new PropertyMetadata(DefaultPenBrush));



        private static SolidColorBrush DefaultGridHeadBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        public SolidColorBrush GridHeaderBrush
        {
            get { return (SolidColorBrush)GetValue(GridHeaderBrushProperty); }
            set { SetValue(GridHeaderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridHeaderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridHeaderBrushProperty =
            DependencyProperty.Register("GridHeaderBrush", typeof(SolidColorBrush), typeof(FFTDrawer), new PropertyMetadata(DefaultGridHeadBrush));

        

        private static SolidColorBrush DefaultGridBrush = Brushes.LightBlue;
        public SolidColorBrush GridBrush
        {
            get { return (SolidColorBrush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(SolidColorBrush), typeof(FFTDrawer),
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
        public SolidColorBrush gLineBrush
        {
            get { return (SolidColorBrush)GetValue(gLineBrushProperty); }
            set { SetValue(gLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty gLineBrushProperty =
            DependencyProperty.Register("gLineBrush", typeof(SolidColorBrush), typeof(FFTDrawer),
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

        private static SolidColorBrush DefaultLineBrushII = Brushes.Blue;
        public SolidColorBrush gLineBrushII
        {
            get { return (SolidColorBrush)GetValue(gLineBrushIIProperty); }
            set { SetValue(gLineBrushIIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gLineBrushII.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty gLineBrushIIProperty =
            DependencyProperty.Register("gLineBrushII", typeof(SolidColorBrush), typeof(FFTDrawer),
                 new FrameworkPropertyMetadata(DefaultLineBrushII, FrameworkPropertyMetadataOptions.None, onLineBrushIIChange
            ));
        private static void onLineBrushIIChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as FFTDrawer;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.gLineBrushII = (SolidColorBrush)args.NewValue;
                mcontrol.Redraw();
            }

        }

        //new PropertyMetadata(0));

        /// <summary>
        /// borderThick
        /// </summary>
        private const double DefaultBorderThick = 0.5;
        public double BorderThick
        {
            get { return (double)GetValue(BorderThickProperty); }
            set { SetValue(BorderThickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for borderThick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThickProperty =
            DependencyProperty.Register("BorderThick", typeof(double), typeof(FFTDrawer),
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
        private SolidColorBrush txtBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        protected void drawGrid(DrawingContext cvs)
        {
            double anx = this.ActualWidth;
            double any = this.ActualHeight;
            double delta, yy;
            delta = (FFTConstaint.FYMax - FFTConstaint.FYMin) / (FFTConstaint.FNumHorzGrid + 1);

           cvs.DrawLine(new Pen(GridHeaderBrush, 1), new Point(0, 0), new Point(anx, 0));
           
            for (int i = 0; i < FFTConstaint.FNumHorzGrid; i++)   //
            {

                yy = logValue_ToFFTScreenY(FFTConstaint.FYMin + delta * (i + 1));
                if (i != (FFTConstaint.FNumHorzGrid - 1) / 2)
                {
                    cvs.DrawLine(new Pen(GridBrush, 1), new Point(0, yy), new Point(anx, yy));
                }
                FormattedText texte = new FormattedText(FFTConstaint.strGridGainTable[i], CIR, FlowDirection.LeftToRight,
                new Typeface("Verdana"), 10, txtBrush);
                cvs.DrawText(texte, new Point(0 - 22, yy - 5));

            }
            //draw middle
            double midle = logValue_ToFFTScreenY(FFTConstaint.FYMin + delta * 3);
            cvs.DrawLine(new Pen(GridBrush, 1), new Point(0, midle), new Point(anx, midle));
            for (int i = 0; i < FFTConstaint.GridFreqTable.Length; i++) //for test the coordinate stand   FFTConstaint.BZPlotBNum
            {
                double xx = freqToLogPosX(FFTConstaint.GridFreqTable[i]);

                double kpos = logPosXToFFTScreenPos(xx);
                //   Debug.WriteLine("xx zuobiao is :  {0}   kpos is :{1} with  freqindex : {2}  freq {3}", xx, kpos, FFTConstaint.GridFreqTable[i], i);
                cvs.DrawLine(new Pen(GridBrush, 1), new Point(kpos, 0), new Point(kpos, any));

                FormattedText texte = new FormattedText(FFTConstaint.strGridFreqTable[i], CIR, FlowDirection.LeftToRight,
                 new Typeface("Verdana"), 10, txtBrush);
                cvs.DrawText(texte, new Point(kpos - 6, any));


            }

            cvs.DrawLine(new Pen(GridHeaderBrush, 1), new Point(0, any), new Point(anx, any));

        }

        /// <summary>
        /// mouse Event below
        /// </summary>
        public delegate void fftDrawMouseDown(Point px, MouseButtonEventArgs e);
        public event fftDrawMouseDown onFFTMouseDownEvent;
        //
        public delegate void fftDrawMouseMove(int freqindex, int gaindex, int qindex, int pkindex, MouseEventArgs e);
        public event fftDrawMouseMove onFFTMouseMoveEvent;

        public delegate void fftDrawMouseUp(Point px, MouseButtonEventArgs e);
        public event fftDrawMouseUp onFFTMouseUpEvent;



        private const double DefaultCaptureRadius = 6.0;
        public double CaptureRadius
        {
            get { return (double)GetValue(CaptureRadiusProperty); }
            set { SetValue(CaptureRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptureRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptureRadiusProperty =
            DependencyProperty.Register("CaptureRadius", typeof(double), typeof(FFTDrawer), new PropertyMetadata(DefaultCaptureRadius));

        private int PKMouSelectingQindex = -1;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            //   QMoveOrignal = 0;
            isTouched = true;
            int sa = 0;
            int sb = 0;
            double diameter = DefaultBlockDiameter;
            if (isSupportMutiLine && activeLine == FFT_LineType.LineII)
            {
                sa = FFTConstaint.CEQ_MAX;
                sb = FFTConstaint.CEQ_MAX * 2;
            }
            else
            {
                sa = 0;
                sb = FFTConstaint.CEQ_MAX;
            }
            for (int i = sa; i < sb; i++)
            {
                double px = getBlockPosX(i);
                double py = getBlockPosY(i);
                Point epx = e.GetPosition(this);
                if ((Math.Abs(px - epx.X) <= CaptureRadius) && (Math.Abs(py - epx.Y) <= CaptureRadius))
                {
                    Mouse.SetCursor(Cursors.Hand);
                    PKMouseIndex = (i >= FFTConstaint.CEQ_MAX ? i - FFTConstaint.CEQ_MAX : i);

                    if (PKMouseIndex >= 0 && PKMouseIndex < FFTConstaint.CEQ_MAX - 2)
                    {
                        activeSelectBlockIndex = PKMouseIndex;    //0..9  for q block active select ,defaut is 0
                        //  PKMouSelectingQindex = -1;
                    }

                    //Debug.WriteLine("mouseDown here....search index is : {0}", PKindex);
                    if (onFFTMouseDownEvent != null)
                    {
                        onFFTMouseDownEvent(e.GetPosition(this), e);
                    }
                    break;
                }

                double qvalue = QShiftRatio * qIndexTemp[i];
                double leftQCenterX = px - diameter / 2 - Qdistance - diameter / 2 - qvalue;
                double rightQCenterX = px + diameter / 2 + Qdistance + diameter / 2 + qvalue;
                if ((Math.Abs(leftQCenterX - epx.X) <= CaptureRadius) && (Math.Abs(py - epx.Y) <= CaptureRadius))
                {
                    QMoveOrignal = FFTConstaint.QMoveLeft;//left qvalue
                    PKMouSelectingQindex = (i >= FFTConstaint.CEQ_MAX ? i - FFTConstaint.CEQ_MAX : i); //0..9
                    Debug.WriteLine("eq qvalue detect index is  {0} qmove orignal is {1}  ", PKMouSelectingQindex, QMoveOrignal);
                    break;
                }
                else if ((Math.Abs(rightQCenterX - epx.X) <= CaptureRadius) && (Math.Abs(py - epx.Y) <= CaptureRadius))
                {
                    QMoveOrignal = FFTConstaint.QMoveRight;//right qvalue
                    PKMouSelectingQindex = (i >= FFTConstaint.CEQ_MAX ? i - FFTConstaint.CEQ_MAX : i); //0..9
                    //  PKMouseIndex = -1;
                    Debug.WriteLine("eq qvalue detect index is  {0} qmove orignal is {1}  ", PKMouSelectingQindex, QMoveOrignal);
                    Mouse.SetCursor(Cursors.Hand);
                    break;
                }

            }
        }
        private int QMoveOrignal = 0;

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            isTouched = false;
            PKMouseIndex = -1;
            PKMouSelectingQindex = -1;
            Mouse.SetCursor(Cursors.None);
            //  Debug.WriteLine("on mouse up...............");
            if (onFFTMouseUpEvent != null)
            {
                onFFTMouseUpEvent(e.GetPosition(this), e);
                Redraw();
            }

            // shift = 0;
            // redraw();
        }




        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isTouched || (PKMouseIndex == -1 && PKMouSelectingQindex == -1))
            {
                #region detect the active mainblock and switch it case when is not touched.
                int sa = 0;
                int sb = 0;
                if (isSupportMutiLine && activeLine == FFT_LineType.LineII)
                {
                    sa = FFTConstaint.CEQ_MAX;
                    sb = FFTConstaint.CEQ_MAX * 2;
                }
                else
                {
                    sa = 0;
                    sb = FFTConstaint.CEQ_MAX;
                }
                for (int i = sa; i < sb; i++)
                {
                    double px = getBlockPosX(i);
                    double py = getBlockPosY(i);
                    Point fepx = e.GetPosition(this);
                    if ((Math.Abs(px - fepx.X) <= CaptureRadius) && (Math.Abs(py - fepx.Y) <= CaptureRadius))
                    {
                        // Mouse.SetCursor(Cursors.Hand);
                        int tmpMouseIndex = (i >= FFTConstaint.CEQ_MAX ? i - FFTConstaint.CEQ_MAX : i);

                        if (tmpMouseIndex >= 0 && tmpMouseIndex < FFTConstaint.CEQ_MAX - 2)
                        {
                            activeSelectBlockIndex = tmpMouseIndex;
                            Redraw();
                        }
                        break;
                    }

                }
                #endregion
                return;
            }

            if (e.LeftButton == MouseButtonState.Released)
            {
                isTouched = false;
                PKMouseIndex = -1;
                PKMouSelectingQindex = -1;

                Mouse.SetCursor(Cursors.None);
                return;
            }

            #region begin move the mainblock in condition

            Mouse.SetCursor(Cursors.Hand);
            Point epx = e.GetPosition(this);
            double mx = epx.X;
            double my = epx.Y;

            if (PKMouseIndex >= 0)
            {
                double freq = fftScreenPosXToFreq(mx);
                double gMaster = 0;
                if (isSupportMutiLine && activeLine == FFT_LineType.LineII)
                    gMaster = masterGainII;
                else
                    gMaster = masterGainI;

                double gain = fftScreenY_ToLogValueY(my) - gMaster;
                int freqindex = HalfSearcher.search(FFTConstaint.FreqTable, FFTConstaint.FreqTable.Length, freq);
                int gaindex = HalfSearcher.search(FFTConstaint.EqGain, FFTConstaint.EqGain.Length, gain);

                if (freqindex >= 0 && freqindex < FFTConstaint.FreqTable.Length && gaindex >= 0 && gaindex < FFTConstaint.EqGain.Length)
                {
                    if (isHasHLPF && (PKMouseIndex == FFTConstaint.CEQ_MAX - 2 || PKMouseIndex == FFTConstaint.CEQ_MAX - 1))
                        gaindex = FFTConstaint.EqGain.Length / 2;

                    if (onFFTMouseMoveEvent != null)
                    {

                        onFFTMouseMoveEvent(freqindex, gaindex, FFTConstaint.NONQvalue, PKMouseIndex, e);
                    }

                    //  Debug.WriteLine("on mouse move now..............");

                }
            }
            #endregion
            //left q or right q block is selected 
            if (PKMouSelectingQindex >= 0)
            {
                int eindex = PKMouSelectingQindex;
                if (activeLine == FFT_LineType.LineII)
                    eindex += FFTConstaint.CEQ_MAX;

                double px = getBlockPosX(eindex);
                double py = getBlockPosY(eindex);

                double currentCentQX = 0;

                Debug.WriteLine("qvalue mouse move index is ---------------: {0} ", eindex);
                if (QMoveOrignal == FFTConstaint.QMoveLeft || QMoveOrignal == FFTConstaint.QMoveRight)
                {
                    if (QMoveOrignal == FFTConstaint.QMoveLeft)
                        currentCentQX = px - BlockDiameter - Qdistance;
                    else
                        currentCentQX = px + BlockDiameter + Qdistance;
                    //caculate the distance between the Qvalue center block x and current mouse x point
                    double deltaQ = (mx - currentCentQX);
                    // if (deltaQ >= 0)
                    {
                        double movQ = (double)deltaQ / QShiftRatio;

                        if (QMoveOrignal == FFTConstaint.QMoveLeft && movQ > 0)
                            movQ = 0;
                        else if (QMoveOrignal == FFTConstaint.QMoveRight && movQ < 0)
                            movQ = 0;
                        //  Debug.WriteLine("qmove orignal is : {0}  deltaQ is  {1}  movQ index is  {2} eqindex :{3}",QMoveOrignal,deltaQ, movQ,eindex);

                        movQ = Math.Abs(Math.Round(movQ));
                        int tmpQindex = limitQvalue((int)movQ);
                        //    Debug.WriteLine("move qvalue is : {0} tmpqindex is :{1}  currentCentQX {2}", movQ, tmpQindex, currentCentQX);
                        if (onFFTMouseMoveEvent != null)
                        {
                            onFFTMouseMoveEvent(FFTConstaint.NONFreq, FFTConstaint.NONGain, tmpQindex, PKMouSelectingQindex, e);
                        }

                    }

                }


            }




        }
        private const int MaxQvalue = 100;

        public int limitQvalue(int newvalue)
        {
            return Math.Max(0, Math.Min(MaxQvalue, newvalue)); //very good
        }


        private const double DefaultGLineWidth = 2.8;
        public double gLineWidth
        {
            get { return (double)GetValue(gLineWidthProperty); }
            set { SetValue(gLineWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for gLineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty gLineWidthProperty =
            DependencyProperty.Register("gLineWidth", typeof(double), typeof(FFTDrawer), new PropertyMetadata(DefaultGLineWidth));

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


        private void drawGlobalLineOld(DrawingContext dc, int chindex)
        {
            Point p1 = new Point();
            Point p2 = new Point();
            Pen mPen = null;
            double gMaster = 0;
            if (chindex == 0)
            {
                mPen = new Pen(gLineBrush, gLineWidth);
                gMaster = masterGainI;

            }
            else
            {
                mPen = new Pen(gLineBrushII, gLineWidth);
                gMaster = masterGainII;
            }
            if(mPen!=null)
            mPen.Freeze();

            int i = 0;
            for (i = 0; i < FFTConstaint.NFFT - 1; i++)
            {

                p1.X = logPosXToFFTScreenPos(i);
                if (p1.X >= 0)
                {
                    //   Debug.WriteLine("point 1 x is :{0}  i is :{1}", p1.X, i);
                    double tmp = logValue_ToFFTScreenY(PointArray[chindex][i] + gMaster);
                    if (tmp <= this.ActualHeight)
                        p1.Y = tmp;
                    //  if (p1.Y > this.ActualHeight)
                    //  p1.Y = this.ActualHeight;
                    //   mPen = new Pen(Brushes.Transparent, gLineWidth);
                    //  else
                    //  mPen = mPen; //new Pen(gLineBrush, gLineWidth);

                    //   p1.Y = this.ActualHeight;

                    p2.X = logPosXToFFTScreenPos(i + 1);
                    if (p2.X <= ActualWidth)
                    {

                        tmp = logValue_ToFFTScreenY(PointArray[chindex][i + 1] + gMaster);
                        if (tmp <= this.ActualHeight)
                            p2.Y = tmp;
                        //if (p2.Y > this.ActualHeight)
                        //    mPen = new Pen(Brushes.Transparent, gLineWidth);
                        // if (p2.Y > this.ActualHeight)
                        //  p2.Y = this.ActualHeight;
                        // else
                        //mPen = new Pen(gLineBrush, gLineWidth);
                    }
                    //  else
                    //  break;
                    dc.DrawLine(mPen, p1, p2);
                }

            }
            //dc.DrawLine()
        }

        public bool isPointInRect(Point pt)
        {

            return (pt.X >= 0 && pt.X <= this.ActualWidth && pt.Y >= 0 && pt.Y <= this.ActualHeight);

        }
        /// <summary>
        /// drawGlobalLine,note in wpf draw with DrawingContext can draw outside line (left or right end)
        /// </summary>
        /// <param name="dc"></param>
        private void drawGlobalLine(DrawingContext dc, int chindex)
        {

            Point p1 = new Point();
            Point p2 = new Point();
            Pen mPen = null;
            double gMaster = 0;
            if (chindex == 0)
            {
                mPen = new Pen(gLineBrush, gLineWidth);
                gMaster = masterGainI;

            }
            else
            {
                mPen = new Pen(gLineBrushII, gLineWidth);
                gMaster = masterGainII;
            }
            if (mPen != null)
                mPen.Freeze();
            int i = 0;
            for (i = 0; i < FFTConstaint.NFFT - 1; i++)
            {

                p1.X = logPosXToFFTScreenPos(i);
                //   Debug.WriteLine("point 1 x is :{0}  i is :{1}", p1.X, i);
                p1.Y = logValue_ToFFTScreenY(PointArray[chindex][i] + gMaster);

                p2.X = logPosXToFFTScreenPos(i + 1);
                p2.Y = logValue_ToFFTScreenY(PointArray[chindex][i + 1] + gMaster);
                if (isPointInRect(p1) && isPointInRect(p2))
                    dc.DrawLine(mPen, p1, p2);


            }

        }

        public void drawWave(DrawingContext dc, int eindex, int chindex)
        {

            double[] tmpAray = new double[FFTConstaint.NFFT]; //+ 2
            double gMaster = 0;
            if (chindex == 0)
            {
                Array.Copy(curvePointArray[eindex], 0, tmpAray, 0, FFTConstaint.NFFT);
                gMaster = masterGainI;
            }

            else
            {
                Array.Copy(curvePointArray[eindex + FFTConstaint.CEQ_MAX], 0, tmpAray, 0, FFTConstaint.NFFT);
                gMaster = masterGainII;
            }

            //tmpAray[FFTConstaint.NFFT+1]=    

            PathFigure pf = new PathFigure();
            for (int i = 0; i < FFTConstaint.NFFT; i++)
            {
                if (logPosXToFFTScreenPos(i) >= 0 && logPosXToFFTScreenPos(i) <= this.ActualWidth)
                {
                    Point pt = new Point(logPosXToFFTScreenPos(i), logValue_ToFFTScreenY(tmpAray[i] + gMaster));
                    if (pt.Y > this.ActualHeight)
                        pt.Y = this.ActualHeight;

                    PathSegment ps = new LineSegment(pt, true);
                    pf.Segments.Add(ps);
                }
            }

            Point pt1 = new Point(this.ActualWidth, logValue_ToFFTScreenY(0 + gMaster));
            if (pt1.Y > this.ActualHeight)
                pt1.Y = this.ActualHeight;
            PathSegment ps1 = new LineSegment(pt1, true);
            pf.Segments.Add(ps1);
            //point2
            Point pt2 = new Point(0, logValue_ToFFTScreenY(0 + gMaster));
            if (pt2.Y > this.ActualHeight)
                pt2.Y = this.ActualHeight;

            PathSegment ps2 = new LineSegment(pt2, true);
            pf.Segments.Add(ps2);
            Geometry g = new PathGeometry(new[] { pf });

            dc.DrawGeometry(new SolidColorBrush(curveColor[eindex]), null, g);



        }

        private Rect combRect(int chindex)
        {
            double gMaster = 0;
            if (chindex == 0)
                gMaster = masterGainI;
            else
                gMaster = masterGainII;
            Point pt0 = new Point(0, this.ActualHeight / 2 - gMaster);
            //  Point pt1 = new Point(0, this.ActualHeight / 2 + gMaster);
            Point pt2 = new Point(this.ActualWidth, this.ActualHeight / 2 - gMaster * 4);
            //Point pt3 = new Point(this.ActualWidth, this.ActualHeight / 2);
            return (new Rect(pt0, pt2));
        }


        public int CEQNumber
        {
            get
            {
                return (isHasHLPF ? FFTConstaint.CEQ_MAX : FFTConstaint.CEQ_MAX - 2);
            }


        }

        public void drawAllPlots(DrawingContext dc)
        {
            drawGlobalLine(dc, 0);
            if (isSupportMutiLine)
                drawGlobalLine(dc, 1);
            if (isDrawWave)
            {
                for (int i = 0; i < CEQNumber; i++) //FFTConstaint.CEQ_MAX/2               
                    drawWave(dc, i, 0);
            }
            for (int i = 0; i < CEQNumber; i++) //FFTConstaint.CEQ_MAX/2                       
                drawSquareMainBlock_withUpText(dc, i, 0);

            // drawSquareQBlock_withLine(dc, activeSelectBlockIndex, 0);
            if (isSupportMutiLine)
            {
                if (isDrawWave)
                {
                    for (int i = 0; i < CEQNumber; i++) //FFTConstaint.CEQ_MAX/2              
                        drawWave(dc, i, 1);
                }

                for (int i = 0; i < CEQNumber; i++) //FFTConstaint.CEQ_MAX/2
                {
                    drawSquareMainBlock_withUpText(dc, i, 1);
                }

            }

            int chindex = (int)activeLine; //0 or 1            
            drawSquareQBlock_withLine(dc, activeSelectBlockIndex, chindex);

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
            dc.DrawRectangle(BackBrush, new Pen(penBrush, BorderThick), new Rect(0, 0, wd, ht));
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
            double gMastr = masterGainI;
            if (eindex >= FFTConstaint.CEQ_MAX)
                gMastr = this.masterGainII;
            return logValue_ToFFTScreenY(GainTemp[eindex] + gMastr);
        }
        #endregion



















        //  protected double screenx


    }



}
