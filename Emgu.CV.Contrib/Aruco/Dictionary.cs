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
   /// <summary>
   /// Dictionary/Set of markers. It contains the inner codification.
   /// </summary>
   public class Dictionary : UnmanagedObject
   {
      /// <summary>
      /// Create a Dictionary using predefined values
      /// </summary>
      /// <param name="name">The name of the predefined dictionary</param>
      public Dictionary(PredefinedDictionaryName name)
      {
         _ptr = ArucoInvoke.cveArucoGetPredefinedDictionary(name);
      }


      /// <summary>
      /// The name of the predefined dictionary
      /// </summary>
      public enum PredefinedDictionaryName
      {
         /// <summary>
         /// Dict4X4_50
         /// </summary>
         Dict4X4_50 = 0,
         /// <summary>
         /// Dict4X4_100
         /// </summary>
         Dict4X4_100,
         /// <summary>
         /// Dict4X4_250
         /// </summary>
         Dict4X4_250,
         /// <summary>
         /// Dict4X4_1000
         /// </summary>
         Dict4X4_1000,
         /// <summary>
         /// Dict5X5_50
         /// </summary>
         Dict5X5_50,
         /// <summary>
         /// Dict5X5_100
         /// </summary>
         Dict5X5_100,
         /// <summary>
         /// Dict5X5_250
         /// </summary>
         Dict5X5_250,
         /// <summary>
         /// Dict5X5_1000
         /// </summary>
         Dict5X5_1000,
         /// <summary>
         /// Dict6X6_50
         /// </summary>
         Dict6X6_50,
         /// <summary>
         /// Dict6X6_100
         /// </summary>
         Dict6X6_100,
         /// <summary>
         /// Dict6X6_250
         /// </summary>
         Dict6X6_250,
         /// <summary>
         /// Dict6X6_1000
         /// </summary>
         Dict6X6_1000,
         /// <summary>
         /// Dict7X7_50
         /// </summary>
         Dict7X7_50,
         /// <summary>
         /// Dict7X7_100
         /// </summary>
         Dict7X7_100,
         /// <summary>
         /// Dict7X7_250
         /// </summary>
         Dict7X7_250,
         /// <summary>
         /// Dict7X7_1000
         /// </summary>
         Dict7X7_1000,
         /// <summary>
         /// standard ArUco Library Markers. 1024 markers, 5x5 bits, 0 minimum distance
         /// </summary>
         DictArucoOriginal
      };

      /// <summary>
      /// Release the unmanaged resource
      /// </summary>
      protected override void DisposeObject()
      {
         //no need to release any object here.
         //The dictionary is static global in C++ code

         _ptr = IntPtr.Zero;
      }
   }

   public static partial class ArucoInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveArucoGetPredefinedDictionary(Dictionary.PredefinedDictionaryName name);
   }
}