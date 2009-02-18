using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// spatial and central moments
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvMoments
   {
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m00;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m10;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m01;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m20;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m11;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m02;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m30;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m21;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m12;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double m03;

      /// <summary>
      /// central moments
      /// </summary>
      public double mu20;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu11;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu02;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu30;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu21;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu12;
      /// <summary>
      /// central moments
      /// </summary>
      public double mu03;

      /// <summary>
      /// m00 != 0 ? 1/sqrt(m00) : 0
      /// </summary>
      public double inv_sqrt_m00;

      /// <summary>
      /// The Gravity Center of this Moment
      /// </summary>
      public MCvPoint2D64f GravityCenter
      {
         get
         {
            return new MCvPoint2D64f(m10 / m00, m01 / m00);
         }
      }

      /// <summary>
      /// The function GetSpatialMoment retrieves the spatial moment, which in case of image moments is defined as:
      /// Mx_order,y_order=sumx,y(I(x,y)?x_order?y_order)
      /// where I(x,y) is the intensity of the pixel (x, y). 
      /// </summary>
      /// <param name="x_order">x order of the retrieved moment, x_order &gt;= 0</param>
      /// <param name="y_order">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>the spatial moment</returns>
      public double GetSpatialMoment(int x_order, int y_order)
      {
         return CvInvoke.cvGetSpatialMoment(ref this, x_order, y_order);
      }

      /// <summary>
      /// The function cvGetCentralMoment retrieves the central moment, which in case of image moments is defined as:
      /// ?x_order,y_order=sumx,y(I(x,y)?x-xc)x_order?y-yc)y_order),
      /// where xc=M10/M00, yc=M01/M00 - coordinates of the gravity center
      /// </summary>
      /// <param name="x_order">x order of the retrieved moment, x_order &gt;= 0.</param>
      /// <param name="y_order">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      public double GetCentralMoment(int x_order, int y_order)
      {
         return CvInvoke.cvGetCentralMoment(ref this, x_order, y_order);
      }

      /// <summary>
      /// Get the HuMoments 
      /// </summary>
      public MCvHuMoments GetHuMoment()
      {
         MCvHuMoments huMoment = new MCvHuMoments();
         CvInvoke.cvGetHuMoments(ref this, ref huMoment);
         return huMoment;
      }
   }
}
