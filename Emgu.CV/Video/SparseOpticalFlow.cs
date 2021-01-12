//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Sparse Optical flow
    /// </summary>
    public interface ISparseOpticalFlow : IAlgorithm
    {
        /// <summary>
        /// Gets the sparse optical flow pointer.
        /// </summary>
        /// <value>
        /// The sparse optical flow .
        /// </value>
        IntPtr SparseOpticalFlowPtr { get; }
    }

    /// <summary>
    /// Extension methods for ISparseOpticalFlow
    /// </summary>
    public static class SparseOpticalFlowExtensions
    {
        /// <summary>
        /// Calculates a sparse optical flow.
        /// </summary>
        /// <param name="opticalFlow">The sparse optical flow</param>
        /// <param name="prevImg">First input image.</param>
        /// <param name="nextImg">Second input image of the same size and the same type as prevImg.</param>
        /// <param name="prevPts">Vector of 2D points for which the flow needs to be found.</param>
        /// <param name="nextPts">Output vector of 2D points containing the calculated new positions of input features in the second image.</param>
        /// <param name="status">Output status vector. Each element of the vector is set to 1 if the flow for the corresponding features has been found.Otherwise, it is set to 0.</param>
        /// <param name="error">Optional output vector that contains error response for each point (inverse confidence).</param>
        public static void Calc(
            this ISparseOpticalFlow opticalFlow, 
            IInputArray prevImg, IInputArray nextImg, 
            IInputArray prevPts, IInputOutputArray nextPts, 
            IOutputArray status,
            IOutputArray error = null
            )
        {
            using (InputArray iaPreImg = prevImg.GetInputArray())
            using (InputArray iaNextImg = nextImg.GetInputArray())
            using (InputArray iaPrevPts = prevPts.GetInputArray())
            using (InputOutputArray ioaNextPts = nextPts.GetInputOutputArray())
            using (OutputArray oaStatus = status.GetOutputArray())
            using (OutputArray oaError = error == null ? OutputArray.GetEmpty() : error.GetOutputArray())
                CvInvoke.cveSparseOpticalFlowCalc(
                    opticalFlow.SparseOpticalFlowPtr,
                    iaPreImg, iaNextImg,
                    iaPrevPts, ioaNextPts,
                    oaStatus, oaError
                    );
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSparseOpticalFlowCalc(
            IntPtr sof,
            IntPtr prevImg, IntPtr nextImg,
            IntPtr prevPts, IntPtr nextPts,
            IntPtr status,
            IntPtr err);


    }
}
