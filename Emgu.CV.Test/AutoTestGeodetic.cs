using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.Geodetic;
using Emgu.CV.Structure;
using Emgu.CV.Tiff;
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
         MCvPoint3D64f ecef = Datum.WGS84.Geodetic2ECEF(coor);
         GeodeticCoordinate coorTransformed = Datum.WGS84.ECEF2Geodetic(ecef);

         GeodeticCoordinate refCoor = new GeodeticCoordinate(43.853374 * Math.PI / 180.0, -79.358321 * Math.PI / 180.0, 230.312);

         GeodeticCoordinate delta = coorTransformed - coor;
         Assert.Less(Math.Abs(delta.Altitude), _epsilon);
         Assert.Less(Math.Abs(delta.Latitude), _epsilon);
         Assert.Less(Math.Abs(delta.Longitude), _epsilon);

         MCvPoint3D64f ned = Datum.WGS84.Geodetic2NED(coor, refCoor);
         coorTransformed = Datum.WGS84.NED2Geodetic(ned, refCoor);
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
            MCvPoint3D64f ned = Datum.WGS84.Geodetic2NED(coor, refCoor);
            GeodeticCoordinate coorTransformed = Datum.WGS84.NED2Geodetic(ned, refCoor);
            tmp = coorTransformed.Altitude;
         }
         watch.Stop();
         Trace.WriteLine(watch.ElapsedMilliseconds);

      }

      public void TestPerformance2()
      {
         GeodeticCoordinate coor = new GeodeticCoordinate(
            GeodeticCoordinate.DegreeToRadian(43.853626),
            GeodeticCoordinate.DegreeToRadian(-79.358981),
            231.0);

         Stopwatch watch = Stopwatch.StartNew();
         double tmp;
         for (int i = 0; i < 10000000; i++)
         {
            MCvPoint3D64f ecef = Datum.WGS84.Geodetic2ECEF(coor);
            GeodeticCoordinate coorTransformed = Datum.WGS84.ECEF2Geodetic(ecef);
            tmp = coorTransformed.Altitude;
         }
         watch.Stop();
         Trace.WriteLine(watch.ElapsedMilliseconds);
      }

      [Test]
      public void TestGeotiff()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(1000, 1000);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         using (TiffWriter<Gray, Byte> writer = new TiffWriter<Gray, Byte>("temp.tif"))
         {
            writer.WriteImage(image);
            writer.WriteGeoTag(new GeodeticCoordinate(
               GeodeticCoordinate.DegreeToRadian(43.853626),
               GeodeticCoordinate.DegreeToRadian(-79.358981),
               231.0),
               image.Size,
               new MCvPoint2D64f(0.05, 0.05));
         }
      }

      [Test]
      public void TestGeotiff2()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(1000, 1000);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         using (TiffWriter<Gray,Byte> writer = new TiffWriter<Gray, Byte>("temp.tif"))
         {
            writer.WriteGeoTag(new GeodeticCoordinate(
               GeodeticCoordinate.DegreeToRadian(43.853626),
               GeodeticCoordinate.DegreeToRadian(-79.358981),
               231.0),
               image.Size,
               new MCvPoint2D64f(0.05, 0.05));
         }
      }
   }
}
