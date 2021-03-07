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
        Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null);

        /// <summary>
        /// Prcocess the image and render directly on it.
        /// </summary>
        /// <param name="image">The image to process and render on.</param>
        /// <returns>The messages that we want to display.</returns>
        String ProcessAndRender(IInputOutputArray image);
    }
}
