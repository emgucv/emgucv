using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// a wrapper for the CvMatND 
   /// </summary>
   /// <typeparam name="TDepth">The type of depth</typeparam>
   public class MatND<TDepth> : Emgu.Util.UnmanagedObject
   {
      private Array _array;

      /// <summary>
      /// The pinned GCHandle to _array;
      /// </summary>
      protected GCHandle _dataHandle;

      /// <summary>
      /// Create a N-dimensional matrix 
      /// </summary>
      /// <param name="sizes">The size for each dimension</param>
      public MatND(params int[] sizes)
      {
         ManagedArray = Array.CreateInstance(typeof(TDepth), sizes);
      }

      private void AllocateHeader()
      {
         if (_ptr == IntPtr.Zero)
         {
            _ptr = Marshal.AllocHGlobal(StructSize.MCvMatND);
            GC.AddMemoryPressure(StructSize.MCvMatND);
         }
      }

      /// <summary>
      /// Get the underneath managed array
      /// </summary>
      public Array ManagedArray
      {
         get { return _array; }
         set
         {
            Debug.Assert(value != null, "The Array cannot be null");

            AllocateHeader();

            if (_dataHandle.IsAllocated)
               _dataHandle.Free(); //free the data handle
            Debug.Assert(!_dataHandle.IsAllocated, "Handle should be freed");

            _array = value;
            _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            int[] dim = new int[_array.Rank];
            for (int i = 0; i < dim.Length; i++)
            {
               dim[i] = _array.GetLength(i);
            }
            CvInvoke.cvInitMatNDHeader(_ptr, dim.Length, dim, CvDepth, _dataHandle.AddrOfPinnedObject());
         }
      }

      ///<summary> Get the depth representation for openCV</summary>
      protected static CvEnum.MAT_DEPTH CvDepth
      {
         get
         {
            return Util.GetMatrixDepth(typeof(TDepth));
         }
      }

      /// <summary>
      /// Release the matrix and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            Marshal.FreeHGlobal(_ptr);
            GC.RemoveMemoryPressure(StructSize.MCvMatND);
            _ptr = IntPtr.Zero;
         }
      }
   }
}
