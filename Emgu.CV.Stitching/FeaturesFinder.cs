//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   /// <summary>
   /// Finds features in the given image.
   /// </summary>
   public abstract class FeaturesFinder : UnmanagedObject
   {
      /// <summary>
      /// Pointer to the unmanaged FeaturesFinder object
      /// </summary>
      protected IntPtr FeaturesFinderPtr;
   }

   /*
   /// <summary>
   /// SURF features finder
   /// </summary>
   public class SurfFeaturesFinder : FeaturesFinder
   {
      /// <summary>
      /// Create SURF Features finder
      /// </summary>
      /// <param name="hessThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="numOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      /// </param>
      /// <param name="numLayers">
      /// The number of layers within each octave
      /// </param>
      /// <param name="numOctavesDescr">The number of Octaves descriptors</param>
      /// <param name="numLayersDescr">The number of Layers descriptors</param>
      public SurfFeaturesFinder(
         double hessThresh = 300, int numOctaves = 3, int numLayers = 4,
         int numOctavesDescr = 3, int numLayersDescr = 4)
      {
         _ptr = StitchingInvoke.cveSurfFeaturesFinderCreate(
            hessThresh, numOctaves, numLayers, numOctavesDescr, numLayersDescr,
            ref FeaturesFinderPtr);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this SurfFeature finder.
      /// </summary>
      protected override void DisposeObject()
      {
         StitchingInvoke.cveSurfFeaturesFinderRelease(ref _ptr);
      }
   }
      
   /// <summary>
   /// Gpu version of the SURF features finder
   /// </summary>
   public class SurfFeaturesFinderGpu : FeaturesFinder
   {
      /// <summary>
      /// Create the GPU version of SURF Features finder
      /// </summary>
      /// <param name="hessThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="numOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      /// </param>
      /// <param name="numLayers">
      /// The number of layers within each octave
      /// </param>
      /// <param name="numOctavesDescr">The number of Octaves descriptors</param>
      /// <param name="numLayersDescr">The number of Layers descriptors</param>
      public SurfFeaturesFinderGpu(
         double hessThresh = 300, int numOctaves = 3, int numLayers = 4,
         int numOctavesDescr = 3, int numLayersDescr = 4)
      {
         _ptr = StitchingInvoke.cveSurfFeaturesFinderGpuCreate(
            hessThresh, numOctaves, numLayers, numOctavesDescr, numLayersDescr,
            ref FeaturesFinderPtr);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this FeaturesFinder
      /// </summary>
      protected override void DisposeObject()
      {
         StitchingInvoke.cveSurfFeaturesFinderGpuRelease(ref  _ptr);
      }
   }*/

   /// <summary>
   /// ORB features finder.
   /// </summary>
   public class OrbFeaturesFinder : FeaturesFinder
   {
      /// <summary>
      /// Creates an ORB features finder
      /// </summary>
      /// <param name="gridSize">Use (3, 1) for default grid size </param>
      /// <param name="nFeature">The number of desired features. </param>
      /// <param name="scaleFactor">Coefficient by which we divide the dimensions from one scale pyramid level to the next.</param>
      /// <param name="nLevels">The number of levels in the scale pyramid. </param>
      public OrbFeaturesFinder(Size gridSize, int nFeature = 1500, float scaleFactor = 1.3f, int nLevels = 5)
      {
         _ptr = StitchingInvoke.cveOrbFeaturesFinderCreate(
            ref gridSize, nFeature, scaleFactor, nLevels,
            ref FeaturesFinderPtr);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this FeaturesFinder
      /// </summary>
      protected override void DisposeObject()
      {
         StitchingInvoke.cveOrbFeaturesFinderRelease(ref _ptr);
      }
   }

   internal static partial class StitchingInvoke
   {
      /*
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
      */

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOrbFeaturesFinderCreate(ref Size gridSize, int nfeature, float scaleFactor, int nlevels, ref IntPtr f);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveOrbFeaturesFinderRelease(ref IntPtr finder);
   }
}
