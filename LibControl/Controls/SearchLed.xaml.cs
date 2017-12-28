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

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for SearchLed.xaml
    /// </summary>
    /// 

    public partial class SearchLed : UserControl
    {
        public SearchLed()
        {
            InitializeComponent();
            strLedTitle = srchLb.Text.Trim();
            initialTimer();
        }

        private int dotNum = -1;

        public void setLedDotNum(int num)
        {
            switch (num)
            {
                case 0:
                    dotLb.Text = ".";
                    break;
                case 1:
                    dotLb.Text = "..";
                    break;
                case 2:
                    dotLb.Text = "...";
                    break;
                case 3:
                    dotLb.Text = "....";
                    break;
                case 4:
                    dotLb.Text = ".....";
                    break;
                case 5:
                    dotLb.Text = "......";
                    break;

            }

        }
        public const int eSwitchInterval = 900;
        private System.Timers.Timer loopTimer;

        private void initialTimer()
        {
            loopTimer = new System.Timers.Timer(eSwitchInterval);
            loopTimer.AutoReset = true;
            loopTimer.Enabled = true;
            loopTimer.Elapsed += new System.Timers.ElapsedEventHandler(loopTimer_ElapsedHandle);
            // loopTimer.Start();
            stopSearch();
        }
        public void setTextColor(Brush texB)
        {
            srchLb.Foreground = texB;
            dotLb.Foreground = texB;
        }
        

        public void stopSearch()
        {
            dotLb.Text = "";
            srchLb.Text = "";
            dotNum = -1;
            loopTimer.Stop();
            this.Height = 0.01;
            scanNum = 0;
        }

        public void startSearch()
        {
            this.Height = 50;
            dotLb.Text = "";
            loopTimer.Start();
            dotNum = -1;
            srchLb.Text = strLedTitle;
            scanNum = 0;
        }
        


        private const int defaultDotMax = 6;

        private  const int DefaultMaxScanNum = 12;
        private int scanNum = 0;





        public int maxDelaySecTimes
        {
            get { return (int)GetValue(maxDelaySecTimesProperty); }
            set { SetValue(maxDelaySecTimesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for maxDelaySecTimes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty maxDelaySecTimesProperty =
            DependencyProperty.Register("maxDelaySecTimes", typeof(int), typeof(SearchLed), new PropertyMetadata(DefaultMaxScanNum));     

       



        private void loopTimer_ElapsedHandle(object sender, System.Timers.ElapsedEventArgs e)
        {
            scanNum++;
            dotNum++;
            if (dotNum >= defaultDotMax)
                dotNum = -1;

            Task.Factory.StartNew(() =>
            {
                BeginInvokeLed(dotNum);
            });

            //   Debug.WriteLine("dotNum is  {0}", dotNum);
        }

        public string strLedTitle;
        public void setLedTitle(string strT)
        {
            srchLb.Text = strT;
            strLedTitle = strT;
        }

        public void BeginInvokeLed(int num) //async
        {
            Dispatcher.Invoke(() =>
            {
                setLedDotNum(num);
                if (scanNum >= maxDelaySecTimes)
                {
                    stopSearch();
                }

            }
            );

        }

    }
}
