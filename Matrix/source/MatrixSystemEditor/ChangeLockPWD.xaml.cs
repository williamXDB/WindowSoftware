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
using MatrixSystemEditor.Matrix;
namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for ChangeLockPWD.xaml
    /// </summary>
    public partial class ChangeLockPWD : Window
    {
        public ChangeLockPWD()
        {
            InitializeComponent();
            initialTimer();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private System.Timers.Timer ackTimer = new System.Timers.Timer(1000);
     
        public void initialTimer()
        {
            //setNormalSendCommuniStatus();
            if (ackTimer == null)
            {
                ackTimer = new System.Timers.Timer(1000);
            }
            ackTimer.AutoReset = true;
            ackTimer.Enabled = true;
            ackTimer.Elapsed += new System.Timers.ElapsedEventHandler(ackTimer_Tick);
            ackTimer.Start();

        }
        private void ackTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            CMatrixData.matrixData.resetCommunicateStatus();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            //String sm = CMatrixData.matrixData.strLockPWD();
            String str = edCpwd.Password;
            // ShowMessage(sm);
            if (CMatrixData.matrixData.isLockPwdRight(str))
            {
                String strN1 = edNpwd.Password;
                String strN2 = edCfpwd.Password;
                if (strN1.CompareTo(strN2) != 0)
                {
                    MessageBox.Show("Two password entries are incorrect!");
                    edCfpwd.Clear();
                    edCfpwd.Focus();
                    return;
                }
                else
                {
                    //write into the DLM8Data data struct 1
                    CMatrixData.matrixData.setLockPWD(strN1);
                    MessageBox.Show("Password changed successfully!");
                    this.DialogResult = true;
                }

            }
            else
            {
                MessageBox.Show("Password is incorrect!");
                edCpwd.Clear();
                edCpwd.Focus();
                return;
            }


        }

        private void edCpwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                edNpwd.Focus();
            }
        }

        private void edNpwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                edCfpwd.Focus();
            }
        }

        private void edCfpwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                okBtn.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            edCpwd.Focus();
            ackTimer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ackTimer.Stop();
        }

    }
}
