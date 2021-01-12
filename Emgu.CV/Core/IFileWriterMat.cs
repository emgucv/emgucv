//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// The inteface for writing a Mat into a file.
    /// </summary>
    public interface IFileWriterMat
    {
        /// <summary>
        /// Write the Mat into the file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="mat">The mat to be written to the file</param>
        /// <returns>True if successful</returns>
        bool WriteFile(Mat mat, String fileName);
    }
}
