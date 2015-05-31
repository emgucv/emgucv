using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// A point with Bgr color information
   /// </summary>
   public struct ColorPoint
   {
      /// <summary>
      /// The position in meters
      /// </summary>
      public MCvPoint3D32f Position;

      /// <summary>
      /// The blue color
      /// </summary>
      public Byte Blue;

      /// <summary>
      /// The green color
      /// </summary>
      public Byte Green;

      /// <summary>
      /// The red color
      /// </summary>
      public Byte Red;
   }
}
