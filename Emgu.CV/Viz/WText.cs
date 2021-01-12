//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if ! ( UNITY_IOS || UNITY_ANDROID )

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{

    /// <summary>
    /// This 2D Widget represents text overlay.
    /// </summary>
    public class WText : UnmanagedObject, IWidget2D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget2dPtr;

        /// <summary>
        /// Constructs a WText.
        /// </summary>
        /// <param name="text">Text content of the widget.</param>
        /// <param name="pos">Position of the text.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="color">Color of the text.</param>
        public WText(String text, Point pos, int fontSize, MCvScalar color)
        {
            using (CvString cvs = new CvString(text))
                _ptr = CvInvoke.cveWTextCreate(cvs, ref pos, fontSize, ref color, ref _widget2dPtr, ref _widgetPtr);

        }

        /// <summary>
        /// Get the pointer to the widget2D object
        /// </summary>
        public IntPtr GetWidget2D
        {
            get { return _widget2dPtr; }
        }

        /// <summary>
        /// Get the pointer to the widget object.
        /// </summary>
        public IntPtr GetWidget
        {
            get { return _widgetPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Viz3d object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
            {
                CvInvoke.cveWTextRelease(ref _ptr);
                _widgetPtr = IntPtr.Zero;
                _widget2dPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWTextCreate(IntPtr text, ref Point pos, int fontSize, ref MCvScalar color, ref IntPtr widget2D, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWTextRelease(ref IntPtr text);

    }
}
#endif