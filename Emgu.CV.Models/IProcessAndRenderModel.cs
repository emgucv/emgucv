//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
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
    /// Process and render model
    /// </summary>
    public interface IProcessAndRenderModel
    {
        /// <summary>
        /// Download and initialize the model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
        Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null);

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <returns>The messages that we want to display.</returns>
        String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut);
    }
}
