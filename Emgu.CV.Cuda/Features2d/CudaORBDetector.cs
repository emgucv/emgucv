//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// An ORB detector using Cuda
   /// </summary>
   public class CudaORBDetector : ORBDetector, IFeature2DAsync
   {

      private IntPtr _feature2DAsyncPtr;

      /// <summary>
      /// Create a ORBDetector using the specific values
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features.</param>
      /// <param name="scaleFactor">Coefficient by which we divide the dimensions from one scale pyramid level to the next.</param>
      /// <param name="nLevels">The number of levels in the scale pyramid.</param>
      /// <param name="firstLevel">The level at which the image is given. If 1, that means we will also look at the image.<paramref name="scaleFactor"/> times bigger</param>
      /// <param name="edgeThreshold">How far from the boundary the points should be.</param>
      /// <param name="WTK_A">How many random points are used to produce each cell of the descriptor (2, 3, 4 ...).</param>
      /// <param name="scoreType">Type of the score to use.</param>
      /// <param name="patchSize">Patch size.</param>
      public CudaORBDetector(
         int numberOfFeatures = 500, 
         float scaleFactor = 1.2f, 
         int nLevels = 8, 
         int edgeThreshold = 31, 
         int firstLevel = 0, 
         int WTK_A = 2, 
         ORBDetector.ScoreType scoreType = ORBDetector.ScoreType.Harris, 
         int patchSize = 31,
         int fastThreshold = 20, 
         bool blurForDescriptor = false)
      {
         _ptr = CudaInvoke.cveCudaORBCreate(
            numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTK_A, scoreType, patchSize, fastThreshold, blurForDescriptor,
            ref _feature2D, ref _feature2DAsyncPtr);
      }


      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cveCudaORBRelease(ref _ptr);
      }

      IntPtr IFeature2DAsync.Feature2DAsyncPtr
      {
         get { return _feature2DAsyncPtr; }
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveCudaORBCreate(
         int numberOfFeatures, 
         float scaleFactor, 
         int nLevels, 
         int edgeThreshold, 
         int firstLevel, 
         int WTA_K, 
         ORBDetector.ScoreType scoreType, 
         int patchSize, 
         int fastThreshold, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool blurForDescriptor, 
         ref IntPtr feature2D, 
         ref IntPtr feature2DAsync);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveCudaORBRelease(ref IntPtr detector);

    
   }
}
