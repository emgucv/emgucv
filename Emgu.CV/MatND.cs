//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#if !NETFX_CORE
using System.Security.Permissions;
#endif
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A MatND is a wrapper to cvMatND of OpenCV. 
   /// </summary>
   /// <typeparam name="TDepth">The type of depth</typeparam>
#if !NETFX_CORE
   [Serializable]
#endif
   public class MatND<TDepth> : CvArray<TDepth>, IEquatable<MatND<TDepth>> where TDepth : new()
   {
      private Array _array;

      /// <summary>
      /// Create a N-dimensional matrix 
      /// </summary>
      /// <param name="sizes">The size for each dimension</param>
      public MatND(params int[] sizes)
      {
         ManagedArray = Array.CreateInstance(typeof(TDepth), sizes);
      }

#if !NETFX_CORE
      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public MatND(SerializationInfo info, StreamingContext context)
      {
         DeserializeObjectData(info, context);
      }
#endif

      /// <summary>
      /// This function is not implemented for MatND
      /// </summary>
      /// <param name="rows">Not implemented</param>
      /// <param name="cols">Not implemented</param>
      /// <param name="numberOfChannels">Not implemented</param>
      protected override void AllocateData(int rows, int cols, int numberOfChannels)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      /// <summary>
      /// This function is not implemented for MatND
      /// </summary>
      public override int NumberOfChannels
      {
         get 
         { 
            throw new Exception("The method or operation is not implemented."); 
         }
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
      public override Array ManagedArray
      {
         get  { return _array; }
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
               dim[i] = _array.GetLength(i);
            
            CvInvoke.cvInitMatNDHeader(_ptr, dim.Length, dim, CvDepth, _dataHandle.AddrOfPinnedObject());
         }
      }

      private int[] GetDimension()
      {
         MCvMatND matND = MCvMatND;
         int[] dim = new int[matND.dims];
         for (int i = 0; i < dim.Length; i++)
         {
            dim[i] = matND.dim[i].Size;
         }
         return dim;
      }

      ///<summary> Get the depth representation for openCV</summary>
      protected static CvEnum.DepthType CvDepth
      {
         get
         {
            return CvInvoke.GetDepthType(typeof(TDepth));
         }
      }

      /// <summary>
      /// Release the matrix and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            Marshal.FreeHGlobal(_ptr);
            GC.RemoveMemoryPressure(StructSize.MCvMatND);
            _ptr = IntPtr.Zero;
         }
         _array = null;
      }

#if !NETFX_CORE
      #region ISerializable Members
      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Dimension", GetDimension());
         info.AddValue("CompressionRatio", SerializationCompressionRatio);
         info.AddValue("Bytes", Bytes);
      }

      /// <summary>
      /// A function used for runtime deserailization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      protected override void DeserializeObjectData(SerializationInfo info, StreamingContext context)
      {
         int[] dimension = (int[])info.GetValue("Dimension", typeof(int[]));
         ManagedArray = Array.CreateInstance(typeof(TDepth), dimension);

         SerializationCompressionRatio = (int)info.GetValue("CompressionRatio", typeof(int));
         Bytes = (Byte[])info.GetValue("Bytes", typeof(Byte[]));
      }
      #endregion
#endif

      /// <summary>
      /// Not Implemented
      /// </summary>
      /// <param name="reader">The XmlReader</param>
      public override void ReadXml(System.Xml.XmlReader reader)
      {
         throw new NotImplementedException("This function is not implemented");
      }

      /// <summary>
      /// Not Implemented
      /// </summary>
      /// <param name="writer">The XmlWriter</param>
      public override void WriteXml(System.Xml.XmlWriter writer)
      {
         throw new NotImplementedException("This function is not implemented");
      }

      /// <summary>
      /// The MCvMatND structure
      /// </summary>
      public MCvMatND MCvMatND
      {
         get
         {
#if NETFX_CORE 
            return Marshal.PtrToStructure<MCvMatND>(_ptr);
#else
            return (MCvMatND)Marshal.PtrToStructure(_ptr, typeof(MCvMatND));
#endif
         }
      }

      /// <summary>
      /// Convert this matrix to different depth
      /// </summary>
      /// <typeparam name="TOtherDepth">The depth type to convert to</typeparam>
      /// <returns>Matrix of different depth</returns>
      public MatND<TOtherDepth> Convert<TOtherDepth>()
         where TOtherDepth : new()
      {
         MatND<TOtherDepth> res = new MatND<TOtherDepth>(GetDimension());
         CvInvoke.cvConvertScale(Ptr, res.Ptr, 1.0, 0.0);
         return res;
      }
     
      #region IEquatable<MatND<TDepth>> Members
      /// <summary>
      /// Check if the two MatND are equal
      /// </summary>
      /// <param name="other">The other MatND to compares to</param>
      /// <returns>True if the two MatND equals</returns>
      public bool Equals(MatND<TDepth> other)
      {
         #region check if the two MatND has equal dimension
         int[] dim1 = GetDimension();
         int[] dim2 = other.GetDimension();
         if (dim1.Length != dim2.Length) return false;
         for (int i = 0; i < dim1.Length; i++)
         {
            if (dim1[i] != dim2[i]) return false;
         }
         #endregion

         using (MatND<TDepth> diff = new MatND<TDepth>(dim1))
         {
            CvInvoke.BitwiseXor(this, other, diff, null);
            return Array.TrueForAll(diff.Bytes, delegate(Byte b) { return b == 0; });
         }
      }

      #endregion
   }
}
