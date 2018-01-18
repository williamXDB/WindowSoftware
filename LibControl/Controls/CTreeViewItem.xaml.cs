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
    /// Interaction logic for CTreeViewItem.xaml
    /// </summary>
    public partial class CTreeViewItem : UserControl
    {
              

        private static string strItem = "";
        public string itemText
        {
            get { return (string)GetValue(itemTextProperty); }
            set { SetValue(itemTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for itemText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty itemTextProperty =
            DependencyProperty.Register("itemText", typeof(string), typeof(CTreeViewItem),
            new FrameworkPropertyMetadata(strItem, FrameworkPropertyMetadataOptions.None, onItemTextChange
            ));

        private static void onItemTextChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CTreeViewItem;
            if (mcontrol != null)
            {
                mcontrol.edTitle.Text= (string)args.NewValue;               

                mcontrol.InvalidateVisual();
            }
        }



        private static ImageSource DefaultItemSrc = null;
        public ImageSource itemSource
        {
            get { return (ImageSource)GetValue(itemSourceProperty); }
            set { SetValue(itemSourceProperty, value); }
        }
        // Using a DependencyProperty as the backing store for itemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty itemSourceProperty =
            DependencyProperty.Register("itemSource", typeof(ImageSource), typeof(CTreeViewItem),
            new FrameworkPropertyMetadata(DefaultItemSrc, FrameworkPropertyMetadataOptions.None, onImageChange
            ));

        private static void onImageChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CTreeViewItem;
            if (mcontrol != null)
            {
                //  mcontrol.isLoopReverse = (bool)args.NewValue;
                //mcontrol.img.Source.SetValue()
                mcontrol.img.Source = (ImageSource)args.NewValue;               
                mcontrol.InvalidateVisual();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public CTreeViewItem()
        {
            InitializeComponent();
        }
    }
}
