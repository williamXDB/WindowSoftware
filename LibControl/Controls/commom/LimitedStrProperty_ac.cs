using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace Lib.Controls
{
    public class LimitedStrProperty
    {
        public static string GetStringContent(DependencyObject obj)
        {
            return (string)obj.GetValue(StringContentProperty);
        }
        /// <summary>
        /// StringContent is the full content
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetStringContent(DependencyObject obj, string value)
        {
            obj.SetValue(StringContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for StringContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringContentProperty =
            DependencyProperty.RegisterAttached("StringContent", typeof(string), typeof(LimitedStrProperty), new PropertyMetadata(default(string), OnStringContentChanged));

        private static void OnStringContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContentControl)
            {
                int limitedLength;
                //first search in myGroupDictionary ,and after search mydictionary
                if (LimitedStrProperty.GetMyGroupName(d) != null && _MyGroupDictionary.TryGetValue(LimitedStrProperty.GetMyGroupName(d), out limitedLength))
                {
                    SetContent(d, limitedLength);
                }
                else if (_MyDictionary.TryGetValue(d, out limitedLength))
                {
                    SetContent(d, limitedLength);
                }
                else
                {
                    SetContent(d, _PrimaryLimitedLength);
                }
                ((ContentControl)d).ToolTip = e.NewValue;
            }
        }


        public static int GetMyLimitedLength(DependencyObject obj)
        {
            return (int)obj.GetValue(MyLimitedLengthProperty);
        }

        public static void SetMyLimitedLength(DependencyObject obj, int value)
        {
            obj.SetValue(MyLimitedLengthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyLimitedLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyLimitedLengthProperty =
            DependencyProperty.RegisterAttached("MyLimitedLength", typeof(int), typeof(LimitedStrProperty), new PropertyMetadata(_PrimaryLimitedLength, OnMyLimitedLengthChanged));

        private static void OnMyLimitedLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_MyDictionary.ContainsKey(d))
            {
                _MyDictionary[d] = (int)e.NewValue;
            }
            else
            {
                _MyDictionary.Add(d, (int)e.NewValue);
            }

            if (LimitedStrProperty.GetStringContent(d) != null)
            {
                int limitedLength = (int)e.NewValue;
                string str = LimitedStrProperty.GetStringContent(d);
                ContentControl contentControl = d as ContentControl;
                if (str.Length > limitedLength)
                {
                    str = str.Remove(limitedLength);
                    str += "...";
                    contentControl.Content = str;
                }
                else
                {
                    contentControl.Content = str;
                }
                contentControl.ToolTip = LimitedStrProperty.GetStringContent(d);
            }
        }



        public static string GetMyGroupName(DependencyObject obj)
        {
            return (string)obj.GetValue(MyGroupNameProperty);
        }

        public static void SetMyGroupName(DependencyObject obj, string value)
        {
            obj.SetValue(MyGroupNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyGroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyGroupNameProperty =
            DependencyProperty.RegisterAttached("MyGroupName", typeof(string), typeof(LimitedStrProperty), new PropertyMetadata(default(string), OnMyGroupNameChanged));

        private static void OnMyGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string groupName = (string)e.NewValue;
            int limitedLength;
            if (_MyGroupDictionary.TryGetValue(groupName, out limitedLength))
            {
                if (LimitedStrProperty.GetStringContent(d) != null)
                {
                    string str = LimitedStrProperty.GetStringContent(d);
                    ContentControl contentControl = d as ContentControl;
                    if (str.Length > limitedLength)
                    {
                        str = str.Remove(limitedLength);
                        str += "...";
                        contentControl.Content = str;
                    }
                    else
                    {
                        contentControl.Content = str;
                    }
                    contentControl.ToolTip = LimitedStrProperty.GetStringContent(d);
                }
            }
            List<DependencyObject> groupControl;
            if (_MyGroupControl.TryGetValue(groupName, out groupControl))
            {
                groupControl.Add(d);
            }
        }

        private static Dictionary<DependencyObject, int> _MyDictionary = new Dictionary<DependencyObject, int>();
        private static Dictionary<string, int> _MyGroupDictionary = new Dictionary<string, int>();
        private static int _PrimaryLimitedLength = 8;
        private static Dictionary<string, List<DependencyObject>> _MyGroupControl = new Dictionary<string, List<DependencyObject>>();
        public static void AddNewGroup(string name, int limitedLength)
        {
            if (_MyGroupDictionary.ContainsKey(name))
            {
                try
                {

                }
                catch(Exception ec)
                {
                    Debug.WriteLine(ec.ToString());
                }

               // throw new Exception("the name has been used");
            }
            else
            {
                _MyGroupDictionary.Add(name, limitedLength);
                _MyGroupControl.Add(name, new List<DependencyObject>());
            }
        }

        public static void RemoveGroup(string name)
        {
            if (_MyGroupDictionary.ContainsKey(name))
            {
                _MyGroupDictionary.Remove(name);
            }
        }

        public static void ChangeGroupValue(string name, int length)
        {
            List<DependencyObject> groupControl;
            if (_MyGroupControl.TryGetValue(name, out groupControl))
            {
                foreach (DependencyObject item in groupControl)
                {
                    SetContent(item, length);
                }
            }
            if (_MyGroupDictionary.ContainsKey(name))
            {
                _MyGroupDictionary[name] = length;
            }
        }

        private static void SetContent(DependencyObject d, int length)
        {
            ContentControl contentControl = d as ContentControl;
            if (contentControl != null)
            {
                string str = LimitedStrProperty.GetStringContent(d);
                SetContent(contentControl, length, str);
            }
        }

        private static void SetContent(ContentControl contentControl, int length, string content)
        {
            if (content.Length > length)
            {
                content = content.Remove(length);
                content += "...";
                contentControl.Content = content;
            }
            else
                contentControl.Content = content;
        }
    }
}
