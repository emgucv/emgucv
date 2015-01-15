//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.Features2D;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped AKAZE detector
   /// </summary>
   public class AKAZE : Feature2D
   {
      /// <summary>
      /// Type of the extracted descriptor
      /// </summary>
      public enum DescriptorType
      {
         /// <summary>
         /// The kaze upright
         /// </summary>
         KazeUpright = 2,
         /// <summary>
         /// The kaze
         /// </summary>
         Kaze = 3,
         /// <summary>
         /// The MLDB upright
         /// </summary>
         MldbUpright = 4,
         /// <summary>
         /// The MLDB
         /// </summary>
         Mldb = 5
      }

      /// <summary>
      /// Create AKAZE using the specific values
      /// </summary>
      /// <param name="descriptorType">Type of the extracted descriptor</param>
      /// <param name="descriptorSize">Size of the descriptor in bits. 0 -> Full size</param>
      /// <param name="descriptorChannels">Number of channels in the descriptor (1, 2, 3)</param>
      /// <param name="threshold">Detector response threshold to accept point</param>
      /// <param name="sublevels"> Default number of sublevels per scale level</param>
      /// <param name="octaves">Maximum octave evolution of the image</param>
      /// <param name="diffusivity">Diffusivity type</param>
      public AKAZE(DescriptorType descriptorType = DescriptorType.Mldb, int descriptorSize = 0, int descriptorChannels = 3,
         float threshold = 0.001f, int octaves = 4, int sublevels = 4, KAZE.Diffusivity diffusivity = KAZE.Diffusivity.PmG2)
      {
         _ptr = CvInvoke.cveAKAZEDetectorCreate(
            descriptorType, descriptorSize, descriptorChannels, 
            threshold, octaves, sublevels, diffusivity,
            ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveAKAZEDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveAKAZEDetectorCreate(
         AKAZE.DescriptorType descriptorType, int descriptorSize, int descriptorChannels,
         float threshold, int octaves, int sublevels, KAZE.Diffusivity diffusivity,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAKAZEDetectorRelease(ref IntPtr detector);
   }
}
