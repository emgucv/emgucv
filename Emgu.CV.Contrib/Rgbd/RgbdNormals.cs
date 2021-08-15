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

    public class RgbdNormals : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        public enum Method
        {
            Fals = 0,
            Linemod = 1,
            Sri = 2
        }

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
