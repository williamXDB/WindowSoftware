// Copyright 2012 lapthorn.net.
//
// This software is provided "as is" without a warranty of any kind. All
// express or implied conditions, representations and warranties, including
// any implied warranty of merchantability, fitness for a particular purpose
// or non-infringement, are hereby excluded. lapthorn.net and its licensors
// shall not be liable for any damages suffered by licensee as a result of
// using the software. In no event will lapthorn.net be liable for any
// lost revenue, profit or data, or for direct, indirect, special,
// consequential, incidental or punitive damages, however caused and regardless
// of the theory of liability, arising out of the use of or inability to use
// software, even if lapthorn.net has been advised of the possibility of
// such damages.
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

public enum EQFragType
{
    EQ_QV = 0, EQ_Gain = 1, EQ_Freq = 2,
    Delay_Ms = 3, Delay_Meter = 4, Delay_Feet = 5,
    OtherType=6,
};

namespace Lib.Controls
{
    /// <summary>
    /// Interaction logic for SpinnerControl.xaml
    /// </summary>
    public class SpinnerControl : UserControl
    {

        public delegate void spinValueClickChange(object sender, int cvalue, EQFragType etype, int itg, EventArgs e);
        public event spinValueClickChange onSpinValueClickChangeEvent;

        #region UpeqType property
        private const EQFragType DefaultUpDownType = EQFragType.EQ_QV;
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        // public EQFragType eqFragType = EQFragType.EQ_QV;
        [Category("SpinnerControl")]
        public EQFragType UpeqType
        {

            get { return (EQFragType)GetValue(UpdownTypeProperty); }
            set
            {
                SetValue(UpdownTypeProperty, value);
                updateMaxRange(0);
            }

        }
        private static readonly DependencyProperty UpdownTypeProperty =
             DependencyProperty.Register("UpeqType", typeof(EQFragType), typeof(SpinnerControl),
             new PropertyMetadata(DefaultUpDownType));

        public void updateMaxRange(int mv)
        {

            switch (UpeqType)
            {
                case EQFragType.EQ_QV:
                    Maximum = StrValueTables.strQFactorTable.Length-1;
                  //  Debug.WriteLine("qfactor maxmum is : {0}", Maximum);
                    Value = mv;
                    UpdateFormattedValue(mv);
                    break;
                case EQFragType.EQ_Gain:
                    Maximum = StrValueTables.strGainTable.Length - 1;
                  //  Debug.WriteLine("eq Gain maxmum is : {0}", Maximum);
                    Value = mv;
                    UpdateFormattedValue(mv);
                    break;
                case EQFragType.EQ_Freq:
                    Maximum = StrValueTables.strFreqTable.Length - 1;
                   // Debug.WriteLine("freq maxmum is : {0}", Maximum);
                    Value = mv;
                    UpdateFormattedValue(mv);
                    break;
                case EQFragType.Delay_Ms://ms
                case EQFragType.Delay_Meter://meter
                case EQFragType.Delay_Feet: //feet
                    this.Maximum = StrValueTables.Delay_Const_Range; //170
                    UpdateFormattedValue(Value);
                    break;
                case EQFragType.OtherType:
                    break;
            }
        }
        #endregion


        #region dependecy iTag
        private const int DefaultiTag = 0;

        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(SpinnerControl), new PropertyMetadata(DefaultiTag));

        #endregion





        private const bool DefaultLock = false;

