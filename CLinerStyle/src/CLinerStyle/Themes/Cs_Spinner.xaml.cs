using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CLinerStyle
{
    /// <summary>
    /// Cs_Cs_Spinner.xaml 的互動邏輯
    /// </summary>
    public partial class Cs_Spinner : UserControl
    {
        public static int DefaultMaximun = 100;
        public static int DefaultMinimun = 0;
        public static int DefaultValueNum = 0;
        public static RoutedCommand IncreaseCommand { get; set; }
        public static RoutedCommand DecreaseCommand { get; set; }
        Timer timer;
        Timer time = new Timer(500);
        public Cs_Spinner()
        {
            InitializeComponent();
            Cs_Spinner.IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(Cs_Spinner));
            Cs_Spinner.DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(Cs_Spinner));
            CommandManager.RegisterClassCommandBinding(typeof(Cs_Spinner), new CommandBinding((ICommand)Cs_Spinner.IncreaseCommand, new ExecutedRoutedEventHandler(Cs_Spinner.OnIncreaseCommand)));
            CommandManager.RegisterClassCommandBinding(typeof(Cs_Spinner), new CommandBinding((ICommand)Cs_Spinner.DecreaseCommand, new ExecutedRoutedEventHandler(Cs_Spinner.OnDecreaseCommand)));
            CommandManager.RegisterClassInputBinding(typeof(Cs_Spinner), new InputBinding((ICommand)Cs_Spinner.IncreaseCommand, (InputGesture)new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(Cs_Spinner), new InputBinding((ICommand)Cs_Spinner.IncreaseCommand, (InputGesture)new KeyGesture(Key.Right)));
            CommandManager.RegisterClassInputBinding(typeof(Cs_Spinner), new InputBinding((ICommand)Cs_Spinner.DecreaseCommand, (InputGesture)new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(typeof(Cs_Spinner), new InputBinding((ICommand)Cs_Spinner.DecreaseCommand, (InputGesture)new KeyGesture(Key.Left)));

        }

        public string Content1
        {
            get { return (string)GetValue(Content1Property); }
            set { SetValue(Content1Property, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Content1Property =
            DependencyProperty.Register("Content1", typeof(string), typeof(Cs_Spinner), null);

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Cs_Spinner ks = d as Cs_Spinner;
            ks.updatestr();
        }




        public int Maximun
        {
            get
            {
                return (int)this.GetValue(Cs_Spinner.MaximunProperty);
            }
            set
            {
                this.SetValue(Cs_Spinner.MaximunProperty, (object)value);
            }
        }
        public static readonly DependencyProperty MaximunProperty =
            DependencyProperty.Register("Maximun", typeof(int), typeof(Cs_Spinner), new PropertyMetadata(DefaultMaximun, null));

        public int Minimun
        {
            get
            {
                return (int)this.GetValue(Cs_Spinner.MinimunProperty);
            }
            set
            {
                this.SetValue(Cs_Spinner.MinimunProperty, (object)value);
            }
        }
        public static readonly DependencyProperty MinimunProperty =
            DependencyProperty.Register("Minimun", typeof(int), typeof(Cs_Spinner), new PropertyMetadata(DefaultMinimun, null));
        public delegate void SliderValueChanged(Object sender, int value);
        public event SliderValueChanged OnKaSliderValueChanged;
        public int ValueNum
        {
            get
            {
                return (int)this.GetValue(ValueChangedPropertyProperty);
            }
            set
            {
                if ((int)value > Maximun) return;
                else if ((int)value < Minimun) return;
                this.SetValue(ValueChangedPropertyProperty, (object)value);
            }
        }
        //private static readonly DependencyProperty ValueChangedPropertyProperty = DependencyProperty.Register("ValueNum", typeof(int), typeof(Ka_Slider), (PropertyMetadata)new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty ValueChangedPropertyProperty =
           DependencyProperty.Register("ValueNum", typeof(int), typeof(Cs_Spinner), new PropertyMetadata(DefaultValueNum, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Cs_Spinner ks = d as Cs_Spinner;
            //ks.updatestr();
        }

        private void Cs_Spinner_Loaded(object sender, RoutedEventArgs e)
        {
            updatestr();
        }




        private void updatestr()
        {
            Content1 = ValueNum.ToString();
        }



        private void btn_MouseMove(object sender, MouseEventArgs e)
        {

        }
        protected static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Cs_Spinner Cs_Spinner = sender as Cs_Spinner;
            if (Cs_Spinner == null)
                return;
            Cs_Spinner.OnIncrease();
        }

        protected void OnIncrease()
        {
            ValueNum = ValueNum + 1;
            //this.Value = Cs_Spinner.LimitValueByBounds(this.Value + this.Change, this);
            //int cvalue = Math.Max(this.Minimum, Math.Min(this.Maximum, this.Value));
            //if (this.onSpinValueClickChangeEvent == null)
            //    return;
            //this.onSpinValueClickChangeEvent((object)this, cvalue, this.UpeqType, this.iTag, new EventArgs());
        }
        protected static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Cs_Spinner Cs_SpinnerControl = sender as Cs_Spinner;
            if (Cs_SpinnerControl == null)
                return;
            Cs_SpinnerControl.OnDecrease();
        }
        protected void OnDecrease()
        {
            ValueNum = ValueNum - 1;
            //this.Value = Cs_SpinnerControl.LimitValueByBounds(this.Value - this.Change, this);
            //int cvalue = Math.Max(this.Minimum, Math.Min(this.Maximum, this.Value));
            //if (this.onSpinValueClickChangeEvent == null)
            //    return;
            //this.onSpinValueClickChangeEvent((object)this, cvalue, this.UpeqType, this.iTag, new EventArgs());
        }
        private void btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    Button btn = sender as Button;
            //    if (Convert.ToInt16(btn.Tag) == 0)
            //    {
            //        ValueNum = ValueNum + 1;
            //    }
            //    else
            //    {
            //        ValueNum = ValueNum - 1;
            //    }
            //}
        }

        enum sub_add
        {
            Add = 0,
            Sub = 1
        }
        sub_add sa;
        private void btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;
            //var element = (FrameworkElement)sender;
            //element.
            if (Convert.ToInt16(btn.Tag) == 0)
            {
                OnIncrease();
                sa = sub_add.Add;
            }
            else
            {
                OnDecrease();
                sa = sub_add.Sub;
            }
            if (OnKaSliderValueChanged != null)
                OnKaSliderValueChanged(this, ValueNum);
            //isStart = true;
            //Timer time = new Timer(500);
            //time.Elapsed += new ElapsedEventHandler((o, ex) => btn.Dispatcher.Invoke(new Action(() =>
            //{
            //    if (isStart)
            //    {

            //    }
            //    time.Stop();
            //    time.Dispose();
            //})));
            time = new Timer(500);
            time.AutoReset = false;
            time.Elapsed += Time_Elapsed;
            time.Close();
            time.Start();
        }
        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer = new Timer();
            timer.Interval = 50;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        private void btn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            if (time != null)
            {
                time.Stop();
                time.Dispose();
            }
        }

        public delegate void updateDo();
        public void dtInvoke(updateDo fdo)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                                      (System.Threading.ThreadStart)delegate ()
                                      {
                                          fdo();
                                      });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (sa == sub_add.Add)
                dtInvoke(() => {
                    if (this.UpBtn.IsPressed == true)
                    {
                        ValueNum = ValueNum + 1; if (OnKaSliderValueChanged != null)
                            OnKaSliderValueChanged(this, ValueNum);
                    }
                });
            else
                dtInvoke(() => {
                    if (this.DownBtn.IsPressed == true)
                    {
                        ValueNum = ValueNum - 1; if (OnKaSliderValueChanged != null)
                            OnKaSliderValueChanged(this, ValueNum);
                    }
                });

        }

    }
}

