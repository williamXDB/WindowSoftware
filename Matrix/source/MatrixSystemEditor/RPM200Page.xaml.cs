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
    /// Interaction logic for RPM200.xaml
    /// </summary>
    public partial class RPM200Page : Window
    {
        public RPM200Page()
        {
            InitializeComponent();
            initialGUI();
        }

        public void initialGUI()
        {
            MaxZoneSetSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onSpinChangeHandle);
            volSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onVolSpinChangeHandle);
            chimerSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onVolSpinChangeHandle);
            masterSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onVolSpinChangeHandle);
            prioritySpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onVolSpinChangeHandle);
            chimeTimeSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onVolSpinChangeHandle);

            initialMSGWindow();

            m_zonGroup.m_ZoneGroupClickEvent += new ZoneGroup.OnZoneGrupSelectEvent(m_zonGroup_m_ZoneGroupClickEvent);

            m_rowZone.m_OnPageZoneClickEvent += m_rowZone_m_OnPageZoneClickEvent;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devindex">device num index in one page zone,not the pageindex</param>
        /// <param name="mdata">32 num data,which zone</param>
        /// <param name="e"></param>
        private void m_rowZone_m_OnPageZoneClickEvent(int devindex, byte[] mdata, RoutedEventArgs e) 
        {
            //  throw new NotImplementedException();    
            int pagezonindex = m_zonGroup.selZoneIndex;
            CMatrixData.matrixData.m_RPMCover.clearRPMData_withZonindex(pagezonindex);
            Array.Copy(mdata, CMatrixData.matrixData.m_RPMCover.m_rpmData[pagezonindex], CDefine.Max_ZonDev * 2);//32

          //  Debug.WriteLine("pagerow click  zow index  {0}    devindex {1}  " ,pagezonindex,devindex);

        }

        private void m_zonGroup_m_ZoneGroupClickEvent(int index, MouseButtonEventArgs e)
        {
            Debug.WriteLine("mgroup outside clickzone item index is " + index);
            refreshZonePage();

        }

        public void showConLed()
        {
            conLed.LedStatus = (NetCilent.netCilent.isConnected() ? LED_Status.LED_Green : LED_Status.LED_Normal);
        }

        private MessageReceiver m_MsgRecver;
        private void initialMSGWindow()
        {
            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.RPMGUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;
            Debug.WriteLine("RVC200 window initial now....");
        }

        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            // Debug.WriteLine("receive crvc200 msg ..........{0}.", e.Message);
            if (e.Message == MatrixCMD.RPM200_MSG_Transfer)
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                int chindex = cds.preWpl;//low param
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.F_RecallCurrentScene:
                        {
                            Debug.WriteLine("rpm msg current sence receive now.....................");
                            refreshPage();
                        }
                        break;
                    case MatrixCMD.F_RdDevInfo:
                        {


                        }
                        break;
                }

            }


        }



        private void onSpinChangeHandle(object sender, int fvalue, EQFragType etpe, int itg, EventArgs e)
        {
            // Debug.WriteLine("current fvalue is :  {0}\t eqfratype :{1}\t iTag is {2}\t",
            // fvalue, etpe, itg);
            //MessageBox.Show(fvalue.ToString());            
            int tmp = (fvalue + 1);
            CMatrixData.matrixData.m_RPMCover.m_maxZoneSetting = (byte)(fvalue);
            MaxZoneSetSpin.valueTxt = tmp.ToString();
            checkRowZoneTurn();

        }

        private void onVolSpinChangeHandle(object sender, int fvalue, EQFragType etpe, int itg, EventArgs e)
        {
            var vspin = sender as SpinnerControl;
            byte tmp = (byte)fvalue;
            switch (itg)
            {
                case 0:
                    CMatrixData.matrixData.m_RPMCover.m_micVolume = tmp;
                    break;
                case 1:
                    CMatrixData.matrixData.m_RPMCover.m_chimeVolume = tmp;
                    break;
                case 2:
                    CMatrixData.matrixData.m_RPMCover.m_masterVolume = tmp;
                    break;
                case 3:
                    CMatrixData.matrixData.m_RPMCover.m_priority = tmp;
                    break;
                case 4:
                    CMatrixData.matrixData.m_RPMCover.m_chimeTime = tmp;
                    break;

            }
            if (itg < 3)
                vspin.valueTxt = tmp.ToString();
            else if (itg == 3)
                vspin.valueTxt = (tmp + 1).ToString();
            else
                vspin.valueTxt = string.Format("{0}S", (double)(CMatrixData.matrixData.m_RPMCover.m_chimeTime + 1) * 0.1);
            //volSpin.valueTxt = tmp.ToString();
        }

        public void refreshZonePage()
        {
            int selindex = m_zonGroup.selZoneIndex;
            Debug.WriteLine("will refresh page tab is : " + selindex);
            Array.Copy(CMatrixData.matrixData.m_RPMCover.m_rpmData[selindex], m_rowZone.m_singleZoneByte, CDefine.Max_ZonDev*2);            
            m_rowZone.refreshAllSingPorts();

        }


        public void checkRowZoneTurn()
        {
            int num = CMatrixData.matrixData.m_RPMCover.m_maxZoneSetting + 1;
            m_zonGroup.setRowZoneShare(num);
            refreshZonePage();
        }

        public void showDeviceInfo()
        {
            //appID
            labelAPID.Text = CMatrixData.matrixData.rpmDevProvision.pMachineID.ToString("X2");
            labelDevID.Text = CMatrixData.matrixData.rpmDevProvision.pDeviceID.ToString("X4");
            //device name
            showDeviceName();
            fmLb.Text = CMatrixData.matrixData.m_RPMCover.getMcuVer();
            this.Title = "RPM200 Editor";
        }
        public void refreshPage()
        {
            showDeviceInfo();
            updateRPMSenceFromData();
            setRotateStatus(false);
        }

        public void showDeviceName()
        {
            string strDev = CMatrixData.matrixData.m_RPMCover.nameofDevice();
            edDevName.Text = strDev;
            Debug.WriteLine("\n show device name is  ............." + strDev);
        }

        public void updateRPMSenceFromData()
        {
            volSpin.Value = CMatrixData.matrixData.m_RPMCover.m_micVolume; //0  32
            volSpin.valueTxt = volSpin.Value.ToString();
            //          
            chimerSpin.Value = CMatrixData.matrixData.m_RPMCover.m_chimeVolume;
            chimerSpin.valueTxt = chimerSpin.Value.ToString();

            masterSpin.Value = CMatrixData.matrixData.m_RPMCover.m_masterVolume;
            masterSpin.valueTxt = masterSpin.Value.ToString();

            prioritySpin.Value = CMatrixData.matrixData.m_RPMCover.m_priority;
            prioritySpin.valueTxt = (prioritySpin.Value + 1).ToString();
            //--------------
            chimeTimeSpin.Value = CMatrixData.matrixData.m_RPMCover.m_chimeTime;
            chimeTimeSpin.valueTxt = string.Format("{0}S", (double)(CMatrixData.matrixData.m_RPMCover.m_chimeTime + 1) * 0.1);
            //
            byte tmp = CMatrixData.matrixData.m_RPMCover.m_maxZoneSetting;
            MaxZoneSetSpin.Value = tmp;
            MaxZoneSetSpin.valueTxt = (tmp + 1).ToString();
            checkRowZoneTurn();
            //
            disPatchAllZoneNameBytes_toZoneGoup();
            refreshZonePage();

        }


     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //
            showConLed();
            checkRowZoneTurn();
            refreshPage();


        }

        private void edDevName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
            // Debug.WriteLine("mouse double click now....");
            var box = sender as TextBox;
            box.Clear();
        }




        private void edDevName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strInput = edDevName.Text.Trim();
            CMatrixData.matrixData.m_RPMCover.setDevName(strInput);
            Debug.WriteLine("text has changed now...." + strInput);
        }

        private void edDevName_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("pre keyup now....");
        }

        private void edDevName_MouseLeave(object sender, MouseEventArgs e)
        {
            var box = sender as TextBox;
            box.IsReadOnly = true;
        }
        public void setRotateStatus(bool sts)
        {
            rotateSpin.IsIndeterminate = sts;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Are you sure to save change to device ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //   m_zonGroup.calcZoneStringToByte();
                //  caculateZoneNamesToByte();
                CMatrixData.matrixData.sendCMD_Save_RPM100Sence();
                if (NetCilent.netCilent.isConnected())
                    setRotateStatus(true);

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_loadFromLocal_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opdlg = new OpenFileDialog();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            opdlg.InitialDirectory = filePath;
            //----------------------
            opdlg.Filter = IORPM200.fileFilter;
            opdlg.DefaultExt = IORPM200.fileExeName;
            opdlg.RestoreDirectory = true;
            Nullable<bool> result = opdlg.ShowDialog();
            if (result == true)
            {
                string fileName = opdlg.FileName;
                // Debug.WriteLine("you select the file name is  " + fileName);
                if (File.Exists(fileName))
                {

                    IORPM200 mrom = FileIOUtility.ReadFromBinaryFile<IORPM200>(fileName);
                    if (mrom != null && mrom.AppID == AppIDList.AP_RPM_100)
                    {
                        CMatrixData.matrixData.m_RPMCover.copy(mrom);
                        if (NetCilent.netCilent.isConnected())
                        {
                            setRotateStatus(true);
                            CMatrixData.matrixData.sendCMD_Save_RPM100Sence();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid file preset!");
                    }
                }
            }

        }

        private void btn_SavePreset_tolocal_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog opdlg = new SaveFileDialog();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            opdlg.InitialDirectory = filePath;
            //----------------------
            opdlg.Filter = IORPM200.fileFilter;
            opdlg.DefaultExt = IORPM200.fileExeName;
            opdlg.RestoreDirectory = true;
            Nullable<bool> result = opdlg.ShowDialog();
            if (result == true)
            {
                // m_zonGroup.calcZoneStringToByte();
                string fileName = opdlg.FileName;
                // Debug.WriteLine("you select the file name is  " + fileName);
                FileIOUtility.WriteToBinaryFile<IORPM200>(fileName, CMatrixData.matrixData.m_RPMCover);
            }

        }

        private void btn_loadFromDevice_Click(object sender, RoutedEventArgs e)
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
                    CMatrixData.matrixData.sendCMD_recallRPMCurrence();

                }
            }
        }

        private void btn_saveToDevice_Click(object sender, RoutedEventArgs e)
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
                    getAllZoneBytes_toData();
                    //m_zonGroup.m_ZoneName
                    CMatrixData.matrixData.sendCMD_Save_RPM100Sence(); //RPM200

                }
            }
        }


        public void disPatchAllZoneNameBytes_toZoneGoup()
        {
            m_zonGroup.copyAllZoneName_fromDataAndUpdate(CMatrixData.matrixData.m_RPMCover.m_zoneName);

        }

        /// <summary>
        /// get all byte about zone name to data to save 
        /// </summary>
        public void getAllZoneBytes_toData()
        {
            m_zonGroup.calcAllZoneNameB_ytes_fromAllItems_toGroups();
            CMatrixData.matrixData.m_RPMCover.copyAllZoneName(m_zonGroup.m_ZoneName);
        }




        private void tttBtn_Click(object sender, RoutedEventArgs e)
        {
            m_rowZone.setRowZoneShare(1);
        }



    }
}
