using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
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
      int Ithreshold; 
      /// <summary>
      /// Radius for smoothness cost function (1 by default; means Potts model)
      /// </summary>
      int interactionRadius; 
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      float K;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      float lambda;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      float lambda1;
      /// <summary>
      /// Parameters for the cost function
      /// </summary>
      float lambda2; 
      
      /// <summary>
      /// 10000 by default, (usually computed adaptively from the input data)
      /// </summary>
      int occlusionCost; 
      /// <summary>
      /// 0 by default; see CvStereoBMState
      /// </summary>
      int minDisparity; 
      /// <summary>
      /// Defined by user; see CvStereoBMState
      /// </summary>
      int numberOfDisparities; 
      /// <summary>
      /// Number of iterations; defined by user.
      /// </summary>
      int maxIters; 

      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr left;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr right;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr dispLeft;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr dispRight;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr ptrLeft;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr ptrRight;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr vtxBuf;
      /// <summary>
      /// Internal buffers
      /// </summary>
      IntPtr edgeBuf;
   }
}
