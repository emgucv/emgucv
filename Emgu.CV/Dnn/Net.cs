//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if !(NETFX_CORE || NETSTANDARD1_4)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
//using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    public enum Backend
    {
        Default,
        Halide,
        InferenceEngine
    }

    public enum Target
    {
        Cpu,
        OpenCL,
        OpenCLFp16,
        Myriad
    }

    /// <summary>
    /// This class allows to create and manipulate comprehensive artificial neural networks.
    /// </summary>
    public partial class Net : UnmanagedObject
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Net()
        {
            _ptr = DnnInvoke.cveDnnNetCreate();
        }

        internal Net(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Sets the new value for the layer output blob.
        /// </summary>
        /// <param name="name">Descriptor of the updating layer output blob.</param>
        /// <param name="blob">Input blob</param>
        public void SetInput(IInputArray blob, String name = "", double scalefactor = 1.0,  MCvScalar mean = new MCvScalar())
        {
            using (CvString nameStr = new CvString(name))
            using (InputArray iaBlob = blob.GetInputArray())
                DnnInvoke.cveDnnNetSetInput(_ptr, iaBlob, nameStr, scalefactor, ref mean);
        }

        /// <summary>
        /// Runs forward pass for the whole network.
        /// </summary>
        /// <param name="outputName">name for layer which output is needed to get</param>
        /// <returns>blob for first output of specified layer</returns>
        public Mat Forward(String outputName)
        {
            using (CvString outputNameStr = new CvString(outputName))
            {
                Mat m = new Mat();
                DnnInvoke.cveDnnNetForward(_ptr, outputNameStr, m);
                return m;
            }
        }

        public void Forward(IOutputArrayOfArrays outputBlobs, String outputName)
        {
            using (OutputArray oaOutputBlobs = outputBlobs.GetOutputArray())
            using (CvString outputNameStr = new CvString(outputName))
            {
                DnnInvoke.cveDnnNetForward2(_ptr, oaOutputBlobs, outputNameStr);
            }
        }

        public void Forward(IOutputArrayOfArrays outputBlobs, String[] outBlobNames)
        {
            using (OutputArray oaOutputBlobs = outputBlobs.GetOutputArray())
            using (VectorOfCvString vcs = new VectorOfCvString(outBlobNames))
                DnnInvoke.cveDnnNetForward3(_ptr, oaOutputBlobs, vcs);
        }

        /// <summary>
        /// Release the memory associated with this network.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnNetRelease(ref _ptr);
            }
        }


        /// <summary>
        /// Return the LayerNames
        /// </summary>
        public String[] LayerNames
        {
            get
            {
                using (VectorOfCvString vs = new VectorOfCvString(DnnInvoke.cveDnnNetGetLayerNames(_ptr), true))
                {
                    return vs.ToArray();
                }
            }
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnNetCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetSetInput(IntPtr net, IntPtr blob, IntPtr name, double scalefactor, ref MCvScalar mean);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetForward(IntPtr net, IntPtr outputName, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetForward2(IntPtr net, IntPtr outputBlobs, IntPtr outputName);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetForward3(IntPtr net, IntPtr outputBlobs, IntPtr outBlobNames);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetRelease(ref IntPtr net);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnNetGetLayerNames(IntPtr net);
    }
}
#endif