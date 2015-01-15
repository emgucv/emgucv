//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvChain
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvChain
   {
      ///<summary>
      /// micsellaneous flags 
      ///</summary>
      public int flags;
      ///<summary>
      /// size of sequence header 
      ///</summary>
      public int header_size;
      ///<summary>
      /// previous sequence 
      ///</summary>
      public IntPtr h_prev;
      ///<summary>
      /// next sequence 
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
      /// total number of elements 
      ///</summary>
      public int total;
      ///<summary>
      /// size of sequence element in bytes 
      ///</summary>
      public int elem_size;
      ///<summary>
      /// maximal bound of the last block 
      ///</summary>
      public IntPtr block_max;
      ///<summary>
      /// current write pointer 
      ///</summary>
      public IntPtr ptr;
      ///<summary>
      /// how many elements allocated when the seq grows 
      ///</summary>
      public int delta_elems;
      ///<summary>
      /// where the seq is stored 
      ///</summary>
      public IntPtr storage;
      ///<summary>
      /// free blocks list 
      ///</summary>
      public IntPtr free_blocks;
      ///<summary>
      /// pointer to the first sequence block 
      ///</summary>
      public IntPtr first;

      /// <summary>
      /// The origin of the chain
      /// </summary>
      public Point origin;

   }
}