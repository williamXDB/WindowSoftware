using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Globalization;
namespace Lib.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MatrixSystemEditor"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MatrixSystemEditor;assembly=MatrixSystemEditor"
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
    ///     <MyNamespace:CLimitGDSlider/>
    ///
    /// </summary>
    public enum GDSliderType
    {
        GDSLIDER_Gate = 20,
        GDSLIDER_Exp = 21,
        GDSLIDER_Dyn = 22,
    };

    public class CLimitGDSlider : CommControl
    {
        public static double[] dynExpRatioTable =
       {

 1.0,     //0
 1.2,     //1
 1.3,     //2
 1.5,     //3
 1.7,     //4
 2.0,     //5
 2.2,     //6
 2.3,     //6
 2.5,    //7
 3.0,     //9
 3.5,     //10
 4.0,     //11
 4.5,     //12
 5.0,     //13
 5.5,     //14
 5.6,     //15
 5.9,     //16
 6.0,     //17
 6.2,     //18
 6.5,     //19
 7.0,     //20
 7.2,     //21
 7.4,     //22
 7.8,     //23
 8.0,     //19

};

        public static double[] dynRatioTable =
 {
 1.0,     //0
 1.2,     //1
 1.3,     //2
 1.5,     //3
 1.7,     //4
 2.0,     //5
 2.2,     //6
 2.3,     //6
 2.5,    //7
 3.0,     //9
 3.5,     //10
 4.0,     //11
 4.5,     //12
 5.0,     //13
 5.5,     //14
 6.0,     //15
 6.5,     //16
 7.0,     //17
 7.5,     //18
 8.0,     //19
 8.5,     //20
 9.0,     //21
 9.5,     //22
 10.0,    //23
 20.0,    //24
 };

        static CLimitGDSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CLimitGDSlider), new FrameworkPropertyMetadata(typeof(CLimitGDSlider)));
        }

        private static System.Windows.Point[] dPoints = new Point[3]; //double type 

        //   


        private const int DefaultGDRatioMax = 24;
        public int dp_gdRatioMax
        {
            get { return (int)GetValue(dp_gdRatioMaxProperty); }
            set { SetValue(dp_gdRatioMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for dp_gdRatioMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdRatioMaxProperty =
            DependencyProperty.Register("dp_gdRatioMax", typeof(int), typeof(CLimitGDSlider), new PropertyMetadata(DefaultGDRatioMax));




        private const int DefaultGDRatioPos = 0;
        public int dp_gdRatioPos
        {
            get { return (int)GetValue(dp_gdRatioPosProperty); }
            set { SetValue(dp_gdRatioPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for dp_gdRatioPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdRatioPosProperty =
            DependencyProperty.Register("dp_gdRatioPos", typeof(int), typeof(CLimitGDSlider),
            new PropertyMetadata(DefaultGDRatioPos));


        //   

        private const int DefaultGDThreshold = 0;
        public int dp_gdThresholdPos
        {
            get { return (int)GetValue(dp_gdThresholdPosProperty); }
            set { SetValue(dp_gdThresholdPosProperty, value); }
        }
        // Using a DependencyProperty as the backing store for dp_dgThresholdPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdThresholdPosProperty =
            DependencyProperty.Register("dp_gdThresholdPos", typeof(int), typeof(CLimitGDSlider), new PropertyMetadata(DefaultGDThreshold));

#if _KMD
        new FrameworkPropertyMetadata(DefaultGDThreshold, FrameworkPropertyMetadataOptions.None, onThresholdChange
            ));

        private static void onThresholdChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLimitGDSlider;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.dp_gdThresholdPos = (int)args.NewValue;
                mcontrol.Redraw();
                
            }
        }
#endif



        //


        private const int DefaultGDThresdMax = 50;
        public int dp_gdThresholdMax
        {
            get { return (int)GetValue(dp_gdThresholdMaxProperty); }
            set { SetValue(dp_gdThresholdMaxProperty, value); }
        }
        // Using a DependencyProperty as the backing store for dp_gdThresholdMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdThresholdMaxProperty =
            DependencyProperty.Register("dp_gdThresholdMax", typeof(int), typeof(CLimitGDSlider), new PropertyMetadata(DefaultGDThresdMax));




        private const int DefaultGDGainMax = 24;
        public int dp_gdGainMax
        {
            get { return (int)GetValue(dp_gdGainMaxProperty); }
            set { SetValue(dp_gdGainMaxProperty, value); }
        }
        // Using a DependencyProperty as the backing store for dp_gdGainMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdGainMaxProperty =
            DependencyProperty.Register("dp_gdGainMax", typeof(int), typeof(CLimitGDSlider), new PropertyMetadata(DefaultGDGainMax));



        //
        private const int DefaultGDGainPos = 0;
        public int dp_gdGainPos
        {
            get { return (int)GetValue(dp_gdGainPosProperty); }
            set { SetValue(dp_gdGainPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for dp_gdGainPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_gdGainPosProperty =
            DependencyProperty.Register("dp_gdGainPos", typeof(int), typeof(CLimitGDSlider),
            new PropertyMetadata(DefaultGDGainPos));

        private const GDSliderType DefaultSliderType = GDSliderType.GDSLIDER_Exp;

        public GDSliderType dp_GDSliderType
        {
            get { return (GDSliderType)GetValue(dp_GDSliderTypeProperty); }
            set { SetValue(dp_GDSliderTypeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for dp_GDSliderType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_GDSliderTypeProperty =
            DependencyProperty.Register("dp_GDSliderType", typeof(GDSliderType), typeof(CLimitGDSlider),
            new PropertyMetadata(DefaultSliderType));


        public CLimitGDSlider()
        {



        }

        /// <summary>
        /// transfor data 
        /// </summary>
        /// <param name="limData"></param>
        public void setLimitData_fresh(LimitEdit limData)
        {

            dp_gdRatioPos = limData.limit_ratio;
            dp_gdThresholdPos = limData.limit_threshold;
            this.Redraw();
        }

        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;
        /// <summary>
        /// flat to initial draw
        /// </summary>
        public void flatControl()
        {
            dp_gdGainPos = dp_gdRatioPos = dp_gdThresholdPos = 0;
            this.Redraw();
        }
        #region EXPGatemark
        public static string[] strLeftExpMark ={
        " 20"," 10"," 0","-10","-20","-30","-40","-50","-60","-70","-80"};

        public static string[] strDownExpMark ={
        "-70","-60","-50","-40","-30","-20","-10","  0","10","20"};
        #endregion
        //
        #region DynMark
        public static string[] strLeftDynMark ={
        " 20"," 12","  6","  0 ","-6","-12","-18","-24","-30"};

        public static string[] strDownDynMark ={
        "-24","-18"," -12","  -6","  0","   6","  12"," 20"  };
                                            

        #endregion


        private void drawMark(DrawingContext cvs)
        {
            string ftext = string.Empty;
            int i = 0;
            double fontsize = 9.0;
            double xleftOffset = -18;
            double yExpQOffset = 2;


            FormattedText texte = null;
            double leftExpStep = (double)this.ActualHeight / (strLeftExpMark.Length-1)-0.2;
            double bottomExpStep = (double)this.ActualWidth / strDownExpMark.Length;


            double leftDynStep = (double)this.ActualHeight / (strLeftDynMark.Length - 1) - 0.35;
            double bottomDynStep = (double)this.ActualWidth / strDownDynMark.Length;

            switch (dp_GDSliderType)
            {
                case GDSliderType.GDSLIDER_Gate:
                case GDSliderType.GDSLIDER_Exp:
                    {
                        for (i = 0; i < strLeftExpMark.Length; i++) //left mark
                        {
                            ftext = strLeftExpMark[i];
                           texte=new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                                new Typeface("Verdana"), fontsize, Brushes.White);                          
                       
                           cvs.DrawText(texte, new Point(xleftOffset, i * leftExpStep));

                        }

                        for (i = 0; i < strDownExpMark.Length; i++) //bottom mark
                        {
                            ftext = strDownExpMark[i];
                            texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                                 new Typeface("Verdana"), fontsize, Brushes.White);

                            cvs.DrawText(texte, new Point(bottomExpStep-8+i*bottomExpStep,this.ActualHeight+1));

                        }



                    }
                    break;
                case GDSliderType.GDSLIDER_Dyn:
                    {

                        for (i = 0; i < strLeftDynMark.Length; i++) //left mark
                        {
                            ftext = strLeftDynMark[i];
                            texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                                 new Typeface("Verdana"), fontsize, Brushes.White);

                            cvs.DrawText(texte, new Point(xleftOffset, i * leftDynStep));

                        }

                        for (i = 0; i < strDownDynMark.Length; i++) //bottom mark
                        {
                            ftext = strDownDynMark[i];
                            texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                                 new Typeface("Verdana"), fontsize, Brushes.White);

                            cvs.DrawText(texte, new Point(bottomDynStep - 8 + i * bottomDynStep, this.ActualHeight + 1));

                        }

                    }
                    break;

            }

        }

        /// <summary>
        /// paint below..
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double wd = this.ActualWidth;
            double ht = this.ActualHeight;
            dc.DrawRectangle(BackBrush, new Pen(), new Rect(0, 0, wd, ht));
            //
            double K = 0;
            double K1 = 0;
            #region switch code to condition
            switch (dp_GDSliderType)
            {
                case GDSliderType.GDSLIDER_Gate:
                    {


                    }
                    break;
                case GDSliderType.GDSLIDER_Exp:
                    {
                        K = (double)ht / wd;
                        K1 = (double)1.0 / dynExpRatioTable[dp_gdRatioPos];
                        double vrStep = ht / dp_gdThresholdMax;

                        double m = (dp_gdThresholdMax - dp_gdThresholdPos) * vrStep;
                        double dt = K1 * (ht - m);
                        dPoints[0].X = wd - dt - m / K;
                        dPoints[0].Y = ht;
                        dPoints[1].X = dPoints[0].X + dt;
                        dPoints[1].Y = m;

                        if (dp_gdThresholdPos == 0)
                        {
                            dPoints[0].X = dPoints[1].X = 0;
                            dPoints[0].Y = dPoints[1].Y = ht;
                        }
                        else if (dp_gdThresholdPos == dp_gdThresholdMax)
                        {
                            dPoints[1].X = wd;
                            dPoints[1].Y = 0;
                        }

                        if (dp_gdRatioPos == dp_gdRatioMax)
                        {
                            dPoints[0].X = dPoints[1].X;
                        }
                        dPoints[2].X = wd;
                        dPoints[2].Y = 0;

                    }
                    break;

                case GDSliderType.GDSLIDER_Dyn:
                    {

                        K = (double)ht / wd;
                        K1 = 1 / dynRatioTable[dp_gdRatioPos];
                        double vrStep = ht / dp_gdThresholdMax;
                        double h = (dp_gdThresholdMax - dp_gdThresholdPos) * vrStep;

                        dPoints[0].X = 0;
                        dPoints[0].Y = ht;

                        if (dp_gdThresholdPos == 0 && dp_gdRatioPos == 0)
                        {
                            dPoints[1].X = 0;
                            dPoints[1].Y = ht;
                            dPoints[2].X = wd;
                            dPoints[2].Y = 0;
                        }
                        else if (dp_gdRatioPos == 0 && dp_gdThresholdPos > 0 && dp_gdThresholdPos < dp_gdThresholdMax)
                        {

                            dPoints[1].X = wd - h / K;
                            dPoints[1].Y = h;

                            dPoints[2].X = wd;
                            dPoints[2].Y = 0;
                        }
                        else if (dp_gdRatioPos == 0 && (dp_gdThresholdPos == dp_gdThresholdMax))
                        {

                            dPoints[1].X = wd;
                            dPoints[1].Y = 0;
                            dPoints[2].X = wd;
                            dPoints[2].Y = 0;
                        }
                        else if (dp_gdRatioPos > 0 && dp_gdRatioPos < dp_gdRatioMax && dp_gdThresholdPos > 0 && dp_gdThresholdPos < dp_gdThresholdMax)
                        {

                            dPoints[1].X = (ht - h) / K;
                            dPoints[1].Y = h;
                            dPoints[2].X = wd;
                            dPoints[2].Y = h - K1 * (wd - dPoints[1].X);

                        }
                        else if (dp_gdRatioPos > 0 && dp_gdRatioPos < dp_gdRatioMax && dp_gdThresholdPos == dp_gdThresholdMax)
                        {

                            dPoints[1].X = wd;
                            dPoints[1].Y = 0;
                            dPoints[2].X = wd;
                            dPoints[2].Y = 0;

                        }
                        else if (dp_gdThresholdPos == 0 && dp_gdRatioPos > 0 && dp_gdRatioPos < dp_gdRatioMax)//thres=0 ratio!=0
                        {

                            dPoints[1].X = 0;
                            dPoints[1].Y = ht;
                            dPoints[2].X = wd;
                            dPoints[2].Y = ht - K1 * wd;

                        }
                        else if (dp_gdRatioPos == dp_gdRatioMax && dp_gdThresholdPos == dp_gdThresholdMax) //ratio max,thresMax
                        {
                            dPoints[1].X = wd;
                            dPoints[1].Y = 0;
                            dPoints[2].X = wd;
                            dPoints[2].Y = 0;
                        }
                        else if (dp_gdRatioPos == dp_gdRatioMax && dp_gdThresholdPos == 0) //ratio max
                        {
                            dPoints[1].X = wd;
                            dPoints[1].Y = ht;

                            dPoints[2].X = wd;
                            dPoints[2].Y = ht;

                        }
                        else if (dp_gdRatioPos == dp_gdRatioMax && dp_gdThresholdPos < dp_gdThresholdMax && dp_gdThresholdPos > 0)
                        {

                            dPoints[1].X = (ht - h) / K;
                            dPoints[1].Y = h;
                            dPoints[2].X = wd;
                            dPoints[2].Y = h - 2;

                        }



                    }
                    break;

            }
            #endregion
            if (dPoints[0].X < 0) dPoints[0].X = 0;

            dc.DrawLine(new Pen(penBrush, 1), dPoints[0], dPoints[1]);
            dc.DrawLine(new Pen(penBrush, 1), dPoints[1], dPoints[2]);
            // Debug.WriteLine("on render drawing now...................");

            drawMark(dc);

        }


        ///

        private static SolidColorBrush DefaultPen = Brushes.Yellow;
        public SolidColorBrush penBrush
        {
            get { return (SolidColorBrush)GetValue(penBrushProperty); }
            set { SetValue(penBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for penBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty penBrushProperty =
            DependencyProperty.Register("penBrush", typeof(SolidColorBrush), typeof(CLimitGDSlider),

          new FrameworkPropertyMetadata(DefaultPen, FrameworkPropertyMetadataOptions.None, onPenBrushChanged

              ));

        private static void onPenBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLimitGDSlider;
            if (mcontrol != null)
            {
                mcontrol.penBrush = (SolidColorBrush)args.NewValue;
                mcontrol.Redraw();
            }


        }














    }
}
