//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   ///<summary> 
   /// A Uniform Multi-dimensional Dense Histogram 
   ///</summary>
   [Serializable]
   public class DenseHistogram : UnmanagedObject, ISerializable, IEquatable<DenseHistogram>
   {
      private MatND<float> _matND;

      #region Constructors
      /// <summary>
      /// Creates a uniform 1-D histogram of the specified size
      /// </summary>
      /// <param name="binSize">The number of bins in this 1-D histogram. </param>
      /// <param name="range">The upper and lower boundary of the bin</param>
      public DenseHistogram(int binSize, RangeF range)
         : this(new int[1] { binSize }, new RangeF[1] { range })
      {
      }

      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public DenseHistogram(SerializationInfo info, StreamingContext context)
      {
         MatND<float> matND = new MatND<float>(info, context);
         RangeF[] ranges = (RangeF[]) info.GetValue("Ranges", typeof(RangeF[]));
         InitializeComponent(matND, ranges);
      }

      private void InitializeComponent(MatND<float> matND, RangeF[] ranges)
      {
         _matND = matND;
         MCvMatND cvMatND = _matND.MCvMatND;

         Debug.Assert(
            ranges.Length == cvMatND.dims,
            "incompatible dimension");
         _ptr = Marshal.AllocHGlobal(StructSize.MCvHistogram);

         #region parse the ranges to appropriate format
         GCHandle rangesHandle = GCHandle.Alloc(ranges, GCHandleType.Pinned);
         IntPtr[] rangesPts = new IntPtr[ranges.Length];
         Int64 address = rangesHandle.AddrOfPinnedObject().ToInt64();
         for (int i = 0; i < rangesPts.Length; i++)
            rangesPts[i] = new IntPtr(address + i * StructSize.RangF);
         #endregion

         CvInvoke.cvMakeHistHeaderForArray(
            cvMatND.dims,
            Array.ConvertAll<MCvMatND.Dimension, int>(cvMatND.dim, delegate(MCvMatND.Dimension d) {
            return d.Size; }), //binSizes
            _ptr,
            _matND.MCvMatND.data,
            rangesPts,
            1);

         rangesHandle.Free();

      }

      /// <summary>
      /// Creates a uniform multi-dimension histogram of the specified size
      /// </summary>
      /// <param name="binSizes">The length of this array is the dimension of the histogram. The values of the array contains the number of bins in each dimension. The total number of bins eaquals the multiplication of all numbers in the array</param>
      /// <param name="ranges">the upper and lower boundaries of the bins</param>
      public DenseHistogram(int[] binSizes, RangeF[] ranges)
      {
         InitializeComponent(new MatND<float>(binSizes), ranges);
      }
      #endregion

      ///<summary> 
      /// Clear this histogram
      ///</summary>
      public void Clear()
      {
         CvInvoke.cvClearHist(_ptr);
      }

      /// <summary>
      /// Project the images to the histogram bins 
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the image</typeparam>
      /// <param name="imgs">images to project</param>
      /// <param name="accumulate">If it is true, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online. </param>
      /// <param name="mask">Can be null if not needed. The operation mask, determines what pixels of the source images are counted</param>
      public void Calculate<TDepth>(Image<Gray, TDepth>[] imgs, bool accumulate, Image<Gray, Byte> mask) where TDepth : new()
      {
         IntPtr[] ptrs = new IntPtr[imgs.Length];
         for (int i = 0; i < imgs.Length; i++)
            ptrs[i] = imgs[i].Ptr;
         Calculate(
            ptrs,
            accumulate,
            mask);
      }

      /// <summary>
      /// Project the matrices to the histogram bins 
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the image</typeparam>
      /// <param name="matrices">Matrices to project</param>
      /// <param name="accumulate">If it is true, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online. </param>
      /// <param name="mask">Can be null if not needed. The operation mask, determines what pixels of the source images are counted</param>
      public void Calculate<TDepth>(Matrix<TDepth>[] matrices, bool accumulate, Matrix<Byte> mask) where TDepth : new()
      {
         IntPtr[] ptrs = new IntPtr[matrices.Length];
         for (int i = 0; i < matrices.Length; i++)
            ptrs[i] = matrices[i].Ptr;
         Calculate(ptrs, accumulate, mask);
      }

      /// <summary>
      /// Project the images to the histogram bins 
      /// </summary>
      /// <param name="imgs">images to project</param>
      /// <param name="accumulate">If it is true, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online. </param>
      /// <param name="mask">Can be null if not needed. The operation mask, determines what pixels of the source images are counted</param>
      public void Calculate(IImage[] imgs, bool accumulate, Image<Gray, Byte> mask)
      {
         IntPtr[] ptrs = new IntPtr[imgs.Length];
         for (int i = 0; i < imgs.Length; i++)
            ptrs[i] = imgs[i].Ptr;
         Calculate(ptrs, accumulate, mask);
      }

      private void Calculate(IntPtr[] arrays, bool accumulate, CvArray<Byte> mask)
      {
         Debug.Assert(arrays.Length == Dimension, Properties.StringTable.IncompatibleDimension);
         CvInvoke.cvCalcHist(
            arrays,
            _ptr,
            accumulate,
            mask == null ? IntPtr.Zero : mask.Ptr);
      }

      /// <summary>
      /// Finds the minimum and maximum histogram bins and their positions
      /// </summary>
      /// <remarks>
      /// Among several extremums with the same value the ones with minimum index (in lexicographical order). 
      /// In case of several maximums or minimums the earliest in lexicographical order extrema locations are returned.
      /// </remarks>
      /// <param name="minValue">Pointer to the minimum value of the histogram </param>
      /// <param name="maxValue">Pointer to the maximum value of the histogram </param>
      /// <param name="minLocation">Pointer to the array of coordinates for minimum </param>
      /// <param name="maxLocation">Pointer to the array of coordinates for maximum </param>
      public void MinMax(out float minValue, out float maxValue, out int[] minLocation, out int[] maxLocation)
      {
         minValue = 0;
         maxValue = 0;
         int dimension = Dimension;
         minLocation = new int[dimension];
         maxLocation = new int[dimension];
         CvInvoke.cvGetMinMaxHistValue(_ptr, ref minValue, ref maxValue, minLocation, maxLocation);
      }

      ///<summary> 
      /// Backproject the histogram into a gray scale image
      ///</summary>
      ///<param name="srcs">Source images, all are of the same size and type</param>
      ///<returns>Destination back projection image of the same type as the source images</returns>
      ///<typeparam name="TDepth">The type of depth of the image</typeparam>
      public Image<Gray, TDepth> BackProject<TDepth>(Image<Gray, TDepth>[] srcs) where TDepth : new()
      {
         Debug.Assert(srcs.Length == Dimension, Properties.StringTable.IncompatibleDimension);

         IntPtr[] imgPtrs =
             Array.ConvertAll<Image<Gray, TDepth>, IntPtr>(
                 srcs,
                 delegate(Image<Gray, TDepth> img) {
            return img.Ptr; });

         Image<Gray, TDepth> res = srcs[0].CopyBlank();
         CvInvoke.cvCalcBackProject(imgPtrs, res.Ptr, _ptr);
         return res;
      }

      /// <summary> 
      /// Compares histogram over each possible rectangular patch of the specified size in the input images, and stores the results to the output map dst.
      /// </summary>
      /// <param name="srcs">Source images, all are of the same size and type</param>
      /// <param name="factor">Normalization factor for histograms, will affect normalization scale of destination image, pass 1 if unsure. </param>
      /// <param name="patchSize">Size of patch slid though the source images.</param>
      /// <param name="method">Comparison method, passed to cvCompareHist.</param>
      /// <typeparam name="TDepth">The type of depth of the image</typeparam>
      /// <returns>Destination back projection image of the same type as the source images</returns>
      public Image<Gray, Single> BackProjectPatch<TDepth>(Image<Gray, TDepth>[] srcs, System.Drawing.Size patchSize, CvEnum.HISTOGRAM_COMP_METHOD method, double factor) where TDepth : new()
      {
         Debug.Assert(srcs.Length == Dimension, Properties.StringTable.IncompatibleDimension);

         IntPtr[] imgPtrs = new IntPtr[srcs.Length];
         for (int i = 0; i < srcs.Length; i++)
            imgPtrs[i] = srcs[i].Ptr;

         Size imgSize = srcs[0].Size;
         Image<Gray, Single> res = new Image<Gray, float>(imgSize.Width - patchSize.Width + 1, imgSize.Height - patchSize.Height + 1);
         CvInvoke.cvCalcBackProjectPatch(imgPtrs, res.Ptr, patchSize, Ptr, method, factor);
         return res;
      }

      ///<summary> 
      /// Backproject the histogram into a matrix
      ///</summary>
      ///<param name="srcs">Source matrices, all are of the same size and type</param>
      ///<returns>Destination back projection matrix of the sametype as the source matrices</returns>
      ///<typeparam name="TDepth">The type of depth of the matrix</typeparam>
      public Matrix<TDepth> BackProject<TDepth>(Matrix<TDepth>[] srcs) where TDepth : new()
      {
         Debug.Assert(srcs.Length == Dimension, Properties.StringTable.IncompatibleDimension);

         IntPtr[] imgPtrs = new IntPtr[srcs.Length];
         for (int i = 0; i < srcs.Length; i++)
            imgPtrs[i] = srcs[i].Ptr;

         Matrix<TDepth> res = new Matrix<TDepth>(srcs[0].Size);
         CvInvoke.cvCalcBackProject(imgPtrs, res.Ptr, _ptr);
         return res;
      }

      ///<summary>
      ///Clears histogram bins that are below the specified threshold.
      ///</summary>
      ///<param name="thresh">The threshold used to clear the bins</param>
      public void Threshold(double thresh)
      {
         CvInvoke.cvThreshHist(_ptr, thresh);
      }

      /// <summary>
      ///  normalizes the histogram bins by scaling them, such that the sum of the bins becomes equal to factor
      /// </summary>
      /// <param name="factor">the sum of the bins after normalization</param>
      public void Normalize(double factor)
      {
         CvInvoke.cvNormalizeHist(Ptr, factor);
      }

      /// <summary>
      /// Copy this histogram to <paramref name="destination"/> 
      /// </summary>
      /// <param name="destination">The histogram to copy to</param>
      public void Copy(DenseHistogram destination)
      {
         IntPtr dst = destination.Ptr;
         CvInvoke.cvCopyHist(_ptr, ref dst);
      }

      #region Indexer

      /// <summary>
      /// Get the specific bin value from the 1D Histogram
      /// </summary>
      /// <param name="index0">The 0th index of the bin</param>
      /// <returns>The value in the histogram bin</returns>
      public double this[int index0]
      {
         get
         {
            return CvInvoke.cvQueryHistValue_1D(_ptr, index0);
         }
      }

      /// <summary>
      /// Get the specific bin value from the 2D Histogram
      /// </summary>
      /// <param name="index0">The 0th index of the bin</param>
      /// <param name="index1">The 1st index of the bin</param>
      /// <returns>The value in the histogram bin</returns>
      public double this[int index0, int index1]
      {
         get
         {
            return CvInvoke.cvQueryHistValue_2D(_ptr, index0, index1);
         }
      }

      /// <summary>
      /// Get the specific bin value from the 2D Histogram
      /// </summary>
      /// <param name="index0">The 0th index of the bin</param>
      /// <param name="index1">The 1st index of the bin</param>
      /// <param name="index2">The 2nd index of the bin</param>
      /// <returns>The value in the histogram bin</returns>
      public double this[int index0, int index1, int index2]
      {
         get
         {
            return CvInvoke.cvQueryHistValue_3D(_ptr, index0, index1, index2);
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Get the equivalent MCvHistogram structure 
      /// </summary>
      public MCvHistogram MCvHistogram
      {
         get
         {
            return (MCvHistogram) Marshal.PtrToStructure(Ptr, typeof(MCvHistogram));
         }
      }

      /// <summary>
      /// Get the number of dimensions for the histogram
      /// </summary>
      public int Dimension { get { return MCvHistogram.mat.dims; } }

      /// <summary>
      /// Get the size of the bin dimensions
      /// </summary>
      public MCvMatND.Dimension[] BinDimension
      {
         get
         {
            MCvHistogram h = MCvHistogram;
            MCvMatND.Dimension[] res = new MCvMatND.Dimension[h.mat.dims];
            Array.Copy(h.mat.dim, res, h.mat.dims);
            return res;
         }
      }

      /// <summary>
      /// Get the MatND representation of this dense histogram. Do not dispose this MatND
      /// </summary>
      public MatND<float> MatND { get { return _matND; } }

      /// <summary>
      /// Get the ranges of this histogram
      /// </summary>
      public RangeF[] Ranges
      {
         get
         {
            MCvHistogram h = MCvHistogram;
            RangeF[] res = h.thresh;
            Array.Resize(ref res, h.mat.dims);
            return res;
         }
      }
      #endregion

      #region Implement UnmanagedObject
      /// <summary>
      /// Release the histogram and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            Marshal.FreeHGlobal(_ptr);
            _ptr = IntPtr.Zero;
         }
      }

      /// <summary>
      /// Release the managed resources associated with this dense histogram
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_matND != null)
         {
            _matND.Dispose();
            _matND = null;
         }
      }
      #endregion

      #region ISerializable Members
      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         //info.AddValue("MatND", MatND);
         _matND.GetObjectData(info, context);
         info.AddValue("Ranges", Ranges);
      }

      #endregion

      #region IEquatable<DenseHistogram> Members
      /// <summary>
      /// Return true if the two DenseHistogram equals
      /// </summary>
      /// <param name="other">The other DenseHistogram to compare with</param>
      /// <returns>True if the two DenseHistogram equals</returns>
      public bool Equals(DenseHistogram other)
      {
         RangeF[] ranges1 = Ranges;
         RangeF[] ranges2 = other.Ranges;
         if (ranges1.Length != ranges2.Length)
            return false;

         for (int i = 0; i < ranges1.Length; i++)
            if (!ranges1[i].Equals(ranges2[i]))
               return false;

         return MatND.Equals(other.MatND);
      }

      #endregion
   }
}
