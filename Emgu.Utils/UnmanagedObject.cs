using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu
{
    /// <summary>
    /// An abstract class that wrap around an unmanaged object
    /// </summary>
    public abstract class UnmanagedObject : DisposableObject
    {
        /// <summary>
        /// A pointer to the unmanaged object
        /// </summary>
        protected IntPtr _ptr;

    }
}
