using System;
using System.Collections.Generic;
using System.Globalization;
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
 *命名空间：DrawDemo
 *文件名：  PainterText
 *版本号：  V1.0.0.0
 *唯一标识：1ba03d05-ad72-4a31-9dae-b4597892a677
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：9/28/2016 2:14:48 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：9/28/2016 2:14:48 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{
    public class PainterText : Control
    {

        public static string[] DrawText ={    
         "  0",
         " -5",
         "-10",
         "-15",
         "-20",
         "-25",
         "-30",
         "-35",
         "-40" 
    
        };

        static PainterText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PainterText), new FrameworkPropertyMetadata(typeof(PainterText)));
        }

        public PainterText()
        {
            //  Color

        }
        public FormattedText getFormatStr(string str, int fontSize, Brush mbrush)
        {
            FormattedText formatText = new FormattedText(str,
                          System.Globalization.CultureInfo.CurrentCulture,
                          System.Windows.FlowDirection.LeftToRight,
                          new Typeface(new FontFamily("Arial").ToString()),
                          fontSize, mbrush);

            return formatText;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            for (int i = 0; i < DrawText.Length; i++)
            {

                string str = DrawText[i];
                FormattedText fStr = getFormatStr(str, 8, Brushes.Black);
                dc.DrawText(fStr, new Point(xoffset, i * fontGap));

            }

        }



        private static  double DefaultXOffset = 1.0;
        public double xoffset
        {
            get { return (double)GetValue(xoffsetProperty); }
            set { SetValue(xoffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for xoffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty xoffsetProperty =
            DependencyProperty.Register("xoffset", typeof(double), typeof(PainterText), 
            new FrameworkPropertyMetadata(DefaultXOffset, FrameworkPropertyMetadataOptions.None, onTextXoffsetChange
                ));         


        private static void onTextXoffsetChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var painter = obj as PainterText;
            if (painter != null)
            {
                painter.xoffset = (double)args.NewValue;             
                painter.redraw();
            }
            
        }


        private static double DefaultFontGap = 15;
        public double fontGap
        {
            get { return (double)GetValue(fontGapProperty); }
            set { SetValue(fontGapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fontGap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fontGapProperty =
            DependencyProperty.Register("fontGap", typeof(double), typeof(PainterText),
            new FrameworkPropertyMetadata(DefaultFontGap, FrameworkPropertyMetadataOptions.None, onTextGapChange
                ));  
            

        private static void onTextGapChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var painter = obj as PainterText;
            if (painter != null)
            {               
                painter.fontGap = (double)args.NewValue;
                painter.redraw();
            }

        }


        private static int DefaultFontSize = 8;
        public int fontSizeOfDraw
        {
            get { return (int)GetValue(fontSizeOfDrawProperty); }
            set { SetValue(fontSizeOfDrawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fontSizeOfDraw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fontSizeOfDrawProperty =
            DependencyProperty.Register("fontSizeOfDraw", typeof(int), typeof(PainterText), 
            new FrameworkPropertyMetadata(DefaultFontSize, FrameworkPropertyMetadataOptions.None, onFontValueChange
                ));     


        private static void onFontValueChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var painter = obj as PainterText;
            if (painter != null)
            {
                painter.fontSizeOfDraw = (int)args.NewValue;
                painter.redraw();                
            }


        }
        //
        public void redraw()
        {
            //  Debug.WriteLine("readraw.....");
            InvalidateVisual();
        }


    }
}
