//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBlob structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlob : IEquatable<MCvBlob>
   {
      /// <summary>
      /// The center of the blob 
      /// </summary>
      public PointF Center;

      /// <summary>
      /// Blob size
      /// </summary>
      public SizeF Size;

      /// <summary>
      /// Blob ID  
      /// </summary>
      public int ID;

      /// <summary>
      /// Convert a MCvBlob to RectangleF
      /// </summary>
      /// <param name="blob">The blob</param>
      /// <returns>The equivalent RectangleF</returns>
      public static explicit operator RectangleF(MCvBlob blob)
      {
         return new RectangleF(blob.Center.X - blob.Size.Width / 2.0f, blob.Center.Y - blob.Size.Height / 2.0f, blob.Size.Width, blob.Size.Height);
      }

      /// <summary>
      /// Convert a MCvBlob to RectangleF
      /// </summary>
      /// <param name="blob">The blob</param>
      /// <returns>The equivalent RectangleF</returns>
      public static explicit operator Rectangle(MCvBlob blob)
      {
         return Rectangle.Round((RectangleF)blob);
      }

      /// <summary>
      /// Get an empty blob
      /// </summary>
      public static MCvBlob Empty
      {
         get
         {
            return new MCvBlob();
         }
      }

      #region IEquatable<MCvBlob> Members
      /// <summary>
      /// Check if the two blobs are equal
      /// </summary>
      /// <param name="other">The blob to compares with</param>
      /// <returns>True if equals</returns>
      public bool Equals(MCvBlob other)
      {
         return Center.Equals(other.Center) && Size.Equals(other.Size) && ID == other.ID;
      }

      #endregion
   }
}