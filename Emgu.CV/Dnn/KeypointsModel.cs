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
using System.Drawing;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// This class represents high-level API for keypoints models.
    /// </summary>
    public partial class KeypointsModel : Model
    {
        /// <summary>
        /// Create a new keypoints model
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public KeypointsModel(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnKeypointsModelCreate1(
                    csModel,
                    csConfig,
                    ref _model);
            }
        }

        /// <summary>
        /// Create model from deep learning network.
        /// </summary>
        /// <param name="net">DNN Network</param>
        public KeypointsModel(Net net)
        {

            _ptr = DnnInvoke.cveDnnKeypointsModelCreate2(
                net,
                ref _model);

        }

        /// <summary>
        /// Given the input frame, create input blob, run net.
        /// </summary>
        /// <param name="frame">The input image.</param>
        /// <param name="thresh">minimum confidence threshold to select a keypoint</param>
        /// <returns>A vector holding the x and y coordinates of each detected keypoint</returns>
        public PointF[] Estimate(IInputArray frame, float thresh = 0.5f)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            using (VectorOfPointF vpf = new VectorOfPointF())
            {
                DnnInvoke.cveDnnKeypointsModelEstimate(
                    _ptr,
                    iaFrame,
                    vpf,
                    thresh);
                return vpf.ToArray();
            }
        }



        /// <summary>
        /// Release the memory associated with this keypoints model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnKeypointsModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;
        }

    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnKeypointsModelCreate1(IntPtr model, IntPtr config, ref IntPtr baseModel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnKeypointsModelCreate2(IntPtr network, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnKeypointsModelRelease(ref IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnKeypointsModelEstimate(
            IntPtr keypointsModel,
            IntPtr frame,
            IntPtr keypoints,
            float thresh);

    }
}
