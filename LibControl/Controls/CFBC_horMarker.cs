using System;
using System.Collections.Generic;
using System.Globalization;
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
    ///     xmlns:MyNamespace="clr-namespace:Lib.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Lib.Controls;assembly=Lib.Controls"
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
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CFBC_horMarker : CommControl
    {
        static CFBC_horMarker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CFBC_horMarker), new FrameworkPropertyMetadata(typeof(CFBC_horMarker)));
        }

        public string[] StrMark = 
         { 
           "-∞" ,
           "-50",
           "-40",
           "-30",
           "-20",
           "-10",
           "-5",
           "0",
           "5",
           "dB",
           "10"                  
         };
        private  readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;
        private void drawMark(DrawingContext dc)
        {
            double wd = this.ActualWidth;
            int num = StrMark.Length - 1;
            int fontSize = 10;
            SolidColorBrush markBrush = Brushes.White;
          
                FormattedText text = null;
                double horPerseg = (double)wd/num;
                int ty = 1;
                double tx = 0;
                

                for (int i = 0; i < num+1; i++)
                {
                    tx = horPerseg * i;
                    text = new FormattedText(StrMark[i], CIR, FlowDirection.LeftToRight,
                                           new Typeface("Verdana"), fontSize, markBrush);
                    dc.DrawText(text, new Point(tx, ty));
                }
         
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double wd = ActualWidth;
            double ht = ActualHeight;
            dc.DrawRectangle(Brushes.Transparent, new Pen(), new Rect(0, 0, wd, ht));      
            drawMark(dc);          
        }
        public void redraw()
        {
            //  Debug.WriteLine("readraw.....");
            InvalidateVisual();
        }
    }
}
