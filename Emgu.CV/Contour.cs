using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// Wrapped class for Contour
    /// </summary>
    public class Contour : Seq<MCvPoint>
    {
        /// <summary>
        /// Craete a contour from the specific IntPtr and storage
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="storage"></param>
        public Contour(IntPtr ptr, MemStorage storage)
            : base(ptr, storage)
        {
        }
    }
}
