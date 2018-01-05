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
        public void SetInput(Mat blob, String name)
        {
            using (CvString outputNameStr = new CvString(name))
                DnnInvoke.cveDnnNetSetInput(_ptr, blob, outputNameStr);
        }

        /*
      /// <summary>
      /// Returns the layer output blob.
      /// </summary>
      /// <param name="outputName">the descriptor of the returning layer output blob.</param>
      /// <returns>The layer output blob.</returns>
      public Blob GetBlob(String outputName)
      {
         using (CvString outputNameStr = new CvString(outputName))
         {
            return new Blob(DnnInvoke.cveDnnNetGetBlob(_ptr, outputNameStr));
         }
      }*/

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
        /// Returns true if there are no layers in the network.
        /// </summary>
        public bool Empty
        {
            get { return DnnInvoke.cveDnnNetEmpty(_ptr); }
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
        internal static extern void cveDnnNetSetInput(IntPtr net, IntPtr blob, IntPtr outputName);
        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal static extern IntPtr cveDnnNetGetBlob(IntPtr net, IntPtr outputName);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetForward(IntPtr net, IntPtr outputName, IntPtr output);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetRelease(ref IntPtr net);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern  bool cveDnnNetEmpty(IntPtr net);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnNetGetLayerNames(IntPtr net);
    }
}
#endif