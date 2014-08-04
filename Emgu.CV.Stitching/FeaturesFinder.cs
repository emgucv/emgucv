//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
   public abstract class FeaturesFinder : UnmanagedObject
   {
      protected IntPtr _featuresFinderPtr;
   }

   public class SurfFeaturesFinder : FeaturesFinder
   {
      public SurfFeaturesFinder(
         double hessThresh, int numOctaves, int numLayers,
         int numOctavesDescr, int numLayersDescr)
      {
         _ptr = StitchingInvoke.cveSurfFeaturesFinderCreate(
            hessThresh, numOctaves, numLayers, numOctavesDescr, numLayersDescr,
            ref _featuresFinderPtr);
      }

      protected override void DisposeObject()
      {
         StitchingInvoke.cveOrbFeaturesFinderRelease(ref _ptr);
      }
   }

   public class SurfFeaturesFinderGpu : FeaturesFinder
   {
      public SurfFeaturesFinderGpu(
         double hessThresh, int numOctaves, int numLayers,
         int numOctavesDescr, int numLayersDescr)
      {
         _ptr = StitchingInvoke.cveSurfFeaturesFinderGpuCreate(
            hessThresh, numOctaves, numLayers, numOctavesDescr, numLayersDescr,
            ref _featuresFinderPtr);
      }

      protected override void DisposeObject()
      {
         StitchingInvoke.cveSurfFeaturesFinderGpuRelease(ref  _ptr);
      }
   }

   public class OrbFeaturesFinder : FeaturesFinder
   {
      public OrbFeaturesFinder(Size gridSize, int nfeature = 1500, float scaleFactor = 1.3f, int nlevels = 5)
      {
         _ptr = StitchingInvoke.cveOrbFeaturesFinderCreate(
            ref gridSize, nfeature, scaleFactor, nlevels,
            ref _featuresFinderPtr);
      }

      protected override void DisposeObject()
      {
         StitchingInvoke.cveOrbFeaturesFinderRelease(ref _ptr);
      }
   }

   internal static partial class StitchingInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSurfFeaturesFinderCreate(
         double hessThresh, int numOctaves, int numLayers,
         int numOctavesDescr, int numLayersDescr, ref IntPtr f);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSurfFeaturesFinderRelease(ref IntPtr finder);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSurfFeaturesFinderGpuCreate(
         double hessThresh, int numOctaves, int numLayers,
         int numOctavesDescr, int numLayersDescr, ref IntPtr f);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSurfFeaturesFinderGpuRelease(ref IntPtr finder);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOrbFeaturesFinderCreate(ref Size gridSize, int nfeature, float scaleFactor, int nlevels, ref IntPtr f);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveOrbFeaturesFinderRelease(ref IntPtr finder);
   }
}
