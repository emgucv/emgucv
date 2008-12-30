using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML.Structure
{
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvDTreeSplitOrdered
   {
      /// <summary>
      /// Index of the variable used in the split 
      /// </summary>
      public int var_idx;

      /// <summary>
      /// When it equals to 1, the inverse split rule is used (i.e. left and right branches are exchanged in the expressions below)
      /// </summary>
      public int inversed;

      /// <summary>
      /// The split quality, a positive number. It is used to choose the best primary split, then to choose and sort the surrogate splits. After the tree is constructed, it is also used to compute variable importance
      /// </summary>
      public float quality;

      /// <summary>
      /// Pointer to the next split in the node split list
      /// </summary>
      public IntPtr next;

      MOrder ord;

      public struct MOrder
      {
         float c;
         int split_point;
      }
   }

   [StructLayout(LayoutKind.Sequential)]
   public struct MCvDTreeSplitCategorical
   {
      /// <summary>
      /// Index of the variable used in the split 
      /// </summary>
      public int var_idx;

      /// <summary>
      /// When it equals to 1, the inverse split rule is used (i.e. left and right branches are exchanged in the expressions below)
      /// </summary>
      public int inversed;

      /// <summary>
      /// The split quality, a positive number. It is used to choose the best primary split, then to choose and sort the surrogate splits. After the tree is constructed, it is also used to compute variable importance
      /// </summary>
      public float quality;

      /// <summary>
      /// Pointer to the next split in the node split list
      /// </summary>
      public IntPtr next;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
      int[] subset;
   }
}
