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
using CommLibrary;

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for CMVIIIModule.xaml
    /// </summary>
    public partial class CMVIIIModule : ComUserMD
    {
        public CMVIIIModule()
        {
            InitializeComponent();
            devinfo = new CDeviceInfo();
            devinfo.devModuleType = Module_Type.Mod_MAVIII;//matrix A8
            devinfo.devProv.pMachineID= AppIDList.AP_Matrix_A8;
        
           

        }
        public override void loadDevInfo(CDeviceInfo devf)
        {

            base.loadDevInfo(devf);            
            edDevID.Text = devf.devProv.pDeviceID.ToString("X4");
            if (devf.devProv.strDevName.Length > 16)
            {
                //string strTmp=devf.devProv.strDevName;
                headLb.Text = devf.devProv.strDevName.Substring(0, 16);
            }
                
            else
                headLb.Text = devf.devProv.strDevName;
        }
        public override void saveDeInfo()
        {
            base.saveDeInfo();   
            

        }

        public override void setDeviceID(UInt16 mid)
        {
            base.setDeviceID(mid);
            devinfo.devProv.pDeviceID = mid;
            edDevID.Text = mid.ToString("X4");
        }

        public override void setDeviceName(string strName)
        {           
            base.setDeviceName(strName);
            devinfo.devProv.strDevName = strName;
            if (strName.Length > 16)
                headLb.Text = strName.Substring(0, 16);
            else
                headLb.Text = strName;
            
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var inputPDlg = new InputHexDialog();
            inputPDlg.Background = border.Background as SolidColorBrush;
            var sres=inputPDlg.ShowDialog();
            if (sres == true)
            {               
                edDevID.Text= inputPDlg.devID.ToString("X4");
                devinfo.devProv.pDeviceID= inputPDlg.devID;
                
            }


        }

        public override void setHeaderStatus(bool status)
        {
            base.setHeaderStatus(status);
            headLb.Background = new SolidColorBrush((status ? activeConHeadColor : unConHeadColor));

        }

        //delete module
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            deleteModule(sender); 
            
        }
    }
}
