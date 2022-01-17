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
    /// <summary>
    /// Object detector using the LINE template matching algorithm with any set of modalities.
    /// </summary>
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

        /// <summary>
        /// Detect objects by template matching. Matches globally at the lowest pyramid level, then refines locally stepping up the pyramid.
        /// </summary>
        /// <param name="sources">Source images, one for each modality.</param>
        /// <param name="threshold">Similarity threshold, a percentage between 0 and 100.</param>
        /// <param name="matches">Template matches, sorted by similarity score.</param>
        /// <param name="classIds">If non-empty, only search for the desired object classes.</param>
        /// <param name="quantizedImages">Optionally return vector&lt;Mat&gt; of quantized images.</param>
        /// <param name="masks">The masks for consideration during matching. The masks should be CV_8UC1 where 255 represents a valid pixel. If non-empty, the vector must be the same size as sources. Each element must be empty or the same size as its corresponding source.</param>
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
        /// Get sampling step T at <paramref name="pyramidLevel"/>.
        /// </summary>
        /// <param name="pyramidLevel">The pyramid level</param>
        /// <returns>Sampling step T</returns>
        public int GetT(int pyramidLevel)
        {
            return LinemodInvoke.cveLinemodDetectorGetT(_ptr, pyramidLevel);
        }

        /// <summary>
        /// Get the modalities used by this detector. You are not permitted to add/remove modalities, but you may cast them to tweak parameters.
        /// </summary>
        public Modality[] Modalities
        {
            get
            {
                if (_ptr == IntPtr.Zero)
                    return null;
                using (VectorOfIntPtr vp = new VectorOfIntPtr())
                {
                    LinemodInvoke.cveLinemodDetectorGetModalities(_ptr, vp);
                    IntPtr[] vpArr = vp.ToArray();
                    Modality[] results = new Modality[vpArr.Length];
                    for (int i = 0; i < vpArr.Length; i++)
                    {
                        results[i] = new Modality(vpArr[i], false);
                    }

                    return results;
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

    /// <summary>
    /// Detector using LINE algorithm with color gradients.
    /// </summary>
    public class LineDetector : Detector
    {
        /// <summary>
        /// Create a detector using LINE algorithm with color gradients. Default parameter settings suitable for VGA images.
        /// </summary>
        public LineDetector()
        {
            _ptr = LinemodInvoke.cveLinemodLineDetectorCreate(ref _sharedPtr);
        }

    }

    /// <summary>
    /// Detector using LINE-MOD algorithm with color gradients and depth normals.
    /// </summary>
    public class LinemodDetector : Detector
    {
        /// <summary>
        /// Create a detector using LINE-MOD algorithm with color gradients and depth normals. Default parameter settings suitable for VGA images.
        /// </summary>
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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveLinemodDetectorGetT(IntPtr detector, int pyramidLevel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodDetectorGetModalities(IntPtr detector, IntPtr vectorOfPtrs);
    }
}
