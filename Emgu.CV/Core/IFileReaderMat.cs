//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    public interface IFileReaderMat
    {
        bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType);
    }
}
