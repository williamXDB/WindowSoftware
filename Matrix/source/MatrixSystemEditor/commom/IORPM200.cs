using CommLibrary;
using Lib.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MatrixSystemEditor.commom
{
    [Serializable]
    public class IORPM200 :IOCommon
    {
        public static string fileFilter = "RPM200 File(*.RPM2)|*.RPM2";    
       
        public static string fileExeName = "RPM2";
        //

        public byte m_priority;
        public byte m_micVolume;
        public byte m_masterVolume;
        public byte m_chimeVolume;
        //  
        public byte m_maxZoneSetting;
        public byte m_chimeTime; //0...     

        public byte[][] m_rpmData;//60*32
        public byte[][] m_zoneName = new byte[CDefine.MaxZoneNum][];              

        public IORPM200()
        {
            len_sence = 3146;   //all sence len pack is :len_sence+18
            AppID = AppIDList.AP_RPM_100;         
            m_rpmData = new byte[CDefine.Max_Zones][]; //60*32
            for (int i = 0; i < CDefine.Max_Zones; i++)  //16
            {
                m_rpmData[i] = new byte[CDefine.Max_ZonDev * 2];
                Array.Clear(m_rpmData[i], 0, CDefine.Max_ZonDev * 2);
            }
            //----------------------------
            m_zoneName = new byte[CDefine.MaxZoneNum][];

            for (int i = 0; i < CDefine.MaxZoneNum; i++)
            {
                m_zoneName[i] = new byte[CDefine.Len_PresetName];
                setRMPZoneName(i, "Zone");
            }

            m_priority = 0;
            m_micVolume = 0;
            m_masterVolume = 0;
            m_chimeVolume = 0;
            Debug.WriteLine("IORPM 200 i s created now...");



        }
        public void copy(IORPM200 mrom)
        {           
            if (mrom != null)
            {
                AppID = mrom.AppID;
                DevID = mrom.DevID;

                m_priority = mrom.m_priority;
                m_micVolume = mrom.m_micVolume;
                m_masterVolume = mrom.m_masterVolume;
                m_chimeVolume = mrom.m_chimeVolume;
                //
                this.m_McuVer = mrom.m_McuVer;
                m_maxZoneSetting = mrom.m_maxZoneSetting;
                m_chimeTime = mrom.m_chimeTime; //0... 

                //---------------------
                Array.Clear(m_DeviceName, 0, CDefine.Len_PresetName);
                Array.Copy(mrom.m_DeviceName, m_DeviceName, CDefine.Len_PresetName);

                //
                for (int i = 0; i < CDefine.Max_Zones; i++)
                {
                    Array.Clear(m_rpmData[i], 0, CDefine.Max_ZonDev * 2);
                    Array.Copy(mrom.m_rpmData[i], m_rpmData[i], CDefine.Max_ZonDev * 2);

                    Array.Clear(m_zoneName[i], 0, CDefine.Len_PresetName);  //copy name
                    Array.Copy(mrom.m_zoneName[i], m_zoneName[i], CDefine.Len_PresetName);
                }

            }

        }


        

        //  public static int Max_Zones = 60;
        // public static int Max_ZonDev = 16;代表了有16台机器，只是其文档中通过32，2n+1 表示高位，2n*0来表示低位，仅此而已
        public void clearRPMData_withZonindex(int index)
        {
            Array.Clear(m_rpmData[index], 0, CDefine.Max_ZonDev * 2);
        }

        public void resetAllZoneData()
        {
            for (int i = 0; i < CDefine.Max_Zones; i++)
            {
                clearRPMData_withZonindex(i);
            }
        }


        public string nameOfZone(int chindex)  //chindex 0..192
        {

            return UtilCover.bytesToString(m_zoneName[chindex], CDefine.Len_FactPName); //fact 16 len
        }

        public void copyAllZoneName(byte[][] mdata)
        {
            if (mdata == null) return;
            for(int i=0;i<CDefine.MaxZoneNum;i++)
            {
                Array.Copy(mdata[i], m_zoneName[i], CDefine.Len_PresetName);
            }
        }

        public void setRMPZoneName(int index, string strname) //rpm100 only keep for save 8 length byte
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.LEN_DeviceName);
            Array.Clear(m_zoneName[index], 0, CDefine.Len_PresetName);
            Array.Copy(tmp, m_zoneName[index], tmp.Length);
        }

        public void resetAllZoneName()
        {
            if (m_zoneName == null) return;

            for (int i = 0; i < CDefine.MaxZoneNum; i++)
            {
                Array.Clear(m_zoneName[i], 0, 20);
            }

        }

        public override byte[] getPacakgeOfData()
        {     

            int i = 0;
            int count = 20;
            byte[] m_rpmSence = new byte[len_sence]; //3164-11-2-5=3164-18=3146
            Array.Copy(m_DeviceName, m_rpmSence, CDefine.Len_PresetName);//20
            //
            string str = "";
            for (int z = 0; z < CDefine.MaxZoneNum; z++) //60*20=1200
            {
                for (i = 0; i < CDefine.Len_PresetName; i++)
                    m_rpmSence[count++] = m_zoneName[z][i];

                str = nameOfZone(z);
                Debug.WriteLine("will save to device with   zone name is  {0}  zinde is  {1}", str, z);
            }


            for (int z = 0; z < CDefine.MaxZoneNum; z++)//
            {
                for (i = 0; i < CDefine.Max_ZonDev * 2; i++)
                    m_rpmSence[count++] = m_rpmData[z][i];
            }

            //
            m_rpmSence[count++] = m_micVolume;//micphone volume
            m_rpmSence[count++] = m_chimeVolume;
            m_rpmSence[count++] = m_masterVolume;
            //
            m_rpmSence[count++] = (byte)(m_priority + 1);
            m_rpmSence[count++] = (byte)(m_chimeTime + 1);

            m_rpmSence[count++] = m_maxZoneSetting; //1
            return m_rpmSence;
        }







        public override void loadPacakge(byte[] m_data, bool isRecall)
        {     
       
            int i = 0;
            int count = 0;
            if (isRecall)
                count += 11;

            for (i = 0; i < CDefine.Len_PresetName; i++)
            {
              m_DeviceName[i] = m_data[count++];
            }

            //
            string str = "";
            for (int z = 0; z < CDefine.MaxZoneNum; z++) //60*20=1200
            {
                for (i = 0; i < CDefine.Len_PresetName; i++)
                    m_zoneName[z][i] = m_data[count++];

                 str = nameOfZone(z);
               //  Debug.WriteLine("receive   zone name is  {0}  zinde is  {1}", str, z);

            }


            

            for (int z = 0; z < CDefine.MaxZoneNum; z++)//
            {
                for (i = 0; i < CDefine.Max_ZonDev * 2; i++)
                    m_rpmData[z][i] = m_data[count++];

                

            }

            /*
            for (int x = 0; x < 60;x++)
            {
                for (int f = 0; f < 16; f++)
                {
                    byte H4 = m_rpmData[x][2 * f + 1];
                    byte L8 = m_rpmData[x][2 * f + 0];
                    byte[] mData12 = CUlitity.catsByteHLToByteAry12(H4, L8);
                 //   IPProces.printAryByte("\n receive sence  parse .Single Zone bytes: with zoneindex " + f, mData12);
                }

            }
            */

                //
                m_micVolume = m_data[count++]; //micphone volume
            m_chimeVolume = m_data[count++];
            m_masterVolume = m_data[count++];
            //
            m_priority = (byte)(m_data[count++] - 1);
            m_chimeTime = (byte)(m_data[count++] - 1);
            m_maxZoneSetting = m_data[count++];

        }




    }
}
