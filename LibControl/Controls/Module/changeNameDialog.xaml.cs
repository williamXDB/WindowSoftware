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
using CommLibrary;

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for InputPasDialog.xaml
    /// </summary>
    public partial class ChangeNameDialog : Window
    {



        public int chindex { get; set; }
       
        public ChangeNameDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (fedInput.Text.Trim().Length > 0)
            {              
                this.DialogResult = true;
                this.Close();
            }

        }
        /// <summary>
        /// fetch the input keybord name string
        /// </summary>
        /// <returns></returns>
        public string getInputName()
        {
           return fedInput.Text.Trim();
        }

        public void setChName(string mstr)
        {
            fedInput.Clear();
            fedInput.Text = mstr.Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetDefault_Click(object sender, RoutedEventArgs e)
        {
            string str = CUlitity.defaultChName(chindex).Trim();
            fedInput.Text = str;
            Debug.WriteLine("set default string is :  " + str);
            this.DialogResult = true;
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9\b]+$");//numeric and backspace
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fedInput.Focus();
            string str = string.Format("channel {0}  name will be changed.", chindex + 1);
            this.Title = str;
        }

        private void edInput_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                btnSave_Click(sender, e);

            }

        }


     
    }
}
