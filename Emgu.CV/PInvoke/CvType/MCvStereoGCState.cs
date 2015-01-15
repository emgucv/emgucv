//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvStereoGCState structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStereoGCState
   {
      /// <summary>
      /// Threshold for piece-wise linear data cost function (5 by default)
      /// </summary>
      public int Ithreshold; 
      /// <summary>
      /// Radius for smoothness cost function (1 by default; means Potts model)
      /// </summary>
      public int interactionRadius; 
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      public float K;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      public float lambda;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      public float lambda1;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      public float lambda2; 
      
      /// <summary>
      /// 10000 by default, (usually computed adaptively from the input data)
      /// </summary>
      public int occlusionCost; 
      /// <summary>
      /// 0 by default; see CvStereoBMState
      /// </summary>
      public int minDisparity; 
      /// <summary>
      /// Defined by user; see CvStereoBMState
      /// </summary>
      public int numberOfDisparities; 
      /// <summary>
      /// Number of iterations; defined by user.
      /// </summary>
      public int maxIters; 

      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr left;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr right;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr dispLeft;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr dispRight;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr ptrLeft;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr ptrRight;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr vtxBuf;
      /// <summary>
      /// Internal buffers
      /// </summary>
      public IntPtr edgeBuf;
   }
}
