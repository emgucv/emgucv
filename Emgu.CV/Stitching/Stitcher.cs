//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Image Stitching.
    /// </summary>
    public partial class Stitcher : SharedPtrObject
    {
        /// <summary>
        /// The stitcher statis
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Ok.
            /// </summary>
            Ok = 0,
            /// <summary>
            /// Error, need more images.
            /// </summary>
            ErrNeedMoreImgs = 1,
            /// <summary>
            /// Error, homography estimateion failed.
            /// </summary>
            ErrHomographyEstFail = 2,
            /// <summary>
            /// Error, camera parameters adjustment failed.
            /// </summary>
            ErrCameraParamsAdjustFail = 3
        }

        /// <summary>
        /// Wave correction kind
        /// </summary>
        public enum WaveCorrectionType
        {
            /// <summary>
            /// horizontal
            /// </summary>
            Horiz,
            /// <summary>
            /// Vertical
            /// </summary>
            Vert
        }

        /// <summary>
        /// Stitch mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Mode for creating photo panoramas. Expects images under perspective transformation and projects resulting pano to sphere.
            /// </summary>
            Panorama = 0,

            /// <summary>
            /// Mode for composing scans. Expects images under affine transformation does not compensate exposure by default.
            /// </summary>
            Scans = 1,

        }

        /// <summary>
        /// Creates a Stitcher configured in one of the stitching modes.
        /// </summary>
        /// <param name="mode">Scenario for stitcher operation. This is usually determined by source of images to stitch and their transformation. </param>

        public Stitcher(Mode mode = Mode.Panorama)
        {
            _ptr = StitchingInvoke.cveStitcherCreate(mode, ref _sharedPtr);
        }

        /// <summary>
        /// Compute the panoramic images given the images
        /// </summary>
        /// <param name="images">The input images. This can be, for example, a VectorOfMat</param>
        /// <param name="pano">The panoramic image</param>
        /// <returns>The stitching status</returns>
        public Status Stitch(IInputArray images, IOutputArray pano)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (OutputArray oaPano = pano.GetOutputArray())
                return StitchingInvoke.cveStitcherStitch(_ptr, iaImages, oaPano);
        }

        /// <summary>
        /// These functions try to match the given images and to estimate rotations of each camera.
        /// </summary>
        /// <param name="images">Input images.</param>
        /// <param name="masks">Masks for each input image specifying where to look for keypoints (optional).</param>
        /// <returns>Status code.</returns>
        public Stitcher.Status EstimateTransform(IInputArrayOfArrays images, IInputArrayOfArrays masks = null)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (InputArray iaMasks = masks == null ? InputArray.GetEmpty() : masks.GetInputArray())
            {
                return StitchingInvoke.cveStitcherEstimateTransform(_ptr, iaImages, iaMasks);
            }
        }

        /// <summary>
        /// These functions try to match the given images and to estimate rotations of each camera.
        /// </summary>
        /// <param name="pano">Final pano.</param>
        /// <returns>Status code.</returns>
        public Stitcher.Status ComposePanorama(IOutputArray pano)
        {
            using (OutputArray oaPano = pano.GetOutputArray())
            {
                return StitchingInvoke.cveStitcherComposePanorama1(_ptr, oaPano);
            }
        }

        /// <summary>
        /// These functions try to compose the given images (or images stored internally from the other function calls) into the final pano under the assumption that the image transformations were estimated before.
        /// </summary>
        /// <param name="images">Input images</param>
        /// <param name="pano">Final pano.</param>
        /// <returns>Status code.</returns>
        public Stitcher.Status ComposePanorama(IInputArrayOfArrays images, IOutputArray pano)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (OutputArray oaPano = pano.GetOutputArray())
                return StitchingInvoke.cveStitcherComposePanorama2(_ptr, iaImages, oaPano);
        }

        /// <summary>
        /// Set the features finder for this stitcher.
        /// </summary>
        /// <param name="finder">The features finder</param>
        public void SetFeaturesFinder(Features2D.Feature2D finder)
        {
            StitchingInvoke.cveStitcherSetFeaturesFinder(_ptr, finder.Feature2DPtr);
        }

        /// <summary>
        /// Set the exposure compensator for this stitcher.
        /// </summary>
        /// <param name="exposureCompensator">The exposure compensator</param>
        public void SetExposureCompensator(ExposureCompensator exposureCompensator)
        {
            StitchingInvoke.cveStitcherSetExposureCompensator(_ptr, exposureCompensator.ExposureCompensatorPtr);
        }

        /// <summary>
        /// Set the bundle adjuster for this stitcher
        /// </summary>
        /// <param name="bundleAdjuster">The bundle adjuster</param>
        public void SetBundleAdjusterCompensator(BundleAdjusterBase bundleAdjuster)
        {
            StitchingInvoke.cveStitcherSetBundleAdjuster(_ptr, bundleAdjuster.BundleAdjusterPtr);
        }

        /// <summary>
        /// Set the seam finder for this stitcher
        /// </summary>
        /// <param name="seamFinder">The seam finder</param>
        public void SetSeamFinder(SeamFinder seamFinder)
        {
            StitchingInvoke.cveStitcherSetSeamFinder(_ptr, seamFinder.SeamFinderPtr);
        }

        /// <summary>
        /// Set the estimator for this stitcher
        /// </summary>
        /// <param name="estimator">The estimator</param>
        public void SetEstimator(Estimator estimator)
        {
            StitchingInvoke.cveStitcherSetEstimator(_ptr, estimator.EstimatorPtr);
        }

        /// <summary>
        /// Set the features matcher for this stitcher
        /// </summary>
        /// <param name="featuresMatcher">The features matcher</param>
        public void SetFeaturesMatcher(FeaturesMatcher featuresMatcher)
        {
            StitchingInvoke.cveStitcherSetFeaturesMatcher(_ptr, featuresMatcher.FeaturesMatcherPtr);
        }

        /// <summary>
        /// Set the warper creator for this stitcher.
        /// </summary>
        /// <param name="warperCreator">The warper creator</param>
        public void SetWarper(WarperCreator warperCreator)
        {
            StitchingInvoke.cveStitcherSetWarper(_ptr, warperCreator.WarperCreatorPtr);
        }

        /// <summary>
        /// Set the blender for this stitcher
        /// </summary>
        /// <param name="blender">The blender</param>
        public void SetBlender(Blender blender)
        {
            StitchingInvoke.cveStitcherSetBlender(_ptr, blender.BlenderPtr);
        }

        /// <summary>
        /// Get or Set a flag to indicate if the stitcher should apply wave correction
        /// </summary>
        public bool WaveCorrection
        {
            get { return StitchingInvoke.cveStitcherGetWaveCorrection(_ptr); }
            set { StitchingInvoke.cveStitcherSetWaveCorrection(_ptr, value); }
        }

        /// <summary>
        /// The wave correction type.
        /// </summary>
        public WaveCorrectionType WaveCorrectionKind
        {
            get { return StitchingInvoke.cveStitcherGetWaveCorrectionKind(_ptr); }
            set { StitchingInvoke.cveStitcherSetWaveCorrectionKind(_ptr, value); }
        }

        /// <summary>
        /// Get or set the pano confidence threshold
        /// </summary>
        public double PanoConfidenceThresh
        {
            get { return StitchingInvoke.cveStitcherGetPanoConfidenceThresh(_ptr); }
            set { StitchingInvoke.cveStitcherSetPanoConfidenceThresh(_ptr, value); }
        }

        /// <summary>
        /// Get or Set the compositing resolution
        /// </summary>
        public double CompositingResol
        {
            get { return StitchingInvoke.cveStitcherGetCompositingResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetCompositingResol(_ptr, value); }
        }

        /// <summary>
        /// Get or Set the seam estimation resolution
        /// </summary>
        public double SeamEstimationResol
        {
            get { return StitchingInvoke.cveStitcherGetSeamEstimationResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetSeamEstimationResol(_ptr, value); }
        }

        /// <summary>
        /// Get or set the registration resolution
        /// </summary>
        public double RegistrationResol
        {
            get { return StitchingInvoke.cveStitcherGetRegistrationResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetRegistrationResol(_ptr, value); }
        }

        /// <summary>
        /// Get or Set the interpolation type.
        /// </summary>
        public CvEnum.Inter InterpolationFlags
        {
            get { return StitchingInvoke.cveStitcherGetInterpolationFlags(_ptr); }
            set { StitchingInvoke.cveStitcherSetInterpolationFlags(_ptr, value);}
        }

        /// <summary>
        /// Release memory associated with this stitcher
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                StitchingInvoke.cveStitcherRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Entry points to the Open CV Stitching module.
    /// </summary>
    public static partial class StitchingInvoke
    {

        static StitchingInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStitcherCreate(
            Stitcher.Mode model,
            ref IntPtr sharedPtr
           );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.Status cveStitcherStitch(IntPtr stitcherWrapper, IntPtr images, IntPtr pano);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetFeaturesFinder(IntPtr stitcherWrapper, IntPtr finder);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWarper(IntPtr stitcher, IntPtr creator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetBlender(IntPtr stitcher, IntPtr b);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetExposureCompensator(IntPtr stitcher, IntPtr exposureComp);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetBundleAdjuster(IntPtr stitcher, IntPtr bundleAdjuster);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetSeamFinder(IntPtr stitcher, IntPtr seamFinder);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetEstimator(IntPtr stitcher, IntPtr estimator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetFeaturesMatcher(IntPtr stitcher, IntPtr featuresMatcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherRelease(ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWaveCorrection(
            IntPtr stitcher,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool flag);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStitcherGetWaveCorrection(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWaveCorrectionKind(IntPtr stitcher, Stitcher.WaveCorrectionType kind);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.WaveCorrectionType cveStitcherGetWaveCorrectionKind(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetPanoConfidenceThresh(IntPtr stitcher, double confThresh);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetPanoConfidenceThresh(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetCompositingResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetCompositingResol(IntPtr stitcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]

        internal static extern void cveStitcherSetSeamEstimationResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetSeamEstimationResol(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetRegistrationResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetRegistrationResol(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CvEnum.Inter cveStitcherGetInterpolationFlags(IntPtr stitcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetInterpolationFlags(IntPtr stitcher, CvEnum.Inter interpFlags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.Status cveStitcherEstimateTransform(IntPtr stitcher, IntPtr images, IntPtr masks);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.Status cveStitcherComposePanorama1(IntPtr stitcher, IntPtr pano);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.Status cveStitcherComposePanorama2(IntPtr stitcher, IntPtr images, IntPtr pano);
    }
}
