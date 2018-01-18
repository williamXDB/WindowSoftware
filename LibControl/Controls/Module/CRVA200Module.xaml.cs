using CommLibrary;
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

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for CRVC200.xaml
    /// </summary>
    public partial class CRVC200Module : ComUserMD
    {
        public CRVC200Module()
        {
            InitializeComponent();
            devinfo = new CDeviceInfo();
            devinfo.devModuleType = Module_Type.Mod_RVA100;
            devinfo.devProv.pMachineID = AppIDList.AP_RVA_100;
            devinfo.strDevName = string.Empty;
        }

     
        public override void setBotomCap(string strCap)
        {
            base.setBotomCap(strCap);
            botomLb.Text = strCap.Trim();
        }

        public override void setHeadColor(SolidColorBrush scb)
        {
            base.setHeadColor(scb);
            border.Background = scb;
        }

        public override void setBodyColor(SolidColorBrush scb)
        {
            base.setBodyColor(scb);
            grid.Background = scb;
        }
        public override void setBodyImage(ImageSource imgsrc)
        {
            base.setBodyImage(imgsrc);
            image.Source = imgsrc;
        }
        /// <summary>
        /// module RVA set
        /// </summary>
        /// <param name="modRva"></param>
        public override void setRVAModuleType(ModuleRVAS modRva)
        {
            base.setRVAModuleType(modRva);
            switch(modRva)
            {
                case ModuleRVAS.MRVA:
                    devinfo.devModuleType = Module_Type.Mod_RVA100;
                    devinfo.devProv.pMachineID = AppIDList.AP_RVA_100;                    
                    break;
                case ModuleRVAS.MRVC:
                    devinfo.devModuleType = Module_Type.Mod_RVC100;// RVC1000
                    devinfo.devProv.pMachineID = AppIDList.AP_RVC_100;//
                    break;
                case ModuleRVAS.MRIO:
                    devinfo.devModuleType = Module_Type.Mod_RIO100;
                    devinfo.devProv.pMachineID = AppIDList.AP_RIO_100;
                    break;
                case ModuleRVAS.RPM:
                    devinfo.devModuleType = Module_Type.Mod_RPM100;
                    devinfo.devProv.pMachineID = AppIDList.AP_RPM_100;
                    break;

            }


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
        //----------
        public override void loadDevInfo(CDeviceInfo devf)
        {
            base.loadDevInfo(devf);            
            edDevID.Text = devf.devProv.pDeviceID.ToString("X4");
            if (devf.devProv.strDevName.Length > 16)
            {           
               
                rheadLb.Text = devf.devProv.strDevName.Substring(0, 16);
            }
            else
                rheadLb.Text = devf.devProv.strDevName;


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
                rheadLb.Text = strName.Substring(0, 16);
            else
                rheadLb.Text = strName;

        }
        /// <summary>
        /// change device ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeDevID_Click(object sender, RoutedEventArgs e)
        {
            var inputPDlg = new InputHexDialog();
            inputPDlg.Background = border.Background as SolidColorBrush;
             var sres = inputPDlg.ShowDialog();
            if (sres == true)
            {
                edDevID.Text = inputPDlg.devID.ToString("X4");
                devinfo.devProv.pDeviceID = inputPDlg.devID;

            }
        }


        public override void setHeaderStatus(bool status)
        {
            base.setHeaderStatus(status);
            rheadLb.Background = new SolidColorBrush((status ? activeConHeadColor : unConHeadColor));

        }

    }
}
