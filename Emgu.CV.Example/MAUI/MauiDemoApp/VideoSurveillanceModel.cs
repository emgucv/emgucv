//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
using System.Diagnostics;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Optional capability for a model that renders several stacked panes and can
    /// focus a single pane full-screen. A hosting page can offer per-pane expand
    /// controls when the model implements this.
    /// </summary>
    public interface IMultiPaneModel
    {
        /// <summary>Pane labels, top to bottom.</summary>
        string[] PaneNames { get; }

        /// <summary>-1 shows all panes stacked; otherwise the pane index shown full-screen.</summary>
        int FocusedPane { get; set; }
    }

    public class VideoSurveillanceModel : DisposableObject, IProcessAndRenderModel, IMultiPaneModel
    {
        // -1 shows both panes stacked; 0 = camera only, 1 = motion mask only.
        private int _focusedPane = -1;

        /// <summary>Pane labels, top to bottom, for the multi-pane host UI.</summary>
        public string[] PaneNames => new[] { "Camera", "Motion" };

        /// <summary>Which pane to show full-screen (-1 = both stacked).</summary>
        public int FocusedPane
        {
            get => _focusedPane;
            set => _focusedPane = value;
        }

        /// <summary>
        /// The rendering method
        /// </summary>
        public RenderType RenderMethod
        {
            get
            {
                return RenderType.Overwrite;
            }
        }

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

        private List<TrackedObject> _trackers = new List<TrackedObject>();

        private static IBackgroundSubtractor _fgDetector;

        protected override void DisposeObject()
        {
            Clear();
        }

        public void Clear()
        {
            if (_fgDetector != null)
            {
                _fgDetector.Clear();

                IDisposable _fgDetectorDisposable = _fgDetector as IDisposable;
                if (_fgDetectorDisposable != null)
                {
                    _fgDetectorDisposable.Dispose();
                }

                _fgDetector = null;
            }

            if (_trackers != null)
            {
                _trackers.Clear();
            }

            if (_smoothedFrame != null)
            {
                _smoothedFrame.Dispose();
                _smoothedFrame = null;
            }

            if (_frameCopy != null)
            {
                _frameCopy.Dispose();
                _frameCopy = null;
            }

            if (_canny != null)
            {
                _canny.Dispose();
                _canny = null;
            }

            if (_foregroundMask != null)
            {
                _foregroundMask.Dispose();
                _foregroundMask = null;
            }

            if (_foregroundMaskBgr != null)
            {
                _foregroundMaskBgr.Dispose();
                _foregroundMaskBgr = null;
            }

            _idCounter = 0;
        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                if (_fgDetector == null)
                    return false;
                if (_trackers == null)
                    return false;
                return true;
            }
        }

        private Mat _smoothedFrame = null;
        private Mat _frameCopy = null;
        private Mat _canny = null;
        private Mat _foregroundMask = null;
        private Mat _foregroundMaskBgr = null;

        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged, object initOptions)
        {
            _fgDetector = new BackgroundSubtractorMOG2();
            await Task.Delay(1);
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (_smoothedFrame == null)
            {
                _smoothedFrame = new Mat();
            }

            if (_frameCopy == null)
            {
                _frameCopy = new Mat();
            }

            if (_canny == null)
            {
                _canny = new Mat();
            }

            if (_foregroundMask == null)
            {
                _foregroundMask = new Mat();
            }

            if (_foregroundMaskBgr == null)
            {
                _foregroundMaskBgr = new Mat();
            }

            using (InputArray iaImageIn = imageIn.GetInputArray())
            {
                iaImageIn.CopyTo(_frameCopy);
                ;
                CvInvoke.GaussianBlur(imageIn, _smoothedFrame, new Size(3, 3), 1); //filter out noises

                #region use the BG/FG detector to find the foreground mask
                _fgDetector.Apply(_smoothedFrame, _foregroundMask);
                CvInvoke.CvtColor(_foregroundMask, _foregroundMaskBgr, ColorConversion.Gray2Bgr);

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
                //using (Mat canny = new Mat())
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    CvInvoke.Canny(_foregroundMask, _canny, 180, 100);
                    CvInvoke.FindContours(_canny, contours, null, RetrType.External,
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
                    CvInvoke.Rectangle(_frameCopy, boundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                    CvInvoke.PutText(_frameCopy, idsTracked[i].ToString(), new Point(boundingBox.X, boundingBox.Y),
                        HersheyFonts.Plain,
                        1.0, new MCvScalar(255.0, 255.0, 255.0));
                }

                //foregroundMaskBgr.CopyTo(imageOut);
                if (_focusedPane == 0)
                {
                    // Camera pane only, full-screen.
                    _frameCopy.CopyTo(imageOut);
                }
                else if (_focusedPane == 1)
                {
                    // Motion (foreground mask) pane only, full-screen.
                    _foregroundMaskBgr.CopyTo(imageOut);
                }
                else
                {
                    // Stack the tracked frame above the foreground mask, separated
                    // by a thin white divider so the two panes read as distinct.
                    // Vertical (not horizontal) concat keeps each pane usefully
                    // sized on a portrait phone, where a double-wide image would
                    // shrink to a thin strip.
                    int dividerHeight = Math.Max(2, _frameCopy.Height / 120);
                    using (Mat divider = new Mat(dividerHeight, _frameCopy.Width, _frameCopy.Depth, _frameCopy.NumberOfChannels))
                    {
                        divider.SetTo(new MCvScalar(255.0, 255.0, 255.0));
                        CvInvoke.VConcat(new Mat[] { _frameCopy, divider, _foregroundMaskBgr }, imageOut);
                    }
                }

            }
            watch.Stop();
            return String.Format("Frame processed in {0} milliseconds", watch.ElapsedMilliseconds);
        }
    }
}
