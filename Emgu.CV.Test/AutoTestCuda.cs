//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.IO;
using System.Text;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using Emgu.CV.Util;
using Emgu.CV.Features;
using Emgu.CV.XFeatures2D;
using System.Runtime.InteropServices;

#if !NETCOREAPP
using Emgu.CV.UI;
#endif

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
    public class TestGpuMat
    {
        [Test]
        public void TestGetCudaEnabledDeviceCount()
        {
            if (CudaInvoke.HasCuda)
            {
                Trace.WriteLine(CudaInvoke.GetCudaDevicesSummary());
            }
        }

        /*
        public int MemTest()
        {
           while (true)
           {
              using (GpuMat<Byte> m = new GpuMat<Byte>(320, 240, 1))
              {
              }
           }
        }*/

        [Test]
        public void TestCudaImageAsyncOps()
        {
            if (!CudaInvoke.HasCuda)
                return;
            int counter = 0;
            Stopwatch watch = Stopwatch.StartNew();
            using (GpuMat img1 = new GpuMat(3000, 2000, DepthType.Cv8U, 3))
            using (GpuMat img2 = new GpuMat(3000, 2000, DepthType.Cv8U, 3))
            using (GpuMat img3 = new GpuMat())
            using (Stream stream = new Stream())
            using (GpuMat mat1 = new GpuMat())
            {
                img1.ConvertTo(mat1, DepthType.Cv8U, 1, 0, stream);
                while (!stream.Completed)
                {
                    if (counter <= int.MaxValue) counter++;
                }

                Trace.WriteLine(String.Format("Counter has been incremented {0} times", counter));

                counter = 0;
                CudaInvoke.CvtColor(img2, img3, CvToolbox.GetColorCvtCode(typeof(Bgr), typeof(Gray)), 1, stream);
                while (!stream.Completed)
                {
                    if (counter <= int.MaxValue) counter++;
                }

                Trace.WriteLine(String.Format("Counter has been incremented {0} times", counter));
            }

            watch.Stop();
            Trace.WriteLine(String.Format("Total time: {0} milliseconds", watch.ElapsedMilliseconds));

        }

        [Test]
        public void TestGpuMatContinuous()
        {
            if (!CudaInvoke.HasCuda)
                return;
            GpuMat mat = new GpuMat(1200, 640, DepthType.Cv8U, 1, true);

            EmguAssert.IsTrue(mat.IsContinuous);
        }

        [Test]
        public void TestGpuMatRange()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat img1 = new Mat(1200, 640, DepthType.Cv8U, 1);
            CvInvoke.Randu(img1, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            using (GpuMat gpuImg1 = new GpuMat(img1))
            using (GpuMat mat = new GpuMat(gpuImg1, new Emgu.CV.Structure.Range(0, 1), Emgu.CV.Structure.Range.All))
            {
                Size s = mat.Size;
            }

        }

        [Test]
        public void TestGpuMatAdd()
        {
            if (!CudaInvoke.HasCuda)
                return;
            int repeat = 1000;
            Mat img1 = new Mat(1200, 640, DepthType.Cv8U, 1);
            Mat img2 = new Mat(img1.Size, DepthType.Cv8U, 1);
            CvInvoke.Randu(img1, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            CvInvoke.Randu(img2, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

            Mat cpuImgSum = new Mat(img1.Size, DepthType.Cv8U, 1);
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
                CvInvoke.Add(img1, img2, cpuImgSum, null, CvEnum.DepthType.Cv8U);
            watch.Stop();
            Trace.WriteLine(
                String.Format("CPU processing time: {0}ms", (double)watch.ElapsedMilliseconds / repeat));

            watch.Reset();
            watch.Start();
            GpuMat gpuImg1 = new GpuMat(img1);
            GpuMat gpuImg2 = new GpuMat(img2);
            GpuMat gpuImgSum = new GpuMat(gpuImg1.Size.Height, gpuImg1.Size.Width, DepthType.Cv8U, 1);
            Stopwatch watch2 = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
                CudaInvoke.Add(gpuImg1, gpuImg2, gpuImgSum);
            watch2.Stop();
            using (Mat cpuImgSumFromGpu = gpuImgSum.ToMat())
            {
                watch.Stop();
                Trace.WriteLine(String.Format("Core GPU processing time: {0}ms",
                    (double)watch2.ElapsedMilliseconds / repeat));
                //Trace.WriteLine(String.Format("Total GPU processing time: {0}ms", (double)watch.ElapsedMilliseconds/repeat));

                EmguAssert.IsTrue(cpuImgSum.Equals(cpuImgSumFromGpu));
            }
        }

        [Test]
        public void TestSplitMerge()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img1 = new Mat(1200, 640, DepthType.Cv8U, 3))
            {
                CvInvoke.Randu(img1, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

                using (GpuMat gpuImg1 = new GpuMat(img1))
                {
                    GpuMat[] channels = gpuImg1.Split(null);

                    for (int i = 0; i < channels.Length; i++)
                    {
                        Mat imgL = channels[i].ToMat();

                        using (Mat imgR = new Mat())
                        {
                            CvInvoke.ExtractChannel(img1, imgR, i);
                            EmguAssert.IsTrue(imgL.Equals(imgR), "failed split GpuMat");
                        }
                    }

                    using (GpuMat gpuImg2 = new GpuMat())
                    {
                        gpuImg2.MergeFrom(channels, null);
                        using (Mat img2 = new Mat())
                        {
                            gpuImg2.Download(img2);
                            EmguAssert.IsTrue(img2.Equals(img1), "failed split and merge test");
                        }
                    }

                    for (int i = 0; i < channels.Length; i++)
                    {
                        channels[i].Dispose();
                    }
                }
            }

        }

        [Test]
        public void TestCudaFlip()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img1 = new Mat(1200, 640, DepthType.Cv8U, 3))
            {
                CvInvoke.Randu(img1, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                
                using (Mat img1Flip = new Mat())
                using (GpuMat cudaImage = new GpuMat(img1))
                using (GpuMat cudaFlip = new GpuMat())
                {
                    CvInvoke.Flip(img1, img1Flip, FlipType.Both);
                    CudaInvoke.Flip(cudaImage, cudaFlip, FlipType.Both,
                        null);
                    cudaFlip.Download(img1);
                    EmguAssert.IsTrue(img1.Equals(img1Flip));
                }
            }

        }

        [Test]
        public void TestConvolutionAndLaplace()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 1);
            CvInvoke.Randu(image, new MCvScalar(0.0), new MCvScalar(255.0));
            Mat kernel = new Mat(3, 3, DepthType.Cv32F, 1);
            kernel.SetTo( new float[]
            {
                0.0f, 1.0f, 0.0f, 
                1.0f, -4.0f, 1.0f,
                0.0f, 1.0f, 0.0f
            });
            
            using (Stream s = new Stream())
            using (GpuMat cudaImg = new GpuMat(image))
            using (GpuMat cudaLaplace = new GpuMat())
            using (CudaLinearFilter cudaLinear =
                new CudaLinearFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, kernel, new Point(1, 1)))
            using (GpuMat cudaConv = new GpuMat())
            using (CudaLaplacianFilter laplacian =
                new CudaLaplacianFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, 1, 1.0))
            {
                cudaLinear.Apply(cudaImg, cudaConv, s);
                laplacian.Apply(cudaImg, cudaLaplace, s);
                s.WaitForCompletion();
                EmguAssert.IsTrue(cudaLaplace.Equals(cudaConv));
            }

        }

        [Test]
        public void TestCudaFilters()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 1);
            CvInvoke.Randu(image, new MCvScalar(0.0), new MCvScalar(255.0));
            using (GpuMat cudaImg1 = new GpuMat(image))
            using (GpuMat cudaImg2 = new GpuMat())
            using (CudaGaussianFilter gaussian = new CudaGaussianFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1,
                new Size(5, 5), 0, 0, CvEnum.BorderType.Default, CvEnum.BorderType.Default))
            using (CudaSobelFilter sobel = new CudaSobelFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, 1, 1, 3, 1.0,
                CvEnum.BorderType.Default, CvEnum.BorderType.Default))
            {
                gaussian.Apply(cudaImg1, cudaImg2, null);
                sobel.Apply(cudaImg1, cudaImg2, null);

            }

        }

        [Test]
        public void TestResizeGray()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 1);
            CvInvoke.Randu(image, new MCvScalar(0.0), new MCvScalar(255.0));

            //Image<Gray, Byte> img = new Image<Gray, byte>("airplane.jpg");
            Mat small = new Mat();
            CvInvoke.Resize(image, small, new Size(100, 200));
            //Image<Gray, Byte> small = img.Resize(100, 200, Emgu.CV.CvEnum.Inter.Linear);

            GpuMat gpuImg = new GpuMat(image);
            GpuMat smallGpuImg = new GpuMat();
            CudaInvoke.Resize(gpuImg, smallGpuImg, small.Size);

            Mat smallGpuDownloadImg = new Mat();
            smallGpuImg.Download(smallGpuDownloadImg);
            Mat diff = new Mat();
            CvInvoke.AbsDiff(small, smallGpuDownloadImg, diff);

        }

        [Test]
        public void TestClone()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 1);
            CvInvoke.Randu(image, new MCvScalar(0.0), new MCvScalar(255.0));

            using (GpuMat gImg1 = new GpuMat(image))
            using (GpuMat gImg2 = new GpuMat())
            using (Mat img2 = new Mat())
            {
                gImg1.CopyTo(gImg2);
                gImg2.Download(img2);
                EmguAssert.IsTrue(image.Equals(img2));
            }

        }

        [Test]
        public void TestColorConvert()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 3);
            CvInvoke.Randu(image, new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            //Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            Mat imgGray = new Mat();
            CvInvoke.CvtColor(image, imgGray, ColorConversion.Bgr2Gray);
            //Image<Gray, Byte> imgGray = img.Convert<Gray, Byte>();
            Mat imgHsv = new Mat();
            CvInvoke.CvtColor(image, imgHsv, ColorConversion.Bgr2Hsv);
            //Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>();

            GpuMat gpuImg = new GpuMat(image);
            GpuMat gpuImgGray = new GpuMat();
            CudaInvoke.CvtColor(gpuImg, gpuImgGray, ColorConversion.Bgr2Gray);
            GpuMat gpuImgHsv = new GpuMat();
            CudaInvoke.CvtColor(gpuImg, gpuImgHsv, ColorConversion.Bgr2Hsv);


            EmguAssert.IsTrue(gpuImgGray.Equals(new GpuMat(imgGray)));
            Mat gpuImgHsvDownloaded = new Mat();
            gpuImgHsv.Download(gpuImgHsvDownloaded);
            EmguAssert.IsTrue(gpuImgHsvDownloaded.Equals(imgHsv));
            EmguAssert.IsTrue(gpuImgHsv.Equals(new GpuMat(imgHsv)));

        }

        [Test]
        public void TestInplaceNot()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = new Mat(300, 400, DepthType.Cv8U, 3);
            GpuMat gpuMat = new GpuMat(image);
            CudaInvoke.BitwiseNot(gpuMat, gpuMat, null, null);
            Mat imageNot = new Mat();
            CvInvoke.BitwiseNot(image, imageNot);
            EmguAssert.IsTrue(gpuMat.Equals(new GpuMat(imageNot)));

        }


        [Test]
        public void TestResizeBgr()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat img = EmguAssert.LoadMat("pedestrian.png");
            //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));

            Size size = new Size(100, 200);

            GpuMat cudaImg = new GpuMat(img);
            GpuMat smallCudaImg = new GpuMat();

            CudaInvoke.Resize(cudaImg, smallCudaImg, size);
            Mat smallCpuImg = new Mat();
            CvInvoke.Resize(img, smallCpuImg, size, 0, 0);
            //Image<Bgr, Byte> smallCpuImg = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.Inter.Linear);
            Mat smallCudaImgDownloaded = new Mat();
            smallCudaImg.Download(smallCudaImgDownloaded);

            Mat diff = new Mat();
            CvInvoke.AbsDiff(smallCudaImgDownloaded, smallCpuImg, diff);
            
            //TODO: Check why they are not an exact match
            //EmguAssert.IsTrue(diff.CountNonzero()[0] == 0);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);

        }


        [Test]
        public void TestCanny()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img = EmguAssert.LoadMat("pedestrian.png", ImreadModes.ColorBgr))
            using (GpuMat cudaImage = new GpuMat(img))
            using (GpuMat gray = new GpuMat())
            using (GpuMat canny = new GpuMat())
            using (CudaCannyEdgeDetector detector = new CudaCannyEdgeDetector(20, 100, 3, false))
            {
                CudaInvoke.CvtColor(cudaImage, gray, ColorConversion.Bgr2Gray);
                detector.Detect(gray, canny);
                //GpuInvoke.Canny(gray, canny, 20, 100, 3, false);
                //ImageViewer.Show(canny);
            }

        }

        [Test]
        public void TestCountNonZero()
        {
            if (!CudaInvoke.HasCuda)
                return;

            //Mat m = new Mat(100, 200, Mat.Depth.Cv8U, 1);
            GpuMat m = new GpuMat(100, 200, DepthType.Cv8U, 1);
            m.SetTo(new MCvScalar(), null, null);
            EmguAssert.IsTrue(0 == CudaInvoke.CountNonZero(m));
            //Trace.WriteLine(String.Format("non zero count: {0}", ));
        }


        [Test]
        public void TestClahe()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat image = EmguAssert.LoadMat("pedestrian.png");
            GpuMat cudaImage = new GpuMat(image);
            GpuMat cudaResult = new GpuMat();

            using (CudaClahe clahe = new CudaClahe(40.0, new Size(8, 8)))
            {
                Mat result = new Mat();
                clahe.Apply(cudaImage, cudaResult, null);
                cudaResult.Download(result);
                //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
            }

        }

        [Test]
        public void TestHOG1()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (CudaHOG hog = new CudaHOG(new Size(64, 128), new Size(16, 16), new Size(8, 8), new Size(8, 8),
                    9))
            using (Mat pedestrianDescriptor = hog.GetDefaultPeopleDetector())
            using (Mat image = EmguAssert.LoadMat("pedestrian.png"))
            {
                hog.SetSVMDetector(pedestrianDescriptor);
                //hog.GroupThreshold = 0;
                Stopwatch watch = Stopwatch.StartNew();
                Rectangle[] rects;
                using (GpuMat cudaImage = new GpuMat(image))
                using (GpuMat gpuBgra = new GpuMat())
                using (VectorOfRect vRect = new VectorOfRect())
                {
                    CudaInvoke.CvtColor(cudaImage, gpuBgra, ColorConversion.Bgr2Bgra);
                    hog.DetectMultiScale(gpuBgra, vRect);
                    rects = vRect.ToArray();
                }

                watch.Stop();

                EmguAssert.AreEqual(rects.Length, 1);

                foreach (Rectangle rect in rects)
                    CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 1);
                    //image.Draw(rect, new Bgr(Color.Red), 1);
                Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }

        }

        [Test]
        public void TestHOG2()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (CudaHOG hog = new CudaHOG(
                    new Size(48, 96), //winSize
                    new Size(16, 16), //blockSize
                    new Size(8, 8), //blockStride
                    new Size(8, 8) //cellSize
                ))
            using (Mat pedestrianDescriptor = hog.GetDefaultPeopleDetector())
            using (Mat image = EmguAssert.LoadMat("pedestrian.png"))
            {
                //float[] pedestrianDescriptor = CudaHOGDescriptor.GetPeopleDetector48x96();
                hog.SetSVMDetector(pedestrianDescriptor);

                Stopwatch watch = Stopwatch.StartNew();
                Rectangle[] rects;
                using (GpuMat cudaImage = new GpuMat(image))
                using (GpuMat gpuBgra = new GpuMat())
                using (VectorOfRect vRect = new VectorOfRect())
                {
                    CudaInvoke.CvtColor(cudaImage, gpuBgra, ColorConversion.Bgr2Bgra);
                    hog.DetectMultiScale(gpuBgra, vRect);
                    rects = vRect.ToArray();
                }

                watch.Stop();

                //Assert.AreEqual(1, rects.Length);

                foreach (Rectangle rect in rects)
                    CvInvoke.Rectangle(image, rect, new MCvScalar(0,0,255), 1);
                    //image.Draw(rect, new Bgr(Color.Red), 1);
                Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }

        }

        [Test]
        public void TestCudaReduce()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img = new Mat(480, 320, DepthType.Cv8U, 3))
            {
                CvInvoke.Randu(img, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                //img.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                using (GpuMat cudaImage = new GpuMat(img))
                using (GpuMat reduced = new GpuMat())
                {
                    CudaInvoke.Reduce(cudaImage, reduced, CvEnum.ReduceDimension.SingleRow,
                        CvEnum.ReduceType.ReduceAvg);
                }
            }
        }

        [Test]
        public void TestErodeDilate()
        {
            if (!CudaInvoke.HasCuda)
                return;

            int morphIter = 2;
            //Image<Gray, Byte> image = new Image<Gray, byte>(640, 320);
            //image.Draw(new CircleF(new PointF(200, 200), 30), new Gray(255.0), 4);
            Mat image = new Mat(640, 320, DepthType.Cv8U, 1);
            CvInvoke.Circle(image, new Point(200, 200), 30, new MCvScalar(255), 4);

            Size ksize = new Size(morphIter * 2 + 1, morphIter * 2 + 1);

            using (GpuMat cudaImage = new GpuMat(image))
            using (GpuMat cudaTemp = new GpuMat())
            using (Stream stream = new Stream())
            using (CudaBoxMaxFilter dilate = new CudaBoxMaxFilter(DepthType.Cv8U, 1, ksize, new Point(-1, -1),
                CvEnum.BorderType.Default, new MCvScalar()))
            using (CudaBoxMinFilter erode = new CudaBoxMinFilter(DepthType.Cv8U, 1, ksize, new Point(-1, -1),
                CvEnum.BorderType.Default, new MCvScalar()))
            {
                //run the GPU version asyncrhonously with stream
                erode.Apply(cudaImage, cudaTemp, stream);
                dilate.Apply(cudaTemp, cudaImage, stream);

                //run the CPU version in parallel to the gpu version.
                using (Mat temp = new Mat())
                {
                    CvInvoke.Erode(image, temp, null, new Point(-1, -1), morphIter, CvEnum.BorderType.Constant,
                        new MCvScalar());
                    CvInvoke.Dilate(temp, image, null, new Point(-1, -1), morphIter, CvEnum.BorderType.Constant,
                        new MCvScalar());
                }

                //syncrhonize with the GPU version
                stream.WaitForCompletion();

                EmguAssert.IsTrue(cudaImage.ToMat().Equals(image));
            }

        }

