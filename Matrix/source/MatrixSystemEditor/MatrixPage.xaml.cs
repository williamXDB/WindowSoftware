#define defFBCShow
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
using System.Diagnostics;
using Lib.Controls;
using CommLibrary;

using MatrixSystemEditor.Matrix;
using MatrixSystemEditor.commom;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using MatrixSystemEditor.pageTab;
using Lib.Controls;

/*
 *  
 <Button Height="100" Width="100">
  <StackPanel>
    <Image Source="img.jpg" />
    <TextBlock Text="Blabla" />
  </StackPanel>
</Button>   
 * 
 * main process function alocated with void UART0_Local_Machine_Parameter_Deal(void) function
 * DC48V,Sensitivity 在9到12通道是没有的，要隐藏才行
 */

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for MatrixPage.xaml
    /// </summary>
    public partial class MatrixPage : Window
    {

        public int currentchindex = 0;
        // private ListenerApparatus listenApparatus = new ListenerApparatus();
        public DispatcherTimer ackTimer = new DispatcherTimer();
        private DispatcherTimer recallListenTimer = new DispatcherTimer();
        private DispatcherTimer onLineListenTimer = new DispatcherTimer();
        private MessageReceiver m_MsgRecver;

        public MatrixPage()
        {

            CMatrixData.shareData();
            InitializeComponent();
            initAllTabpages();
            isSerialModeType = true;

            //  if (listenApparatus == null)
            //  listenApparatus = new ListenerApparatus();
            //       
            // listenApparatus.onListenDoEvent += new ListenerApparatus.listenDoEvent(onListeningDo);
            //lslider_0.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(mySliderValueChanged);
            initialFaderSlider();
            initialButtonRightClick();

            if (m_MsgRecver == null)
                m_MsgRecver = new MessageReceiver(MatrixCMD.MatrixGUIClass, null);
            m_MsgRecver.WndProc += WindowMsg_recevierWndProc;



            if (ackTimer == null)
                ackTimer = new DispatcherTimer();
            ackTimer.Interval = new TimeSpan(0, 0, 4);
            ackTimer.Tick += ackTimer_Tick;
            ackTimer.Stop();

            if (recallListenTimer == null)
                recallListenTimer = new DispatcherTimer();
            recallListenTimer.Interval = new TimeSpan(0, 0, 3);
            recallListenTimer.Tick += recallListenTimer_Tick;
            recallListenTimer.Stop();


            if (onLineListenTimer == null)
            {
                onLineListenTimer = new DispatcherTimer();
            }
            onLineListenTimer.Interval = new TimeSpan(0, 0, 3);
            onLineListenTimer.Tick += onLineListenTimer_Tick;
            onLineListenTimer.Stop();
            //    
            if (verifyIPList == null)
                verifyIPList = new List<string>();


            cbxDevList.SelectionChanging += new SelectionChangingEventHandler(devComboxList_selectChanging);
            LimitedStrProperty.AddNewGroup("ContentLimit", Limit_Len);

        }
        public const int Limit_Len = 8;

        private void devComboxList_selectChanging(object sender, SelectionChangingEventArgs e)
        {            //
            TComboBox cbx = sender as TComboBox;
            if (NetCilent.netCilent.isConnected())
            {
                int filterindex = cbx.SelectedIndex;
                CMatrixData.matrixData.setCurrentDevId(filterindex);
                labelDevID.Text = CMatrixData.matrixData.getStrDevID(filterindex);

                ackTimer.Stop();
                onLineCheckCounter = 0;
                CMatrixData.matrixData.procesRecallAckComStatus();
                CMatrixData.matrixData.sendCMD_recallCurrence(true);
                recallReturn = false;
                recallListenTimer.Start();

                LoadLedForm lodingForm = new LoadLedForm();
                lodingForm.ParentWindow = this;
                lodingForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                lodingForm.ShowDialog();

            }

        }


        public void runSyncWindow()
        {

            if (lodf != null)
            {
                lodf.WindowStartupLocation = WindowStartupLocation.Manual;
                lodf.ParentWindow = this;
                lodf.ShowDialog();
            }
        }

        public void cxhWindow()
        {
            /*
            WindowStartupLocation = WindowStartupLocation.Manual;
            Application curApp = Application.Current;
            Window mainWind = curApp.MainWindow;
            this.Left = mainWind.Left + (mainWind.ActualWidth - this.ActualWidth) / 2;
            this.Top = mainWind.Top + (mainWind.ActualHeight - this.ActualHeight) / 2;
            */
            if (NetCilent.netCilent.isConnected())
                runSyncWindow();

        }

        public void clearTimer()
        {
            onLineListenTimer.Stop();
            ackTimer.Stop();
            recallListenTimer.Stop();

        }


        /// <summary>
        /// listen to check online or offline 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onLineListenTimer_Tick(object sender, EventArgs e)
        {
            onLineCheckCounter++;

            if ((onLineCheckCounter == CMatrixData.matrixData.m_CommuStatus.responseAckCounter)
                 && (CMatrixData.matrixData.m_CommuStatus.commuteStatus != ACK_Status.M_ConectedNormal) && !isDefaultSetting_beging)
            {
                onLineCheckCounter = 0;
                Debug.WriteLine("Listen detect  comustestatus {0}  response time {1}  matrix A8 is offline %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%"
                    , CMatrixData.matrixData.m_CommuStatus.commuteStatus, CMatrixData.matrixData.m_CommuStatus.responseAckCounter
                    );
                conLed.LedStatus = LED_Status.LED_Normal;
                onLineListenTimer.Stop();
                ackTimer.Stop();

                if (NetCilent.netCilent != null)
                {
                    NetCilent.netCilent.disConnect();
                }
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                          (ThreadStart)delegate()
                          {
                              clearAllChanlMeter();
                              //listenApparatus.stopListen();
                          });
            }

            if (onLineCheckCounter > ACK_Status.Max_Status_Counter)
            {
                onLineCheckCounter = 0;
            }

        }

        private int circleCounter = 0;
        private bool recallReturn = false;

        private void recallListenTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            circleCounter++;
            if (circleCounter == 8 && !recallReturn)
            {
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_recallCurrence();
                Debug.WriteLine("recall sence  the senconde time....................");

            }
            else if (circleCounter == CMatrixData.matrixData.getSpecialRecall(4) && !recallReturn)
            {
                circleCounter = 0;
                recallReturn = true;
                recallListenTimer.Stop();
                MessageBox.Show("Racall current scene failure, the device is not ready, please check.");
            }
            Debug.WriteLine("recall current sence listen now.....");
        }




        public int onLineCheckCounter = 0; //for check counter listener


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ackTimer_Tick(object sender, EventArgs e)
        {
            if (NetCilent.netCilent.isConnected())
            {
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_ACK();
                CMatrixData.matrixData.setupCommunicateACKStatus();
            }


        }




        public void showDeviInfo()
        {
            labelAPID.Text = CMatrixData.matrixData.devProvision.pMachineID.ToString("X2");
            labelDevID.Text = CMatrixData.matrixData.devProvision.pDeviceID.ToString("X4");
            cbxDevList.Text = labelDevID.Text;
            this.Title = "MatrixA8 Editor";
        }

        private void onListeningDo(object sender, EventArgs e)
        {
            //
            Debug.WriteLine("on listening check.....outside");
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                          (ThreadStart)delegate()
                          {
                              sysPge.setRotateSpinStatus(false);
                              //listenApparatus.stopListen();
                          });

        }
        public void sendMSG_note_closeLoding()
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.LoadLEDGUIClass, MatrixCMD.LoadLed_MSG_Transfer, 0, 0);

        }

        /// <summary>
        /// Message process body below
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            //
            if (e.Message == MatrixCMD.MatrixA8_MSG_Transfer)
            {
                if (CMatrixData.matrixData.m_CommuStatus.commuteStatus != CDefine.M_ResetDefaultFactory &&
                   CMatrixData.matrixData.m_CommuStatus.commuteStatus != CDefine.M_ResetDefaultSetting)
                {
                    onLineCheckCounter = 0; //reset the counter 
                    CMatrixData.matrixData.resetCommunicateStatus();
                }

                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                int chindex = cds.preWpl;//low param
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.F_InputDG411Gain://sensitivity
                        {
                            Debug.WriteLine("receive cmd message sensitivity command over...............");
                            dspChangePge.updateSensitivity(currentchindex);
                        }
                        break;
                    case MatrixCMD.F_InputDC48VFlag:
                        {
                            Debug.WriteLine("receive cmd message dc48vflag command over...............");
                            dspChangePge.updateDC48V(currentchindex);
                        }
                        break;

                    case MatrixCMD.F_SigMeters:
                        {
                            // Debug.WriteLine("receive cmd message sigmeter command over...............");
                            refreshAllMeters();
                        }
                        break;
                    case MatrixCMD.F_SetPageZoneIndex:
                        {
                            // this.updateZone();
                            Debug.WriteLine("receive cmd message pagezone command over...............");
                        }
                        break;
                    case MatrixCMD.F_AutoMixerSetting:
                        {

                            autoMxPge.updateAutoMixerParam_fromData();
                        }
                        break;
                    case MatrixCMD.F_AutoMixerCHSelect:
                        {
                            autoMxPge.updateAutoMixerCHSelect_fromData();
                            Debug.WriteLine("msg receive automixer chselect now...................");

                        }
                        break;
                    case MatrixCMD.F_RelayControl:
                        {
                            sysPge.updateRelay();
                            Debug.WriteLine("receive cmd message relay command over...............");
                        }
                        break;
                    case MatrixCMD.F_StoreSinglePreset:

                        break;

                    case MatrixCMD.F_RecallSinglePreset:
                    case MatrixCMD.F_LoadFromPC:
                        {
                            isDefaultSetting_beging = false;
                            sysPge.setRotateSpinStatus(false);
                            turnHeadPageWithiTag(0);
                            turnChanel(0, true);
                            // rootTab.SelectedIndex = 0; //20170722

                        }
                        break;

                    case MatrixCMD.F_Ack:
                        {

                        }
                        break;
                    ///ducker receiver
                    case MatrixCMD.F_DuckerParameter:
                        {
                            Debug.WriteLine("ducker param receive begin..................");
                            duckPge.updateDuckerParameters_fromData();
                            Debug.WriteLine("ducker parameters message receive over....");
                        }
                        break;
                    case MatrixCMD.F_DuckerGainInsert:
                    case MatrixCMD.F_DuckerInputMixer:
                        {
                            duckPge.updateDuckerMixer();
                            Debug.WriteLine("ducker input mixer message receive over....");
                        }
                        break;
                    case MatrixCMD.F_GetFBCStatus:
                        {

                            fbcPge.updateFBCLedStatus(CMatrixData.matrixData.m_fbcNextStep);

                            if (chindex > 0)
                            {
                                fbcPge.fbcDrawEQ();
                                // FBCledCTL.setBlinkindex(CMatrixData.matrixData.m_fbcNextStep);
                            }
                            Debug.WriteLine("FBC getstatus.message.. over. status: {0}, nextstep:{1}.", chindex, CMatrixData.matrixData.m_fbcNextStep);
                            // updateFBCShowTest(CMatrixData.matrixData.m_fbcNextStep);

                        }
                        break;
                    case MatrixCMD.MSG_TCP_ParaConnected:
                        {
                            Debug.WriteLine("tcp msg after connect and refresh now........");

                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                  (ThreadStart)delegate()
                                  {
                                      afterParalConnect();
                                  });

                        }
                        break;

                    case MatrixCMD.F_FBCSetting:
                        {
                            fbcPge.updateFBCSetting_fromData();
                        }
                        break;
                    case MatrixCMD.F_FBCSetup:
                        {
                            //  fbcAutoSetBtn.IsSelected = (CMatrixData.matrixData.m_FbcParam[2] == 1); //fbc static flag setting //20170722
                            if (CMatrixData.matrixData.m_FBCClearFlag[0] == 1)
                            {
                                CMatrixData.matrixData.m_FBCClearFlag[0] = 0;
                                //   fbcClearDynamicFilterBtn.IsSelected = false;//20170722

                            }
                            else if (CMatrixData.matrixData.m_FBCClearFlag[1] == 1)
                            {
                                CMatrixData.matrixData.m_FBCClearFlag[1] = 0;
                                //fbcClearAllFilterBtn.IsSelected = false; //20170722
                            }

                            if (CMatrixData.matrixData.m_FbcParam[2] == 1) //fbc static filter
                            {
                                CMatrixData.matrixData.resetFBCWhenSetup_Pressed(); //reset for myself


                            }
                            fbcPge.updateFBCSetting_fromData();

                        }
                        break;

                    case MatrixCMD.F_GetPresetList:
                        {
                            Debug.WriteLine("begin read message preset list....");
                            //  Debug.WriteLine("over read preset list....");
                            savePge.refreshPresetListBox();
                            CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_StoreSinglePreset, 0);
                            ackTimer.Start();
                        }
                        break;
                    case MatrixCMD.F_InpuPhase:
                    case MatrixCMD.F_OutputPhase:
                        {

                            dspChangePge.updatePolarity(currentchindex);
                        }
                        break;
                    case MatrixCMD.F_InpuGain:
                    case MatrixCMD.F_OutputGain:
                        {
                            Debug.WriteLine("receive msg... to proces phader gain..");
                            updateSliderCaseLeftoRight(chindex);//chindex is the falg for judging 
                        }
                        break;

                    case MatrixCMD.F_MemoryExport:
                        {
                            savePge.processExport(chindex);
                            if (chindex == CDefine.Memory_Max_Package - 1)
                            {
                                popupDialogForChoose();
                                ackTimer.Start();
                            }

                        }

                        break;
                    case MatrixCMD.F_MemoryImportAck:
                        {
                            savePge.processImport(chindex);

                        }
                        break;


                    case MatrixCMD.F_OutEQFlat:
                    case MatrixCMD.F_InputEQFlat:
                        {
                            Debug.WriteLine("over..receive channel  message eqflat...");
                            dspChangePge.updateWholeEQControl_fromData(currentchindex);
                        }
                        break;
                    case MatrixCMD.F_InputEQ:
                    case MatrixCMD.F_OutputEQ:
                        {
                            //
                            byte[] tmp = cds.getBitAry();
                            int eindex = tmp[0];
                            chindex = tmp[1];
                            dspChangePge.updateEQPart_fromData(chindex, eindex);
                            Debug.WriteLine("over..receive message inputEQ..eqindex: {0} channel index {1}", eindex, chindex);
                        }

                        break;
                    case MatrixCMD.F_MatrixMixer:
                        {

                            if (chindex < CDefine.FBCLeftChanel)
                                subMatrixPge.updateMatrixWithRow_fromData(chindex);
                            fbcPge.updateWholeFBCMatrixSwitcher_fromData();
                            // updateAutoMixerCHSelect_fromData();
                            Debug.WriteLine("over..receive message matrix.command. matrix row is :{0}", chindex);
                        }

                        break;
                    case MatrixCMD.F_ReChName:
                        {
                            updateSingleChanelName(chindex); //chindex:0..23

                        }
                        break;

                    case MatrixCMD.F_Mute:
                        {
                            updateAllChanelMute_fromData();
                            Debug.WriteLine("over..receive message channel mute...");
                        }
                        break;
                    case MatrixCMD.F_OutputCOMP:
                    case MatrixCMD.F_InputCOMP:
                        {
                            dspChangePge.updateNoiseJustControl_fromData(chindex, 1);
                            Debug.WriteLine("over..receive message chanel compressor....");
                        }
                        break;
                    case MatrixCMD.F_OutputDelay:
                    case MatrixCMD.F_InputDelay:
                        {
                            Debug.WriteLine("over..receive message channel delay...chindex {0}", chindex);
                            dspChangePge.updateDelay_fromData(chindex);
                        }
                        break;
                    case MatrixCMD.F_InputExpGATE:
                        {
                            dspChangePge.updateNoiseJustControl_fromData(chindex, 0);
                            Debug.WriteLine("over..receive message channel input expGate....");
                        }
                        break;
                    case MatrixCMD.F_PCGetDeviceInfo:
                    case MatrixCMD.F_RdDevInfo:
                        {
                            sysPge.updateDeviceName();
                            if (!isSerialModeType)
                                setActiveModeName(CMatrixData.matrixData.nameofDevice());
                            CMDSender.sendMSG_note_ChangeDevName(MatrixCMD.F_PCGetDeviceInfo, 0);//stop rotating

                        }
                        break;
                    case MatrixCMD.F_RecallCurrentScene:
                        {
                            recallReturn = true;
                            circleCounter = 0;
                            recallListenTimer.Stop();
                            sendMSG_note_closeLoding();
                            refreshAllpages();

                            if (!isDefaultSetting_beging) //20170413)
                            {
                                //  rootTab.SelectedIndex = 0; //20170722
                            }
                            if (CMatrixData.matrixData.isLowFirmware())
                            {

                                MessageBox.Show(CDefine.Warning_LowMCUVer, "Low Firmware version upgrade reminder!");
                            }
                            ackTimer.Start();
                            onLineListenTimer.Start();
                            popupPwdWindow();//check the lock flag is on or not
                            //  setRotateSpinStatus(false);
                            Debug.WriteLine("receive message current sence over ........................");
                        }
                        break;

                    case MatrixCMD.F_ReturnDefaultSetting:
                    case MatrixCMD.F_ResetToFactorySetting:
                        {
                            isDefaultSetting_beging = false;
                            CMatrixData.matrixData.resetCommunicateStatus();
                            sysPge.setRotateSpinStatus(false);
                            // rootTab.SelectedIndex = 0; //20170722
                            // this.updateWholeEQControl_fromData(currentchindex);

                            //string strm="recevie return from "+(cds.preWph==MatrixCMD.F_ReturnDefaultSetting?" default setting":"reset to factory settings");
                            //Debug.WriteLine(strm);
                            refreshAllpages();
                            turnHeadPageWithiTag(0);
                            turnChanel(0, true);

                        }
                        break;

                    case MatrixCMD.MSG_NoticeDisconnect:
                        {
                            conLed.LedStatus = LED_Status.LED_Normal;
                            if (NetCilent.netCilent.isConnected())
                                NetCilent.netCilent.disConnect();

                            recallListenTimer.Stop();
                            onLineListenTimer.Stop();
                            dtInvoke(clearAllChanlMeter);

                        }
                        break;
                    case MatrixCMD.F_ReadLockPWD:
                        {

                            this.popupPwdWindow();

                        }
                        break;



                }


            }




        }

        public bool isDefaultSetting_beging = false;

        /// <summary>
        /// poup dialog
        /// </summary>
        public void popupDialogForChoose()
        {

            if (MessageBox.Show("Do you want to save to file ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                Debug.WriteLine("you choose yes item to save");
                saveDlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                saveDlg.Filter = CDefine.FILE_MEMORY_Filter;
                saveDlg.OverwritePrompt = true;
                if (saveDlg.ShowDialog() == true)
                {
                    string strp = saveDlg.FileName;
                    IOBinaryOperation.writeBinaryToFile(strp, CMatrixData.matrixData.m_meoryRead);
                }

                savePge.setExportMemoryEnable();

            }


        }

        /// <summary>
        /// update Memory etc.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="lb"></param>


        public void updateAllChannelName()
        {
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                updateSingleChanelName(i);
            }

        }

        /// <summary>
        /// update single channel name to button
        /// </summary>
        /// <param name="chindex">[base 0...23]</param>
        public void updateSingleChanelName(int chindex)
        {
            string strbtn = "";
            CSwitcher sbtn = null;
            // for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            // {
            strbtn = string.Format("ichbtn_{0}", chindex);
            sbtn = (CSwitcher)this.FindName(strbtn);
            if (sbtn != null)
            {
                string strName = CMatrixData.matrixData.nameOfChannel(chindex);
                // sbtn.Content = strName;
                LimitedStrProperty.SetStringContent(sbtn, strName.Trim());
            }
            // }


        }

        public void refreshAllpages()
        {
            // rightStr();           

            turnChanel(currentchindex, true);

            updateAllSliderMute_fromData();

            savePge.refreshPresetListBox();//save load             

            subMatrixPge.updateMatrixPage_fromData();//matrix page update
            fbcPge.updateWholeFBCMatrixSwitcher_fromData();
            //
            updateDuckerPage_fromData();//duckerpage
            //
            fbcPge.updateFBCPage_fromData(); //fbc page
            //

            autoMxPge.updateAutoMixerParam_fromData(); //auto settings
            autoMxPge.updateAutoMixerCHSelect_fromData();//auto chselect
            //            
            sysPge.updateSystemPage();

            updateAllChannelName();
            //
            savePge.stopAllSprocketRotating();
            updateFBCShowTest(0);
            dspChangePge.switchEQControl(currentchindex);
            firmLb.Text = CMatrixData.matrixData.matrixVer();


        }


        /// <summary>
        /// 
        /// </summary>
        public void updateDuckerPage_fromData()
        {
            duckPge.updateDuckerMixer();
            duckPge.updateDuckerParameters_fromData();
        }

        private void zonValueChange(object sender, int cvalue, EQFragType etype, int itg, EventArgs e)
        {

            CMatrixData.matrixData.m_pageZone = (byte)cvalue;
            if (NetCilent.netCilent.isConnected())
            {
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_ZoneChange();
            }
            // else
            //  updateZone();
        }


        /// <summary>
        /// update led status from data
        /// </summary>
        private void mySliderValueChanged(object sender, int newPos, EventArgs e)
        {
            Debug.WriteLine("slide new pos is :  {0}", newPos);
            var slider = sender as CSlider;
            slider.Posvalue = newPos;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tg"></param>
        /// <returns></returns>
        public int tabtnTagToIndex(int tg)
        {
            int index = 0;
            if (tg < 2)
                index = tg;
            else if (tg == 2)
                index = 0;
            else
                index = tg - 1;
            return index;
        }


        #region channel fader about
        /// <summary>
        /// cxh fader
        /// </summary>
        private void initialFaderSlider()
        {
            string strSlider = null;

            CSlider slider = null;
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                strSlider = string.Format("islider_{0}", i);

                slider = (CSlider)this.FindName(strSlider);
                if (slider != null)
                {
                    slider.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(chanelSliderValueChanged);
                    slider.onSliderMouseDownEvent += new CSlider.sliderMouseDown(chanelSliderMouseDown);

                }

            }
            //  MatrixSystemEditor.commom
            /*
             * //20170722   
               initialDuckerSliderAbout();                 
            */

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chanelSliderMouseDown(object sender, EventArgs e)
        {

            var csder = sender as CSlider;
            int chindex = csder.iTag;
            Debug.WriteLine(string.Format("chanel mouse down chindex is :  {0}", chindex));
            turnChanel(chindex, true);

        }

        /// <summary>
        /// GUI update with chindex
        /// </summary>
        /// <param name="chindex"></param>
        public void updateGUI_withChindex(int chindex)
        {
            dspChangePge.updateDspPage(chindex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newPos"></param>
        /// <param name="e"></param>
        private void chanelSliderValueChanged(object sender, int newPos, EventArgs e)
        {

            if (CMatrixData.matrixData.isReady)
            {
                var slider = sender as CSlider;
                //  Debug.WriteLine("slider tag is  {0}  new pos is {1}", slider.iTag,newPos);
                int chindex = slider.iTag;
                CMatrixData.matrixData.setChGain(chindex, (byte)newPos);
                if (NetCilent.netCilent.isConnected())
                {
                    //if no connect  
                    onLineCheckCounter = 0;
                    CMatrixData.matrixData.sendCMD_FaderGain(chindex);
                    updateSliderCaseLeftoRight(0);
                }
                else
                {
                    updateSingleSlider_fromData(chindex);
                }
                CMatrixData.matrixData.isReady = false;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void updateAllChanelMute_fromData()
        {
            int k = 0;
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                updateChannelMute_fromData(i);

            }
        }


        /// <summary>
        /// update all fader slider -------------
        /// </summary>
        public void updateAllSliderMute_fromData()
        {
            int k = 0;
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                updateSingleSlider_fromData(i);
                updateChannelMute_fromData(i);

            }
        }
        public bool isSerialModeType { get; set; } //williamxia20170717

        public void onSerialModeTypeChange(bool isModeSerial) //williamxia20170717
        {
            Thickness ness = new Thickness(1, 0, 1, 10);//deveBordList
            Visibility vb;
            Thickness nesSearch;
            if (isModeSerial)
            {
                ness = new Thickness(1, 10, CDefine.Width_leftScan, 20);//5,3,3,20
                nesSearch = new Thickness(-50, 0, 5, 0);
                vb = Visibility.Hidden;
                SearchDevices.Margin = nesSearch;
                cxhWindow();//20170717
            }
            else
            {
                ness = new Thickness(1, 10, 3, 20);
                nesSearch = new Thickness(2, 0, 5, 0);
                SearchDevices.Margin = nesSearch;
                vb = Visibility.Visible;
                //-----para mode:
                initialSocket_and_Data();
                initialCanClickTimer();
                if (!NetCilent.netCilent.isConnected())//20170726
                    leftRefresh_click();

            }

            left_deveBordList.Margin = ness;
            left_deveBordList.Visibility = vb;
            SearchDevices.Visibility = vb;
        }

        /// <summary>
        /// </summary>
        /// <param name="sts"></param>
        public void startupScanList(bool sts)
        {
            Thickness ness = new Thickness(1, 0, 1, 10);//deveBordList
            Visibility vb;
            //  Thickness nesSearch;
            //Margin="2,0,5,0"
            if (sts)
            {
                vb = Visibility.Visible;
                ness = new Thickness(1, 10, 3, 20);
                //  nesSearch = new Thickness(2, 0, 5, 0);

            }
            else
            {
                vb = Visibility.Hidden;
                ness = new Thickness(1, 10, CDefine.Width_leftScan, 20);//5,3,3,20
                //  nesSearch = new Thickness(-50, 0, 5, 0);
            }
            //  SearchDevices.Margin = nesSearch;

            left_deveBordList.Margin = ness;
            left_deveBordList.Visibility = vb;
        }

        #region proccess update CMeters
        /// <summary>
        /// sigmeter channel about to process
        /// </summary>
        /// <param name="chindex">base 0...23</param>
        public void updateSingleChanlMeter(int chindex)
        {
            if (chindex < 0 || chindex >= CFinal.ChanelMax * 2) return;

            string strMeter = string.Format("iMeter_{0}", chindex);
            byte gain = CMatrixData.matrixData.sigMeterLedvalue[chindex];

            var meter = (CMeter)this.FindName(strMeter);
            if (meter != null)
            {
                meter.posvalue = gain;

            }

        }

        public void clearAllChanlMeter()
        {
            CMatrixData.matrixData.resetSigMeter();
            updateAllChanelMeters();
        }

        public void updateAllChanelMeters()
        {
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum * 2; i++)
            {
                updateSingleChanlMeter(i);
            }
        }




        /// <summary>
        /// input/output compressor
        /// </summary>
        private void updateCurrentChanelCompMeter()
        {
            int chindex = currentchindex;//current chindex  is :base 0...23
            if (chindex < 0 || chindex >= CMatrixFinal.Max_MatrixChanelNum * 2) return;
            byte gain = CMatrixData.matrixData.compMeterLedvalue[chindex];
            if (gain > 16) gain = 16;
            dspChangePge.updateDynMeter(gain);

        }

        public void refreshAllMeters()
        {
            dspChangePge.updateCurrentChanelExpGateMeter();
            updateCurrentChanelCompMeter();
            updateAllChanelMeters();
        }








        #endregion

        public void updateSliderCaseLeftoRight(int flag)
        {
            int i = 0;
            int k = (flag == 0 ? 0 : CFinal.ChanelMax);
            for (i = 0 + k; i < CFinal.ChanelMax + k; i++)
            {
                updateSingleSlider_fromData(i);

            }
        }

        /// <summary>
        /// support 0..23:[0..11]  [12..23]
        /// </summary>
        public void updateSingleSlider_fromData(int chindex)
        {
            if (chindex < 0 || chindex >= CFinal.ChanelMax * 2) return;

            // Debug.WriteLine("update single slider from data with chindex is  {0}", chindex);
            string strSlider = string.Format("islider_{0}", chindex);
            byte gain = CMatrixData.matrixData.m_ChanelEdit[chindex].chGain;


            var slider = (CSlider)this.FindName(strSlider);
            if (slider != null)
            {
                slider.Posvalue = gain;
                double tmp = (0.5 * gain - 80);

                string edGain = String.Format("{0}dB", tmp);
                string strEd = string.Format("ichEd_{0}", chindex);
                var box = (TextBox)this.FindName(strEd);
                if (box != null)
                {
                    box.Text = edGain;
                }
            }

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headPageTabClick(object sender, RoutedEventArgs e)
        {
            //
            CSwitcher sbtn = sender as CSwitcher;
            gotoPage(sbtn.iTag);

        }

        public void gotoPage(int pindex)
        {

            turnHeadPageWithiTag(pindex);
            int tmp = currentchindex;
            if (pindex == 0 || pindex == 2)
            {
                if (tmp >= 0 && tmp < CFinal.ChanelMax && pindex == 2)
                {
                    currentchindex += CFinal.ChanelMax;
                }
                else if (tmp >= CFinal.ChanelMax && tmp < CFinal.ChanelMax * 2 && pindex == 0)
                {

                    currentchindex -= CFinal.ChanelMax;

                }
                turnChanel(currentchindex, false);

            }
            else if (pindex == 4)
            {
                fbcPge.updateFBCLedStatus(CMatrixData.matrixData.m_fbcNextStep);
            }
        }



        DSpChanelPage dspChangePge;
        SubMatrixPage subMatrixPge;
        DuckerPage duckPge;
        FBCPage fbcPge;
        AutoMixerPage autoMxPge;
        SaveLoadPage savePge;
        SystemPage sysPge;

        private void initAllTabpages()
        {
            dspChangePge = new DSpChanelPage();
            subMatrixPge = new SubMatrixPage();

            duckPge = new DuckerPage();
            fbcPge = new FBCPage();
            autoMxPge = new AutoMixerPage();

            savePge = new SaveLoadPage();
            sysPge = new SystemPage();


            dspChangePge.ParentWindow = this;
            subMatrixPge.ParentWindow = this;

            duckPge.ParentWindow = this;
            fbcPge.ParentWindow = this;
            autoMxPge.ParentWindow = this;
            //
            savePge.ParentWindow = this;
            sysPge.ParentWindow = this;

            midFrame.Content = dspChangePge;
        }

        public void turnHeadPageWithiTag(int itg)
        {
            turnHeadTabButton(itg);
            switch (itg)
            {
                case 0:
                    midFrame.Content = dspChangePge;
                    break;
                case 1:
                    midFrame.Content = subMatrixPge;
                    break;
                case 2:
                    midFrame.Content = dspChangePge;
                    break;
                case 3:
                    midFrame.Content = duckPge;
                    break;
                case 4:
                    midFrame.Content = fbcPge;
                    break;
                case 5:
                    midFrame.Content = autoMxPge;
                    break;
                case 6:
                    midFrame.Content = savePge;
                    break;
                case 7:
                    midFrame.Content = sysPge;
                    break;
            }

        }

        private void goToleftScrollEnd_Click(object sender, RoutedEventArgs e)
        {
            leftScroll.ScrollToEnd();
            leftScroll.ScrollToHorizontalOffset(leftScroll.ActualWidth);
            Debug.WriteLine("scroo to end now.....");
        }

        private void goToleftScrollHome_Click(object sender, RoutedEventArgs e)
        {
            leftScroll.ScrollToHome();
            Debug.WriteLine("scroo to home now.....");
        }

        private void goToRightScrolEnd_Click(object sender, RoutedEventArgs e)
        {
            rightScroll.ScrollToEnd();
            rightScroll.ScrollToHorizontalOffset(rightScroll.ActualWidth);
        }

        private void goToRightScrolHome_Click(object sender, RoutedEventArgs e)
        {
            rightScroll.ScrollToHome();
        }

        /// <summary>
        /// channel mute with chindex:0...23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imute_0_Click(object sender, RoutedEventArgs e)
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
            int chindex = btn.iTag;
            CMatrixData.matrixData.m_ChanelEdit[chindex].chMute = tmp;
            if (NetCilent.netCilent.isConnected())
            {
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_ChanelMute();
            }
            else
                updateAllChanelMute_fromData();

        }

        /// <summary>
        /// channel mute
        /// </summary>
        public void updateChannelMute_fromData(int chindex)
        {
            byte mute = CMatrixData.matrixData.m_ChanelEdit[chindex].chMute;

            string strMute = string.Format("imute_{0}", chindex);
            var muteBtn = (CSwitcher)this.FindName(strMute);
            if (muteBtn != null)
            {
                muteBtn.IsSelected = (mute > 0);
            }

        }

        private int getPageIndex()
        {
            int index = 0;
            CSwitcher sbtn = null;
            string strbtn = "";
            for (int i = 0; i < 8; i++)
            {
                strbtn = string.Format("tabHeadBtn_{0}", i);
                sbtn = (CSwitcher)this.FindName(strbtn);
                if (sbtn != null && sbtn.IsSelected)
                {
                    index = i;
                    break;
                }

            }
            return index;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ichbtn_0_Click(object sender, RoutedEventArgs e)
        {
            // Debug.WriteLine("click now..............");
            CSwitcher btn = sender as CSwitcher;
            int chindex = btn.iTag;

            if (getPageIndex() != 0)
            {
                gotoPage(0);
            }
            turnChanel(chindex, true);

        }

        /// <summary>
        /// turn channel---------------
        /// </summary>
        /// <param name="chindex"></param>
        public void turnChanel(int chindex, bool mturnHeadTab) //switchPanelWitchChannel is in here
        {
            lightChanelBtnWithChindex(chindex, mturnHeadTab);
            dspChangePge.switchPanelWithChannel(chindex);
            turnCopyEdit(chindex);
            //  Debug.WriteLine("here ..root tab index is : {0}",rootTab.SelectedIndex);

            updateGUI_withChindex(chindex);

        }


        ///
        private void initialButtonRightClick()
        {
            string strbtn = "";
            CSwitcher sbtn = null;
            TextBox tb = null;
            String strBox = "";
            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                strbtn = string.Format("ichbtn_{0}", i);
                sbtn = (CSwitcher)this.FindName(strbtn);
                sbtn.MouseDoubleClick += ichbtn_0_MouseDoubleClick;
                strBox = string.Format("iedName_{0}", i);
                tb = (TextBox)this.FindName(strBox);
                if (tb != null)
                {
                    tb.LostFocus += tb_LostFocus;
                    tb.KeyDown += tb_KeyDown;
                    tb.MouseDoubleClick += tb_MouseDoubleClick;

                }

            }

        }

        private void tb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var box = sender as TextBox;
            box.Clear();
            box.Focus();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            // throw new NotImplementedException();
            var tbx = sender as TextBox;
            int chindex = Int16.Parse((sender as TextBox).Tag.ToString());
            string strbtn = "";
            CSwitcher sbtn = null;
            strbtn = string.Format("ichbtn_{0}", chindex);
            sbtn = (CSwitcher)this.FindName(strbtn);
            if (e.Key == Key.Enter)
            {
                string wstr = tbx.Text.Trim();

                //btn.Content = wstr;

                CMatrixData.matrixData.setNameOfChannel(chindex, wstr);
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_ChangeChName(chindex);
                tbx.Visibility = Visibility.Hidden;
                if (sbtn != null)
                {
                    sbtn.Visibility = Visibility.Visible;
                    LimitedStrProperty.SetStringContent(sbtn, wstr);
                }

            }



        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            var tbx = sender as TextBox;
            tbx.Visibility = Visibility.Hidden;
            int i = Int16.Parse(tbx.Tag.ToString());
            string strbtn = "";
            CSwitcher sbtn = null;
            strbtn = string.Format("ichbtn_{0}", i);

            sbtn = (CSwitcher)this.FindName(strbtn);
            if (sbtn != null)
            {
                sbtn.Visibility = Visibility.Visible;

            }

        }
        //
        public void turnCopyEdit(int chindex) //0..23
        {
            int cindex = chindex;
            if (chindex >= CFinal.ChanelMax)
                cindex = Math.Abs(chindex - CFinal.ChanelMax);

            string strCh = CUlitity.defaultChName(cindex);
            /*
             * //20170722
            if (chindex < CFinal.ChanelMax)
                edCopfrom.Text = string.Format("Input {0}", strCh);
            else
                edCopfrom.Text = string.Format("Output {0}", strCh);
            */



        }
        /// <summary>
        /// light the choose button with its itag
        /// </summary>
        /// <param name="chindex"></param>
        private void lightChanelBtnWithChindex(int chindex, bool mturnHeadTab)
        {
            currentchindex = chindex;
            string strbtn = "";
            CSwitcher sbtn = null;


            for (int i = 0; i < CFinal.ChanelMax * 2; i++)
            {
                strbtn = string.Format("ichbtn_{0}", i);
                sbtn = (CSwitcher)this.FindName(strbtn);
                sbtn.IsSelected = false;
            }

            strbtn = string.Format("ichbtn_{0}", chindex);
            sbtn = (CSwitcher)this.FindName(strbtn);
            if (!sbtn.IsSelected)
                sbtn.IsSelected = true;

            if (mturnHeadTab)
            {
                if (chindex >= CFinal.ChanelMax)
                {
                    turnHeadTabButton(2);
                }
                else
                {
                    turnHeadTabButton(0);
                }
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        /// <summary>
        /// matrix about below 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        public void refreshFirstPage()
        {
            updateAllSliderMute_fromData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        LoadLedForm lodf;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            showDeviInfo();
            if (lodf == null)
                lodf = new LoadLedForm();
            //Debug.WriteLine("sendcmd recall current scence...");
            if (NetCilent.netCilent.isConnected())
            {
                conLed.LedStatus = LED_Status.LED_Green;
                ackTimer.Stop();
                onLineCheckCounter = 0;

                CMatrixData.matrixData.procesRecallAckComStatus();
                CMatrixData.matrixData.sendCMD_recallCurrence(true);
                recallReturn = false;
                recallListenTimer.Start();
            }
            else
            {
                conLed.LedStatus = LED_Status.LED_Normal;
                refreshAllpages();
            }

            this.Width = CDefine.Matrix_Width;
            this.Height = CDefine.Matrix_Height;
            onSerialModeTypeChange(isSerialModeType);//williamxia20170719     

            cbxDevList.ItemsSource = CMatrixData.matrixData.gStrA8DevList();
            if (CMatrixData.matrixData.gStrA8DevList().Length > 0)
                cbxDevList.setSelectindex(0);
            cbxDevList.IsEnabled = (CMatrixData.matrixData.gStrA8DevList().Length > 1);
            devPanel.Visibility = (CMatrixData.matrixData.gStrA8DevList().Length < 2 ? Visibility.Hidden : Visibility.Visible);


        }

        public void prepareForParalConnect()
        {
            recallListenTimer.Stop();
            ackTimer.Stop();
            isSerialModeType = false;

        }
        public void afterParalConnect()
        {
            showDeviInfo();
            //Debug.WriteLine("sendcmd recall current scence...");
            if (NetCilent.netCilent.isConnected())
            {
                conLed.LedStatus = LED_Status.LED_Green;
                ackTimer.Stop();
                onLineCheckCounter = 0;
                startupScanList(false); //close scan list of left

                CMatrixData.matrixData.procesRecallAckComStatus();
                CMatrixData.matrixData.sendCMD_recallCurrence(true);
                recallReturn = false;
                recallListenTimer.Start();

                lodf = new LoadLedForm();
                runSyncWindow();
            }
            else
            {
                conLed.LedStatus = LED_Status.LED_Normal;
                refreshAllpages();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tindex"></param>
        private void turnHeadTabButton(int tindex)
        {
            CSwitcher sbtn = null;
            string strbtn = "";
            for (int i = 0; i < 8; i++)
            {
                strbtn = string.Format("tabHeadBtn_{0}", i);
                sbtn = (CSwitcher)this.FindName(strbtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = false;
                }

            }
            strbtn = string.Format("tabHeadBtn_{0}", tindex);
            sbtn = (CSwitcher)this.FindName(strbtn);
            if (sbtn != null)
            {
                sbtn.IsSelected = true;
            }


        }






        private static SaveFileDialog saveDlg = new SaveFileDialog();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private static OpenFileDialog openDlg = new OpenFileDialog();
        /// <summary>
        /// load sence preset files from local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        /// <summary>
        /// matrix channels copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// saveload devindex:0:devcice:1:local
        /// </summary>
        /// <returns></returns>
        public int devindex()
        {
            /*
             * //20170722
            int res = 0;
            if (itemRadio_0.IsChecked.HasValue
                && (bool)itemRadio_0.IsChecked)
                res = 0;
            else res = 1;
            
            return res;
             */
            return 0;
        }


        private void CSwitcher_Click(object sender, RoutedEventArgs e)
        {

        }








        /// <summary>
        /// relaycontrol about two buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>




        #region fbc transaction part process
        /// <summary>
        /// update the matrix of fbc channel part 
        /// </summary>


        public void updateFBCShowTest(int nextpos)
        {
#if defFBCShow_
           for(int i=0;i<24;i++)
           {
               tfbcShow.m_FBCStatus[i] = CMatrixData.matrixData.m_FbcFilterStatus[i]; 
               tfbcShow.m_FBCFreq[i] = CMatrixData.matrixData.m_FbcEQData[i].eq_freqindex;
               tfbcShow.m_FBCGain[i] = CMatrixData.matrixData.m_FbcEQData[i].eq_gainindex;
                           
           }
            tfbcShow.currentPos = (byte)nextpos;
            tfbcShow.refreshControl();
#endif

        }


        private void fbcReset_Click(object sender, RoutedEventArgs e) //fbc setup
        {
            //
            var btn = sender as CSwitcher;
            byte tmp = 0;
            if (btn.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            CMatrixData.matrixData.m_FbcParam[2] = tmp;

            if (NetCilent.netCilent.isConnected())
            {
                //CMatrixData.matrixData.sendCMD_FBCSetting(1);
                onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_FBCSetup();
            }
            else
            {
                btn.IsSelected = (tmp > 0);
                if (tmp == 1)
                {
                    CMatrixData.matrixData.resetFBCData();
                    fbcPge.updateFBCSetting_fromData();
                    fbcPge.fbcDrawEQ();
                }
            }




        }





        /// <summary>
        /// fbc setup 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #endregion

        public void loadMemory_fromLocal()
        {

            Debug.WriteLine("load memory from local");

            openDlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openDlg.Filter = CDefine.FILE_MEMORY_Filter;
            openDlg.CheckFileExists = true;
            if (openDlg.ShowDialog() == true)
            {

                string strp = openDlg.FileName;
                // IOBinaryOperation.writeBinaryToFile(strp, XCoreData.m_meoryRead);

                if (readMemoryFileFromLocal(strp))
                {
                    if (NetCilent.netCilent.isConnected())
                    {
                        savePge.beginExportRateKnob();
                        ackTimer.Stop();
                        onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_ImportLocalMemoryDataToDevice(0);
                    }


                }
            }

        }
        public bool readMemoryFileFromLocal(string filepath)  //mcu:0,dsp:1
        {

            bool res = false;
            int fileLen = (int)IOBinaryOperation.fileLength(filepath);

            Debug.WriteLine("read file length is : {0}", fileLen);
            if (fileLen > 0)
            {

                CMatrixData.matrixData.clearMemoryRead();
                if (File.Exists(filepath) && (fileLen == CDefine.Memory_Max_Package * CDefine.Memory_Per_Packeg_len))
                {

                    CMatrixData.matrixData.m_meoryRead = IOBinaryOperation.readBinaryFile(filepath, fileLen);
                    res = true;
                    //  Debug.WriteLine("memory load : " + BitConverter.ToString(XCoreData.m_meoryRead));
                }


                //---------------------
            }
            return res;

        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            clearTimer();
            if (lodf != null)
                lodf.Close();

            if (isSerialModeType)
            {
                this.DialogResult = true;
            }
            else
            {
                //
                //   Debug.WriteLine("will send exit msg now................................................");           
                Environment.Exit(0);
            }

        }



        /// <summary>
        /// for pop up a form to change the channel name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edChanelDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var txtbox = sender as TextBox;
            txtbox.Clear();
            txtbox.IsReadOnly = false;
            txtbox.Background = Brushes.White;
            txtbox.Focus();
        }

        /// <summary>
        /// adjust if the lock flag is 1 and then lock the window
        /// </summary>
        public void popupPwdWindow()
        {
            Debug.WriteLine("ready popup lock window,check the password lock flag is :{0}", CMatrixData.matrixData.lockFlag);
            if (CMatrixData.matrixData.lockFlag == 1)
            {
                var unlockW = new NoteLockPwdWindow();
                //sendcmd lockpassword
                if (unlockW.ShowDialog() == true)
                {
                    Debug.WriteLine("unlock password...");
                    //sendcmd unlock password 
                    CMatrixData.matrixData.lockFlag = 0;
                    CMatrixData.matrixData.sendCMD_WriteLockPWD();
                }
                else
                {
                    Debug.WriteLine("dialog is not ok,password is not ok");
                    this.Close();
                }

            }


        }



        private void btnCloseWindow_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region for parall part increase era
        private DeviceScaner devScan;

        private static List<CDeviceModule> devList = new List<CDeviceModule>();

        private List<String> verifyIPList = new List<string>();

        public void AddNewModuleFromRouterList(String strIP, string mname)
        {
            if (strIP.Trim().Length < 1) return;
            if (devScan.indexOfScanedIP(strIP) >= 0) //filter the same ip ,bug a lot of device 
            {
                Debug.WriteLine("add to new modeule from ....." + strIP);
                int index = devScan.indexOfScanedIP(strIP);
                AddNewModule(devScan.scanedIpList[index], mname);
            }

        }
        private System.Timers.Timer clickTimer;
        private const int ElapsTime = 3000;
        public void initialCanClickTimer()
        {

            if (clickTimer == null)
            {
                clickTimer = new System.Timers.Timer(ElapsTime + 500);//three seconds per loop
                clickTimer.Elapsed += new System.Timers.ElapsedEventHandler(clickTimer_elapsed);
            }
            clickTimer.AutoReset = true;
            clickTimer.Enabled = true;
            clickTimer.Start();
        }
        private bool isCanClick = false;
        private void clickTimer_elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            isCanClick = true;
        }
        public void setActiveModeName(String strName)
        {
            foreach (CDeviceModule dev in devList)
            {
                if (dev != null && dev.isSelected)
                {
                    dev.DeviceName = strName;
                }

            }
        }

        public void AddNewModule(RouterInfo rpif, string modName = null)
        {
            CDeviceModule module = new CDeviceModule();
            module.Margin = new Thickness(5);
            devLisPanel.Children.Add(module);
            modTagCounter++;

            module.iTag = modTagCounter;//devList.Count-1;//base 1...

            devList.Add(module);
            module.devProvCopy(verifyDevinfo.devProv);
            module.DeviceName = modName;
            module.DevAddr = rpif.RouterAddr;
            module.DevMac = rpif.RouterMac;
            module.RouterStyle = rpif.RouterStyle;
            string tmp = string.Format("add new module index is  {0} ip is {1}", module.iTag, rpif.RouterAddr);
            Debug.WriteLine(tmp);
            //  Debug.WriteLine("search found router info type is :  {0}", rpif.RouterStyle);

            module.onclickEvent += new CDeviceModule.onClick(onDevModel_Click);
            // module.MaxWidth = FinalConst.CWidth_Module;          
            module.MouseRightButtonDown += module_MouseRightButonDown;

            devCountLabel.Text = devList.Count.ToString();
            devListSroll.ScrollToBottom();

        }

        private void onDevModel_Click(object sender, RoutedEventArgs e)
        {
            CDeviceModule module = (CDeviceModule)sender;
            if (isCanClick)
            {

                int itg = module.iTag;
                currentIPIndex = itg;
                conLed.LedStatus = LED_Status.LED_Normal;
                if (module.isSelected == false)
                {
                    module.isSelected = true;
                    Debug.WriteLine("device isselected false,so changed to true");
                    //begin to connect to device
                    setConnectStatus(module, true);
                }
                else
                {
                    module.isSelected = false;
                    setConnectStatus(module, false);

                }
                foreach (CDeviceModule dev in devList)
                {
                    if (dev.iTag != module.iTag)
                    {
                        dev.isSelected = false;
                    }
                }

                Thread.Sleep(500);
                isCanClick = false;
            }
            else
            {
                MessageBox.Show("Don't repeat mouse clicks within 3 seconds!");
                return;
            }



        }
        //
        /// <summary>
        /// for paral connect 
        /// </summary>
        /// <param name="stats"></param>
        public void setConnectStatus(CDeviceModule devMode, bool stats)
        {
            if (currentIPIndex >= 0 && stats)
            {
                try
                {
                    string strIP = devMode.DevAddr;    //devScan.scanedIpList[currentIPIndex].RouterAddr;
                    Debug.WriteLine("will connected ip is :{0} ......." + strIP);
                    // stopCheckAck();                   
                    string localIP = IPProces.getIPAddress();
                    if (strIP.Length > 1 && IPProces.isInSameSubnet(strIP, localIP))
                    {
                        CMatrixData.matrixData.devProvision.devProvisionCopy(devMode.modDevProv);
                        // cdevIP.Content = XCoreData.coreData.CDeviceIP = strIP;
                        if (NetCilent.netCilent != null)
                            NetCilent.netCilent.connect(strIP);
                        Debug.WriteLine("begin connect with client now................................................");
                    }
                    else
                    {
                        MessageBox.Show("Detected multipile network cards!");

                    }


                }
                catch (SocketException soc)
                {
                    Debug.WriteLine("soc {0}", soc.ErrorCode);
                    //  stopCheckAck();

                }
                catch (Exception ec)
                {
                    Debug.WriteLine(ec.ToString());
                    MessageBox.Show("Connected to device failure.999999999999999999999999");
                    // stopCheckAck();
                }
            }
            else if (!stats)
            {
                if (NetCilent.netCilent != null && NetCilent.netCilent.isConnected())
                    NetCilent.netCilent.disConnect();
                //stopCheckAck();

            }

        }

        private void module_MouseRightButonDown(object sender, MouseButtonEventArgs e)
        {
            //int clickNumber = e.ClickCount;
            CDeviceModule module = sender as CDeviceModule;
            ContextMenu menu = new ContextMenu();
            MenuItem chnameItem = new MenuItem();
            chnameItem.Header = "Change Name";
            chnameItem.Click += changeDeviceNameItem_Click;
            menu.Items.Add(chnameItem);
            module.ContextMenu = menu;
        }
        private void changeDeviceNameItem_Click(object sender, RoutedEventArgs e)
        {

            var mdevDlg = new CNewDeveName();
            mdevDlg.onsubmitNewDeviceChangedEvent += new CNewDeveName.submitNewDeviceChanged(onsubmitNewDevname);
            mdevDlg.ShowDialog();

        }

        private void onsubmitNewDevname(object sender)
        {
            var dlg = sender as CNewDeveName;
            if (dlg != null)
            {
                string strName = dlg.getInputName();
                this.sendstrDevname(strName);
            }


        }



        public void sendstrDevname(string strDevName)
        {
            byte[] tmp = UtilCover.stringToBytes(strDevName, 16);
            Array.Clear(CMatrixData.matrixData.m_DeviceName, 0, 16);
            Array.Copy(tmp, CMatrixData.matrixData.m_DeviceName, tmp.Length);
            CMatrixData.matrixData.sendCMD_submitToChangeDevName();

        }


        private void initialSocket_and_Data()
        {

            if (devScan == null)
            {
                devScan = new DeviceScaner();
                devScan.isSupportUSB = true;
                Debug.WriteLine("nvscan is null");
            }
            devScan.onUDPBeinScanEvent += new DeviceScaner.onDeviceBeginScan(onScanBegin);
            devScan.onUDPStopScanEvent += new DeviceScaner.onDeviceStopScan(onScanStop);
            devScan.onScanChangedEvent += new DeviceScaner.onDeviceScanChanged(onScanChanged);
            //   Debug.WriteLine("nvscan load status result is : {0}", udpScan.isLoadedFailure);

            if (devList == null)
                devList = new List<CDeviceModule>();



        }




        private void onScanChanged(Object sender, RouterInfo rpinfo, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                  (ThreadStart)delegate()
                                                  {
                                                      //routerList.Add(rpinfo);
                                                      //  AddNewModule(rpinfo);
                                                      Debug.WriteLine("udp scan receive......." + rpinfo.RouterAddr);
                                                  });


        }
        public void startScanUpdate()
        {
            rootGrid.IsEnabled = false;
            verifyIPList.Clear();
            modTagCounter = 0;
        }

        private void onScanStop(object sender, EventArgs e)
        {

            // Debug.WriteLine("udp scan  stop now...");
            dtInvoke(stopScanUpdate);


        }
        public delegate void updateDo();
        public void dtInvoke(updateDo fdo)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                      (ThreadStart)delegate()
                                      {
                                          fdo();
                                      });

        }
        private NetCilent[] verifyClient;
        public void setVerifyClientConnect(int index)
        {
            // return;    

            if (verifyClient[index] == null)
            {
                verifyClient[index] = new NetCilent();
                verifyClient[index].ReceiveByteEvent += new Cilent.onByteReceive(onVerifyReceiveByte);
                verifyClient[index].onConnectedEvent += new Cilent.onConnected(onVerifyConnected);

            }
            else
            {
                if (verifyClient[index].isConnected())
                {
                    verifyClient[index].disConnect();
                    //  Debug.WriteLine("very fi disconnnect.....aaaa");
                }
            }

            if (index >= 0 && index < devScan.scanedIpList.Count)
            {
                //  Debug.WriteLine("verify ip ..index {0}...", index);
                string strIP = devScan.scanedIpList[index].RouterAddr;
                Debug.WriteLine(">>> verify ip is :..." + strIP);
                try
                {

                    string localIP = IPProces.getIPAddress();
                    if (strIP.Length > 1 && IPProces.isInSameSubnet(strIP, localIP))
                    {
                        //  verifyClient[index]
                        verifyClient[index].connectPort = 5000;
                        verifyClient[index].connect(strIP);
                        //   Debug.WriteLine("begin connect with client now......................   "+strip

                    }
                }
                catch (SocketException e)
                {
                    if (verifyClient[index] != null && verifyClient[index].isConnected())
                        verifyClient[index].disConnect();

                }
                catch (Exception ec)
                {
                    // verifyClient.clearRlease();
                    Debug.WriteLine("\n");
                    Debug.WriteLine(ec.ToString());
                    Debug.WriteLine("\n verfiscoket  connect failure with index tag {0}", index);
                    // MessageBox.Show("Connected to device failure.999999999999999999999999");
                }

            }

        }
        private void onVerifyConnected(string conIP, EventArgs e)
        {
            int index = devScan.indexOfScanedIP(conIP);
            if (index < 0) return;
            if (verifyClient[index] != null && verifyClient[index].isConnected())
            {
                Debug.WriteLine("will send by verifysocket with ip: " + conIP);
                CMDSender.sendCMD_scanDevicesID(verifyClient[index]);
            }


        }

        private void sendCMD_ReadMatrixDevInfo(Cilent mcilent)
        {
            DeviceProvision devpro = new DeviceProvision();
            // devpro.
            CMDSender.sendCMD_readDevices(mcilent, new DeviceProvision());
        }


        CDeviceInfo verifyDevinfo = new CDeviceInfo();

        public void closeAllVerifySockets()
        {
            if (NetCilent.netCilent != null && NetCilent.netCilent.isConnected())
            {
                NetCilent.netCilent.disConnect();

            }


        }

        private void verfiConvertData(String conIP, List<byte> Package)
        {
            //
            int appID = Package[5] * 256 + Package[6];//0xffff
            int devID = Package[7] * 256 + Package[8];//0xffff
            int CMD = Package[9] * 256 + Package[10];
            int locaAPID = Package[11] * 256 + Package[12];
            //XCoreData.coreData.APP_ID == FinalConst.ID_MACHINE
            //Debug.WriteLine("receive app id {0} ,device id is {1}  ---------------------", appID, devID);
            if (Package.Count == 40 && (appID == CDefine.AP_ScandID) && locaAPID == CDefine.AP_Matrix_Main)  //while app id is AP520|CSEQ V2
            {
                if (devScan.indexOfScanedIP(conIP) >= 0 && verifyIPList.FindIndex(r => r.Equals(conIP)) < 0)
                {
                    verifyIPList.Add(conIP);
                    switch (CMD)
                    {

                        case MatrixCMD.F_ScanDeviceID:
                            {
                                byte[] m_deviceName = new byte[CDefine.Len_FactPName];//16
                                int count = 15;
                                Array.Clear(m_deviceName, 0, 16);
                                for (int i = 0; i < CDefine.Len_FactPName; i++)
                                {
                                    m_deviceName[i] = Package[count++];

                                }
                                IPProces.printAryByte("\nverify devicename byte: ", m_deviceName);

                                string devName = UtilCover.bytesToString(m_deviceName, CDefine.Len_FactPName);
                                //  Debug.WriteLine("\n receive device name is  " + devName);

                                verifyDevinfo.devProv.strDevName = devName;

                                verifyDevinfo.devProv.pMachineID = verifyDevinfo.AppID = (ushort)locaAPID;//appid
                                verifyDevinfo.devProv.pDeviceID = verifyDevinfo.DeviceID = (ushort)(Package[13] * 256 + Package[14]);
                                // CMatrixData.matrixData.devProvision.devProvisionCopy(verifyDevinfo.devProv);                      
                                Debug.WriteLine(verifyDevinfo.getDevInfo());

                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (ThreadStart)delegate()
                                {
                                    //add new module here here...
                                    setSearchIndicator(false);
                                    AddNewModuleFromRouterList(conIP, devName);
                                    int index = devScan.indexOfScanedIP(conIP);
                                    verifyClient[index].disConnect();

                                });

                            }
                            break;
                        default:
                            break;
                    }
                }

            }

        }

        private int modTagCounter = 0;

        private static List<byte> m_recCmd = new List<byte>();

        private void verifyReadByte(String strIP, byte ch)
        {
            if (m_recCmd.Count() == 0 && ch != CommConst.UTRAL_H0)
            {
                m_recCmd.Clear();
                return;
            }
            if (m_recCmd.Count() == 1 && ch != CommConst.UTRAL_H1)
            {
                m_recCmd.Clear();
                return;

            }
            if (m_recCmd.Count() == 2 && ch != CommConst.UTRAL_H2)
            {
                m_recCmd.Clear();
                return;
            }
            //-----------------------------------------------
            m_recCmd.Add(ch);
            if (m_recCmd.Count() > 5)
            {
                int len = m_recCmd[3] * 256 + m_recCmd[4];

                if (m_recCmd.Count() == len && (m_recCmd[len - 1] == CommConst.UTRAL_End))
                {

                    // int cmd = m_recCmd[9] * 256 + m_recCmd[10];
                    // Debug.WriteLine("will verify string ip is " + strIP);

                    verfiConvertData(strIP, m_recCmd);
                    m_recCmd.Clear();


                }


            }

        }
        private void onVerifyReceiveByte(String conIP, byte[] m_rData, int dlen, EventArgs e)
        {

            Debug.WriteLine("verify socket recevie with IP" + conIP);

            for (int i = 0; i < dlen; i++)
            {
                verifyReadByte(conIP, m_rData[i]);
            }
            string str = BitConverter.ToString(m_rData, 0, dlen);
            Debug.WriteLine("very receive byte on recevie func.... " + str);
        }

        public void stopScanUpdate()
        {
            rootGrid.IsEnabled = true;
            this.IsEnabled = true;
            int count = devScan.scanedIpList.Count;
            //Debug.WriteLine("dev scan ip list count is  {0}  "+"devscan strIP "+devScan.scanedIpList[0].RouterAddr, count);
            verifyClient = new NetCilent[count];

            for (int i = 0; i < count; i++)
            {
                setVerifyClientConnect(i);
                Thread.Sleep(500);
            }

            // checkIDtimer.Start();
        }
        private void onScanBegin(object sender, EventArgs e)
        {
            //  Debug.WriteLine("udp scan  begin now...");

            dtInvoke(startScanUpdate);

        }

        private SearchLed schIndicator;
        public void addSchLed()
        {
            if (schIndicator == null)
            {
                schIndicator = new SearchLed();

            }
            if (schIndicator.Parent == null)
                devLisPanel.Children.Add(schIndicator);
            schIndicator.Margin = new Thickness(2, 200, 2, 2);

            // <oc:SearchLed  Name="schIndicator"></oc:SearchLed>
        }
        public void setSearchIndicator(bool sts)
        {
            if (sts)
            {
                schIndicator.Visibility = Visibility.Visible;
                schIndicator.startSearch();
                // Debug.WriteLine("start search led now...");
                schIndicator.Margin = new Thickness(2, 200, 2, 2);
            }
            else
            {
                schIndicator.Visibility = Visibility.Hidden;
                schIndicator.stopSearch();
                //   Debug.WriteLine("stop search led now...");
                schIndicator.Margin = new Thickness(2, 0, 2, 2);
            }

        }
        public int currentIPIndex = -1; //mark it as nonius
        public void stopScan()
        {

            if (NetCilent.netCilent.isConnected())
                NetCilent.netCilent.disConnect();
            devLisPanel.Children.Clear();
            devList.Clear();
            //routerList.Clear();//routerList clear
            SearchDevices.IsEnabled = true;
            devCountLabel.Text = "0";
            //  stopCheckAck();
            //   XCoreData.coreData.isConnected = false;
            //  cdevIP.Content = "";
            currentIPIndex = 0;

        }
        private void SearchDevices_mouseDown(object sender, MouseButtonEventArgs e)
        {
            bool sts = !(left_deveBordList.Visibility == Visibility.Visible);
            startupScanList(sts);
            if (sts)
            {
                if (!NetCilent.netCilent.isConnected())
                    leftRefresh_click();
            }

        }
        #endregion

        private void leftRefresh_click()
        {
            stopScan();
            addSchLed();
            this.IsEnabled = false;
            setSearchIndicator(true);
            //nvScan
            if (!devScan.isLoadedFailure)
            {

                devScan.startScan();
            }
            else
            {
                // MessageBox.Show("Socket loaded failure!");
            }
        }

        private void img_leftBtnMousUpClick(object sender, MouseButtonEventArgs e)
        {
            leftRefresh_click();
        }

        private void closeCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseDevLine1.Stroke = Brushes.Red;
            CloseDevLine2.Stroke = Brushes.Red;
        }

        private void closeCanvas_MouseLeave(object sender, MouseEventArgs e)
        {

            CloseDevLine1.Stroke = Brushes.Gray;
            CloseDevLine2.Stroke = Brushes.Gray;
        }

        private void closeCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startupScanList(false);
        }

        private void menu_systemClick(object sender, MouseButtonEventArgs e)
        {
            MenuWin mwin = new MenuWin();
            mwin.ParentWindow = this;
            mwin.ShowDialog();
        }

        private void winTitle_doubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("content click now.......................");
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void titile_drawMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ichbtn_0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
            var btn = sender as CSwitcher;
            int chindex = btn.iTag;

            TextBox tb = null;
            String strBox = "";
            strBox = string.Format("iedName_{0}", chindex);
            tb = (TextBox)this.FindName(strBox);
            if (tb != null)
            {
                tb.Visibility = Visibility.Visible;
                btn.Visibility = Visibility.Hidden;
                tb.Clear();
                tb.Focus();


            }

        }

        //textbox lost focus
        private void ichEd_0_LostFocus(object sender, RoutedEventArgs e)
        {
            var tbox = sender as TextBox;
            string strName = tbox.Name.Trim();  //ichEd_0
            int chindex = int.Parse(strName.Substring(6));
            updateSingleSlider_fromData(chindex);
            tbox.Background = Brushes.Gray;
            onLineCheckCounter = 0;
            tbox.IsReadOnly = true;

        }
        public static double checkAvalibleInput(double tmp, double MinInput, double MaxInput)
        {
            double tmpmv = 0;
            if (tmp < MinInput)
                tmpmv = MinInput;
            else if (tmp > MaxInput)
                tmpmv = MaxInput;
            else
                tmpmv = tmp;
            tmpmv = input2Value(tmpmv);

            return tmpmv;
        }

        public static double input2Value(double mv)
        {

            int t = 0;
            bool isPog = (mv < 0);

            double x = Math.Abs(Math.Round(mv, 1));
            int a = 0;
            double y = 0;

            a = (int)(x * 10 - (int)x * 10);
            if (a >= 0 && a < 5)
            {
                t = 0;
                a = 0;
            }
            else if (a > 5 && a <= 9)
            {
                t = 1;
                a = 0;
            }
            else  //==5
            {
                t = 0;
                a = 5;
            }
            y = (double)((int)x * 10 + t * 10 + a) / 10;
            if (isPog)
                y = y * -1;

            return y;


        }

        public const double MinInput = -80;
        public const double MaxInput = 15;
        //keydown input
        private void ichEd_0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txtBox = sender as TextBox;
                txtBox.Background = Brushes.Gray;
                string strName = txtBox.Name.Trim();  //ichEd_0

                //  Debug.WriteLine("ed double click with ed name is  "+strName.Substring(6));

                int chindex = int.Parse(strName.Substring(6));

                double dfout = 0;
                string strTemp = txtBox.Text.Trim();
                if (Double.TryParse(strTemp, out dfout))
                {
                    double inputValue = checkAvalibleInput(dfout, MinInput, MaxInput);
                    double position = (int)((inputValue + 80) * 2);
                    txtBox.Text = inputValue.ToString();
                    CMatrixData.matrixData.setChGain(chindex, (byte)position);

                    updateSingleSlider_fromData(chindex);
                    // if (NetCilent.netCilent.isConnected())
                    {
                        //if no connect  
                        onLineCheckCounter = 0;
                        CMatrixData.matrixData.sendCMD_FaderGain(chindex);
                    }
                }
                txtBox.IsReadOnly = true;
            }

        }




    }
}
