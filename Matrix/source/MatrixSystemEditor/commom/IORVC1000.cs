using Lib.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSystemEditor.commom
{
    [Serializable]
    public class IORVC1000 : IOCommon
    {
        public const int MAX_CH = 12;

        public byte[] m_ChanControlFlag;
        public byte[] m_RoutingEnableFlag;

       public static  string FileFilter = "RVC100 File(*.RVC2)|*.RVC2";
       public static string FileExeName = "RVC2";

        public IORVC1000()
        {
            AppID = AppIDList.AP_RVC_100;
            len_sence = 75 - 18;
            m_ChanControlFlag = new byte[MAX_CH * 2];
            //
            m_RoutingEnableFlag = new byte[MAX_CH];
            len_sence = 75 - 18;   //all sence len pack is :len_sence+18
            sFileFilter = "RVC100 File(*.RVC2)|*.RVC2";
            sFileName = "RVC2";
        }

        public void copy(IORVC1000 mIORV)
        {
            if (mIORV != null)
            {
                this.DevID = mIORV.DevID;
                this.m_McuVer = mIORV.m_McuVer;
                this.sFileFilter = mIORV.sFileFilter;
                this.sFileName = mIORV.sFileName;
                //
                Array.Copy(mIORV.m_DeviceName, m_DeviceName, LEN_DEVN);
                Array.Copy(mIORV.m_ChanControlFlag, m_ChanControlFlag, MAX_CH * 2);
                //
                Array.Copy(mIORV.m_RoutingEnableFlag, m_RoutingEnableFlag, MAX_CH);

            }


        }


        public override void loadPacakge(byte[] m_data, bool isRecall = false)
        {
            // 1
            int i = 0;
            int count = 0;
            if (isRecall)
                count = 11;

            for (i = 0; i < MAX_CH * 2; i++) //input and output
            {
                m_ChanControlFlag[i] = m_data[count++];
            }

            for (i = 0; i < MAX_CH; i++) //Routing enable flag
            {
                m_RoutingEnableFlag[i] = m_data[count++];
            }

            for (i = 0; i < LEN_DEVN; i++)
            {
                m_DeviceName[i] = m_data[count++];

            }
            m_McuVer = m_data[count++];
        }



        public override byte[] getPacakgeOfData()  //no 
        {
            int i = 0;
            int count = 0;
            byte[] m_Sence = new byte[len_sence]; //3164-11-2-5=3164-18=3146

            for (i = 0; i < MAX_CH * 2; i++) //input and output
            {
                m_Sence[count++] = m_ChanControlFlag[i];
            }

            for (i = 0; i < MAX_CH; i++) //Routing enable flag
            {
                m_Sence[count++] = m_RoutingEnableFlag[i];
            }

            for (i = 0; i < LEN_DEVN; i++)
            {
                m_Sence[count++] = m_DeviceName[i];

            }
            m_Sence[count++] = m_McuVer;
            return m_Sence;
        }























    }






}
