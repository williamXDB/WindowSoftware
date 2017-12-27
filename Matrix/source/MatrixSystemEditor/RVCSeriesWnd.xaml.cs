using CommLibrary;
using CSFileIO;
using Lib.Controls;
using MatrixSystemEditor.commom;
using MatrixSystemEditor.Matrix;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for RVCSeriesWnd.xaml
    /// </summary>
    public partial class RVCSeriesWnd : Window
    {
        public RVCSeriesWnd()
        {
            InitializeComponent();
            initialMSGWindow();
        }

        private MessageReceiver m_MsgRecver;
        private void initialMSGWindow()
        {

            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.RVCUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;
            Debug.WriteLine("RVC1000 window initial now....");
        }

        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            // Debug.WriteLine("receive crvc200 msg ..........{0}.", e.Message);
            if (e.Message == MatrixCMD.RVC1000_MSG_Transfer)
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                int chindex = cds.preWpl;//low param
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.F_RecallCurrentScene:
                    case MatrixCMD.F_LoadFromPC:
                        {
                            Debug.WriteLine("rvc msg current sence receive now.....................");
                            refreshPage();
                        }
                        break;
                    case MatrixCMD.F_RdDevInfo:
                        {
                            showDevInfo();
                        }
                        break;                  


                }

            }


        }


        private void btnSaveToDevice_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to save all params to device ?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                if (!NetCilent.netCilent.isConnected())
                {
                    MessageBox.Show("Please connect the remote device first!", "RPM200", MessageBoxButton.OK);
                }
                else
                {
                    setRotateStatus(true);
                    CMatrixData.matrixData.sendCMD_Save_RVC1000Sence();

                }
            }
        }

        private void btnLoadFromDevice_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to load all params from device ?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                if (!NetCilent.netCilent.isConnected())
                {
                    MessageBox.Show("Please connect the remote device first!", "RPM200", MessageBoxButton.OK);
                }
                else
                {
                    setRotateStatus(true);
                    //
                    CMatrixData.matrixData.sendCMD_recallRPVCurrence();

                }
            }

        }

        private void btnSavePresetToLocal_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog opdlg = new SaveFileDialog();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            opdlg.InitialDirectory = filePath;
            //----------------------
            opdlg.Filter = IORVC1000.FileFilter;
            opdlg.DefaultExt = IORVC1000.FileExeName;
            opdlg.RestoreDirectory = true;
            Nullable<bool> result = opdlg.ShowDialog();
            if (result == true)
            {
                // m_zonGroup.calcZoneStringToByte();
                string fileName = opdlg.FileName;
                // Debug.WriteLine("you select the file name is  " + fileName);             
                FileIOUtility.WriteToBinaryFile<IORVC1000>(fileName, CMatrixData.matrixData.m_RVCover);

            }

        }

        private void btnLoadPreFromLocal_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opdlg = new OpenFileDialog();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            opdlg.InitialDirectory = filePath;
            //----------------------
            opdlg.Filter = IORVC1000.FileFilter;
            opdlg.DefaultExt = IORVC1000.FileExeName;
            opdlg.RestoreDirectory = true;
            Nullable<bool> result = opdlg.ShowDialog();
            if (result == true)
            {
                string fileName = opdlg.FileName;
                // Debug.WriteLine("you select the file name is  " + fileName);
                if (File.Exists(fileName))
                {
                    IORVC1000 mrom = FileIOUtility.ReadFromBinaryFile<IORVC1000>(fileName);
                    if (mrom != null && mrom.AppID == AppIDList.AP_RVC_100)
                    {
                        CMatrixData.matrixData.m_RVCover.copy(mrom);
                        if (NetCilent.netCilent.isConnected())
                        {
                            setRotateStatus(true);
                            CMatrixData.matrixData.sendCMD_recallRPVCurrence();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid file preset!");
                    }
                }
            }

        }
        public void setRotateStatus(bool sts)
        {
            rotateSpin.IsIndeterminate = sts;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var box = sender as TextBox;
            box.Clear();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            string strInput = box.Text.Trim();
            CMatrixData.matrixData.m_RVCover.setDevName(strInput);
        }

        private void chanel_CheckBox_Click(object sender, RoutedEventArgs e) //
        {
            var box = sender as CheckBox;
            int index = Int32.Parse(box.Tag.ToString());
            Nullable<bool> result = box.IsChecked;
            byte vb = 0;
            if (result == true)
                vb = 1;
            else
                vb = 0;
            CMatrixData.matrixData.m_RVCover.m_ChanControlFlag[index] = vb;

            Debug.WriteLine("checkbox  value is  " + vb);
        }
        public void showAppID_deviceID()
        {

            lbApID.Text = CMatrixData.matrixData.rvcDevProvision.pMachineID.ToString("X2");
            lbDevID.Text = CMatrixData.matrixData.rvcDevProvision.pDeviceID.ToString("X4");
        }


        private void Route_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var box = sender as CheckBox;
            int index = Int32.Parse(box.Tag.ToString());
            Nullable<bool> result = box.IsChecked;
            byte vb = 0;
            if (result == true)
                vb = 1;
            else
                vb = 0;
            CMatrixData.matrixData.m_RVCover.m_RoutingEnableFlag[index] = vb;

        }


        public void refreshPage()
        {
            updateAllChanelCheck();
            updateAllRouteCheck();
            showDevInfo();
            setRotateStatus(false);
        }
        public void showConLed()
        {
            conLed.LedStatus = (NetCilent.netCilent.isConnected() ? LED_Status.LED_Green : LED_Status.LED_Normal);
        }

        public void showDevInfo()
        {
            showConLed();
            updateDevName();
            showAppID_deviceID();
        }

        public void updateAllChanelCheck()
        {
            string strCbx = "";
            CheckBox cb = null;
            for (int i = 0; i < 24; i++)
            {
                strCbx = string.Format("ch_cbx{0}", i);
                cb = (CheckBox)this.FindName(strCbx);
                if (cb != null)
                {
                    byte vb = CMatrixData.matrixData.m_RVCover.m_ChanControlFlag[i];
                    cb.IsChecked = (vb > 0);
                }
            }

        }

        public void updateAllRouteCheck()
        {
            //   strRowZunit = string.Format("rowZone_{0}", i);
            //        sRowzUnit = (RowZoneUnit)this.FindName(strRowZunit);
            string strCbx = "";
            CheckBox cb = null;
            for (int i = 0; i < 12; i++)
            {
                strCbx = string.Format("rout_cbx{0}", i);
                cb = (CheckBox)this.FindName(strCbx);
                if (cb != null)
                {
                    byte vb = CMatrixData.matrixData.m_RVCover.m_RoutingEnableFlag[i];
                    cb.IsChecked = (vb > 0);
                }
            }


        }

        public void updateDevName()
        {
            eDevName.Text = CMatrixData.matrixData.m_RVCover.nameofDevice();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showDevInfo();

        }


    }
}
