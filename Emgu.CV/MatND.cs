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
         Data = Array.CreateInstance(typeof(TDepth), sizes);
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
      public Array Data
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
            Type typeOfDepth = typeof(TDepth);

            if (typeOfDepth == typeof(Single))
               return CvEnum.MAT_DEPTH.CV_32F;
            if (typeOfDepth == typeof(UInt32))
               return Emgu.CV.CvEnum.MAT_DEPTH.CV_32S;
            if (typeOfDepth == typeof(SByte))
               return Emgu.CV.CvEnum.MAT_DEPTH.CV_8S;
            if (typeOfDepth == typeof(Byte))
               return CvEnum.MAT_DEPTH.CV_8U;
            if (typeOfDepth == typeof(Double))
               return CvEnum.MAT_DEPTH.CV_64F;
            if (typeOfDepth == typeof(UInt16))
               return CvEnum.MAT_DEPTH.CV_16U;
            if (typeOfDepth == typeof(Int16))
               return CvEnum.MAT_DEPTH.CV_16S;
            throw new NotImplementedException("Unsupported matrix depth");
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
