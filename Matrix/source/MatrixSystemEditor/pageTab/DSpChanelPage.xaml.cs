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
using Lib.Controls;
using System.Diagnostics;


namespace MatrixSystemEditor.pageTab
{
    /// <summary>
    /// Interaction logic for DSpChanelPage.xaml
    /// </summary>
    public partial class DSpChanelPage : Page
    {
        private MatrixPage _parentWin;
        private CDrawerFFT myfft;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public void updateDynMeter(byte gain)
        {
            dynMeter.posvalue = gain;
        }

        /// <summary>
        /// exp gate meter (input only)
        /// </summary>
        /// <param name="chindex">base 0..11</param>
        public void updateCurrentChanelExpGateMeter()
        {

            int chindex = _parentWin.currentchindex;//current chindex  is :base 0...23

            if (chindex < 0 || chindex >= CMatrixFinal.Max_MatrixChanelNum) return;

            byte bpas = CMatrixData.matrixData.m_ChanelEdit[chindex].gateExpData.limit_bypas;

            if (bpas == 1)
                expLed.LedStatus = LED_Status.LED_Normal;
            else
            {
                byte dynMeterGain = CMatrixData.matrixData.compMeterLedvalue[chindex];
                byte gflag = CMatrixData.matrixData.igateExpMeterLedValue[chindex];
                if (gflag == 1 && dynMeterGain == 0)
                    expLed.LedStatus = LED_Status.LED_Red;
                else
                    expLed.LedStatus = LED_Status.LED_Green;
            }
        }

        public DSpChanelPage()
        {
            InitializeComponent();
            sensiSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onSensitivityChange);
            myfft = new CDrawerFFT(FFTConstaint.CEQ_MAX, userfft);
            userfft.onFFTMouseMoveEvent += new FFTDrawer.fftDrawMouseMove(onfftDrawMouseMove);
            userfft.onFFTMouseDownEvent += userfft_onFFTMouseDownEvent;
            userfft.onFFTMouseUpEvent += userfft_onFFTMouseUpEvent;
            delayCtl.onDelayControlValueChangedEvent += new CDelayControl.delayControlValueChanged(onDelayControlChangeEvent);
            delayCtl.onDelayEditClickEvent += new CDelayControl.delayEditClick(onDelayEditClick);
            delayCtl.onDelayEditKeyDownEvent += new CDelayControl.delayEditKeyDown(onDelayKeyDownClick);
            delayCtl.onDelayEditLostFocusEvent += new CDelayControl.delayEditLostFocus(DelayCtl_onDelayEditLostFocusEvent);

            dynGSlider.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(dynGainValueChanged);


            leftNoiseControl.onNoiseGateControlChangedEvent += new CNoiseGateAjustControl.noiseGateControlChanged(onNoiseControlChanged);
            rightNoiseControl.onNoiseGateControlChangedEvent += new CNoiseGateAjustControl.noiseGateControlChanged(onNoiseControlChanged);
            eqControl.onEQControlChangedEvent += new CEQControlII.eqControlChanged(onEqValueChanged);


        }
        public const double MinInput = 0;
        public const double MaxInput = 1361.29;
        private void onDelayKeyDownClick(object sender)
        {
            var txtBox = sender as TextBox;

            double dfout = 0;
            string strTemp = txtBox.Text.Trim();
            if (Double.TryParse(strTemp, out dfout))
            {
                double inputValue = CDefine.checkAvalibleInput(dfout, MinInput, MaxInput);
                int position = CDelayControl.decodeDelayvalue(inputValue);
                txtBox.Text = inputValue.ToString();
                int chindex = _parentWin.currentchindex;

                CMatrixData.matrixData.m_ChanelEdit[chindex].delayTime = position;

                if (NetCilent.netCilent.isConnected())
                {                 
                   CMatrixData.matrixData.sendCMD_ChanelDelay(chindex);
                }
                else
                {
                    updateDelay_fromData(chindex);

                }
            }

            txtBox.IsReadOnly = true;



        }
        private void DelayCtl_onDelayEditLostFocusEvent(object sender)
        {
            var txtBox = sender as TextBox;
            //double dfout = 0;
            int chindex = _parentWin.currentchindex;            
            updateDelay_fromData(chindex);
            txtBox.IsReadOnly = true;
            
        }

