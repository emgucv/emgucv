//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.PpfMatch3d
{
    /// <summary>
    /// This class implements a very efficient and robust variant of the iterative closest point (ICP) algorithm. The task is to register a 3D model (or point cloud) against a set of noisy target data. The variants are put together by myself after certain tests. The task is to be able to match partial, noisy point clouds in cluttered scenes, quickly. You will find that my emphasis is on the performance, while retaining the accuracy. 
    /// </summary>
    public class ICP : UnmanagedObject
    {
        /// <summary>
        /// The sampling type
        /// </summary>
        public enum SamplingType
        {
            /// <summary>
            /// Uniform
            /// </summary>
            Uniform = 0,
            /// <summary>
            /// Gelfand
            /// </summary>
            Gelfand = 1
        }

        /// <summary>
        /// Constructor to a very efficient and robust variant of the iterative closest point (ICP) algorithm.
        /// </summary>
        /// <param name="iterations">number of iterations</param>
        /// <param name="tolerence">Controls the accuracy of registration at each iteration of ICP.</param>
        /// <param name="rejectionScale">Robust outlier rejection is applied for robustness. This value actually corresponds to the standard deviation coefficient. Points with rejectionScale * sigma are ignored during registration.</param>
        /// <param name="numLevels">Number of pyramid levels to proceed. Deep pyramids increase speed but decrease accuracy. Too coarse pyramids might have computational overhead on top of the inaccurate registrtaion. This parameter should be chosen to optimize a balance. Typical values range from 4 to 10.</param>
        /// <param name="sampleType">Currently this parameter is ignored and only uniform sampling is applied. </param>
        /// <param name="numMaxCorr">Currently this parameter is ignored and only PickyICP is applied. Leave it as 1.</param>
        public ICP(
			int iterations, 
			float tolerence = 0.05f, 
			float rejectionScale = 2.5f, 
			int numLevels = 6,
            SamplingType sampleType = SamplingType.Uniform, 
			int numMaxCorr = 1)
        {
            _ptr = PpfMatch3dInvoke.cveICPCreate(
                iterations,
                tolerence,
                rejectionScale,
                numLevels,
                sampleType,
                numMaxCorr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with the ICP
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                PpfMatch3dInvoke.cveICPRelease(ref _ptr);
        }

        /// <summary>
        /// Perform registration.
        /// </summary>
        /// <param name="srcPC">The input point cloud for the model. Expected to have the normals (Nx6). Currently, CV_32F is the only supported data type.</param>
        /// <param name="dstPC">The input point cloud for the scene. It is assumed that the model is registered on the scene. Scene remains static. Expected to have the normals (Nx6). Currently, CV_32F is the only supported data type.</param>
        /// <param name="residual">The output registration error.</param>
        /// <param name="pose">Transformation between srcPC and dstPC.</param>
        /// <returns>On successful termination, the function returns 0.</returns>
		public int RegisterModelToScene(Mat srcPC, Mat dstPC, ref double residual, Mat pose)
		{
			return PpfMatch3dInvoke.cveICPRegisterModelToScene(_ptr, srcPC, dstPC, ref residual, pose);
        }
    }

    /// <summary>
    /// Entry points to the Open CV Surface Matching module
    /// </summary>
    public static partial class PpfMatch3dInvoke
    {
        static PpfMatch3dInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveICPCreate(
			int iterations, 
			float tolerence, 
			float rejectionScale, 
			int numLevels, 
			ICP.SamplingType sampleType, 
			int numMaxCorr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveICPRelease(ref IntPtr icp);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveICPRegisterModelToScene(IntPtr icp, IntPtr srcPC, IntPtr dstPC, ref double residual, IntPtr pose);
        
    }
}