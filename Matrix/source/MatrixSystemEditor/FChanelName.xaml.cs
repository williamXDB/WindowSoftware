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
using System.Diagnostics;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for FChanelName.xaml
    /// </summary>
    public partial class FChanelName : Window
    {
        public FChanelName()
        {
            InitializeComponent();
        }

        public const double MinInput = -80;
        public const double MaxInput = 15;

        public double checkAvalibleInput(double tmp)
        {
            double tmpmv = 0;
            if (tmp < MinInput)
                tmpmv = MinInput;
            else if (tmp > MaxInput)
                tmpmv = MaxInput;
            else
                tmpmv = tmp;
            tmpmv = input2Value(tmpmv);

            return tmpmv;
        }

        public double inputValue = 0;
        public int position = 0;

        //屏蔽粘贴非法字符  
        private void edBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxData = sender as TextBox;
            #region key down check----
            // 方法二：通过循环所有字符串；允许输入负号把空格屏蔽
            if (string.IsNullOrEmpty(txtOld))
            {
                txtOld = textBoxData.Text;
            }
            else if (!string.IsNullOrEmpty(txtOld))
            {
                // 判断句号出现的次数
                if ((textBoxData.Text.Split(Char.Parse("."))).Length > 2)
                {
                    textBoxData.Text = txtOld;
                }
                else
                {
                    string strTxt = "";
                    foreach (char c in textBoxData.Text)
                    {
                        int num = 0;
                        if (c.ToString() == "-" && !strTxt.Contains("-"))
                        {
                            strTxt = "-" + strTxt;
                        }
                        if (c.ToString() == "." && !strTxt.Contains("."))
                        {
                            strTxt = strTxt + c.ToString();
                        }
                        if (Int32.TryParse(c.ToString(), out num))
                        {
                            strTxt = strTxt + c.ToString();
                        }
                        txtOld = strTxt;
                    }
                    textBoxData.Text = strTxt;
                }

            }
            #endregion

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateInputBox();
            edBox.Focus();            
        }
        double input2Value(double mv)
        {

            int t = 0;
            bool isPog = (mv < 0);

            double x = Math.Abs(Math.Round(mv, 1));
            int a = 0;
            double y = 0;

            a = (int)(x * 10 - (int)x * 10);
            if (a >= 0 && a < 5)
            {
                t = 0;
                a = 0;
            }
            else if (a > 5 && a <= 9)
            {
                t = 1;
                a = 0;
            }
            else  //==5
            {
                t = 0;
                a = 5;
            }
            y = (double)((int)x * 10 + t * 10 + a) / 10;
            if (isPog)
                y = y * -1;

            return y;


        }
        public string txtOld = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBoxData = sender as TextBox;
            #region only accept number,float and so on.
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract) // 小键盘
            {
                if (textBoxData.Text.Contains("-") && e.Key == Key.Subtract)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if ((((e.Key >= Key.D0 && e.Key <= Key.D9)
                     || e.Key == Key.OemMinus)
                     ) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift
                     )  // 大键盘
            {
                if (textBoxData.Text.Contains("-") && e.Key == Key.OemMinus)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            #endregion


            #region disalbe the invalid input except double,float number
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal) // 小键盘
            {
                if (textBoxData.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9
                      || e.Key == Key.OemMinus
                      || e.Key == Key.OemPeriod)
                    ) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift) // 大键盘
            {
                if (textBoxData.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            #endregion

            if (e.Key == Key.Enter)
            {
                Debug.WriteLine("you enter the 13 code..........................");
                double dfout = 0;
                string strTemp = textBoxData.Text.Trim();
                if (Double.TryParse(strTemp, out dfout))
                {
                    inputValue = checkAvalibleInput(dfout);
                    position = (int)((inputValue + 80) * 2);
                    textBoxData.Text = inputValue.ToString();
                    this.DialogResult = true;

                }


            }


        }



        public void updateInputBox()
        {
            position = (int)((inputValue + 80) * 2);
            Debug.WriteLine("update input box with inputvalue is  {0}", inputValue);
            edBox.Text = inputValue.ToString();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("you enter the 13 code..........................");
            double dfout = 0;
            string strTemp = edBox.Text.Trim();
            if (Double.TryParse(strTemp, out dfout))
            {
                inputValue = checkAvalibleInput(dfout);
                position = (int)((inputValue + 80) * 2);
                edBox.Text = inputValue.ToString();
                this.DialogResult = true;

            }
        }


    }
}
