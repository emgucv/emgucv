/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBGStatModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvFGDStatModelParams
   {
      /// <summary>
      /// Quantized levels per 'color' component. Power of two, typically 32, 64 or 128.
      /// </summary>
      public int Lc;			
      /// <summary>
      /// Number of color vectors used to model normal background color variation at a given pixel. Try 15 for default.
      /// </summary>
      public int N1c;		
      /// <summary>
      /// Number of color vectors retained at given pixel.  Must be > N1c, typically ~ 5/3 of N1c.
      /// </summary>
      /// <remarks>Used to allow the first N1c vectors to adapt over time to changing background.</remarks>
      public int N2c;		

      /// <summary>
      /// Quantized levels per 'color co-occurrence' component.  Power of two, typically 16, 32 or 64.
      /// </summary>
      public int Lcc;		
      /// <summary>
      /// Number of color co-occurrence vectors used to model normal background color variation at a given pixel. Try 25 for default.
      /// </summary>
      public int N1cc;		
      /// <summary>
      /// Number of color co-occurrence vectors retained at given pixel.  Must be > N1cc, typically ~ 5/3 of N1cc.	
      /// </summary>
      /// <remarks>Used to allow the first N1cc vectors to adapt over time to changing background.</remarks>
      public int N2cc;		

      /// <summary>
      /// If 1 we ignore holes within foreground blobs. Defaults to 1.
      /// </summary>
      public int is_obj_without_holes;
      /// <summary>
      /// Number of erode-dilate-erode foreground-blob cleanup iterations.
      /// </summary>
      /// <remarks>These erase one-pixel junk blobs and merge almost-touching blobs. Default value is 1.</remarks>
      public int perform_morphing;

      /// <summary>
      /// How quickly we forget old background pixel values seen.  Typically set to 0.1
      /// </summary>
      public float alpha1;		
      /// <summary>
      /// Controls speed of feature learning. Depends on T. Typical value circa 0.005.
      /// </summary>
      public float alpha2;
      /// <summary>
      /// Alternate to alpha2, used (e.g.) for quicker initial convergence. Typical value 0.1.
      /// </summary>
      public float alpha3;		

      /// <summary>
      /// Affects color and color co-occurrence quantization, typically set to 2.
      /// </summary>
      public float delta;		
      /// <summary>
      /// A percentage value which determines when new features can be recognized as new background. (Typically 0.9).
      /// </summary>
      public float T;			

      /// <summary>
      /// Discard foreground blobs whose bounding box is smaller than this threshold. Try 15 for default
      /// </summary>
      public float minArea;
   }
}
*/