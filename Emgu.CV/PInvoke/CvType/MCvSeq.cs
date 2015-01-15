//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvSeq
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSeq
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
      /// Previous sequence 
      ///</summary>
      public IntPtr h_prev;
      ///<summary>
      /// Next sequence 
      ///</summary>
      public IntPtr h_next;
      ///<summary>
      /// 2nd previous sequence 
      ///</summary>
      public IntPtr v_prev;
      ///<summary>
      /// 2nd next sequence 
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
      /// How many elements allocated when the sequence grows (sequence granularity 
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
   }
}
