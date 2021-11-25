//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.DnnSuperres
{
    /// <summary>
    /// A class to upscale images via convolutional neural networks. The following four models are implemented:
    /// "edsr", "espcn", "fsrcnn", "lapsrn"
    /// </summary>
    public class DnnSuperResImpl : UnmanagedObject
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DnnSuperResImpl()
        {
            _ptr = DnnSuperresInvoke.cveDnnSuperResImplCreate();
        }

        /// <summary>
        /// Constructor which immediately sets the desired model.
        /// </summary>
        /// <param name="algorithm">String containing one of the desired models: "edsr", "espcn", "fsrcnn", "lapsrn"</param>
        /// <param name="scale">Integer specifying the upscale factor</param>
        public DnnSuperResImpl(String algorithm, int scale)
        {
            using (CvString csAlgorithm = new CvString(algorithm))
                _ptr = DnnSuperresInvoke.cveDnnSuperResImplCreate2(csAlgorithm, scale);
        }

        /// <summary>
        /// Read the model from the given path.
        /// </summary>
        /// <param name="path">Path to the model file.</param>
        public void ReadModel(String path)
        {
            using (CvString csPath = new CvString(path))
                DnnSuperresInvoke.cveDnnSuperResImplReadModel1(_ptr, csPath);
        }

        /// <summary>
        /// Read the model from the given path.
        /// </summary>
        /// <param name="weight">Path to the model weights file.</param>
        /// <param name="definition">Path to the model definition file.</param>
        public void ReadModel(String weight, String definition)
        {
            using (CvString csWeight = new CvString(weight))
            using (CvString csDefinition = new CvString(definition))
            {
                DnnSuperresInvoke.cveDnnSuperResImplReadModel2(_ptr, csWeight, csDefinition);
            }
        }

        /// <summary>
        /// Set desired model.
        /// </summary>
        /// <param name="algorithm">String containing one of the desired models: "edsr", "espcn", "fsrcnn", "lapsrn"</param>
        /// <param name="scale">Integer specifying the upscale factor</param>
        public void SetModel(String algorithm, int scale)
        {
            using (CvString csAlgorithm = new CvString(algorithm))
                DnnSuperresInvoke.cveDnnSuperResImplSetModel(_ptr, csAlgorithm, scale);
        }

        /// <summary>
        /// Upsample via neural network.
        /// </summary>
        /// <param name="img">Image to upscale</param>
        /// <param name="result">Destination upscaled image</param>
        public void Upsample(IInputArray img, IOutputArray result)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
            {
                DnnSuperresInvoke.cveDnnSuperResImplUpsample(_ptr, iaImg, oaResult);
            }
        }

        /// <summary>
        /// Get the scale factor of the model.
        /// </summary>
        public int Scale
        {
            get { return DnnSuperresInvoke.cveDnnSuperResImplGetScale(_ptr); }
        }

        /// <summary>
        /// Get the name of the algorithm.
        /// </summary>
        public String Algorithm
        {
            get
            {
                using (CvString csAlgorithm = new CvString())
                {
                    DnnSuperresInvoke.cveDnnSuperResImplGetAlgorithm(_ptr, csAlgorithm);
                    return csAlgorithm.ToString();
                }
            }
        }

        /// <summary>
        /// Release this memory associated with this Dnn super resolution model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnSuperresInvoke.cveDnnSuperResImplRelease(ref _ptr);
            }
        }
    }

    internal static partial class DnnSuperresInvoke
    {

        static DnnSuperresInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnSuperResImplCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnSuperResImplCreate2(IntPtr algorithm, int scale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplSetModel(IntPtr dnnSuperRes, IntPtr algorithm, int scale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplReadModel1(IntPtr dnnSuperRes, IntPtr path);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplReadModel2(IntPtr dnnSuperRes, IntPtr weights, IntPtr definition);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplUpsample(IntPtr dnnSuperRes, IntPtr img, IntPtr result);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplRelease(ref IntPtr dnnSuperRes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnSuperResImplGetScale(IntPtr dnnSuperRes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplGetAlgorithm(IntPtr dnnSuperRes, IntPtr algorithm);
    }
}
