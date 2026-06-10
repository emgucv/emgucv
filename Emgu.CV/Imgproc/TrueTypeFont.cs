//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Wrapper on top of a truetype/opentype/etc font (cv::FontFace). The loaded font
    /// can be passed to CvInvoke.PutText and CvInvoke.GetTextSize.
    /// </summary>
    public partial class TrueTypeFont : UnmanagedObject
    {
        /// <summary>
        /// Load a font at the specified path or with the specified name.
        /// </summary>
        /// <param name="fontPathOrName">Either path to the custom font or the name of an embedded font: "sans", "italic" or "uni". A null or empty value loads the default embedded font.</param>
        public TrueTypeFont(String fontPathOrName = null)
        {
            using (CvString csFontPathOrName = new CvString(fontPathOrName ?? String.Empty))
                _ptr = CvInvoke.cveFontFaceCreate(csFontPathOrName);
        }

        /// <summary>
        /// Load a new font face.
        /// </summary>
        /// <param name="fontPathOrName">Either path to the custom font or the name of an embedded font: "sans", "italic" or "uni". An empty value loads the default embedded font.</param>
        /// <returns>True if the font is successfully loaded.</returns>
        public bool Set(String fontPathOrName)
        {
            using (CvString csFontPathOrName = new CvString(fontPathOrName ?? String.Empty))
                return CvInvoke.cveFontFaceSet(_ptr, csFontPathOrName);
        }

        /// <summary>
        /// Get the name of the loaded font.
        /// </summary>
        public String Name
        {
            get
            {
                using (CvString csName = new CvString())
                {
                    CvInvoke.cveFontFaceGetName(_ptr, csName);
                    return csName.ToString();
                }
            }
        }

        /// <summary>
        /// Set the current variable font instance.
        /// </summary>
        /// <param name="parameters">The list of pairs key1, value1, key2, value2, ... where the keys are FOURCC's of the variation axes (e.g. 'wght', 'slnt') and the values are specified in 16.16 fixed-point format, that is, integer values need to be shifted by 16 (or multiplied by 65536).</param>
        /// <returns>True if the instance is successfully set.</returns>
        public bool SetInstance(VectorOfInt parameters)
        {
            return CvInvoke.cveFontFaceSetInstance(_ptr, parameters);
        }

        /// <summary>
        /// Get the current variable font instance.
        /// </summary>
        /// <param name="parameters">The list of pairs key1, value1, key2, value2, ... describing the current variable font instance.</param>
        /// <returns>True if the instance is successfully retrieved.</returns>
        public bool GetInstance(VectorOfInt parameters)
        {
            return CvInvoke.cveFontFaceGetInstance(_ptr, parameters);
        }

        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveFontFaceRelease(ref _ptr);
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFontFaceCreate(IntPtr fontPathOrName);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFontFaceRelease(ref IntPtr fontFace);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveFontFaceSet(IntPtr fontFace, IntPtr fontPathOrName);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFontFaceGetName(IntPtr fontFace, IntPtr name);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveFontFaceSetInstance(IntPtr fontFace, IntPtr parameters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveFontFaceGetInstance(IntPtr fontFace, IntPtr parameters);
    }
}
