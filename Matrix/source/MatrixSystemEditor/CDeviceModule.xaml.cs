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
using CommLibrary;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CDeviceModule.xaml
    /// </summary>
    public partial class CDeviceModule : UserControl
    {
        public CDeviceModule()
        {
            InitializeComponent();
            modDevProv = new DeviceProvision();
            rootPanel.Click += rootPanel_Click;
        }

        private void rootPanel_Click(object sender, RoutedEventArgs e)
        {
            if (onclickEvent != null)
            {
                onclickEvent(this, e);
            }

        }
        public Device_AccesType RouterStyle { get; set; }
        /// <summary>
        /// change the background brush
        /// </summary>
        /// 
        private static SolidColorBrush DefaultBrush = Brushes.Beige;
        public SolidColorBrush BackBrush
        {
            get { return (SolidColorBrush)GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(SolidColorBrush), typeof(CDeviceModule),
              new FrameworkPropertyMetadata(DefaultBrush,
                FrameworkPropertyMetadataOptions.None, OnChangedBrushChanged));

        private static void OnChangedBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (Brush)args.NewValue;
                var oldValue = (Brush)args.OldValue;
                devControl.rootPanel.Background = newValue;
                devControl.InvalidateVisual();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        private static SolidColorBrush DefaultActiveBorderBrush = Brushes.Red;
        private static SolidColorBrush DefaultUnactiveBorderBrush = Brushes.Gray;

        public SolidColorBrush activeBroderBrush
        {
            get { return (SolidColorBrush)GetValue(activeBroderBrushProperty); }
            set { SetValue(activeBroderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeBroderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeBroderBrushProperty =
            DependencyProperty.Register("activeBroderBrush", typeof(SolidColorBrush), typeof(CDeviceModule),
        new FrameworkPropertyMetadata(DefaultActiveBorderBrush,
                FrameworkPropertyMetadataOptions.None, OnActiveBrushChanged));

        private static void OnActiveBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (Brush)args.NewValue;
                var oldValue = (Brush)args.OldValue;
                devControl.rootPanel.activeBrush = newValue;
                devControl.InvalidateVisual();
            }

        }

        /// <summary>
        /// unactive brush changed
        /// </summary>
        public SolidColorBrush unActiveBorderBrush
        {
            get { return (SolidColorBrush)GetValue(unActiveBorderBrushProperty); }
            set { SetValue(unActiveBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for unActiveBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty unActiveBorderBrushProperty =
            DependencyProperty.Register("unActiveBorderBrush", typeof(SolidColorBrush), typeof(CDeviceModule),
         new FrameworkPropertyMetadata(DefaultUnactiveBorderBrush,
                FrameworkPropertyMetadataOptions.None, OnUnactiveBrushChanged));

        private static void OnUnactiveBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (Brush)args.NewValue;
                var oldValue = (Brush)args.OldValue;
                devControl.rootPanel.unActiveBrush = newValue;
                devControl.InvalidateVisual();

            }
        }


        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(CDeviceModule), new PropertyMetadata(0));



        private const bool DefaultStatus = false;
        public bool isSelected
        {
            get { return (bool)GetValue(isSelectedProperty); }
            set { SetValue(isSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isSelectedProperty =
            DependencyProperty.Register("isSelected", typeof(bool), typeof(CDeviceModule),
             new FrameworkPropertyMetadata(DefaultStatus,
                FrameworkPropertyMetadataOptions.None, OnSelectedChanged));

        private static void OnSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (bool)args.NewValue;
                var oldValue = (bool)args.OldValue;
                devControl.rootPanel.IsSelected = newValue;
                devControl.InvalidateVisual();
            }
        }

        public delegate void onClick(object sender, RoutedEventArgs e);
        public event onClick onclickEvent;


        private const string DefaultIP = "127.0.0.1";
        public string DevAddr
        {
            get { return (string)GetValue(DevAddrProperty); }
            set { SetValue(DevAddrProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevAddr.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevAddrProperty =
            DependencyProperty.Register("DevAddr", typeof(string), typeof(CDeviceModule),
              new FrameworkPropertyMetadata(DefaultIP,
                FrameworkPropertyMetadataOptions.None, OnDevIPChanged));

        private static void OnDevIPChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (string)args.NewValue;
                var oldValue = (string)args.OldValue;
                devControl.edAddr.Text = newValue;
            }
        }

        public DeviceProvision modDevProv { get; set; }

        public void devProvCopy(DeviceProvision mdev)
        {
            if (modDevProv == null)
                modDevProv = new DeviceProvision();
            modDevProv.devProvisionCopy(mdev);
        }

        private const string DefaultName = "CS-EQ";
        public string DeviceName
        {
            get { return (string)GetValue(DeviceNameProperty); }
            set { SetValue(DeviceNameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DeviceName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeviceNameProperty =
            DependencyProperty.Register("DeviceName", typeof(string), typeof(CDeviceModule),
            new FrameworkPropertyMetadata(DefaultName,
                FrameworkPropertyMetadataOptions.None, OnDevnameChanged));

        private static void OnDevnameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (string)args.NewValue;
                var oldValue = (string)args.OldValue;
                devControl.edTitle.Text = newValue;

            }
        }

        private const string DefaultMAC = "00-00-00-00-00-00-00";
        public string DevMac
        {
            get { return (string)GetValue(DevMacProperty); }
            set { SetValue(DevMacProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMac.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMacProperty =
            DependencyProperty.Register("DevMac", typeof(string), typeof(CDeviceModule),
         new FrameworkPropertyMetadata(DefaultMAC,
                FrameworkPropertyMetadataOptions.None, OnMacChanged));

        private static void OnMacChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var devControl = obj as CDeviceModule;
            if (devControl != null)
            {
                var newValue = (string)args.NewValue;
                var oldValue = (string)args.OldValue;
                devControl.edMac.Text = newValue;

            }
        }


        //-----------------------------------------------------------------------------------------------



    }
}
