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
            m_ptr = CvInvoke.cvCreateMemStorage(0);
        }

        /// <summary>
        /// Release the storage
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseMemStorage(ref m_ptr);
        }

        /// <summary>
        /// The Pointer to the storage
        /// </summary>
        public IntPtr Ptr
        {
            get { return m_ptr;  }
        }

    }
}
