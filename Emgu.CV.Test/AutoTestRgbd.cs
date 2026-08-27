//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Linemod;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
    public class AutoTestRgbd
    {
        // Image/rectangle size must be divisible by every pyramid level's sampling step T
        // (5 and 8 by default) or Detector.Match throws "response_map.rows % T == 0"; 160
        // is divisible by 5, and by 8 after the level-1 pyrDown halves it to 80.
        private static readonly Rectangle ObjectRegion = new Rectangle(40, 40, 80, 80);

        // A high-contrast filled rectangle on a black background, synthesized so the LINE
        // color-gradient modality has plenty of strong edges to extract template features from.
        private static Mat CreateTemplateImage()
        {
            Mat img = new Mat(new Size(160, 160), DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(0, 0, 0));
            CvInvoke.Rectangle(img, ObjectRegion, new MCvScalar(0, 0, 255), -1);
            return img;
        }

        [Test]
        public void TestLineDetectorAddTemplateAndMatch()
        {
            using (LineDetector detector = new LineDetector())
            {
                EmguAssert.AreEqual(2, detector.PyramidLevels);
                EmguAssert.AreEqual(0, detector.NumTemplates);
                EmguAssert.AreEqual(0, detector.NumClasses);
                EmguAssert.AreEqual(5, detector.GetT(0));
                EmguAssert.AreEqual(8, detector.GetT(1));

                Modality[] modalities = detector.Modalities;
                EmguAssert.AreEqual(1, modalities.Length);
                EmguAssert.AreEqual("ColorGradient", modalities[0].Name);

                using (Mat img = CreateTemplateImage())
                using (Mat mask = new Mat(img.Size, DepthType.Cv8U, 1))
                using (VectorOfMat sources = new VectorOfMat())
                {
                    // The mask marks the object silhouette; AddTemplate looks for gradient
                    // features along the mask's border, so it must follow the rectangle's
                    // outline rather than covering the whole (background-included) frame.
                    mask.SetTo(new MCvScalar(0));
                    CvInvoke.Rectangle(mask, ObjectRegion, new MCvScalar(255), -1);
                    sources.Push(img);

                    Rectangle boundingBox = new Rectangle();
                    int templateId = detector.AddTemplate(sources, "test_class", mask, ref boundingBox);

                    EmguAssert.IsTrue(templateId >= 0, "AddTemplate should return a valid (non-negative) template id");
                    EmguAssert.IsTrue(boundingBox.Width > 0 && boundingBox.Height > 0, "AddTemplate should produce a non-empty bounding box");

                    EmguAssert.AreEqual(1, detector.NumTemplates);
                    EmguAssert.AreEqual(1, detector.NumClasses);
                    EmguAssert.AreEqual(1, detector.ClassIds.Length);
                    EmguAssert.AreEqual("test_class", detector.ClassIds[0]);

                    using (VectorOfLinemodMatch matches = new VectorOfLinemodMatch())
                    {
                        detector.Match(sources, 60f, matches);

                        EmguAssert.IsTrue(matches.Size > 0, "Match should find at least one match against the training image");
                        using (Match m = matches[0])
                        {
                            EmguAssert.AreEqual("test_class", m.class_id);
                            EmguAssert.AreEqual(templateId, m.TemplateId);
                            EmguAssert.IsTrue(m.Similarity >= 60f, "Match similarity should meet the requested threshold");
                        }
                    }
                }
            }
        }
    }
}
