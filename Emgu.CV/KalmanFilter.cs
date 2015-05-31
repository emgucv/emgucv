//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Kalman Filter 
   /// </summary>
   public partial class KalmanFilter : UnmanagedObject
   {
      public KalmanFilter(int dynamParams, int measureParams, int controlParams, int type)
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

      public Mat Correct(Mat measurement)
      {
         return new Mat(CvInvoke.cveKalmanFilterCorrect(_ptr, measurement), false);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveKalmanFilterRelease(ref _ptr);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, int type);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveKalmanFilterRelease(ref IntPtr filter);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterPredict(IntPtr kalman, IntPtr control);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveKalmanFilterCorrect(IntPtr kalman, IntPtr measurement);
   }
}
