using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV.DebuggerVisualizers.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat m = new Mat(640, 480, DepthType.Cv8U, 3);
            m.SetTo(new MCvScalar(255, 0, 0));
            ImageVisualizer.TestShowVisualizer(m);
        }
    }
}
