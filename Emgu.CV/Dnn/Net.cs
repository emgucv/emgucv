//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
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
        /// <param name="scaleFactor">An optional normalization scale.</param>
        /// <param name="mean">An optional mean subtraction values.</param>
        public void SetInput(IInputArray blob, String name = "", double scaleFactor = 1.0,  MCvScalar mean = new MCvScalar())
        {
            using (CvString nameStr = new CvString(name))
            using (InputArray iaBlob = blob.GetInputArray())
                DnnInvoke.cveDnnNetSetInput(_ptr, iaBlob, nameStr, scaleFactor, ref mean);
        }

        /// <summary>
        /// Runs forward pass for the whole network.
        /// </summary>
        /// <param name="outputName">name for layer which output is needed to get</param>
        /// <returns>blob for first output of specified layer</returns>
        public Mat Forward(String outputName = "")
        {
            using (CvString outputNameStr = new CvString(outputName))
            {
                Mat m = new Mat();
                DnnInvoke.cveDnnNetForward(_ptr, outputNameStr, m);
                return m;
            }
        }

        /// <summary>
        /// Runs forward pass to compute output of layer with name outputName.
        /// </summary>
        /// <param name="outputBlobs">Contains all output blobs for specified layer.</param>
        /// <param name="outputName">Name for layer which output is needed to get</param>
        public void Forward(IOutputArrayOfArrays outputBlobs, String outputName = "")
        {
            using (OutputArray oaOutputBlobs = outputBlobs.GetOutputArray())
            using (CvString outputNameStr = new CvString(outputName))
            {
                DnnInvoke.cveDnnNetForward2(_ptr, oaOutputBlobs, outputNameStr);
            }
        }

        /// <summary>
        /// Runs forward pass to compute outputs of layers listed in outBlobNames.
        /// </summary>
        /// <param name="outputBlobs">Contains blobs for first outputs of specified layers.</param>
        /// <param name="outBlobNames">Names for layers which outputs are needed to get</param>
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

        /// <summary>
        /// Converts string name of the layer to the integer identifier.
        /// </summary>
        /// <param name="layerName">The name of the layer</param>
        /// <returns>The id of the layer</returns>
        public int GetLayerId(String layerName)
        {
            using (CvString csLayerName = new CvString(layerName))
            {
                return DnnInvoke.cveDnnGetLayerId(_ptr, csLayerName);
            }
        }

        /// <summary>
        /// Returns layer with specified name which the network use.
        /// </summary>
        /// <param name="layerName">The name of the layer</param>
        /// <returns>Layer with specified name which the network use.</returns>
        public Layer GetLayer(String layerName)
        {
            IntPtr sharedPtr = IntPtr.Zero;
            IntPtr ptr;
            using (CvString csLayerName = new CvString(layerName))
                ptr = DnnInvoke.cveDnnGetLayerByName(_ptr, csLayerName, ref sharedPtr);
            return new Layer(sharedPtr, ptr);
        }

        /// <summary>
        /// Returns layer with specified id which the network use.
        /// </summary>
        /// <param name="layerId">The id of the layer</param>
        /// <returns>Layer with specified id which the network use.</returns>
        public Layer GetLayer(int layerId)
        {
            IntPtr sharedPtr = IntPtr.Zero;
            IntPtr ptr = DnnInvoke.cveDnnGetLayerById(_ptr, layerId, ref sharedPtr);
            return new Layer(sharedPtr, ptr);
        }

        /// <summary>
        /// Returns indexes of layers with unconnected outputs.
        /// </summary>
        public int[] UnconnectedOutLayers
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    DnnInvoke.cveDnnNetGetUnconnectedOutLayers(_ptr, vi);
                    return vi.ToArray();
                }
            }
        }

        /// <summary>
        /// Returns names of layers with unconnected outputs.
        /// </summary>
        public String[] UnconnectedOutLayersNames
        {
            get
            {
                using (VectorOfCvString vi = new VectorOfCvString())
                {
                    DnnInvoke.cveDnnNetGetUnconnectedOutLayersNames(_ptr, vi);
                    return vi.ToArray();
                }
            }
        }

        /// <summary>
        /// Dump net to String
        /// </summary>
        /// <returns>
        /// String with structure, hyperparameters, backend, target and fusion
        /// Call method after setInput().
        /// To see correct backend, target and fusion run after forward().
        /// </returns>
        public String Dump()
        {
            using (CvString s = new CvString())
            {
                DnnInvoke.cveDnnNetDump(_ptr, s);
                return s.ToString();
            }
        }

        /// <summary>
        /// Dump net structure, hyperparameters, backend, target and fusion to dot file
        /// </summary>
        /// <param name="path">Path to output file with .dot extension</param>
        public void DumpToFile(String path)
        {
            using (CvString p = new CvString(path))
                DnnInvoke.cveDnnNetDumpToFile(_ptr, p);
        }

        /// <summary>
        /// Returns overall time for inference and timings (in ticks) for layers. Indexes in returned vector correspond to layers ids. Some layers can be fused with others, in this case zero ticks count will be return for that skipped layers.
        /// </summary>
        /// <param name="timings">Vector for tick timings for all layers.</param>
        /// <returns>Overall ticks for model inference.</returns>
        public Int64 GetPerfProfile(VectorOfDouble timings = null)
        {
            if (timings != null)
                return DnnInvoke.cveDnnNetGetPerfProfile(_ptr, timings);
            else
            {
                using (VectorOfDouble vd = new VectorOfDouble())
                    return DnnInvoke.cveDnnNetGetPerfProfile(_ptr, vd);
            }
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnNetCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetSetInput(IntPtr net, IntPtr blob, IntPtr name, double scaleFactor, ref MCvScalar mean);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnGetLayerId(IntPtr net, IntPtr layer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnGetLayerByName(IntPtr net, IntPtr layerName, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnGetLayerById(IntPtr net, int layerId, ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetGetUnconnectedOutLayers(IntPtr net, IntPtr layerIds);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetGetUnconnectedOutLayersNames(IntPtr net, IntPtr layerNames);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Int64 cveDnnNetGetPerfProfile(IntPtr net, IntPtr timings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetDump(IntPtr net, IntPtr dnnString);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnNetDumpToFile(IntPtr net, IntPtr path);
    }
}
