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
    /// Interaction logic for CRightTextModule.xaml
    /// </summary>
    public partial class CRightTextModule : ComUserMD
    {
        public CRightTextModule()
        {
            InitializeComponent();
            
            devinfo = new CDeviceInfo();
            devinfo.devModuleType = Module_Type.Mod_TxtRht;//text right
           
        }
        public override void loadDevInfo(CDeviceInfo devf)
        {
            base.loadDevInfo(devf);
            edLbR.Text = devf.noteTxt;
        }
        public override void saveDeInfo()
        {
            base.saveDeInfo();

        }

        private void edLbR_TextChanged(object sender, TextChangedEventArgs e)
        {
            devinfo.noteTxt = edLbR.Text.Trim();
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
