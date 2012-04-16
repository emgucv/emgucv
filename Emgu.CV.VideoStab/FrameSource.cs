using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime.InteropServices;
using System;

namespace Emgu.CV.VideoStab
{
   public abstract class FrameSource : UnmanagedObject
   {
      private IntPtr _frameBuffer;

      public Image<Bgr, Byte> NextFrame()
      {
         if (!VideoStabInvoke.CaptureFrameSourceGetNextFrame(Ptr, ref _frameBuffer) || _frameBuffer == IntPtr.Zero)
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
         if (_frameBuffer != null)
         {
            CvInvoke.cvReleaseImage(ref _frameBuffer);
         }
      }
   }
}