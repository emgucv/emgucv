//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
#if !NETFX_CORE
   [DebuggerTypeProxy(typeof(DenseHistogram.DebuggerProxy))]
#endif
   public class DenseHistogram : Mat
   {
      private int[] _binSizes;
      private RangeF[] _ranges;

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
      /// Creates a uniform multi-dimension histogram of the specified size
      /// </summary>
      /// <param name="binSizes">The length of this array is the dimension of the histogram. The values of the array contains the number of bins in each dimension. The total number of bins eaquals the multiplication of all numbers in the array</param>
      /// <param name="ranges">the upper and lower boundaries of the bins</param>
      public DenseHistogram(int[] binSizes, RangeF[] ranges)
      {
         _binSizes = binSizes;
         _ranges = ranges;
      }
      #endregion

      ///<summary> 
      /// Clear this histogram
      ///</summary>
      public void Clear()
      {
         SetTo(new MCvScalar());
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
         Mat[] mats = new Mat[imgs.Length];
         for (int i = 0; i < imgs.Length; i++)
         {
            mats[i] = imgs[i].Mat;
         }

         Calculate(mats, accumulate, mask);
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
         Mat[] mats = new Mat[matrices.Length];
         for (int i = 0; i < matrices.Length; i++)
         {
            mats[i] = matrices[i].Mat;
         }

         Calculate(mats, accumulate, mask);
      }

      /*
      /// <summary>
      /// Project the images to the histogram bins 
      /// </summary>
      /// <param name="imgs">images to project</param>
      /// <param name="accumulate">If it is true, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online. </param>
      /// <param name="mask">Can be null if not needed. The operation mask, determines what pixels of the source images are counted</param>
      public void Calculate(IImage[] imgs, bool accumulate, Image<Gray, Byte> mask)
      {
         Mat[] mats = new Mat[imgs.Length];
         for (int i = 0; i < imgs.Length; i++)
         {
            mats[i] = imgs[i].Mat;
         }
         return Calculate(mats, accumulate, mask);
      }*/

      private float[] GetRangeAsFloatVec()
      {
         float[] rangevec = new float[_ranges.Length * 2];
         
         int counter = 0;
         for (int i = 0; i < _ranges.Length; i++)
         {
            rangevec[counter++] = _ranges[i].Min;
            rangevec[counter++] = _ranges[i].Max;
         }
         return rangevec;
      }

      private void Calculate(Mat[] arrays, bool accumulate, CvArray<Byte> mask)
      {
#if !(NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO ))
         Debug.Assert(arrays.Length == _binSizes.Length, Properties.StringTable.IncompatibleDimension);
#endif
         int[] channels = new int[arrays.Length];
         for (int i = 0; i < arrays.Length; i++)
            channels[i] = i;

         using (VectorOfMat vm = new VectorOfMat(arrays))
         {
            CvInvoke.CalcHist(vm, channels, mask == null ? null : mask.Mat, this, _binSizes, GetRangeAsFloatVec(), accumulate);
         } 
      }

      ///<summary> 
      /// Backproject the histogram into a gray scale image
      ///</summary>
      ///<param name="srcs">Source images, all are of the same size and type</param>
      ///<returns>Destination back projection image of the same type as the source images</returns>
      ///<typeparam name="TDepth">The type of depth of the image</typeparam>
      public Image<Gray, TDepth> BackProject<TDepth>(Image<Gray, TDepth>[] srcs) where TDepth : new()
      {
#if !(NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO ))
         Debug.Assert(srcs.Length == _binSizes.Length, Properties.StringTable.IncompatibleDimension);
#endif
         using (VectorOfMat vm = new VectorOfMat())
         {
            vm.Push(srcs);

            int[] channels = new int[srcs.Length];
            for (int i = 0; i < channels.Length; i++)
            {
               channels[i] = i;
            }
            Image<Gray, TDepth> res = srcs[0].CopyBlank();
            CvInvoke.CalcBackProject(vm, channels, this, res.Mat, GetRangeAsFloatVec(), 1.0);

            return res;
         }
      }

      ///<summary> 
      /// Backproject the histogram into a matrix
      ///</summary>
      ///<param name="srcs">Source matrices, all are of the same size and type</param>
      ///<returns>Destination back projection matrix of the sametype as the source matrices</returns>
      ///<typeparam name="TDepth">The type of depth of the matrix</typeparam>
      public Matrix<TDepth> BackProject<TDepth>(Matrix<TDepth>[] srcs) where TDepth : new()
      {
#if !(NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO ))
         Debug.Assert(srcs.Length == _binSizes.Length, Properties.StringTable.IncompatibleDimension);
#endif
         using (VectorOfMat vm = new VectorOfMat())
         {
            vm.Push(srcs);

            int[] channels = new int[srcs.Length];
            for (int i = 0; i < channels.Length; i++)
            {
               channels[i] = i;
            }
            Matrix<TDepth> res = new Matrix<TDepth>(srcs[0].Size);
            CvInvoke.CalcBackProject(vm, channels, this, res.Mat, GetRangeAsFloatVec(), 1.0);

            return res;
         }
      }

      #region Properties
      /// <summary>
      /// Get the size of the bin dimensions
      /// </summary>
      public int[] BinDimension
      {
         get
         {
            return _binSizes;
         }
      }

      /// <summary>
      /// Get the ranges of this histogram
      /// </summary>
      public RangeF[] Ranges
      {
         get
         {
            return _ranges;
         }
      }
      #endregion

      /// <summary>
      /// Gets the bin values.
      /// </summary>
      /// <returns>The bin values</returns>
      public float[] GetBinValues()
      {
         if (IsEmpty)
            return null;
         int totalDim = 1;
         for (int i = 0; i < _binSizes.Length; i++)
         {
            totalDim *= _binSizes[i];
         }
         float[] result = new float[totalDim];
         GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
         try
         {
            using (
               Mat m = new Mat(this.Rows, this.Cols, this.Depth, this.NumberOfChannels, handle.AddrOfPinnedObject(),
                  this.Step))
            {
               CopyTo(m);
            }
         }
         finally
         {
            handle.Free();
         }
         return result;
      }
      
         
      

      internal class DebuggerProxy
      {
         private DenseHistogram _v;

         public DebuggerProxy(DenseHistogram v)
         {
            _v = v;
         }

         public float[] BinValues
         {
            get { return _v.GetBinValues(); }
         }
      }

   }
}
