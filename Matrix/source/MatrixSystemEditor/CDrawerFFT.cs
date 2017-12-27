using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using Lib.Controls;
using System.Threading;
using MatrixSystemEditor.Matrix;
using Lib.Controls.FFTControl;

namespace MatrixSystemEditor
{
    class CDrawerFFT
    {
        FFTDrawer fftControl;
        XChannelFFTDataGenerator chanlDataGenerator;
        public CDrawerFFT(int dEqNum, FFTDrawer fft)
        {
            if (fft == null)
            {
                Debug.WriteLine("FFTDrawer is null @@@@@@@@@");
                MessageBox.Show("fftDrawer cannot be null");
            }

            fftControl = fft;
            chanlDataGenerator = new XChannelFFTDataGenerator(FFTConstaint.CEQ_MAX);
        }



        public void drawEQ(int chindex)
        {
           
            int EQMax = fftControl.CEQNumber;
             Debug.WriteLine("drawEQ chindex is    {0}----------------------------------------------------", chindex);
            if (!fftControl.isSupportMutiLine)
            {
                for (int i = 0; i < EQMax; i++)
                {
                    byte bypas = CMatrixData.matrixData.m_ChanelEdit[chindex].m_eqEdit[i].eq_byPass;

                    if (chindex < 12 && i>4 && i<8)
                        bypas = 1;
                    if (bypas == 0)
                        chanlDataGenerator.setEQParam(CMatrixData.matrixData.paramOfEQ(i, chindex), i, fftControl);
                    else
                        chanlDataGenerator.setEQParam(CMatrixData.matrixData.paramOfFlatEQ(i, chindex), i, fftControl);
                }

            }         

            chanlDataGenerator.setFFTAdapter(fftControl);
            fftControl.Redraw();

        }

    }

    class FBCDrawFFT
    {
        
        FBCFFTDraw fftControl;

        XFBCFFTDataGenerator FBCFFTGenerator;
        public FBCDrawFFT(FBCFFTDraw fft)
        {
            if (fft == null)
            {
                Debug.WriteLine("FFTDrawer is null @@@@@@@@@");
                MessageBox.Show("fftDrawer cannot be null");
            }

            fftControl = fft;
            FBCFFTGenerator = new XFBCFFTDataGenerator();
        }

        public void drawEQ()
        {
            Array.Copy(CMatrixData.matrixData.m_FbcFilterStatus, fftControl.m_FlatFlag, 24);        
            for (int i = 0; i < FFTConstaint.FBC_FFTNum; i++)
                {
                
                   // byte bypas = CMatrixData.m_fbcEQData[i].eq_byPass;
                  //  if (bypas == 0)
                        FBCFFTGenerator.setEQParam(CMatrixData.matrixData.paramOfFBCEQ(i), i, fftControl);
                 //   else
                        
                     //   FBCFFTGenerator.setEQParam(CMatrixData.matrixData.paramOfFBCFlatEQ(i), i, fftControl);
               }

            FBCFFTGenerator.setFBCFFTAdapter(fftControl);
            fftControl.Redraw();

        }

    }

}
