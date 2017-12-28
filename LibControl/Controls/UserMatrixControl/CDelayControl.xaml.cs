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

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for CDelayControl.xaml
    /// </summary>

    public partial class CDelayControl : UserControl
    {
        public const double DELAY_CONST = 0.0208333333;
        public const double DELAY_CONST_M = 343.5;//delay M

        public const int MAX_DELAY = 0xFF3E; //65342


        public const double MaxShowValue = 1361.29;

        public static int decodeDelayvalue(double dValue)
        {
            double tmp = dValue;
            int resInt = 0;
            if (tmp > MaxShowValue)
                tmp = MaxShowValue;
            else if (tmp < 0)
                tmp = 0;
            resInt = (int)(tmp / DELAY_CONST);
            return resInt;
        }

        public int DelayMax
        {
            get { return (int)GetValue(DelayMaxProperty); }
            set { SetValue(DelayMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DelayMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelayMaxProperty =
            DependencyProperty.Register("DelayMax", typeof(int), typeof(CDelayControl),

        new FrameworkPropertyMetadata(MAX_DELAY, FrameworkPropertyMetadataOptions.None, onDelayMaxChange
            ));

        private static void onDelayMaxChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CDelayControl;
            if (mcontrol != null)
            {
                mcontrol.DelayMax = (int)args.NewValue;
                mcontrol.dslider.Maximum = (int)args.NewValue;
            }

        }
        //-----------------------

        private const byte defalutBypas = 0;
        public byte delayByPas
        {
            get { return (byte)GetValue(delayByPasProperty); }
            set { SetValue(delayByPasProperty, value); }
        }
        // Using a DependencyProperty as the backing store for delayByPas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty delayByPasProperty =
            DependencyProperty.Register("delayByPas", typeof(byte), typeof(CDelayControl), new PropertyMetadata(defalutBypas));

        public int delayPosvalue
        {
            get { return (int)GetValue(delayPosProperty); }
            set
            {
                SetValue(delayPosProperty, value);
                // updateDelay();
            }
        }

        // Using a DependencyProperty as the backing store for delayPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty delayPosProperty =
            DependencyProperty.Register("delayPosvalue", typeof(int), typeof(CDelayControl), new PropertyMetadata(0));

        public CDelayControl()
        {
            InitializeComponent();
            dslider.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(mySliderValueChanged);
        }


        public delegate void delayControlValueChanged(Object sender);
        public event delayControlValueChanged onDelayControlValueChangedEvent;



        public delegate void delayEditClick(Object sender);
        public event delayEditClick onDelayEditClickEvent;

        public delegate void delayEditKeyDown(Object sender);
        public event delayEditKeyDown onDelayEditKeyDownEvent;

     

        private void mySliderValueChanged(object sender, int newPos, EventArgs e)
        {
            // Debug.WriteLine("export to slide new pos is :  {0}", newPos);
            var slider = sender as CSlider;
            delayPosvalue = newPos;
            if (onDelayControlValueChangedEvent != null) //no by pass
            {
                onDelayControlValueChangedEvent(this);
            }


        }

        /// <summary>
        /// update delaypos and power status etc.
        /// </summary>
        public void refreshControl()
        {
            updateDelayPosValue();
            updateDelayPower();
        }

        public string gDelayStr()
        {
            int mpos = delayPosvalue;
            //   dslider.Posvalue = mpos;
            return caculateDelayStr(mpos, 0);
        }

        public void updateDelayPosValue()
        {
            int mpos = delayPosvalue;
            dslider.Posvalue = mpos;
            String strMsDely=caculateDelayStr(mpos, 0);
            edmS.Text =strMsDely;
            edM.Text = caculateDelayStr(mpos, 1);
            Debug.WriteLine("update delay posvalue with " + mpos.ToString()+"  so cacl delaystr  "+strMsDely);
            
        }

        public  string caculateDelayStr(int xdelay, byte flag)
        {

            int dpos = CUlitity.limitValue(xdelay, DelayMax);

            double delaypos = DELAY_CONST * dpos;

            string strRes = "";
            if (flag == 0) //ms
            {
                //  strRes = string.Format("{0}mS", delaypos);
            }
            else if (flag == 1) //m
            {
                delaypos = delaypos * DELAY_CONST_M / 1000;
                // delaypos *= 3.2808;
               
                //strRes = string.Format("{0}m", delaypos);
            }

            strRes = CUlitity.fRound(delaypos, 2).ToString("f2");
            Debug.WriteLine("caculate str is "+delaypos);
            return strRes;
        }

        public void updateDelayPower()
        {
            // delayByPas
            delayPowerBtn.IsSelected = (delayByPas > 0);
            dslider.IsEnabled = (delayByPas == 0);
        }


        private void delayPowerBtn_Click(object sender, RoutedEventArgs e)
        {
            //
            var btn = sender as CSwitcher;

            if (btn.IsSelected)
            {
                delayByPas = 0;
            }
            else
            {
                delayByPas = 1;
            }
            //-------------------
            if (onDelayControlValueChangedEvent != null)
            {
                onDelayControlValueChangedEvent(this);
            }

        }

        private void edmS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (onDelayEditClickEvent != null)
            {
                var box = sender as TextBox;
                box.Background = Brushes.White;
                onDelayEditClickEvent(sender);
            }

        }

        private void edmS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (onDelayEditKeyDownEvent != null)
                {
                    var box = sender as TextBox;
                    box.Background = Brushes.Gray;
                    onDelayEditKeyDownEvent(sender);
                }
                
            }
        }
        public delegate void delayEditLostFocus(Object sender);
        public event delayEditLostFocus onDelayEditLostFocusEvent;
        private void edmS_LostFocus(object sender, RoutedEventArgs e)
        {
            if (onDelayEditLostFocusEvent != null)
            {
                var box = sender as TextBox;
                box.Background = Brushes.Gray;
                onDelayEditLostFocusEvent(sender);
            }
        }
    }
}
