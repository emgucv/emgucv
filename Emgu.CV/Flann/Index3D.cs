//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Flann
{
   /// <summary>
   /// Create index for 3D points
   /// </summary>
   public class Index3D : UnmanagedObject
   {
      private MCvPoint3D32f[] _points;
      private Index _flannIndex;
      private Matrix<float> _dataMatrix;
      private Matrix<float> _query;
      private Matrix<float> _distance;
      private Matrix<int> _index;
      private GCHandle _dataHandle;

      /// <summary>
      /// Create a flann index for 3D points
      /// </summary>
      /// <param name="points">The IPosition3D array</param>
      /// <param name="ip">The index parameters</param>
      public Index3D(MCvPoint3D32f[] points, IIndexParams ip)
      {
         _points = points;

         _dataHandle = GCHandle.Alloc(_points, GCHandleType.Pinned);
         _dataMatrix = new Matrix<float>(_points.Length, 3, _dataHandle.AddrOfPinnedObject());
         
         _flannIndex = new Index(_dataMatrix, ip);
         
         _query = new Matrix<float>(1, 3);
         _distance = new Matrix<float>(1, 1);
         _index = new Matrix<int>(1, 1);
      }

      /// <summary>
      /// Find the approximate nearest position in 3D
      /// </summary>
      /// <param name="position">The position to start the search from</param>
      /// <param name="squareDist">The square distance of the nearest neighbour</param>
      /// <returns>The index with the nearest 3D position</returns>
      public int ApproximateNearestNeighbour(MCvPoint3D32f position, out double squareDist)
      {
         _query.Data[0, 0] = position.X;
         _query.Data[0, 1] = position.Y;
         _query.Data[0, 2] = position.Z;
         _flannIndex.KnnSearch(_query, _index, _distance, 1, 1);

         squareDist = _distance.Data[0, 0];
         return _index.Data[0, 0];
      }

      /// <summary>
      /// Release the resource used by this object
      /// </summary>
      protected override void DisposeObject()
      {
         _index.Dispose();
         _dataHandle.Free();
         _dataMatrix.Dispose();
         _query.Dispose();
         _distance.Dispose();
      }
   }
}
