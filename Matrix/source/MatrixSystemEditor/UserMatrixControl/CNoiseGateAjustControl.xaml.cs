﻿using Lib.Controls;

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
using System.Diagnostics;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CNoiseGateAjustControl.xaml
    /// </summary>
    public partial class CNoiseGateAjustControl : UserControl
    {
        public static string[] strNoiseRatioTableStr =
{
    "1:1",     //0
    "1.2:1",     //1
    "1.3:1",     //2
    "1.5:1",     //3
    "1.7:1",     //4
    "2.0:1",     //5
    "2.2:1",     //6
    "2.3:1",    //7
    "2.5:1",     //8
    "3.0:1",     //9
    "3.5:1",     //10
    "4.0:1",     //11
    "4.5:1",     //12
    "5.0:1",     //13
    "5.5:1",     //14
    "6.0:1",     //15
    "6.5:1",     //16
    "7.0:1",     //17
    "7.5:1",     //18
    "8.0:1",     //19
    "8.5:1",     //20
    "9.0:1",     //21
    "9.5:1",     //22
    "10:1",    //23
    "Limit",    //24
};

        public static string[] strNoiseReleseTable =
    {
    "10ms",    //00
    "20ms",    //01
    "25ms",    //02
    "30ms",    //03
    "50ms",    //04
    "100ms",    //05
    "120ms",   //06
    "150ms",   //07
    "175ms",   //08
    "200ms",   //09
    "250ms",   //10
    "300ms",   //11
    "350ms",   //12
    "400ms",   //13
    "450ms",   //14
    "500ms",   //15
    "550ms",   //16
    "600ms",   //17
    "650ms",   //18
    "700ms",  //19
    "750ms",  //20
    "800ms",  //21
    "850ms",  //22
    "900ms",  //23
    "1s",  //24
};
        public static string[] strNoiseAttackTime =
    {  //25
    "10mS ",    //0
    "15mS ",    //1
    "20mS ",    //2
    "25mS ",    //3
    "30mS ",    //4
    "35mS ",    //5
    "40mS ",    //6
    "45mS ",    //7
    "50mS ",    //8
    "55mS ",    //9
    "60mS ",    //10
    "65mS ",    //11
    "70mS ",    //12
    "75mS ",    //13
    "80mS ",    //14
    "85mS ",    //15
    "90mS ",    //16
    "95mS ",    //17
    "100mS",    //18
    "105mS",    //19
    "110mS",    //20
    "115mS",    //21
    "120mS",    //22
    "140mS",    //23
    "150mS",  //24
};



        private const int DefaultThres = 50;
        public int throsholdMax
        {
            get { return (int)GetValue(throsholdMaxProperty); }
            set { SetValue(throsholdMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for throsholdMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty throsholdMaxProperty =
            DependencyProperty.Register("throsholdMax", typeof(int), typeof(CNoiseGateAjustControl),
        new FrameworkPropertyMetadata(DefaultThres, FrameworkPropertyMetadataOptions.None, onThresholdMaxChange
            ));
        private static void onThresholdMaxChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CNoiseGateAjustControl;
            if (mcontrol != null)
            {
                mcontrol.throsholdMax=(int)args.NewValue;        
                mcontrol.threSpin.Maximum = (int)args.NewValue;
                mcontrol.refreshControl();
            }
        }


        public int thresRadix
        {
            get { return (int)GetValue(thresRadixProperty); }
            set { SetValue(thresRadixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for thresRadix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty thresRadixProperty =
            DependencyProperty.Register("thresRadix", typeof(int), typeof(CNoiseGateAjustControl), new PropertyMetadata(-30));

        //
        private const double defaultStep = 1.0;
        public double dp_thresStep
        {
            get { return (double)GetValue(dp_thresStepProperty); }
            set { SetValue(dp_thresStepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for dp_thresStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dp_thresStepProperty =
            DependencyProperty.Register("dp_thresStep", typeof(double), typeof(CNoiseGateAjustControl), new PropertyMetadata(defaultStep));

        

        

        public CNoiseGateAjustControl()
        {
            InitializeComponent();

            initital();
        }
        /// <summary>
        /// threshold:50,other is 24 as default
        /// </summary>
        public LimitEdit glimtData = new LimitEdit();
        /// <summary>
        /// 0:Exp/Gate 1:compressor
        /// </summary>
        public int LimitFlag
        {
            get { return (int)GetValue(LimitFlagProperty); }
            set { SetValue(LimitFlagProperty, value); }
        }

        private const int DefaultLimitFlag = 0;
        public static readonly DependencyProperty LimitFlagProperty =
            DependencyProperty.Register("LimitFlag", typeof(int), typeof(CNoiseGateAjustControl),
        new FrameworkPropertyMetadata(DefaultLimitFlag, FrameworkPropertyMetadataOptions.None, onLimitFlagChange
            ));

        private static void onLimitFlagChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CNoiseGateAjustControl;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                mcontrol.LimitFlag = (int)args.NewValue;
                mcontrol.updateTitle();
            }
        }

        public void updateTitle()
        {
            if (LimitFlag == 0)
            {
                FlatBtn.Content = "Flat EXP";
            }
            else
            {
                FlatBtn.Content = "Flat Comp";
            }
            this.InvalidateVisual();

        }
        //spinchange with itag set order:Threshold 0; attack:1  ratio:2 release 3; bypass 4

        private void initital()
        {
            if (glimtData == null)
                glimtData = new LimitEdit();
            threSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(noiseCtlSpinValueClickChange);
            ratioSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(noiseCtlSpinValueClickChange);
            attackSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(noiseCtlSpinValueClickChange);
            releaseSpin.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(noiseCtlSpinValueClickChange);


        }

        private void noiseCtlSpinValueClickChange(object sender, int cvalue, EQFragType etype, int itg, EventArgs e)
        {
            //
            var scontrol = sender as SpinnerControl;
            int tg = scontrol.iTag;
            switch (tg)
            {
                case 0://threshold
                    glimtData.limit_threshold = (byte)cvalue;
                    break;
                case 1: //attack
                    glimtData.limit_attack = (byte)cvalue;
                    break;
                case 2: //ratio
                    glimtData.limit_ratio = (byte)cvalue;
                    break;
                case 3://release
                    glimtData.limit_release = cvalue;
                    break;
            }
            if (onNoiseGateControlChangedEvent != null)
            {
                onNoiseGateControlChangedEvent(this);
            }




        }

        public void refreshControl()
        {
            byte thres = glimtData.limit_threshold;
            byte ratio = glimtData.limit_ratio;
            byte attack = glimtData.limit_attack;
            int release = glimtData.limit_release;

            threSpin.Value = thres;
            ratioSpin.Value = ratio;
            attackSpin.Value = attack;
            releaseSpin.Value = release;
            bypasBtn.IsSelected = (glimtData.limit_bypas > 0);
            //---------------
            int tmp = glimtData.limit_threshold;
            double ftmp = thresRadix + tmp * dp_thresStep;
            Debug.WriteLine("noise gatecontrol:   thresradix : {0} thres step: {1}  threshold value: {2}",thresRadix,dp_thresStep,tmp);
            string strThres = string.Format("{0}dB", ftmp);

            string strRatio = strNoiseRatioTableStr[ratio];
            string strAttack = strNoiseAttackTime[attack];
            string strRelease = strNoiseReleseTable[release];

            threSpin.valueTxt = strThres;
            ratioSpin.valueTxt = strRatio;
            attackSpin.valueTxt = strAttack;
            releaseSpin.valueTxt = strRelease;
        }


        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(CNoiseGateAjustControl), new PropertyMetadata(0));



        /// <summary>
        /// bypass click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bypasBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;

            if (btn.IsSelected)
            {
                glimtData.limit_bypas = 0;
            }
            else
            {
                glimtData.limit_bypas = 1;
            }
            //-------------------
            if (onNoiseGateControlChangedEvent != null)
            {
                onNoiseGateControlChangedEvent(this);
            }

        }

        public delegate void noiseGateControlChanged(Object sender);
        public event noiseGateControlChanged onNoiseGateControlChangedEvent;

        private void FlatBtn_Click(object sender, RoutedEventArgs e)
        {
            glimtData.clearData();
            if(iTag==0)//expGate
            {
                glimtData.limit_attack = 8;
                glimtData.limit_release = 15;
            }
            else if(iTag==1)
            {
                glimtData.limit_attack = 8;
                glimtData.limit_release = 11;

            }
            if (onNoiseGateControlChangedEvent != null)
            {
                onNoiseGateControlChangedEvent(this);
            }

        }






    }
}
