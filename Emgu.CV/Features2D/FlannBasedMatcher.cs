/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Flann;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   public class FlannBasedMatcher : DescriptorMatcher
   {
      public FlannBasedMatcher(IIndexParams indexParams, SearchParams search)
      {
         _ptr = CvInvoke.cveFlannBasedMatcherCreate
            (ref _descriptorMatcherPtr, 
            indexParams.IndexParamPtr,
            ((IIndexParams) search).IndexParamPtr);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveFlannBasedMatcherRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveFlannBasedMatcherCreate(
         ref IntPtr dmPtr,
         IntPtr ip, IntPtr sp);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveFlannBasedMatcherRelease(ref IntPtr matcher);
   }
}*/
