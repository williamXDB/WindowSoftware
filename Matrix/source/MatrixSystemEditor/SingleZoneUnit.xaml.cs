using CommLibrary;
using Lib.Controls;
using MatrixSystemEditor.Matrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SingleZoneUnit.xaml
    /// </summary>
    public partial class SingleZoneUnit : UserControl
    {
        public SingleZoneUnit()
        {
            InitializeComponent();
            if (m_singleZone == null)
              m_singleZone = new byte[Max_Port];
          
            initialAllPorts();

        }

        public const int Max_Port = 12; //each device has 12 device port to save a byte
        public byte[] m_singleZone = new byte[Max_Port];       
       

        private const int DefaultTag = 0;

        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(SingleZoneUnit),

                  new FrameworkPropertyMetadata(DefaultTag, FrameworkPropertyMetadataOptions.None, onTagChange
            ));

        private static void onTagChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as SingleZoneUnit;
            if (mcontrol != null)
            {
                int t = (int)args.NewValue;
                /*
               // Debug.WriteLine("on tag change with tag is  {0}", t);
                if (t >= 0 && t < 9)
                    mcontrol.zoneLb.Text = string.Format("00{0} ", t + 1);
                else if (t >= 9 && t < 99)
                    mcontrol.zoneLb.Text = string.Format("0{0} ", t + 1);
                else
                    mcontrol.zoneLb.Text = string.Format("{0} ", t + 1);
                */
            }

        }

        private void initialAllPorts()
        {
            CSwitcher sw = null;
            for (int i = 0; i < Max_Port; i++)
            {
                String btnName = string.Format("btn_port{0}", i);
                sw = (CSwitcher)FindName(btnName);
                if (sw != null)
                {
                    sw.Click += sw_Click;

                }


            }

        }

        public delegate void OnSingleZoneClickEvent(int devindex, int index, byte[] data, RoutedEventArgs e); //which device,which item is click
        public event OnSingleZoneClickEvent m_SingleZoneClickEvent;


        private void sw_Click(object sender, RoutedEventArgs e)
        {
            CSwitcher sb = sender as CSwitcher;
            int index = sb.iTag;
            byte tmp = 0;
            if (sb.IsSelected)
                tmp = 0;
            else
                tmp = 1;
            m_singleZone[index] = tmp;
            sb.IsSelected = (tmp > 0);

            if (m_SingleZoneClickEvent != null)
            {

                IPProces.printAryByte("\nSingle Zone bytes: ", m_singleZone);
                m_SingleZoneClickEvent(iTag, index, m_singleZone, e);
            }

        }

        public void refreshAllPorts()
        {
            CSwitcher sw = null;

            for (int i = 0; i < Max_Port; i++)
            {
                String btnName = string.Format("btn_port{0}", i);
                sw = (CSwitcher)FindName(btnName);
                if (sw != null)
                    sw.IsSelected = (m_singleZone[i] > 0);
              //  Debug.WriteLine("singzone status is " + m_singleZone[i] + " " + i);

            }

        }

        /// <summary>
        /// zone Name
        /// </summary>
        public string zoneName
        {
            get
            {

                return getZoneName();
            }
            set
            {
                //zoneBtn.Text = value;
                setZoneName(value);

            }
        }

        public string getZoneName()
        {
            return UtilCover.bytesToString(m_singleZone, CDefine.Len_FactPName); //fact 16 len 

        }
        private int maxEdLen = 16;
        public void setZoneName(string strName)
        {

            byte[] tmp = UtilCover.stringToBytes(strName, maxEdLen);

            // string hex= BitConverter.ToString(tmp)        //  Debug.WriteLine("set zone look hex con: " + hex);

            Array.Clear(m_singleZone, 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_singleZone, tmp.Length);
        }

        private void zoneBtn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var edt = sender as TextBox;
            edt.IsReadOnly = false;


        }

        private void zoneBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            // var edt = sender as TextBox;
            //edt.IsReadOnly = true;
        }

        private void zoneBtn_TextChanged(object sender, TextChangedEventArgs e)
        {
            var edt = sender as TextBox;
            if (edt.Text.Trim().Length > 0)
            {
                setZoneName(edt.Text.Trim());
            }

        }

        private void zoneBtn_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //zoneBtn.Clear();
        }



        //-------------------------------
        private const string DGroupDevID = "1000";
        public String GroupDevID
        {
            get { return (String)GetValue(GroupDevIDProperty); }
            set { SetValue(GroupDevIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupDevID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupDevIDProperty =
            DependencyProperty.Register("GroupDevID", typeof(String), typeof(SingleZoneUnit),
                      new FrameworkPropertyMetadata(DGroupDevID, FrameworkPropertyMetadataOptions.None, onGropDevidChange
            ));

        private static void onGropDevidChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as SingleZoneUnit;
            if (mcontrol != null)
            {
                mcontrol.GroupDevID = (string)args.NewValue;
                mcontrol.lbDevID.Text = (string)args.NewValue;
            }
        }


        private const string DGroupTitle = "MatrixA8";
        public string GroupTitle
        {
            get { return (string)GetValue(GroupTitleProperty); }
            set { SetValue(GroupTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupTitleProperty =
            DependencyProperty.Register("GroupTitle", typeof(string), typeof(SingleZoneUnit),
                    new FrameworkPropertyMetadata(DGroupTitle, FrameworkPropertyMetadataOptions.None, onGropTitleChange
            ));

        private static void onGropTitleChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as SingleZoneUnit;
            if (mcontrol != null)
            {
                mcontrol.GroupTitle = (string)args.NewValue;
                mcontrol.lbTitle.Text = (string)args.NewValue;

            }
        }










    }

}
