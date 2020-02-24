//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    public static class FileReaderMat
    {
        public static bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            var readersTypes = Emgu.Util.Toolbox.GetIntefaceImplementationFromAssembly<Emgu.CV.IFileReaderMat>();

            foreach (var type in readersTypes)
            {

                Emgu.CV.IFileReaderMat reader = Activator.CreateInstance(type) as Emgu.CV.IFileReaderMat;
                if (reader.ReadFile(fileName, mat, loadType))
                    return true;
            }

            return false;
        }
    }
}
