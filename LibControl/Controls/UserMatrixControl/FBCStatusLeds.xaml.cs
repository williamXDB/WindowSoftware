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
    /// Interaction logic for FBCStatusLeds.xaml
    /// </summary>
    public partial class FBCStatusLeds : UserControl
    {
        public const int Max_Fbc_ledNum = 24;
        public FBCStatusLeds()
        {
            InitializeComponent();
            if (m_ledStatus == null)
            {
                m_ledStatus = new byte[24];
            }

        }

        public byte[] m_ledStatus = new byte[24];
        private const byte tmp = 0;   

        /// <summary>
        /// -------------------------reflresh with LED
        /// </summary>
        public void reflreshLed(int preferFBCSetup, int fblinkindex = -1)
        {
            CLed led = null;
            for (int i = 0; i < Max_Fbc_ledNum; i++)
            {
                string strLed = string.Format("fbcled_{0}", i);
                led = (CLed)this.FindName(strLed);
                if (led != null)
                {
                    led.stopBlink();
                    if (m_ledStatus[i] == 1)
                    {                     
                      led.LedStatus = LED_Status.LED_Green;
                      led.BackBrush = led.GreenBrush;
                        
                    }                        
                    else if (m_ledStatus[i] == 2)
                    {                      
                        led.LedStatus = LED_Status.LED_Red;
                        led.BackBrush = led.RedBrush;
                    }                        
                    else if (m_ledStatus[i] == 0 && fblinkindex==i)
                    {
                        if (preferFBCSetup == 1)
                            led.startRedBlink();
                        else
                            led.startGreenBlink();
                    }
                    else
                    {
                        led.stopBlink();
                        led.BackBrush = led.NormalBrush;
                     //   Debug.WriteLine("ledstatus stopblink with index {0}   ledvalue {1}", i, m_ledStatus[i]);
                    }
                }
              //  Debug.WriteLine("ledstatus value is  {0}  index  {1}",m_ledStatus[i],i);
            }
           // setBlinkindex(fblinkindex);
        }


    }
}
