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
    /// Interaction logic for CRouterLanModule.xaml
    /// </summary>
    public partial class CRouterLanModule : ComUserMD
    {
        public CRouterLanModule()
        {
            InitializeComponent();
            devinfo = new CDeviceInfo();
            devinfo.devModuleType = Module_Type.Mod_LAN;//lan router 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devf"></param>
        public override void loadDevInfo(CDeviceInfo devf)
        {
            base.loadDevInfo(devf);

        }
        public override void saveDeInfo()
        {
            base.saveDeInfo();

        }
        public override void setRouterAddr(string strAddr)
        {
            base.setRouterAddr(strAddr);
            ipLabel.Text = strAddr;
        }


        public override void setHeaderStatus(bool status)
        {
            base.setHeaderStatus(status);
            rheadLb.Background = new SolidColorBrush((status ? activeConHeadColor : unConHeadColor));

        }

        public override void setBodyColor(SolidColorBrush scb)
        {
            base.setBodyColor(scb);
            bgrid.Background = scb;
        }

        public override void setHeadColor(SolidColorBrush scb)
        {
            base.setHeadColor(scb);
            unConHeadColor = scb.Color;
            rheadLb.Background = scb;
            bottomLabel.Background = scb;
        }


        public override void setDeviceID(UInt16 mid)
        {
            base.setDeviceID(mid);
        }

        public override void setDeviceName(string strName)
        {
            base.setDeviceName(strName);
        }
        /// <summary>
        /// delete module
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            deleteModule(sender);
        }

    }
}
