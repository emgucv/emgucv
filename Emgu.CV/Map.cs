using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.Data;

namespace Emgu.CV
{
    /// <summary>
    /// A Map is similar to an Image, except that the location of the pixels is defined by 
    /// its area and resolution
    /// </summary>
    /// <typeparam name="C">The color of this map</typeparam>
    /// <typeparam name="D">The depth of this map</typeparam>
    public class Map<C, D> : Image<C, D>  where C : ColorType, new() 
    {
        private Rectangle<double> _area;

        /// <summary>
        /// Create a new Image Map defined by the Rectangle area. The center (0.0, 0.0) of this map is 
        /// defined by the center of the rectangle.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="resolution">The resolution of x (y), (e.g. a value of 0.5 means each cell in the map is 0.5 unit in x (y) dimension)</param>
        /// <param name="color"> The initial color of the map</param>
        public Map(Rectangle<double> area, Point2D<double> resolution, C color)
             : base(
                System.Convert.ToInt32((area.Width) / resolution.X),
                System.Convert.ToInt32((area.Height) / resolution.Y),
                color)
        { 
            _area = area;
        }

        /// <summary>
        /// Create a new Image Map defined by the Rectangle area. The center (0.0, 0.0) of this map is 
        /// defined by the center of the rectangle. The initial value of the map is 0.0
        /// </summary>
        /// <param name="area"></param>
        /// <param name="resolution">The resolution of x (y), (e.g. a value of 0.5 means each cell in the map is 0.5 unit in x (y) dimension)</param>
        public Map(Rectangle<double> area, Point2D<double> resolution)
            : this(area, resolution, new C())
        {
        }

        /// <summary>
        /// Create a new Map using the specific image and the rectangle area
        /// </summary>
        /// <param name="image">The image of this map</param>
        /// <param name="area">The area of this map</param>
        public Map(Image<C, D> image, Rectangle<double> area)
            : base(image.Width, image.Height)
        {
            image.Copy(this);
            _area = area;
        }
    
        /// <summary>
        /// The area of this map as a rectangle
        /// </summary>
        public Rectangle<double> Area
        {
            get { return _area; }
        }

        /// <summary>
        /// The resolution of this map as a 2D point
        /// </summary>
        public Point2D<double> Resolution
        {
            get { return new Point2D<double>(Area.Width / Width, Area.Height / Height); }
        }

        /// <summary>
        /// Map a point to a position in the internal image
        /// </summary>
        /// <typeparam name="D2"></typeparam>
        /// <param name="pt"></param>
        /// <returns></returns>
        private Point2D<double> MapPoint<D2>( Point2D<D2> pt) where D2: IComparable, new()
        {
            return new Point2D<double>(
                (System.Convert.ToDouble(pt.X) - System.Convert.ToDouble(Area.Left)) / Resolution.X,
                (System.Convert.ToDouble(pt.Y) - System.Convert.ToDouble(Area.Bottom)) / Resolution.Y);
        }

        /// <summary>
        /// Draw a rectangle in the map
        /// </summary>
        /// <typeparam name="T">The type of the rectangle</typeparam>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color for the rectangle</param>
        /// <param name="thickness">The thickness of the rectangle, any value less than or equal to 0 will result in a filled rectangle</param>
        public void Draw<T>(Rectangle<T> rect, C color, int thickness) where T : IComparable, new()
        {
            Point2D<double> p = rect.Size.Convert<double>();
            Point2D<double> size = new Point2D<double>(p.X / Resolution.X, p.Y / Resolution.Y );
            base.Draw(new Rectangle<double>(MapPoint<T>(rect.Center), size), color, thickness);
        }

        /// <summary>
        /// Draw a line segment in the map
        /// </summary>
        /// <typeparam name="T">The type of the line</typeparam>
        /// <param name="line">The line to be draw</param>
        /// <param name="color">The color for the line</param>
        /// <param name="thickness">The thickness of the line</param>
        public void Draw<T>(LineSegment2D<T> line, C color, int thickness) where T : IComparable, new()
        {
            Point2D<int> p1 = MapPoint<T>(line.P1).Convert<int>();
            Point2D<int> p2 = MapPoint<T>(line.P2).Convert<int>();
            base.Draw(new LineSegment2D<int>(p1, p2), color, thickness);
        }

        /// <summary>
        /// Compute a new map where each element is obtained from converter
        /// </summary>
        /// <typeparam name="D2">The depth of the new Map</typeparam>
        /// <param name="converter">The converter that use the element from <i>this</i> map and the location of each pixel as input to compute the result</param>
        /// <returns> A new map where each element is obtained from converter</returns>
        public Map<C, D2> Convert<D2>(Emgu.Utils.Func<D, double, double, D2> converter)
        {
            double rx = Resolution.X, ry = Resolution.Y, ox = Area.Left, oy = Area.Bottom;

            Emgu.Utils.Func<D, int, int, D2> iconverter =
                delegate(D data, int row, int col)
                {
                    //convert an int position to double position
                    return converter(data, col * rx + ox, row * ry + oy);
                };
            return new Map<C,D2>(base.Convert<D2>(iconverter), Area);
        }
    }
}
