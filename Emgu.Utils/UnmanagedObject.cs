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
        protected IntPtr _ptr = IntPtr.Zero;

        /// <summary>
        /// Pointer to the unmanaged object
        /// </summary>
        public IntPtr Ptr
        {
            get
            {
                return _ptr;
            }

        }

        /// <summary>
        /// implicit operator for IntPtr
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static implicit operator IntPtr(UnmanagedObject obj)
        {
            return obj._ptr;
        }
    }
}
