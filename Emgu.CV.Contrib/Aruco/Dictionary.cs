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
using System.Diagnostics;

namespace Emgu.CV.Aruco
{
   public class Dictionary : UnmanagedObject
   {
      public Dictionary(PredefinedDictionaryName name)
      {
         _ptr = ArucoInvoke.cveArucoGetPredefinedDictionary(name);
      }

      public enum PredefinedDictionaryName
      {
         Dict4X4_50 = 0,
         Dict4X4_100,
         Dict4X4_250,
         Dict4X4_1000,
         Dict5X5_50,
         Dict5X5_100,
         Dict5X5_250,
         Dict5X5_1000,
         Dict6X6_50,
         Dict6X6_100,
         Dict6X6_250,
         Dict6X6_1000,
         Dict7X7_50,
         Dict7X7_100,
         Dict7X7_250,
         Dict7X7_1000,
         DictArucoOriginal
      };

      protected override void DisposeObject()
      {
         //no need to release any object here.
         //The dictionary is static global

         _ptr = IntPtr.Zero;
      }
   }

   public static partial class ArucoInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveArucoGetPredefinedDictionary(Dictionary.PredefinedDictionaryName name);
   }
}