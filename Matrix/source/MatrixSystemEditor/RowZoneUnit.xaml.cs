using CommLibrary;
using Lib.Controls;
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

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for RowZoneUnit.xaml
    /// </summary>
    public partial class RowZoneUnit : UserControl
    {
        public const int Max_SingleZone = 32;
        public const int MaxSinglePort = 12;
        public const int Max_Zones = 60;


        public byte[] m_singleZoneByte; //32:2n+1 is High,2*n+0 is Low,from right to left 

        public RowZoneUnit()
        {
            InitializeComponent();
            initialSingleRowByte();
        }

        public void initialSingleRowByte()
        {
            m_singleZoneByte = new byte[Max_SingleZone];

            initalAllDev();

        }

        //singleDev_0
        public void initalAllDev() //zonindex 0..60
        {
            //void OnSingleZoneClickEvent(int devindex,int index,byte[] data, RoutedEventArgs e); //which device,which item is click
            SingleZoneUnit sgzUnit = null;
            string strSgzunit = string.Empty;
            for (int i = 0; i < Max_SingleZone / 2; i++) //16 devices
            {
                strSgzunit = string.Format("singleDev_{0}", i);
                sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);
                if (sgzUnit != null)
                {
                    sgzUnit.m_SingleZoneClickEvent += sgzUnit_m_SingleZoneClickEvent; //new SingleZoneUnit.OnSingleZoneClickEvent(sgzUnit_m_SingleZoneClickEvent);

                }

            }



        }

        public delegate void OnPageZoneClickEvent(int devindex, byte[] mdata, RoutedEventArgs e); //which device,which item is click

        public event OnPageZoneClickEvent m_OnPageZoneClickEvent;



        void sgzUnit_m_SingleZoneClickEvent(int devindex, int index, byte[] data, RoutedEventArgs e)
        {
            //------------    
            byte bH4 = CUlitity.convertByte12ToBHigh4(data);
            byte bL8 = CUlitity.convertByte12ToBLow8(data);
            //
            m_singleZoneByte[2 * devindex + 0] = bL8;
            m_singleZoneByte[2 * devindex + 1] = bH4;

            IPProces.printAryByte("\n before  ...Single Zone bytes: ", data);
            Debug.WriteLine("high 4 byte is {0}  low 8 bytes is {1}   single zone unit index is {2}", bH4, bL8,devindex);
            //-------------
            // Debug.WriteLine("This single group click with device index is ............" + devindex + "   itemindex is  " + index);
            if (m_OnPageZoneClickEvent != null)
            {
                m_OnPageZoneClickEvent(devindex, m_singleZoneByte, e);
            }

        }

        //singleDev_0
        public void refreshAllSingPorts() //zonindex 0..60*32
        {
          
            SingleZoneUnit sgzUnit = null;
            string strSgzunit = string.Empty;
            for (int i = 0; i < Max_SingleZone / 2; i++) //16 devices
            {
                strSgzunit = string.Format("singleDev_{0}", i);
                sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);

                byte H4 = m_singleZoneByte[2 * i + 1];
                byte L8 = m_singleZoneByte[2 * i + 0];              

                byte[] mData12 = CUlitity.catsByteHLToByteAry12(H4, L8);               
             //   IPProces.printAryByte("fresh with single ports array with devindex  "+i, mData12);
                if (sgzUnit != null)
                {
                    Array.Copy(mData12, sgzUnit.m_singleZone, MaxSinglePort);
                    sgzUnit.refreshAllPorts();
                }

            }



        }

        //singleDev_0
        public void setRowZoneShare(int zonNum) //zonindex 0..60*32
        {
            if (zonNum < 0 || zonNum > Max_SingleZone / 2) return;
            SingleZoneUnit sgzUnit = null;
            string strSgzunit = string.Empty;
            for (int i = 0; i < Max_SingleZone / 2; i++)
            {
                strSgzunit = string.Format("singleDev_{0}", i);
                sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);
                if (sgzUnit != null)
                {
                    if (i >= zonNum)
                        sgzUnit.IsEnabled = false;
                    else
                        sgzUnit.IsEnabled = true;
                }
            }

        }



        //------------------------------------------
        private const int DefaultRindex = 0;
        public int Rowindex
        {
            get { return (int)GetValue(RowindexProperty); }
            set { SetValue(RowindexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rowindex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowindexProperty =
            DependencyProperty.Register("Rowindex", typeof(int), typeof(RowZoneUnit),
              new FrameworkPropertyMetadata(DefaultRindex, FrameworkPropertyMetadataOptions.None, onRowIndexChange
            ));

        private static void onRowIndexChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as RowZoneUnit;
            if (mcontrol != null)
            {
                int R = (int)args.NewValue;
                mcontrol.updateAllSingleZoneUnit(R);
            }

        }


        public void setZoneName(int index, string strName)
        {
            SingleZoneUnit sgzUnit = null;
            string strSgzunit = string.Empty;

            strSgzunit = string.Format("zoneBtn_{0}", index);
            sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);
            if (sgzUnit != null)
            {
                // Debug.WriteLine("update single zone iteral now..{0}", index);
                sgzUnit.zoneName = strName;
            }
        }


        public string getZoneName(int index)
        {
            SingleZoneUnit sgzUnit = null;
            string strSgzunit = string.Empty;

            strSgzunit = string.Format("zoneBtn_{0}", index);
            sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);
            if (sgzUnit != null)
            {
                return sgzUnit.zoneName;
            }
            else return string.Empty;

        }

        public const int COLMAX = 12;
        public void updateAllSingleZoneUnit(int rindex)
        {
            SingleZoneUnit sgzUnit = null;
            //  if (rindex > 1) return;
            string strSgzunit = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                strSgzunit = string.Format("zoneBtn_{0}", i);
                sgzUnit = (SingleZoneUnit)this.FindName(strSgzunit);
                if (sgzUnit != null)
                {
                    // Debug.WriteLine("update single zone iteral now..{0}", i);
                    int tmpi = i;
                    sgzUnit.iTag = COLMAX * rindex + tmpi;
                }
            }
        }


    }
}