#if NONFREE
        [Test]
        public void TestCudaSURFKeypointDetection()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, byte> image = new Image<Gray, byte>(200, 100);
                image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
                GpuMat gpuMat = new GpuMat(image);

                EmguAssert.IsTrue(gpuMat.ToMat().Equals(image.Mat));

                CudaSURF cudaSurf = new CudaSURF(100.0f, 2, 4, false, 0.01f, false);
                GpuMat cudaKpts = cudaSurf.DetectKeyPointsRaw(gpuMat, null);
                VectorOfKeyPoint kpts = new VectorOfKeyPoint();
                cudaSurf.DownloadKeypoints(cudaKpts, kpts);
            }
        }
#endif

        [Test]
        public void TestCudaFASTDetector()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img = EmguAssert.LoadMat("box.png"))
            using (GpuMat cudaImage = new GpuMat(img))
            using (GpuMat grayCudaImage = new GpuMat())
            using (CudaFastFeatureDetector featureDetector =
                new CudaFastFeatureDetector(10, true, FastFeatureDetector.DetectorType.Type9_16, 1000))
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            using (GpuMat keyPointsMat = new GpuMat())
            {
                CudaInvoke.CvtColor(cudaImage, grayCudaImage, ColorConversion.Bgr2Gray);
                featureDetector.DetectAsync(grayCudaImage, keyPointsMat);
                featureDetector.Convert(keyPointsMat, kpts);
                //featureDetector.DetectKeyPointsRaw(grayCudaImage, null, keyPointsMat);

                //featureDetector.DownloadKeypoints(keyPointsMat, kpts);

                foreach (MKeyPoint kpt in kpts.ToArray())
                {
                    CvInvoke.Circle(img, Point.Round(kpt.Point), 3, new MCvScalar(0, 255, 0), 1);
                    //img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
                }

                //ImageViewer.Show(img);
            }
        }

        [Test]
        public void TestCudaOrbDetector()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat img = EmguAssert.LoadMat("box.png"))
            using (GpuMat cudaImage = new GpuMat(img))
            using (GpuMat grayCudaImage = new GpuMat())
            using (CudaORBDetector detector = new CudaORBDetector(500))
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            using (GpuMat keyPointMat = new GpuMat())
            using (GpuMat descriptorMat = new GpuMat())
            {
                CudaInvoke.CvtColor(cudaImage, grayCudaImage, ColorConversion.Bgr2Gray);

                //Async version
                detector.DetectAsync(grayCudaImage, keyPointMat);
                detector.Convert(keyPointMat, kpts);

                foreach (MKeyPoint kpt in kpts.ToArray())
                {
                    CvInvoke.Circle(img, Point.Round(kpt.Point), 3, new MCvScalar(0, 255, 0), 1);
                    //img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
                }

                //sync version
                detector.DetectRaw(grayCudaImage, kpts);

                //ImageViewer.Show(img);
            }
        }

        [Test]
        public void TestCudaPyr()
        {
            if (!CudaInvoke.HasCuda)
                return;
            //Image<Gray, Byte> img = new Image<Gray, byte>(640, 480);
            Mat img = new Mat(640, 480, DepthType.Cv8U, 3);
            CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255));
            //img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            Mat down = new Mat();
            Mat up = new Mat();
            CvInvoke.PyrDown(img, down);
            CvInvoke.PyrUp(down, up);
            //Image<Gray, Byte> down = img.PyrDown();
            //Image<Gray, Byte> up = down.PyrUp();

            GpuMat gImg = new GpuMat(img);
            GpuMat gDown = new GpuMat();
            GpuMat gUp = new GpuMat();
            CudaInvoke.PyrDown(gImg, gDown, null);
            CudaInvoke.PyrUp(gDown, gUp, null);

            Mat gDownCpu = new Mat();
            gDown.Download(gDownCpu);
            Mat gUpCpu = new Mat();
            gUp.Download(gDownCpu);

            CvInvoke.AbsDiff(down, gDownCpu, down);
            CvInvoke.AbsDiff(up, gUpCpu, up);
            double[] minVals, maxVals;
            Point[] minLocs, maxLocs;
            down.MinMax(out minVals, out maxVals, out minLocs, out maxLocs);
            double maxVal = 0.0;
            for (int i = 0; i < maxVals.Length; i++)
            {
                if (maxVals[i] > maxVal)
                    maxVal = maxVals[i];
            }

            Trace.WriteLine(String.Format("Max diff: {0}", maxVal));
            EmguAssert.IsTrue(maxVal <= 1.0);
            //Assert.LessOrEqual(maxVal, 1.0);

            up.MinMax(out minVals, out maxVals, out minLocs, out maxLocs);
            maxVal = 0.0;
            for (int i = 0; i < maxVals.Length; i++)
            {
                if (maxVals[i] > maxVal)
                    maxVal = maxVals[i];
            }

            Trace.WriteLine(String.Format("Max diff: {0}", maxVal));
            EmguAssert.IsTrue(maxVal <= 1.0);
            //Assert.LessOrEqual(maxVal, 1.0);
        }


        [Test]
        public void TestMatchTemplate()
        {
            if (!CudaInvoke.HasCuda)
                return;

            #region prepare synthetic image for testing

            int templWidth = 50;
            int templHeight = 50;
            Point templCenter = new Point(120, 100);

            //Create a random object
            Mat randomObj = new Mat(templWidth, templHeight, DepthType.Cv8U, 3);
            CvInvoke.Randu(randomObj, new MCvScalar(), new MCvScalar(255, 255, 255));
            //randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

            //Draw the object in image1 center at templCenter;
            //Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
            Mat img = new Mat(300, 200, DepthType.Cv8U, 3);
            Rectangle objectLocation = new Rectangle(templCenter.X - (templWidth >> 1),
                templCenter.Y - (templHeight >> 1), templWidth, templHeight);
            using (Mat tmp = new Mat(img, objectLocation))
                randomObj.CopyTo(tmp);
            //img.ROI = objectLocation;
            //randomObj.Copy(img, null);
            //img.ROI = Rectangle.Empty;

            #endregion

            Mat match = new Mat();
            CvInvoke.MatchTemplate(img, randomObj, match, TemplateMatchingType.Sqdiff);
            //Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TemplateMatchingType.Sqdiff);
            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;
            match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            double gpuMinVal = 0, gpuMaxVal = 0;
            Point gpuMinLoc = Point.Empty, gpuMaxLoc = Point.Empty;
            GpuMat cudaImage = new GpuMat(img);
            GpuMat gpuRandomObj = new GpuMat(randomObj);
            GpuMat gpuMatch = new GpuMat();
            using (CudaTemplateMatching buffer =
                new CudaTemplateMatching(DepthType.Cv8U, 3, CvEnum.TemplateMatchingType.Sqdiff))
            using (Stream stream = new Stream())
            {
                buffer.Match(cudaImage, gpuRandomObj, gpuMatch, stream);
                //GpuInvoke.MatchTemplate(CudaImage, gpuRandomObj, gpuMatch, CvEnum.TM_TYPE.CV_TM_SQDIFF, buffer, stream);
                stream.WaitForCompletion();
                CudaInvoke.MinMaxLoc(gpuMatch, ref gpuMinVal, ref gpuMaxVal, ref gpuMinLoc, ref gpuMaxLoc, null);
            }

            EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
            EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
            EmguAssert.IsTrue(minLoc[0].Equals(gpuMinLoc));
            EmguAssert.IsTrue(maxLoc[0].Equals(gpuMaxLoc));

        }

        [Test]
        public void TestCudaRemap()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat xmap = new Mat(2, 2, DepthType.Cv32F, 1);
            xmap.SetTo( new float[]
            {
                0.0f, 0.0f,
                1.0f, 1.0f
            });
            Mat ymap = new Mat(2, 2, DepthType.Cv32F, 1);
            ymap.SetTo( new float[]
                {
                    0.0f, 1.0f, 
                    0.0f, 1.0f
                });
            /*
            Image<Gray, float> xmap = new Image<Gray, float>(2, 2);
            xmap.Data[0, 0, 0] = 0;
            xmap.Data[0, 1, 0] = 0;
            xmap.Data[1, 0, 0] = 1;
            xmap.Data[1, 1, 0] = 1;
            Image<Gray, float> ymap = new Image<Gray, float>(2, 2);
            ymap.Data[0, 0, 0] = 0;
            ymap.Data[0, 1, 0] = 1;
            ymap.Data[1, 0, 0] = 0;
            ymap.Data[1, 1, 0] = 1;
            */

            Mat image = new Mat(2, 2, DepthType.Cv8U, 1);
            CvInvoke.Randn(image, new MCvScalar(), new MCvScalar(255));
            //Image<Gray, Byte> image = new Image<Gray, byte>(2, 2);
            //image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

            using (GpuMat CudaImage = new GpuMat(image))
            using (GpuMat xCudaImage = new GpuMat(xmap))
            using (GpuMat yCudaImage = new GpuMat(ymap))
            using (GpuMat remapedImage = new GpuMat())
            {
                CudaInvoke.Remap(CudaImage, remapedImage, xCudaImage, yCudaImage, CvEnum.Inter.Cubic,
                    CvEnum.BorderType.Default, new MCvScalar(), null);
            }
        }

        [Test]
        public void TestCudaWarpPerspective()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat transformation = new Mat(3, 3, DepthType.Cv32F, 1);
            transformation.SetTo(new MCvScalar());
            CvInvoke.SetIdentity(transformation, new MCvScalar(1.0));
            //transformation.SetIdentity();

            Mat image = new Mat(480, 320, DepthType.Cv8U, 1);
            CvInvoke.Randn(image, new MCvScalar(), new MCvScalar(255));
            //Image<Gray, byte> image = new Image<Gray, byte>(480, 320);
            //image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

            using (GpuMat cudaImage = new GpuMat(image))
            using (GpuMat resultCudaImage = new GpuMat())
            {
                CudaInvoke.WarpPerspective(cudaImage, resultCudaImage, transformation, cudaImage.Size,
                    CvEnum.Inter.Cubic, CvEnum.BorderType.Default, new MCvScalar(), null);
            }
        }

        [Test]
        public void TestCudaPyrLKOpticalFlow()
        {
            if (!CudaInvoke.HasCuda)
                return;
            //Image<Gray, Byte> prevImg, currImg;
            //AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
            GpuMat[] images = OpticalFlowImage();
            Mat flow = new Mat();
            using (CudaDensePyrLKOpticalFlow opticalflow = new CudaDensePyrLKOpticalFlow(new Size(21, 21), 3, 30, false)
            )
            using (CudaSparsePyrLKOpticalFlow opticalFlowSparse = new CudaSparsePyrLKOpticalFlow(new Size(21, 21)))
            //using (CudaImage<Gray, Byte> prevGpu = new CudaImage<Gray, byte>(prevImg))
            //using (CudaImage<Gray, byte> currGpu = new CudaImage<Gray, byte>(currImg))
            using (GpuMat flowGpu = new GpuMat())
            using (VectorOfPointF prevPts = new VectorOfPointF())
            using (GpuMat currPtrsGpu = new GpuMat())
            using (GpuMat statusGpu = new GpuMat())
            {
                opticalflow.Calc(images[0], images[1], flowGpu);

                flowGpu.Download(flow);

                int pointsCount = 100;
                Size s = images[0].Size;
                PointF[] points = new PointF[pointsCount];
                Random r = new Random(1234);
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new PointF(r.Next(s.Height), r.Next(s.Width));
                }

                prevPts.Push(points);
                using (GpuMat prevPtsGpu = new GpuMat(prevPts))
                    opticalFlowSparse.Calc(images[0], images[1], prevPtsGpu, currPtrsGpu, statusGpu);
            }

            for (int i = 0; i < images.Length; i++)
                images[i].Dispose();
        }

        [Test]
        public void TestCudaUploadDownload()
        {
            if (!CudaInvoke.HasCuda)
                return;

            Mat m = new Mat(new Size(480, 320), DepthType.Cv8U, 3);
            CvInvoke.Randu(m, new MCvScalar(), new MCvScalar(255, 255, 255));

            #region test for async download & upload

            Stream stream = new Stream();
            GpuMat gm1 = new GpuMat();
            gm1.Upload(m, stream);

            Mat m2 = new Mat();
            gm1.Download(m2, stream);

            stream.WaitForCompletion();
            EmguAssert.IsTrue(m.Equals(m2));

            #endregion

            #region test for blocking download & upload

            GpuMat gm2 = new GpuMat();
            gm2.Upload(m);
            Mat m3 = new Mat();
            gm2.Download(m3);
            EmguAssert.IsTrue(m.Equals(m3));

            #endregion
        }


        [Test]
        public void TestCudaBroxOpticalFlow()
        {
            if (!CudaInvoke.HasCuda)
                return;

            //Image<Gray, Byte> prevImg, currImg;
            //AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
            GpuMat[] images = OpticalFlowImage();
            Mat flow = new Mat();
            CudaBroxOpticalFlow opticalflow = new CudaBroxOpticalFlow();
            //using (CudaImage<Gray, float> prevGpu = new CudaImage<Gray, float>(prevImg.Convert<Gray, float>()))
            //using (CudaImage<Gray, float> currGpu = new CudaImage<Gray, float>(currImg.Convert<Gray, float>()))
            using (GpuMat flowGpu = new GpuMat())
            {
                opticalflow.Calc(images[0], images[1], flowGpu);

                flowGpu.Download(flow);
            }

            for (int i = 0; i < images.Length; i++)
                images[i].Dispose();
        }


        [Test]
        public void TestBilaterialFilter()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat img = EmguAssert.LoadMat("pedestrian.png");
            Mat gray = new Mat();
            CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
            //Image<Gray, byte> gray = img.Convert<Gray, Byte>();
            GpuMat cudaImage = new GpuMat(gray);
            GpuMat gpuBilaterial = new GpuMat();
            CudaInvoke.BilateralFilter(cudaImage, gpuBilaterial, 7, 5, 5, CvEnum.BorderType.Default, null);

            //Emgu.CV.UI.ImageViewer.Show(gray.ConcateHorizontal(gpuBilaterial.ToImage()));

        }

