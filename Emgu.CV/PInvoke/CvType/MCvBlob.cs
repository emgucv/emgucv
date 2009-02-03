using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBlob structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlob
   {
      /// <summary>
      /// The center of the blob 
      /// </summary>
      public PointF center; 

      /// <summary>
      /// blob size
      /// </summary>
      public SizeF size;

      /// <summary>
      /// blob ID  
      /// </summary>
      public int ID;

      /// <summary>
      /// Get the rectangle of this blob
      /// </summary>
      /// <returns>The rectangle of this blob</returns>
      public Rectangle GetRectangle()
      {
         return new Rectangle( (int)(center.X - size.Width / 2.0f), (int) (center.Y - size.Height / 2.0f), (int)size.Width, (int)size.Height); 
      }
   }
}
