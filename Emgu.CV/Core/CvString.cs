//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapper for cv::String. This class support UTF-8 chars.
   /// </summary>
   public class CvString : UnmanagedObject
   {
      private bool _needDispose;

      internal CvString(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create a CvString from System.String
      /// </summary>
      /// <param name="s">The System.String object to be converted to CvString</param>
      public CvString(String s)
      {
         if (s == null)
         {
            _ptr = CvInvoke.cveStringCreate();
         }
         else
         {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            Array.Resize(ref bytes, bytes.Length + 1);
            bytes[bytes.Length - 1] = 0; //The end of string '\0' character
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            _ptr = CvInvoke.cveStringCreateFromStr(handle.AddrOfPinnedObject());
            handle.Free();
         }
         
         _needDispose = true;
      }

      /// <summary>
      /// Create an empty CvString
      /// </summary>
      public CvString()
      {
         _ptr = CvInvoke.cveStringCreate();
         _needDispose = true;
      }

      /// <summary>
      /// Get the string representation of the CvString
      /// </summary>
      /// <returns>The string representation of the CvString</returns>
      public override string ToString()
      {
         IntPtr cStr = IntPtr.Zero;
         int size = 0;
         CvInvoke.cveStringGetCStr(_ptr, ref cStr, ref size);
         Byte[] data = new byte[size];
         Marshal.Copy(cStr, data, 0, size);

         return Encoding.UTF8.GetString(data, 0, data.Length);

      }

      /// <summary>
      /// Gets the length of the string
      /// </summary>
      /// <value>
      /// The length of the string
      /// </value>
      public int Length
      {
         get { return CvInvoke.cveStringGetLength(_ptr); }
      }

      /// <summary>
      /// Release all the unmanaged resource associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            CvInvoke.cveStringRelease(ref _ptr);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveStringCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveStringRelease(ref IntPtr str);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveStringGetLength(IntPtr str);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveStringGetCStr(IntPtr str, ref IntPtr cStr, ref int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveStringCreateFromStr(IntPtr c);
   }
}
