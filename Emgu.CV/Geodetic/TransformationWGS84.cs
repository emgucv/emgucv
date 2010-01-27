using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Geodetic
{
   /// <summary>
   /// Defines WGS84 transformation
   /// </summary>
   public static class TransformationWGS84
   {
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

      /// <summary>
      /// Convert geodetic coordinate to ECEF coordinate
      /// </summary>
      /// <param name="coordinate">the geodetic coordinate</param>
      /// <returns>The ECEF coordinate</returns>
      public static MCvPoint3D64f Geodetic2ECEF(GeodeticCoordinate coordinate)
      {
         double sinPhi = Math.Sin(coordinate.Latitude);

         double N = A / Math.Sqrt(1.0 - E * E * sinPhi * sinPhi);

         double tmp1 = (N + coordinate.Altitude) * Math.Cos(coordinate.Latitude);

         return new MCvPoint3D64f(
            tmp1 * Math.Cos(coordinate.Longitude), 
            tmp1 * Math.Sin(coordinate.Longitude),
            ((B * B) / (A * A) * N + coordinate.Altitude) * sinPhi);
      }

      /// <summary>
      /// Convert ECEF coordinate to geodetic coordinate
      /// </summary>
      /// <param name="ecef">The ecef coordinate</param>
      /// <returns>The geodetic coordinate</returns>
      public static GeodeticCoordinate Geodetic2LLA(MCvPoint3D64f ecef)
      {
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
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to ENU (East North UP) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The ENU (East North UP) coordinate related to the reference coordinate</returns>
      public static MCvPoint3D64f Geodetic2ENU(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
      {
         MCvPoint3D64f delta = Geodetic2ECEF(coor) - Geodetic2ECEF(refCoor);
         double sinPhi = Math.Sin(refCoor.Latitude);
         double cosPhi = Math.Cos(refCoor.Latitude);
         double sinLambda = Math.Sin(refCoor.Longitude);
         double cosLambda = Math.Cos(refCoor.Longitude);

         return new MCvPoint3D64f(
            -sinLambda * delta.x + cosLambda * delta.y,
            -sinPhi * cosLambda * delta.x - sinPhi * sinLambda * delta.y + cosPhi * delta.z,
            cosPhi * cosLambda * delta.x + cosPhi * sinLambda * delta.y + sinPhi * delta.z);
      }

      /// <summary>
      /// Convert <paramref name="coor"/> to NED (North East Down) coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="coor">The Geodetic Coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The NED (North East Down) coordinate related to the reference coordinate</returns>
      public static MCvPoint3D64f Geodetic2NED(GeodeticCoordinate coor, GeodeticCoordinate refCoor)
      {
         MCvPoint3D64f enu = Geodetic2ENU(coor, refCoor);
         return new MCvPoint3D64f(enu.y, enu.x, -enu.z);
      }

      /// <summary>
      /// Convert <paramref name="enu"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="enu">The ENU (East North UP) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public static GeodeticCoordinate ENU2Geodetic(MCvPoint3D64f enu, GeodeticCoordinate refCoor)
      {
         double sinPhi = Math.Sin(refCoor.Latitude);
         double cosPhi = Math.Cos(refCoor.Latitude);
         double sinLambda = Math.Sin(refCoor.Longitude);
         double cosLambda = Math.Cos(refCoor.Longitude);

         MCvPoint3D64f ecefDelta = new MCvPoint3D64f(
            -sinLambda * enu.x - sinPhi * cosLambda * enu.y + cosPhi * cosLambda * enu.z,
            cosLambda * enu.x - sinPhi * sinLambda * enu.y + cosPhi * sinLambda * enu.z,
            cosPhi * enu.y + sinPhi * enu.z);

         return Geodetic2LLA(ecefDelta + Geodetic2ECEF(refCoor));
      }

      /// <summary>
      /// Convert <paramref name="ned"/> to Geodetic coordinate using the reference coordinate <paramref name="refCoor"/>
      /// </summary>
      /// <param name="ned">The NED (North East Down) coordinate to be converted</param>
      /// <param name="refCoor">The reference Geodetic coordinate</param>
      /// <returns>The Geodetic coordinate</returns>
      public static GeodeticCoordinate NED2Geodetic(MCvPoint3D64f ned, GeodeticCoordinate refCoor)
      {
         MCvPoint3D64f enu = new MCvPoint3D64f(ned.y, ned.x, -ned.z);
         return ENU2Geodetic(enu, refCoor);
      }
   }
}
