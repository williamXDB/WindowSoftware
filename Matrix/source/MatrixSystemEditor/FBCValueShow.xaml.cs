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

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for FBCValueShow.xaml
    /// </summary>
    public partial class FBCValueShow : UserControl
    {
        public const int Max_NumIO = 24;
        public byte[] m_FBCStatus = new byte[Max_NumIO];

        public int[] m_FBCFreq = new int[Max_NumIO];

        public int[] m_FBCGain = new int[Max_NumIO];
        public byte currentPos = 0;


        public FBCValueShow()
        {
            InitializeComponent();
        }

        //--------------all FBC status
        public void updateGUI_AllFBC_status()
        {
            //fbcStatus_0
            string strLb=null;
            TextBlock textLb=null;
            for(int i=0;i<Max_NumIO;i++)
            {
               strLb=string.Format("fbcStatus_{0}", i);
               textLb = (TextBlock)this.FindName(strLb);
                if(textLb!=null)
                {
                    textLb.Text = m_FBCStatus[i].ToString();
                }

            }
            
        }
        //--------------all FBC freq
        public void updateGUI_AllFBC_Freq()
        {
            //fbcStatus_0
            string strLb = null;
            TextBlock textLb = null;
            for (int i = 0; i < Max_NumIO; i++)
            {
                strLb = string.Format("fbcFreq_{0}", i);
                textLb = (TextBlock)this.FindName(strLb);
                if (textLb != null)
                {
                    textLb.Text = m_FBCFreq[i].ToString();
                }

            }

        }
        //--------------all FBC gain
        public void updateGUI_AllFBC_gain()
        {
            //fbcStatus_0
            string strLb = null;
            TextBlock textLb = null;
            for (int i = 0; i < Max_NumIO; i++)
            {
                strLb = string.Format("fbcGain_{0}", i);
                textLb = (TextBlock)this.FindName(strLb);
                if (textLb != null)
                {
                    textLb.Text = m_FBCGain[i].ToString();
                }

            }

        }

        //---------------
        public void refreshControl()
        {
            updateGUI_AllFBC_status();
            updateGUI_AllFBC_gain();
            updateGUI_AllFBC_Freq();
            nPosLb.Text = currentPos.ToString();
        }


    }
}
