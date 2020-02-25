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
        private static Emgu.CV.IFileReaderMat[] _fileReaderMatArr;

        public static bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            if (_fileReaderMatArr == null)
            {
                Type[] readersTypes = Emgu.Util.Toolbox.GetIntefaceImplementationFromAssembly<Emgu.CV.IFileReaderMat>();
                Emgu.CV.IFileReaderMat[] matArr = new IFileReaderMat[readersTypes.Length];
                for (int i = 0; i < readersTypes.Length; i++)
                {
                    matArr[i] = Activator.CreateInstance(readersTypes[i]) as Emgu.CV.IFileReaderMat;
                }

                _fileReaderMatArr = matArr;
            }

            foreach (var reader in _fileReaderMatArr)
            {
                if (reader.ReadFile(fileName, mat, loadType))
                    return true;
            }

            return false;
        }
    }
}
