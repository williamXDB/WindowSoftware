using MatrixSystemEditor.Matrix;
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
    /// Interaction logic for CFBCSourceControl.xaml
    /// </summary>
    public partial class CDuckerSourceControl : UserControl
    {
        public byte[] m_DuckerSourch = new byte[CFinal.ChanelMax];
        public byte[] m_DuckerBgmLocal = new byte[CFinal.ChanelMax];
        public byte[] m_DuckerNetInput = new byte[CFinal.ChanelMax];
        /// <summary>
        /// 
        /// </summary>
        public CDuckerSourceControl()
        {
            InitializeComponent();
            resetData();
        }

        public delegate void DuckerValueControlChanged(Object sender);
        public event DuckerValueControlChanged onDuckerValueControlChangedEvent;


        public void resetData()
        {
            if (m_DuckerSourch != null)
                m_DuckerSourch = new byte[CFinal.ChanelMax];
            if (m_DuckerBgmLocal != null)
                m_DuckerBgmLocal = new byte[CFinal.ChanelMax];
            if (m_DuckerNetInput != null)
                m_DuckerNetInput = new byte[CFinal.ChanelMax];
            Array.Clear(m_DuckerSourch, 0, CFinal.ChanelMax);
            Array.Clear(m_DuckerBgmLocal, 0, CFinal.ChanelMax);
            Array.Clear(m_DuckerNetInput, 0, CFinal.ChanelMax);
        }

        private void bgmNetWorkClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerNetInput[index] = tmp;
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this);
            }

        }

        private void bgmLocalClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerBgmLocal[index] = tmp;
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this);
            }

        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceLocalClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerSourch[index] = tmp;
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void refreshControl()
        {
            CSwitcher sbtn = null;
            string strBtn = "";
            for (int i = 0; i < CFinal.ChanelMax; i++)
            {
                strBtn = string.Format("sourceBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = (m_DuckerSourch[i] > 0);
                }

            }
            for (int i = 0; i < CFinal.ChanelMax; i++)  //btm
            {
                strBtn = string.Format("bgmLocalBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {

                    sbtn.IsSelected = (m_DuckerBgmLocal[i] > 0);
                }

            }
            for (int i = 0; i < CFinal.ChanelMax - 4; i++)
            {
                strBtn = string.Format("networkBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = (m_DuckerNetInput[i] > 0);
                }

            }
            //bgmLocalBtn,networkBtn

        }




    }
}
