using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    ///<summary>
    ///The contour of an object
    ///</summary>
    public class Seq<T> : UnmanagedObject, IEnumerable<T> where T : struct 
    {
        /// <summary>
        /// The pointer to the storage used by this sequence
        /// </summary>
        protected MemStorage _stor;

        /// <summary>
        /// Create an empty contour
        /// </summary>
        /// <param name="storage"> The memory to be used for this contour</param>
        private Seq(MemStorage storage )
        {
            _stor = storage;
        }

        /// <summary>
        /// Create a sequence using the specific <paramref name="seq_flag"/> and <paramref name="storage"/>
        /// </summary>
        /// <param name="seq_flag">the sequence flag</param>
        /// <param name="storage">the storage</param>
        public Seq(int seq_flag, MemStorage storage)
            : this (storage)
        {
            _ptr = CvInvoke.cvCreateSeq(
                seq_flag, Marshal.SizeOf(typeof(MCvSeq)),
                Marshal.SizeOf(typeof(T)),
                storage.Ptr);
        }

        /// <summary>
        /// Create a sequence from the unmanaged pointer and the storage used by the pointer
        /// </summary>
        /// <param name="seq">The unmanaged sequence</param>
        /// <param name="storage">The memory storage this sequence utilize</param>
        public Seq(IntPtr seq, MemStorage storage)
            : this(storage)
        {
            _ptr = seq;
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
            int total = Total;
            T[] res = new T[total];
            for (int i = 0; i < total; i++)
                res[i] = this[i];
            return res;
        }

        /// <summary>
        /// return an enumerator of the elements in the sequence
        /// </summary>
        /// <returns>an enumerator of the elements in the sequence</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Total; i++)
                yield return this[i];
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
        }

        /// <summary>
        /// Creates a sequence that represents the specified slice of the input sequence. The new sequence either shares the elements with the original sequence or has own copy of the elements. So if one needs to process a part of sequence but the processing function does not have a slice parameter, the required sub-sequence may be extracted using this function
        /// </summary>
        /// <param name="slice">The part of the sequence to extract</param>
        /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is NULL, the function uses the storage containing the input sequence.</param>
        /// <param name="copy_data">The flag that indicates whether to copy the elements of the extracted slice </param>
        /// <returns></returns>
        public Seq<T> Slice(MCvSlice slice, MemStorage storage, bool copy_data)
        {
            return new Seq<T>(CvInvoke.cvSeqSlice(Ptr, slice, storage.Ptr, copy_data), storage);
        }

        ///<summary> The number of vertices in the contour</summary>
        public int Total { get { return ((MCvSeq)Marshal.PtrToStructure(Ptr, typeof(MCvSeq))).total; } }

        /// <summary>
        /// Release the contour and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
        }

        ///<summary> 
        /// Get the area of the contour 
        ///</summary>
        public double Area
        {
            get
            {
                return Math.Abs(CvInvoke.cvContourArea(Ptr, new MCvSlice(0, 0x3fffffff)));
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
        /// The function cvApproxPoly approximates one or more curves and returns the approximation result[s]. In case of multiple curves approximation the resultant tree will have the same structure as the input one (1:1 correspondence)
        /// </summary>
        /// <param name="accuracy">The desired approximation accuracy</param>
        /// <param name="storage"> The storage the resulting sequence use</param>
        /// <returns>The approximated contour</returns>
        public Seq<T> ApproxPoly(double accuracy, MemStorage storage)
        {
            return new Seq<T>(
                CvInvoke.cvApproxPoly(
                Ptr,
                System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)),
                storage.Ptr,
                CvEnum.APPROX_POLY_TYPE.CV_POLY_APPROX_DP,
                accuracy,
                0),
                storage);
        }

        ///<summary> The smallest Bouding Rectangle </summary>
        public Rectangle<double> BoundingRectangle
        {
            get
            {
                Rectangle<double> res = new Rectangle<double>();
                res.MCvRect = CvInvoke.cvBoundingRect(Ptr, false);
                return res;
            }
        }

        /// <summary>
        /// The function cvClearSeq removes all elements from the sequence. The function does not return the memory to the storage, but this memory is reused later when new elements are added to the sequence. This function time complexity is O(1). 
        /// </summary>
        public void Clear()
        {
            CvInvoke.cvClearSeq(Ptr);
        }
    };
}
