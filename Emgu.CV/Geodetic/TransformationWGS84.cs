#define PINVOKE

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.Geodetic
{
   /// <summary>
   /// Defines WGS84 transformation
   /// </summary>
   public static class TransformationWGS84
   {
#if PINVOKE
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformGeodetic2ECEF(ref GeodeticCoordinate coordinate, ref MCvPoint3D64f ecef);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformECEF2Geodetic(ref MCvPoint3D64f ecef, ref GeodeticCoordinate coordinate);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformGeodetic2ENU(ref GeodeticCoordinate coor, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref MCvPoint3D64f enu);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformENU2Geodetic(ref MCvPoint3D64f enu, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref GeodeticCoordinate coor);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformGeodetic2NED(ref GeodeticCoordinate coor, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref MCvPoint3D64f ned);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void transformNED2Geodetic(ref MCvPoint3D64f ned, ref GeodeticCoordinate refCoor, ref MCvPoint3D64f refEcef, ref GeodeticCoordinate coor);
#else
      /// <summary>
      /// Value of a from WGS84
      /// </summary>
      public const double A = 6378137.0;
      /// <summary>
      /// Value of f from WGS84
      /// </summary>
      public const double F = 1.0 / 298.257223563;
      /// <summary>
      /// Value of b from WGS84
      /// </summary>
      public const double B = 6356752.31424518;
      /// <summary>
      /// Value of e from WGS84
      /// </summary>
      public static readonly double E;
      /// <summary>
      /// Value of e' from WGS84
      /// </summary>
      public static readonly double EP;

      static TransformationWGS84()
      {
         E = Math.Sqrt((A * A - B * B) / (A * A));
         EP = Math.Sqrt((A * A - B * B) / (B * B));
      }
#endif

      /// <summary>
      /// Convert geodetic coordinate to ECEF coordinate
      /// </summary>
      /// <param name="coordinate">the geodetic coordinate</param>
      /// <returns>The ECEF coordinate</returns>
      public static MCvPoint3D64f Geodetic2ECEF(GeodeticCoordinate coordinate)
      {
#if PINVOKE
         MCvPoint3D64f res = new MCvPoint3D64f();
         transformGeodetic2ECEF(ref coordinate, ref res);
         return res;
#else
         double sinPhi = Math.Sin(coordinate.Latitude);

         double N = A / Math.Sqrt(1.0 - E * E * sinPhi * sinPhi);

         double tmp1 = (N + coordinate.Altitude) * Math.Cos(coordinate.Latitude);

         return new MCvPoint3D64f(
            tmp1 * Math.Cos(coordinate.Longitude), 
            tmp1 * Math.Sin(coordinate.Longitude),
            ((B * B) / (A * A) * N + coordinate.Altitude) * sinPhi);
#endif
      }

      /// <summary>
      /// Convert ECEF coordinate to geodetic coordinate
      /// </summary>
      /// <param name="ecef">The ecef coordinate</param>
      /// <returns>The geodetic coordinate</returns>
      public static GeodeticCoordinate ECEF2Geodetic(MCvPoint3D64f ecef)
      {
#if PINVOKE
         GeodeticCoordinate res = new GeodeticCoordinate();
         transformECEF2Geodetic(ref ecef, ref res);
         return res;
#else
         GeodeticCoordinate coor = new GeodeticCoordinate();
         double p = Math.Sqrt(ecef.x * ecef.x + ecef.y * ecef.y);
         double theta = Math.Atan2(ecef.z * A, p * B);
         double sinTheta = Math.Sin(theta);
         double cosTheta = Math.Cos(theta);

         coor.Longitude = Math.Atan2(ecef.y, ecef.x);
         coor.Latitude = Math.Atan2(
            ecef.z + EP * EP * B * sinTheta * sinTheta * sinTheta,
            p - E * E * A * cosTheta * cosTheta * cosTheta);
         double sinLat = Math.Sin(coor.Latitude);
         double N = A / Math.Sqrt(1.0 - E * E * sinLat * sinLat);
         double cosLat = Math.Cos(coor.Latitude);
         coor.Altitude = p / cosLat - N;
         
         return coor;
#endif
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to ENU (East North UP) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The ENU (East North UP) coordinate related to the reference coordinate</returns>
      public static MCvPoint3D64f Geodetic2ENU(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
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
      public static MCvPoint3D64f Geodetic2ENU(GeodeticCoordinate coor, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
#if PINVOKE
         MCvPoint3D64f p = new MCvPoint3D64f();
         transformGeodetic2ENU(ref coor, ref refCoor, ref refEcef, ref p);
         return p;
#else
         MCvPoint3D64f delta = Geodetic2ECEF(coor) - refEcef;
         double sinPhi = Math.Sin(refCoor.Latitude);
         double cosPhi = Math.Cos(refCoor.Latitude);
         double sinLambda = Math.Sin(refCoor.Longitude);
         double cosLambda = Math.Cos(refCoor.Longitude);

         double cosLambda_DeltaX = cosLambda * delta.x;
         double sinLambda_DeltaY = sinLambda * delta.y;

         return new MCvPoint3D64f(
            -sinLambda * delta.x + cosLambda * delta.y,
            -sinPhi * cosLambda_DeltaX - sinPhi * sinLambda_DeltaY + cosPhi * delta.z,
            cosPhi * cosLambda_DeltaX + cosPhi * sinLambda_DeltaY + sinPhi * delta.z);
#endif
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to NED (North East Down) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The NED (North East Down) coordinate related to the reference coordinate</returns>
      public static MCvPoint3D64f Geodetic2NED(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
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
      public static MCvPoint3D64f Geodetic2NED(GeodeticCoordinate coor, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
#if PINVOKE
         MCvPoint3D64f ned = new MCvPoint3D64f();
         transformGeodetic2NED(ref coor, ref refCoor, ref refEcef, ref ned);
         return ned;
#else
         MCvPoint3D64f enu = Geodetic2ENU(coor, refCoor, refEcef);
         return new MCvPoint3D64f(enu.y, enu.x, -enu.z);
#endif
      }

      /// <summary>
      /// Convert <paramref name="enu"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="enu">The ENU (East North UP) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public static GeodeticCoordinate ENU2Geodetic(MCvPoint3D64f enu, GeodeticCoordinate refCoor)
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
      public static GeodeticCoordinate ENU2Geodetic(MCvPoint3D64f enu, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
#if PINVOKE
         GeodeticCoordinate coor = new GeodeticCoordinate();
         transformENU2Geodetic(ref enu, ref refCoor, ref refEcef, ref coor);
         return coor;
#else
         double sinPhi = Math.Sin(refCoor.Latitude);
         double cosPhi = Math.Cos(refCoor.Latitude);
         double sinLambda = Math.Sin(refCoor.Longitude);
         double cosLambda = Math.Cos(refCoor.Longitude);

         double sinPhi_EnuY = sinPhi * enu.y;
         double cosPhi_EnuZ = cosPhi * enu.z;
         MCvPoint3D64f ecefDelta = new MCvPoint3D64f(
            -sinLambda * enu.x - cosLambda * sinPhi_EnuY + cosLambda * cosPhi_EnuZ,
            cosLambda * enu.x - sinLambda * sinPhi_EnuY + sinLambda * cosPhi_EnuZ,
            cosPhi * enu.y + sinPhi * enu.z);

         return ECEF2Geodetic(ecefDelta + refEcef);
#endif
      }

      /// <summary>
      /// Convert <paramref name="ned"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="ned">The NED (North East Down) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <param name="refEcef"><paramref name="refCoor"/> in ECEF format. If this is provided, it speeds up the computation</param>
      /// <returns>The Geodetic coordinate</returns>
      public static GeodeticCoordinate NED2Geodetic(MCvPoint3D64f ned, GeodeticCoordinate refCoor, MCvPoint3D64f refEcef)
      {
#if PINVOKE
         GeodeticCoordinate coor = new GeodeticCoordinate();
         transformNED2Geodetic(ref ned, ref refCoor, ref refEcef, ref coor);
         return coor;
#else
         MCvPoint3D64f enu = new MCvPoint3D64f(ned.y, ned.x, -ned.z);
         return ENU2Geodetic(enu, refCoor, refEcef);
#endif
      }

      /// <summary>
      /// Convert <paramref name="ned"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="ned">The NED (North East Down) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public static GeodeticCoordinate NED2Geodetic(MCvPoint3D64f ned, GeodeticCoordinate refCoor)
      {
         return NED2Geodetic(ned, refCoor, Geodetic2ECEF(refCoor));
      }
   }
}
