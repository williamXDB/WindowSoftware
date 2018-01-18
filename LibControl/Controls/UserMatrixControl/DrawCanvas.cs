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
using CommLibrary;
//PM> Install-Package System.Data.SQLite.Core
namespace Lib.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MatrixSystemEditor"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MatrixSystemEditor;assembly=MatrixSystemEditor"
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
    ///     <MyNamespace:DrawCanvas/>
    ///
    /// </summary>
    /// 
    public enum PointMoveDirection
    {
        XMNon = 98, XMLeft, XMRight, XMUP, XMDown
    };


    public class DrawCanvas : Canvas
    {
        static DrawCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawCanvas), new FrameworkPropertyMetadata(typeof(DrawCanvas)));

        }

        public DrawCanvas()
        {

            AllowDrop = true;
            if (devList == null)
                devList = new List<CDeviceInfo>();
            if (iosqlOperator == null)
                iosqlOperator = new IOSqliteOperation();
            initialData();

            if (dPointAry == null)
                dPointAry = new List<Point>(); //for draw         

        }
        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;
        /// <summary>
        /// draw canvas..........
        /// </summary>
        /// <param name="dc"></param>

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            //render canvas.............
            if (pindex > 0)
            {
                for (int i = 0; i < pindex; i++)
                    dc.DrawLine(new Pen(Brushes.Black, 1), linePoint[2 * i], linePoint[2 * i + 1]);

            }


        }


        private const double DefaultCompareDia = 3.0;
        public double compareDia
        {
            get { return (double)GetValue(compareDiaProperty); }
            set { SetValue(compareDiaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for compareDia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty compareDiaProperty =
            DependencyProperty.Register("compareDia", typeof(double), typeof(DrawCanvas), new PropertyMetadata(DefaultCompareDia));


        #region pointMove direction
        /// <summary>
        /// get the next point move to left or right or up or down
        /// </summary>
        /// <param name="ptSrc"></param>
        /// <param name="ptDst"></param>
        /// <returns></returns>
        public PointMoveDirection gpointMoveOrt(Point ptSrc, Point ptDst)
        {
            PointMoveDirection pmvd = PointMoveDirection.XMNon; ;

            double deltaX = Math.Abs(ptSrc.X - ptDst.X);
            double deltaY = Math.Abs(ptSrc.Y = ptDst.Y);

            if (deltaX >= compareDia && deltaY < compareDia)
            {
                if (ptDst.X > ptSrc.X)
                    pmvd = PointMoveDirection.XMRight;//move right
                else if (ptDst.X < ptSrc.X)
                    pmvd = PointMoveDirection.XMLeft;//move left

            }
            else if (deltaY >= compareDia && deltaX < compareDia)
            {

                if (ptDst.X > ptSrc.X)
                    pmvd = PointMoveDirection.XMRight;//move right
                else if (ptDst.X < ptSrc.X)
                    pmvd = PointMoveDirection.XMLeft;//move left


            }
            return pmvd;
        }

        public Point getAnglePoint(Point ptSrc, Point ptNext)
        {

            Point newPt = new Point();
            newPt.X = ptNext.X;
            newPt.Y = ptNext.Y;
            PointMoveDirection ptOrt = gpointMoveOrt(ptSrc, ptNext);
            if (ptOrt == PointMoveDirection.XMLeft || ptOrt == PointMoveDirection.XMRight)
            {
                newPt.X = ptNext.X;
                newPt.Y = ptSrc.Y;
                //
            }
            else if (ptOrt == PointMoveDirection.XMUP || ptOrt == PointMoveDirection.XMDown)
            {
                newPt.X = ptNext.X;
                newPt.Y = ptSrc.Y;
            }
            return newPt;

        }


        private const double DefaultPerMoveDia = 10.0;
        public double perMoveDia
        {
            get { return (double)GetValue(perMoveDiaProperty); }
            set { SetValue(perMoveDiaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for perMoveDia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty perMoveDiaProperty =
            DependencyProperty.Register("perMoveDia", typeof(double), typeof(DrawCanvas), new PropertyMetadata(DefaultPerMoveDia));

        /// <summary>
        /// case user move mouse to draw,so caculate the angle line point to draw
        /// </summary>
        /// <param name="ptSrc"></param>
        /// <param name="ptDst"></param>
        /// <returns></returns>
        public Point getNewCorrectPoint(Point ptSrc, Point ptDst)
        {
            Point ptRes = new Point();
            ptRes.X = ptDst.X;
            ptSrc.Y = ptDst.Y;
            //-----------------
            PointMoveDirection ptOrt = gpointMoveOrt(ptSrc, ptDst);

            double deltaX = Math.Abs(ptSrc.X - ptDst.X);
            double deltaY = Math.Abs(ptSrc.Y = ptDst.Y);

            int xStep = (int)(deltaX / perMoveDia);
            int yStep = (int)(deltaY / perMoveDia);

            switch (ptOrt)
            {
                case PointMoveDirection.XMUP:

                    if (yStep > 0 && (deltaY % perMoveDia != 0))
                    {
                        ptRes.Y = ptSrc.X - (yStep + 1) * perMoveDia;
                    }

                    break;
                case PointMoveDirection.XMDown:
                    if (yStep > 0 && (deltaY % perMoveDia != 0))
                    {
                        ptRes.Y = ptSrc.X + (yStep + 1) * perMoveDia;
                    }
                    break;

                case PointMoveDirection.XMLeft:
                    if (xStep > 0 && (deltaX % perMoveDia != 0))
                    {
                        ptRes.X = ptSrc.X - (xStep + 1) * perMoveDia;
                    }
                    break;
                case PointMoveDirection.XMRight:
                    if (xStep > 0 && (deltaX % perMoveDia != 0))
                    {
                        ptRes.X = ptSrc.X + (xStep + 1) * perMoveDia;
                    }
                    break;
            }
            if (ptRes.X < 0)
                ptRes.X = 0;
            else if (ptRes.X > this.ActualWidth)
                ptRes.X = this.ActualWidth;

            if (ptRes.Y < 0)
                ptRes.Y = 0;
            else if (ptRes.Y > this.ActualHeight)
                ptRes.Y = this.ActualHeight;
            return ptRes;
        }


        #endregion


        public void Redraw()
        {
            this.InvalidateVisual();
        }

        public void AddChildren(UIElement ue, Point vpoint, bool isSetLocation)
        {
            if (ue != null)
            {
                Children.Add(ue);
                if (isSetLocation)
                {
                    Canvas.SetLeft(ue, vpoint.X);
                    Canvas.SetTop(ue, vpoint.Y);
                }
                ///-----------------------------------------
                ue.MouseLeftButtonDown += component_MouseLeftButtonDown;
                ue.MouseLeftButtonUp += component_MouseLeftButtonUp;
                ue.MouseMove += component_MouseMove;
                // ue.MouseRightButtonDown += component_mouseRightButonDown;
            }

        }

        /// <summary>
        /// on mouse event override
        /// </summary>
        /// <param name="e"></param>
        /// 
        int pindex = 0;//0..5

        private const int DefaultConPointNum = 6;
        public int MaxSegPointNum
        {
            get { return (int)GetValue(MaxSegPointNumProperty); }
            set { SetValue(MaxSegPointNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxSegPointNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxSegPointNumProperty =
            DependencyProperty.Register("MaxSegPointNum", typeof(int), typeof(DrawCanvas), new PropertyMetadata(DefaultConPointNum));

        private const int Max_LinePoint = 10;



        Point[] linePoint = new Point[Max_LinePoint];

        private void initialData()
        {
            if (linePoint == null)
            {
                linePoint = new Point[Max_LinePoint];

            }
            for (int i = 0; i < Max_LinePoint; i++)
            {
                linePoint[i] = new Point();
            }

        }



        //by shortcut switch ,the cbeginDrawLine is turn true of false

        #region mouse event proces define..
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Point pt = e.GetPosition(this);
            if (cbeginDrawLine)
            {
                dPointAry.Add(pt);
            }

        }


        public void stopDrawLine()
        {
            Debug.WriteLine("stop draw line now......line number {0}....current point number is {1}..", dLineList.Count, dPointAry.Count);
            cbeginDrawLine = false;
            PointLine pline = new PointLine();
            pline.LinePointAry = dPointAry;
            dLineList.Add(pline);
            dPointAry.Clear();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        int tesC = 0;
        System.Windows.Point mousePt;

        private List<Point> dPointAry = new List<Point>(); //for draw
        public List<PointLine> dLineList = new List<PointLine>();

        //public bool MyProperty { get; set; }



        private const bool DefaultDrawLine = false;
        public bool cbeginDrawLine
        {
            get { return (bool)GetValue(cbeginDrawLineProperty); }
            set { SetValue(cbeginDrawLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cbeginDrawLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cbeginDrawLineProperty =
            DependencyProperty.Register("cbeginDrawLine", typeof(bool), typeof(DrawCanvas),
             new FrameworkPropertyMetadata(DefaultDrawLine,
                FrameworkPropertyMetadataOptions.None, OnBeginLineChanged));

        private static void OnBeginLineChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var dwc = obj as DrawCanvas;
            if (dwc != null)
            {
                var newValue = (bool)args.NewValue;
                //  btn.setTxt(newValue);
                dwc.cbeginDrawLine = newValue;
                Debug.WriteLine("beginDrawline  value is {0}", newValue);


            }
        }




        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            tesC++;
            mousePt = e.GetPosition(this);
            // Debug.WriteLine("canvas on mouse move...........................{0}", tesC);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }
        #endregion

        private void component_mouseRightButonDown(object sender, MouseButtonEventArgs e)
        {
            //
            if (sender.GetType().IsSubclassOf(typeof(ComUserMD)))
            {
                ComUserMD module = sender as ComUserMD;
                if (module.ContextMenu != null)
                {
                    // module.ContextMenu.sh

                    Debug.WriteLine("right mouse click now....................");
                    //e.Handled = true;
                    // module.ContextMenu.IsOpen = true;

                    // ContextMenuService.

                }

            }


        }

        private void component_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                source = (ComUserMD)sender;
                Mouse.Capture(source);
                captured = true;
                x_shape = Canvas.GetLeft(source); //get the control x coordinate in the canvas
                x_canvas = e.GetPosition(this).X; //get relative parent x coordinate

                y_shape = Canvas.GetTop(source);
                y_canvas = e.GetPosition(this).Y;//get the mouse Y currently

            }
            else
            {
                Mouse.Capture(null);
            }


            // Debug.WriteLine("draw canvas .......mouse leftbutton down now.....................");
        }




        private void component_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured && source != null)
            {
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;
                x_shape += x - x_canvas;             //caculate the difference in x coordinate move
                Canvas.SetLeft(source, x_shape);
                x_canvas = x;
                y_shape += y - y_canvas;
                Canvas.SetTop(source, y_shape);
                y_canvas = y;
            }
        }

        private void component_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            captured = false;
        }
        //set and initialize controls below--------------------

        public void setRemoteIP(String strIP)
        {
            foreach (UIElement o in this.Children)
            {
                if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                {
                    ComUserMD sControl = (ComUserMD)o;
                    // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                    if (
                        (sControl.devinfo.devModuleType == Module_Type.Mod_LAN)
                        || (sControl.devinfo.devModuleType == Module_Type.Mod_CLIV))
                    {
                        sControl.setRouterAddr(strIP);
                        break;
                    }
                }
            }

        }


        public bool isMiscellaneous(Module_Type modType)
        {
            bool isres = false;

            switch (modType)
            {
                case Module_Type.Mod_LAN: //if is lan module ,must only one exists
                    {
                        foreach (UIElement o in this.Children)
                        {
                            if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                            {
                                ComUserMD sControl = (ComUserMD)o;
                                // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                                if (
                                    (sControl.devinfo.devModuleType == Module_Type.Mod_LAN)
                                    || (sControl.devinfo.devModuleType == Module_Type.Mod_CLIV))
                                {
                                    isres = true;
                                    break;
                                }



                            }

                        }

                    }
                    break;
                case Module_Type.Mod_CLIV:
                    {
                        foreach (UIElement o in this.Children)
                        {
                            if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                            {
                                ComUserMD sControl = (ComUserMD)o;
                                // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                                if (sControl.devinfo.devModuleType == Module_Type.Mod_LAN)
                                {
                                    isres = true;
                                    break;
                                }



                            }

                        }

                    }
                    break;

            }

            return isres;

        }

        /// <summary>
        /// CL V4 module
        /// </summary>
        /// <returns></returns>
        public int getCLIVModuleNum()
        {
            int num = 0;
            foreach (UIElement o in this.Children)
            {
                if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                {
                    ComUserMD sControl = (ComUserMD)o;
                    if (sControl.devinfo.devModuleType == Module_Type.Mod_CLIV)
                        num++;

                }

            }
            return num;
        }


        public void checkDeviceModule_fromSpecial(DeviceProvision spcDev)
        {
            foreach (UIElement o in this.Children)
            {
                if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                {
                    ComUserMD sControl = (ComUserMD)o;
                    // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                    if (sControl.devinfo.devProv.isSameToAnotherDevice(spcDev))
                    {
                        sControl.setHeaderStatus(true);//clear the headStatus 
                        sControl.devinfo.devProv.devProvisionCopy(spcDev);
                        sControl.setDeviceName(spcDev.strDevName);
                     //   Debug.WriteLine("check device moudle spcdev {0}", spcDev.pDeviceID);

                    }
                    if (sControl.devinfo.devModuleType == Module_Type.Mod_LAN || sControl.devinfo.devModuleType == Module_Type.Mod_CLIV)
                    {

                        if (sControl.devinfo.lineIndex == CUlitity.lineindexOfDevID(spcDev.pDeviceID))
                        {
                            sControl.setHeaderStatus(true);
                            Debug.WriteLine("lan or CLIV check device moudle spcdev {0} modeType {1}", spcDev.pDeviceID, sControl.devinfo.devModuleType);
                        }

                    }

                }

            }



        }

        public List<CDeviceInfo> devList = new List<CDeviceInfo>();

        private IOSqliteOperation iosqlOperator = new IOSqliteOperation();

        public void initAllDeviceModuleHead()
        {
            foreach (UIElement o in this.Children)
            {
                if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                {
                    ComUserMD sControl = (ComUserMD)o;
                    // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                    sControl.setHeaderStatus(false);//clear the headStatus 

                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void saveChildrenToDataBase()
        {
            // CommUser 
            devList.Clear();
            foreach (UIElement o in this.Children)
            {
                if (o.GetType().IsSubclassOf(typeof(ComUserMD)))
                {
                    ComUserMD sControl = (ComUserMD)o;
                    // Debug.WriteLine("iterater module device {0}", sControl.devinfo.devModuleType);
                    sControl.saveDeInfo();
                    devList.Add(sControl.devinfo); //for save the deviceModule position


                }
            }

            iosqlOperator.renewSaveModuleInfo(devList);
        }

        public void readChilrendFromDataBase(ComUserMD.mouseDoubleClick dbClickEvent)
        {
            iosqlOperator.readToDevList(devList);
            loadModule_fromDB(dbClickEvent);
        }
        public void loadModule_fromDB(ComUserMD.mouseDoubleClick dbClickEvent)
        {
            Children.Clear();//first clear all
            ComUserMD module = null;
            if (devList != null && devList.Count > 0)
            {
                foreach (CDeviceInfo dev in devList)
                {
                    switch (dev.devModuleType)
                    {
                        case Module_Type.Mod_MAVIII: //matrix A8
                            module = new CMVIIIModule();
                            break;
                        case Module_Type.Mod_CLIV: //cl-4
                            module = new CSwitcherIVModule();
                            module.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                            module.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                            break;
                        case Module_Type.Mod_LAN: //lan interface
                            module = new CRouterLanModule();
                            module.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                            module.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                            break;
                        case Module_Type.Mod_TxtLft: //text left
                            module = new CLeftTextModule();
                            break;
                        case Module_Type.Mod_TxtRht://text right
                            module = new CRightTextModule();
                            break;
                        case Module_Type.Mod_RVC100:  //RVC200 
                            {
                                module = new CRVC200Module();
                                module.setRVAModuleType(ModuleRVAS.MRVC);
                                module.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0x60, 0x4a, 0x7B)));
                                module.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xcc, 0xc1, 0xda)));
                                module.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RVC1000.PNG", UriKind.RelativeOrAbsolute)));
                                module.setBotomCap("RVC1000");
                            }
                            break;
                        case Module_Type.Mod_RVA100:
                            {
                                module = new CRVC200Module();
                                module.setRVAModuleType(ModuleRVAS.MRVA);
                                module.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0xea, 0x6c, 0x0a)));
                                module.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xfd, 0xea, 0xda)));
                                module.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RVC1000.PNG", UriKind.RelativeOrAbsolute)));
                                module.setBotomCap("RVA200");
                            }
                            break;
                        case Module_Type.Mod_RIO100:
                            {
                                module = new CRVC200Module();
                                module.setRVAModuleType(ModuleRVAS.MRIO);
                                module.setHeadColor(new SolidColorBrush(Color.FromArgb(0XFF, 0x45, 0x6F, 0x2B)));
                                module.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RIO200.PNG", UriKind.RelativeOrAbsolute)));
                                module.setBodyColor(new SolidColorBrush(Color.FromArgb(0xff, 0xD7, 0xE4, 0xBD)));
                                module.setBotomCap("RIO200");
                            }
                            break;
                        case Module_Type.Mod_RPM100:
                            {
                                module = new CRVC200Module();
                                module.setRVAModuleType(ModuleRVAS.RPM);
                                module.setBodyImage(new BitmapImage(new Uri("pack://application:,,,/MatrixSystemEditor;component/Resources/RPM200.PNG", UriKind.RelativeOrAbsolute)));
                                module.setBotomCap("RPM200");
                            }
                            break;

                    }

                    this.AddChildren(module, new Point(0, 0), false);
                    module.loadDevInfo(dev);
                    module.onMouseDoubleClickEvent += new ComUserMD.mouseDoubleClick(dbClickEvent);

                }

            }

        }


    }
}
