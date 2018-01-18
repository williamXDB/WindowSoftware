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

namespace Lib.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LineArrowDemo"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LineArrowDemo;assembly=LineArrowDemo"
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
    ///     <MyNamespace:CLineArrow/>
    ///
    /// </summary>
    /// 



    public enum ArowType
    {
        Arow_Left = 98,
        Arow_Right = 99
    }

    public class CLineArrow : Control
    {

        #region property define begin

        private const int DefaultArrowLen = 18;
        public int ArrowLength
        {
            get { return (int)GetValue(ArrowLengthProperty); }
            set { SetValue(ArrowLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowLengthProperty =
            DependencyProperty.Register("ArrowLength", typeof(int), typeof(CLineArrow),

           new FrameworkPropertyMetadata(DefaultArrowLen, FrameworkPropertyMetadataOptions.None, onArrowLenChange
                ));

        private static void onArrowLenChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLineArrow;
            if (mcontrol != null)
            {
                mcontrol.ArrowLength= (int)args.NewValue;               
                mcontrol.redraw();
            }
        }


        private const double DefaultRowAngle = 30;
        public double RrowAngle
        {
            get { return (double)GetValue(RrowAngleProperty); }
            set { SetValue(RrowAngleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for RrowAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RrowAngleProperty =
            DependencyProperty.Register("RrowAngle", typeof(double), typeof(CLineArrow),

           new FrameworkPropertyMetadata(DefaultRowAngle, FrameworkPropertyMetadataOptions.None, onArrowAngleChange
                ));

        private static void onArrowAngleChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLineArrow;
            if (mcontrol != null)
            {
                mcontrol.RrowAngle = (double)args.NewValue;               
                mcontrol.redraw();
            }
        }


        private const ArowType DefaultFlag = ArowType.Arow_Left;
        public ArowType ArowFlag
        {
            get { return (ArowType)GetValue(ArowFlagProperty); }
            set { SetValue(ArowFlagProperty, value); }
        }
        // Using a DependencyProperty as the backing store for  ArowFlag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArowFlagProperty =
            DependencyProperty.Register("ArowFlag", typeof(ArowType), typeof(CLineArrow),

         new FrameworkPropertyMetadata(DefaultFlag, FrameworkPropertyMetadataOptions.None, onArrowTypeChange
                ));

        private static void onArrowTypeChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLineArrow;
            if (mcontrol != null)
            {
                mcontrol.ArowFlag = (ArowType)args.NewValue;               
                mcontrol.redraw();
            }
        }


        private static SolidColorBrush DefaultBorderBrush = Brushes.Black;

        public SolidColorBrush ArowBorderBrush
        {
            get { return (SolidColorBrush)GetValue(ArowBorderBrushProperty); }
            set { SetValue(ArowBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArowBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArowBorderBrushProperty =
            DependencyProperty.Register("ArowBorderBrush", typeof(SolidColorBrush), typeof(CLineArrow),

             new FrameworkPropertyMetadata(DefaultFillBrush, FrameworkPropertyMetadataOptions.None, onBorderBrushChange
                ));

        private static void onBorderBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLineArrow;
            if (mcontrol != null)
            {
                mcontrol.ArowBorderBrush = (SolidColorBrush)args.NewValue;
                mcontrol.redraw();
            }
        }

        private static SolidColorBrush DefaultFillBrush = Brushes.Black;
        public SolidColorBrush ArowFilBrush
        {
            get { return (SolidColorBrush)GetValue(ArowFilBrushProperty); }
            set { SetValue(ArowFilBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArowFilBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArowFilBrushProperty =
            DependencyProperty.Register("ArowFilBrush", typeof(SolidColorBrush), typeof(CLineArrow), 
          new FrameworkPropertyMetadata(DefaultFillBrush, FrameworkPropertyMetadataOptions.None, onBackBrushChange
                ));

        private static void onBackBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CLineArrow;
            if (mcontrol != null)
            {
                mcontrol.ArowFilBrush = (SolidColorBrush)args.NewValue;
                mcontrol.redraw();
            }


        }
        #endregion

        static CLineArrow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CLineArrow), new FrameworkPropertyMetadata(typeof(CLineArrow)));
        }


        public void redraw()
        {
            this.InvalidateVisual();
        }


        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double wd = ActualWidth;
            double ht = ActualHeight;
            //dc.DrawRectangle(BackBrush, new Pen(penBrush, borderThick), new Rect(0, 0, wd, ht));

            int ax = ArrowLength;
            double kb = ((double)RrowAngle / 180) * Math.PI;

            kb = Math.Tan(kb);

            Point[] pt = new Point[3];
            if (ArowFlag == ArowType.Arow_Left)
            {
                pt[0].X = 0;
                pt[1].X = ax;
            }
            else if (ArowFlag == ArowType.Arow_Right)
            {
                pt[0].X = wd;
                pt[1].X = pt[0].X - ax;
            }
        
            pt[0].Y = ht / 2;
            pt[1].Y = ht / 2 - kb * ax;

            pt[2].X = pt[1].X;
            pt[2].Y = ht - pt[1].Y;



            #region draw triangle 
            StreamGeometry streamGtry = new StreamGeometry();

            using (StreamGeometryContext gemContext = streamGtry.Open())
            {
                gemContext.BeginFigure(pt[0], true, true);
                PointCollection poins = new PointCollection { pt[1], pt[2] };
                gemContext.PolyLineTo(poins, true, true);
            }
            if(streamGtry!=null)
            streamGtry.Freeze();
            dc.DrawGeometry(ArowFilBrush, new Pen(ArowBorderBrush, 1), streamGtry);
            #endregion

            if (ArowFlag == ArowType.Arow_Left)
            {
                dc.DrawLine(new Pen(ArowFilBrush, 1), new Point(pt[1].X, pt[0].Y), new Point(wd, pt[0].Y));

            }
            else if (ArowFlag == ArowType.Arow_Right)
            {
                dc.DrawLine(new Pen(ArowFilBrush, 1), new Point(pt[1].X, pt[0].Y), new Point(0, pt[0].Y));
            }      
           

        }
    }
}
