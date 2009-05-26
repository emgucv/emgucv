using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvStarKeypoiny
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStarKeypoint
   {
      /// <summary>
      /// 
      /// </summary>
      public Point pt;
      /// <summary>
      /// 
      /// </summary>
      public int size;
      /// <summary>
      /// 
      /// </summary>
      public float response;
   }
}
