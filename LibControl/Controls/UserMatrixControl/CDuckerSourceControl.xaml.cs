﻿using System;
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
    /// Interaction logic for CFBCSourceControl.xaml
    /// </summary>
    public partial class CDuckerSourceControl : UserControl
    {
        public byte[] m_DuckerSourch = new byte[CMatrixFinal.Max_MatrixChanelNum];
        public byte[] m_DuckerBgmLocal = new byte[CMatrixFinal.Max_MatrixChanelNum*2];
      
        /// <summary>
        /// 
        /// </summary>
        public CDuckerSourceControl()
        {
            InitializeComponent();
            resetData();
        }

        public delegate void DuckerValueControlChanged(Object sender,int flag);
        public event DuckerValueControlChanged onDuckerValueControlChangedEvent;


        public void resetData()
        {
            if (m_DuckerSourch != null)
                m_DuckerSourch = new byte[CMatrixFinal.Max_MatrixChanelNum];
            if (m_DuckerBgmLocal != null)
                m_DuckerBgmLocal = new byte[CMatrixFinal.Max_MatrixChanelNum*2];

            Array.Clear(m_DuckerSourch, 0, CMatrixFinal.Max_MatrixChanelNum);
            Array.Clear(m_DuckerBgmLocal, 0, CMatrixFinal.Max_MatrixChanelNum * 2);
           
        }

        private void bgmNetWorkClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerBgmLocal[index + CMatrixFinal.Max_MatrixChanelNum] = tmp;
           
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this,1);
            }

        }

        private void bgmLocalClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerBgmLocal[index] = tmp;
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this,1);
            }

        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceLocalClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as CSwitcher;
            int index = btn.iTag;
            byte tmp = 0;
            if (btn.IsSelected)
            {
                tmp = 0;
            }
            else
            {
                tmp = 1;
            }
            m_DuckerSourch[index] = tmp;
            //
            if (onDuckerValueControlChangedEvent != null)
            {
                onDuckerValueControlChangedEvent(this,0);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void refreshControl()
        {
            CSwitcher sbtn = null;
            string strBtn = "";

            //source select part
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)
            {
                strBtn = string.Format("sourceBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = (m_DuckerSourch[i] > 0);
                }

            }

            //BGM setting part
            for (int i = 0; i < CMatrixFinal.Max_MatrixChanelNum; i++)  //btm
            {
                strBtn = string.Format("bgmLocalBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {

                    sbtn.IsSelected = (m_DuckerBgmLocal[i] > 0);
                }

            }
            for (int i = 0; i < 8; i++)
            {
                strBtn = string.Format("networkBtn{0}", i);
                sbtn = (CSwitcher)this.FindName(strBtn);
                if (sbtn != null)
                {
                    sbtn.IsSelected = (m_DuckerBgmLocal[i + CMatrixFinal.Max_MatrixChanelNum] > 0);
                }

            }
            //bgmLocalBtn,networkBtn

        }




    }
}
