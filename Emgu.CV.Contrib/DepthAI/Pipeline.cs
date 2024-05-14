//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
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
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Dai
{
    /// <summary>
    /// DepthAI Pipeline
    /// </summary>
    public partial class Pipeline : UnmanagedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        public Pipeline()
        {
            _ptr = DaiInvoke.daiPipelineCreate();
        }


        /// <summary>
        /// Release all unmanaged memory associated with the Pipeline.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DaiInvoke.daiPipelineRelease(ref _ptr);
            }
        }

        /// <summary>
        /// Gets the OpenVINO version used in the DepthAI pipeline.
        /// </summary>
        /// <value>
        /// The OpenVINO version.
        /// </value>
        /// <remarks>
        /// This property represents the specific release of OpenVINO used in the DepthAI pipeline.
        /// </remarks>
        public OpenVino.Version OpenVinoVersion
        {
            get
            {
                return DaiInvoke.daiPipelineGetOpenVINOVersion(_ptr);
            }
        }

        /// <summary>
        /// Creates a new instance of the ColorCamera class.
        /// </summary>
        /// <returns>
        /// A new instance of the ColorCamera class.
        /// </returns>
        public ColorCamera CreateColorCamera()
        {
            IntPtr colorCameraSharedPtr = IntPtr.Zero;
            IntPtr nodePtr = IntPtr.Zero;
            IntPtr colorCameraPtr = DaiInvoke.daiPipelineCreateColorCamera(_ptr, ref colorCameraSharedPtr, ref nodePtr);
            return new ColorCamera(colorCameraPtr, colorCameraSharedPtr, nodePtr);
        }

        /// <summary>
        /// Creates an instance of the MonoCamera class.
        /// </summary>
        /// <returns>
        /// A new MonoCamera object.
        /// </returns>
        public MonoCamera CreateMonoCamera()
        {
            IntPtr monoCameraSharedPtr = IntPtr.Zero;
            IntPtr nodePtr = IntPtr.Zero;
            IntPtr monoCameraPtr = DaiInvoke.daiPipelineCreateColorCamera(_ptr, ref monoCameraSharedPtr, ref nodePtr);
            return new MonoCamera(monoCameraPtr, monoCameraSharedPtr, nodePtr);
        }

        /// <summary>
        /// Creates an instance of the NeuralNetwork class.
        /// </summary>
        /// <returns>
        /// A new instance of the NeuralNetwork class.
        /// </returns>
        public NeuralNetwork CreateNeuralNetwork()
        {
            IntPtr neuralNetworkSharedPtr = IntPtr.Zero;
            IntPtr nodePtr = IntPtr.Zero;
            IntPtr neuralNetworkPtr = DaiInvoke.daiPipelineCreateColorCamera(_ptr, ref neuralNetworkSharedPtr, ref nodePtr);
            return new NeuralNetwork(neuralNetworkPtr, neuralNetworkSharedPtr, nodePtr);
        }

        /// <summary>
        /// Creates a StereoDepth object.
        /// </summary>
        /// <returns>
        /// A new instance of the StereoDepth class.
        /// </returns>
        public StereoDepth CreateStereoDepth()
        {
            IntPtr stereoDepthSharedPtr = IntPtr.Zero;
            IntPtr nodePtr = IntPtr.Zero;
            IntPtr stereoDepthPtr = DaiInvoke.daiPipelineCreateStereoDepth(_ptr, ref stereoDepthSharedPtr, ref nodePtr);
            return new StereoDepth(stereoDepthPtr, stereoDepthSharedPtr, nodePtr);
        }

        /// <summary>
        /// Creates an instance of XLinkOut.
        /// </summary>
        /// <returns>
        /// A new instance of the XLinkOut class.
        /// </returns>
        public XLinkOut CreateXLinkOut()
        {
            IntPtr xLinkOutSharedPtr = IntPtr.Zero;
            IntPtr xLinkOutPtr = DaiInvoke.daiPipelineCreateXLinkOut(_ptr, ref xLinkOutSharedPtr);
            return new XLinkOut(xLinkOutPtr, xLinkOutSharedPtr);
        }

    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiPipelineRelease(ref IntPtr pipeline);
        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreateColorCamera(
            IntPtr pipeline, 
            ref IntPtr colorCameraSharedPtr, 
            ref IntPtr nodePtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreateMonoCamera(
            IntPtr pipeline,
            ref IntPtr monoCameraSharedPtr,
            ref IntPtr nodePtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreateNeuralNetwork(
            IntPtr pipeline, 
            ref IntPtr neuralNetworkSharedPtr, 
            ref IntPtr nodePtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreateStereoDepth(
            IntPtr pipeline,
            ref IntPtr stereoDepthSharedPtr,
            ref IntPtr nodePtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiPipelineCreateXLinkOut(IntPtr pipeline, ref IntPtr xLinkOutSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern OpenVino.Version daiPipelineGetOpenVINOVersion(IntPtr pipeline);

    }
}