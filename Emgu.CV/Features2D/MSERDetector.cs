//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped CvMSERParams structure
   /// </summary>
   public class MSERDetector : DisposableObject, IKeyPointDetector
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvMserGetFeatureDetector(ref MCvMSERParams detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvMserFeatureDetectorRelease(ref IntPtr detector);
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
         _delta = delta;
         _maxArea = maxArea;
         _minArea = minArea;
         _maxVariation = maxVariation;
         _minDiversity = minDiversity;
         _maxEvolution = maxEvolution;
         _areaThreshold = areaThreshold;
         _minMargin = minMargin;
         _edgeBlurSize = edgeBlurSize;

         MCvMSERParams p = GetMSERParameters();
         _featureDetectorPtr = CvMserGetFeatureDetector(ref p);
      }

      /// <summary>
      /// Create the default MSER detector
      /// </summary>
      public MSERDetector()
         : this(5, 14400, 60, .25f, .2f, 200, 1.01, 0.003, 5)
      {
      }

      private int _delta;
      private int _maxArea;
      private int _minArea;
      private float _maxVariation;
      private float _minDiversity;
      private int _maxEvolution;
      private double _areaThreshold;
      private double _minMargin;
      private int _edgeBlurSize;

      private IntPtr _featureDetectorPtr;

      /// <summary>
      /// Delta, in the code, it compares (size_{i}-size_{i-delta})/size_{i-delta}
      /// </summary>
      public int Delta { get { return _delta; } }

      /// <summary>
      /// Prune the area which bigger than max_area
      /// </summary>
      public int MaxArea { get { return _maxArea; } }
      /// <summary>
      /// Prune the area which smaller than min_area
      /// </summary>
      public int MinArea { get { return _minArea; } }

      /// <summary>
      /// Prune the area have simliar size to its children
      /// </summary>
      public float MaxVariation { get { return _maxVariation; } }

      /// <summary>
      /// Trace back to cut off mser with diversity &lt; min_diversity
      /// </summary>
      public float MinDiversity { get { return _minDiversity; } }

      /// <summary>
      /// For color image, the evolution steps
      /// </summary>
      public int MaxEvolution { get { return _maxEvolution; } }

      /// <summary>
      /// The area threshold to cause re-initialize
      /// </summary>
      public double AreaThreshold { get { return _areaThreshold; } }

      /// <summary>
      /// Ignore too small margin
      /// </summary>
      public double MinMargin { get { return _minMargin; } }

      /// <summary>
      /// The aperture size for edge blur
      /// </summary>
      public int EdgeBlurSize { get { return _edgeBlurSize; } }


      /// <summary>
      /// Extracts the contours of Maximally Stable Extremal Regions
      /// </summary>
      /// <param name="image">The image where mser will be extracted from</param>
      /// <param name="mask">Can be null if not needed. Optional parameter for the region of interest</param>
      /// <param name="storage">The storage where the contour will be saved</param>
      /// <returns>The MSER regions</returns>
      public Seq<Point>[] ExtractContours(IImage image, Image<Gray, Byte> mask, MemStorage storage)
      {
         MCvMSERParams p = GetMSERParameters();
         IntPtr mserPtr = new IntPtr();
         CvInvoke.cvExtractMSER(image.Ptr, mask, ref mserPtr, storage, p);
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
         using (VectorOfKeyPoint kpts = DetectKeyPointsRaw(image, mask))
         {
            return kpts.ToArray();
         }
      }

      #region KeyPointDetector Members
      /// <summary>
      /// Detect the MSER keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract MSER keypoints from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of MSER key points</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_featureDetectorPtr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      public IntPtr FeatureDetectorPtr
      {
         get
         {
            return _featureDetectorPtr;
         }
      }
      #endregion

      /// <summary>
      /// Get the MSER parameters
      /// </summary>
      /// <returns>The MSER parameters</returns>
      public MCvMSERParams GetMSERParameters()
      {
         MCvMSERParams p = new MCvMSERParams();
         p.Delta = Delta;
         p.MaxArea = MaxArea;
         p.MinArea = MinArea;
         p.MaxVariation = MaxVariation;
         p.MinDiversity = MinDiversity;
         p.MaxEvolution = MaxEvolution;
         p.AreaThreshold = AreaThreshold;
         p.MinMargin = MinMargin;
         p.EdgeBlurSize = EdgeBlurSize;
         return p;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvMserFeatureDetectorRelease(ref _featureDetectorPtr);
      }
   }
}
