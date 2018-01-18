using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lib.Controls
{

    public class XCoverFFTCaculate
    {
        public double MatsterGain
        {
            get;
            set;
        }
        // private static double[] PointArray = new double[FFTConstaint.NFFT];
        private static double[] a = new double[3];
        private static double[] b = new double[3];
        private const double DefaultHLGain = 0.0;
        private const double DefaultQValue = 0.05;

        private const int cENum = 6;
        private  double[][] butterWorthAray = new double[cENum][];

        public XCoverFFTCaculate()
        {            
           for(int i=0;i<cENum;i++)
           {
               butterWorthAray[i]=new double[FFTConstaint.NFFT];
           }
        }

        Point addc(Point x, Point y) //double ,so must add reference system.windows
        {
            Point z = new Point(0.0, 0.0);
            z.X = x.X + y.X;
            z.Y = x.Y + y.Y;
            return z;
        }

        double abss(Point x)
        {
            double res = 0.0;
            res = Math.Pow(x.X, 2) + Math.Pow(x.Y, 2);
            res = Math.Sqrt(res);
            return res;
        }


        // ------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Gain"></param>
        /// <param name="Frequency"></param>
        /// <param name="QFactor"></param>
        /// <param name="FiltType"></param>
        /// <param name="fCoeffA"></param>
        /// <param name="fCoeffB"></param>
        /// <returns></returns>
        private bool PeakShlv(double Gain, double Frequency, double QFactor, double FiltType, double fCoeffA, double fCoeffB)
        {

            double k, v0;
            double Den, Den1, Den2;
            Array.Clear(a, 0, 3);
            Array.Clear(b, 0, 3);

            k = Math.Tan(FFTConstaint.PI * Frequency / FFTConstaint.SAMPLE_RATE);
            if (FiltType == 1)
            {
                if (Gain >= 0) // Peak with positive gain
                {
                    v0 = Math.Pow(10.0, Gain / 20.0);
                    Den = 1 + (k / QFactor) + k * k;
                    a[0] = (1 + (v0 * k / QFactor) + k * k) / Den;
                    a[1] = (2 * (k * k - 1)) / Den;
                    a[2] = (1 - (v0 * k / QFactor) + k * k) / Den;
                    b[0] = 1;
                    b[1] = a[1];
                    b[2] = (1 - (k / QFactor) + k * k) / Den;
                }
                else // Peak with negative gain
                {
                    try
                    {
                        v0 = Math.Pow(10.0, -Gain / 20.0);
                    }
                    catch (Exception e)
                    {
                        v0 = 0.0;

                    }
                    Den = 1 + (v0 * k / QFactor) + k * k;
                    a[0] = (1 + k / QFactor + k * k) / Den;
                    a[1] = (2 * (k * k - 1)) / Den;
                    a[2] = (1 - k / QFactor + k * k) / Den;
                    b[0] = 1;
                    b[1] = a[1];
                    b[2] = (1 - (v0 * k / QFactor) + k * k) / Den;
                }
            }
            else if (FiltType == 4) // Low-pass filter (Butterworth)
            {
                Den = 1 + Math.Sqrt(2.0) * k + k * k;
                a[0] = (k * k) / Den;
                a[1] = (2 * k * k) / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (k * k - 1)) / Den;
                b[2] = (1 - Math.Sqrt(2.0) * k + k * k) / Den;
            }
            else if (FiltType == 5) // High-pass filter (Butterworth)
            {
                Den = 1 + Math.Sqrt(2.0) * k + k * k;
                a[0] = 1 / Den;
                a[1] = -2 / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (k * k - 1)) / Den;
                b[2] = (1 - Math.Sqrt(2) * k + k * k) / Den;
            }
            else if (FiltType == 6) // Band-pass filter
            {
                Den = QFactor + k + QFactor * k * k;
                a[0] = 2 * k / Den;
                a[1] = 0;
                a[2] = -a[0];
                b[0] = 1;
                b[1] = (2 * QFactor * (k * k - 1)) / Den;
                b[2] = (QFactor - k + QFactor * k * k) / Den;
            }
            else if (FiltType == 8) // 1st order Low-pass filter
            {
                Den = 1 + k;
                a[0] = k / Den;
                a[1] = a[0];
                a[2] = 0;
                b[0] = 1;
                b[1] = (k - 1) / Den;
                b[2] = 0;
            }
            else if (FiltType == 9) // 1st order  High-pass filter
            {
                Den = 1 + k;
                a[0] = -1 / Den;
                a[1] = 1 / Den;
                a[2] = 0;
                b[0] = 1;
                b[1] = (k - 1) / Den;
                b[2] = 0;
            }
            else if (FiltType == 10) // Bypass
            {
                a[0] = 1;
                a[1] = 0;
                a[2] = 0;
                b[0] = 1;
                b[1] = 0;
                b[2] = 0;
            }
            else if (FiltType == 11) // Bypass
            {
                // case 0: 1st order Low-pass filter
                Den = 1 + 1 / fCoeffA * k;
                a[0] = 1 / fCoeffA * k / Den;
                a[1] = a[0];
                a[2] = 0;
                b[0] = 1;
                b[1] = (1 / fCoeffA * k - 1) / Den;
                b[2] = 0;
            }
            else if (FiltType == 12)
            {
                // case 1: // Butterworth Low-pass filter
                Den = 1 + fCoeffA * k + k * k;
                a[0] = (k * k) / Den;
                a[1] = (2 * k * k) / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (k * k - 1)) / Den;
                b[2] = (1 - fCoeffA * k + k * k) / Den;
            }
            else if (FiltType == 13)
            {
                // case 2:  Bessel Low-pass filter
                Den = 1 + fCoeffA / fCoeffB * k +
                1 / fCoeffB * k * k;
                a[0] = (1 / fCoeffB * k * k) / Den;
                a[1] = (2 * 1 / fCoeffB * k * k) / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (1 / fCoeffB * k * k - 1)) / Den;
                b[2] = (1 - fCoeffA / fCoeffB * k +
                        1 / fCoeffB * k * k) / Den;
            }
            else if (FiltType == 14)
            {
                // case 3:  1st order  High-pass filter
                Den = 1 + fCoeffA * k;
                a[0] = 1 / Den;
                a[1] = -1 / Den;
                a[2] = 0;
                b[0] = 1;
                b[1] = (fCoeffA * k - 1) / Den;
                b[2] = 0;
            }
            else if (FiltType == 15)
            {
                // case 4:  Butterworth High-pass filter
                Den = 1 + fCoeffA * k + k * k;
                a[0] = 1 / Den;
                a[1] = -2 / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (k * k - 1)) / Den;
                b[2] = (1 - fCoeffA * k + k * k) / Den;
            }
            else if (FiltType == 16)
            {
                // case 5:  Bessel High-pass filter
                Den = 1 + fCoeffA * k + fCoeffB * k * k;
                a[0] = 1 / Den;
                a[1] = -2 / Den;
                a[2] = a[0];
                b[0] = 1;
                b[1] = (2 * (fCoeffB * k * k - 1)) / Den;
                b[2] = (1 - fCoeffA * k + fCoeffB * k * k) / Den;
            }
            else
            {
                if (Gain >= 0)
                {
                    try
                    {
                        v0 = Math.Pow(10.0, Gain / 20.0);
                    }
                    catch (Exception e)
                    {
                        v0 = 0.0;

                    }

                    Den = 1 + Math.Sqrt(2.0) * k + k * k;
                    if (FiltType == 2)
                    { // Low frequency shelving with positive gain
                        k = Math.Tan(FFTConstaint.PI * Frequency / FFTConstaint.SAMPLE_RATE);
                        a[0] = (1 + Math.Sqrt(2 * v0) * k + v0 * k * k) / Den;
                        a[1] = 2 * (v0 * k * k - 1) / Den;
                        a[2] = (1 - Math.Sqrt(2 * v0) * k + v0 * k * k) / Den;
                        b[0] = 1;
                        b[1] = 2 * (k * k - 1) / Den;
                        b[2] = (1 - Math.Sqrt(2.0) * k + k * k) / Den;
                    }
                    else if (FiltType == 3)
                    { // High frequency shelving with positive gain
                        k = Math.Tan(FFTConstaint.PI * Frequency / FFTConstaint.SAMPLE_RATE);
                        a[0] = (v0 + Math.Sqrt(2 * v0) * k + k * k) / Den;
                        a[1] = 2 * (k * k - v0) / Den;
                        a[2] = (v0 - Math.Sqrt(2 * v0) * k + k * k) / Den;
                        b[0] = 1;
                        b[1] = 2 * (k * k - 1) / Den;
                        b[2] = (1 - Math.Sqrt(2.0) * k + k * k) / Den;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        v0 = Math.Pow(10.0, (-(Gain) / 20.0));
                    }
                    catch (Exception e)
                    {
                        v0 = 0.0;
                    }
                    if (FiltType == 2)
                    { // Low frequency shelving with negative gain
                        k = Math.Tan(FFTConstaint.PI * Frequency / FFTConstaint.SAMPLE_RATE);
                        Den = 1 + Math.Sqrt(2 * v0) * k + v0 * k * k;
                        a[0] = (1 + Math.Sqrt(2.0) * k + k * k) / Den;
                        a[1] = 2 * (k * k - 1) / Den;
                        a[2] = (1 - Math.Sqrt(2.0) * k + k * k) / Den;
                        b[0] = 1;
                        b[1] = 2 * (v0 * k * k - 1) / Den;
                        b[2] = (1 - Math.Sqrt(2 * v0) * k + v0 * k * k) / Den;
                    }
                    else if (FiltType == 3)
                    { // High frequency shelving with negative gain
                        k = Math.Tan(FFTConstaint.PI * Frequency / FFTConstaint.SAMPLE_RATE);
                        Den1 = v0 + Math.Sqrt(2 * v0) * k + k * k;
                        Den2 = 1 + Math.Sqrt(2 / v0) * k + (k * k / v0);
                        a[0] = (1 + Math.Sqrt(2.0) * k + k * k) / Den1;
                        a[1] = 2 * (k * k - 1) / Den1;
                        a[2] = (1 - Math.Sqrt(2.0) * k + k * k) / Den1;
                        b[0] = 1;
                        b[1] = 2 * ((k * k / v0) - 1) / Den2;
                        b[2] = (1 - Math.Sqrt(2 / v0) * k + (k * k / v0)) / Den2;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// caculateFFT
        /// </summary>
        /// <param name="gain"></param>s
        /// <param name="freq"></param>
        /// <param name="qfactor"></param>
        /// <param name="type"> 1 is defalut value</param>
        /// <param name="fCoeffA"></param>
        /// <param name="fCoeffB"></param>
        public double[] calculateEQ_FFT(double gain, double freq, double qfactor, int type, double fCoeffA, double fCoeffB)
        {
            // ************************************
            // ****define local parameter**********
            // ************************************
            int i, j, nb = 3, na = 3;
            double[] resArray = new double[FFTConstaint.NFFT];

            Point sumb = new Point(0.0, 0.0);
            Point suma = new Point(0.0, 0.0);
            Point temp = new Point(0.0, 0.0);
            double pr;// dTemp;
            //Array.Clear(PointArray, 0, FFTConstaint.NFFT);
            double[] ww = new double[FFTConstaint.NFFT];
            // ------------------------------------------
            try
            {
                PeakShlv(gain, freq, qfactor, type, fCoeffA, fCoeffB);
                // ------------------------------------------
                // ************FFT*************************

                for (i = 0; i < FFTConstaint.NFFT; i++)
                {
                    ww[i] = Math.Pow(10, Math.Log10(FFTConstaint.SAMPLERATE) * i / FFTConstaint.NFFT) * FFTConstaint.PI / FFTConstaint.SAMPLERATE;

                    sumb.X = b[0]; // sumb=b(1);
                    sumb.Y = 0;
                    suma.X = a[0]; // suma=a(1);
                    suma.Y = 0;

                    for (j = 1; j < nb; j++)
                    {
                        pr = j * ww[i]; // pr=(j-1).*ww(k);
                        temp.X = b[j] * Math.Cos(-pr); // exp(-i*pr)
                        temp.Y = b[j] * Math.Sin(-pr);
                        sumb = addc(sumb, temp); // sumb=sumb+b(j).*exp(-i*pr);
                    }
                    for (j = 1; j < na; j++)
                    {
                        pr = j * ww[i]; // pr=(j-1).*ww(k)
                        temp.X = a[j] * Math.Cos(-pr);
                        temp.Y = a[j] * Math.Sin(-pr); // exp(-i*pr)
                        suma = addc(suma, temp); // suma=suma+a(j).*exp(-i*pr);
                    }
                    // PointArray[i] = -(20.0)*log10(abss(sumb)/abss(suma))-(m_mastergain)*0.0322;
                    if (abss(sumb) / abss(suma) > 0)
                    {
                        resArray[i] = -(20.0) * Math.Log10(abss(sumb) / abss(suma)) - MatsterGain;

                    }

                }


            }
            catch (Exception e)
            {
            }
            return resArray;

        }

        private void clearButterWorthAray()
        {

            for(int i=0;i<cENum;i++)
            {
                Array.Clear(butterWorthAray[i], 0, FFTConstaint.NFFT);
            }

        }

        public double[] calculateHLPF_FFT_Default()
        {
            return calculateEQ_FFT(0, 19.7, 0.05, 1, 0.0, 0.0);
        }

        /// <summary>
        /// process HLpf with freq //HPLFGlags  0 high pass  1:low pass
        /// </summary>
        public double[] calculateHLPF_FFT(byte HPLFlags, byte filter, double freq)   //HPLFGlags  0 high pass  1:low pass
        {

            double[] tempArray = new double[FFTConstaint.NFFT];
            double fCoefA1 = 0.0, fCoefB1 = 0.0;
            HLPassType iFilterType = 0;
            const int DefaultFilter = 1;
            //FFTConstaint.XoverType type;
            //type=(FFTConstaint.XoverType)Enum.ToObject(typeof(FFTConstaint.XoverType), sfilter);         
            //byte filter =(byte)FFTConstaint.getFilterWithType(type);
            clearButterWorthAray();
           // Debug.WriteLine("dispatch with calculateHLPF_FFT  with --------", filter);

            switch (filter)
            {
                case 0://by pass
                    {

                        iFilterType = HLPassType.HLBYPASS;
                        tempArray = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);

                    }
                    break;
                case 1: //butterworth -6dB/oct
                    {
                        if (HPLFlags > 0) iFilterType = HLPassType.LP6dB; //low pass  11
                        else iFilterType = HLPassType.BESLP12dB; //high pass   14

                        fCoefA1 = 0.6;
                        fCoefB1 = 0;
                        tempArray = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);
                    }
                    break;

                case 2:////butterworth 12
                case 3:////butterworth 18
                case 4:////butterworth 24
                case 5://butterworth 30
                case 6://butterworth 36
                case 7://butterworth 42
                case 8://butterworth 48
                    {
                        
                       
                        if (FFTConstaint.fButterWorthCoef[filter - 1, 0] == 1)
                        {
                            fCoefA1 = 1;
                            fCoefB1 = 0;

                            iFilterType = (HPLFlags > 0 ? HLPassType.LP6dB : HLPassType.BESLP12dB);
                            butterWorthAray[0] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);


                        }
                        else
                        {
                            butterWorthAray[0] = calculateHLPF_FFT_Default();
                        }

                        for (int i = 1; i < 5; i++)
                        {

                            if (FFTConstaint.fButterWorthCoef[filter - 1, i] != 0)
                            {

                                fCoefA1 = FFTConstaint.fButterWorthCoef[filter - 1, i];
                                fCoefB1 = 0;
                                iFilterType = (HPLFlags > 0 ? HLPassType.HP6dB : HLPassType.LKLP12dB);
                                butterWorthAray[i] = calculateEQ_FFT(0.0, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);
                            }
                        }
                        butterWorthAray[5] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, DefaultFilter, fCoefA1, fCoefB1);

                        for (int j = 0; j < cENum; j++)
                        {
                            for (int i = 0; i < FFTConstaint.NFFT; i++)
                            {
                                tempArray[i] += butterWorthAray[j][i];
                            }

                        }


                    }

                    break;

                case 9: //Bessel --6dB/OCT
                case 10: //Bessel --12dB/OCT
                case 11: //Bessel ---18dB/OCT
                case 12: //Bessel --24dB/OCT
                case 13: //Bessel --30db/OCT
                case 14: //Bessel --36B/OCT
                case 15: //Bessel --42dB/OCT
                case 16: //Bessel --48B/OCT
                    {

                        if (FFTConstaint.fBesselCoef[filter - 9, 0, 0] != 0)
                        {

                            fCoefA1 = FFTConstaint.fBesselCoef[filter - 9, 0, 0];
                            fCoefB1 = 0;
                            iFilterType = (HPLFlags > 0 ? HLPassType.LP6dB : HLPassType.BESLP12dB);

                            butterWorthAray[0] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);
                        }

                        for (int i = 1; i < 5; i++)
                        {
                            if (FFTConstaint.fBesselCoef[filter - 9, i, 0] != 0)
                            {
                                fCoefA1 = FFTConstaint.fBesselCoef[filter - 9, i, 0];
                                fCoefB1 = FFTConstaint.fBesselCoef[filter - 9, i, 1];

                                iFilterType = (HPLFlags > 0 ? HLPassType.BWLP12dB : HLPassType.CHVLP12dB);

                                butterWorthAray[i] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);

                            }

                        }
                        butterWorthAray[5] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, DefaultFilter, fCoefA1, fCoefB1);

                        for (int j = 0; j < cENum; j++)
                        {
                            for (int i = 0; i < FFTConstaint.NFFT; i++)
                            {
                                tempArray[i] += butterWorthAray[j][i];
                            }

                        }

                    }

                    break;
                case 17: //link/rikey -12dB/oct
                case 18: //link/Riky -24dB/oct
                    {                 //three points diejia           

                        for (int i = 0; i < 2; i++)
                        {
                            if ((filter - 17) == 0)
                            {
                                fCoefA1 = 0.8;
                                fCoefB1 = 0.0;
                                iFilterType = (HPLFlags > 0 ? HLPassType.LP6dB : HLPassType.BESLP12dB);
                            }
                            else
                            {
                                fCoefA1 = FFTConstaint.fButterWorthCoef[filter - 17, 1];
                                Debug.WriteLine("LK24  fcoefa1 is  {0}", fCoefA1);

                                fCoefB1 = 0;
                                iFilterType = (HPLFlags > 0 ? HLPassType.HP6dB : HLPassType.LKLP12dB);
                            }
                            butterWorthAray[i] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);

                        }
                        for (int i = 2; i < 5; i++)
                            butterWorthAray[i] = calculateHLPF_FFT_Default();
                        butterWorthAray[5] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, DefaultFilter, fCoefA1, fCoefB1);

                        for (int j = 0; j < cENum; j++)
                        {
                            for (int i = 0; i < FFTConstaint.NFFT; i++)
                            {
                                tempArray[i] += butterWorthAray[j][i];
                            }

                        }

                    }
                    break;
                case 19: //Link/Rikey -36dB/oct
                case 20: //Link/Rikey -48dB/oct     4point jiejia
                    {

                        for (int i = 0; i < 2; i++)
                        {
                            if ((filter - 19) == 0)
                            {
                                fCoefA1 = 1.6180;
                                Debug.WriteLine("Link Rikey 36 fcore fA is {0}", fCoefA1);
                                fCoefB1 = 0;
                                iFilterType = (HPLFlags > 0 ? HLPassType.LP6dB : HLPassType.BESLP12dB);


                            }
                            else
                            {
                                fCoefA1 = FFTConstaint.fButterWorthCoef[filter - 17, 2];
                                fCoefB1 = 0;
                                Debug.WriteLine("Link Rikey 48 fcore fA is {0}", fCoefA1);

                                iFilterType = (HPLFlags > 0 ? HLPassType.HP6dB : HLPassType.LKLP12dB);
                            }
                            butterWorthAray[i] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, (int)iFilterType, fCoefA1, fCoefB1);
                        }
                        //
                        fCoefA1 = FFTConstaint.fButterWorthCoef[filter - 17, 1];
                        fCoefB1 = 0;
                        iFilterType = (HPLFlags > 0 ? HLPassType.HP6dB : HLPassType.LKLP12dB);
                        butterWorthAray[4] = calculateHLPF_FFT_Default();
                        butterWorthAray[3] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, DefaultFilter, fCoefA1, fCoefB1);
                        butterWorthAray[5] = calculateEQ_FFT(DefaultHLGain, freq, DefaultQValue, DefaultFilter, fCoefA1, fCoefB1);

                        for (int j = 0; j < cENum; j++)
                        {
                            for (int i = 0; i < FFTConstaint.NFFT; i++)
                            {
                                tempArray[i] += butterWorthAray[j][i];
                            }
                        }
                    }
                    break;

            }
            return tempArray;
        }


    }





}
