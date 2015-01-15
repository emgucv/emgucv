//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Util;

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
      public double M00;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M10;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M01;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M20;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M11;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M02;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M30;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M21;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M12;
      /// <summary>
      /// spatial moments
      /// </summary>
      public double M03;

      /// <summary>
      /// central moments
      /// </summary>
      public double Mu20;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu11;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu02;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu30;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu21;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu12;
      /// <summary>
      /// central moments
      /// </summary>
      public double Mu03;

      /// <summary>
      /// m00 != 0 ? 1/sqrt(m00) : 0
      /// </summary>
      public double InvSqrtM00;

      /// <summary>
      /// The Gravity Center of this Moment
      /// </summary>
      public MCvPoint2D64f GravityCenter
      {
         get
         {
            return new MCvPoint2D64f(M10 / M00, M01 / M00);
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
      /// <param name="xOrder">x order of the retrieved moment, x_order &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      public double GetCentralMoment(int xOrder, int yOrder)
      {
         return CvInvoke.cvGetCentralMoment(ref this, xOrder, yOrder);
      }

      /// <summary>
      /// Retrieves normalized central moment, which in case of image moments is defined as:
      /// eta_{x_order,y_order}=mu_{x_order,y_order} / M00^{(y_order+x_order)/2+1},
      /// where mu_{x_order,y_order} is the central moment
      /// </summary>
      /// <param name="xOrder">x order of the retrieved moment, x_order &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, y_order &gt;= 0 and x_order + y_order &lt;= 3</param>
      /// <returns>The normalized center moment</returns>
      public double GetNormalizedCentralMoment(int xOrder, int yOrder)
      {
         return CvInvoke.cvGetNormalizedCentralMoment(ref this, xOrder, yOrder);
      }

      /// <summary>
      /// Get the HuMoments 
      /// </summary>
      /// <returns>The Hu moment computed from this moment</returns>
      public double[] GetHuMoment()
      {
         using (VectorOfDouble vd = new VectorOfDouble())
         {
            CvInvoke.HuMoments(this, vd);
            return vd.ToArray();
         }
        
      }
   }
}