#if CUDACODEC
        [Test]
        public void TestCudaVideoWriter()
        {
           if (CudaInvoke.HasCuda)
           {
              using (CudaVideoWriter writer = new CudaVideoWriter("cudavideo.avi", new Size(640, 480), 24))
              {
                 using (GpuMat m1 = new GpuMat(480, 640, DepthType.Cv8U, 3))
                    writer.Write(m1, false);
                 using (GpuMat m2 = new GpuMat(480, 640, DepthType.Cv8U, 3))
                    writer.Write(m2, true);
              }
           }
        }

        [Test]
        public void TestCudaVideoReader()
        {
            int numberOfFrames = 10;
            int width = 640;
            int height = 480;
            String fileName = AutoTestVarious.GetTempFileName() + ".avi";

            Image<Bgr, Byte>[] images = new Image<Bgr, byte>[numberOfFrames];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Image<Bgr, byte>(width, height);
                images[i].SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            }

            int fourcc = VideoWriter.Fourcc('M', 'J', 'P', 'G');
            Backend[] backends = CvInvoke.WriterBackends;
            int backend_idx = 0; //any backend;
            foreach (Backend be in backends)
            {
                if (be.Name.Equals("CV_MJPEG"))
                {
                    backend_idx = be.ID;
                    break;
                }
            }

            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('H', '2', '6', '4'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('M', 'J', 'P', 'G'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('X', '2', '6', '4'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, -1, 5, new Size( width, height ), true))
            using (VideoWriter writer =
 new VideoWriter(fileName, backend_idx, fourcc, 24, new Size(width, height), true))
            {
                //EmguAssert.IsTrue(writer.IsOpened);
                for (int i = 0; i < numberOfFrames; i++)
                {
                    writer.Write(images[i].Mat);
                }
            }

            FileInfo fi = new FileInfo(fileName);
            EmguAssert.IsTrue(fi.Exists && fi.Length != 0, "File should not be empty");

            using (CudaVideoReader reader = new CudaVideoReader(fileName))
            {
                var formatInfo = reader.Format;
                int count = 0;
                using (GpuMat frame = new GpuMat())
                    while (reader.NextFrame(frame))
                    {
                        EmguAssert.IsTrue(frame.Size.Width == width);
                        EmguAssert.IsTrue(frame.Size.Height == height);

                        count++;
                    }
                EmguAssert.IsTrue(numberOfFrames == count);
            }
            File.Delete(fi.FullName);
        }
