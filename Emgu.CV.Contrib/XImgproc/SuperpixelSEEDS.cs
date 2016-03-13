//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Ximgproc
{
   public class SupperpixelSEEDS : UnmanagedObject
   {
      public SupperpixelSEEDS(int imageWidth, int imageHeight, int imageChannels,
         int numSuperpixels, int numLevels, int prior,
         int histogramBins,
         bool doubleStep)
      {
         _ptr = XimgprocInvoke.cveSuperpixelSEEDSCreate(
            imageWidth, imageHeight, imageChannels,
            numSuperpixels, numLevels, prior,
            histogramBins, doubleStep);
      }

      public int NumberOfSuperpixels
      {
         get { return XimgprocInvoke.cveSuperpixelSEEDSGetNumberOfSuperpixels(_ptr); }
      }

      public void GetLabels(IOutputArray labels)
      {
         using (OutputArray oaLabels = labels.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSEEDSGetLabels(_ptr, oaLabels);
      }

      public void GetLabelContourMask(IOutputArray image, bool thickLine)
      {
         using (OutputArray oaImage = image.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSEEDSGetLabelContourMask(_ptr, oaImage, thickLine);
      }

      public void Iterate(IInputArray img, int numIterations)
      {
         using (InputArray iaImg = img.GetInputArray())
            XimgprocInvoke.cveSuperpixelSEEDSIterate(_ptr, iaImg, numIterations);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveSuperpixelSEEDSRelease(ref _ptr);
         }
      }
   }


   public static partial class XimgprocInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperpixelSEEDSCreate(
         int imageWidth, int imageHeight, int imageChannels,
         int numSuperpixels, int numLevels, int prior,
         int histogramBins, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool doubleStep
         );

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveSuperpixelSEEDSGetNumberOfSuperpixels(IntPtr seeds);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSEEDSGetLabels(IntPtr seeds, IntPtr labelsOut);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSEEDSGetLabelContourMask(
         IntPtr seeds, 
         IntPtr image, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool thickLine);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSEEDSRelease(ref IntPtr seeds);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSEEDSIterate(IntPtr seeds, IntPtr img, int numIterations);
   }
}
