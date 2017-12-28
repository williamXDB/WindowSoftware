using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Lib.Controls.FFTControl;
namespace Lib.Controls
{
    public class EQParam
    {
        public double Freq { get; set; }
        public double Gain { get; set; }
        public double QValue { get; set; }
        public int TypeFilter { get; set; }
        public byte ByPass { get; set; }
        public EQParam()
        {

        }
        public EQParam(double freq, double gain, double qvalue, int typef, byte bypass)
        {
            Freq = freq;
            Gain = gain;
            QValue = qvalue;
            TypeFilter = typef;
            ByPass = bypass;
        }
        public void setEQParam(double freq, double gain, double qvalue, int typef, byte bypass)
        {
            Freq = freq;
            Gain = gain;
            QValue = qvalue;
            TypeFilter = typef;
            ByPass = bypass;
        }

        public void showEQParam()
        {
            StringBuilder strParam = new StringBuilder();
            strParam.AppendFormat("param freq :{0}", Freq);
            strParam.AppendFormat("\tparam Gai :{1}", Gain);
            strParam.AppendFormat("\tparam Qvalue :{2}", QValue);
            strParam.AppendFormat("\tparam typfilter :{3}", TypeFilter);
            strParam.AppendFormat("\tparam bypass :{4}", ByPass);
            Debug.WriteLine("param print : " + strParam.ToString());
        }


    }
    /// <summary>
    /// 
    /// for XFBCFFT Draw Generator
    /// </summary>
    public class XFBCFFTDataGenerator
    {
        #region FBCFFT data define
        private const double DefaultEQffCofA = 0;
        private const int FBCEQMax = FFTConstaint.FBC_FFTNum;
        private  EQParam[] m_EQParam = new EQParam[FBCEQMax];       
        private  double[] gEQData = new double[FFTConstaint.NFFT];
        #endregion


        public XFBCFFTDataGenerator()  //10 segment EQ
        {
            //0:HPF,1..8 EQ 9:LPF
            if(gEQData==null)
                 gEQData = new double[FFTConstaint.NFFT];
            if(m_EQParam==null)
                m_EQParam = new EQParam[FBCEQMax];

            for (int i = 0; i < FBCEQMax; i++)
            {
                m_EQParam[i] = new EQParam();               
            }


        }
        /// <summary>
        /// fbc eq setparam
        /// </summary>
        /// <param name="param"></param>
        /// <param name="eindex"></param>
        /// <param name="fftD"></param>
        public void setEQParam(EQParam param, int eindex, FBCFFTDraw fftD) //[0..23]
        {
            if (eindex < FBCEQMax)
            {
                m_EQParam[eindex] = param;
                fftD.setBlockData(param.Freq, param.Gain, eindex);               
            }
        }
        public void clearEQData()
        {
            Array.Clear(gEQData, 0, FFTConstaint.NFFT);           
        }
       
        public void setFBCFFTAdapter(FBCFFTDraw fbcDrawer)
        {
                     
                XCoverFFTCaculate coverfft = new XCoverFFTCaculate();
                clearEQData();
                double[] tmpData=new double[FFTConstaint.NFFT];
                for (int j = 0; j < FBCEQMax; j++) //0...23
                {
                    tmpData = coverfft.calculateEQ_FFT(m_EQParam[j].Gain, m_EQParam[j].Freq,
                       m_EQParam[j].QValue, m_EQParam[j].TypeFilter, DefaultEQffCofA, DefaultEQffCofA);

                    for (int i = 0; i < FFTConstaint.NFFT; i++)
                    {
                        gEQData[i] += tmpData[i];
                    }
                    Array.Clear(tmpData,0,FFTConstaint.NFFT);
                }             
             
                fbcDrawer.setGlobalEQData(gEQData);             
       
         
        }

    }


    /// <summary>
    /// for FFTChannel Draw Generator
    /// </summary>
    public class XChannelFFTDataGenerator
    {
        private const double DefaultEQffCofA = 0;
        private const int EQMax = FFTConstaint.CEQ_MAX;
        private static EQParam[] m_EQParam = new EQParam[FFTConstaint.CEQ_MAX * 2];
        private static double[][] s_EQData = new double[FFTConstaint.CEQ_MAX * 2][];
        private static double[] gEQData = new double[FFTConstaint.NFFT];
        private static double[] gEQDataII = new double[FFTConstaint.NFFT];

        public int EQNumber { get; set; }
        public XChannelFFTDataGenerator(int EQNum)  //10 segment EQ
        {
            //0:HPF,1..8 EQ 9:LPF
            EQNumber = EQNum;
            for (int i = 0; i < EQNum * 2; i++)
            {
                m_EQParam[i] = new EQParam();
                s_EQData[i] = new double[FFTConstaint.NFFT];
            }


        }

