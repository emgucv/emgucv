//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   public abstract class MatDataAllocator : UnmanagedObject
   {
      protected IntPtr _memoryAllocator;
      private GCHandle _dataHandle;

      private Array _data;
      public Array Data
      {
         get
         {
            return _data;
         }
      }

#if IOS
      [MonoTouch.MonoPInvokeCallback(typeof(MatAllocateCallback))]
#endif
      internal IntPtr AllocateCallback(Mat.DepthType type, int channels, int totalInBytes)
      {
         FreeData();
         
         switch (type)
         {
            //case Mat.DepthType.Cv8U:
            //   _data = new byte[totalInBytes];
            //   break;
            case Mat.DepthType.Cv8S:
               _data = new SByte[totalInBytes];
               break;
            case Mat.DepthType.Cv16U:
               _data = new UInt16[totalInBytes >> 1];
               break;
            case Mat.DepthType.Cv16S:
               _data = new Int16[totalInBytes >> 1];
               break;
            case Mat.DepthType.Cv32S:
               _data = new Int32[totalInBytes >> 2];
               break;
            case Mat.DepthType.Cv32F:
               _data = new float[totalInBytes >> 2];
               break;
            case Mat.DepthType.Cv64F:
               _data = new double[totalInBytes >> 3];
               break;
            default:
               _data = new byte[totalInBytes];
               break;
         }

         _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
         return _dataHandle.AddrOfPinnedObject();
      }

#if IOS
      [MonoTouch.MonoPInvokeCallback(typeof(MatDeallocateCallback))]
#endif
      internal void DeallocateCallback(IntPtr data)
      {
         FreeData();
      }

      private void FreeData()
      {
         if (_dataHandle.IsAllocated)
            _dataHandle.Free();
         if (_data != null)
            _data = null;
      }

      protected override void DisposeObject()
      {
         if (_memoryAllocator != IntPtr.Zero)
            MatDataAllocatorInvoke.cvMatAllocatorRelease(ref _memoryAllocator);

         FreeData();
      }
   }

   internal static class MatDataAllocatorInvoke
   {
      static MatDataAllocatorInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      internal delegate IntPtr MatAllocateCallback(Mat.DepthType type, int channels, int totalInBytes);

      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      internal delegate void MatDeallocateCallback(IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatAllocatorRelease(ref IntPtr allocator);
   }
}
