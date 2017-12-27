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
using System.Diagnostics;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CEQControlII.xaml
    /// </summary>
    public partial class CEQControlII : UserControl
    {

        public EQEdit[] m_eqEdit = new EQEdit[CFinal.NormalEQMax];
        public byte mEntireByPass = 0;

        public CEQControlII()
        {
            InitializeComponent();
            initializeParameter();

        }


        public void setEQFlat()
        {
            int CEQMax = CFinal.NormalEQMax;
            for (byte i = 0; i < CEQMax; i++)
            {

                m_eqEdit[i].eq_byPass = 0;
                m_eqEdit[i].eq_Filterindex = 0;
                m_eqEdit[i].eq_freqindex = CFinal.inital_freq_ary[i];
                m_eqEdit[i].eq_gainindex = CFinal.Inital_Gain_index;
                m_eqEdit[i].eq_qfactorindex = CFinal.Inital_QV_index;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        public void refreshControl()
        {
            for (int i = 0; i < CFinal.NormalEQMax; i++)
            {
                updateGUI_withEqindex(i);
            }
            bool sts = (mEntireByPass == 0);
            //   Debug.WriteLine("refresh eqcontroll found bypass value {0} ",sts);
            setEntireEnabledStatus(sts);

        }

        /// <summary>
        /// update eq gui
        /// </summary>
        /// <param name="eqindex"></param>
        public void updateGUI_withEqindex(int eindex)
        {
            // CheckBox fcbox = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "chbox");
            string strName = string.Format("ckbypas{0}", eindex);//check bypas
            var sOb = (CheckBox)this.FindName(strName);
            if (sOb != null)
                sOb.IsChecked = (m_eqEdit[eindex].eq_byPass > 0);


            //
            strName = string.Format("cbxeqType{0}", eindex);//eq filter type contain HLPF bypasfilter
            var cbox = (TComboBox)this.FindName(strName);
            if (cbox != null)
            {
                int findex = m_eqEdit[eindex].eq_Filterindex;
                cbox.setSelectindex(findex);
            }

            //
            strName = string.Format("spinQ_{0}", eindex);//eq qfactor
            var spinQ = (SpinnerControl)this.FindName(strName);
            if (spinQ != null)
            {
                spinQ.Value = m_eqEdit[eindex].eq_qfactorindex;
                //  Debug.WriteLine("spinq control  is not null  and  qvalue is :{0}..........", spinQ.Value);                
            }

            //
            strName = string.Format("spinGain_{0}", eindex);//eq qfactor
            var spinG = (SpinnerControl)this.FindName(strName);
            if (spinG != null)
                spinG.Value = m_eqEdit[eindex].eq_gainindex;

            //
            strName = string.Format("spinFreq_{0}", eindex);//freq value
            var spinFreq = (SpinnerControl)this.FindName(strName);
            if (spinFreq != null)
            {
                spinFreq.Value = m_eqEdit[eindex].eq_freqindex;
                // Debug.WriteLine("freq name is : " + strName + "   eindex is : {0} freqindex : {1}", eindex, spinFreq.Value);
            }


        }



        private void initializeParameter()
        {
            if (m_eqEdit == null)
                m_eqEdit = new EQEdit[CFinal.NormalEQMax];
            for (int i = 0; i < CFinal.NormalEQMax; i++)
            {
                m_eqEdit[i] = new EQEdit();
            }
            setEQFlat();//reset EQ parameters

            // strHL_FILTER        
            cbxeqType8.ItemsSource = CFinal.strHL_FILTER;
            cbxeqType8.setSelectindex(0); //spinFreq_8
            cbxeqType9.ItemsSource = CFinal.strHL_FILTER;
            cbxeqType9.setSelectindex(0);

            //set and initialize controls below
            TraverseChildrenControls tchildrens = new TraverseChildrenControls();
            foreach (object o in tchildrens.GetChildren(eqGrid, 1))
            {

                if (o.GetType() == typeof(SpinnerControl))
                {
                    SpinnerControl spinControl = (SpinnerControl)o;
                    spinControl.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onSpinChangeHandle);

                }
                if (o.GetType() == typeof(CheckBox))  //bypass or nots
                {
                    var cbx = (CheckBox)o;
                    cbx.Click += checkBypas_Click;

                }
                if (o.GetType() == typeof(TComboBox))  //combobox filter 
                {

                    var cbox = (TComboBox)o;
                    cbox.SelectionChanging += new SelectionChangingEventHandler(eqcombox_SelectionChanging);

                }


            }
            //xover highfilter pass and low filter pass
            spinFreq_8.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onSpinChangeHandle);
            spinFreq_9.onSpinValueClickChangeEvent += new SpinnerControl.spinValueClickChange(onSpinChangeHandle);
            //
            cbxeqType8.SelectionChanging += eqcombox_SelectionChanging;
            cbxeqType9.SelectionChanging += eqcombox_SelectionChanging;


        }
        /// <summary>
        /// eq filter selected changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eqcombox_SelectionChanging(object sender, SelectionChangingEventArgs e) //eq type
        {
            //  e.Cancel = true;
            TComboBox cbx = sender as TComboBox;
            int eindex = cbx.iTg;
            int filterindex = cbx.SelectedIndex;
            m_eqEdit[eindex].eq_Filterindex = (byte)filterindex;
            if (onEQControlChangedEvent != null)
            {
                onEQControlChangedEvent(this, eindex);
            }



        }
        public delegate void eqControlChanged(Object sender, int eqindex);
        public event eqControlChanged onEQControlChangedEvent;


        //click bypass 
        /// <summary>
        /// eq bypass click envent change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBypas_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = sender as CheckBox;
            string estr = cbx.Name.Substring(cbx.Name.Length - 1, 1);
            int eindex = int.Parse(estr);

            if (cbx.IsChecked.HasValue)
            {
                byte tmp = (byte)((bool)cbx.IsChecked ? 1 : 0);
                m_eqEdit[eindex].eq_byPass = tmp;
                if (onEQControlChangedEvent != null)
                {
                    onEQControlChangedEvent(this, eindex);
                }

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fvalue"></param>
        /// <param name="etpe"></param>
        /// <param name="itg"></param>
        /// <param name="e"></param>
        private void onSpinChangeHandle(object sender, int fvalue, EQFragType etpe, int itg, EventArgs e)
        {
            Debug.WriteLine("current fvalue is :  {0}\t eqfratype :{1}\t iTag is {2}\t",
                  fvalue, etpe, itg);
            int eindex = itg;
            switch (etpe)
            {
                case EQFragType.EQ_QV:
                    m_eqEdit[eindex].eq_qfactorindex = (byte)fvalue;
                    break;
                case EQFragType.EQ_Gain:
                    m_eqEdit[eindex].eq_gainindex = (byte)fvalue;
                    break;
                case EQFragType.EQ_Freq:
                    m_eqEdit[eindex].eq_freqindex = fvalue;
                    break;
            }
            if (onEQControlChangedEvent != null)
            {
                onEQControlChangedEvent(this, eindex);
            }

        }


        /// <summary>
        /// EQ flat click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flatBtn_Click(object sender, RoutedEventArgs e)
        {
            setEQFlat();//only change data ,but not update the GUI
            // refreshControl();
            if (onEQControlChangedEvent != null) //eq flat
            {

                onEQControlChangedEvent(this, CFinal.EQEntireFlat);
            }
        }

        /// <summary>
        /// EQ bypass click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bypasAllBtn_Click(object sender, RoutedEventArgs e)
        {
            //
            CSwitcher btn = sender as CSwitcher; //only change data ,but not update the GUI
            if (btn.IsSelected)
            {
                mEntireByPass = 0;
            }
            else
                mEntireByPass = 1;
            if (onEQControlChangedEvent != null) //eq flat
            {
                onEQControlChangedEvent(this, CFinal.EQEntireBypass);
            }


        }
        public void updateWholeBypassStatus()
        {
            bool sts = (mEntireByPass == 0);          
            setEntireEnabledStatus(sts);
        }
        /// <summary>
        /// set entire enable or not status
        /// </summary>
        /// <param name="sts"></param>
        private void setEntireEnabledStatus(bool sts)
        {

            TraverseChildrenControls tchildrens = new TraverseChildrenControls();
            foreach (object o in tchildrens.GetChildren(eqGrid, 1))
            {

                if (o.GetType() == typeof(SpinnerControl))
                {
                    SpinnerControl spinControl = (SpinnerControl)o;
                    spinControl.IsEnabled = sts;

                }
                if (o.GetType() == typeof(CheckBox))  //bypass or nots
                {
                    var cbx = (CheckBox)o;
                    cbx.IsEnabled = sts;

                }
                if (o.GetType() == typeof(TComboBox))  //combobox filter 
                {

                    var cbox = (TComboBox)o;
                    cbox.IsEnabled = sts;

                }


            }
            //xover highfilter pass and low filter pass
            spinFreq_8.IsEnabled = sts;
            spinFreq_9.IsEnabled = sts;
            //
            cbxeqType8.IsEnabled = sts;
            cbxeqType9.IsEnabled = sts;
            bypasAllBtn.IsSelected = !sts;

        }




    }
}
