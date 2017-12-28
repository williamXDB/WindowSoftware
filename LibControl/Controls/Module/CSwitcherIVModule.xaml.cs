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


namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for CSwitcherIVModule.xaml
    /// </summary>
    public partial class CSwitcherIVModule : ComUserMD
    {
        public CSwitcherIVModule()
        {
            InitializeComponent();
            devinfo = new CDeviceInfo();
            devinfo.devModuleType = Module_Type.Mod_CLIV;//CL-4 
            cbBox.SelectionChanging += cbox_SelectionChanging;

        }

        public override void setHeaderStatus(bool status)
        {
            base.setHeaderStatus(status);

            IVheadLb.Background = new SolidColorBrush((status ? activeConHeadColor : unConHeadColor));

        }

        public override void setHeadColor(SolidColorBrush scb)
        {
            base.setHeadColor(scb);
            IVheadLb.Background = scb;
            unConHeadColor = scb.Color;
            botomLabel.Background = scb;

        }

        public override void setRouterAddr(string strAddr)
        {
            base.setRouterAddr(strAddr);
            ipLabel.Text = strAddr;
        }


        public override void setBodyColor(SolidColorBrush scb)
        {
            base.setBodyColor(scb);
            bgrid.Background = scb;
        }


        private void cbox_SelectionChanging(object sender, SelectionChangingEventArgs e) //eq type
        {
            //  e.Cancel = true;
            var cbx = sender as TComboBox;
            devinfo.lineIndex = cbx.SelectedIndex;
        }

        public override void loadDevInfo(CDeviceInfo devf)
        {
            base.loadDevInfo(devf);
            cbBox.SelectedIndex = devf.lineIndex;
        }
        public override void saveDeInfo()
        {
            base.saveDeInfo();
            //  devinfo.lineIndex = cbBox.SelectedIndex;

        }


        public override void setDeviceID(UInt16 mid)
        {
            base.setDeviceID(mid);
        }

        public override void setDeviceName(string strName)
        {
            base.setDeviceName(strName);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            deleteModule(sender);
        }
    }
}
