using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Emgu.CV
{
    /// <summary>
    /// A 2D cross
    /// </summary>
    /// <typeparam name="T">The type of the cross element</typeparam>
    public class Cross2D<T> : Rectangle<T> where T : IComparable, new()
    {
        /// <summary>
        /// Construct a cross
        /// </summary>
        /// <param name="center">The center of the cross</param>
        /// <param name="width">the width of the cross</param>
        /// <param name="height">the height of the cross</param>
        public Cross2D(Point2D<T> center, T width, T height)
            : base(center, width, height)
        {

        }

        /// <summary>
        /// Get the horizonal linesegment of this cross
        /// </summary>
        public LineSegment2D<T> Horizontal
        {
            get
            {
                Point2D<double> c = Convert<double>();
                double width = System.Convert.ToDouble(_size.X);
                //double height = System.Convert.ToDouble(_size.Y);
                return new LineSegment2D<T>(
                    (new Point2D<double>(c.X - width / 2.0, c.Y)).Convert<T>(),
                    (new Point2D<double>(c.Y + width / 2.0, c.Y)).Convert<T>());
            }
        }

        /// <summary>
        /// Get the vertical linesegment of this cross
        /// </summary>
        public LineSegment2D<T> Vertical
        {
            get
            {
                Point2D<double> c = Convert<double>();
                //double width = System.Convert.ToDouble(_size.X);
                double height = System.Convert.ToDouble(_size.Y);
                return new LineSegment2D<T>(
                    (new Point2D<double>(c.X, c.Y - height / 2.0)).Convert<T>(),
                    (new Point2D<double>(c.Y, c.Y + height / 2.0)).Convert<T>());
            }
        }
    }
}
