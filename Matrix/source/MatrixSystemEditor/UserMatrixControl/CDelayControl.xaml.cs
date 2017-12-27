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
using Lib.Controls;
using CommLibrary;
using System.Diagnostics;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CDelayControl.xaml
    /// </summary>


    public partial class CDelayControl : UserControl
    {
        public const double DELAY_CONST = 0.0208333333;
        public const double DELAY_CONST_M = 343.5;//delay M
        public const int MAX_DELAY = 0xFF3E;


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




        private void mySliderValueChanged(object sender, int newPos, EventArgs e)
        {
            Debug.WriteLine("export to slide new pos is :  {0}", newPos);
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


        public void updateDelayPosValue()
        {
            int mpos = delayPosvalue;
            dslider.Posvalue = mpos;
            edmS.Text = caculateDelayStr(mpos, 0);
            edM.Text = caculateDelayStr(mpos, 1);
        }

        public string caculateDelayStr(int xdelay, byte flag)
        {

            int dpos = xdelay;
            if (dpos > MAX_DELAY)
                dpos = MAX_DELAY;
            else if (dpos < 0) dpos = 0;
            double delaypos = DELAY_CONST * dpos;

            string strRes = "";
            if (flag == 0) //ms
            {
                //  strRes = string.Format("{0}mS", delaypos);
            }
            else if (flag == 1) //m
            {
                delaypos = delaypos * DELAY_CONST_M / 1000;
                delaypos *= 3.2808;
                delaypos = Math.Round(delaypos, 2);
                //strRes = string.Format("{0}m", delaypos);
            }

            delaypos = Math.Round(delaypos, 2);
            strRes = delaypos.ToString();
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

    }
}
