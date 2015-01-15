//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of Byte.
   /// </summary>
   public class VectorOfDataMatrixCode : Emgu.Util.UnmanagedObject
   {
      static VectorOfDataMatrixCode()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create an empty vector of DataMatrixCode
      /// </summary>
      public VectorOfDataMatrixCode()
      {
         _ptr = VectorOfDataMatrixCodeCreate();
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return VectorOfDataMatrixCodeGetSize(_ptr);
         }
      }

      /// <summary>
      /// Extract the DataMatrixCode from the image
      /// </summary>
      /// <param name="image">The image where DataMatrixCode will be extracted from</param>
      public void Find(Image<Gray, Byte> image)
      {
         CvInvoke.VectorOfDataMatrixCodeFind(_ptr, image);
      }

      /// <summary>
      /// Draw the DataMatrixCode on the image
      /// </summary>
      /// <param name="image">The image which the DataMatrixCode will be drawn to</param>
      public void Draw(Image<Gray, Byte> image)
      {
         CvInvoke.VectorOfDataMatrixCodeDraw(_ptr, image);
      }
      
      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            VectorOfDataMatrixCodeRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeCreateSize(int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeRelease(ref IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfDataMatrixCodeGetSize(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeClear(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeGetItem(IntPtr v, int index);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeFind(IntPtr v, IntPtr image);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeDraw(IntPtr v, IntPtr image);
   }
}
*/