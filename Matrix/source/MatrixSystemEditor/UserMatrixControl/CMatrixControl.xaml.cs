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
using Lib.Controls;
using CommLibrary;
using System.Diagnostics;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for CMatrixControl.xaml
    /// </summary>
    public partial class CMatrixControl : UserControl
    {
        private const int Max_CHNum = 20;
        public CMatrixControl()
        {
            for (int i = 0; i < Max_CHNum; i++)
            {
                m_matrix[i] = new byte[Max_CHNum];
            }
            InitializeComponent();
            retrieve_setMatrixProperty();
        }

        public byte[][] m_matrix = new byte[Max_CHNum][];
        private static List<CSwitcher> switcherList = new List<CSwitcher>();

        /// <summary>
        /// search switcher button
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private CSwitcher findButonWith_iTag(int index) //RootGrid
        {

            TraverseChildrenControls tchildrens = new TraverseChildrenControls();
            CSwitcher sbtn = null;
            foreach (object o in tchildrens.GetChildren(RootGrid, 1))
            {

                if (o.GetType() == typeof(CSwitcher))
                {
                    sbtn = (CSwitcher)o;
                    if (sbtn != null && sbtn.iTag == index)
                    {
                        break;
                    }
                }

            }
            return sbtn;

        }

        private void drawVerticalLine(int lindex, DrawingContext dc)
        {
            int biTag = lindex * 20;
            int eiTag = lindex * 20 + 19;
            CSwitcher btn1 = findButonWith_iTag(biTag);
            CSwitcher btn2 = findButonWith_iTag(eiTag);
            Pen linePen = new Pen(Brushes.Gray, 1);
            if (btn1 != null && btn2 != null)
            {
                Point btnPt1 = btn1.TransformToAncestor(this).Transform(new Point(0, 0));
                Point btnPt2 = btn2.TransformToAncestor(this).Transform(new Point(0, 0));
                Point pt1 = btnPt1;
                Point pt2 = btnPt2;

                pt1.X += btn1.ActualWidth / 2;
                pt2.X = pt1.X;
                pt1.Y = 62;
                pt1.Y += btn1.ActualHeight / 2;
                pt2.Y += btn2.ActualHeight / 2;
                dc.DrawLine(linePen, pt1, pt2);
                Rect tmpRect = new Rect(pt1.X - 12, 5, 24, 65);
                dc.DrawRectangle(null, linePen, tmpRect);

            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sgrid"></param>
        private void retrieve_setMatrixProperty()
        {
            TraverseChildrenControls tchildrens = new TraverseChildrenControls();
            foreach (object o in tchildrens.GetChildren(RootGrid, 1))
            {

                if (o.GetType() == typeof(CSwitcher))
                {
                    CSwitcher cBtn = (CSwitcher)o;
                    // cBtn.Content = cBtn.iTag.ToString();
                    cBtn.Click += matrixButton_Switch;
                    switcherList.Add(cBtn);
                }



            }

        }
        public delegate void matrixValueControlChanged(Object sender, int row, int column);
        public event matrixValueControlChanged onMatrixValueControlChangedEvent;


        private void matrixButton_Switch(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            Debug.WriteLine("matrix btn click with {0}", btn.iTag);
            int index = btn.iTag;
            int row = index / Max_CHNum;
            int column = index % Max_CHNum;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_matrix[row][column] = tmp;
            // btn.IsSelected = (tmp>0);
            if (onMatrixValueControlChangedEvent != null)
            {
                onMatrixValueControlChangedEvent(this, row, column);

            }

        }

        #region Outside user interface
        //interface for user outside support
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void updateWithOnRow(int r)
        {
            byte tmpR = 0;
            int index = 0;
            for (int i = 0; i < Max_CHNum; i++)
            {
                tmpR = m_matrix[r][i];
                index = r * Max_CHNum + i;
                switcherList[index].IsSelected = (tmpR > 0);
            }



        }
        public void refreshcontrol()
        {
            // for(int i=0;)
            for (int r = 0; r < Max_CHNum; r++)
            {
                updateWithOnRow(r);
            }
        }
        #endregion


        /// <summary>
        /// drawHorizontal
        /// </summary>
        /// <param name="lindex"></param>
        /// <param name="dc"></param>
        private void drawHorLine(int lindex, DrawingContext dc)
        {
            int biTag = lindex;
            int eiTag = lindex + 380;
            CSwitcher btn1 = findButonWith_iTag(biTag);
            CSwitcher btn2 = findButonWith_iTag(eiTag);
            if (btn1 != null && btn2 != null)
            {
                Point btnPt1 = btn1.TransformToAncestor(this).Transform(new Point(0, 0));
                Point btnPt2 = btn2.TransformToAncestor(this).Transform(new Point(0, 0));
                Point pt1 = btnPt1;
                Point pt2 = btnPt2;
                pt1.X -= 85.5;
                pt1.Y += btn1.ActualHeight / 2;
                pt2.Y += btn2.ActualHeight / 2;
                if (lindex % 2 == 0)
                    dc.DrawLine(new Pen(Brushes.Purple, 1), pt1, pt2);
                else
                    dc.DrawLine(new Pen(Brushes.Gray, 1), pt1, pt2);
            }


        }
        /// <summary>
        /// render below
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            for (int i = 0; i < 20; i++)
            {
                drawHorLine(i, dc);
                drawVerticalLine(i, dc);
            }
            Debug.WriteLine("matrix control draw now...................");

            // dc.DrawLine(new Pen(Brushes.Black,1),btn1.re)
        }

    }
}
