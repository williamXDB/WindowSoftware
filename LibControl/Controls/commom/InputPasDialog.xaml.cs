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

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for InputPasDialog.xaml
    /// </summary>
    public partial class InputPasDialog : Window
    {
        public InputPasDialog()
        {
            InitializeComponent();
            InputPWD = "888888";
        }

        public string InputPWD { get; set; }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btn ok is pressed now............");
            string input = edInput.Password.Trim();
            if(input.CompareTo(InputPWD)==0)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                chekRLB.Text = "Invalid Password";

            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new Regex("[^a-zA-Z0-9\b]+$");//numeric and backspace
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
