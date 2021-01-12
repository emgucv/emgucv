//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint2D64f
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint2D64f : IEquatable<MCvPoint2D64f>, IInterpolatable<MCvPoint2D64f>
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
        /// Create a MCvPoint2D64f structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        public MCvPoint2D64f(double x, double y)
        {
            this.X = x; this.Y = y;
        }

        #region operator overloads
        /// <summary>
        /// Compute the sum of two 3D points
        /// </summary>
        /// <param name="p1">The first point to be added</param>
        /// <param name="p2">The second point to be added</param>
        /// <returns>The sum of two points</returns>
        public static MCvPoint2D64f operator +(MCvPoint2D64f p1, MCvPoint2D64f p2)
        {
            return new MCvPoint2D64f(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Subtract <paramref name="p2"/> from <paramref name="p1"/>
        /// </summary>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The point to be added</param>
        /// <returns>The sum of two points</returns>
        public static MCvPoint2D64f operator -(MCvPoint2D64f p1, MCvPoint2D64f p2)
        {
            return new MCvPoint2D64f(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint2D64f operator *(MCvPoint2D64f p, double scale)
        {
            return new MCvPoint2D64f(p.X * scale, p.Y * scale);
        }

        /// <summary>
        /// Multiply the point with a scale
        /// </summary>
        /// <param name="p">The point to be multiplied</param>
        /// <param name="scale">The scale</param>
        /// <returns>The point multiplied by the scale</returns>
        public static MCvPoint2D64f operator *(double scale, MCvPoint2D64f p)
        {
            return p * scale;
        }
        #endregion

        #region IEquatable<MCvPoint2D64f> Members
        /// <summary>
        /// Returns true if the two points equals.
        /// </summary>
        /// <param name="other">The other point to compare with</param>
        /// <returns>True if the two points equals</returns>
        public bool Equals(MCvPoint2D64f other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        #endregion

        #region IInterpolatable<MCvPoint2D64f> Members

        /// <inheritdoc />
        double IInterpolatable<MCvPoint2D64f>.InterpolationIndex
        {
            get { return X; }
        }
        /*
        void IInterpolatable<MCvPoint2D64f>.Mul(double scale)
        {
           x *= scale;
           y *= scale;
        }

        void IInterpolatable<MCvPoint2D64f>.Add(MCvPoint2D64f i)
        {
           x += i.x;
           y += i.y;
        }

        void IInterpolatable<MCvPoint2D64f>.Sub(MCvPoint2D64f i)
        {
           x -= i.x;
           y -= i.y;
        }*/

        /// <inheritdoc />
        MCvPoint2D64f IInterpolatable<MCvPoint2D64f>.LinearInterpolate(MCvPoint2D64f other, double index)
        {
            double f1 = (other.X - index) / (other.X - this.X);
            double f2 = 1.0 - f1;
            return new MCvPoint2D64f(X * f1 + other.X * f2, Y * f1 + other.Y * f2);
        }
        #endregion
    }
}

