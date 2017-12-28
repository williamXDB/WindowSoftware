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

namespace CLinerStyle
{
    /// <summary>
    /// Cs_Slider.xaml 的互動邏輯
    /// </summary>
    public partial class Cs_Slider : UserControl
    {
        public double thumbHeight = 36;
        public double thumbWidth = 20;
        public static int DefaultMaximun = 100;
        public static int DefaultMinimun = 0;
        public static int DefaultValueNum = 0;
        public static double Cs_SliderHeight;
        public Cs_Slider()
        {
            InitializeComponent();
            thumb.Height = thumbHeight;
            thumb.Width = thumbWidth;
        }
        public int Maximun
        {
            get
            {
                return (int)this.GetValue(Cs_Slider.MaximunProperty);
            }
            set
            {
                this.SetValue(Cs_Slider.MaximunProperty, (object)value);
            }
        }
        public static readonly DependencyProperty MaximunProperty =
            DependencyProperty.Register("Maximun", typeof(int), typeof(Cs_Slider), new PropertyMetadata(DefaultMaximun, null));

        public int Minimun
        {
            get
            {
                return (int)this.GetValue(Cs_Slider.MinimunProperty);
            }
            set
            {
                this.SetValue(Cs_Slider.MinimunProperty, (object)value);
            }
        }
        public static readonly DependencyProperty MinimunProperty =
            DependencyProperty.Register("Minimun", typeof(int), typeof(Cs_Slider), new PropertyMetadata(DefaultMinimun, null));



        public static readonly DependencyProperty thumbProperty = DependencyProperty.Register("imagethumb", typeof(ImageSource), typeof(Cs_Slider), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Cs_Slider.onThumbChange)));

        public ImageSource imagethumb
        {
            get
            {
                return (ImageSource)this.GetValue(Cs_Slider.thumbProperty);
            }
            set
            {
                this.SetValue(Cs_Slider.thumbProperty, (object)value);
            }
        }
        private static void onThumbChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Cs_Slider Cs_Slider = obj as Cs_Slider;
            if (Cs_Slider == null)
                return;

            //Cs_Slider.recaculateThumb();
            //Cs_Slider.recaculate();
            Cs_Slider.InvalidateVisual();
        }

        public delegate void Cs_SliderValueChanged(Object sender, int value);
        public event Cs_SliderValueChanged OnKaCs_SliderValueChanged;
        public int ValueNum
        {
            get
            {
                return (int)this.GetValue(ValueChangedPropertyProperty);
            }
            set
            {
                this.SetValue(ValueChangedPropertyProperty, (object)value);
            }
        }
        //private static readonly DependencyProperty ValueChangedPropertyProperty = DependencyProperty.Register("ValueNum", typeof(int), typeof(Cs_Slider), (PropertyMetadata)new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty ValueChangedPropertyProperty =
           DependencyProperty.Register("ValueNum", typeof(int), typeof(Cs_Slider), new PropertyMetadata(DefaultValueNum, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //int newValue = (int)e.NewValue;
            Cs_Slider ks = d as Cs_Slider;
            //ks.thumb.Margin = new Thickness(0, (ks.ValueNum - ks.Minimun) * Cs_SliderHeight / (ks.Maximun - ks.Minimun), 0, 0);
            ks.InvalidateVisual();
        }
        SolidColorBrush BRUSHES = new SolidColorBrush(Color.FromArgb(0xFF, 0x6B, 0x6B, 0x6B));

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double actualWidth = this.ActualWidth;
            double actualHeight = this.ActualHeight;
            //Pen pen2 = new Pen(Brushes.Black, 2);
            //pen2.Freeze();
            //dc.DrawRoundedRectangle(null, pen2, new Rect(this.ActualWidth / 2 - 4, 19, 8, this.ActualHeight - 38), 3, 3);

            //Pen pen3 = new Pen(Brushes.Black, 2);
            //pen3.Freeze();
            //dc.DrawRoundedRectangle(null, pen3, new Rect(this.ActualWidth / 2 - 6, 17, 12, this.ActualHeight - 34), 3, 3);

            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.EndPoint = new Point(1, 0.5);
            lgb.StartPoint = new Point(0, 0.5);
            GradientStop gs = new GradientStop();
            gs.Color = Color.FromArgb(0xff, 0x78, 0x78, 0x78);
            gs.Offset = 0;
            //GradientStop gs3 = new GradientStop();
            //gs3.Color = Color.FromArgb(0xff, 0x3E, 0x3E, 0x3E);
            //gs3.Offset = 1;
            GradientStop gs2 = new GradientStop();
            gs2.Color = Color.FromArgb(0xff, 0x29, 0x29, 0x29);
            gs2.Offset = 0.6;
            lgb.GradientStops.Add(gs);
            lgb.GradientStops.Add(gs2);
            //lgb.GradientStops.Add(gs3);
            Pen pen = new Pen(BRUSHES, 2);
            pen.Freeze();
            dc.DrawRoundedRectangle(lgb, null, new Rect(this.ActualWidth / 2 - 5, 18, 10, this.ActualHeight - 36), 3, 3);
            //动态更改长度后重新计算长度
            Cs_SliderHeight = this.ActualHeight - 36;
            this.thumb.Margin = new Thickness(0, (this.ValueNum - this.Minimun) * Cs_SliderHeight / (this.Maximun - this.Minimun), 0, 0);

        }

        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            double top = thumb.Margin.Top + e.VerticalChange;
            if (top < 0) top = 0;
            else if (top > Cs_SliderHeight) top = Cs_SliderHeight;
            ValueNum = (int)((top / Cs_SliderHeight) * (Maximun - Minimun) + Minimun);
            if (OnKaCs_SliderValueChanged != null) //no by pass
            {
                OnKaCs_SliderValueChanged(this, ValueNum);
            }

        }

    }
}


