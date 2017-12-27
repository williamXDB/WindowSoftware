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
using System.Windows.Shapes;
using CommLibrary;
using  MatrixSystemEditor.Matrix;

namespace MatrixSystemEditor
{


    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
           
        }       

        /// <summary>
        /// 
        /// </summary>
        public void refreshCorpInfo()
        {
            lbProduct.Text = CMatrixData.matrixData.corpInfo.ProductName;            
            lbVersion.Text = CMatrixData.matrixData.corpInfo.ProductVer;
            lbCompany.Text = CMatrixData.matrixData.corpInfo.corpENName;
            lbURL.Text = CMatrixData.matrixData.corpInfo.corpWebSite;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshCorpInfo();
        }



    }
}
