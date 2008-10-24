using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   public class SURFFeature
   {
      private MCvSURFPoint _point;

      public MCvSURFPoint Point
      {
         get { return _point; }
         set { _point = value; }
      }

      private Matrix<float> _descriptor;

      public Matrix<float> Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      public SURFFeature(ref MCvSURFPoint point, ref MCvSURFDescriptor descriptor)
      {
         _point = point;
         _descriptor = new Matrix<float>(descriptor.values);
      }

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
      }
   }

   public class SURFFeatureExtended
   {
      private MCvSURFPoint _point;

      public MCvSURFPoint Point
      {
         get { return _point; }
         set { _point = value; }
      }

      private Matrix<float> _descriptor;

      public Matrix<float> Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      public SURFFeatureExtended(ref MCvSURFPoint point, ref MCvSURFDescriptorExtended descriptor)
      {
         _point = point;
         _descriptor = new Matrix<float>(descriptor.values);
      }
   }
}
