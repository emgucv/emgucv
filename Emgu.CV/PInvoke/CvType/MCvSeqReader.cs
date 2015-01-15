//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvSeqReader structure
   /// </summary>
   public struct MCvSeqReader
   {
      /// <summary>
      /// The size of the header
      /// </summary>
      public int header_size;

      /// <summary>
      /// sequence, beign read
      /// </summary>
      public IntPtr seq;
      /// <summary>
      /// current block
      /// </summary>
      public IntPtr block;
      /// <summary>
      /// pointer to element be read next 
      /// </summary>
      public IntPtr ptr;
      /// <summary>
      ///  pointer to the beginning of block 
      /// </summary>
      public IntPtr block_min;
      /// <summary>
      /// pointer to the end of block
      /// </summary>
      public IntPtr block_max;
      /// <summary>
      /// = seq->first->start_index 
      /// </summary>
      public int delta_index;
      /// <summary>
      /// pointer to previous element
      /// </summary>
      public IntPtr prev_elem;
   }
}
