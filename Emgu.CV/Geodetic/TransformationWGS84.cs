using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Geodetic
{
   /// <summary>
   /// Defines a datum
   /// </summary>
   public class Datum : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr datumCreateWGS84();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr datumCreate(double a, double b);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void datumRelease(ref IntPtr datum);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformGeodetic2ECEF(IntPtr datum, ref GeodeticCoordinate coordinate, ref MCvPoint3D64f ecef);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformECEF2Geodetic(IntPtr datum, ref MCvPoint3D64f ecef, ref GeodeticCoordinate coordinate);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformGeodetic2ENU(IntPtr datum, ref GeodeticCoordinate coor, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref MCvPoint3D64f enu);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformENU2Geodetic(IntPtr datum, ref MCvPoint3D64f enu, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref GeodeticCoordinate coor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformGeodetic2NED(IntPtr datum, ref GeodeticCoordinate coor, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref MCvPoint3D64f ned);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void transformNED2Geodetic(IntPtr datum, ref MCvPoint3D64f ned, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref GeodeticCoordinate coor);
      #endregion

      /// <summary>
      /// The WGS84 datum
      /// </summary>
      public static readonly Datum WGS84 = new Datum();

      /// <summary>
      /// Creates a WGS 84 datum
      /// </summary>
      public Datum()
      {
         _ptr = datumCreateWGS84();
      }

      /// <summary>
      /// Create a datum with the specific radius
      /// </summary>
      /// <param name="a">Value of the major (transverse) radius (in meter)</param>
      /// <param name="b">Value of the minor (conjugate) radius (in meter)</param>
      public Datum(double a, double b)
      {
         _ptr = datumCreate(a, b);
      }

      /// <summary>
      /// Convert geodetic coordinate to ECEF coordinate
      /// </summary>
      /// <param name="coordinate">the geodetic coordinate</param>
      /// <returns>The ECEF coordinate</returns>
      public MCvPoint3D64f Geodetic2ECEF(GeodeticCoordinate coordinate)
      {
         MCvPoint3D64f res = new MCvPoint3D64f();
         transformGeodetic2ECEF(_ptr, ref coordinate, ref res);
         return res;
      }

      /// <summary>
      /// Convert ECEF coordinate to geodetic coordinate
      /// </summary>
      /// <param name="ecef">The ecef coordinate</param>
      /// <returns>The geodetic coordinate</returns>
      public GeodeticCoordinate ECEF2Geodetic(MCvPoint3D64f ecef)
      {
         GeodeticCoordinate res = new GeodeticCoordinate();
         transformECEF2Geodetic(_ptr, ref ecef, ref res);
         return res;
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to ENU (East North UP) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The ENU (East North UP) coordinate related to the reference coordinate</returns>
      public MCvPoint3D64f Geodetic2ENU(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
      {
         return Geodetic2ENU(coor, refCoor, Geodetic2ECEF(refCoor));
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to ENU (East North UP) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <param name="refEcef"><paramref name="refCoor"/> in ECEF format. If this is provided, it speeds up the computation</param>
      /// <returns>The ENU (East North UP) coordinate related to the reference coordinate</returns>
      public MCvPoint3D64f Geodetic2ENU(GeodeticCoordinate coor, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
         MCvPoint3D64f p = new MCvPoint3D64f();
         transformGeodetic2ENU(_ptr, ref coor, ref refCoor, ref refEcef, ref p);
         return p;
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to NED (North East Down) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The NED (North East Down) coordinate related to the reference coordinate</returns>
      public MCvPoint3D64f Geodetic2NED(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
      {
         return Geodetic2NED(coor, refCoor, Geodetic2ECEF(refCoor));
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to NED (North East Down) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <param name="refEcef"><paramref name="refCoor"/> in ECEF format. If this is provided, it speeds up the computation</param>
      /// <returns>The NED (North East Down) coordinate related to the reference coordinate</returns>
      public MCvPoint3D64f Geodetic2NED(GeodeticCoordinate coor, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
         MCvPoint3D64f ned = new MCvPoint3D64f();
         transformGeodetic2NED(_ptr, ref coor, ref refCoor, ref refEcef, ref ned);
         return ned;
      }

      /// <summary>
      /// Convert <paramref name="enu"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="enu">The ENU (East North UP) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public GeodeticCoordinate ENU2Geodetic(MCvPoint3D64f enu, GeodeticCoordinate refCoor)
      {
         return ENU2Geodetic(enu, refCoor, Geodetic2ECEF(refCoor));
      }

      /// <summary>
      /// Convert <paramref name="enu"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="enu">The ENU (East North UP) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <param name="refEcef"><paramref name="refCoor"/> in ECEF format. If this is provided, it speeds up the computation</param>
      /// <returns>The Geodetic coordinate</returns>
      public GeodeticCoordinate ENU2Geodetic(MCvPoint3D64f enu, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
         GeodeticCoordinate coor = new GeodeticCoordinate();
         transformENU2Geodetic(_ptr, ref enu, ref refCoor, ref refEcef, ref coor);
         return coor;
      }

      /// <summary>
      /// Convert <paramref name="ned"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="ned">The NED (North East Down) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <param name="refEcef"><paramref name="refCoor"/> in ECEF format. If this is provided, it speeds up the computation</param>
      /// <returns>The Geodetic coordinate</returns>
      public GeodeticCoordinate NED2Geodetic(MCvPoint3D64f ned, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
         GeodeticCoordinate coor = new GeodeticCoordinate();
         transformNED2Geodetic(_ptr, ref ned, ref refCoor, ref refEcef, ref coor);
         return coor;
      }

      /// <summary>
      /// Convert <paramref name="ned"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="ned">The NED (North East Down) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public GeodeticCoordinate NED2Geodetic(MCvPoint3D64f ned, GeodeticCoordinate refCoor)
      {
         return NED2Geodetic(ned, refCoor, Geodetic2ECEF(refCoor));
      }

      /// <summary>
      /// Release the unmanaged memory associated with this datum
      /// </summary>
      protected override void DisposeObject()
      {
         datumRelease(ref _ptr);
      }
   }
}
