using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvSubdiv2DPoint
    {
        public int flags;
        public long first;
        public MCvPoint2D32f pt;
    }
}
