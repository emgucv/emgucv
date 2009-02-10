using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   /// A Uniform Multi-dimensional Histogram 
   ///</summary>
   public class Histogram : UnmanagedObject
   {
      private float[] _data;
      private GCHandle _dataHandle;

      #region Constructors
      /// <summary>
      /// Creates a uniform 1 dimension histogram of the specified size
      /// </summary>
      /// <param name="binSize">The length of this array is the dimension of the histogram. The values of the array contains the number of bins in each dimension. The total number of bins eaquals the multiplication of all numbers in the array</param>
      /// <param name="range">the upper and lower boundaries of the bins</param>
      public Histogram(int binSize, RangeF range)
         : this (new int[1] {binSize}, new RangeF[1] {range})
      {
      }

      /// <summary>
      /// Creates a uniform multi dimension histogram of the specified size
      /// </summary>
      /// <param name="binSizes">The length of this array is the dimension of the histogram. The values of the array contains the number of bins in each dimension. The total number of bins eaquals the multiplication of all numbers in the array</param>
      /// <param name="ranges">the upper and lower boundaries of the bins</param>
      public Histogram(int[] binSizes, RangeF[] ranges)
      {
         Debug.Assert(
             ranges.Length == binSizes.Length,
             "incompatible dimension");

         #region parse the ranges to appropriate format
         GCHandle rangesHandle = GCHandle.Alloc(ranges, GCHandleType.Pinned);
         IntPtr[] rangesPts = new IntPtr[ranges.Length];
         Int64 address = rangesHandle.AddrOfPinnedObject().ToInt64();
         for (int i = 0; i < rangesPts.Length; i++)
            rangesPts[i] = new IntPtr( address + i * (Marshal.SizeOf(typeof(RangeF))) );
         #endregion

         #region alocate memory for the underline data storage
         int size = 1;
         foreach (int s in binSizes) size *= s;
         _data = new float[size];
         _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
         #endregion 

         _ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvHistogram)));

         CvInvoke.cvMakeHistHeaderForArray(
            binSizes.Length,
            binSizes,
            _ptr,
            _dataHandle.AddrOfPinnedObject(),
            rangesPts,
            1);
      }
      #endregion

      ///<summary> 
      /// Clear this histogram
      ///</summary>
      public void Clear()
      {
         CvInvoke.cvClearHist(_ptr);
      }

      ///<summary> 
      /// Project the images to the histogram bins 
      ///</summary>
      ///<param name="imgs">image to project</param>
      public void Accumulate<TDepth>(Image<Gray, TDepth>[] imgs)
      {
         Accumulate<TDepth>(imgs, null);
      }

      /// <summary>
      /// Project the images to the histogram bins 
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the image</typeparam>
      /// <param name="imgs">image to project</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public void Accumulate<TDepth>(Image<Gray, TDepth>[] imgs, Image<Gray, Byte> mask)
      {
         Debug.Assert(imgs.Length == Dimension, "incompatible dimension");

         IntPtr[] imgPtrs =
             System.Array.ConvertAll<Image<Gray, TDepth>, IntPtr>(
                 imgs,
                 delegate(Image<Gray, TDepth> img) { return img.Ptr; });

         CvInvoke.cvCalcHist(
            imgPtrs, 
            _ptr, 
            true, 
            mask == null ? IntPtr.Zero : mask.Ptr);
      }

      /// <summary>
      /// Project the images to the histogram bins 
      /// </summary>
      /// <param name="imgs"></param>
      public void Accumulate(IImage[] imgs)
      {
         Debug.Assert(imgs.Length == Dimension, "incompatible dimension");
         IntPtr[] imgPtrs =
             System.Array.ConvertAll<IImage, IntPtr>(
                 imgs,
                 delegate(IImage img) { return img.Ptr; });
         CvInvoke.cvCalcHist(imgPtrs, _ptr, true, IntPtr.Zero);
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
      /// Backproject the histogram into an gray scale image
      ///</summary>
      public Image<Gray, TDepth> BackProject<TDepth>(Image<Gray, TDepth>[] srcs)
      {
         Debug.Assert(srcs.Length == Dimension, "incompatible dimension");

         IntPtr[] imgPtrs =
             System.Array.ConvertAll<Image<Gray, TDepth>, IntPtr>(
                 srcs,
                 delegate(Image<Gray, TDepth> img) { return img.Ptr; });

         Image<Gray, TDepth> res = srcs[0].CopyBlank();
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
      public void Copy(Histogram destination)
      {
         IntPtr dst = destination.Ptr;
         CvInvoke.cvCopyHist(_ptr, ref dst);
      }

      #region Indexer
      ///<summary> Retrieve item counts for the specific bin </summary>
      [Obsolete("Use the indexer instead, will be removed in the next version")]
      public double Query(params int[] binIndex)
      {
         Debug.Assert(binIndex.Length > 0, "Please specify at least one index");
         Debug.Assert(binIndex.Length == Dimension, "incompatible dimension");

         switch (binIndex.Length)
         {
            case 1:
               return CvInvoke.cvQueryHistValue_1D(_ptr, binIndex[0]);
            case 2:
               return CvInvoke.cvQueryHistValue_2D(_ptr, binIndex[0], binIndex[1]);
            case 3:
               return CvInvoke.cvQueryHistValue_3D(_ptr, binIndex[0], binIndex[1], binIndex[2]);
            default:
               throw new NotImplementedException(String.Format("Retrive from {0} dimensional histogram is not implemented", binIndex.Length));
         }
      }

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

      #region properties
      /// <summary>
      /// Get the equivalent MCvHistogram structure 
      /// </summary>
      public MCvHistogram MCvHistogram
      {
         get
         {
            return (MCvHistogram)Marshal.PtrToStructure(Ptr, typeof(MCvHistogram));
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
      /// Get the data in this histogram
      /// </summary>
      public float[] Data { get { return _data; } }

      /// <summary>
      /// Get the ranges of this histogram
      /// </summary>
      public RangeF[] Ranges
      {
         get
         {
            MCvHistogram h = MCvHistogram;
            RangeF[] res = new RangeF[h.mat.dims];
            Array.Copy(h.thresh, res, res.Length);
            return res;
         }
      }
      #endregion

      #region implement UnmanagedObject
      /// <summary>
      /// Release the histogram and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         Marshal.FreeHGlobal(_ptr);
         _dataHandle.Free();
      }
      #endregion


   }
}
