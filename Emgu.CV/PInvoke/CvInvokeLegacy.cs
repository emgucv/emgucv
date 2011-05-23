//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSnakeImage(
         IntPtr image,
         IntPtr points,
         int length,
         [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
         [MarshalAs(UnmanagedType.LPArray)] float[] beta,
         [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
         int coeffUsage,
         Size win,
         MCvTermCriteria criteria,
         int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSnakeImage(
         IntPtr image,
         [In, Out]
         Point[] points,
         int length,
         [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
         [MarshalAs(UnmanagedType.LPArray)] float[] beta,
         [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
         int coeffUsage,
         Size win,
         MCvTermCriteria criteria,
         int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If true, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      public static void cvSnakeImage(
         IntPtr image,
         IntPtr points,
         int length,
         float[] alpha,
         float[] beta,
         float[] gamma,
         int coeffUsage,
         Size win,
         MCvTermCriteria criteria,
         bool calcGradient)
      {
         cvSnakeImage(
            image,
            points,
            length,
            alpha,
            beta,
            gamma,
            coeffUsage,
            win,
            criteria,
            calcGradient ? 1 : 0);
      }

      #region Eigen Objects
      #region cvEigenDecomposite function
      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigInput">Pointer to the array of IplImage input objects</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <returns>Calculated coefficients; an output parameter</returns>
      public static float[] cvEigenDecomposite(
         IntPtr obj,
         IntPtr[] eigInput,
         IntPtr avg)
      {
         float[] coeffs = new float[eigInput.Length];
         cvEigenDecomposite(
             obj,
             eigInput.Length,
             eigInput,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             IntPtr.Zero,
             avg,
             coeffs);
         return coeffs;
      }

      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigenvecCount">Number of eigen objects</param>
      /// <param name="eigInput">Pointer either to the array of IplImage input objects or to the read callback function according to the value of the parameter <paramref name="ioFlags"/></param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <param name="coeffs">Calculated coefficients; an output parameter</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvEigenDecomposite(
         IntPtr obj,
         int eigenvecCount,
         IntPtr[] eigInput,
         CvEnum.EIGOBJ_TYPE ioFlags,
         IntPtr userData,
         IntPtr avg,
         float[] coeffs);

      #endregion

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="input">Pointer to the array of IplImage input objects </param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      /// <param name="eigVecs">Pointer either to the array of eigen objects</param>
      /// <returns>Pointer either to the array of eigen objects or to the write callback function</returns>
      public static void cvCalcEigenObjects(
         IntPtr[] input,
         ref MCvTermCriteria calcLimit,
         IntPtr[] eigVecs,
         float[] eigVals,
         IntPtr avg)
      {
         cvCalcEigenObjects(
             input.Length,
             input,
             eigVecs,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             0,
             IntPtr.Zero,
             ref calcLimit,
             avg,
             eigVals);
      }

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="nObjects">Number of source objects</param>
      /// <param name="input">Pointer either to the array of IplImage input objects or to the read callback function</param>
      /// <param name="output">Pointer either to the array of eigen objects or to the write callback function</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="ioBufSize">Input/output buffer size in bytes. The size is zero, if unknown</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvCalcEigenObjects(
         int nObjects,
         IntPtr[] input,
         IntPtr[] output,
         CvEnum.EIGOBJ_TYPE ioFlags,
         int ioBufSize,
         IntPtr userData,
         ref MCvTermCriteria calcLimit,
         IntPtr avg,
         float[] eigVals);

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. 
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      public static void cvEigenProjection(
         IntPtr[] inputVecs,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj)
      {
         CvInvoke.cvEigenProjection(
             inputVecs,
             inputVecs.Length,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             IntPtr.Zero,
             coeffs,
             avg,
             proj);
      }

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. Depending on io_flags parameter it may be used either in direct access or callback mode.
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="eigenvecCount">Number of eigenvectors</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userdata">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void cvEigenProjection(
         IntPtr[] inputVecs,
         int eigenvecCount,
         CvEnum.EIGOBJ_TYPE ioFlags,
         IntPtr userdata,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj);
      #endregion

      #region Condensation
      /// <summary>
      /// Creates CvConDensation structure and returns pointer to the structure
      /// </summary>
      /// <param name="dynamParams">Dimension of the state vector</param>
      /// <param name="measureParams">Dimension of the measurement vector</param>
      /// <param name="sampleCount">Number of samples</param>
      /// <returns>Pointer to the CvConDensation structure</returns>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateConDensation(int dynamParams, int measureParams, int sampleCount);

      /// <summary>
      /// Releases the structure CvConDensation (see cvConDensation) and frees all memory previously allocated for the structure. 
      /// </summary>
      /// <param name="condens">Pointer to the CvConDensation structure</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseConDensation(ref IntPtr condens);

      /// <summary>
      /// Fills the samples arrays in the structure CvConDensation with values within specified ranges. 
      /// </summary>
      /// <param name="condens">Pointer to a structure to be initialized</param>
      /// <param name="lowerBound">Vector of the lower boundary for each dimension</param>
      /// <param name="upperBound">Vector of the upper boundary for each dimension</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConDensInitSampleSet(IntPtr condens, IntPtr lowerBound, IntPtr upperBound);

      /// <summary>
      /// Estimates the subsequent stochastic model state from its current state
      /// </summary>
      /// <param name="condens">Pointer to the structure to be updated</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConDensUpdateByTime(IntPtr condens);
      #endregion

      /// <summary>
      /// Calculates 2D pair-wise geometrical histogram (PGH), described in [Iivarinen97], for the contour. The algorithm considers every pair of the contour edges. The angle between the edges and the minimum/maximum distances are determined for every pair. To do this each of the edges in turn is taken as the base, while the function loops through all the other edges. When the base edge and any other edge are considered, the minimum and maximum distances from the points on the non-base edge and line of the base edge are selected. The angle between the edges defines the row of the histogram in which all the bins that correspond to the distance between the calculated minimum and maximum distances are incremented (that is, the histogram is transposed relatively to [Iivarninen97] definition). The histogram can be used for contour matching
      /// </summary>
      /// <param name="contour">Input contour. Currently, only integer point coordinates are allowed</param>
      /// <param name="hist">Calculated histogram; must be two-dimensional</param>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcPGH(IntPtr contour, IntPtr hist);

      /// <summary>
      /// Checks planar subdivision for correctness. It is not an absolute check, but it verifies some relations between quad-edges
      /// </summary>
      /// <param name="subdiv">Pointer to the MCvSubdiv2D</param>
      /// <returns>True if valid, false otherwise</returns>
      [DllImport(OPENCV_LEGACY_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool icvSubdiv2DCheck(IntPtr subdiv);
   }
}