        public bool isLock
        {
            get { return (bool)GetValue(isLockProperty); }
            set { SetValue(isLockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isLock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isLockProperty =
            DependencyProperty.Register("isLock", typeof(bool), typeof(SpinnerControl), new PropertyMetadata(DefaultLock));

                

        public SpinnerControl()
        {
             this.Loaded += control_Loaded;
            //new FrameworkPropertyMetadata()
            //new PropertyMetadata(DefaultiTag));            
            
        }

        private void control_Loaded(object sender,RoutedEventArgs e)
        {
            
          // Debug.WriteLine("spin user oem control loaded .................................");
            updateMaxRange(Value);

        }

        static SpinnerControl()
        {
            InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerControl), new FrameworkPropertyMetadata(typeof(SpinnerControl)));

         //   Debug.WriteLine("here cxh in static constructor function ......");

        }

        #region FormattedValue property

        /// <summary>
        /// Dependency property identifier for the formatted value with limited 
        /// write access to the underlying read-only dependency property:  we
        /// can only use SetValue on this, not on the property itself.
        /// </summary>
        private static readonly DependencyPropertyKey FormattedValuePropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("FormattedValue", typeof(string), typeof(SpinnerControl),
            new PropertyMetadata(DefaultValue.ToString()));

        /// <summary>
        /// The dependency property for the formatted value.
        /// </summary>
        private static readonly DependencyProperty FormattedValueProperty = FormattedValuePropertyKey.DependencyProperty;

        /// <summary>
        /// Returns the formatted version of the value, with the specified
        /// number of DecimalPlaces.
        /// </summary>
        public string FormattedValue
        {
            get
            {
                return (string)GetValue(FormattedValueProperty);
            }
        }

        /// <summary>
        /// Update the formatted value.
        /// </summary>
        /// <param name="newValue"></param>
        protected void UpdateFormattedValue(int newValue)
        {
           // NumberFormatInfo numberFormatInfo = new NumberFormatInfo() { NumberDecimalDigits = DecimalPlaces };
            //  use fixed point, and the built-in NumberFormatInfo
            //  implementation of IFormatProvider


            string formattedValue = "";
            bool isInRange = false;

            //if(UpeqType==EQFragType.EQ_Gain)
            // Debug.WriteLine("a being in control do update formatted value now......{0}  maximum {1} ",newValue,Maximum);


            int val = Math.Max(Minimum, Math.Min(Maximum, newValue));                     

            switch (UpeqType)
            {
                case EQFragType.EQ_QV:
                    {
                        formattedValue = StrValueTables.strQFactorTable[val];
                        isInRange = true;
                       // Debug.WriteLine("spincontrol do update value now......{0} itag:{1} --eqtype{2} ", val, iTag, UpeqType); 
                    }
                    break;
                case EQFragType.EQ_Gain:
                    {
                        formattedValue = StrValueTables.strGainTable[val];
                        isInRange = true;
                    }
                    break;
                case EQFragType.EQ_Freq:
                    {
                        formattedValue = StrValueTables.strFreqTable[val];
                        isInRange = true;
                    }

                    break;
                case EQFragType.Delay_Ms:
                    {
                        formattedValue = string.Format("{0}ms", val);
                        isInRange = true;
                    }

                    break;
                case EQFragType.Delay_Meter:
                    {
                        //  formattedValue = val*StrValueTables. save the decimal 1 points end
                        isInRange = true;
                        float tmp = StrValueTables.Delay_ConstI * 340 / 1000;
                        formattedValue = string.Format("{0}mt", Math.Round(tmp * val, 2));
                    }

                    break;
                case EQFragType.Delay_Feet:
                    {
                        float tmp = StrValueTables.Delay_ConstI * 340 / 1000 * StrValueTables.Delay_ConstII;
                        formattedValue = string.Format("{0}ft", Math.Round(tmp*val, 2));
                        isInRange = true;
                    }

                    break;
                case EQFragType.OtherType:
                    //set the text outside to the textblock
                    break;

            }

            // var formattedValue = newValue.ToString("f", numberFormatInfo);          

            //  Set the value of the FormattedValue property via its property key
            if (isInRange)
                SetValue(FormattedValuePropertyKey, formattedValue);

            //  Debug.WriteLine("value is : {0} ", Value);
        }
        #endregion

        public void setTxt(string str)
        {
            SetValue(FormattedValuePropertyKey, str);
        }


        private const string DefaultValueTxt = "";
        public string valueTxt
        {
            get { return (string)GetValue(valueTxtProperty); }
            set { SetValue(valueTxtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for valueTxt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty valueTxtProperty =
            DependencyProperty.Register("valueTxt", typeof(string), typeof(SpinnerControl),
           new FrameworkPropertyMetadata(DefaultValueTxt,
                FrameworkPropertyMetadataOptions.None, OnValueTxtChanged));

        private static void OnValueTxtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var spinc = obj as SpinnerControl;
            if (spinc != null)
            {
                var newValue = (string)args.NewValue;
              //  btn.setTxt(newValue);
                spinc.setTxt(newValue);              

            }
        }     


        #region Value property
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        [Category("SpinnerControl")]
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(SpinnerControl),
            new FrameworkPropertyMetadata(DefaultValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged,
                CoerceValue
                ));

        /// <summary>
        /// If the value changes, update the text box that displays the Value 
        /// property to the consumer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SpinnerControl control = obj as SpinnerControl;
            if (control != null)
            {
                var newValue = (int)args.NewValue;
                var oldValue = (int)args.OldValue;

                control.UpdateFormattedValue(newValue);



                //   Debug.WriteLine("updated new value is :{0} ", newValue);


                RoutedPropertyChangedEventArgs<int> e =
                    new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ValueChangedEvent);

                control.OnValueChanged(e);
            }
        }

        /// <summary>
        /// Raise the ValueChanged event.  Derived classes can use this.
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnValueChanged(RoutedPropertyChangedEventArgs<int> e)
        {
            RaiseEvent(e);
        }

