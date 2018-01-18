using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.Matrix
 *文件名：  CMatrixDefine
 *版本号：  V1.0.0.0
 *唯一标识：c3defb58-49e5-44bd-a991-a467ae1eb03a
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/9/2016 7:49:53 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/9/2016 7:49:53 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace Lib.Controls
{

    public class CAutoMixerParam
    {
        public byte autoPower;
        public int autoAttack;
        public int autoRelease;
        public byte autoHavgTau;//i don't know of it

        public CAutoMixerParam()
        {
            autoPower = 0;
            autoAttack = 0;
            autoRelease = 0;
            autoHavgTau = 0;
        }

        /// <summary>
        /// copy Param now..
        /// </summary>
        /// <param name="aparam"></param>
        public void copyParm(CAutoMixerParam aparam)
        {
            autoPower = aparam.autoPower;
            autoAttack = aparam.autoAttack;
            autoRelease = aparam.autoRelease;
            autoHavgTau = aparam.autoHavgTau;
        }


    }

    public class DuckerParameter
    {
        public byte threshod;
        public int attack;
        public int release;
        public byte depth;
        public int hold;
        public byte bypas;

        /// <summary>
        /// construct duckerparameter
        /// </summary>
        public DuckerParameter()
        {
            flatDucker();
        }

        public void flatDucker()
        {
            threshod = 0;
            attack = 0;
            release = 0;
            depth = 0;
            hold = 0;
            bypas = 0;
        }


    }


    public class EQEdit
    {
        public byte eq_byPass;
        public byte eq_Filterindex;//peak,high/lowshelf
        public byte eq_qfactorindex;

        public int  eq_gainindex; //20161230 by william
        public int eq_freqindex; //high/low /100
        public EQEdit()
        {
            clearData();
        }

        public void copyEQ(EQEdit edata)
        {
            eq_byPass = edata.eq_byPass;
            eq_Filterindex = edata.eq_Filterindex;
            eq_qfactorindex = edata.eq_qfactorindex;
            eq_gainindex = edata.eq_gainindex;
            eq_freqindex = edata.eq_freqindex;
        }

        public void clearData()
        {
            eq_byPass = 0;
            eq_Filterindex = 0;//peak,high/lowshelf
            eq_qfactorindex = 0;
            eq_gainindex = 0;
            eq_freqindex = 0; //high/low /100

        }
        /// <summary>
        /// showEQ editor
        /// </summary>
        public void showEQEdit(int eindex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("eqindex is " + eindex.ToString());

            string temp = string.Format("  eq bypass {0} \t", eq_byPass);
            builder.Append(temp);

            temp = string.Format("eq filter :{0}\t", eq_Filterindex);
            builder.Append(temp);

            temp = string.Format("eq qfactor :{0}\t", eq_qfactorindex);
            builder.Append(temp);

            temp = string.Format("eq gain :{0}\t", eq_gainindex);
            builder.Append(temp);

            temp = string.Format("eq freq :{0}\t", eq_freqindex);
            builder.Append(temp);
            Debug.WriteLine(builder.ToString());
        }

    };
    public class LimitEdit
    {
        public byte limit_threshold;
        public byte limit_ratio;
        public int limit_attack;
        public int limit_release;
        public byte limit_bypas;
        public byte limit_gain;

        public LimitEdit()
        {
            clearData();
        }

        public void clearData()
        {
            limit_threshold = 0;
            limit_ratio = 0;
            limit_attack = 0;
            limit_release = 0;
            limit_bypas = 0;
            limit_gain = 0;
        }
        /// <summary>
        /// copy limit data from here
        /// </summary>
        /// <param name="ldata"></param>
        public void copyLimitData(LimitEdit ldata)
        {
            if (ldata != null)
            {
                limit_attack = ldata.limit_attack;
                limit_bypas = ldata.limit_bypas;
                limit_gain = ldata.limit_gain;
                limit_ratio = ldata.limit_ratio;
                limit_release = ldata.limit_release;
                limit_threshold = ldata.limit_threshold;
            }


        }


        /// <summary>
        /// 
        /// </summary>
        public void showLimitInformation(int chindex=0)
        {

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("limit channelindex {0} print:\t", chindex);

            string temp = string.Format("dyn threshold index {0} \t", limit_threshold);
            builder.Append(temp);

            temp = string.Format("dyn ratio index is :{0}\t", limit_ratio);
            builder.Append(temp);

            temp = string.Format("dyn attack index is :{0}\t", limit_attack);
            builder.Append(temp);

            temp = string.Format("dyn release index is :{0}\t", limit_release);
            builder.Append(temp);

            temp = string.Format("dyn bypass index is :{0}\t", limit_bypas);
            builder.Append(temp);

            temp = string.Format("dyn gain index is :{0}\t", limit_gain);
            Debug.WriteLine(builder.ToString());
        }

    };


    public class CHEdit
    {
        public byte invert;//invt ploarity
        public byte chGain;
        public byte chMute;
        public byte delayPower;
        public int delayTime;
        public byte sensitivityindex;
        public byte DC48VFlag;
        //
        //EQ class
        public EQEdit[] m_eqEdit;//10 seg eq
        public byte eqAll_bypas;

        public LimitEdit gateExpData;
        public LimitEdit dynLimitData;

        private void initChanelData()
        {
            invert = 0;
            chGain = 0;
            chMute = 0;
            delayPower = 0;
            delayTime = 0;
            eqAll_bypas = 0;
            sensitivityindex = 0;//0dB
            DC48VFlag = 0;
        }

        /// <summary>
        /// construct class
        /// </summary>
        /// <param name="eqNum"></param>
        public CHEdit(int eqNum) //total eq num
        {

            m_eqEdit = new EQEdit[eqNum];
            for (int i = 0; i < eqNum; i++)
            {
                m_eqEdit[i] = new EQEdit();
            }
            gateExpData = new LimitEdit();
            dynLimitData = new LimitEdit();
            initChanelData();
        }

        public void clearData()
        {
            initChanelData();
            for (int i = 0; i < m_eqEdit.Length; i++)
            {
                m_eqEdit[i].clearData();
            }
            gateExpData.clearData();
            dynLimitData.clearData();
        }
        ///
        public void showGain_delay_phase_mute()
        {

            StringBuilder builder = new StringBuilder();
            string temp = string.Format("chanel gain {0} \t", chGain);
            builder.Append(temp);
            //
            temp = string.Format("chanel mute {0} \t", chMute);
            builder.Append(temp);
            //
            temp = string.Format("chanel delayPower {0} \t", delayPower);
            builder.Append(temp);
            //
            temp = string.Format("chanel delaytime {0} \t", delayTime);
            builder.Append(temp);
            //
            temp = string.Format("chanel phase {0} \t", invert);
            builder.Append(temp);
            //
            temp = string.Format("chanel eqall_bypass {0} \t", eqAll_bypas);
            builder.Append(temp);

        }

        public void showChanelInfo()
        {
            for (int i = 0; i < m_eqEdit.Length; i++)
            {
                m_eqEdit[i].showEQEdit(i);
            }
            gateExpData.showLimitInformation();
            dynLimitData.showLimitInformation();
            showGain_delay_phase_mute();
        }






    };





    public class CommomDefine
    {




    };
}
