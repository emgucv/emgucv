//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint3D32f
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint3D32f : IEquatable<MCvPoint3D32f>
    {
        /// <summary>
        /// x-coordinate
        /// </summary>
        public float X;

        /// <summary>
        /// y-coordinate
        /// </summary>
        public float Y;

        /// <summary>
        /// z-coordinate
        /// </summary>
        public float Z;

        /// <summary>
        /// Create a MCvPoint3D32f structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        public MCvPoint3D32f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Return the cross product of two 3D point
        /// </summary>
        /// <param name="point">the other 3D point</param>
        /// <returns>The cross product of the two 3D point</returns>
        public MCvPoint3D32f CrossProduct(MCvPoint3D32f point)
        {
            //A x B = <Ay*Bz - Az*By, Az*Bx - Ax*Bz, Ax*By - Ay*Bx>
            return new MCvPoint3D32f(
               Y * point.Z - Z * point.Y,
               Z * point.X - X * point.Z,
               X * point.Y - Y * point.X);
        }

        /// <summary>
        /// Return the dot product of two 3D point
        /// </summary>
        /// <param name="point">the other 3D point</param>
        /// <returns>The dot product of the two 3D point</returns>
        public float DotProduct(MCvPoint3D32f point)
        {
            return X * point.X + Y * point.Y + Z * point.Z;
        }

        /// <summary>
        /// return the norm of this 3D point
        /// </summary>
        public double Norm
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        /// <summary>
        /// Get the normalized point
        /// </summary>
        /// <returns>The normalized point</returns>
        public MCvPoint3D32f GetNormalizedPoint()
        {
            float norm = (float)Norm;
            return new MCvPoint3D32f(X / norm, Y / norm, Z / norm);
        }

        /// <summary>
        /// The implicit operator to convert MCvPoint3D32f to MCvPoint3D64f
        /// </summary>
        /// <param name="point">The point to be converted</param>
        /// <returns>The converted point</returns>
        public static implicit operator MCvPoint3D64f(MCvPoint3D32f point)
        {
            return new MCvPoint3D64f(point.X, point.Y, point.Z);
        }

        #region operator overloads
        /// <summary>
        /// Subtract one point from the other
        /// </summary>
        /// <param name="p1">The point to subtract from</param>
        /// <param name="p2">The value to be subtracted</param>
        /// <returns>The subtraction of one point from the other</returns>
        public static MCvPoint3D32f operator -(MCvPoint3D32f p1, MCvPoint3D32f p2)
        {
            return new MCvPoint3D32f(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        /// <summary>
        /// Compute the sum of two 3D points
        /// </summary>
        /// <param name="p1">The first point to be added</param>
        /// <param name="p2">The second point to be added</param>
        /// <returns>The sum of two points</returns>
        public static MCvPoint3D32f operator +(MCvPoint3D32f p1, MCvPoint3D32f p2)
        {
            return new MCvPoint3D32f(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint3D32f operator *(MCvPoint3D32f p, float scale)
        {
            return new MCvPoint3D32f(p.X * scale, p.Y * scale, p.Z * scale);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint3D32f operator *(float scale, MCvPoint3D32f p)
        {
            return p * scale;
        }
        #endregion

        #region IEquatable<MCvPoint3D32f> Members
        /// <summary>
        /// Return true if the location of the two points are equal
        /// </summary>
        /// <param name="other">The other point to compare with</param>
        /// <returns>True if the location of the two points are equal</returns>
        public bool Equals(MCvPoint3D32f other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }
        #endregion
    }
}
