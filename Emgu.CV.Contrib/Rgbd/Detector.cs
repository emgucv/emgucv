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

    public abstract partial class Detector : SharedPtrObject
    {
        /// <summary>
        /// Read the detector from file node
        /// </summary>
        /// <param name="fn">The file node to read the detector from</param>
        public void Read(FileNode fn)
        {
            LinemodInvoke.cveLinemodDetectorRead(_ptr, fn);
        }

        /// <summary>
        /// Write the detector to file storage
        /// </summary>
        /// <param name="fs">The file storage to write the detector into.</param>
        public void Write(FileStorage fs)
        {
            LinemodInvoke.cveLinemodDetectorWrite(_ptr, fs);
        }

        /// <summary>
        /// Add new object template.
        /// </summary>
        /// <param name="sources">Source images, one for each modality.</param>
        /// <param name="classId">Object class ID.</param>
        /// <param name="objectMask">Mask separating object from background.</param>
        /// <param name="boundingBox">Return bounding box of the extracted features.</param>
        /// <returns>Template ID, or -1 if failed to extract a valid template.</returns>
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
        /// Get class ids
        /// </summary>
        public String[] ClassIds
        {
            get
            {
                using (VectorOfCvString vcs = new VectorOfCvString())
                {
                    LinemodInvoke.cveLinemodDetectorGetClassIds(_ptr, vcs);
                    return vcs.ToArray();
                }
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
        internal static extern void cveLinemodDetectorGetClassIds(IntPtr detector, IntPtr classIds);

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
