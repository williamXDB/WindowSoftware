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
using System.Diagnostics;

namespace Lib.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:delayctlDemo"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:delayctlDemo;assembly=delayctlDemo"
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
    ///     <MyNamespace:CTreeView/>
    ///
    /// </summary>
    
    public class CTreeView : TreeView
    {
        private const double XMouseoffset = 10.0;
        private Point _lastMouseDown;
        private TreeViewItem draggedItem;
        static CTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTreeView), new FrameworkPropertyMetadata(typeof(CTreeView)));
          
        }

        public CTreeView()
        {
            this.DragOver += treeView_DragOver;
            this.MouseMove += treeView_MouseMove;
          //  Debug.WriteLine("ctree view construct now................");

        }

        private void treeView_DragOver(object sender, DragEventArgs e) //must keep
        {
            e.Handled = true;          
          
        }  
        

        private void treeView_MouseMove(object sender, MouseEventArgs e) //must
        {
            try
            {
               
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(this);
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        draggedItem = (TreeViewItem)this.SelectedItem;
                        if (draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(this, this.SelectedValue,
                                DragDropEffects.Move);                     

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
   

       

    }
}
