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
        /// Release the storage
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseMemStorage(ref _ptr);
        }

    }
}
