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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Collections;


namespace CommLibrary
{


    /// <summary>
    /// Interaction logic for WScanForm.xaml
    /// </summary>
    public partial class WScanForm : Window
    {

        /// <summary>
        /// udpSocket begin.........................................
        /// </summary>
        /// 
        private DeviceScaner udpScan = new DeviceScaner();
        //
        private const int Send_Delay = 50;
        private string localIP;
        public int delayMills = 5;

        public WScanForm()
        {
            initialSocket_and_Data();
            InitializeComponent();

        }

        public int DelayMills
        {
            get;
            set;
        }


        private void onScanStop(object sender, EventArgs e)
        {
            Debug.WriteLine("udp scan  stop now...");
            dtInvoke(stopScanUpdateRefresh);
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

        public void stopScanUpdateRefresh()
        {
            pbar.IsIndeterminate = false;
            beginScanBtn.IsEnabled = true;
            beginScanBtn.Content = "Scan";
        }
        public void startScanUpdateRefresh()
        {
            scanBox.Items.Clear();
            pbar.IsIndeterminate = true;
            beginScanBtn.IsEnabled = false;
            beginScanBtn.Content = "Stop";

        }
        private void onScanBegin(object sender, EventArgs e)
        {
            //  Debug.WriteLine("udp scan  begin now...");
            dtInvoke(startScanUpdateRefresh);


        }

        private void initialSocket_and_Data()
        {

            if (udpScan == null)
            {
                udpScan = new DeviceScaner();
                Debug.WriteLine("nvscan is null");
            }
            udpScan.onUDPBeinScanEvent += new DeviceScaner.onDeviceBeginScan(onScanBegin);
            udpScan.onUDPStopScanEvent += new DeviceScaner.onDeviceStopScan(onScanStop);
            udpScan.onScanChangedEvent += new DeviceScaner.onDeviceScanChanged(onScanChanged);

            Debug.WriteLine("nvscan load status result is : {0}", udpScan.isLoadedFailure);



        }

        private void onScanChanged(Object sender, RouterInfo rpinfo, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                               (ThreadStart)delegate()
                                               {
                                                   string res = string.Format("                      {0}                      {1}", rpinfo.RouterAddr, rpinfo.RouterMac);
                                                   scanBox.Items.Add(res);
                                               });
            Debug.WriteLine("udp scan receive.......ip is : " + rpinfo.RouterAddr);

        }

        private void beginScanBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(100);
            try
            {
                if (!udpScan.isLoadedFailure)
                    udpScan.startScan();
                else
                {
                    Debug.WriteLine("");
                    if (udpScan != null)
                    {
                        udpScan.hasStopScan = false;
                        udpScan.stopScan();
                        MessageBox.Show("Socket port 8000 or 4899 is occupied!");

                    }

                    // return;
                }
            }
            catch (Exception ec)
            {


            }

        }
        public void setPbarStyle(Style mstyle)
        {
            pbar.Style = mstyle;
        }

        private void beginConBtn_Click(object sender, RoutedEventArgs e)
        {


        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //
            // Debug.WriteLine("windows is closing now.........................");
            beginScanBtn.Content = "Scan";
            udpScan.clearSocket();
            pbar.IsIndeterminate = false;
            this.DialogResult = true;
            //  Debug.WriteLine("windows is closing over.........................");

        }

        private void btnMac_Click(object sender, RoutedEventArgs e)
        {
            //for test



        }

        private void scanBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox box = (ListBox)e.OriginalSource;
            int index = box.SelectedIndex;

            //   Debug.WriteLine("select item index is : {0}", index); 
            if (index >= 0 && index < udpScan.scanedIpList.Count)
                ipBox.Text = udpScan.scanedIpList[index].RouterAddr;

        }

        private void beginClose_Click(object sender, RoutedEventArgs e)
        {
            //close
            beginScanBtn.Content = "Scan";
            pbar.IsIndeterminate = false;
            this.DialogResult = true;

        }


        /// <summary>
        /// getScan IP case when close form
        /// </summary>
        /// <returns></returns>
        public string getScanedIP()
        {

            if (udpScan.scanedIpList.Count < 1)
                return null;
            else
            {
                if (scanBox.SelectedIndex == -1)
                    return udpScan.scanedIpList[0].ToString();
                else
                {
                    int index = scanBox.SelectedIndex;
                    return udpScan.scanedIpList[index].RouterAddr;

                }
            }


        }

    }
}
