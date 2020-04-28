//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.Util
{
   /// <summary>
   /// A DataLogger for unmanaged code to log data back to managed code, using callback.
   /// </summary>
   public class DataLogger : UnmanagedObject
   {
      private int _loggerId;

      /// <summary>
      /// Create a MessageLogger and register the callback function
      /// </summary>
      /// <param name="logLevel">The log level.</param>
      public DataLogger(int logLevel)
      {
         lock (typeof(DataLoggerHelper))
         {
            _loggerId = DataLoggerHelper.TotalLoggerCount;
            DataLoggerHelper.TotalLoggerCount++;
         }
         
         _ptr = CvInvoke.DataLoggerCreate(logLevel, _loggerId);
         CvInvoke.DataLoggerRegisterCallback(_ptr, DataLoggerHelper.Handler);
         DataLoggerHelper.OnDataReceived += this.HelperDataHandler;
      }

      /// <summary>
      /// The event that will be raised when the unmanaged code send over data
      /// </summary>
      public event EventHandler<EventArgs<IntPtr>> OnDataReceived;

      private void HelperDataHandler(IntPtr data, int loggerId)
      {
         if (loggerId == _loggerId && OnDataReceived != null)
         {
            OnDataReceived(this, new EventArgs<IntPtr>(data));
         }
      }

      /// <summary>
      /// Log some data
      /// </summary>
      /// <param name="data">Pointer to some unmanaged data</param>
      /// <param name="logLevel">The logLevel. The Log function only logs when the <paramref name="logLevel"/> is greater or equals to the DataLogger's logLevel</param>
      public void Log(IntPtr data, int logLevel)
      {
         CvInvoke.DataLoggerLog(_ptr, data, logLevel);
      }

      /// <summary>
      /// Release the DataLogger and all the unmanaged memory associated with it.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.DataLoggerRelease(ref _ptr);
         DataLoggerHelper.OnDataReceived -= this.HelperDataHandler;
      }
   }

   /// <summary>
   /// A generic version of the DataLogger
   /// </summary>
   /// <typeparam name="T">The supported type includes System.String and System.ValueType</typeparam>
   public class DataLogger<T> : DisposableObject
   {
      private DataLogger _logger;

      private EventHandler<EventArgs<IntPtr>> _handler;

      /// <summary>
      /// Create a new DataLogger
      /// </summary>
      /// <param name="logLevel">The log level.</param>
      public DataLogger(int logLevel)
      {
         _logger = new DataLogger(logLevel);
         _handler = DataHandler;
         _logger.OnDataReceived += _handler;
      }

      /// <summary>
      /// The event that will be raised when the unmanaged code send over data
      /// </summary>
      public event EventHandler<EventArgs<T>> OnDataReceived;

      /// <summary>
      /// Log some data
      /// </summary>
      /// <param name="data">The data to be logged</param>
      /// <param name="logLevel">The logLevel. The Log function only logs when the <paramref name="logLevel"/> is greater or equals to the DataLogger's logLevel</param>
      public void Log(T data, int logLevel)
      {
         if (typeof(T) == typeof(String))
         {
            String d = data as String;
            IntPtr unmanagedData = Marshal.StringToHGlobalAnsi(d);
            _logger.Log(unmanagedData, logLevel);
            Marshal.FreeHGlobal(unmanagedData);
         } else
         {
            IntPtr unmanagedData = Marshal.AllocHGlobal(Toolbox.SizeOf<T>());

            Marshal.StructureToPtr(data, unmanagedData, false);
            _logger.Log(unmanagedData, logLevel);
            Marshal.FreeHGlobal(unmanagedData);
         }
      }

      private void DataHandler(Object sender, EventArgs<IntPtr> e)
      {
         if (OnDataReceived != null)
         {
            T result;
            if (typeof(T) == typeof(String))
            {
               result = (T) ((Object) Marshal.PtrToStringAnsi(e.Value));
            } else
            {
                result = Marshal.PtrToStructure<T>(e.Value);
            }
            OnDataReceived(this, new EventArgs<T>(result));
         }
      }

      /// <summary>
      /// Pointer to the unmanaged object
      /// </summary>
      public IntPtr Ptr
      {
         get
         {
            return _logger.Ptr;
         }
      }

      /// <summary>
      /// Implicit operator for IntPtr
      /// </summary>
      /// <param name="obj">The DataLogger</param>
      /// <returns>The unmanaged pointer for this DataLogger</returns>
      public static implicit operator IntPtr(DataLogger<T> obj)
      {
         return obj == null ? IntPtr.Zero : obj.Ptr;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this DataLogger
      /// </summary>
      protected override void DisposeObject()
      {
         _logger.OnDataReceived -= _handler;
         _logger.Dispose();
      }
   }

   internal static class DataLoggerHelper
   {
      /// <summary>
      /// The event that will be raised when the unmanaged code send over data
      /// </summary>
      public static event DataCallback OnDataReceived;

      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      public delegate void DataCallback(IntPtr data, int loggerId);

      public static DataCallback Handler = DataHandler;

#if __IOS__
      //[ObjCRuntime.MonoPInvokeCallback(typeof(DataLoggerHelper.DataCallback))]
#endif
      public static void DataHandler(IntPtr data, int loggerId)
      {
         if (OnDataReceived != null)
            OnDataReceived(data, loggerId);
      }

      public static volatile int TotalLoggerCount = 0;
   }
}
