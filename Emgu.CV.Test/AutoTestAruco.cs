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
using DetectorParameters = Emgu.CV.Aruco.DetectorParameters;
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
    public class AutoTestAruco
    {
        [Test]
        public void TestArucoCustomBoard()
        {
            using (Aruco.Dictionary dictionary = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_50))
            using (Mat corners = new Mat(4, 1, DepthType.Cv32F, 3))
            using (VectorOfMat objPoints = new VectorOfMat(corners))
            using (VectorOfInt ids = new VectorOfInt(new[] { 7 }))
            {
                //One marker, 4 corners at custom 3D positions (clockwise from top-left)
                float[] pts = {
                    0.0f, 0.0f, 0.05f,
                    0.1f, 0.0f, 0.05f,
                    0.1f, 0.1f, 0.05f,
                    0.0f, 0.1f, 0.05f };
                corners.SetTo(pts);

                using (ArucoBoard board = new ArucoBoard(objPoints, dictionary, ids))
                {
                    EmguAssert.IsTrue(board.Ptr != IntPtr.Zero);
                    EmguAssert.IsTrue(board.BoardPtr != IntPtr.Zero);
                }
            }
        }

        [Test]
        public void TestArucoCreateBoard()
        {
            Emgu.CV.Aruco.DetectorParameters p = DetectorParameters.GetDefault();

            Size imageSize = new Size();
            int markersX = 4;
            int markersY = 4;
            int markersLength = 80;
            int markersSeparation = 30;
            int margins = markersSeparation;
            imageSize.Width = markersX * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            imageSize.Height = markersY * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            int borderBits = 1;

            Aruco.Dictionary dictionary = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
            Aruco.GridBoard board = new GridBoard(markersX, markersY, markersLength, markersSeparation, dictionary);
            Mat boardImage = new Mat();
            board.GenerateImage(imageSize, boardImage, margins, borderBits);
            CvInvoke.Imwrite("board.png", boardImage);
        }
    }
}
