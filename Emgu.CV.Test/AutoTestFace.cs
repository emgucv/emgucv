//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
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
    public class AutoTestFace
    {
        [Test]
        public void TestFaceRecognizer()
        {
            int trainingImgCount = 20;
            int numComponents = trainingImgCount / 5;
            Mat[] images = new Mat[trainingImgCount];
            int[] labels = new int[trainingImgCount];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
                CvInvoke.Randu(images[i], new MCvScalar(0), new MCvScalar(255));

                labels[i] = i;
            }

            Mat[] images2 = new Mat[trainingImgCount];
            int[] labels2 = new int[trainingImgCount];
            for (int i = 0; i < images2.Length; i++)
            {
                images2[i] = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
                CvInvoke.Randu(images2[i], new MCvScalar(0), new MCvScalar(255));

                labels2[i] = i + labels.Length;
            }

            Mat sample = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
            CvInvoke.Randu(sample, new MCvScalar(0), new MCvScalar(255));

            EigenFaceRecognizer eigen = new EigenFaceRecognizer(numComponents, double.MaxValue);

            eigen.Train(images, labels);
            FaceRecognizer.PredictionResult result;
            for (int i = 0; i < images.Length; i++)
            {
                result = eigen.Predict(images[i]);
                EmguAssert.IsTrue(result.Label == i);
            }

            result = eigen.Predict(sample);
            Trace.WriteLine(String.Format("Eigen distance: {0}", result.Distance));
            String filePath = Path.Combine(Path.GetTempPath(), "abc.xml");

            eigen.Write(filePath);
            using (EigenFaceRecognizer eigen2 = new EigenFaceRecognizer(numComponents, double.MaxValue))
            {
                eigen2.Read(filePath);
                for (int i = 0; i < images.Length; i++)
                {
                    result = eigen2.Predict(images[i]);
                    EmguAssert.IsTrue(result.Label == i);
                }
            }

            FisherFaceRecognizer fisher = new FisherFaceRecognizer(0, double.MaxValue);
            fisher.Train(images, labels);
            for (int i = 0; i < images.Length; i++)
            {
                result = fisher.Predict(images[i]);
                EmguAssert.IsTrue(result.Label == i);
            }
            result = fisher.Predict(sample);
            Trace.WriteLine(String.Format("Fisher distance: {0}", result.Distance));

            LBPHFaceRecognizer lbph = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
            lbph.Train(images, labels);
            lbph.Update(images2, labels2);
            
            using (VectorOfMat vm = lbph.Histograms)
            {
                EmguAssert.IsTrue(vm.Size == images.Length + images2.Length);
            }
            for (int i = 0; i < images.Length; i++)
            {
                EmguAssert.IsTrue(lbph.Predict(images[i]).Label == i);
            }

        }
    }
}
