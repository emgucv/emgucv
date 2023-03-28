using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    public class CharucoCameraCalibrationModel : Emgu.CV.Models.IProcessAndRenderModel
    {

        private Mat _cameraMatrix = new Mat();
        private Mat _distCoeffs = new Mat();

        private VectorOfVectorOfInt _allIds = new VectorOfVectorOfInt();
        private VectorOfVectorOfVectorOfPointF _allCorners = new VectorOfVectorOfVectorOfPointF();

        private VectorOfVectorOfPointF _allCornersConcatenated = new VectorOfVectorOfPointF();
        private VectorOfInt _allIdsConcatenated = new VectorOfInt();
        private VectorOfInt _markerCounterPerFrame = new VectorOfInt();
        private List<Mat> _allImgs = new List<Mat>();

        private Size _imageSize = Size.Empty;

        int _markersX = 7;
        int _markersY = 7;
        int _markersLength = 80;
        int _markersSeparation = 30;

        private DetectorParameters _detectorParameters;
        private Dictionary _dict;

        private Dictionary ArucoDictionary
        {
            get
            {
                return _dict;
            }

        }

        private CharucoBoard _charucoBoard;
        private CharucoBoard Board
        {
            get
            {

                return _charucoBoard;
            }
        }

        /// <summary>
        /// Release the memory associated with this model
        /// </summary>
        public void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_dict != null)
            {
                _dict.Dispose();
                _dict = null;
            }
            if (_charucoBoard != null)
            {
                _charucoBoard.Dispose();
                _charucoBoard = null;
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

            UseOneFrame = false;
            _frameUsable = false;
        }


        public void GetCharucoBoard(IOutputArray boardImage)
        {
            Size imageSize = new Size();

            int margins = _markersSeparation;
            imageSize.Width = _markersX * (_markersLength + _markersSeparation) - _markersSeparation + 2 * margins;
            imageSize.Height = _markersY * (_markersLength + _markersSeparation) - _markersSeparation + 2 * margins;
            int borderBits = 1;
            Board.GenerateImage(imageSize, boardImage, margins, borderBits);
        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                if (_dict == null)
                    return false;
                if (_charucoBoard == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Initialize the CharucoCameraCalibrationModel
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Asyn task</returns>

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, object initOptions = null)
#endif
        {
            Clear();

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return null;
#else
            _dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict7X7_100);
            _charucoBoard = new CharucoBoard(_markersX, _markersY, _markersLength, _markersSeparation, ArucoDictionary);
            _detectorParameters = DetectorParameters.GetDefault();
            await Task.Delay(1);
#endif
        }

        private bool _frameUsable = false;

        /// <summary>
        /// Return true if the current frame is usable
        /// </summary>
        public bool FrameUsable {
            get
            {
                return _frameUsable;
            }
        }

        private void SetFrameUsable(bool value)
        {
            if (value != _frameUsable)
            {
                _frameUsable = value;
                if (FrameUsableChanged != null)
                {
                    FrameUsableChanged(this, _frameUsable);
                }
            }
        }

        public event EventHandler<bool> FrameUsableChanged;

        public event EventHandler OnFrameAdded; 

        public bool UseOneFrame { get; set; }

        
        public String CalibrateCamera()
        {
            int totalPoints = _markerCounterPerFrame.ToArray().Sum();
            if (totalPoints == 0)
                return null;

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

                return String.Format("Camera calibration completed. Aruco reprojection error: {0}; Charuco reprojection error {1}", arucoRepErr, repError);
            }
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            using (InputArray iaImageIn = imageIn.GetInputArray())
            using (Mat imageInMat = iaImageIn.GetMat())
            using (VectorOfInt ids = new VectorOfInt())
            using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
            using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF())
            {
                imageInMat.CopyTo(imageOut);
                //DetectorParameters p = DetectorParameters.GetDefault();
                ArucoInvoke.DetectMarkers(imageIn, ArucoDictionary, corners, ids, _detectorParameters, rejected);

                if (ids.Size > 0)
                {
                    ArucoInvoke.RefineDetectedMarkers(imageIn, Board, corners, ids, rejected, null, null, 10, 3,
                        true, null, _detectorParameters);

                    using (Mat currentCharucoCorners = new Mat())
                    using (Mat currentCharucoIds = new Mat())
                    {
                        ArucoInvoke.InterpolateCornersCharuco(
                            corners,
                            ids,
                            imageIn,
                            _charucoBoard,
                            currentCharucoCorners,
                            currentCharucoIds);

                        ArucoInvoke.DrawDetectedMarkers(imageOut, corners, ids, new MCvScalar(0, 255, 0));
                        if (!currentCharucoCorners.IsEmpty)
                        {
                            ArucoInvoke.DrawDetectedCornersCharuco(imageOut, currentCharucoCorners,
                                currentCharucoIds, new MCvScalar(255, 0, 0));
                        }
                    }

                    SetFrameUsable(true);

                    if (UseOneFrame)
                    {
                        Mat m = new Mat();
                        imageInMat.CopyTo(m);
                        _allImgs.Add(m);

                        _allCorners.Push(corners);
                        _allIds.Push(ids);

                        _allCornersConcatenated.Push(corners);
                        _allIdsConcatenated.Push(ids);
                        _markerCounterPerFrame.Push(new int[] { corners.Size });
                        _imageSize = m.Size;

                        if (OnFrameAdded != null)
                        {
                            OnFrameAdded(this, new EventArgs());
                        }

                        UseOneFrame = false;
                    }
                }
                else
                {
                    SetFrameUsable(false);
                }
            }
            return String.Format("Using {0} points", _markerCounterPerFrame.ToArray().Sum());
        }
    }
}
