using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Controls
{
   public class HalfSearcher
    {
        public static int search(double[] buf, int length, double mvalue)
        {
            int mid, start = 0, end = length - 1;
            while (start < end)
            {
                mid = (start + end) / 2;
                if (buf[mid] < mvalue)
                {
                    start = mid + 1;
                }
                else if (buf[mid] > mvalue)
                {
                    end = mid - 1;
                }
                else
                {
                    return mid;
                }
            }
            return start;
        }
    }
}



