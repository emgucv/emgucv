using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    public class Contour : Seq<MCvPoint>
    {
        public Contour(IntPtr ptr, MemStorage storage)
            : base(ptr, storage)
        {
        }
    }
}
