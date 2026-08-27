//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.XImgproc;

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
    public class AutoTestXImgproc
    {
        private const int ImgSize = 120;

        // A BGR image split into four differently-colored quadrants, giving edge/line/
        // segmentation algorithms clear boundaries to find without depending on bundled
        // sample images.
        private static Mat CreateColorImage()
        {
            Mat img = new Mat(new Size(ImgSize, ImgSize), DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(0, 0, 0));
            CvInvoke.Rectangle(img, new Rectangle(0, 0, ImgSize / 2, ImgSize / 2), new MCvScalar(0, 0, 255), -1);
            CvInvoke.Rectangle(img, new Rectangle(ImgSize / 2, 0, ImgSize / 2, ImgSize / 2), new MCvScalar(0, 255, 0), -1);
            CvInvoke.Rectangle(img, new Rectangle(0, ImgSize / 2, ImgSize / 2, ImgSize / 2), new MCvScalar(255, 0, 0), -1);
            CvInvoke.Rectangle(img, new Rectangle(ImgSize / 2, ImgSize / 2, ImgSize / 2, ImgSize / 2), new MCvScalar(255, 255, 255), -1);
            return img;
        }

        private static Mat CreateGrayImage()
        {
            using (Mat color = CreateColorImage())
            {
                Mat gray = new Mat();
                CvInvoke.CvtColor(color, gray, ColorConversion.Bgr2Gray);
                return gray;
            }
        }

        [Test]
        public void TestDTFilter()
        {
            using (Mat guide = CreateColorImage())
            using (Mat dst = new Mat())
            using (DTFilter filter = new DTFilter(guide, 10.0, 20.0))
            {
                filter.Filter(guide, dst);
                EmguAssert.IsTrue(!dst.IsEmpty, "DTFilter output should not be empty");
                EmguAssert.AreEqual(guide.Size, dst.Size);
            }
        }

        [Test]
        public void TestEdgeDrawing()
        {
            using (Mat gray = CreateGrayImage())
            using (EdgeDrawing ed = new EdgeDrawing())
            {
                ed.DetectEdges(gray);

                using (Mat edgeImage = new Mat())
                {
                    ed.GetEdgeImage(edgeImage);
                    EmguAssert.IsTrue(!edgeImage.IsEmpty, "EdgeDrawing edge image should not be empty");
                    EmguAssert.IsTrue(CvInvoke.CountNonZero(edgeImage) > 0, "EdgeDrawing should detect at least some edges");
                }

                using (Mat gradientImage = new Mat())
                {
                    ed.GetGradientImage(gradientImage);
                    EmguAssert.IsTrue(!gradientImage.IsEmpty, "EdgeDrawing gradient image should not be empty");
                }

                using (Mat lines = new Mat())
                {
                    ed.DetectLines(lines);
                    EmguAssert.IsTrue(lines.Rows > 0, "EdgeDrawing should detect at least one line");
                }

                using (Mat ellipses = new Mat())
                {
                    // Just exercise the call path; the synthetic rectangles need not contain ellipses.
                    ed.DetectEllipses(ellipses);
                }
            }
        }

        [Test]
        public void TestFastLineDetector()
        {
            using (Mat gray = new Mat(new Size(ImgSize, ImgSize), DepthType.Cv8U, 1))
            {
                gray.SetTo(new MCvScalar(0));
                CvInvoke.Line(gray, new Point(5, 5), new Point(ImgSize - 5, ImgSize - 5), new MCvScalar(255), 2);

                using (FastLineDetector fld = new FastLineDetector())
                {
                    LineSegment2DF[] lines = fld.Detect(gray);
                    EmguAssert.IsTrue(lines.Length > 0, "FastLineDetector should detect at least one line segment");

                    using (Mat display = new Mat(gray.Size, DepthType.Cv8U, 3))
                    {
                        display.SetTo(new MCvScalar(0, 0, 0));
                        fld.DrawSegments(display, lines);
                        EmguAssert.IsTrue(CvInvoke.CountNonZero(gray) > 0, "sanity: source line should be drawn");
                    }
                }
            }
        }

        [Test]
        public void TestGraphSegmentation()
        {
            using (Mat color = CreateColorImage())
            using (Mat dst = new Mat())
            using (GraphSegmentation gs = new GraphSegmentation())
            {
                gs.ProcessImage(color, dst);
                EmguAssert.IsTrue(!dst.IsEmpty, "GraphSegmentation output should not be empty");
                EmguAssert.AreEqual(color.Size, dst.Size);
            }
        }

        [Test]
        public void TestRidgeDetectionFilter()
        {
            using (Mat gray = CreateGrayImage())
            using (Mat output = new Mat())
            using (RidgeDetectionFilter filter = new RidgeDetectionFilter())
            {
                filter.GetRidgeFilteredImage(gray, output);
                EmguAssert.IsTrue(!output.IsEmpty, "RidgeDetectionFilter output should not be empty");
                EmguAssert.AreEqual(gray.Size, output.Size);
            }
        }

        [Test]
        public void TestScanSegment()
        {
            using (Mat color = CreateColorImage())
            using (Mat lab = new Mat())
            {
                CvInvoke.CvtColor(color, lab, ColorConversion.Bgr2Lab);

                using (ScanSegment scanSegment = new ScanSegment(ImgSize, ImgSize, 10))
                {
                    scanSegment.Iterate(lab);

                    using (Mat labels = new Mat())
                    {
                        scanSegment.GetLabels(labels);
                        EmguAssert.IsTrue(!labels.IsEmpty, "ScanSegment labels should not be empty");
                    }

                    using (Mat mask = new Mat())
                    {
                        scanSegment.GetLabelContourMask(mask);
                        EmguAssert.IsTrue(!mask.IsEmpty, "ScanSegment contour mask should not be empty");
                    }
                }
            }
        }

        [Test]
        public void TestSuperpixelLSC()
        {
            using (Mat color = CreateColorImage())
            using (SuperpixelLSC lsc = new SuperpixelLSC(color, 20, 0.075f))
            {
                lsc.Iterate(4);
                lsc.EnforceLabelConnectivity();

                EmguAssert.IsTrue(lsc.NumberOfSuperpixels > 0, "SuperpixelLSC should produce at least one superpixel");

                using (Mat labels = new Mat())
                {
                    lsc.GetLabels(labels);
                    EmguAssert.IsTrue(!labels.IsEmpty, "SuperpixelLSC labels should not be empty");
                }

                using (Mat mask = new Mat())
                {
                    lsc.GetLabelContourMask(mask);
                    EmguAssert.IsTrue(!mask.IsEmpty, "SuperpixelLSC contour mask should not be empty");
                }
            }
        }

        [Test]
        public void TestSuperpixelSEEDS()
        {
            using (Mat color = CreateColorImage())
            using (SupperpixelSEEDS seeds = new SupperpixelSEEDS(ImgSize, ImgSize, 3, 100, 4, 2, 5, false))
            {
                seeds.Iterate(color, 4);

                EmguAssert.IsTrue(seeds.NumberOfSuperpixels > 0, "SuperpixelSEEDS should produce at least one superpixel");

                using (Mat labels = new Mat())
                {
                    seeds.GetLabels(labels);
                    EmguAssert.IsTrue(!labels.IsEmpty, "SuperpixelSEEDS labels should not be empty");
                }

                using (Mat mask = new Mat())
                {
                    seeds.GetLabelContourMask(mask);
                    EmguAssert.IsTrue(!mask.IsEmpty, "SuperpixelSEEDS contour mask should not be empty");
                }
            }
        }

        [Test]
        public void TestSuperpixelSLIC()
        {
            using (Mat color = CreateColorImage())
            using (SupperpixelSLIC slic = new SupperpixelSLIC(color, SupperpixelSLIC.Algorithm.SLIC, 20, 10f))
            {
                slic.Iterate(4);
                slic.EnforceLabelConnectivity();

                EmguAssert.IsTrue(slic.NumberOfSuperpixels > 0, "SuperpixelSLIC should produce at least one superpixel");

                using (Mat labels = new Mat())
                {
                    slic.GetLabels(labels);
                    EmguAssert.IsTrue(!labels.IsEmpty, "SuperpixelSLIC labels should not be empty");
                }

                using (Mat mask = new Mat())
                {
                    slic.GetLabelContourMask(mask);
                    EmguAssert.IsTrue(!mask.IsEmpty, "SuperpixelSLIC contour mask should not be empty");
                }
            }
        }
    }
}
