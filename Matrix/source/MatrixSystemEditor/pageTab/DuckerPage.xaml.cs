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
    /// Interaction logic for DuckerPage.xaml
    /// </summary>
    public partial class DuckerPage : Page
    {
        private MatrixPage _parentWin;

        public MatrixPage ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public DuckerPage()
        {
            InitializeComponent();
            duckerCtl.onDuckerValueControlChangedEvent += new CDuckerSourceControl.DuckerValueControlChanged(onDuckerChanged);
            initialDuckerSliderAbout();
        }

        private void initialDuckerSliderAbout()
        {
            CSlider dslider = null;
            for (int i = 0; i < CFinal.Max_DuckerParms - 1; i++)
            {
                string strSlider = string.Format("dslider_{0}", i);
                dslider = (CSlider)this.FindName(strSlider);
                if (dslider != null)
                {
                    dslider.iTag = i;
                    dslider.onSliderMouseMoveEvent += new CSlider.sliderMouseMove(onDuckerSliderValueChanged);
                }

            }


        }

        private void onDuckerChanged(object sender, int flag)
        {
            Array.Copy(duckerCtl.m_DuckerSourch, CMatrixData.matrixData.m_DuckerSourch, CDefine.Max_DuckerInputMixer);//20170722
            Array.Copy(duckerCtl.m_DuckerBgmLocal, CMatrixData.matrixData.m_DuckerBgm, CFinal.IO_MaxMatrixBus);//20170722

            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                if (flag == 0) //up mixer
                {

                    CMatrixData.matrixData.sendCMD_DuckerMixer();
                }
                else
                {

                    CMatrixData.matrixData.sendCMD_DuckerGain();
                }

            }
            else
            {
                updateDuckerMixer();

            }
        }
        public void updateDuckerMixer()
        {            
           
            Array.Copy(CMatrixData.matrixData.m_DuckerSourch, duckerCtl.m_DuckerSourch, CDefine.Max_DuckerInputMixer);
            Array.Copy(CMatrixData.matrixData.m_DuckerBgm, duckerCtl.m_DuckerBgmLocal, CFinal.IO_MaxMatrixBus);
            duckerCtl.refreshControl();            
        }

        private void duckerFlat_Click(object sender, RoutedEventArgs e)
        {

            CMatrixData.matrixData.flatDuckerParameter();
            _parentWin.onLineCheckCounter = 0;
            CMatrixData.matrixData.sendCMD_DuckerParameter();
            updateDuckerParameters_fromData();
        }

        private void duckerBypas_Click(object sender, RoutedEventArgs e)
        {
            byte tmp = 0;
            var btn = (CSwitcher)sender as CSwitcher;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            CMatrixData.matrixData.m_duckerParameters[5] = tmp;
            _parentWin.onLineCheckCounter = 0;
            CMatrixData.matrixData.sendCMD_DuckerParameter();
            updateDuckerParameters_fromData();

        }

        public string strDuckerParameter(int valueindex, int itemindex)
        {
            string strResult = "";
            switch (itemindex)
            {
                case 0: //duck threshold
                    strResult = string.Format("{0}dB", valueindex - 80); //[0..80]
                    break;
                case 1: //duck depth
                    strResult = string.Format("{0}dB", valueindex - 60); //[0..60]
                    break;
                case 2: //duck attack                   
                    strResult = CFinal.strDuck_Attack[valueindex];
                    break;
                case 3: //duck hold
                    strResult = CFinal.strDuck_Hold[valueindex];
                    break;
                case 4: //duck release
                    strResult = CFinal.strDuck_Release[valueindex];
                    break;

            }
            return strResult;


        }
        public void updateDuckerParameters_fromData()
        {

            TextBox edbox = null;
            CSlider dslider = null;
            byte tmpV = 0;
            //  int i = 0; threshold,attack,release,depth,hold,powerOn

            byte tmpBypas = (byte)CMatrixData.matrixData.m_duckerParameters[5];
            duckerBypas.IsSelected = (tmpBypas > 0); //20170722
            for (int i = 0; i < CFinal.Max_DuckerParms - 1; i++)
            {

                string strSlider = string.Format("dslider_{0}", i);
                dslider = (CSlider)this.FindName(strSlider);
                if (dslider != null)
                    dslider.IsEnabled = (tmpBypas == 0);

                tmpV = (byte)CMatrixData.matrixData.m_duckerParameters[i];
                if (dslider != null)
                    dslider.Posvalue = tmpV;
                //  Debug.WriteLine("pos value is : {0}", tmpV);
                if (dslider != null)
                {
                    string strEd = string.Format("edDucker_{0}", i);
                    if (strEd != null)
                    {
                        edbox = (TextBox)this.FindName(strEd);
                        if (edbox != null)
                            edbox.Text = strDuckerParameter(tmpV, i);
                    }

                }

            }




        }


        /// <summary>
        /// Ducker parameter slider changed below
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newPos"></param>
        /// <param name="e"></param>
        private void onDuckerSliderValueChanged(object sender, int newPos, EventArgs e)
        {
            var slider = (CSlider)sender as CSlider;
            int itemindex = slider.iTag;
            //ducker:threshold,depth,attack,hold,release,powerOn
            CMatrixData.matrixData.m_duckerParameters[itemindex] = (byte)newPos;
            //  Debug.WriteLine("itemindex is :  {0}",itemindex);
            if (NetCilent.netCilent.isConnected())
            {
                _parentWin.onLineCheckCounter = 0;
                CMatrixData.matrixData.sendCMD_DuckerParameter();
            }
            else
               updateDuckerParameters_fromData();


        }
    }
}
