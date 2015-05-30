//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// The motion history class
   /// </summary>
   /// <remarks>
   /// For help on using this class, take a look at the Motion Detection example
   /// </remarks>
   public class MotionHistory : DisposableObject
   {
      //private IInputArray _foregroundMask;
      private double _mhiDuration;
      private Mat _mhi;
      private Mat _mask = new Mat();
      private Mat _orientation = new Mat();
      
      private DateTime _initTime;
      private DateTime _lastTime;
      private double _maxTimeDelta;
      private double _minTimeDelta;


      /// <summary>
      /// The motion mask. 
      /// Do not dispose this image.
      /// </summary>
      public Mat Mask
      {
         get
         {
            return _mask;
         }
      }

      /// <summary>
      /// Create a motion history object
      /// </summary>
      /// <param name="mhiDuration">In second, the duration of motion history you wants to keep</param>
      /// <param name="maxTimeDelta">In second. Any change happens between a time interval greater than this will not be considered</param>
      /// <param name="minTimeDelta">In second. Any change happens between a time interval smaller than this will not be considered.</param>
      public MotionHistory(double mhiDuration, double maxTimeDelta, double minTimeDelta)
         : this (mhiDuration, maxTimeDelta, minTimeDelta, DateTime.Now)
      {
      }

      /// <summary>
      /// Create a motion history object
      /// </summary>
      /// <param name="mhiDuration">In second, the duration of motion history you wants to keep</param>
      /// <param name="maxTimeDelta">In second. Any change happens between a time interval larger than this will not be considered</param>
      /// <param name="minTimeDelta">In second. Any change happens between a time interval smaller than this will not be considered.</param>
      /// <param name="startTime">The start time of the motion history</param>
      public MotionHistory(double mhiDuration, double maxTimeDelta, double minTimeDelta, DateTime startTime)
      {
         _mhiDuration = mhiDuration;
         _initTime = startTime;
         _maxTimeDelta = maxTimeDelta;
         _minTimeDelta = minTimeDelta;
      }

      /// <summary>
      /// Update the motion history with the specific image and current timestamp
      /// </summary>
      /// <param name="image">The image to be added to history</param>
      public void Update(Mat image)
      {
         Update(image, DateTime.Now);
      }

      /// <summary>
      /// Update the motion history with the specific image and the specific timestamp
      /// </summary>
      /// <param name="foregroundMask">The foreground of the image to be added to history</param>
      /// <param name="timestamp">The time when the image is captured</param>
      public void Update(Mat foregroundMask, DateTime timestamp)
      {
         _lastTime = timestamp;
         TimeSpan ts = _lastTime.Subtract(_initTime);
         if (_mhi == null)
            _mhi = new Mat(foregroundMask.Rows, foregroundMask.Cols, DepthType.Cv32F, 1);
         CvInvoke.UpdateMotionHistory(foregroundMask, _mhi, ts.TotalSeconds, _mhiDuration);
         double scale = 255.0 / _mhiDuration;
         _mhi.ConvertTo(_mask, DepthType.Cv8U, scale, (_mhiDuration - ts.TotalSeconds) * scale);
         
         CvInvoke.CalcMotionGradient(_mhi, _mask, _orientation, _maxTimeDelta, _minTimeDelta);
      }

      /// <summary>
      /// Get a sequence of motion component
      /// </summary>
      /// <returns>A sequence of motion components</returns>
      public void GetMotionComponents(IOutputArray segMask, VectorOfRect boundingRects)
      {
         TimeSpan ts = _lastTime.Subtract(_initTime);
         
         CvInvoke.SegmentMotion(_mhi, segMask, boundingRects, ts.TotalSeconds, _maxTimeDelta);
      }

      /// <summary>
      /// Given a rectangle area of the motion, output the angle of the motion and the number of pixels that are considered to be motion pixel 
      /// </summary>
      /// <param name="motionRectangle">The rectangle area of the motion</param>
      /// <param name="angle">The orientation of the motion</param>
      /// <param name="motionPixelCount">Number of motion pixels within silhouette ROI</param>
      /// <param name="forgroundMask">The foreground mask used to calculate the motion info.</param>
      public void MotionInfo(Mat forgroundMask, System.Drawing.Rectangle motionRectangle, out double angle, out double motionPixelCount)
      {
         TimeSpan ts = _lastTime.Subtract(_initTime);
         // select component ROI
         using (Mat forgroundMaskRect = new Mat(forgroundMask, motionRectangle))
         using (Mat mhiRect = new Mat(_mhi, motionRectangle))
         using (Mat orientationRect = new Mat(_orientation, motionRectangle))
         using (Mat maskRect = new Mat(_mask, motionRectangle))
         {
            // calculate orientation
            angle = CvInvoke.CalcGlobalOrientation(orientationRect, maskRect, mhiRect, ts.TotalSeconds, _mhiDuration);
            angle = 360.0 - angle; // adjust for images with top-left origin

            // calculate number of points within silhouette ROI
            motionPixelCount = CvInvoke.Norm(forgroundMaskRect, null, CvEnum.NormType.L1);  
         }
      }

      /// <summary>
      /// Release unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
      }

      /// <summary>
      /// Release any images associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_mhi != null) _mhi.Dispose();
         if (_mask != null) _mask.Dispose();
         if (_orientation != null) _orientation.Dispose();
      }
   }
}
