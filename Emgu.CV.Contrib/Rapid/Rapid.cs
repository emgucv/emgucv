//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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


namespace Emgu.CV.Rapid
{
    /// <summary>
    /// The rapid tracker interface
    /// </summary>
    public interface ITracker
    {
        /// <summary>
        /// Pointer to the native Tracker object.
        /// </summary>
        IntPtr TrackerPtr { get; }
    }

    /// <summary>
    /// Wrapper around silhouette based 3D object tracking function for uniform access
    /// </summary>
    public class Rapid : SharedPtrObject, ITracker, IAlgorithm
    {
        private IntPtr _trackerPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a new instance of Rapid tracker
        /// </summary>
        /// <param name="pts3d">The 3D points of the mesh</param>
        /// <param name="tris">Triangle face connectivity</param>
        public Rapid(IInputArray pts3d, IInputArray tris)
        {
            using (InputArray iaPts3d = pts3d.GetInputArray())
            using (InputArray iaTris = tris.GetInputArray())
                _ptr = RapidInvoke.cveRapidCreate(iaPts3d, iaTris, ref _trackerPtr, ref _algorithmPtr, ref _sharedPtr);

        }

        /// <summary>
        /// Pointer to the native Tracker object
        /// </summary>
        public IntPtr TrackerPtr
        {
            get { return _trackerPtr; }
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                RapidInvoke.cveRapidRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
                _trackerPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Implements "Optimal local searching for fast and robust textureless 3D object tracking in highly cluttered backgrounds"
    /// </summary>
    public class OLSTracker : SharedPtrObject, ITracker, IAlgorithm
    {
        private IntPtr _trackerPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a new instance of OLS tracker
        /// </summary>
        /// <param name="pts3d">The 3D points of the mesh</param>
        /// <param name="tris">Triangle face connectivity</param>
        /// <param name="histBins">Number of histogram bins</param>
        /// <param name="sobelThresh">Sobel threshold</param>
        public OLSTracker(IInputArray pts3d, IInputArray tris, int histBins = 8, Byte sobelThresh = (byte)10)
        {
            using (InputArray iaPts3d = pts3d.GetInputArray())
            using (InputArray iaTris = tris.GetInputArray())
                _ptr = RapidInvoke.cveOLSTrackerCreate(iaPts3d, iaTris, histBins, sobelThresh, ref _trackerPtr, ref _algorithmPtr, ref _sharedPtr);

        }

        /// <summary>
        /// Pointer to the native Tracker object
        /// </summary>
        public IntPtr TrackerPtr
        {
            get { return _trackerPtr; }
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                RapidInvoke.cveOLSTrackerRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
                _trackerPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Class that contains entry points for the Rapid module.
    /// </summary>
    public static partial class RapidInvoke
    {
        /// <summary>
        /// Debug draw markers of matched correspondences onto a lineBundle
        /// </summary>
        /// <param name="bundle">the lineBundle</param>
        /// <param name="cols">column coordinates in the line bundle</param>
        /// <param name="colors">colors for the markers. Defaults to white.</param>
        public static void DrawCorrespondencies(
            IInputOutputArray bundle,
            IInputArray cols,
            IInputArray colors = null)
        {
            using (InputOutputArray ioaBundle = bundle.GetInputOutputArray())
            using (InputArray iaCols = cols.GetInputArray())
            using (InputArray iaColors = colors == null ? InputArray.GetEmpty() : colors.GetInputArray())
            {
                cveDrawCorrespondencies(ioaBundle, iaCols, iaColors);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDrawCorrespondencies(
            IntPtr bundle,
            IntPtr cols,
            IntPtr colors);

        /// <summary>
        /// Debug draw search lines onto an image
        /// </summary>
        /// <param name="img">The output image</param>
        /// <param name="locations">The source locations of a line bundle</param>
        /// <param name="color">The line color</param>
        public static void DrawSearchLines(
            IInputOutputArray img,
            IInputArray locations,
            MCvScalar color)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            using (InputArray iaLocations = locations.GetInputArray())
            {
                cveDrawSearchLines(ioaImg, iaLocations, ref color);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDrawSearchLines(
            IntPtr img,
            IntPtr locations,
            ref MCvScalar color);

        /// <summary>
        /// Draw a wireframe of a triangle mesh
        /// </summary>
        /// <param name="img">The output image</param>
        /// <param name="pts2d">The 2d points obtained by projectPoints</param>
        /// <param name="tris">Triangle face connectivity</param>
        /// <param name="color">Line color</param>
        /// <param name="type">Line type</param>
        /// <param name="cullBackface">Enable back-face culling based on CCW order</param>
        public static void DrawWireframe(
            IInputOutputArray img,
            IInputArray pts2d,
            IInputArray tris,
            MCvScalar color,
            CvEnum.LineType type = LineType.EightConnected,
            bool cullBackface = false)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            using (InputArray iaPts2d = pts2d.GetInputArray())
            using (InputArray iaTris = tris.GetInputArray())
            {
                cveDrawWireframe(ioaImg, iaPts2d, iaTris, ref color, type, cullBackface);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDrawWireframe(
            IntPtr img,
            IntPtr pts2d,
            IntPtr tris,
            ref MCvScalar color,
            CvEnum.LineType type,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool cullBackface);

        /// <summary>
        /// Extract control points from the projected silhouette of a mesh
        /// </summary>
        /// <param name="num">Number of control points</param>
        /// <param name="len">Search radius (used to restrict the ROI)</param>
        /// <param name="pts3d">The 3D points of the mesh</param>
        /// <param name="rvec">Rotation between mesh and camera</param>
        /// <param name="tvec">Translation between mesh and camera</param>
        /// <param name="K">Camera intrinsic</param>
        /// <param name="imsize">Size of the video frame</param>
        /// <param name="tris">Triangle face connectivity</param>
        /// <param name="ctl2d">The 2D locations of the control points</param>
        /// <param name="ctl3d">Matching 3D points of the mesh</param>
        public static void ExtractControlPoints(
            int num,
            int len,
            IInputArray pts3d,
            IInputArray rvec,
            IInputArray tvec,
            IInputArray K,
            Size imsize,
            IInputArray tris,
            IOutputArray ctl2d,
            IOutputArray ctl3d)
        {
            using (InputArray iaPts3d = pts3d.GetInputArray())
            using (InputArray iaRvec = rvec.GetInputArray())
            using (InputArray iaTvec = tvec.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaTris = tris.GetInputArray())
            using (OutputArray oaCtl2d = ctl2d.GetOutputArray())
            using (OutputArray oaCtl3d = ctl3d.GetOutputArray())
            {
                cveExtractControlPoints(num, len, iaPts3d, iaRvec, iaTvec, iaK, ref imsize, iaTris, oaCtl2d, oaCtl3d);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveExtractControlPoints(
            int num,
            int len,
            IntPtr pts3d,
            IntPtr rvec,
            IntPtr tvec,
            IntPtr K,
            ref Size imsize,
            IntPtr tris,
            IntPtr ctl2d,
            IntPtr ctl3d);

        /// <summary>
        /// Extract the line bundle from an image
        /// </summary>
        /// <param name="len">The search radius. The bundle will have 2*len + 1 columns.</param>
        /// <param name="ctl2d">The search lines will be centered at this points and orthogonal to the contour defined by them. The bundle will have as many rows.</param>
        /// <param name="img">The image to read the pixel intensities values from</param>
        /// <param name="bundle">Line bundle image with size ctl2d.rows() x (2 * len + 1) and the same type as img</param>
        /// <param name="srcLocations">The source pixel locations of bundle in img as CV_16SC2</param>
        public static void ExtractLineBundle(
            int len,
            IInputArray ctl2d,
            IInputArray img,
            IOutputArray bundle,
            IOutputArray srcLocations)
        {
            using (InputArray iaCtl2d = ctl2d.GetInputArray())
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaBundle = bundle.GetOutputArray())
            using (OutputArray oaSrcLocations = srcLocations.GetOutputArray())
                cveExtractLineBundle(len, iaCtl2d, iaImg, oaBundle, oaSrcLocations);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveExtractLineBundle(
            int len,
            IntPtr ctl2d,
            IntPtr img,
            IntPtr bundle,
            IntPtr srcLocations);

        /// <summary>
        /// Find corresponding image locations by searching for a maximal sobel edge along the search line (a single row in the bundle)
        /// </summary>
        /// <param name="bundle">The line bundle</param>
        /// <param name="cols">Correspondence-position per line in line-bundle-space</param>
        /// <param name="response">The sobel response for the selected point</param>
        public static void FindCorrespondencies(
            IInputArray bundle,
            IOutputArray cols,
            IOutputArray response)
        {
            using (InputArray iaBundle = bundle.GetInputArray())
            using (OutputArray oaCols = cols.GetOutputArray())
            using (OutputArray oaResponse = response.GetOutputArray())
            {
                cveFindCorrespondencies(iaBundle, oaCols, oaResponse);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFindCorrespondencies(
            IntPtr bundle,
            IntPtr cols,
            IntPtr response);

        /// <summary>
        /// Collect corresponding 2d and 3d points based on correspondencies and mask
        /// </summary>
        /// <param name="cols">Correspondence-position per line in line-bundle-space</param>
        /// <param name="srcLocations">The source image location</param>
        /// <param name="pts2d">2d points</param>
        /// <param name="pts3d">3d points</param>
        /// <param name="mask">mask containing non-zero values for the elements to be retained</param>
        public static void ConvertCorrespondencies(
            IInputArray cols,
            IInputArray srcLocations,
            IOutputArray pts2d,
            IInputOutputArray pts3d = null,
            IInputArray mask = null)
        {
            using (InputArray iaCols = cols.GetInputArray())
            using (InputArray iaSrcLocations = srcLocations.GetInputArray())
            using (OutputArray oaPts2d = pts2d.GetOutputArray())
            using (InputOutputArray ioaPts3d = pts3d.GetInputOutputArray())
            using (InputArray iaMask = mask == null? InputArray.GetEmpty() : mask.GetInputArray())
            {
                cveConvertCorrespondencies(iaCols, iaSrcLocations, oaPts2d, ioaPts3d, iaMask);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveConvertCorrespondencies(
            IntPtr cols,
            IntPtr srcLocations,
            IntPtr pts2d,
            IntPtr pts3d,
            IntPtr mask);

        /// <summary>
        /// High level function to execute a single rapid iteration
        /// </summary>
        /// <param name="img">The video frame</param>
        /// <param name="num">Number of search lines</param>
        /// <param name="len">Search line radius</param>
        /// <param name="pts3d">The 3D points of the mesh</param>
        /// <param name="tris">Triangle face connectivity</param>
        /// <param name="K">Camera matrix</param>
        /// <param name="rvec">Rotation between mesh and camera. Input values are used as an initial solution.</param>
        /// <param name="tvec">Translation between mesh and camera. Input values are used as an initial solution.</param>
        /// <param name="rmsd">The 2d reprojection difference</param>
        /// <returns>Ratio of search lines that could be extracted and matched</returns>
        public static float Rapid(
            IInputArray img,
            int num,
            int len,
            IInputArray pts3d,
            IInputArray tris,
            IInputArray K,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            ref double rmsd)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (InputArray iaPts3d = pts3d.GetInputArray())
            using (InputArray iaTris = tris.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputOutputArray ioaRvec = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTvec = tvec.GetInputOutputArray())
            {
                return cveRapid(iaImg, num, len, iaPts3d, iaTris, iaK, ioaRvec, ioaTvec, ref rmsd);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveRapid(
            IntPtr img,
            int num,
            int len,
            IntPtr pts3d,
            IntPtr tris,
            IntPtr K,
            IntPtr rvec,
            IntPtr tvec,
            ref double rmsd);

        /// <summary>
        /// High level function to execute a rapid iteration
        /// </summary>
        /// <param name="tracker">The tracker</param>
        /// <param name="img">The video frame</param>
        /// <param name="num">Number of search lines</param>
        /// <param name="len">Search line radius</param>
        /// <param name="K">Camera matrix</param>
        /// <param name="rvec">Rotation between mesh and camera. Input values are used as an initial solution.</param>
        /// <param name="tvec">Translation between mesh and camera. Input values are used as an initial solution.</param>
        /// <param name="termcrit">The termination criteria. Use 5 iteration and 1.5 eps for default.</param>
        /// <returns>Ratio of search lines that could be extracted and matched</returns>
        public static float Compute(
            this ITracker tracker,
            IInputArray img,
            int num,
            int len,
            IInputArray K,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            MCvTermCriteria termcrit)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputOutputArray ioaRvec = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTvec = tvec.GetInputOutputArray())
            {
                return cveTrackerCompute(tracker.TrackerPtr, iaImg, num, len, iaK, ioaRvec, ioaTvec, ref termcrit);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveTrackerCompute(
                IntPtr tracker,
                IntPtr img,
                int num,
                int len,
                IntPtr K,
                IntPtr rvec,
                IntPtr tvec,
                ref MCvTermCriteria termcrit);

        /// <summary>
        /// Clear the tracker state
        /// </summary>
        /// <param name="tracker">The tracker</param>
        public static void ClearState(this ITracker tracker)
        {
            cveTrackerClearState(tracker.TrackerPtr);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerClearState(IntPtr tracker);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRapidCreate(IntPtr pts3d, IntPtr tris, ref IntPtr tracker, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRapidRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOLSTrackerCreate(
            IntPtr pts3d,
            IntPtr tris,
            int histBins,
            Byte sobelThresh,
            ref IntPtr tracker,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOLSTrackerRelease(ref IntPtr sharedPtr);

    }
}
