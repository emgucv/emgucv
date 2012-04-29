//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// Returns information about one of or all of the registered modules
      /// </summary>
      /// <param name="moduleName">Name of the module of interest, or IntPtr.Zero, which means all the modules.</param>
      /// <param name="version">Information about the module(s), including version</param>
      /// <param name="loadedAddonPlugins">The list of names and versions of the optimized plugins that CXCORE was able to find and load</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetModuleInfo(
         IntPtr moduleName,
         ref IntPtr version,
         ref IntPtr loadedAddonPlugins);

      #region Memory Management
      /*
      private static IntPtr DefaultCvAllocFunc(uint size, IntPtr UserData)
      {
         return Marshal.AllocHGlobal((int)size);
      }

      private static int DefaultCvFreeFunc(IntPtr ptr, IntPtr userData)
      {
         Marshal.FreeHGlobal(ptr);
      }*/

      /// <summary>
      /// Delegate used to allocate data by OpenCV
      /// </summary>
      /// <param name="size">Size of the memory to allocate</param>
      /// <param name="userData">User data that is transparetly passed to the custom functions</param>
      /// <returns>Pointer to the allocated memort</returns>
      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      public delegate IntPtr CvAllocFunc(uint size, IntPtr userData);

      /// <summary>
      /// Delegate used to dellocate OpenCV memory
      /// </summary>
      /// <param name="ptr">The memory to dellocate</param>
      /// <param name="userData">User data that is transparetly passed to the custom functions</param>
      /// <returns></returns>
      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      public delegate int CvFreeFunc(IntPtr ptr, IntPtr userData);

      /// <summary>
      /// The function cvSetMemoryManager sets user-defined memory managment functions (substitutors for malloc and free) that will be called by cvAlloc, cvFree and higher-level functions (e.g. cvCreateImage)
      /// </summary>
      /// <param name="allocFunc">Allocation function</param>
      /// <param name="freeFunc">Deallocation function</param>
      /// <param name="userdata">User data that is transparetly passed to the custom functions</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetMemoryManager(
         CvAllocFunc allocFunc,
         CvFreeFunc freeFunc,
         IntPtr userdata);

      #endregion

      #region Error handling
      /// <summary>
      /// The default Exception callback to handle Error thrown by OpenCV
      /// </summary>
      public static readonly CvErrorCallback CvErrorHandlerThrowException = (CvErrorCallback) CvErrorHandler;
      /// <summary>
      /// An error handler which will ignore any error and continute
      /// </summary>
      public static readonly CvErrorCallback CvErrorHandlerIgnoreError = (CvErrorCallback) CvIgnoreErrorErrorHandler;

      /// <summary>
      /// A custome error handler for opencv
      /// </summary>
      /// <param name="status">The numeric code for error status</param>
      /// <param name="funcName">The source file name where error is encountered</param>
      /// <param name="errMsg">A description of the error</param>
      /// <param name="fileName">The source file name where error is encountered</param>
      /// <param name="line">The line number in the souce where error is encountered</param>
      /// <param name="userData">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <returns></returns>
#if IOS
      [MonoTouch.MonoPInvokeCallback(typeof(CvErrorCallback))]
#endif
      private static int CvIgnoreErrorErrorHandler(
                  int status,
         String funcName,
         String errMsg,
         String fileName,
         int line,
         IntPtr userData)
      {
         cvSetErrStatus(Emgu.CV.CvEnum.ERROR_CODES.CV_STSOK); //clear the error status
         return 0; //signal the process to continute
      }

      /// <summary>
      /// A custome error handler for opencv
      /// </summary>
      /// <param name="status">The numeric code for error status</param>
      /// <param name="funcName">The source file name where error is encountered</param>
      /// <param name="errMsg">A description of the error</param>
      /// <param name="fileName">The source file name where error is encountered</param>
      /// <param name="line">The line number in the souce where error is encountered</param>
      /// <param name="userData">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <returns></returns>
#if IOS
      [MonoTouch.MonoPInvokeCallback(typeof(CvErrorCallback))]