        private void onDelayEditClick(object sender)
        {
            var txtBox = sender as TextBox;
            txtBox.Text = "";
            txtBox.IsReadOnly = false;
        }

        private void userfft_onFFTMouseUpEvent(Point px, MouseButtonEventArgs e)
        {
            Debug.WriteLine("fft mouse up now....");
            if (NetCilent.netCilent.isConnected())
                _parentWin.ackTimer.Start();
        }

        private void userfft_onFFTMouseDownEvent(Point px, MouseButtonEventArgs e)
        {
            Debug.WriteLine("fft mouse down.now...");
            if (NetCilent.netCilent.isConnected())
                _parentWin.ackTimer.Stop();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="freqindex"></param>
        /// <param name="gaindex"></param>
        /// <param name="pkindex"></param>
        /// <param name="e"></param>
        public void onfftDrawMouseMove(int freqindex, int gaindex, int qindex, int pkindex, MouseEventArgs e)
        {

            if (pkindex >= 0 && CMatrixData.matrixData != null)
            {
                if (CMatrixData.matrixData.isReady)  //williamxia20170419
                {

                    if (NetCilent.netCilent.isConnected())
                        _parentWin.ackTimer.Stop();
                    int chindex = _parentWin.currentchindex;

                    if (CMatrixData.matrixData.m_ChanelEdit[chindex].eqAll_bypas == 1) return;


                    if (freqindex == FFTConstaint.NONFreq && qindex != FFTConstaint.NONQvalue)  //just qvalue changed
                    {
                        CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[pkindex].eq_qfactorindex = (byte)qindex;
                    }
                    else if (freqindex != FFTConstaint.NONFreq && qindex == FFTConstaint.NONQvalue) //freq and gain changed
                    {
                        CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[pkindex].eq_freqindex = freqindex;
                        CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[pkindex].eq_gainindex = (byte)gaindex;

                    }

                    Debug.WriteLine("gain index is : {0} chindex is : {1}", gaindex, chindex);
                    //double sfreq = FFTConstaint.FreqTable[freqindex];
                    //double sgain = FFTConstaint.EqGain[gaindex];

                    if (NetCilent.netCilent.isConnected())
                    {

                        _parentWin.onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_ChanelEQ(chindex, pkindex);

                    }
                    else
                    {
                        myfft.drawEQ(chindex);
                        updateSingleEQControl_fromData(chindex, pkindex);
                    }
                    CMatrixData.matrixData.isReady = false;
                }

#if KO_
                if (XCoreData.gCilent().isConnected())
                {
                    int chindex = 0;
                    if (pkindex < 8)
                        XCoreData.sendCMD_EQ(chindex, pkindex);
                    else
                    {
                        //int 

                        byte hltype = (byte)(pkindex == 8 ? 1 : 0);
                        XCoreData.sendCMD_HLPF(chindex, hltype);
                    }

                }
                else
                {
                    myfft.drawEQ();
                    updateEQ_GUI_fromCoreData(pkindex);

                }
#endif


            }

            //  Debug.WriteLine("onfftDraw mousemove  hre....");
        }

        private void dc48VBtn_Click(object sender, RoutedEventArgs e)
        { //
            byte tmp = 0;
            int chindex = _parentWin.currentchindex;
            var btn = (CSwitcher)sender as CSwitcher;
            if (btn.IsSelected)
            {
                tmp = 1;
            }
            else
            {
                tmp = 0;
            }
            CMatrixData.matrixData.m_ChanelEdit[chindex].DC48VFlag = tmp;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_DC48V();
            }
            else
                updateDC48V(chindex);

        }
        public void updateDspPage(int chindex)
        {
            //left part
            updatePolarity(chindex); //polarity ----------------
            updateDC48V(chindex);
            updateSensitivity(chindex);
            updateNoiseJustControl_fromData(chindex, 0);

            //mid
            updateWholeEQControl_fromData(chindex);

            //right part
            updateNoiseJustControl_fromData(chindex, 1);
            updateDelay_fromData(chindex);
            updateDynGainSlider_fromData(chindex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chindex">0..23</param>
        public void switchEQControl(int chindex)
        {
            Debug.WriteLine("switch eq control with chindex is........... {0}", chindex);
            if (chindex < CFinal.ChanelMax) //chindex<12
            {

                eqControl.InputType = CEQControlII.InputI;//20170722
                Debug.WriteLine("eqcontrol inputype is setted inputI...........");

            }
            else
            {
                eqControl.InputType = CEQControlII.InputII;//20170722
                Debug.WriteLine("eqcontrol inputype is setted inputII...........");
            }


        }

        public void switchPanelWithChannel(int chindex) //switchEQcontrol is first invoked
        {
            switchEQControl(_parentWin.currentchindex);
            leftxExpGateGrid.Visibility = (chindex >= CFinal.ChanelMax ? Visibility.Hidden : Visibility.Visible);
            leftNoiseControl.Visibility = (chindex >= CFinal.ChanelMax ? Visibility.Hidden : Visibility.Visible);

            // rightDynControlPanel.Visibility = (chindex >= CFinal.ChanelMax ? Visibility.Visible : Visibility.Hidden);
            //  rightNoiseControl.Visibility = (chindex >= CFinal.ChanelMax ? Visibility.Visible : Visibility.Hidden);
            dc48VBtn.Visibility = (chindex >= CFinal.ChanelMax - 4 ? Visibility.Hidden : Visibility.Visible);
            gpxSensi.Visibility = (chindex >= CFinal.ChanelMax - 4 ? Visibility.Hidden : Visibility.Visible);

        }


        /// <summary>
        /// update gui
        /// </summary>
        /// <param name="chindex"></param>
        public void updateDC48V(int chindex)
        {
            byte tmp = CMatrixData.matrixData.m_ChanelEdit[chindex].DC48VFlag;
            dc48VBtn.IsSelected = (tmp == 0);
        }
        private void ploarityBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            int chindex = _parentWin.currentchindex;
            CMatrixData.matrixData.m_ChanelEdit[chindex].invert = tmp;

            Debug.WriteLine("polarity click now.... phase is : {0}", tmp);
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_Phase(chindex);
            }
            else
                updatePolarity(chindex);

        }
        /// <summary>
        /// sensitivity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cvalue"></param>
        /// <param name="etype"></param>
        /// <param name="itg"></param>
        /// <param name="e"></param>
        private void onSensitivityChange(object sender, int cvalue, EQFragType etype, int itg, EventArgs e)
        {
            //
            int idex = cvalue;
            string strDB = CMatrixFinal.strSensitivity[idex];
            CMatrixData.matrixData.m_ChanelEdit[_parentWin.currentchindex].sensitivityindex = (byte)cvalue;

            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_sensitivity();
            }
            else
                updateSensitivity(_parentWin.currentchindex);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chindex"></param>
        public void updateSensitivity(int chindex)
        {

            int index = CMatrixData.matrixData.m_ChanelEdit[chindex].sensitivityindex;
            sensiSpin.Value = index;
            sensiSpin.valueTxt = CMatrixFinal.strSensitivity[index];
        }

