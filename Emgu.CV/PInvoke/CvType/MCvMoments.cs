//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
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
      /// Retrieves the spatial moment, which in case of image moments is defined as:
      /// M_{x_order,y_order}=sum_{x,y}(I(x,y) * x^{x_order} * y^{y_order})
      /// where I(x,y) is the intensity of the pixel (x, y). 
      /// </summary>
      /// <param name="xOrder">x order of the retrieved moment, x_order &gt;= 0</param>
      /// <param name="yOrder">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The spatial moment of the specific order</returns>
      public double GetSpatialMoment(int xOrder, int yOrder)
      {
         return CvInvoke.cvGetSpatialMoment(ref this, xOrder, yOrder);
      }

      /// <summary>
      /// Retrieves the central moment, which in case of image moments is defined as:
      /// mu_{x_order,y_order}=sum_{x,y}(I(x,y)*(x-x_c)^{x_order} * (y-y_c)^{y_order}),
      /// where x_c=M10/M00, y_c=M01/M00 - coordinates of the gravity center
      /// </summary>
      /// <param name="x_order">x order of the retrieved moment, x_order &gt;= 0.</param>
      /// <param name="y_order">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      public double GetCentralMoment(int x_order, int y_order)
      {
         return CvInvoke.cvGetCentralMoment(ref this, x_order, y_order);
      }

      /// <summary>
      /// Retrieves normalized central moment, which in case of image moments is defined as:
      /// eta_{x_order,y_order}=mu_{x_order,y_order} / M00^{(y_order+x_order)/2+1},
      /// where mu_{x_order,y_order} is the central moment
      /// </summary>
      /// <param name="x_order">x order of the retrieved moment, x_order &gt;= 0.</param>
      /// <param name="y_order">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The normalized center moment</returns>
      public double GetNormalizedCentralMoment(int x_order, int y_order)
      {
         return CvInvoke.cvGetNormalizedCentralMoment(ref this, x_order, y_order);
      }

      /// <summary>
      /// Get the HuMoments 
      /// </summary>
      /// <returns>The Hu moment computed from this moment</returns>
      public MCvHuMoments GetHuMoment()
      {
         MCvHuMoments huMoment = new MCvHuMoments();
         CvInvoke.cvGetHuMoments(ref this, ref huMoment);
         return huMoment;
      }
   }
}
