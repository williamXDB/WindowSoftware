using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for CSwitchers.xaml
    /// </summary>
    public partial class CSwitchers : UserControl
    {
        public CSwitchers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// default brush 
        /// </summary>
        private static SolidColorBrush DefaultActiveBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0, 255, 0));
        private static SolidColorBrush DefaultUActiveBrush = new SolidColorBrush(Color.FromArgb(0xFF, 255, 255, 0));

        public Brush activeBrush
        {
            get { return (Brush)GetValue(activeBrushPropertyProperty); }
            set { SetValue(activeBrushPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeBrushProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeBrushPropertyProperty =
            DependencyProperty.Register("activeBrush",
            typeof(Brush), typeof(CSwitchers),
            new FrameworkPropertyMetadata(DefaultActiveBrush,
                FrameworkPropertyMetadataOptions.None, OnActiveBrushChanged));


        public Brush unActiveBrush
        {
            get { return (Brush)GetValue(unActiveBrushPropertyProperty); }
            set { SetValue(unActiveBrushPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeBrushProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty unActiveBrushPropertyProperty =
            DependencyProperty.Register("unActiveBrush",
            typeof(Brush), typeof(CSwitchers),
            new FrameworkPropertyMetadata(DefaultUActiveBrush,
                FrameworkPropertyMetadataOptions.None, OnUnactiveBrushChanged));


        //new PropertyMetadata(DefaultUActiveBrush));

        private static void OnActiveBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var btn = obj as CSwitchers;
           
            if (btn != null)
            {
                var newValue = (Brush)args.NewValue;
                var oldValue = (Brush)args.OldValue;
                btn.activeBrush = newValue;
                btn.updateBrushWithValue();

            }
        }

        private static void OnUnactiveBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var btn = obj as CSwitchers;
            if (btn != null)
            {
                var newValue = (Brush)args.NewValue;
                var oldValue = (Brush)args.OldValue;
                btn.unActiveBrush = newValue;
                btn.updateBrushWithValue();
            }
        }


        /// <summary>
        /// registry a property for just bool isselected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedPropertyProperty); }
            set { SetValue(IsSelectedPropertyProperty, value); }

        }
        // Using a DependencyProperty as the backing store for IsSelectedProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsSelectedPropertyProperty =
        //    DependencyProperty.Register("IsSelectedProperty", typeof(bool), typeof(CSwitcher), new PropertyMetadata(0));

        private static bool DefaultValue = false;

        private static readonly DependencyProperty IsSelectedPropertyProperty =
                    DependencyProperty.Register("IsSelected", typeof(bool), typeof(CSwitchers),
                    new FrameworkPropertyMetadata(DefaultValue,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        OnSelectedChanged
                        ));


        private static void OnSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var btn = obj as CSwitchers;
            if (btn != null)
            {
                var newValue = (bool)args.NewValue;
                var oldValue = (bool)args.OldValue;
                // control.UpdateFormattedValue(newValue);
                // if(newValue!=oldValue)
                btn.updateBrushWithValue();
                //   Debug.WriteLine("updated new value is :{0} ", newValue);
                RoutedPropertyChangedEventArgs<bool> e =
                  new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, ValueChangedEvent);
                btn.OnValueChanged(e);

            }
        }
        public void updateBrushWithValue()
        {

            Brush brush = (IsSelected ? activeBrush : unActiveBrush);

            inerButn.Background = brush;
            Debug.WriteLine("update brush now.........123456............");


        }

        /// <summary>
        /// for the subclass 
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnValueChanged(RoutedPropertyChangedEventArgs<bool> e)
        {
            RaiseEvent(e);
        }

        #region Events

        /// <summary>
        /// The ValueChangedEvent, raised if  the value changes.
        /// </summary>
        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<bool>), typeof(CSwitchers));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<bool> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        #endregion

    }
}
