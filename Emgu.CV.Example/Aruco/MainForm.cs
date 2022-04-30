//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;

namespace Aruco
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();


            _detectorParameters = DetectorParameters.GetDefault();

            try
            {
                _capture = new VideoCapture();
                if (!_capture.IsOpened)
                {
                    _capture = null;
                    throw new NullReferenceException("Unable to open video capture");
                }
                else
                {
                    _capture.ImageGrabbed += ProcessFrame;
                }
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            UpdateMessage(String.Empty);
        }

        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private bool _useThisFrame = false;

        int _markersX = 7;
        int _markersY = 7;
        int _markersLength = 80;
        int _markersSeparation = 30;

        private Dictionary _dict;

        private Dictionary ArucoDictionary
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict7X7_100);
                return _dict;
            }

        }

        private CharucoBoard _charucoBoard;
        private CharucoBoard Board
        {
            get
            {
                if (_charucoBoard == null)
                {
                    _charucoBoard = new CharucoBoard(_markersX, _markersY, _markersLength, _markersSeparation, ArucoDictionary);
                }
                return _charucoBoard;
            }
        }

        private void printCharucoBoardButton_Click(object sender, EventArgs e)
        {
            Size imageSize = new Size();

            int margins = _markersSeparation;
            imageSize.Width = _markersX * (_markersLength + _markersSeparation) - _markersSeparation + 2 * margins;
            imageSize.Height = _markersY * (_markersLength + _markersSeparation) - _markersSeparation + 2 * margins;
            int borderBits = 1;

            Mat boardImage = new Mat();
            Board.Draw(imageSize, boardImage, margins, borderBits);
            bmIm = boardImage.ToBitmap();
            PrintImage();

        }

        private void PrintImage()
        {
            PrintDocument pd = new PrintDocument();

            //pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            //pd.OriginAtMargins = false;
            //pd.DefaultPageSettings.Landscape = true;

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();

            printPreviewDialog1.Document = pd;
            //printPreviewDialog1.AutoScale = true;
            printPreviewDialog1.ShowDialog();


        }

        Image bmIm;

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            double cmToUnits = 100 / 2.54;
            e.Graphics.DrawImage(bmIm, 0, 0, (float)(15 * cmToUnits), (float)(15 * cmToUnits));
        }

        Mat _frame = new Mat();
        Mat _frameCopy = new Mat();

        Mat _cameraMatrix = new Mat();
        Mat _distCoeffs = new Mat();
        Mat rvecs = new Mat();
        Mat tvecs = new Mat();

        private VectorOfVectorOfInt _allIds = new VectorOfVectorOfInt();
        private VectorOfVectorOfVectorOfPointF _allCorners = new VectorOfVectorOfVectorOfPointF();

        private VectorOfVectorOfPointF _allCornersConcatenated = new VectorOfVectorOfPointF();
        private VectorOfInt _allIdsConcatenated = new VectorOfInt();
        private VectorOfInt _markerCounterPerFrame = new VectorOfInt();
        private List<Mat> _allImgs = new List<Mat>();

        private Size _imageSize = Size.Empty;

        private DetectorParameters _detectorParameters;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                _frame.CopyTo(_frameCopy);

                //cameraImageBox.Image = _frame;
                using (VectorOfInt ids = new VectorOfInt())
                using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
                using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF())
                {
                    //DetectorParameters p = DetectorParameters.GetDefault();
                    ArucoInvoke.DetectMarkers(_frameCopy, ArucoDictionary, corners, ids, _detectorParameters, rejected);

                    if (ids.Size > 0)
                    {
                        ArucoInvoke.RefineDetectedMarkers(_frameCopy, Board, corners, ids, rejected, null, null, 10, 3, true, null, _detectorParameters);
                        //cameraButton.Text = "Calibrate camera";

                        using (Mat currentCharucoCorners = new Mat())
                        using (Mat currentCharucoIds = new Mat())
                        {
                            ArucoInvoke.InterpolateCornersCharuco(
                                corners,
                                ids,
                                _frameCopy,
                                _charucoBoard,
                                currentCharucoCorners,
                                currentCharucoIds);

                            ArucoInvoke.DrawDetectedMarkers(_frameCopy, corners, ids, new MCvScalar(0, 255, 0));
                            if (!currentCharucoCorners.IsEmpty)
                            {
                                ArucoInvoke.DrawDetectedCornersCharuco(_frameCopy, currentCharucoCorners, currentCharucoIds, new MCvScalar(255, 0, 0));
                            }
                        }

                        this.Invoke((Action)delegate
                           {
                               useThisFrameButton.Enabled = true;
                           });

                        if (_useThisFrame)
                        {
                            Mat m = new Mat();
                            _frame.CopyTo(m);
                            _allImgs.Add(_frame);

                            _allCorners.Push(corners);
                            _allIds.Push(ids);

                            _allCornersConcatenated.Push(corners);
                            _allIdsConcatenated.Push(ids);
                            _markerCounterPerFrame.Push(new int[] { corners.Size });
                            _imageSize = _frame.Size;
                            UpdateMessage(String.Format("Using {0} points", _markerCounterPerFrame.ToArray().Sum()));
                            _useThisFrame = false;
                        }
                    }
                    else
                    {
                        this.Invoke((Action)delegate
                       {
                           useThisFrameButton.Enabled = false;
                       });

                        //cameraButton.Text = "Stop Capture";
                    }
                    cameraImageBox.Image = _frameCopy.Clone();
                }
            }
            else
            {
                UpdateMessage("VideoCapture was not created");
            }
        }

        private void UpdateMessage(String message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)delegate { UpdateMessage(message); });
                return;
            }

            messageLabel.Text = message;
        }

        private void cameraButton_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {
                    int totalPoints = _markerCounterPerFrame.ToArray().Sum();
                    if (totalPoints > 0)
                    {
                        // calibrate camera using aruco markers
                        double arucoRepErr = ArucoInvoke.CalibrateCameraAruco(
                            _allCornersConcatenated,
                            _allIdsConcatenated,
                            _markerCounterPerFrame,
                            Board,
                            _imageSize,
                            _cameraMatrix,
                            _distCoeffs,
                            null,
                            null,
                            CalibType.Default,
                            new MCvTermCriteria(30, double.Epsilon));
                        int nFrames = _allCorners.Size;
                        using (VectorOfMat allCharucoCorners = new VectorOfMat())
                        using (VectorOfMat allCharucoIds = new VectorOfMat())
                        {
                            for (int i = 0; i < nFrames; i++)
                            {
                                using (Mat currentCharucoCorners = new Mat())
                                using (Mat currentCharucoIds = new Mat())
                                using (VectorOfVectorOfPointF currentCorners = _allCorners[i])
                                using (VectorOfInt currentIds = _allIds[i])

                                {
                                    ArucoInvoke.InterpolateCornersCharuco(
                                        currentCorners,
                                        currentIds,
                                        _allImgs[i],
                                        Board,
                                        currentCharucoCorners,
                                        currentCharucoIds,
                                        _cameraMatrix,
                                        _distCoeffs
                                    );
                                    allCharucoCorners.Push(currentCharucoCorners);
                                    allCharucoIds.Push(currentCharucoIds);
                                }
                            }

                            // calibrate camera using charuco
                            double repError =
                                ArucoInvoke.CalibrateCameraCharuco(
                                    allCharucoCorners,
                                    allCharucoIds,
                                    Board,
                                    _imageSize,
                                    _cameraMatrix,
                                    _distCoeffs,
                                    null,
                                    null,
                                    CalibType.Default,
                                    new MCvTermCriteria(30, double.Epsilon));

                            UpdateMessage(String.Format("Camera calibration completed. Aruco reprojection error: {0}; Charuco reprojection error {1}", arucoRepErr, repError));
                        }

                        _allCorners.Clear();
                        _allIds.Clear();
                        _markerCounterPerFrame.Clear();
                        _allIdsConcatenated.Clear();
                        _allCornersConcatenated.Clear();

                        foreach (var img in _allImgs)
                            img.Dispose();
                        
                        _allImgs.Clear();

                        _imageSize = Size.Empty;

                    }

                    //stop the capture
                    cameraButton.Text = "Start Capture";
                    _capture.Stop();
                    //_capture.Dispose();
                    //_capture.Pause();

                }
                else
                {
                    //start the capture
                    cameraButton.Text = "Stop Capture";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }

        private void useThisFrameButton_Click(object sender, EventArgs e)
        {
            _useThisFrame = true;
        }
    }
}
