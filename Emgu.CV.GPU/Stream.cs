using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Encapculates Cuda Stream. Provides interface for async coping.
   /// Passed to each function that supports async kernel execution.
   /// Reference counting is enabled
   /// </summary>
   public class Stream :UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr streamCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void streamRelease(ref IntPtr stream);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void streamWaitForCompletion(IntPtr stream);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool streamQueryIfComplete(IntPtr stream);
      #endregion

      /// <summary>
      /// Create a new Cuda Stream
      /// </summary>
      public Stream()
      {
         _ptr = streamCreate();
      }

      /// <summary>
      /// Wait for the completion
      /// </summary>
      public void WaitForCompletion()
      {
         streamWaitForCompletion(_ptr);
      }

      /// <summary>
      /// Check if the stream is completed
      /// </summary>
      public bool Completed
      {
         get { return streamQueryIfComplete(_ptr); }
      }

      /// <summary>
      /// Release the stream
      /// </summary>
      protected override void DisposeObject()
      {
         streamRelease(ref _ptr);
      }
   }
}
