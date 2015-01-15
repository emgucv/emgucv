/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//Contributed by Daniel Bell, modified by Canming

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// LevMarqSparse solver
   /// </summary>
   //public class LevMarqSparse : UnmanagedObject
   public static class LevMarqSparse
   {
      static LevMarqSparse()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      
      // /// <summary>
      // /// Create a LevMarqSparse solver
      // /// </summary>
      //public LevMarqSparse()
      //{
      //   _ptr = CvInvoke.CvCreateLevMarqSparse();
      //}

      /// <summary>
      /// Useful function to do simple bundle adjustment tasks
      /// </summary>
      /// <param name="points">Positions of points in global coordinate system (input and output), values will be modified by bundle adjustment</param>
      /// <param name="imagePoints">Projections of 3d points for every camera</param>
      /// <param name="visibility">Visibility of 3d points for every camera</param>
      /// <param name="cameraMatrix">Intrinsic matrices of all cameras (input and output), values will be modified by bundle adjustment</param>
      /// <param name="R">rotation matrices of all cameras (input and output), values will be modified by bundle adjustment</param>
      /// <param name="T">translation vector of all cameras (input and output), values will be modified by bundle adjustment</param>
      /// <param name="distCoeffcients">distortion coefficients of all cameras (input and output), values will be modified by bundle adjustment</param>
      /// <param name="termCrit">Termination criteria, a reasonable value will be (30, 1.0e-12) </param>
      public static void BundleAdjust(
         MCvPoint3D64f[] points, MCvPoint2D64f[][] imagePoints, int[][] visibility,
         Matrix<double>[] cameraMatrix, Matrix<double>[] R, Matrix<double>[] T, Matrix<double>[] distCoeffcients, MCvTermCriteria termCrit)
      {
         using (Matrix<double> imagePointsMat = CvToolbox.GetMatrixFromPoints(imagePoints))
         using (Matrix<int> visibilityMat = CvToolbox.GetMatrixFromArrays(visibility))
         using (VectorOfMat cameraMatVec = new VectorOfMat())
         using (VectorOfMat rMatVec = new VectorOfMat())
         using (VectorOfMat tMatVec = new VectorOfMat())
         using (VectorOfMat distorMatVec = new VectorOfMat())
         {
            cameraMatVec.Push(cameraMatrix);
            rMatVec.Push(R);
            tMatVec.Push(T);
            distorMatVec.Push(distCoeffcients);


            GCHandle handlePoints = GCHandle.Alloc(points, GCHandleType.Pinned);

            CvLevMarqSparseAdjustBundle(
               cameraMatrix.Length,
               points.Length, handlePoints.AddrOfPinnedObject(),
               imagePointsMat, visibilityMat, cameraMatVec, rMatVec, tMatVec, distorMatVec, ref termCrit);

            handlePoints.Free();

         }
      }

      
      // /// <summary>
      // /// Release all unmanaged memory associated with the LevMarqSpare solver.
      // /// </summary>
      //protected override void DisposeObject()
      //{
      //   CvInvoke.CvReleaseLevMarqSparse(ref _ptr);
      //}

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvLevMarqSparseAdjustBundle(
         int numberOfFrames,
         int pointCount,
         IntPtr points, //positions of points in global coordinate system (input and output)
         IntPtr imagePoints, //projections of 3d points for every camera
         IntPtr visibility, //visibility of 3d points for every camera
         IntPtr cameraMatrix, //intrinsic matrices of all cameras (input and output)
         IntPtr R, //rotation matrices of all cameras (input and output)
         IntPtr T, //translation vector of all cameras (input and output)
         IntPtr distCoefs, //distortion coefficients of all cameras (input and output)
         ref MCvTermCriteria termCrit);

      
      //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      //internal static extern IntPtr CvCreateLevMarqSparse();

      //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      //internal static extern void CvReleaseLevMarqSparse(ref IntPtr levMarq);
      
   }

}

*/