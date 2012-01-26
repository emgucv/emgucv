//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A MemStorage is a wrapper to cvMemStorage of OpenCV. 
   /// </summary>
   public class MemStorage : UnmanagedObject
   {
      //private List<MemStorage> _childStorageList;
      
      private MemStorage(IntPtr ptr)
      {
         _ptr = ptr;
         //_childStorageList = new List<MemStorage>();
      }

      /// <summary>
      /// Create a OpenCV MemStorage
      /// </summary>
      public MemStorage()
      {
         _ptr = CvInvoke.cvCreateMemStorage(0);
      }

      /// <summary>
      /// Resets the top (free space boundary) of the storage to the very beginning. This function does not deallocate any memory. If the storage has a parent, the function returns all blocks to the parent
      /// </summary>
      public void Clear()
      {
         CvInvoke.cvClearMemStorage(_ptr);
      }

      /// <summary>
      /// Creates a child memory storage that is similar to simple memory storage except for the differences in the memory allocation/deallocation mechanism. When a child storage needs a new block to add to the block list, it tries to get this block from the parent. The first unoccupied parent block available is taken and excluded from the parent block list. If no blocks are available, the parent either allocates a block or borrows one from its own parent, if any. In other words, the chain, or a more complex structure, of memory storages where every storage is a child/parent of another is possible. When a child storage is released or even cleared, it returns all blocks to the parent. In other aspects, the child storage is the same as the simple storage
      /// </summary>
      /// <returns>Child MemStorage</returns>
      public MemStorage CreateChildMemStorage()
      {
         IntPtr childStoragePtr = CvInvoke.cvCreateChildMemStorage(_ptr);
         MemStorage childStorage = new MemStorage(childStoragePtr);
         //_childStorageList.Add(childStorage);
         return childStorage;
      }

      /// <summary>
      /// Release the storage
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseMemStorage(ref _ptr);
      }
   }
}
