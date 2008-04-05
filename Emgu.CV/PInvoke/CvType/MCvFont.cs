using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvFont
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvFont
    {
        ///<summary>
        /// =CV_FONT_* 
        ///</summary>
        public int font_face;
        ///<summary>
        /// font data and metrics 
        ///</summary>
        public IntPtr ascii;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr greek;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr cyrillic;
        /// <summary>
        /// hscale
        /// </summary>
        public float hscale;
        /// <summary>
        /// vscale
        /// </summary>
        public float vscale;
        ///<summary>
        /// slope coefficient: 0 - normal, >0 - italic 
        ///</summary>
        public float shear;
        ///<summary>
        /// letters thickness 
        ///</summary>
        public int thickness;
        ///<summary>
        /// horizontal interval between letters 
        ///</summary>
        public float dx;
        /// <summary>
        /// type of line
        /// </summary>
        public int line_type;
    }
}
