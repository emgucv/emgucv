using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Emgu.Util
{
   public class BinaryFileStorage<T> : IEnumerable<T> where T : struct
   {
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
         Byte[] buffer = new Byte[size];
         GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
         IntPtr ptr = handle.AddrOfPinnedObject();
         using (FileStream stream = _fileInfo.OpenWrite())
         {
            foreach (T s in samples)
            {
               Marshal.StructureToPtr(s, ptr, false);
               stream.Write(buffer, 0, size);
            }
         }
         handle.Free();
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

      #region IEnumerable<T> Members
      /// <summary>
      /// Get the data in this storage
      /// </summary>
      /// <returns>The data in this storage</returns>
      public IEnumerator<T> GetEnumerator()
      {
         using (FileStream stream = _fileInfo.OpenRead())
         {
            int elementSize = Marshal.SizeOf(typeof(T));
            Byte[] buffer = new byte[elementSize];
            GCHandle handler = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr addr = handler.AddrOfPinnedObject();

            while (stream.Read(buffer, 0, elementSize) > 0)
            {
               yield return (T)Marshal.PtrToStructure(addr, typeof(T));
            }
            handler.Free();
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
