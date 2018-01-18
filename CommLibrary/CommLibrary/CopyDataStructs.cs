using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    public struct COPYDATASTRUCT
    {
        public IntPtr hData;      
        public int dLength;        
        [MarshalAs(UnmanagedType.LPStr)]
        public string stringData;

        public int preWph;//high
        public int preWpl;//low

        
  
        public void pringData()
        {
             if(hData!=null && dLength>0)
             {
                 byte[] tmp = new byte[dLength];
                 Marshal.Copy(hData, tmp, 0, dLength);

                 string str="structData len:"+dLength.ToString() +"   "+BitConverter.ToString(tmp);
                 str += " \n preWh: " + preWph.ToString();
                 str += " \n preWpl  " + preWpl.ToString();
                 Console.WriteLine(str);


             }

        }
        /// <summary>
        /// get the bit Array to return
        /// </summary>
        /// <returns></returns>
        public byte[] getBitAry()
        {

            byte[] tmp = new byte[dLength];
            Marshal.Copy(hData, tmp, 0, dLength);
            return tmp;
        }



      
    }


}
