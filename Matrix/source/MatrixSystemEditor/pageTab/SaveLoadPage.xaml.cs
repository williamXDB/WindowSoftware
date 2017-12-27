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
using CommLibrary;
using System.Diagnostics;
using Lib.Controls;
using System.Windows.Forms;
using MatrixSystemEditor.commom;

namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for SaveLoadPage.xaml
    /// </summary>
    public partial class SaveLoadPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public SaveLoadPage()
        {
            InitializeComponent();

        }

        /// <summary>
        /// read the memory data to local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportMemoryToLocalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("It will import all memory presets to local and will take several minutes, are you sure?", "Warning!", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (NetCilent.netCilent.isConnected())
                {
                    _parentWin.ackTimer.Stop();
                    sprocketExport.IsIndeterminate = true;
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_exportMemoryDataToLocal();
                    importMemoryToDevBtn.IsEnabled = false;
                }


            }

        }
        #region memory import and memory export

        /// <summary>
        /// load the memory data to device 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importMemoryToDevBtn_Click(object sender, RoutedEventArgs e)
        {
            exportMemoryToLocalBtn.IsEnabled = false;
            _parentWin.loadMemory_fromLocal();
        }

        public void beginExportRateKnob()
        {
            sprocketImport.IsIndeterminate = true;
        }

        public void updatePrecent(int index, TextBlock lb)
        {
            int max = CDefine.Memory_Max_Package;
            double fk = (double)(index + 1) / max;
            int pos = (int)(fk * 100);
            lb.Text = string.Format("{0}%", pos);

        }

        public void processExport(int chindex)
        {
            updatePrecent(chindex, exportProgreslabel);
            if (chindex == 0)
            {
                exportMemoryToLocalBtn.IsEnabled = false;
                sprocketExport.IsIndeterminate = false;

            }
            else if (chindex == CDefine.Memory_Max_Package - 1)
            {
                exportProgreslabel.Text = "100%";
                importMemoryToDevBtn.IsEnabled = true;
                exportMemoryToLocalBtn.IsEnabled = true;
            }


        }

        public void processImport(int chindex)
        {

            updatePrecent(chindex, importProgreslabel);
            if (chindex == 0)
            {
                importMemoryToDevBtn.IsEnabled = false;
                sprocketImport.IsIndeterminate = false;
                exportMemoryToLocalBtn.IsEnabled = false;


            }
            else if (chindex == CDefine.Memory_Max_Package - 1)
            {
                importProgreslabel.Text = "100%";
                _parentWin.ackTimer.Start();
                importMemoryToDevBtn.IsEnabled = true;
                exportMemoryToLocalBtn.IsEnabled = true;

            }

        }


        #endregion

        private void itemRadio_1_Click(object sender, RoutedEventArgs e)
        {
            var rbtn = sender as System.Windows.Controls.RadioButton;
            int tg = int.Parse((rbtn.Tag as string));
            Debug.WriteLine("radio click tg is : {0}", tg);
            if (tg == 1)
            {
                deleteBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                deleteBtn.Visibility = Visibility.Visible;
            }


        }
        //selct all for copy
        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CSwitcher bc = sender as CSwitcher;
            if (bc.IsSelected)
            {
                cpyGpx.disSelectAll();
                bc.IsSelected = false;
            }
            else
            {
                cpyGpx.selectEntire();
                bc.IsSelected = true;
            }
        }


        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {

            if (System.Windows.MessageBox.Show("Are you sure to copy?", "Warning!", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                int findex = (_parentWin.currentchindex < CMatrixFinal.Max_MatrixChanelNum ? 0 : 1);
                Array.Copy(cpyGpx.m_checkList, CMatrixData.matrixData.m_copy[findex],
                    CMatrixFinal.Max_MatrixChanelNum);
                if (NetCilent.netCilent.isConnected())
                {
                    sprocketCopy.IsIndeterminate = true;
                    // spro
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_Copy(_parentWin.currentchindex);
                }

            }

        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

            int selectindex = pLstBox.SelectedIndex + 1;
            bool isEmpty = CMatrixData.matrixData.determinePresetEmptyInPos(selectindex);
            if (isEmpty)
            {
                System.Windows.MessageBox.Show("You cannot delete a empty Preset!");
                return;
            }

            if (selectindex >= 1 && !isEmpty)
            {
                sprocketSaveLoad.IsIndeterminate = true;
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_deletePreset(selectindex);
            }


        }
        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {

            if (itemRadio_0.IsChecked.HasValue && itemRadio_0.IsChecked.Value) //load from device 
            {


                int selNum = pLstBox.SelectedIndex + 1;
                bool isEmpty = CMatrixData.matrixData.determinePresetEmptyInPos(selNum);
                if (isEmpty)
                {
                    System.Windows.Forms.MessageBox.Show("You cannot load a empty Preset!");
                    return;
                }
                if (selNum >= 1)
                {
                    if (NetCilent.netCilent.isConnected())
                    {

                        _parentWin.isDefaultSetting_beging = true;
                        _parentWin.ackTimer.Stop();
                        sprocketSaveLoad.IsIndeterminate = true;
                        _parentWin.onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_recallSinglePreset(selNum);
                    }


                }
            }
            else  //load from local
            {
                openDlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                openDlg.Filter = CDefine.MPFFilter;
                openDlg.CheckFileExists = true;
                if (openDlg.ShowDialog() == true)
                {
                    string strp = openDlg.FileName;
                    int fileLen = (int)IOBinaryOperation.fileLength(strp);
                    if (fileLen == (CDefine.LEN_Sence - 18))
                    {
                        byte[] iread = IOBinaryOperation.readBinaryFile(strp, fileLen); //sencenlen-
                        CMatrixData.matrixData.iRead_CurrentScene(iread, true);

                        //   string bStr = BitConverter.ToString(iread);
                        // Debug.WriteLine("load factory presets: " + bStr);
                        if (NetCilent.netCilent.isConnected())
                        {
                            _parentWin.isDefaultSetting_beging = true;
                            sprocketSaveLoad.IsIndeterminate = true;
                            _parentWin.onLineCheckCounter = 0;
                            CMatrixData.matrixData.sendCMD_loadFromPC(iread);
                        }

                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Dismatch the factory presets,maybe the file has been damaged!");
                        return;
                    }
                }
            }

        }

        public void stopAllSprocketRotating()
        {
            sprocketCopy.IsIndeterminate = false; //20170722
            sprocketSaveLoad.IsIndeterminate = false;//20170722

        }

        /// <summary>
        /// 
        /// </summary>
        public void refreshPresetListBox()
        {
            pLstBox.Items.Clear();
            string strK = "";
            for (int i = 1; i < CDefine.Max_Presets; i++)
            {
                strK = CMatrixData.matrixData.nameOfPresetIndex(i);
                pLstBox.Items.Add(strK);
            }
            sprocketSaveLoad.IsIndeterminate = false;

        }



        /// <summary>
        /// cleare item here
        /// </summary>
        public void clearPresetList()
        {
            pLstBox.Items.Clear();
        }
        public void setExportMemoryEnable()
        {
            exportMemoryToLocalBtn.IsEnabled = true;
        }


        private void onsubmitNewDevname(object sender)
        {
            var dlg = sender as CNewDeveName;
            if (dlg != null)
            {

                string mname = dlg.edInput.Text.Trim();
                string dlgTag = dlg.Tag as string;

                int selNum = pLstBox.SelectedIndex + 1; //overceed 0
                CMatrixData.matrixData.setPresetName(selNum, mname);
                if (NetCilent.netCilent.isConnected())
                {
                    _parentWin.onLineCheckCounter = 0;
                    sprocketSaveLoad.IsIndeterminate = true;
                    CMatrixData.matrixData.sendCMD_savePresetName(selNum);
                    CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 1);//begin roting
                }


                //    Debug.WriteLine("send device name to change....");
            }
            Debug.WriteLine("on submit to new devname...now...");


        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (itemRadio_0.IsChecked.HasValue && itemRadio_0.IsChecked.Value) //save to device with name
            {
                if (pLstBox.SelectedIndex >= 0)
                {
                    var cnew = new CNewDeveName();
                    cnew.onsubmitNewDeviceChangedEvent += new CNewDeveName.submitNewDeviceChanged(onsubmitNewDevname);
                    cnew.Tag = "presetChangeName";
                    cnew.Title = "Change Preset Name";
                    cnew.setSubTitle("Please input presetname:");
                    cnew.ShowDialog();
                }
                else
                {
                    edCpyPreStatus.Text = "You should select a position first.";
                }
            }
            else  //save current sence to local PC
            {
                saveDlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                saveDlg.Filter = "MatrixPresets File|*.MCSP";//matrix Current Scene presetfile
                saveDlg.OverwritePrompt = true;
                if (saveDlg.ShowDialog() == true)
                {
                    string strp = saveDlg.FileName;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(strp);
                    // MessageBox.Show(fileName);
                    CMatrixData.matrixData.setPresetName(0, fileName);
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.resetCommunicateStatus();
                    byte[] mPData = CMatrixData.matrixData.getPackageOfCurrentScene();
                    bool resload = (IOBinaryOperation.writeBinaryToFile(strp, mPData));
                    edCpyPreStatus.Text = (resload ? "Sucessful saved to file!" : "Fail to save to file!");

                    // BitConverter.ToString(mFactoryData);//for show...
                }

            }

        }

        private Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();
        private Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();

        /// <summary>
        /// selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pLstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lbox = sender as System.Windows.Controls.ListBox;
            int index = lbox.SelectedIndex;
            Debug.WriteLine("listbox select index is : {0}", index);
            edCpyPreStatus.Text = string.Format("You have selected position index :{0}.", index);
        }



    }



}
