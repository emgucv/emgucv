using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvMSERParams structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvMSERParams
   {
      /// <summary>
      /// Get the default MSER parameters
      /// </summary>
      /// <returns>The default MSER parameter</returns>
      public static MCvMSERParams GetDefaultParameter()
      {
         MCvMSERParams param = new MCvMSERParams();
         param.delta = 5;
         param.minArea = 60;
         param.maxArea = 14400;
         param.maxVariation = .25f;
         param.minDiversity = .2f;
         param.maxEvolution = 200;
         param.areaThreshold = 1.01;
         param.minMargin = 0.003;
         param.edgeBlurSize = 5;
         return param;
      }

      /// <summary>
      /// Delta, in the code, it compares (size_{i}-size_{i-delta})/size_{i-delta}
      /// </summary>
      public int delta;

      /// <summary>
      /// Prune the area which bigger than max_area
      /// </summary>
      public int maxArea;
      /// <summary>
      /// Prune the area which smaller than min_area
      /// </summary>
      public int minArea;

      /// <summary>
      /// Prune the area have simliar size to its children
      /// </summary>
      public float maxVariation;

      /// <summary>
      /// Trace back to cut off mser with diversity &lt; min_diversity
      /// </summary>
      public float minDiversity;

      #region  Params for MSER of color image
      /// <summary>
      /// For color image, the evolution steps
      /// </summary>
      public int maxEvolution;

      /// <summary>
      /// The area threshold to cause re-initialize
      /// </summary>
      public double areaThreshold;

      /// <summary>
      /// ignore too small margin
      /// </summary>
      public double minMargin;

      /// <summary>
      /// the aperture size for edge blur
      /// </summary>
      public int edgeBlurSize;
      #endregion
   }
}
