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

namespace MatrixSystemEditor
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
        }

      public static  byte[] m_ledStatus = new byte[24];
      private const byte tmp = 0;

      public void reflreshLed()
      {
          CLed led = null;        
          for(int i=0;i<Max_Fbc_ledNum;i++)
          {
              string strLed=string.Format("fbcled_{0}",i);
              led =(CLed)this.FindName(strLed);
              LED_Status sts=(m_ledStatus[i]==tmp?LED_Status.LED_Normal:LED_Status.LED_Green);
              if(led!=null)
              {
                  led.LedStatus = sts;
              }
          }


      }


    }
}
