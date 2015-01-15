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
   public struct MCvBGStatModel
   {
      /// <summary>
      /// The type of BG model
      /// </summary>
      public int Type;

      /// <summary>
      /// Pointer to the function that can be used to release this BGStatModel
      /// </summary>
      public IntPtr CvReleaseBGStatModel;

      /// <summary>
      /// Pointer to the function that can be used to update this BGStatModel
      /// </summary>
      public IntPtr CvUpdateBGStatModel;

      /// <summary>
      /// 8UC3 reference background image
      /// </summary>
      public IntPtr Background;

      /// <summary>
      /// 8UC1 foreground image
      /// </summary>
      public IntPtr Foreground;

      /// <summary>
      /// 8UC3 reference background image, can be null
      /// </summary>
      public IntPtr Layers;

      /// <summary>
      /// Can be zero
      /// </summary>
      public int LayerCount;

      /// <summary>
      /// Storage for foreground_regions
      /// </summary>
      public IntPtr Storage;

      /// <summary>
      /// Foreground object contours
      /// </summary>
      public IntPtr ForegroundRegions;

   }
}
*/