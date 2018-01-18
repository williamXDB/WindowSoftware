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
    /// Interaction logic for FBCSwitcherControl.xaml
    /// </summary>
    public partial class FBCSwitcherControl : UserControl
    {
        public byte[][] m_fbcSwitcher = new byte[2][];

        public delegate void FBCSwitchControlChanged(Object sender, int chanelindex);
        public event FBCSwitchControlChanged onFBCSwitchValueChangedEvent;

        public FBCSwitcherControl()
        {
            InitializeComponent();
            initialParameters();
        }

        public const int FBC_LEN = 20;


        private void initialParameters()
        {
            if (m_fbcSwitcher == null)
                m_fbcSwitcher = new byte[2][];
            for (int i = 0; i < 2; i++)
            {
                m_fbcSwitcher[i] = new byte[32];
            }

        }
        public const int FBCChanel = 28;//index not num

        public const int FBCLeftChanel = 20;
        /// <summary>
        /// CSwitcher click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inBtn_0_Click(object sender, RoutedEventArgs e)
        {
            var btn = (CSwitcher)sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;

            int kx = (index >= FBC_LEN ?index-20 : FBCLeftChanel);

            int chindex = (index >= FBC_LEN ? 1 : 0);

            int fx = (index >= FBC_LEN ? index - 20 : index);

            m_fbcSwitcher[chindex][fx] = tmp;
            if (onFBCSwitchValueChangedEvent != null)
            {
                onFBCSwitchValueChangedEvent(this, kx);
            }

        }

        


        ///----------------------
        private void updateFBCSwitcher(int chindex)
        {
            int index = 0;
            string strBtn = string.Empty;
            CSwitcher btn = null;
            for (index = 0; index < 20; index++)
            {
                if (chindex == 0)
                    strBtn = string.Format("inBtn_{0}", index);
                else
                    strBtn = string.Format("inBtn_{0}", index + 20);

                btn = (CSwitcher)this.FindName(strBtn);
                if (btn != null)
                {
                    btn.IsSelected = (m_fbcSwitcher[chindex][index] > 0);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void refreshControl()
        {

            for (int ch = 0; ch < 2; ch++)
            {
                updateFBCSwitcher(ch);
            }

        }
        ///
















    }
}
