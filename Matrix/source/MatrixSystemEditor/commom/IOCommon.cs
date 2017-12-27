using CommLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSystemEditor.commom
{
    [Serializable]
    public class IOCommon
    {
        public static int LEN_DEVN = 20;
        public int AppID;
        public int DevID;
        public byte m_McuVer;

        public int len_sence;

        public byte[] m_DeviceName;//20
        //
        public string sFileName;
        public string sFileFilter;

        public string kVer(byte fver)
        {
            return string.Format("v{0}.{1}", fver >> 4, fver & 0x0f);
        }
        public string getMcuVer()
        {
            return this.kVer(m_McuVer);
        }

        public IOCommon()
        {
            len_sence = 0;
            AppID = 0;
            DevID = 0x1000;
            m_McuVer = 0;
            m_DeviceName = new byte[LEN_DEVN];

            Debug.WriteLine("IOCommon  is created  iniside now...");

        }

        public string nameofDevice()
        {

            return UtilCover.bytesToString(m_DeviceName, CDefine.Len_FactPName); //fact 16 len
        }

        public void setDevName(string strname) //rpm100 only keep for save 8 length byte
        {
            byte[] tmp = UtilCover.stringToBytes(strname, CDefine.Len_FactPName);
            Array.Clear(m_DeviceName, 0, LEN_DEVN);
            Array.Copy(tmp, m_DeviceName, tmp.Length);
        }

        public virtual void loadPacakge(byte[] m_data, bool isRecall)
        {

        }
        public virtual byte[] getPacakgeOfData()
        {
            byte[] m=new byte[1];
            return m;
        }


    }
}
