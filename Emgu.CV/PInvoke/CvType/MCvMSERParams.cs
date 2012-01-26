//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvMSERParams
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvMSERParams
   {
      /// <summary>
      /// Delta, in the code, it compares (size_{i}-size_{i-delta})/size_{i-delta}
      /// </summary>
      public int Delta;

      /// <summary>
      /// Prune the area which bigger than max_area
      /// </summary>
      public int MaxArea;
      /// <summary>
      /// Prune the area which smaller than min_area
      /// </summary>
      public int MinArea;

      /// <summary>
      /// Prune the area have simliar size to its children
      /// </summary>
      public float MaxVariation;

      /// <summary>
      /// Trace back to cut off mser with diversity &lt; min_diversity
      /// </summary>
      public float MinDiversity;

      /// <summary>
      /// For color image, the evolution steps
      /// </summary>
      public int MaxEvolution;

      /// <summary>
      /// The area threshold to cause re-initialize
      /// </summary>
      public double AreaThreshold;

      /// <summary>
      /// Ignore too small margin
      /// </summary>
      public double MinMargin;

      /// <summary>
      /// The aperture size for edge blur
      /// </summary>
      public int EdgeBlurSize;
   }
}
