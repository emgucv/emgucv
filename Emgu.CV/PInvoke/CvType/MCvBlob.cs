using System;
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
      public PointF Center; 

      /// <summary>
      /// blob size
      /// </summary>
      public SizeF Size;

      /// <summary>
      /// blob ID  
      /// </summary>
      public int ID;

      /// <summary>
      /// Convert a MCvBlob to RectangleF
      /// </summary>
      /// <param name="blob">The blob</param>
      /// <returns>The equivalent RectangleF</returns>
      public static implicit operator RectangleF(MCvBlob blob)
      {
         return new RectangleF( blob.Center.X - blob.Size.Width / 2.0f, blob.Center.Y - blob.Size.Height / 2.0f, blob.Size.Width, blob.Size.Height); 
      }
   }
}
