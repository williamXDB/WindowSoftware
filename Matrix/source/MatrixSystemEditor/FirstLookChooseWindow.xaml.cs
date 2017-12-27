using MatrixSystemEditor.Matrix;
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

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for FirstLookChooseWindow.xaml
    /// </summary>
    public partial class FirstLookChooseWindow : Window
    {
        public FirstLookChooseWindow()
        {
            InitializeComponent();
            CMatrixData.shareData();//first initial data  
            this.Title = CMatrixData.matrixData.corpInfo.getProcesTitle();            
        }

        private void btnDevSeries_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mwd = new MainWindow();
            mwd.isSerialMode = true;
            this.Close();
            mwd.ShowDialog();
            
        }

        private void btnDevParalle_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mwd = new MainWindow();
            mwd.isSerialMode = false;
            MatrixPage matrixPg = new MatrixPage();
            matrixPg.prepareForParalConnect();
            this.Close();
            matrixPg.ShowDialog();
        }

        private void Enter_click(object sender, RoutedEventArgs e)
        {
            MainWindow mwd = new MainWindow();
            if(radioI.IsChecked.HasValue && radioI.IsChecked.Value)//serail
            {                
                mwd.isSerialMode = true;
                this.Close();
                mwd.ShowDialog();
            }
            else
            {               
                mwd.isSerialMode = false;
                MatrixPage matrixPg = new MatrixPage();
                matrixPg.prepareForParalConnect();
                this.Close();
                matrixPg.ShowDialog();
            }

        }
    }
}