        /// <summary>
        /// polarity
        /// </summary>
        /// <param name="chindex"></param>
        public void updatePolarity(int chindex)
        {
            byte invt = CMatrixData.matrixData.m_ChanelEdit[chindex].invert;
            ploarityBtn.IsSelected = (invt > 0);//20170722
            edPloarity.Text = (invt > 0 ? "180°" : "0°");//20170722
            Debug.WriteLine("polarity update with chindex :{0} phase value {1}", chindex, invt);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newPos"></param>
        /// <param name="e"></param>
        private void dynGainValueChanged(object sender, int newPos, EventArgs e)
        {
            //
            int chindex = _parentWin.currentchindex;
            CMatrixData.matrixData.m_ChanelEdit[chindex].dynLimitData.limit_gain = (byte)newPos;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_Compressor(chindex);
            }
            else
                updateDynGainSlider_fromData(chindex);

        }

        /// <summary>
        /// update DynGain GUI slider below
        /// </summary>
        /// <param name="chindex"></param>
        public void updateDynGainSlider_fromData(int chindex)
        {
            int gaindex = CMatrixData.matrixData.m_ChanelEdit[chindex].dynLimitData.limit_gain;
            dynGSlider.Posvalue = gaindex;
            string strGain = CFinal.strCompGainTable[gaindex];
            dynEdGain.Text = strGain;

        }

