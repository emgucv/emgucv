//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
   
      /// <summary>
      /// Solve given (non-integer) linear programming problem using the Simplex Algorithm (Simplex Method). 
      /// What we mean here by “linear programming problem” (or LP problem, for short) can be formulated as:
      /// Maximize c x subject to: Ax &lt;= b and x &gt;= 0 
      /// </summary>
      /// <param name="functionMatrix">This row-vector corresponds to c in the LP problem formulation (see above). It should contain 32- or 64-bit floating point numbers. As a convenience, column-vector may be also submitted, in the latter case it is understood to correspond to c^T.</param>
      /// <param name="constraintMatrix">m-by-n+1 matrix, whose rightmost column corresponds to b in formulation above and the remaining to A. It should containt 32- or 64-bit floating point numbers.</param>
      /// <param name="zMatrix">The solution will be returned here as a column-vector - it corresponds to c in the formulation above. It will contain 64-bit floating point numbers.</param>
      /// <returns>The return codes</returns>
      public static CvEnum.SolveLPResult SolveLP(Mat functionMatrix, Mat constraintMatrix, Mat zMatrix)
      {
         return cveSolveLP(functionMatrix, constraintMatrix, zMatrix);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern CvEnum.SolveLPResult cveSolveLP(IntPtr functionMatrix, IntPtr constraintMatrix, IntPtr zMatrix);

      /// <summary>
      /// Primal-dual algorithm is an algorithm for solving special types of variational problems (that is, finding a function to minimize some functional).
      /// As the image denoising, in particular, may be seen as the variational problem, primal-dual algorithm then can be used to perform 
      /// denoising and this is exactly what is implemented.
      /// </summary>
      /// <param name="observations">This array should contain one or more noised versions of the image that is to be restored.</param>
      /// <param name="result">Here the denoised image will be stored. There is no need to do pre-allocation of storage space, as it will be automatically allocated, if necessary.</param>
      /// <param name="lambda">Corresponds to  in the formulas above. As it is enlarged, the smooth (blurred) images are treated more favorably than detailed (but maybe more noised) ones. Roughly speaking, as it becomes smaller, the result will be more blur but more sever outliers will be removed.</param>
      /// <param name="niters">Number of iterations that the algorithm will run. Of course, as more iterations as better, but it is hard to quantitatively refine this statement, so just use the default and increase it if the results are poor.</param>
      public static void DenoiseTVL1(Mat[] observations, Mat result, double lambda, int niters)
      {
         using (Util.VectorOfMat vm = new Util.VectorOfMat(observations))
         {
            cveDenoiseTVL1(vm, result, lambda, niters);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDenoiseTVL1(IntPtr observations, IntPtr result, double lambda, int niters);
   }
}
