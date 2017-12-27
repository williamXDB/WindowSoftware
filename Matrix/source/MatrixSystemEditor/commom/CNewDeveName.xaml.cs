using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommLibrary;
using Lib.Controls;


namespace MatrixSystemEditor.commom
{
    /// <summary>
    /// Interaction logic for CNewDeveName.xaml
    /// </summary>
    public partial class CNewDeveName : Window
    {

        public delegate void  submitNewDeviceChanged(Object sender);
        public event submitNewDeviceChanged onsubmitNewDeviceChangedEvent;

        private MessageReceiver m_MsgRecver;

        public CNewDeveName()
        {
            InitializeComponent();
            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.CGNewDevNameGUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;

        }
        public string getInputName()
        {
            return edInput.Text.Trim();
        }
        public void setBackground(SolidColorBrush scb)
        {
            this.Background = scb;
        }
        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
           ///process message
            if (e.Message == MatrixCMD.ChangeDevName_MSG_Transfer)
            {
                 COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                int flag = cds.preWpl;//low param
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.F_PCGetDeviceInfo:
                        {                            
                            setRotateStatus((flag>0));
                            this.Close();
                        }

                        break;
                    case MatrixCMD.F_StoreSinglePreset:
                        {
                            setRotateStatus(false);
                            this.Close();
                        }
                        break;

                }

            

            }


        }

        public void setSubTitle(string str)
        {
            subTitleLB.Text = str;       

        }

        /// <summary>
        /// set rotatSpin true:visible ,otherwise hidden.
        /// </summary>
        /// <param name="sts"></param>
        public void setRotateStatus(bool sts)
        {
            rotateSpin.IsIndeterminate = sts;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (edInput.Text.Trim().Length > 0)
            {

               // this.DialogResult = true;
               // this.Close();
                if(onsubmitNewDeviceChangedEvent!=null)
                {
                    onsubmitNewDeviceChangedEvent(this);
                }

            }

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9\b]+$");//numeric and backspace
            e.Handled = regex.IsMatch(e.Text);
        }

        private void edInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                submitBtn_Click(sender,e);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            edInput.Focus();
        }



    }



}
