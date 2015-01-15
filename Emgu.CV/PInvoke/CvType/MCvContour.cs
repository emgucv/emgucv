//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvContour
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvContour
   {
      ///<summary>
      /// Micsellaneous flags 
      ///</summary>
      public int flags;
      ///<summary>
      /// Size of sequence header 
      ///</summary>
      public int header_size;
      ///<summary>
      /// Pointer to the previous sequence 
      ///</summary>
      public IntPtr h_prev;
      ///<summary>
      /// Pointer to the next sequence 
      ///</summary>
      public IntPtr h_next;
      ///<summary>
      /// Pointer to the 2nd previous sequence 
      ///</summary>
      public IntPtr v_prev;
      ///<summary>
      /// Pointer to the 2nd next sequence 
      ///</summary>
      public IntPtr v_next;
      ///<summary>
      /// Total number of elements 
      ///</summary>
      public int total;
      ///<summary>
      /// Size of sequence element in bytes 
      ///</summary>
      public int elem_size;
      ///<summary>
      /// Maximal bound of the last block 
      ///</summary>
      public IntPtr block_max;
      ///<summary>
      /// Current write pointer 
      ///</summary>
      public IntPtr ptr;
      ///<summary>
      /// How many elements allocated when the seq grows 
      ///</summary>
      public int delta_elems;
      ///<summary>
      /// Where the seq is stored 
      ///</summary>
      public IntPtr storage;
      ///<summary>
      /// Free blocks list 
      ///</summary>
      public IntPtr free_blocks;
      ///<summary>
      /// Pointer to the first sequence block 
      ///</summary>
      public IntPtr first;

      /// <summary>
      /// If computed, stores the minimum enclosing rectangle
      /// </summary>
      public System.Drawing.Rectangle rect;
      /// <summary>
      /// Color
      /// </summary>
      public int color;
      /// <summary>
      /// Reserved0
      /// </summary>
      public int reserved0;
      /// <summary>
      /// Reserved1
      /// </summary>
      public int reserved1;
      /// <summary>
      /// Reserved2
      /// </summary>
      public int reserved2;
   }
}