#endif

        /*
        [Test]
        public void TestSoftcascadeCuda()
        {
           if (CudaInvoke.HasCuda)
           {
              using (CudaDeviceInfo cdi = new CudaDeviceInfo())
              {

                 using (CudaSoftCascadeDetector detector = new CudaSoftCascadeDetector(EmguAssert.GetFile("inria_caltech-17.01.2013.xml"), 0.4, 5.0, 55, SoftCascadeDetector.RejectionCriteria.Default))
                 using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png"))
                 using (Cuda.CudaImage<Bgr, Byte> gImage = new CudaImage<Bgr, byte>(image))
                 using (Matrix<int> rois = new Matrix<int>(1, 4))
                 {
                    Stopwatch watch = Stopwatch.StartNew();

                    Size s = gImage.Size;
                    rois.Data[0, 2] = s.Width;
                    rois.Data[0, 3] = s.Height;
                    using (GpuMat<int> gRois = new GpuMat<int>(rois))
                    {
                       Emgu.CV.Cuda.GpuMat result = detector.Detect(gImage, gRois, null);
                    }
                    watch.Stop();
                    //foreach (SoftCascadeDetector.Detection detection in detections)
                    //   image.Draw(detection.BoundingBox, new Bgr(Color.Red), 1);

                    //Emgu.CV.UI.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
                 }
              }
           }
        }*/

        [Test]
        public void TestGEMM()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat a = new Mat(3, 3, DepthType.Cv32F, 1);
            Mat b = new Mat(3, 3, DepthType.Cv32F, 1);
            Mat c = new Mat(3, 3, DepthType.Cv32F, 1);
            Mat d = new Mat(3, 3, DepthType.Cv32F, 1);
            GpuMat ga = new GpuMat();
            GpuMat gb = new GpuMat();
            GpuMat gc = new GpuMat();
            GpuMat gd = new GpuMat();
            ga.Upload(a);
            gb.Upload(b);
            gc.Upload(c);
            gd.Upload(d);

            CvInvoke.Gemm(a, b, 1.0, c, 0.0, d);
            CudaInvoke.Gemm(ga, gb, 1.0, gc, 0.0, gd);

        }

        [Test]
        public void TestHughCircle()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat m = new Mat(480, 480, DepthType.Cv8U, 1);
            m.SetTo(new MCvScalar(0));
            CvInvoke.Circle(m, new Point(240, 240), 100, new MCvScalar(150), 10);
            GpuMat gm = new GpuMat();
            gm.Upload(m);
            using (CudaHoughCirclesDetector detector = new CudaHoughCirclesDetector(1, 30, 120, 30, 10, 400))
            using (GpuMat circlesGpu = new GpuMat())
            using (Mat circlesMat = new Mat())
            {
                detector.Detect(gm, circlesGpu);
                circlesGpu.Download(circlesMat);
                CircleF[] circles = new CircleF[circlesMat.Cols];
                GCHandle circlesHandle = GCHandle.Alloc(circles, GCHandleType.Pinned);
                Emgu.CV.Util.CvToolbox.Memcpy(circlesHandle.AddrOfPinnedObject(), circlesMat.DataPointer,
                    Marshal.SizeOf(typeof(CircleF)) * circles.Length);
                circlesHandle.Free();
                foreach (var circle in circles)
                {
                    CvInvoke.Circle(m, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(255));
                }
            }

        }

        [Test]
        public void TestCudaHoughCircle()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat m = new Mat(480, 480, DepthType.Cv8U, 1);
            m.SetTo(new MCvScalar(0));
            CvInvoke.Circle(m, new Point(240, 240), 100, new MCvScalar(150), 10);
            GpuMat gm = new GpuMat();
            gm.Upload(m);
            using (CudaHoughCirclesDetector detector = new CudaHoughCirclesDetector(1, 30, 120, 30, 10, 400))
            {
                CircleF[] circles = detector.Detect(gm);
                foreach (var circle in circles)
                {
                    CvInvoke.Circle(m, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(255));
                }
            }



        }

        [Test]
        public void TestBruteForceHammingDistance()
        {
            if (!CudaInvoke.HasCuda)
                return;

            Mat box = EmguAssert.LoadMat("box.png");
            FastFeatureDetector fast = new FastFeatureDetector(100, true);
            BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);

            #region extract features from the object image

            Stopwatch stopwatch = Stopwatch.StartNew();
            VectorOfKeyPoint modelKeypoints = new VectorOfKeyPoint();
            fast.DetectRaw(box, modelKeypoints);
            Mat modelDescriptors = new Mat();
            brief.Compute(box, modelKeypoints, modelDescriptors);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec",
                stopwatch.ElapsedMilliseconds));

            #endregion

            Mat observedImage = EmguAssert.LoadMat("box_in_scene.png", ImreadModes.Grayscale);

            #region extract features from the observed image

            stopwatch.Reset();
            stopwatch.Start();
            VectorOfKeyPoint observedKeypoints = new VectorOfKeyPoint();
            fast.DetectRaw(observedImage, observedKeypoints);
            Mat observedDescriptors = new Mat();
            brief.Compute(observedImage, observedKeypoints, observedDescriptors);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec",
                stopwatch.ElapsedMilliseconds));

            #endregion

            Mat homography = null;
            using (GpuMat gpuModelDescriptors = new GpuMat(modelDescriptors)
            ) //initialization of GPU code might took longer time.
            {
                stopwatch.Reset();
                stopwatch.Start();
                CudaBFMatcher hammingMatcher = new CudaBFMatcher(DistanceType.Hamming);

                //BFMatcher hammingMatcher = new BFMatcher(BFMatcher.DistanceType.Hamming, modelDescriptors);
                int k = 2;
                Mat trainIdx = new Mat(observedKeypoints.Size, k, DepthType.Cv32S , 1);
                Mat distance = new Mat(trainIdx.Size, DepthType.Cv32F, 1);

                using (GpuMat gpuObservedDescriptors = new GpuMat(observedDescriptors))
                //using (GpuMat<int> gpuTrainIdx = new GpuMat<int>(trainIdx.Rows, trainIdx.Cols, 1, true))
                //using (GpuMat<float> gpuDistance = new GpuMat<float>(distance.Rows, distance.Cols, 1, true))
                using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                {
                    Stopwatch w2 = Stopwatch.StartNew();
                    //hammingMatcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k);
                    hammingMatcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k, null, true);
                    w2.Stop();
                    Trace.WriteLine(String.Format(
                        "Time for feature matching (excluding data transfer): {0} milli-sec",
                        w2.ElapsedMilliseconds));
                    //gpuTrainIdx.Download(trainIdx);
                    //gpuDistance.Download(distance);


                    Mat mask = new Mat(distance.Rows, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));
                    FeaturesToolbox.VoteForUniqueness(matches, 0.8, mask);

                    int nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        nonZeroCount = FeaturesToolbox.VoteForSizeAndOrientation(modelKeypoints,
                            observedKeypoints,
                            matches, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                            homography = FeaturesToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints,
                                observedKeypoints, matches, mask, 2);
                        nonZeroCount = CvInvoke.CountNonZero(mask);
                    }

                    stopwatch.Stop();
                    Trace.WriteLine(String.Format(
                        "Time for feature matching (including data transfer): {0} milli-sec",
                        stopwatch.ElapsedMilliseconds));
                }
            }

            if (homography != null)
            {
                //Rectangle rect = box.ROI;
                Rectangle rect = new Rectangle(Point.Empty, box.Size);
                PointF[] pts = new PointF[]
                {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)
                };

                PointF[] points = CvInvoke.PerspectiveTransform(pts, homography);
                //homography.ProjectPoints(points);

                //Merge the object image and the observed image into one big image for display
                Mat res = new Mat();
                CvInvoke.VConcat(box, observedImage, res);
                //Image<Gray, Byte> res = box.ConcateVertical(observedImage);

                for (int i = 0; i < points.Length; i++)
                    points[i].Y += box.Height;
                CvInvoke.Polylines(
                    res, 
                    Array.ConvertAll<PointF, Point>(points, Point.Round), 
                    true, 
                    new MCvScalar(255.0), 
                    5);
                //res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
                //ImageViewer.Show(res);
            }

        }

        public GpuMat[] OpticalFlowImage()
        {
            Mat[] images = AutoTestVarious.OpticalFlowImage();
            GpuMat[] gmats = new GpuMat[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                GpuMat g = new GpuMat();
                g.Upload(images[i]);
                images[i].Dispose();
                gmats[i] = g;
            }

            return gmats;

        }

        [Test]
        public void TestNVidiaOpticalFlow_1_0()
        {
            if (!CudaInvoke.HasCuda)
                return;
            int cudaDevice = CudaInvoke.GetDevice();
            using (CudaDeviceInfo deviceInfo = new CudaDeviceInfo(cudaDevice))
            {
                if (deviceInfo.CudaComputeCapability < new Version(7, 5))
                    return;
            }
            GpuMat[] images = OpticalFlowImage();
            GpuMat result = new GpuMat();

            Stopwatch watch = Stopwatch.StartNew();
            NvidiaOpticalFlow_1_0 flow = new NvidiaOpticalFlow_1_0(images[0].Size);

            flow.Calc(images[0], images[1], result);

            watch.Stop();
            EmguAssert.WriteLine(String.Format(
                "Time: {0} milliseconds",
                watch.ElapsedMilliseconds));
            for (int i = 0; i < images.Length; i++)
            {
                images[i].Dispose();
            }
        }

        [Test]
        public void TestNVidiaOpticalFlow_2_0()
        {
            if (!CudaInvoke.HasCuda)
                return;
            int cudaDevice = CudaInvoke.GetDevice();
            using (CudaDeviceInfo deviceInfo = new CudaDeviceInfo(cudaDevice))
            {
                if (deviceInfo.CudaComputeCapability < new Version(7, 5))
                    return;
            }
            GpuMat[] images = OpticalFlowImage();
            GpuMat flow = new GpuMat();
            GpuMat floatFlow = new GpuMat();
            Stopwatch watch = Stopwatch.StartNew();
            NvidiaOpticalFlow_2_0 nof = new NvidiaOpticalFlow_2_0(images[0].Size);

            nof.Calc(images[0], images[1], flow);
            nof.ConvertToFloat(flow, floatFlow);

            watch.Stop();
            EmguAssert.WriteLine(String.Format(
                "Time: {0} milliseconds",
                watch.ElapsedMilliseconds));
            for (int i = 0; i < images.Length; i++)
            {
                images[i].Dispose();
            }
        }

        [Test]
        public void TestCLAHE()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr))
            using (Mat result = new Mat())
            {
                for (int i = 0; i <= 1000; i++)
                {
                    using (CudaClahe clahe = new Emgu.CV.Cuda.CudaClahe(10, new System.Drawing.Size(8, 8)))
                    using (GpuMat gMat = new GpuMat())
                    using (GpuMat gMatLab = new GpuMat())
                    using (VectorOfGpuMat vectorOfGMats = new VectorOfGpuMat(3))
                    using (GpuMat gMatOut = new GpuMat())
                    {
                        gMat.Upload(image);
                        CudaInvoke.CvtColor(gMat, gMatLab, Emgu.CV.CvEnum.ColorConversion.Bgr2Lab, 0);
                        CudaInvoke.Split(gMatLab, vectorOfGMats);
                        clahe.Apply(vectorOfGMats[0], gMatOut);
                        gMatOut.CopyTo(vectorOfGMats[0]);
                        CudaInvoke.Merge(vectorOfGMats, gMatLab);
                        CudaInvoke.CvtColor(gMatLab, gMat, Emgu.CV.CvEnum.ColorConversion.Lab2Bgr, 0);
                        gMat.Download(result);
                    }
                }
            }
        }

        [Test]
        public void TestPerformanceConvlution()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Mat ImageMat1 = EmguAssert.LoadMat("lena.jpg", ImreadModes.Grayscale);
            ImageMat1.ConvertTo(ImageMat1, DepthType.Cv32F);
            GpuMat ImageGpuMa1 = new GpuMat();
            ImageGpuMa1.Upload(ImageMat1);
            filter2d(ImageMat1);
            cuda_convolve(ImageGpuMa1);
        }
        public Mat filter2d(Mat imgMat1)
        {
            //image dimensions are (w=784,h=565)
            Size s1 = new Size(imgMat1.Cols, imgMat1.Rows);
            Mat dst = new Mat(s1, DepthType.Cv32F, 1);
            Point o = new Point(-1, -1);
            Mat kernel = (Mat.Ones(3, 3, DepthType.Cv32F, 1)) / 9;
            int iteration = 1000;//p is iteration for running each function

            CvInvoke.Filter2D(imgMat1, dst, kernel, o);

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            for (int i = 0; i < iteration; i++)
            {
                CvInvoke.Filter2D(imgMat1, dst, kernel, o);
            }
            stopwatch3.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch3.ElapsedMilliseconds);

            return imgMat1;
        }
        public GpuMat cuda_convolve(GpuMat imgGPUMat1)
        {
            //image dimensions are (w=784,h=565)
            Mat kernel = (Mat.Ones(3, 3, DepthType.Cv32F, 1)) / 9;
            GpuMat gKernel = new GpuMat();
            gKernel.Upload(kernel);
            GpuMat imgGPUMat3 = new GpuMat();
            CudaConvolution l = new CudaConvolution(imgGPUMat1.Size);
            int iteration = 1000;//p is iteration for running each function
            l.Convolve(imgGPUMat1, kernel, imgGPUMat3, false);

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            for (int i = 0; i < iteration; i++)
            {
                l.Convolve(imgGPUMat1, kernel, imgGPUMat3, false);
            }
            stopwatch3.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch3.ElapsedMilliseconds);

            return imgGPUMat3;
        }
    }
}
