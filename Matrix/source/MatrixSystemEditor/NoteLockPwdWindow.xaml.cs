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
    /// Interaction logic for NoteLockPwdWindow.xaml
    /// </summary>
    public partial class NoteLockPwdWindow : Window
    {
        public NoteLockPwdWindow()
        {
            InitializeComponent();
            inputCount = 0;
            initialTimer();
        }
        private int inputCount = 0;
        private const int MaxErrorNum = 100;

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
        bool isOK = false;
        private void unlockBtn_Click(object sender, RoutedEventArgs e)
        {
            isOK = false;

            if (CMatrixData.matrixData.isLockPwdRight(edInput.Password.Trim()))
            {
                isOK = true;
                this.DialogResult = true;
            }
            else
            {
                inputCount++;
                if (inputCount > MaxErrorNum)
                {
                    MessageBox.Show("The password error exceeds the maximum number of entries!");
                    System.Environment.Exit(0);
                }
                else
                {
                    //  erLb.Visibility = Visibility.Visible;
                    MessageBox.Show("Password is incorrect!");
                    //   erLb.Text = string.Format("Password input error {0} time", inputCount);
                    edInput.Clear();
                    edInput.Focus();

                }




            }

        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void edInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                unlockBtn_Click(sender, e);

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            edInput.Focus();
            ackTimer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ackTimer.Stop();
#if _xx
            if (!isOK)
            {
                System.Environment.Exit(0);
            }
#endif
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }


    }
}
