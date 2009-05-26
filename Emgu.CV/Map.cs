using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// A Map is similar to an Image, except that the location of the pixels is defined by 
   /// its area and resolution
   /// </summary>
   /// <typeparam name="TColor">The color of this map</typeparam>
   /// <typeparam name="TDepth">The depth of this map</typeparam>
   public class Map<TColor, TDepth> : Image<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      private RectangleF _area;

      /// <summary>
      /// Get the area of this map as a rectangle
      /// </summary>
      public RectangleF Area
      {
         get { return _area; }
      }

      /// <summary>
      /// Get the resolution of this map as a 2D point
      /// </summary>
      public PointF Resolution
      {
         get { return new PointF(Area.Width / Width, Area.Height / Height); }
      }

      /// <summary>
      /// Create a new Image Map defined by the Rectangle area. The center (0.0, 0.0) of this map is 
      /// defined by the center of the rectangle.
      /// </summary>
      /// <param name="area"></param>
      /// <param name="resolution">The resolution of x (y), (e.g. a value of 0.5 means each cell in the map is 0.5 unit in x (y) dimension)</param>
      /// <param name="color"> The initial color of the map</param>
      public Map(RectangleF area, PointF resolution, TColor color)
         : base(
              (int)((area.Width) / resolution.X),
              (int)((area.Height) / resolution.Y),
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
      public Map(RectangleF area, PointF resolution)
         : this(area, resolution, new TColor())
      {
      }

      /// <summary>
      /// Create a new Map using the specific image and the rectangle area
      /// </summary>
      /// <param name="image">The image of this map</param>
      /// <param name="area">The area of this map</param>
      public Map(Image<TColor, TDepth> image, RectangleF area)
         : base(image.Size)
      {
         image.CopyTo(this);
         _area = area;
      }

      //private delegate Point PointTransformationFunction(PointF point);

      /// <summary>
      /// Map a point to a position in the internal image
      /// </summary>
      /// <param name="pt"></param>
      /// <returns></returns>
      private Point MapPointToImagePoint(PointF pt)
      {
         return
            new Point(
             (int)((pt.X - Area.Left) / Resolution.X),
             (int)((pt.Y - Area.Top) / Resolution.Y));
      }

      private Rectangle MapRectangleToImageRectangle(RectangleF rect)
      {
         return
            new Rectangle(MapPointToImagePoint(rect.Location),
               new Size((int) (rect.Width / Resolution.X),(int)( rect.Height / Resolution.Y)));
      }

      /// <summary>
      /// Map an image point to a Map point
      /// </summary>
      /// <param name="pt">The point on image</param>
      /// <returns>The point on map</returns>
      private PointF ImagePointToMapPoint(Point pt)
      {
         return
            new PointF(
               pt.X * Resolution.X + Area.Left,
               pt.Y * Resolution.Y + Area.Top);
      }

      private RectangleF ImageRectangleToMapRectangle(Rectangle rect)
      {
         return
            new RectangleF(ImagePointToMapPoint(rect.Location),
               new SizeF(rect.Width * Resolution.X, rect.Height * Resolution.Y)); 
      }

      /// <summary>
      /// Make a copy of the specific ROI (Region of Interest) from the map
      /// </summary>
      /// <param name="roi">The roi to be copied</param>
      /// <returns>The roi region on the map</returns>
      public Map<TColor, TDepth> Copy(RectangleF roi)
      {
         return new Map<TColor, TDepth>(
            base.Copy(MapRectangleToImageRectangle(roi)),
            roi);
      }

      ///<summary> 
      /// Get or Set the region of interest for this map. To clear the ROI, set it to System.Drawing.Rectangle.Empty
      ///</summary>
      public new RectangleF ROI
      {
         set
         {
            if (value.Equals(RectangleF.Empty))
            {
               //reset the image ROI
               CvInvoke.cvResetImageROI(Ptr);
            }
            else
            {  //set the image ROI to the specific value
               base.ROI = MapRectangleToImageRectangle( value );
            }
         }
         get
         {
            //return the image ROI
            return ImageRectangleToMapRectangle( base.ROI );
         }
      }

      /// <summary>
      /// Draw a rectangle in the map
      /// </summary>
      /// <param name="rect">The rectangle to draw</param>
      /// <param name="color">The color for the rectangle</param>
      /// <param name="thickness">The thickness of the rectangle, any value less than or equal to 0 will result in a filled rectangle</param>
      public void Draw(RectangleF rect, TColor color, int thickness)
      {
         base.Draw(new Rectangle(
            MapPointToImagePoint(new PointF(rect.Left + rect.Width / 2.0f, rect.Top + rect.Height / 2.0f)),
            new Size((int)(rect.Width / Resolution.X), (int)(rect.Height / Resolution.Y))
            ), color, thickness);
      }

      /// <summary>
      /// Draw a line segment in the map
      /// </summary>
      /// <param name="line">The line to be draw</param>
      /// <param name="color">The color for the line</param>
      /// <param name="thickness">The thickness of the line</param>
      public override void Draw(LineSegment2DF line, TColor color, int thickness)
      {
         base.Draw(new LineSegment2D(MapPointToImagePoint(line.P1), MapPointToImagePoint(line.P2)), color, thickness);
      }

      ///<summary> Draw a Circle of the specific color and thickness </summary>
      ///<param name="circle"> The circle to be drawn</param>
      ///<param name="color"> The color of the circle </param>
      ///<param name="thickness"> If thickness is less than 1, the circle is filled up </param>
      public override void Draw(CircleF circle, TColor color, int thickness)
      {
         base.Draw(
            new CircleF(MapPointToImagePoint(circle.Center), circle.Radius / Resolution.X),
            color,
            thickness);
      }

      ///<summary> Draw a convex polygon of the specific color and thickness </summary>
      ///<param name="polygon"> The convex polygon to be drawn</param>
      ///<param name="color"> The color of the convex polygon </param>
      ///<param name="thickness"> If thickness is less than 1, the triangle is filled up </param>
      public override void Draw(IConvexPolygonF polygon, TColor color, int thickness)
      {
         Point[] pts = Array.ConvertAll<PointF, Point>(
             polygon.GetVertices(),
             MapPointToImagePoint);

         if (thickness > 0)
            base.DrawPolyline(pts, true, color, thickness);
         else
            base.FillConvexPoly(pts, color);
      }

      /// <summary>
      /// Draw the text using the specific font on the image
      /// </summary>
      /// <param name="message">The text message to be draw</param>
      /// <param name="font">The font used for drawing</param>
      /// <param name="bottomLeft">The location of the bottom left corner of the font</param>
      /// <param name="color">The color of the text</param>
      public void Draw(String message, ref MCvFont font, PointF bottomLeft, TColor color)
      {
         base.Draw(message, ref font, MapPointToImagePoint(bottomLeft), color);
      }

      /// <summary>
      /// Draw the polyline defined by the array of 2D points
      /// </summary>
      /// <param name="pts">the points that defines the poly line</param>
      /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
      /// <param name="color">the color used for drawing</param>
      /// <param name="thickness">the thinkness of the line</param>
      public void DrawPolyline(PointF[] pts, bool isClosed, TColor color, int thickness)
      {
         base.DrawPolyline(
             Array.ConvertAll<PointF, Point>(pts, MapPointToImagePoint),
             isClosed,
             color,
             thickness);
      }

      /// <summary>
      /// Compute a new map where each element is obtained from converter
      /// </summary>
      /// <typeparam name="TOtherDepth">The depth of the new Map</typeparam>
      /// <param name="converter">The converter that use the element from <i>this</i> map and the location of each pixel as input to compute the result</param>
      /// <returns> A new map where each element is obtained from converter</returns>
      public Map<TColor, TOtherDepth> Convert<TOtherDepth>(Emgu.Util.Toolbox.Func<TDepth, double, double, TOtherDepth> converter)
         where TOtherDepth : new()
      {
         double rx = Resolution.X, ry = Resolution.Y, ox = Area.Left, oy = Area.Top;

         Emgu.Util.Toolbox.Func<TDepth, int, int, TOtherDepth> iconverter =
             delegate(TDepth data, int row, int col)
             {
                //convert an int position to double position
                return converter(data, col * rx + ox, row * ry + oy);
             };
         return new Map<TColor, TOtherDepth>(base.Convert<TOtherDepth>(iconverter), Area);
      }
   }
}

