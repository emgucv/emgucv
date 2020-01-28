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

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Parameters for the detectMarker process
    /// </summary>
    public struct DetectorParameters
    {
        /// <summary>
        /// Type of corner refinement method
        /// </summary>
        public enum RefinementMethod
        {
            /// <summary>
            /// Default corners
            /// </summary>
            None,
            /// <summary>
            /// Refine the corners using subpix
            /// </summary>
            Subpix,
            /// <summary>
            /// Refine the corners using the contour-points
            /// </summary>
            Contour   
        }


        /// <summary>
        /// minimum window size for adaptive thresholding before finding contours (default 3)
        /// </summary>
        public int AdaptiveThreshWinSizeMin;
        /// <summary>
        /// maximum window size for adaptive thresholding before finding contours (default 23).
        /// </summary>
        public int AdaptiveThreshWinSizeMax;
        /// <summary>
        /// increments from adaptiveThreshWinSizeMin to adaptiveThreshWinSizeMax during the thresholding (default 10).
        /// </summary>
        public int AdaptiveThreshWinSizeStep;
        /// <summary>
        /// constant for adaptive thresholding before finding contours (default 7)
        /// </summary>
        public double AdaptiveThreshConstant;
        /// <summary>
        ///  determine minimum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 0.03).
        /// </summary>
        public double MinMarkerPerimeterRate;
        /// <summary>
        /// determine maximum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 4.0).
        /// </summary>
        public double MaxMarkerPerimeterRate;
        /// <summary>
        /// minimum accuracy during the polygonal approximation process to determine which contours are squares.
        /// </summary>
        public double PolygonalApproxAccuracyRate;
        /// <summary>
        /// minimum distance between corners for detected markers relative to its perimeter (default 0.05)
        /// </summary>
        public double MinCornerDistanceRate;
        /// <summary>
        /// minimum distance of any corner to the image border for detected markers (in pixels) (default 3)
        /// </summary>
        public int MinDistanceToBorder;
        /// <summary>
        /// minimum mean distance beetween two marker corners to be considered similar, so that the smaller one is removed. The rate is relative to the smaller perimeter of the two markers (default 0.05).
        /// </summary>
        public double MinMarkerDistanceRate;
        /// <summary>
        /// Corner refinement method
        /// </summary>
        public RefinementMethod CornerRefinementMethod;
        /// <summary>
        /// window size for the corner refinement process (in pixels) (default 5).
        /// </summary>
        public int CornerRefinementWinSize;
        /// <summary>
        /// maximum number of iterations for stop criteria of the corner refinement process (default 30).
        /// </summary>
        public int CornerRefinementMaxIterations;
        /// <summary>
        /// minimum error for the stop criteria of the corner refinement process (default: 0.1)
        /// </summary>
        public double CornerRefinementMinAccuracy;
        /// <summary>
        /// number of bits of the marker border, i.e. marker border width (default 1).
        /// </summary>
        public int MarkerBorderBits;
        /// <summary>
        /// number of bits (per dimension) for each cell of the marker when removing the perspective (default 8).
        /// </summary>
        public int PerspectiveRemovePixelPerCell;
        /// <summary>
        /// width of the margin of pixels on each cell not considered for the determination of the cell bit. Represents the rate respect to the total size of the cell, i.e. perpectiveRemovePixelPerCell (default 0.13)
        /// </summary>
        public double PerspectiveRemoveIgnoredMarginPerCell;
        /// <summary>
        /// maximum number of accepted erroneous bits in the border (i.e. number of allowed white bits in the border). Represented as a rate respect to the total number of bits per marker (default 0.35).
        /// </summary>
        public double MaxErroneousBitsInBorderRate;
        /// <summary>
        ///  minimun standard deviation in pixels values during the decodification step to apply Otsu thresholding (otherwise, all the bits are set to 0 or 1 depending on mean higher than 128 or not) (default 5.0)
        /// </summary>
        public double MinOtsuStdDev;
        /// <summary>
        /// error correction rate respect to the maximun error correction capability for each dictionary. (default 0.6).
        /// </summary>
        public double ErrorCorrectionRate;

        /// <summary>
        /// Detection of quads can be done on a lower-resolution image, improving speed at a
        /// cost of pose accuracy and a slight decrease in detection rate.Decoding the binary payload is still
        /// done at full resolution.
        /// </summary>
        public float AprilTagQuadDecimate;
        /// <summary>
        /// What Gaussian blur should be applied to the segmented image (used for quad detection?)
        /// Parameter is the standard deviation in pixels.Very noisy images benefit from non-zero values(e.g. 0.8).
        /// </summary>
        public float AprilTagQuadSigma;

        /// <summary>
        /// reject quads containing too few pixels.
        /// </summary>
        public int AprilTagMinClusterPixels;
        /// <summary>
        /// how many corner candidates to consider when segmenting a group of pixels into a quad.
        /// </summary>
        public int AprilTagMaxNmaxima;
        /// <summary>
        /// Reject quads where pairs of edges have angles that are close to straight or close to
        /// 180 degrees.Zero means that no quads are rejected. (In radians).
        /// </summary>
        public float AprilTagCriticalRad;
        /// <summary>
        /// When fitting lines to the contours, what is the maximum mean squared error
        /// allowed? This is useful in rejecting contours that are far from being quad shaped; rejecting
        /// these quads "early" saves expensive decoding processing.
        /// </summary>
        public float AprilTagMaxLineFitMse;
        /// <summary>
        /// When we build our model of black &amp; white pixels, we add an extra check that
        /// the white model must be(overall) brighter than the black model.How much brighter? (in pixel values, [0, 255]).
        /// </summary>
        public int AprilTagMinWhiteBlackDiff;
        /// <summary>
        /// should the thresholded image be deglitched? Only useful for very noisy images
        /// </summary>
        public int AprilTagDeglitch;

        /// <summary>
        /// Get the detector parameters with default values
        /// </summary>
        /// <returns>The default detector parameters</returns>
        public static DetectorParameters GetDefault()
        {
            DetectorParameters p = new DetectorParameters();
            ArucoInvoke.cveArucoDetectorParametersGetDefault(ref p);
            return p;
        }
    }
}