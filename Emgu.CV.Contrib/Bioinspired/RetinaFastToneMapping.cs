//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.Bioinspired
{
    /// <summary>
    /// A wrapper class which allows the tone mapping algorithm of Meylan &amp; al(2007) to be used with OpenCV.
    /// </summary>
    public class RetinaFastToneMapping : SharedPtrObject
    {

        /// <summary>
        /// Create a wrapper class which allows the tone mapping algorithm of Meylan &amp; al(2007) to be used with OpenCV.
        /// </summary>
        /// <param name="inputSize">The size of the images to process</param>
        public RetinaFastToneMapping(Size inputSize)
        {
            _ptr = BioinspiredInvoke.cveRetinaFastToneMappingCreate(ref inputSize, ref _sharedPtr);
        }

        

        /// <summary>
        /// Applies a luminance correction (initially High Dynamic Range (HDR) tone mapping)
        /// </summary>
        /// <param name="inputImage">The input image to process RGB or gray levels</param>
        /// <param name="outputToneMappedImage">The output tone mapped image</param>
        public void ApplyFastToneMapping(
            IInputArray inputImage,
            IOutputArray outputToneMappedImage)
        {
            using (InputArray iaInputImage = inputImage.GetInputArray())
            using (OutputArray oaOutputToneMappedImage = outputToneMappedImage.GetOutputArray())
                BioinspiredInvoke.cveRetinaFastToneMappingApplyFastToneMapping(_ptr, iaInputImage, oaOutputToneMappedImage);
        }

        /// <summary>
        /// Updates tone mapping behaviors by adjusting the local luminance computation area
        /// </summary>
        /// <param name="photoreceptorsNeighborhoodRadius">The first stage local adaptation area</param>
        /// <param name="ganglioncellsNeighborhoodRadius">The second stage local adaptation area</param>
        /// <param name="meanLuminanceModulatorK">The factor applied to modulate the meanLuminance information (default is 1, see reference paper)</param>
        public void Setup(
            float photoreceptorsNeighborhoodRadius = 3.0f,
            float ganglioncellsNeighborhoodRadius = 1.0f,
            float meanLuminanceModulatorK = 1.0f)
        {
            BioinspiredInvoke.cveRetinaFastToneMappingSetup(
                _ptr, 
                photoreceptorsNeighborhoodRadius,
                ganglioncellsNeighborhoodRadius,
                meanLuminanceModulatorK);
        }

        /// <summary>
        /// Release all unmanaged memory associated with the RetinaFastToneMapping model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                BioinspiredInvoke.cveRetinaFastToneMappingRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }
}
