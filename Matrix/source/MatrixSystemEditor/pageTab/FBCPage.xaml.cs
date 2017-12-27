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
using System.Diagnostics;


namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for FBCPage.xaml
    /// </summary>
    public partial class FBCPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public FBCPage()
        {
            InitializeComponent();
            fbcfft = new FBCDrawFFT(fbcFFTDraw);
            FbcSwitcher.onFBCSwitchValueChangedEvent += new FBCSwitcherControl.FBCSwitchControlChanged(onFBCSwitchChanged);
            fbcSlider.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(fbcSliderValueChanged);
            fbcModSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onFBCSpinChangeHandle);
            fbcRelesSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onFBCSpinChangeHandle);

        }
        /// <summary>
        /// FBC Switcher click now...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="chanelindex"></param>
        private void onFBCSwitchChanged(object sender, int chanelindex)
        {
            //
            FBCSwitcherControl fbc = sender as FBCSwitcherControl;
            Debug.WriteLine("fbc switch chindex is  {0}", chanelindex);

            if (chanelindex == CDefine.FBCLeftChanel) //left 20
            {
                Array.Copy(fbc.m_fbcSwitcher[0],
                   CMatrixData.matrixData.m_matrixAry[CDefine.FBCLeftChanel], CDefine.Matrix_CTL_NUM);


            }
            else //right 0...19
            {
                for (int i = 0; i < 20; i++)
                {
                    CMatrixData.matrixData.m_matrixAry[i][CDefine.FBCChanel] = fbc.m_fbcSwitcher[1][i];

                }

            }

            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_Matrix(chanelindex);

            }
            else
            {
                fbc.refreshControl();
            }


        }
        public void updateWholeFBCMatrixSwitcher_fromData()
        {

            Array.Copy(CMatrixData.matrixData.m_matrixAry[CDefine.FBCLeftChanel],
                          FbcSwitcher.m_fbcSwitcher[0], CDefine.Matrix_CTL_NUM);  //left part channel20           

            for (int i = 0; i < CDefine.Matrix_CTL_NUM; i++)
            {
                FbcSwitcher.m_fbcSwitcher[1][i] = CMatrixData.matrixData.m_matrixAry[i][CDefine.FBCChanel];
            }
            FbcSwitcher.refreshControl();

        }

        //fbc
        private void onFBCSpinChangeHandle(object sender, int cvalue, EQFragType etype, int itg, EventArgs e)
        {
            CMatrixData.matrixData.m_FbcParam[itg] = (byte)cvalue;//fbc gain
            // Debug.WriteLine("fbc spin tag is {0} new value is  {1}......................",itg, cvalue);
           
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_FBCSetting();
            }
            else
                updateFBCSetting_fromData();
        }
        //
        private void fbcSliderValueChanged(object sender, int newPos, EventArgs e)
        {
            CMatrixData.matrixData.m_FbcParam[1] = (byte)newPos;//fbc gain
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_FBCSetting();
            }
            else
                updateFBCSetting_fromData();

        }

        private void fbcAutoSetBtn_Click(object sender, RoutedEventArgs e) //FBC setup
        {
            var btn = sender as CSwitcher;
            byte tmp = 0;

            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;

            CMatrixData.matrixData.m_FbcParam[2] = tmp;
            CMatrixData.matrixData.m_FbcParam[4] = 0;
            CMatrixData.matrixData.m_FbcParam[5] = 0;
            if (NetCilent.netCilent.isConnected())
            {
                //CMatrixData.matrixData.sendCMD_FBCSetting(1);
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_FBCSetup();
            }
            else
            {
                // btn.IsSelected = (tmp > 0);
                //  if (tmp == 1)
                // {
                //  CMatrixData.matrixData.resetFBCData();
                //   this.updateFBCSetting_fromData();
                //  fbcfft.drawEQ();
                // }
                btn.IsSelected = (tmp > 0);
            }


        }



        private void fbcClearDynamicFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            byte tmp = 0;
            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            CMatrixData.matrixData.m_FBCClearFlag[0] = 1;//must be 1
            CMatrixData.matrixData.m_FBCClearFlag[1] = 0;//clear all filters must be 0 

            if (NetCilent.netCilent.isConnected())
            {
                //CMatrixData.matrixData.sendCMD_FBCSetting(1);

                btn.IsSelected = (tmp > 0);
                if (tmp == 1)
                {
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_FBCSetup();
                }
            }
            else
            {
                // btn.IsSelected = (tmp > 0);
                //  if (tmp == 1)
                // {
                //  CMatrixData.matrixData.resetFBCData();
                //   this.updateFBCSetting_fromData();
                //  fbcfft.drawEQ();
                // }
                btn.IsSelected = (tmp > 0);
            }

        }

        private void fbcClearAllFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            byte tmp = 0;
            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            CMatrixData.matrixData.m_FBCClearFlag[0] = 0;//must be setted 0
            CMatrixData.matrixData.m_FBCClearFlag[1] = 1;//clear all filters must be 1

            if (NetCilent.netCilent.isConnected())
            {
                btn.IsSelected = (tmp > 0);
                if (tmp == 1)
                {
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_FBCSetup();
                }
            }
            else
            {
                // btn.IsSelected = (tmp > 0);
                //  if (tmp == 1)
                // {
                //  CMatrixData.matrixData.resetFBCData();
                //   this.updateFBCSetting_fromData();
                //  fbcfft.drawEQ();
                // }
                btn.IsSelected = (tmp > 0);
            }
        }

        private void fbcBypasBtn_Click(object sender, RoutedEventArgs e)
        {
            var cbuton = sender as CSwitcher;
            int item = cbuton.iTag;
            byte tmp = 0;
            if (cbuton.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            //m_FbcParam = new byte[3];//fbc_bypas,fbc_gain,fbc_staticFilterSetttingFlag
            CMatrixData.matrixData.m_FbcParam[0] = tmp;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_FBCSetting();
            }
            else
                updateFBCSetting_fromData();

        } 
       
        private FBCDrawFFT fbcfft;

        public void updateFBCPage_fromData()
        {
            //updateWholeFBCSwitcher();
            updateFBCSetting_fromData();
            updateFBCLedStatus(CMatrixData.matrixData.m_fbcNextStep);
            fbcfft.drawEQ();  //FBC update EQ  

        }
        public void fbcDrawEQ()
        {
            fbcfft.drawEQ();
        }

        public void updateFBCLedStatus(int blinkindex)
        {
            Array.Copy(CMatrixData.matrixData.m_FbcFilterStatus,
              FBCledCTL.m_ledStatus, CDefine.MAX_FBCFilter);//20170722
            FBCledCTL.reflreshLed(CMatrixData.matrixData.m_FbcParam[2], blinkindex);//20170722
        }
        /// <summary>
        /// update fbc setting :gain slider,fbc bypass
        /// </summary>
        public void updateFBCSetting_fromData() //fbc gain,fbc mode,filter relase time
        {

            fbcAutoSetBtn.IsSelected = (CMatrixData.matrixData.m_FbcParam[2] == 1);

            byte tmp = CMatrixData.matrixData.m_FbcParam[0]; //bypass
            fbcBypasBtn.IsSelected = (tmp > 0);
            fbcSlider.IsEnabled = (tmp == 0);
                
            byte tmpv = CMatrixData.matrixData.m_FbcParam[1];//fbc gain
            fbcSetLabel.Text = CFinal.strChFGainTable[tmpv];
            fbcSlider.Posvalue = tmpv;
            //fbc mode and fbc filter rease time
            byte fbcmod = CMatrixData.matrixData.m_FbcParam[4];
            if (fbcmod > 1)
                fbcmod = 1;
            byte fbcFilterRlease = CMatrixData.matrixData.m_FbcParam[5];
            if (fbcFilterRlease > 2)
                fbcFilterRlease = 2;
            //
            fbcModSpin.Value = fbcmod;
            fbcRelesSpin.Value = fbcFilterRlease;
            //
            fbcModSpin.valueTxt = CDefine.strFBCMode[fbcmod];
            fbcRelesSpin.valueTxt = CDefine.strFBCFilterRelease[fbcFilterRlease];
            //           
            this.updateFBCLedStatus(CMatrixData.matrixData.m_fbcNextStep);
        }

    }
}
