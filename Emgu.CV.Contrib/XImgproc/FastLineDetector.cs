//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.XImgproc
{

    /// <summary>
    /// Class implementing the FLD (Fast Line Detector) algorithm
    /// </summary>
    public class FastLineDetector : SharedPtrObject
    {
        /// <summary>
        /// Initializes a new instance of the FastLineDetector object.
        /// </summary>
        /// <param name="lengthThreshold">Segment shorter than this will be discarded.</param>
        /// <param name="distanceThreshold">A point placed from a hypothesis line segment farther than this will be regarded as an outlier.</param>
        /// <param name="cannyThreshold1">First threshold for hysteresis procedure in Canny().</param>
        /// <param name="cannyThreshold2">Second threshold for hysteresis procedure in Canny().</param>
        /// <param name="cannyApertureSize">Aperture size for the Sobel operator in Canny().</param>
        /// <param name="doMerge">If true, incremental merging of segments will be performed </param>
        public FastLineDetector(
            int lengthThreshold = 10,
            float distanceThreshold = 1.414213562f,
            double cannyThreshold1 = 50.0,
            double cannyThreshold2 = 50.0,
            int cannyApertureSize = 3,
            bool doMerge = false)
        {
            _ptr = XImgprocInvoke.cveFastLineDetectorCreate(
                lengthThreshold,
                distanceThreshold,
                cannyThreshold1,
                cannyThreshold2,
                cannyApertureSize,
                doMerge,
                ref _sharedPtr);
        }

        /// <summary>
        /// Finds lines in the input image.
        /// </summary>
        /// <param name="image">Image to detect lines in.</param>
        /// <returns>The detected line segments</returns>
        public LineSegment2DF[] Detect(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (Mat matLines = new Mat())
            using (OutputArray oaLines = matLines.GetOutputArray())
            {
                // Process image
                XImgprocInvoke.cveFastLineDetectorDetect(_ptr, iaImage, oaLines);

                // Convert data in Mat to list of LineSegment2DF objects
                float[] pointData = new float[matLines.Total.ToInt32() * matLines.ElementSize / 4];
                LineSegment2DF[] lines = new LineSegment2DF[pointData.Length / 4];
                matLines.CopyTo(pointData);

                // Each line is represented by 4 floats
                for (int i = 0; i < pointData.Length / 4; i++)
                {
                    lines[i] = new LineSegment2DF(
                        new PointF(pointData[i * 4], pointData[(i * 4) + 1]),
                        new PointF(pointData[(i * 4) + 2], pointData[(i * 4) + 3]));
                }

                return lines;
            }
        }

        /// <summary>
        /// Draws the line segments on a given image.
        /// </summary>
        /// <param name="image">The image, where the lines will be drawn. Should be bigger or equal to the image, where the lines were found.</param>
        /// <param name="lines">A vector of the lines that needed to be drawn.</param>
        /// <param name="drawArrows">If true, arrow heads will be drawn.</param>
        public void DrawSegments(IInputOutputArray image, LineSegment2DF[] lines, bool drawArrows = false)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (Mat matLines = new Mat(lines.Length, 1, DepthType.Cv32F, 4))
            {
                float[] pointData = new float[lines.Length * 4];
                for (int i = 0; i < lines.Length; i++)
                {
                    pointData[i * 4] = lines[i].P1.X;
                    pointData[(i * 4) + 1] = lines[i].P1.Y;
                    pointData[(i * 4) + 2] = lines[i].P2.X;
                    pointData[(i * 4) + 3] = lines[i].P2.Y;
                }
                matLines.SetTo(pointData);
                using (InputArray iaLines = matLines.GetInputArray())
                {
                    XImgprocInvoke.cveFastLineDetectorDrawSegments(_ptr, ioaImage, iaLines, drawArrows);
                }
            }
        }

        /// <inheritdoc />
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveFastLineDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFastLineDetectorCreate(
            int lengthThreshold,
            float distanceThreshold,
            double cannyThreshold1,
            double cannyThreshold2,
            int cannyApertureSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool doMerge,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFastLineDetectorDetect(IntPtr fld, IntPtr image, IntPtr lines);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFastLineDetectorDrawSegments(
            IntPtr fld,
            IntPtr image,
            IntPtr lines,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool drawArrow);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFastLineDetectorRelease(ref IntPtr fld);
    }
}
