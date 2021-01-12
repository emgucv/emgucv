//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Class implementing EdgeBoxes algorithm from C. Lawrence Zitnick and Piotr Dollár. Edge boxes: Locating object proposals from edges. In ECCV, 2014.
    /// </summary>
    public class EdgeBoxes : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithm;

        /// <summary>
        /// Pointer to cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }

        /// <summary>
        /// Create an EdgeBox
        /// </summary>
        /// <param name="alpha">Step size of sliding window search.</param>
        /// <param name="beta">Nms threshold for object proposals.</param>
        /// <param name="eta">Adaptation rate for nms threshold.</param>
        /// <param name="minScore">Min score of boxes to detect.</param>
        /// <param name="maxBoxes">Max number of boxes to detect.</param>
        /// <param name="edgeMinMag">Edge min magnitude. Increase to trade off accuracy for speed.</param>
        /// <param name="edgeMergeThr">Edge merge threshold. Increase to trade off accuracy for speed.</param>
        /// <param name="clusterMinMag">Cluster min magnitude. Increase to trade off accuracy for speed.</param>
        /// <param name="maxAspectRatio">Max aspect ratio of boxes.</param>
        /// <param name="minBoxArea">Minimum area of boxes.</param>
        /// <param name="gamma">Affinity sensitivity.</param>
        /// <param name="kappa">Scale sensitivity.</param>
        public EdgeBoxes(
            float alpha = 0.65f,
            float beta = 0.75f,
            float eta = 1,
            float minScore = 0.01f,
            int maxBoxes = 10000,
            float edgeMinMag = 1.0f,
            float edgeMergeThr = 0.5f,
            float clusterMinMag = 0.5f,
            float maxAspectRatio = 3f,
            float minBoxArea = 1000f,
            float gamma = 2f,
            float kappa = 1.5f)
		{
            _ptr = XImgprocInvoke.cveEdgeBoxesCreate(
                alpha, 
                beta, 
                eta, 
                minScore,
                maxBoxes,
                edgeMinMag,
                edgeMergeThr,
                clusterMinMag,
                maxAspectRatio,
                minBoxArea,
                gamma,
                kappa,
                ref _algorithm, 
                ref _sharedPtr);
		}

        /// <summary>
        /// Returns array containing proposal boxes.
        /// </summary>
        /// <param name="edgeMap">edge image.</param>
        /// <param name="orientationMap">orientation map.</param>
        /// <returns>Proposal boxes.</returns>
        public Rectangle[] GetBoundingBoxes(IInputArray edgeMap, IInputArray orientationMap)
        {
            using (InputArray iaEdgeMap = edgeMap.GetInputArray())
            using (InputArray iaOrientationMap = orientationMap.GetInputArray())
            using (VectorOfRect vr = new VectorOfRect())
            {
                XImgprocInvoke.cveEdgeBoxesGetBoundingBoxes(_ptr, iaEdgeMap, iaOrientationMap, vr);
                return vr.ToArray();
            }
        }


        /// <inheritdoc />
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveEdgeBoxesRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveEdgeBoxesCreate(
			float alpha,
			float beta,
			float eta,
			float minScore,
			int   maxBoxes,
			float edgeMinMag,
			float edgeMergeThr,
			float clusterMinMag,
			float maxAspectRatio,
			float minBoxArea,
			float gamma,
			float kappa,
	        ref IntPtr algorithm,
	        ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void  cveEdgeBoxesGetBoundingBoxes(
            IntPtr edgeBoxes, 
            IntPtr edgeMap, 
            IntPtr orientationMap, 
            IntPtr boxes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeBoxesRelease(ref IntPtr sharedPtr);
    }
}
