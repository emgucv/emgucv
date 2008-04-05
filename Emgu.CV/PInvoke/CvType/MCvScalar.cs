using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvScalar 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvScalar
    {
        /// <summary>
        /// The scalar value
        /// </summary>
        public double v0, v1, v2, v3;

        /// <summary>
        /// The scalar values as a vector (of size 4)
        /// </summary>
        public double[] v
        {
            get
            {
                return new double[4] { v0, v1, v2, v3 };
            }
        }

        /// <summary>
        /// Create a new MCvScalar structure using the specific values
        /// </summary>
        /// <param name="values"></param>
        public MCvScalar(params double[] values)
        {
            v0 = values.Length > 0 ? values[0] : 0.0;
            v1 = values.Length > 1 ? values[1] : 0.0;
            v2 = values.Length > 2 ? values[2] : 0.0;
            v3 = values.Length > 3 ? values[3] : 0.0;
        }
    }
}
