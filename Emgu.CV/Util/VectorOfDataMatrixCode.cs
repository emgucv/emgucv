//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

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
      /// <summary>
      /// Create an empty vector of DataMatrixCode
      /// </summary>
      public VectorOfDataMatrixCode()
      {
         _ptr = CvInvoke.VectorOfDataMatrixCodeCreate();
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return CvInvoke.VectorOfDataMatrixCodeGetSize(_ptr);
         }
      }

      /*
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
      */
      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.VectorOfDataMatrixCodeRelease(_ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfDataMatrixCodeGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfDataMatrixCodeGetItem(IntPtr v, int index);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeFind(IntPtr v, IntPtr image);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfDataMatrixCodeDraw(IntPtr v, IntPtr image);
   }
}
