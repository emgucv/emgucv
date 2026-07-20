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
    public class AutoTestFlann
    {
        /*
        [Test]
        public void TestKDTree()
        {
           float[][] features = new float[10][];
           for (int i = 0; i < features.Length; i++)
              features[i] = new float[] { (float) i };
           FeatureTree tree = new FeatureTree(features);

           Matrix<Int32> result;
           Matrix<double> distance;
           float[][] features2 = new float[1][];
           features2[0] = new float[] { 5.0f };

           tree.FindFeatures(features2, out result, out distance, 1, 20);
           EmguAssert.IsTrue(result[0, 0] == 5);
           EmguAssert.IsTrue(distance[0, 0] == 0.0);
        }

        [Test]
        public void TestSpillTree()
        {
           float[][] features = new float[10][];
           for (int i = 0; i < features.Length; i++)
              features[i] = new float[] { (float) i };
           FeatureTree tree = new FeatureTree(features, 50, .7, .1);

           Matrix<Int32> result;
           Matrix<double> distance;
           float[][] features2 = new float[1][];
           features2[0] = new float[] { 5.0f };

           tree.FindFeatures(features2, out result, out distance, 1, 20);
           EmguAssert.IsTrue(result[0, 0] == 5);
           EmguAssert.IsTrue(distance[0, 0] == 0.0);
        }*/

        [Test]
        public void TestFlannLinear()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };
            Flann.LinearIndexParams p = new LinearIndexParams();
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Mat indices = new Mat();
            Mat distances = new Mat();
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            int[] indicesValue = indices.GetData(false) as int[];
            float[] distanceValue = distances.GetData(false) as float[];
            EmguAssert.IsTrue(indicesValue[0] == 5);
            EmguAssert.IsTrue(distanceValue[0] == 0.0);
        }

        [Test]
        public void TestFlannKDTree()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };

            Flann.KdTreeIndexParams p = new KdTreeIndexParams(4);
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Mat indices = new Mat();
            Mat distances = new Mat();
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            int[] indicesValue = indices.GetData(false) as int[];
            float[] distanceValue = distances.GetData(false) as float[];
            EmguAssert.IsTrue(indicesValue[0] == 5);
            EmguAssert.IsTrue(distanceValue[0] == 0.0);
        }

        [Test]
        public void TestFlannCompositeTree()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };

            Flann.CompositeIndexParams p = new CompositeIndexParams(4, 32, 11, Emgu.CV.Flann.CenterInitType.Random, 0.2f);
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Mat indices = new Mat();
            Mat distances = new Mat();
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            int[] indicesValue = indices.GetData(false) as int[];
            float[] distanceValue = distances.GetData(false) as float[];
            EmguAssert.IsTrue(indicesValue[0] == 5);
            EmguAssert.IsTrue(distanceValue[0] == 0.0);
        }


        [Test]
        public void TestFlannHierarchicalClusteringIndex()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };

            Flann.HierarchicalClusteringIndexParams p = new HierarchicalClusteringIndexParams();
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Mat indices = new Mat();
            Mat distances = new Mat();
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            int[] indicesValue = indices.GetData(false) as int[];
            float[] distanceValue = distances.GetData(false) as float[];
            EmguAssert.IsTrue(indicesValue[0] == 5);
            EmguAssert.IsTrue(distanceValue[0] == 0.0);
        }
    }
}
