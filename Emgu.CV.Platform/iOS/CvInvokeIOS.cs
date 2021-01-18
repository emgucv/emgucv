//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary>
    /// CvInvoke for iOS
    /// </summary>
    public static class CvInvokeIOS
    {

        private static readonly bool _libraryLoaded;

        static CvInvokeIOS()
        {
            _libraryLoaded = CvInvoke.Init();
            if (_libraryLoaded)
                CvInvoke.RedirectError(CvInvokeIOS.CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Return true if the class is loaded.
        /// </summary>
        public static bool Init()
        {
            return _libraryLoaded;
        }

        /// <summary>
        /// The default Exception callback to handle Error thrown by OpenCV
        /// </summary>
        public static readonly CvInvoke.CvErrorCallback CvErrorHandlerThrowException = (CvInvoke.CvErrorCallback)CvErrorHandler;

        /// <summary>
        /// An error handler which will ignore any error and continue
        /// </summary>
        public static readonly CvInvoke.CvErrorCallback CvErrorHandlerIgnoreError = (CvInvoke.CvErrorCallback)CvIgnoreErrorErrorHandler;

        /// <summary>
        /// A custom error handler for OpenCV
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="funcName">The source file name where error is encountered</param>
        /// <param name="errMsg">A description of the error</param>
        /// <param name="fileName">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        /// <param name="userData">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <returns>if 0, signal the process to continue</returns>
        [ObjCRuntime.MonoPInvokeCallback(typeof(CvInvoke.CvErrorCallback))]
        private static int CvIgnoreErrorErrorHandler(
            int status,
            IntPtr funcName,
            IntPtr errMsg,
            IntPtr fileName,
            int line,
            IntPtr userData)
        {
            CvInvoke.SetErrStatus(Emgu.CV.CvEnum.ErrorCodes.StsOk); //clear the error status
            return 0; //signal the process to continue
        }

        /// <summary>
        /// A custom error handler for OpenCV
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="funcName">The source file name where error is encountered</param>
        /// <param name="errMsg">A description of the error</param>
        /// <param name="fileName">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        /// <param name="userData">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <returns>If 0, signal the process to continue</returns>
        [ObjCRuntime.MonoPInvokeCallback(typeof(CvInvoke.CvErrorCallback))]
        private static int CvErrorHandler(
            int status,
            IntPtr funcName,
            IntPtr errMsg,
            IntPtr fileName,
            int line,
            IntPtr userData)
        {
            try
            {
                CvInvoke.SetErrStatus(Emgu.CV.CvEnum.ErrorCodes.StsOk); //clear the error status
                return 0; //signal the process to continue
            }
            finally
            {
                String funcNameStr = Marshal.PtrToStringAnsi(funcName);
                String errMsgStr = Marshal.PtrToStringAnsi(errMsg);
                String fileNameStr = Marshal.PtrToStringAnsi(fileName);
                throw new CvException(status, funcNameStr, errMsgStr, fileNameStr, line);
            }
        }
    }
}
