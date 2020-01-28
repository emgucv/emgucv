//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Tracking
{
    /// <summary>
    /// KCF is a novel tracking framework that utilizes properties of circulant matrix to enhance the processing speed.
    /// The original paper of KCF is available at http://home.isr.uc.pt/~henriques/circulant/index.html
    /// as well as the matlab implementation.
    /// </summary>
    public class TrackerKCF : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Feature type to be used in the tracking grayscale, colornames, compressed color-names
        /// The modes available now:
        /// -   "GRAY" -- Use grayscale values as the feature
        /// -   "CN" -- Color-names feature
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Grayscale
            /// </summary>
            Gray = 1,
            /// <summary>
            /// Color
            /// </summary>
            Cn = 2,
            /// <summary>
            /// Custom
            /// </summary>
            Custom = 4
        }

        /// <summary>
        /// Creates a KCF Tracker
        /// </summary>
        /// <param name="detectThresh">detection confidence threshold</param>
        /// <param name="sigma">gaussian kernel bandwidth</param>
        /// <param name="lambda">regularization</param>
        /// <param name="interpFactor">linear interpolation factor for adaptation</param>
        /// <param name="outputSigmaFactor">spatial bandwidth (proportional to target)</param>
        /// <param name="pcaLearningRate">compression learning rate</param>
        /// <param name="resize">activate the resize feature to improve the processing speed</param>
        /// <param name="splitCoeff">split the training coefficients into two matrices</param>
        /// <param name="wrapKernel">wrap around the kernel values</param>
        /// <param name="compressFeature">activate the pca method to compress the features</param>
        /// <param name="maxPatchSize">threshold for the ROI size</param>
        /// <param name="compressedSize">feature size after compression</param>
        /// <param name="descPca">compressed descriptors of TrackerKCF::MODE</param>
        /// <param name="descNpca">non-compressed descriptors of TrackerKCF::MODE</param>
        public TrackerKCF(
            float detectThresh = 0.5f,
            float sigma = 0.2f,
            float lambda = 0.01f,
            float interpFactor = 0.075f,
            float outputSigmaFactor = 1.0f/16.0f,
            float pcaLearningRate = 0.15f,
            bool resize = true,
            bool splitCoeff = true,
            bool wrapKernel = false,
            bool compressFeature = true,
            int maxPatchSize = 80*80,
            int compressedSize = 2,
            Mode descPca = Mode.Cn,
            Mode descNpca = Mode.Gray)
        {
            _ptr = ContribInvoke.cveTrackerKCFCreate(
                detectThresh,
                sigma,
                lambda,
                interpFactor,
                outputSigmaFactor,
                pcaLearningRate,
                resize,
                splitCoeff,
                wrapKernel,
                compressFeature,
                maxPatchSize,
                compressedSize,
                descPca,
                descNpca,
                ref _trackerPtr,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerKCFRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }
}
