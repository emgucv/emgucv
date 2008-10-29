using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   /// <summary>
   /// A SURF feature
   /// </summary>
   public class SURFFeature
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
      public SURFFeature(ref MCvSURFPoint point, float[,] descriptor)
      {
         _point = point;
         _descriptor = new Matrix<float>(descriptor);
      }

      /*
      /// <summary>
      /// Find from the candidate the best match and return the distance between the descriptors.
      /// </summary>
      /// <param name="candidates">A list of candidate to find a match from</param>
      /// <param name="maxDist">The maximum distance between a match</param>
      /// <param name="match">The returned best matched candidate, if there exist one</param>
      /// <param name="distance">The distance between the descriptors</param>
      public void FindBestMatch(SURFFeature[] candidates, double maxDist,  out SURFFeature match, out double distance)
      {
         match = null;
         distance = -1.0;

         foreach (SURFFeature f in candidates)
         {
            if (f.Point.laplacian != Point.laplacian)
               continue;

            double d = CvInvoke.cvNorm(_descriptor.Ptr, f.Descriptor.Ptr, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);

            if ( distance < maxDist && ( distance < 0.0 || distance > d ))
            {
               distance = d;
               match = f;
            }
         }
      }*/
   }
}
