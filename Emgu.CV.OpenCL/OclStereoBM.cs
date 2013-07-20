//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// Use Block Matching algorithm to find stereo correspondence
   /// </summary>
   public class OclStereoBM : UnmanagedObject
   {
      /// <summary>
      /// Preset type
      /// </summary>
      public enum PresetType
      {
         /// <summary>
         /// Basic
         /// </summary>
         BasicPreset = 0,
         /// <summary>
         /// prefilter xsobel
         /// </summary>
         PrefilterXSobel = 1
      };

      /// <summary>
      /// Create a stereoBM 
      /// </summary>
      /// <param name="preset">Preset type</param>
      /// <param name="numberOfDisparities">The number of disparities. Must be multiple of 8. Use 64 for default </param>
      /// <param name="winSize">The SAD window size. Use 19 for default</param>
      public OclStereoBM(PresetType preset, int numberOfDisparities, int winSize)
      {
         _ptr = OclInvoke.oclStereoBMCreate(preset, numberOfDisparities, winSize);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The disparity map</param>
      public void FindStereoCorrespondence(OclImage<Gray, Byte> left, OclImage<Gray, Byte> right, OclImage<Gray, Byte> disparity)
      {
         OclInvoke.oclStereoBMFindStereoCorrespondence(_ptr, left, right, disparity);
      }

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclStereoBMRelease(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclStereoBMCreate(OclStereoBM.PresetType preset, int ndisparities, int winSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclStereoBMFindStereoCorrespondence(IntPtr stereoBM, IntPtr left, IntPtr right, IntPtr disparity);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclStereoBMRelease(ref IntPtr stereoBM);
   }
}