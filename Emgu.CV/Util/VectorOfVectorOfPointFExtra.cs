//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public partial class VectorOfVectorOfPointF : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      /// <summary>
      /// Convert the standard vector to arrays of Point
      /// </summary>
      /// <returns>Arrays of Point</returns>
      public PointF[][] ToArrayOfArray()
      {
         int size = Size;
         PointF[][] res = new PointF[size][];
         for (int i = 0; i < size; i++)
         {
            VectorOfPointF v = this[i];
            res[i] = v.ToArray();
         }
         return res;
      }

      private VectorOfPointF[] _tmp;

      public VectorOfVectorOfPointF(PointF[][] points)
         : this()
      {
         _tmp = new VectorOfPointF[points.Length];
         for (int i = 0; i < points.Length; i++)
         {
            _tmp[i] = new VectorOfPointF(points[i]);
         }
         Push(_tmp);
      }

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
