//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Util
{
    /// <summary>
    /// A raw data storage
    /// </summary>
    /// <typeparam name="T">The type of elements in the storage</typeparam>
    public class BinaryFileStorage<T> : IEnumerable<T>
        where T : struct
    {
        private static int _elementSize = Toolbox.SizeOf<T>();

        private int _trunkSize;

        private const int _defaultTrunkSize = 4096;

        /// <summary>
        /// The file info
        /// </summary>
        protected FileInfo _fileInfo;

        /// <summary>
        /// Create a binary File Storage
        /// </summary>
        /// <param name="fileName">The file name of the storage</param>
        public BinaryFileStorage(String fileName)
           : this(fileName, _defaultTrunkSize)
        {
        }

        /// <summary>
        /// Create a binary File Storage
        /// </summary>
        /// <param name="fileName">The file name of the storage</param>
        /// <param name="trunkSize">The data will be read in trunk of this size internally. Can be use to seed up the file read. A good number will be 4096</param>
        public BinaryFileStorage(String fileName, int trunkSize)
        {
            _fileInfo = new FileInfo(fileName);
            _trunkSize = trunkSize <= 0 ? _defaultTrunkSize : trunkSize;
        }

        /// <summary>
        /// Create a binary File Storage with the specific data
        /// </summary>
        /// <param name="fileName">The file name of the storage, all data in the existing file will be replaced</param>
        /// <param name="samples">The data which will be stored in the storage</param>
        public BinaryFileStorage(String fileName, IEnumerable<T> samples)
           : this(fileName)
        {
            Clear();
            Append(samples);
        }

        /// <summary>
        /// Append the samples to the end of the storage
        /// </summary>
        /// <param name="samples">The samples to be appended to the storage</param>
        public void Append(IEnumerable<T> samples)
        {
            int elementsInTrunk = _trunkSize / _elementSize;
            if (elementsInTrunk <= 0)
                elementsInTrunk = 1;

            byte[] byteBuffer = new byte[elementsInTrunk * _elementSize];
            int index = 0;

            using (PinnedArray<T> buffer = new PinnedArray<T>(elementsInTrunk))
            using (FileStream stream = _fileInfo.Open(FileMode.Append, FileAccess.Write))
            using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize))
            {
                IntPtr ptr = buffer.AddrOfPinnedObject();
                foreach (T s in samples)
                {
                    buffer.Array[index++] = s;

                    if (index == buffer.Array.Length)
                    {
                        int writeCount = index * _elementSize;
                        Marshal.Copy(ptr, byteBuffer, 0, writeCount);
                        bufferStream.Write(byteBuffer, 0, writeCount);
                        index = 0;
                    }
                }
                if (index != 0)
                {
                    int writeCount = index * _elementSize;
                    Marshal.Copy(ptr, byteBuffer, 0, writeCount);
                    bufferStream.Write(byteBuffer, 0, writeCount);
                    //index = 0;
                }
            }
        }

        /// <summary>
        /// The file name of the storage
        /// </summary>
        public String FileName
        {
            get
            {
                return _fileInfo.FullName;
            }
        }

        /// <summary>
        /// Delete all data in the existing storage, if there is any.
        /// </summary>
        public void Clear()
        {
            if (_fileInfo.Exists)
                _fileInfo.Delete();
        }

        /// <summary>
        /// Estimate the number of elements in this storage as the size of the storage divided by the size of the elements
        /// </summary>
        /// <returns>An estimation of the number of elements in this storage</returns>
        public int EstimateSize()
        {
            return
               _fileInfo.Exists ?
               (int)(_fileInfo.Length / (_elementSize)) :
               0;
        }

        /// <summary>
        /// Get a copy of the first element in the storage. If the storage is empty, a default value will be returned
        /// </summary>
        /// <returns>A copy of the first element in the storage. If the storage is empty, a default value will be returned</returns>
        public T Peek()
        {
            using (FileStream stream = _fileInfo.OpenRead())
            using (PinnedArray<Byte> buffer = new PinnedArray<byte>(_elementSize))
            {
                return (stream.Read(buffer.Array, 0, _elementSize) > 0) ?
                    Marshal.PtrToStructure<T>(buffer.AddrOfPinnedObject()) : new T();
            }
        }

        /// <summary>
        /// Get the sub-sampled data in this storage
        /// </summary>
        /// <param name="subsampleRate">The sub-sample rate</param>
        /// <returns>The sub-sampled data in this storage</returns>
        public IEnumerable<T> GetSubsamples(int subsampleRate)
        {
            if (!_fileInfo.Exists)
                yield break;

            int bufferSize = _elementSize * subsampleRate;
            using (FileStream stream = _fileInfo.OpenRead())
            using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize))
            using (PinnedArray<Byte> buffer = new PinnedArray<byte>(bufferSize))
            using (PinnedArray<T> structure = new PinnedArray<T>(subsampleRate))
            {
                IntPtr structAddr = structure.AddrOfPinnedObject();
                IntPtr addr = buffer.AddrOfPinnedObject();
                while (bufferStream.Read(buffer.Array, 0, bufferSize) > 0)
                {
                    CvToolbox.Memcpy(structAddr, addr, bufferSize);
                    yield return structure.Array[0];
                }
            }
        }

        #region IEnumerable<T> Members
        /// <summary>
        /// Get the data in this storage
        /// </summary>
        /// <returns>The data in this storage</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (!_fileInfo.Exists)
                yield break;

            int elementsInTrunk = _trunkSize / _elementSize;
            if (elementsInTrunk <= 0)
                elementsInTrunk = 1;

            Byte[] buffer = new byte[_elementSize * elementsInTrunk];

            using (FileStream stream = _fileInfo.OpenRead())
            using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize))
            using (PinnedArray<T> structures = new PinnedArray<T>(elementsInTrunk))
            {
                IntPtr structAddr = structures.AddrOfPinnedObject();
                int bytesRead;
                while ((bytesRead = bufferStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Marshal.Copy(buffer, 0, structAddr, bytesRead);
                    int elementsRead = bytesRead / _elementSize;
                    for (int i = 0; i < elementsRead; i++)
                    {
                        yield return structures.Array[i];
                    }
                }
            }
        }
        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
