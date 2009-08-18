using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvBGCodeBookModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public class MCvBGCodeBookModel
   {
      /// <summary>
      /// 
      /// </summary>
      public Size size;
      /// <summary>
      /// 
      /// </summary>
      public int t;
      /// <summary>
      /// 
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public Byte[] cbBounds;
      /// <summary>
      /// 
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public Byte[] modMin;
      /// <summary>
      /// 
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      public Byte[] modMax;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr cbmap;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr storage;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr freeList;
   }
}
