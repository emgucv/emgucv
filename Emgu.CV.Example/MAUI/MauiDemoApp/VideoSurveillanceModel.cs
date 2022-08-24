//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using Emgu.Util;
using Size = System.Drawing.Size;
using Point = System.Drawing.Point;

namespace Emgu.CV.Models
{

    public class VideoSurveillanceModel : IProcessAndRenderModel
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

        private int _idCounter = 0;

        List<TrackedObject> _trackers = new List<TrackedObject>();

        private static IBackgroundSubtractor _fgDetector;

        public void Dispose()
        {
            Clear();
            
            if (_fgDetector != null)
            {
                IDisposable _fgDetectorDisposable = _fgDetector as IDisposable;
                if (_fgDetectorDisposable != null)
                {
                    _fgDetectorDisposable.Dispose();
                }
                _fgDetector = null;
            }
        }

        public void Clear()
        {
            _fgDetector.Clear();
            _trackers.Clear();
            _idCounter = 0;
        }

        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged, object initOptions)
        {
            _fgDetector = new BackgroundSubtractorMOG2();
            await Task.Delay(1);
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            using (InputArray iaImageIn = imageIn.GetInputArray())
            using (Mat smoothedFrame = new Mat())
            using (Mat frameCopy = new Mat())
            using (Mat foregroundMask = new Mat())
            using (Mat foregroundMaskBgr = new Mat())
            {
                iaImageIn.CopyTo(frameCopy);
                ;
                CvInvoke.GaussianBlur(imageIn, smoothedFrame, new Size(3, 3), 1); //filter out noises

                #region use the BG/FG detector to find the foreground mask
                _fgDetector.Apply(smoothedFrame, foregroundMask);
                CvInvoke.CvtColor(foregroundMask, foregroundMaskBgr, ColorConversion.Gray2Bgr);

                #endregion

                List<TrackedObject> toBeRemovedList = new List<TrackedObject>();
                List<Rectangle> boundingBoxesTracked = new List<Rectangle>();
                List<int> idsTracked = new List<int>();

                foreach (TrackedObject t in _trackers)
                {
                    Rectangle boundingBox;
                    bool success = t.Update(imageIn, out boundingBox);
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
                    CvInvoke.Canny(foregroundMask, canny, 180, 100);
                    CvInvoke.FindContours(canny, contours, null, RetrType.External,
                        ChainApproxMethod.ChainApproxSimple);
                    int count = contours.Size;
                    for (int i = 0; i < count; i++)
                    {
                        using (VectorOfPoint contour = contours[i])
                        using (VectorOfPoint approxContour = new VectorOfPoint())
                        {
                            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05,
                                true);
                            if (CvInvoke.ContourArea(approxContour, false) >
                                minAreaThreshold) //only consider contours with area greater than threshold
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
                    t.Init(imageIn, r);
                    _trackers.Add(t);
                    _idCounter++;
                }

                for (int i = 0; i < boundingBoxesTracked.Count; i++)
                {
                    Rectangle boundingBox = boundingBoxesTracked[i];
                    CvInvoke.Rectangle(frameCopy, boundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                    CvInvoke.PutText(frameCopy, idsTracked[i].ToString(), new Point(boundingBox.X, boundingBox.Y),
                        FontFace.HersheyPlain,
                        1.0, new MCvScalar(255.0, 255.0, 255.0));
                }

                CvInvoke.HConcat(frameCopy, foregroundMaskBgr, imageOut);
            }

            return String.Empty;
        }
    }
}