        private static int LimitValueByBounds(int newValue, SpinnerControl control)
        {
            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue)); //very good
            //  then ensure the number of decimal places is correct.

            //  Debug.WriteLine("before newvalue is : {0}", newValue);
          //  newValue = Decimal.Round(newValue, control.DecimalPlaces);

            //  Debug.WriteLine("after newvalue is : {0}", newValue);
            return newValue;
        }

        private static object CoerceValue(DependencyObject obj, object value)
        {
            int newValue = (int)value;
            SpinnerControl control = obj as SpinnerControl;

            if (control != null)
            {
                //  ensure that the value stays within the bounds of the minimum and
                //  maximum values that we define.
                newValue = LimitValueByBounds(newValue, control);
            }

            return newValue;
        }


        #endregion


        #region MinimumValue property
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        [Category("SpinnerControl")]
        public int Minimum
        {
            get { return (int)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        private static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(SpinnerControl),
            new PropertyMetadata(DefaultMinimumValue));
        #endregion


        #region MaximumValue property
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        [Category("SpinnerControl")]
        public int Maximum
        {
            get { return (int)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        private static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(SpinnerControl),
            new PropertyMetadata(DefaultMaximumValue));

        #endregion



        #region DecimalPlaces property
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        [Category("SpinnerControl")]
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        private static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(SpinnerControl),
            new PropertyMetadata(DefaultDecimalPlaces));

        #endregion


        #region Change property
        /// <summary>
        /// This is the Control property that we expose to the user.
        /// </summary>
        [Category("SpinnerControl")]
        public int Change
        {
            get { return (int)GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }

        private static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register("Change", typeof(int), typeof(SpinnerControl),
            new PropertyMetadata(DefaultChange));

        #endregion


        #region Default values

        /// <summary>
        /// Define the min, max and starting value, which we then expose 
        /// as dependency properties.
        /// </summary>
        private const int DefaultMinimumValue = 0,
            DefaultMaximumValue = 100,
            DefaultValue = DefaultMinimumValue,
            DefaultChange = 1;

        /// <summary>
        /// The default number of decimal places, i.e. 0, and show the
        /// spinner control as an int initially.
        /// </summary>
        private const int DefaultDecimalPlaces = 0;
        #endregion


        #region Command Stuff
        public static RoutedCommand IncreaseCommand { get; set; }

        protected static void OnIncreaseCommand(Object sender, ExecutedRoutedEventArgs e)
        {
            SpinnerControl control = sender as SpinnerControl;

            if (control != null)
            {
                control.OnIncrease();
            }
        }

        protected void OnIncrease()
        {
            //  see https://connect.microsoft.com/VisualStudio/feedback/details/489775/
            //  for why we do this.
            Value = LimitValueByBounds(Value + Change, this);

            int val = (int)Math.Max(Minimum, Value);
            if (onSpinValueClickChangeEvent != null)
            {
                onSpinValueClickChangeEvent(this,val, UpeqType, iTag, new EventArgs());  //iTag
               // Debug.WriteLine("onInCrease ....................");

            }


        }

        public static RoutedCommand DecreaseCommand { get; set; }

        protected static void OnDecreaseCommand(Object sender, ExecutedRoutedEventArgs e)
        {
            SpinnerControl control = sender as SpinnerControl;

            if (control != null)
            {
                control.OnDecrease();
            }
        }

        protected void OnDecrease()
        {
            //  see https://connect.microsoft.com/VisualStudio/feedback/details/489775/
            //  for why we do this.
            Value = LimitValueByBounds(Value - Change, this);
            int val = (int)Math.Max(Minimum, Value);
            if (onSpinValueClickChangeEvent != null)
            {
                onSpinValueClickChangeEvent(this,val, UpeqType, iTag, new EventArgs());  //iTag
               // Debug.WriteLine("onInCrease ....................");

            }
        }

        /// <summary>
        /// Since we're using RoutedCommands for the up/down buttons, we need to
        /// register them with the command manager so we can tie the events
        /// to callbacks in the control.
        /// </summary>
        private static void InitializeCommands()
        {
            //  create instances
            IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(SpinnerControl));
            DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(SpinnerControl));

            //  register the command bindings - if the buttons get clicked, call these methods.
            CommandManager.RegisterClassCommandBinding(typeof(SpinnerControl), new CommandBinding(IncreaseCommand, OnIncreaseCommand));
            CommandManager.RegisterClassCommandBinding(typeof(SpinnerControl), new CommandBinding(DecreaseCommand, OnDecreaseCommand));

            //  lastly bind some inputs:  i.e. if the user presses up/down arrow 
            //  keys, call the appropriate commands.
            CommandManager.RegisterClassInputBinding(typeof(SpinnerControl), new InputBinding(IncreaseCommand, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(SpinnerControl), new InputBinding(IncreaseCommand, new KeyGesture(Key.Right)));
            CommandManager.RegisterClassInputBinding(typeof(SpinnerControl), new InputBinding(DecreaseCommand, new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(typeof(SpinnerControl), new InputBinding(DecreaseCommand, new KeyGesture(Key.Left)));
        }
        #endregion


        #region Events

        /// <summary>
        /// The ValueChangedEvent, raised if  the value changes.
        /// </summary>
        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<int>), typeof(SpinnerControl));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        #endregion
    }
}
