//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint3D64f
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint3D64f : IEquatable<MCvPoint3D64f>
    {
        /// <summary>
        /// x-coordinate
        /// </summary>
        public double X;

        /// <summary>
        /// y-coordinate
        /// </summary>
        public double Y;

        /// <summary>
        /// z-coordinate
        /// </summary>
        public double Z;

        /// <summary>
        /// Create a MCvPoint3D64f structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        public MCvPoint3D64f(double x, double y, double z)
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
        public MCvPoint3D64f CrossProduct(MCvPoint3D64f point)
        {
            //A x B = <Ay*Bz - Az*By, Az*Bx - Ax*Bz, Ax*By - Ay*Bx>
            return new MCvPoint3D64f(
               Y * point.Z - Z * point.Y,
               Z * point.X - X * point.Z,
               X * point.Y - Y * point.X);
        }

        /// <summary>
        /// Return the dot product of two 3D point
        /// </summary>
        /// <param name="point">the other 3D point</param>
        /// <returns>The dot product of the two 3D point</returns>
        public double DotProduct(MCvPoint3D64f point)
        {
            return X * point.X + Y * point.Y + Z * point.Z;
        }

        #region operator overloads
        /// <summary>
        /// Compute the sum of two 3D points
        /// </summary>
        /// <param name="p1">The first point to be added</param>
        /// <param name="p2">The second point to be added</param>
        /// <returns>The sum of two points</returns>
        public static MCvPoint3D64f operator +(MCvPoint3D64f p1, MCvPoint3D64f p2)
        {
            return new MCvPoint3D64f(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        /// <summary>
        /// Subtract <paramref name="p2"/> from <paramref name="p1"/>
        /// </summary>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The point to be added</param>
        /// <returns>The sum of two points</returns>
        public static MCvPoint3D64f operator -(MCvPoint3D64f p1, MCvPoint3D64f p2)
        {
            return new MCvPoint3D64f(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint3D64f operator *(MCvPoint3D64f p, double scale)
        {
            return new MCvPoint3D64f(p.X * scale, p.Y * scale, p.Z * scale);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint3D64f operator *(double scale, MCvPoint3D64f p)
        {
            return p * scale;
        }
        #endregion

        #region IEquatable<MCvPoint3D64f> Members
        /// <summary>
        /// Check if the other point equals to this point
        /// </summary>
        /// <param name="other">The point to be compared</param>
        /// <returns>True if the two points are equal</returns>
        public bool Equals(MCvPoint3D64f other)
        {
            return X.Equals(other.X)
                   && Y.Equals(other.Y)
                   && Z.Equals(other.Z);
        }

        #endregion
    }
}
