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
using MatrixSystemEditor.Matrix;
using Lib.Controls;
using System.Diagnostics;
using CommLibrary;

namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for MatrixPage.xaml
    /// </summary>
    public partial class SubMatrixPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }


        public SubMatrixPage()
        {
            InitializeComponent();
            //            matrixCtl.
            matrixCtl.onMatrixValueControlChangedEvent += new CMatrixControl.matrixValueControlChanged(onMatrixChanged);
            matrixCtl.setUniformColor(Brushes.Black);
        }

        #region matrix about process
        /// <summary>
        /// matrix 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void onMatrixChanged(object sender, int row, int column)
        {
            //
            CMatrixControl mcontrol = sender as CMatrixControl;
            // CMatrixData.m_ChanelEdit[]  
            CMatrixData.matrixData.m_matrixAry[row][column] = mcontrol.m_matrix[row][column];
            Debug.WriteLine("matrix click row {0} column {1}", row, column);
            if (NetCilent.netCilent.isConnected())
            {

                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_Matrix(row);
            }
            else
                updateMatrixWithRow_fromData(row);

        }

        public void updateMatrixWithRow_fromData(int r)
        {
             Array.Copy(CMatrixData.matrixData.m_matrixAry[r], matrixCtl.m_matrix[r], CDefine.Matrix_CTL_NUM);
             matrixCtl.refreshControl();//20170722
        }
        //
        public void updateMatrixPage_fromData()
        {
            for (int i = 0; i < CDefine.Matrix_CTL_NUM; i++)
            {
                Array.Copy(CMatrixData.matrixData.m_matrixAry[i], matrixCtl.m_matrix[i], CDefine.Matrix_CTL_NUM);
            }
            matrixCtl.refreshControl();    


        }

      
        #endregion
    }
}
