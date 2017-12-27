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

using System.Diagnostics;
using Lib.Controls;


namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CPYGCheckBox.xaml
    /// </summary>
    public partial class CPYGCheckBox : UserControl
    {

        public byte[] m_checkList = new byte[CFinal.ChanelMax];
        public CPYGCheckBox()
        {
            InitializeComponent();
            if (m_checkList == null)
            {
                m_checkList = new byte[CFinal.ChanelMax];
            }
        }

        private void checkBtn_0_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender as CheckBox;
            if (cbx.IsChecked.HasValue)
            {
                int index = int.Parse(CUlitity.rightStr("_", cbx.Name).Trim());
                //   Debug.WriteLine("checkbox radio index is :  {0}", index);
                bool tmp = (bool)cbx.IsChecked;
                m_checkList[index] = (tmp ? CFinal.BII : CFinal.BI);

            }
        }

        private void setWholeStatus(bool sts)
        {
            CheckBox rbtn = null;
            for (int i = 0; i < CFinal.ChanelMax; i++)
            {
                string strCheck = string.Format("checkBtn_{0}", i);//checkBtn_11
                rbtn = (CheckBox)this.FindName(strCheck);
                if (rbtn != null)
                {
                    rbtn.IsChecked = sts;
                    m_checkList[i] = (sts?CFinal.BII:CFinal.BI);
                }

            }

        }

        public void disSelectAll()
        {
            setWholeStatus(false);

        }
        /// <summary>
        /// selcect all
        /// </summary>
        public void selectEntire()
        {
            setWholeStatus(true);

        }





    }
}