#endif
      private static int CvErrorHandler(
         int status,
         String funcName,
         String errMsg,
         String fileName,
         int line,
         IntPtr userData)
      {
         try
         {
            cvSetErrStatus(Emgu.CV.CvEnum.ERROR_CODES.CV_STSOK); //clear the error status
            return 0; //signal the process to continute
         } finally
         {
            throw new CvException(status, funcName, errMsg, fileName, line);
         }
      }

      /// <summary>
      /// Define an error callback that can be registered using cvRedirectError function
      /// </summary>
      /// <param name="status">The numeric code for error status</param>
      /// <param name="funcName">The source file name where error is encountered</param>
      /// <param name="errMsg">A description of the error</param>
      /// <param name="fileName">The source file name where error is encountered</param>
      /// <param name="line">The line number in the souce where error is encountered</param>
      /// <param name="userData">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <returns></returns>
      [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
      public delegate int CvErrorCallback(
         int status, String funcName, String errMsg, String fileName, int line, IntPtr userData);

      /// <summary>
      /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
      /// </summary>
      /// <param name="errorHandler">The new error handler</param>
      /// <param name="userdata">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvRedirectError(
          CvErrorCallback errorHandler,
          IntPtr userdata,
          IntPtr prevUserdata);

      /// <summary>
      /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
      /// </summary>
      /// <param name="errorHandler">Pointer to the new error handler</param>
      /// <param name="userdata">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvRedirectError(
          IntPtr errorHandler,
          IntPtr userdata,
          IntPtr prevUserdata);

      /// <summary>
      /// Sets the specified error mode.
      /// </summary>
      /// <param name="errorMode">The error mode</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvSetErrMode(int errorMode);

      /// <summary>
      /// Returns the current error mode
      /// </summary>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetErrMode();

      /// <summary>
      /// Returns the current error status - the value set with the last cvSetErrStatus call. Note, that in Leaf mode the program terminates immediately after error occurred, so to always get control after the function call, one should call cvSetErrMode and set Parent or Silent error mode.
      /// </summary>
      /// <returns>the current error status</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetErrStatus();

      /// <summary>
      /// Sets the error status to the specified value. Mostly, the function is used to reset the error status (set to it CV_StsOk) to recover after error. In other cases it is more natural to call cvError or CV_ERROR.
      /// </summary>
      /// <param name="code">The error status.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetErrStatus(CvEnum.ERROR_CODES code);
      #endregion

      #region Sequence
      /// <summary>
      /// Creates a sequence and returns the pointer to it. The function allocates the sequence header in the storage block as one continuous chunk and sets the structure fields flags, elem_size, header_size and storage to passed values, sets delta_elems to the default value (that may be reassigned using cvSetSeqBlockSize function), and clears other header fields, including the space after the first sizeof(CvSeq) bytes
      /// </summary>
      /// <param name="seqFlags">Flags of the created sequence. If the sequence is not passed to any function working with a specific type of sequences, the sequence value may be set to 0, otherwise the appropriate type must be selected from the list of predefined sequence types</param>
      /// <param name="headerSize">Size of the sequence header; must be greater or equal to sizeof(CvSeq). If a specific type or its extension is indicated, this type must fit the base type header</param>
      /// <param name="elemSize">Size of the sequence elements in bytes. The size must be consistent with the sequence type. For example, for a sequence of points to be created, the element type CV_SEQ_ELTYPE_POINT should be specified and the parameter elem_size must be equal to sizeof(CvPoint). </param>
      /// <param name="storage">Sequence location.</param>
      /// <returns>A pointer to the sequence</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateSeq(
         int seqFlags,
         int headerSize,
         int elemSize,
         IntPtr storage);

      /// <summary>
      /// Adds an element to the end of sequence and retuns pointer to the allocated element. If the input element is NULL, the function simply allocates a space for one more element.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">Added element</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPush(IntPtr seq, IntPtr element);

      /// <summary>
      /// Adds an element to the front of sequence and retuns pointer to the allocated element. If the input element is NULL, the function simply allocates a space for one more element.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">Added element</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPushFront(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence end.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">If the pointer is not zero, the function copies the removed element to this location</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPop(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence beginning.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">If the pointer is not zero, the function copies the removed element to this location</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPopFront(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence middle
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="index">Index of removed element</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqRemove(IntPtr seq, int index);

      /// <summary>
      /// Shifts the sequence elements from the inserted position to the nearest end of the sequence and copies the element content there if the pointer is not IntPtr.Zero
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="beforeIndex">Index before which the element is inserted. Inserting before 0 (the minimal allowed value of the parameter) is equal to cvSeqPushFront and inserting before seq->total (the maximal allowed value of the parameter) is equal to cvSeqPush</param>
      /// <param name="element">Inserted element</param>
      /// <returns>Pointer to the inserted element</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvSeqInsert(IntPtr seq, int beforeIndex, IntPtr element);

      /// <summary>
      /// Adds several elements to either end of the sequence. The elements are added to the sequence in the same order as they are arranged in the input array but they can fall into different sequence blocks.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="elements">Added elements</param>
      /// <param name="count">Number of elements to push</param>
      /// <param name="backOrFront">
      /// If != 0, the elements are added to the beginning of sequence;
      /// Otherwise the elements are added to the end of sequence </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPushMulti(
          IntPtr seq,
          IntPtr elements,
          int count,
          CvEnum.BACK_OR_FRONT backOrFront);

      /// <summary>
      /// Removes several elements from either end of the sequence. If the number of the elements to be removed exceeds the total number of elements in the sequence, the function removes as many elements as possible
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="elements">Removed elements</param>
      /// <param name="count">Number of elements to remove</param>
      /// <param name="backOrFront">
      /// If != 0, the elements are added to the beginning of sequence;
      /// Otherwise the elements are added to the end of sequence </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSeqPopMulti(
          IntPtr seq,
          IntPtr elements,
          int count,
          CvEnum.BACK_OR_FRONT backOrFront);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="reader"></param>
      /// <param name="direction"></param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvChangeSeqBlock(
         ref MCvSeqReader reader,
         int direction);

      /// <summary>
      /// Move to the next element in the sequence
      /// </summary>
      /// <param name="elemSize">the size of the element</param>
      /// <param name="reader">the sequence reader</param>
      public static void CV_NEXT_SEQ_ELEM(int elemSize, ref MCvSeqReader reader)
      {
         Int64 newAddress = reader.ptr.ToInt64() + elemSize;
         reader.ptr = new IntPtr(newAddress);
         if (newAddress >= reader.block_max.ToInt64())
            cvChangeSeqBlock(ref reader, 1);
      }

      /// <summary>
      /// forward reading the sequence reader
      /// </summary>
      /// <typeparam name="T">The type of structure to be read</typeparam>
      /// <param name="reader">The sequence reader</param>
      /// <returns>The read object</returns>
      public static T CV_READ_SEQ_ELEM<T>(ref MCvSeqReader reader)
      {
         T res = (T) Marshal.PtrToStructure(reader.ptr, typeof(T));
         CV_NEXT_SEQ_ELEM(Marshal.SizeOf(typeof(T)), ref reader);
         return res;
      }

      /// <summary>
      /// Determined whether the specified node is occupied or not
      /// </summary>
      /// <param name="ptr">Pointer to the node</param>
      /// <returns>true if the specified node is occupied</returns>
      public static bool CV_IS_SET_ELEM(IntPtr ptr)
      {
         return Marshal.ReadInt32(ptr) >= 0;
         //return ((MCvSetElem)Marshal.PtrToStructure(ptr, typeof(MCvSetElem))).flags >= 0;
      }

      /// <summary>
      /// Initializes the reader state
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="reader">Reader state; initialized by the function</param>
      /// <param name="reverse">Determines the direction of the sequence traversal. If reverse is 0, the reader is positioned at the first sequence element, otherwise it is positioned at the last element.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvStartReadSeq(
         IntPtr seq,
         ref MCvSeqReader reader,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool reverse);

      /// <summary>
      /// Finds the element with the given index in the sequence and returns the pointer to it. If the element is not found, the function returns 0. The function supports negative indices, where -1 stands for the last sequence element, -2 stands for the one before last, etc. If the sequence is most likely to consist of a single sequence block or the desired element is likely to be located in the first block, then the macro CV_GET_SEQ_ELEM( elemType, seq, index ) should be used, where the parameter elemType is the type of sequence elements ( CvPoint for example), the parameter seq is a sequence, and the parameter index is the index of the desired element. The macro checks first whether the desired element belongs to the first block of the sequence and returns it if it does, otherwise the macro calls the main function GetSeqElem. Negative indices always cause the cvGetSeqElem call. The function has O(1) time complexity assuming that number of blocks is much smaller than the number of elements.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="index">Index of element</param>
      /// <returns>the pointer to the element with the given index in the sequence</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetSeqElem(IntPtr seq, int index);

      /// <summary>
      /// Removes all elements from the sequence. The function does not return the memory to the storage, but this memory is reused later when new elements are added to the sequence. This function time complexity is O(1). 
      /// </summary>
      /// <param name="seq">Sequence</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvClearSeq(IntPtr seq);
      #endregion

      /// <summary>
      /// initializes CvMat header so that it points to the same data as the original array but has different shape - different number of channels, different number of rows or both
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="header">Output header to be filled</param>
      /// <param name="new_cn">New number of channels. new_cn = 0 means that number of channels remains unchanged</param>
      /// <param name="new_rows">New number of rows. new_rows = 0 means that number of rows remains unchanged unless it needs to be changed according to new_cn value. destination array to be changed</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvReshape(
         IntPtr arr,
         IntPtr header,
         int new_cn,
         int new_rows);

      /// <summary>
      /// Fills the destination array with source array tiled:
      /// dst(i,j)=src(i mod rows(src), j mod cols(src))So the destination array may be as larger as well as smaller than the source array
      /// </summary>
      /// <param name="src">Source array, image or matrix</param>
      /// <param name="dst">Destination array, image or matrix</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRepeat(IntPtr src, IntPtr dst);

      /// <summary>
      /// This function is the opposite to cvSplit. If the destination array has N channels then if the first N input channels are not IntPtr.Zero, all they are copied to the destination array, otherwise if only a single source channel of the first N is not IntPtr.Zero, this particular channel is copied into the destination array, otherwise an error is raised. Rest of source channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to insert a single channel into the image. 
      /// </summary>
      /// <param name="src0">Input channels.</param>
      /// <param name="src1">Input channels.</param>
      /// <param name="src2">Input channels.</param>
      /// <param name="src3">Input channels.</param>
      /// <param name="dst">Destination array. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMerge(
          IntPtr src0,
          IntPtr src1,
          IntPtr src2,
          IntPtr src3,
          IntPtr dst);

      /// <summary>
      /// The function cvMixChannels is a generalized form of cvSplit and cvMerge and some forms of cvCvtColor. It can be used to change the order of the planes, add/remove alpha channel, extract or insert a single plane or multiple planes etc.
      /// </summary>
      /// <param name="src">The array of input arrays.</param>
      /// <param name="srcCount">The number of input arrays</param>
      /// <param name="dst">The array of output arrays</param>
      /// <param name="dstCount">The number of output arrays</param>
      /// <param name="fromTo">The array of pairs of indices of the planes copied. from_to[k*2] is the 0-based index of the input plane, and from_to[k*2+1] is the index of the output plane, where the continuous numbering of the planes over all the input and over all the output arrays is used. When from_to[k*2] is negative, the corresponding output plane is filled with 0's.</param>
      /// <param name="pairCount">The number of pairs in from_to, or the number of the planes copied</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMixChannels(
         IntPtr[] src,
         int srcCount,
         IntPtr[] dst,
         int dstCount,
         int[] fromTo,
         int pairCount);

      /// <summary>
      /// The function cvMixChannels is a generalized form of cvSplit and cvMerge and some forms of cvCvtColor. It can be used to change the order of the planes, add/remove alpha channel, extract or insert a single plane or multiple planes etc.
      /// </summary>
      /// <param name="src">The array of input arrays.</param>
      /// <param name="dst">The array of output arrays</param>
      /// <param name="fromTo">The array of pairs of indices of the planes copied. from_to[k*2] is the 0-based index of the input plane, and from_to[k*2+1] is the index of the output plane, where the continuous numbering of the planes over all the input and over all the output arrays is used. When from_to[k*2] is negative, the corresponding output plane is filled with 0's.</param>
      public static void cvMixChannels(
         IntPtr[] src,
         IntPtr[] dst,
         int[] fromTo)
      {
         cvMixChannels(src, src.Length, dst, dst.Length, fromTo, fromTo.Length >> 1);
      }

      /// <summary>
      /// Shuffles the matrix by swapping randomly chosen pairs of the matrix elements on each iteration (where each element may contain several components in case of multi-channel arrays)
      /// </summary>
      /// <param name="mat">The input/output matrix. It is shuffled in-place. </param>
      /// <param name="rng">Pointer to MCvRNG radom number generator. Use IntPtr.Zero if not sure</param>
      /// <param name="iterFactor">The relative parameter that characterizes intensity of the shuffling performed. The number of iterations (i.e. pairs swapped) is round(iter_factor*rows(mat)*cols(mat)), so iter_factor=0 means that no shuffling is done, iter_factor=1 means that the function swaps rows(mat)*cols(mat) random pairs etc</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRandShuffle(IntPtr mat, IntPtr rng, double iterFactor);

      /// <summary>
      /// This function is the opposite to cvSplit. If the destination array has N channels then if the first N input channels are not IntPtr.Zero, all they are copied to the destination array, otherwise if only a single source channel of the first N is not IntPtr.Zero, this particular channel is copied into the destination array, otherwise an error is raised. Rest of source channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to insert a single channel into the image. 
      /// </summary>
      /// <param name="src0">Input channels.</param>
      /// <param name="src1">Input channels.</param>
      /// <param name="src2">Input channels.</param>
      /// <param name="src3">Input channels.</param>
      /// <param name="dst">Destination array. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvMerge")]
      public static extern void cvCvtPlaneToPix(
          IntPtr src0,
          IntPtr src1,
          IntPtr src2,
          IntPtr src3,
          IntPtr dst);

      /// <summary>
      /// Inverses every bit of every array element:
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="des">The destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvNot(IntPtr src, IntPtr des);

      /// <summary>
      /// Calculates per-element maximum of two arrays:
      /// dst(I)=max(src1(I), src2(I))
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array. </param>
      /// <param name="dst">The destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMax(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// <para>Calculates per-element maximum of array and scalar:</para>
      /// <para>dst(I)=max(src(I), value)</para>
      /// <para>All the arrays must have a single channel, the same data type and the same size (or ROI size).</para>
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="value">The scalar value</param>
      /// <param name="dst">The destination array. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMaxS(IntPtr src, double value, IntPtr dst);

      /// <summary>
      /// Returns the number of non-zero elements in arr:
      /// result = sumI arr(I)!=0
      /// In case of IplImage both ROI and COI are supported.
      /// </summary>
      /// <param name="arr">The image</param>
      /// <returns>the number of non-zero elements in image</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvCountNonZero(IntPtr arr);

      /// <summary>
      /// Calculates per-element minimum of two arrays:
      /// dst(I)=min(src1(I),src2(I))
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMin(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// Calculates minimum of array and scalar:
      /// dst(I)=min(src(I), value)
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="value">The scalar value</param>
      /// <param name="dst">The destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMinS(IntPtr src, double value, IntPtr dst);

      /// <summary>
      /// Adds one array to another one:
      /// dst(I)=src1(I)+src2(I) if mask(I)!=0All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array.</param>
      /// <param name="src2">The second source array.</param>
      /// <param name="dst">The destination array.</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAdd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Adds scalar <paramref name="value"/> to every element in the source array src1 and stores the result in dst
      /// dst(I)=src(I)+value if mask(I)!=0
      /// All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src">The source array.</param>
      /// <param name="value">Added scalar.</param>
      /// <param name="dst">The destination array.</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAddS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Subtracts one array from another one:
      /// dst(I)=src1(I)-src2(I) if mask(I)!=0
      /// All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSub(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Subtracts a scalar from every element of the source array:
      /// dst(I)=src(I)-value if mask(I)!=0
      /// All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="value">Subtracted scalar</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed. </param>
      public static void cvSubS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask)
      {
         CvInvoke.cvAddS(
            src,
            new MCvScalar(-value.v0, -value.v1, -value.v2, -value.v3),
            dst,
            mask);
      }

      /// <summary>
      /// Subtracts every element of source array from a scalar:
      /// dst(I)=value-src(I) if mask(I)!=0
      /// All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="value">Subtracted scalar</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSubRS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Divides one array by another:
      /// dst(I)=scale * src1(I)/src2(I), if src1!=IntPtr.Zero;
      /// dst(I)=scale/src2(I),      if src1==IntPtr.Zero;
      /// All the arrays must have the same type, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array. If the pointer is IntPtr.Zero, the array is assumed to be all 1s. </param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="scale">Optional scale factor </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDiv(IntPtr src1, IntPtr src2, IntPtr dst, double scale);

      /// <summary>
      /// Calculates per-element product of two arrays:
      /// dst(I)=scale*src1(I)*src2(I)
      /// All the arrays must have the same type, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array. </param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="scale">Optional scale factor</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMul(IntPtr src1, IntPtr src2, IntPtr dst, double scale);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of two arrays:
      /// dst(I)=src1(I) &amp; src2(I) if mask(I)!=0
      /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise conjunction of array and scalar:
      /// dst(I)=src(I)&amp;value if mask(I)!=0
      /// Prior to the actual operation the scalar is converted to the same type as the arrays. In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="value">Scalar to use in the operation</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAndS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise disjunction of two arrays:
      /// dst(I)=src1(I)|src2(I)
      /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise disjunction of array and scalar:
      /// dst(I)=src(I)|value if mask(I)!=0
      /// Prior to the actual operation the scalar is converted to the same type as the arrays. In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="value">Scalar to use in the operation</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvOrS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of two arrays:
      /// dst(I)=src1(I)^src2(I) if mask(I)!=0
      /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Mask, 8-bit single channel array; specifies elements of destination array to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise conjunction of array and scalar:
      /// dst(I)=src(I)^value if mask(I)!=0
      /// Prior to the actual operation the scalar is converted to the same type as the arrays. In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="value">Scalar to use in the operation</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvXorS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      #region Copying and Filling
      /// <summary>
      /// Copies selected elements from input array to output array:
      /// dst(I)=src(I) if mask(I)!=0. 
      /// If any of the passed arrays is of IplImage type, then its ROI and COI fields are used. Both arrays must have the same type, the same number of dimensions and the same size. The function can also copy sparse arrays (mask is not supported in this case).
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="des">The destination array</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCopy(IntPtr src, IntPtr des, IntPtr mask);

      /// <summary>
      /// Copies scalar value to every selected element of the destination array:
      ///arr(I)=value if mask(I)!=0
      ///If array arr is of IplImage type, then is ROI used, but COI must not be set
      /// </summary>
      /// <param name="arr">The destination array</param>
      /// <param name="value">Fill value</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSet(IntPtr arr, MCvScalar value, IntPtr mask);

      /// <summary>
      /// Clears the array. In case of dense arrays (CvMat, CvMatND or IplImage) cvZero(array) is equivalent to cvSet(array,cvScalarAll(0),0), in case of sparse arrays all the elements are removed
      /// </summary>
      /// <param name="arr">array to be cleared</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetZero(IntPtr arr);

      /// <summary>
      /// Clears the array. In case of dense arrays (CvMat, CvMatND or IplImage) cvZero(array) is equivalent to cvSet(array,cvScalarAll(0),0), in case of sparse arrays all the elements are removed
      /// </summary>
      /// <param name="arr">array to be cleared</param>
      public static void cvZero(IntPtr arr)
      {
         cvSetZero(arr);
      }

      /// <summary>
      /// Initializes scaled identity matrix:
      /// arr(i,j)=value if i=j,
      /// 0 otherwise
      /// </summary>
      /// <param name="mat">The matrix to initialize (not necesserily square).</param>
      /// <param name="value">The value to assign to the diagonal elements.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetIdentity(IntPtr mat, MCvScalar value);

      /// <summary>
      /// Initializes the matrix as following:
      /// arr(i,j)=(end-start)*(i*cols(arr)+j)/(cols(arr)*rows(arr))
      /// </summary>
      /// <param name="mat">The matrix to initialize. It should be single-channel 32-bit, integer or floating-point</param>
      /// <param name="start">The lower inclusive boundary of the range</param>
      /// <param name="end">The upper exclusive boundary of the range</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRange(IntPtr mat, double start, double end);
      #endregion

      #region Math Functions
      /// <summary>
      /// Calculates either magnitude, angle, or both of every 2d vector (x(I),y(I)):
      /// magnitude(I)=sqrt( x(I)2+y(I)2 ),
      /// angle(I)=atan( y(I)/x(I) )
      /// The angles are calculated with ~0.1 degree accuracy. For (0,0) point the angle is set to 0
      /// </summary>
      /// <param name="x">The array of x-coordinates </param>
      /// <param name="y">The array of y-coordinates</param>
      /// <param name="magnitude">The destination array of magnitudes, may be set to IntPtr.Zero if it is not needed </param>
      /// <param name="angle">The destination array of angles, may be set to IntPtr.Zero if it is not needed. The angles are measured in radians (0..2?) or in degrees (0..360?). </param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCartToPolar(
         IntPtr x,
         IntPtr y,
         IntPtr magnitude,
         IntPtr angle,
         int angleInDegrees);

      /// <summary>
      /// Calculates either magnitude, angle, or both of every 2d vector (x(I),y(I)):
      /// magnitude(I)=sqrt( x(I)2+y(I)2 ),
      /// angle(I)=atan( y(I)/x(I) )
      /// The angles are calculated with ~0.1 degree accuracy. For (0,0) point the angle is set to 0
      /// </summary>
      /// <param name="x">The array of x-coordinates </param>
      /// <param name="y">The array of y-coordinates</param>
      /// <param name="magnitude">The destination array of magnitudes, may be set to IntPtr.Zero if it is not needed </param>
      /// <param name="angle">The destination array of angles, may be set to IntPtr.Zero if it is not needed. The angles are measured in radians (0..2?) or in degrees (0..360?). </param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      public static void cvCartToPolar(
         IntPtr x,
         IntPtr y,
         IntPtr magnitude,
         IntPtr angle,
         bool angleInDegrees)
      {
         cvCartToPolar(x, y, magnitude, angle, angleInDegrees ? 1 : 0);
      }

      /// <summary>
      /// Calculates either x-coodinate, y-coordinate or both of every vector magnitude(I)* exp(angle(I)*j), j=sqrt(-1):
      /// x(I)=magnitude(I)*cos(angle(I)),
      /// y(I)=magnitude(I)*sin(angle(I))
      /// </summary>
      /// <param name="magnitude">The array of magnitudes. If it is IntPtr.Zero, the magnitudes are assumed all 1's</param>
      /// <param name="angle">The array of angles, whether in radians or degrees</param>
      /// <param name="x">The destination array of x-coordinates, may be set to IntPtr.Zero if it is not needed</param>
      /// <param name="y">The destination array of y-coordinates, mau be set to IntPtr.Zero if it is not needed</param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPolarToCart(
         IntPtr magnitude,
         IntPtr angle,
         IntPtr x,
         IntPtr y,
         int angleInDegrees);

      /// <summary>
      /// Calculates either x-coodinate, y-coordinate or both of every vector magnitude(I)* exp(angle(I)*j), j=sqrt(-1):
      /// x(I)=magnitude(I)*cos(angle(I)),
      /// y(I)=magnitude(I)*sin(angle(I))
      /// </summary>
      /// <param name="magnitude">The array of magnitudes. If it is IntPtr.Zero, the magnitudes are assumed all 1's</param>
      /// <param name="angle">The array of angles, whether in radians or degrees</param>
      /// <param name="x">The destination array of x-coordinates, may be set to IntPtr.Zero if it is not needed</param>
      /// <param name="y">The destination array of y-coordinates, mau be set to IntPtr.Zero if it is not needed</param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      public static void cvPolarToCart(
         IntPtr magnitude,
         IntPtr angle,
         IntPtr x,
         IntPtr y,
         bool angleInDegrees)
      {
         cvPolarToCart(magnitude, angle, x, y, angleInDegrees ? 1 : 0);
      }

      /// <summary>
      /// Raises every element of input array to p:
      /// dst(I)=src(I)p, if p is integer
      /// dst(I)=abs(src(I))p, otherwise
      /// That is, for non-integer power exponent the absolute values of input array elements are used. However, it is possible to get true values for negative values using some extra operations, as the following sample, computing cube root of array elements, shows:
      /// CvSize size = cvGetSize(src);
      /// CvMat* mask = cvCreateMat( size.height, size.width, CV_8UC1 );
      /// cvCmpS( src, 0, mask, CV_CMP_LT ); /* find negative elements */
      /// cvPow( src, dst, 1./3 );
      /// cvSubRS( dst, cvScalarAll(0), dst, mask ); /* negate the results of negative inputs */
      /// cvReleaseMat( &amp;mask );
      /// For some values of power, such as integer values, 0.5 and -0.5, specialized faster algorithms are used.
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array, should be the same type as the source</param>
      /// <param name="power">The exponent of power</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPow(IntPtr src, IntPtr dst, double power);

      /// <summary>
      /// Calculates exponent of every element of input array:
      /// dst(I)=exp(src(I))
      /// Maximum relative error is 7e-6. Currently, the function converts denormalized values to zeros on output
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvExp(IntPtr src, IntPtr dst);

      /// <summary>
      /// Calculates natural logarithm of absolute value of every element of input array:
      /// dst(I)=log(abs(src(I))), src(I)!=0
      /// dst(I)=C,  src(I)=0
      /// Where C is large negative number (-700 in the current implementation)
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLog(IntPtr src, IntPtr dst);

      /// <summary>
      /// finds real roots of a cubic equation:
      /// coeffs[0]*x^3 + coeffs[1]*x^2 + coeffs[2]*x + coeffs[3] = 0
      /// (if coeffs is 4-element vector)
      /// or
      /// x^3 + coeffs[0]*x^2 + coeffs[1]*x + coeffs[2] = 0
      /// (if coeffs is 3-element vector)
      /// </summary>
      /// <param name="coeffs">The equation coefficients, array of 3 or 4 elements</param>
      /// <param name="roots">The output array of real roots. Should have 3 elements. Padded with zeros if there is only one root</param>
      /// <returns>the number of real roots found</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvSolveCubic(IntPtr coeffs, IntPtr roots);

      /// <summary>
      /// Finds all real and complex roots of any degree polynomial with real coefficients
      /// </summary>
      /// <param name="coeffs">The (degree + 1)-length array of equation coefficients (CV_32FC1 or CV_64FC1)</param>
      /// <param name="roots">The degree-length output array of real or complex roots (CV_32FC2 or CV_64FC2)</param>
      /// <param name="maxiter">The maximum number of iterations</param>
      /// <param name="fig">The required figures of precision required</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSolvePoly(
         IntPtr coeffs,
         IntPtr roots,
         int maxiter,
         int fig);

      /// <summary>
      /// Solves linear system (src1)*(dst) = (src2)
      /// </summary>
      /// <param name="src1">The source matrix in the LHS</param>
      /// <param name="src2">The source matrix in the RHS</param>
      /// <param name="dst">The result</param>
      /// <param name="method">The method for solving the equation</param>
      /// <returns>0 if src1 is a singular and CV_LU method is used</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int  cvSolve(
         IntPtr src1, 
         IntPtr src2, 
         IntPtr dst,
         CvEnum.SOLVE_METHOD method);
      #endregion

      #region Discrete Transforms
      /// <summary>
      /// Performs forward or inverse transform of 1D or 2D floating-point array
      /// In case of real (single-channel) data, the packed format, borrowed from IPL, is used to to represent a result of forward Fourier transform or input for inverse Fourier transform
      /// </summary>
      /// <param name="src">Source array, real or complex</param>
      /// <param name="dst">Destination array of the same size and same type as the source</param>
      /// <param name="flags">Transformation flags</param>
      /// <param name="nonzeroRows">Number of nonzero rows to in the source array (in case of forward 2d transform), or a number of rows of interest in the destination array (in case of inverse 2d transform). If the value is negative, zero, or greater than the total number of rows, it is ignored. The parameter can be used to speed up 2d convolution/correlation when computing them via DFT. See the sample below</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDFT(
         IntPtr src,
         IntPtr dst,
         CvEnum.CV_DXT flags,
         int nonzeroRows);

      /// <summary>
      /// Returns the minimum number N that is greater to equal to size0, such that DFT of a vector of size N can be computed fast. In the current implementation N=2^p x 3^q x 5^r for some p, q, r. 
      /// </summary>
      /// <param name="size0">Vector size</param>
      /// <returns>The minimum number N that is greater to equal to size0, such that DFT of a vector of size N can be computed fast. In the current implementation N=2^p x 3^q x 5^r for some p, q, r. </returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetOptimalDFTSize(int size0);

      /// <summary>
      /// Performs per-element multiplication of the two CCS-packed or complex matrices that are results of real or complex Fourier transform. 
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array of the same type and the same size of the sources</param>
      /// <param name="flags"></param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMulSpectrums(IntPtr src1, IntPtr src2, IntPtr dst, CvEnum.MUL_SPECTRUMS_TYPE flags);

      /// <summary>
      /// Performs forward or inverse transform of 1D or 2D floating-point array
      /// </summary>
      /// <param name="src">Source array, real 1D or 2D array</param>
      /// <param name="dst">Destination array of the same size and same type as the source</param>
      /// <param name="flags">Transformation flags</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDCT(IntPtr src, IntPtr dst, CvEnum.CV_DCT_TYPE flags);
      #endregion

      /// <summary>
      /// Calculates a part of the line segment which is entirely in the image. It returns 0 if the line segment is completely outside the image and 1 otherwise.
      /// </summary>
      /// <param name="imgSize">Size of the image</param>
      /// <param name="pt1">First ending point of the line segment. It is modified by the function</param>
      /// <param name="pt2">Second ending point of the line segment. It is modified by the function.</param>
      /// <returns>It returns 0 if the line segment is completely outside the image and 1 otherwise.</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvClipLine(Size imgSize, ref Point pt1, ref Point pt2);

      /// <summary>
      /// Calculates absolute difference between two arrays.
      /// dst(I)c = abs(src1(I)c - src2(I)c).
      /// All the arrays must have the same data type and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAbsDiff(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// Calculates absolute difference between array and scalar
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="value">The scalar</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAbsDiffS(IntPtr src, IntPtr dst, MCvScalar value);

      /// <summary>
      /// Calculated weighted sum of two arrays as following:
      /// dst(I)=src1(I)*alpha+src2(I)*beta+gamma
      /// All the arrays must have the same type and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array.</param>
      /// <param name="alpha">Weight of the first array elements.</param>
      /// <param name="src2">The second source array. </param>
      /// <param name="beta">Weight of the second array elements.</param>
      /// <param name="gamma">Scalar, added to each sum. </param>
      /// <param name="dst">The destination array.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst);

      /// <summary>
      /// Does the range check for every element of the input array:
      /// dst(I)=lower0 &lt;= src(I)0 &lt;= upper0
      /// for a single-channel array,
      /// dst(I)=lower0 &lt;= src(I)0 &lt;= upper0 &amp;&amp;
      ///     lower1 &lt;= src(I)1 &lt;= upper1
      /// for a two-channel array etc.
      /// dst(I) is set to 0xff (all '1'-bits) if src(I) is within the range and 0 otherwise. All the arrays must have the same size (or ROI size)
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="lower">The inclusive lower boundary</param>
      /// <param name="upper">The inclusive upper boundary</param>
      /// <param name="dst">The destination array, must have 8u or 8s type</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInRangeS(
         IntPtr src,
         MCvScalar lower,
         MCvScalar upper,
         IntPtr dst);

      /// <summary>
      /// Performs range check for every element of the input array:
      /// dst(I)=lower(I)_0 &lt;= src(I)_0 &lt;= upper(I)_0
      /// For single-channel arrays,
      /// dst(I)=lower(I)_0 &lt;= src(I)_0 &lt;= upper(I)_0 &amp;&amp;
      /// lower(I)_1 &lt;= src(I)_1 &lt;= upper(I)_1
      /// For two-channel arrays etc.
      /// dst(I) is set to 0xff (all '1'-bits) if src(I) is within the range and 0 otherwise. All the arrays must have the same type, except the destination, and the same size (or ROI size)
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="lower">The lower values stored in an image of same type &amp; size as <paramref name="src"/></param>
      /// <param name="upper">The upper values stored in an image of same type &amp; size as <paramref name="src"/></param>
      /// <param name="dst">The resulting mask</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInRange(
         IntPtr src,
         IntPtr lower,
         IntPtr upper,
         IntPtr dst);

      /// <summary>
      /// Returns the calculated norm. The multiple-channel array are treated as single-channel, that is, the results for all channels are combined. 
      /// </summary>
      /// <param name="arr1">The first source image</param>
      /// <param name="arr2">The second source image. If it is IntPtr.Zero, the absolute norm of arr1 is calculated, otherwise absolute or relative norm of arr1-arr2 is calculated</param>
      /// <param name="normType">Type of norm</param>
      /// <param name="mask">The optional operation mask</param>
      /// <returns>The calculated norm</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvNorm(
          IntPtr arr1,
          IntPtr arr2,
          Emgu.CV.CvEnum.NORM_TYPE normType,
          IntPtr mask);

      #region Initialization
      /// <summary>
      /// Creates the header and allocates data. 
      /// </summary>
      /// <param name="size">Image width and height.</param>
      /// <param name="depth">Bit depth of image elements</param>
      /// <param name="channels">
      /// Number of channels per element(pixel). Can be 1, 2, 3 or 4. The channels are interleaved, for example the usual data layout of a color image is:
      /// b0 g0 r0 b1 g1 r1 ...
      /// </param>
      /// <returns>A pointer to IplImage </returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateImage(
         Size size,
         CvEnum.IPL_DEPTH depth,
         int channels);

      /// <summary>
      /// Allocates, initializes, and returns the structure IplImage.
      /// </summary>
      /// <param name="size">Image width and height.</param>
      /// <param name="depth">Bit depth of image elements</param>
      /// <param name="channels">
      /// Number of channels per element(pixel). Can be 1, 2, 3 or 4. The channels are interleaved, for example the usual data layout of a color image is:
      /// b0 g0 r0 b1 g1 r1 ...
      /// </param>
      /// <returns> The structure IplImage</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateImageHeader(
         Size size,
         CvEnum.IPL_DEPTH depth,
         int channels);

      /// <summary>
      /// Initializes the image header structure, pointer to which is passed by the user, and returns the pointer.
      /// </summary>
      /// <param name="image">Image header to initialize.</param>
      /// <param name="size">Image width and height.</param>
      /// <param name="depth">Image depth </param>
      /// <param name="channels">Number of channels </param>
      /// <param name="origin">IPL_ORIGIN_TL or IPL_ORIGIN_BL.</param>
      /// <param name="align">Alignment for image rows, typically 4 or 8 bytes.</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvInitImageHeader(
         IntPtr image,
         Size size,
         CvEnum.IPL_DEPTH depth,
         int channels,
         int origin,
         int align);

      /// <summary>
      /// Assigns user data to the array header.
      /// </summary>
      /// <param name="arr">Array header.</param>
      /// <param name="data">User data.</param>
      /// <param name="step">Full row length in bytes.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetData(IntPtr arr, IntPtr data, int step);

      /// <summary>
      /// Releases the header.
      /// </summary>
      /// <param name="image">Pointer to the deallocated header.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseImageHeader(ref IntPtr image);

      /// <summary>
      /// Initializes already allocated CvMat structure. It can be used to process raw data with OpenCV matrix functions.
      /// </summary>
      /// <param name="mat">Pointer to the matrix header to be initialized.</param>
      /// <param name="rows">Number of rows in the matrix.</param>
      /// <param name="cols">Number of columns in the matrix.</param>
      /// <param name="type">Type of the matrix elements.</param>
      /// <param name="data">Optional data pointer assigned to the matrix header</param>
      /// <param name="step">Full row width in bytes of the data assigned. By default, the minimal possible step is used, i.e., no gaps is assumed between subsequent rows of the matrix.</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvInitMatHeader(
         IntPtr mat,
         int rows,
         int cols,
         CV.CvEnum.MAT_DEPTH type,
         IntPtr data,
         int step);

      /// <summary>
      /// Initializes already allocated CvMat structure. It can be used to process raw data with OpenCV matrix functions.
      /// </summary>
      /// <param name="mat">Pointer to the matrix header to be initialized.</param>
      /// <param name="rows">Number of rows in the matrix.</param>
      /// <param name="cols">Number of columns in the matrix.</param>
      /// <param name="type">Type of the matrix elements.</param>
      /// <param name="data">Optional data pointer assigned to the matrix header</param>
      /// <param name="step">Full row width in bytes of the data assigned. By default, the minimal possible step is used, i.e., no gaps is assumed between subsequent rows of the matrix.</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvInitMatHeader(
         IntPtr mat,
         int rows,
         int cols,
         int type,
         IntPtr data,
         int step);

      /// <summary>
      /// Sets the channel of interest to a given value. Value 0 means that all channels are selected, 1 means that the first channel is selected etc. If ROI is NULL and coi != 0, ROI is allocated.
      /// </summary>
      /// <param name="image">Image header</param>
      /// <param name="coi">Channel of interest starting from 1. If 0, the COI is unset.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetImageCOI(IntPtr image, int coi);

      /// <summary>
      /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
      /// </summary>
      /// <param name="image">Image header. </param>
      /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetImageCOI(IntPtr image);

      /// <summary>
      /// Releases image ROI. After that the whole image is considered selected.
      /// </summary>
      /// <param name="image">Image header</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvResetImageROI(IntPtr image);

      /// <summary>
      /// Sets the image ROI to a given rectangle. If ROI is NULL and the value of the parameter rect is not equal to the whole image, ROI is allocated. 
      /// </summary>
      /// <param name="image">Image header.</param>
      /// <param name="rect">ROI rectangle.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetImageROI(IntPtr image, Rectangle rect);

      /// <summary>
      /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
      /// </summary>
      /// <param name="image">Image header.</param>
      /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern Rectangle cvGetImageROI(IntPtr image);

      /// <summary>
      /// Allocates header for the new matrix and underlying data, and returns a pointer to the created matrix. Matrices are stored row by row. All the rows are aligned by 4 bytes. 
      /// </summary>
      /// <param name="rows">Number of rows in the matrix.</param>
      /// <param name="cols">Number of columns in the matrix.</param>
      /// <param name="type">Type of the matrix elements.</param>
      /// <returns>A pointer to the created matrix</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateMat(int rows, int cols, CvEnum.MAT_DEPTH type);

      /// <summary>
      /// Initializes CvMatND structure allocated by the user
      /// </summary>
      /// <param name="mat">Pointer to the array header to be initialized</param>
      /// <param name="dims">Number of array dimensions</param>
      /// <param name="sizes">Array of dimension sizes</param>
      /// <param name="type">Type of array elements</param>
      /// <param name="data">Optional data pointer assigned to the matrix header</param>
      /// <returns>Pointer to the array header</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvInitMatNDHeader(
         IntPtr mat,
         int dims,
         [In]
         int[] sizes,
         CV.CvEnum.MAT_DEPTH type,
         IntPtr data);

      /// <summary>
      /// Decrements the matrix data reference counter and releases matrix header
      /// </summary>
      /// <param name="mat">Double pointer to the matrix.</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseMat(ref IntPtr mat);

      /// <summary>
      /// The function allocates a multi-dimensional sparse array. Initially the array contain no elements, that is Get or GetReal returns zero for every index
      /// </summary>
      /// <param name="dims">Number of array dimensions</param>
      /// <param name="sizes">Array of dimension sizes</param>
      /// <param name="type">Type of array elements</param>
      /// <returns>Pointer to the array header</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateSparseMat(
         int dims,
         IntPtr sizes,
         CV.CvEnum.MAT_DEPTH type);

      /// <summary>
      /// The function releases the sparse array and clears the array pointer upon exit.
      /// </summary>
      /// <param name="mat">Reference of the pointer to the array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseSparseMat(ref IntPtr mat);

      #endregion

      #region Memory Storages
      /// <summary>
      /// Creates a memory storage and returns pointer to it. Initially the storage is empty. All fields of the header, except the block_size, are set to 0.
      /// </summary>
      /// <param name="blockSize"></param>
      /// <returns>Size of the storage blocks in bytes. If it is 0, the block size is set to default value - currently it is 64K. </returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateMemStorage(int blockSize);

      /// <summary>
      /// Creates a child memory storage that is similar to simple memory storage except for the differences in the memory allocation/deallocation mechanism. When a child storage needs a new block to add to the block list, it tries to get this block from the parent. The first unoccupied parent block available is taken and excluded from the parent block list. If no blocks are available, the parent either allocates a block or borrows one from its own parent, if any. In other words, the chain, or a more complex structure, of memory storages where every storage is a child/parent of another is possible. When a child storage is released or even cleared, it returns all blocks to the parent. In other aspects, the child storage is the same as the simple storage.
      /// </summary>
      /// <param name="parent">Parent memory storage</param>
      /// <returns>ChildMemStorage</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateChildMemStorage(IntPtr parent);

      /// <summary>
      /// Resets the top (free space boundary) of the storage to the very beginning. This function does not deallocate any memory. If the storage has a parent, the function returns all blocks to the parent.
      /// </summary>
      /// <param name="storage">Memory storage</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvClearMemStorage(IntPtr storage);

      /// <summary>
      /// Deallocates all storage memory blocks or returns them to the parent, if any. Then it deallocates the storage header and clears the pointer to the storage. All children of the storage must be released before the parent is released.
      /// </summary>
      /// <param name="storage">Pointer to the released storage</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseMemStorage(ref IntPtr storage);
      #endregion

      /// <summary>
      /// Loads object from file. It provides a simple interface to cvRead. After object is loaded, the file storage is closed and all the temporary buffers are deleted. Thus, to load a dynamic structure, such as sequence, contour or graph, one should pass a valid destination memory storage to the function.
      /// </summary>
      /// <param name="fileName">File name</param>
      /// <param name="memstorage">Memory storage for dynamic structures, such as CvSeq or CvGraph. It is not used for matrices or images</param>
      /// <param name="name">Optional object name. If it is IntPtr.Zero, the first top-level object in the storage will be loaded</param>
      /// <param name="realName">Optional output parameter that will contain name of the loaded object (useful if name=IntPtr.Zero). </param>
      /// <returns>Loaded object from file</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvLoad")]
      private static extern IntPtr _cvLoad(
          [MarshalAs(StringMarshalType)] String fileName,
          IntPtr memstorage,
          [MarshalAs(StringMarshalType)] String name,
          IntPtr realName);

      /// <summary>
      /// Loads object from file. It provides a simple interface to cvRead. After object is loaded, the file storage is closed and all the temporary buffers are deleted. Thus, to load a dynamic structure, such as sequence, contour or graph, one should pass a valid destination memory storage to the function.
      /// </summary>
      /// <param name="fileName">File name</param>
      /// <param name="memstorage">Memory storage for dynamic structures, such as CvSeq or CvGraph. It is not used for matrices or images</param>
      /// <param name="name">Optional object name. If it is NULL, the first top-level object in the storage will be loaded</param>
      /// <param name="realName">Optional output parameter that will contain name of the loaded object (useful if name=NULL). </param>
      /// <returns>Loaded object from file</returns>
      public static IntPtr cvLoad(string fileName, IntPtr memstorage, string name, IntPtr realName)
      {
         try
         {
            return _cvLoad(fileName, memstorage, name, realName);
         } catch (CvException)
         {
            //cv.dll needed to be load before creating HaarCascade object
            //creating the following dummy will do the job
            //a bug(?) in OpenCV
            //see http://opencvlibrary.sourceforge.net/FaceDetection 
            //after step 11 there is an explaination
            using (Image<Gray, Byte> dummy = new Image<Gray, Byte>(1, 1))
            {
               dummy._Erode(1);
            }
            //After opencv structure reorganization, opencv_objdetect is needed in memory
            //This can be done by loading the HOG detector
            using (HOGDescriptor desc = new HOGDescriptor())
            {
            }

            return _cvLoad(fileName, memstorage, name, realName);
         }
      }

      /// <summary>
      /// Creates a sequence that represents the specified slice of the input sequence. The new sequence either shares the elements with the original sequence or has own copy of the elements. So if one needs to process a part of sequence but the processing function does not have a slice parameter, the required sub-sequence may be extracted using this function.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="slice">The part of the sequence to extract</param>
      /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is IntPtr.Zero, the function uses the storage containing the input sequence.</param>
      /// <param name="copyData">The flag that indicates whether to copy the elements of the extracted slice (copy_data!=0) or not (copy_data=0)</param>
      /// <returns>A pointer to CvSeq</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvSeqSlice(
         IntPtr seq,
         MCvSlice slice,
         IntPtr storage,
         int copyData);

      /// <summary>
      /// Creates a sequence that represents the specified slice of the input sequence. The new sequence either shares the elements with the original sequence or has own copy of the elements. So if one needs to process a part of sequence but the processing function does not have a slice parameter, the required sub-sequence may be extracted using this function.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="slice">The part of the sequence to extract</param>
      /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is IntPtr.Zero, the function uses the storage containing the input sequence.</param>
      /// <param name="copyData">The flag that indicates whether to copy the elements of the extracted slice (copyData == true) or not (copyData=false)</param>
      /// <returns>A pointer to CvSeq</returns>
      public static IntPtr cvSeqSlice(IntPtr seq, MCvSlice slice, IntPtr storage, bool copyData)
      {
         return cvSeqSlice(seq, slice, storage, copyData ? 1 : 0);
      }

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="value">The assigned value </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetReal1D(IntPtr arr, int idx0, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index </param>
      /// <param name="value">The assigned value </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetReal2D(IntPtr arr, int idx0, int idx1, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index </param>
      /// <param name="idx2">The third zero-based component of the element index </param>
      /// <param name="value">The assigned value </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetReal3D(IntPtr arr, int idx0, int idx1, int idx2, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx">Array of the element indices </param>
      /// <param name="value">The assigned value </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetRealND(
         IntPtr arr,
         [In]
         int[] idx,
         double value);

      /// <summary>
      /// Clears (sets to zero) the particular element of dense array or deletes the element of sparse array. If the element does not exists, the function does nothing
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx">Array of the element indices </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvClearND(
         IntPtr arr,
         [In]
         int[] idx);

      /// <summary>
      /// Assign the new value to the particular element of array
      /// </summary>
      /// <param name="arr">Input array. </param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <param name="value">The assigned value</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSet2D(IntPtr arr, int idx0, int idx1, MCvScalar value);

      /// <summary>
      /// Flips the array in one of different 3 ways (row and column indices are 0-based):
      /// dst(i,j)=src(rows(src)-i-1,j) if flip_mode = 0
      /// dst(i,j)=src(i,cols(src1)-j-1) if flip_mode &gt; 0
      /// dst(i,j)=src(rows(src)-i-1,cols(src)-j-1) if flip_mode &lt; 0
      /// </summary>
      /// <param name="src">Source array.</param>
      /// <param name="dst">Destination array.</param>
      /// <param name="flipMode">
      /// Specifies how to flip the array.
      /// flip_mode = 0 means flipping around x-axis, 
      /// flip_mode &gt; 0 (e.g. 1) means flipping around y-axis and 
      /// flip_mode &lt; 0 (e.g. -1) means flipping around both axises. 
      ///</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFlip(IntPtr src, IntPtr dst, int flipMode);

      /// <summary>
      /// Flips the array in one of different 3 ways (row and column indices are 0-based)
      /// </summary>
      /// <param name="src">Source array.</param>
      /// <param name="dst">Destination array.</param>
      /// <param name="flipType">Specifies how to flip the array.</param>
      public static void cvFlip(IntPtr src, IntPtr dst, CvEnum.FLIP flipType)
      {
         int flipMode =
            //-1 indicates vertical and horizontal flip
            flipType == (Emgu.CV.CvEnum.FLIP.HORIZONTAL | Emgu.CV.CvEnum.FLIP.VERTICAL) ? -1 :
         //1 indicates horizontal flip only
            flipType == Emgu.CV.CvEnum.FLIP.HORIZONTAL ? 1 :
         //0 indicates vertical flip only
            0;
         cvFlip(src, dst, flipMode);
      }

      /// <summary>
      /// Draws the line segment between pt1 and pt2 points in the image. The line is clipped by the image or ROI rectangle. For non-antialiased lines with integer coordinates the 8-connected or 4-connected Bresenham algorithm is used. Thick lines are drawn with rounding endings. Antialiased lines are drawn using Gaussian filtering.
      /// </summary>
      /// <param name="img">The image</param>
      /// <param name="pt1">First point of the line segment</param>
      /// <param name="pt2">Second point of the line segment</param>
      /// <param name="color">Line color</param>
      /// <param name="thickness">Line thickness. </param>
      /// <param name="lineType">Type of the line:
      /// 8 (or 0) - 8-connected line.
      /// 4 - 4-connected line.
      /// CV_AA - antialiased line. 
      /// </param>
      /// <param name="shift">Number of fractional bits in the point coordinates</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLine(
          IntPtr img,
          Point pt1,
          Point pt2,
          MCvScalar color,
          int thickness,
          [MarshalAs(UnmanagedType.U4)] 
          CvEnum.LINE_TYPE lineType,
          int shift);

      /// <summary>
      /// Draws a single or multiple polygonal curves
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="pts">Array of pointers to polylines</param>
      /// <param name="npts">Array of polyline vertex counters</param>
      /// <param name="contours">Number of polyline contours</param>
      /// <param name="isClosed">
      /// Indicates whether the polylines must be drawn closed. 
      /// If !=0, the function draws the line from the last vertex of every contour to the first vertex.
      /// </param>
      /// <param name="color">Polyline color</param>
      /// <param name="thickness">Thickness of the polyline edges</param>
      /// <param name="lineType">Type of the line segments, see cvLine description</param>
      /// <param name="shift">Number of fractional bits in the vertex coordinates</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPolyLine(
         IntPtr img,
         [In]
         IntPtr[] pts,
         [In]
         int[] npts,
         int contours,
         int isClosed,
         MCvScalar color,
         int thickness,
         [MarshalAs(UnmanagedType.U4)] 
         CvEnum.LINE_TYPE lineType,
         int shift);

      /// <summary>
      /// Draws a single or multiple polygonal curves
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="pts">Array of pointers to polylines</param>
      /// <param name="npts">Array of polyline vertex counters</param>
      /// <param name="contours">Number of polyline contours</param>
      /// <param name="isClosed">
      /// Indicates whether the polylines must be drawn closed. 
      /// If true, the function draws the line from the last vertex of every contour to the first vertex.
      /// </param>
      /// <param name="color">Polyline color</param>
      /// <param name="thickness">Thickness of the polyline edges</param>
      /// <param name="lineType">Type of the line segments, see cvLine description</param>
      /// <param name="shift">Number of fractional bits in the vertex coordinates</param>
      public static void cvPolyLine(
         IntPtr img,
         [In]
         IntPtr[] pts,
         [In]
         int[] npts,
         int contours,
         bool isClosed,
         MCvScalar color,
         int thickness,
         CvEnum.LINE_TYPE lineType,
         int shift)
      {
         cvPolyLine(img, pts, npts, contours, isClosed ? 1 : 0, color, thickness, lineType, shift);
      }

      /// <summary>
      /// Draws a rectangle with two opposite corners pt1 and pt2
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="pt1">One of the rectangle vertices</param>
      /// <param name="pt2">Opposite rectangle vertex</param>
      /// <param name="color">Line color </param>
      /// <param name="thickness">Thickness of lines that make up the rectangle. Negative values make the function to draw a filled rectangle.</param>
      /// <param name="lineType">Type of the line</param>
      /// <param name="shift">Number of fractional bits in the point coordinates</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRectangle(
         IntPtr img,
         Point pt1,
         Point pt2,
         MCvScalar color,
         int thickness,
         [MarshalAs(UnmanagedType.U4)] 
         CvEnum.LINE_TYPE lineType,
         int shift);

      #region Accessing Elements and sub-Arrays
      /// <summary>
      /// Returns header, corresponding to a specified rectangle of the input array. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array. ROI is taken into account by the function so the sub-array of ROI is actually extracted.
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the resultant sub-array header.</param>
      /// <param name="rect">Zero-based coordinates of the rectangle of interest.</param>
      /// <returns>the resultant sub-array header</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetSubRect(IntPtr arr, IntPtr submat, Rectangle rect);

      /// <summary>
      /// Return the header, corresponding to a specified row span of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the prelocated memory of resulting sub-array header</param>
      /// <param name="startRow">Zero-based index of the starting row (inclusive) of the span</param>
      /// <param name="endRow">Zero-based index of the ending row (exclusive) of the span</param>
      /// <param name="deltaRow">Index step in the row span. That is, the function extracts every delta_row-th row from start_row and up to (but not including) end_row</param>
      /// <returns>The header, corresponding to a specified row span of the input array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetRows(IntPtr arr, IntPtr submat, int startRow, int endRow, int deltaRow);

      /// <summary>
      /// Return the header, corresponding to a specified row of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the prelocate memory of the resulting sub-array header</param>
      /// <param name="row">Zero-based index of the selected row</param>
      /// <returns>The header, corresponding to a specified row of the input array</returns>
      public static IntPtr cvGetRow(IntPtr arr, IntPtr submat, int row)
      {
         return cvGetRows(arr, submat, row, row + 1, 1);
      }

      /// <summary>
      /// Return the header, corresponding to a specified col span of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the prelocated mempry of the resulting sub-array header</param>
      /// <param name="startCol">Zero-based index of the selected column</param>
      /// <param name="endCol">Zero-based index of the ending column (exclusive) of the span</param>
      /// <returns>The header, corresponding to a specified col span of the input array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetCols(IntPtr arr, IntPtr submat, int startCol, int endCol);

      /// <summary>
      /// Return the header, corresponding to a specified column of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the prelocate memory of the resulting sub-array header</param>
      /// <param name="col">Zero-based index of the selected column</param>
      /// <returns>The header, corresponding to a specified column of the input array</returns>
      public static IntPtr cvGetCol(IntPtr arr, IntPtr submat, int col)
      {
         return cvGetCols(arr, submat, col, col + 1);
      }
      #endregion

      /// <summary>
      /// returns the header, corresponding to a specified diagonal of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the resulting sub-array header</param>
      /// <param name="diag">Array diagonal. Zero corresponds to the main diagonal, -1 corresponds to the diagonal above the main etc., 1 corresponds to the diagonal below the main etc</param>
      /// <returns>Pointer to the resulting sub-array header</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetDiag(IntPtr arr, IntPtr submat, int diag);

      /// <summary>
      /// Returns number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.
      /// </summary>
      /// <param name="arr">array header</param>
      /// <returns>number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern Size cvGetSize(IntPtr arr);

      /// <summary>
      /// Draws a simple or filled circle with given center and radius. The circle is clipped by ROI rectangle.
      /// </summary>
      /// <param name="img">Image where the circle is drawn</param>
      /// <param name="center">Center of the circle</param>
      /// <param name="radius">Radius of the circle.</param>
      /// <param name="color">Color of the circle</param>
      /// <param name="thickness">Thickness of the circle outline if positive, otherwise indicates that a filled circle has to be drawn</param>
      /// <param name="lineType">Type of the circle boundary</param>
      /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCircle(
         IntPtr img,
         Point center,
         int radius,
         MCvScalar color,
         int thickness,
         [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
         int shift);

      /// <summary>
      /// Divides a multi-channel array into separate single-channel arrays. Two modes are available for the operation. If the source array has N channels then if the first N destination channels are not IntPtr.Zero, all they are extracted from the source array, otherwise if only a single destination channel of the first N is not IntPtr.Zero, this particular channel is extracted, otherwise an error is raised. Rest of destination channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to extract a single channel from the image
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst0">Destination channels</param>
      /// <param name="dst1">Destination channels</param>
      /// <param name="dst2">Destination channels</param>
      /// <param name="dst3">Destination channels</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSplit(IntPtr src, IntPtr dst0, IntPtr dst1, IntPtr dst2, IntPtr dst3);

      /// <summary>
      /// Divides a multi-channel array into separate single-channel arrays. Two modes are available for the operation. If the source array has N channels then if the first N destination channels are not IntPtr.Zero, all they are extracted from the source array, otherwise if only a single destination channel of the first N is not IntPtr.Zero, this particular channel is extracted, otherwise an error is raised. Rest of destination channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to extract a single channel from the image
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst0">Destination channels</param>
      /// <param name="dst1">Destination channels</param>
      /// <param name="dst2">Destination channels</param>
      /// <param name="dst3">Destination channels</param>
      public static void cvCvtPixToPlane(IntPtr src, IntPtr dst0, IntPtr dst1, IntPtr dst2, IntPtr dst3)
      {
         cvSplit(src, dst0, dst1, dst2, dst3);
      }

      /// <summary>
      /// Draws a simple or thick elliptic arc or fills an ellipse sector. The arc is clipped by ROI rectangle. A piecewise-linear approximation is used for antialiased arcs and thick arcs. All the angles are given in degrees.
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="center">Center of the ellipse</param>
      /// <param name="axes">Length of the ellipse axes</param>
      /// <param name="angle">Rotation angle</param>
      /// <param name="startAngle">Starting angle of the elliptic arc</param>
      /// <param name="endAngle">Ending angle of the elliptic arc</param>
      /// <param name="color">Ellipse color</param>
      /// <param name="thickness">Thickness of the ellipse arc</param>
      /// <param name="lineType">Type of the ellipse boundary</param>
      /// <param name="shift">Number of fractional bits in the center coordinates and axes' values</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvEllipse(
          IntPtr img,
          Point center,
          Size axes,
          double angle,
          double startAngle,
          double endAngle,
          MCvScalar color,
          int thickness,
          CvEnum.LINE_TYPE lineType,
          int shift);

      /// <summary>
      /// Draws a simple or thick elliptic arc or fills an ellipse sector. The arc is clipped by ROI rectangle. A piecewise-linear approximation is used for antialiased arcs and thick arcs. All the angles are given in degrees.
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="box">The box the define the ellipse area</param>
      /// <param name="color">Ellipse color</param>
      /// <param name="thickness">Thickness of the ellipse arc</param>
      /// <param name="lineType">Type of the ellipse boundary</param>
      /// <param name="shift">Number of fractional bits in the center coordinates and axes' values</param>
      public static void cvEllipseBox(
          IntPtr img,
          MCvBox2D box,
          MCvScalar color,
          int thickness,
          CvEnum.LINE_TYPE lineType,
          int shift)
      {
         Size axes = new Size();
         axes.Width = (int) Math.Round(box.size.Height * 0.5);
         axes.Height = (int) Math.Round(box.size.Width * 0.5);

         cvEllipse(img, Point.Round(box.center), axes, box.angle, 0, 360, color, thickness, lineType, shift);
      }

      /// <summary>
      /// Fills the destination array with values from the look-up table. Indices of the entries are taken from the source array. That is, the function processes each element of src as following:
      /// dst(I)=lut[src(I)+DELTA]
      /// where DELTA=0 if src has depth CV_8U, and DELTA=128 if src has depth CV_8S
      /// </summary>
      /// <param name="src">Source array of 8-bit elements</param>
      /// <param name="dst">Destination array of arbitrary depth and of the same number of channels as the source array</param>
      /// <param name="lut">Look-up table of 256 elements; should have the same depth as the destination array. In case of multi-channel source and destination arrays, the table should either have a single-channel (in this case the same table is used for all channels), or the same number of channels as the source/destination array</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLUT(IntPtr src, IntPtr dst, IntPtr lut);

      /// <summary>
      /// This function has several different purposes and thus has several synonyms. It copies one array to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
      /// dst(I)=src(I)*scale + (shift,shift,...)
      /// All the channels of multi-channel arrays are processed independently.
      /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination array element type, it is set to the nearest representable value on the real axis.
      /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate cvConvert synonym. If source and destination array types have equal types, this is also a special case that can be used to scale and shift a matrix or an image and that fits to cvScale synonym.
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst">Destination array</param>
      /// <param name="scale">Scale factor</param>
      /// <param name="shift">Value added to the scaled source array elements</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConvertScale(IntPtr src, IntPtr dst, double scale, double shift);

      /// <summary>
      /// This function has several different purposes and thus has several synonyms. It copies one array to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
      /// dst(I)=src(I)*scale + (shift,shift,...)
      /// All the channels of multi-channel arrays are processed independently.
      /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination array element type, it is set to the nearest representable value on the real axis.
      /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate cvConvert synonym. If source and destination array types have equal types, this is also a special case that can be used to scale and shift a matrix or an image and that fits to cvScale synonym.
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst">Destination array</param>
      /// <param name="scale">Scale factor</param>
      /// <param name="shift">Value added to the scaled source array elements</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvConvertScale")]
      public static extern void cvCvtScale(IntPtr src, IntPtr dst, double scale, double shift);

      /// <summary>
      /// Same as cvConvertScale(src, dest, 1, 0);
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dest">Destination array</param>
      public static void cvConvert(IntPtr src, IntPtr dest)
      {
         cvConvertScale(src, dest, 1, 0);
      }

      /// <summary>
      /// Similar to cvCvtScale but it stores absolute values of the conversion results:
      /// dst(I)=abs(src(I)*scale + (shift,shift,...))
      /// The function supports only destination arrays of 8u (8-bit unsigned integers) type, for other types the function can be emulated by combination of cvConvertScale and cvAbs functions.
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst">Destination array (should have 8u depth). </param>
      /// <param name="scale">ScaleAbs factor</param>
      /// <param name="shift">Value added to the scaled source array elements</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConvertScaleAbs(IntPtr src, IntPtr dst, double scale, double shift);

      #region statistic
      /// <summary>
      /// Calculates the average value M of array elements, independently for each channel:
      ///N = sumI mask(I)!=0
      ///Mc = 1/N * sumI,mask(I)!=0 arr(I)c
      ///If the array is IplImage and COI is set, the function processes the selected channel only and stores the average to the first scalar component (S0).
      /// </summary>
      /// <param name="arr">The array</param>
      /// <param name="mask">The optional operation mask</param>
      /// <returns>average (mean) of array elements</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvAvg(IntPtr arr, IntPtr mask);

      /// <summary>
      /// The function cvAvgSdv calculates the average value and standard deviation of array elements, independently for each channel
      /// </summary>
      /// <remarks>If the array is IplImage and COI is set, the function processes the selected channel only and stores the average and standard deviation to the first compoenents of output scalars (M0 and S0).</remarks>
      /// <param name="arr">The array</param>
      /// <param name="mean">Pointer to the mean value</param>
      /// <param name="stdDev">Pointer to the standard deviation</param>
      /// <param name="mask">The optional operation mask</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAvgSdv(IntPtr arr, ref MCvScalar mean, ref MCvScalar stdDev, IntPtr mask);

      /// <summary>
      /// Calculates sum S of array elements, independently for each channel
      /// Sc = sumI arr(I)c
      /// If the array is IplImage and COI is set, the function processes the selected channel only and stores the sum to the first scalar component (S0).
      /// </summary>
      /// <param name="arr">The array</param>
      /// <returns>The sum of arary elements</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvSum(IntPtr arr);

      /// <summary>
      /// Reduces matrix to a vector by treating the matrix rows/columns as a set of 1D vectors and performing the specified operation on the vectors until a single row/column is obtained. 
      /// </summary>
      /// <remarks>
      /// The function can be used to compute horizontal and vertical projections of an raster image. 
      /// In case of CV_REDUCE_SUM and CV_REDUCE_AVG the output may have a larger element bit-depth to preserve accuracy. 
      /// And multi-channel arrays are also supported in these two reduction modes
      /// </remarks>
      /// <param name="src">The input matrix</param>
      /// <param name="dst">The output single-row/single-column vector that accumulates somehow all the matrix rows/columns</param>
      /// <param name="dim">The dimension index along which the matrix is reduce.</param>
      /// <param name="type">The reduction operation type</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReduce(IntPtr src, IntPtr dst, CvEnum.REDUCE_DIMENSION dim, CvEnum.REDUCE_TYPE type);
      #endregion

      /// <summary>
      /// Releases the header and the image data.
      /// </summary>
      /// <param name="image">Double pointer to the header of the deallocated image</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseImage(ref IntPtr image);

      /// <summary>
      /// Draws contour outlines in the image if thickness &gt;=0 or fills area bounded by the contours if thickness&lt;0.
      /// </summary>
      /// <param name="img">Image where the contours are to be drawn. Like in any other drawing function, the contours are clipped with the ROI</param>
      /// <param name="contour">Pointer to the first contour</param>
      /// <param name="externalColor">Color of the external contours</param>
      /// <param name="holeColor">Color of internal contours </param>
      /// <param name="maxLevel">Maximal level for drawn contours. If 0, only contour is drawn. If 1, the contour and all contours after it on the same level are drawn. If 2, all contours after and all contours one level below the contours are drawn, etc. If the value is negative, the function does not draw the contours following after contour but draws child contours of contour up to abs(maxLevel)-1 level. </param>
      /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative the contour interiors are drawn</param>
      /// <param name="lineType">Type of the contour segments</param>
      /// <param name="offset">Shift all the point coordinates by the specified value. It is useful in case if the contours retrived in some image ROI and then the ROI offset needs to be taken into account during the rendering. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDrawContours(
          IntPtr img,
          IntPtr contour,
          MCvScalar externalColor,
          MCvScalar holeColor,
          int maxLevel,
          int thickness,
          [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
          Point offset);

      /// <summary>
      /// Fills convex polygon interior. This function is much faster than The function cvFillPoly and can fill not only the convex polygons but any monotonic polygon, i.e. a polygon whose contour intersects every horizontal line (scan line) twice at the most
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="pts">Array of pointers to a single polygon</param>
      /// <param name="npts">Polygon vertex counter</param>
      /// <param name="color">Polygon color</param>
      /// <param name="lineType">Type of the polygon boundaries</param>
      /// <param name="shift">Number of fractional bits in the vertex coordinates</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFillConvexPoly(
         IntPtr img,
         [In]
         Point[] pts,
         int npts,
         MCvScalar color,
         [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
         int shift);

      #region Text
      /// <summary>
      /// Initializes the font structure that can be passed to text rendering functions
      /// </summary>
      /// <param name="font">Pointer to the font structure initialized by the function</param>
      /// <param name="fontFace">Font name identifier. Only a subset of Hershey fonts are supported now</param>
      /// <param name="hscale">Horizontal scale. If equal to 1.0f, the characters have the original width depending on the font type. If equal to 0.5f, the characters are of half the original width</param>
      /// <param name="vscale">Vertical scale. If equal to 1.0f, the characters have the original height depending on the font type. If equal to 0.5f, the characters are of half the original height</param>
      /// <param name="shear">Approximate tangent of the character slope relative to the vertical line. Zero value means a non-italic font, 1.0f means 45 slope, etc. thickness Thickness of lines composing letters outlines. The function cvLine is used for drawing letters</param>
      /// <param name="thickness">Thickness of the text strokes</param>
      /// <param name="lineType">Type of the strokes</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInitFont(
          ref MCvFont font,
          CvEnum.FONT fontFace,
          double hscale,
          double vscale,
          double shear,
          int thickness,
          [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType);

      /// <summary>
      /// Renders the text in the image with the specified font and color. The printed text is clipped by ROI rectangle. Symbols that do not belong to the specified font are replaced with the rectangle symbol.
      /// </summary>
      /// <param name="img">Input image</param>
      /// <param name="text">String to print</param>
      /// <param name="org">Coordinates of the bottom-left corner of the first letter</param>
      /// <param name="font">Pointer to the font structure</param>
      /// <param name="color">Text color</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPutText(IntPtr img, [MarshalAs(UnmanagedType.LPStr)] String text, Point org, ref MCvFont font, MCvScalar color);

      /// <summary>
      /// Calculates the binding rectangle for the given text string when a specified font is used
      /// </summary>
      /// <param name="textString">Input string</param>
      /// <param name="font">The font structure</param>
      /// <param name="textSize">Resultant size of the text string. Height of the text does not include the height of character parts that are below the baseline</param>
      /// <param name="baseline">y-coordinate of the baseline relatively to the bottom-most text point</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetTextSize([MarshalAs(UnmanagedType.LPStr)] String textString, ref MCvFont font, ref Size textSize, ref int baseline);
      #endregion

      /// <summary>
      /// Copies the entire sequence or subsequence to the specified buffer and returns the pointer to the buffer
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="elements">Pointer to the destination array that must be large enough. It should be a pointer to data, not a matrix header</param>
      /// <param name="slice">The sequence part to copy to the array</param>
      /// <returns>the pointer to the buffer</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCvtSeqToArray(IntPtr seq, IntPtr elements, MCvSlice slice);

      /// <summary>
      /// Initializes sequence header for array. The sequence header as well as the sequence block are allocated by the user (for example, on stack). No data is copied by the function. The resultant sequence will consists of a single block and have IntPtr.Zero storage pointer, thus, it is possible to read its elements, but the attempts to add elements to the sequence will raise an error in most cases
      /// </summary>
      /// <param name="seqType">Type of the created sequence</param>
      /// <param name="headerSize">Size of the header of the sequence. Parameter sequence must point to the structure of that size or greater size.</param>
      /// <param name="elemSize">Size of the sequence element</param>
      /// <param name="elements">Elements that will form a sequence</param>
      /// <param name="total">Total number of elements in the sequence. The number of array elements must be equal to the value of this parameter</param>
      /// <param name="seq">Pointer to the local variable that is used as the sequence header. </param>
      /// <param name="block">Pointer to the local variable that is the header of the single sequence block. </param>
      /// <returns>Pointer to the local variable that is used as the sequence header</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvMakeSeqHeaderForArray(
         int seqType,
         int headerSize,
         int elemSize,
         IntPtr elements,
         int total,
         IntPtr seq,
         IntPtr block);

      /// <summary>
      /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole array, selected ROI (in case of IplImage) or, if mask is not IntPtr.Zero, in the specified array region. If the array has more than one channel, it must be IplImage with COI set. In case if multi-dimensional arrays min_loc->x and max_loc->x will contain raw (linear) positions of the extremums
      /// </summary>
      /// <param name="arr">The source array, single-channel or multi-channel with COI set</param>
      /// <param name="minVal">Pointer to returned minimum value</param>
      /// <param name="maxVal">Pointer to returned maximum value</param>
      /// <param name="minLoc">Pointer to returned minimum location</param>
      /// <param name="maxLoc">Pointer to returned maximum location</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMinMaxLoc(
         IntPtr arr,
         ref double minVal,
         ref double maxVal,
         ref Point minLoc,
         ref Point maxLoc,
         IntPtr mask);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvGet1D(IntPtr arr, int idx0);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvGet2D(IntPtr arr, int idx0, int idx1);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <param name="idx2">The third zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvGet3D(IntPtr arr, int idx0, int idx1, int idx2);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetReal1D(IntPtr arr, int idx0);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetReal2D(IntPtr arr, int idx0, int idx1);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <param name="idx2">The third zero-based component of the element index </param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetReal3D(IntPtr arr, int idx0, int idx1, int idx2);

      /// <summary>
      /// Return the value of the specified bin of 1D histogram. In case of sparse histogram the function returns 0, if the bin is not present in the histogram, and no new bin is created. 
      /// </summary>
      /// <param name="hist">Histogram</param>
      /// <param name="idx0">Indices of the bin</param>
      /// <returns>the value of the specified bin of 1D histogram</returns>
      public static double cvQueryHistValue_1D(IntPtr hist, int idx0)
      {
         return cvGetReal1D(Marshal.ReadIntPtr(hist, sizeof(int)), idx0);
         //MCvHistogram h = (MCvHistogram) Marshal.PtrToStructure(hist, typeof(MCvHistogram));
         //return cvGetReal1D(h.bins, idx0); 
      }

      /// <summary>
      /// Return the value of the specified bin of 2D histogram. In case of sparse histogram the function returns 0, if the bin is not present in the histogram, and no new bin is created. 
      /// </summary>
      /// <param name="hist">Histogram</param>
      /// <param name="idx0">Indices of the bin</param>
      /// <param name="idx1">Indices of the bin</param>
      /// <returns>the value of the specified bin of 2D histogram</returns>
      public static double cvQueryHistValue_2D(IntPtr hist, int idx0, int idx1)
      {
         return cvGetReal2D(Marshal.ReadIntPtr(hist, sizeof(int)), idx0, idx1);
      }

      /// <summary>
      /// Return the value of the specified bin of 3D histogram. In case of sparse histogram the function returns 0, if the bin is not present in the histogram, and no new bin is created. 
      /// </summary>
      /// <param name="hist">Histogram</param>
      /// <param name="idx0">Indices of the bin</param>
      /// <param name="idx1">Indices of the bin</param>
      /// <param name="idx2">Indices of the bin</param>
      /// <returns>the value of the specified bin of 3D histogram</returns>
      public static double cvQueryHistValue_3D(IntPtr hist, int idx0, int idx1, int idx2)
      {
         return cvGetReal3D(Marshal.ReadIntPtr(hist, sizeof(int)), idx0, idx1, idx2);
      }

      /// <summary>
      /// Switches between the mode, where only pure C implementations from cxcore, OpenCV etc. are used, and the mode, where IPP and MKL functions are used if available. When cvUseOptimized(0) is called, all the optimized libraries are unloaded. The function may be useful for debugging, IPP&amp;MKL upgrade on the fly, online speed comparisons etc.  Note that by default the optimized plugins are loaded, so it is not necessary to call cvUseOptimized(1) in the beginning of the program (actually, it will only increase the startup time)
      /// </summary>
      /// <param name="optimize">1 to turn on optimization, 0 to turn off</param>
      /// <returns>The number of optimized functions loaded</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvUseOptimized(int optimize);

      /// <summary>
      /// Switches between the mode, where only pure C implementations from cxcore, OpenCV etc. are used, and the mode, where IPP and MKL functions are used if available. When cvUseOptimized(0) is called, all the optimized libraries are unloaded. The function may be useful for debugging, IPP&amp;MKL upgrade on the fly, online speed comparisons etc.  Note that by default the optimized plugins are loaded, so it is not necessary to call cvUseOptimized(1) in the beginning of the program (actually, it will only increase the startup time)
      /// </summary>
      /// <param name="optimize">true to turn on optimization, false to turn off</param>
      /// <returns>The number of optimized functions loaded</returns>
      public static int cvUseOptimized(bool optimize)
      {
         return cvUseOptimized(optimize ? 1 : 0);
      }

      /// <summary>
      /// Fills the destination array with uniformly or normally distributed random numbers.
      /// </summary>
      /// <param name="rng">the seed for the random number generator</param>
      /// <param name="arr">The destination array</param>
      /// <param name="dist_type">Distribution type</param>
      /// <param name="param1">The first parameter of distribution. In case of uniform distribution it is the inclusive lower boundary of random numbers range. In case of normal distribution it is the mean value of random numbers</param>
      /// <param name="param2">The second parameter of distribution. In case of uniform distribution it is the exclusive upper boundary of random numbers range. In case of normal distribution it is the standard deviation of random numbers</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRandArr(ref UInt64 rng, IntPtr arr, CvEnum.RAND_TYPE dist_type, MCvScalar param1, MCvScalar param2);

      #region Linear Algebra
      /// <summary>
      /// Calculates and returns the Euclidean dot product of two arrays.
      /// src1 dot src2 = sumI(src1(I)*src2(I))
      /// In case of multiple channel arrays the results for all channels are accumulated. In particular, cvDotProduct(a,a), where a is a complex vector, will return ||a||2. The function can process multi-dimensional arrays, row by row, layer by layer and so on.
      /// </summary>
      /// <param name="src1">The first source array.</param>
      /// <param name="src2">The second source array</param>
      /// <returns>the Euclidean dot product of two arrays</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvDotProduct(IntPtr src1, IntPtr src2);

      /// <summary>
      /// Computes eigenvalues and eigenvectors of a symmetric matrix
      /// </summary>
      /// <param name="mat">The input symmetric square matrix, modified during the processing</param>
      /// <param name="evects">The output matrix of eigenvectors, stored as subsequent rows</param>
      /// <param name="evals">The output vector of eigenvalues, stored in the descending order (order of eigenvalues and eigenvectors is syncronized, of course)</param>
      /// <param name="eps">Accuracy of diagonalization. Typically, DBL EPSILON (about 10^(-15)) works well. THIS PARAMETER IS CURRENTLY IGNORED.</param>
      /// <param name="lowindex">Optional index of largest eigenvalue/-vector to calculate. If either low- or highindex is supplied the other is required, too. Indexing is 1-based. Use 0 for default.</param>
      /// <param name="highindex">Optional index of smallest eigenvalue/-vector to calculate. If either low- or highindex is supplied the other is required, too. Indexing is 1-based. Use 0 for default.</param>
      /// <remarks>Currently the function is slower than cvSVD yet less accurate, so if A is known to be positivelydefined (for example, it is a covariance matrix)it is recommended to use cvSVD to find eigenvalues and eigenvectors of A, especially if eigenvectors are not required.</remarks>
      /// <example>To calculate the largest eigenvector/-value set lowindex = highindex = 1. For legacy reasons this function always returns a square matrix the same size as the source matrix with eigenvectors and a vector the length of the source matrix with eigenvalues. The selected eigenvectors/-values are always in the first highindex - lowindex + 1 rows.</example>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvEigenVV(
         IntPtr mat,
         IntPtr evects,
         IntPtr evals,
         double eps,
         int lowindex,
         int highindex);

      /// <summary>
      /// normalizes the input array so that it's norm or value range takes the certain value(s).
      /// </summary>
      /// <param name="src">The input array</param>
      /// <param name="dst">The output array; in-place operation is supported</param>
      /// <param name="a">The minimum/maximum value of the output array or the norm of output array</param>
      /// <param name="b">The maximum/minimum value of the output array</param>
      /// <param name="norm_type">The normalization type</param>
      /// <param name="mask">The operation mask. Makes the function consider and normalize only certain array elements</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvNormalize(
         IntPtr src,
         IntPtr dst,
         double a,
         double b,
         CvEnum.NORM_TYPE norm_type,
         IntPtr mask);

      /// <summary>
      /// Calculates the cross product of two 3D vectors
      /// </summary>
      /// <param name="src1">The first source vector</param>
      /// <param name="src2">The second source vector</param>
      /// <param name="dst">The destination vect</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCrossProduct(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// Performs generalized matrix multiplication:
      /// dst = alpha*op(src1)*op(src2) + beta*op(src3), where op(X) is X or XT
      /// </summary>
      /// <param name="src1">The first source array. </param>
      /// <param name="src2">The second source array. </param>
      /// <param name="alpha">The scalar</param>
      /// <param name="src3">The third source array (shift). Can be IntPtr.Zero, if there is no shift.</param>
      /// <param name="beta">The scalar</param>
      /// <param name="dst">The destination array.</param>
      /// <param name="tABC">The gemm operation type</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGEMM(
          IntPtr src1,
          IntPtr src2,
          double alpha,
          IntPtr src3,
          double beta,
          IntPtr dst,
          CvEnum.GEMM_TYPE tABC);

      /// <summary>
      /// Performs matrix transformation of every element of array src and stores the results in dst
      /// Both source and destination arrays should have the same depth and the same size or selected ROI size. transmat and shiftvec should be real floating-point matrices.
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="transmat">Transformation matrix</param>
      /// <param name="shiftvec">Optional shift vector</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvTransform(IntPtr src, IntPtr dst, IntPtr transmat, IntPtr shiftvec);

      /// <summary>
      /// Transforms every element of src (by treating it as 2D or 3D vector) in the following way:
      /// (x, y, z) -> (x'/w, y'/w, z'/w) or
      /// (x, y) -> (x'/w, y'/w),
      /// where
      /// (x', y', z', w') = mat4x4 * (x, y, z, 1) or
      /// (x', y', w') = mat3x3 * (x, y, 1)
      /// and w = w'   if w'!=0,
      ///        inf  otherwise
      /// </summary>
      /// <param name="src">The source three-channel floating-point array</param>
      /// <param name="dst">The destination three-channel floating-point array</param>
      /// <param name="mat">3x3 or 4x4 transformation matrix</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPerspectiveTransform(IntPtr src, IntPtr dst, IntPtr mat);

      /// <summary>
      /// Calculates the product of src and its transposition.
      /// The function evaluates dst=scale(src-delta)*(src-delta)^T if order=0, and dst=scale(src-delta)^T*(src-delta) otherwise.
      /// </summary>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix</param>
      /// <param name="order">Order of multipliers</param>
      /// <param name="delta">An optional array, subtracted from <paramref name="src"/> before multiplication</param>
      /// <param name="scale">An optional scaling</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMulTransposed(
         IntPtr src,
         IntPtr dst,
         int order,
         IntPtr delta,
         double scale);

      /// <summary>
      /// Returns sum of diagonal elements of the matrix <paramref name="mat"/>.
      /// </summary>
      /// <param name="mat">the matrix</param>
      /// <returns>sum of diagonal elements of the matrix src1</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvScalar cvTrace(IntPtr mat);

      /// <summary>
      /// Transposes matrix src1:
      /// dst(i,j)=src(j,i)
      /// Note that no complex conjugation is done in case of complex matrix. Conjugation should be done separately: look at the sample code in cvXorS for example
      /// </summary>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvTranspose(IntPtr src, IntPtr dst);

      /// <summary>
      /// Returns determinant of the square matrix mat. The direct method is used for small matrices and Gaussian elimination is used for larger matrices. For symmetric positive-determined matrices it is also possible to run SVD with U=V=NULL and then calculate determinant as a product of the diagonal elements of W
      /// </summary>
      /// <param name="mat">The pointer to the matrix</param>
      /// <returns>determinant of the square matrix mat</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvDet(IntPtr mat);

      /// <summary>
      /// Inverts matrix src1 and stores the result in src2
      /// </summary>
      /// <param name="src">The source matrix.</param>
      /// <param name="dst">The destination matrix</param>
      /// <param name="method">Inversion method</param>
      /// <returns></returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvInvert(IntPtr src, IntPtr dst, CvEnum.SOLVE_METHOD method);

      /// <summary>
      /// Decomposes matrix A into a product of a diagonal matrix and two orthogonal matrices:
      /// A=U*W*VT
      /// Where W is diagonal matrix of singular values that can be coded as a 1D vector of singular values and U and V. All the singular values are non-negative and sorted (together with U and V columns) in descenting order.
      /// </summary>
      /// <remarks>
      /// SVD algorithm is numerically robust and its typical applications include: 
      /// 1. accurate eigenvalue problem solution when matrix A is square, symmetric and positively defined matrix, for example, when it is a covariation matrix. W in this case will be a vector of eigen values, and U=V is matrix of eigen vectors (thus, only one of U or V needs to be calculated if the eigen vectors are required) 
      /// 2. accurate solution of poor-conditioned linear systems 
      /// 3. least-squares solution of overdetermined linear systems. This and previous is done by cvSolve function with CV_SVD method 
      /// 4. accurate calculation of different matrix characteristics such as rank (number of non-zero singular values), condition number (ratio of the largest singular value to the smallest one), determinant (absolute value of determinant is equal to the product of singular values). All the things listed in this item do not require calculation of U and V matrices. 
      /// </remarks>
      /// <param name="A">Source MxN matrix</param>
      /// <param name="W">Resulting singular value matrix (MxN or NxN) or vector (Nx1). </param>
      /// <param name="U">Optional left orthogonal matrix (MxM or MxN). If CV_SVD_U_T is specified, the number of rows and columns in the sentence above should be swapped</param>
      /// <param name="V">Optional right orthogonal matrix (NxN)</param>
      /// <param name="flags">Operation flags</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSVD(IntPtr A, IntPtr W, IntPtr U, IntPtr V, CvEnum.SVD_TYPE flags);

      /// <summary>
      /// Calculates the covariation matrix and, optionally, mean vector of the set of input vectors. 
      /// </summary>
      /// <remarks>The function can be used for PCA, for comparing vectors using Mahalanobis distance etc. </remarks>
      /// <param name="vects">The input vectors. They all must have the same type and the same size. The vectors do not have to be 1D, they can be 2D (e.g. images) etc</param>
      /// <param name="count">The number of input vectors</param>
      /// <param name="covMat">The output covariation matrix that should be floating-point and square</param>
      /// <param name="avg">The input or output (depending on the flags) array - the mean (average) vector of the input vectors. </param>
      /// <param name="flags">The operation flags</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcCovarMatrix(
          IntPtr[] vects,
          int count,
          IntPtr covMat,
          IntPtr avg,
          CvEnum.COVAR_METHOD flags);

      /// <summary>
      /// Calculates the weighted distance between two vectors and returns it
      /// </summary>
      /// <param name="vec1">The first 1D source vector</param>
      /// <param name="vec2">The second 1D source vector</param>
      /// <param name="mat">The inverse covariation matrix</param>
      /// <returns>the Mahalanobis distance</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvMahalanobis(IntPtr vec1, IntPtr vec2, IntPtr mat);

      /// <summary>
      /// Performs PCA analysis of the vector set. First, it uses cvCalcCovarMatrix to compute covariation matrix and then it finds its eigenvalues and eigenvectors. The output number of eigenvalues/eigenvectors should be less than or equal to MIN(rows(data),cols(data)). 
      /// </summary>
      /// <param name="data">The input data; each vector is either a single row (CV_PCA_DATA_AS_ROW) or a single column (CV_PCA_DATA_AS_COL). </param>
      /// <param name="avg">The mean (average) vector, computed inside the function or provided by user</param>
      /// <param name="eigenvalues">The output eigenvalues of covariation matrix. </param>
      /// <param name="eigenvectors">The output eigenvectors of covariation matrix (i.e. principal components); one vector per row.</param>
      /// <param name="flags"></param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcPCA(
          IntPtr data,
          IntPtr avg,
          IntPtr eigenvalues,
          IntPtr eigenvectors,
          CvEnum.PCA_TYPE flags);


      /// <summary>
      /// Projects vectors to the specified subspace
      /// </summary>
      /// <param name="data">The input data. Each vector is eigher a single row or a single column.</param>
      /// <param name="avg">
      /// The mean (average) vector. If it is a single-row vector, it means that the input vectors are stored as rows of data;
      /// Otherwise, it should be a single-column vector, then the vectors are stored as columns of data.
      /// </param>
      /// <param name="eigenvectors">
      /// The eigenvectors (principle components). One vector per row.
      /// </param>
      /// <param name="result">
      /// The output matrix of decomposition coefficients.
      /// The number of rows must be the same as the number of vectors, the number of columns
      /// must be less than or equal to the number of rows in eigenvectos.
      /// That it is less, the input vectors are projected into subspace of the first cols(result)
      /// principle components.
      /// </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvProjectPCA(
          IntPtr data,
          IntPtr avg,
          IntPtr eigenvectors,
          IntPtr result);
      #endregion

      /// <summary>
      /// Fills output variables with low-level information about the array data. All output parameters are optional, so some of the pointers may be set to NULL. If the array is IplImage with ROI set, parameters of ROI are returned. 
      /// </summary>
      /// <param name="arr">Array header</param>
      /// <param name="data">Output pointer to the whole image origin or ROI origin if ROI is set</param>
      /// <param name="step">Output full row length in bytes</param>
      /// <param name="roiSize">Output ROI size</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetRawData(IntPtr arr, out IntPtr data, out int step, out Size roiSize);

      /// <summary>
      /// Returns matrix header for the input array that can be matrix - CvMat, image - IplImage or multi-dimensional dense array - CvMatND* (latter case is allowed only if allowND != 0) . In the case of matrix the function simply returns the input pointer. In the case of IplImage* or CvMatND* it initializes header structure with parameters of the current image ROI and returns pointer to this temporary structure. Because COI is not supported by CvMat, it is returned separately. 
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="header">Pointer to CvMat structure used as a temporary buffer</param>
      /// <param name="coi">Optional output parameter for storing COI</param>
      /// <param name="allowND">If non-zero, the function accepts multi-dimensional dense arrays (CvMatND*) and returns 2D (if CvMatND has two dimensions) or 1D matrix (when CvMatND has 1 dimension or more than 2 dimensions). The array must be continuous</param>
      /// <returns>Returns matrix header for the input array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetMat(IntPtr arr, IntPtr header, out int coi, int allowND);

      /// <summary>
      /// Returns image header for the input array that can be matrix - CvMat*, or image - IplImage*.
      /// </summary>
      /// <param name="arr">Input array. </param>
      /// <param name="imageHeader">Pointer to IplImage structure used as a temporary buffer.</param>
      /// <returns>Returns image header for the input array</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetImage(IntPtr arr, IntPtr imageHeader);

      /// <summary>
      /// Checks that every array element is neither NaN nor Infinity. If CV_CHECK_RANGE is set, it also checks that every element is greater than or equal to minVal and less than maxVal. 
      /// </summary>
      /// <param name="arr">The array to check.</param>
      /// <param name="flags">The operation flags, CHECK_NAN_INFINITY or combination of
      /// CHECK_RANGE - if set, the function checks that every value of array is within [minVal,maxVal) range, otherwise it just checks that every element is neigther NaN nor Infinity.
      /// CHECK_QUIET - if set, the function does not raises an error if an element is invalid or out of range 
      /// </param>
      /// <param name="minVal">The inclusive lower boundary of valid values range. It is used only if CHECK_RANGE is set.</param>
      /// <param name="maxVal">The exclusive upper boundary of valid values range. It is used only if CHECK_RANGE is set.</param>
      /// <returns>Returns nonzero if the check succeeded, i.e. all elements are valid and within the range, and zero otherwise. In the latter case if CV_CHECK_QUIET flag is not set, the function raises runtime error.</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvCheckArr(IntPtr arr, CvEnum.CHECK_TYPE flags, double minVal, double maxVal);

      /// <summary>
      /// Return the current number of threads that are used by parallelized (via OpenMP) OpenCV functions.
      /// </summary>
      /// <returns>the current number of threads that are used by parallelized (via OpenMP) OpenCV functions</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetNumThreads();

      /// <summary>
      /// Sets the number of threads that are used by parallelized OpenCV functions. 
      /// </summary>
      /// <param name="threadsCount">The number of threads that are used by parallelized OpenCV functions. When the argument is zero or negative, and at the beginning of the program, the number of threads is set to the number of processors in the system, as returned by the function omp_get_num_procs() from OpenMP runtime. </param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetNumThreads(int threadsCount);

      /// <summary>
      /// Returns the index, from 0 to cvGetNumThreads()-1, of the thread that called the function. It is a wrapper for the function omp_get_thread_num() from OpenMP runtime. The retrieved index may be used to access local-thread data inside the parallelized code fragments. 
      /// </summary>
      /// <returns>The index, from 0 to cvGetNumThreads()-1, of the thread that called the function. It is a wrapper for the function omp_get_thread_num() from OpenMP runtime. The retrieved index may be used to access local-thread data inside the parallelized code fragments. </returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetThreadNum();

      /// <summary>
      /// Compares the corresponding elements of two arrays and fills the destination mask array:
      /// dst(I)=src1(I) op src2(I),
      /// dst(I) is set to 0xff (all '1'-bits) if the particular relation between the elements is true and 0 otherwise. 
      /// All the arrays must have the same type, except the destination, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first image to compare with</param>
      /// <param name="src2">The second image to comapare with</param>
      /// <param name="dst">dst(I) is set to 0xff (all '1'-bits) if the particular relation between the elements is true and 0 otherwise.</param>
      /// <param name="cmpOp">The comparison operator type</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCmp(IntPtr src1, IntPtr src2, IntPtr dst, CvEnum.CMP_TYPE cmpOp);

      /// <summary>
      /// Compares the corresponding elements of array and scalar and fills the destination mask array:
      /// dst(I)=src(I) op scalar,
      /// where op is '=', '&gt;', '&gt;=', '&lt;', '&lt;=' or '!='.
      /// dst(I) is set to 0xff (all '1'-bits) if the particular relation between the elements is true and 0 otherwise. All the arrays must have the same size (or ROI size)
      /// </summary>
      /// <param name="src">The source array, must have a single channel</param>
      /// <param name="value">The scalar value to compare each array element with</param>
      /// <param name="dst">The destination array, must have 8u or 8s type</param>
      /// <param name="cmpOp">The flag specifying the relation between the elements to be checked</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCmpS(IntPtr src, double value, IntPtr dst, CvEnum.CMP_TYPE cmpOp);

      /// <summary>
      /// Returns the textual description for the specified error status code. In case of unknown status the function returns NULL pointer. 
      /// </summary>
      /// <param name="status">The error status</param>
      /// <returns>the textual description for the specified error status code.</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern String cvErrorStr(int status);

      #region File Storage
      /// <summary>
      /// Opens file storage for reading or writing data. In the latter case a new file is created or existing file is rewritten. Type of the read of written file is determined by the filename extension: .xml for XML, and .yml or .yaml for YAML
      /// </summary>
      /// <param name="filename">Name of the file associated with the storage</param>
      /// <param name="memstorage">Memory storage used for temporary data and for storing dynamic structures, such as CvSeq or CvGraph. If it is NULL, a temporary memory storage is created and used</param>
      /// <param name="flags"></param>
      /// <returns>Pointer to CvFileStorage structure</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvOpenFileStorage(
         [MarshalAs(StringMarshalType)] String filename,
         IntPtr memstorage,
         CvEnum.STORAGE_OP flags);

      /// <summary>
      /// Closes the file associated with the storage and releases all the temporary structures. It must be called after all I/O operations with the storage are finished
      /// </summary>
      /// <param name="fs">Reference to the pointer of the released file storage</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseFileStorage(ref IntPtr fs);
      #endregion

      #region Reading Data

      /// <summary>
      /// 
      /// </summary>
      /// <param name="fs">File storage</param>
      /// <param name="map">The parent map. If it is IntPtr.Zero, the function searches a top-level node</param>
      /// <param name="name">The node name</param>
      /// <returns></returns>
      public static IntPtr cvReadByName(
         IntPtr fs,
         IntPtr map,
         String name)
      {
         return cvRead(fs, cvGetFileNodeByName(fs, map, name), IntPtr.Zero);
      }

      /// <summary>
      /// Decodes user object (creates object in a native representation from the file storage subtree) and returns it
      /// </summary>
      /// <param name="fs">File storage</param>
      /// <param name="node">The root object node</param>
      /// <param name="attributes">Unused parameter</param>
      /// <returns>Pointer to the user object</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvRead(
         IntPtr fs,
         IntPtr node,
         IntPtr attributes);

      /// <summary>
      /// Finds a file node by name
      /// </summary>
      /// <param name="fs">File storage</param>
      /// <param name="map">The parent map. If it is NULL, the function searches in all the top-level nodes (streams), starting from the first one. </param>
      /// <param name="name">The file node name</param>
      /// <returns>Pointer to the specific file node</returns>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetFileNodeByName(
         IntPtr fs,
         IntPtr map,
         [MarshalAs(StringMarshalType)] String name);

      #endregion

      #region Miscellaneous Functions
      /// <summary>
      /// implements k-means algorithm that finds centers of cluster_count clusters and groups the input samples around the clusters. On output labels(i) contains a cluster index for sample stored in the i-th row of samples matrix
      /// </summary>
      /// <param name="samples">Floating-point matrix of input samples, one row per sample</param>
      /// <param name="clusterCount">Number of clusters to split the set by</param>
      /// <param name="labels">Output integer vector storing cluster indices for every sample</param>
      /// <param name="termcrit">Specifies maximum number of iterations and/or accuracy (distance the centers move by between the subsequent iterations)</param>
      /// <param name="attempts">The number of attemps. Use 2 if not sure</param>
      /// <param name="rng">Pointer to CvRNG, use IntPtr.Zero if not sure</param>
      /// <param name="flags">Flags, use 0 if not sure</param>
      /// <param name="centers">Pointer to array of centers, use IntPtr.Zero if not sure</param>
      /// <param name="compactness">Pointer to array of doubles, use IntPtr.Zero if not sure</param>
      [DllImport(OPENCV_CORE_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvKMeans2(
         IntPtr samples,
         int clusterCount,
         IntPtr labels,
         MCvTermCriteria termcrit,
         int attempts,
         IntPtr rng,
         CvEnum.KMeansInitType flags,
         IntPtr centers,
         IntPtr compactness);
      #endregion
   }
}
