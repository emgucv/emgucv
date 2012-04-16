using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoStab
{
   internal class CaptureFrameSource : FrameSource
   {
      public CaptureFrameSource(Capture capture)
      {
         _ptr = VideoStabInvoke.CaptureFrameSourceCreate(capture);
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.CaptureFrameSourceRelease(ref _ptr);
         base.DisposeObject();
      }
   }
}
