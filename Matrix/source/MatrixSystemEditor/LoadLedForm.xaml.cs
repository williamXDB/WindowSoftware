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
using Lib.Controls;
using CommLibrary;
namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for LoadLedForm.xaml
    /// </summary>
    public partial class LoadLedForm : Window
    {
        private MatrixPage _parentWin;

        private MessageReceiver m_MsgRecver;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public LoadLedForm()
        {
            InitializeComponent();
            loadLed.setLedTitle("Recall Current Scene");
            loadLed.Visibility = Visibility.Visible;
            loadLed.startSearch();

            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.LoadLEDGUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;

        }

        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            //
            if (e.Message == MatrixCMD.LoadLed_MSG_Transfer)
            {

                this.Close();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void window_loaded(object sender, RoutedEventArgs e)
        {
            this.Left = _parentWin.Left + (_parentWin.Width - this.ActualWidth) / 2;
            this.Top = _parentWin.Top + (_parentWin.Height - this.ActualHeight) / 2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


    }
}
