using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lib.Controls
{
    public  class PointLine
    {

        public int LineID { get; set; }
        public List<Point> LinePointAry;
        public PointLine()
        {
            LineID = 0;
            LinePointAry=new List<Point>();
        }

           

    }
}
