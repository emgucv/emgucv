using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvSeqBlock
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSeqBlock
   {
      /// <summary>
      /// Previous sequence block.
      /// </summary>
      public IntPtr prev;
      /// <summary>
      /// Next sequence block.
      /// </summary>
      IntPtr next;
      /// <summary>
      /// Index of the first element in the block + sequence-&gt;first-&gt;start_index.   
      /// </summary>
      int start_index;
      /// <summary>
      /// Number of elements in the block.
      /// </summary>
      int count;
      /// <summary>
      /// Pointer to the first element of the block.
      /// </summary>
      IntPtr data;
   }
}
