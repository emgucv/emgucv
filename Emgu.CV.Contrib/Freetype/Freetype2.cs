//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Draw UTF-8 strings with freetype/harfbuzz.
    /// </summary>    
    public class Freetype2 : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create instance to draw UTF-8 strings.
        /// </summary>
        public Freetype2()
        {
            _ptr = FreetypeInvoke.cveFreeType2Create(ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Native algorithm pointer
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Load font data.
        /// </summary>
        /// <param name="fontFileName">FontFile Name</param>
        /// <param name="id">Face index to select a font faces in a single file.</param>
        public void LoadFontData(String fontFileName, int id)
        {
            using (CvString csFontFileName = new CvString(fontFileName))
            {
                FreetypeInvoke.cveFreeType2LoadFontData(_ptr, csFontFileName, id);
            }
        }

        /// <summary>
        /// Set the number of split points from bezier-curve to line. If you want to draw large glyph, large is better. If you want to draw small glyph, small is better.
        /// </summary>
        /// <param name="num">Number of split points from bezier-curve to line</param>
        public void SetSplitNumber(int num)
        {
            FreetypeInvoke.cveFreeType2SetSplitNumber(_ptr, num);
        }

        /// <summary>
        /// Renders the specified text string in the image. Symbols that cannot be rendered using the specified font are replaced by "Tofu" or non-drawn.
        /// </summary>
        /// <param name="img">Image.</param>
        /// <param name="text">Text string to be drawn.</param>
        /// <param name="org">Bottom-left/Top-left corner of the text string in the image.</param>
        /// <param name="fontHeight">Drawing font size by pixel unit.</param>
        /// <param name="color">Text color.</param>
        /// <param name="thickness">Thickness of the lines used to draw a text when negative, the glyph is filled. Otherwise, the glyph is drawn with this thickness.</param>
        /// <param name="lineType">Line type</param>
        /// <param name="bottomLeftOrigin">When true, the image data origin is at the bottom-left corner. Otherwise, it is at the top-left corner.</param>
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

        /// <summary>
        /// Calculates the width and height of a text string.
        /// </summary>
        /// <param name="text">Input text string.</param>
        /// <param name="fontHeight">Drawing font size by pixel unit.</param>
        /// <param name="thickness">Thickness of lines used to render the text.</param>
        /// <param name="baseLine">y-coordinate of the baseline relative to the bottom-most text point.</param>
        /// <returns>The approximate size of a box that contains the specified text</returns>
        public Size GetTextSize(
            String text,
            int fontHeight, 
            int thickness,
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
