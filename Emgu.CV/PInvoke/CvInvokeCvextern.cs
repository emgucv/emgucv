//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    public partial class CvInvoke
    {
        /// <summary>
        /// This function retrieve the Open CV structure sizes in unmanaged code
        /// </summary>
        /// <returns>The structure that will hold the Open CV structure sizes</returns>
        public static CvStructSizes GetCvStructSizes()
        {
            CvStructSizes sizes = new CvStructSizes();
            cveGetCvStructSizes(ref sizes);
            return sizes;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetCvStructSizes(ref CvStructSizes sizes);

        /// <summary>
        /// Get the dictionary that hold the Open CV build flags. The key is a String and the value is type double. If it is a flag, 0 means false and 1 means true 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, double> ConfigDict
        {
            get
            {
                using (VectorOfCvString vs = new VectorOfCvString())
                using (VectorOfDouble vd = new VectorOfDouble())
                {
                    cveGetConfigDict(vs, vd);

                    String[] keys = vs.ToArray();
                    double[] values = vd.ToArray();

                    Dictionary<String, double> dict = new Dictionary<string, double>();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        dict[keys[i]] = values[i];
                    }

                    return dict;
                }
                
            }
            
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetConfigDict(IntPtr names, IntPtr values);

        /*
        public static void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, MCvScalar color)
        {
           TestDrawLine(img, startX, startY, endX, endY, color.v0, color.v1, color.v2, color.v3);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="testDrawLine")]
        private static extern void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, double v0, double v1, double v2, double v3);

        /// <summary>
        /// Implements the chamfer matching algorithm on images taking into account both distance from
        /// the template pixels to the nearest pixels and orientation alignment between template and image
        /// contours.
        /// </summary>
        /// <param name="img">The edge image where search is performed</param>
        /// <param name="templ">The template (an edge image)</param>
        /// <param name="contours">The output contours</param>
        /// <param name="cost">The cost associated with the matching</param>
        /// <param name="templScale">The template scale</param>
        /// <param name="maxMatches">The maximum number of matches</param>
        /// <param name="minMatchDistance">The minimum match distance</param>
        /// <param name="padX">PadX</param>
        /// <param name="padY">PadY</param>
        /// <param name="scales">Scales</param>
        /// <param name="minScale">Minimum scale</param>
        /// <param name="maxScale">Maximum scale</param>
        /// <param name="orientationWeight">Orientation weight</param>
        /// <param name="truncate">Truncate</param>
        /// <returns>The number of matches</returns>
        public static int ChamferMatching(Mat img, Mat templ,
           out Point[][] contours, out float[] cost,
           double templScale = 1, int maxMatches = 20,
           double minMatchDistance = 1.0, int padX = 3,
           int padY = 3, int scales = 5, double minScale = 0.6, double maxScale = 1.6,
           double orientationWeight = 0.5, double truncate = 20)
        {
           using (Emgu.CV.Util.VectorOfVectorOfPoint vecOfVecOfPoint = new Util.VectorOfVectorOfPoint())
           using (Emgu.CV.Util.VectorOfFloat vecOfFloat = new Util.VectorOfFloat())
           {
              int count = cveChamferMatching(img, templ, vecOfVecOfPoint, vecOfFloat, templScale, maxMatches, minMatchDistance, padX, padY, scales, minScale, maxScale, orientationWeight, truncate);
              contours = vecOfVecOfPoint.ToArrayOfArray();
              cost = vecOfFloat.ToArray();
              return count;
           }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveChamferMatching(
           IntPtr img, IntPtr templ,
           IntPtr results, IntPtr cost,
           double templScale, int maxMatches,
           double minMatchDistance, int padX,
           int padY, int scales, double minScale, double maxScale,
           double orientationWeight, double truncate);
        */


        /*
        /// <summary>
        /// Applies the adaptive bilateral filter to an image.
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The destination image; will have the same size and the same type as src</param>
        /// <param name="ksize">The kernel size. This is the neighborhood where the local variance will be calculated, and where pixels will contribute (in a weighted manner).</param>
        /// <param name="sigmaSpace">Filter sigma in the coordinate space. Larger value of the parameter means that farther pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d>0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace.</param>
        /// <param name="maxSigmaColor">Maximum allowed sigma color (will clamp the value calculated in the ksize neighborhood. Larger value of the parameter means that more dissimilar pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d>0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace. Use 20 for default.</param>
        /// <param name="anchor">Use (-1, -1) for default</param>
        /// <param name="borderType">Pixel extrapolation method.</param>
        public static void AdaptiveBilateralFilter(IInputArray src, IOutputArray dst, Size ksize, double sigmaSpace, double maxSigmaColor, Point anchor, CvEnum.BorderType borderType = CvEnum.BorderType.Default)
        {
           cveAdaptiveBilateralFilter(src.InputArrayPtr, dst.OutputArrayPtr, ref ksize, sigmaSpace, maxSigmaColor, ref anchor, borderType);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAdaptiveBilateralFilter(IntPtr src, IntPtr dst, ref Size ksize, double sigmaSpace, double maxSigmaColor, ref Point anchor, CvEnum.BorderType borderType);
        */
    }
}
