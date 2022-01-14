//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Linemod
{

    public abstract class Detector : SharedPtrObject
    {

        public void Read(FileNode fn)
        {
            LinemodInvoke.cveLinemodDetectorRead(_ptr, fn);
        }

        public void Write(FileStorage fs)
        {
            LinemodInvoke.cveLinemodDetectorWrite(_ptr, fs);
        }

        public int AddTemplate(
            VectorOfMat sources,
            String classId,
            Mat objectMask,
            ref Rectangle boundingBox)
        {
            using (CvString csClassId = new CvString(classId))
            {
                return LinemodInvoke.cveLinemodDetectorAddTemplate(
                    _ptr,
                    sources,
                    csClassId,
                    objectMask,
                    ref boundingBox);
            }
        }

        public void Match(
            VectorOfMat sources,
            float threshold,
            VectorOfLinemodMatch matches,
            VectorOfCvString classIds = null,
            IOutputArrayOfArrays quantizedImages = null,
            VectorOfMat masks = null)
        {
            using (OutputArray oaQuantizedImages =
                   quantizedImages == null ? OutputArray.GetEmpty() : quantizedImages.GetOutputArray())
            {
                LinemodInvoke.cveLinemodDetectorMatch(
                    _ptr,
                    sources, 
                    threshold,
                    matches,
                    classIds,
                    oaQuantizedImages,
                    masks
                    );
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                LinemodInvoke.cveLinemodDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public class LineDetector : Detector
    {
        public LineDetector()
        {
            _ptr = LinemodInvoke.cveLinemodLineDetectorCreate(ref _sharedPtr);
        }

    }

    public class LinemodDetector : Detector
    {
        public LinemodDetector()
        {
            _ptr = LinemodInvoke.cveLinemodLinemodDetectorCreate(ref _sharedPtr);
        }
    }


    public static partial class LinemodInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLinemodLinemodDetectorCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLinemodLineDetectorCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodDetectorRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodDetectorRead(IntPtr detector, IntPtr fn);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodDetectorWrite(IntPtr detector, IntPtr fs);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveLinemodDetectorAddTemplate(
            IntPtr detector,
            IntPtr sources,
            IntPtr classId,
            IntPtr objectMask,
            ref Rectangle boundingBox);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodDetectorMatch(
            IntPtr detector,
            IntPtr sources,
            float threshold,
            IntPtr matches,
            IntPtr classIds,
            IntPtr quantizedImages,
            IntPtr masks);

    }
}
