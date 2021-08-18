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


namespace Emgu.CV.Rgbd
{
    /// <summary>
    /// Object that can compute the normals in an image. It is an object as it can cache data for speed efficiency
    /// </summary>
    public class RgbdNormals : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Rgbd method
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// FALS (the fastest) 
            /// </summary>
            Fals = 0,
            /// <summary>
            /// Linemod
            /// </summary>
            Linemod = 1,
            /// <summary>
            /// SRI
            /// </summary>
            Sri = 2
        }

        /// <summary>
        /// Create a new RgbdNormals object that can compute the normals in an image.
        /// </summary>
        /// <param name="rows">The number of rows of the depth image normals will be computed on</param>
        /// <param name="cols">The number of cols of the depth image normals will be computed on</param>
        /// <param name="depth">The depth of the normals (only CV_32F or CV_64F)</param>
        /// <param name="k">The calibration matrix to use</param>
        /// <param name="windowSize">The window size to compute the normals: can only be 1,3,5 or 7</param>
        /// <param name="method">The methods to use</param>
        public RgbdNormals(
            int rows,
            int cols,
            DepthType depth,
            IInputArray k,
            int windowSize = 5,
            Method method = Method.Fals)
        {
            using (InputArray iaK = k.GetInputArray())
                _ptr = RgbdInvoke.cveRgbdNormalsCreate(
                    rows,
                    cols,
                    depth,
                    iaK,
                    windowSize,
                    method,
                    ref _algorithmPtr,
                    ref _sharedPtr
                );
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Given a set of 3d points in a depth image, compute the normals at each point.
        /// </summary>
        /// <param name="points">A rows x cols x 3 matrix of CV_32F/CV64F or a rows x cols x 1 CV_U16S</param>
        /// <param name="normals">A rows x cols x 3 matrix</param>
        public void Apply(
            IInputArray points,
            IOutputArray normals)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
                RgbdInvoke.cveRgbdNormalsApply(
                    _ptr,
                    iaPoints,
                    oaNormals);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                RgbdInvoke.cveRgbdNormalsRelease(ref _sharedPtr);
                _algorithmPtr = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }
    }
    
    public static partial class RgbdInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRgbdNormalsCreate(
            int rows,
            int cols,
            DepthType depth,
            IntPtr K,
            int windowSize,
            RgbdNormals.Method method, 
            ref IntPtr algorithm,
            ref IntPtr sharedPtr
        );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRgbdNormalsRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveRgbdNormalsApply(
            IntPtr rgbdNormals,
            IntPtr points,
            IntPtr normals);
    }
}
