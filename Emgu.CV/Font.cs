using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// A wraper to CvFont
    /// </summary>
    public class Font : UnmanagedObject
    {
        /// <summary>
        /// Create a Font of the specific type, horizontal scale and vertical scale
        /// </summary>
        /// <param name="type">The type of the font</param>
        /// <param name="hscale">The horizontal scale of the font</param>
        /// <param name="vscale">the vertical scale of the fonr</param>
        public Font(CvEnum.FONT type, double hscale, double vscale)
        {
            _ptr = Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(typeof(MCvFont)));
            CvInvoke.cvInitFont(_ptr, type, hscale, vscale, 0, 1, CvEnum.LINE_TYPE.EIGHT_CONNECTED);
        }

        /// <summary>
        /// Release the font object
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            Marshal.FreeHGlobal(_ptr);
        }
    }
}
