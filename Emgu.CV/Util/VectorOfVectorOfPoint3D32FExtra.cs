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
   public partial class VectorOfVectorOfPoint3D32F : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      /// <summary>
      /// Convert the standard vector to arrays of Point
      /// </summary>
      /// <returns>Arrays of Point</returns>
      public MCvPoint3D32f[][] ToArrayOfArray()
      {
         int size = Size;
         MCvPoint3D32f[][] res = new MCvPoint3D32f[size][];
         for (int i = 0; i < size; i++)
         {
            VectorOfPoint3D32F v = this[i];
            res[i] = v.ToArray();
         }
         return res;
      }

      private VectorOfPoint3D32F[] _tmp;

      /// <summary>
      /// Create VectorOfVectorOfPoint3D32F from vector of vector of points
      /// </summary>
      /// <param name="points"></param>
      public VectorOfVectorOfPoint3D32F(MCvPoint3D32f[][] points)
         : this()
      {
         _tmp = new VectorOfPoint3D32F[points.Length];
         for (int i = 0; i < points.Length; i++)
         {
            _tmp[i] = new VectorOfPoint3D32F(points[i]);
         }
         Push(_tmp);
      }

      /// <summary>
      /// Release the managed resource
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
