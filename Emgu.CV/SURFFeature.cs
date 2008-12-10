using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A SURF feature
   /// </summary>
   public class SURFFeature : DisposableObject
   {
      private MCvSURFPoint _point;

      /// <summary>
      /// The SURF point
      /// </summary>
      public MCvSURFPoint Point
      {
         get { return _point; }
         set { _point = value; }
      }

      private Matrix<float> _descriptor;

      /// <summary>
      /// The SURF descriptor as a matrix (A 64x1 matrix for regular descriptor; A 128x1 matrix for extended descriptor)
      /// </summary>
      public Matrix<float> Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      /// <summary>
      /// Create a SURF feature from the specific point and descriptor
      /// </summary>
      /// <param name="point">The MCvSURFPoint structure</param>
      /// <param name="descriptor">The point descriptor</param>
      public SURFFeature(ref MCvSURFPoint point, Matrix<float> descriptor)
      {
         _point = point;
         _descriptor = descriptor;
      }

      /// <summary>
      /// Release the memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         _descriptor.Dispose();
      }
   }
}
