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
   public abstract class FeaturesFinder
   {
      protected IntPtr _featuresFinderPtr;
   }

   public class SurfFeaturesFinder : FeaturesFinder
   {
   }

   internal static partial class StitchingInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSurfFeaturesFinderCreate(
         double hess_thresh, int num_octaves, int num_layers,
         int num_octaves_descr, int num_layers_descr, ref IntPtr f);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSurfFeaturesFinderGpuCreate(
         double hess_thresh, int num_octaves, int num_layers,
         int num_octaves_descr, int num_layers_descr, ref IntPtr f);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOrbFeaturesFinderCreate(ref Size grid_size, int nfeaturea, float scaleFactor, int nlevels, ref IntPtr f);
   }
}
