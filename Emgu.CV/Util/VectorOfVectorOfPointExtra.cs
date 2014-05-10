//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of VectorOfPoint.
   /// </summary>
   public partial class VectorOfVectorOfPoint : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      /// <summary>
      /// Convert the standard vector to arrays of Point
      /// </summary>
      /// <returns>Arrays of Point</returns>
      public Point[][] ToArrayOfArray()
      {
         int size = Size;
         Point[][] res = new Point[size][];
         for (int i = 0; i < size; i++)
         {
            using (VectorOfPoint v = this[i])
            {
               res[i] = v.ToArray();
            }
         }
         return res;
      }

      private VectorOfPoint[] _tmp;

      /// <summary>
      /// Create VectorOfVectorOfPoint from vector of vector of points
      /// </summary>
      /// <param name="points"></param>
      public VectorOfVectorOfPoint(Point[][] points)
         : this()
      {
         _tmp = new VectorOfPoint[points.Length];
         for (int i = 0; i < points.Length; i++)
         {
            _tmp[i] = new VectorOfPoint(points[i]);
         }
         Push(_tmp);
      }

      /// <summary>
      /// Release tehe managed resources
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (_tmp != null)
         {
            for (int i = 0; i < _tmp.Length; i++)
            {
               if (_tmp[i] != null)
               {
                  _tmp[i].Dispose();
               }
            }
            _tmp = null;
         }
      }
   }
}
