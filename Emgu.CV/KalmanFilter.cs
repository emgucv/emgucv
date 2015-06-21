//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// The class implements a standard Kalman filter. However, you can modify transitionMatrix, controlMatrix, and measurementMatrix to get
   /// an extended Kalman filter functionality.
   /// </summary>
   public partial class KalmanFilter : UnmanagedObject
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="KalmanFilter"/> class.
      /// </summary>
      /// <param name="dynamParams">Dimensionality of the state.</param>
      /// <param name="measureParams">Dimensionality of the measurement.</param>
      /// <param name="controlParams">Dimensionality of the control vector.</param>
      /// <param name="type">Type of the created matrices that should be Cv32F or Cv64F</param>
      public KalmanFilter(int dynamParams, int measureParams, int controlParams, DepthType type = DepthType.Cv32F)
      {
         _ptr = CvInvoke.cveKalmanFilterCreate(dynamParams, measureParams, controlParams, type);
      }

      /// <summary>
      /// Perform the predict operation using the option control input
      /// </summary>
      /// <param name="control">The control.</param>
      /// <returns>The predicted state. </returns>
      public Mat Predict(Mat control = null)
      {
         return new Mat(CvInvoke.cveKalmanFilterPredict(_ptr, control), false);
      }

      /// <summary>
      /// Updates the predicted state from the measurement.
      /// </summary>
      /// <param name="measurement">The measured system parameters</param>
      /// <returns></returns>
      public Mat Correct(Mat measurement)
      {
         return new Mat(CvInvoke.cveKalmanFilterCorrect(_ptr, measurement), false);
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveKalmanFilterRelease(ref _ptr);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, DepthType type);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveKalmanFilterRelease(ref IntPtr filter);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterPredict(IntPtr kalman, IntPtr control);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterCorrect(IntPtr kalman, IntPtr measurement);
   }
}
