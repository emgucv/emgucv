using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// A wrapper for the CvMoment structure
    /// </summary>
    public class Moment : UnmanagedObject
    {
        /// <summary>
        /// A pointer to the CvMoment structure
        /// </summary>
        public new IntPtr Ptr { get { return _ptr; } set { _ptr = value; } }

        /// <summary>
        /// Create a CvMoment structure
        /// </summary>
        public Moment()
        {
            _ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvMoments)));
        }

        /// <summary>
        /// returns a managed CvMoment structure
        /// </summary>
        public MCvMoments CvMoment
        {
            get { return (MCvMoments)Marshal.PtrToStructure(_ptr, typeof(MCvMoments)); }
        }

        /// <summary>
        /// Hu moments structure
        /// </summary>
        public MCvHuMoments CvHuMoment
        {
            get 
            {
                IntPtr stor = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvHuMoments)));
                CvInvoke.cvGetHuMoments(Ptr, stor);
                MCvHuMoments res = (MCvHuMoments)Marshal.PtrToStructure(stor, typeof(MCvHuMoments));
                Marshal.FreeHGlobal(stor);
                return res;
            }
        }

        /// <summary>
        /// The Gravity Center of this Moment
        /// </summary>
        public Point2D<double> GravityCenter
        {
            get
            {
                MCvMoments moment = CvMoment;
                return new Point2D<double>(moment.m10 / moment.m00, moment.m01 / moment.m00);
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
            return CvInvoke.cvGetSpatialMoment(Ptr, x_order, y_order);
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
            return CvInvoke.cvGetCentralMoment(Ptr, x_order, y_order);
        }

        /// <summary>
        /// Release the CvMoment strucutre and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            Marshal.FreeHGlobal(_ptr);
        }
    }
}
