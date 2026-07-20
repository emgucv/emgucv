//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
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
    public class AutoTestStitching
    {
        [Test]
        public void TestStitching1()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("stitch2.jpg");
            images[2] = EmguAssert.LoadMat("stitch3.jpg");
            images[3] = EmguAssert.LoadMat("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher())
            {
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                    using (VectorOfCameraParams vcp = stitcher.Cameras())
                    {
                        int[] component = stitcher.Component();
                    }
                }
                //Emgu.CV.WinForms.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching2()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("stitch2.jpg");
            images[2] = EmguAssert.LoadMat("stitch3.jpg");
            images[3] = EmguAssert.LoadMat("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher())
            using (ORB finder = new ORB())
            {
                stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.WinForms.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching3()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("lena.jpg");
            images[2] = EmguAssert.LoadMat("dog416.png");
            images[3] = EmguAssert.LoadMat("pedestrian.png");

            using (Stitcher stitcher = new Stitcher())
            //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
            {
                //stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.WinForms.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching4()
        {
            Mat[] images = new Mat[1];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");


            using (Stitcher stitcher = new Stitcher())
            //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
            {
                //stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.WinForms.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching5()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("stitch2.jpg");
            images[2] = EmguAssert.LoadMat("stitch3.jpg");
            images[3] = EmguAssert.LoadMat("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher(Stitcher.Mode.Panorama))
            using (ORB detector = new ORB())
            using (SphericalWarper warper = new SphericalWarper())
            using (SeamFinder finder = new GraphCutSeamFinder())
            using (BlocksChannelsCompensator compensator = new BlocksChannelsCompensator())
            using (FeatherBlender blender = new FeatherBlender())
            {
                stitcher.SetFeaturesFinder(detector);
                stitcher.SetWarper(warper);
                stitcher.SetSeamFinder(finder);
                stitcher.SetExposureCompensator(compensator);
                stitcher.SetBlender(blender);

                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }

                //Emgu.CV.WinForms.ImageViewer.Show(result);
            }
        }

        /*
        [Test]
        public void TestStitching5()
        {
           Mat[] images = new Mat[2];

           images[0] = new Mat();
           images[1] = new Mat();

           using (Stitcher stitcher = new Stitcher(false))
           //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
           {
              //stitcher.SetFeaturesFinder(finder);
              Mat result = new Mat();
              using (VectorOfMat vm = new VectorOfMat())
              {
                 vm.Push(images);
                 stitcher.Stitch(vm, result);
              }
              //Emgu.CV.WinForms.ImageViewer.Show(result);
           }
        }*/
    }
}
