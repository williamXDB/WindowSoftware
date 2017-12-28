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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for InputPasDialog.xaml
    /// </summary>
    public partial class InputHexDialog : Window
    {

        public UInt16 devID { get; set; }
        public InputHexDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btn ok is pressed now............");
            if(edInput.Text.Trim().Length>=4)
            {
                this.DialogResult = true;
                devID = UInt16.Parse(edInput.Text.Trim(), NumberStyles.HexNumber);
            }
            
            this.Close();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-fA-F0-9\b]+$");//numeric and backspace
            e.Handled = regex.IsMatch(e.Text);
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            edInput.Focus();
        }

        private void edInput_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.Key==Key.Return)
            {
                btnOK_Click(sender, e);

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
     



    }
}
