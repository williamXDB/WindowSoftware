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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ChangeX"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ChangeX;assembly=ChangeX"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChangingComboBox/>
    ///
    /// </summary>
    /// 
    public class SelectionChangingEventArgs : RoutedEventArgs
    {
        public bool Cancel { get; set; }
    }

    public delegate void
    SelectionChangingEventHandler(Object sender, SelectionChangingEventArgs e);


    public class TComboBox : ComboBox
    {
        static TComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TComboBox), new FrameworkPropertyMetadata(typeof(TComboBox)));
        }

        public int _index{get;set;}
        public int _lastIndex { get; set; }
        public bool _suppress { get; set; }



        public bool MyProperty { get; set; }     

        

        public event SelectionChangingEventHandler SelectionChanging;
        public new event SelectionChangedEventHandler SelectionChanged;



        public int iTg
        {
            get { return (int)GetValue(iTgProperty); }
            set { SetValue(iTgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTgProperty =
            DependencyProperty.Register("iTg", typeof(int), typeof(TComboBox), new PropertyMetadata(0));

        


        public TComboBox()
        {
            _index = -1;
            _lastIndex = 0;
            _suppress = false;
            base.SelectionChanged += InternalSelectionChanged;
        }

        public void setSelectindex(int index)
        {
            _suppress = true;
            this.SelectedIndex = index;
            _suppress = false;
        }

        private void InternalSelectionChanged(Object s, SelectionChangedEventArgs e)
        {
            var args = new SelectionChangingEventArgs();
            OnSelectionChanging(args);
            if (args.Cancel)
            {
                return;
            }
            OnSelectionChanged(e);
        }

        public new void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (_suppress) return;

            // The selection has changed, so _index must be updated
            _index = SelectedIndex;
            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
        }

        public void OnSelectionChanging(SelectionChangingEventArgs e)
        {
            if (_suppress) return;

            // Recall the last SelectedIndex before raising SelectionChanging
            _lastIndex = (_index >= 0) ? _index : SelectedIndex;
            if (SelectionChanging == null) return;

            // Invoke user event handler and revert to last 
            // selected index if user cancels the change
            SelectionChanging(this, e);
            if (e.Cancel)
            {
                _suppress = true;
                SelectedIndex = _lastIndex;
                _suppress = false;
            }
        }
    }
}
