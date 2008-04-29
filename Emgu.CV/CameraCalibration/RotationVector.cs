using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
    /// <summary>
    /// A 3D rotation matrix
    /// </summary>
    public class RotationVector : Matrix<float>
    {
        /// <summary>
        /// Create a (3x1) 3D rotation vector.
        /// </summary>
        public RotationVector()
            : base(3, 1)
        {
        }

        /// <summary>
        /// Create a rotation vector using the specific values
        /// </summary>
        /// <param name="value"></param>
        public RotationVector(float[] value)
            : this()
        {
            Debug.Assert(value.Length == 3);
            Data = new float[,] 
                {   { value[0] }, 
                    { value[1] }, 
                    { value[2] }
                };
        }

        /// <summary>
        /// Get or Set the (3x3) rotation matrix represented by this rotation vector
        /// </summary>
        public Matrix<float> RotationMatrix
        {
            get
            {
                Matrix<float> mat = new Matrix<float>(3, 3);
                CvInvoke.cvRodrigues2(Ptr, mat.Ptr, IntPtr.Zero);
                return mat;
            }
            set
            {
                CvInvoke.cvRodrigues2(value.Ptr, Ptr, IntPtr.Zero);
            }
        }
    }
}
