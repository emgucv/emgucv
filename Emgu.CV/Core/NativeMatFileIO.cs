//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// Native implementation to read files into Mat or Images.
    /// </summary>
    public static class NativeMatFileIO
    {
        private static Emgu.CV.IFileReaderMat[] _fileReaderMatArr;
        private static Emgu.CV.IFileWriterMat[] _fileWriterMatArr;

        /// <summary>
        /// Read a file into Mat using native implementations
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="mat">The Mat to read the file into</param>
        /// <param name="loadType">The image load type.</param>
        /// <returns>True if successful</returns>
        public static bool ReadFileToMat(String fileName, Mat mat, CvEnum.ImreadModes loadType)
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

            foreach (IFileReaderMat reader in _fileReaderMatArr)
            {
                if (reader.ReadFile(fileName, mat, loadType))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Write a Mat into a file using native implementations
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="mat">The Mat to be written</param>
        /// <returns>True if successful</returns>
        public static bool WriteMatToFile(Mat mat, String fileName)
        {
            if (_fileWriterMatArr == null)
            {
                Type[] writerTypes = Emgu.Util.Toolbox.GetIntefaceImplementationFromAssembly<Emgu.CV.IFileWriterMat>();
                Emgu.CV.IFileWriterMat[] matArr = new IFileWriterMat[writerTypes.Length];
                for (int i = 0; i < writerTypes.Length; i++)
                {
                    matArr[i] = Activator.CreateInstance(writerTypes[i]) as Emgu.CV.IFileWriterMat;
                }

                _fileWriterMatArr = matArr;
            }

            foreach (IFileWriterMat writer in _fileWriterMatArr)
            {
                if (writer.WriteFile(mat, fileName))
                    return true;
            }

            return false;
        }


    }
}
