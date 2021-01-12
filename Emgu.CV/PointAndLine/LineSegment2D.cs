//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Emgu.CV.Structure
{
    /// <summary> 
    /// A line segment 
    /// </summary>
    public struct LineSegment2D
    {
        /// <summary> A point on the line </summary>
        private Point _p1;
        /// <summary> An other point on the line </summary>
        private Point _p2;

        /// <summary> A point on the line </summary>
        public Point P1 { get { return _p1; } set { _p1 = value; } }

        /// <summary> An other point on the line </summary>
        public Point P2 { get { return _p2; } set { _p2 = value; } }

        /// <summary> 
        /// Create a line segment with the specific starting point and end point 
        /// </summary>
        /// <param name="p1">The first point on the line segment</param>
        /// <param name="p2">The second point on the line segment</param>
        public LineSegment2D(Point p1, Point p2)
        {
            _p1 = p1;
            _p2 = p2;
        }

        /// <summary> The direction of the line, the norm of which is 1 </summary>
        public PointF Direction
        {
            get
            {
                int dx = P1.X - P2.X;
                int dy = P1.Y - P2.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                return new PointF(dx / dist, dy / dist);
            }
        }

        /// <summary>
        /// Determine which side of the line the 2D point is at
        /// </summary>
        /// <param name="point">the point</param>
        /// <returns>
        /// 1 if on the right hand side;
        /// 0 if on the line;
        /// -1 if on the left hand side;
        /// </returns>
        public int Side(Point point)
        {
            float res = (P2.X - P1.X) * (point.Y - P1.Y) - (point.X - P1.X) * (P2.Y - P1.Y);
            return res > 0.0f ? 1 :
               res < 0.0f ? -1 : 0;
        }

        /// <summary>
        /// Get the exterior angle between this line and <paramref name="otherLine"/>
        /// </summary>
        /// <param name="otherLine">The other line</param>
        /// <returns>The exterior angle between this line and <paramref name="otherLine"/></returns>
        public double GetExteriorAngleDegree(LineSegment2D otherLine)
        {
            PointF direction1 = Direction;
            PointF direction2 = otherLine.Direction;
            double radianAngle = Math.Atan2(direction2.Y, direction2.X) - Math.Atan2(direction1.Y, direction1.X);
            double degreeAngle = radianAngle * (180.0 / Math.PI);
            return
                degreeAngle <= -180.0 ? degreeAngle + 360 :
                degreeAngle > 180.0 ? degreeAngle - 360 :
                degreeAngle;
        }

        /// <summary> 
        /// Get the length of the line segment 
        /// </summary>
        public double Length
        {
            get
            {
                int dx = P1.X - P2.X;
                int dy = P1.Y - P2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }
    }
}
