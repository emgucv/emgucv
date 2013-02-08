//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Reconstructs the selected image area from the pixel near the area boundary. The function may be used to remove dust and scratches from a scanned photo, or to remove undesirable objects from still images or video.
      /// </summary>
      /// <param name="src">The input 8-bit 1-channel or 3-channel image</param>
      /// <param name="mask">The inpainting mask, 8-bit 1-channel image. Non-zero pixels indicate the area that needs to be inpainted</param>
      /// <param name="dst">The output image of the same format and the same size as input</param>
      /// <param name="flags">The inpainting method</param>
      /// <param name="inpaintRadius">The radius of circlular neighborhood of each point inpainted that is considered by the algorithm</param>
      [DllImport(OPENCV_PHOTO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInpaint(IntPtr src, IntPtr mask, IntPtr dst, double inpaintRadius, CvEnum.INPAINT_TYPE flags);

   }
}
