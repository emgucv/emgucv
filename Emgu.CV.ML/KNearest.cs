//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.ML
{
   /// <summary>
   /// The KNearest classifier
   /// </summary>
   public class KNearest : StatModel
   {
      /// <summary>
      /// Create a default KNearest classifier
      /// </summary>
      public KNearest()
      {
         _ptr = MlInvoke.CvKNearestDefaultCreate();
      }

      /// <summary>
      /// Creaet a KNearest classifier using the specific traing data
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      public KNearest(Matrix<float> trainData, Matrix<float> responses, Matrix<Byte> sampleIdx, bool isRegression, int maxK)
      {
         _ptr = MlInvoke.CvKNearestCreate(trainData, responses, sampleIdx == null? IntPtr.Zero : sampleIdx.Ptr, isRegression, maxK);
      }

      /// <summary>
      /// Release the classifer and all the memory associated with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvKNearestRelease(ref _ptr);
      }

      /// <summary>
      /// Update the KNearest classifier using the specific traing data.
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="updateBase">
      /// If true, the existing classifer is updated using the new training data;
      /// Otherwise, the classifier is trained from scratch</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData, 
         Matrix<float> responses, 
         Matrix<Byte> sampleIdx, 
         bool isRegression, 
         int maxK,
         bool updateBase)
      {
         return MlInvoke.CvKNearestTrain(
            _ptr, 
            trainData.Ptr, 
            responses.Ptr, 
            sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr, 
            isRegression, 
            maxK, 
            updateBase);
      }
      
      /// <summary>
      /// For each input vector (which are rows of the matrix <paramref name="samples"/>) the method finds k &lt;= get_max_k() nearest neighbor. In case of regression, the predicted result will be a mean value of the particular vector's neighbor responses. In case of classification the class is determined by voting.
      /// </summary>
      /// <param name="samples">The sample matrix where each row is a sample</param>
      /// <param name="k">The number of nearest neighbor to find</param>
      /// <param name="results">
      /// Can be null if not needed.
      /// If regression, return a mean value of the particular vector's neighbor responses;
      /// If classification, return the class determined by voting.
      /// </param>
      /// <param name="kNearestNeighbors">Should be null if not needed. Setting it to non-null values incures a performance panalty. A matrix of (k * samples.Rows) rows and (samples.Cols) columns that will be filled the data of the K nearest-neighbor for each sample</param>
      /// <param name="neighborResponses">Should be null if not needed. The response of the neighbors. A vector of k*_samples->rows elements.</param>
      /// <param name="dist">Should be null if not needed. The distances from the input vectors to the neighbors. A vector of k*_samples->rows elements.</param>
      /// <returns>In case of regression, the predicted result will be a mean value of the particular vector's neighbor responses. In case of classification the class is determined by voting</returns>
      public float FindNearest(
         Matrix<float> samples,
         int k,
         Matrix<float> results,
         Matrix<float> kNearestNeighbors,
         Matrix<float> neighborResponses,
         Matrix<float> dist)
      {
         IntPtr[] neighbors = null;
         if (kNearestNeighbors != null)
         {
            Debug.Assert(kNearestNeighbors.Rows == k * samples.Rows && kNearestNeighbors.Cols == samples.Cols, "The kNeighbors must have (k*samples.Rows) rows and samples.Cols columns.");
            neighbors = new IntPtr[k * samples.Rows];
         } 
         float res = MlInvoke.CvKNearestFindNearest(_ptr, samples.Ptr, k, results, neighbors, neighborResponses, dist);
         if (kNearestNeighbors != null)
         {
            IntPtr data; int step; Size size;
            CvInvoke.cvGetRawData(kNearestNeighbors.Ptr, out data, out step, out size);
            Int64 dataAddress = data.ToInt64();
            int elements = k * samples.Rows;
            int length = samples.Cols * sizeof(float);
            for (int i = 0; i < elements; i++)
            {
               Emgu.Util.Toolbox.memcpy(new IntPtr(dataAddress + i * step), neighbors[i], length);
            }
         }
         return res;
      }
   }
}
