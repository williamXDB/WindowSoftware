using CommLibrary;
using Lib.Controls;
using MatrixSystemEditor.commom;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }


        public SystemPage()
        {
            InitializeComponent();
        }

        private void btnRelay_Click(object sender, RoutedEventArgs e)
        {
            var cbuton = sender as CSwitcher;
            int item = cbuton.iTag;

            byte tmp = 0;
            if (cbuton.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            CMatrixData.matrixData.m_Relay[item] = tmp;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_RelayControl();
            }
            else
                updateRelay();
        }
        public void updateRelay()
        {
            byte tmp = CMatrixData.matrixData.m_Relay[0];
            btnRelayI.IsSelected = (tmp > 0);
            tmp = CMatrixData.matrixData.m_Relay[1];
            btnRelayII.IsSelected = (tmp > 0);
        }
        public void updateDeviceName()
        {
            edtDevice.Text = CMatrixData.matrixData.nameofDevice(); //20170722
        }

        public void updateSystemPage()
        {
            updateRelay();
           updateDeviceName();
        }



        private void btnChangeDevName_Click(object sender, RoutedEventArgs e)
        {
            var cDevDlg = new CNewDeveName();
            cDevDlg.Tag = "devChangeName";
            cDevDlg.onsubmitNewDeviceChangedEvent += new CNewDeveName.submitNewDeviceChanged(onsubmitNewDevname);
            cDevDlg.ShowDialog();
        }
        #region change device name---------------



        private void onsubmitNewDevname(object sender)
        {
            var dlg = sender as CNewDeveName;
            if (dlg != null)
            {

                string mname = dlg.edInput.Text.Trim();
                string dlgTag = dlg.Tag as string;
                if (dlgTag.CompareTo("devChangeName") == 0) //change device name
                {
                    CMatrixData.matrixData.setDevicename(mname);
                    if (NetCilent.netCilent.isConnected())
                    {
                        CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 1);
                        _parentWin.onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_submitToChangeDevName();
                    }

                }
                else  //change preset name
                {
                    /*
                     * 20170722
                    int selNum = pLstBox.SelectedIndex + 1; //overceed 0
                    CMatrixData.matrixData.setPresetName(selNum, mname);
                    if (NetCilent.netCilent.isConnected())
                    {
                        onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_savePresetName(selNum);
                        CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 1);//begin roting
                    }
                    */

                }

                //    Debug.WriteLine("send device name to change....");
            }
            Debug.WriteLine("on submit to new devname...now...");


        }
        #endregion
        private void resetFatoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("All the presets would be reseted to factory! Are you sure to reset to factory setting?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (NetCilent.netCilent.isConnected())
                {
                    //facsp
                    _parentWin.isDefaultSetting_beging = true;
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_ResetToDefaultFactory();
                    setRotateSpinStatus(true);
                    //factSpin
                }


            }
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to reset to default setting?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (NetCilent.netCilent.isConnected())
                {

                    _parentWin.onLineCheckCounter = 0;
                    _parentWin.isDefaultSetting_beging = true;
                    //facsp
                    CMatrixData.matrixData.sendCMD_ResetToDefaultSetting();
                    setRotateSpinStatus(true);
                }

            }

        }
        public void setRotateSpinStatus(bool status)
        {
            if (!status)
            {
                factSpin.IsIndeterminate = false;
                factSpin.Visibility = Visibility.Hidden;
                itorLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                factSpin.IsIndeterminate = true;
                factSpin.Visibility = Visibility.Visible;
                itorLabel.Visibility = Visibility.Visible;
            }

        }
    }
}
