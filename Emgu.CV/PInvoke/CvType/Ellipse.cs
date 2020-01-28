//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Emgu.CV.Structure
{
   ///<summary>
   ///An ellipse
   ///</summary>
   public struct Ellipse
   {
      private RotatedRect _box2D;

      /// <summary>
      /// The RotatedRect representation of this ellipse
      /// </summary>
      public RotatedRect RotatedRect
      {
         get { return _box2D; }
         set { _box2D = value; }
      }

      /// <summary>
      /// Create an ellipse with specific parameters
      /// </summary>
      /// <param name="center"> The center of the ellipse</param>
      /// <param name="size"> The width and height of the ellipse</param>
      /// <param name="angle"> The rotation angle in radian for the ellipse</param>
      public Ellipse(PointF center, SizeF size, float angle)
      {
         _box2D = new RotatedRect(center, size, angle);
      }

      /// <summary>
      /// Create an ellipse from the specific RotatedRect
      /// </summary>
      /// <param name="box2d">The RotatedRect representation of this ellipse</param>
      public Ellipse(RotatedRect box2d)
      {
         _box2D = box2d;
      }
   }
}
