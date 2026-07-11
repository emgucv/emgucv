//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Models;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Face detection model that dispatches to one of the available face
    /// detectors based on the init option: Yunet (objdetect + dnn),
    /// CascadeClassifier (objdetect) or Face Landmark (face + dnn).
    /// </summary>
    public class FaceDetectionModel : DisposableObject, IProcessAndRenderModel
    {
        /// <summary>
        /// Picker option for the FaceDetectorYN (Yunet) detector. Requires the objdetect and dnn modules.
        /// </summary>
        public const String Yunet = "Yunet";

        /// <summary>
        /// Picker option for the cascade classifier face and eye detector. Requires the objdetect module.
        /// </summary>
        public const String CascadeClassifier = "CascadeClassifier";

        /// <summary>
        /// Picker option for the face and landmark detector. Requires the face and dnn modules.
        /// </summary>
        public const String FaceLandmark = "Face Landmark (DNN)";

        private String _currentOption = null;
        private IProcessAndRenderModel _model = null;

        /// <summary>
        /// The rendering method. All the underlying detectors render with the Update method.
        /// </summary>
        public RenderType RenderMethod
        {
            get
            {
                return RenderType.Update;
            }
        }

        /// <summary>
        /// Return true if the underlying detector is created and initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _model != null && _model.Initialized;
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_model != null)
            {
                _model.Clear();
                IDisposable disposable = _model as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
                _model = null;
            }
            _currentOption = null;
        }

        /// <summary>
        /// Release the memory associated with this face detection model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        private static String GetDefaultOption()
        {
            //Prefer Yunet when the dnn module is available, otherwise fall
            //back to the cascade classifier that only needs objdetect.
            bool haveDNN = CvInvoke.ConfigDict["HAVE_OPENCV_DNN"] != 0;
            return haveDNN ? Yunet : CascadeClassifier;
        }

        /// <summary>
        /// Download and initialize the face detector chosen by the init option.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">One of the option strings: "Yunet", "CascadeClassifier" or "Face Landmark (DNN)". If null, Yunet is used when the dnn module is available, CascadeClassifier otherwise.</param>
        /// <returns>Async task</returns>
        public async Task Init(
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
        {
            String option = initOptions as String;
            if (String.IsNullOrEmpty(option))
                option = GetDefaultOption();

            if (_model != null && !option.Equals(_currentOption))
                Clear();

            if (_model == null)
            {
                if (option.Equals(Yunet))
                    _model = new FaceDetectorYNModel();
                else if (option.Equals(FaceLandmark))
                    _model = new FaceAndLandmarkDetector();
                else
                    _model = new CascadeFaceAndEyeDetector();
                _currentOption = option;
            }

            await _model.Init(onDownloadProgressChanged, null);
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image</param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            return _model.ProcessAndRender(imageIn, imageOut);
        }
    }
}
