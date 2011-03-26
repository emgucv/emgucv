//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   /// <summary>
   /// A DataLogger for unmanaged code to log data back to managed code, using callback.
   /// </summary>
   public class DataLogger : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention=CvInvoke.CvCallingConvention)]
      private static extern IntPtr DataLoggerCreate(int logLevel);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void DataLoggerRelease(ref IntPtr logger);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void DataLoggerRegisterCallback(
         IntPtr logger,
         DataCallback messageCallback);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void DataLoggerLog(
         IntPtr logger, 
         IntPtr data,
         int logLevel);
      #endregion

      /// <summary>
      /// The event that will be raised when the unmanaged code send over data
      /// </summary>
      public event DataCallbackHandler OnDataReceived;

      /// <summary>
      /// Define the type of callback when data is received
      /// </summary>
      /// <param name="sender">The DataLogger that send the message</param>
      /// <param name="e">The data</param>
      public delegate void DataCallbackHandler(object sender, EventArgs<IntPtr> e);

      private DataCallback _handler;

      /// <summary>
      /// Create a MessageLogger and register the callback function
      /// </summary>
      /// <param name="logLevel">The log level.</param>
      public DataLogger(int logLevel)
      {
         _ptr = DataLoggerCreate(logLevel);
         _handler = DataHandler;
         DataLoggerRegisterCallback(_ptr, _handler);
      }

      /// <summary>
      /// Log some data
      /// </summary>
      /// <param name="data">Pointer to some unmanaged data</param>
      /// <param name="logLevel">The logLevel. The Log function only logs when the <paramref name="logLevel"/> is greater or equals to the DataLogger's logLevel</param>
      public void Log(IntPtr data, int logLevel)
      {
         DataLoggerLog(_ptr, data, logLevel);
      }

      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      private delegate void DataCallback(IntPtr data);

      private void DataHandler(IntPtr data)
      {
         if (OnDataReceived != null)
            OnDataReceived(this, new EventArgs<IntPtr>(data));
      }

      /// <summary>
      /// Release the DataLogger and all the unmanaged memory associated with it.
      /// </summary>
      protected override void DisposeObject()
      {
         DataLoggerRelease(ref _ptr);
      }
   }

   /// <summary>
   /// A generic version of the DataLogger
   /// </summary>
   /// <typeparam name="T">The supported type includes System.String and System.ValueType</typeparam>
   public class DataLogger<T> : DisposableObject
   {
      private DataLogger _logger; 

      /// <summary>
      /// Define the type of callback when data is received
      /// </summary>
      /// <param name="sender">The DataLogger that send the message</param>
      /// <param name="e">The data</param>
      public delegate void DataCallbackHandler(object sender, EventArgs<T> e);

      private DataLogger.DataCallbackHandler _handler;

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
      public event DataCallbackHandler OnDataReceived;

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
         }
         else
         {
            IntPtr unmanagedData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
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
               result = (T)((Object)Marshal.PtrToStringAnsi(e.Value));
            }
            else
            {
               result = (T)Marshal.PtrToStructure(e.Value, typeof(T));
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
         _logger.Dispose();
      }
   }
}
