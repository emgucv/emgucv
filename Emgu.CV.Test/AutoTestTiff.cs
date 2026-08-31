//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Tiff;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif

namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestTiff
    {
        [Test]
        public void TestGeoTiffWriter()
        {
            String fileName = Path.Combine(Path.GetTempPath(), "test_geotiff.tif");
            try
            {
                using (Mat image = new Mat(new Size(64, 64), DepthType.Cv8U, 3))
                {
                    image.SetTo(new MCvScalar(128, 64, 32));
                    using (TiffWriter writer = new TiffWriter(fileName))
                    {
                        // Geo tags must be written before the image data is flushed.
                        // ModelTiepoint: [i, j, k, x, y, z] — pixel (0,0) maps to lon/lat (10.0, 50.0)
                        double[] tiepoint = new double[] { 0, 0, 0, 10.0, 50.0, 0.0 };
                        // ModelPixelScale: [scaleX, scaleY, scaleZ] — 0.001 degrees/pixel
                        double[] pixelScale = new double[] { 0.001, 0.001, 0.0 };
                        writer.WriteGeoTag(tiepoint, pixelScale);
                        writer.WriteImage(image);
                    }
                }
                EmguAssert.IsTrue(File.Exists(fileName), "GeoTIFF file was not created");
                EmguAssert.IsTrue(new FileInfo(fileName).Length > 0, "GeoTIFF file is empty");
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        [Test]
        public void TestTileTiffWriter()
        {
            String fileName = Path.Combine(Path.GetTempPath(), "test_tile.tif");
            try
            {
                Size imageSize = new Size(64, 64);
                Size tileSize = new Size(32, 32);
                using (Mat image = new Mat(imageSize, DepthType.Cv8U, 3))
                {
                    image.SetTo(new MCvScalar(100, 150, 200));
                    using (TileTiffWriter writer = new TileTiffWriter(fileName, imageSize, tileSize))
                    {
                        writer.WriteImage(image);
                    }
                }
                EmguAssert.IsTrue(File.Exists(fileName), "Tiled TIFF file was not created");
                EmguAssert.IsTrue(new FileInfo(fileName).Length > 0, "Tiled TIFF file is empty");
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }
    }
}
