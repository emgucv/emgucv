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
   public struct DetectorParameters
   {

      public int AdaptiveThreshWinSizeMin;
      public int AdaptiveThreshWinSizeMax;
      public int AdaptiveThreshWinSizeStep;
      public double AdaptiveThreshConstant;
      public double MinMarkerPerimeterRate;
      public double MaxMarkerPerimeterRate;
      public double PolygonalApproxAccuracyRate;
      public double MinCornerDistanceRate;
      public int MinDistanceToBorder;
      public double MinMarkerDistanceRate;
      public bool DoCornerRefinement;
      public int CornerRefinementWinSize;
      public int CornerRefinementMaxIterations;
      public double CornerRefinementMinAccuracy;
      public int MarkerBorderBits;
      public int PerspectiveRemovePixelPerCell;
      public double PerspectiveRemoveIgnoredMarginPerCell;
      public double MaxErroneousBitsInBorderRate;
      public double MinOtsuStdDev;
      public double ErrorCorrectionRate;
   }


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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDrawMarker(IntPtr dictionary, int id, int sidePixels, IntPtr img, int borderBits);

      public static void DrawMarker(Dictionary dict, int id, int sidePixels, IOutputArray img, int borderBits = 1)
      {
         using (OutputArray oaImg = img.GetOutputArray())
            cveArucoDrawMarker(dict, id, sidePixels, oaImg, borderBits);
      }

      public static void DetectMarkers(
         IInputArray image, Dictionary dict, IOutputArray corners,
         IOutputArray ids, DetectorParameters parameters,
         IOutputArray rejectedImgPoints
         )
      {
         
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDetectMarkers(IntPtr image, IntPtr dictionary, IntPtr corners,
         IntPtr ids, ref DetectorParameters parameters,
         IntPtr rejectedImgPoints);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoGridBoardCreate(
         int markersX, int markersY, float markerLength, float markerSeparation,
         IntPtr dictionary);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoGridBoardRelease(ref IntPtr gridBoard);
   }
}