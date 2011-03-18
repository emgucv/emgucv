//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Emgu.Util
{
   /// <summary>
   /// A raw data storage
   /// </summary>
   /// <typeparam name="T">The type of elments in the storage</typeparam>
   public class BinaryFileStorage<T> : IEnumerable<T> where T : struct
   {
      private int _trunkSize;

      /// <summary>
      /// The file info
      /// </summary>
      protected FileInfo _fileInfo;

      /// <summary>
      /// Create a binary File Storage
      /// </summary>
      /// <param name="fileName">The file name of the storage</param>
      public BinaryFileStorage(String fileName)
      {
         _fileInfo = new FileInfo(fileName);
      }

      /// <summary>
      /// Create a binary File Storage
      /// </summary>
      /// <param name="fileName">The file name of the storage</param>
      /// <param name="trunkSize">The data will be read in trunk of this size internally. Can be use to seed up the file read. A good number will be 4096</param>
      public BinaryFileStorage(String fileName, int trunkSize)
      {
         _fileInfo = new FileInfo(fileName);
         _trunkSize = trunkSize;
      }

      /// <summary>
      /// Create a binary File Storage with the specific data
      /// </summary>
      /// <param name="fileName">The file name of the storage</param>
      /// <param name="samples">The data which will be stored in the storage</param>
      public BinaryFileStorage(String fileName, IEnumerable<T> samples)
      {
         _fileInfo = new FileInfo(fileName);
         if (_fileInfo.Exists)
            _fileInfo.Delete();

         int size = Marshal.SizeOf(typeof(T));

         using (PinnedArray<Byte> buffer = new PinnedArray<byte>(size))
         using (FileStream stream = _fileInfo.OpenWrite())
         using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize <= 0 ? 4096 : _trunkSize))
         {
            IntPtr ptr = buffer.AddrOfPinnedObject();
            foreach (T s in samples)
            {
               Marshal.StructureToPtr(s, ptr, false);
               bufferStream.Write(buffer.Array, 0, size);
            }
         }
      }

      /// <summary>
      /// Get a copy of the first element in the storage. If the storage is empty, a default value will be returned
      /// </summary>
      /// <returns>A copy of the first element in the storage. If the storage is empty, a default value will be returned</returns>
      public T Peek()
      {
         int elementSize = Marshal.SizeOf(typeof(T));
         using (FileStream stream = _fileInfo.OpenRead())
         using (PinnedArray<Byte> buffer = new PinnedArray<byte>(elementSize))
         {
            return (stream.Read(buffer.Array, 0, elementSize) > 0) ?
               (T)Marshal.PtrToStructure(buffer.AddrOfPinnedObject(), typeof(T)) :
               new T();
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
      /// Estimate the number of elements in this storage as the size of the storage divided by the size of the elements
      /// </summary>
      /// <returns>An estimation of the number of elements in this storage</returns>
      public int EstimateSize()
      {
         return (int)(_fileInfo.Length / (Marshal.SizeOf(typeof(T))));
      }

      /// <summary>
      /// Get the subsampled data in this storage
      /// </summary>
      /// <param name="subsampleRate">The subsample rate</param>
      /// <returns>The subsampled data in this storage</returns>
      public IEnumerable<T> GetSubsamples(int subsampleRate)
      {
         int elementSize = Marshal.SizeOf(typeof(T));
         int bufferSize = elementSize * subsampleRate;

         using (FileStream stream = _fileInfo.OpenRead())
         using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize <= 0 ? 4096 : _trunkSize))
         using (PinnedArray<Byte> buffer = new PinnedArray<byte>(bufferSize))
         using (PinnedArray<T> structure = new PinnedArray<T>(subsampleRate))
         {
            IntPtr structAddr = structure.AddrOfPinnedObject();
            IntPtr addr = buffer.AddrOfPinnedObject();
            while (bufferStream.Read(buffer.Array, 0, bufferSize) > 0)
            {
               Toolbox.memcpy(structAddr, addr, bufferSize);
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
         
         int elementSize = Marshal.SizeOf(typeof(T));
         using (FileStream stream = _fileInfo.OpenRead())
         using (BufferedStream bufferStream = new BufferedStream(stream, _trunkSize <= 0 ? 4096 : _trunkSize))
         using (PinnedArray<Byte> buffer = new PinnedArray<byte>(elementSize))
         using (PinnedArray<T> structure = new PinnedArray<T>(1))
         {
            IntPtr structAddr = structure.AddrOfPinnedObject();
            IntPtr addr = buffer.AddrOfPinnedObject();

            while (bufferStream.Read(buffer.Array, 0, elementSize) > 0)
            {
               Toolbox.memcpy(structAddr, addr, elementSize);
               yield return structure.Array[0];
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
