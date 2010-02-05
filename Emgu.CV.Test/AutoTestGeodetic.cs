using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.Geodetic;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestGeodetic
   {
      public static readonly double _epsilon = 1.0e-8;

      [Test]
      public void TestConvertion()
      {
         GeodeticCoordinate coor = new GeodeticCoordinate(43.853626 * Math.PI / 180.0, -79.358981 * Math.PI / 180.0, 231.0);
         MCvPoint3D64f ecef = TransformationWGS84.Geodetic2ECEF(coor);
         GeodeticCoordinate coorTransformed = TransformationWGS84.ECEF2Geodetic(ecef);

         GeodeticCoordinate refCoor = new GeodeticCoordinate(43.853374 * Math.PI / 180.0, -79.358321 * Math.PI / 180.0, 230.312);

         GeodeticCoordinate delta = coorTransformed - coor;
         Assert.Less(Math.Abs(delta.Altitude), _epsilon);
         Assert.Less(Math.Abs(delta.Latitude), _epsilon);
         Assert.Less(Math.Abs(delta.Longitude), _epsilon);

         MCvPoint3D64f ned = TransformationWGS84.Geodetic2NED(coor, refCoor);
         coorTransformed = TransformationWGS84.NED2Geodetic(ned, refCoor);
         delta = coorTransformed - coor;
         Assert.Less(Math.Abs(delta.Altitude), _epsilon);
         Assert.Less(Math.Abs(delta.Latitude), _epsilon);
         Assert.Less(Math.Abs(delta.Longitude), _epsilon);
      }

      public void TestPerformance1()
      {
         GeodeticCoordinate coor = new GeodeticCoordinate(43.853626 * Math.PI / 180.0, -79.358981 * Math.PI / 180.0, 231.0);
         GeodeticCoordinate refCoor = new GeodeticCoordinate(43.853374 * Math.PI / 180.0, -79.358321 * Math.PI / 180.0, 230.312);

         Stopwatch watch = Stopwatch.StartNew();
         double tmp;
         for (int i = 0; i < 10000000; i++)
         {
            MCvPoint3D64f ned = TransformationWGS84.Geodetic2NED(coor, refCoor);
            GeodeticCoordinate coorTransformed = TransformationWGS84.NED2Geodetic(ned, refCoor);
            tmp = coorTransformed.Altitude;
         }
         watch.Stop();
         Trace.WriteLine(watch.ElapsedMilliseconds);

      }

      public void TestPerformance2()
      {
         GeodeticCoordinate coor = new GeodeticCoordinate(43.853626 * Math.PI / 180.0, -79.358981 * Math.PI / 180.0, 231.0);

         Stopwatch watch = Stopwatch.StartNew();
         double tmp;
         for (int i = 0; i < 10000000; i++)
         {
            MCvPoint3D64f ecef = TransformationWGS84.Geodetic2ECEF(coor);
            GeodeticCoordinate coorTransformed = TransformationWGS84.ECEF2Geodetic(ecef);
            tmp = coorTransformed.Altitude;
         }
         watch.Stop();
         Trace.WriteLine(watch.ElapsedMilliseconds);
      }
   }
}
