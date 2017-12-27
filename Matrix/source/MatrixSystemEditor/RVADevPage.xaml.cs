using CommLibrary;
using Lib.Controls;
using MatrixSystemEditor.Matrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MatrixSystemEditor.commom;
using System.Runtime.InteropServices;
using System.Windows.Threading;


namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CRVC200Window.xaml
    /// </summary>
    public partial class RVADevPage : Window
    {
        private int onLineCheckCounter = 0;

        private DispatcherTimer ackTimer;

        public Module_Type moduleType;


        public RVADevPage()
        {
            Debug.WriteLine("rvc200 is created now...");
            InitializeComponent();
            initialGUI();

        }


        private void initialGUI()
        {

            initialMSGWindow();
            ackTimer = new DispatcherTimer();
            ackTimer.Interval = new TimeSpan(0, 0, 4);
            ackTimer.Tick += ackTimer_Tick;
            ackTimer.Stop();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ackTimer_Tick(object sender, EventArgs e)
        {
            //  if (NetCilent.netCilent.isConnected())
            // CMatrixData.matrixData.sendCMD_ACK();
            // refreshAesLed();
        }

        #region event data wrting below...



        public void updateGUI_GlobalPage()
        {

            showDeviInfo();
            showConLed();
        }

        //event channel
        private void ichbtn_0_Click(object sender, RoutedEventArgs e)
        {
            //
        }


        #endregion

        private MessageReceiver m_MsgRecver;

        //inital msg window 
        private void initialMSGWindow()
        {
            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.RVAGUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;
            Debug.WriteLine("RVC200 window initial now....");


        }

        /// <summary>
        /// window MSG for receiver below..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            //
            Debug.WriteLine("receive crvc200 msg ..........{0}.", e.Message);
            if (e.Message == MatrixCMD.RVA200_MSG_Transfer)
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                int chindex = cds.preWpl;//low param
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.F_Ack:
                        {
                            Debug.WriteLine("rva200 msg ack receive now.....................");
                            refreshAesLed();

                        }
                        break;
                    case MatrixCMD.F_RdDevInfo:
                        {
                            Debug.WriteLine("read msg device info now...................");

                            if (moduleType == Module_Type.Mod_RIO100)
                                lbDevName.Text = CMatrixData.matrixData.nameofRIODev();
                            else if (moduleType == Module_Type.Mod_RVC100)
                                lbDevName.Text = CMatrixData.matrixData.nameofRVCDev();
                            else
                                lbDevName.Text = CMatrixData.matrixData.nameofRVADev();

                            CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 0);//stop rotating
                        }
                        break;
                }

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateGUI_GlobalPage();
            string name = this.GetType().Name;
            Debug.WriteLine("class name is " + name);
            ackTimer.Start();

        }

        public void setAsePanel_show(bool sts)
        {
            if (sts)
                aesPanel.Visibility = Visibility.Visible;
            else
                aesPanel.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// 
        /// </summary>
        public void showDeviInfo()
        {
            Debug.WriteLine("show devinfo with module type {0}", moduleType);
            switch (moduleType)
            {
                case Module_Type.Mod_RIO100:
                    {
                        labelAPID.Text = CMatrixData.matrixData.rioDevProvision.pMachineID.ToString("X2");
                        labelDevID.Text = CMatrixData.matrixData.rioDevProvision.pDeviceID.ToString("X4");
                        //
                        lbDevName.Text = CMatrixData.matrixData.nameofRIODev();
                        fmLb.Text = CMatrixData.matrixData.rioVer();
                        this.Title = "RIO200 Editor";
                    }
                    break;
                case Module_Type.Mod_RVA100:
                    {
                        labelAPID.Text = CMatrixData.matrixData.rvaDevProvision.pMachineID.ToString("X2");
                        labelDevID.Text = CMatrixData.matrixData.rvaDevProvision.pDeviceID.ToString("X4");
                        //
                        lbDevName.Text = CMatrixData.matrixData.nameofRVADev();
                        fmLb.Text = CMatrixData.matrixData.rvaVer();
                        this.Title = "RVA200 Editor";
                    }
                    break;
                case Module_Type.Mod_RVC100:
                    {
                        labelAPID.Text = CMatrixData.matrixData.rvcDevProvision.pMachineID.ToString("X2");
                        labelDevID.Text = CMatrixData.matrixData.rvcDevProvision.pDeviceID.ToString("X4");
                        //
                        lbDevName.Text = CMatrixData.matrixData.nameofRVCDev();
                        fmLb.Text = CMatrixData.matrixData.rvcVer();
                        this.Title = "RVC1000 Editor";
                    }
                    break;

            }

        }

        public void refreshAesLed()
        {
            // CMatrixData.matrixData.m_RVAEsStatus =
            Debug.WriteLine("rva devcie refresh paging now.............{0}...", CMatrixData.matrixData.m_RVAEsStatus);
            aesLed.LedStatus = (CMatrixData.matrixData.m_RVAEsStatus == 1 ? LED_Status.LED_Green : LED_Status.LED_Normal);
        }

        public void showConLed()
        {
            conLed.LedStatus = (NetCilent.netCilent.isConnected() ? LED_Status.LED_Green : LED_Status.LED_Normal);

        }

        private void cgDevNameBtn_Click(object sender, RoutedEventArgs e)
        {
           // if (moduleType == Module_Type.Mod_RIO100) return;
            // CMDSender.sendMsgWithoutData(MatrixCMD.RVAGUIClass, MatrixCMD.RVA200_MSG_Transfer, 89, 0);
            var cDevDlg = new CNewDeveName();
            Debug.WriteLine("popup device name change dialog now..............");
            cDevDlg.onsubmitNewDeviceChangedEvent += new CNewDeveName.submitNewDeviceChanged(onsubmitNewDevname);
            cDevDlg.setBackground(this.Background as SolidColorBrush);
            cDevDlg.ShowDialog();

        }

        public void setBackground(SolidColorBrush scb)
        {
            this.Background = scb;
        }

        private void onsubmitNewDevname(object sender)
        {
            var dlg = sender as CNewDeveName;
            if (dlg != null)
            {
                ModuleRVAS mrvas;
                //MatrixCMD.RVAGUIClass        
                string mname = dlg.edInput.Text.Trim();
                Debug.WriteLine("onsubmit new device name is  " + mname);
                if (moduleType == Module_Type.Mod_RIO100)
                    mrvas = ModuleRVAS.MRIO;
                else if (moduleType == Module_Type.Mod_RVC100)
                    mrvas = ModuleRVAS.MRVC;
                else
                    mrvas = ModuleRVAS.MRVA;
                CMatrixData.matrixData.setRVASDeviceName(mrvas, mname);

                if (NetCilent.netCilent.isConnected())
                {
                    CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 1);
                    //devP.123           

                    CMatrixData.matrixData.sendCMD_submitToChangeRVASDevName(mrvas);
                }


            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ackTimer.Stop();
            this.DialogResult = true;
        }


    }


}
