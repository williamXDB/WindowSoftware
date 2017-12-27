using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Diagnostics;

namespace StringButton
{
    public class OemAttachProperty
    {
        public static string GetStringContent(DependencyObject obj)
        {
            return (string)obj.GetValue(StringContentProperty);
        }

        public static void SetStringContent(DependencyObject obj, string value)
        {
            obj.SetValue(StringContentProperty, value);
        }

        public const int maxTxtLength = 8;

        // Using a DependencyProperty as the backing store for StringContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringContentProperty =
            DependencyProperty.RegisterAttached("StringContent", typeof(string), typeof(OemAttachProperty), new PropertyMetadata(default(string),OnStringContentChanged));

        private static void OnStringContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ContentControl)
            {
                ContentControl contentControl = d as ContentControl;
                if(contentControl.Content is string)
                {
                    string str = contentControl.Content as string;
                    if (str.Length > maxTxtLength)
                    { 
                        if(str.EndsWith("..."))
                        {
                            if (str.Length > maxTxtLength+3)
                            {
                                str = str.Remove(maxTxtLength);
                                str += "...";
                                contentControl.Content = str;
                            }
                        }
                        else
                        {
                            str = str.Remove(maxTxtLength);
                            str += "...";
                            Debug.WriteLine(str.Length);
                            contentControl.Content = str;

                        }
                    }
                }
            }
        }
    }
}
