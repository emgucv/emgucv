using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Library to invoke OpenCV functions
   /// </summary>
   public static class CvInvoke
   {
      /// <summary>
      /// string marshaling type
      /// </summary>
      private const UnmanagedType _stringMarshalType = UnmanagedType.LPStr;

      #region define the PInvoke file names
      // The following name are .Net Windows specific
      // When run under Mono, change the mapping in Emgu.CV.dll.config accordingly

      /// <summary>
      /// The file name of the cxcore library
      /// </summary>
      public const string CXCORE_LIBRARY = "cxcore110.dll";
      /// <summary>
      /// The file name of the cv library
      /// </summary>
      public const string CV_LIBRARY = "cv110.dll";
      /// <summary>
      /// The file name of the highgui library
      /// </summary>
      public const string HIGHGUI_LIBRARY = "highgui110.dll";
      /// <summary>
      /// The file name of the cvaux library
      /// </summary>
      public const string CVAUX_LIBRARY = "cvaux110.dll";
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
      public const string EXTERN_LIBRARY = "cvextern.dll";
      #endregion 

      /// <summary>
      /// Static Constructor to setup opencv environment
      /// </summary>
      static CvInvoke()
      {
         /*
         if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
         {
            
            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //System.IO.FileInfo file = new System.IO.FileInfo(asm.Location);
            //System.IO.DirectoryInfo directory = file.Directory;
            //System.Security.AccessControl.DirectorySecurity security = directory.GetAccessControl();
            //Emgu.Util.Toolbox.SetDllDirectory(directory.FullName);
           
            String loadLibraryErrorMessage =
               "Unable to load {0}. Please check the following: 1. {0} is located in the same folder as Emgu.CV.dll; 2. MSVCRT 8.0 SP1 is installed.";
            LoadLibrary(CXCORE_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(CV_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(HIGHGUI_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(CVAUX_LIBRARY, loadLibraryErrorMessage);
         }*/

         //Use the custom error handler
         cvRedirectError(CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
      }

      /*
      private static void LoadLibrary(string libraryName, string errorMessage)
      {
         errorMessage = String.Format(errorMessage, libraryName);
         try
         {
            IntPtr handle = Emgu.Util.Toolbox.LoadLibrary(libraryName);
            if (handle == IntPtr.Zero)
               throw new DllNotFoundException(errorMessage);
         }
         catch (Exception e)
         {
            throw new DllNotFoundException(errorMessage, e);
         }
      }*/

      #region CXCORE_LIBRARY

      /// <summary>
      /// Returns information about one of or all of the registered modules
      /// </summary>
      /// <param name="moduleName">Name of the module of interest, or NULL, which means all the modules.</param>
      /// <param name="version">Information about the module(s), including version</param>
      /// <param name="loadedAddonPlugins">The list of names and versions of the optimized plugins that CXCORE was able to find and load</param>
      [DllImport(CXCORE_LIBRARY)]
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
      public delegate IntPtr CvAllocFunc(uint size, IntPtr userData);

      /// <summary>
      /// Delegate used to dellocate OpenCV memory
      /// </summary>
      /// <param name="ptr">The memory to dellocate</param>
      /// <param name="userData">User data that is transparetly passed to the custom functions</param>
      /// <returns></returns>
      public delegate int CvFreeFunc(IntPtr ptr, IntPtr userData);

      /// <summary>
      /// The function cvSetMemoryManager sets user-defined memory managment functions (substitutors for malloc and free) that will be called by cvAlloc, cvFree and higher-level functions (e.g. cvCreateImage)
      /// </summary>
      /// <param name="allocFunc">Allocation function</param>
      /// <param name="freeFunc">Deallocation function</param>
      /// <param name="userdata">User data that is transparetly passed to the custom functions</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetMemoryManager(
         CvAllocFunc allocFunc,
         CvFreeFunc freeFunc,
         IntPtr userdata);

      #endregion

      #region Error handling
      /// <summary>
      /// The default Exception callback to handle Error thrown by OpenCV
      /// </summary>
      public static readonly CvErrorCallback CvErrorHandlerThrowException = (CvErrorCallback)CvErrorHandler;
      /// <summary>
      /// An error handler which will ignore any error and continute
      /// </summary>
      public static readonly CvErrorCallback CvErrorHandlerIgnoreError = (CvErrorCallback)CvIgnoreErrorErrorHandler;

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
         }
         finally
         {
            throw new CvException(status, funcName, errMsg, fileName, line);
         }
      }

      /// <summary>
      /// The Error callback that can be registered by cvRedirectError
      /// </summary>
      /// <param name="status">The numeric code for error status</param>
      /// <param name="funcName">The source file name where error is encountered</param>
      /// <param name="errMsg">A description of the error</param>
      /// <param name="fileName">The source file name where error is encountered</param>
      /// <param name="line">The line number in the souce where error is encountered</param>
      /// <param name="userData">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <returns></returns>
      public delegate int CvErrorCallback(
         int status, String funcName, String errMsg, String fileName, int line, IntPtr userData);

      /// <summary>
      /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
      /// </summary>
      /// <param name="errorHandler">The new error handler</param>
      /// <param name="userdata">Arbitrary pointer that is transparetly passed to the error handler.</param>
      /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
      /// <returns></returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvRedirectError(
          IntPtr errorHandler,
          IntPtr userdata,
          IntPtr prevUserdata);

      /// <summary>
      /// Sets the specified error mode.
      /// </summary>
      /// <param name="errorMode">The error mode</param>
      /// <returns></returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvSetErrMode(int errorMode);

      /// <summary>
      /// Returns the current error mode
      /// </summary>
      /// <returns></returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvGetErrMode();

      /// <summary>
      /// Returns the current error status - the value set with the last cvSetErrStatus call. Note, that in Leaf mode the program terminates immediately after error occured, so to always get control after the function call, one should call cvSetErrMode and set Parent or Silent error mode.
      /// </summary>
      /// <returns>the current error status</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvGetErrStatus();

      /// <summary>
      /// Sets the error status to the specified value. Mostly, the function is used to reset the error status (set to it CV_StsOk) to recover after error. In other cases it is more natural to call cvError or CV_ERROR.
      /// </summary>
      /// <param name="code">The error status.</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSeqPush(IntPtr seq, IntPtr element);

      /// <summary>
      /// Adds an element to the front of sequence and retuns pointer to the allocated element. If the input element is NULL, the function simply allocates a space for one more element.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">Added element</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSeqPushFront(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence end.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">If the pointer is not zero, the function copies the removed element to this location</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSeqPop(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence beginning.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="element">If the pointer is not zero, the function copies the removed element to this location</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSeqPopFront(IntPtr seq, IntPtr element);

      /// <summary>
      /// Removes element from sequence middle
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="index">Index of removed element</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSeqRemove(IntPtr seq, int index);

      /// <summary>
      /// Shifts the sequence elements from the inserted position to the nearest end of the sequence and copies the element content there if the pointer is not IntPtr.Zero
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="beforeIndex">Index before which the element is inserted. Inserting before 0 (the minimal allowed value of the parameter) is equal to cvSeqPushFront and inserting before seq->total (the maximal allowed value of the parameter) is equal to cvSeqPush</param>
      /// <param name="element">Inserted element</param>
      /// <returns>Pointer to the inserted element</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvSeqInsert( IntPtr seq, int beforeIndex, IntPtr element );

      /// <summary>
      /// Adds several elements to either end of the sequence. The elements are added to the sequence in the same order as they are arranged in the input array but they can fall into different sequence blocks.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="elements">Added elements</param>
      /// <param name="count">Number of elements to push</param>
      /// <param name="backOrFront">
      /// If != 0, the elements are added to the beginning of sequence;
      /// Otherwise the elements are added to the end of sequence </param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
         T res = (T)Marshal.PtrToStructure(reader.ptr, typeof(T));
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvStartReadSeq(
          IntPtr seq,
          ref MCvSeqReader reader,
          bool reverse);

      /// <summary>
      /// Finds the element with the given index in the sequence and returns the pointer to it. If the element is not found, the function returns 0. The function supports negative indices, where -1 stands for the last sequence element, -2 stands for the one before last, etc. If the sequence is most likely to consist of a single sequence block or the desired element is likely to be located in the first block, then the macro CV_GET_SEQ_ELEM( elemType, seq, index ) should be used, where the parameter elemType is the type of sequence elements ( CvPoint for example), the parameter seq is a sequence, and the parameter index is the index of the desired element. The macro checks first whether the desired element belongs to the first block of the sequence and returns it if it does, otherwise the macro calls the main function GetSeqElem. Negative indices always cause the cvGetSeqElem call. The function has O(1) time complexity assuming that number of blocks is much smaller than the number of elements.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="index">Index of element</param>
      /// <returns>the pointer to the element with the given index in the sequence</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetSeqElem(IntPtr seq, int index);

      /// <summary>
      /// Removes all elements from the sequence. The function does not return the memory to the storage, but this memory is reused later when new elements are added to the sequence. This function time complexity is O(1). 
      /// </summary>
      /// <param name="seq">Sequence</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvRepeat(IntPtr src, IntPtr dst);

      /// <summary>
      /// This function is the opposite to cvSplit. If the destination array has N channels then if the first N input channels are not NULL, all they are copied to the destination array, otherwise if only a single source channel of the first N is not NULL, this particular channel is copied into the destination array, otherwise an error is raised. Rest of source channels (beyond the first N) must always be NULL. For IplImage cvCopy with COI set can be also used to insert a single channel into the image. 
      /// </summary>
      /// <param name="src0">Input channels.</param>
      /// <param name="src1">Input channels.</param>
      /// <param name="src2">Input channels.</param>
      /// <param name="src3">Input channels.</param>
      /// <param name="dst">Destination array. </param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMixChannels( 
         IntPtr[] src, 
         int srcCount,
         IntPtr[] dst, 
         int dstCount,
         int[] fromTo, 
         int pairCount );

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
      /// This function is the opposite to cvSplit. If the destination array has N channels then if the first N input channels are not NULL, all they are copied to the destination array, otherwise if only a single source channel of the first N is not NULL, this particular channel is copied into the destination array, otherwise an error is raised. Rest of source channels (beyond the first N) must always be NULL. For IplImage cvCopy with COI set can be also used to insert a single channel into the image. 
      /// </summary>
      /// <param name="src0">Input channels.</param>
      /// <param name="src1">Input channels.</param>
      /// <param name="src2">Input channels.</param>
      /// <param name="src3">Input channels.</param>
      /// <param name="dst">Destination array. </param>
      [DllImport(CXCORE_LIBRARY, EntryPoint = "cvMerge")]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvNot(IntPtr src, IntPtr des);

      /// <summary>
      /// Calculates per-element maximum of two arrays:
      /// dst(I)=max(src1(I), src2(I))
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array. </param>
      /// <param name="dst">The destination array</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMax(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// <para>Calculates per-element maximum of array and scalar:</para>
      /// <para>dst(I)=max(src(I), value)</para>
      /// <para>All the arrays must have a single channel, the same data type and the same size (or ROI size).</para>
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="value">The scalar value</param>
      /// <param name="dst">The destination array. </param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMaxS(IntPtr src, double value, IntPtr dst);

      /// <summary>
      /// Returns the number of non-zero elements in arr:
      /// result = sumI arr(I)!=0
      /// In case of IplImage both ROI and COI are supported.
      /// </summary>
      /// <param name="arr">The image</param>
      /// <returns>the number of non-zero elements in image</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvCountNonZero(IntPtr arr);

      /// <summary>
      /// Calculates per-element minimum of two arrays:
      /// dst(I)=min(src1(I),src2(I))
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMin(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// Calculates minimum of array and scalar:
      /// dst(I)=min(src(I), value)
      /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="value">The scalar value</param>
      /// <param name="dst">The destination array</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMinS(IntPtr src, double value, IntPtr dst);

      /// <summary>
      /// Adds one array to another one:
      /// dst(I)=src1(I)+src2(I) if mask(I)!=0All the arrays must have the same type, except the mask, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array.</param>
      /// <param name="src2">The second source array.</param>
      /// <param name="dst">The destination array.</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed. </param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSubRS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Divides one array by another:
      /// dst(I)=scale * src1(I)/src2(I), if src1!=NULL
      /// dst(I)=scale/src2(I),      if src1=NULL
      /// All the arrays must have the same type, and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array. If the pointer is NULL, the array is assumed to be all 1s. </param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="scale">Optional scale factor </param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvOrS(IntPtr src, MCvScalar value, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of two arrays:
      /// dst(I)=src1(I)^src2(I) if mask(I)!=0
      /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      /// <param name="mask">mask, 8-bit single channel array; specifies elements of destination array to be changed.</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvCopy(IntPtr src, IntPtr des, IntPtr mask);

      /// <summary>
      /// Copies scalar value to every selected element of the destination array:
      ///arr(I)=value if mask(I)!=0
      ///If array arr is of IplImage type, then is ROI used, but COI must not be set
      /// </summary>
      /// <param name="arr">The destination array</param>
      /// <param name="value">Fill value</param>
      /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSet(IntPtr arr, MCvScalar value, IntPtr mask);

      /// <summary>
      /// Clears the array. In case of dense arrays (CvMat, CvMatND or IplImage) cvZero(array) is equivalent to cvSet(array,cvScalarAll(0),0), in case of sparse arrays all the elements are removed
      /// </summary>
      /// <param name="arr">array to be cleared</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetIdentity(IntPtr mat, MCvScalar value);

      /// <summary>
      /// Initializes the matrix as following:
      /// arr(i,j)=(end-start)*(i*cols(arr)+j)/(cols(arr)*rows(arr))
      /// </summary>
      /// <param name="mat">The matrix to initialize. It should be single-channel 32-bit, integer or floating-point</param>
      /// <param name="start">The lower inclusive boundary of the range</param>
      /// <param name="end">The upper exclusive boundary of the range</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvRange(IntPtr mat, double start, double end);
      #endregion

      #region Math Functions
      /// <summary>
      /// The function emulates the human "foveal" vision and can be used for fast scale and rotation-invariant template matching, for object tracking etc.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="M">Magnitude scale parameter</param>
      /// <param name="flags">A combination of interpolation method and the optional flag CV_WARP_FILL_OUTLIERS and/or CV_WARP_INVERSE_MAP</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvLogPolar(
         IntPtr src,
         IntPtr dst,
         System.Drawing.PointF center,
         double M,
         int flags);

      /// <summary>
      /// Calculates either magnitude, angle, or both of every 2d vector (x(I),y(I)):
      /// magnitude(I)=sqrt( x(I)2+y(I)2 ),
      /// angle(I)=atan( y(I)/x(I) )
      /// The angles are calculated with ~0.1 degree accuracy. For (0,0) point the angle is set to 0
      /// </summary>
      /// <param name="x">The array of x-coordinates </param>
      /// <param name="y">The array of y-coordinates</param>
      /// <param name="magnitude">The destination array of magnitudes, may be set to NULL if it is not needed </param>
      /// <param name="angle">The destination array of angles, may be set to NULL if it is not needed. The angles are measured in radians (0..2?) or in degrees (0..360?). </param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      [DllImport(CXCORE_LIBRARY)]
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
      /// <param name="magnitude">The destination array of magnitudes, may be set to NULL if it is not needed </param>
      /// <param name="angle">The destination array of angles, may be set to NULL if it is not needed. The angles are measured in radians (0..2?) or in degrees (0..360?). </param>
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
      /// <param name="magnitude">The array of magnitudes. If it is NULL, the magnitudes are assumed all 1's</param>
      /// <param name="angle">The array of angles, whether in radians or degrees</param>
      /// <param name="x">The destination array of x-coordinates, may be set to NULL if it is not needed</param>
      /// <param name="y">The destination array of y-coordinates, mau be set to NULL if it is not needed</param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
      [DllImport(CXCORE_LIBRARY)]
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
      /// <param name="magnitude">The array of magnitudes. If it is NULL, the magnitudes are assumed all 1's</param>
      /// <param name="angle">The array of angles, whether in radians or degrees</param>
      /// <param name="x">The destination array of x-coordinates, may be set to NULL if it is not needed</param>
      /// <param name="y">The destination array of y-coordinates, mau be set to NULL if it is not needed</param>
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvPow(IntPtr src, IntPtr dst, double power);

      /// <summary>
      /// Calculates exponent of every element of input array:
      /// dst(I)=exp(src(I))
      /// Maximum relative error is 7e-6. Currently, the function converts denormalized values to zeros on output
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvExp(IntPtr src, IntPtr dst);

      /// <summary>
      /// Calculates natural logarithm of absolute value of every element of input array:
      /// dst(I)=log(abs(src(I))), src(I)!=0
      /// dst(I)=C,  src(I)=0
      /// Where C is large negative number (-700 in the current implementation)
      /// </summary>
      /// <param name="src">The source array</param>
      /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvSolveCubic( IntPtr coeffs, IntPtr roots );

      /// <summary>
      /// Finds all real and complex roots of any degree polynomial with real coefficients
      /// </summary>
      /// <param name="coeffs">The (degree + 1)-length array of equation coefficients (CV_32FC1 or CV_64FC1)</param>
      /// <param name="roots">The degree-length output array of real or complex roots (CV_32FC2 or CV_64FC2)</param>
      /// <param name="maxiter">The maximum number of iterations</param>
      /// <param name="fig">The required figures of precision required</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void  cvSolvePoly(
         IntPtr coeffs, 
         IntPtr roots,
         int maxiter, 
         int fig);
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvGetOptimalDFTSize(int size0);

      /// <summary>
      /// Performs per-element multiplication of the two CCS-packed or complex matrices that are results of real or complex Fourier transform. 
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array of the same type and the same size of the sources</param>
      /// <param name="flags"></param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMulSpectrums(IntPtr src1, IntPtr src2, IntPtr dst, CvEnum.MUL_SPECTRUMS_TYPE flags);

      /// <summary>
      /// Performs forward or inverse transform of 1D or 2D floating-point array
      /// </summary>
      /// <param name="src">Source array, real 1D or 2D array</param>
      /// <param name="dst">Destination array of the same size and same type as the source</param>
      /// <param name="flags">Transformation flags</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvDCT(IntPtr src, IntPtr dst, CvEnum.CV_DCT_TYPE flags);
      #endregion

      /// <summary>
      /// Calculates a part of the line segment which is entirely in the image. It returns 0 if the line segment is completely outside the image and 1 otherwise.
      /// </summary>
      /// <param name="imgSize">Size of the image</param>
      /// <param name="pt1">First ending point of the line segment. It is modified by the function</param>
      /// <param name="pt2">Second ending point of the line segment. It is modified by the function.</param>
      /// <returns>It returns 0 if the line segment is completely outside the image and 1 otherwise.</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvClipLine(System.Drawing.Size imgSize, ref System.Drawing.Point pt1, ref System.Drawing.Point pt2);

      /// <summary>
      /// Calculates absolute difference between two arrays.
      /// dst(I)c = abs(src1(I)c - src2(I)c).
      /// All the arrays must have the same data type and the same size (or ROI size)
      /// </summary>
      /// <param name="src1">The first source array</param>
      /// <param name="src2">The second source array</param>
      /// <param name="dst">The destination array</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvAbsDiff(IntPtr src1, IntPtr src2, IntPtr dst);

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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvAddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst);

      /// <summary>
      /// Does the range check for every element of the input array:
      /// dst(I)=lower0 &lt;= src(I)0 &lt; upper0
      /// for a single-channel array,
      /// dst(I)=lower0 &lt;= src(I)0 &lt; upper0 &amp;&amp;
      ///     lower1 &lt;= src(I)1 &lt; upper1
      /// for a two-channel array etc.
      /// dst(I) is set to 0xff (all '1'-bits) if src(I) is within the range and 0 otherwise. All the arrays must have the same size (or ROI size)
      /// </summary>
      /// <param name="src">The first source array</param>
      /// <param name="lower">The inclusive lower boundary</param>
      /// <param name="upper">The exclusive upper boundary</param>
      /// <param name="dst">The destination array, must have 8u or 8s type</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvInRangeS(IntPtr src, MCvScalar lower, MCvScalar upper, IntPtr dst);

      /// <summary>
      /// Returns the calculated norm. The multiple-channel array are treated as single-channel, that is, the results for all channels are combined. 
      /// </summary>
      /// <param name="arr1">The first source image</param>
      /// <param name="arr2">The second source image. If it is NULL, the absolute norm of arr1 is calculated, otherwise absolute or relative norm of arr1-arr2 is calculated</param>
      /// <param name="normType">Type of norm</param>
      /// <param name="mask">The optional operation mask</param>
      /// <returns>The calculated norm</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCreateImage(
         System.Drawing.Size size, 
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCreateImageHeader(
         System.Drawing.Size size, 
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvInitImageHeader(
         IntPtr image,
         System.Drawing.Size size,
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetData(IntPtr arr, IntPtr data, int step);

      /// <summary>
      /// Releases the header.
      /// </summary>
      /// <param name="image">Pointer to the deallocated header.</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvInitMatHeader(
         IntPtr mat,
         int rows,
         int cols,
         CV.CvEnum.MAT_DEPTH type,
         IntPtr data,
         int step);

      /// <summary>
      /// Sets the channel of interest to a given value. Value 0 means that all channels are selected, 1 means that the first channel is selected etc. If ROI is NULL and coi != 0, ROI is allocated.
      /// </summary>
      /// <param name="image">Image header. </param>
      /// <param name="coi">Channel of interest.</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetImageCOI(IntPtr image, int coi);

      /// <summary>
      /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
      /// </summary>
      /// <param name="image">Image header. </param>
      /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvGetImageCOI(IntPtr image);

      /// <summary>
      /// Releases image ROI. After that the whole image is considered selected.
      /// </summary>
      /// <param name="image">Image header</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvResetImageROI(IntPtr image);

      /// <summary>
      /// Sets the image ROI to a given rectangle. If ROI is NULL and the value of the parameter rect is not equal to the whole image, ROI is allocated. 
      /// </summary>
      /// <param name="image">Image header.</param>
      /// <param name="rect">ROI rectangle.</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetImageROI(IntPtr image, System.Drawing.Rectangle rect);

      /// <summary>
      /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
      /// </summary>
      /// <param name="image">Image header.</param>
      /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern System.Drawing.Rectangle cvGetImageROI(IntPtr image);

      /// <summary>
      /// Allocates header for the new matrix and underlying data, and returns a pointer to the created matrix. Matrices are stored row by row. All the rows are aligned by 4 bytes. 
      /// </summary>
      /// <param name="rows">Number of rows in the matrix.</param>
      /// <param name="cols">Number of columns in the matrix.</param>
      /// <param name="type">Type of the matrix elements.</param>
      /// <returns>A pointer to the created matrix</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCreateMat(int rows, int cols, CvEnum.MAT_DEPTH type);

      /// <summary>
      /// A fast inline substitution for cvInitMatHeader. Namely, it is equivalent to:
      ///     CvMat mat;
      ///     cvInitMatHeader( &amp;mat, rows, cols, type, data, CV_AUTOSTEP );
      /// </summary>
      /// <param name="rows">Number of rows in the matrix.</param>
      /// <param name="cols">Number of columns in the matrix.</param>
      /// <param name="type">Type of the matrix elements (see CreateMat).</param>
      /// <param name="data">Optional data pointer assigned to the matrix header.</param>
      /// <returns></returns>
      public static IntPtr cvMat(int rows, int cols, CV.CvEnum.MAT_DEPTH type, IntPtr data)
      {
         IntPtr mat = Marshal.AllocHGlobal(StructSize.MCvMat);
         CvInvoke.cvInitMatHeader(mat, rows, cols, type, data, 0);
         return mat;
      }

      /// <summary>
      /// Initializes CvMatND structure allocated by the user
      /// </summary>
      /// <param name="mat">Pointer to the array header to be initialized</param>
      /// <param name="dims">Number of array dimensions</param>
      /// <param name="sizes">Array of dimension sizes</param>
      /// <param name="type">Type of array elements</param>
      /// <param name="data">Optional data pointer assigned to the matrix header</param>
      /// <returns>Pointer to the array header</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvReleaseMat(ref IntPtr mat);
      #endregion

      #region Memory Storages
      /// <summary>
      /// Creates a memory storage and returns pointer to it. Initially the storage is empty. All fields of the header, except the block_size, are set to 0.
      /// </summary>
      /// <param name="blockSize"></param>
      /// <returns>Size of the storage blocks in bytes. If it is 0, the block size is set to default value - currently it is 64K. </returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCreateMemStorage(int blockSize);

      /// <summary>
      /// Creates a child memory storage that is similar to simple memory storage except for the differences in the memory allocation/deallocation mechanism. When a child storage needs a new block to add to the block list, it tries to get this block from the parent. The first unoccupied parent block available is taken and excluded from the parent block list. If no blocks are available, the parent either allocates a block or borrows one from its own parent, if any. In other words, the chain, or a more complex structure, of memory storages where every storage is a child/parent of another is possible. When a child storage is released or even cleared, it returns all blocks to the parent. In other aspects, the child storage is the same as the simple storage.
      /// </summary>
      /// <param name="parent">Parent memory storage</param>
      /// <returns>ChildMemStorage</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCreateChildMemStorage(IntPtr parent);

      /// <summary>
      /// Resets the top (free space boundary) of the storage to the very beginning. This function does not deallocate any memory. If the storage has a parent, the function returns all blocks to the parent.
      /// </summary>
      /// <param name="storage">Memory storage</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvClearMemStorage(IntPtr storage);

      /// <summary>
      /// Deallocates all storage memory blocks or returns them to the parent, if any. Then it deallocates the storage header and clears the pointer to the storage. All children of the storage must be released before the parent is released.
      /// </summary>
      /// <param name="storage">Pointer to the released storage</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvReleaseMemStorage(ref IntPtr storage);
      #endregion

      /// <summary>
      /// Loads object from file. It provides a simple interface to cvRead. After object is loaded, the file storage is closed and all the temporary buffers are deleted. Thus, to load a dynamic structure, such as sequence, contour or graph, one should pass a valid destination memory storage to the function.
      /// </summary>
      /// <param name="fileName">File name</param>
      /// <param name="memstorage">Memory storage for dynamic structures, such as CvSeq or CvGraph. It is not used for matrices or images</param>
      /// <param name="name">Optional object name. If it is NULL, the first top-level object in the storage will be loaded</param>
      /// <param name="realName">Optional output parameter that will contain name of the loaded object (useful if name=NULL). </param>
      /// <returns>Loaded object from file</returns>
      [DllImport(CXCORE_LIBRARY, EntryPoint = "cvLoad")]
      private static extern IntPtr _cvLoad(
          [MarshalAs(_stringMarshalType)] String fileName,
          IntPtr memstorage,
          [MarshalAs(_stringMarshalType)] String name,
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
         }
         catch (CvException)
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
            return _cvLoad(fileName, memstorage, name, realName);
         }
      }

      /// <summary>
      /// Creates a sequence that represents the specified slice of the input sequence. The new sequence either shares the elements with the original sequence or has own copy of the elements. So if one needs to process a part of sequence but the processing function does not have a slice parameter, the required sub-sequence may be extracted using this function.
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="slice">The part of the sequence to extract</param>
      /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is NULL, the function uses the storage containing the input sequence.</param>
      /// <param name="copyData">The flag that indicates whether to copy the elements of the extracted slice (copy_data!=0) or not (copy_data=0)</param>
      /// <returns>A pointer to CvSeq</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      /// <param name="storage">The destination storage to keep the new sequence header and the copied data if any. If it is NULL, the function uses the storage containing the input sequence.</param>
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetReal1D(IntPtr arr, int idx0, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index </param>
      /// <param name="value">The assigned value </param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetReal2D(IntPtr arr, int idx0, int idx1, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index </param>
      /// <param name="idx2">The third zero-based component of the element index </param>
      /// <param name="value">The assigned value </param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetReal3D(IntPtr arr, int idx0, int idx1, int idx2, double value);

      /// <summary>
      /// Assign the new value to the particular element of single-channel array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx">Array of the element indices </param>
      /// <param name="value">The assigned value </param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetRealND(
         IntPtr arr,
         [In]
         int[] idx, 
         double value);

      /// <summary>
      /// clears (sets to zero) the particular element of dense array or deletes the element of sparse array. If the element does not exists, the function does nothing
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="idx">Array of the element indices </param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvFlip(IntPtr src, IntPtr dst, int flipMode);

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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvLine(
          IntPtr img,
          System.Drawing.Point pt1,
          System.Drawing.Point pt2,
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvRectangle(
         IntPtr img,
         System.Drawing.Point pt1,
         System.Drawing.Point pt2,
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetSubRect(IntPtr arr, IntPtr submat, System.Drawing.Rectangle rect);

      /// <summary>
      /// Return the header, corresponding to a specified row span of the input array
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="submat">Pointer to the prelocated memory of resulting sub-array header</param>
      /// <param name="startRow">Zero-based index of the starting row (inclusive) of the span</param>
      /// <param name="endRow">Zero-based index of the ending row (exclusive) of the span</param>
      /// <param name="deltaRow">Index step in the row span. That is, the function extracts every delta_row-th row from start_row and up to (but not including) end_row</param>
      /// <returns>The header, corresponding to a specified row span of the input array</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetDiag(IntPtr arr, IntPtr submat, int diag);

      /// <summary>
      /// Returns number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.
      /// </summary>
      /// <param name="arr">array header</param>
      /// <returns>number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern System.Drawing.Size cvGetSize(IntPtr arr);

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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvCircle(
         IntPtr img,
         System.Drawing.Point center,
         int radius,
         MCvScalar color,
         int thickness,
         [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
         int shift);

      /// <summary>
      /// Divides a multi-channel array into separate single-channel arrays. Two modes are available for the operation. If the source array has N channels then if the first N destination channels are not NULL, all they are extracted from the source array, otherwise if only a single destination channel of the first N is not NULL, this particular channel is extracted, otherwise an error is raised. Rest of destination channels (beyond the first N) must always be NULL. For IplImage cvCopy with COI set can be also used to extract a single channel from the image
      /// </summary>
      /// <param name="src">Source array</param>
      /// <param name="dst0">Destination channels</param>
      /// <param name="dst1">Destination channels</param>
      /// <param name="dst2">Destination channels</param>
      /// <param name="dst3">Destination channels</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSplit(IntPtr src, IntPtr dst0, IntPtr dst1, IntPtr dst2, IntPtr dst3);

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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvEllipse(
          IntPtr img,
          System.Drawing.Point center,
          System.Drawing.Size axes,
          double angle,
          double startAngle,
          double endAngle,
          MCvScalar color,
          int thickness,
          [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
          int shift);

      /// <summary>
      /// Fills the destination array with values from the look-up table. Indices of the entries are taken from the source array. That is, the function processes each element of src as following:
      /// dst(I)=lut[src(I)+DELTA]
      /// where DELTA=0 if src has depth CV_8U, and DELTA=128 if src has depth CV_8S
      /// </summary>
      /// <param name="src">Source array of 8-bit elements</param>
      /// <param name="dst">Destination array of arbitrary depth and of the same number of channels as the source array</param>
      /// <param name="lut">Look-up table of 256 elements; should have the same depth as the destination array. In case of multi-channel source and destination arrays, the table should either have a single-channel (in this case the same table is used for all channels), or the same number of channels as the source/destination array</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY, EntryPoint = "cvConvertScale")]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern MCvScalar cvAvg(IntPtr arr, IntPtr mask);

      /// <summary>
      /// The function cvAvgSdv calculates the average value and standard deviation of array elements, independently for each channel
      /// </summary>
      /// <remarks>If the array is IplImage and COI is set, the function processes the selected channel only and stores the average and standard deviation to the first compoenents of output scalars (M0 and S0).</remarks>
      /// <param name="arr">The array</param>
      /// <param name="mean">Pointer to the mean value</param>
      /// <param name="stdDev">Pointer to the standard deviation</param>
      /// <param name="mask">The optional operation mask</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvAvgSdv(IntPtr arr, ref MCvScalar mean, ref MCvScalar stdDev, IntPtr mask);

      /// <summary>
      /// Calculates sum S of array elements, independently for each channel
      /// Sc = sumI arr(I)c
      /// If the array is IplImage and COI is set, the function processes the selected channel only and stores the sum to the first scalar component (S0).
      /// </summary>
      /// <param name="arr">The array</param>
      /// <returns>The sum of arary elements</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      /// <param name="type">The reduction operation type</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvReduce(IntPtr src, IntPtr dst, CvEnum.REDUCE_TYPE type);
      #endregion

      /// <summary>
      /// Releases the header and the image data.
      /// </summary>
      /// <param name="image">Double pointer to the header of the deallocated image</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvDrawContours(
          IntPtr img,
          IntPtr contour,
          MCvScalar externalColor,
          MCvScalar holeColor,
          int maxLevel,
          int thickness,
          [MarshalAs(UnmanagedType.U4)] CvEnum.LINE_TYPE lineType,
          System.Drawing.Point offset);

      /// <summary>
      /// Fills convex polygon interior. This function is much faster than The function cvFillPoly and can fill not only the convex polygons but any monotonic polygon, i.e. a polygon whose contour intersects every horizontal line (scan line) twice at the most
      /// </summary>
      /// <param name="img">Image</param>
      /// <param name="pts">Array of pointers to a single polygon</param>
      /// <param name="npts">Polygon vertex counter</param>
      /// <param name="color">Polygon color</param>
      /// <param name="lineType">Type of the polygon boundaries</param>
      /// <param name="shift">Number of fractional bits in the vertex coordinates</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvFillConvexPoly(
         IntPtr img,
         [In]
         System.Drawing.Point[] pts,
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvPutText(IntPtr img, [MarshalAs(UnmanagedType.LPStr)] String text, System.Drawing.Point org, ref MCvFont font, MCvScalar color);

      /// <summary>
      /// Calculates the binding rectangle for the given text string when a specified font is used
      /// </summary>
      /// <param name="textString">Input string</param>
      /// <param name="font">The font structure</param>
      /// <param name="textSize">Resultant size of the text string. Height of the text does not include the height of character parts that are below the baseline</param>
      /// <param name="baseline">y-coordinate of the baseline relatively to the bottom-most text point</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvGetTextSize([MarshalAs(UnmanagedType.LPStr)] String textString, ref MCvFont font, ref System.Drawing.Size textSize, ref int baseline);
      #endregion

      /// <summary>
      /// Copies the entire sequence or subsequence to the specified buffer and returns the pointer to the buffer
      /// </summary>
      /// <param name="seq">Sequence</param>
      /// <param name="elements">Pointer to the destination array that must be large enough. It should be a pointer to data, not a matrix header</param>
      /// <param name="slice">The sequence part to copy to the array</param>
      /// <returns>the pointer to the buffer</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvCvtSeqToArray(IntPtr seq, IntPtr elements, MCvSlice slice);

      /// <summary>
      /// initializes sequence header for array. The sequence header as well as the sequence block are allocated by the user (for example, on stack). No data is copied by the function. The resultant sequence will consists of a single block and have NULL storage pointer, thus, it is possible to read its elements, but the attempts to add elements to the sequence will raise an error in most cases
      /// </summary>
      /// <param name="seqType">Type of the created sequence</param>
      /// <param name="headerSize">Size of the header of the sequence. Parameter sequence must point to the structure of that size or greater size.</param>
      /// <param name="elemSize">Size of the sequence element</param>
      /// <param name="elements">Elements that will form a sequence</param>
      /// <param name="total">Total number of elements in the sequence. The number of array elements must be equal to the value of this parameter</param>
      /// <param name="seq">Pointer to the local variable that is used as the sequence header. </param>
      /// <param name="block">Pointer to the local variable that is the header of the single sequence block. </param>
      /// <returns>Pointer to the local variable that is used as the sequence header</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvMakeSeqHeaderForArray(
         int seqType, 
         int headerSize, 
         int elemSize,
         IntPtr elements, 
         int total,
         IntPtr seq, 
         IntPtr block);

      /// <summary>
      /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole array, selected ROI (in case of IplImage) or, if mask is not NULL, in the specified array region. If the array has more than one channel, it must be IplImage with COI set. In case if multi-dimensional arrays min_loc->x and max_loc->x will contain raw (linear) positions of the extremums
      /// </summary>
      /// <param name="arr">The source array, single-channel or multi-channel with COI set</param>
      /// <param name="minVal">Pointer to returned minimum value</param>
      /// <param name="maxVal">Pointer to returned maximum value</param>
      /// <param name="minLoc">Pointer to returned minimum location</param>
      /// <param name="maxLoc">Pointer to returned maximum location</param>
      /// <param name="mask">The optional mask that is used to select a subarray</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMinMaxLoc(
         IntPtr arr,
         ref double minVal,
         ref double maxVal,
         ref System.Drawing.Point minLoc,
         ref System.Drawing.Point maxLoc,
         IntPtr mask);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern MCvScalar cvGet1D(IntPtr arr, int idx0);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern MCvScalar cvGet2D(IntPtr arr, int idx0, int idx1);

      /// <summary>
      /// Return the particular array element
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index</param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <param name="idx2">The third zero-based component of the element index</param>
      /// <returns>the particular array element</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern MCvScalar cvGet3D(IntPtr arr, int idx0, int idx1, int idx2);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvGetReal1D(IntPtr arr, int idx0);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvGetReal2D(IntPtr arr, int idx0, int idx1);

      /// <summary>
      /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
      /// </summary>
      /// <param name="arr">Input array. Must have a single channel</param>
      /// <param name="idx0">The first zero-based component of the element index </param>
      /// <param name="idx1">The second zero-based component of the element index</param>
      /// <param name="idx2">The third zero-based component of the element index </param>
      /// <returns>the particular element of single-channel array</returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvDotProduct(IntPtr src1, IntPtr src2);

      /// <summary>
      /// normalizes the input array so that it's norm or value range takes the certain value(s).
      /// </summary>
      /// <param name="src">The input array</param>
      /// <param name="dst">The output array; in-place operation is supported</param>
      /// <param name="a">The minimum/maximum value of the output array or the norm of output array</param>
      /// <param name="b">The maximum/minimum value of the output array</param>
      /// <param name="norm_type">The normalization type</param>
      /// <param name="mask">The operation mask. Makes the function consider and normalize only certain array elements</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvCrossProduct(IntPtr src1, IntPtr src2, IntPtr dst);

      /// <summary>
      /// Performs generalized matrix multiplication:
      /// dst = alpha*op(src1)*op(src2) + beta*op(src3), where op(X) is X or XT
      /// </summary>
      /// <param name="src1">The first source array. </param>
      /// <param name="src2">The second source array. </param>
      /// <param name="alpha"></param>
      /// <param name="src3">The third source array (shift). Can be NULL, if there is no shift.</param>
      /// <param name="beta"></param>
      /// <param name="dst">The destination array.</param>
      /// <param name="tABC"></param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvPerspectiveTransform(IntPtr src, IntPtr dst, IntPtr mat);

      /// <summary>
      /// Calculates the product of src and its transposition.
      /// The function evaluates dst=(src-delta)*(src-delta)^T if order=0, and dst=(src-delta)^T*(src-delta) otherwise.
      /// </summary>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix</param>
      /// <param name="order">Order of multipliers</param>
      /// <param name="delta">An optional array, subtracted from <paramref name="src"/> before multiplication</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvMulTransposed(IntPtr src, IntPtr dst, int order, IntPtr delta);

      /// <summary>
      /// Returns sum of diagonal elements of the matrix <paramref name="src1"/>.
      /// </summary>
      /// <param name="mat">the matrix</param>
      /// <returns>sum of diagonal elements of the matrix src1</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern MCvScalar cvTrace(IntPtr mat);

      /// <summary>
      /// Transposes matrix src1:
      /// dst(i,j)=src(j,i)
      /// Note that no complex conjugation is done in case of complex matrix. Conjugation should be done separately: look at the sample code in cvXorS for example
      /// </summary>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvTranspose(IntPtr src, IntPtr dst);

      /// <summary>
      /// Returns determinant of the square matrix mat. The direct method is used for small matrices and Gaussian elimination is used for larger matrices. For symmetric positive-determined matrices it is also possible to run SVD with U=V=NULL and then calculate determinant as a product of the diagonal elements of W
      /// </summary>
      /// <param name="mat">The pointer to the matrix</param>
      /// <returns>determinant of the square matrix mat</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvDet(IntPtr mat);

      /// <summary>
      /// Inverts matrix src1 and stores the result in src2
      /// </summary>
      /// <param name="src">The source matrix.</param>
      /// <param name="dst">The destination matrix</param>
      /// <param name="method">Inversion method</param>
      /// <returns></returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvInvert(IntPtr src, IntPtr dst, CvEnum.INVERT_METHOD method);

      /// <summary>
      /// Decomposes matrix A into a product of a diagonal matrix and two orthogonal matrices:
      /// A=U*W*VT
      /// Where W is diagonal matrix of singular values that can be coded as a 1D vector of singular values and U and V. All the singular values are non-negative and sorted (together with U and and V columns) in descenting order.
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern double cvMahalanobis(IntPtr vec1, IntPtr vec2, IntPtr mat);

      /// <summary>
      /// Performs PCA analysis of the vector set. First, it uses cvCalcCovarMatrix to compute covariation matrix and then it finds its eigenvalues and eigenvectors. The output number of eigenvalues/eigenvectors should be less than or equal to MIN(rows(data),cols(data)). 
      /// </summary>
      /// <param name="data">The input data; each vector is either a single row (CV_PCA_DATA_AS_ROW) or a single column (CV_PCA_DATA_AS_COL). </param>
      /// <param name="avg">The mean (average) vector, computed inside the function or provided by user</param>
      /// <param name="eigenvalues">The output eigenvalues of covariation matrix. </param>
      /// <param name="eigenvectors">The output eigenvectors of covariation matrix (i.e. principal components); one vector per row.</param>
      /// <param name="flags"></param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvGetRawData(IntPtr arr, out IntPtr data, out int step, out System.Drawing.Size roiSize);

      /// <summary>
      /// Returns matrix header for the input array that can be matrix - CvMat, image - IplImage or multi-dimensional dense array - CvMatND* (latter case is allowed only if allowND != 0) . In the case of matrix the function simply returns the input pointer. In the case of IplImage* or CvMatND* it initializes header structure with parameters of the current image ROI and returns pointer to this temporary structure. Because COI is not supported by CvMat, it is returned separately. 
      /// </summary>
      /// <param name="arr">Input array</param>
      /// <param name="header">Pointer to CvMat structure used as a temporary buffer</param>
      /// <param name="coi">Optional output parameter for storing COI</param>
      /// <param name="allowND">If non-zero, the function accepts multi-dimensional dense arrays (CvMatND*) and returns 2D (if CvMatND has two dimensions) or 1D matrix (when CvMatND has 1 dimension or more than 2 dimensions). The array must be continuous</param>
      /// <returns>Returns matrix header for the input array</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetMat( IntPtr arr, IntPtr header, out int coi, int allowND );

      /// <summary>
      /// Returns image header for the input array that can be matrix - CvMat*, or image - IplImage*.
      /// </summary>
      /// <param name="arr">Input array. </param>
      /// <param name="imageHeader">Pointer to IplImage structure used as a temporary buffer.</param>
      /// <returns>Returns image header for the input array</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetImage( IntPtr arr, IntPtr imageHeader );

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
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvCheckArr(IntPtr arr, CvEnum.CHECK_TYPE flags, double minVal, double maxVal);

      /// <summary>
      /// Return the current number of threads that are used by parallelized (via OpenMP) OpenCV functions.
      /// </summary>
      /// <returns>the current number of threads that are used by parallelized (via OpenMP) OpenCV functions</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern int cvGetNumThreads();

      /// <summary>
      /// Sets the number of threads that are used by parallelized OpenCV functions. 
      /// </summary>
      /// <param name="threadsCount">The number of threads that are used by parallelized OpenCV functions. When the argument is zero or negative, and at the beginning of the program, the number of threads is set to the number of processors in the system, as returned by the function omp_get_num_procs() from OpenMP runtime. </param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvSetNumThreads(int threadsCount);

      /// <summary>
      /// Returns the index, from 0 to cvGetNumThreads()-1, of the thread that called the function. It is a wrapper for the function omp_get_thread_num() from OpenMP runtime. The retrieved index may be used to access local-thread data inside the parallelized code fragments. 
      /// </summary>
      /// <returns>The index, from 0 to cvGetNumThreads()-1, of the thread that called the function. It is a wrapper for the function omp_get_thread_num() from OpenMP runtime. The retrieved index may be used to access local-thread data inside the parallelized code fragments. </returns>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvCmpS(IntPtr src, double value, IntPtr dst, CvEnum.CMP_TYPE cmpOp);

      /// <summary>
      /// Returns the textual description for the specified error status code. In case of unknown status the function returns NULL pointer. 
      /// </summary>
      /// <param name="status">The error status</param>
      /// <returns>the textual description for the specified error status code.</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern String cvErrorStr(int status);

      #region File Storage
      /// <summary>
      /// Opens file storage for reading or writing data. In the latter case a new file is created or existing file is rewritten. Type of the read of written file is determined by the filename extension: .xml for XML, and .yml or .yaml for YAML
      /// </summary>
      /// <param name="filename">Name of the file associated with the storage</param>
      /// <param name="memstorage">Memory storage used for temporary data and for storing dynamic structures, such as CvSeq or CvGraph. If it is NULL, a temporary memory storage is created and used</param>
      /// <param name="flags"></param>
      /// <returns>Pointer to CvFileStorage structure</returns>
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvOpenFileStorage(
         [MarshalAs(_stringMarshalType)] String filename, 
         IntPtr memstorage, 
         CvEnum.STORAGE_OP flags );

      /// <summary>
      /// Closes the file associated with the storage and releases all the temporary structures. It must be called after all I/O operations with the storage are finished
      /// </summary>
      /// <param name="fs">Reference to the pointer of the released file storage</param>
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
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
      [DllImport(CXCORE_LIBRARY)]
      public static extern IntPtr cvGetFileNodeByName(
         IntPtr fs,
         IntPtr map,
         [MarshalAs(_stringMarshalType)] String name);

      #endregion

      #region Miscellaneous Functions
      /// <summary>
      /// implements k-means algorithm that finds centers of cluster_count clusters and groups the input samples around the clusters. On output labels(i) contains a cluster index for sample stored in the i-th row of samples matrix
      /// </summary>
      /// <param name="samples">Floating-point matrix of input samples, one row per sample</param>
      /// <param name="cluster_count">Number of clusters to split the set by</param>
      /// <param name="labels">Output integer vector storing cluster indices for every sample</param>
      /// <param name="termcrit">Specifies maximum number of iterations and/or accuracy (distance the centers move by between the subsequent iterations)</param>
      [DllImport(CXCORE_LIBRARY)]
      public static extern void cvKMeans2( 
         IntPtr samples, 
         int cluster_count,
         IntPtr labels, 
         MCvTermCriteria termcrit );
      #endregion

      #endregion

      #region CV_LIBRARY

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="flags"> flags </param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvWarpAffine(
          IntPtr src,
          IntPtr dst,
          IntPtr mapMatrix,
          int flags,
          MCvScalar fillval);

      /// <summary>
      /// Similar to other geometrical transformations, some interpolation method (specified by user) is used to extract pixels with non-integer coordinates.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapx">The map of x-coordinates (32fC1 image)</param>
      /// <param name="mapy">The map of y-coordinates (32fC1 image)</param>
      /// <param name="flags">A combination of interpolation method and the optional flag CV_WARP_FILL_OUTLIERS </param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvRemap(IntPtr src, IntPtr dst,
            IntPtr mapx, IntPtr mapy,
            int flags,
            MCvScalar fillval);

      /// <summary>
      /// Finds all the motion segments and marks them in seg_mask with individual values each (1,2,...). It also returns a sequence of CvConnectedComp structures, one per each motion components. After than the motion direction for every component can be calculated with cvCalcGlobalOrientation using extracted mask of the particular component (using cvCmp) 
      /// </summary>
      /// <param name="mhi">Motion history image</param>
      /// <param name="segMask">Image where the mask found should be stored, single-channel, 32-bit floating-point</param>
      /// <param name="storage">Memory storage that will contain a sequence of motion connected components</param>
      /// <param name="timestamp">Current time in milliseconds or other units</param>
      /// <param name="segThresh">Segmentation threshold; recommended to be equal to the interval between motion history "steps" or greater</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvSegmentMotion(
          IntPtr mhi,
          IntPtr segMask,
          IntPtr storage,
          double timestamp,
          double segThresh);

      /// <summary>
      /// Calculates the general motion direction in the selected region and returns the angle between 0 and 360. At first the function builds the orientation histogram and finds the basic orientation as a coordinate of the histogram maximum. After that the function calculates the shift relative to the basic orientation as a weighted sum of all orientation vectors: the more recent is the motion, the greater is the weight. The resultant angle is a circular sum of the basic orientation and the shift. 
      /// </summary>
      /// <param name="orientation">Motion gradient orientation image; calculated by the function cvCalcMotionGradient.</param>
      /// <param name="mask">Mask image. It may be a conjunction of valid gradient mask, obtained with cvCalcMotionGradient and mask of the region, whose direction needs to be calculated. </param>
      /// <param name="mhi">Motion history image.</param>
      /// <param name="timestamp">Current time in milliseconds or other units, it is better to store time passed to cvUpdateMotionHistory before and reuse it here, because running cvUpdateMotionHistory and cvCalcMotionGradient on large images may take some time.</param>
      /// <param name="duration">Maximal duration of motion track in milliseconds, the same as in cvUpdateMotionHistory</param>
      /// <returns>The angle</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvCalcGlobalOrientation(
                  IntPtr orientation,
                  IntPtr mask,
                  IntPtr mhi,
                  double timestamp,
                  double duration);

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. First it convolves source image with the specified filter and then downsamples the image by rejecting even rows and columns.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPyrDown(IntPtr src, IntPtr dst, CvEnum.FILTER_TYPE filter);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition. First it upsamples the source image by injecting even zero rows and columns and then convolves result with the specified filter multiplied by 4 for interpolation. So the destination image is four times larger than the source image.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPyrUp(IntPtr src, IntPtr dst, CvEnum.FILTER_TYPE filter);

      /// <summary>
      /// The function cvPyrSegmentation implements image segmentation by pyramids. The pyramid builds up to the level level. The links between any pixel a on level i and its candidate father pixel b on the adjacent level are established if 
      /// p(c(a),c(b))&gt;threshold1. After the connected components are defined, they are joined into several clusters. Any two segments A and B belong to the same cluster, if 
      /// p(c(A),c(B))&gt;threshold2. The input image has only one channel, then 
      /// p(c1,c2)=|c1-c2|. If the input image has three channels (red, green and blue), then 
      /// p(c1,c2)=0.3*(c1r-c2r)+0.59 * (c1g-c2g)+0.11 *(c1b-c2b) . There may be more than one connected component per a cluster.
      /// </summary>
      /// <param name="src">The source image, should be 8-bit single-channel or 3-channel images </param>
      /// <param name="dst">The destination image, should be 8-bit single-channel or 3-channel images, same size as src </param>
      /// <param name="storage">Storage; stores the resulting sequence of connected components</param>
      /// <param name="comp">Pointer to the output sequence of the segmented components</param>
      /// <param name="level">Maximum level of the pyramid for the segmentation</param>
      /// <param name="threshold1">Error threshold for establishing the links</param>
      /// <param name="threshold2">Error threshold for the segments clustering</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPyrSegmentation(
          IntPtr src,
          IntPtr dst,
          IntPtr storage,
          out IntPtr comp,
          int level,
          double threshold1,
          double threshold2);

      /// <summary>
      /// Implements one of the variants of watershed, non-parametric marker-based segmentation algorithm, described in [Meyer92] Before passing the image to the function, user has to outline roughly the desired regions in the image markers with positive (>0) indices, i.e. every region is represented as one or more connected components with the pixel values 1, 2, 3 etc. Those components will be "seeds" of the future image regions. All the other pixels in markers, which relation to the outlined regions is not known and should be defined by the algorithm, should be set to 0's. On the output of the function, each pixel in markers is set to one of values of the "seed" components, or to -1 at boundaries between the regions.
      /// </summary>
      /// <remarks>Note, that it is not necessary that every two neighbor connected components are separated by a watershed boundary (-1's pixels), for example, in case when such tangent components exist in the initial marker image. </remarks>
      /// <param name="image">The input 8-bit 3-channel image</param>
      /// <param name="markers">The input/output Int32 depth single-channel image (map) of markers. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvWatershed(IntPtr image, IntPtr markers);

      #region Computational Geometry
      /// <summary>
      /// Finds minimum area rectangle that contains both input rectangles inside
      /// </summary>
      /// <param name="rect1">First rectangle </param>
      /// <param name="rect2">Second rectangle </param>
      /// <returns>The minimum area rectangle that contains both input rectangles inside</returns>
      [DllImport(CV_LIBRARY)]
      public static extern System.Drawing.Rectangle cvMaxRect(ref System.Drawing.Rectangle rect1, ref System.Drawing.Rectangle rect2);

      /// <summary>
      /// Fits line to 2D or 3D point set 
      /// </summary>
      /// <param name="points">Sequence or array of 2D or 3D points with 32-bit integer or floating-point coordinates</param>
      /// <param name="distType">The distance used for fitting </param>
      /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
      /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line),  0.01 would be a good default</param>
      /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
      /// <param name="line">The output line parameters. In case of 2d fitting it is array of 4 floats (vx, vy, x0, y0) where (vx, vy) is a normalized vector collinear to the line and (x0, y0) is some point on the line. In case of 3D fitting it is array of 6 floats (vx, vy, vz, x0, y0, z0) where (vx, vy, vz) is a normalized vector collinear to the line and (x0, y0, z0) is some point on the line.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFitLine(
          IntPtr points,
          CvEnum.DIST_TYPE distType,
          double param,
          double reps,
          double aeps,
          [Out] float[] line);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="pt">An array of size 8, where the coordinate for ith point is: [pt[i&gt;&gt;1], pt[(i&gt;&gt;1)+1]]</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvBoxPoints(
         MCvBox2D box, 
         [Out]
         float[] pt);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="pt">An array of size 4 points</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvBoxPoints(
         MCvBox2D box, 
         [Out]
         System.Drawing.PointF[] pt);

      /// <summary>
      /// Calculates ellipse that fits best (in least-squares sense) to a set of 2D points. The meaning of the returned structure fields is similar to those in cvEllipse except that size stores the full lengths of the ellipse axises, not half-lengths
      /// </summary>
      /// <param name="points">Sequence or array of points</param>
      /// <returns>The ellipse that fits best (in least-squares sense) to a set of 2D points</returns>
      [DllImport(CV_LIBRARY)]
      public static extern MCvBox2D cvFitEllipse2(IntPtr points);

      /// <summary>
      /// The function cvConvexHull2 finds convex hull of 2D point set using Sklansky's algorithm. 
      /// </summary>
      /// <param name="input">Sequence or array of 2D points with 32-bit integer or floating-point coordinates</param>
      /// <param name="hullStorage">The destination array (CvMat*) or memory storage (CvMemStorage*) that will store the convex hull. If it is array, it should be 1d and have the same number of elements as the input array/sequence. On output the header is modified so to truncate the array downto the hull size</param>
      /// <param name="orientation">Desired orientation of convex hull: CV_CLOCKWISE or CV_COUNTER_CLOCKWISE</param>
      /// <param name="returnPoints">If non-zero, the points themselves will be stored in the hull instead of indices if hull_storage is array, or pointers if hull_storage is memory storage</param>
      /// <returns>If hull_storage is memory storage, the function creates a sequence containing the hull points or pointers to them, depending on return_points value and returns the sequence on output</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvConvexHull2(
          IntPtr input,
          IntPtr hullStorage,
          CvEnum.ORIENTATION orientation,
          int returnPoints);

      #endregion

      #region Plannar Subdivisions
      /// <summary>
      /// Creates an empty Delaunay subdivision, where 2d points can be added further using function cvSubdivDelaunay2DInsert. All the points to be added must be within the specified rectangle, otherwise a runtime error will be raised. 
      /// </summary>
      /// <param name="rect">Rectangle that includes all the 2d points that are to be added to subdivision.</param>
      /// <param name="storage">Container for subdivision</param>
      /// <returns></returns>
      public static IntPtr cvCreateSubdivDelaunay2D(System.Drawing.Rectangle rect, IntPtr storage)
      {
         IntPtr subdiv = cvCreateSubdiv2D((int)CvEnum.SEQ_KIND.CV_SEQ_KIND_SUBDIV2D,
                 Marshal.SizeOf(typeof(MCvSubdiv2D)),
                 Marshal.SizeOf(typeof(MCvSubdiv2DPoint)),
                 Marshal.SizeOf(typeof(MCvQuadEdge2D)),
                 storage);

         cvInitSubdivDelaunay2D(subdiv, rect);
         return subdiv;
      }

      /// <summary>
      /// Initializes Delaunay triangulation 
      /// </summary>
      /// <param name="subdiv"></param>
      /// <param name="rect"></param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvInitSubdivDelaunay2D(IntPtr subdiv, System.Drawing.Rectangle rect);

      /// <summary>
      /// Locates input point within subdivision. It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point. 
      /// </summary>
      /// <param name="subdiv">Delaunay or another subdivision</param>
      /// <param name="pt">Input point</param>
      /// <returns>pointer to the found subdivision vertex (CvSubdiv2DPoint)</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvFindNearestPoint2D(IntPtr subdiv, System.Drawing.PointF pt);

      /// <summary>
      /// Creates new subdivision
      /// </summary>
      /// <param name="subdiv_type"></param>
      /// <param name="header_size"></param>
      /// <param name="vtx_size"></param>
      /// <param name="quadedge_size"></param>
      /// <param name="storage"></param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateSubdiv2D(
          int subdiv_type,
          int header_size,
          int vtx_size,
          int quadedge_size,
          IntPtr storage);

      /// <summary>
      /// Inserts a single point to subdivision and modifies the subdivision topology appropriately. If a points with same coordinates exists already, no new points is added. The function returns pointer to the allocated point. No virtual points coordinates is calculated at this stage.
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision created by function cvCreateSubdivDelaunay2D</param>
      /// <param name="pt">Inserted point.</param>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvSubdivDelaunay2DInsert(IntPtr subdiv, System.Drawing.PointF pt);

      /// <summary>
      /// Locates input point within subdivision
      /// </summary>
      /// <param name="subdiv">Plannar subdivision</param>
      /// <param name="pt">The point to locate</param>
      /// <param name="edge">The output edge the point falls onto or right to</param>
      /// <param name="vertex">Optional output vertex double pointer the input point coincides with</param>
      /// <returns>The type of location for the point</returns>
      [DllImport(CV_LIBRARY)]
      public static extern CvEnum.Subdiv2DPointLocationType cvSubdiv2DLocate(IntPtr subdiv, System.Drawing.PointF pt,
                                           out IntPtr edge,
                                           ref IntPtr vertex);

      /// <summary>
      /// Calculates coordinates of virtual points. All virtual points corresponding to some vertex of original subdivision form (when connected together) a boundary of Voronoi cell of that point
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision, where all the points are added already</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcSubdivVoronoi2D(IntPtr subdiv);

      #endregion

      #region Pose Estimation
      /// <summary>
      /// Allocates memory for the object structure and computes the object inverse matrix. 
      /// </summary>
      /// <remarks>The preprocessed object data is stored in the structure CvPOSITObject, internal for OpenCV, which means that the user cannot directly access the structure data. The user may only create this structure and pass its pointer to the function. 
      /// Object is defined as a set of points given in a coordinate system. The function cvPOSIT computes a vector that begins at a camera-related coordinate system center and ends at the points[0] of the object. 
      /// Once the work with a given object is finished, the function cvReleasePOSITObject must be called to free memory</remarks>
      /// <param name="points3D">A two dimensional array contains the points of the 3D object model, the second dimension must be 3. </param>
      /// <param name="pointCount">Number of object points</param>
      /// <returns>A pointer to the CvPOSITObject</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreatePOSITObject(float[,] points3D, int pointCount);

      /// <summary>
      /// Implements POSIT algorithm. Image coordinates are given in a camera-related coordinate system. The focal length may be retrieved using camera calibration functions. At every iteration of the algorithm new perspective projection of estimated pose is computed. 
      /// </summary>
      /// <remarks>Difference norm between two projections is the maximal distance between corresponding points. </remarks>
      /// <param name="positObject">Pointer to the object structure</param>
      /// <param name="imagePoints">2D array to the object points projections on the 2D image plane, the second dimension must be 2.</param>
      /// <param name="focalLength">Focal length of the camera used</param>
      /// <param name="criteria">Termination criteria of the iterative POSIT algorithm. The parameter criteria.epsilon serves to stop the algorithm if the difference is small.</param>
      /// <param name="rotationMatrix">A vector which contains the 9 elements of the 3x3 rotation matrix</param>
      /// <param name="translationVector">Translation vector (3x1)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPOSIT(IntPtr positObject, float[,] imagePoints, double focalLength,
              MCvTermCriteria criteria, float[] rotationMatrix, float[] translationVector);

      /// <summary>
      /// The function cvReleasePOSITObject releases memory previously allocated by the function cvCreatePOSITObject. 
      /// </summary>
      /// <param name="positObject">pointer to CvPOSIT structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleasePOSITObject(ref IntPtr positObject);
      #endregion

      #region Feature Matching
      /// <summary>
      /// Constructs a balanced kd-tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <returns>A balanced kd-tree index of the given feature vectors</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateFeatureTree(IntPtr desc);

      /// <summary>
      /// Deallocates the given kd-tree
      /// </summary>
      /// <param name="tr">Pointer to tree being destroyed</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseFeatureTree(IntPtr tr);

      /// <summary>
      /// Finds (with high probability) the k nearest neighbors in tr for each of the given (row-)vectors in desc, using best-bin-first searching ([Beis97]). The complexity of the entire operation is at most O(m*emax*log2(n)), where n is the number of vectors in the tree
      /// </summary>
      /// <param name="tr">Pointer to kd-tree index of reference vectors</param>
      /// <param name="desc">m x d matrix of (row-)vectors to find the nearest neighbors of</param>
      /// <param name="results">m x k set of row indices of matching vectors (referring to matrix passed to cvCreateFeatureTree). Contains -1 in some columns if fewer than k neighbors found</param>
      /// <param name="dist">m x k matrix of distances to k nearest neighbors</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">The maximum number of leaves to visit</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindFeatures(
         IntPtr tr, 
         IntPtr desc,
		   IntPtr results, 
         IntPtr dist, 
         int k, 
         int emax);

      /// <summary>
      /// Performs orthogonal range seaching on the given kd-tree. That is, it returns the set of vectors v in tr that satisfy bounds_min[i] &lt;= v[i] &lt;= bounds_max[i], 0 &lt;= i &lt; d, where d is the dimension of vectors in the tree. The function returns the number of such vectors found
      /// </summary>
      /// <param name="tr">Pointer to kd-tree index of reference vectors</param>
      /// <param name="boundsMin">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving minimum value for each dimension</param>
      /// <param name="boundsMax">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving maximum value for each dimension</param>
      /// <param name="results">1 x m or m x 1 vector (CV_32SC1) to contain output row indices (referring to matrix passed to cvCreateFeatureTree)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindFeaturesBoxed(
         IntPtr tr,
         IntPtr boundsMin, 
         IntPtr boundsMax,
         IntPtr results);

      #endregion

      /// <summary>
      /// Erodes the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the minimum is taken:
      /// dst=erode(src,element):  dst(x,y)=min((x',y') in element))src(x+x',y+y')
      ///The function supports the in-place mode. Erosion can be applied several (iterations) times. In case of color image each channel is processed independently.
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is NULL, a 3x3 rectangular structuring element is used.</param>
      /// <param name="iterations">Number of times erosion is applied.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvErode(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Dilates the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the maximum is taken
      /// The function supports the in-place mode. Dilation can be applied several (iterations) times. In case of color image each channel is processed independently
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is NULL, a 3x3 rectangular structuring element is used</param>
      /// <param name="iterations">Number of times erosion is applied</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDilate(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Resizes image src so that it fits exactly to dst. If ROI is set, the function consideres the ROI as supported as usual
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="interpolation">Interpolation method</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvResize(IntPtr src, IntPtr dst, CvEnum.INTER interpolation);

      /// <summary>
      /// Deallocates the cascade that has been created manually or loaded using cvLoadHaarClassifierCascade or cvLoad
      /// </summary>
      /// <param name="cascade">Double pointer to the released cascade. The pointer is cleared by the function. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseHaarClassifierCascade(ref IntPtr cascade);

      /// <summary>
      /// Reconstructs the selected image area from the pixel near the area boundary. The function may be used to remove dust and scratches from a scanned photo, or to remove undesirable objects from still images or video.
      /// </summary>
      /// <param name="src">The input 8-bit 1-channel or 3-channel image</param>
      /// <param name="mask">The inpainting mask, 8-bit 1-channel image. Non-zero pixels indicate the area that needs to be inpainted</param>
      /// <param name="dst">The output image of the same format and the same size as input</param>
      /// <param name="flags">The inpainting method</param>
      /// <param name="inpaintRadius">The radius of circlular neighborhood of each point inpainted that is considered by the algorithm</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvInpaint(IntPtr src, IntPtr mask, IntPtr dst, CvEnum.INPAINT_TYPE flags, double inpaintRadius);

      /// <summary>
      /// Smooths image using one of several methods. Every of the methods has some features and restrictions listed below
      /// Blur with no scaling works with single-channel images only and supports accumulation of 8-bit to 16-bit format (similar to cvSobel and cvLaplace) and 32-bit floating point to 32-bit floating-point format.
      /// Simple blur and Gaussian blur support 1- or 3-channel, 8-bit and 32-bit floating point images. These two methods can process images in-place.
      /// Median and bilateral filters work with 1- or 3-channel 8-bit images and can not process images in-place.
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="type">Type of the smoothing</param>
      /// <param name="param1">The first parameter of smoothing operation</param>
      /// <param name="param2">The second parameter of smoothing operation. In case of simple scaled/non-scaled and Gaussian blur if param2 is zero, it is set to param1</param>
      /// <param name="param3">In case of Gaussian kernel this parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size:
      /// sigma = (n/2 - 1)*0.3 + 0.8, where n=param1 for horizontal kernel,
      /// n=param2 for vertical kernel.
      /// With the standard sigma for small kernels (3x3 to 7x7) the performance is better. If param3 is not zero, while param1 and param2 are zeros, the kernel size is calculated from the sigma (to provide accurate enough operation). 
      /// </param>
      /// <param name="param4">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSmooth(
          IntPtr src,
          IntPtr dst,
          CvEnum.SMOOTH_TYPE type,
          int param1,
          int param2,
          double param3,
          double param4);

      /// <summary>
      /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative. The first case corresponds to
      /// <pre> |-1  0  1|
      ///  |-2  0  2|
      ///  |-1  0  1|</pre>
      ///kernel and the second one corresponds to
      ///  |-1 -2 -1|
      ///  | 0  0  0|
      ///  | 1  2  1|
      ///or
      ///  | 1  2  1|
      ///  | 0  0  0|
      ///  |-1 -2 -1|
      ///kernel, depending on the image origin (origin field of IplImage structure). No scaling is done, so the destination image usually has larger by absolute value numbers than the source image. To avoid overflow, the function requires 16-bit destination image if the source image is 8-bit. The result can be converted back to 8-bit using cvConvertScale or cvConvertScaleAbs functions. Besides 8-bit images the function can process 32-bit floating-point images. Both source and destination must be single-channel images of equal size or ROI size
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="xorder">Order of the derivative x </param>
      /// <param name="yorder">Order of the derivative y</param>
      /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, <paramref name="appertureSize"/> x <paramref name="appertureSize"/> separable kernel will be used to calculate the derivative. For aperture_size=1 3x1 or 1x3 kernel is used (Gaussian smoothing is not done). There is also special value CV_SCHARR (=-1) that corresponds to 3x3 Scharr filter that may give more accurate results than 3x3 Sobel. Scharr aperture is: 
      /// | -3 0  3|
      /// |-10 0 10|
      /// | -3 0  3|
      ///for x-derivative or transposed for y-derivative. 
      ///</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSobel(IntPtr src, IntPtr dst, int xorder, int yorder, int apertureSize);

      /// <summary>
      /// Calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator:
      /// dst(x,y) = d2src/dx2 + d2src/dy2
      /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
      /// |0  1  0|
      /// |1 -4  1|
      /// |0  1  0|
      /// Similar to cvSobel function, no scaling is done and the same combinations of input and output formats are supported. 
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image. </param>
      /// <param name="aperture_size">Aperture size </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvLaplace(IntPtr src, IntPtr dst, int aperture_size);

      /// <summary>
      /// Finds the edges on the input image image and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="threshold1">The first threshold</param>
      /// <param name="threshold2">The second threshold.</param>
      /// <param name="aperture_size">Aperture parameter for Sobel operator </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCanny(
          IntPtr image,
          IntPtr edges,
          double threshold1,
          double threshold2,
          int aperture_size);

      /// <summary>
      /// Tests whether the input contour is convex or not. The contour must be simple, i.e. without self-intersections. 
      /// </summary>
      /// <param name="contour">Tested contour (sequence or array of points). </param>
      /// <returns>true if convex</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvCheckContourConvexity(IntPtr contour);

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex). It returns positive, negative or zero value, correspondingly
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="pt">The point tested against the contour</param>
      /// <param name="measureDist">If != 0, the function estimates distance from the point to the nearest contour edge</param>
      /// <returns>
      /// When measureDist = 0, the return value is &gt;0 (inside), &lt;0 (outside) and =0 (on edge), respectively. 
      /// When measureDist != 0, it is a signed distance between the point and the nearest contour edge
      /// </returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvPointPolygonTest(
          IntPtr contour,
          System.Drawing.PointF pt,
          int measureDist);

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex). It returns positive, negative or zero value, correspondingly
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="pt">The point tested against the contour</param>
      /// <param name="measureDist">If true, the function estimates distance from the point to the nearest contour edge</param>
      /// <returns>
      /// When measureDist == false, the return value is &gt;0 (inside), &lt;0 (outside) and =0 (on edge), respectively. 
      /// When measureDist == true, it is a signed distance between the point and the nearest contour edge
      /// </returns>
      public static double cvPointPolygonTest(
         IntPtr contour,
         System.Drawing.PointF pt,
         bool measureDist)
      {
         return cvPointPolygonTest(contour, pt, measureDist ? 1 : 0);
      }

      /// <summary>
      /// Finds a circumscribed rectangle of the minimal area for 2D point set by building convex hull for the set and applying rotating calipers technique to the hull.
      /// </summary>
      /// <param name="points">Sequence of points, or two channel int/float depth matrix</param>
      /// <param name="storage">temporary memory storage</param>
      /// <returns>a circumscribed rectangle of the minimal area for 2D point set</returns>
      [DllImport(CV_LIBRARY)]
      public static extern MCvBox2D cvMinAreaRect2(IntPtr points, IntPtr storage);

      /// <summary>
      /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm. It returns nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)
      /// </summary>
      /// <param name="points">Sequence or array of 2D points</param>
      /// <param name="center">Output parameter. The center of the enclosing circle</param>
      /// <param name="radius">Output parameter. The radius of the enclosing circle.</param>
      /// <returns>Nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvMinEnclosingCircle(IntPtr points, out System.Drawing.PointF center, out float radius);

      /// <summary>
      /// Calculates 2D pair-wise geometrical histogram (PGH), described in [Iivarinen97], for the contour. The algorithm considers every pair of the contour edges. The angle between the edges and the minimum/maximum distances are determined for every pair. To do this each of the edges in turn is taken as the base, while the function loops through all the other edges. When the base edge and any other edge are considered, the minimum and maximum distances from the points on the non-base edge and line of the base edge are selected. The angle between the edges defines the row of the histogram in which all the bins that correspond to the distance between the calculated minimum and maximum distances are incremented (that is, the histogram is transposed relatively to [Iivarninen97] definition). The histogram can be used for contour matching
      /// </summary>
      /// <param name="contour">Input contour. Currently, only integer point coordinates are allowed</param>
      /// <param name="hist">Calculated histogram; must be two-dimensional</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcPGH(IntPtr contour, IntPtr hist);

      #region Contour Processing Functions
      /// <summary>
      /// Approximates one or more curves and returns the approximation result[s]. In case of multiple curves approximation the resultant tree will have the same structure as the input one (1:1 correspondence). 
      /// </summary>
      /// <param name="src_seq">Sequence of array of points</param>
      /// <param name="header_size">Header size of approximated curve[s].</param>
      /// <param name="storage">Container for approximated contours. If it is NULL, the input sequences' storage is used</param>
      /// <param name="method">Approximation method</param>
      /// <param name="parameter">Desired approximation accuracy</param>
      /// <param name="parameter2">If case if src_seq is sequence it means whether the single sequence should be approximated or all sequences on the same level or below src_seq (see cvFindContours for description of hierarchical contour structures). And if src_seq is array (CvMat*) of points, the parameter specifies whether the curve is closed (parameter2!=0) or not (parameter2=0). </param>
      /// <returns> the approximation result</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvApproxPoly(
          IntPtr src_seq,
          int header_size,
          IntPtr storage,
          CvEnum.APPROX_POLY_TYPE method,
          double parameter,
          int parameter2);

      /// <summary>
      /// Returns the up-right bounding rectangle for 2d point set
      /// </summary>
      /// <param name="points">Either a 2D point set, represented as a sequence (CvSeq*, CvContour*) or vector (CvMat*) of points, or 8-bit single-channel mask image (CvMat*, IplImage*), in which non-zero pixels are considered</param>
      /// <param name="update">The update flag. Here is list of possible combination of the flag values and type of contour: 
      /// points is CvContour*, update=0: the bounding rectangle is not calculated, but it is read from rect field of the contour header. 
      /// points is CvContour*, update=1: the bounding rectangle is calculated and written to rect field of the contour header. For example, this mode is used by cvFindContours. 
      /// points is CvSeq* or CvMat*: update is ignored, the bounding rectangle is calculated and returned. 
      /// </param>
      /// <returns>The up-right bounding rectangle for 2d point set</returns>
      [DllImport(CV_LIBRARY)]
      public static extern System.Drawing.Rectangle cvBoundingRect(
          IntPtr points,
          int update);

      /// <summary>
      /// Returns the up-right bounding rectangle for 2d point set
      /// </summary>
      /// <param name="points">Either a 2D point set, represented as a sequence (CvSeq*, CvContour*) or vector (CvMat*) of points, or 8-bit single-channel mask image (CvMat*, IplImage*), in which non-zero pixels are considered</param>
      /// <param name="update">The update flag. Here is list of possible combination of the flag values and type of contour: 
      /// points is CvContour*, update=false: the bounding rectangle is not calculated, but it is read from rect field of the contour header. 
      /// points is CvContour*, update=true: the bounding rectangle is calculated and written to rect field of the contour header. For example, this mode is used by cvFindContours. 
      /// points is CvSeq* or CvMat*: update is ignored, the bounding rectangle is calculated and returned. 
      /// </param>
      /// <returns>The up-right bounding rectangle for 2d point set</returns>
      public static System.Drawing.Rectangle cvBoundingRect(IntPtr points, bool update)
      {
         return cvBoundingRect(points, update ? 1 : 0);
      }

      /// <summary>
      /// Calculates area of the whole contour or contour section. 
      /// </summary>
      /// <param name="contour">Seq (sequence or array of vertices). </param>
      /// <param name="slice">Starting and ending points of the contour section of interest, by default area of the whole contour is calculated</param>
      /// <returns>The area of the whole contour or contour section</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvContourArea(IntPtr contour, MCvSlice slice);

      /// <summary>
      /// Calculates length or curve as sum of lengths of segments between subsequent points
      /// </summary>
      /// <param name="curve">Sequence or array of the curve points</param>
      /// <param name="slice">Starting and ending points of the curve, by default the whole curve length is calculated</param>
      /// <param name="is_closed">
      /// Indicates whether the curve is closed or not. There are 3 cases:
      /// is_closed=0 - the curve is assumed to be unclosed. 
      /// is_closed&gt;0 - the curve is assumed to be closed. 
      /// is_closed&lt;0 - if curve is sequence, the flag CV_SEQ_FLAG_CLOSED of ((CvSeq*)curve)-&gt;flags is checked to determine if the curve is closed or not, otherwise (curve is represented by array (CvMat*) of points) it is assumed to be unclosed. 
      /// </param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvArcLength(IntPtr curve, MCvSlice slice, int is_closed);
      
      /// <summary>
      /// Find the perimeter of the contour
      /// </summary>
      /// <param name="contour">Pointer to the contour</param>
      /// <returns>the perimeter of the contour</returns>
      public static double cvContourPerimeter(IntPtr contour)
      {
         return cvArcLength(contour, MCvSlice.WholeSeq, 1);
      }

      /// <summary>
      /// Creates binary tree representation for the input contour and returns the pointer to its root.
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="storage">Container for output tree</param>
      /// <param name="threshold">If the parameter threshold is less than or equal to 0, the function creates full binary tree representation. If the threshold is greater than 0, the function creates representation with the precision threshold: if the vertices with the interceptive area of its base line are less than threshold, the tree should not be built any further</param>
      /// <returns>The binary tree representation for the input contour</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateContourTree(
         IntPtr contour,
         IntPtr storage,
         double threshold);

      /// <summary>
      /// Return the contour from its binary tree representation
      /// </summary>
      /// <param name="tree">Contour tree</param>
      /// <param name="storage">Container for the reconstructed contour</param>
      /// <param name="criteria">Criteria, where to stop reconstruction</param>
      /// <returns>The contour represented by this contour tree</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvContourFromContourTree(
         IntPtr tree,
         IntPtr storage,
         MCvTermCriteria criteria);
      
      /// <summary>
      /// Calculates the value of the matching measure for two contour trees. The similarity measure is calculated level by level from the binary tree roots. If at the certain level difference between contours becomes less than threshold, the reconstruction process is interrupted and the current difference is returned
      /// </summary>
      /// <param name="tree1">First contour tree</param>
      /// <param name="tree2">Second contour tree</param>
      /// <param name="method">Similarity measure, only CV_CONTOUR_TREES_MATCH_I1 is supported</param>
      /// <param name="threshold">Similarity threshold</param>
      /// <returns>The value of the matching measure for two contour trees</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvMatchContourTrees( 
         IntPtr tree1, 
         IntPtr tree2,
         CvEnum.MATCH_CONTOUR_TREE_METHOD method, 
         double threshold );
      #endregion

      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix. If you want to apply different kernels to different channels, split the image using cvSplit into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, System.Drawing.Point anchor);

      /// <summary>
      /// Copies the source 2D array into interior of destination array and makes a border of the specified type around the copied area. The function is useful when one needs to emulate border type that is different from the one embedded into a specific algorithm implementation. For example, morphological functions, as well as most of other filtering functions in OpenCV, internally use replication border type, while the user may need zero border or a border, filled with 1's or 255's
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="offset">Coordinates of the top-left corner (or bottom-left in case of images with bottom-left origin) of the destination image rectangle where the source image (or its ROI) is copied. Size of the rectangle matches the source image size/ROI size</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="value">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCopyMakeBorder( 
         IntPtr src, 
         IntPtr dst, 
         System.Drawing.Point offset,
         CvEnum.BORDER_TYPE bordertype, 
         MCvScalar value);

      /// <summary>
      /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image (cvCmpS could be also used for this purpose) or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by threshold_type
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="threshold">Threshold value</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="thresholdType">Thresholding type </param>
      [DllImport(CV_LIBRARY)]
      public static extern double cvThreshold(
         IntPtr src, 
         IntPtr dst, 
         double threshold,
         double maxValue, 
         CvEnum.THRESH thresholdType);

      /// <summary>
      /// Transforms grayscale image to binary image. 
      /// Threshold calculated individually for each pixel. 
      /// For the method CV_ADAPTIVE_THRESH_MEAN_C it is a mean of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel
      /// neighborhood, subtracted by param1. 
      /// For the method CV_ADAPTIVE_THRESH_GAUSSIAN_C it is a weighted sum (gaussian) of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel neighborhood, subtracted by param1.
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="adaptiveType">Adaptive_method </param>
      /// <param name="thresholdType">Thresholding type. must be one of CV_THRESH_BINARY, CV_THRESH_BINARY_INV  </param>
      /// <param name="blockSize">The size of a pixel neighborhood that is used to calculate a threshold value for the pixel: 3, 5, 7, ... </param>
      /// <param name="param1">Constant subtracted from mean or weighted mean. It may be negative. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvAdaptiveThreshold(
         IntPtr src,
         IntPtr dst,
         double maxValue,
         CvEnum.ADAPTIVE_THRESHOLD_TYPE adaptiveType,
         CvEnum.THRESH thresholdType,
         int blockSize,
         double param1);

      /// <summary>
      /// Implements a particular case of application of line iterators. The function reads all the image points lying on the line between pt1 and pt2, including the ending points, and stores them into the buffer
      /// </summary>
      /// <param name="image">Image to sample the line from</param>
      /// <param name="pt1">Starting the line point.</param>
      /// <param name="pt2">Ending the line point</param>
      /// <param name="buffer">Buffer to store the line points; must have enough size to store max( |pt2.x-pt1.x|+1, |pt2.y-pt1.y|+1 ) points in case of 8-connected line and |pt2.x-pt1.x|+|pt2.y-pt1.y|+1 in case of 4-connected line</param>
      /// <param name="connectivity">The line connectivity, 4 or 8</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvSampleLine(IntPtr image, System.Drawing.Point pt1, System.Drawing.Point pt2, IntPtr buffer, CvEnum.CONNECTIVITY connectivity);

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; (for example, ~1/4 to 1/16 of the image area in case of video conferencing). 
      /// </summary>
      /// <param name="image">Image to detect objects in.</param>
      /// <param name="cascade">Haar classifier cascade in internal representation</param>
      /// <param name="storage">Memory storage to store the resultant sequence of the object candidate rectangles</param>
      /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
      /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
      /// <param name="flags">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing</param>
      /// <param name="minSize">Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection). </param>
      /// <returns>Rectangular regions in the given image that are likely to contain objects the cascade has been trained for</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvHaarDetectObjects(
         IntPtr image, 
         IntPtr cascade,
         IntPtr storage, 
         double scaleFactor,
         int minNeighbors,
         CvEnum.HAAR_DETECTION_TYPE flags,
         System.Drawing.Size minSize);

      /// <summary>
      /// Retrieves contours from the binary image and returns the number of retrieved contours. The pointer first_contour is filled by the function. It will contain pointer to the first most outer contour or NULL if no contours is detected (if the image is completely black). Other contours may be reached from first_contour using h_next and v_next links. The sample in cvDrawContours discussion shows how to use contours for connected component detection. Contours can be also used for shape analysis and object recognition - see squares.c in OpenCV sample directory
      /// </summary>
      /// <param name="image">The source 8-bit single channel image. Non-zero pixels are treated as 1s, zero pixels remain 0s - that is image treated as binary. To get such a binary image from grayscale, one may use cvThreshold, cvAdaptiveThreshold or cvCanny. The function modifies the source image content</param>
      /// <param name="storage">Container of the retrieved contours</param>
      /// <param name="firstContour">Output parameter, will contain the pointer to the first outer contour</param>
      /// <param name="headerSize">Size of the sequence header, &gt;=sizeof(CvChain) if method=CV_CHAIN_CODE, and &gt;=sizeof(CvContour) otherwise</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindContours(
         IntPtr image,
         IntPtr storage,
         ref IntPtr firstContour,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         System.Drawing.Point offset);

      /// <summary>
      /// Finds circles in grayscale image using some modification of Hough transform
      /// </summary>
      /// <param name="image">The input 8-bit single-channel grayscale image</param>
      /// <param name="circleStorage">The storage for the circles detected. It can be a memory storage (in this case a sequence of circles is created in the storage and returned by the function) or single row/single column matrix (CvMat*) of type CV_32FC3, to which the circles' parameters are written. The matrix header is modified by the function so its cols or rows will contain a number of lines detected. If circle_storage is a matrix and the actual number of lines exceeds the matrix size, the maximum possible number of circles is returned. Every circle is encoded as 3 floating-point numbers: center coordinates (x,y) and the radius</param>
      /// <param name="method">Currently, the only implemented method is CV_HOUGH_GRADIENT</param>
      /// <param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
      /// <param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
      /// <param name="param1">The first method-specific parameter. In case of CV_HOUGH_GRADIENT it is the higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller). </param>
      /// <param name="param2">The second method-specific parameter. In case of CV_HOUGH_GRADIENT it is accumulator threshold at the center detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first</param>
      /// <param name="minRadius">Minimal radius of the circles to search for</param>
      /// <param name="maxRadius">Maximal radius of the circles to search for. By default the maximal radius is set to max(image_width, image_height). </param>
      /// <returns>Pointer to the sequence of circles</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvHoughCircles(
         IntPtr image,
         IntPtr circleStorage,
         CvEnum.HOUGH_TYPE method,
         double dp,
         double minDist,
         double param1,
         double param2,
         int minRadius,
         int maxRadius);

      /// <summary>
      /// Converts input image from one color space to another. The function ignores colorModel and channelSeq fields of IplImage header, so the source image color space should be specified correctly (including order of the channels in case of RGB space, e.g. BGR means 24-bit format with B0 G0 R0 B1 G1 R1 ... layout, whereas RGB means 24-bit format with R0 G0 B0 R1 G1 B1 ... layout). 
      /// </summary>
      /// <param name="src">The source 8-bit (8u), 16-bit (16u) or single-precision floating-point (32f) image</param>
      /// <param name="dst">The destination image of the same data type as the source one. The number of channels may be different</param>
      /// <param name="code">Color conversion operation that can be specifed using CV_src_color_space2dst_color_space constants </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCvtColor(IntPtr src, IntPtr dst, CvEnum.COLOR_CONVERSION code);

      /// <summary>
      /// The function cvHoughLines2 implements a few variants of Hough transform for line detection
      /// </summary>
      /// <param name="image">The input 8-bit single-channel binary image. In case of probabilistic method the image is modified by the function</param>
      /// <param name="lineStorage">The storage for the lines detected. It can be a memory storage (in this case a sequence of lines is created in the storage and returned by the function) or single row/single column matrix (CvMat*) of a particular type (see below) to which the lines' parameters are written. The matrix header is modified by the function so its cols or rows will contain a number of lines detected. If line_storage is a matrix and the actual number of lines exceeds the matrix size, the maximum possible number of lines is returned (in case of standard hough transform the lines are sorted by the accumulator value). </param>
      /// <param name="method">The Hough transform variant</param>
      /// <param name="rho">Distance resolution in pixel-related units</param>
      /// <param name="theta">Angle resolution measured in radians</param>
      /// <param name="threshold">Threshold parameter. A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
      /// <param name="param1">The first method-dependent parameter:
      /// For classical Hough transform it is not used (0). 
      /// For probabilistic Hough transform it is the minimum line length. 
      /// For multi-scale Hough transform it is divisor for distance resolution rho. (The coarse distance resolution will be rho and the accurate resolution will be (rho / param1))
      /// </param>
      /// <param name="param2">The second method-dependent parameter:
      /// For classical Hough transform it is not used (0). 
      /// For probabilistic Hough transform it is the maximum gap between line segments lieing on the same line to treat them as the single line segment (i.e. to join them). 
      /// For multi-scale Hough transform it is divisor for angle resolution theta. (The coarse angle resolution will be theta and the accurate resolution will be (theta / param2)). 
      /// </param>
      /// <returns>Pointer to the decetected lines</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvHoughLines2(
         IntPtr image, 
         IntPtr lineStorage, 
         CvEnum.HOUGH_TYPE method,
         double rho, 
         double theta, 
         int threshold,
         double param1, 
         double param2);

      /// <summary>
      /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characeteristics including 7 Hu invariants.
      /// </summary>
      /// <param name="arr">Image (1-channel or 3-channel with COI set) or polygon (CvSeq of points or a vector of points)</param>
      /// <param name="moments">Pointer to returned moment state structure</param>
      /// <param name="binary">(For images only) If the flag is non-zero, all the zero pixel values are treated as zeroes, all the others are treated as 1s</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvMoments(IntPtr arr, ref MCvMoments moments, int binary);

      /// <summary>
      /// Finds corners with big eigenvalues in the image. 
      /// </summary>
      /// <remarks>
      /// The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. 
      /// Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). 
      /// The next step is rejecting the corners with the minimal eigenvalue less than quality_level*max(eigImage(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features.
      /// </remarks>
      /// <param name="image">The source 8-bit or floating-point 32-bit, single-channel image</param>
      /// <param name="eigImage">Temporary floating-point 32-bit image of the same size as image</param>
      /// <param name="tempImage">Another temporary image of the same size and same format as eig_image</param>
      /// <param name="corners">Output parameter. Detected corners</param>
      /// <param name="cornerCount">Output parameter. Number of detected corners</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used</param>
      /// <param name="mask">Region of interest. The function selects points either in the specified region or in the whole image if the mask is NULL</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
      /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal.</param>
      /// <param name="k">Free parameter of Harris detector; used only if <paramref name="useHarris"/> != 0</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvGoodFeaturesToTrack(
          IntPtr image,
          IntPtr eigImage,
          IntPtr tempImage,
          IntPtr corners,
          ref int cornerCount,
          double qualityLevel,
          double minDistance,
          IntPtr mask,
          int blockSize,
          int useHarris,
          double k);

      /// <summary>
      /// Finds corners with big eigenvalues in the image. 
      /// </summary>
      /// <remarks>
      /// The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. 
      /// Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). 
      /// The next step is rejecting the corners with the minimal eigenvalue less than quality_level*max(eigImage(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features.
      /// </remarks>
      /// <param name="image">The source 8-bit or floating-point 32-bit, single-channel image</param>
      /// <param name="eigImage">Temporary floating-point 32-bit image of the same size as image</param>
      /// <param name="tempImage">Another temporary image of the same size and same format as eig_image</param>
      /// <param name="corners">Output parameter. Detected corners</param>
      /// <param name="cornerCount">Output parameter. Number of detected corners</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used</param>
      /// <param name="mask">Region of interest. The function selects points either in the specified region or in the whole image if the mask is NULL</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
      /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal.</param>
      /// <param name="k">Free parameter of Harris detector; used only if <paramref name="useHarris"/> != 0</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvGoodFeaturesToTrack(
         IntPtr image,
         IntPtr eigImage,
         IntPtr tempImage,
         [Out]
         System.Drawing.PointF[] corners,
         ref int cornerCount,
         double qualityLevel,
         double minDistance,
         IntPtr mask,
         int blockSize,
         int useHarris,
         double k);

      /// <summary>
      /// Finds robust features in the image. For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
      /// </summary>
      /// <param name="image">The input 8-bit grayscale image</param>
      /// <param name="mask">The optional input 8-bit mask. The features are only found in the areas that contain more than 50% of non-zero mask pixels</param>
      /// <param name="keypoints">The output parameter; double pointer to the sequence of keypoints. This will be the sequence of MCvSURFPoint structures</param>
      /// <param name="descriptors">The optional output parameter; double pointer to the sequence of descriptors; Depending on the params.extended value, each element of the sequence will be either 64-element or 128-element floating-point (CV_32F) vector. If the parameter is NULL, the descriptors are not computed</param>
      /// <param name="storage">Memory storage where keypoints and descriptors will be stored</param>
      /// <param name="parameters">Various algorithm parameters put to the structure CvSURFParams</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvExtractSURF(
         IntPtr image, IntPtr mask,
         out IntPtr keypoints,
         out IntPtr descriptors,
         IntPtr storage,
         MCvSURFParams parameters);

      /// <summary>
      /// Create a CvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThreshold">      
      /// only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extended">      
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </param>
      /// <returns>The MCvSURFParams structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern MCvSURFParams cvSURFParams(double hessianThreshold, int extended);

      #region Camera Calibration
      /// <summary>
      /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters.
      /// Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points). 
      /// </summary>
      /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
      /// <param name="rotationVector">The rotation vector, 1x3 or 3x1</param>
      /// <param name="translationVector">The translation vector, 1x3 or 3x1</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is NULL, all distortion coefficients are considered 0's</param>
      /// <param name="imagePoints">The output array of image points, 2xN or Nx2, where N is the total number of points in the view</param>
      /// <param name="dpdrot">Optional Nx3 matrix of derivatives of image points with respect to components of the rotation vector</param>
      /// <param name="dpdt">Optional Nx3 matrix of derivatives of image points w.r.t. components of the translation vector</param>
      /// <param name="dpdf">Optional Nx2 matrix of derivatives of image points w.r.t. fx and fy</param>
      /// <param name="dpdc">Optional Nx2 matrix of derivatives of image points w.r.t. cx and cy</param>
      /// <param name="dpddist">Optional Nx4 matrix of derivatives of image points w.r.t. distortion coefficients</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvProjectPoints2(
          IntPtr objectPoints,
          IntPtr rotationVector,
          IntPtr translationVector,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr imagePoints,
          IntPtr dpdrot,
          IntPtr dpdt,
          IntPtr dpdf,
          IntPtr dpdc,
          IntPtr dpddist);

      /// <summary>
      /// Finds perspective transformation H=||hij|| between the source and the destination planes
      /// </summary>
      /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates), where N is the number of points. </param>
      /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates) </param>
      /// <param name="homography">Output 3x3 homography matrix. Homography matrix is determined up to a scale, thus it is normalized to make h33=1</param>
      /// <param name="method">The type of the method</param>
      /// <param name="ransacReprojThreshold">The maximum allowed reprojection error to treat a point pair as an inlier. The parameter is only used in RANSAC-based homography estimation. E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3</param>
      /// <param name="mask">The optional output mask set by a robust method (RANSAC or LMEDS). </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindHomography(
         IntPtr srcPoints,
         IntPtr dstPoints,
         IntPtr homography,
         CvEnum.HOMOGRAPHY_METHOD method,
         double ransacReprojThreshold,
         IntPtr mask);

      /// <summary>
      /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints">The joint matrix of corresponding image points, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each particular view, 1xM or Mx1, where M is the number of a scene views</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="intrinsicMatrix">The output camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
      /// <param name="distortionCoeffs">The output 4x1 or 1x4 vector of distortion coefficients [k1, k2, p1, p2]</param>
      /// <param name="rotationVectors">The output 3xM or Mx3 array of rotation vectors (compact representation of rotation matrices, see cvRodrigues2). </param>
      /// <param name="translationVectors">The output 3xM or Mx3 array of translation vectors</param>
      /// <param name="flags">Different flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalibrateCamera2(
          IntPtr objectPoints,
          IntPtr imagePoints,
          IntPtr pointCounts,
          System.Drawing.Size imageSize,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr rotationVectors,
          IntPtr translationVectors,
          CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// computes various useful camera (sensor/lens) characteristics using the computed camera calibration matrix, image frame resolution in pixels and the physical aperture size
      /// </summary>
      /// <param name="calibMatr">The matrix of intrinsic parameters</param>
      /// <param name="imgWidth">Image width in pixels</param>
      /// <param name="imgHeight">Image height in pixels</param>
      /// <param name="apertureWidth">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="apertureHeight">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="fovx">Field of view angle in x direction in degrees</param>
      /// <param name="fovy">Field of view angle in y direction in degrees </param>
      /// <param name="focalLength">Focal length in realworld units </param>
      /// <param name="principalPoint">The principal point in realworld units </param>
      /// <param name="pixelAspectRatio">The pixel aspect ratio ~ fy/f</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalibrationMatrixValues(
         IntPtr calibMatr,
         int imgWidth,
         int imgHeight,
         double apertureWidth,
         double apertureHeight,
         ref double fovx,
         ref double fovy,
         ref double focalLength,
         ref MCvPoint2D64f principalPoint,
         ref double pixelAspectRatio);


      /// <summary>
      /// Estimates extrinsic camera parameters using known intrinsic parameters and and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error
      /// </summary>
      /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
      /// <param name="imagePoints">The array of corresponding image points, 2xN or Nx2, where N is the number of points in the view</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is NULL, all distortion coefficients are considered 0's.</param>
      /// <param name="rotationVector">The output 3x1 or 1x3 rotation vector (compact representation of a rotation matrix, see cvRodrigues2). </param>
      /// <param name="translationVector">The output 3x1 or 1x3 translation vector</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindExtrinsicCameraParams2(
         IntPtr objectPoints,
         IntPtr imagePoints,
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr rotationVector,
         IntPtr translationVector);

      /// <summary>
      /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
      /// R2=R*R1,
      /// T2=R*T1 + T
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints1">The joint matrix of corresponding image points in the views from the 1st camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="imagePoints2">The joint matrix of corresponding image points in the views from the 2nd camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each view, 1xM or Mx1, where M is the number of views</param>
      /// <param name="cameraMatrix1">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs1">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="cameraMatrix2">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs2">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems </param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="E">The optional output essential matrix</param>
      /// <param name="F">The optional output fundamental matrix </param>
      /// <param name="termCrit">Termination criteria for the iterative optimiziation algorithm</param>
      /// <param name="flags"></param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoCalibrate(
         IntPtr objectPoints,
         IntPtr imagePoints1,
         IntPtr imagePoints2,
         IntPtr pointCounts,
         IntPtr cameraMatrix1,
         IntPtr distCoeffs1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs2,
         System.Drawing.Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr E,
         IntPtr F,
         MCvTermCriteria termCrit,
         CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// computes the rectification transformations without knowing intrinsic parameters of the cameras and their relative position in space, hence the suffix "Uncalibrated". Another related difference from cvStereoRectify is that the function outputs not the rectification transformations in the object (3D) space, but the planar perspective transformations, encoded by the homography matrices H1 and H2. The function implements the following algorithm [Hartley99]. 
      /// </summary>
      /// <remarks>
      /// Note that while the algorithm does not need to know the intrinsic parameters of the cameras, it heavily depends on the epipolar geometry. Therefore, if the camera lenses have significant distortion, it would better be corrected before computing the fundamental matrix and calling this function. For example, distortion coefficients can be estimated for each head of stereo camera separately by using cvCalibrateCamera2 and then the images can be corrected using cvUndistort2
      /// </remarks>
      /// <param name="points1">The array of 2D points</param>
      /// <param name="points2">The array of 2D points</param>
      /// <param name="F">Fundamental matrix. It can be computed using the same set of point pairs points1 and points2 using cvFindFundamentalMat</param>
      /// <param name="imageSize">Size of the image</param>
      /// <param name="H1">The rectification homography matrices for the first images</param>
      /// <param name="H2">The rectification homography matrices for the second images</param>
      /// <param name="threshold">If the parameter is greater than zero, then all the point pairs that do not comply the epipolar geometry well enough (that is, the points for which fabs(points2[i]T*F*points1[i])>threshold) are rejected prior to computing the homographies</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoRectifyUncalibrated(
         IntPtr points1,
         IntPtr points2,
         IntPtr F,
         System.Drawing.Size imageSize,
         IntPtr H1,
         IntPtr H2,
         double threshold);

      /// <summary>
      /// computes the rotation matrices for each camera that (virtually) make both camera image planes the same plane. Consequently, that makes all the epipolar lines parallel and thus simplifies the dense stereo correspondence problem. On input the function takes the matrices computed by cvStereoCalibrate and on output it gives 2 rotation matrices and also 2 projection matrices in the new coordinates. The function is normally called after cvStereoCalibrate that computes both camera matrices, the distortion coefficients, R and T
      /// </summary>
      /// <param name="cameraMatrix1">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="cameraMatrix2">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="distCoeffs1">The vectors of distortion coefficients for first camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="distCoeffs2">The vectors of distortion coefficients for second camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image used for stereo calibration</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems</param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="R1">3x3 Rectification transforms (rotation matrices) for the first camera</param>
      /// <param name="R2">3x3 Rectification transforms (rotation matrices) for the second camera</param>
      /// <param name="P1">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="P2">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="Q">The optional output disparity-to-depth mapping matrix, 4x4, see cvReprojectImageTo3D. </param>
      /// <param name="flags">The operation flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoRectify(
         IntPtr cameraMatrix1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs1,
         IntPtr distCoeffs2,
         System.Drawing.Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr R1,
         IntPtr R2,
         IntPtr P1,
         IntPtr P2,
         IntPtr Q,
         CvEnum.STEREO_RECTIFY_TYPE flags);

      /// <summary>
      /// Transforms the image to compensate radial and tangential lens distortion. The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same.
      /// </summary>
      /// <param name="src">The input (distorted) image</param>
      /// <param name="dst">The output (corrected) image</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1].</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2].</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvUndistort2(
          IntPtr src,
          IntPtr dst,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs);

      /// <summary>
      /// Pre-computes the undistortion map - coordinates of the corresponding pixel in the distorted image for every pixel in the corrected image. Then, the map (together with input and output images) can be passed to cvRemap function. 
      /// </summary>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. </param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvInitUndistortMap(
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr mapx, 
         IntPtr mapy);

      /// <summary>
      /// This function is an extended version of cvInitUndistortMap. That is, in addition to the correction of lens distortion, the function can also apply arbitrary perspective transformation R and finally it can scale and shift the image according to the new camera matrix
      /// </summary>
      /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is NULL, the identity matrix is used</param>
      /// <param name="newCameraMatrix">The new camera matrix A'=[fx' 0 cx'; 0 fy' cy'; 0 0 1]</param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvInitUndistortRectifyMap( 
         IntPtr cameraMatrix,
         IntPtr distCoeffs,
         IntPtr R,
         IntPtr newCameraMatrix,
         IntPtr mapx, 
         IntPtr mapy );

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The output array of corners detected</param>
      /// <param name="cornerCount">The output corner counter. If it is not NULL, the function stores there the number of corners found</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>Non-zero value if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindChessboardCorners(
         IntPtr image,
         System.Drawing.Size patternSize,
         float[,] corners,
         ref int cornerCount,
         CvEnum.CALIB_CB_TYPE flags);

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The output array of corners detected</param>
      /// <param name="cornerCount">The output corner counter. If it is not NULL, the function stores there the number of corners found</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>Non-zero value if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindChessboardCorners(
         IntPtr image,
         System.Drawing.Size patternSize,
         IntPtr corners,
         ref int cornerCount,
         CvEnum.CALIB_CB_TYPE flags);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         System.Drawing.Size patternSize,
         float[,] corners,
         int count,
         int patternWasFound);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         System.Drawing.Size patternSize,
         [In]
         System.Drawing.PointF[] corners,
         int count,
         int patternWasFound);

      #endregion

      #region Epipolar Geometry, Stereo Correspondence
      /// <summary>
      /// Calculates fundamental matrix using one of four methods listed above and returns the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. 
      /// </summary>
      /// <param name="points1">Array of the first image points of 2xN, Nx2, 3xN or Nx3 size (where N is number of points). Multi-channel 1xN or Nx1 array is also acceptable. The point coordinates should be floating-point (single or double precision) </param>
      /// <param name="points2">Array of the second image points of the same size and format as points1</param>
      /// <param name="fundamentalMatrix">The output fundamental matrix or matrices. The size should be 3x3 or 9x3 (7-point method may return up to 3 matrices).</param>
      /// <param name="method">Method for computing the fundamental matrix </param>
      /// <param name="param1">Use 3.0 for default. The parameter is used for RANSAC method only. It is the maximum distance from point to epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. Usually it is set somewhere from 1 to 3. </param>
      /// <param name="param2">Use 0.99 for default. The parameter is used for RANSAC or LMedS methods only. It denotes the desirable level of confidence of the fundamental matrix estimate. </param>
      /// <param name="status">The optional pointer to output array of N elements, every element of which is set to 0 for outliers and to 1 for the "inliers", i.e. points that comply well with the estimated epipolar geometry. The array is computed only in RANSAC and LMedS methods. For other methods it is set to all 1’s.</param>
      /// <returns>the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. </returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindFundamentalMat(IntPtr points1,
         IntPtr points2,
         IntPtr fundamentalMatrix,
         CvEnum.CV_FM method,
         double param1,
         double param2,
         IntPtr status);

      /// <summary>
      /// For every point in one of the two images of stereo-pair the function cvComputeCorrespondEpilines finds equation of a line that contains the corresponding point (i.e. projection of the same 3D point) in the other image. Each line is encoded by a vector of 3 elements l=[a,b,c]^T, so that: 
      /// l^T*[x, y, 1]^T=0, or
      /// a*x + b*y + c = 0
      /// From the fundamental matrix definition (see cvFindFundamentalMatrix discussion), line l2 for a point p1 in the first image (which_image=1) can be computed as: 
      /// l2=F*p1 and the line l1 for a point p2 in the second image (which_image=1) can be computed as: 
      /// l1=F^T*p2Line coefficients are defined up to a scale. They are normalized (a2+b2=1) are stored into correspondent_lines
      /// </summary>
      /// <param name="points">The input points. 2xN, Nx2, 3xN or Nx3 array (where N number of points). Multi-channel 1xN or Nx1 array is also acceptable.</param>
      /// <param name="whichImage">Index of the image (1 or 2) that contains the points</param>
      /// <param name="fundamentalMatrix">Fundamental matrix </param>
      /// <param name="correspondentLines">Computed epilines, 3xN or Nx3 array </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvComputeCorrespondEpilines(
         IntPtr points,
         int whichImage,
         IntPtr fundamentalMatrix,
         IntPtr correspondentLines);

      /// <summary>
      /// Converts 2D or 3D points from/to homogenious coordinates, or simply copies or transposes the array. In case if the input array dimensionality is larger than the output, each point coordinates are divided by the last coordinate
      /// </summary>
      /// <param name="src">The input point array, 2xN, Nx2, 3xN, Nx3, 4xN or Nx4 (where N is the number of points). Multi-channel 1xN or Nx1 array is also acceptable</param>
      /// <param name="dst">The output point array, must contain the same number of points as the input; The dimensionality must be the same, 1 less or 1 more than the input, and also within 2..4.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvConvertPointsHomogeneous(IntPtr src, IntPtr dst);

      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. It is possible to override any of the parameters at any time between the calls to cvFindStereoCorrespondenceBM
      /// </summary>
      /// <param name="type">ID of one of the pre-defined parameter sets. Any of the parameters can be overridden after creating the structure.</param>
      /// <param name="numberOfDisparities">The number of disparities. If the parameter is 0, it is taken from the preset, otherwise the supplied value overrides the one from preset. </param>
      /// <returns>Pointer to the stereo correspondece structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateStereoBMState( 
         CvEnum.STEREO_BM_TYPE type,
         int numberOfDisparities);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">The state to be released</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseStereoBMState(ref IntPtr state);

      /// <summary>
      /// computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         IntPtr state);

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         ref MCvStereoBMState state);

      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. 
      /// </summary>
      /// <param name="numberOfDisparities">The number of disparities. The disparity search range will be state.minDisparity &lt;= disparity &lt; state.minDisparity + state.numberOfDisparities</param>
      /// <param name="maxIters">Maximum number of iterations. On each iteration all possible (or reasonable) alpha-expansions are tried. The algorithm may terminate earlier if it could not find an alpha-expansion that decreases the overall cost function value</param>
      /// <returns>The initialized stereo correspondence structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateStereoGCState(
         int numberOfDisparities,
         int maxIters);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">A reference to the pointer of StereoGCState structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseStereoGCState(ref IntPtr state);

      /// <summary>
      /// Computes disparity maps for the input rectified stereo pair
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="dispLeft">The optional output single-channel 16-bit signed left disparity map of the same size as input images.</param>
      /// <param name="dispRight">The optional output single-channel 16-bit signed right disparity map of the same size as input images</param>
      /// <param name="state">Stereo correspondence structure</param>
      /// <param name="useDisparityGuess">If the parameter is not zero, the algorithm will start with pre-defined disparity maps. Both dispLeft and dispRight should be valid disparity maps. Otherwise, the function starts with blank disparity maps (all pixels are marked as occlusions)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceGC( 
         IntPtr left, 
         IntPtr right,
         IntPtr dispLeft, 
         IntPtr dispRight,
         IntPtr state,
         int useDisparityGuess);

      /// <summary>
      /// Computes disparity maps for the input rectified stereo pair
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="dispLeft">The optional output single-channel 16-bit signed left disparity map of the same size as input images.</param>
      /// <param name="dispRight">The optional output single-channel 16-bit signed right disparity map of the same size as input images</param>
      /// <param name="state">Stereo correspondence structure</param>
      /// <param name="useDisparityGuess">If the parameter is not zero, the algorithm will start with pre-defined disparity maps. Both dispLeft and dispRight should be valid disparity maps. Otherwise, the function starts with blank disparity maps (all pixels are marked as occlusions)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceGC(
         IntPtr left,
         IntPtr right,
         IntPtr dispLeft,
         IntPtr dispRight,
         ref MCvStereoGCState state,
         int useDisparityGuess);

      /// <summary>
      /// Transforms 1-channel disparity map to 3-channel image, a 3D surface.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="image3D">3-channel, 16-bit integer or 32-bit floating-point image - the output map of 3D points</param>
      /// <param name="Q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReprojectImageTo3D( 
         IntPtr disparity,
         IntPtr image3D, 
         IntPtr Q );

      #endregion 

      /// <summary>
      /// Iterates to find the object center given its back projection and initial position of search window. The iterations are made until the search window center moves by less than the given value and/or until the function has done the maximum number of iterations. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram</param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished. </param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field). </param>
      /// <returns>the number of iterations made</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvMeanShift(
         IntPtr probImage,
         System.Drawing.Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp);

      /// <summary>
      /// Implements CAMSHIFT object tracking algrorithm ([Bradski98]). First, it finds an object center using cvMeanShift and, after that, calculates the object size and orientation. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram </param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished</param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field).</param>
      /// <param name="box">Circumscribed box for the object. If not NULL, contains object size and orientation</param>
      /// <returns>number of iterations made within cvMeanShift</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvCamShift(
         IntPtr probImage,
         System.Drawing.Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp,
         out MCvBox2D box);

      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvMatchTemplate(
          IntPtr image,
          IntPtr templ,
          IntPtr result,
          CvEnum.TM_TYPE method);

      /// <summary>
      /// Compares two shapes. The 3 implemented methods all use Hu moments
      /// </summary>
      /// <param name="object1">First contour or grayscale image</param>
      /// <param name="object2">Second contour or grayscale image</param>
      /// <param name="method">Comparison method</param>
      /// <param name="parameter">Method-specific parameter (is not used now)</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvMatchShapes(
         IntPtr object1,
         IntPtr object2,
         CvEnum.CONTOURS_MATCH_TYPE method,
         double parameter);


      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSnakeImage(
          IntPtr image,
          IntPtr points,
          int length,
          [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
          [MarshalAs(UnmanagedType.LPArray)] float[] beta,
          [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
          int coeffUsage,
          System.Drawing.Size win,
          MCvTermCriteria criteria,
          int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSnakeImage(
         IntPtr image,
         [In,Out]
         System.Drawing.Point[] points,
         int length,
         [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
         [MarshalAs(UnmanagedType.LPArray)] float[] beta,
         [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
         int coeffUsage,
         System.Drawing.Size win,
         MCvTermCriteria criteria,
         int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If true, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      public static void cvSnakeImage(
           IntPtr image,
           IntPtr points,
           int length,
           float[] alpha,
           float[] beta,
           float[] gamma,
           int coeffUsage,
           System.Drawing.Size win,
           MCvTermCriteria criteria,
           bool calcGradient)
      {
         cvSnakeImage(
            image,
            points,
            length,
            alpha,
            beta,
            gamma,
            coeffUsage,
            win,
            criteria,
            calcGradient ? 1 : 0);
      }

      /// <summary>
      /// The function cvCreateStructuringElementEx creates an structuring element.
      /// </summary>
      /// <param name="cols">Number of columns in the structuring element.</param>
      /// <param name="rows">Number of rows in the structuring element.</param>
      /// <param name="anchorX">Relative horizontal offset of the anchor point.</param>
      /// <param name="anchorY">Relative vertical offset of the anchor point.</param>
      /// <param name="shape">Shape of the structuring element.</param>
      /// <param name="values">
      /// Pointer to the structuring element data, representing row-by-row scanning of the element matrix.
      /// Non-zero values indicate points that belong to the element.
      /// If the pointer is NULL, then all values are considered non-zero, that is, the element is of a rectangular shape.
      /// This parameter is considered only if the shape is CV_SHAPE_CUSTOM.
      /// </param>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateStructuringElementEx(
          int cols,
          int rows,
          int anchorX,
          int anchorY,
          CvEnum.CV_ELEMENT_SHAPE shape,
          int[,] values);

      /// <summary>
      /// Releases the structuring element.
      /// </summary>
      /// <param name="ppElement">Pointer to the deallocated structuring element.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseStructuringElement(ref IntPtr ppElement);

      /// <summary>
      /// Performs advanced morphological transformations.
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="temp">
      /// Temporary image, required in some cases.
      /// The temporary image temp is required for morphological gradient and, in case of in-place operation, for "top hat" and "black hat".
      /// </param>
      /// <param name="element">Structuring element.</param>
      /// <param name="operation">Type of morphological operation.</param>
      /// <param name="iterations">Number of times erosion and dilation are applied.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvMorphologyEx(
         IntPtr src,
         IntPtr dst,
         IntPtr temp,
         IntPtr element,
         CvEnum.CV_MORPH_OP operation,
         int iterations);

      #region Histograms
      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if != 0, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform == 0, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject</param>
      /// <returns>A pointer to the histogram</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         int uniform);

      /// <summary>
      /// Finds the minimum and maximum histogram bins and their positions
      /// </summary>
      /// <remarks>
      /// Among several extremums with the same value the ones with minimum index (in lexicographical order). 
      /// In case of several maximums or minimums the earliest in lexicographical order extrema locations are returned.
      /// </remarks>
      /// <param name="hist">Histogram</param>
      /// <param name="minValue">Pointer to the minimum value of the histogram </param>
      /// <param name="maxValue">Pointer to the maximum value of the histogram </param>
      /// <param name="minIdx">Pointer to the array of coordinates for minimum </param>
      /// <param name="maxIdx">Pointer to the array of coordinates for maximum </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvGetMinMaxHistValue( 
         IntPtr hist,
         ref float minValue, 
         ref float maxValue,
         int[] minIdx, 
         int[] maxIdx);

      /// <summary>
      /// Normalizes the histogram bins by scaling them, such that the sum of the bins becomes equal to factor
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="factor">Normalization factor</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvNormalizeHist(IntPtr hist, double factor);

      /// <summary>
      /// Clears histogram bins that are below the specified threshold
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="threshold">Threshold level</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvThreshHist(IntPtr hist, double threshold);


      /// <summary>
      /// Sets all histogram bins to 0 in case of dense histogram and removes all histogram bins in case of sparse array
      /// </summary>
      /// <param name="hist">Histogram</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvClearHist(IntPtr hist);

      /// <summary>
      /// initializes the histogram, which header and bins are allocated by user. No cvReleaseHist need to be called afterwards. Only dense histograms can be initialized this way. 
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="data">The underline memory storage (pointer to array of float)</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>Pointer to the histogram</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvMakeHistHeaderForArray( 
         int dims, 
         [In] int[] sizes, 
         IntPtr hist,
         IntPtr data, 
         [In] IntPtr[] ranges, 
         int uniform);

      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>A pointer to the histogram</returns>
      public static IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         bool uniform)
      {
         return cvCreateHist(dims, sizes, type, ranges, uniform ? 1 : 0);
      }

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcArrHist(
          IntPtr[] image,
          IntPtr hist,
          int accumulate,
          IntPtr mask);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcArrHist(IntPtr[] image, IntPtr hist, bool accumulate, IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Makes a copy of the histogram. If the second histogram pointer *dst is NULL, a new histogram of the same size as src is created. Otherwise, both histograms must have equal types and sizes. Then the function copies the source histogram bins values to destination histogram and sets the same bin values ranges as in src.
      /// </summary>
      /// <param name="src">The source histogram</param>
      /// <param name="dst">The destination histogram</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCopyHist( IntPtr src, ref IntPtr dst );

      /// <summary>
      /// Compares two dense histograms
      /// </summary>
      /// <param name="hist1">The first dense histogram. </param>
      /// <param name="hist2">The second dense histogram.</param>
      /// <param name="method">Comparison method</param>
      /// <returns>Result of the comparison</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvCompareHist(
         IntPtr hist1, 
         IntPtr hist2, 
         CvEnum.HISTOGRAM_COMP_METHOD method);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcHist(
          IntPtr[] image,
          IntPtr hist,
          bool accumulate,
          IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Calculates the back project of the histogram. 
      /// For each tuple of pixels at the same position of all input single-channel images the function puts the value of the histogram bin, corresponding to the tuple, to the destination image. 
      /// In terms of statistics, the value of each output image pixel is probability of the observed tuple given the distribution (histogram). 
      /// </summary>
      /// <example>
      /// To find a red object in the picture, one may do the following: 
      /// 1. Calculate a hue histogram for the red object assuming the image contains only this object. The histogram is likely to have a strong maximum, corresponding to red color. 
      /// 2. Calculate back projection of a hue plane of input image where the object is searched, using the histogram. Threshold the image. 
      /// 3. Find connected components in the resulting picture and choose the right component using some additional criteria, for example, the largest connected component. 
      /// That is the approximate algorithm of Camshift color object tracker, except for the 3rd step, instead of which CAMSHIFT algorithm is used to locate the object on the back projection given the previous object position. 
      /// </example>
      /// <param name="image">Source images (though you may pass CvMat** as well), all are of the same size and type </param>
      /// <param name="backProject">Destination back projection image of the same type as the source images</param>
      /// <param name="hist">Histogram</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcArrBackProject(IntPtr[] image, IntPtr backProject, IntPtr hist);

      /// <summary>
      /// The algorithm normalizes brightness and increases contrast of the image
      /// </summary>
      /// <param name="src">The input 8-bit single-channel image</param>
      /// <param name="dst">The output image of the same size and the same data type as src</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvEqualizeHist(IntPtr src, IntPtr dst);

      /// <summary>
      /// Calculates the back project of the histogram. 
      /// For each tuple of pixels at the same position of all input single-channel images the function puts the value of the histogram bin, corresponding to the tuple, to the destination image. 
      /// In terms of statistics, the value of each output image pixel is probability of the observed tuple given the distribution (histogram). 
      /// </summary>
      /// <example>
      /// To find a red object in the picture, one may do the following: 
      /// 1. Calculate a hue histogram for the red object assuming the image contains only this object. The histogram is likely to have a strong maximum, corresponding to red color. 
      /// 2. Calculate back projection of a hue plane of input image where the object is searched, using the histogram. Threshold the image. 
      /// 3. Find connected components in the resulting picture and choose the right component using some additional criteria, for example, the largest connected component. 
      /// That is the approximate algorithm of Camshift color object tracker, except for the 3rd step, instead of which CAMSHIFT algorithm is used to locate the object on the back projection given the previous object position. 
      /// </example>
      /// <param name="image">Source images (though you may pass CvMat** as well), all are of the same size and type </param>
      /// <param name="backProject">Destination back projection image of the same type as the source images</param>
      /// <param name="hist">Histogram</param>
      [DllImport(CV_LIBRARY, EntryPoint = "cvCalcArrBackProject")]
      public static extern void cvCalcBackProject(
         IntPtr[] image,
         IntPtr backProject,
         IntPtr hist);

      /// <summary>
      /// Compares histogram, computed over each possible rectangular patch of the specified size in the input images, and stores the results to the output map dst.
      /// </summary>
      /// <remarks>In pseudo-code the operation may be written as:
      ///for (x,y) in images (until (x+patch_size.width-1,y+patch_size.height-1) is inside the images) do
      ///    compute histogram over the ROI (x,y,x+patch_size.width,y+patch_size.height) in images
      ///       (see cvCalcHist)
      ///    normalize the histogram using the factor
      ///       (see cvNormalizeHist)
      ///    compare the normalized histogram with input histogram hist using the specified method
      ///       (see cvCompareHist)
      ///    store the result to dst(x,y)
      ///end for
      ///</remarks>
      /// <param name="images">Source images (though, you may pass CvMat** as well), all of the same size</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="patchSize">Size of patch slid though the source images. </param>
      /// <param name="hist">Histogram </param>
      /// <param name="method">Comparison methof</param>
      /// <param name="factor">Normalization factor for histograms, will affect normalization scale of destination image, pass 1. if unsure.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcBackProjectPatch(
         IntPtr[] images,
         IntPtr dst,
         System.Drawing.Size patchSize,
         IntPtr hist,
         CvEnum.HISTOGRAM_COMP_METHOD method,
         float factor);

      /// <summary>
      /// calculates the object probability density from the two histograms as:
      /// dist_hist(I)=0,      if hist1(I)==0;
      /// dist_hist(I)=scale,  if hist1(I)!=0 &amp;&amp; hist2(I)&gt;hist1(I);
      /// dist_hist(I)=hist2(I)*scale/hist1(I), if hist1(I)!=0 &amp;&amp; hist2(I)&lt;=hist1(I)
      /// </summary>
      /// <param name="hist1">First histogram (the divisor)</param>
      /// <param name="hist2">Second histogram.</param>
      /// <param name="dstHist">Destination histogram. </param>
      /// <param name="scale">Scale factor for the destination histogram.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcProbDensity(
         IntPtr hist1,
         IntPtr hist2,
         IntPtr dstHist,
         double scale);

      /// <summary>
      /// Releases the histogram (header and the data). 
      /// The pointer to histogram is cleared by the function. 
      /// If *hist pointer is already NULL, the function does nothing.
      /// </summary>
      /// <param name="hist">Double pointer to the released histogram</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseHist(ref IntPtr hist);
      #endregion

      #region Optical flow
      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel.</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels. </param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowLK(
              IntPtr prev,
              IntPtr curr,
              System.Drawing.Size winSize,
              IntPtr velx,
              IntPtr vely);

      /// <summary>
      /// Computes flow for every pixel of the first input image using Horn &amp; Schunck algorithm 
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel</param>
      /// <param name="curr">Second image, 8-bit, single-channel</param>
      /// <param name="usePrevious">Uses previous (input) velocity field</param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel</param>
      /// <param name="lambda">Lagrangian multiplier</param>
      /// <param name="criteria">Criteria of termination of velocity computing</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowHS(
              IntPtr prev,
              IntPtr curr,
              int usePrevious,
              IntPtr velx,
              IntPtr vely,
              double lambda,
              MCvTermCriteria criteria);

      /// <summary>
      /// Calculates optical flow for overlapped blocks block_size.width * block_size.height pixels each, thus the velocity fields are smaller than the original images. For every block in prev the functions tries to find a similar block in curr in some neighborhood of the original block or shifted by (velx(x0,y0),vely(x0,y0)) block as has been calculated by previous function call (if use_previous=1)
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel. </param>
      /// <param name="blockSize">Size of basic blocks that are compared.</param>
      /// <param name="shiftSize">Block coordinate increments. </param>
      /// <param name="maxRange">Size of the scanned neighborhood in pixels around block.</param>
      /// <param name="usePrevious">Uses previous (input) velocity field. </param>
      /// <param name="velx">Horizontal component of the optical flow of floor((prev->width - block_size.width)/shiftSize.width) x floor((prev->height - block_size.height)/shiftSize.height) size, 32-bit floating-point, single-channel. </param>
      /// <param name="vely">Vertical component of the optical flow of the same size velx, 32-bit floating-point, single-channel.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowBM(
              IntPtr prev,
              IntPtr curr,
              System.Drawing.Size blockSize,
              System.Drawing.Size shiftSize,
              System.Drawing.Size maxRange,
              int usePrevious,
              IntPtr velx,
              IntPtr vely);

      /// <summary>
      /// Implements sparse iterative version of Lucas-Kanade optical flow in pyramids ([Bouguet00]). It calculates coordinates of the feature points on the current video frame given their coordinates on the previous frame. The function finds the coordinates with sub-pixel accuracy. 
      /// </summary>
      /// <remarks>Both parameters prev_pyr and curr_pyr comply with the following rules: if the image pointer is 0, the function allocates the buffer internally, calculates the pyramid, and releases the buffer after processing. Otherwise, the function calculates the pyramid and stores it in the buffer unless the flag CV_LKFLOW_PYR_A[B]_READY is set. The image should be large enough to fit the Gaussian pyramid data. After the function call both pyramids are calculated and the readiness flag for the corresponding image can be set in the next call (i.e., typically, for all the image pairs except the very first one CV_LKFLOW_PYR_A_READY is set). </remarks>
      /// <param name="prev">First frame, at time t. </param>
      /// <param name="curr">Second frame, at time t + dt .</param>
      /// <param name="prevPyr">Buffer for the pyramid for the first frame. If the pointer is not NULL , the buffer must have a sufficient size to store the pyramid from level 1 to level #level ; the total size of (image_width+8)*image_height/3 bytes is sufficient. </param>
      /// <param name="currPyr">Similar to prev_pyr, used for the second frame. </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found. </param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input </param>
      /// <param name="count">Number of feature points.</param>
      /// <param name="winSize">Size of the search window of each pyramid level.</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc. </param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise.</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points. Optional parameter; can be NULL </param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped.</param>
      /// <param name="flags">Miscellaneous flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowPyrLK(
          IntPtr prev,
          IntPtr curr,
          IntPtr prevPyr,
          IntPtr currPyr,
          float[,] prevFeatures,
          float[,] currFeatures,
          int count,
          System.Drawing.Size winSize,
          int level,
          Byte[] status,
          float[] trackError,
          MCvTermCriteria criteria,
          CvEnum.LKFLOW_TYPE flags);

      /// <summary>
      /// Implements sparse iterative version of Lucas-Kanade optical flow in pyramids ([Bouguet00]). It calculates coordinates of the feature points on the current video frame given their coordinates on the previous frame. The function finds the coordinates with sub-pixel accuracy. 
      /// </summary>
      /// <remarks>Both parameters prev_pyr and curr_pyr comply with the following rules: if the image pointer is 0, the function allocates the buffer internally, calculates the pyramid, and releases the buffer after processing. Otherwise, the function calculates the pyramid and stores it in the buffer unless the flag CV_LKFLOW_PYR_A[B]_READY is set. The image should be large enough to fit the Gaussian pyramid data. After the function call both pyramids are calculated and the readiness flag for the corresponding image can be set in the next call (i.e., typically, for all the image pairs except the very first one CV_LKFLOW_PYR_A_READY is set). </remarks>
      /// <param name="prev">First frame, at time t. </param>
      /// <param name="curr">Second frame, at time t + dt .</param>
      /// <param name="prevPyr">Buffer for the pyramid for the first frame. If the pointer is not NULL , the buffer must have a sufficient size to store the pyramid from level 1 to level #level ; the total size of (image_width+8)*image_height/3 bytes is sufficient. </param>
      /// <param name="currPyr">Similar to prev_pyr, used for the second frame. </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found. </param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input </param>
      /// <param name="count">Number of feature points.</param>
      /// <param name="winSize">Size of the search window of each pyramid level.</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc. </param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise.</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points. Optional parameter; can be NULL </param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped.</param>
      /// <param name="flags">Miscellaneous flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowPyrLK(
         IntPtr prev,
         IntPtr curr,
         IntPtr prevPyr,
         IntPtr currPyr,
         [In]
         System.Drawing.PointF[] prevFeatures,
         [Out]
         System.Drawing.PointF[] currFeatures,
         int count,
         System.Drawing.Size winSize,
         int level,
         Byte[] status,
         float[] trackError,
         MCvTermCriteria criteria,
         CvEnum.LKFLOW_TYPE flags);

      #endregion

      /// <summary>
      /// Retrieves the spatial moment, which in case of image moments is defined as:
      /// MxOrder,yOrder=sumx,y(I(x,y) * xxOrder * yyOrder)
      /// where I(x,y) is the intensity of the pixel (x, y). 
      /// </summary>
      /// <param name="moments">The moment state</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0. </param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The spatial moment</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvGetSpatialMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      /// <summary>
      /// Retrieves the central moment, which in case of image moments is defined as:
      /// Mu xOrder,yOrder=sumx,y(I(x,y) * (x-xc)xOrder *(y-yc)yOrder),
      /// where xc=M10/M00, yc=M01/M00 - coordinates of the gravity center
      /// </summary>
      /// <param name="moments">Pointer to the moment state structure</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvGetCentralMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      #region Accumulation of Background Statistics
      /// <summary>
      /// Adds the whole image or its selected region to accumulator sum
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point. (each channel of multi-channel image is processed independently). </param>
      /// <param name="sum">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvAcc(IntPtr image, IntPtr sum, IntPtr mask);

      /// <summary>
      /// Adds the input image image or its selected region, raised to power 2, to the accumulator sqsum
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="sqsum">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSquareAcc(IntPtr image, IntPtr sqsum, IntPtr mask);

      /// <summary>
      /// Adds product of 2 images or thier selected regions to accumulator acc
      /// </summary>
      /// <param name="image1">First input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="image2">Second input image, the same format as the first one</param>
      /// <param name="acc">Accumulator of the same number of channels as input images, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvMultiplyAcc(IntPtr image1, IntPtr image2, IntPtr acc, IntPtr mask);

      /// <summary>
      /// Calculates weighted sum of input image image and the accumulator acc so that acc becomes a running average of frame sequence:
      /// acc(x,y)=(1-<paramref name="alpha"/>) * acc(x,y) + <paramref name="alpha"/> * image(x,y) if mask(x,y)!=0
      /// where <paramref name="alpha"/> regulates update speed (how fast accumulator forgets about previous frames). 
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently). </param>
      /// <param name="acc">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="alpha">Weight of input image</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvRunningAvg(IntPtr image, IntPtr acc, double alpha, IntPtr mask);
      #endregion

      /// <summary>
      /// Converts a rotation vector to rotation matrix or vice versa. Rotation vector is a compact representation of rotation matrix. Direction of the rotation vector is the rotation axis and the length of the vector is the rotation angle around the axis. The rotation matrix R, corresponding to the rotation vector r.
      /// </summary>
      /// <param name="src">The input rotation vector (3x1 or 1x3) or rotation matrix (3x3). </param>
      /// <param name="dst">The output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively</param>
      /// <param name="jacobian">Optional output Jacobian matrix, 3x9 or 9x3 - partial derivatives of the output array components w.r.t the input array components</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvRodrigues2(IntPtr src, IntPtr dst, IntPtr jacobian);

      /// <summary>
      /// Calculates seven Hu invariants
      /// </summary>
      /// <param name="moments">Pointer to the moment state structure</param>
      /// <param name="hu_moments">Pointer to Hu moments structure.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvGetHuMoments(ref MCvMoments moments, ref MCvHuMoments hu_moments);

      #region Kalman Filter
      /// <summary>
      /// Allocates CvKalman and all its matrices and initializes them somehow. 
      /// </summary>
      /// <param name="dynamParams">dimensionality of the state vector</param>
      /// <param name="measureParams">dimensionality of the measurement vector </param>
      /// <param name="controlParams">dimensionality of the control vector </param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateKalman(int dynamParams, int measureParams, int controlParams);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanCorrect(IntPtr kalman, IntPtr measurement);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanCorrect(ref MCvKalman kalman, IntPtr measurement);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanPredict(IntPtr kalman, IntPtr control);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanPredict(ref MCvKalman kalman, IntPtr control);

      /// <summary>
      /// Releases the structure CvKalman and all underlying matrices
      /// </summary>
      /// <param name="kalman">reference of the pointer to the Kalman filter structure.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseKalman(ref IntPtr kalman);
      #endregion

      /// <summary>
      /// Updates the motion history image as following:
      /// mhi(x,y)=timestamp  if silhouette(x,y)!=0
      ///         0          if silhouette(x,y)=0 and mhi(x,y)&lt;timestamp-duration
      ///         mhi(x,y)   otherwise
      /// That is, MHI pixels where motion occurs are set to the current timestamp, while the pixels where motion happened far ago are cleared. 
      /// </summary>
      /// <param name="silhouette">Silhouette mask that has non-zero pixels where the motion occurs. </param>
      /// <param name="mhi">Motion history image, that is updated by the function (single-channel, 32-bit floating-point) </param>
      /// <param name="timestamp">Current time in milliseconds or other units. </param>
      /// <param name="duration">Maximal duration of motion track in the same units as timestamp. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvUpdateMotionHistory(
          IntPtr silhouette,
          IntPtr mhi,
          double timestamp,
          double duration);

      /// <summary>
      /// Calculates the derivatives Dx and Dy of mhi and then calculates gradient orientation as:
      ///orientation(x,y)=arctan(Dy(x,y)/Dx(x,y))
      ///where both Dx(x,y)' and Dy(x,y)' signs are taken into account (as in cvCartToPolar function). After that mask is filled to indicate where the orientation is valid (see delta1 and delta2 description). 
      /// </summary>
      /// <param name="mhi">Motion history image</param>
      /// <param name="mask">Mask image; marks pixels where motion gradient data is correct. Output parameter.</param>
      /// <param name="orientation">Motion gradient orientation image; contains angles from 0 to ~360. </param>
      /// <param name="delta1">The function finds minimum (m(x,y)) and maximum (M(x,y)) mhi values over each pixel (x,y) neihborhood and assumes the gradient is valid only if min(delta1,delta2) &lt;= M(x,y)-m(x,y) &lt;= max(delta1,delta2). </param>
      /// <param name="delta2">The function finds minimum (m(x,y)) and maximum (M(x,y)) mhi values over each pixel (x,y) neihborhood and assumes the gradient is valid only if min(delta1,delta2) &lt;= M(x,y)-m(x,y) &lt;= max(delta1,delta2).</param>
      /// <param name="apertureSize">Aperture size of derivative operators used by the function: CV_SCHARR, 1, 3, 5 or 7 (see cvSobel). </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcMotionGradient(
          IntPtr mhi,
          IntPtr mask,
          IntPtr orientation,
          double delta1,
          double delta2,
          int apertureSize);

      /// <summary>
      /// Runs the Harris edge detector on image. Similarly to cvCornerMinEigenVal and cvCornerEigenValsAndVecs, for each pixel it calculates 2x2 gradient covariation matrix M over block_size x block_size neighborhood. Then, it stores
      /// det(M) - k*trace(M)^2
      /// to the destination image. Corners in the image can be found as local maxima of the destination image.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="harrisResponce">Image to store the Harris detector responces. Should have the same size as image </param>
      /// <param name="blockSize">Neighborhood size </param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator (see cvSobel). format. In the case of floating-point input format this parameter is the number of the fixed float filter used for differencing. </param>
      /// <param name="k">Harris detector free parameter.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCornerHarris(
          IntPtr image,
          IntPtr harrisResponce,
          int blockSize,
          int apertureSize,
          double k);

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="corners">Initial coordinates of the input corners and refined coordinates on output</param>
      /// <param name="count">Number of corners</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindCornerSubPix(
         IntPtr image,
         float[,] corners,
         int count,
         System.Drawing.Size win,
         System.Drawing.Size zeroZone,
         MCvTermCriteria criteria);

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="corners">Initial coordinates of the input corners and refined coordinates on output</param>
      /// <param name="count">Number of corners</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindCornerSubPix(
         IntPtr image,
         [In, Out]
         System.Drawing.PointF[] corners,
         int count,
         System.Drawing.Size win,
         System.Drawing.Size zeroZone,
         MCvTermCriteria criteria);

      /// <summary>
      /// Calculates one or more integral images for the source image 
      /// Using these integral images, one may calculate sum, mean, standard deviation over arbitrary up-right or rotated rectangular region of the image in a constant time.
      /// It makes possible to do a fast blurring or fast block correlation with variable window size etc. In case of multi-channel images sums for each channel are accumulated independently. 
      /// </summary>
      /// <param name="image">The source image, WxH, 8-bit or floating-point (32f or 64f) image.</param>
      /// <param name="sum">The integral image, W+1xH+1, 32-bit integer or double precision floating-point (64f). </param>
      /// <param name="sqsum">The integral image for squared pixel values, W+1xH+1, double precision floating-point (64f). </param>
      /// <param name="tiltedSum">The integral for the image rotated by 45 degrees, W+1xH+1, the same data type as sum.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvIntegral(
         IntPtr image,
         IntPtr sum,
         IntPtr sqsum,
         IntPtr tiltedSum);

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">3? transformation matrix</param>
      /// <param name="flags"></param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvWarpPerspective(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix,
         int flags,
         MCvScalar fillval);

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3? matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvGetPerspectiveTransform( 
         System.Drawing.PointF[] src, 
         System.Drawing.PointF[] dst,
         IntPtr mapMatrix );

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3? matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvGetPerspectiveTransform(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Calculates rotation matrix
      /// </summary>
      /// <param name="center">Center of the rotation in the source image. </param>
      /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner).</param>
      /// <param name="scale">Isotropic scale factor</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cv2DRotationMatrix(
          System.Drawing.PointF center,
          double angle,
          double scale,
          IntPtr mapMatrix);

      /// <summary>
      /// Calculates distance to closest zero pixel for all non-zero pixels of source image
      /// </summary>
      /// <param name="src">Source 8-bit single-channel (binary) image.</param>
      /// <param name="dst">Output image with calculated distances (32-bit floating-point, single-channel). </param>
      /// <param name="distanceType">Type of distance</param>
      /// <param name="maskSize">Size of distance transform mask; can be 3 or 5.
      /// In case of CV_DIST_L1 or CV_DIST_C the parameter is forced to 3, because 3x3 mask gives the same result as 5x5 yet it is faster.</param>
      /// <param name="userMask">User-defined mask in case of user-defined distance.
      /// It consists of 2 numbers (horizontal/vertical shift cost, diagonal shift cost) in case of 3x3 mask
      /// and 3 numbers (horizontal/vertical shift cost, diagonal shift cost, knights move cost) in case of 5x5 mask.</param>
      /// <param name="labels">The optional output 2d array of labels of integer type and the same size as src and dst.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDistTransform(
         IntPtr src,
         IntPtr dst,
         CvEnum.DIST_TYPE distanceType,
         int maskSize,
         float[] userMask,
         IntPtr labels);

      /// <summary>
      /// Fills a connected component with given color.
      /// </summary>
      /// <param name="src">Input 1- or 3-channel, 8-bit or floating-point image. It is modified by the function unless CV_FLOODFILL_MASK_ONLY flag is set.</param>
      /// <param name="seedPoint">The starting point.</param>
      /// <param name="newVal">New value of repainted domain pixels.</param>
      /// <param name="loDiff">Maximal lower brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="upDiff">Maximal upper brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="comp">Pointer to structure the function fills with the information about the repainted domain.</param>
      /// <param name="flags">The operation flags.
      /// Lower bits contain connectivity value, 4 (by default) or 8, used within the function.
      /// Connectivity determines which neighbors of a pixel are considered.
      /// Upper bits can be 0 or combination of the following flags:
      /// CV_FLOODFILL_FIXED_RANGE - if set the difference between the current pixel and seed pixel is considered,
      /// otherwise difference between neighbor pixels is considered (the range is floating).
      /// CV_FLOODFILL_MASK_ONLY - if set, the function does not fill the image (new_val is ignored),
      /// but the fills mask (that must be non-NULL in this case). </param>
      /// <param name="mask">Operation mask,
      /// should be singe-channel 8-bit image, 2 pixels wider and 2 pixels taller than image.
      /// If not NULL, the function uses and updates the mask, so user takes responsibility of initializing mask content.
      /// Floodfilling can't go across non-zero pixels in the mask, for example, an edge detector output can be used as a mask to stop filling at edges.
      /// Or it is possible to use the same mask in multiple calls to the function to make sure the filled area do not overlap.
      /// Note: because mask is larger than the filled image, pixel in mask that corresponds to (x,y) pixel in image will have coordinates (x+1,y+1).</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFloodFill(
         IntPtr src,
         System.Drawing.Point seedPoint,
         MCvScalar newVal,
         MCvScalar loDiff,
         MCvScalar upDiff,
         out MCvConnectedComp comp,
         int flags,
         IntPtr mask);

      /// <summary>
      /// Fills a connected component with given color.
      /// </summary>
      /// <param name="src">Input 1- or 3-channel, 8-bit or floating-point image. It is modified by the function unless CV_FLOODFILL_MASK_ONLY flag is set.</param>
      /// <param name="seedPoint">The starting point.</param>
      /// <param name="newVal">New value of repainted domain pixels.</param>
      /// <param name="loDiff">Maximal lower brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="upDiff">Maximal upper brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="comp">Pointer to structure the function fills with the information about the repainted domain.</param>
      /// <param name="mask">Operation mask,
      /// should be singe-channel 8-bit image, 2 pixels wider and 2 pixels taller than image.
      /// If not NULL, the function uses and updates the mask, so user takes responsibility of initializing mask content.
      /// Floodfilling can't go across non-zero pixels in the mask, for example, an edge detector output can be used as a mask to stop filling at edges.
      /// Or it is possible to use the same mask in multiple calls to the function to make sure the filled area do not overlap.
      /// Note: because mask is larger than the filled image, pixel in mask that corresponds to (x,y) pixel in image will have coordinates (x+1,y+1).</param>
      /// <param name="connectivity">The connectivity of flood fill</param>
      /// <param name="flags">The flood fill types</param>
      public static void cvFloodFill(
         IntPtr src,
         System.Drawing.Point seedPoint,
         MCvScalar newVal,
         MCvScalar loDiff,
         MCvScalar upDiff,
         out MCvConnectedComp comp,
         CvEnum.CONNECTIVITY connectivity,
         CvEnum.FLOODFILL_FLAG flags,
         IntPtr mask)
      {
         cvFloodFill(src, seedPoint, newVal, loDiff, upDiff, out comp, (int)connectivity | (int)flags, mask);
      }

      /*
              /// <summary>
              ///  Fits a line into set of 2d points in a robust way (M-estimator technique) 
              /// </summary>
              public static void cvFitLine2D(IntPtr points, int count, int dist,
                                    float param, float reps, float aeps, IntPtr line)
              {
                  MCvMat mat = cvMat(1, count, CV_MAKETYPE((int)MAT_DEPTH.CV_32F, 2), points);
                  //float _param = (param != IntPtr.Zero )? *(float*)param : 0.f;
                  //assert( dist != CV_DIST_USER );
                  IntPtr l = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 1000);
                  cvFitLine(mat, dist, param, reps, aeps, line);
                  Marshal.FreeHGlobal(l);
              }
      */
      #endregion

      #region HIGHGUI_LIBRARY
      /// <summary>
      /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
      /// </summary>
      /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed</param>
      /// <returns>Pointer to the capture structure</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern IntPtr cvCreateCameraCapture(int index);

      /// <summary>
      /// Allocates and initialized the CvCapture structure for reading the video stream from the specified file. 
      ///After the allocated structure is not used any more it should be released by cvReleaseCapture function. 
      /// </summary>
      /// <param name="filename">Name of the video file</param>
      /// <returns>Pointer to the capture structure</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern IntPtr cvCreateFileCapture([MarshalAs(_stringMarshalType)] String filename);

      /// <summary>
      /// The function cvReleaseCapture releases the CvCapture structure allocated by cvCreateFileCapture or cvCreateCameraCapture
      /// </summary>
      /// <param name="capture">pointer to video capturing structure. </param>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern void cvReleaseCapture(ref IntPtr capture);

      /// <summary>
      /// Grabs a frame from camera or video file, decompresses and returns it. This function is just a combination of cvGrabFrame and cvRetrieveFrame in one call. The returned image should not be released or modified by user. 
      /// </summary>
      /// <param name="capture">video capturing structure</param>
      /// <returns></returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern IntPtr cvQueryFrame(IntPtr capture);

      /// <summary>
      /// Retrieves the specified property of camera or video file
      /// </summary>
      /// <param name="capture">video capturing structure</param>
      /// <param name="prop">property identifier</param>
      /// <returns> the specified property of camera or video file</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern double cvGetCaptureProperty(IntPtr capture, CvEnum.CAP_PROP prop);

      /// <summary>
      /// Sets the specified property of video capturing
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="property_id">Property identifier</param>
      /// <param name="value">Value of the property</param>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern void cvSetCaptureProperty(IntPtr capture, CvEnum.CAP_PROP property_id, double value);

      /// <summary>
      /// Loads an image from the specified file and returns the pointer to the loaded image. Currently the following file formats are supported: 
      /// Windows bitmaps - BMP, DIB; 
      /// JPEG files - JPEG, JPG, JPE; 
      /// Portable Network Graphics - PNG; 
      /// Portable image format - PBM, PGM, PPM; 
      /// Sun rasters - SR, RAS; 
      /// TIFF files - TIFF, TIF; 
      /// OpenEXR HDR images - EXR; 
      /// JPEG 2000 images - jp2. 
      /// </summary>
      /// <param name="filename">The name of the file to be loaded</param>
      /// <param name="loadType"></param>
      /// <returns>The loaded image</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern IntPtr cvLoadImage(
         [MarshalAs(_stringMarshalType)] String filename,
         CvEnum.LOAD_IMAGE_TYPE loadType);

      /// <summary>
      /// Saves the image to the specified file. The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format
      /// </summary>
      /// <param name="filename">The name of the file to be saved to</param>
      /// <param name="image">The image to be saved</param>
      /// <returns></returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern int cvSaveImage([MarshalAs(_stringMarshalType)] String filename, IntPtr image);

      [DllImport(HIGHGUI_LIBRARY, EntryPoint = "cvNamedWindow")]
      private static extern int _cvNamedWindow([MarshalAs(_stringMarshalType)] String name, int flags);

      /// <summary>
      /// Creates a window which can be used as a placeholder for images and trackbars. Created windows are reffered by their names. 
      /// If the window with such a name already exists, the function does nothing.
      /// </summary>
      /// <param name="name">Name of the window which is used as window identifier and appears in the window caption</param>
      public static int cvNamedWindow(String name)
      {
         return _cvNamedWindow(name, 1);
      }

      /// <summary>
      /// Waits for key event infinitely (delay &lt;= 0) or for "delay" milliseconds. 
      /// </summary>
      /// <param name="delay">Delay in milliseconds.</param>
      /// <returns>The code of the pressed key or -1 if no key were pressed until the specified timeout has elapsed</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern int cvWaitKey(int delay);

      /// <summary>
      /// Shows the image in the specified window
      /// </summary>
      /// <param name="name">Name of the window</param>
      /// <param name="image">Image to be shown</param>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern void cvShowImage([MarshalAs(_stringMarshalType)] String name, IntPtr image);

      /// <summary>
      /// Destroys the window with a given name
      /// </summary>
      /// <param name="name">Name of the window to be destroyed</param>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern void cvDestroyWindow([MarshalAs(_stringMarshalType)] String name);

      /// <summary>
      /// Creates video writer structure.
      /// </summary>
      /// <param name="filename">Name of the output video file.</param>
      /// <param name="fourcc">4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.</param>
      /// <param name="fps">Framerate of the created video stream. </param>
      /// <param name="frameSize">Size of video frames.</param>
      /// <param name="isColor">If != 0, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
      /// <returns>The video writer</returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern IntPtr cvCreateVideoWriter(
          [MarshalAs(_stringMarshalType)] String filename,
          int fourcc,
          double fps,
          System.Drawing.Size frameSize,
          int isColor);

      /// <summary>
      /// Creates video writer structure.
      /// </summary>
      /// <param name="filename">Name of the output video file.</param>
      /// <param name="fourcc">4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.</param>
      /// <param name="fps">Framerate of the created video stream. </param>
      /// <param name="frameSize">Size of video frames.</param>
      /// <param name="isColor">If it is true, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
      /// <returns>The video writer</returns>
      public static IntPtr cvCreateVideoWriter(String filename,
          int fourcc,
          double fps,
          System.Drawing.Size frameSize,
          bool isColor)
      {
         return cvCreateVideoWriter(filename, fourcc, fps, frameSize, isColor ? 1 : 0);
      }

      /// <summary>
      /// Finishes writing to video file and releases the structure.
      /// </summary>
      /// <param name="writer">pointer to video file writer structure</param>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern void cvReleaseVideoWriter(ref IntPtr writer);

      /// <summary>
      /// Writes/appends one frame to video file.
      /// </summary>
      /// <param name="writer">video writer structure.</param>
      /// <param name="image">the written frame</param>
      /// <returns></returns>
      [DllImport(HIGHGUI_LIBRARY)]
      public static extern int cvWriteFrame(IntPtr writer, IntPtr image);

      #endregion

      #region CV MACROS

      /// <summary>
      /// This function performs the same as CV_MAKETYPE macro
      /// </summary>
      /// <param name="depth">The type of depth</param>
      /// <param name="cn">The number of channels</param>
      /// <returns></returns>
      public static int CV_MAKETYPE(int depth, int cn)
      {
         return ((depth) + (((cn) - 1) << 3));
      }

      /*
      private static int _CV_MAT_DEPTH(int flag)
      {
         return flag & ((1 << 3) - 1);
      }
      private static int _CV_MAT_TYPE(int type)
      {
         return type & ((1 << 3) * 64 - 1);
      }

      private static int _CV_MAT_CN(int flag)
      {
         return ((((flag) & ((64 - 1) << 3)) >> 3) + 1);
      }
      private static int _CV_ELEM_SIZE(int type)
      {
         return (_CV_MAT_CN(type) << ((((4 / 4 + 1) * 16384 | 0x3a50) >> _CV_MAT_DEPTH(type) * 2) & 3));
      }*/

      /*
      public static MCvMat cvMat(int rows, int cols, int type, IntPtr data)
      {
          MCvMat m;
          Debug.Assert(_CV_MAT_DEPTH(type) <= (int)CvEnum.MAT_DEPTH.CV_64F);
          type = _CV_MAT_TYPE(type);
          m.type = 0x42420000 | (1 << 14) | type;
          m.cols = cols;
          m.rows = rows;
          m.step = rows > 1 ? m.cols * _CV_ELEM_SIZE(type) : 0;
          m.data = data;
          m.refcount = IntPtr.Zero;
          m.hdr_refcount = 0;
          return m;
      }*/

      /// <summary>
      /// Generate 4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.
      /// </summary>
      /// <param name="c1"></param>
      /// <param name="c2"></param>
      /// <param name="c3"></param>
      /// <param name="c4"></param>
      /// <returns></returns>
      public static int CV_FOURCC(char c1, char c2, char c3, char c4)
      {
         return (((c1) & 255) + (((c2) & 255) << 8) + (((c3) & 255) << 16) + (((c4) & 255) << 24));
      }
      #endregion

      #region CVAUX_LIBRARY
      /*
        /// <summary>
        /// Wrapped CvCallBack function pointer in CvAux
        /// </summary>
        /// <param name="index">index for the elements of user data</param>
        /// <param name="buffer">buffer used to store the returned element</param>
        /// <param name="user_data">user data</param>
        /// <returns>error code</returns>
        public delegate int CvCallBack(int index, IntPtr buffer, ref MUserData user_data);
        */

      #region Eigen Objects
      #region cvEigenDecomposite function
      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigInput">Pointer to the array of IplImage input objects</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <return>Calculated coefficients; an output parameter</return>
      public static float[] cvEigenDecomposite(
          IntPtr obj,
          IntPtr[] eigInput,
          IntPtr avg)
      {
         float[] coeffs = new float[eigInput.Length];
         cvEigenDecomposite(
             obj,
             eigInput.Length,
             eigInput,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             IntPtr.Zero,
             avg,
             coeffs);
         return coeffs;
      }

      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigenvecCount">Number of eigen objects</param>
      /// <param name="eigInput">Pointer either to the array of IplImage input objects or to the read callback function according to the value of the parameter <paramref name="ioFlags"/></param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <param name="coeffs">Calculated coefficients; an output parameter</param>
      [DllImport(CVAUX_LIBRARY)]
      private static extern void cvEigenDecomposite(
         IntPtr obj,
         int eigenvecCount,
         IntPtr[] eigInput,
         CvEnum.EIGOBJ_TYPE ioFlags,
         IntPtr userData,
         IntPtr avg,
         float[] coeffs);

      #endregion

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="input">Pointer to the array of IplImage input objects </param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      /// <param name="eigVecs">Pointer either to the array of eigen objects</param>
      /// <return>Pointer either to the array of eigen objects or to the write callback function</return>
      public static void cvCalcEigenObjects(
         IntPtr[] input,
         ref MCvTermCriteria calcLimit,
         IntPtr[] eigVecs,
         float[] eigVals,
         IntPtr avg)
      {
         cvCalcEigenObjects(
             input.Length,
             input,
             eigVecs,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             0,
             IntPtr.Zero,
             ref calcLimit,
             avg,
             eigVals);
      }

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="nObjects">Number of source objects</param>
      /// <param name="input">Pointer either to the array of IplImage input objects or to the read callback function</param>
      /// <param name="output">Pointer either to the array of eigen objects or to the write callback function</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="ioBufSize">Input/output buffer size in bytes. The size is zero, if unknown</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      [DllImport(CVAUX_LIBRARY)]
      private static extern void cvCalcEigenObjects(
         int nObjects,
         IntPtr[] input,
         IntPtr[] output,
         CvEnum.EIGOBJ_TYPE ioFlags,
         int ioBufSize,
         IntPtr userData,
         ref MCvTermCriteria calcLimit,
         IntPtr avg,
         float[] eigVals);

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. 
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      public static void cvEigenProjection(
         IntPtr[] inputVecs,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj)
      {
         CvInvoke.cvEigenProjection(
             inputVecs,
             inputVecs.Length,
             CvEnum.EIGOBJ_TYPE.CV_EIGOBJ_NO_CALLBACK,
             IntPtr.Zero,
             coeffs,
             avg,
             proj);
      }

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. Depending on io_flags parameter it may be used either in direct access or callback mode.
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="eigenvecCount">Number of eigenvectors</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userdata">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      [DllImport(CVAUX_LIBRARY)]
      private extern static void cvEigenProjection(
         IntPtr[] inputVecs,
         int eigenvecCount,
         CvEnum.EIGOBJ_TYPE ioFlags,
         IntPtr userdata,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj);
      #endregion

      #region background / forground  statistic
      /// <summary>
      /// Create a Gaussian background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(CVAUX_LIBRARY)]
      public extern static IntPtr cvCreateGaussianBGModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(CVAUX_LIBRARY)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a forground model
      /// </summary>
      /// <param name="firstFrame">The first frame</param>
      /// <param name="parameters">The forground statistic parameters</param>
      /// <returns>Pointer to the forground model</returns>
      [DllImport(CVAUX_LIBRARY)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr firstFrame, ref MCvFGDStatModelParams parameters);
      #endregion

      /*
      /// <summary>
      /// Calculates disparity for stereo-pair 
      /// </summary>
      /// <param name="leftImage">Left image of stereo pair, rectified grayscale 8-bit image</param>
      /// <param name="rightImage">Right image of stereo pair, rectified grayscale 8-bit image</param>
      /// <param name="mode">Algorithm used to find a disparity</param>
      /// <param name="depthImage">Destination depth image, grayscale 8-bit image that codes the scaled disparity, so that the zero disparity (corresponding to the points that are very far from the cameras) maps to 0, maximum disparity maps to 255.</param>
      /// <param name="maxDisparity">Maximum possible disparity. The closer the objects to the cameras, the larger value should be specified here. Too big values slow down the process significantly</param>
      /// <param name="param1">constant occlusion penalty</param>
      /// <param name="param2">constant match reward</param>
      /// <param name="param3">defines a highly reliable region (set of contiguous pixels whose reliability is at least param3)</param>
      /// <param name="param4">defines a moderately reliable region</param>
      /// <param name="param5">defines a slightly reliable region</param>
      [DllImport(CVAUX_LIBRARY)]
      public extern static void cvFindStereoCorrespondence(
                 IntPtr leftImage, IntPtr rightImage,
                 int mode, IntPtr depthImage,
                   int maxDisparity,
                   double param1, double param2, double param3,
                   double param4, double param5);
      */
      #endregion

      #region EXTERN_LIBRARY
      #region Forground detector
      /// <summary>
      /// Create a simple forground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">Pointer to the parameters of the detector</param>
      /// <returns>Pointer the to forground detector</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateFGDetectorBase(CvEnum.FORGROUND_DETECTOR_TYPE type, IntPtr param);

      /// <summary>
      /// Create a simple forground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">The parameters of the detector</param>
      /// <returns>Pointer the to forground detector</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateFGDetectorBase(CvEnum.FORGROUND_DETECTOR_TYPE type, ref MCvFGDStatModelParams param);
      
      /// <summary>
      /// Release the forground detector
      /// </summary>
      /// <param name="detector">The forground detector to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvFGDetectorRelease(IntPtr detector); 
      #endregion

      #region BlobSeq
      /// <summary>
      /// Create a BlobSeq
      /// </summary>
      /// <param name="blobSize">The size of the blob in bytes</param>
      /// <returns>Pointer to the BlobSeq</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobSeqCreate(int blobSize);

      /// <summary>
      /// Release the blob sequence
      /// </summary>
      /// <param name="blobSeq">The BlobSeq to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobSeqRelease(IntPtr blobSeq);
      #endregion

      #region BlobDetector
      /// <summary>
      /// Release the blob detector
      /// </summary>
      /// <param name="detector">the detector to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobDetectorRelease(IntPtr detector);

      /// <summary>
      /// Get the specific blob from the blob sequence
      /// </summary>
      /// <param name="blobSeq">the blob sequence</param>
      /// <param name="blobIndex">the index of the blob to be retrieved</param>
      /// <returns>Pointer to the specific blob</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobSeqGetBlob(IntPtr blobSeq, int blobIndex);

      /// <summary>
      /// Get the number of blob in the blob sequence
      /// </summary>
      /// <param name="blobSeq">The blob sequence</param>
      /// <returns>The number of blob in the blob sequence</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static int CvBlobSeqGetBlobNum(IntPtr blobSeq);

      /// <summary>
      /// Detect new blobs.
      /// </summary>
      /// <param name="detector">The blob detector</param>
      /// <param name="img">The image</param>
      /// <param name="imgFG">The forground mask</param>
      /// <param name="newBlobList">The new blob list</param>
      /// <param name="oldBlobList">The old blob list</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static int CvBlobDetectorDetectNewBlob(IntPtr detector, IntPtr img, IntPtr imgFG, IntPtr newBlobList, IntPtr oldBlobList);

      /// <summary>
      /// Get a simple blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobDetectorSimple();

      /// <summary>
      /// Get a CC blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobDetectorCC();
      #endregion

      #region BlobTracker
      /// <summary>
      /// Simple blob tracker based on connected component tracking
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerCC();

      /// <summary>
      /// Connected component tracking and mean-shift particle filter collion-resolver
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerCCMSPF();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerMSFG();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components:
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerMSFGS();

      /// <summary>
      /// Meanshift without connected-components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerMS();

      /// <summary>
      /// Particle filtering via Bhattacharya coefficient, which
      /// is roughly the dot-product of two probability densities.
      /// </summary>
      /// <remarks>See: Real-Time Tracking of Non-Rigid Objects using Mean Shift Comanicius, Ramesh, Meer, 2000, 8p</remarks>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerMSPF();

      /// <summary>
      /// Release the blob tracker
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobTrackerRealease(IntPtr tracker);

      /// <summary>
      /// Return number of currently tracked blobs
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <returns>Number of currently tracked blobs</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static int CvBlobTrackerGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Return pointer to specified by index blob
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      /// <returns>Pointer to specified by index blob</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobTrackerGetBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Delete blob by its index
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobTrackerDelBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Add new blob to track it and assign to this blob personal ID
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blob">pointer to structure with blob parameters (ID is ignored)</param>
      /// <param name="currentImage">current image</param>
      /// <param name="currentForgroundMask">current foreground mask</param>
      /// <returns>Pointer to new added blob</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobTrackerAddBlob(IntPtr tracker, ref MCvBlob blob, IntPtr currentImage, IntPtr currentForgroundMask  );
      #endregion

      #region BlobTrackPostProc
      /// <summary>
      /// Returns a Kalman blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcKalman();

      /// <summary>
      /// Returns a TimeAverRect blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverRect();

      /// <summary>
      /// Returns a TimeAverExp blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverExp();

      /// <summary>
      /// Release the blob tracking post process module
      /// </summary>
      /// <param name="postProc">The post process module to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobTrackPostProcRelease(IntPtr postProc);
      #endregion

      #region BlobTrackerAuto
      /// <summary>
      /// Create blob tracker auto ver1
      /// </summary>
      /// <param name="param">The parameters for the tracker</param>
      /// <returns>Pointer to the blob tracker auto</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvCreateBlobTrackerAuto1(ref MCvBlobTrackerAutoParam1 param);

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobTrackerAutoRelease(IntPtr tracker);

      /// <summary>
      /// Get the blob of specific index from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="index">The index of the blob</param>
      /// <returns>Pointer to the the blob</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobTrackerAutoGetBlob(IntPtr tracker, int index);
      /// <summary>
      /// Get the blob of specific id from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>Pointer to the blob of specific ID</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobTrackerAutoGetBlobByID(IntPtr tracker, int blobID);
      /// <summary>
      /// Get the number of blobs in the auto blob tracker 
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>The number of blobs</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static int CvBlobTrackerAutoGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Process a image frame
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="pImg">The frame to process</param>
      /// <param name="pMask">The forground mask, can be IntPtr.Zero if not needed</param>
      [DllImport(EXTERN_LIBRARY)]
      public extern static void CvBlobTrackerAutoProcess(IntPtr tracker, IntPtr pImg, IntPtr pMask);

      /// <summary>
      /// Get the forground mask
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>Pointer to the forground mask</returns>
      [DllImport(EXTERN_LIBRARY)]
      public extern static IntPtr CvBlobTrackerAutoGetFGMask(IntPtr tracker);
      #endregion
      #endregion
   }
}
