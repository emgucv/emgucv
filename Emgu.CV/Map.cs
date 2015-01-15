//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.Serialization;
#if !NETFX_CORE
using System.Security.Permissions;
#endif
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A Map is similar to an Image, except that the location of the pixels is defined by 
   /// its area and resolution
   /// </summary>
   /// <typeparam name="TColor">The color of this map</typeparam>
   /// <typeparam name="TDepth">The depth of this map</typeparam>
#if !NETFX_CORE
   [Serializable]
#endif
   public class Map<TColor, TDepth> : Image<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      private RectangleF _area;
      private PointF _resolution;

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
         get { return _resolution; }
      }

      /// <summary>
      /// Create a new Image Map defined by the Rectangle area. The center (0.0, 0.0) of this map is 
      /// defined by the center of the rectangle.
      /// </summary>
      /// <param name="area"></param>
      /// <param name="resolution">The resolution of x (y), (e.g. a value of 0.5 means each cell in the map is 0.5 unit in x (y) dimension)</param>
      /// <param name="color"> The initial color of the map</param>
      public Map(RectangleF area, PointF resolution, TColor color)
         : this(area, resolution)
      {
         SetValue(color);
      }

      /// <summary>
      /// Create a new Image Map defined by the Rectangle area. The center (0.0, 0.0) of this map is 
      /// defined by the center of the rectangle. The initial value of the map is 0.0
      /// </summary>
      /// <param name="area"></param>
      /// <param name="resolution">The resolution of x (y), (e.g. a value of 0.5 means each cell in the map is 0.5 unit in x (y) dimension)</param>
      public Map(RectangleF area, PointF resolution)
         : base(               
            (int) Math.Round((area.Width) / resolution.X),
            (int) Math.Round((area.Height) / resolution.Y))
      {
         _area = area;
         _resolution = resolution;
      }

      /*
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
         _resolution = new PointF( area.Width / image.Width, area.Height / image.Height);
      }*/

      //private delegate Point PointTransformationFunction(PointF point);

      /// <summary>
      /// Map a point to a position in the internal image
      /// </summary>
      /// <param name="pt"></param>
      /// <returns></returns>
      public Point MapPointToImagePoint(MCvPoint2D64f pt)
      {
         return new Point(
           (int)Math.Round(( pt.X - Area.Left) / Resolution.X),
           (int)Math.Round(( pt.Y - Area.Top) / Resolution.Y));
      }

      /// <summary>
      /// Map a point to a position in the internal image
      /// </summary>
      /// <param name="pt"></param>
      /// <returns></returns>
      public Point MapPointToImagePoint(PointF pt)
      {
         return new Point(
           (int)Math.Round((pt.X - Area.Left) / Resolution.X),
           (int)Math.Round((pt.Y - Area.Top) / Resolution.Y));
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
      public PointF ImagePointToMapPoint(Point pt)
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
      /// Get a copy of the map in the specific area
      /// </summary>
      /// <param name="area">the area of the map to be retrieve</param>
      /// <returns>The area of the map</returns>
      public Map<TColor, TDepth> Copy(RectangleF area)
      {
         Map<TColor, TDepth> res = new Map<TColor, TDepth>(area, _resolution);
         if (Area.Contains(area))
         {  //the specific area is a subregion of current area
            ROI = area;
            CvInvoke.cvCopy(Ptr, res, IntPtr.Zero);
            ROI = RectangleF.Empty;
         }
         else if (Area.IntersectsWith(area))
         {  //partial intersect
            RectangleF intersectRegion = Area;
            intersectRegion.Intersect(area);
            ROI = intersectRegion;
            res.ROI = intersectRegion;
            CvInvoke.cvCopy(Ptr, res, IntPtr.Zero);
            ROI = RectangleF.Empty;
            res.ROI = RectangleF.Empty;
         }
         //else
         {  //the specific area do not over lap with current area at all
            //do nothing
         }
         return res;
      }


      ///<summary> 
      /// Get or Set the region of interest for this map. To clear the ROI, set it to System.Drawing.RectangleF.Empty
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

#if !( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
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
      /// <param name="lineType">Line type</param>
      /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
      public override void Draw(LineSegment2DF line, TColor color, int thickness, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
      {
         base.Draw(new LineSegment2DF(MapPointToImagePoint(line.P1), MapPointToImagePoint(line.P2)), color, thickness, lineType, shift);
      }

      ///<summary> Draw a Circle of the specific color and thickness </summary>
      ///<param name="circle"> The circle to be drawn</param>
      ///<param name="color"> The color of the circle </param>
      ///<param name="thickness"> If thickness is less than 1, the circle is filled up </param>
      /// <param name="lineType">Line type</param>
      /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
      public override void Draw(CircleF circle, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
      {
         base.Draw(
            new CircleF(MapPointToImagePoint(circle.Center), circle.Radius / Resolution.X),
            color,
            thickness, lineType, shift);
      }

      ///<summary> Draw a convex polygon of the specific color and thickness </summary>
      ///<param name="polygon"> The convex polygon to be drawn</param>
      ///<param name="color"> The color of the convex polygon </param>
      ///<param name="thickness"> If thickness is less than 1, the triangle is filled up </param>
      public override void Draw(IConvexPolygonF polygon, TColor color, int thickness)
      {
         Point[] pts =
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<PointF, Point>(
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
      /// <param name="fontFace">Font type.</param>
      /// <param name="fontScale">Font scale factor that is multiplied by the font-specific base size.</param>
      /// <param name="bottomLeft">The location of the bottom left corner of the font</param>
      /// <param name="color">The color of the text</param>
      /// <param name="thickness">Thickness of the lines used to draw a text.</param>
      /// <param name="lineType">Line type</param>
      /// <param name="bottomLeftOrigin">When true, the image data origin is at the bottom-left corner. Otherwise, it is at the top-left corner.</param>
      public override void Draw(String message, Point bottomLeft, CvEnum.FontFace fontFace, double fontScale, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, bool bottomLeftOrigin = false)
      {
         base.Draw(message, MapPointToImagePoint(bottomLeft), fontFace, fontScale, color, thickness, lineType, bottomLeftOrigin);
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
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<PointF, Point>(pts, MapPointToImagePoint),
            isClosed,
            color,
            thickness);
      }
#endif

#if !NETFX_CORE
      #region Implement ISerializable interface
      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public Map(SerializationInfo info, StreamingContext context)
         :base(info, context)
      {
         _area = (RectangleF)info.GetValue("Area", typeof(RectangleF));
         _resolution = (PointF)info.GetValue("Resolution", typeof(PointF));
      }

      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">streaming context</param>
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("Area", _area);
         info.AddValue("Resolution", _resolution);
      }
      #endregion
#endif

      /*
      /// <summary>
      /// Compute a new map where each element is obtained from converter
      /// </summary>
      /// <typeparam name="TOtherDepth">The depth of the new Map</typeparam>
      /// <param name="converter">The converter that use the element from <i>this</i> map and the location of each pixel as input to compute the result</param>
      /// <returns> A new map where each element is obtained from converter</returns>
      public Map<TColor, TOtherDepth> Convert<TOtherDepth>(Func<TDepth, double, double, TOtherDepth> converter)
         where TOtherDepth : new()
      {
         double rx = Resolution.X, ry = Resolution.Y, ox = Area.Left, oy = Area.Top;

         Func<TDepth, int, int, TOtherDepth> iconverter =
             delegate(TDepth data, int row, int col)
             {
                //convert an int position to double position
                return converter(data, col * rx + ox, row * ry + oy);
             };
         return new Map<TColor, TOtherDepth>(base.Convert<TOtherDepth>(iconverter), Area);
      }*/
   }
}

