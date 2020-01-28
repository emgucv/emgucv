//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Emgu.CV
{
    /// <summary>
    /// A (3x1) Rodrigues rotation vector. Rotation vector is a compact representation of rotation matrix. Direction of the rotation vector is the rotation axis and the length of the vector is the rotation angle around the axis. 
    /// </summary>
    [Serializable]
    public class RotationVector3D : Matrix<double>
    {
        /// <summary>
        /// Constructor used to deserialize 3D rotation vector
        /// </summary>
        /// <param name="info">The serialization info</param>
        /// <param name="context">The streaming context</param>
        public RotationVector3D(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

        /// <summary>
        /// Create a 3D rotation vector (3x1 Matrix).
        /// </summary>
        public RotationVector3D()
           : base(3, 1)
        {
        }

        /// <summary>
        /// Create a rotation vector using the specific values
        /// </summary>
        /// <param name="value">The values of the (3 x 1) Rodrigues rotation vector</param>
        public RotationVector3D(double[] value)
           : this()
        {
            Debug.Assert(value.Length == 3, "Rodrigues rotation Vector must have size == 3");
            Buffer.BlockCopy(value, 0, this.Data, 0, 3*sizeof(double));
        }

        /// <summary>
        /// Get or Set the (3x3) rotation matrix represented by this rotation vector.
        /// </summary>
        [XmlIgnore]
        public Mat RotationMatrix
        {
            get
            {
                Mat mat = new Mat();
                CvInvoke.Rodrigues(this, mat, null);
                return mat;
            }
            set
            {
                Debug.Assert(value.Rows == 3 && value.Cols == 3, "The rotation matrix should be a 3x3 matrix");

                CvInvoke.Rodrigues(value, this, null);

            }
        }
    }
}
