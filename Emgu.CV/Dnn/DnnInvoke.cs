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
using System.Drawing;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// Entry points to the Open CV Dnn module
    /// </summary>
    public static partial class DnnInvoke
    {
        static DnnInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        /// <summary>
        /// Creates 4-dimensional blob from image. Optionally resizes and crops image from center, subtract mean values, scales values by scalefactor, swap Blue and Red channels.
        /// </summary>
        /// <param name="image">Input image (with 1- or 3-channels).</param>
        /// <param name="scaleFactor">Multiplier for image values.</param>
        /// <param name="size">Spatial size for output image</param>
        /// <param name="mean">Scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">Flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <param name="crop">Flag which indicates whether image will be cropped after resize or not</param>
        /// <param name="ddepth">Depth of output blob. Choose CV_32F or CV_8U.</param>
        /// <returns>4-dimensional Mat with NCHW dimensions order.</returns>
        public static Mat BlobFromImage(
            IInputArray image, 
            double scaleFactor = 1.0, 
            Size size = new Size(), 
            MCvScalar mean = new MCvScalar(), 
            bool swapRB = false, 
            bool crop = false, 
            CvEnum.DepthType ddepth = CvEnum.DepthType.Cv32F)
        {
            Mat blob = new Mat();
            BlobFromImage(image, blob, scaleFactor, size, mean, swapRB, crop, ddepth);
            return blob;
        }

        /// <summary>
        /// Creates 4-dimensional blob from image. Optionally resizes and crops image from center, subtract mean values, scales values by scalefactor, swap Blue and Red channels.
        /// </summary>
        /// <param name="image">Input image (with 1- or 3-channels).</param>
        /// <param name="blob">4-dimensional output array with NCHW dimensions order.</param>
        /// <param name="scaleFactor">Multiplier for image values.</param>
        /// <param name="size">Spatial size for output image</param>
        /// <param name="mean">Scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">Flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <param name="crop">Flag which indicates whether image will be cropped after resize or not</param>
        /// <param name="ddepth">Depth of output blob. Choose CV_32F or CV_8U.</param>
        public static void BlobFromImage(
            IInputArray image, 
            IOutputArray blob, 
            double scaleFactor = 1.0, 
            Size size = new Size(), 
            MCvScalar mean = new MCvScalar(), 
            bool swapRB = false, 
            bool crop = false,
            CvEnum.DepthType ddepth = CvEnum.DepthType.Cv32F)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaBlob = blob.GetOutputArray())
                cveDnnBlobFromImage(iaImage, oaBlob, scaleFactor, ref size, ref mean, swapRB, crop, ddepth);
            
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImage(
            IntPtr image,
            IntPtr blob,
            double scaleFactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool crop,
            CvEnum.DepthType ddepth);

        /// <summary>
        /// Creates 4-dimensional blob from series of images. Optionally resizes and crops images from center, subtract mean values, scales values by scalefactor, swap Blue and Red channels.
        /// </summary>
        /// <param name="images">Input images (all with 1- or 3-channels).</param>
        /// <param name="scaleFactor">Multiplier for images values.</param>
        /// <param name="size">Spatial size for output image</param>
        /// <param name="mean">Scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">Flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <param name="crop">Flag which indicates whether image will be cropped after resize or not</param>
        /// <param name="ddepth">Depth of output blob. Choose CV_32F or CV_8U.</param>
        /// <returns>Input image is resized so one side after resize is equal to corresponding dimension in size and another one is equal or larger. Then, crop from the center is performed.</returns>
        public static Mat BlobFromImages(
            Mat[] images, 
            double scaleFactor = 1.0, 
            Size size = new Size(), 
            MCvScalar mean = new MCvScalar(), 
            bool swapRB = false, 
            bool crop = false,
            CvEnum.DepthType ddepth = CvEnum.DepthType.Cv32F)
        {
            Mat blob = new Mat();
            using (VectorOfMat vm = new VectorOfMat(images))
            {
                BlobFromImages(vm, blob, scaleFactor, size, mean, swapRB, crop, ddepth);
            }
            return blob;
        }

        /// <summary>
        /// Creates 4-dimensional blob from series of images. Optionally resizes and crops images from center, subtract mean values, scales values by scale factor, swap Blue and Red channels.
        /// </summary>
        /// <param name="images">input images (all with 1-, 3- or 4-channels).</param>
        /// <param name="blob">4-dimensional OutputArray with NCHW dimensions order.</param>
        /// <param name="scaleFactor">multiplier for images values.</param>
        /// <param name="size">spatial size for output image</param>
        /// <param name="mean">scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <param name="crop">	flag which indicates whether image will be cropped after resize or not</param>
        /// <param name="ddepth">Depth of output blob. Choose CV_32F or CV_8U.</param>
        public static void BlobFromImages(
            IInputArrayOfArrays images, 
            IOutputArray blob, 
            double scaleFactor = 1.0, 
            Size size = new Size(), 
            MCvScalar mean = new MCvScalar(), 
            bool swapRB = false, 
            bool crop = false,
            CvEnum.DepthType ddepth = CvEnum.DepthType.Cv32F)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (OutputArray oaBlob = blob.GetOutputArray())
            {
                cveDnnBlobFromImages(iaImages, oaBlob, scaleFactor, ref size, ref mean, swapRB, crop, ddepth);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImages(
            IntPtr images,
            IntPtr blob,
            double scalefactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool crop,
            CvEnum.DepthType ddepth);

        /// <summary>
        /// Parse a 4D blob and output the images it contains as 2D arrays through a simpler data structure (std::vector&lt;cv::Mat&gt;).
        /// </summary>
        /// <param name="blob">4 dimensional array (images, channels, height, width) in floating point precision (CV_32F) from which you would like to extract the images.</param>
        /// <param name="images">Array of 2D Mat containing the images extracted from the blob in floating point precision (CV_32F). They are non normalized neither mean added. The number of returned images equals the first dimension of the blob (batch size). Every image has a number of channels equals to the second dimension of the blob (depth).</param>
        public static void ImagesFromBlob(Mat blob, IOutputArrayOfArrays images)
        {
            using (OutputArray oaImages = images.GetOutputArray())
            {
                cveDnnImagesFromBlob(blob, oaImages);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnImagesFromBlob(IntPtr blob, IntPtr images);

        /// <summary>
        /// Reads a network model stored in Darknet model files.
        /// </summary>
        /// <param name="cfgFile">path to the .cfg file with text description of the network architecture.</param>
        /// <param name="darknetModel">path to the .weights file with learned network.</param>
        /// <returns>Network object that ready to do forward, throw an exception in failure cases.</returns>
        public static Net ReadNetFromDarknet(String cfgFile, String darknetModel = null)
        {
            using (CvString cfgFileStr = new CvString(cfgFile))
            using (CvString darknetModelStr = darknetModel == null ? new CvString() : new CvString(darknetModel))
            {
                return new Net(cveReadNetFromDarknet(cfgFileStr, darknetModelStr));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromDarknet(IntPtr cfgFile, IntPtr darknetModel);


        /// <summary>
        /// Reads a network model stored in Darknet model files.
        /// </summary>
        /// <param name="bufferCfg">Buffer containing the content of the .cfg file with text description of the network architecture.</param>
        /// <param name="bufferModel">Buffer containing the content of the the .weights file with learned network.</param>
        /// <returns>Net object.</returns>
        public static Net ReadNetFromDarknet(byte[] bufferCfg, byte[] bufferModel = null)
        {
            GCHandle bufferCfgHandle = GCHandle.Alloc(bufferCfg, GCHandleType.Pinned);
            GCHandle bufferModelHandle = bufferModel == null ? new GCHandle() : GCHandle.Alloc(bufferModel, GCHandleType.Pinned);

            try
            {
                return new Net(cveReadNetFromDarknet2(
                    bufferCfgHandle.AddrOfPinnedObject(),
                    bufferCfg.Length,
                    bufferModel == null ? IntPtr.Zero : bufferModelHandle.AddrOfPinnedObject(),
                    bufferModel == null ? 0 : bufferModel.Length));
            }
            finally
            {
                bufferCfgHandle.Free();
                if (bufferModelHandle.IsAllocated)
                    bufferModelHandle.Free();
            }

        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromDarknet2(IntPtr bufferCfg, int lenCfg, IntPtr bufferModel, int lenModel);


        /// <summary>
        /// Reads a network model stored in Caffe framework's format.
        /// </summary>
        /// <param name="prototxt">Buffer containing the content of the .prototxt file</param>
        /// <param name="caffeModel">Buffer containing the content of the .caffemodel file</param>
        /// <returns>Net object.</returns>
        public static Net ReadNetFromCaffe(byte[] prototxt, byte[] caffeModel = null)
        {
            GCHandle prototxtHandle = GCHandle.Alloc(prototxt, GCHandleType.Pinned);
            GCHandle caffeModelHandle = caffeModel == null? new GCHandle() : GCHandle.Alloc(caffeModel, GCHandleType.Pinned);

            try
            {
                return new Net(cveReadNetFromCaffe2(
                    prototxtHandle.AddrOfPinnedObject(), 
                    prototxt.Length, 
                    caffeModel == null ? IntPtr.Zero : caffeModelHandle.AddrOfPinnedObject(),
                    caffeModel == null ? 0 : caffeModel.Length));
            } finally
            {
                prototxtHandle.Free();
                if (caffeModelHandle.IsAllocated)
                    caffeModelHandle.Free();
            }
            
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromCaffe2(IntPtr bufferProto, int lenProto, IntPtr bufferModel, int lenModel);

        /// <summary>
        /// Reads a network model stored in Caffe framework's format.
        /// </summary>
        /// <param name="prototxt">path to the .prototxt file with text description of the network architecture.</param>
        /// <param name="caffeModel">path to the .caffemodel file with learned network.</param>
        /// <returns>Net object.</returns>
        public static Net ReadNetFromCaffe(String prototxt, String caffeModel = null)
        {
            using (CvString prototxtStr = new CvString(prototxt))
            using (CvString caffeModelStr = caffeModel == null ? new CvString() : new CvString(caffeModel))
            {
                return new Net(cveReadNetFromCaffe(prototxtStr, caffeModelStr));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromCaffe(IntPtr prototxt, IntPtr caffeModel);


        /// <summary>
        /// Reads a network model stored in TensorFlow framework's format.
        /// </summary>
        /// <param name="model">path to the .pb file with binary protobuf description of the network architecture</param>
        /// <param name="config">path to the .pbtxt file that contains text graph definition in protobuf format. Resulting Net object is built by text graph using weights from a binary one that let us make it more flexible.</param>
        /// <returns>Net object.</returns>
        public static Net ReadNetFromTensorflow(String model, String config = null)
        {
            using (CvString modelStr = new CvString(model))
            using (CvString configStr = config == null ? new CvString() : new CvString(config))
            {
                return new Net(cveReadNetFromTensorflow(modelStr, configStr));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromTensorflow(IntPtr model, IntPtr config);

        /// <summary>
        /// Reads a network model stored in TensorFlow framework's format.
        /// </summary>
        /// <param name="model">buffer containing the content of the pb file</param>
        /// <param name="config">buffer containing the content of the pbtxt file</param>
        /// <returns>Net object.</returns>
        public static Net ReadNetFromTensorflow(byte[] model, byte[] config = null)
        {
            GCHandle modelHandle = GCHandle.Alloc(model, GCHandleType.Pinned);
            GCHandle configHandle = config == null ? new GCHandle() : GCHandle.Alloc(config, GCHandleType.Pinned);

            try
            {
                return new Net(cveReadNetFromTensorflow2(
                    modelHandle.AddrOfPinnedObject(),
                    model.Length,
                    config == null ? IntPtr.Zero : configHandle.AddrOfPinnedObject(),
                    config == null ? 0 : config.Length));
            }
            finally
            {
                modelHandle.Free();
                if (configHandle.IsAllocated)
                    configHandle.Free();
            }

        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromTensorflow2(IntPtr bufferModel, int lenModel, IntPtr bufferConfig, int lenConfig);

        /// <summary>
        /// Reads a network model ONNX.
        /// </summary>
        /// <param name="onnxFile">Path to the .onnx file with text description of the network architecture.</param>
        /// <returns>Network object that ready to do forward, throw an exception in failure cases.</returns>
        public static Net ReadNetFromONNX(String onnxFile)
        {
            using (CvString csOnnxFile = new CvString(onnxFile))
            {
                return new Net(cveReadNetFromONNX(csOnnxFile));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromONNX(IntPtr onnxFile);

        /// <summary>
        /// Creates blob from .pb file.
        /// </summary>
        /// <param name="path">Path to the .pb file with input tensor.</param>
        /// <returns>The blob</returns>
        public static Mat ReadTensorFromONNX(String path)
        {
            Mat m = new Mat();
            using (CvString csPath = new CvString(path))
            {
                cveReadTensorFromONNX(csPath, m);
            }
            return m;
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveReadTensorFromONNX(IntPtr path, IntPtr tensor);

        /// <summary>
        /// Read deep learning network represented in one of the supported formats.
        /// </summary>
        /// <param name="model">
        /// Binary file contains trained weights. The following file extensions are expected for models from different frameworks:
        ///    *.caffemodel(Caffe, http://caffe.berkeleyvision.org/)
        ///    *.pb (TensorFlow, https://www.tensorflow.org/)
        ///    *.t7 | *.net (Torch, http://torch.ch/)
        ///    *.weights (Darknet, https://pjreddie.com/darknet/)
        ///    *.bin (DLDT, https://software.intel.com/openvino-toolkit)</param>
        /// <param name="config">
        /// Text file contains network configuration. It could be a file with the following extensions:
        ///    *.prototxt(Caffe, http://caffe.berkeleyvision.org/)
        ///    *.pbtxt (TensorFlow, https://www.tensorflow.org/)
        ///    *.cfg (Darknet, https://pjreddie.com/darknet/)
        ///    *.xml (DLDT, https://software.intel.com/openvino-toolkit)
        /// </param>
        /// <param name="framework">Explicit framework name tag to determine a format.</param>
        /// <returns>Net object.</returns>
        public static Net ReadNet(String model, String config = null, String framework = null)
        {
            using (CvString modelStr = new CvString(model))
            using (CvString configStr = new CvString(config == null ? String.Empty : config))
            using (CvString frameworkStr = new CvString(framework == null ? String.Empty : framework))
            {
                return new Net(cveReadNet(modelStr, configStr, frameworkStr));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNet(IntPtr model, IntPtr config, IntPtr framework);

        /// <summary>
        /// Load a network from Intel's Model Optimizer intermediate representation.
        /// </summary>
        /// <param name="xml">XML configuration file with network's topology.</param>
        /// <param name="bin">Binary file with trained weights.</param>
        /// <returns>Net object. Networks imported from Intel's Model Optimizer are launched in Intel's Inference Engine backend.</returns>
        public static Net ReadNetFromModelOptimizer(String xml, String bin)
        {
            using (CvString xmlStr = new CvString(xml))
            using (CvString binStr = new CvString(bin))
            {
                return new Net(cveReadNetFromModelOptimizer(xmlStr, binStr));
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveReadNetFromModelOptimizer(IntPtr xml, IntPtr bin);


        /// <summary>
        /// Convert all weights of Caffe network to half precision floating point
        /// </summary>
        /// <param name="src">Path to origin model from Caffe framework contains single precision floating point weights (usually has .caffemodel extension).</param>
        /// <param name="dst">Path to destination model with updated weights.</param>
        public static void ShrinkCaffeModel(String src, String dst)
        {
            using (CvString csSrc = new CvString(src))
            using (CvString csDst = new CvString(dst))
                cveDnnShrinkCaffeModel(csSrc, csDst);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnShrinkCaffeModel(IntPtr src, IntPtr dst);

        /// <summary>
        /// Create a text representation for a binary network stored in protocol buffer format.
        /// </summary>
        /// <param name="model">A path to binary network.</param>
        /// <param name="output">A path to output text file to be created.</param>
        public static void WriteTextGraph(String model, String output)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csOutput = new CvString(output))
                cveDnnWriteTextGraph(csModel, csOutput);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnWriteTextGraph(IntPtr model, IntPtr output);


        /// <summary>
        /// Performs non maximum suppression given boxes and corresponding scores.
        /// </summary>
        /// <param name="bboxes">A set of bounding boxes to apply NMS.</param>
        /// <param name="scores">A set of corresponding confidences.</param>
        /// <param name="scoreThreshold">A threshold used to filter boxes by score.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression.</param>
        /// <param name="eta">A coefficient in adaptive threshold</param>
        /// <param name="topK">If &gt;0, keep at most top_k picked indices.</param>
        /// <returns>The indices of the boxes to keep after NMS</returns>
        public static int[] NMSBoxes(Rectangle[] bboxes, float[] scores, float scoreThreshold, float nmsThreshold, float eta = 1.0f, int topK = 0)
        {
            using(VectorOfRect vBoxes = new VectorOfRect(bboxes))
            using(VectorOfFloat vScores = new VectorOfFloat(scores))
            using (VectorOfInt indices = new VectorOfInt())
            {
                NMSBoxes(vBoxes, vScores, scoreThreshold, nmsThreshold, indices, eta, topK);
                return indices.ToArray();
            }
        }

        /// <summary>
        /// Performs non maximum suppression given boxes and corresponding scores.
        /// </summary>
        /// <param name="bboxes">A set of bounding boxes to apply NMS.</param>
        /// <param name="scores">A set of corresponding confidences.</param>
        /// <param name="scoreThreshold">A threshold used to filter boxes by score.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression.</param>
        /// <param name="indices">The kept indices of bboxes after NMS.</param>
        /// <param name="eta">A coefficient in adaptive threshold</param>
        /// <param name="topK">If &gt;0, keep at most top_k picked indices.</param>
        public static void NMSBoxes(VectorOfRect bboxes, VectorOfFloat scores, float scoreThreshold, float nmsThreshold, VectorOfInt indices, float eta=1.0f, int topK=0)
        {
            cveDnnNMSBoxes(bboxes, scores, scoreThreshold, nmsThreshold, indices, eta, topK);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnNMSBoxes(
            IntPtr bboxes,
            IntPtr scores,
            float scoreThreshold,
            float nmsThreshold,
            IntPtr indices,
            float eta,
            int topK);

        /// <summary>
        /// Get the list of available DNN Backends
        /// </summary>
        /// <returns>The available backend and target pair</returns>
        public static BackendTargetPair[] GetAvailableBackends()
        {
            using (VectorOfInt viBackends = new VectorOfInt())
            using (VectorOfInt viTargets = new VectorOfInt())
            {
                cveDNNGetAvailableBackends(viBackends, viTargets);
                int[] backendArr = viBackends.ToArray();
                int[] targetArr = viTargets.ToArray();

                BackendTargetPair[] availableBackends = new BackendTargetPair[backendArr.Length];
                for (int i = 0; i < backendArr.Length; i++)
                {
                    availableBackends[i] = new BackendTargetPair((Backend) backendArr[i], (Target) targetArr[i]);
                }

                return availableBackends;
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDNNGetAvailableBackends(IntPtr backends, IntPtr targets);
    }
}
