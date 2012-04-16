using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emgu.CV.VideoStab
{
   public class OnePassStabilizer : FrameSource
   {
      private CaptureFrameSource _captureFrameSource;

      public OnePassStabilizer(Capture capture)
      {
         _captureFrameSource = new CaptureFrameSource(capture);
         _ptr = VideoStabInvoke.OnePassStabilizerCreate(_captureFrameSource);
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.OnePassStabilizerRelease(ref _ptr);
         _captureFrameSource.Dispose();
         base.DisposeObject();
      }
   }
}