        public void setEQParam(EQParam param, int eindex, FFTDrawer fftD) //[0..19]
        {
            if (eindex < EQNumber * 2)
            {
                m_EQParam[eindex] = param;
              //  param.QValue
                fftD.setBlockData(param.Freq, param.Gain, param.QValue,eindex);
                fftD.setCureFlat((param.ByPass == 1), eindex, false);
            }
        }
        public void clearWaveEQData()
        {
            Array.Clear(gEQData, 0, FFTConstaint.NFFT);
            Array.Clear(gEQDataII, 0, FFTConstaint.NFFT);
            for (int k = 0; k < EQMax * 2; k++)
            {
                Array.Clear(s_EQData[k], 0, FFTConstaint.NFFT);
            }
        }
        private static Object stateObject = new Object();
        public void setFFTAdapter(FFTDrawer fftDrawer)
        {
            //   int chindex = 0;
            bool isHasHLPF = fftDrawer.isHasHLPF;
            bool isSupportDoubleLine = fftDrawer.isSupportMutiLine;
            // lock (stateObject)
            //   {
            //  new Thread(() =>
            {
                XCoverFFTCaculate coverfft = new XCoverFFTCaculate();
                clearWaveEQData();
                for (int j = 0; j < EQNumber - 2; j++) //0...7
                {
                    s_EQData[j] = coverfft.calculateEQ_FFT(m_EQParam[j].Gain, m_EQParam[j].Freq,
                       m_EQParam[j].QValue, m_EQParam[j].TypeFilter, DefaultEQffCofA, DefaultEQffCofA);

                    for (int i = 0; i < FFTConstaint.NFFT; i++)
                    {
                        gEQData[i] += s_EQData[j][i];
                    }
                }
                if (isHasHLPF)
                {
                    //HPF by pass filter  //HPLFGlags  0 high pass  1:low pass
                    int eindex = EQMax - 2;
                    s_EQData[eindex] = coverfft.calculateHLPF_FFT(0, (byte)m_EQParam[eindex].TypeFilter, m_EQParam[eindex].Freq);

                    for (int i = 0; i < FFTConstaint.NFFT; i++)
                    {
                        gEQData[i] += s_EQData[eindex][i];

                    }
                    //low by pass filter
                    eindex = EQMax - 1;
                    s_EQData[eindex] = coverfft.calculateHLPF_FFT(1, (byte)m_EQParam[eindex].TypeFilter, m_EQParam[eindex].Freq);
                    for (int i = 0; i < FFTConstaint.NFFT; i++)
                    {
                        gEQData[i] += s_EQData[eindex][i];
                    }
                }
                //sencond channel 1 chindex = 1;                    
                if (isSupportDoubleLine)
                {
                    for (int j = FFTConstaint.CEQ_MAX; j < EQNumber * 2 - 2; j++) //[10...17]
                    {
                        s_EQData[j] = coverfft.calculateEQ_FFT(m_EQParam[j].Gain, m_EQParam[j].Freq,
                           m_EQParam[j].QValue, m_EQParam[j].TypeFilter, DefaultEQffCofA, DefaultEQffCofA);

                        for (int i = 0; i < FFTConstaint.NFFT; i++)
                        {
                            gEQDataII[i] += s_EQData[j][i];

                        }

                    }
                    if (isHasHLPF)
                    {
                        //HPF by pass filter
                        int eindex = EQMax * 2 - 2;

                        s_EQData[eindex] = coverfft.calculateHLPF_FFT(0, (byte)m_EQParam[eindex].TypeFilter, m_EQParam[eindex].Freq);

                        for (int i = 0; i < FFTConstaint.NFFT; i++)
                        {
                            gEQDataII[i] += s_EQData[eindex][i];

                        }
                        //low by pass filter
                        eindex = EQMax * 2 - 1;
                        s_EQData[eindex] = coverfft.calculateHLPF_FFT(1, (byte)m_EQParam[eindex].TypeFilter, m_EQParam[eindex].Freq);

                        for (int i = 0; i < FFTConstaint.NFFT; i++)
                        {
                            gEQDataII[i] += s_EQData[eindex][i];
                        }
                    }
                }

                //GUI BeginInvoke is Asynchronously but Invoke is synchronously
                // mWnd.Dispatcher.Invoke(DispatcherPriority.Normal, new System.Action(delegate()
                //{
                for (int k = 0; k < EQMax * 2; k++)
                {
                    fftDrawer.set_CurveEQData(k, s_EQData[k]);
                }
                fftDrawer.setGlobalEQData(gEQData, 0);
                fftDrawer.setGlobalEQData(gEQDataII, 1);
                //    }));
                //  }).Start();
                //}


            }
            //  }
        }
    }
}


