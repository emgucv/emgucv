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
   public class CaptureFrameSource : FrameSource
   {
      public CaptureFrameSource(Capture capture)
      {
         _ptr = VideoStabInvoke.CaptureFrameSourceCreate(capture);
         _framSourcePtr = _ptr;
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.CaptureFrameSourceRelease(ref _ptr);
         base.DisposeObject();
      }
   }
}
