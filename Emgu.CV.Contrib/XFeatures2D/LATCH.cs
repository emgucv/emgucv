//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
   /// <summary>
   /// latch Class for computing the LATCH descriptor.
   /// If you find this code useful, please add a reference to the following paper in your work:
   /// Gil Levi and Tal Hassner, "LATCH: Learned Arrangements of Three Patch Codes", arXiv preprint arXiv:1501.03719, 15 Jan. 2015
   /// LATCH is a binary descriptor based on learned comparisons of triplets of image patches.
   /// </summary>
   public class LATCH : Feature2D
   {
      /// <summary>
      /// Create LATCH descriptor extractor
      /// </summary>
      /// <param name="bytes">The size of the descriptor - can be 64, 32, 16, 8, 4, 2 or 1</param>
      /// <param name="rotationInvariance">Whether or not the descriptor should compensate for orientation changes.</param>
      /// <param name="halfSsdSize">the size of half of the mini-patches size. For example, if we would like to compare triplets of patches of size 7x7x
      /// then the half_ssd_size should be (7-1)/2 = 3.</param>
      public LATCH(int bytes = 32, bool rotationInvariance = true, int halfSsdSize = 3)
      {
         _ptr = ContribInvoke.cveLATCHCreate(bytes, rotationInvariance, halfSsdSize, ref _feature2D);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with BRIEF
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ContribInvoke.cveLATCHRelease(ref _ptr);
         }
         base.DisposeObject();
      }
   }

   public static partial class ContribInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveLATCHCreate(
         int bytes, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool rotationInvariance, 
         int halfSsdSize, 
         ref IntPtr extractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveLATCHRelease(ref IntPtr extractor);
   }
}