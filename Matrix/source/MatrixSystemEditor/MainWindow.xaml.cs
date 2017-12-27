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
using MatrixSystemEditor.commom;
using System.Windows.Threading;
using System.Threading;
using System.Runtime.InteropServices;


namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {




        private List<DeviceProvision> scanDevList = new List<DeviceProvision>();
        private DispatcherTimer scanCheckTimer = new DispatcherTimer();
        public string _connectIP { get; set; }
        public bool isConnected { get; set; } //123

        private MessageReceiver main_MsgRecver;



        private void initMwdParameter()
        {
            if (main_MsgRecver == null)
                main_MsgRecver = new MessageReceiver(MatrixCMD.CMainWindowGUIClass, null);
            main_MsgRecver.WndProc += WindowMsg_recevierWndProc;
        }

        private void WindowMsg_recevierWndProc(object sender, MessageEventArgs e)
        {
            // throw new NotImplementedException();
            if (e.Message == MatrixCMD.MainWindow_MSG_Transfer)
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(e.lParam, t);
                switch (cds.preWph) //highcmd 
                {
                    case MatrixCMD.WM_Msg_Exit:
                        {
                            this.Close();
                        }
                        break;
                    default:
                        break;


                }



            }
        }

        public MainWindow()
        {
            isSerialMode = true;
            beginCheck();
            initalPrimally();
            InitializeComponent();
            initMwdParameter();
            initTimer();


            //--------------initial socket and GUI below-
            initialSocket();
            initialContexteMenu();
            //---------setup corporation         

            this.Title = CMatrixData.matrixData.corpInfo.getProcesTitle();


        }
        private void initalPrimally()
        {

            if (m_recCmd == null)
                m_recCmd = new List<byte>();
            m_recCmd.Clear();
            if (scanDevList == null)
                scanDevList = new List<DeviceProvision>();

            CMatrixData.shareData();//first initial data    
            _connectIP = string.Empty;
            isConnected = false;


        }
        /// <summary>
        ///  begin drag .........
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dragindex"></param>

        /// <summary>
        /// enviroment check ,but must have .netframwork first
        /// </summary>
        public void beginCheck()
        {
            try
            {
                if (!UtilCover.Get45or451FromRegistry())
                {
                    MessageBox.Show("Please install .Net Framework4.5 or above first.");
                    Environment.Exit(0);
                }

            }
            catch (TypeInitializationException ak)
            {
                MessageBox.Show("Please install .Net Framework4.5 or above first.\t" + ak.ToString());
                Environment.Exit(0);
            }
            catch (Exception ec)
            {

                MessageBox.Show("Please install .Net Framework4.5 or above first.\t" + ec.ToString());
                Environment.Exit(0);
            }
        }
        public void initialSocket()
        {
            NetCilent.shareCilent();
            NetCilent.netCilent.ReceiveByteEvent += new Cilent.onByteReceive(onCommuteReceiveByte);
            NetCilent.netCilent.onConnectedEvent += new Cilent.onConnected(onCommuteConnected);
            NetCilent.netCilent.onDisconnectEvent += new Cilent.onDisconnect(onCommuteDisconnected);
        }


        private System.Timers.Timer conTimer = new System.Timers.Timer();
        public void initTimer()
        {
            if (scanCheckTimer == null)
                scanCheckTimer = new DispatcherTimer();
            scanCheckTimer.Interval = new TimeSpan(0, 0, 0, CDefine.ACK_Loop_Interval);
            scanCheckTimer.Tick += scanCheckTimer_Tick;
            scanCheckTimer.Stop();
            if (conTimer == null)
            {
                conTimer = new System.Timers.Timer(3000);
            }
            conTimer.AutoReset = true;
            conTimer.Elapsed += new System.Timers.ElapsedEventHandler(conTimer_Elapsed);
            conTimer.Enabled = true;
            conTimer.Stop();
        }
        public void clearTimer()
        {
            conTimer.Stop();
            conTimer.Close();
            scanCheckTimer.Stop();
            GC.Collect();

        }

        private int contNumber = 0;
        private bool isRepeat = false;
        private void conTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            isRepeat = false;
        }
        /// <summary>
        /// connect to remote Device    
        ///         /// </summary>
        /// <param name="stats"></param>
        public void setConnectStatus(bool stats)
        {
            Debug.WriteLine("begin connect to with ip is : " + _connectIP);

            if (stats && UtilCover.IPCheck(_connectIP))
            {

                try
                {

                    string localIP = IPProces.getIPAddress();
                    if (IPProces.isInSameSubnet(_connectIP, localIP))
                    {
                        NetCilent.netCilent.connect(_connectIP);
                        //IoSocketProces.ioProc.connectWithIP(strIP);
                        Debug.WriteLine("begin connect with client now................................................");
                    }
                    else
                    {
                        string str = string.Format("Detected multipile network cards or invalid IP:{0}", _connectIP);
                        MessageBox.Show(str);

                    }

                }
                catch (Exception ec)
                {
                    _connectIP = string.Empty;
                    Debug.WriteLine(ec.ToString());
                    // MessageBox.Show("Connected to device failure.999999999999999999999999");
                }
            }
            else
            {
                //  IoSocketProces.ioProc.Disconnect();
                NetCilent.netCilent.disConnect();
            }


        }

        /// <summary>
        /// socket commute when disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCommuteDisconnected(String conIP, EventArgs e)
        {
            isConnected = false;
            checkConnect = false;
            checkBegin = false;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               (ThreadStart)delegate()
               {
                   conLed.LedStatus = LED_Status.LED_Normal;
                   connetItem.IsEnabled = true;
                   netConfigItem.IsEnabled = true;
                   connetItem.Content = "Connect";
                   isConnected = false;
                   edRemotDevIP.Content = "";
                   itemConStatus.Content = "Offline";
                   scanCheckTimer.Stop();
                   dwpanel.initAllDeviceModuleHead();

               });
            Debug.WriteLine("client disconnnected ...........00000000000000000000000  ");

        }

        public bool isSerialMode { get; set; }
        /// <summary>
        /// socket commute when connected
        /// </summary>
        /// <param name="conIP"></param>
        /// <param name="e"></param>
        private void onCommuteConnected(string conIP, EventArgs e)
        {
            //
            Debug.WriteLine("has connected to the ip " + conIP);
            // isConnected = true;
            checkConnect = true;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate()
                {

                    isConnected = true;
                    _connectIP = conIP;
                    //get all device in this router with special IP
                    if (isSerialMode)
                    {
                        // ipItem.Content 
                        itemConStatus.Content = "Online";
                        edRemotDevIP.Content = conIP;
                        conLed.LedStatus = LED_Status.LED_Green;
                        connetItem.Content = "Disconnect";
                        netConfigItem.IsEnabled = false;
                        connetItem.IsEnabled = true;
                        checkBegin = true;//begin check now..
                        scanCheckTimer.Start();//begin check now..
                        this.sendCMD_ScanAllDevice();
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                      (ThreadStart)delegate()
                                      {
                                          CMatrixData.matrixData.activeDevProvision.devProvisionCopy(CMatrixData.matrixData.devProvision);//only add for paralla
                                          checkBegin = false;
                                          scanCheckTimer.Stop();
                                          this.sendMSG_note_MatrixAVIII(MatrixCMD.MSG_TCP_ParaConnected, 0);
                                      });

                    }

                }
               );

        }

        /// <summary>
        /// socket commute receive 
        /// </summary>
        /// <param name="m_rData"></param>
        /// <param name="dlen"></param>
        /// <param name="e"></param>
        private void onCommuteReceiveByte(String conIP, byte[] m_rData, int dlen, EventArgs e)
        {
            for (int i = 0; i < dlen; i++)
            {
                recByte(m_rData[i]);
            }
            //  string str = BitConverter.ToString(m_rData, 0, rlen);
            //  Debug.WriteLine("receive byte on recevie func.... " + str);

        }
        private List<byte> m_recCmd = new List<byte>();
        private void recByte(byte ch)
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
            //  Debug.WriteLine("recevie ch is :  " + ch.ToString("X2") + "  m_reccmd count is : " + m_recCmd.Count.ToString());
            if (m_recCmd.Count() >= CDefine.MIN_LEN_PACK)
            {
                int cmd = m_recCmd[9] * 256 + m_recCmd[10];
                int len = m_recCmd[3] * 256 + m_recCmd[4];

                if (m_recCmd.Count() == len && (m_recCmd[len - 1] == CommConst.UTRAL_End))
                {
                    // try
                    // {
                    //    Debug.WriteLine("begin convert with cmd {0},pack len: {1}", cmd, len);
                    convertData(m_recCmd);
                    // }
                    //  catch(Exception e)
                    //  {
                    //   }
                    // finally
                    //{
                    m_recCmd.Clear();
                    // }


                }
            }

        }

        private int devScanNum = 0;




        private void iRead_ScanDevPack(List<byte> Pack)
        {

            devScanNum++;
            //  Debug.WriteLine("devScanNum is  {0}", devScanNum);
            DeviceProvision devpro = new DeviceProvision();

            int count = 11; //11+4+16+7
            devpro.pMachineID = (UInt16)(Pack[count++] * 256 + Pack[count++]);
            devpro.pDeviceID = (UInt16)(Pack[count++] * 256 + Pack[count++]);


            for (int i = 0; i < CDefine.LEN_DeviceName; i++)
            {
                devpro.pDeviceName[i] = Pack[count++];
            }
            //  Debug.WriteLine("scaning appID: {0} deviceID: {1}", devpro.pMachineID, devpro.pDeviceID);

            for (int i = 0; i < 2; i++)
                devpro.pFirmwareVer[i] = Pack[count++];

            for (int i = 0; i < 5; i++) //device serialNumber ++
                devpro.pDevSNONumber[i] = Pack[count++];
            //mcu ver and dsp ver
#if devpro_add
            if (!isInDevlist(devpro))
                scanDevList.Add(devpro);
#endif

            String strName = devpro.getStrDevName();
            if (devpro.pMachineID == CDefine.AP_Matrix_Main)
            {
                CMatrixData.matrixData.AddToA8DevList(devpro.strDevName, devpro.pDeviceID);
                Debug.WriteLine(string.Format("scaned device name is {0}  appid {1}  deviceid {2}", devpro.strDevName, devpro.pMachineID, devpro.pDeviceID));
            }

            Debug.WriteLine("scaned app ID is  {0} -----", devpro.pMachineID);

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
              (ThreadStart)delegate()
              {
                  if (devScanNum == 1)
                      dwpanel.initAllDeviceModuleHead();//initial all williamxia20170614
                  dwpanel.checkDeviceModule_fromSpecial(devpro);
              });



        }


        /// <summary>
        /// 
        /// </summary>
        public void initialAllDeviceLed()
        {



        }


        private bool isInDevlist(DeviceProvision sdev)
        {
            bool isExists = false;
            foreach (DeviceProvision mdev in scanDevList)
            {
                if (mdev.pMachineID == sdev.pMachineID && mdev.pDeviceID == sdev.pDeviceID)
                {

                    isExists = true;
                    break;
                }

            }

            return isExists;
        }

        private const int MinscanpackLen = 38;
        //--------------------------------
        private void convertData(List<byte> Package)
        {
            //
            if (Package.Count < 13) return;

            int appID = Package[5] * 256 + Package[6];
            int devID = Package[7] * 256 + Package[8];
            int CMD = Package[9] * 256 + Package[10];
            //  if (appID == activeDevProvision.pMachineID && devID == activeDevProvision.pDeviceID)

            //  Debug.WriteLine("active machid: {0}  deviceid: {1}  recappID {2} devicID {3} command {4}",
            //   activeDevProvision.pMachineID, activeDevProvision.pDeviceID, appID, devID, CMD);

            if (appID == CDefine.All_Machine_APPID) //all scan local machine 
            {
                checkConnect = true;

                // Debug.WriteLine("fff dev scan data :" + BitConverter.ToString(Package.ToArray()));
                if (CMD == 0x03 && Package.Count > MinscanpackLen)
                {

                    iRead_ScanDevPack(Package);
                }

                // Debug.WriteLine("device recive now...");
            }
            #region Matrix series command receive...               
            else if (appID == CMatrixData.matrixData.activeDevProvision.pMachineID && devID == CMatrixData.matrixData.activeDevProvision.pDeviceID)
            {
                checkConnect = true; //if there is byte to receive ,so we can consider is connected of course.

                //   CMatrixData.matrixData.m_CommuStatus.commuteStatus = CDefine.M_ConectedNormal;               
                switch (CMD)
                {
                    case MatrixCMD.F_InputDG411Gain://sensitivity
                        {
                            CMatrixData.matrixData.iRead_sensitivity(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;
                    case MatrixCMD.F_InputDC48VFlag:
                        {
                            Debug.WriteLine("begin read input DC48V flag below");
                            CMatrixData.matrixData.iRead_DC48VFlag(Package);
                            int chindex = Package[11];
                            this.sendMSG_note_MatrixAVIII(CMD, 0);

                        }
                        break;


                    case MatrixCMD.F_SigMeters:
                        {
                            CMatrixData.matrixData.iRead_sigMeters(Package);
                            // Debug.WriteLine("receive sigmeter now.......");
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;
                    case MatrixCMD.F_SetPageZoneIndex:
                        {
                            CMatrixData.matrixData.iRead_PageZone(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                            Debug.WriteLine("receive cmd  pagezone command over...............");
                        }
                        break;

                    case MatrixCMD.F_AutoMixerCHSelect:
                        {
                            CMatrixData.matrixData.iRead_AutoMixerSelect(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;
                    case MatrixCMD.F_AutoMixerSetting:
                        {
                            CMatrixData.matrixData.iRead_AutoMixerSetting(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;
                    case MatrixCMD.F_RelayControl:
                        {
                            CMatrixData.matrixData.iRead_Relay(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                            // Debug.WriteLine("receive cmd relay command over...............");
                        }
                        break;
                    case MatrixCMD.F_StoreSinglePreset:

                        break;
                    case MatrixCMD.F_Ack:
                        {
                            int cmd = Package[11] * 256 + Package[12];//subcmd 
                            switch (cmd)
                            {
                                case MatrixCMD.F_InputEQFlat:
                                case MatrixCMD.F_OutEQFlat:
                                    {
                                        this.sendMSG_note_MatrixAVIII(cmd, 0);
                                        Debug.WriteLine("channel eqflat..return as ack subcommnd.........");
                                    }
                                    break;
                                case MatrixCMD.F_ReturnDefaultSetting:
                                case MatrixCMD.F_ResetToFactorySetting:
                                case MatrixCMD.F_RecallSinglePreset:
                                case MatrixCMD.F_LoadFromPC:
                                    {
                                        this.sendMSG_note_MatrixAVIII(cmd, 0);
                                        //  Debug.WriteLine("default setting return through defaultseeting in ack now..................");
                                    }
                                    break;

                            }
                            if (appID >= AppIDList.AP_RPM_100 && appID <= AppIDList.AP_RVA_100)
                            {
                                CMatrixData.matrixData.m_RVAEsStatus = Package[13];
                                switch (appID)
                                {
                                    
                                    case AppIDList.AP_RPM_100:
                                        {

                                            sendMSG_note_RPM200Page(cmd, 0);
                                        }
                                        break;

                                    case AppIDList.AP_RIO_100:
                                        {
                                            //sendMSG_note_RVA200

                                        }
                                        break;
                                    case AppIDList.AP_RVC_100:
                                    case AppIDList.AP_RVA_100:
                                         {
                                            Debug.WriteLine("send ap rva200 now ...");
                                            sendMSG_note_RVC1000Page(cmd, 0);
                                        }
                                        break;

                                }


                            }

                        }

                        break;
                    ///ducker receiver
                    case MatrixCMD.F_DuckerParameter:
                        {
                            // Debug.WriteLine("ducker param receive begin..................");
                            CMatrixData.matrixData.iRead_DuckerParameter(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                            Debug.WriteLine("ducker parameters receive over....");
                        }
                        break;
                    case MatrixCMD.F_DuckerInputMixer:
                        {
                            CMatrixData.matrixData.iRead_DuckerInputMixer(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                            Debug.WriteLine("ducker input mixer receive over....");
                        }
                        break;
                    case MatrixCMD.F_DuckerGainInsert:
                        {
                            CMatrixData.matrixData.iRead_DuckerGainInsert(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                            Debug.WriteLine("ducker input mixer gain receive over....");
                        }
                        break;
                    case MatrixCMD.F_GetFBCStatus:
                        {
                            CMatrixData.matrixData.iRead_FBCStatus(Package);
                            // byte[] tmpAry = new byte[2]; 
                            byte status = Package[11];
                            this.sendMSG_note_MatrixAVIII(CMD, status);
                            //  this.sendMSG_note_MatrixAVIII(CMD, tmpAry);

                        }
                        break;

                    case MatrixCMD.F_FBCSetting:
                        {
                            CMatrixData.matrixData.iRead_FBCSetting(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;

                    case MatrixCMD.F_FBCSetup:
                        {
                            CMatrixData.matrixData.m_FbcParam[2] = Package[11];
                            CMatrixData.matrixData.m_FBCClearFlag[0] = Package[12];//FBC dynamicFilterClearFlag
                            CMatrixData.matrixData.m_FBCClearFlag[1] = Package[13];//FBCAll FilterClearFlag                        
                            this.sendMSG_note_MatrixAVIII(CMD, 0); //20161229 20:39

                        }
                        break;

                    case MatrixCMD.F_GetPresetList:
                        {
                            Debug.WriteLine("begin read preset list....");
                            CMatrixData.matrixData.iRead_PresetList(Package);
                            //  Debug.WriteLine("over read preset list....");
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }

                        break;
                    case MatrixCMD.F_InpuPhase:
                    case MatrixCMD.F_OutputPhase:
                        {
                            Debug.WriteLine("begin process chanel phase....");
                            CMatrixData.matrixData.iRead_Phase(Package);//input /output
                            int cmdType = Package[9] * 256 + Package[10];
                            int flag = (cmdType == MatrixCMD.F_OutputPhase ? 1 : 0);
                            this.sendMSG_note_MatrixAVIII(CMD, flag);

                        }
                        break;
                    case MatrixCMD.F_InpuGain:
                    case MatrixCMD.F_OutputGain:
                        {
                            Debug.WriteLine("begin process chanel fader gain....");
                            CMatrixData.matrixData.iRead_FaderGain(Package);//input /output
                            int cmdType = Package[9] * 256 + Package[10];
                            int flag = (cmdType == MatrixCMD.F_OutputGain ? 1 : 0);
                            this.sendMSG_note_MatrixAVIII(CMD, flag);
                        }
                        break;
                    case MatrixCMD.F_LoadFromPC:
                        {

                        }
                        break;
                    case MatrixCMD.F_MemoryImportAck:
                        {
                            int packindex = Package[11];
                            this.sendMSG_note_MatrixAVIII(CMD, packindex);
                            if (packindex < CDefine.Memory_Max_Package - 1) //0..23
                            {
                                CMatrixData.matrixData.sendCMD_ImportLocalMemoryDataToDevice(packindex + 1);

                            }
                            //   Debug.WriteLine("recevie sucess pack index is :{0}", packindex);

                        }
                        break;


                    case MatrixCMD.F_MemoryExport:
                        {
                            int index = Package[11];//max:25 0..24
                            CMatrixData.matrixData.pushMemoryData_toExport(Package);
                            // Debug.WriteLine("memory export with index is {0}", index);
                            this.sendMSG_note_MatrixAVIII(CMD, index);
                        }


                        break;
                    case MatrixCMD.F_OutEQFlat:
                    case MatrixCMD.F_InputEQFlat:
                        {
                            //device not return on this command 
                            // CMatrixData.matrixData.iRead_EQFlat(Package);
                            //   int chindex = Package[11] - 1;//11  ----CHNum--
                            // this.sendMSG_note_MatrixAVIII(CMD, chindex);
                            Debug.WriteLine("over..receive channel eqflat...");
                        }
                        break;
                    case MatrixCMD.F_InputEQ:
                    case MatrixCMD.F_OutputEQ:
                        {
                            CMatrixData.matrixData.iRead_EQ(Package);

                            int cmdType = Package[9] * 256 + Package[10];
                            int chindex = Package[11] - 1; //
                            int eqindex = Package[12] - 1;
                            if (cmdType == MatrixCMD.F_OutputEQ)
                                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]  

                            int feindex = ((eqindex >= 2 && eqindex < CFinal.NormalEQMax) ? eqindex - 2 : eqindex + 8);
                            byte[] eqMsgPack = new byte[2];
                            eqMsgPack[0] = (byte)feindex;
                            eqMsgPack[1] = (byte)chindex;
                            this.sendMSG_note_MatrixAVIII(CMD, eqMsgPack);
                            Debug.WriteLine("over..receive inputEQ...");
                        }
                        break;
                    case MatrixCMD.F_MatrixMixer:
                        {
                            CMatrixData.matrixData.iRead_MatrixSingleChanel(Package);
                            Debug.WriteLine("over..receive matrix.command..");
                            int outputbus = Package[11] - 1;
                            this.sendMSG_note_MatrixAVIII(CMD, outputbus);
                        }
                        break;

                    case MatrixCMD.F_ReChName:
                        {
                            CMatrixData.matrixData.iRead_renewChname(Package);
                            int chindex = Package[11] - 1;
                            int status = Package[12];
                            if (status == 1)
                                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23] 
                            this.sendMSG_note_MatrixAVIII(CMD, chindex);

                        }
                        break;

                    case MatrixCMD.F_PCGetDeviceInfo:
                        {
                            CMatrixData.matrixData.iRead_MatrixA8DeviceInfo(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;

                    case MatrixCMD.F_Mute:
                        {
                            CMatrixData.matrixData.iRead_Mute(Package);
                            //   Debug.WriteLine("over..receive channel mute...");
                            this.sendMSG_note_MatrixAVIII(CMD, 0);

                        }
                        break;
                    case MatrixCMD.F_OutputCOMP:
                    case MatrixCMD.F_InputCOMP:
                        {
                            CMatrixData.matrixData.iRead_Compressor(Package);
                            Debug.WriteLine("over..receive chanel compressor....");
                            int chindex = Package[11] - 1;
                            this.sendMSG_note_MatrixAVIII(CMD, chindex);
                        }
                        break;
                    case MatrixCMD.F_ReadLockPWD:
                        {
                            CMatrixData.matrixData.iRead_LockPwd(Package);
                            this.sendMSG_note_MatrixAVIII(CMD, 0);
                        }
                        break;

                    case MatrixCMD.F_OutputDelay:
                    case MatrixCMD.F_InputDelay:
                        {
                            CMatrixData.matrixData.iRead_Delay(Package);

                            int cmdindex = 9;
                            int cmdType = Package[cmdindex++] * 256 + Package[cmdindex++];
                            int chindex = Package[cmdindex++] - 1;
                            if (cmdType == MatrixCMD.F_OutputDelay)
                                chindex += CMatrixFinal.Max_MatrixChanelNum; //case output:[12..23]
                            Debug.WriteLine("receive input/output delay now.....");
                            this.sendMSG_note_MatrixAVIII(CMD, chindex);

                        }
                        break;
                    case MatrixCMD.F_InputExpGATE:
                        {
                            Debug.WriteLine("receive input expgate now....");
                            CMatrixData.matrixData.iRead_InputExpGate(Package);
                            int chindex = Package[11] - 1;
                            this.sendMSG_note_MatrixAVIII(CMD, chindex);
                            Debug.WriteLine("over..receive channel input expGate....");
                        }
                        break;
                    case MatrixCMD.F_RecallCurrentScene:
                        {
                            switch (appID)
                            {
                                case AppIDList.AP_Matrix_A8:
                                    {
                                        //  Debug.WriteLine("begin receive current sence  ........................");
                                        CMatrixData.matrixData.iRead_CurrentScene(Package.ToArray(), false);
                                        Debug.WriteLine("receive current sence over ........................");
                                        this.sendMSG_note_MatrixAVIII(CMD, 0);
                                    }
                                    break;
                                case AppIDList.AP_RPM_100:  //08
                                    {
                                        Debug.WriteLine("receive rpm200 sence now..");
                                        CMatrixData.matrixData.m_RPMCover.loadPacakge(Package.ToArray(), true);                                       
                                        sendMSG_note_RPM200Page(CMD, 0);
                                    }
                                    break;
                                case AppIDList.AP_RVC_100: //09
                                case AppIDList.AP_RVA_100://10
                                    {
                                        CMatrixData.matrixData.m_RVCover.loadPacakge(Package.ToArray(), true);
                                        sendMSG_note_RVC1000Page(CMD, 0);
                                    }
                                    break;

                            }

                        }
                        break;

                    case MatrixCMD.F_RdDevInfo: //
                        {
                            Debug.WriteLine("appID  receive  {0}.............", appID);

                            switch (appID)
                            {
                                case AppIDList.AP_DLM_A88:
                                case AppIDList.AP_Matrix_A8:
                                    {
                                        CMatrixData.matrixData.iRead_MatrixA8DeviceInfo(Package);
                                        Debug.WriteLine("receive read device info ........................");
                                        this.sendMSG_note_MatrixAVIII(CMD, 0);
                                    }
                                    break;
                                case AppIDList.AP_RVA_100:
                                    CMatrixData.matrixData.iRead_RvA200_DeviceInfo(Package);
                                    this.sendMSG_note_RVA200(CMD, 0);
                                    break;
                                case AppIDList.AP_RVC_100:
                                    CMatrixData.matrixData.iRead_Rvc1000_DeviceInfo(Package);
                                    this.sendMSG_note_RVA200(CMD, 0);
                                    break;
                                case AppIDList.AP_RIO_100:
                                    {
                                        CMatrixData.matrixData.iRead_Rio100_DeviceInfo(Package);
                                        Debug.WriteLine("receive read device info rva200.........cmd{0}", CMD);
                                        this.sendMSG_note_RVA200(CMD, 0);
                                    }
                                    break;


                            }

                        }

                        break;


                }
            #endregion

            }

        }


        private bool checkBegin = false;
        private int checkTimerN = 0;
        private bool checkConnect = false;

        private void scanCheckTimer_Tick(object sender, EventArgs e)
        {
            if (checkBegin)
            {
                //printD("beging check by checkTimer...............@XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                Debug.WriteLine("begin check by scan check timer.");

                checkTimerN++;
                if (checkConnect)
                {
                    checkConnect = false;
                    checkTimerN = 0;
                    this.sendCMD_ScanAllDevice();
                    //to check device
                }
                else  // mean the connect is disconnnect now
                {
                    //
                    if (checkTimerN < 3 && NetCilent.netCilent.isConnected())
                    {

                        this.sendCMD_ScanAllDevice();
                    }
                    else
                    {
                        checkTimerN = 0;
                        scanCheckTimer.Stop();
                        this.doCloseAfterCheckLineConnect();
                        conLed.LedStatus = LED_Status.LED_Normal;
                        connetItem.IsEnabled = true;
                        netConfigItem.IsEnabled = true;
                        this.sendMSG_note_MatrixAVIII(MatrixCMD.MSG_NoticeDisconnect, 0);
                        //close and send imformation to show timer close

                    }


                }
            }
            else
            {
                checkTimerN = 0;
            }



        }
        /// <summary>
        /// 
        /// </summary>
        public void doCloseAfterCheckLineConnect()
        {

            Debug.WriteLine("after detect disconnnect offline so do something below...");
            NetCilent.netCilent.disConnect(); //first clos the socket
            dwpanel.initAllDeviceModuleHead();
            checkConnect = false;
            isConnected = false;
            connetItem.Content = "Connect";


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }

        private void verDlg_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWnd = new AboutWindow();
            aboutWnd.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Module_Type intToMoudleType(int index)
        {
            Module_Type mtype = Module_Type.Mod_CLIV;
            switch (index)
            {
                case 0:
                    mtype = Module_Type.Mod_CLIV;
                    break;
                case 1:
                    mtype = Module_Type.Mod_LAN;
                    break;
                case 2:
                    mtype = Module_Type.Mod_MAVIII;
                    break;
                case 3:
                    mtype = Module_Type.Mod_TxtLft;
                    break;
                case 4:
                    mtype = Module_Type.Mod_TxtRht;
                    break;
                case 5://RVA200
                    mtype = Module_Type.Mod_RVA100;
                    break;
                case 6:
                    mtype = Module_Type.Mod_RIO100;
                    break;
                case 7:
                    mtype = Module_Type.Mod_RVC100;
                    break;
                case 8://RPM200
                    mtype = Module_Type.Mod_RPM100;
                    break;

            }
            return mtype;

        }

        /// <summary>
        /// 
        /// in C#,when you drag a compoent to another target b,so when mouse release 
        /// you can write the event in dragevent for target b
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dwpanel_Drop(object sender, DragEventArgs e)
        {
            Point cvmovePoint = cvmovePoint = e.GetPosition(dwpanel);
            Debug.WriteLine("begin drag with index is : {0}", dragNewindex);
            if (dragNewindex >= 0)
            {
                Module_Type mtype = intToMoudleType(dragNewindex);
                if (dwpanel.isMiscellaneous(mtype))
                {
                    MessageBox.Show("The CL-4 and Lan Interface Module cannot coexist!");
                    return;

                }
                if (dwpanel.getCLIVModuleNum() > 3)
                {
                    MessageBox.Show("Exceed the max number of CL-4");
                    return;

                }
                ComUserMD model = null;
                switch (dragNewindex)
                {
                    case 0: //cl-4 //Router
                        model = new CSwitcherIVModule();
                        model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                        model.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                        //  model.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/Router.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case 1: //lan Interface Moduel
                        model = new CRouterLanModule();
                        model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                        model.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                        break;
                    case 2: //matrixA8
                        model = new CMVIIIModule();
                        break;
                    case 3: //Text-Left
                        model = new CLeftTextModule();
                        break;
                    case 4: //Text-Right
                        model = new CRightTextModule();
                        break;
                    case 5://RVC200
                        model = new CRVC200Module();
                        model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                        model.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                        model.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RVC1000.PNG", UriKind.RelativeOrAbsolute)));

                        //model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0x45, 0x6F, 0x2B)));

                        break;
                    case 6:
                        model = new CRVC200Module();
                        model.setRVAModuleType(ModuleRVAS.MRIO);
                        model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0x45, 0x6F, 0x2B)));
                        model.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RIO200.PNG", UriKind.RelativeOrAbsolute)));
                        model.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xD7, 0xE4, 0xBD)));
                        model.setBotomCap("RIO200");
                        //ModuleRVAS.MRIO
                        break;
                    case 7:
                        model = new CRVC200Module();
                        model.setRVAModuleType(ModuleRVAS.MRVC);
                        model.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0x60, 0x4a, 0x7B)));
                        model.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xcc, 0xc1, 0xda)));
                        model.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RVC1000.PNG", UriKind.RelativeOrAbsolute)));
                        model.setBotomCap("RVC1000");
                        break;
                    case 8:
                        model = new CRVC200Module();
                        model.setRVAModuleType(ModuleRVAS.RPM);
                        model.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RPM200.PNG", UriKind.RelativeOrAbsolute)));

                        model.setBotomCap("RPM200");
                        break;

                }
                dwpanel.AddChildren(model, cvmovePoint, true);
                model.onMouseDoubleClickEvent += new ComUserMD.mouseDoubleClick(moduleClick);
            }
        }

        /// <summary>
        /// module doublclick proces.........below........
        /// </summary>
        /// <param name="sender"></param>
        private void moduleClick(object sender)
        {
            //  Debug.WriteLine("user control module click now----------------------------");
            ComUserMD userCtl = sender as ComUserMD;
            CMatrixData.matrixData.activeDevProvision.devProvisionCopy(userCtl.devinfo.devProv);
            CMatrixData.matrixData.devProvision.devProvisionCopy(CMatrixData.matrixData.activeDevProvision);
            Debug.WriteLine("module clik appid {0} deviceid {1}", CMatrixData.matrixData.activeDevProvision.pMachineID, CMatrixData.matrixData.activeDevProvision.pDeviceID);
            string devname = CMatrixData.matrixData.activeDevProvision.getStrDevName();
            switch (userCtl.devinfo.devModuleType)
            {
                case Module_Type.Mod_MAVIII:
                    {
                        CMatrixData.matrixData.setDevicename(devname);
                        var mtrixPage = new MatrixPage();
                        checkBegin = false;
                        var sres = mtrixPage.ShowDialog();
                        if (sres.HasValue && sres.Value)
                        {
                            checkBegin = true;
                        }
                    }
                    break;
                //case Module_Type.Mod_RVA100:
                //    {
                //        CMatrixData.matrixData.setRVA200DevName(devname);
                //        CMatrixData.matrixData.rvaDevProvision.devProvisionCopy(CMatrixData.matrixData.activeDevProvision);
                //        var rvaPage = new RVCSeriesWnd();
                //       // rvaPage.setBackground(new SolidColorBrush(Color.FromRgb(0xE4, 0x6C, 0x0A)));
                //      //  rvaPage.moduleType = Module_Type.Mod_RVA100;
                //        checkBegin = false;
                //        CMatrixData.matrixData.sendCMD_ReadDevinfo(CMatrixData.matrixData.activeDevProvision);
                //        var sres = rvaPage.ShowDialog();
                //        if (sres.HasValue && sres.Value)
                //        {
                //            checkBegin = true;
                //        }
                //    }
                //    break;

                case Module_Type.Mod_RVC100:
                case Module_Type.Mod_RVA100:
                    {
                        CMatrixData.matrixData.setRVC100DevName(devname);
                        CMatrixData.matrixData.rvcDevProvision.devProvisionCopy(CMatrixData.matrixData.activeDevProvision);
                        CMatrixData.matrixData.sendCMD_recallRPVCurrence();
                        var rvcPage = new RVCSeriesWnd();                      
                        CMatrixData.matrixData.m_RVCover.setDevName(devname);
                        checkBegin = false;
                        var sres = rvcPage.ShowDialog();
                        if (sres.HasValue && sres.Value)
                        {
                            checkBegin = true;
                        }            
                    

                    }
                    break;
                case Module_Type.Mod_RIO100:
                    {


                        CMatrixData.matrixData.rioDevProvision.devProvisionCopy(CMatrixData.matrixData.activeDevProvision);
                        CMatrixData.matrixData.sendCMD_ReadDevinfo(CMatrixData.matrixData.activeDevProvision);
                        var rvaPage = new RVADevPage();
                        rvaPage.setBackground(new SolidColorBrush(Color.FromRgb(0x45, 0x6F, 0x2B)));
                        CMatrixData.matrixData.setRIO100DevName(devname);
                        rvaPage.moduleType = Module_Type.Mod_RIO100;
                        checkBegin = false;
                        var sres = rvaPage.ShowDialog();
                        if (sres.HasValue && sres.Value)
                        {

                            checkBegin = true;
                        }

                    }
                    break;
                case Module_Type.Mod_RPM100:
                    {

                        CMatrixData.matrixData.rpmDevProvision.devProvisionCopy(CMatrixData.matrixData.activeDevProvision);
                        CMatrixData.matrixData.sendCMD_recallRPMCurrence();
                        var rpmPage = new RPM200Page();
                        CMatrixData.matrixData.m_RPMCover.setDevName(devname);

                        checkBegin = false;
                        var sres = rpmPage.ShowDialog();
                        if (sres.HasValue && sres.Value)
                        {
                            checkBegin = true;
                        }

                    }
                    break;



            }



        }
        /// <summary>
        /// 
        /// </summary>
        ContextMenu rcontextMenu = new ContextMenu();
        private void initialContexteMenu()
        {

            if (rcontextMenu == null)
            {
                rcontextMenu = new ContextMenu();
                rcontextMenu.Items.Add(new MenuItem().Header = "Add to Faver");
                rcontextMenu.Items.Add(new MenuItem().Header = "Copy");
                rcontextMenu.Items.Add(new MenuItem().Header = "delete");
                rcontextMenu.Items.Add(new MenuItem().Header = "FCopy");

            }


        }


        //when selected item change now
        private int dragNewindex = 0;
        private void devieListTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // int selindex;
            CTreeView tv = sender as CTreeView;
            TreeViewItem titem = (TreeViewItem)tv.SelectedItem;
            dragNewindex = titem.TabIndex;
            Debug.WriteLine("treeview selected change item index is :  {0}", dragNewindex);


        }

        /// <summary>L
        /// for scan all devices which in all the same router
        /// </summary>
        public void sendCMD_ScanAllDevice()
        {
            devScanNum = 0;
            m_recCmd.Clear();
            scanDevList.Clear();//first clear the bufferk
            //01 20 03 00 0D FF FF FF FF 00 03 00 40
            Byte[] m_cast = { 0x01, 0x20, 0x03, 0x00, 0x0D, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x03, 0x00, 0x40 };
            if (NetCilent.netCilent != null && NetCilent.netCilent.isConnected())
            {
                NetCilent.netCilent.sendByte(m_cast);
                Debug.WriteLine("sendcmd scan all device over.........");
            }


        }

        public void ReleaseSocketTCP_Connect()
        {
            try
            {
                //IoSocketProces.ioProc.clearSocket();
                NetCilent.netCilent.clear();

            }
            catch (Exception ec)
            {
                Debug.WriteLine("disconnnect error  {0}", ec.ToString());

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            clearTimer();
            dwpanel.saveChildrenToDataBase();
            ReleaseSocketTCP_Connect();
            Debug.WriteLine("socket close now...");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dwpanel.readChilrendFromDataBase(moduleClick);
            cLocalIP.Content = IPProces.getIPAddress();

        }

        private void mtrixBtn_Click(object sender, RoutedEventArgs e)
        {
            InputHexDialog inpDlg = new InputHexDialog();
            var sres = inpDlg.ShowDialog();
            if (sres == true)
            {
                string str = inpDlg.devID.ToString("X4");
                Debug.WriteLine("input text is : " + str);
            }

        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connetItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("scan now...");


        }



        /// <summary>
        /// connnect to items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connetItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (_connectIP == null || _connectIP.Trim().Length < 1)
            {
                MessageBox.Show("Remote device ip is empty or invalid.");
                return;
            }
            conTimer.Start();
            if (isRepeat)
            {
                MessageBox.Show("Don't repeat it within 3 seconds.");
                return;
            }
            try
            {
                isRepeat = true;
                if (!isConnected && UtilCover.IPCheck(_connectIP))
                {
                    Debug.WriteLine("connnect with connect item click");
                    btn.IsEnabled = false;
                    netConfigItem.IsEnabled = false;
                    CMatrixData.matrixData.m_A8DevList.Clear();

                    this.setConnectStatus(true);
                }
                else
                {
                    Debug.WriteLine("disconnnect with connect item click");
                    btn.IsEnabled = false;
                    this.setConnectStatus(false);
                    conTimer.Stop();
                }


            }
            catch (Exception ec)
            {
                Debug.WriteLine("error socket connect :  " + ec.Message);
                IsEnabled = true;
                _connectIP = string.Empty;
                isConnected = false;
                connetItem.IsEnabled = true;
                netConfigItem.IsEnabled = true;
                // this.setConnectStatus(false);
            }

        }


        private void netConfigItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (isConnected)
            {
                doCloseAfterCheckLineConnect();
            }

            _connectIP = string.Empty;
            var scanf = new WScanForm();
            scanf.setPbarStyle((Style)FindResource("grdPbr"));
            scanf.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/bScan.png", UriKind.RelativeOrAbsolute)));
            var sres = scanf.ShowDialog();
            if (sres.HasValue && sres.Value)
            {
                edRemotDevIP.Content = _connectIP = scanf.getScanedIP();
                dwpanel.setRemoteIP(_connectIP);
                Debug.WriteLine("scaned ip proces below.............." + _connectIP);
                connetItem.IsEnabled = true;
            }


        }


        #region Message transform between the matrix define below
        public void sendMSG_note_MatrixAVIII(int hcmd, int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.MatrixGUIClass, MatrixCMD.MatrixA8_MSG_Transfer, hcmd, lcmd);

        }



        public void sendMSG_note_MatrixAVIII(int hcmd, byte[] data)
        {

            CMDSender.sendMsgWithData(MatrixCMD.MatrixGUIClass, MatrixCMD.MatrixA8_MSG_Transfer,
                data, hcmd);

        }
        #endregion


        #region transform between the RPM200 define below
        public void sendMSG_note_RPM200Page(int hcmd, int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.RPMGUIClass, MatrixCMD.RPM200_MSG_Transfer, hcmd, lcmd);

        }

        #endregion


        public void sendMSG_note_RVC1000Page(int hcmd, int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.RVCUIClass, MatrixCMD.RVC1000_MSG_Transfer, hcmd, lcmd);

        }

        #region Message transform between the Rva200 define below



        public void sendMSG_note_RVA200(int hcmd, int lcmd)
        {
            CMDSender.sendMsgWithoutData(MatrixCMD.RVCUIClass, MatrixCMD.RVC1000_MSG_Transfer, hcmd, lcmd);

        }



        public void sendMSG_note_RVA200(int hcmd, byte[] data)
        {

            CMDSender.sendMsgWithData(MatrixCMD.RVAGUIClass, MatrixCMD.RVA200_MSG_Transfer,
                data, hcmd);

        }

        #endregion

        private void kBlink_Click(object sender, RoutedEventArgs e)
        {
            conLed.startBlink();
        }

        private void stopBlink_Click(object sender, RoutedEventArgs e)
        {
            conLed.stopBlink();
        }






    }
}
