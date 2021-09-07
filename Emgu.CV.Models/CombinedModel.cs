//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Combined model
    /// </summary>
    public class CombinedModel : DisposableObject, IProcessAndRenderModel
    {
        private IProcessAndRenderModel[] _models;
        private bool _disposeChildren = false;

        /// <summary>
        /// Combine multiple IProcessAndRenderModel into a single model.
        /// </summary>
        /// <param name="models">The models to be combined.</param>
        public CombinedModel(params IProcessAndRenderModel[] models)
        {
            _models = models;
        }

        /// <summary>
        /// Get the list of combined models.
        /// </summary>
        public IProcessAndRenderModel[] Models
        {
            get
            {
                return _models;
            }
        }

        /// <summary>
        /// If true, will dispose all the IProcessAndRenderModel passed in the constructor when this object is disposed. Otherwise it will not dispose the children when it is disposed.
        /// </summary>
        public bool DisposeChildren
        {
            get { return _disposeChildren; }
            set { _disposeChildren = value; }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_models != null)
            {
                foreach (var model in _models)
                {
                    model.Clear();
                }
            }
        }

        /// <summary>
        /// Download and initialize the model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">Initialization options</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged, Object initOptions)
#else
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged, Object initOptions)
#endif
        {
            if (_models != null)
            {
                foreach (var model in _models)
                {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    yield return model.Init(onDownloadProgressChanged, initOptions);
#else
                    await model.Init(onDownloadProgressChanged, initOptions);
#endif
                }
            }
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">
        /// The output image. In a combined model, it should be different than <paramref name="imageIn"/> to avoid having the output of one model being passed as the input of the other model.
        /// Note that if nothing is detected, the output image will remain unchanged.
        /// It is a good idea to copy the pixels over from the <paramref name="imageIn"/> to <paramref name="imageOut"/> before passing it to this function.
        /// </param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            StringBuilder allMsg = new StringBuilder();
            
            foreach (var model in _models)
            {
                String msg = model.ProcessAndRender(imageIn, imageOut);
                allMsg.AppendLine(msg);
            }

            return allMsg.ToString();
        }

        /// <summary>
        /// Release the memory associated with this combined model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_disposeChildren)
            {
                foreach (var child in _models)
                {
                    child.Dispose();
                }
            }

            _models = null;
        }
    }
}
