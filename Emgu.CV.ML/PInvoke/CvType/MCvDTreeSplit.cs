//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// Decision tree node split
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvDTreeSplit
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
      private int[] _subset;
      
      /// <summary>
      /// Get or Set the Order of this TreeSplit
      /// </summary>
      public MOrder Order
      {
         get
         {
            GCHandle handle = GCHandle.Alloc(_subset, GCHandleType.Pinned);
#if NETFX_CORE 
            MOrder res = Marshal.PtrToStructure<MOrder>(handle.AddrOfPinnedObject());
#else
            MOrder res = (MOrder) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(MOrder));
#endif
            handle.Free();
            return res;
         }
         set
         {
            GCHandle handle = GCHandle.Alloc(_subset, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
            handle.Free();
         }
      }

      /// <summary>
      /// Wrapped Order structure
      /// </summary>
      public struct MOrder
      {
         /// <summary>
         /// The threshold value in case of split on an ordered variable.
         /// The rule is: if var_value &lt; c then next_node&lt;-left else next_node&lt;-right
         /// </summary>
         public float c;
         /// <summary>
         /// Used internally by the training algorithm
         /// </summary>
         public int split_point;
      }

      /// <summary>
      /// Get the bit array indicating the value subset in case of split on a categorical variable.
      /// The rule is: if var_value in subset then next_node&lt;-left else next_node&lt;-right
      /// </summary>
      public int[] Subset
      {
         get
         {
            return _subset;
         }
      }
   }
}
