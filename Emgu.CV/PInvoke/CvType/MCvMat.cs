//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvMat
   /// </summary>
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
   public struct MCvMat
   {
      /// <summary>
      /// CvMat signature (CV_MAT_MAGIC_VAL), element type and flags
      /// </summary>
      public int Type;

      /// <summary>
      /// full row length in bytes
      /// </summary>
      public int Step;

      /// <summary>
      /// underlying data reference counter
      /// </summary>
      public IntPtr Refcount;

      /// <summary>
      /// Header reference count
      /// </summary>
      public int HdrRefcount;

      /// <summary>
      /// data pointers
      /// </summary>
      public IntPtr Data;

      /// <summary>
      /// number of rows
      /// </summary>
      public int Rows;

      /// <summary>
      /// number of columns
      /// </summary>
      public int Cols;

      /// <summary>
      /// Width
      /// </summary>
      public int Width { get { return Cols; } }

      /// <summary>
      /// Height
      /// </summary>
      public int Height { get { return Rows; } }

      /// <summary>
      /// Get the number of channels
      /// </summary>
      public int NumberOfChannels
      {
         get
         {
            return ((((Type) & ((64 - 1) << 3)) >> 3) + 1);
         }
      }
   }

   /// <summary>
   /// Constants used by the MCvMat structure
   /// </summary>
   internal static class MCvMatConstants
   {
      /// <summary>
      /// Offset of roi
      /// </summary>
      public static readonly int TypeOffset = (int)Marshal.OffsetOf<MCvMat>("Type");
   }
}
