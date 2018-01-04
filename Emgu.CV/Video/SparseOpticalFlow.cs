//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
        
        public static void Calc(
            this IDenseOpticalFlow opticalFlow, 
            IInputArray prevImg, IInputArray nextImg, IInputArray ptrPts, IInputOutputArray nextPts
            )
        {

            
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
