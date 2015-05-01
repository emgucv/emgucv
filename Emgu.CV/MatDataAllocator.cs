//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   /// <summary>
   /// Matrix data allocator. Base class for Mat that handles the matrix data allocation and deallocation
   /// </summary>
   public abstract class MatDataAllocator : UnmanagedObject
   {
      internal IntPtr _memoryAllocator;
      private GCHandle _dataHandle;
      internal IntPtr _allocateDataActionPtr;
      internal IntPtr _freeDataActionPtr;

      private Array _data;

      internal void InitActionPtr()
      {
         GCHandle allocateDataActionHandle = GCHandle.Alloc((AllocateDataAction) AllocateData);
         _allocateDataActionPtr = GCHandle.ToIntPtr(allocateDataActionHandle);
         GCHandle freeDataActionHandle = GCHandle.Alloc((FreeDataAction)FreeData);
         _freeDataActionPtr = GCHandle.ToIntPtr(freeDataActionHandle);
      }

      /// <summary>
      /// Get the managed data used by the Mat
      /// </summary>
      public Array Data
      {
         get
         {
            return _data;
         }
      }

      private delegate IntPtr AllocateDataAction(CvEnum.DepthType type, int channels, int totalInBytes);
      private delegate void FreeDataAction();

      private IntPtr AllocateData(CvEnum.DepthType type, int channels, int totalInBytes)
      {
         FreeData();

         switch (type)
         {
            //case CvEnum.DepthType.Cv8U:
            //   _data = new byte[totalInBytes];
            //   break;
            case CvEnum.DepthType.Cv8S:
               _data = new SByte[totalInBytes];
               break;
            case CvEnum.DepthType.Cv16U:
               _data = new UInt16[totalInBytes >> 1];
               break;
            case CvEnum.DepthType.Cv16S:
               _data = new Int16[totalInBytes >> 1];
               break;
            case CvEnum.DepthType.Cv32S:
               _data = new Int32[totalInBytes >> 2];
               break;
            case CvEnum.DepthType.Cv32F:
               _data = new float[totalInBytes >> 2];
               break;
            case CvEnum.DepthType.Cv64F:
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
      [ObjCRuntime.MonoPInvokeCallback(typeof(MatDataAllocatorInvoke.MatAllocateCallback))]
#endif
      static internal IntPtr AllocateCallback(CvEnum.DepthType type, int channels, int totalInBytes, IntPtr allocateDataActionPtr)
      {
         GCHandle handle = GCHandle.FromIntPtr(allocateDataActionPtr);
         AllocateDataAction allocateDataAction = (AllocateDataAction)handle.Target;
         return allocateDataAction(type, channels, totalInBytes);
      }

#if IOS
      [ObjCRuntime.MonoPInvokeCallback(typeof(MatDataAllocatorInvoke.MatDeallocateCallback))]
#endif
      static internal void DeallocateCallback(IntPtr freeDataActionPtr)
      {
         GCHandle handle = GCHandle.FromIntPtr(freeDataActionPtr);
         FreeDataAction freeDataAction = (FreeDataAction)handle.Target;
         freeDataAction();
      }

      private void FreeData()
      {
         if (_dataHandle.IsAllocated)
            _dataHandle.Free();
         if (_data != null)
            _data = null;
      }

      /// <summary>
      /// Release resource associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_memoryAllocator != IntPtr.Zero)
            MatDataAllocatorInvoke.cveMatAllocatorRelease(ref _memoryAllocator);

         if (_allocateDataActionPtr != IntPtr.Zero)
         {
            GCHandle.FromIntPtr(_allocateDataActionPtr).Free();
            _allocateDataActionPtr = IntPtr.Zero;
         }
         if (_freeDataActionPtr != IntPtr.Zero)
         {
            GCHandle.FromIntPtr(_freeDataActionPtr).Free();
            _freeDataActionPtr = IntPtr.Zero;
         }

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
      internal delegate IntPtr MatAllocateCallback(CvEnum.DepthType type, int channels, int totalInBytes, IntPtr allocateDataActionPtr);

      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      internal delegate void MatDeallocateCallback(IntPtr data);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatAllocatorRelease(ref IntPtr allocator);
   }
}
