//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Cache the size of various header in bytes
   /// </summary>
   public static class StructSize
   {
      private static readonly int _PointF;
      private static readonly int _RangF;
      private static readonly int _MCvMat;
      private static readonly int _MCvSeq;
      private static readonly int _MCvContour;
      private static readonly int _MIplImage;
      private static readonly int _MCvSeqBlock;
      private static readonly int _MCvPoint3D32f;
      private static readonly int _MCvMatND;
      private static readonly int _MCvPoint2D64f;
      //private static readonly int _MCvHistogram;
      private static readonly int _MCvBlob;

      /// <summary>
      /// The size of PointF
      /// </summary>
      public static int PointF
      {
         get { return _PointF; }
      }

      /// <summary>
      /// The size of RangF
      /// </summary>
      public static int RangF
      {
         get { return _RangF; }
      }

      /// <summary>
      /// The size of PointF
      /// </summary>
      public static int MCvPoint2D64f
      {
         get { return _MCvPoint2D64f; }
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
      /// The size of CvSeqBlock
      /// </summary>
      public static int MCvSeqBlock
      {
         get { return _MCvSeqBlock; }
      }

      /// <summary>
      /// The size of MCvPoint3D32f
      /// </summary>
      public static int MCvPoint3D32f
      {
         get { return _MCvPoint3D32f; }
      }

      /// <summary>
      /// The size of MCvMatND
      /// </summary>
      public static int MCvMatND
      {
         get { return _MCvMatND; }
      }

      /*
      /// <summary>
      /// The size of MCvHistogram
      /// </summary>
      public static int MCvHistogram
      {
         get { return _MCvHistogram; }
      }*/

      /// <summary>
      /// The size of MCvBlob
      /// </summary>
      public static int MCvBlob
      {
         get { return _MCvBlob; }
      }

      static StructSize()
      {
         _PointF = Toolbox.SizeOf<System.Drawing.PointF>();
         _RangF = Toolbox.SizeOf<RangeF>();
         _MCvMat = Toolbox.SizeOf<MCvMat>();
         _MCvSeq = Toolbox.SizeOf<MCvSeq>();
         _MCvContour = Toolbox.SizeOf<MCvContour>();
         _MIplImage = Toolbox.SizeOf<MIplImage>();
         _MCvSeqBlock = Toolbox.SizeOf<MCvSeqBlock>();
         _MCvPoint3D32f = Toolbox.SizeOf<MCvPoint3D32f>();
         _MCvMatND = Toolbox.SizeOf<MCvMatND>();
         _MCvPoint2D64f = Toolbox.SizeOf<MCvPoint2D64f>();
         //_MCvHistogram = Toolbox.SizeOf(typeof(MCvHistogram));
         _MCvBlob = Toolbox.SizeOf<MCvBlob>();
      }
   }
}
