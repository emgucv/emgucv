//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Geodetic
{
   /// <summary>
   /// A geodetic coordinate that is defined by its latitude, longitude and altitude
   /// </summary>
   public struct GeodeticCoordinate : IEquatable<GeodeticCoordinate>
   {
      private double _latitude;
      private double _longitude;
      private double _altitude;

      /// <summary>
      /// Indicates the origin of the Geodetic Coordinate
      /// </summary>
      public static readonly GeodeticCoordinate Empty = new GeodeticCoordinate();

      /// <summary>
      /// Create a geodetic coordinate using the specific values
      /// </summary>
      /// <param name="latitude">Latitude in radian</param>
      /// <param name="longitude">Longitude in radian</param>
      /// <param name="altitude">Altitude in meters</param>
      public GeodeticCoordinate(double latitude, double longitude, double altitude)
      {
         _latitude = latitude;
         _longitude = longitude;
         _altitude = altitude;
      }

      /// <summary>
      /// Latitude (phi) in radian
      /// </summary>
      public double Latitude
      {
         get
         {
            return _latitude;
         }
         set
         {
            _latitude = value;
         }
      }

      /// <summary>
      /// Longitude (lambda) in radian
      /// </summary>
      public double Longitude
      {
         get
         {
            return _longitude;
         }
         set
         {
            _longitude = value;
         }
      }

      /// <summary>
      /// Altitude (height) in meters
      /// </summary>
      public double Altitude
      {
         get
         {
            return _altitude;
         }
         set
         {
            _altitude = value;
         }
      }

      /// <summary>
      /// Compute the sum of two GeodeticCoordinates
      /// </summary>
      /// <param name="coor1">The first coordinate to be added</param>
      /// <param name="coor2">The second coordinate to be added</param>
      /// <returns>The sum of two GeodeticCoordinates</returns>
      public static GeodeticCoordinate operator +(GeodeticCoordinate coor1, GeodeticCoordinate coor2)
      {
         return new GeodeticCoordinate(
            coor1.Latitude + coor2.Latitude,
            coor1.Longitude + coor2.Longitude,
            coor1.Altitude + coor2.Altitude);
      }

      /// <summary>
      /// Compute <paramref name="coor1"/> - <paramref name="coor2"/>
      /// </summary>
      /// <param name="coor1">The first coordinate</param>
      /// <param name="coor2">The coordinate to be subtracted</param>
      /// <returns><paramref name="coor1"/> - <paramref name="coor2"/></returns>
      public static GeodeticCoordinate operator -(GeodeticCoordinate coor1, GeodeticCoordinate coor2)
      {
         return new GeodeticCoordinate(
            coor1.Latitude - coor2.Latitude,
            coor1.Longitude - coor2.Longitude,
            coor1.Altitude - coor2.Altitude);
      }

      /// <summary>
      /// Compute <paramref name="coor"/> * <paramref name="scale"/>
      /// </summary>
      /// <param name="coor">The coordinate</param>
      /// <param name="scale">The scale to be multiplied</param>
      /// <returns><paramref name="coor"/> * <paramref name="scale"/></returns>
      public static GeodeticCoordinate operator *(GeodeticCoordinate coor, double scale)
      {
         return new GeodeticCoordinate(
            coor.Latitude * scale,
            coor.Longitude * scale,
            coor.Altitude * scale);
      }

      #region IEquatable<GeodeticCoordinate> Members
      /// <summary>
      /// Check if this Geodetic coordinate equals <paramref name="other"/>
      /// </summary>
      /// <param name="other">The other coordinate to be compared with</param>
      /// <returns>True if two coordinates equals</returns>
      public bool Equals(GeodeticCoordinate other)
      {
         return
            Latitude.Equals(other.Latitude)
            && Longitude.Equals(other.Longitude)
            && Altitude.Equals(other.Altitude);
      }
      #endregion

      /// <summary>
      /// Convert radian to degree
      /// </summary>
      /// <param name="radian">radian</param>
      /// <returns>degree</returns>
      public static double RadianToDegree(double radian)
      {
         return radian * (180.0 / Math.PI);
      }

      /// <summary>
      /// Convert degree to radian
      /// </summary>
      /// <param name="degree">degree</param>
      /// <returns>radian</returns>
      public static double DegreeToRadian(double degree)
      {
         return degree * (Math.PI / 180.0);
      }

   }
}
