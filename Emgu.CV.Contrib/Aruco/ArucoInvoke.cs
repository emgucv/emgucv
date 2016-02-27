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

   public static partial class ArucoInvoke
   {
      static ArucoInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDrawMarker(IntPtr dictionary, int id, int sidePixels, IntPtr img, int borderBits);
      public static void DrawMarker(Dictionary dict, int id, int sidePixels, IOutputArray img, int borderBits = 1)
      {
         using (OutputArray oaImg = img.GetOutputArray())
            cveArucoDrawMarker(dict, id, sidePixels, oaImg, borderBits);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDetectMarkers(IntPtr image, IntPtr dictionary, IntPtr corners,
         IntPtr ids, ref DetectorParameters parameters,
         IntPtr rejectedImgPoints);
      public static void DetectMarkers(
         IInputArray image, Dictionary dict, IOutputArrayOfArrays corners,
         IOutputArray ids, DetectorParameters parameters,
         IOutputArrayOfArrays rejectedImgPoints = null
         )
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaCorners = corners.GetOutputArray())
         using (OutputArray oaIds = ids.GetOutputArray())
         using (OutputArray oaRejectedImgPoints = rejectedImgPoints != null ? rejectedImgPoints.GetOutputArray() : OutputArray.GetEmpty())
         {
            cveArucoDetectMarkers(iaImage, dict, oaCorners, oaIds, ref parameters, oaRejectedImgPoints);
         }
      }

      public static void DrawAxis(
         IInputOutputArray image, IInputArray cameraMatrix, IInputArray distCoeffs,
         IInputArray rvec, IInputArray tvec, float length)
      {
         using (InputOutputArray ioaImage = image.GetInputOutputArray())
         using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
         using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
         using (InputArray iaRvec = rvec.GetInputArray())
         using (InputArray iaTvec = tvec.GetInputArray())
         {
            cveArucoDrawAxis(ioaImage, iaCameraMatrix, iaDistCoeffs, iaRvec, iaTvec, length);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDrawAxis(IntPtr image, IntPtr cameraMatrix, IntPtr distCoeffs, IntPtr rvec, IntPtr tvec, float length);

   }
}