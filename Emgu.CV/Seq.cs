//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   ///<summary>
   /// Wrapper to cvSeq of OpenCV. 
   ///</summary>
   ///<typeparam name="T">The type of elements in this sequence, must be a structure</typeparam>
   public class Seq<T> : IEnumerable<T> where T : struct
   {
      /// <summary>
      /// The pointer to this sequence
      /// </summary>
      protected IntPtr _ptr;

      /// <summary>
      /// Get the pointer of this sequence
      /// </summary>
      public IntPtr Ptr
      {
         get
         {
            return _ptr;
         }
      }

      /// <summary>
      /// The pointer to the storage used by this sequence
      /// </summary>
      protected readonly MemStorage _stor;

      /// <summary>
      /// The size of the elements in this sequence
      /// </summary>
      protected static readonly int _sizeOfElement = Marshal.SizeOf(typeof(T));

      #region Constructors
      /// <summary>
      /// Create a sequence using the specific <paramref name="seqFlag"/> and <paramref name="storage"/>
      /// </summary>
      /// <param name="seqFlag">Flags of the created sequence. If the sequence is not passed to any function working with a specific type of sequences, the sequence value may be set to 0, otherwise the appropriate type must be selected from the list of predefined sequence types</param>
      /// <param name="storage">the storage</param>
      public Seq(int seqFlag, MemStorage storage)
      {
         _stor = storage;
         _ptr = CvInvoke.cvCreateSeq(
             FixElementType(seqFlag),
             StructSize.MCvSeq,
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
      public Seq(CvEnum.SEQ_ELTYPE eltype, CvEnum.SEQ_KIND kind, CvEnum.SEQ_FLAG flag, MemStorage stor)
         : this(((int)kind | (int)eltype | (int)flag), stor)
      {
      }

      /// <summary>
      /// Create a sequence using the specific <paramref name="storage"/>
      /// </summary>
      /// <param name="storage">the storage</param>
      public Seq(MemStorage storage)
         : this(0, storage)
      {
      }

      /// <summary>
      /// Fix the input element type and return the correct one
      /// </summary>
      /// <param name="seqType">The input sequence type</param>
      /// <returns>The best element type that match this sequence</returns>
      protected static int FixElementType(int seqType)
      {
         int elementTypeID;

         Type elementType = typeof(T);

         if (elementType == typeof(PointF))
         {
            elementTypeID = CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2);
         }
         else if (elementType == typeof(Point))
         {
            elementTypeID = CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32S, 2);
         }
         else if (elementType == typeof(MCvPoint2D64f))
         {
            elementTypeID = CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_64F, 2);
         }
         else
         {  // if no match found simply return the original value
            return seqType;
         }

         return (seqType & (~(int)CvEnum.SeqConst.CV_SEQ_ELTYPE_MASK)) + elementTypeID;
      }

      /// <summary>
      /// Get or Set the element Type
      /// </summary>
      protected int ElementType
      {
         get
         {
            return MCvSeq.flags & (int)CvEnum.SeqConst.CV_SEQ_ELTYPE_MASK;
         }
         set
         {
            if (ElementType != value)
               Marshal.WriteInt32(
                  new IntPtr(Ptr.ToInt64() + Marshal.OffsetOf(typeof(MCvSeq), "flags").ToInt64()), 
                  (MCvSeq.flags & (~ (int)CvEnum.SeqConst.CV_SEQ_ELTYPE_MASK )) + value);
         }
      }

      /// <summary>
      /// Create a sequence from the unmanaged pointer and the storage used by the pointer
      /// </summary>
      /// <param name="seq">The unmanaged sequence</param>
      /// <param name="storage">The memory storage this sequence utilize</param>
      public Seq(IntPtr seq, MemStorage storage)
      {
         _ptr = seq;
         _stor = storage;
      }
      #endregion

      #region Push functions
      /// <summary>
      /// Push the data to the sequence
      /// </summary>
      /// <param name="data">The data to be pushed into the sequence</param>
      public void Push(T data)
      {
         IntPtr dataCopy = Marshal.AllocHGlobal(_sizeOfElement);
         Marshal.StructureToPtr(data, dataCopy, false);
         CvInvoke.cvSeqPush(Ptr, dataCopy);
         Marshal.FreeHGlobal(dataCopy); ;
      }

      /// <summary>
      /// Push the data to the sequence
      /// </summary>
      /// <param name="data">The data to be pushed into the sequence</param>
      public void PushFront(T data)
      {
         IntPtr dataCopy = Marshal.AllocHGlobal(_sizeOfElement);
         Marshal.StructureToPtr(data, dataCopy, false);
         CvInvoke.cvSeqPushFront(Ptr, dataCopy);

         Marshal.FreeHGlobal(dataCopy); ;
      }

      /// <summary>
      /// Push multiple elements to the sequence
      /// </summary>
      /// <param name="data">The data to push to the sequence</param>
      /// <param name="backOrFront">Specify if pushing to the back or front</param>
      public void PushMulti(T[] data, CvEnum.BACK_OR_FRONT backOrFront)
      {
         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         CvInvoke.cvSeqPushMulti(Ptr, handle.AddrOfPinnedObject(), data.Length, backOrFront);
         handle.Free();
      }
      #endregion 

      #region Pop functions
      /// <summary>
      /// Pop an element from the back of the sequence 
      /// </summary>
      /// <returns>An element from the back of the sequence</returns>
      public T Pop()
      {
         IntPtr dataCopy = Marshal.AllocHGlobal(_sizeOfElement);
         CvInvoke.cvSeqPop(Ptr, dataCopy);
         T res = (T)Marshal.PtrToStructure(dataCopy, typeof(T));
         Marshal.FreeHGlobal(dataCopy);
         return res;
      }

      /// <summary>
      /// Pop an element from the front of the sequence 
      /// </summary>
      /// <returns>An element from the front of the sequence</returns>
      public T PopFront()
      {
         IntPtr dataCopy = Marshal.AllocHGlobal(_sizeOfElement);
         CvInvoke.cvSeqPopFront(Ptr, dataCopy);
         T res = (T)Marshal.PtrToStructure(dataCopy, typeof(T));
         Marshal.FreeHGlobal(dataCopy);
         return res;
      }

      /// <summary>
      /// Pop multiple elements from the sequence
      /// </summary>
      /// <param name="count">The number of elements to be poped</param>
      /// <param name="backOrFront">The location the pop operation is started</param>
      /// <returns>The elements poped from the sequence</returns>
      public T[] PopMulti(int count, CvEnum.BACK_OR_FRONT backOrFront)
      {
         count = Math.Min(count, Total);
         T[] res = new T[count];
         GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
         CvInvoke.cvSeqPopMulti(Ptr, handle.AddrOfPinnedObject(), count, backOrFront);
         handle.Free();
         return res;
      }
      #endregion

      /// <summary>
      /// Removes element from sequence middle
      /// </summary>
      /// <param name="index">Index of removed element</param>
      public void RemoveAt(int index)
      {
         CvInvoke.cvSeqRemove(Ptr, index);
      }

      /// <summary>
      /// Inserts element in sequence middle
      /// </summary>
      /// <param name="index">Index before which the element is inserted. Inserting before 0 (the minimal allowed value of the parameter) is equal to cvSeqPushFront and inserting before seq->total (the maximal allowed value of the parameter) is equal to cvSeqPush</param>
      /// <param name="data">Inserted element</param>
      public void Insert(int index, T data)
      {
         IntPtr dataCopy = Marshal.AllocHGlobal(_sizeOfElement);
         Marshal.StructureToPtr(data, dataCopy, false);
         CvInvoke.cvSeqInsert(Ptr, index, dataCopy);
         Marshal.FreeHGlobal(dataCopy);
      }

      /// <summary>
      /// A Pointer to the storage used by this Seq
      /// </summary>
      public MemStorage Storage { get { return _stor; } }

      /// <summary>
      /// Get the MCvSeq structure
      /// </summary>
      public MCvSeq MCvSeq
      {
         get { return (MCvSeq)Marshal.PtrToStructure(Ptr, typeof(MCvSeq)); }
      }

      /// <summary>
      /// Get the minimum area rectangle for this point sequence
      /// </summary>
      /// <returns>The minimum area rectangle</returns>
      public MCvBox2D GetMinAreaRect()
      {
         return GetMinAreaRect(null);
      }

      /// <summary>
      /// Get the minimum area rectangle for this point sequence
      /// </summary>
      /// <param name="stor">The temporary storage to use</param>
      /// <returns>The minimum area rectangle</returns>
      public MCvBox2D GetMinAreaRect(MemStorage stor)
      {
         return CvInvoke.cvMinAreaRect2(Ptr, stor == null ? IntPtr.Zero : stor.Ptr);
      }

      /// <summary>
      /// Get the convex hull of this point sequence
      /// </summary>
      /// <param name="orientation">The orientation of the convex hull</param>
      /// <param name="stor">The storage for the resulting sequence</param>
      /// <returns>The result convex hull</returns>
      public Seq<T> GetConvexHull(CvEnum.ORIENTATION orientation, MemStorage stor)
      {
         IntPtr hull = CvInvoke.cvConvexHull2(Ptr, stor, orientation, 1);
         return new Seq<T>(hull, stor);
      }

      /// <summary>
      /// Get the convex hull of this point sequence, the resulting convex hull use the same storage as the current sequence
      /// </summary>
      /// <param name="orientation">The orientation of the convex hull</param>
      /// <returns>The result convex hull</returns>
      public Seq<T> GetConvexHull(CvEnum.ORIENTATION orientation)
      {
         return GetConvexHull(orientation, _stor);
      }

      /// <summary>
      /// Obtain the <paramref name="index"/> element in this sequence
      /// </summary>
      /// <param name="index">the index of the element</param>
      /// <returns>the <paramref name="index"/> element in this sequence</returns>
      public T this[int index]
      {
         get
         {
            return (T)Marshal.PtrToStructure(CvInvoke.cvGetSeqElem(_ptr, index), typeof(T));
         }
      }

      /// <summary>
      /// Convert this sequence to array
      /// </summary>
      /// <returns>the array representation of this sequence</returns>
      public T[] ToArray()
      {
         T[] res = new T[Total];
         if (res.Length == 0) 
            return res;
         GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
         CvInvoke.cvCvtSeqToArray(Ptr, handle.AddrOfPinnedObject(), MCvSlice.WholeSeq);
         handle.Free();
         return res;
      }

      /// <summary>
      /// return an enumerator of the elements in the sequence
      /// </summary>
      /// <returns>an enumerator of the elements in the sequence</returns>
      public IEnumerator<T> GetEnumerator()
      {
         using (PinnedArray<T> buffer = new PinnedArray<T>(1))
         {
            IntPtr address = buffer.AddrOfPinnedObject();
            for (int i = 0; i < Total; i++)
            {
               Toolbox.memcpy(address, CvInvoke.cvGetSeqElem(_ptr, i), _sizeOfElement);
               yield return buffer.Array[0];
               //yield return (T)Marshal.PtrToStructure(CvInvoke.cvGetSeqElem(_ptr, i), typeof(T));
               //yield return this[i];
            }
         }
      }

      /// <summary>
      /// return an enumerator of the elements in the sequence
      /// </summary>
      /// <returns>an enumerator of the elements in the sequence</returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      /// Same as h_next pointer in CvSeq
      /// </summary>
      public Seq<T> HNext
      {
         get
         {
            MCvSeq seq = MCvSeq;
            return seq.h_next == IntPtr.Zero ? null : new Seq<T>(seq.h_next, Storage);
         }
         set
         {
            MCvSeq seq = MCvSeq;
            seq.h_next = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Same as h_prev pointer in CvSeq
      /// </summary>
      public Seq<T> HPrev
      {
         get
         {
            MCvSeq seq = MCvSeq;
            return seq.h_prev == IntPtr.Zero ? null : new Seq<T>(seq.h_prev, Storage);
         }
         set
         {
            MCvSeq seq = MCvSeq;
            seq.h_prev = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }

      }

      /// <summary>
      /// Same as v_next pointer in CvSeq
      /// </summary>
      public Seq<T> VNext
      {
         get
         {
            MCvSeq seq = MCvSeq;
            return seq.v_next == IntPtr.Zero ? null : new Seq<T>(seq.v_next, Storage);
         }
         set
         {
            MCvSeq seq = MCvSeq;
            seq.v_next = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Same as v_prev pointer in CvSeq
      /// </summary>
      public Seq<T> VPrev
      {
         get
         {
            MCvSeq seq = MCvSeq;
            return seq.v_prev == IntPtr.Zero ? null : new Seq<T>(seq.v_prev, Storage);
         }
         set
         {
            MCvSeq seq = MCvSeq;
            seq.v_prev = value == null ? IntPtr.Zero : value.Ptr;
            Marshal.StructureToPtr(seq, _ptr, false);
         }
      }

      /// <summary>
      /// Creates a sequence that represents the specified slice of the input sequence. The new sequence either shares the elements with the original sequence or has own copy of the elements. So if one needs to process a part of sequence but the processing function does not have a slice parameter, the required sub-sequence may be extracted using this function
      /// </summary>
      /// <param name="slice">The part of the sequence to extract</param>
      /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is NULL, the function uses the storage containing the input sequence.</param>
      /// <param name="copy_data">The flag that indicates whether to copy the elements of the extracted slice </param>
      /// <returns>A sequence that represents the specified slice of the input sequence</returns>
      public Seq<T> Slice(MCvSlice slice, MemStorage storage, bool copy_data)
      {
         return new Seq<T>(CvInvoke.cvSeqSlice(Ptr, slice, storage.Ptr, copy_data), storage);
      }

      ///<summary> Get the number of eelments in the sequence</summary>
      public int Total
      {
         get { return MCvSeq.total; }
      }

      ///<summary> 
      /// Get the area of the contour 
      ///</summary>
      public double Area
      {
         get
         {
            return CvInvoke.cvContourArea(Ptr, MCvSlice.WholeSeq, 0);
         }
      }

      ///<summary> 
      /// Indicate if the coutour is a convex one 
      ///</summary>
      public bool Convex
      {
         get
         {
            return CvInvoke.cvCheckContourConvexity(Ptr) != 0;
         }
      }

      ///<summary> 
      /// The perimeter of the sequence 
      ///</summary>
      public double Perimeter
      {
         get
         {
            return Math.Abs(CvInvoke.cvContourPerimeter(Ptr));
         }
      }

      /// <summary>
      /// Approximates one curves and returns the approximation result
      /// </summary>
      /// <param name="accuracy">The desired approximation accuracy</param>
      /// <param name="storage"> The storage used by the resulting sequence. If null, the storage of this sequence is used.</param>
      /// <returns>The approximated contour</returns>
      public Seq<T> ApproxPoly(double accuracy, MemStorage storage)
      {
         return ApproxPoly(accuracy, 0, storage);
      }

      /// <summary>
      /// Approximates one or more curves and returns the approximation result[s]. In case of multiple curves approximation the resultant tree will have the same structure as the input one (1:1 correspondence)
      /// </summary>
      /// <param name="accuracy">The desired approximation accuracy</param>
      /// <param name="storage"> The storage used by the resulting sequence. If null, the storage of this sequence is used.</param>
      /// <param name="maxLevel">
      /// Maximal level for sequence approximation. 
      /// If 0, only sequence is arrpoximated. 
      /// If 1, the sequence and all sequence after it on the same level are approximated. 
      /// If 2, all sequence after and all sequence one level below the contours are approximated, etc. If the value is negative, the function does not approximate the sequence following after contour but draws child sequences of sequence up to abs(maxLevel)-1 level
      /// </param>
      /// <returns>The approximated contour</returns>
      public Seq<T> ApproxPoly(double accuracy, int maxLevel, MemStorage storage)
      {
         MemStorage stor = storage ?? Storage;
         return new Seq<T>(
             CvInvoke.cvApproxPoly(
             Ptr,
             StructSize.MCvContour,
             stor.Ptr,
             CvEnum.APPROX_POLY_TYPE.CV_POLY_APPROX_DP,
             accuracy,
             maxLevel),
             stor);
      }

      /// <summary>
      /// Approximates one curve and returns the approximation result, the result use the same storage as the current sequence
      /// </summary>
      /// <param name="accuracy">The desired approximation accuracy</param>
      /// <returns>The approximated contour</returns>
      public Seq<T> ApproxPoly(double accuracy)
      {
         return ApproxPoly(accuracy, _stor);
      }

      ///<summary> Get the smallest bouding rectangle </summary>
      public virtual Rectangle BoundingRectangle
      {
         get
         {
            return CvInvoke.cvBoundingRect(Ptr, false);
         }
      }

      /// <summary>
      /// Removes all elements from the sequence. The function does not return the memory to the storage, but this memory is reused later when new elements are added to the sequence. This function time complexity is O(1). 
      /// </summary>
      public void Clear()
      {
         CvInvoke.cvClearSeq(Ptr);
      }

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex)
      /// </summary>
      /// <param name="point">The point to be tested</param>
      /// <returns>positive if inside; negative if out side; 0 if on the contour</returns>
      public virtual double InContour(PointF point)
      {
         return CvInvoke.cvPointPolygonTest(Ptr, point, 0);
      }

      /// <summary>
      /// Determines the distance from the point to the contour
      /// </summary>
      /// <param name="point">The point to measured distance</param>
      /// <returns>positive distance if inside; negative distance if outside; 0 if on the contour</returns>
      public double Distance(PointF point)
      {
         return CvInvoke.cvPointPolygonTest(Ptr, point, 1);
      }

      /// <summary>
      /// Get the moments for this point sequence
      /// </summary>
      /// <returns>the moments for this point sequence</returns>
      public MCvMoments GetMoments()
      {
         MCvMoments moment = new MCvMoments();
         CvInvoke.cvMoments(Ptr, ref moment, 0);
         return moment;
      }

      /// <summary>
      /// Compare the shape of the current contour with <paramref name="objectToMatch"/> 
      /// </summary>
      /// <param name="objectToMatch">The object to match</param>
      /// <param name="method">contour matching method</param>
      /// <returns>The degree of the similarity</returns>
      public double MatchShapes(Seq<T> objectToMatch, CvEnum.CONTOURS_MATCH_TYPE method)
      {
         return CvInvoke.cvMatchShapes(Ptr, objectToMatch.Ptr, method, 0.0);
      }

      /// <summary>
      /// Finds all convexity defects of the input contour and returns a sequence of the CvConvexityDefect structures. 
      /// </summary>
      /// <param name="storage">Container for output sequence of convexity defects. If it is NULL, contour or hull (in that order) storage is used.</param>
      /// <param name="orientation">Orientation where the convexity Defacts is returned.</param>
      /// <returns>The sequence of the CvConvexityDefect structures.</returns>
      public Seq<MCvConvexityDefect> GetConvexityDefacts(MemStorage storage, Emgu.CV.CvEnum.ORIENTATION orientation)
      {
         MemStorage stor = storage ?? Storage;
         IntPtr convexHull = CvInvoke.cvConvexHull2(Ptr, stor, orientation, 0);
         IntPtr seq = CvInvoke.cvConvexityDefects(Ptr, convexHull, stor);
         return new Seq<MCvConvexityDefect>(seq, stor);
      }

      /// <summary>
      /// Implicit operator for IntPtr
      /// </summary>
      /// <param name="sequence">The sequence</param>
      /// <returns>The unmanaged pointer for this object</returns>
      public static implicit operator IntPtr(Seq<T> sequence)
      {
         return sequence == null ? IntPtr.Zero : sequence._ptr;
      }
   }
}
