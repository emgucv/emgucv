//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
   public abstract class FrameSource : UnmanagedObject
   {
      private IntPtr _frameBuffer;

      protected IntPtr _framSourcePtr;

      public Image<Bgr, Byte> NextFrame()
      {
         if (!VideoStabInvoke.FrameSourceGetNextFrame(_framSourcePtr, ref _frameBuffer) || _frameBuffer == IntPtr.Zero)
            return null;

         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(_frameBuffer, typeof(MIplImage));

         Image<Bgr, Byte> res;
         if (iplImage.nChannels == 1)
         {  //if the image captured is Grayscale, convert it to BGR
            res = new Image<Bgr, Byte>(iplImage.width, iplImage.height);
            CvInvoke.cvCvtColor(_frameBuffer, res.Ptr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
         }
         else
         {
            res = new Image<Bgr, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }

         return res;
      }

      protected override void DisposeObject()
      {
         if (_frameBuffer != IntPtr.Zero)
         {
            CvInvoke.cvReleaseImage(ref _frameBuffer);
         }
         _framSourcePtr = IntPtr.Zero;
      }
   }
}