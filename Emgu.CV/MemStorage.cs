using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// Wrapper to the OpenCV MemStorage
    /// </summary>
    public class MemStorage : UnmanagedObject
    {
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
        /// Release the storage
        /// </summary>
        protected override void DisposeObject()
        {
            CvInvoke.cvReleaseMemStorage(ref _ptr);
        }

    }
}
