using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> A line segment </summary>
    public class LineSegment2D<T> : Line2D<T> where T : IComparable, new()
    {
        ///<summary> Create a line segment with the specific starting point and end point </summary>
        public LineSegment2D(Point2D<T> p1, Point2D<T> p2) : base(p1, p2) { }

        ///<summary> The length of the line segment </summary>
        public double Length { get { return (_p1 - _p2).Norm; } }

    };
}
