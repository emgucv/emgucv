//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public class BOWKMeansTrainer : UnmanagedObject
   {
      static BOWKMeansTrainer()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a new BOWKmeans trainer
      /// </summary>
      /// <param name="clusterCount">Number of clusters to split the set by.</param>
      /// <param name="termcrit">Specifies maximum number of iterations and/or accuracy (distance the centers move by between the subsequent iterations). Use empty termcrit for default.</param>
      /// <param name="attempts">The number of attemps. Use 3 for default</param>
      /// <param name="flags">Kmeans initialization flag. Use PPCenters for default.</param>
      public BOWKMeansTrainer(int clusterCount, MCvTermCriteria termcrit, int attempts, CvEnum.KMeansInitType flags)
      {
         _ptr = CvBOWKMeansTrainerCreate(clusterCount, ref termcrit, attempts, flags);
      }

      /// <summary>
      /// Get the number of descriptors
      /// </summary>
      public int DescriptorCount
      {
         get
         {
            return CvBOWKMeansTrainerGetDescriptorCount(_ptr);
         }
      }

      /// <summary>
      /// Add the descriptors to the trainer
      /// </summary>
      /// <param name="descriptors">The descriptors to be added to the trainer</param>
      public void Add(Mat descriptors)
      {
         CvBOWKMeansTrainerAdd(_ptr, descriptors);
      }

      /// <summary>
      /// Cluster the descriptors and return the cluster centers
      /// </summary>
      /// <returns>The cluster centers</returns>
      public void Cluster(IOutputArray cluster)
      {
         using (OutputArray oaCluster = cluster.GetOutputArray())
         CvBOWKMeansTrainerCluster(_ptr, oaCluster);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvBOWKMeansTrainerRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBOWKMeansTrainerCreate(int clusterCount, ref MCvTermCriteria termcrit, int attempts, CvEnum.KMeansInitType flags);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWKMeansTrainerRelease(ref IntPtr trainer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvBOWKMeansTrainerGetDescriptorCount(IntPtr trainer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWKMeansTrainerAdd(IntPtr trainer, IntPtr descriptors);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWKMeansTrainerCluster(IntPtr trainer, IntPtr cluster);
   }
}
