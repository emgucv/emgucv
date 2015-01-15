//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// A 2D cross
   /// </summary>
   public struct Cross2DF 
   {
      private PointF _center;
      private SizeF _size;

      /// <summary>
      /// The center of this cross
      /// </summary>
      public PointF Center
      {
         get { return _center; }
         set { _center = value; }
      }

      /// <summary>
      /// The size of this cross
      /// </summary>
      public SizeF Size
      {
         get { return _size; }
         set { _size = value; }
      }

      /// <summary>
      /// Construct a cross
      /// </summary>
      /// <param name="center">The center of the cross</param>
      /// <param name="width">the width of the cross</param>
      /// <param name="height">the height of the cross</param>
      public Cross2DF(PointF center, float width, float height)
      {
         _center = center;
         _size = new SizeF(width, height);
      }

      /// <summary>
      /// Get the horizonal linesegment of this cross
      /// </summary>
      public LineSegment2DF Horizontal
      {
         get
         {
            return new LineSegment2DF(
                (new PointF(_center.X - (_size.Width / 2.0f), _center.Y)),
                (new PointF(_center.X + (_size.Width / 2.0f), _center.Y)));
         }
      }

      /// <summary>
      /// Get the vertical linesegment of this cross
      /// </summary>
      public LineSegment2DF Vertical
      {
         get
         {
            return new LineSegment2DF(
                (new PointF(_center.X, _center.Y - (_size.Height / 2.0f))),
                (new PointF(_center.X, _center.Y + (_size.Height / 2.0f))));
         }
      }
   }
}
