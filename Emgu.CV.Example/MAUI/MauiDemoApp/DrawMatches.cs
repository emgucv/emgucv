//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using PointF = System.Drawing.PointF;
using Point = System.Drawing.Point;

namespace FeatureMatchingExample
{
    public static class DrawMatches
    {
        public static void FindMatch(
            Mat modelImage, 
            Mat observedImage, 
            Feature2D featureDetectorExtractor,
            out long matchTime, 
            out VectorOfKeyPoint modelKeyPoints, 
            out VectorOfKeyPoint observedKeyPoints, 
            VectorOfVectorOfDMatch matches, 
            out Mat mask, 
            out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.80;

            Stopwatch watch;
            homography = null;

            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();

            using (UMat uModelImage = modelImage.GetUMat(AccessType.Read))
            using (UMat uObservedImage = observedImage.GetUMat(AccessType.Read))
            {
                //extract features from the object image
                using Mat modelDescriptors = new Mat();
                featureDetectorExtractor.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                using Mat observedDescriptors = new Mat();
                featureDetectorExtractor.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);

                // Brute force, slower but more accurate
                // You can use KDTree for faster matching with slight loss in accuracy
                using (Emgu.CV.Flann.LinearIndexParams ip = new Emgu.CV.Flann.LinearIndexParams()) 
                using (Emgu.CV.Flann.SearchParams sp = new SearchParams())
                using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
                {
                    matcher.Add(modelDescriptors);

                    matcher.KnnMatch(observedDescriptors, matches, k, null);
                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));
                    FeaturesToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                    int nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        nonZeroCount = FeaturesToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                            matches, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                            homography = FeaturesToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                                observedKeyPoints, matches, mask, 2);
                    }
                }
                watch.Stop();

            }
            matchTime = watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <param name="featureDetectorExtractor">The feature detector extractor</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        public static Mat Draw(Mat modelImage, Mat observedImage, Feature2D featureDetectorExtractor, out long matchTime)
        {
            return Draw(modelImage, observedImage, featureDetectorExtractor, out matchTime, out _, out _);
        }

        /// <summary>
        /// Same as <see cref="Draw(Mat, Mat, Feature2D, out long)"/> but also reports the number of
        /// inlier matches and whether the object was located (a homography was found).
        /// </summary>
        public static Mat Draw(Mat modelImage, Mat observedImage, Feature2D featureDetectorExtractor, out long matchTime, out int matchCount, out bool objectLocated)
        {
            Mat homography;
            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, featureDetectorExtractor, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
                   out mask, out homography);

                matchCount = mask == null ? 0 : CvInvoke.CountNonZero(mask);
                objectLocated = homography != null;

                return Draw(modelImage, modelKeyPoints, observedImage, observedKeyPoints, matches, mask, homography,
                   new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), new MCvScalar(255, 0, 0, 255), out _);
            }
        }

        /// <summary>
        /// Draw the result of <see cref="FindMatch"/>: the model and observed images side by side, the matched
        /// features and, if a homography was found, the projected outline of the model in the observed image.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="modelKeyPoints">The model key points</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="observedKeyPoints">The observed key points</param>
        /// <param name="matches">The matches</param>
        /// <param name="mask">The match mask, non-zero for the matches to draw</param>
        /// <param name="homography">The homography, or null if the model was not located</param>
        /// <param name="matchColor">The color of the match lines</param>
        /// <param name="singlePointColor">The color of the unmatched key points</param>
        /// <param name="outlineColor">The color of the projected outline</param>
        /// <param name="outline">The projected outline in the observed image's coordinates, or null if the model was not located</param>
        /// <returns>The drawing</returns>
        public static Mat Draw(
            Mat modelImage, VectorOfKeyPoint modelKeyPoints,
            Mat observedImage, VectorOfKeyPoint observedKeyPoints,
            VectorOfVectorOfDMatch matches, Mat mask, Mat homography,
            MCvScalar matchColor, MCvScalar singlePointColor, MCvScalar outlineColor,
            out Point[] outline)
        {
            outline = null;
            //Draw the matched keypoints
            Mat result = new Mat();
            FeaturesToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
               matches, result, matchColor, singlePointColor, mask);

            #region draw the projected region on the image

            if (homography != null)
            {
                //draw a rectangle along the projected model
                Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                PointF[] pts = new PointF[]
                {
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Right, rect.Top),
                    new PointF(rect.Left, rect.Top)
                };
                pts = CvInvoke.PerspectiveTransform(pts, homography);

                // The observed image is drawn on the left, so its coordinates are unchanged.
                outline = new Point[pts.Length];
                for (int i = 0; i < outline.Length; i++)
                    outline[i] = Point.Round(pts[i]);

                using (VectorOfPoint vp = new VectorOfPoint(outline))
                {
                    CvInvoke.Polylines(result, vp, true, outlineColor, 5);
                }
            }
            #endregion

            return result;
        }
    }
}
