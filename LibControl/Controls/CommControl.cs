using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：HelloMove
 *文件名：  CommControl
 *版本号：  V1.0.0.0
 *唯一标识：53749f43-2a0e-486e-bba5-dcaeebe30a79
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/30/2016 9:54:34 AM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/30/2016 9:54:34 AM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class CommControl : Control
    {
        static CommControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommControl), new FrameworkPropertyMetadata(typeof(CommControl)));
        }

        public int limitValue(int mva,int nMax,int nMin)
        {
            return Math.Max(nMin - 1, Math.Min(nMax + 1, mva));
        }
        //
        //------
        #region property define
        private const int DefaultTag = 0;
        public int iTag
        {
            get { return (int)GetValue(iTagProperty); }
            set { SetValue(iTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for iTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iTagProperty =
            DependencyProperty.Register("iTag", typeof(int), typeof(CommControl), new PropertyMetadata(DefaultTag));


        private static SolidColorBrush DefaultBackBrush = Brushes.Gray;

        public SolidColorBrush BackBrush
        {
            get { return (SolidColorBrush)GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(SolidColorBrush), typeof(CommControl),
             new FrameworkPropertyMetadata(DefaultBackBrush, FrameworkPropertyMetadataOptions.None, onBackBrushChange
                ));

        private static void onBackBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var mcontrol = obj as CommControl;
            if (mcontrol != null)
            {
                mcontrol.BackBrush = (SolidColorBrush)args.NewValue;
                mcontrol.Redraw();
            }


        }
        #endregion

        public void Redraw()
        {
            
            InvalidateVisual();
        }
       

    }
}
