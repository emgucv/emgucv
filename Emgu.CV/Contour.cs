//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapped class for Contour
   /// </summary>
   /// <typeparam name="T">The type of elements in the Contour, either PointF or Point.</typeparam>
   public class Contour<T> : Seq<T> where T : struct
   {
      /// <summary>
      /// Craete a contour from the specific IntPtr and storage
      /// </summary>
      /// <param name="ptr">The unmanged Pointer to the sequence</param>
      /// <param name="storage">The storage used by this contour</param>
      public Contour(IntPtr ptr, MemStorage storage)
         : base(ptr, storage)
      {
      }

      /// <summary>
      /// Create a contour using the specific <paramref name="seqFlag"/> and <paramref name="storage"/>
      /// </summary>
      /// <param name="seqFlag">Flags of the created contour. If the contour is not passed to any function working with a specific type of sequences, the sequence value may be set to 0, otherwise the appropriate type must be selected from the list of predefined contour types</param>
      /// <param name="storage">The storage</param>
      public Contour(int seqFlag, MemStorage storage)
         : this(IntPtr.Zero, storage)
      {
         _ptr = CvInvoke.cvCreateSeq(
             FixElementType(seqFlag),
             StructSize.MCvContour,
             _sizeOfElement,
             storage.Ptr);
      }

      /// <summary>
      /// Create a contour of the specific kind, type and flag
      /// </summary>
      /// <param name="kind">The kind of the sequence</param>
      /// <param name="eltype">The type of the sequence</param>
      /// <param name="flag">The flag of the sequence</param>
      /// <param name="stor">The storage</param>
      public Contour(CvEnum.SeqEltype eltype, CvEnum.SeqKind kind, CvEnum.SeqFlag flag, MemStorage stor)
         : this(((int)kind | (int)eltype | (int)flag), stor)
      {
      }

      /// <summary>
      /// Create a contour using the specific <paramref name="storage"/>
      /// </summary>
      /// <param name="storage">The storage to be used</param>
      public Contour(MemStorage storage)
         : this((int)CvEnum.SeqType.Polygon, storage)
      {
      }


      /// <summary>
      /// Return the MCvContour structure
      /// </summary>
      public MCvContour MCvContour
      {
         get
         {
#if NETFX_CORE
            return Marshal.PtrToStructure<MCvContour>(Ptr);
#else
            return (MCvContour)Marshal.PtrToStructure(Ptr, typeof(MCvContour));
#endif
         }
      }

      /// <summary>
      /// Same as h_next pointer in CvSeq
      /// </summary>
      public new Contour<T> HNext
      {
         get
         {
            MCvContour seq = MCvContour;
            return seq.h_next == IntPtr.Zero ? null : new Contour<T>(seq.h_next, Storage);
         }
         set
         {
            MCvContour seq = MCvContour;
            seq.h_next = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Same as h_prev pointer in CvSeq
      /// </summary>
      public new Contour<T> HPrev
      {
         get
         {
            MCvContour seq = MCvContour;
            return seq.h_prev == IntPtr.Zero ? null : new Contour<T>(seq.h_prev, Storage);
         }
         set
         {
            MCvContour seq = MCvContour;
            seq.h_prev = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Same as v_next pointer in CvSeq
      /// </summary>
      public new Contour<T> VNext
      {
         get
         {
            MCvContour seq = MCvContour;
            return seq.v_next == IntPtr.Zero ? null : new Contour<T>(seq.v_next, Storage);
         }
         set
         {
            MCvContour seq = MCvContour;
            seq.v_next = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Same as v_prev pointer in CvSeq
      /// </summary>
      public new Contour<T> VPrev
      {
         get
         {
            MCvContour seq = MCvContour;
            return seq.v_prev == IntPtr.Zero ? null : new Contour<T>(seq.v_prev, Storage);
         }
         set
         {
            MCvContour seq = MCvContour;
            seq.v_prev = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

   }
}
*/