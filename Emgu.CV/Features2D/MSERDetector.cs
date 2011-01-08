using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped CvMSERParams structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MSERDetector : IKeyPointDetector
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvMSERKeyPoints(
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         ref MSERDetector mser);
      #endregion

      /// <summary>
      /// Create a MSER detector using the specific parameters
      /// </summary>
      /// <param name="delta">Use 5 as defalut. In the code, it compares (size_{i}-size_{i-delta})/size_{i-delta}</param>
      /// <param name="maxArea">Use 60 as default. Prune the area which bigger than max_area</param>
      /// <param name="minArea">Use 14400 as default. Prune the area which smaller than min_area</param>
      /// <param name="maxVariation">Use .25f as default. Prune the area have simliar size to its children</param>
      /// <param name="minDiversity">Use .2f as default. Trace back to cut off mser with diversity &lt; min_diversity</param>
      /// <param name="maxEvolution">Use 200 as default. For color image, the evolution steps</param>
      /// <param name="areaThreshold">Use 1.01 as default. The area threshold to cause re-initialize</param>
      /// <param name="minMargin">Use 0.003 as default. Ignore too small margin</param>
      /// <param name="edgeBlurSize">Use 5 as default. The aperture size for edge blur</param>
      public MSERDetector(
         int delta, int maxArea, int minArea, float maxVariation, float minDiversity,
         int maxEvolution, double areaThreshold, double minMargin, int edgeBlurSize)
      {
         Delta = delta;
         MaxArea = maxArea;
         MinArea = minArea;
         MaxVariation = maxVariation;
         MinDiversity = minDiversity;
         MaxEvolution = maxEvolution;
         AreaThreshold = areaThreshold;
         MinMargin = minMargin;
         EdgeBlurSize = edgeBlurSize;
      }

      /// <summary>
      /// Get the default MSER parameters
      /// </summary>
      /// <returns>The default MSER parameter</returns>
      public void Init()
      {
         Delta = 5;
         MinArea = 60;
         MaxArea = 14400;
         MaxVariation = .25f;
         MinDiversity = .2f;
         MaxEvolution = 200;
         AreaThreshold = 1.01;
         MinMargin = 0.003;
         EdgeBlurSize = 5;
      }

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


      /// <summary>
      /// Extracts the contours of Maximally Stable Extremal Regions
      /// </summary>
      /// <param name="image">The image where mser will be extracted from</param>
      /// <param name="mask">Can be null if not needed. Optional parameter for the region of interest</param>
      /// <param name="storage">The storage where the contour will be saved</param>
      /// <returns>The MSER regions</returns>
      public Seq<Point>[] ExtractContours(IImage image, Image<Gray, Byte> mask, MemStorage storage)
      {
         IntPtr mserPtr = new IntPtr();
         CvInvoke.cvExtractMSER(image.Ptr, mask, ref mserPtr, storage, this);
         IntPtr[] mserSeq = new Seq<IntPtr>(mserPtr, storage).ToArray();
         return Array.ConvertAll<IntPtr, Seq<Point>>(mserSeq, delegate(IntPtr ptr) { return new Seq<Point>(ptr, storage); });
      }

      /// <summary>
      /// Detect the MSER keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract MSER keypoints from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of MSER key points</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            CvMSERKeyPoints(image, mask, kpts, ref this);
            return kpts.ToArray();
         }
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the MSER keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract keypoints from</param>
      /// <returns>The array of fast keypoints</returns>
      public Emgu.CV.Structure.MKeyPoint[] DetectKeyPoints(Image<Emgu.CV.Structure.Gray, byte> image)
      {
         return DetectKeyPoints(image, null);
      }

      #endregion
   }
}
