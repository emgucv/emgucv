using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Cache the size of various header in bytes
   /// </summary>
   public static class HeaderSize
   {
      private static int _PointF;
      private static int _MCvMat;
      private static int _MCvSeq;
      private static int _MCvContour;
      private static int _MIplImage;
      private static int _MCvSeqBlock;

      /// <summary>
      /// The size of PointF
      /// </summary>
      public static int PointF
      {
         get { return _PointF; }
      }

      /// <summary>
      /// The size of MCvMat
      /// </summary>
      public static int MCvMat
      {
         get { return _MCvMat; }
      }

      /// <summary>
      /// The size of MCvSeq
      /// </summary>
      public static int MCvSeq
      {
         get { return _MCvSeq; }
      }

      /// <summary>
      /// The size of MCvContour
      /// </summary>
      public static int MCvContour
      {
         get { return _MCvContour; }
      }

      /// <summary>
      /// The size of IplImage
      /// </summary>
      public static int MIplImage
      {
         get { return _MIplImage; }
      }

      /// <summary>
      /// The size of IplImage
      /// </summary>
      public static int MCvSeqBlock
      {
         get { return _MCvSeqBlock; }
      }

      static HeaderSize()
      {
         _PointF = Marshal.SizeOf(typeof(System.Drawing.PointF));
         _MCvMat = Marshal.SizeOf(typeof(MCvMat));
         _MCvSeq = Marshal.SizeOf(typeof(MCvSeq));
         _MCvContour = Marshal.SizeOf(typeof(MCvContour));
         _MIplImage = Marshal.SizeOf(typeof(MIplImage));
         _MCvSeqBlock = Marshal.SizeOf(typeof(MCvSeqBlock));
      }
   }
}
