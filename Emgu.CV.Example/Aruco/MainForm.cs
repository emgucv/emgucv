//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

        int markersX = 4;
        int markersY = 4;
        int markersLength = 80;
        int markersSeparation = 30;

        private Dictionary _dict;

        private Dictionary ArucoDictionary
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
                return _dict;
            }

        }

        private GridBoard _gridBoard;
        private GridBoard ArucoBoard
        {
            get
            {
                if (_gridBoard == null)
                {
                    _gridBoard = new GridBoard(markersX, markersY, markersLength, markersSeparation, ArucoDictionary);
                }
                return _gridBoard;
            }
        }

        private void printArucoBoardButton_Click(object sender, EventArgs e)
        {
            Size imageSize = new Size();

            int margins = markersSeparation;
            imageSize.Width = markersX * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            imageSize.Height = markersY * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            int borderBits = 1;

            Mat boardImage = new Mat();
            ArucoBoard.Draw(imageSize, boardImage, margins, borderBits);
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

        private VectorOfInt _allIds = new VectorOfInt();
        private VectorOfVectorOfPointF _allCorners = new VectorOfVectorOfPointF();
        private VectorOfInt _markerCounterPerFrame = new VectorOfInt();
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
                        ArucoInvoke.RefineDetectedMarkers(_frameCopy, ArucoBoard, corners, ids, rejected, null, null, 10, 3, true, null, _detectorParameters);
                        //cameraButton.Text = "Calibrate camera";
                        this.Invoke((Action)delegate
                       {
                           useThisFrameButton.Enabled = true;
                       });
                        ArucoInvoke.DrawDetectedMarkers(_frameCopy, corners, ids, new MCvScalar(0, 255, 0));

                        if (!_cameraMatrix.IsEmpty && !_distCoeffs.IsEmpty)
                        {
                            ArucoInvoke.EstimatePoseSingleMarkers(corners, markersLength, _cameraMatrix, _distCoeffs, rvecs, tvecs);
                            for (int i = 0; i < ids.Size; i++)
                            {
                                using (Mat rvecMat = rvecs.Row(i))
                                using (Mat tvecMat = tvecs.Row(i))
                                using (VectorOfDouble rvec = new VectorOfDouble())
                                using (VectorOfDouble tvec = new VectorOfDouble())
                                {
                                    double[] values = new double[3];
                                    rvecMat.CopyTo(values);
                                    rvec.Push(values);
                                    tvecMat.CopyTo(values);
                                    tvec.Push(values);


                                    ArucoInvoke.DrawAxis(_frameCopy, _cameraMatrix, _distCoeffs, rvec, tvec,
                                       markersLength * 0.5f);

                                }
                            }
                        }

                        if (_useThisFrame)
                        {
                            _allCorners.Push(corners);
                            _allIds.Push(ids);
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
                        double repError = ArucoInvoke.CalibrateCameraAruco(_allCorners, _allIds, _markerCounterPerFrame, ArucoBoard, _imageSize,
                           _cameraMatrix, _distCoeffs, null, null, CalibType.Default, new MCvTermCriteria(30, double.Epsilon));

                        UpdateMessage(String.Format("Camera calibration completed with reprojection error: {0}", repError));
                        _allCorners.Clear();
                        _allIds.Clear();
                        _markerCounterPerFrame.Clear();
                        _imageSize = Size.Empty;

                    }

                    //stop the capture
                    cameraButton.Text = "Start Capture";
                    _capture.Pause();

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
