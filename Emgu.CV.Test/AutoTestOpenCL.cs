//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.OpenCL;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.Nonfree;
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestOpenCL
   {
      [Test]
      public void TestOclInfo()
      {
         using (VectorOfOclPlatformInfo oclPlatformInfos = OclInvoke.GetPlatforms())
         {
            if (oclPlatformInfos.Size > 0)
            {
               for (int i = 0; i < oclPlatformInfos.Size; i++)
               {
                  OclPlatformInfo platformInfo = oclPlatformInfos[i];
                  String platformName = platformInfo.Name;
                  Trace.WriteLine(String.Format("Platform {0}: {1}", i, platformName));

                  VectorOfOclDeviceInfo devices = platformInfo.Devices;
                  for (int j = 0; j < devices.Size; j++)
                  {
                     OclDeviceInfo device = devices[j];
                     Trace.WriteLine(String.Format("   Device {0}: {1}", j, device.Name));
                  }
               }
            }
            Trace.WriteLine("count = " + oclPlatformInfos.Size);
         }
      }

      
      [Test]
      public void TestOclMatAdd()
      {
         if (OclInvoke.HasOpenCL)
         {
            int repeat = 1000;
            Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
            Image<Gray, Byte> img2 = new Image<Gray, byte>(img1.Size);
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            img2.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            Image<Gray, Byte> cpuImgSum = new Image<Gray, byte>(img1.Size);
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
               CvInvoke.cvAdd(img1, img2, cpuImgSum, IntPtr.Zero);
            watch.Stop();
            Trace.WriteLine(String.Format("CPU processing time: {0}ms", (double)watch.ElapsedMilliseconds / repeat));

            watch.Reset(); 

            Stopwatch watch2 = new Stopwatch();

            using (VectorOfOclPlatformInfo oclInfoVec = OclInvoke.GetPlatforms())
            {
               for (int j = 0; j < oclInfoVec.Size; j++)
               {
                  OclPlatformInfo platform = oclInfoVec[j];
                  VectorOfOclDeviceInfo devices = platform.Devices;
                  for (int k = 0; k < devices.Size; k++)
                  {
                     OclDeviceInfo device = devices[k];
                     OclInvoke.SetDevice(device);

                     OclImage<Gray, Byte> gpuImg1 = new OclImage<Gray, byte>(img1);
                     OclImage<Gray, Byte> gpuImg2 = new OclImage<Gray, byte>(img2);
                     OclImage<Gray, Byte> gpuImgSum = new OclImage<Gray, byte>(gpuImg1.Size);

                     //This trigger the compilation of OCL code, should be run outside of performance testing
                     OclInvoke.Add(gpuImg1, gpuImg2, gpuImgSum, IntPtr.Zero);

                     watch2.Start();
                     for (int i = 0; i < repeat; i++)
                     {
                        OclInvoke.Add(gpuImg1, gpuImg2, gpuImgSum, IntPtr.Zero);
                     }
                     watch2.Stop();
                     Trace.WriteLine(String.Format("OpenCL platform: {0}; device: {1}; processing time: {2}ms", platform.Name, device.Name, (double)watch2.ElapsedMilliseconds / repeat));

                     Image<Gray, Byte> cpuImgSumFromGpu = gpuImgSum.ToImage();
                     Assert.IsTrue(cpuImgSum.Equals(cpuImgSumFromGpu));
                     OclInvoke.Finish();
                  }
               }
            }
           
            
            //Trace.WriteLine(String.Format("Total GPU processing time: {0}ms", (double)watch.ElapsedMilliseconds/repeat));

            
         }
      }

      [Test]
      public void TestSplitMerge()
      {
         
         if (OclInvoke.HasOpenCL)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

               using (OclImage<Bgr, Byte> gpuImg1 = new OclImage<Bgr, byte>(img1))
               {
                  OclImage<Gray, Byte>[] channels = gpuImg1.Split();

                  for (int i = 0; i < channels.Length; i++)
                  {
                     Image<Gray, Byte> imgL = channels[i].ToImage();
                     Image<Gray, Byte> imgR = img1[i];
                     Assert.IsTrue(imgL.Equals(imgR), "failed split GpuMat");
                  }

                  using (OclImage<Bgr, Byte> gpuImg2 = new OclImage<Bgr, byte>(channels[0].Size))
                  {
                     gpuImg2.MergeFrom(channels);
                     Assert.IsTrue(gpuImg2.ToImage().Equals(img1), "failed split and merge test");
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
      public void TestOclFlip()
      {
         if (OclInvoke.HasOpenCL)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
               using (Image<Bgr, Byte> img1Flip = img1.Flip(CvEnum.FLIP.HORIZONTAL | CvEnum.FLIP.VERTICAL))
               using (OclImage<Bgr, Byte> gpuImg1 = new OclImage<Bgr, byte>(img1))
               using (OclImage<Bgr, Byte> gpuFlip = new OclImage<Bgr, byte>(img1.Size))
               {
                  OclInvoke.Flip(gpuImg1, gpuFlip, CvEnum.FLIP.HORIZONTAL | CvEnum.FLIP.VERTICAL);
                  gpuFlip.Download(img1);
                  Assert.IsTrue(img1.Equals(img1Flip));
               }
            }
         }
      }

      [Test]
      public void TestConvolutionAndLaplace()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));
            int nonZero = image.CountNonzero()[0];
            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            using (ConvolutionKernelF kernel = new ConvolutionKernelF(k))
            using (OclImage<Gray, Byte> gpuImg1 = new OclImage<Gray, byte>(image))
            using (OclImage<Gray, Byte> gpuLaplace = new OclImage<Gray, Byte>(image.Size))
            using (OclImage<Gray, Byte> gpuConv = gpuImg1.Convolution(kernel))
            {
               int gpuNonZero = OclInvoke.CountNonZero(gpuImg1);
               Assert.IsTrue(nonZero == gpuNonZero);
               OclInvoke.Laplacian(gpuImg1, gpuLaplace, 1, 1.0);
               int laplaceNonZero0 = OclInvoke.CountNonZero(gpuLaplace);
               int convNonZero0 = OclInvoke.CountNonZero(gpuConv);
               
               Image<Gray, Byte> imgLaplace = new Image<Gray, Byte>(gpuLaplace.Size);
               
               gpuLaplace.Download(imgLaplace);
               Image<Gray, Byte> imgConv = new Image<Gray, Byte>(gpuConv.Size);
               gpuConv.Download(imgConv);

               int laplaceNonZero1 = imgLaplace.CountNonzero()[0];
               int convNonZero1 = imgConv.CountNonzero()[0];
               Assert.IsTrue(laplaceNonZero0 == laplaceNonZero1);
               Assert.IsTrue(convNonZero0 == convNonZero1);
               //Assert.IsTrue(gpuLaplace.Equals(gpuConv));
            }

         }
      }

      [Test]
      public void TestHughCircle()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("board.jpg");
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();
            OclImage<Gray, Byte> oclImage = new OclImage<Gray, byte>(gray);

            CircleF[] circles = OclInvoke.HoughCircles(oclImage, 1, 10, 100, 50, 1, 30, 1000);

            OclMat<float> oclCircles = new OclMat<float>();
            
            foreach (CircleF c in circles)
            {
               img.Draw(c, new Bgr(Color.Red), 1);
            }

            //Emgu.CV.UI.ImageViewer.Show(img);
         }
      }

      [Test]
      public void TestBilaterialFilter()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();
            OclImage<Gray, Byte> oclImage = new OclImage<Gray, byte>(gray);
            //oclImage.SetTo(new MCvScalar(255, 255, 255, 255), null);
            OclImage<Gray, Byte> oclBilaterial = new OclImage<Gray, byte>(oclImage.Size);
            OclInvoke.BilateralFilter(oclImage, oclBilaterial, 5, 5, 5, CvEnum.BORDER_TYPE.DEFAULT);

            //Emgu.CV.UI.ImageViewer.Show(gray.ConcateHorizontal(oclBilaterial.ToImage()));
         }
      }

      [Test]
      public void TestClahe()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("pedestrian.png");
            OclImage<Gray, Byte> oclImage = new OclImage<Gray, byte>(image);
            OclImage<Gray, Byte> oclResult = new OclImage<Gray, byte>(oclImage.Size);
            Image<Gray, Byte> result = new Image<Gray, byte>(oclResult.Size);
            OclInvoke.CLAHE(oclImage, oclResult, 4, new Size(8, 8));
            oclResult.Download(result);
            Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
         }
      }

      [Test]
      public void TestClone()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            using (OclImage<Gray, Byte> gImg1 = new OclImage<Gray, byte>(img))
            using (OclImage<Gray, Byte> gImg2 = gImg1.Clone())
            using (Image<Gray, Byte> img2 = gImg2.ToImage())
            {
               Assert.IsTrue(img.Equals(img2));
            }
         }
      }

      [Test]
      public void TestColorConvert()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            Image<Gray, Byte> imgGray = img.Convert<Gray, Byte>();
            Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>();

            OclImage<Bgr, Byte> gpuImg = new OclImage<Bgr, Byte>(img);
            OclImage<Gray, Byte> gpuImgGray = gpuImg.Convert<Gray, Byte>();
            //OclImage<Hsv, Byte> gpuImgHsv = gpuImg.Convert<Hsv, Byte>();

            Assert.IsTrue(gpuImgGray.Equals(new OclImage<Gray, Byte>(imgGray)));
            //Assert.IsTrue(gpuImgHsv.ToImage().Equals(imgHsv));
            //Assert.IsTrue(gpuImgHsv.Equals(new OclImage<Hsv, Byte>(imgHsv)));
         }
      }

      [Test]
      public void TestInplaceNot()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            OclImage<Bgr, Byte> gpuMat = new OclImage<Bgr, byte>(img);
            Assert.IsTrue(OclInvoke.CountNonZero(gpuMat) == 0);
            OclInvoke.BitwiseNot(gpuMat, gpuMat);
            //int nonZero = OclInvoke.CountNonZero(gpuMat);
            //Assert.IsTrue(nonZero == gpuMat.Size.Width * gpuMat.Size.Height);
            Image<Bgr, Byte> imgNot = new Image<Bgr, byte>(img.Size);
            gpuMat.Download(imgNot);


            Assert.IsTrue(gpuMat.Equals(new OclImage<Bgr, Byte>(img.Not())));
         }
      }

      [Test]
      public void TestResizeBgr()
      {
         if (OclInvoke.HasOpenCL)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
            //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));

            Size size = new Size(100, 200);

            OclImage<Bgr, Byte> gpuImg = new OclImage<Bgr, byte>(img);
            OclImage<Bgr, byte> smallGpuImg = new OclImage<Bgr, byte>(size);

            OclInvoke.Resize(gpuImg, smallGpuImg, 0, 0, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Image<Bgr, Byte> smallCpuImg = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);


            Image<Bgr, Byte> diff = smallGpuImg.ToImage().AbsDiff(smallCpuImg);
            //TODO: Check why they are not an excat match
            //Assert.IsTrue(diff.CountNonzero()[0] == 0);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
         }
      }

      [Test]
      public void TestCanny()
      {
         if (OclInvoke.HasOpenCL)
         {
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            using (OclImage<Bgr, Byte> CudaImage = new OclImage<Bgr, byte>(image))
            using (OclImage<Gray, Byte> gray = CudaImage.Convert<Gray, Byte>())
            using (OclImage<Gray, Byte> canny = new OclImage<Gray, byte>(gray.Size))
            {
               OclInvoke.Canny(gray, canny, 20, 100, 3, false);
               //Emgu.CV.UI.ImageViewer.Show(canny);
            }
         }
      }

      [Test]
      public void TestHOG1()
      {
         if (OclInvoke.HasOpenCL)
         {
            using (OclHOGDescriptor hog = new OclHOGDescriptor())
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            {
               float[] pedestrianDescriptor = OclHOGDescriptor.GetDefaultPeopleDetector();
               hog.SetSVMDetector(pedestrianDescriptor);

               Stopwatch watch = Stopwatch.StartNew();
               Rectangle[] rects;
               /*
               //using (OclImage<Bgr, Byte> CudaImage = new OclImage<Bgr, byte>(image))
               //using (OclImage<Bgra, Byte> gpuBgra = CudaImage.Convert<Bgra, Byte>())
               using (Image<Bgra, Byte> imgBgra = image.Convert<Bgra, Byte>())
               using (OclImage<Bgra, Byte> gpuBgra = new OclImage<Bgra,byte>(imgBgra))
                  rects = hog.DetectMultiScale(gpuBgra);
               */
               using (OclImage<Bgr, Byte> CudaImage = new OclImage<Bgr, byte>(image))
               using (OclImage<Gray, Byte> gpuGray = CudaImage.Convert<Gray, Byte>())
               {
                  rects = hog.DetectMultiScale(gpuGray);
               }
               watch.Stop();

               Assert.AreEqual(1, rects.Length);

               foreach (Rectangle rect in rects)
                  image.Draw(rect, new Bgr(Color.Red), 1);
               Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

               //Emgu.CV.UI.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
         }
      }

      [Test]
      public void TestErodeDilate()
      {
         if (!OclInvoke.HasOpenCL)
            return;

         int morphIter = 1;
         Image<Gray, Byte> image = new Image<Gray, byte>(640, 320);
         image.Draw(new CircleF(new PointF(200, 200), 30), new Gray(255.0), 4);

         using (OclImage<Gray, Byte> CudaImage = new OclImage<Gray, byte>(image))
         using (OclImage<Gray, Byte> gpuTemp = new OclImage<Gray, byte>(CudaImage.Size))
         {
            OclInvoke.Erode(CudaImage, gpuTemp, IntPtr.Zero, new Point(-1, -1), morphIter);
            OclInvoke.Dilate(gpuTemp, CudaImage, IntPtr.Zero, new Point(-1, -1), morphIter);

            using (Image<Gray, Byte> temp = new Image<Gray, byte>(image.Size))
            {
               CvInvoke.cvErode(image, temp, IntPtr.Zero, morphIter);
               CvInvoke.cvDilate(temp, image, IntPtr.Zero, morphIter);
            }

            Assert.IsTrue(CudaImage.ToImage().Equals(image));
         }

      }

      [Test]
      public void TestOclSURFKeypointDetection()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         
         Image<Gray, byte> image = new Image<Gray, byte>(200, 100);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            OclImage<Gray, Byte> gpuMat = new OclImage<Gray, byte>(image);

            Assert.IsTrue(gpuMat.ToImage().Equals(image));

            OclSURFDetector gpuSurf = new OclSURFDetector(100.0f, 2, 4, false, 0.01f, false);
            OclMat<float> gpuKpts = gpuSurf.DetectKeyPointsRaw(gpuMat, null);
            VectorOfKeyPoint kpts = new VectorOfKeyPoint();
            gpuSurf.DownloadKeypoints(gpuKpts, kpts);
         
      }

      [Test]
      public void TestGpuPyr()
      {
         if (!OclInvoke.HasOpenCL)
            return;

         Image<Gray, Byte> img = new Image<Gray, byte>(640, 480);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
         Image<Gray, Byte> down = img.PyrDown();
         Image<Gray, Byte> up = down.PyrUp();

         OclImage<Gray, Byte> gImg = new OclImage<Gray, byte>(img);
         OclImage<Gray, Byte> gDown = new OclImage<Gray, byte>(gImg.Size.Width >> 1, gImg.Size.Height >> 1);
         OclImage<Gray, Byte> gUp = new OclImage<Gray, byte>(img.Size);
         OclInvoke.PyrDown(gImg, gDown);
         OclInvoke.PyrUp(gDown, gUp);

         CvInvoke.cvAbsDiff(down, gDown.ToImage(), down);
         CvInvoke.cvAbsDiff(up, gUp.ToImage(), up);
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
         Assert.LessOrEqual(maxVal, 1.0);

         up.MinMax(out minVals, out maxVals, out minLocs, out maxLocs);
         maxVal = 0.0;
         for (int i = 0; i < maxVals.Length; i++)
         {
            if (maxVals[i] > maxVal)
               maxVal = maxVals[i];
         }
         Trace.WriteLine(String.Format("Max diff: {0}", maxVal));
         Assert.LessOrEqual(maxVal, 1.0);
      }

      [Test]
      public void TestMatchTemplate()
      {
         if (!OclInvoke.HasOpenCL)
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

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         Point[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         double gpuMinVal = 0, gpuMaxVal = 0;
         Point gpuMinLoc = Point.Empty, gpuMaxLoc = Point.Empty;
         OclImage<Bgr, Byte> CudaImage = new OclImage<Bgr, byte>(img);
         OclImage<Bgr, Byte> gpuRandomObj = new OclImage<Bgr, byte>(randomObj);
         OclImage<Gray, Single> gpuMatch = new OclImage<Gray, float>(match.Size);
         using (OclMatchTemplateBuf buffer = new OclMatchTemplateBuf())
         {
            OclInvoke.MatchTemplate(CudaImage, gpuRandomObj, gpuMatch, CvEnum.TM_TYPE.CV_TM_SQDIFF, buffer);
            OclInvoke.MinMaxLoc(gpuMatch, ref gpuMinVal, ref gpuMaxVal, ref gpuMinLoc, ref gpuMaxLoc, IntPtr.Zero);
         }

         EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
         EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
         //EmguAssert.IsTrue(minLoc[0].Equals(gpuMinLoc));
         //EmguAssert.IsTrue(maxLoc[0].Equals(gpuMaxLoc));

      }

      [Test]
      public void TestCascadeClassifierFaceDetect()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, byte>("lena.jpg");
         //using (HaarCascade cascade = new HaarCascade("eye_12.xml"))
         using (OclCascadeClassifier cascade = new OclCascadeClassifier("haarcascade_eye.xml"))
         //using (HaarCascade cascade = new HaarCascade("haarcascade_frontalface_alt2.xml"))
         {
            Rectangle[] objects = cascade.DetectMultiScale(image, 1.05, 10, new Size(10, 10), Size.Empty);
            foreach (Rectangle obj in objects)
               image.Draw(obj, new Gray(0.0), 1);

            //UI.ImageViewer.Show(image);
         }
      }

      [Test]
      public void TestOclOpticalFlowDual_TVL1()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         OclImage<Gray, Byte> prevImg, currImg;
         OpticalFlowOclImage(out prevImg, out currImg);
         OclImage<Gray, Single> flowx = new OclImage<Gray, float>(prevImg.Size);
         OclImage<Gray, Single> flowy = new OclImage<Gray, float>(prevImg.Size);

         OclOpticalFlowDual_TVL1 flow = new OclOpticalFlowDual_TVL1();
         flow.Dense(prevImg, currImg, flowx, flowy);
         
      }


      [Test]
      public void TestOclOpticalFlowLK()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         OclImage<Gray, Byte> prevImg, currImg;
         OpticalFlowOclImage(out prevImg, out currImg);
         OclPyrLKOpticalFlow flow = new OclPyrLKOpticalFlow(new Size(10, 10), 3, 10, false);

         OclImage<Gray, Single> flowx = new OclImage<Gray, float>(prevImg.Size);
         OclImage<Gray, Single> flowy = new OclImage<Gray, float>(prevImg.Size);
         flow.Dense(prevImg, currImg, flowx, flowy);
      }

      private static void OpticalFlowOclImage(out OclImage<Gray, Byte> preOclImg, out OclImage<Gray, Byte> currOclImg)
      {
         Image<Gray, Byte> prevImg, currImg;
         AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
         preOclImg = new OclImage<Gray, byte>(prevImg);
         currOclImg = new OclImage<Gray, byte>(currImg);
         prevImg.Dispose();
         currImg.Dispose();
      }

      [Test]
      public void TestMeanShiftFiltering()
      {
         if (!OclInvoke.HasOpenCL)
            return;
       
         Image<Bgra, byte> image = EmguAssert.LoadImage<Bgra, Byte>("pedestrian.png");
         Image<Bgra, Byte> segmentResult = new Image<Bgra, byte>(image.Size);
         OclImage<Bgra, Byte> oclImage = new OclImage<Bgra,byte>(image);
         OclImage<Bgra, Byte> oclFilterResult = new OclImage<Bgra,byte>(oclImage.Size);
         OclMat<Int16> oclPoints = new OclMat<short>(oclImage.Size, 2);
         MCvTermCriteria termCrit = new MCvTermCriteria(5, 1);
         OclInvoke.MeanShiftSegmentation(oclImage, segmentResult, 10, 20, 10, termCrit);
         OclInvoke.MeanShiftFiltering(oclImage, oclFilterResult, 10, 20, termCrit);
         OclInvoke.MeanShiftProc(oclImage, oclFilterResult, oclPoints, 10, 20, termCrit);
         //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(segmentResult));
      
      }

      [Test]
      public void TestWarpPerspective()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         Matrix<float> transformation = new Matrix<float>(3, 3);
         transformation.SetIdentity();

         Image<Gray, byte> image = new Image<Gray, byte>(480, 320);
         image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

         using (OclImage<Gray, byte> oclImage = new OclImage<Gray, byte>(image))
         using (OclImage<Gray, Byte> resultOclImage = new OclImage<Gray, byte>(oclImage.Size))
         {
            OclInvoke.WarpPerspective(oclImage, resultOclImage, transformation, CvEnum.INTER.CV_INTER_CUBIC);
         }
      }

      [Test]
      public void TestBruteForceHammingDistance()
      {
         if (!OclInvoke.HasOpenCL)
            return;
         Image<Gray, byte> box = new Image<Gray, byte>("box.png");
         FastDetector fast = new FastDetector(100, true);
         BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);

         #region extract features from the object image
         Stopwatch stopwatch = Stopwatch.StartNew();
         VectorOfKeyPoint modelKeypoints = fast.DetectRaw(box, null);
         Matrix<Byte> modelDescriptors = brief.Compute(box, null, modelKeypoints);
         stopwatch.Stop();
         Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
         #endregion

         Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

         #region extract features from the observed image
         stopwatch.Reset(); stopwatch.Start();
         VectorOfKeyPoint observedKeypoints = fast.DetectRaw(observedImage, null);
         Matrix<Byte> observedDescriptors = brief.Compute(observedImage, null, observedKeypoints);
         stopwatch.Stop();
         Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
         #endregion

         HomographyMatrix homography = null;
         using (OclMat<Byte> gpuModelDescriptors = new OclMat<byte>(modelDescriptors)) //initialization of GPU code might took longer time.
         {
            stopwatch.Reset(); stopwatch.Start();
            OclBruteForceMatcher<byte> hammingMatcher = new OclBruteForceMatcher<Byte>(DistanceType.Hamming);

            //BruteForceMatcher hammingMatcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.Hamming, modelDescriptors);
            int k = 2;
            Matrix<int> trainIdx = new Matrix<int>(observedKeypoints.Size, k);
            Matrix<float> distance = new Matrix<float>(trainIdx.Size);

            using (OclMat<Byte> gpuObservedDescriptors = new OclMat<byte>(observedDescriptors))
            using (OclMat<int> gpuTrainIdx = new OclMat<int>())
            using (OclMat<float> gpuDistance = new OclMat<float>())
            {
               Stopwatch w2 = Stopwatch.StartNew();
               hammingMatcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuTrainIdx, gpuDistance, k,  null);
               w2.Stop();
               Trace.WriteLine(String.Format("Time for feature matching (excluding data transfer): {0} milli-sec", w2.ElapsedMilliseconds));

               using (Matrix<int> trainIdxReshaped = trainIdx.Reshape(gpuTrainIdx.NumberOfChannels, gpuTrainIdx.Rows))
               using (Matrix<float> distanceReshaped = distance.Reshape(gpuDistance.NumberOfChannels, gpuDistance.Rows))
               {
                  gpuTrainIdx.Download(trainIdxReshaped);
                  gpuDistance.Download(distanceReshaped);
               }
            }

            Matrix<Byte> mask = new Matrix<byte>(distance.Rows, 1);
            mask.SetValue(255);
            Features2DToolbox.VoteForUniqueness(distance, 0.8, mask);

            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= 4)
            {
               nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeypoints, observedKeypoints, trainIdx, mask, 1.5, 20);
               if (nonZeroCount >= 4)
                  homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints, observedKeypoints, trainIdx, mask, 2);
               nonZeroCount = CvInvoke.cvCountNonZero(mask);
            }

            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time for feature matching (including data transfer): {0} milli-sec", stopwatch.ElapsedMilliseconds));
         }

         if (homography != null)
         {
            Rectangle rect = box.ROI;
            PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};

            PointF[] points = pts.Clone() as PointF[];
            homography.ProjectPoints(points);

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
