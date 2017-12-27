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
using Lib.Controls;
using CommLibrary;


namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for AutoMixerPage.xaml
    /// </summary>
    public partial class AutoMixerPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public AutoMixerPage()
        {
            InitializeComponent();
            autoMixerChSelect.onAutoMixerValueChanged += new AutoMixerInput.autoMixerValueChanged(autoMixerMatrixChanged);
            autoMixerSder.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(onAutoMixerSliderValueChanged);

        }
        private void onAutoMixerSliderValueChanged(object sender, int newPos, EventArgs e)
        {
            //
            CMatrixData.matrixData.autoMixerParam.autoRelease = newPos;
            if (NetCilent.netCilent.isConnected())
            {

                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_AutoMixerSetting();
            }
            else
            {
              updateAutoMixerParam_fromData();
            }


        }
        private void autoMixerMatrixChanged(object sender)
        {
            //

            Array.Copy(autoMixerChSelect.m_SourchInput,
                CMatrixData.matrixData.m_autoMixerCHSelect, CMatrixFinal.Max_MatrixChanelNum);
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_AutoMixerSelect();
            }
            else
            {
                autoMixerChSelect.refreshControl();
            }

        }

        public void updateAutoMixerCHSelect_fromData()
        {
            //2017
            Array.Copy(CMatrixData.matrixData.m_autoMixerCHSelect,
              autoMixerChSelect.m_SourchInput, CMatrixFinal.Max_MatrixChanelNum);
             autoMixerChSelect.refreshControl();
        }


        private void AutoPowerBtn_Click(object sender, RoutedEventArgs e)
        {
            byte tmp = 0;
            var btn = sender as CSwitcher;
            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            CMatrixData.matrixData.autoMixerParam.autoPower = tmp;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_AutoMixerSetting();
            }
            else
            {

                updateAutoMixerParam_fromData();
            }


        }
      
        public void updateAutoMixerParam_fromData()
        {
            byte mp = CMatrixData.matrixData.autoMixerParam.autoPower;
            AutoPowerBtn.IsSelected = (mp > 0);
            string name = (mp > 0 ? "ON" : "OFF");
            autoMixerSder.IsEnabled = (mp > 0);
            //
            int release = CMatrixData.matrixData.autoMixerParam.autoRelease;
            if (release > 24)
                release = 23; //williamxia20161230
            AutoPowerBtn.Content = name;
            autoMixerSder.Posvalue = release;
            edActiveTime.Text = CDefine.strAutoReleseTable[release];
        }


    }
}
