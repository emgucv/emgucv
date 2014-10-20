//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Random tree
   /// </summary>
   public class RTrees : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      public class Params : UnmanagedObject
      {


         public Params(
            int maxDepth, int minSampleCount,
            double regressionAccuracy, bool useSurrogates,
            int maxCategories, Mat priors,
            bool calcVarImportance, int nactiveVars,
            MCvTermCriteria termCrit)
         {
            _ptr =
               MlInvoke.CvRTParamsCreate(
                  maxDepth, minSampleCount, regressionAccuracy, useSurrogates,
                  maxCategories, priors ?? IntPtr.Zero, calcVarImportance, nactiveVars, ref termCrit
            );
         }

         protected override void DisposeObject()
         {
            MlInvoke.CvRTParamsRelease(ref _ptr);
         }
      }

      /// <summary>
      /// Create a random tree
      /// </summary>
      public RTrees(Params p)
      {
         _ptr = MlInvoke.CvRTreesCreate(p, ref _statModelPtr, ref _algorithmPtr);
      }


      /// <summary>
      /// Release the random tree and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvRTreesRelease(ref _ptr);
         _statModelPtr = IntPtr.Zero;
         _algorithmPtr = IntPtr.Zero;
      }

      /*

      /// <summary>
      /// Get the number of Trees in the Random tree
      /// </summary>
      public int TreeCount
      {
         get
         {
            return MlInvoke.CvRTreesGetTreeCount(Ptr);
         }
      }

      /// <summary>
      /// Get the variable importance matrix
      /// </summary>
      public Matrix<float> VarImportance
      {
         get
         {
            IntPtr matPtr = MlInvoke.CvRTreesGetVarImportance(Ptr);
            if (matPtr == IntPtr.Zero) return null;
            MCvMat mat = (MCvMat)Marshal.PtrToStructure(matPtr, typeof(MCvMat));

            Matrix<float> res = new Matrix<float>(mat.Rows, mat.Cols, 1, mat.Data, mat.Step);
            Debug.Assert(mat.Type == res.MCvMat.Type, "Matrix type is not float");
            return res;
         }
      }*/

      IntPtr IStatModel.StatModelPtr
      {
         get { return _statModelPtr; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithmPtr; }
      }
   }
}
