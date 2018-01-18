using Lib.Controls;
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
    /// Interaction logic for AutoMixerInput.xaml
    /// </summary>
    public partial class AutoMixerInput : UserControl
    {
        public byte[] m_SourchInput = new byte[CMatrixFinal.Max_MatrixChanelNum];

        public delegate void autoMixerValueChanged(Object sender);
        public event autoMixerValueChanged onAutoMixerValueChanged;

        public AutoMixerInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// reset Source input data 
        /// </summary>
        public void resetData()
        {
            if (m_SourchInput == null)
                m_SourchInput = new byte[CMatrixFinal.Max_MatrixChanelNum];

            Array.Clear(m_SourchInput, 0, CMatrixFinal.Max_MatrixChanelNum);

        }




        /// <summary>
        /// refresh the GUI with array data
        /// </summary>
        public void refreshControl()
        {
            CSwitcher sbtn = null;
            string strBtn = "";

            //source select part
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
            {
                strBtn = string.Format("sourceBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = (m_SourchInput[i] > 0);
                }

            }


        }
        /// <summary>
        /// 
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
            m_SourchInput[index] = tmp;          
            //
            if (onAutoMixerValueChanged != null)
            {
                onAutoMixerValueChanged(this);
            }
        }



    }
}