        private void onDelayControlChangeEvent(object sender)
        {
            CDelayControl mcontrol = sender as CDelayControl;
            // Debug.WriteLine("mcontrol delayvalue :{0} delay status{1}", mcontrol.delayPosvalue, mcontrol.delayByPas);
            int chindex = _parentWin.currentchindex;
            CMatrixData.matrixData.m_ChanelEdit[chindex].delayTime = mcontrol.delayPosvalue;
            // Debug.WriteLine("delaytime is  {0}", CMatrixData.matrixData.m_ChanelEdit[chindex].delayTime);

            CMatrixData.matrixData.m_ChanelEdit[chindex].delayPower = mcontrol.delayByPas;
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_ChanelDelay(chindex);
            }
            else
                updateDelay_fromData(chindex);

        }
        /// <summary>
        /// update delay
        /// </summary>
        /// <param name="chindex"></param>
        public void updateDelay_fromData(int chindex)
        {
            delayCtl.delayPosvalue = CMatrixData.matrixData.m_ChanelEdit[chindex].delayTime;
            Debug.WriteLine("msg prepare to update with value:  {0}", CMatrixData.matrixData.m_ChanelEdit[chindex].delayTime);
            delayCtl.delayByPas = CMatrixData.matrixData.m_ChanelEdit[chindex].delayPower;           
            delayCtl.refreshControl();
        }


        /// </summary>
        /// <param name="chindex"></param>
        /// <param name="flag"></param>
        public void updateNoiseJustControl_fromData(int chindex, int flag) //0:expGate 1:compressor
        {

            CNoiseGateAjustControl gcontrol = null;

            if (flag == 0)
            {
                gcontrol = leftNoiseControl; //means the adjust parameters control
                gcontrol.glimtData.copyLimitData(CMatrixData.matrixData.m_ChanelEdit[chindex].gateExpData);
                leftExpGateControl.setLimitData_fresh(CMatrixData.matrixData.m_ChanelEdit[chindex].gateExpData);



            }
            else
            {
                gcontrol = rightNoiseControl;//means the right adjust parameters control
                gcontrol.glimtData.copyLimitData(CMatrixData.matrixData.m_ChanelEdit[chindex].dynLimitData);
                rightExpGateControl.setLimitData_fresh(CMatrixData.matrixData.m_ChanelEdit[chindex].dynLimitData);

                updateDynGainSlider_fromData(chindex);//compressor gdslider 
            }
            gcontrol.refreshControl();


        }



        /// <summary>
        /// CEQControl changeEQEvent below
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eqindex"></param>
        private void onEqValueChanged(object sender, int eqindex)
        {
            //var eqctl = sender as CEQControlII;          
            int chindex = _parentWin.currentchindex;
            bool refresh = false;
            if (eqindex == CFinal.EQEntireBypass) //eqcontrol do bypass entirely
            {
                CMatrixData.matrixData.m_ChanelEdit[chindex].eqAll_bypas = eqControl.mEntireByPass;//entire eqbypass value assign //20170722

                if (NetCilent.netCilent.isConnected())
                {
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_ChanelEQ(chindex, 0);
                }
                else
                {
                    eqControl.updateWholeBypassStatus();//20170722
                }

                refresh = false;

            }
            else if (eqindex == CFinal.EQEntireFlat) //eqflat
            {

                //eq flat data below.............
                CMatrixData.matrixData.setUser_EQFlat(chindex);
                for (int i = 0; i < CFinal.NormalEQMax; i++)
                {
                    eqControl.m_eqEdit[i].copyEQ(CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[i]);//20170722
                }

                ///send cmd eq..
                if (NetCilent.netCilent.isConnected())
                {
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_EQFlat(chindex);
                }
                else
                {
                    updateWholeEQControl_fromData(chindex);
                }

                refresh = true;

            }
            else //normall eq controll change ,not bypass,not entire flat [0..9]
            {
                refresh = true;
                CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[eqindex].copyEQ(eqControl.m_eqEdit[eqindex]);//20170722
                if (NetCilent.netCilent.isConnected())
                {
                    _parentWin.onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_ChanelEQ(chindex, eqindex);//keep the local data is same with control
                }
                else
                    updateSingleEQControl_fromData(chindex, eqindex); ///1111


            }

            if (refresh && !NetCilent.netCilent.isConnected())
                myfft.drawEQ(chindex);
            Debug.WriteLine("eq index is :  {0}", eqindex);
        }
        /// <summary>
        /// for update in use when recevie from remote EQ
        /// </summary>
        /// <param name="chindex"></param>
        /// <param name="eqindex"></param>
        public void updateEQPart_fromData(int chindex, int eqindex)
        {
            updateSingleEQControl_fromData(chindex, eqindex);
            myfft.drawEQ(chindex);
        }

        /// <summary>
        /// update single eqcontrol from data now.
        /// </summary>
        /// <param name="chindex"></param>
        /// <param name="eqindex"></param>
        public void updateSingleEQControl_fromData(int chindex, int eqindex)
        {
            if (eqindex >= 0 && eqindex < CFinal.NormalEQMax)
            {
                eqControl.m_eqEdit[eqindex].copyEQ(CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[eqindex]);

                eqControl.mEntireByPass = CMatrixData.matrixData.m_ChanelEdit[chindex].eqAll_bypas;
                eqControl.updateGUI_withEqindex(eqindex);
                eqControl.refreshControl();

            }
            Debug.WriteLine("update singele eqcontrol.....");
        }

        ///---------------------
        public void updateWholeEQControl_fromData(int chindex)
        {

            for (int i = 0; i < CFinal.NormalEQMax; i++)
            {
                eqControl.m_eqEdit[i].copyEQ(CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[i]);
            }
            eqControl.mEntireByPass = CMatrixData.matrixData.m_ChanelEdit[chindex].eqAll_bypas;
            eqControl.refreshControl();
            myfft.drawEQ(chindex);
            // Debug.WriteLine("update whole  eqcontrol.....");          

        }

        public void updateEQControl_fromData(int chindex, int eindex)
        {
            eqControl.m_eqEdit[eindex].copyEQ(CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[eindex]);
            eqControl.updateGUI_withEqindex(eindex);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void onNoiseControlChanged(object sender)
        {
            var mcontrol = sender as CNoiseGateAjustControl;
            int tg = mcontrol.iTag;//0:left expGate,1:compressor noisegate
            int chindex = _parentWin.currentchindex;
            if (tg == 0)
            {
                CMatrixData.matrixData.m_ChanelEdit[chindex].gateExpData.copyLimitData(mcontrol.glimtData);

            }
            else if (tg == 1)
            {
                CMatrixData.matrixData.m_ChanelEdit[chindex].dynLimitData.copyLimitData(mcontrol.glimtData);


            }
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;

                if (tg == 0)

                    CMatrixData.matrixData.sendCMD_InputGateExp(chindex);
                else
                    CMatrixData.matrixData.sendCMD_Compressor(chindex);

            }
            else
            {
                updateNoiseJustControl_fromData(chindex, tg);
            }



        }























    }
}
