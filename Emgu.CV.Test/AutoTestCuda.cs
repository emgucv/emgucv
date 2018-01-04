//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;
using System.Runtime.InteropServices;

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
            if (CudaInvoke.HasCuda)
            {
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
        }

        [Test]
        public void TestGpuMatContinuous()
        {
            if (!CudaInvoke.HasCuda)
                return;
            GpuMat<Byte> mat = new GpuMat<byte>(1200, 640, 1, true);
            Assert.IsTrue(mat.IsContinuous);
        }

        [Test]
        public void TestGpuMatRange()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
                img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                using (GpuMat gpuImg1 = new GpuMat(img1))
                using (GpuMat mat = new GpuMat(gpuImg1, new Range(0, 1), Range.All))
                {
                    Size s = mat.Size;
                }
            }
        }

        [Test]
        public void TestGpuMatAdd()
        {
            if (CudaInvoke.HasCuda)
            {
                int repeat = 1000;
                Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
                Image<Gray, Byte> img2 = new Image<Gray, byte>(img1.Size);
                img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                img2.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                Image<Gray, Byte> cpuImgSum = new Image<Gray, byte>(img1.Size);
                Stopwatch watch = Stopwatch.StartNew();
                for (int i = 0; i < repeat; i++)
                    CvInvoke.Add(img1, img2, cpuImgSum, null, CvEnum.DepthType.Cv8U);
                watch.Stop();
                Trace.WriteLine(String.Format("CPU processing time: {0}ms", (double)watch.ElapsedMilliseconds / repeat));

                watch.Reset(); watch.Start();
                CudaImage<Gray, Byte> gpuImg1 = new CudaImage<Gray, byte>(img1);
                CudaImage<Gray, Byte> gpuImg2 = new CudaImage<Gray, byte>(img2);
                CudaImage<Gray, Byte> gpuImgSum = new CudaImage<Gray, byte>(gpuImg1.Size);
                Stopwatch watch2 = Stopwatch.StartNew();
                for (int i = 0; i < repeat; i++)
                    CudaInvoke.Add(gpuImg1, gpuImg2, gpuImgSum);
                watch2.Stop();
                Image<Gray, Byte> cpuImgSumFromGpu = gpuImgSum.ToImage();
                watch.Stop();
                Trace.WriteLine(String.Format("Core GPU processing time: {0}ms", (double)watch2.ElapsedMilliseconds / repeat));
                //Trace.WriteLine(String.Format("Total GPU processing time: {0}ms", (double)watch.ElapsedMilliseconds/repeat));

                Assert.IsTrue(cpuImgSum.Equals(cpuImgSumFromGpu));
            }
        }

        [Test]
        public void TestSplitMerge()
        {
            if (CudaInvoke.HasCuda)
            {
                using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
                {
                    img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

                    using (GpuMat gpuImg1 = new GpuMat(img1))
                    {
                        GpuMat[] channels = gpuImg1.Split(null);

                        for (int i = 0; i < channels.Length; i++)
                        {
                            Mat imgL = channels[i].ToMat();
                            Image<Gray, Byte> imgR = img1[i];
                            Assert.IsTrue(imgL.Equals(imgR.Mat), "failed split GpuMat");
                        }

                        using (GpuMat gpuImg2 = new GpuMat())
                        {
                            gpuImg2.MergeFrom(channels, null);
                            using (Image<Bgr, byte> img2 = new Image<Bgr, byte>(img1.Size))
                            {
                                gpuImg2.Download(img2);
                                Assert.IsTrue(img2.Equals(img1), "failed split and merge test");
                            }
                        }

                        for (int i = 0; i < channels.Length; i++)
                        {
                            channels[i].Dispose();
                        }
                    }
                }
            }
        }

        [Test]
        public void TestCudaFlip()
        {
            if (CudaInvoke.HasCuda)
            {
                using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
                {
                    img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                    using (Image<Bgr, Byte> img1Flip = img1.Flip(CvEnum.FlipType.Horizontal | CvEnum.FlipType.Vertical))
                    using (CudaImage<Bgr, Byte> cudaImage = new CudaImage<Bgr, byte>(img1))
                    using (CudaImage<Bgr, Byte> cudaFlip = new CudaImage<Bgr, byte>(img1.Size))
                    {
                        CudaInvoke.Flip(cudaImage, cudaFlip, CvEnum.FlipType.Horizontal | CvEnum.FlipType.Vertical, null);
                        cudaFlip.Download(img1);
                        Assert.IsTrue(img1.Equals(img1Flip));
                    }
                }
            }
        }

        [Test]
        public void TestConvolutionAndLaplace()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
                image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

                float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
                using (ConvolutionKernelF kernel = new ConvolutionKernelF(k))
                using (Stream s = new Stream())
                using (GpuMat cudaImg = new GpuMat(image))
                using (GpuMat cudaLaplace = new GpuMat())
                using (CudaLinearFilter cudaLinear = new CudaLinearFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, kernel, kernel.Center))
                using (GpuMat cudaConv = new GpuMat())
                using (CudaLaplacianFilter laplacian = new CudaLaplacianFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, 1, 1.0))
                {
                    cudaLinear.Apply(cudaImg, cudaConv, s);
                    laplacian.Apply(cudaImg, cudaLaplace, s);
                    s.WaitForCompletion();
                    Assert.IsTrue(cudaLaplace.Equals(cudaConv));
                }
            }
        }

        [Test]
        public void TestCudaFilters()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
                image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));
                using (GpuMat cudaImg1 = new GpuMat(image))
                using (GpuMat cudaImg2 = new GpuMat())
                using (CudaGaussianFilter gaussian = new CudaGaussianFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, new Size(5, 5), 0, 0, CvEnum.BorderType.Default, CvEnum.BorderType.Default))
                using (CudaSobelFilter sobel = new CudaSobelFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, 1, 1, 3, 1.0, CvEnum.BorderType.Default, CvEnum.BorderType.Default))
                {
                    gaussian.Apply(cudaImg1, cudaImg2, null);
                    sobel.Apply(cudaImg1, cudaImg2, null);

                }
            }
        }

        [Test]
        public void TestResizeGray()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
                img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

                //Image<Gray, Byte> img = new Image<Gray, byte>("airplane.jpg");

                Image<Gray, Byte> small = img.Resize(100, 200, Emgu.CV.CvEnum.Inter.Linear);
                CudaImage<Gray, Byte> gpuImg = new CudaImage<Gray, byte>(img);
                CudaImage<Gray, byte> smallGpuImg = new CudaImage<Gray, byte>(small.Size);
                CudaInvoke.Resize(gpuImg, smallGpuImg, small.Size);
                Image<Gray, Byte> diff = smallGpuImg.ToImage().AbsDiff(small);
                //ImageViewer.Show(smallGpuImg.ToImage());
                //ImageViewer.Show(small);
                //Assert.IsTrue(smallGpuImg.ToImage().Equals(small));
            }
        }

        [Test]
        public void TestClone()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
                img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

                using (CudaImage<Gray, Byte> gImg1 = new CudaImage<Gray, byte>(img))
                using (CudaImage<Gray, Byte> gImg2 = gImg1.Clone(null))
                using (Image<Gray, Byte> img2 = gImg2.ToImage())
                {
                    Assert.IsTrue(img.Equals(img2));
                }
            }
        }

        [Test]
        public void TestColorConvert()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
                img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
                Image<Gray, Byte> imgGray = img.Convert<Gray, Byte>();
                Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>();

                CudaImage<Bgr, Byte> gpuImg = new CudaImage<Bgr, Byte>(img);
                CudaImage<Gray, Byte> gpuImgGray = gpuImg.Convert<Gray, Byte>();
                CudaImage<Hsv, Byte> gpuImgHsv = gpuImg.Convert<Hsv, Byte>();

                Assert.IsTrue(gpuImgGray.Equals(new CudaImage<Gray, Byte>(imgGray)));
                Assert.IsTrue(gpuImgHsv.ToImage().Equals(imgHsv));
                Assert.IsTrue(gpuImgHsv.Equals(new CudaImage<Hsv, Byte>(imgHsv)));
            }
        }

        [Test]
        public void TestInplaceNot()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
                CudaImage<Bgr, Byte> gpuMat = new CudaImage<Bgr, byte>(img);
                CudaInvoke.BitwiseNot(gpuMat, gpuMat, null, null);
                Assert.IsTrue(gpuMat.Equals(new CudaImage<Bgr, Byte>(img.Not())));
            }
        }


        [Test]
        public void TestResizeBgr()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
                //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));

                Size size = new Size(100, 200);

                CudaImage<Bgr, Byte> cudaImg = new CudaImage<Bgr, byte>(img);
                CudaImage<Bgr, byte> smallCudaImg = new CudaImage<Bgr, byte>(size);

                CudaInvoke.Resize(cudaImg, smallCudaImg, size);
                Image<Bgr, Byte> smallCpuImg = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.Inter.Linear);


                Image<Bgr, Byte> diff = smallCudaImg.ToImage().AbsDiff(smallCpuImg);
                //TODO: Check why they are not an exact match
                //Assert.IsTrue(diff.CountNonzero()[0] == 0);
                //ImageViewer.Show(smallGpuImg.ToImage());
                //ImageViewer.Show(small);
            }
        }


        [Test]
        public void TestCanny()
        {
            if (CudaInvoke.HasCuda)
            {
                using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
                using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(image))
                using (CudaImage<Gray, Byte> gray = CudaImage.Convert<Gray, Byte>())
                using (CudaImage<Gray, Byte> canny = new CudaImage<Gray, byte>(gray.Size))
                using (CudaCannyEdgeDetector detector = new CudaCannyEdgeDetector(20, 100, 3, false))
                {
                    detector.Detect(gray, canny);
                    //GpuInvoke.Canny(gray, canny, 20, 100, 3, false);
                    //ImageViewer.Show(canny);
                }
            }
        }

        [Test]
        public void TestCountNonZero()
        {
            if (!CudaInvoke.HasCuda)
                return;

            //Mat m = new Mat(100, 200, Mat.Depth.Cv8U, 1);
            CudaImage<Gray, Byte> m = new CudaImage<Gray, Byte>(100, 200);
            m.SetTo(new MCvScalar(), null, null);
            EmguAssert.IsTrue(0 == CudaInvoke.CountNonZero(m));
            //Trace.WriteLine(String.Format("non zero count: {0}", ));
        }


        [Test]
        public void TestClahe()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("pedestrian.png");
                CudaImage<Gray, Byte> cudaImage = new CudaImage<Gray, byte>(image);
                CudaImage<Gray, Byte> cudaResult = new CudaImage<Gray, byte>(cudaImage.Size);

                using (CudaClahe clahe = new CudaClahe(40.0, new Size(8, 8)))
                {
                    Image<Gray, Byte> result = new Image<Gray, byte>(cudaResult.Size);
                    clahe.Apply(cudaImage, cudaResult, null);
                    cudaResult.Download(result);
                    //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
                }
            }
        }

        [Test]
        public void TestHOG1()
        {
            if (CudaInvoke.HasCuda)
            {
                using (CudaHOG hog = new CudaHOG(new Size(64, 128), new Size(16, 16), new Size(8, 8), new Size(8, 8), 9))
                using (Mat pedestrianDescriptor = hog.GetDefaultPeopleDetector())
                using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
                {
                    hog.SetSVMDetector(pedestrianDescriptor);
                    //hog.GroupThreshold = 0;
                    Stopwatch watch = Stopwatch.StartNew();
                    Rectangle[] rects;
                    using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(image))
                    using (CudaImage<Bgra, Byte> gpuBgra = CudaImage.Convert<Bgra, Byte>())
                    using (VectorOfRect vRect = new VectorOfRect())
                    {
                        hog.DetectMultiScale(gpuBgra, vRect);
                        rects = vRect.ToArray();
                    }
                    watch.Stop();

                    Assert.AreEqual(1, rects.Length);

                    foreach (Rectangle rect in rects)
                        image.Draw(rect, new Bgr(Color.Red), 1);
                    Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                    //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
                }
            }
        }

        [Test]
        public void TestHOG2()
        {
            if (CudaInvoke.HasCuda)
            {
                using (CudaHOG hog = new CudaHOG(
                   new Size(48, 96), //winSize
                   new Size(16, 16), //blockSize
                   new Size(8, 8), //blockStride
                   new Size(8, 8)  //cellSize
                   ))
                using (Mat pedestrianDescriptor = hog.GetDefaultPeopleDetector())
                using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
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
                        image.Draw(rect, new Bgr(Color.Red), 1);
                    Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                    //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
                }
            }
        }

        [Test]
        public void TestCudaReduce()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 320))
            {
                img.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
                using (GpuMat cudaImage = new GpuMat(img))
                using (GpuMat reduced = new GpuMat())
                {
                    CudaInvoke.Reduce(cudaImage, reduced, CvEnum.ReduceDimension.SingleRow, CvEnum.ReduceType.ReduceAvg);
                }
            }
        }

        [Test]
        public void TestErodeDilate()
        {
            if (!CudaInvoke.HasCuda)
                return;

            int morphIter = 2;
            Image<Gray, Byte> image = new Image<Gray, byte>(640, 320);
            image.Draw(new CircleF(new PointF(200, 200), 30), new Gray(255.0), 4);

            Size ksize = new Size(morphIter * 2 + 1, morphIter * 2 + 1);

            using (GpuMat cudaImage = new GpuMat(image))
            using (GpuMat cudaTemp = new GpuMat())
            using (Stream stream = new Stream())
            using (CudaBoxMaxFilter dilate = new CudaBoxMaxFilter(DepthType.Cv8U, 1, ksize, new Point(-1, -1), CvEnum.BorderType.Default, new MCvScalar()))
            using (CudaBoxMinFilter erode = new CudaBoxMinFilter(DepthType.Cv8U, 1, ksize, new Point(-1, -1), CvEnum.BorderType.Default, new MCvScalar()))
            {
                //run the GPU version asyncrhonously with stream
                erode.Apply(cudaImage, cudaTemp, stream);
                dilate.Apply(cudaTemp, cudaImage, stream);

                //run the CPU version in parallel to the gpu version.
                using (Image<Gray, Byte> temp = new Image<Gray, byte>(image.Size))
                {
                    CvInvoke.Erode(image, temp, null, new Point(-1, -1), morphIter, CvEnum.BorderType.Constant, new MCvScalar());
                    CvInvoke.Dilate(temp, image, null, new Point(-1, -1), morphIter, CvEnum.BorderType.Constant, new MCvScalar());
                }

                //syncrhonize with the GPU version
                stream.WaitForCompletion();

                Assert.IsTrue(cudaImage.ToMat().Equals(image.Mat));
            }

        }

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

        [Test]
        public void TestCudaFASTDetector()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>("box.png"))
            using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(img))
            using (CudaImage<Gray, Byte> grayCudaImage = CudaImage.Convert<Gray, Byte>())
            using (CudaFastFeatureDetector featureDetector = new CudaFastFeatureDetector(10, true, FastDetector.DetectorType.Type9_16, 1000))
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            using (GpuMat keyPointsMat = new GpuMat())
            {
                featureDetector.DetectAsync(grayCudaImage, keyPointsMat);
                featureDetector.Convert(keyPointsMat, kpts);
                //featureDetector.DetectKeyPointsRaw(grayCudaImage, null, keyPointsMat);

                //featureDetector.DownloadKeypoints(keyPointsMat, kpts);

                foreach (MKeyPoint kpt in kpts.ToArray())
                {
                    img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
                }

                //ImageViewer.Show(img);
            }
        }

        [Test]
        public void TestCudaOrbDetector()
        {
            if (!CudaInvoke.HasCuda)
                return;
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>("box.png"))
            using (GpuMat cudaImage = new GpuMat(img))
            using (GpuMat grayCudaImage = new GpuMat())
            using (CudaORBDetector detector = new CudaORBDetector(500))
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            using (GpuMat keyPointMat = new GpuMat())
            using (GpuMat descriptorMat = new GpuMat())
            {
                CudaInvoke.CvtColor(cudaImage, grayCudaImage, ColorConversion.Bgr2Gray);
                detector.DetectAsync(grayCudaImage, keyPointMat);
                detector.Convert(keyPointMat, kpts);
                //detector.ComputeRaw(grayCudaImage, null, keyPointMat, descriptorMat);
                //detector.DownloadKeypoints(keyPointMat, kpts);

                foreach (MKeyPoint kpt in kpts.ToArray())
                {
                    img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
                }

                //ImageViewer.Show(img);
            }
        }

        [Test]
        public void TestCudaPyr()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Image<Gray, Byte> img = new Image<Gray, byte>(640, 480);
            img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            Image<Gray, Byte> down = img.PyrDown();
            Image<Gray, Byte> up = down.PyrUp();

            CudaImage<Gray, Byte> gImg = new CudaImage<Gray, byte>(img);
            CudaImage<Gray, Byte> gDown = new CudaImage<Gray, byte>(img.Size.Width >> 1, img.Size.Height >> 1);
            CudaImage<Gray, Byte> gUp = new CudaImage<Gray, byte>(img.Size);
            CudaInvoke.PyrDown(gImg, gDown, null);
            CudaInvoke.PyrUp(gDown, gUp, null);

            CvInvoke.AbsDiff(down, gDown.ToImage(), down);
            CvInvoke.AbsDiff(up, gUp.ToImage(), up);
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
            Image<Bgr, Byte> randomObj = new Image<Bgr, byte>(templWidth, templHeight);
            randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

            //Draw the object in image1 center at templCenter;
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
            Rectangle objectLocation = new Rectangle(templCenter.X - (templWidth >> 1), templCenter.Y - (templHeight >> 1), templWidth, templHeight);
            img.ROI = objectLocation;
            randomObj.Copy(img, null);
            img.ROI = Rectangle.Empty;
            #endregion

            Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TemplateMatchingType.Sqdiff);
            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;
            match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            double gpuMinVal = 0, gpuMaxVal = 0;
            Point gpuMinLoc = Point.Empty, gpuMaxLoc = Point.Empty;
            GpuMat cudaImage = new GpuMat(img);
            GpuMat gpuRandomObj = new GpuMat(randomObj);
            GpuMat gpuMatch = new GpuMat();
            using (CudaTemplateMatching buffer = new CudaTemplateMatching(DepthType.Cv8U, 3, CvEnum.TemplateMatchingType.Sqdiff))
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
            Image<Gray, float> xmap = new Image<Gray, float>(2, 2);
            xmap.Data[0, 0, 0] = 0; xmap.Data[0, 1, 0] = 0;
            xmap.Data[1, 0, 0] = 1; xmap.Data[1, 1, 0] = 1;
            Image<Gray, float> ymap = new Image<Gray, float>(2, 2);
            ymap.Data[0, 0, 0] = 0; ymap.Data[0, 1, 0] = 1;
            ymap.Data[1, 0, 0] = 0; ymap.Data[1, 1, 0] = 1;

            Image<Gray, Byte> image = new Image<Gray, byte>(2, 2);
            image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

            using (CudaImage<Gray, Byte> CudaImage = new CudaImage<Gray, byte>(image))
            using (CudaImage<Gray, float> xCudaImage = new CudaImage<Gray, float>(xmap))
            using (CudaImage<Gray, float> yCudaImage = new CudaImage<Gray, float>(ymap))
            using (CudaImage<Gray, Byte> remapedImage = new CudaImage<Gray, byte>(CudaImage.Size))
            {
                CudaInvoke.Remap(CudaImage, remapedImage, xCudaImage, yCudaImage, CvEnum.Inter.Cubic, CvEnum.BorderType.Default, new MCvScalar(), null);
            }
        }

        [Test]
        public void TestCudaWarpPerspective()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Matrix<float> transformation = new Matrix<float>(3, 3);
            transformation.SetIdentity();

            Image<Gray, byte> image = new Image<Gray, byte>(480, 320);
            image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

            using (GpuMat cudaImage = new GpuMat(image))
            using (CudaImage<Gray, Byte> resultCudaImage = new CudaImage<Gray, byte>())
            {
                CudaInvoke.WarpPerspective(cudaImage, resultCudaImage, transformation, cudaImage.Size, CvEnum.Inter.Cubic, CvEnum.BorderType.Default, new MCvScalar(), null);
            }
        }

        [Test]
        public void TestCudaPyrLKOpticalFlow()
        {
            if (!CudaInvoke.HasCuda)
                return;
            Image<Gray, Byte> prevImg, currImg;
            AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
            Mat flow = new Mat();
            CudaDensePyrLKOpticalFlow opticalflow = new CudaDensePyrLKOpticalFlow(new Size(21, 21), 3, 30, false);
            using (CudaImage<Gray, Byte> prevGpu = new CudaImage<Gray, byte>(prevImg))
            using (CudaImage<Gray, byte> currGpu = new CudaImage<Gray, byte>(currImg))
            using (GpuMat flowGpu = new GpuMat())
            {
                opticalflow.Calc(prevGpu, currGpu, flowGpu);

                flowGpu.Download(flow);
            }
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
            Image<Gray, Byte> prevImg, currImg;
            AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
            Mat flow = new Mat();
            CudaBroxOpticalFlow opticalflow = new CudaBroxOpticalFlow();
            using (CudaImage<Gray, float> prevGpu = new CudaImage<Gray, float>(prevImg.Convert<Gray, float>()))
            using (CudaImage<Gray, float> currGpu = new CudaImage<Gray, float>(currImg.Convert<Gray, float>()))
            using (GpuMat flowGpu = new GpuMat())
            {
                opticalflow.Calc(prevGpu, currGpu, flowGpu);

                flowGpu.Download(flow);
            }
        }


        [Test]
        public void TestBilaterialFilter()
        {

            if (CudaInvoke.HasCuda)
            {
                Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
                Image<Gray, byte> gray = img.Convert<Gray, Byte>();
                CudaImage<Gray, Byte> CudaImage = new CudaImage<Gray, byte>(gray);

                CudaImage<Gray, Byte> gpuBilaterial = new CudaImage<Gray, byte>(CudaImage.Size);
                CudaInvoke.BilateralFilter(CudaImage, gpuBilaterial, 7, 5, 5, CvEnum.BorderType.Default, null);

                //Emgu.CV.UI.ImageViewer.Show(gray.ConcateHorizontal(gpuBilaterial.ToImage()));
            }
        }

        /*
        [Test]
        public void TestCudaVideoReadWrite()
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
        }*/

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
            if (CudaInvoke.HasCuda)
            {
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
        }

        [Test]
        public void TestHughCircle()
        {
            if (CudaInvoke.HasCuda)
            {
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
                    Emgu.CV.Util.CvToolbox.Memcpy(circlesHandle.AddrOfPinnedObject(), circlesMat.DataPointer, Marshal.SizeOf(typeof(CircleF)) * circles.Length);
                    circlesHandle.Free();
                    foreach (var circle in circles)
                    {
                        CvInvoke.Circle(m, Point.Round(circle.Center), (int)circle.Radius, new MCvScalar(255));
                    }
                }
            }
        }

        [Test]
        public void TestCudaHoughCircle()
        {
            if (CudaInvoke.HasCuda)
            {
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

        }

        [Test]
        public void TestBruteForceHammingDistance()
        {
            if (CudaInvoke.HasCuda)
            {
                Image<Gray, byte> box = new Image<Gray, byte>("box.png");
                FastDetector fast = new FastDetector(100, true);
                BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);

                #region extract features from the object image
                Stopwatch stopwatch = Stopwatch.StartNew();
                VectorOfKeyPoint modelKeypoints = new VectorOfKeyPoint();
                fast.DetectRaw(box, modelKeypoints);
                Mat modelDescriptors = new Mat();
                brief.Compute(box, modelKeypoints, modelDescriptors);
                stopwatch.Stop();
                Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
                #endregion

                Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

                #region extract features from the observed image
                stopwatch.Reset(); stopwatch.Start();
                VectorOfKeyPoint observedKeypoints = new VectorOfKeyPoint();
                fast.DetectRaw(observedImage, observedKeypoints);
                Mat observedDescriptors = new Mat();
                brief.Compute(observedImage, observedKeypoints, observedDescriptors);
                stopwatch.Stop();
                Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
                #endregion

                Mat homography = null;
                using (GpuMat<Byte> gpuModelDescriptors = new GpuMat<byte>(modelDescriptors)) //initialization of GPU code might took longer time.
                {
                    stopwatch.Reset(); stopwatch.Start();
                    CudaBFMatcher hammingMatcher = new CudaBFMatcher(DistanceType.Hamming);

                    //BFMatcher hammingMatcher = new BFMatcher(BFMatcher.DistanceType.Hamming, modelDescriptors);
                    int k = 2;
                    Matrix<int> trainIdx = new Matrix<int>(observedKeypoints.Size, k);
                    Matrix<float> distance = new Matrix<float>(trainIdx.Size);

                    using (GpuMat<Byte> gpuObservedDescriptors = new GpuMat<byte>(observedDescriptors))
                    //using (GpuMat<int> gpuTrainIdx = new GpuMat<int>(trainIdx.Rows, trainIdx.Cols, 1, true))
                    //using (GpuMat<float> gpuDistance = new GpuMat<float>(distance.Rows, distance.Cols, 1, true))
                    using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                    {
                        Stopwatch w2 = Stopwatch.StartNew();
                        //hammingMatcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k);
                        hammingMatcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k, null, true);
                        w2.Stop();
                        Trace.WriteLine(String.Format("Time for feature matching (excluding data transfer): {0} milli-sec",
                           w2.ElapsedMilliseconds));
                        //gpuTrainIdx.Download(trainIdx);
                        //gpuDistance.Download(distance);


                        Mat mask = new Mat(distance.Rows, 1, DepthType.Cv8U, 1);
                        mask.SetTo(new MCvScalar(255));
                        Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

                        int nonZeroCount = CvInvoke.CountNonZero(mask);
                        if (nonZeroCount >= 4)
                        {
                            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeypoints, observedKeypoints,
                               matches, mask, 1.5, 20);
                            if (nonZeroCount >= 4)
                                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints,
                                   observedKeypoints, matches, mask, 2);
                            nonZeroCount = CvInvoke.CountNonZero(mask);
                        }

                        stopwatch.Stop();
                        Trace.WriteLine(String.Format("Time for feature matching (including data transfer): {0} milli-sec",
                           stopwatch.ElapsedMilliseconds));
                    }
                }

                if (homography != null)
                {
                    Rectangle rect = box.ROI;
                    PointF[] pts = new PointF[] {
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};

                    PointF[] points = CvInvoke.PerspectiveTransform(pts, homography);
                    //homography.ProjectPoints(points);

                    //Merge the object image and the observed image into one big image for display
                    Image<Gray, Byte> res = box.ConcateVertical(observedImage);

                    for (int i = 0; i < points.Length; i++)
                        points[i].Y += box.Height;
                    res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
                    //ImageViewer.Show(res);
                }
            }
        }
    }
}
