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
    ///     <MyNamespace:DynGainMark/>
    ///
    /// </summary>
    public class DynGainMark : CommControl
    {
        private static readonly CultureInfo CIR = System.Globalization.CultureInfo.CurrentCulture;

        static DynGainMark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DynGainMark), new FrameworkPropertyMetadata(typeof(DynGainMark)));
        }

        public static string[] strDYNMark = { 
         "  -2","  -6","-10","-14","-18","-22","-26","-30","-34","-38","-42","-46","-48","-50"                                   
        };




        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            double wd = ActualWidth;
            double ht = ActualHeight;
            double leftDynStep = (double)this.ActualHeight / (strDYNMark.Length)  -0.25;

            double xleftOffset = 1.0;
            FormattedText texte = null;
            double fontsize = 9.0;
            string ftext = string.Empty;
            for (int i = 0; i < strDYNMark.Length; i++)
            {
                ftext = strDYNMark[i];
                texte = new FormattedText(ftext, CIR, FlowDirection.LeftToRight,
                     new Typeface("Verdana"), fontsize, Brushes.White);
                dc.DrawText(texte, new Point(xleftOffset, i * leftDynStep));

            }

        }


    }
}
