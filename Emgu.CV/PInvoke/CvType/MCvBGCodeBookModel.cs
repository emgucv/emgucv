/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvBGCodeBookModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBGCodeBookModel : IEquatable<MCvBGCodeBookModel>
   {
      /// <summary>
      /// The size of the trained images
      /// </summary>
      public Size Size;
      /// <summary>
      /// The number of images used during the trainging process
      /// </summary>
      public int T;
      /// <summary>
      /// CbBounds0
      /// </summary>
      public Byte CbBounds0;
      /// <summary>
      /// CbBounds1
      /// </summary>
      public Byte CbBounds1;
      /// <summary>
      /// CbBounds1
      /// </summary>
      public Byte CbBounds2;
      /// <summary>
      /// ModMin0
      /// </summary>
      public Byte ModMin0;
      /// <summary>
      /// ModMin1
      /// </summary>
      public Byte ModMin1;
      /// <summary>
      /// ModMin2
      /// </summary>
      public Byte ModMin2;
      /// <summary>
      /// ModMax0
      /// </summary>
      public Byte ModMax0;
      /// <summary>
      /// ModMax1
      /// </summary>
      public Byte ModMax1;
      /// <summary>
      /// ModMax2
      /// </summary>
      public Byte ModMax2;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr Cbmap;
      /// <summary>
      /// Pointer to the MemStorage
      /// </summary>
      public IntPtr Storage;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr FreeList;

      #region IEquatable<MCvBGCodeBookModel> Members
      /// <summary>
      /// Check if two MCvBGCodeBookModel equals
      /// </summary>
      /// <param name="other">The other model to compare with</param>
      /// <returns>True if equals</returns>
      public bool Equals(MCvBGCodeBookModel other)
      {
         return Size.Equals(other.Size) && T == other.T
            && CbBounds0 == other.CbBounds0 && CbBounds1 == other.CbBounds1 && CbBounds2 == other.CbBounds2
            && ModMin0 == other.ModMin0 && ModMin1 == other.ModMin1 && ModMin2 == other.ModMin2
            && ModMax0 == other.ModMax0 && ModMax1 == other.ModMax1 && ModMax2 == other.ModMax2
            && Cbmap == other.Cbmap
            && Storage == other.Storage
            && FreeList == other.FreeList;
      }

      #endregion
   }
}
*/