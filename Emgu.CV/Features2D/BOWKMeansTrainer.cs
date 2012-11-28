//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Kmeans-based class to train visual vocabulary using the bag of visual words approach.
   /// </summary>
   /// <typeparam name="T">The type of vocabulary, either byte or float</typeparam>
   public class BOWKMeansTrainer : UnmanagedObject
   {
      /// <summary>
      /// Create a new BOWKmeans trainer
      /// </summary>
      /// <param name="clusterCount">Number of clusters to split the set by.</param>
      /// <param name="termcrit">Specifies maximum number of iterations and/or accuracy (distance the centers move by between the subsequent iterations). Use empty termcrit for default.</param>
      /// <param name="attempts">The number of attemps. Use 3 for default</param>
      /// <param name="flags">Kmeans initialization flag. Use PPCenters for default.</param>
      public BOWKMeansTrainer(int clusterCount, MCvTermCriteria termcrit, int attempts, CvEnum.KMeansInitType flags)
      {
         _ptr = CvInvoke.CvBOWKMeansTrainerCreate(clusterCount, termcrit, attempts, flags);
      }

      /// <summary>
      /// Get the number of descriptors
      /// </summary>
      public int DescriptorCount
      {
         get
         {
            return CvInvoke.CvBOWKMeansTrainerGetDescriptorCount(_ptr);
         }
      }

      /// <summary>
      /// Add the descriptors to the trainer
      /// </summary>
      /// <param name="descriptor"></param>
      public void Add(Matrix<float> descriptors)
      {
         CvInvoke.CvBOWKMeansTrainerAdd(_ptr, descriptors);
      }

      /// <summary>
      /// Cluster the descriptors and return the cluster centers
      /// </summary>
      /// <returns>The cluster centers</returns>
      public Matrix<float> Cluster()
      {
         using (Mat m = new Mat())
         {
            CvInvoke.CvBOWKMeansTrainerCluster(_ptr, m);
            Matrix<float> result = new Matrix<float>(m.Size);
            CvInvoke.cvMatCopyToCvArr(m, result);
            return result;
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBOWKMeansTrainerRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBOWKMeansTrainerCreate(int clusterCount, MCvTermCriteria termcrit, int attempts, CvEnum.KMeansInitType flags);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWKMeansTrainerRelease(ref IntPtr trainer);

            [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvBOWKMeansTrainerGetDescriptorCount(IntPtr trainer);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
            internal extern static void CvBOWKMeansTrainerAdd(IntPtr trainer, IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWKMeansTrainerCluster(IntPtr trainer, IntPtr descriptors);
   }
}