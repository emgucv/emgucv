//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.OCR
{
    
    /// <summary>
    /// Interface to the TessResultRenderer
    /// </summary>
    public interface ITessResultRenderer
    {
        /// <summary>
        /// Pointer to the unmanaged TessResultRendered
        /// </summary>
        IntPtr TessResultRendererPtr { get; }
    }

    /// <summary>
    /// Provides a debugger proxy for the <see cref="ITessResultRenderer"/> interface.
    /// This class is used to simplify the debugging experience by exposing key properties
    /// of the <see cref="ITessResultRenderer"/> implementation in a more accessible manner.
    /// </summary>
    public class TessResultRendererDebuggerProxy
    {
        private ITessResultRenderer _v;

        /// <summary>
        /// Initializes a new instance of the <see cref="TessResultRendererDebuggerProxy"/> class.
        /// </summary>
        /// <param name="v">
        /// An implementation of the <see cref="ITessResultRenderer"/> interface to be wrapped by this debugger proxy.
        /// </param>
        /// <remarks>
        /// This constructor is used to create a debugger proxy for the provided <see cref="ITessResultRenderer"/> instance,
        /// allowing easier inspection of its properties during debugging.
        /// </remarks>
        public TessResultRendererDebuggerProxy(ITessResultRenderer v)
        {
            _v = v;
        }

        /// <summary>
        /// Gets a value indicating whether the associated Tesseract result renderer is in a "happy" state.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer is in a "happy" state; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property provides a simplified way to check the internal state of the Tesseract result renderer
        /// during debugging. A "happy" state typically indicates that the renderer is functioning correctly.
        /// </remarks>
        public bool Happy
        {
            get
            {
                return _v.Happy();
            }
        }

        /// <summary>
        /// Gets the index of the last image provided to the renderer.
        /// </summary>
        /// <remarks>
        /// This property reflects the index of the most recent image given to the renderer, 
        /// regardless of whether the image was successfully processed or not. 
        /// It provides insight into the current state of the rendering process:
        /// - During a document lifecycle, it indicates the number of the current or last image.
        /// - In a completed document, it represents the total number of images processed.
        /// - If no document was started, it returns -1.
        /// </remarks>
        /// <value>
        /// The index of the last image provided to the renderer, or -1 if no document was started.
        /// </value>
        public int ImageNum
        {
            get
            {
                return _v.ImageNum();
            }
        }

    }
}
