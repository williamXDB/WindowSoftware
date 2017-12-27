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
    /// Interaction logic for FBCSwitcherControl.xaml
    /// </summary>
    public partial class FBCSwitcherControl : UserControl
    {
        public byte[] m_fbcSwitcher = new byte[40];


        public delegate void FBCSwitchControlChanged(Object sender,int chanelindex);
        public event FBCSwitchControlChanged onFBCSwitchValueChangedEvent;

        public FBCSwitcherControl()
        {
            InitializeComponent();
            initialParameters();
        }

        private void initialParameters()
        {
            if (m_fbcSwitcher == null)
                m_fbcSwitcher = new byte[40];

        }
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
            m_fbcSwitcher[index] = tmp;
            if(onFBCSwitchValueChangedEvent!=null)
            {
                onFBCSwitchValueChangedEvent(this,index/20);
            }

        }
        ///
        public void updateFBCSwitcher(int chindex)
        {
            int index = 0;
            CSwitcher btn = null;
            int ibegin = (chindex == 0 ? 0 : 20);
            int iend = (chindex == 0 ? 19 : 39);
            for (index = ibegin; index <= iend; index++)
            {
                string strBtn = string.Format("inBtn_{0}", index);
                btn = (CSwitcher)this.FindName(strBtn);
                if (btn != null)
                {
                    btn.IsSelected = (m_fbcSwitcher[index] > 0);
                }
            }



        }
        public void refreshControl()
        {
            int index = 0;
            CSwitcher btn = null;
            for (index = 0; index <40; index++)
            {
                string strBtn = string.Format("inBtn_{0}", index);
                btn = (CSwitcher)this.FindName(strBtn);
                if (btn != null)
                {
                    btn.IsSelected = (m_fbcSwitcher[index] > 0);
                }
            }

        }
        ///
     















    }
}
