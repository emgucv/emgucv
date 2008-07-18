using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
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
        public double m00, m10, m01, m20, m11, m02, m30, m21, m12, m03;

        /// <summary>
        /// central moments
        /// </summary>
        public double mu20, mu11, mu02, mu30, mu21, mu12, mu03;

        /// <summary>
        /// m00 != 0 ? 1/sqrt(m00) : 0
        /// </summary>
        public double inv_sqrt_m00;

        /// <summary>
        /// The Gravity Center of this Moment
        /// </summary>
        public Point2D<double> GravityCenter
        {
            get
            {
                return new Point2D<double>(m10 / m00, m01 / m00);
            }
        }

        /// <summary>
        /// The function GetSpatialMoment retrieves the spatial moment, which in case of image moments is defined as:
        /// Mx_order,y_order=sumx,y(I(x,y)•xx_order•yy_order)
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
        public MCvHuMoments GetCvHuMoment()
        {
            MCvHuMoments huMoment = new MCvHuMoments();
            CvInvoke.cvGetHuMoments(ref this, ref huMoment);
            return huMoment;            
        }
    }
}
