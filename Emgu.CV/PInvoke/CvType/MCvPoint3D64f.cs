using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed Structure equivalent to CvPoint3D64f
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvPoint3D64f
   {
      /// <summary>
      /// x-coordinate
      /// </summary>
      public double x;

      /// <summary>
      /// y-coordinate
      /// </summary>
      public double y;

      /// <summary>
      /// z-coordinate
      /// </summary>
      public double z;

      /// <summary>
      /// Create a MCvPoint3D64f structure with the specific x and y coordinates
      /// </summary>
      /// <param name="x">x-coordinate</param>
      /// <param name="y">y-coordinate</param>
      /// <param name="z">z-coordinate</param>
      public MCvPoint3D64f(double x, double y, double z)
      {
         this.x = x;
         this.y = y;
         this.z = z;
      }
   }
}
