//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE || NETSTANDARD1_4)
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
    /// Small interface class for loading trained serialized models of different dnn-frameworks.
    /// </summary>
    public class Importer : UnmanagedObject
    {
        internal Importer(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Creates the importer of Caffe framework network.
        /// </summary>
        /// <param name="prototxt">path to the .prototxt file with text description of the network architecture.</param>
        /// <param name="caffeModel">path to the .caffemodel file with learned network.</param>
        /// <returns>The created importer, NULL in failure cases.</returns>
        public static Importer CreateCaffeImporter(String prototxt, String caffeModel)
        {
            using (CvString prototxtStr = new CvString(prototxt))
            using (CvString caffeModelStr = new CvString(caffeModel))
            {
                IntPtr result = DnnInvoke.cveDnnCreateCaffeImporter(prototxtStr, caffeModelStr);
                return result == IntPtr.Zero ? null : new Importer(result);
            }
        }

        /// <summary>
        /// Creates the importer of TensorFlow framework network.
        /// </summary>
        /// <param name="model">Path to the .pb file with binary protobuf description of the network architecture.</param>
        /// <returns>The created importer, NULL in failure cases.</returns>
        public static Importer CreateTensorflowImporter(String model)
        {
            
            using (CvString modelStr = new CvString(model))
            {
                IntPtr result = DnnInvoke.cveDnnCreateTensorflowImporter(modelStr);
                return result == IntPtr.Zero ? null : new Importer(result);
            }
        }

        /// <summary>
        /// Adds loaded layers into the <paramref name="net"/> and sets connetions between them.
        /// </summary>
        /// <param name="net">The net model</param>
        public void PopulateNet(Net net)
        {
            DnnInvoke.cveDnnImporterPopulateNet(_ptr, net);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Importer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnImporterRelease(ref _ptr);
            }
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnCreateCaffeImporter(IntPtr prototxt, IntPtr caffeModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnCreateTensorflowImporter(IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnImporterRelease(ref IntPtr importer);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnImporterPopulateNet(IntPtr importer, IntPtr net);

        /// <summary>
        /// Initialize dnn module and built-in layers.
        /// </summary>
        [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveDnnInitModule", CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void DnnInitModule();
    }
}
#endif