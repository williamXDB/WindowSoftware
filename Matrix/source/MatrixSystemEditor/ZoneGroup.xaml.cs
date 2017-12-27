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
using CommLibrary;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for ZoneGroup.xaml
    /// </summary>
    public partial class ZoneGroup : UserControl
    {

        public const int Max_Zone = 60;
        public const int Len_Name = 20;

        public byte[][] m_ZoneName;
        public ZoneGroup()
        {
            InitializeComponent();
            assignAllZoneItem();
            //    Debug.WriteLine("read zone list initial now");
            m_ZoneName = new byte[Max_Zone][];
            for (int i = 0; i < Max_Zone; i++)
                m_ZoneName[i] = new byte[Len_Name];
        }

        private int selIndex = 0;
        public void setSelIndex(int index)
        {
            int mindex = index;
            if (index <= 0)
                mindex = 0;
            else if (index > 59)
                mindex = 59;
            selIndex = mindex;
            setSelectWithindex(mindex);

        }

        public void ZoneNameCpy(byte[][] mData)
        {
            if (mData.Length == m_ZoneName.Length)
            {
                for (int i = 0; i < Max_Zone; i++)
                {
                    Array.Copy(mData[i], m_ZoneName[i], Len_Name);
                }

            }

        }

        //public void caculateZoneNamesToByte()
        //{
        //    int i = 0;
        //    RowZoneUnit sRowzUnit = null;
        //    string strSingZoneName = string.Empty;
        //    string strRowZunit = string.Empty;
        //    int zonRowMax = 6;// zoneSpin.Value;
        //    //first reset
        //    CMatrixData.matrixData.m_RPMCover.resetAllZoneName();

        //    for (i = 0; i < zonRowMax + 1; i++)
        //    {
        //        strRowZunit = string.Format("rowZone_{0}", i);
        //        sRowzUnit = (RowZoneUnit)this.FindName(strRowZunit);
        //        if (sRowzUnit != null)
        //        {
        //            for (int j = 0; j < 12; j++)
        //            {
        //                if (sRowzUnit != null)
        //                {
        //                    strSingZoneName = sRowzUnit.getZoneName(j);
        //                    CMatrixData.matrixData.m_RPMCover.setRMPZoneName(12 * i + j, strSingZoneName);
        //                    // CMatrixData.matrixData
        //                }
        //            }
        //        }
        //    }

        //}


        public delegate void OnZoneGrupSelectEvent(int index, MouseButtonEventArgs e);
        public event OnZoneGrupSelectEvent m_ZoneGroupClickEvent;

        public void assignAllZoneItem()
        {

            foreach (UIElement element in ugrid.Children)
            {
                if (element is ZoneItem)
                {
                    //  Debug.WriteLine("zoneItem foreach now.....");
                    (element as ZoneItem).m_ZonClickEvent += new ZoneItem.OnZoneClickEvent(ZoneGroup_m_ZonClickEvent);
                }
            }
        }





        private void ZoneGroup_m_ZonClickEvent(object sender, int index, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("zone group click with index is  " + index);
            setSelectWithindex(index);
            if (m_ZoneGroupClickEvent != null)
            {
                m_ZoneGroupClickEvent(index, e);
            }

        }

        public void copyAllZoneName_fromDataAndUpdate(byte[][] mdata)
        {

            if (mdata == null) return;
            string strZonItem = "";
            ZoneItem mPzon = null;

            for (int i = 0; i < CDefine.MaxZoneNum; i++)
            {
                Array.Copy(mdata[i], m_ZoneName[i], CDefine.Len_PresetName);

                strZonItem = string.Format("zonitem_{0}", i);
                mPzon = (ZoneItem)this.FindName(strZonItem);
                if (mPzon != null)
                {
                    Array.Copy(mdata[i], mPzon.m_ZoneName, CDefine.Len_PresetName);
                    mPzon.refreshZoneEdit();
                  //  Debug.WriteLine("refresh all zone name is " + getZoneName(i) + "  zindex is  " + i);
                }

            }



        }

        public void calcAllZoneNameB_ytes_fromAllItems_toGroups()
        {

            ZoneItem mitem = null;
            foreach (UIElement element in ugrid.Children)
            {
                if (element is ZoneItem)
                {
                    mitem = (element as ZoneItem);
                    int zindex = mitem.iTag;
                    Array.Copy(mitem.m_ZoneName, m_ZoneName[zindex], Len_Name);
                }
            }
        }

        public void setSelectWithindex(int index)
        {
            foreach (UIElement element in ugrid.Children)
            {
                if (element is ZoneItem)
                {
                    if ((element as ZoneItem).iTag == index)
                    {
                        (element as ZoneItem).isZoneSelected = true;
                        selZoneIndex = selIndex = index;

                    }
                    else
                    {
                        (element as ZoneItem).isZoneSelected = false;
                    }
                }
            }
        }

        public void setZoneNameWithIndex(int index, string mName)
        {
            foreach (UIElement element in ugrid.Children)
            {
                if (element is ZoneItem)
                {
                    if ((element as ZoneItem).iTag == index)
                    {
                        (element as ZoneItem).ZoneName = mName;
                        break;
                    }
                }

            }

        }



        public String getZoneName(int index)
        {

            return UtilCover.bytesToString(m_ZoneName[index], CDefine.Len_FactPName);

        }
        public void refreshAllZoneName()
        {
            int i = 0;
            string strZonItem = "";
            ZoneItem mPzon = null;

            for (i = 0; i < Max_Zone; i++)
            {
                strZonItem = string.Format("zonitem_{0}", i);
                mPzon = (ZoneItem)this.FindName(strZonItem);
                if (mPzon != null)
                {
                    mPzon.refreshZoneEdit();
                   // Debug.WriteLine("refresh all zone name is " + getZoneName(i) + "  zindex is  " + i);

                }

            }
        }


        //singleDev_0
        public void setRowZoneShare(int zonNum) //zonindex 0..60*32
        {
            if (zonNum < 0 || zonNum > Max_Zone) return;
            ZoneItem sgItem = null;
            string strZoneItem = string.Empty;
            for (int i = 0; i < Max_Zone; i++)
            {
                strZoneItem = string.Format("zonitem_{0}", i);
                sgItem = (ZoneItem)this.FindName(strZoneItem);
                if (sgItem != null)
                {
                    if (i >= zonNum)
                    {
                        sgItem.IsEnabled = false;
                        sgItem.Background = Brushes.Red;
                    }
                    else
                    {
                        sgItem.IsEnabled = true;
                        sgItem.Background = Brushes.Transparent;
                    }
                }
            }
            if (zonNum <= selIndex && zonNum >= 1)
            {
                selIndex = selZoneIndex = zonNum - 1;
                setSelectWithindex(selZoneIndex);
            }


        }

        public int selZoneIndex
        {
            get { return (int)GetValue(selZoneIndexProperty); }
            set { SetValue(selZoneIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for selZoneIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selZoneIndexProperty =
            DependencyProperty.Register("selZoneIndex", typeof(int), typeof(ZoneGroup), new PropertyMetadata(0));




    }
}
