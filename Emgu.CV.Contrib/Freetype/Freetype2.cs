//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Freetype
{
    
    public class Freetype2 : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        public Freetype2()
        {
            _ptr = FreetypeInvoke.cveFreeType2Create(ref _algorithmPtr, ref _sharedPtr);
        }

        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        public void LoadFontData(String fontFileName, int id)
        {
            using (CvString csFontFileName = new CvString(fontFileName))
            {
                FreetypeInvoke.cveFreeType2LoadFontData(_ptr, csFontFileName, id);
            }
        }

        public void SetSplitNumber(int num)
        {
            FreetypeInvoke.cveFreeType2SetSplitNumber(_ptr, num);
        }

        public void PutText(
            IInputOutputArray img,
            String text,
            Point org,
            int fontHeight,
            MCvScalar color,
            int thickness,
            LineType lineType,
            bool bottomLeftOrigin
        )
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
                using (CvString csText = new CvString(text))
            {
                FreetypeInvoke.cveFreeType2PutText(_ptr, ioaImg, csText, ref org, fontHeight, ref color, thickness, lineType, bottomLeftOrigin);
            }
        }

        public Size GetTextSize(
            String text,
            int fontHeight, int thickness,
            ref int baseLine)
        {
            Size s = new Size();
            using (CvString csText = new CvString(text))
            {
                FreetypeInvoke.cveFreeType2GetTextSize(_ptr, csText, fontHeight, thickness, ref baseLine, ref s);
            }

            return s;
        }

        /// <summary>
        /// Release all the unmanaged memory associate with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                FreetypeInvoke.cveFreeType2Release(ref _sharedPtr);
                _algorithmPtr = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }
    }



    /// <summary>
    /// This class wraps the functional calls to the OpenCV Freetype modules
    /// </summary>
    public static partial class FreetypeInvoke
    {
        static FreetypeInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFreeType2Create(ref IntPtr algorithmPtr, ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFreeType2Release(ref IntPtr sharedPtr);

        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFreeType2LoadFontData(IntPtr freetype, IntPtr fontFileName, int id);

        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFreeType2SetSplitNumber(IntPtr freetype, int num);
        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFreeType2PutText(
            IntPtr freetype,
            IntPtr img,
            IntPtr text,
            ref Point org,
            int fontHeight, 
            ref MCvScalar color,
            int thickness, 
            LineType lineType, 
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool bottomLeftOrigin
        );
        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFreeType2GetTextSize(
            IntPtr freetype,
            IntPtr text,
            int fontHeight, int thickness,
            ref int baseLine,
            ref Size size);
            
    }

}
