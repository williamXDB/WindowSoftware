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


/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.commom
 *文件名：  ComUserMD
 *版本号：  V1.0.0.0
 *唯一标识：bdd6a293-d59c-419f-9d70-3f9d3c67e8e9
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/17/2016 7:29:31 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/17/2016 7:29:31 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 * 对于usercontrol 继承的这种，必须使用不同的name，比如CLeftText
/************************************************************************************/


namespace Lib.Controls
{

    /*
     ID type list below:
     *   Left/Right Text devID:0x5000,0x5001
     *   LanRouter:0x5002
     *   cl-IV Switcher:0x5003
     *   
     *   MatrixA8 or later from 0x1000 +
     *   
     * 
     */
    public class ComUserMD : UserControl
    {


        #region property define-------------------------------

#if DoduleTypeDefine
        private const Module_Type DefaultMType = Module_Type.Mod_CLIV;
        public Module_Type ModuleType
        {
            get { return (Module_Type)GetValue(ModuleTypeProperty); }
            set { SetValue(ModuleTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModuleType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModuleTypeProperty =
            DependencyProperty.Register("ModuleType", typeof(Module_Type), typeof(ComUserMD),
            new PropertyMetadata(DefaultMType));
        //---------------------------------   
#endif
        #endregion





        private static Color DefaultUnColor = Color.FromRgb(0xC0, 0xC0, 0xC0);

        public Color unConHeadColor
        {
            get { return (Color)GetValue(unConHeadColorProperty); }
            set { SetValue(unConHeadColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for unConHeadColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty unConHeadColorProperty =
            DependencyProperty.Register("unConHeadColor", typeof(Color), typeof(ComUserMD), new PropertyMetadata(DefaultUnColor));


        private static SolidColorBrush DefaultHeadColor = new SolidColorBrush(Color.FromRgb(0x25, 0x40, 0x61)); //green color
        public SolidColorBrush HeadColor
        {
            get { return (SolidColorBrush)GetValue(HeadColorProperty); }
            set { SetValue(HeadColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeConHeadColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadColorProperty =
            DependencyProperty.Register("HeadColor", typeof(SolidColorBrush), typeof(CRVC200Module), new PropertyMetadata(DefaultHeadColor));


        private static Color DefaultActColor = Color.FromRgb(0, 0xFF, 0); //green color
        public Color activeConHeadColor
        {
            get { return (Color)GetValue(activeConHeadColorProperty); }
            set { SetValue(activeConHeadColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for activeConHeadColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty activeConHeadColorProperty =
            DependencyProperty.Register("activeConHeadColor", typeof(Color), typeof(ComUserMD), new PropertyMetadata(DefaultActColor));




        public CDeviceInfo devinfo;

        public void setDevX(double x)
        {
            devinfo.DevPoint.X = x;
            Canvas.SetLeft(this, x);

        }
        public void setDevY(double y)
        {
            devinfo.DevPoint.Y = y;
            Canvas.SetTop(this, y);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mid"></param>
        public virtual void setDeviceID(UInt16 mid)
        {


        }
        /// <summary>
        ///         /// 
        /// </summary>
        /// <param name="pt"></param>
        public virtual void setDeviceName(string strName)
        {


        }



        public void setDevPoint(Point pt)
        {
            devinfo.DevPoint = pt;
            Canvas.SetLeft(this, pt.X);
            Canvas.SetTop(this, pt.Y);
        }

        #region virutal function
        public virtual void saveDeInfo()
        {
            devinfo.DevPoint.X = Canvas.GetLeft(this);
            devinfo.DevPoint.Y = Canvas.GetTop(this);

        }
        public virtual void setRouterAddr(String strAddr)
        {

        }

        public virtual void setBotomCap(string strCap)
        {

        }

        public virtual void setHeadColor(SolidColorBrush scb)
        {

        }
        public virtual void setBodyColor(SolidColorBrush scb)
        {

        }

        public virtual void setBodyImage(ImageSource imgsrc)
        {

        }
        public virtual void setRVAModuleType(ModuleRVAS modRva)
        {

        }

        private bool headStatus = false;

        public virtual void setHeaderStatus(bool status)
        {
            headStatus = status;
        }



        public virtual void loadDevInfo(CDeviceInfo devf) //load dev into it
        {
            devinfo.copyDevinfo(devf);
            setDevX(devinfo.DevPoint.X);
            setDevY(devinfo.DevPoint.Y);
        }
        #endregion


        public ComUserMD()
        {
            if (devinfo == null)
            {
                devinfo = new CDeviceInfo();
            }
            devinfo.devProv.pDeviceID = 0x1000;
            devinfo.devProv.strDevName = "DeviceID";



        }
        #region clickEvent below

        public delegate void mouseDoubleClick(Object sender);
        public event mouseDoubleClick onMouseDoubleClickEvent;

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (onMouseDoubleClickEvent != null)
            {
                onMouseDoubleClickEvent(this);
            }

        }


        #endregion

        public void deleteModule(object sender)
        {
            var grid2 = (Grid)ContextMenuService.GetPlacementTarget(
                 LogicalTreeHelper.GetParent(sender as MenuItem));
            ComUserMD mod;
            if (Convert.ToInt16(grid2.Tag) == 100)
            {
                var aaa = (Border)LogicalTreeHelper.GetParent(grid2);
                mod = (ComUserMD)LogicalTreeHelper.GetParent(aaa);
            }
            else
            {
                mod = (ComUserMD)LogicalTreeHelper.GetParent(grid2);
            }
            var cvs = (Canvas)LogicalTreeHelper.GetParent(mod);
            cvs.Children.Remove(mod);

        }



    }
}
