using CommLibrary;
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
using System.Windows.Shapes;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for MenuWin.xaml
    /// </summary>
    public partial class MenuWin : Window
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }


        public MenuWin()
        {
            InitializeComponent();
        }

        private void lockSysBtn_click(object sender, RoutedEventArgs e)
        {

            CMatrixData.matrixData.lockFlag = 1;
            this.Close();
            //sendcmd lockpassword
            if (!NetCilent.netCilent.isConnected())
            {
                _parentWin.popupPwdWindow();
            }
            else
            {
                CMatrixData.matrixData.sendCMD_WriteLockPWD();

            }
        }

        private void pwdSetBtn_click(object sender, RoutedEventArgs e)
        {
            //
            var chLPwdWindow = new ChangeLockPWD();
            this.Close();
            if (chLPwdWindow.ShowDialog() == true)
            {
                CMatrixData.matrixData.sendCMD_WriteLockPWD();
            }
        }

    }
}
