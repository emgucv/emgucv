//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// DNN Backend and Target pair
    /// </summary>
    public struct BackendTargetPair
    {
        /// <summary>
        /// Dnn Backend and Target pair
        /// </summary>
        /// <param name="backend">The backend</param>
        /// <param name="target">The target</param>
        public BackendTargetPair(Backend backend, Target target)
        {
            this.Backend = backend;
            this.Target = target;
        }

        /// <summary>
        /// The backend
        /// </summary>
        public Backend Backend;

        /// <summary>
        /// The target
        /// </summary>
        public Target Target;
    }
}
