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
    /// Interaction logic for ZoneItem.xaml
    /// </summary>
    public partial class ZoneItem : UserControl
    {

        public byte[] m_ZoneName;
        public ZoneItem()
        {
            InitializeComponent();
            m_ZoneName = new byte[20];
        }
        private const string DTitle = "01";

        public string ZoneTile
        {
            get { return (string)GetValue(ZoneTileProperty); }
            set
            {
                SetValue(ZoneTileProperty, value);
            }


        }

        // Using a DependencyProperty as the backing store for ZoneTile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneTileProperty =
            DependencyProperty.Register("ZoneTile", typeof(string), typeof(ZoneItem),
              new FrameworkPropertyMetadata(DTitle, FrameworkPropertyMetadataOptions.None, onZoneTitleChange
            ));

        private static void onZoneTitleChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as ZoneItem;
            if (mcontrol != null)
            {
                mcontrol.ZoneTile = (string)args.NewValue;
                mcontrol.zlbTitle.Text = (string)args.NewValue;
            }
        }


        private const string DZoneName = "Zone";
        public string ZoneName
        {
            get { return (string)GetValue(ZoneNameProperty); }
            set { SetValue(ZoneNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneNameProperty =
            DependencyProperty.Register("ZoneName", typeof(string), typeof(ZoneItem),
                  new FrameworkPropertyMetadata(DZoneName, FrameworkPropertyMetadataOptions.None, onZoneNameChange
            ));

        private static void onZoneNameChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as ZoneItem;
            if (mcontrol != null)
            {
                mcontrol.ZoneName = (string)args.NewValue;
                mcontrol.edZone.Text = (string)args.NewValue;
            }
        }



        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(ZoneItem),

                  new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, onZoneTagChange
            ));


        private static void onZoneTagChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as ZoneItem;
            if (mcontrol != null)
            {
                mcontrol.iTag = (int)args.NewValue;

            }
        }


        private static SolidColorBrush DBrush = Brushes.Yellow;

        public SolidColorBrush activeBrush
        {
            get { return (SolidColorBrush)GetValue(activeBrushProperty); }
            set { SetValue(activeBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeBrushProperty =
            DependencyProperty.Register("activeBrush", typeof(SolidColorBrush), typeof(ZoneItem),
                new FrameworkPropertyMetadata(DBrush, FrameworkPropertyMetadataOptions.None, onActiveBrushChange
            ));


        private static void onActiveBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as ZoneItem;
            if (mcontrol != null)
            {
                mcontrol.activeBrush = (SolidColorBrush)args.NewValue;
                mcontrol.refreshBrush();
            }
        }

        public void refreshBrush()
        {
            this.BorderBrush = (isZoneSelected ? activeBrush : Brushes.Transparent);

        }


        private const bool DefSelect = false;
        public bool isZoneSelected
        {
            get { return (bool)GetValue(isZoneSelectedProperty); }
            set { SetValue(isZoneSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isZoneSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isZoneSelectedProperty =
            DependencyProperty.Register("isZoneSelected", typeof(bool), typeof(ZoneItem),
                 new FrameworkPropertyMetadata(DefSelect, FrameworkPropertyMetadataOptions.None, onZoneSelectChange
            ));

        private static void onZoneSelectChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as ZoneItem;
            if (mcontrol != null)
            {
                mcontrol.isZoneSelected = (bool)args.NewValue;
                mcontrol.refreshBrush();

            }
        }
        public delegate void OnZoneClickEvent(Object sender, int index, MouseButtonEventArgs e);
        public event OnZoneClickEvent m_ZonClickEvent;


        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_ZonClickEvent != null)
            {
                m_ZonClickEvent(this, iTag, e);
            }
            //  Debug.WriteLine("zoneItem grid inner click event happen now...");
        }

        private void edZone_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_ZonClickEvent != null)
            {
                m_ZonClickEvent(this, iTag, e);
            }
        }

        private void edZone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            string strInput = box.Text.Trim();
            byte[] tmp = UtilCover.stringToBytes(strInput, CDefine.Len_FactPName);
            if (m_ZoneName == null) m_ZoneName = new byte[Max_len];
            else
                Array.Clear(m_ZoneName, 0, Max_len);
            Array.Copy(tmp, m_ZoneName, tmp.Length);

        }
        public void refreshZoneEdit()
        {
            edZone.Text = UtilCover.bytesToString(m_ZoneName, CDefine.Len_FactPName);

        }
        public const int Max_len = 20;




    }





















}
