//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.ServiceModel.Configuration;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace VideoSurveillance
{
    public class TrackedObject : TrackerKCF
    {
        public TrackedObject(int id) :
            base()
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public partial class VideoSurveillance : Form
    {
        private int _idCounter = 0;

        List<TrackedObject> _trackers = new List<TrackedObject>();

        private static VideoCapture _cameraCapture;

        private static IBackgroundSubtractor _fgDetector;
        //private static SimpleBlobDetector _blobDetector;

        public VideoSurveillance()
        {
            InitializeComponent();
            Run();
        }

        void Run()
        {
            try
            {
                _cameraCapture = new VideoCapture();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            _fgDetector = new BackgroundSubtractorMOG2();
            //_blobDetector = new SimpleBlobDetector();
            
            Application.Idle += ProcessFrame;
        }

        void ProcessFrame(object sender, EventArgs e)
        {
            Mat frame = _cameraCapture.QueryFrame();
            Mat smoothedFrame = new Mat();
            CvInvoke.GaussianBlur(frame, smoothedFrame, new Size(3, 3), 1); //filter out noises
            
            #region use the BG/FG detector to find the forground mask
            Mat forgroundMask = new Mat();
            _fgDetector.Apply(smoothedFrame, forgroundMask);
            #endregion

            List<TrackedObject> toBeRemovedList = new List<TrackedObject>();
            List<Rectangle> boundingBoxesTracked = new List<Rectangle>();
            List<int> idsTracked = new List<int>();

            foreach (TrackedObject t in _trackers)
            {
                Rectangle boundingBox;
                bool success = t.Update(frame, out boundingBox);
                if (success)
                {
                    boundingBoxesTracked.Add(boundingBox);
                    idsTracked.Add(t.Id);
                    
                }
                else 
                    toBeRemovedList.Add(t);
            }

            foreach (TrackedObject t in toBeRemovedList)
            {
                _trackers.Remove(t);
                t.Dispose();
            }

            List<Rectangle> newObjects = new List<Rectangle>();
            int minAreaThreshold = 100;
            using (Mat canny = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.Canny(forgroundMask, canny, 180, 100);
                CvInvoke.FindContours(canny, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > minAreaThreshold) //only consider contours with area greater than 250
                        {
                            Rectangle r = CvInvoke.BoundingRectangle(approxContour);
                            bool overlapped = false;
                            foreach (var trackedObj in boundingBoxesTracked)
                            {
                                if (r.IntersectsWith(trackedObj))
                                {
                                    overlapped = true;
                                    break;
                                }
                            }
                            if (!overlapped)
                                newObjects.Add(r);
                        }
                    }
                }
            }
            
            foreach (Rectangle r in newObjects)
            {
                TrackedObject t = new TrackedObject(_idCounter);
                boundingBoxesTracked.Add(r);
                idsTracked.Add(_idCounter);
                t.Init(frame, r);
                _trackers.Add(t);
                _idCounter++;
            }

            for (int i = 0; i < boundingBoxesTracked.Count; i++)
            {
                Rectangle boundingBox = boundingBoxesTracked[i];
                CvInvoke.Rectangle(frame, boundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                CvInvoke.PutText(frame, idsTracked[i].ToString(), new Point(boundingBox.X, boundingBox.Y), FontFace.HersheyPlain,
                    1.0, new MCvScalar(255.0, 255.0, 255.0));
            }

            imageBox1.Image = frame;
            imageBox2.Image = forgroundMask;
        }
    }
}