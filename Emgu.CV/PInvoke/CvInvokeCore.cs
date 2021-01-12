//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    public partial class CvInvoke
    {
        #region Error handling

        /// <summary>
        /// The default Exception callback to handle Error thrown by OpenCV
        /// </summary>
        public static readonly CvErrorCallback CvErrorHandlerThrowException = (CvErrorCallback)CvErrorHandler;

        /// <summary>
        /// An error handler which will ignore any error and continue
        /// </summary>
        public static readonly CvErrorCallback CvErrorHandlerIgnoreError = (CvErrorCallback)CvIgnoreErrorErrorHandler;

        /// <summary>
        /// A custom error handler for OpenCV
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="funcName">The source file name where error is encountered</param>
        /// <param name="errMsg">A description of the error</param>
        /// <param name="fileName">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        /// <param name="userData">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <returns>if 0, signal the process to continue</returns>
#if (UNITY_WSA || UNITY_ANDROID) && (!UNITY_EDITOR)
        [AOT.MonoPInvokeCallback(typeof(CvErrorCallback))]
#endif
        private static int CvIgnoreErrorErrorHandler(
            int status,
            IntPtr funcName,
            IntPtr errMsg,
            IntPtr fileName,
            int line,
            IntPtr userData)
        {
            SetErrStatus(Emgu.CV.CvEnum.ErrorCodes.StsOk); //clear the error status
            return 0; //signal the process to continue
        }

        /// <summary>
        /// A custom error handler for OpenCV
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="funcName">The source file name where error is encountered</param>
        /// <param name="errMsg">A description of the error</param>
        /// <param name="fileName">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        /// <param name="userData">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <returns>If 0, signal the process to continue</returns>
#if (UNITY_WSA || UNITY_ANDROID) && (!UNITY_EDITOR)
        [AOT.MonoPInvokeCallback(typeof(CvErrorCallback))]
#endif
        private static int CvErrorHandler(
            int status,
            IntPtr funcName,
            IntPtr errMsg,
            IntPtr fileName,
            int line,
            IntPtr userData)
        {
            try
            {
                SetErrStatus(Emgu.CV.CvEnum.ErrorCodes.StsOk); //clear the error status
                return 0; //signal the process to continue
            }
            finally
            {
                String funcNameStr = Marshal.PtrToStringAnsi(funcName);
                String errMsgStr = Marshal.PtrToStringAnsi(errMsg);
                String fileNameStr = Marshal.PtrToStringAnsi(fileName);
                throw new CvException(status, funcNameStr, errMsgStr, fileNameStr, line);
            }
        }

        /// <summary>
        /// Define an error callback that can be registered using cvRedirectError function
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="funcName">The source file name where error is encountered</param>
        /// <param name="errMsg">A description of the error</param>
        /// <param name="fileName">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        /// <param name="userData">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
        public delegate int CvErrorCallback(
            int status, IntPtr funcName, IntPtr errMsg, IntPtr fileName, int line, IntPtr userData);

#if UNITY_IOS
        /// <summary>
        /// Returns the current error status - the value set with the last cvSetErrStatus call. Note, that in Leaf mode the program terminates immediately after error occurred, so to always get control after the function call, one should call cvSetErrMode and set Parent or Silent error mode.
        /// </summary>
        /// <returns>the current error status</returns>
        public static int GetErrStatus()
        {
            return 0;
        }

        /// <summary>
        /// Sets the error status to the specified value. Mostly, the function is used to reset the error status (set to it CV_StsOk) to recover after error. In other cases it is more natural to call cvError or CV_ERROR.
        /// </summary>
        /// <param name="code">The error status.</param>
        public static void SetErrStatus(CvEnum.ErrorCodes code)
        {
        }

        /// <summary>
        /// Returns the textual description for the specified error status code. In case of unknown status the function returns NULL pointer. 
        /// </summary>
        /// <param name="status">The error status</param>
        /// <returns>the textual description for the specified error status code.</returns>
        public static String ErrorStr(int status)
        {
            return String.Empty;
        }

        /// <summary>
        /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
        /// </summary>
        /// <param name="errorHandler">The new error handler</param>
        /// <param name="userdata">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
        /// <returns></returns>
        public static IntPtr RedirectError(
          CvErrorCallback errorHandler,
          IntPtr userdata,
          IntPtr prevUserdata)
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
        /// </summary>
        /// <param name="errorHandler">Pointer to the new error handler</param>
        /// <param name="userdata">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
        /// <returns></returns>
        public static IntPtr RedirectError(
          IntPtr errorHandler,
          IntPtr userdata,
          IntPtr prevUserdata)
        {
            return IntPtr.Zero;
        }
#else
        /// <summary>
        /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
        /// </summary>
        /// <param name="errorHandler">The new error handler</param>
        /// <param name="userdata">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
        /// <returns>Pointer to the old error handler</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveRedirectError")]
        public static extern IntPtr RedirectError(
            CvErrorCallback errorHandler,
            IntPtr userdata,
            IntPtr prevUserdata);

        /// <summary>
        /// Sets a new error handler that can be one of standard handlers or a custom handler that has the certain interface. The handler takes the same parameters as cvError function. If the handler returns non-zero value, the program is terminated, otherwise, it continues. The error handler may check the current error mode with cvGetErrMode to make a decision.
        /// </summary>
        /// <param name="errorHandler">Pointer to the new error handler</param>
        /// <param name="userdata">Arbitrary pointer that is transparently passed to the error handler.</param>
        /// <param name="prevUserdata">Pointer to the previously assigned user data pointer.</param>
        /// <returns>Pointer to the old error handler</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveRedirectError")]
        public static extern IntPtr RedirectError(
            IntPtr errorHandler,
            IntPtr userdata,
            IntPtr prevUserdata);

        /// <summary>
        /// Sets the specified error mode.
        /// </summary>
        /// <param name="errorMode">The error mode</param>
        /// <returns>The previous error mode</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetErrMode")]
        public static extern int SetErrMode(int errorMode);

        /// <summary>
        /// Returns the current error mode
        /// </summary>
        /// <returns>The error mode</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetErrMode")]
        public static extern int GetErrMode();

        /// <summary>
        /// Returns the current error status - the value set with the last cvSetErrStatus call. Note, that in Leaf mode the program terminates immediately after error occurred, so to always get control after the function call, one should call cvSetErrMode and set Parent or Silent error mode.
        /// </summary>
        /// <returns>The current error status</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetErrStatus")]
        public static extern int GetErrStatus();

        /// <summary>
        /// Sets the error status to the specified value. Mostly, the function is used to reset the error status (set to it CV_StsOk) to recover after error. In other cases it is more natural to call cvError or CV_ERROR.
        /// </summary>
        /// <param name="code">The error status.</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetErrStatus")]
        public static extern void SetErrStatus(CvEnum.ErrorCodes code);

        /// <summary>
        /// Returns the textual description for the specified error status code. In case of unknown status the function returns NULL pointer. 
        /// </summary>
        /// <param name="status">The error status</param>
        /// <returns>the textual description for the specified error status code.</returns>
        public static String ErrorStr(int status)
        {
            var ptr = cveErrorStr(status);
            return ptr == IntPtr.Zero ? String.Empty : Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveErrorStr(int status);
#endif

        #endregion

        /// <summary>
        /// initializes CvMat header so that it points to the same data as the original array but has different shape - different number of channels, different number of rows or both
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="header">Output header to be filled</param>
        /// <param name="newCn">New number of channels. new_cn = 0 means that number of channels remains unchanged</param>
        /// <param name="newRows">New number of rows. new_rows = 0 means that number of rows remains unchanged unless it needs to be changed according to new_cn value. destination array to be changed</param>
        /// <returns>The CvMat header</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveReshape")]
        public static extern IntPtr cvReshape(
            IntPtr arr,
            IntPtr header,
            int newCn,
            int newRows);

        /// <summary>
        /// Fills the destination array with source array tiled:
        /// dst(i,j)=src(i mod rows(src), j mod cols(src))So the destination array may be as larger as well as smaller than the source array
        /// </summary>
        /// <param name="src">Source array, image or matrix</param>
        /// <param name="dst">Destination array, image or matrix</param>
        /// <param name="nx">Flag to specify how many times the src is repeated along the vertical axis.</param>
        /// <param name="ny">Flag to specify how many times the src is repeated along the horizontal axis.</param>
        public static void Repeat(IInputArray src, int ny, int nx, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveRepeat(iaSrc, ny, nx, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRepeat(IntPtr src, int ny, int nx, IntPtr dst);

        /// <summary>
        /// This function is the opposite to cvSplit. If the destination array has N channels then if the first N input channels are not IntPtr.Zero, all they are copied to the destination array, otherwise if only a single source channel of the first N is not IntPtr.Zero, this particular channel is copied into the destination array, otherwise an error is raised. Rest of source channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to insert a single channel into the image. 
        /// </summary>
        /// <param name="mv">Input vector of matrices to be merged; all the matrices in mv must have the same size and the same depth.</param>
        /// <param name="dst">output array of the same size and the same depth as mv[0]; The number of channels will be the total number of channels in the matrix array.</param>
        public static void Merge(IInputArrayOfArrays mv, IOutputArray dst)
        {
            using (InputArray iaMv = mv.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveMerge(iaMv, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMerge(IntPtr mv, IntPtr dst);

        /// <summary>
        /// The function cvMixChannels is a generalized form of cvSplit and cvMerge and some forms of cvCvtColor. It can be used to change the order of the planes, add/remove alpha channel, extract or insert a single plane or multiple planes etc.
        /// </summary>
        /// <param name="src">The array of input arrays.</param>
        /// <param name="dst">The array of output arrays</param>
        /// <param name="fromTo">The array of pairs of indices of the planes copied. from_to[k*2] is the 0-based index of the input plane, and from_to[k*2+1] is the index of the output plane, where the continuous numbering of the planes over all the input and over all the output arrays is used. When from_to[k*2] is negative, the corresponding output plane is filled with 0's.</param>
        /// <remarks>Unlike many other new-style C++ functions in OpenCV, mixChannels requires the output arrays to be pre-allocated before calling the function.</remarks>
        public static void MixChannels(
            IInputArrayOfArrays src,
            IInputOutputArray dst,
            int[] fromTo)
        {
            GCHandle handle = GCHandle.Alloc(fromTo, GCHandleType.Pinned);
            using (InputArray iaSrc = src.GetInputArray())
            using (InputOutputArray ioaDst = dst.GetInputOutputArray())
                cveMixChannels(iaSrc, ioaDst, handle.AddrOfPinnedObject(), fromTo.Length >> 1);
            handle.Free();
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMixChannels(
            IntPtr src,
            IntPtr dst,
            IntPtr fromTo,
            int npairs);

        /// <summary>
        /// Extract the specific channel from the image
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The channel</param>
        /// <param name="coi">0 based index of the channel to be extracted</param>
        public static void ExtractChannel(IInputArray src, IOutputArray dst, int coi)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveExtractChannel(iaSrc, oaDst, coi);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveExtractChannel(IntPtr src, IntPtr dst, int coi);

        /// <summary>
        /// Insert the specific channel to the image
        /// </summary>
        /// <param name="src">The source channel</param>
        /// <param name="dst">The destination image where the channel will be inserted into</param>
        /// <param name="coi">0-based index of the channel to be inserted</param>
        public static void InsertChannel(IInputArray src, IInputOutputArray dst, int coi)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputOutputArray oaDst = dst.GetInputOutputArray())
                cveInsertChannel(iaSrc, oaDst, coi);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveInsertChannel(IntPtr src, IntPtr dst, int coi);


        /// <summary>
        /// Shuffles the matrix by swapping randomly chosen pairs of the matrix elements on each iteration (where each element may contain several components in case of multi-channel arrays)
        /// </summary>
        /// <param name="mat">The input/output matrix. It is shuffled in-place. </param>
        /// <param name="rng">Pointer to MCvRNG random number generator. Use 0 if not sure</param>
        /// <param name="iterFactor">The relative parameter that characterizes intensity of the shuffling performed. The number of iterations (i.e. pairs swapped) is round(iter_factor*rows(mat)*cols(mat)), so iter_factor=0 means that no shuffling is done, iter_factor=1 means that the function swaps rows(mat)*cols(mat) random pairs etc</param>
        public static void RandShuffle(IInputOutputArray mat, double iterFactor, UInt64 rng)
        {
            using (InputOutputArray ioaMat = mat.GetInputOutputArray())
                cveRandShuffle(ioaMat, iterFactor, rng);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRandShuffle(IntPtr mat, double iterFactor, UInt64 rng);

        /// <summary>
        /// Inverses every bit of every array element:
        /// </summary>
        /// <param name="src">The source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="mask">The optional mask for the operation, use null to ignore</param>
        public static void BitwiseNot(IInputArray src, IOutputArray dst, IInputArray mask = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveBitwiseNot(iaSrc, oaDst, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBitwiseNot(IntPtr src, IntPtr dst, IntPtr mask);

        /// <summary>
        /// Calculates per-element maximum of two arrays:
        /// dst(I)=max(src1(I), src2(I))
        /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array. </param>
        /// <param name="dst">The destination array</param>
        public static void Max(IInputArray src1, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveMax(iaSrc1, iaSrc2, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMax(IntPtr src1, IntPtr src2, IntPtr dst);

        /// <summary>
        /// Returns the number of non-zero elements in arr:
        /// result = sumI arr(I)!=0
        /// In case of IplImage both ROI and COI are supported.
        /// </summary>
        /// <param name="arr">The image</param>
        /// <returns>the number of non-zero elements in image</returns>
        public static int CountNonZero(IInputArray arr)
        {
            using (InputArray iaArr = arr.GetInputArray())
                return cveCountNonZero(iaArr);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveCountNonZero(IntPtr arr);

        /// <summary>
        /// Find the location of the non-zero pixel
        /// </summary>
        /// <param name="src">The source array</param>
        /// <param name="idx">The output array where the location of the pixels are sorted</param>
        public static void FindNonZero(IInputArray src, IOutputArray idx)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaIdx = idx.GetOutputArray())
                cveFindNonZero(iaSrc, oaIdx);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFindNonZero(IntPtr src, IntPtr idx);

        /// <summary>
        /// Computes PSNR image/video quality metric
        /// </summary>
        /// <param name="src1">The first source image</param>
        /// <param name="src2">The second source image</param>
        /// <returns>the quality metric</returns>
        public static double PSNR(IInputArray src1, IInputArray src2)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
                return cvePSNR(iaSrc1, iaSrc2);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cvePSNR(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates per-element minimum of two arrays:
        /// dst(I)=min(src1(I),src2(I))
        /// All the arrays must have a single channel, the same data type and the same size (or ROI size).
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        public static void Min(IInputArray src1, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveMin(iaSrc1, iaSrc2, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMin(IntPtr src1, IntPtr src2, IntPtr dst);

        /// <summary>
        /// Adds one array to another one:
        /// dst(I)=src1(I)+src2(I) if mask(I)!=0All the arrays must have the same type, except the mask, and the same size (or ROI size)
        /// </summary>
        /// <param name="src1">The first source array.</param>
        /// <param name="src2">The second source array.</param>
        /// <param name="dst">The destination array.</param>
        /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed. </param>
        /// <param name="dtype">Optional depth type of the output array</param>
        public static void Add(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null,
            CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveAdd(iaSrc1, iaSrc2, oaDst, iaMask, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAdd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, CvEnum.DepthType dtype);

        /// <summary>
        /// Subtracts one array from another one:
        /// dst(I)=src1(I)-src2(I) if mask(I)!=0
        /// All the arrays must have the same type, except the mask, and the same size (or ROI size)
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
        /// <param name="dtype">Optional depth of the output array</param>
        public static void Subtract(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null,
            CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveSubtract(iaSrc1, iaSrc2, oaDst, iaMask, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSubtract(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask,
            CvEnum.DepthType dtype);

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
        /// <param name="dtype">Optional depth of the output array</param>
        public static void Divide(IInputArray src1, IInputArray src2, IOutputArray dst, double scale = 1.0,
            CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveDivide(iaSrc1, iaSrc2, oaDst, scale, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDivide(IntPtr src1, IntPtr src2, IntPtr dst, double scale,
            CvEnum.DepthType dtype);

        /// <summary>
        /// Calculates per-element product of two arrays:
        /// dst(I)=scale*src1(I)*src2(I)
        /// All the arrays must have the same type, and the same size (or ROI size)
        /// </summary>
        /// <param name="src1">The first source array. </param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="scale">Optional scale factor</param>
        /// <param name="dtype">Optional depth of the output array</param>
        public static void Multiply(IInputArray src1, IInputArray src2, IOutputArray dst, double scale = 1.0,
            CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveMultiply(iaSrc1, iaSrc2, oaDst, scale, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMultiply(IntPtr src1, IntPtr src2, IntPtr dst, double scale,
            CvEnum.DepthType dtype);

        /// <summary>
        /// Calculates per-element bit-wise logical conjunction of two arrays:
        /// dst(I)=src1(I) &amp; src2(I) if mask(I)!=0
        /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
        public static void BitwiseAnd(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveBitwiseAnd(iaSrc1, iaSrc2, oaDst, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBitwiseAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

        /// <summary>
        /// Calculates per-element bit-wise disjunction of two arrays:
        /// dst(I)=src1(I)|src2(I)
        /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
        public static void BitwiseOr(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveBitwiseOr(iaSrc1, iaSrc2, oaDst, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBitwiseOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

        /// <summary>
        /// Calculates per-element bit-wise logical conjunction of two arrays:
        /// dst(I)=src1(I)^src2(I) if mask(I)!=0
        /// In the case of floating-point arrays their bit representations are used for the operation. All the arrays must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="mask">Mask, 8-bit single channel array; specifies elements of destination array to be changed.</param>
        public static void BitwiseXor(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveBitwiseXor(iaSrc1, iaSrc2, oaDst, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBitwiseXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

        #region Copying and Filling

        /// <summary>
        /// Copies selected elements from input array to output array:
        /// dst(I)=src(I) if mask(I)!=0. 
        /// If any of the passed arrays is of IplImage type, then its ROI and COI fields are used. Both arrays must have the same type, the same number of dimensions and the same size. The function can also copy sparse arrays (mask is not supported in this case).
        /// </summary>
        /// <param name="src">The source array</param>
        /// <param name="des">The destination array</param>
        /// <param name="mask">Operation mask, 8-bit single channel array; specifies elements of destination array to be changed</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveCopy")]
        public static extern void cvCopy(IntPtr src, IntPtr des, IntPtr mask);

        /// <summary>
        /// Initializes scaled identity matrix:
        /// arr(i,j)=value if i=j,
        /// 0 otherwise
        /// </summary>
        /// <param name="mat">The matrix to initialize (not necessarily square).</param>
        /// <param name="value">The value to assign to the diagonal elements.</param>
        public static void SetIdentity(IInputOutputArray mat, MCvScalar value)
        {
            using (InputOutputArray ioaMat = mat.GetInputOutputArray())
                cveSetIdentity(ioaMat, ref value);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetIdentity(IntPtr mat, ref MCvScalar value);


        /// <summary>
        /// Initializes the matrix as following:
        /// arr(i,j)=(end-start)*(i*cols(arr)+j)/(cols(arr)*rows(arr))
        /// </summary>
        /// <param name="mat">The matrix to initialize. It should be single-channel 32-bit, integer or floating-point</param>
        /// <param name="start">The lower inclusive boundary of the range</param>
        /// <param name="end">The upper exclusive boundary of the range</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveRange")]
        public static extern void cvRange(IntPtr mat, double start, double end);

        #endregion

        #region Math Functions

        /// <summary>
        /// Calculates the magnitude and angle of 2D vectors.
        /// magnitude(I)=sqrt( x(I)^2+y(I)^2 ),
        /// angle(I)=atan2( y(I)/x(I) ) 
        /// The angles are calculated with accuracy about 0.3 degrees. For the point (0,0), the angle is set to 0.
        /// </summary>
        /// <param name="x">Array of x-coordinates; this must be a single-precision or double-precision floating-point array.</param>
        /// <param name="y">Array of y-coordinates, that must have the same size and same type as x.</param>
        /// <param name="magnitude">Output array of magnitudes of the same size and type as x.</param>
        /// <param name="angle">Output array of angles that has the same size and type as x; the angles are measured in radians (from 0 to 2*Pi) or in degrees (0 to 360 degrees).</param>
        /// <param name="angleInDegrees">A flag, indicating whether the angles are measured in radians (which is by default), or in degrees.</param>
        public static void CartToPolar(
            IInputArray x,
            IInputArray y,
            IOutputArray magnitude,
            IOutputArray angle,
            bool angleInDegrees = false)
        {
            using (InputArray iaX = x.GetInputArray())
            using (InputArray iaY = y.GetInputArray())
            using (OutputArray oaMagitude = magnitude.GetOutputArray())
            using (OutputArray oaAngle = angle.GetOutputArray())
                cveCartToPolar(iaX, iaY, oaMagitude, oaAngle, angleInDegrees);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCartToPolar(
            IntPtr x,
            IntPtr y,
            IntPtr magnitude,
            IntPtr angle,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool angleInDegrees);

        /// <summary>
        /// Calculates either x-coordinate, y-coordinate or both of every vector magnitude(I)* exp(angle(I)*j), j=sqrt(-1):
        /// x(I)=magnitude(I)*cos(angle(I)),
        /// y(I)=magnitude(I)*sin(angle(I))
        /// </summary>
        /// <param name="magnitude">Input floating-point array of magnitudes of 2D vectors; it can be an empty matrix (=Mat()), in this case, the function assumes that all the magnitudes are =1; if it is not empty, it must have the same size and type as angle</param>
        /// <param name="angle">input floating-point array of angles of 2D vectors.</param>
        /// <param name="x">Output array of x-coordinates of 2D vectors; it has the same size and type as angle.</param>
        /// <param name="y">Output array of y-coordinates of 2D vectors; it has the same size and type as angle.</param>
        /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians or in degrees</param>
        public static void PolarToCart(
            IInputArray magnitude,
            IInputArray angle,
            IOutputArray x,
            IOutputArray y,
            bool angleInDegrees = false)
        {
            using (InputArray iaMagnitude = magnitude.GetInputArray())
            using (InputArray iaAngle = angle.GetInputArray())
            using (OutputArray oaX = x.GetOutputArray())
            using (OutputArray oaY = y.GetOutputArray())
                cvePolarToCart(iaMagnitude, iaAngle, oaX, oaY, angleInDegrees);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePolarToCart(
            IntPtr magnitude,
            IntPtr angle,
            IntPtr x,
            IntPtr y,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool angleInDegrees);

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
        public static void Pow(IInputArray src, double power, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cvePow(iaSrc, power, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePow(IntPtr src, double power, IntPtr dst);

        /// <summary>
        /// Calculates exponent of every element of input array:
        /// dst(I)=exp(src(I))
        /// Maximum relative error is 7e-6. Currently, the function converts denormalized values to zeros on output
        /// </summary>
        /// <param name="src">The source array</param>
        /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
        public static void Exp(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveExp(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveExp(IntPtr src, IntPtr dst);

        /// <summary>
        /// Calculates natural logarithm of absolute value of every element of input array:
        /// dst(I)=log(abs(src(I))), src(I)!=0
        /// dst(I)=C,  src(I)=0
        /// Where C is large negative number (-700 in the current implementation)
        /// </summary>
        /// <param name="src">The source array</param>
        /// <param name="dst">The destination array, it should have double type or the same type as the source</param>
        public static void Log(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveLog(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLog(IntPtr src, IntPtr dst);

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
        public static int SolveCubic(IInputArray coeffs, IOutputArray roots)
        {
            using (InputArray iaCoeffs = coeffs.GetInputArray())
            using (OutputArray oaRoots = roots.GetOutputArray())
                return cveSolveCubic(iaCoeffs, oaRoots);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveSolveCubic(IntPtr coeffs, IntPtr roots);

        /// <summary>
        /// Finds all real and complex roots of any degree polynomial with real coefficients
        /// </summary>
        /// <param name="coeffs">The (degree + 1)-length array of equation coefficients (CV_32FC1 or CV_64FC1)</param>
        /// <param name="roots">The degree-length output array of real or complex roots (CV_32FC2 or CV_64FC2)</param>
        /// <param name="maxiter">The maximum number of iterations</param>
        /// <returns>The max difference.</returns>
        public static double SolvePoly(IInputArray coeffs, IOutputArray roots, int maxiter = 300)
        {
            using (InputArray iaCoeffs = coeffs.GetInputArray())
            using (OutputArray oaRoots = roots.GetOutputArray())
                return cveSolvePoly(iaCoeffs, oaRoots, maxiter);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveSolvePoly(
            IntPtr coeffs,
            IntPtr roots,
            int maxiter);

        /// <summary>
        /// Solves linear system (src1)*(dst) = (src2)
        /// </summary>
        /// <param name="src1">The source matrix in the LHS</param>
        /// <param name="src2">The source matrix in the RHS</param>
        /// <param name="dst">The result</param>
        /// <param name="method">The method for solving the equation</param>
        /// <returns>0 if src1 is a singular and CV_LU method is used</returns>
        public static int Solve(
            IInputArray src1,
            IInputArray src2,
            IOutputArray dst,
            CvEnum.DecompMethod method)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                return cveSolve(iaSrc1, iaSrc2, oaDst, method);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveSolve(
            IntPtr src1,
            IntPtr src2,
            IntPtr dst,
            CvEnum.DecompMethod method);

        /// <summary>
        /// Sorts each matrix row or each matrix column in
        /// ascending or descending order.So you should pass two operation flags to
        /// get desired behaviour.
        /// </summary>
        /// <param name="src">input single-channel array.</param>
        /// <param name="dst">output array of the same size and type as src.</param>
        /// <param name="flags">operation flags</param>
        public static void Sort(IInputArray src, IOutputArray dst, CvEnum.SortFlags flags)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveSort(iaSrc, oaDst, flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSort(IntPtr src, IntPtr dst, CvEnum.SortFlags flags);

        /// <summary>
        /// Sorts each matrix row or each matrix column in the
        /// ascending or descending order.So you should pass two operation flags to
        /// get desired behaviour. Instead of reordering the elements themselves, it
        /// stores the indices of sorted elements in the output array.
        /// </summary>
        /// <param name="src">input single-channel array.</param>
        /// <param name="dst">output integer array of the same size as src.</param>
        /// <param name="flags">operation flags</param>
        public static void SortIdx(IInputArray src, IOutputArray dst, CvEnum.SortFlags flags)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveSortIdx(iaSrc, oaDst, flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSortIdx(IntPtr src, IntPtr dst, CvEnum.SortFlags flags);

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
        public static void Dft(
            IInputArray src,
            IOutputArray dst,
            CvEnum.DxtType flags,
            int nonzeroRows)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveDft(iaSrc, oaDst, flags, nonzeroRows);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDft(
            IntPtr src,
            IntPtr dst,
            CvEnum.DxtType flags,
            int nonzeroRows);

        /// <summary>
        /// Returns the minimum number N that is greater to equal to size0, such that DFT of a vector of size N can be computed fast. In the current implementation N=2^p x 3^q x 5^r for some p, q, r. 
        /// </summary>
        /// <param name="vecsize">Vector size</param>
        /// <returns>The minimum number N that is greater to equal to size0, such that DFT of a vector of size N can be computed fast. In the current implementation N=2^p x 3^q x 5^r for some p, q, r. </returns>
        public static int GetOptimalDFTSize(int vecsize)
        {
            return cveGetOptimalDFTSize(vecsize);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveGetOptimalDFTSize(int vecsize);

        /// <summary>
        /// Performs per-element multiplication of the two CCS-packed or complex matrices that are results of real or complex Fourier transform. 
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array of the same type and the same size of the sources</param>
        /// <param name="flags">Operation flags; currently, the only supported flag is DFT_ROWS, which indicates that each row of src1 and src2 is an independent 1D Fourier spectrum.</param>
        /// <param name="conjB">Optional flag that conjugates the second input array before the multiplication (true) or not (false).</param>
        public static void MulSpectrums(IInputArray src1, IInputArray src2, IOutputArray dst,
            CvEnum.MulSpectrumsType flags, bool conjB = false)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveMulSpectrums(iaSrc1, iaSrc2, oaDst, flags, conjB);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMulSpectrums(
            IntPtr src1, IntPtr src2, IntPtr dst, CvEnum.MulSpectrumsType flags,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool conjB);

        /// <summary>
        /// Performs forward or inverse transform of 1D or 2D floating-point array
        /// </summary>
        /// <param name="src">Source array, real 1D or 2D array</param>
        /// <param name="dst">Destination array of the same size and same type as the source</param>
        /// <param name="flags">Transformation flags</param>
        public static void Dct(IInputArray src, IOutputArray dst, CvEnum.DctType flags)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveDct(iaSrc, oaDst, flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDct(IntPtr src, IntPtr dst, CvEnum.DctType flags);

        #endregion

        /// <summary>
        /// Calculates a part of the line segment which is entirely in the rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="pt1">First ending point of the line segment. It is modified by the function</param>
        /// <param name="pt2">Second ending point of the line segment. It is modified by the function.</param>
        /// <returns>It returns false if the line segment is completely outside the rectangle and true otherwise.</returns>

        public static bool ClipLine(Rectangle rectangle, ref Point pt1, ref Point pt2)
        {
            return cveClipLine(ref rectangle, ref pt1, ref pt2);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveClipLine(ref Rectangle rect, ref Point pt1, ref Point pt2);

        /// <summary>
        /// Calculates absolute difference between two arrays.
        /// dst(I)c = abs(src1(I)c - src2(I)c).
        /// All the arrays must have the same data type and the same size (or ROI size)
        /// </summary>
        /// <param name="src1">The first source array</param>
        /// <param name="src2">The second source array</param>
        /// <param name="dst">The destination array</param>
        public static void AbsDiff(IInputArray src1, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveAbsDiff(iaSrc1, iaSrc2, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAbsDiff(IntPtr src1, IntPtr src2, IntPtr dst);


        /// <summary>
        /// Calculates the sum of a scaled array and another array.
        /// </summary>
        /// <param name="src1">First input array</param>
        /// <param name="alpha">Scale factor for the first array</param>
        /// <param name="src2">Second input array of the same size and type as src1</param>
        /// <param name="dst">Output array of the same size and type as src1</param>
        public static void ScaleAdd(IInputArray src1, double alpha, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveScaleAdd(iaSrc1, alpha, iaSrc2, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveScaleAdd(IntPtr src1, double alpha, IntPtr src2, IntPtr dst);

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
        /// <param name="dtype">Optional depth of the output array; when both input arrays have the same depth</param>
        public static void AddWeighted(IInputArray src1, double alpha, IInputArray src2, double beta, double gamma,
            IOutputArray dst, CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveAddWeighted(iaSrc1, alpha, iaSrc2, beta, gamma, oaDst, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma,
            IntPtr dst, CvEnum.DepthType dtype);

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
        public static void InRange(
            IInputArray src,
            IInputArray lower,
            IInputArray upper,
            IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaLower = lower.GetInputArray())
            using (InputArray iaUpper = upper.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveInRange(iaSrc, iaLower, iaUpper, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveInRange(
            IntPtr src,
            IntPtr lower,
            IntPtr upper,
            IntPtr dst);

        /// <summary>
        /// Returns the calculated norm. The multiple-channel array are treated as single-channel, that is, the results for all channels are combined. 
        /// </summary>
        /// <param name="arr1">The first source image</param>
        /// <param name="arr2">The second source image. If it is null, the absolute norm of arr1 is calculated, otherwise absolute or relative norm of arr1-arr2 is calculated</param>
        /// <param name="normType">Type of norm</param>
        /// <param name="mask">The optional operation mask</param>
        /// <returns>The calculated norm</returns>
        public static double Norm(IInputArray arr1, IInputOutputArray arr2,
            CvEnum.NormType normType = CvEnum.NormType.L2, IInputArray mask = null)
        {
            using (InputArray iaArr1 = arr1.GetInputArray())
            using (InputOutputArray iaArr2 = arr2 == null ? InputOutputArray.GetEmpty() : arr2.GetInputOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                return cveNorm(iaArr1, iaArr2, normType, iaMask);
        }

        /// <summary>
        /// Returns the calculated norm. The multiple-channel array are treated as single-channel, that is, the results for all channels are combined. 
        /// </summary>
        /// <param name="arr1">The first source image</param>
        /// <param name="normType">Type of norm</param>
        /// <param name="mask">The optional operation mask</param>
        /// <returns>The calculated norm</returns>
        public static double Norm(IInputArray arr1, CvEnum.NormType normType = CvEnum.NormType.L2,
            IInputArray mask = null)
        {
            return Norm(arr1, null, normType, mask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveNorm(
            IntPtr arr1,
            IntPtr arr2,
            Emgu.CV.CvEnum.NormType normType,
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
        public static IntPtr cvCreateImage(Size size, CvEnum.IplDepth depth, int channels)
        {
            return cveCreateImageHeader(ref size, depth, channels);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveCreateImage(
            ref Size size,
            CvEnum.IplDepth depth,
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
        public static IntPtr cvCreateImageHeader(
            Size size,
            CvEnum.IplDepth depth,
            int channels)
        {
            return cveCreateImageHeader(ref size, depth, channels);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveCreateImageHeader(
            ref Size size,
            CvEnum.IplDepth depth,
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
        /// <returns>Pointer to the image header</returns>
        public static IntPtr cvInitImageHeader(
            IntPtr image,
            Size size,
            CvEnum.IplDepth depth,
            int channels,
            int origin,
            int align)
        {
            return cveInitImageHeader(image, ref size, depth, channels, origin, align);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveInitImageHeader(
            IntPtr image,
            ref Size size,
            CvEnum.IplDepth depth,
            int channels,
            int origin,
            int align);

        /// <summary>
        /// Assigns user data to the array header.
        /// </summary>
        /// <param name="arr">Array header.</param>
        /// <param name="data">User data.</param>
        /// <param name="step">Full row length in bytes.</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetData")]
        public static extern void cvSetData(IntPtr arr, IntPtr data, int step);

        /// <summary>
        /// Releases the header.
        /// </summary>
        /// <param name="image">Pointer to the deallocated header.</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention,
            EntryPoint = "cveReleaseImageHeader")]
        public static extern void cvReleaseImageHeader(ref IntPtr image);

        /*
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
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern IntPtr cvInitMatHeader(
           IntPtr mat,
           int rows,
           int cols,
           CV.CvEnum.DepthType type,
           IntPtr data,
           int step);
  */

        /// <summary>
        /// Initializes already allocated CvMat structure. It can be used to process raw data with OpenCV matrix functions.
        /// </summary>
        /// <param name="mat">Pointer to the matrix header to be initialized.</param>
        /// <param name="rows">Number of rows in the matrix.</param>
        /// <param name="cols">Number of columns in the matrix.</param>
        /// <param name="type">Type of the matrix elements.</param>
        /// <param name="data">Optional data pointer assigned to the matrix header</param>
        /// <param name="step">Full row width in bytes of the data assigned. By default, the minimal possible step is used, i.e., no gaps is assumed between subsequent rows of the matrix.</param>
        /// <returns>Pointer to the CvMat</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveInitMatHeader")]
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
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetImageCOI")]
        public static extern void cvSetImageCOI(IntPtr image, int coi);

        /// <summary>
        /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
        /// </summary>
        /// <param name="image">Image header. </param>
        /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetImageCOI")]
        public static extern int cvGetImageCOI(IntPtr image);

        /// <summary>
        /// Releases image ROI. After that the whole image is considered selected.
        /// </summary>
        /// <param name="image">Image header</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveResetImageROI")]
        public static extern void cvResetImageROI(IntPtr image);

        /// <summary>
        /// Sets the image ROI to a given rectangle. If ROI is NULL and the value of the parameter rect is not equal to the whole image, ROI is allocated. 
        /// </summary>
        /// <param name="image">Image header.</param>
        /// <param name="rect">ROI rectangle.</param>
        public static void cvSetImageROI(IntPtr image, Rectangle rect)
        {
            cveSetImageROI(image, ref rect);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetImageROI(IntPtr image, ref Rectangle rect);


        /// <summary>
        /// Returns channel of interest of the image (it returns 0 if all the channels are selected).
        /// </summary>
        /// <param name="image">Image header.</param>
        /// <returns>channel of interest of the image (it returns 0 if all the channels are selected)</returns>
        public static Rectangle cvGetImageROI(IntPtr image)
        {
            Rectangle rect = new Rectangle();
            cveGetImageROI(image, ref rect);
            return rect;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetImageROI(IntPtr image, ref Rectangle rect);

        /// <summary>
        /// Allocates header for the new matrix and underlying data, and returns a pointer to the created matrix. Matrices are stored row by row. All the rows are aligned by 4 bytes. 
        /// </summary>
        /// <param name="rows">Number of rows in the matrix.</param>
        /// <param name="cols">Number of columns in the matrix.</param>
        /// <param name="type">Type of the matrix elements.</param>
        /// <returns>A pointer to the created matrix</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveCreateMat")]
        public static extern IntPtr cvCreateMat(int rows, int cols, CvEnum.DepthType type);

        /// <summary>
        /// Initializes CvMatND structure allocated by the user
        /// </summary>
        /// <param name="mat">Pointer to the array header to be initialized</param>
        /// <param name="dims">Number of array dimensions</param>
        /// <param name="sizes">Array of dimension sizes</param>
        /// <param name="type">Type of array elements</param>
        /// <param name="data">Optional data pointer assigned to the matrix header</param>
        /// <returns>Pointer to the array header</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveInitMatNDHeader")]
        public static extern IntPtr cvInitMatNDHeader(
            IntPtr mat,
            int dims,
            [In] int[] sizes,
            CV.CvEnum.DepthType type,
            IntPtr data);

        /// <summary>
        /// Decrements the matrix data reference counter and releases matrix header
        /// </summary>
        /// <param name="mat">Double pointer to the matrix.</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveReleaseMat")]
        public static extern void cvReleaseMat(ref IntPtr mat);

        /// <summary>
        /// The function allocates a multi-dimensional sparse array. Initially the array contain no elements, that is Get or GetReal returns zero for every index
        /// </summary>
        /// <param name="dims">Number of array dimensions</param>
        /// <param name="sizes">Array of dimension sizes</param>
        /// <param name="type">Type of array elements</param>
        /// <returns>Pointer to the array header</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveCreateSparseMat")]
        public static extern IntPtr cvCreateSparseMat(
            int dims,
            IntPtr sizes,
            CV.CvEnum.DepthType type);

        /// <summary>
        /// The function releases the sparse array and clears the array pointer upon exit.
        /// </summary>
        /// <param name="mat">Reference of the pointer to the array</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveReleaseSparseMat")]
        public static extern void cvReleaseSparseMat(ref IntPtr mat);

        #endregion

        /// <summary>
        /// Assign the new value to the particular element of single-channel array
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <param name="value">The assigned value </param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetReal1D")]
        public static extern void cvSetReal1D(IntPtr arr, int idx0, double value);

        /// <summary>
        /// Assign the new value to the particular element of single-channel array
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <param name="idx1">The second zero-based component of the element index </param>
        /// <param name="value">The assigned value </param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetReal2D")]
        public static extern void cvSetReal2D(IntPtr arr, int idx0, int idx1, double value);

        /// <summary>
        /// Assign the new value to the particular element of single-channel array
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <param name="idx1">The second zero-based component of the element index </param>
        /// <param name="idx2">The third zero-based component of the element index </param>
        /// <param name="value">The assigned value </param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetReal3D")]
        public static extern void cvSetReal3D(IntPtr arr, int idx0, int idx1, int idx2, double value);

        /// <summary>
        /// Assign the new value to the particular element of single-channel array
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="idx">Array of the element indices </param>
        /// <param name="value">The assigned value </param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveSetRealND")]
        public static extern void cvSetRealND(
            IntPtr arr,
            [In] int[] idx,
            double value);

        /// <summary>
        /// Clears (sets to zero) the particular element of dense array or deletes the element of sparse array. If the element does not exists, the function does nothing
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="idx">Array of the element indices </param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveClearND")]
        public static extern void cvClearND(
            IntPtr arr,
            [In] int[] idx);

        /// <summary>
        /// Assign the new value to the particular element of array
        /// </summary>
        /// <param name="arr">Input array. </param>
        /// <param name="idx0">The first zero-based component of the element index</param>
        /// <param name="idx1">The second zero-based component of the element index</param>
        /// <param name="value">The assigned value</param>
        public static void cvSet2D(IntPtr arr, int idx0, int idx1, MCvScalar value)
        {
            cveSet2D(arr, idx0, idx1, ref value);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSet2D(IntPtr arr, int idx0, int idx1, ref MCvScalar value);

        /// <summary>
        /// Flips the array in one of different 3 ways (row and column indices are 0-based)
        /// </summary>
        /// <param name="src">Source array.</param>
        /// <param name="dst">Destination array.</param>
        /// <param name="flipType">Specifies how to flip the array.</param>
        public static void Flip(IInputArray src, IOutputArray dst, CvEnum.FlipType flipType)
        {
            int flipMode =
                //-1 indicates vertical and horizontal flip
                flipType == (Emgu.CV.CvEnum.FlipType.Horizontal | Emgu.CV.CvEnum.FlipType.Vertical) ? -1 :
                //1 indicates horizontal flip only
                flipType == Emgu.CV.CvEnum.FlipType.Horizontal ? 1 :
                //0 indicates vertical flip only
                0;
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveFlip(iaSrc, oaDst, flipMode);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFlip(IntPtr src, IntPtr dst, int flipMode);

        /// <summary>
        /// Rotates a 2D array in multiples of 90 degrees.
        /// </summary>
        /// <param name="src">input array.</param>
        /// <param name="dst">output array of the same type as src.  The size is the same with ROTATE_180, and the rows and cols are switched for ROTATE_90 and ROTATE_270.</param>
        /// <param name="rotateCode">an enum to specify how to rotate the array</param>
        public static void Rotate(IInputArray src, IOutputArray dst, RotateFlags rotateCode)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveRotate(iaSrc, oaDst, rotateCode);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRotate(IntPtr src, IntPtr dst, RotateFlags rotateCode);

        #region Accessing Elements and sub-Arrays

        /// <summary>
        /// Returns header, corresponding to a specified rectangle of the input array. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array. ROI is taken into account by the function so the sub-array of ROI is actually extracted.
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="submat">Pointer to the resultant sub-array header.</param>
        /// <param name="rect">Zero-based coordinates of the rectangle of interest.</param>
        /// <returns>the resultant sub-array header</returns>
        public static IntPtr cvGetSubRect(IntPtr arr, IntPtr submat, Rectangle rect)
        {
            return cveGetSubRect(arr, submat, ref rect);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGetSubRect(IntPtr arr, IntPtr submat, ref Rectangle rect);


        /// <summary>
        /// Return the header, corresponding to a specified row span of the input array
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="submat">Pointer to the prelocated memory of resulting sub-array header</param>
        /// <param name="startRow">Zero-based index of the starting row (inclusive) of the span</param>
        /// <param name="endRow">Zero-based index of the ending row (exclusive) of the span</param>
        /// <param name="deltaRow">Index step in the row span. That is, the function extracts every delta_row-th row from start_row and up to (but not including) end_row</param>
        /// <returns>The header, corresponding to a specified row span of the input array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetRows")]
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
        /// <param name="submat">Pointer to the prelocated memory of the resulting sub-array header</param>
        /// <param name="startCol">Zero-based index of the selected column</param>
        /// <param name="endCol">Zero-based index of the ending column (exclusive) of the span</param>
        /// <returns>The header, corresponding to a specified col span of the input array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetCols")]
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
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetDiag")]
        public static extern IntPtr cvGetDiag(IntPtr arr, IntPtr submat, int diag);

        /// <summary>
        /// Returns number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.
        /// </summary>
        /// <param name="arr">array header</param>
        /// <returns>number of rows (CvSize::height) and number of columns (CvSize::width) of the input matrix or image. In case of image the size of ROI is returned.</returns>
        public static Size cvGetSize(IntPtr arr)
        {
            int width = 0, height = 0;
            cveGetSize(arr, ref width, ref height);
            return new Size(width, height);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetSize(IntPtr arr, ref int width, ref int height);

        /// <summary>
        /// Draws a simple or filled circle with given center and radius. The circle is clipped by ROI rectangle.
        /// </summary>
        /// <param name="img">Image where the circle is drawn</param>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="color">Color of the circle</param>
        /// <param name="thickness">Thickness of the circle outline if positive, otherwise indicates that a filled circle has to be drawn</param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public static void Circle(IInputOutputArray img, Point center, int radius, MCvScalar color, int thickness = 1,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
                cveCircle(ioaImg, ref center, radius, ref color, thickness, lineType, shift);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCircle(
            IntPtr img,
            ref Point center,
            int radius,
            ref MCvScalar color,
            int thickness,
            CvEnum.LineType lineType,
            int shift);

        /// <summary>
        /// Divides a multi-channel array into separate single-channel arrays. Two modes are available for the operation. If the source array has N channels then if the first N destination channels are not IntPtr.Zero, all they are extracted from the source array, otherwise if only a single destination channel of the first N is not IntPtr.Zero, this particular channel is extracted, otherwise an error is raised. Rest of destination channels (beyond the first N) must always be IntPtr.Zero. For IplImage cvCopy with COI set can be also used to extract a single channel from the image
        /// </summary>
        /// <param name="src">Input multi-channel array</param>
        /// <param name="mv">Output array or vector of arrays</param>
        public static void Split(IInputArray src, IOutputArray mv)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaMv = mv.GetOutputArray())
                cveSplit(iaSrc, oaMv);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSplit(IntPtr src, IntPtr mv);

        /*
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
        }*/

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
        public static void Ellipse(IInputOutputArray img, Point center, Size axes, double angle, double startAngle,
            double endAngle, MCvScalar color, int thickness = 1,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
                cveEllipse(ioaImg, ref center, ref axes, angle, startAngle, endAngle, ref color, thickness, lineType,
                    shift);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveEllipse(IntPtr img, ref Point center, ref Size axes,
            double angle, double startAngle, double endAngle,
            ref MCvScalar color, int thickness, CvEnum.LineType lineType, int shift);


        /// <summary>
        /// Draws a simple or thick elliptic arc or fills an ellipse sector. The arc is clipped by ROI rectangle. A piecewise-linear approximation is used for antialiased arcs and thick arcs. All the angles are given in degrees.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="box">The box the define the ellipse area</param>
        /// <param name="color">Ellipse color</param>
        /// <param name="thickness">Thickness of the ellipse arc</param>
        /// <param name="lineType">Type of the ellipse boundary</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and axes' values</param>
        public static void Ellipse(
            IInputOutputArray img,
            RotatedRect box,
            MCvScalar color,
            int thickness = 1,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected,
            int shift = 0)
        {
            Size axes = new Size();
            axes.Width = (int)Math.Round(box.Size.Height * 0.5);
            axes.Height = (int)Math.Round(box.Size.Width * 0.5);

            Ellipse(img, Point.Round(box.Center), axes, box.Angle, 0, 360, color, thickness, lineType, shift);
        }

        /// <summary>
        /// Draws a marker on a predefined position in an image.
        /// </summary>
        /// <param name="img">Image.</param>
        /// <param name="position">The point where the crosshair is positioned.</param>
        /// <param name="color">Line color.</param>
        /// <param name="markerType">The specific type of marker you want to use</param>
        /// <param name="markerSize">The length of the marker axis [default = 20 pixels]</param>
        /// <param name="thickness">Line thickness.</param>
        /// <param name="lineType">Type of the line</param>
        public static void DrawMarker(
            IInputOutputArray img,
            Point position,
            MCvScalar color,
            CvEnum.MarkerTypes markerType,
            int markerSize = 20,
            int thickness = 1,
            CvEnum.LineType lineType = LineType.EightConnected)
        {
            using (InputOutputArray ioImg = img.GetInputOutputArray())
            {
                cveDrawMarker(ioImg, ref position, ref color, markerType, markerSize, thickness, lineType);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDrawMarker(
            IntPtr img,
            ref Point position,
            ref MCvScalar color,
            CvEnum.MarkerTypes markerType,
            int markerSize,
            int thickness,
            CvEnum.LineType lineType);

        /// <summary>
        /// Fills the destination array with values from the look-up table. Indices of the entries are taken from the source array. That is, the function processes each element of src as following:
        /// dst(I)=lut[src(I)+DELTA]
        /// where DELTA=0 if src has depth CV_8U, and DELTA=128 if src has depth CV_8S
        /// </summary>
        /// <param name="src">Source array of 8-bit elements</param>
        /// <param name="dst">Destination array of arbitrary depth and of the same number of channels as the source array</param>
        /// <param name="lut">Look-up table of 256 elements; should have the same depth as the destination array. In case of multi-channel source and destination arrays, the table should either have a single-channel (in this case the same table is used for all channels), or the same number of channels as the source/destination array</param>
        public static void LUT(IInputArray src, IInputArray lut, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaLut = lut.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveLUT(iaSrc, iaLut, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLUT(IntPtr src, IntPtr lut, IntPtr dst);

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
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveConvertScale")]
        public static extern void cvConvertScale(IntPtr src, IntPtr dst, double scale, double shift);

        /*
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
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvConvertScale")]
        public static extern void cvCvtScale(IntPtr src, IntPtr dst, double scale, double shift);

        /// <summary>
        /// Same as cvConvertScale(src, dest, 1, 0);
        /// </summary>
        /// <param name="src">Source array</param>
        /// <param name="dest">Destination array</param>
        public static void cvConvert(IntPtr src, IntPtr dest)
        {
           cvConvertScale(src, dest, 1, 0);
        }*/

        /// <summary>
        /// Similar to cvCvtScale but it stores absolute values of the conversion results:
        /// dst(I)=abs(src(I)*scale + (shift,shift,...))
        /// The function supports only destination arrays of 8u (8-bit unsigned integers) type, for other types the function can be emulated by combination of cvConvertScale and cvAbs functions.
        /// </summary>
        /// <param name="src">Source array</param>
        /// <param name="dst">Destination array (should have 8u depth). </param>
        /// <param name="scale">ScaleAbs factor</param>
        /// <param name="shift">Value added to the scaled source array elements</param>
        public static void ConvertScaleAbs(IInputArray src, IOutputArray dst, double scale, double shift)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveConvertScaleAbs(iaSrc, oaDst, scale, shift);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveConvertScaleAbs(IntPtr src, IntPtr dst, double scale, double shift);

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
        public static MCvScalar Mean(IInputArray arr, IInputArray mask = null)
        {
            MCvScalar result = new MCvScalar();
            using (InputArray iaArr = arr.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveMean(iaArr, iaMask, ref result);
            return result;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMean(IntPtr src, IntPtr mask, ref MCvScalar result);

        /// <summary>
        /// The function cvAvgSdv calculates the average value and standard deviation of array elements, independently for each channel
        /// </summary>
        /// <remarks>If the array is IplImage and COI is set, the function processes the selected channel only and stores the average and standard deviation to the first compoenents of output scalars (M0 and S0).</remarks>
        /// <param name="arr">The array</param>
        /// <param name="mean">Pointer to the mean value</param>
        /// <param name="stdDev">Pointer to the standard deviation</param>
        /// <param name="mask">The optional operation mask</param>
        public static void MeanStdDev(IInputArray arr, ref MCvScalar mean, ref MCvScalar stdDev,
            IInputArray mask = null)
        {
            using (VectorOfDouble meanVec = new VectorOfDouble(4))
            using (VectorOfDouble stdDevVec = new VectorOfDouble(4))
            {
                MeanStdDev(arr, meanVec, stdDevVec, mask);
                double[] meanVal = meanVec.ToArray();
                double[] stdVal = stdDevVec.ToArray();
                mean.V0 = meanVal[0];
                stdDev.V0 = stdVal[0];

                if (meanVal.Length > 1)
                {
                    mean.V1 = meanVal[1];
                    stdDev.V1 = stdVal[1];
                }

                if (meanVal.Length > 2)
                {
                    mean.V2 = meanVal[2];
                    stdDev.V2 = stdVal[2];
                }

                if (meanVal.Length > 3)
                {
                    mean.V3 = meanVal[3];
                    stdDev.V3 = stdVal[3];
                }
            }
        }

        /// <summary>
        /// Calculates a mean and standard deviation of array elements.
        /// </summary>
        /// <param name="arr">Input array that should have from 1 to 4 channels so that the results can be stored in MCvScalar</param>
        /// <param name="mean">Calculated mean value</param>
        /// <param name="stdDev">Calculated standard deviation</param>
        /// <param name="mask">Optional operation mask</param>
        public static void MeanStdDev(IInputArray arr, IOutputArray mean, IOutputArray stdDev, IInputArray mask = null)
        {
            using (InputArray iaArr = arr.GetInputArray())
            using (OutputArray oaMean = mean.GetOutputArray())
            using (OutputArray oaStdDev = stdDev.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveMeanStdDev(iaArr, oaMean, oaStdDev, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMeanStdDev(IntPtr arr, IntPtr mean, IntPtr stdDev, IntPtr mask);

        /// <summary>
        /// Calculates sum S of array elements, independently for each channel
        /// Sc = sumI arr(I)c
        /// If the array is IplImage and COI is set, the function processes the selected channel only and stores the sum to the first scalar component (S0).
        /// </summary>
        /// <param name="src">The array</param>
        /// <returns>The sum of array elements</returns>
        public static MCvScalar Sum(IInputArray src)
        {
            MCvScalar result = new MCvScalar();
            using (InputArray iaSrc = src.GetInputArray())
                cveSum(iaSrc, ref result);
            return result;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSum(IntPtr src, ref MCvScalar result);

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
        /// <param name="dtype">Optional depth type of the output array</param>
        public static void Reduce(IInputArray src, IOutputArray dst,
            CvEnum.ReduceDimension dim = CvEnum.ReduceDimension.Auto,
            CvEnum.ReduceType type = CvEnum.ReduceType.ReduceSum, CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveReduce(iaSrc, oaDst, dim, type, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveReduce(IntPtr src, IntPtr dst, CvEnum.ReduceDimension dim, CvEnum.ReduceType type,
            CvEnum.DepthType dtype);

        #endregion

        /// <summary>
        /// Releases the header and the image data.
        /// </summary>
        /// <param name="image">Double pointer to the header of the deallocated image</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveReleaseImage")]
        public static extern void cvReleaseImage(ref IntPtr image);

        /// <summary>
        /// Draws contours outlines or filled contours.
        /// </summary>
        /// <param name="image">Image where the contours are to be drawn. Like in any other drawing function, the contours are clipped with the ROI</param>
        /// <param name="contours">All the input contours. Each contour is stored as a point vector.</param>
        /// <param name="contourIdx">Parameter indicating a contour to draw. If it is negative, all the contours are drawn.</param>
        /// <param name="color">Color of the contours </param>
        /// <param name="maxLevel">Maximal level for drawn contours. If 0, only contour is drawn. If 1, the contour and all contours after it on the same level are drawn. If 2, all contours after and all contours one level below the contours are drawn, etc. If the value is negative, the function does not draw the contours following after contour but draws child contours of contour up to abs(maxLevel)-1 level. </param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative the contour interiors are drawn</param>
        /// <param name="lineType">Type of the contour segments</param>
        /// <param name="hierarchy">Optional information about hierarchy. It is only needed if you want to draw only some of the contours</param>
        /// <param name="offset">Shift all the point coordinates by the specified value. It is useful in case if the contours retrieved in some image ROI and then the ROI offset needs to be taken into account during the rendering. </param>
        public static void DrawContours(
            IInputOutputArray image,
            IInputArrayOfArrays contours,
            int contourIdx,
            MCvScalar color,
            int thickness = 1,
            CvEnum.LineType lineType = LineType.EightConnected,
            IInputArray hierarchy = null,
            int maxLevel = int.MaxValue,
            Point offset = new Point())
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaContours = contours.GetInputArray())
            using (InputArray iaHierarchy = hierarchy == null ? InputArray.GetEmpty() : hierarchy.GetInputArray())
                cveDrawContours(
                    ioaImage,
                    iaContours,
                    contourIdx,
                    ref color,
                    thickness,
                    lineType,
                    iaHierarchy,
                    maxLevel,
                    ref offset);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDrawContours(
            IntPtr image,
            IntPtr contour,
            int coutourIdx,
            ref MCvScalar color,
            int thickness,
            CvEnum.LineType lineType,
            IntPtr hierarchy,
            int maxLevel,
            ref Point offset);

        /// <summary>
        /// Fills convex polygon interior. This function is much faster than The function cvFillPoly and can fill not only the convex polygons but any monotonic polygon, i.e. a polygon whose contour intersects every horizontal line (scan line) twice at the most
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="points">Array of pointers to a single polygon</param>
        /// <param name="color">Polygon color</param>
        /// <param name="lineType">Type of the polygon boundaries</param>
        /// <param name="shift">Number of fractional bits in the vertex coordinates</param>
        public static void FillConvexPoly(IInputOutputArray img, IInputArray points, MCvScalar color,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            using (InputArray iaPoints = points.GetInputArray())
                cveFillConvexPoly(ioaImg, iaPoints, ref color, lineType, shift);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFillConvexPoly(
            IntPtr img,
            IntPtr pts,
            ref MCvScalar color,
            CvEnum.LineType lineType,
            int shift);

        /// <summary>
        /// Fills the area bounded by one or more polygons.
        /// </summary>
        /// <param name="img">Image.</param>
        /// <param name="points">Array of polygons where each polygon is represented as an array of points.</param>
        /// <param name="color">Polygon color</param>
        /// <param name="lineType">Type of the polygon boundaries.</param>
        /// <param name="shift">Number of fractional bits in the vertex coordinates.</param>
        /// <param name="offset">Optional offset of all points of the contours.</param>
        public static void FillPoly(IInputOutputArray img, IInputArray points, MCvScalar color,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0, Point offset = new Point())
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            using (InputArray iaPoints = points.GetInputArray())
                cveFillPoly(ioaImg, iaPoints, ref color, lineType, shift, ref offset);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFillPoly(
            IntPtr img,
            IntPtr pts,
            ref MCvScalar color,
            CvEnum.LineType lineType,
            int shift,
            ref Point offset);

        #region Text

        /// <summary>
        /// Renders the text in the image with the specified font and color. The printed text is clipped by ROI rectangle. Symbols that do not belong to the specified font are replaced with the rectangle symbol.
        /// </summary>
        /// <param name="img">Input image</param>
        /// <param name="text">String to print</param>
        /// <param name="org">Coordinates of the bottom-left corner of the first letter</param>
        /// <param name="fontFace">Font type.</param>
        /// <param name="fontScale">Font scale factor that is multiplied by the font-specific base size.</param>
        /// <param name="color">Text color</param>
        /// <param name="thickness">Thickness of the lines used to draw a text.</param>
        /// <param name="lineType">Line type</param>
        /// <param name="bottomLeftOrigin">When true, the image data origin is at the bottom-left corner. Otherwise, it is at the top-left corner.</param>
        public static void PutText(
            IInputOutputArray img, String text, Point org, CvEnum.FontFace fontFace, double fontScale,
            MCvScalar color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected,
            bool bottomLeftOrigin = false)
        {
            using (CvString s = new CvString(text))
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
                cvePutText(ioaImg, s, ref org, fontFace, fontScale, ref color, thickness, lineType, bottomLeftOrigin);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePutText(
            IntPtr img,
            IntPtr text,
            ref Point org, CvEnum.FontFace fontFace, double fontScale,
            ref MCvScalar color, int thickness, CvEnum.LineType lineType,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool bottomLeftOrigin);

        /// <summary>
        /// Calculates the width and height of a text string.
        /// </summary>
        /// <param name="text">Input text string.</param>
        /// <param name="fontFace">Font to use</param>
        /// <param name="fontScale">Font scale factor that is multiplied by the font-specific base size.</param>
        /// <param name="thickness">Thickness of lines used to render the text. </param>
        /// <param name="baseLine">Y-coordinate of the baseline relative to the bottom-most text point.</param>
        /// <returns>The size of a box that contains the specified text.</returns>
        public static Size GetTextSize(String text, CvEnum.FontFace fontFace, double fontScale, int thickness,
            ref int baseLine)
        {
            Size s = new Size();
            using (CvString textStr = new CvString(text))
                cveGetTextSize(textStr, fontFace, fontScale, thickness, ref baseLine, ref s);
            return s;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetTextSize(IntPtr text, CvEnum.FontFace fontFace, double fontScale,
            int thickness, ref int baseLine, ref Size size);

        #endregion

        /*
        /// <summary>
        /// Copies the entire sequence or subsequence to the specified buffer and returns the pointer to the buffer
        /// </summary>
        /// <param name="seq">Sequence</param>
        /// <param name="elements">Pointer to the destination array that must be large enough. It should be a pointer to data, not a matrix header</param>
        /// <param name="slice">The sequence part to copy to the array</param>
        /// <returns>the pointer to the buffer</returns>
  #if ANDROID
        public static IntPtr cvCvtSeqToArray(IntPtr seq, IntPtr elements, MCvSlice slice)
        {
           return cvCvtSeqToArray(seq, elements, slice.start_index, slice.end_index);
        }

        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cvCvtSeqToArray(IntPtr seq, IntPtr elements, int startIndex, int endIndex);
  #else
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern IntPtr cvCvtSeqToArray(IntPtr seq, IntPtr elements, MCvSlice slice);
  #endif

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
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern IntPtr cvMakeSeqHeaderForArray(
           int seqType,
           int headerSize,
           int elemSize,
           IntPtr elements,
           int total,
           IntPtr seq,
           IntPtr block);
        */
        internal static void MinMax(IInputArray arr, out double[] minValues, out double[] maxValues,
            out Point[] minLocations, out Point[] maxLocations)
        {
            using (InputArray iaArr = arr.GetInputArray())
            {
                int numberOfChannels = iaArr.GetChannels();
                minValues = new double[numberOfChannels];
                maxValues = new double[numberOfChannels];
                minLocations = new Point[numberOfChannels];
                maxLocations = new Point[numberOfChannels];

                double minVal = 0, maxVal = 0;
                Point minLoc = new Point(), maxLoc = new Point();
                //int[] minIdx = new int[2], maxIdx = new int[2];
                if (numberOfChannels == 1)
                {
                    if (iaArr.IsUMat)
                    {
                        //Open CV's MinMaxLoc seems to have a bug in the OpenCL implementation. 
                        //Converting UMat to Mat to force execution on CPU
                        //TODO: Remove this UMat case handling in the future if the MinMaxLoc implementation for UMat is fixed.
                        using (Mat m = iaArr.GetMat())
                        {
                            CvInvoke.MinMaxLoc(m, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                        }

                    }
                    else
                        CvInvoke.MinMaxLoc(arr, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                    minValues[0] = minVal;
                    maxValues[0] = maxVal;
                    minLocations[0] = minLoc;
                    maxLocations[0] = maxLoc;
                }
                else
                {
                    using (Mat channel = new Mat())
                        for (int i = 0; i < numberOfChannels; i++)
                        {
                            CvInvoke.ExtractChannel(arr, channel, i);
                            CvInvoke.MinMaxLoc(channel, ref minVal, ref maxVal, ref minLoc, ref maxLoc, null);
                            minValues[i] = minVal;
                            maxValues[i] = maxVal;
                            minLocations[i] = minLoc;
                            maxLocations[i] = maxLoc;
                        }
                }
            }
        }


        /// <summary>
        /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole array, selected ROI (in case of IplImage) or, if mask is not IntPtr.Zero, in the specified array region. If the array has more than one channel, it must be IplImage with COI set. In case if multi-dimensional arrays min_loc->x and max_loc->x will contain raw (linear) positions of the extremums
        /// </summary>
        /// <param name="arr">The source array, single-channel or multi-channel with COI set</param>
        /// <param name="minVal">Pointer to returned minimum value</param>
        /// <param name="maxVal">Pointer to returned maximum value</param>
        /// <param name="minLoc">Pointer to returned minimum location</param>
        /// <param name="maxLoc">Pointer to returned maximum location</param>
        /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
        public static void MinMaxLoc(
            IInputArray arr,
            ref double minVal,
            ref double maxVal,
            ref Point minLoc,
            ref Point maxLoc,
            IInputArray mask = null)
        {
            using (InputArray iaArr = arr.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveMinMaxLoc(iaArr, ref minVal, ref maxVal, ref minLoc, ref maxLoc, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMinMaxLoc(
            IntPtr arr,
            ref double minVal,
            ref double maxVal,
            ref Point minLoc,
            ref Point maxLoc,
            IntPtr mask);

        /// <summary>
        /// Copies the source 2D array into interior of destination array and makes a border of the specified type around the copied area. The function is useful when one needs to emulate border type that is different from the one embedded into a specific algorithm implementation. For example, morphological functions, as well as most of other filtering functions in OpenCV, internally use replication border type, while the user may need zero border or a border, filled with 1's or 255's
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The destination image</param>
        /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
        /// <param name="value">Value of the border pixels if bordertype=CONSTANT</param>
        /// <param name="bottom">Parameter specifying how many pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="left">Parameter specifying how many pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="right">Parameter specifying how many pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="top">Parameter specifying how many pixels in each direction from the source image rectangle to extrapolate.</param>
        public static void CopyMakeBorder(
            IInputArray src,
            IOutputArray dst,
            int top, int bottom, int left, int right,
            CvEnum.BorderType bordertype,
            MCvScalar value = new MCvScalar())
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveCopyMakeBorder(iaSrc, oaDst, top, bottom, left, right, bordertype, ref value);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCopyMakeBorder(
            IntPtr src,
            IntPtr dst,
            int top, int bottom, int left, int right,
            CvEnum.BorderType bordertype,
            ref MCvScalar value);

        /// <summary>
        /// Return the particular array element
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index</param>
        /// <returns>the particular array element</returns>
        public static MCvScalar cvGet1D(IntPtr arr, int idx0)
        {
            MCvScalar value = new MCvScalar();
            cveGet1D(arr, idx0, ref value);
            return value;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGet1D(IntPtr arr, int idx0, ref MCvScalar value);

        /// <summary>
        /// Return the particular array element
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index</param>
        /// <param name="idx1">The second zero-based component of the element index</param>
        /// <returns>the particular array element</returns>
        public static MCvScalar cvGet2D(IntPtr arr, int idx0, int idx1)
        {
            MCvScalar value = new MCvScalar();
            cveGet2D(arr, idx0, idx1, ref value);
            return value;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGet2D(IntPtr arr, int idx0, int idx1, ref MCvScalar value);

        /// <summary>
        /// Return the particular array element
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index</param>
        /// <param name="idx1">The second zero-based component of the element index</param>
        /// <param name="idx2">The third zero-based component of the element index</param>
        /// <returns>the particular array element</returns>
        public static MCvScalar cvGet3D(IntPtr arr, int idx0, int idx1, int idx2)
        {
            MCvScalar value = new MCvScalar();
            cveGet3D(arr, idx0, idx1, idx2, ref value);
            return value;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGet3D(IntPtr arr, int idx0, int idx1, int idx2, ref MCvScalar value);

        /// <summary>
        /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <returns>the particular element of single-channel array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetReal1D")]
        public static extern double cvGetReal1D(IntPtr arr, int idx0);

        /// <summary>
        /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <param name="idx1">The second zero-based component of the element index</param>
        /// <returns>the particular element of single-channel array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetReal2D")]
        public static extern double cvGetReal2D(IntPtr arr, int idx0, int idx1);

        /// <summary>
        /// Return the particular element of single-channel array. If the array has multiple channels, runtime error is raised. Note that cvGet*D function can be used safely for both single-channel and multiple-channel arrays though they are a bit slower.
        /// </summary>
        /// <param name="arr">Input array. Must have a single channel</param>
        /// <param name="idx0">The first zero-based component of the element index </param>
        /// <param name="idx1">The second zero-based component of the element index</param>
        /// <param name="idx2">The third zero-based component of the element index </param>
        /// <returns>the particular element of single-channel array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetReal3D")]
        public static extern double cvGetReal3D(IntPtr arr, int idx0, int idx1, int idx2);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveUseOptimized();

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetUseOptimized(
            [MarshalAs(BoolMarshalType)] bool onoff);

        /// <summary>
        /// Enables or disables the optimized code.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use optimized]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>The function can be used to dynamically turn on and off optimized code (code that uses SSE2, AVX, and other instructions on the platforms that support it). It sets a global flag that is further checked by OpenCV functions. Since the flag is not checked in the inner OpenCV loops, it is only safe to call the function on the very top level in your application where you can be sure that no other OpenCV function is currently executed.</remarks>
        public static bool UseOptimized
        {
            get { return cveUseOptimized(); }
            set { cveSetUseOptimized(value); }
        }

        /// <summary>
        /// Returns full configuration time cmake output.
        /// Returned value is raw cmake output including version control system revision, compiler version, compiler flags, enabled modules and third party libraries, etc.Output format depends on target architecture.
        /// </summary>
        public static String BuildInformation
        {
            get
            {
                using (CvString bi = new CvString())
                {
                    cveGetBuildInformation(bi);
                    return bi.ToString();
                }

            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetBuildInformation(IntPtr buildInformation);

        /// <summary>
        /// Fills the array with normally distributed random numbers.
        /// </summary>
        /// <param name="dst">Output array of random numbers; the array must be pre-allocated and have 1 to 4 channels.</param>
        /// <param name="mean">Mean value (expectation) of the generated random numbers.</param>
        /// <param name="stddev">Standard deviation of the generated random numbers; it can be either a vector (in which case a diagonal standard deviation matrix is assumed) or a square matrix.</param>
        public static void Randn(IInputOutputArray dst, MCvScalar mean, MCvScalar stddev)
        {
            using (ScalarArray saMean = new ScalarArray(mean))
            using (ScalarArray saStddev = new ScalarArray(stddev))
            {
                Randn(dst, saMean, saStddev);
            }
        }

        /// <summary>
        /// Fills the array with normally distributed random numbers.
        /// </summary>
        /// <param name="dst">Output array of random numbers; the array must be pre-allocated and have 1 to 4 channels.</param>
        /// <param name="mean">Mean value (expectation) of the generated random numbers.</param>
        /// <param name="stddev">Standard deviation of the generated random numbers; it can be either a vector (in which case a diagonal standard deviation matrix is assumed) or a square matrix.</param>
        public static void Randn(IInputOutputArray dst, IInputArray mean, IInputArray stddev)
        {
            using (InputOutputArray ioaDst = dst.GetInputOutputArray())
            using (InputArray iaMean = mean.GetInputArray())
            using (InputArray iaStddev = stddev.GetInputArray())
            {
                cveRandn(ioaDst, iaMean, iaStddev);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRandn(IntPtr dst, IntPtr mean, IntPtr stddev);

        /// <summary>
        /// Generates a single uniformly-distributed random number or an array of random numbers.
        /// </summary>
        /// <param name="dst">Output array of random numbers; the array must be pre-allocated.</param>
        /// <param name="low">Inclusive lower boundary of the generated random numbers.</param>
        /// <param name="high">Exclusive upper boundary of the generated random numbers.</param>
        public static void Randu(IInputOutputArray dst, MCvScalar low, MCvScalar high)
        {
            using (ScalarArray iaLow = new ScalarArray(low))
            using (ScalarArray iaHigh = new ScalarArray(high))
            {
                Randu(dst, iaLow, iaHigh);
            }
        }

        /// <summary>
        /// Generates a single uniformly-distributed random number or an array of random numbers.
        /// </summary>
        /// <param name="dst">Output array of random numbers; the array must be pre-allocated.</param>
        /// <param name="low">Inclusive lower boundary of the generated random numbers.</param>
        /// <param name="high">Exclusive upper boundary of the generated random numbers.</param>
        public static void Randu(IInputOutputArray dst, IInputArray low, IInputArray high)
        {
            using (InputOutputArray ioaDst = dst.GetInputOutputArray())
            using (InputArray iaLow = low.GetInputArray())
            using (InputArray iaHigh = high.GetInputArray())
            {
                cveRandu(ioaDst, iaLow, iaHigh);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRandu(IntPtr dst, IntPtr low, IntPtr high);

        /*
        /// <summary>
        /// Fills the destination array with uniformly or normally distributed random numbers.
        /// </summary>
        /// <param name="rng">the seed for the random number generator</param>
        /// <param name="arr">The destination array</param>
        /// <param name="distType">Distribution type</param>
        /// <param name="param1">The first parameter of distribution. In case of uniform distribution it is the inclusive lower boundary of random numbers range. In case of normal distribution it is the mean value of random numbers</param>
        /// <param name="param2">The second parameter of distribution. In case of uniform distribution it is the exclusive upper boundary of random numbers range. In case of normal distribution it is the standard deviation of random numbers</param>
  #if ANDROID
        public static void cvRandArr(ref UInt64 rng, IntPtr arr, CvEnum.RandType distType, MCvScalar param1, MCvScalar param2)
        {
           cvRandArr(ref rng, arr, distType, param1.V0, param1.V1, param1.V2, param1.V3, param2.V0, param2.V1, param2.V2, param2.V3);
        }

        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvRandArr(
           ref UInt64 rng, IntPtr arr, CvEnum.RandType dist_type, 
           double param1v0, double param1v1, double param1v2, double param1v3,
           double param2v0, double param2v1, double param2v2, double param2v3);
  #else
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cvRandArr(ref UInt64 rng, IntPtr arr, CvEnum.RandType distType, MCvScalar param1, MCvScalar param2);
  #endif*/

        #region Linear Algebra

        /*
        /// <summary>
        /// Calculates and returns the Euclidean dot product of two arrays.
        /// src1 dot src2 = sumI(src1(I)*src2(I))
        /// In case of multiple channel arrays the results for all channels are accumulated. In particular, cvDotProduct(a,a), where a is a complex vector, will return ||a||2. The function can process multi-dimensional arrays, row by row, layer by layer and so on.
        /// </summary>
        /// <param name="src1">The first source array.</param>
        /// <param name="src2">The second source array</param>
        /// <returns>the Euclidean dot product of two arrays</returns>
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern double cvDotProduct(IntPtr src1, IntPtr src2);*/

        /// <summary>
        /// Computes eigenvalues and eigenvectors of a symmetric matrix
        /// </summary>
        /// <param name="src">The input symmetric square matrix, modified during the processing</param>
        /// <param name="eigenVectors">The output matrix of eigenvectors, stored as subsequent rows</param>
        /// <param name="eigenValues">The output vector of eigenvalues, stored in the descending order (order of eigenvalues and eigenvectors is syncronized, of course)</param>
        /// <remarks>Currently the function is slower than cvSVD yet less accurate, so if A is known to be positivelydefined (for example, it is a covariance matrix)it is recommended to use cvSVD to find eigenvalues and eigenvectors of A, especially if eigenvectors are not required.</remarks>
        /// <example>To calculate the largest eigenvector/-value set lowindex = highindex = 1. For legacy reasons this function always returns a square matrix the same size as the source matrix with eigenvectors and a vector the length of the source matrix with eigenvalues. The selected eigenvectors/-values are always in the first highindex - lowindex + 1 rows.</example>
        public static void Eigen(
            IInputArray src,
            IOutputArray eigenValues,
            IOutputArray eigenVectors = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaEigenValues = eigenValues.GetOutputArray())
            using (OutputArray oaEigenVectors =
                eigenVectors == null ? OutputArray.GetEmpty() : eigenVectors.GetOutputArray())
                cveEigen(iaSrc, oaEigenValues, oaEigenVectors);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveEigen(IntPtr src, IntPtr eigenValues, IntPtr eigenVectors);

        /// <summary>
        /// normalizes the input array so that it's norm or value range takes the certain value(s).
        /// </summary>
        /// <param name="src">The input array</param>
        /// <param name="dst">The output array; in-place operation is supported</param>
        /// <param name="alpha">The minimum/maximum value of the output array or the norm of output array</param>
        /// <param name="beta">The maximum/minimum value of the output array</param>
        /// <param name="normType">The normalization type</param>
        /// <param name="mask">The operation mask. Makes the function consider and normalize only certain array elements</param>
        /// <param name="dType">Optional depth type for the dst array</param>
        public static void Normalize(
            IInputArray src,
            IOutputArray dst,
            double alpha = 1,
            double beta = 0,
            CvEnum.NormType normType = CvEnum.NormType.L2,
            CvEnum.DepthType dType = CvEnum.DepthType.Default,
            IInputArray mask = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveNormalize(iaSrc, oaDst, alpha, beta, normType, dType, iaMask);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveNormalize(
            IntPtr src,
            IntPtr dst,
            double alpha,
            double beta,
            CvEnum.NormType normType,
            CvEnum.DepthType dType,
            IntPtr mask);

        /*
        /// <summary>
        /// Calculates the cross product of two 3D vectors
        /// </summary>
        /// <param name="src1">The first source vector</param>
        /// <param name="src2">The second source vector</param>
        /// <param name="dst">The destination vect</param>
        [DllImport(OpencvCoreLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cvCrossProduct(IntPtr src1, IntPtr src2, IntPtr dst);
        */
        /// <summary>
        /// Performs generalized matrix multiplication:
        /// dst = alpha*op(src1)*op(src2) + beta*op(src3), where op(X) is X or XT
        /// </summary>
        /// <param name="src1">The first source array. </param>
        /// <param name="src2">The second source array. </param>
        /// <param name="alpha">The scalar</param>
        /// <param name="src3">The third source array (shift). Can be null, if there is no shift.</param>
        /// <param name="beta">The scalar</param>
        /// <param name="dst">The destination array.</param>
        /// <param name="tAbc">The Gemm operation type</param>
        public static void Gemm(
            IInputArray src1,
            IInputArray src2,
            double alpha,
            IInputArray src3,
            double beta,
            IOutputArray dst,
            CvEnum.GemmType tAbc = CvEnum.GemmType.Default)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (InputArray iaSrc3 = src3 == null ? InputArray.GetEmpty() : src3.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())

                cveGemm(iaSrc1, iaSrc2, alpha, iaSrc3, beta, oaDst, tAbc);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGemm(
            IntPtr src1,
            IntPtr src2,
            double alpha,
            IntPtr src3,
            double beta,
            IntPtr dst,
            CvEnum.GemmType tAbc);

        /// <summary>
        /// Performs matrix transformation of every element of array src and stores the results in dst
        /// Both source and destination arrays should have the same depth and the same size or selected ROI size. transmat and shiftvec should be real floating-point matrices.
        /// </summary>
        /// <param name="src">The first source array</param>
        /// <param name="dst">The destination array</param>
        /// <param name="m"> transformation 2x2 or 2x3 floating-point matrix.</param>
        public static void Transform(IInputArray src, IOutputArray dst, IInputArray m)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaM = m.GetInputArray())
                cveTransform(iaSrc, oaDst, iaM);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveTransform(IntPtr src, IntPtr dst, IntPtr transmat);

        /// <summary>
        /// Transforms every element of src in the following way:
        /// (x, y) -> (x'/w, y'/w),
        /// where
        /// (x', y', w') = mat3x3 * (x, y, 1)
        /// and w = w'   if w'!=0,
        ///        inf  otherwise
        /// </summary>
        /// <param name="src">The source points</param>
        /// <param name="mat">3x3 floating-point transformation matrix.</param>
        /// <returns>The destination points</returns>
        public static PointF[] PerspectiveTransform(PointF[] src, IInputArray mat)
        {
            PointF[] dst = new PointF[src.Length];
            GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
            GCHandle destHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
            using (Matrix<float> pointMat = new Matrix<float>(src.Length, 1, 2, handle.AddrOfPinnedObject(), 0))
            using (Mat dstMat = new Mat(dst.Length, 1, DepthType.Cv32F, 2, destHandle.AddrOfPinnedObject(), 8))
            {
                CvInvoke.PerspectiveTransform(pointMat, dstMat, mat);
            }

            handle.Free();
            destHandle.Free();
            return dst;
        }

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
        /// <param name="mat">3x3 or 4x4 floating-point transformation matrix.</param>
        public static void PerspectiveTransform(IInputArray src, IOutputArray dst, IInputArray mat)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMat = mat.GetInputArray())
                cvePerspectiveTransform(iaSrc, oaDst, iaMat);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePerspectiveTransform(IntPtr src, IntPtr dst, IntPtr mat);

        /// <summary>
        /// Calculates the product of src and its transposition.
        /// The function evaluates dst=scale(src-delta)*(src-delta)^T if order=0, and dst=scale(src-delta)^T*(src-delta) otherwise.
        /// </summary>
        /// <param name="src">The source matrix</param>
        /// <param name="dst">The destination matrix</param>
        /// <param name="aTa">Order of multipliers</param>
        /// <param name="delta">An optional array, subtracted from <paramref name="src"/> before multiplication</param>
        /// <param name="scale">An optional scaling</param>
        /// <param name="dtype">Optional depth type of the output array</param>
        public static void MulTransposed(
            IInputArray src,
            IOutputArray dst,
            bool aTa,
            IInputArray delta = null,
            double scale = 1,
            CvEnum.DepthType dtype = CvEnum.DepthType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaDelta = delta == null ? InputArray.GetEmpty() : delta.GetInputArray())
                cveMulTransposed(iaSrc, oaDst, aTa, iaDelta, scale, dtype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMulTransposed(
            IntPtr src,
            IntPtr dst,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool aTa,
            IntPtr delta,
            double scale,
            CvEnum.DepthType dtype);

        /// <summary>
        /// Returns sum of diagonal elements of the matrix <paramref name="mat"/>.
        /// </summary>
        /// <param name="mat">the matrix</param>
        /// <returns>sum of diagonal elements of the matrix src1</returns>
        public static MCvScalar Trace(IInputArray mat)
        {
            MCvScalar trace = new MCvScalar();
            using (InputArray iaMat = mat.GetInputArray())
                cveTrace(iaMat, ref trace);
            return trace;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveTrace(IntPtr mat, ref MCvScalar result);

        /// <summary>
        /// Transposes matrix src1:
        /// dst(i,j)=src(j,i)
        /// Note that no complex conjugation is done in case of complex matrix. Conjugation should be done separately: look at the sample code in cvXorS for example
        /// </summary>
        /// <param name="src">The source matrix</param>
        /// <param name="dst">The destination matrix</param>
        public static void Transpose(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveTranspose(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveTranspose(IntPtr src, IntPtr dst);

        /// <summary>
        /// Returns determinant of the square matrix mat. The direct method is used for small matrices and Gaussian elimination is used for larger matrices. For symmetric positive-determined matrices it is also possible to run SVD with U=V=NULL and then calculate determinant as a product of the diagonal elements of W
        /// </summary>
        /// <param name="mat">The pointer to the matrix</param>
        /// <returns>determinant of the square matrix mat</returns>
        public static double Determinant(IInputArray mat)
        {
            using (InputArray iaMat = mat.GetInputArray())
                return cveDeterminant(iaMat);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveDeterminant(IntPtr mat);

        /// <summary>
        /// Finds the inverse or pseudo-inverse of a matrix. This function inverts the matrix src and stores the result in dst . When the matrix src is singular or non-square, the function calculates the pseudo-inverse matrix (the dst matrix) so that norm(src*dst - I) is minimal, where I is an identity matrix.
        /// </summary>
        /// <param name="src">The input floating-point M x N matrix.</param>
        /// <param name="dst">The output matrix of N x M size and the same type as src.</param>
        /// <param name="method">Inversion method</param>
        /// <returns>
        /// In case of the DECOMP_LU method, the function returns non-zero value if the inverse has been successfully calculated and 0 if src is singular.
        /// In case of the DECOMP_SVD method, the function returns the inverse condition number of src (the ratio of the smallest singular value to the largest singular value) and 0 if src is singular. The SVD method calculates a pseudo-inverse matrix if src is singular.
        /// Similarly to DECOMP_LU, the method DECOMP_CHOLESKY works only with non-singular square matrices that should also be symmetrical and positively defined. In this case, the function stores the inverted matrix in dst and returns non-zero. Otherwise, it returns 0.
        /// </returns>
        public static double Invert(IInputArray src, IOutputArray dst, CvEnum.DecompMethod method)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                return cveInvert(iaSrc, oaDst, method);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveInvert(IntPtr src, IntPtr dst, CvEnum.DecompMethod method);

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
        /// <param name="src">Source MxN matrix</param>
        /// <param name="w">Resulting singular value matrix (MxN or NxN) or vector (Nx1). </param>
        /// <param name="u">Optional left orthogonal matrix (MxM or MxN). If CV_SVD_U_T is specified, the number of rows and columns in the sentence above should be swapped</param>
        /// <param name="v">Optional right orthogonal matrix (NxN)</param>
        /// <param name="flags">Operation flags</param>
        public static void SVDecomp(IInputArray src, IOutputArray w, IOutputArray u, IOutputArray v,
            CvEnum.SvdFlag flags)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaW = w.GetOutputArray())
            using (OutputArray oaU = u.GetOutputArray())
            using (OutputArray oaV = v.GetOutputArray())
            {
                cveSVDecomp(iaSrc, oaW, oaU, oaV, flags);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSVDecomp(IntPtr src, IntPtr w, IntPtr u, IntPtr v, CvEnum.SvdFlag flags);

        /// <summary>
        /// Performs a singular value back substitution.
        /// </summary>
        /// <param name="w">Singular values</param>
        /// <param name="u">Left singular vectors</param>
        /// <param name="vt">Transposed matrix of right singular vectors.</param>
        /// <param name="rhs">Right-hand side of a linear system</param>
        /// <param name="dst">Found solution of the system.</param>
        public static void SVBackSubst(IInputArray w, IInputArray u, IInputArray vt, IInputArray rhs, IOutputArray dst)
        {
            using (InputArray iaW = w.GetInputArray())
            using (InputArray iaU = u.GetInputArray())
            using (InputArray iaVt = vt.GetInputArray())
            using (InputArray iaRhs = rhs.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveSVBackSubst(iaW, iaU, iaVt, iaRhs, oaDst);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSVBackSubst(IntPtr w, IntPtr u, IntPtr vt, IntPtr rhs, IntPtr dst);

        /// <summary>
        /// Calculates the covariance matrix of a set of vectors.
        /// </summary>
        /// <param name="samples">Samples stored either as separate matrices or as rows/columns of a single matrix.</param>
        /// <param name="covar">Output covariance matrix of the type ctype and square size.</param>
        /// <param name="mean">Input or output (depending on the flags) array as the average value of the input vectors.</param>
        /// <param name="flags">Operation flags</param>
        /// <param name="ctype">Type of the matrix</param>
        public static void CalcCovarMatrix(
            IInputArray samples,
            IOutputArray covar,
            IInputOutputArray mean,
            CvEnum.CovarMethod flags,
            CvEnum.DepthType ctype = CvEnum.DepthType.Cv64F)
        {
            using (InputArray iaSamples = samples.GetInputArray())
            using (OutputArray oaCovar = covar.GetOutputArray())
            using (InputOutputArray ioaMean = mean.GetInputOutputArray())
                cveCalcCovarMatrix(iaSamples, oaCovar, ioaMean, flags, ctype);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCalcCovarMatrix(
            IntPtr samples,
            IntPtr covar,
            IntPtr mean,
            CvEnum.CovarMethod flags,
            CvEnum.DepthType ctype);

        /// <summary>
        /// Calculates the weighted distance between two vectors and returns it
        /// </summary>
        /// <param name="v1">The first 1D source vector</param>
        /// <param name="v2">The second 1D source vector</param>
        /// <param name="iconvar">The inverse covariation matrix</param>
        /// <returns>the Mahalanobis distance</returns>
        public static double Mahalanobis(IInputArray v1, IInputArray v2, IInputArray iconvar)
        {
            using (InputArray iaV1 = v1.GetInputArray())
            using (InputArray iaV2 = v2.GetInputArray())
            using (InputArray iaIconvar = iconvar.GetInputArray())
                return cveMahalanobis(iaV1, iaV2, iaIconvar);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveMahalanobis(IntPtr v1, IntPtr v2, IntPtr iconvar);

        /// <summary>
        /// Performs Principal Component Analysis of the supplied dataset.
        /// </summary>
        /// <param name="data">Input samples stored as the matrix rows or as the matrix columns.</param>
        /// <param name="mean">Optional mean value; if the matrix is empty, the mean is computed from the data.</param>
        /// <param name="eigenvectors">The eigenvectors.</param>
        /// <param name="maxComponents">Maximum number of components that PCA should retain; by default, all the components are retained.</param>
        public static void PCACompute(IInputArray data, IInputOutputArray mean, IOutputArray eigenvectors,
            int maxComponents = 0)
        {
            using (InputArray iaData = data.GetInputArray())
            using (InputOutputArray ioaMean = mean.GetInputOutputArray())
            using (OutputArray oaEigenvectors = eigenvectors.GetOutputArray())
                cvePCACompute1(iaData, ioaMean, oaEigenvectors, maxComponents);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePCACompute1(IntPtr data, IntPtr mean, IntPtr eigenvectors, int maxComponents);

        /// <summary>
        /// Performs Principal Component Analysis of the supplied dataset.
        /// </summary>
        /// <param name="data">Input samples stored as the matrix rows or as the matrix columns.</param>
        /// <param name="mean">Optional mean value; if the matrix is empty, the mean is computed from the data.</param>
        /// <param name="eigenvectors">The eigenvectors.</param>
        /// <param name="retainedVariance">Percentage of variance that PCA should retain. Using this parameter will let the PCA decided how many components to retain but it will always keep at least 2.</param>
        public static void PCACompute(IInputArray data, IInputOutputArray mean, IOutputArray eigenvectors,
            double retainedVariance)
        {
            using (InputArray iaData = data.GetInputArray())
            using (InputOutputArray ioaMean = mean.GetInputOutputArray())
            using (OutputArray oaEigenvectors = eigenvectors.GetOutputArray())
                cvePCACompute2(iaData, ioaMean, oaEigenvectors, retainedVariance);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePCACompute2(IntPtr data, IntPtr mean, IntPtr eigenvectors,
            double retainedVariance);

        /// <summary>
        /// Projects vector(s) to the principal component subspace.
        /// </summary>
        /// <param name="data">Input vector(s); must have the same dimensionality and the same layout as the input data used at PCA phase</param>
        /// <param name="mean">The mean.</param>
        /// <param name="eigenvectors">The eigenvectors.</param>
        /// <param name="result">The result.</param>
        public static void PCAProject(IInputArray data, IInputArray mean, IInputArray eigenvectors, IOutputArray result)
        {
            using (InputArray iaData = data.GetInputArray())
            using (InputArray iaMean = mean.GetInputArray())
            using (InputArray iaEigenVectors = eigenvectors.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
                cvePCAProject(iaData, iaMean, iaEigenVectors, oaResult);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePCAProject(IntPtr data, IntPtr mean, IntPtr eigenvectors, IntPtr result);

        /// <summary>
        /// Reconstructs vectors from their PC projections.
        /// </summary>
        /// <param name="data">Coordinates of the vectors in the principal component subspace</param>
        /// <param name="mean">The mean.</param>
        /// <param name="eigenvectors">The eigenvectors.</param>
        /// <param name="result">The result.</param>
        public static void PCABackProject(IInputArray data, IInputArray mean, IInputArray eigenvectors,
            IOutputArray result)
        {
            using (InputArray iaData = data.GetInputArray())
            using (InputArray iaMean = mean.GetInputArray())
            using (InputArray iaEigenVectors = eigenvectors.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
                cvePCABackProject(iaData, iaMean, iaEigenVectors, oaResult);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePCABackProject(IntPtr data, IntPtr mean, IntPtr eigenvectors, IntPtr result);

        #endregion

        /// <summary>
        /// Fills output variables with low-level information about the array data. All output parameters are optional, so some of the pointers may be set to NULL. If the array is IplImage with ROI set, parameters of ROI are returned. 
        /// </summary>
        /// <param name="arr">Array header</param>
        /// <param name="data">Output pointer to the whole image origin or ROI origin if ROI is set</param>
        /// <param name="step">Output full row length in bytes</param>
        /// <param name="roiSize">Output ROI size</param>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetRawData")]
        public static extern void cvGetRawData(IntPtr arr, out IntPtr data, out int step, out Size roiSize);

        /// <summary>
        /// Returns matrix header for the input array that can be matrix - CvMat, image - IplImage or multi-dimensional dense array - CvMatND* (latter case is allowed only if allowND != 0) . In the case of matrix the function simply returns the input pointer. In the case of IplImage* or CvMatND* it initializes header structure with parameters of the current image ROI and returns pointer to this temporary structure. Because COI is not supported by CvMat, it is returned separately. 
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="header">Pointer to CvMat structure used as a temporary buffer</param>
        /// <param name="coi">Optional output parameter for storing COI</param>
        /// <param name="allowNd">If non-zero, the function accepts multi-dimensional dense arrays (CvMatND*) and returns 2D (if CvMatND has two dimensions) or 1D matrix (when CvMatND has 1 dimension or more than 2 dimensions). The array must be continuous</param>
        /// <returns>Returns matrix header for the input array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetMat")]
        public static extern IntPtr cvGetMat(IntPtr arr, IntPtr header, out int coi, int allowNd);

        /// <summary>
        /// Returns image header for the input array that can be matrix - CvMat*, or image - IplImage*.
        /// </summary>
        /// <param name="arr">Input array. </param>
        /// <param name="imageHeader">Pointer to IplImage structure used as a temporary buffer.</param>
        /// <returns>Returns image header for the input array</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveGetImage")]
        public static extern IntPtr cvGetImage(IntPtr arr, IntPtr imageHeader);


        /// <summary>
        /// Checks that every array element is neither NaN nor Infinity. If CV_CHECK_RANGE is set, it also checks that every element is greater than or equal to minVal and less than maxVal. 
        /// </summary>
        /// <param name="arr">The array to check.</param>
        /// <param name="flags">The operation flags, CHECK_NAN_INFINITY or combination of
        /// CHECK_RANGE - if set, the function checks that every value of array is within [minVal,maxVal) range, otherwise it just checks that every element is neither NaN nor Infinity.
        /// CHECK_QUIET - if set, the function does not raises an error if an element is invalid or out of range 
        /// </param>
        /// <param name="minVal">The inclusive lower boundary of valid values range. It is used only if CHECK_RANGE is set.</param>
        /// <param name="maxVal">The exclusive upper boundary of valid values range. It is used only if CHECK_RANGE is set.</param>
        /// <returns>Returns nonzero if the check succeeded, i.e. all elements are valid and within the range, and zero otherwise. In the latter case if CV_CHECK_QUIET flag is not set, the function raises runtime error.</returns>
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cveCheckArr")]
        public static extern int cvCheckArr(IntPtr arr, CvEnum.CheckType flags, double minVal, double maxVal);

#if !UNITY_IOS
        /// <summary>
        /// Get or set the number of threads that are used by parallelized OpenCV functions
        /// </summary>
        /// <remarks>When the argument is zero or negative, and at the beginning of the program, the number of threads is set to the number of processors in the system, as returned by the function omp_get_num_procs() from OpenMP runtime. </remarks>
        public static int NumThreads
        {
            get { return cveGetNumThreads(); }
            set { cveSetNumThreads(value); }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveGetNumThreads();


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetNumThreads(int threadsCount);

        /// <summary>
        /// Returns the index, from 0 to cvGetNumThreads()-1, of the thread that called the function. It is a wrapper for the function omp_get_thread_num() from OpenMP runtime. The retrieved index may be used to access local-thread data inside the parallelized code fragments. 
        /// </summary>
        public static int ThreadNum
        {
            get { return cveGetThreadNum(); }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveGetThreadNum();

        /// <summary>
        /// Returns the number of logical CPUs available for the process.
        /// </summary>
        public static int NumberOfCPUs
        {
            get { return cveGetNumberOfCPUs(); }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveGetNumberOfCPUs();
#endif

        /// <summary>
        /// Compares the corresponding elements of two arrays and fills the destination mask array:
        /// dst(I)=src1(I) op src2(I),
        /// dst(I) is set to 0xff (all '1'-bits) if the particular relation between the elements is true and 0 otherwise. 
        /// All the arrays must have the same type, except the destination, and the same size (or ROI size)
        /// </summary>
        /// <param name="src1">The first image to compare with</param>
        /// <param name="src2">The second image to compare with</param>
        /// <param name="dst">dst(I) is set to 0xff (all '1'-bits) if the particular relation between the elements is true and 0 otherwise.</param>
        /// <param name="cmpOp">The comparison operator type</param>
        public static void Compare(IInputArray src1, IInputArray src2, IOutputArray dst, CvEnum.CmpType cmpOp)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveCompare(iaSrc1, iaSrc2, oaDst, cmpOp);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCompare(IntPtr src1, IntPtr src2, IntPtr dst, CvEnum.CmpType cmpOp);


        /// <summary>
        /// Converts CvMat, IplImage , or CvMatND to Mat.
        /// </summary>
        /// <param name="arr">Input CvMat, IplImage , or CvMatND.</param>
        /// <param name="allowND">When true (default value), CvMatND is converted to 2-dimensional Mat, if it is possible (see the discussion below); if it is not possible, or when the parameter is false, the function will report an error</param>
        /// <param name="copyData">When false (default value), no data is copied and only the new header is created, in this case, the original array should not be deallocated while the new matrix header is used; if the parameter is true, all the data is copied and you may deallocate the original array right after the conversion.</param>
        /// <param name="coiMode">Parameter specifying how the IplImage COI (when set) is handled. If coiMode=0 and COI is set, the function reports an error. If coiMode=1 , the function never reports an error. Instead, it returns the header to the whole original image and you will have to check and process COI manually. </param>
        /// <returns>The Mat header</returns>
        public static Mat CvArrToMat(IntPtr arr, bool copyData = false, bool allowND = true, int coiMode = 0)
        {
            return new Mat(cveArrToMat(arr, copyData, allowND, coiMode), true, false);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveArrToMat(
            IntPtr cvArray,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool copyData,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool allowND,
            int coiMode);

        /// <summary>
        /// Horizontally concatenate two images
        /// </summary>
        /// <param name="src1">The first image</param>
        /// <param name="src2">The second image</param>
        /// <param name="dst">The result image</param>
        public static void HConcat(IInputArray src1, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveHConcat(iaSrc1, iaSrc2, oaDst);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveHConcat(IntPtr src1, IntPtr src2, IntPtr dst);

        /// <summary>
        /// Horizontally concatenate two images
        /// </summary>
        /// <param name="srcs">Input array or vector of matrices. all of the matrices must have the same number of rows and the same depth.</param>
        /// <param name="dst">output array. It has the same number of rows and depth as the src, and the sum of cols of the src. same depth.</param>
        public static void HConcat(Mat[] srcs, IOutputArray dst)
        {
            using (VectorOfMat vm = new VectorOfMat(srcs))
                HConcat(vm, dst);
        }

        /// <summary>
        /// Horizontally concatenate two images
        /// </summary>
        /// <param name="srcs">Input array or vector of matrices. all of the matrices must have the same number of rows and the same depth.</param>
        /// <param name="dst">output array. It has the same number of rows and depth as the src, and the sum of cols of the src. same depth.</param>
        public static void HConcat(IInputArrayOfArrays srcs, IOutputArray dst)
        {
            using (InputArray iaSrcs = srcs.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveHConcat2(iaSrcs, oaDst);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveHConcat2(IntPtr src, IntPtr dst);

        /// <summary>
        /// Vertically concatenate two images
        /// </summary>
        /// <param name="src1">The first image</param>
        /// <param name="src2">The second image</param>
        /// <param name="dst">The result image</param>
        public static void VConcat(IInputArray src1, IInputArray src2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveVConcat(iaSrc1, iaSrc2, oaDst);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveVConcat(IntPtr src1, IntPtr src2, IntPtr dst);

        /// <summary>
        /// The function vertically concatenates two or more matrices
        /// </summary>
        /// <param name="srcs">Input array or vector of matrices. all of the matrices must have the same number of cols and the same depth</param>
        /// <param name="dst">Output array. It has the same number of cols and depth as the src, and the sum of rows of the src. same depth.</param>
        public static void VConcat(Mat[] srcs, IOutputArray dst)
        {
            using (VectorOfMat vm = new VectorOfMat(srcs))
                VConcat(vm, dst);
        }

        /// <summary>
        /// The function vertically concatenates two or more matrices
        /// </summary>
        /// <param name="srcs">Input array or vector of matrices. all of the matrices must have the same number of cols and the same depth</param>
        /// <param name="dst">Output array. It has the same number of cols and depth as the src, and the sum of rows of the src. same depth.</param>
        public static void VConcat(IInputArrayOfArrays srcs, IOutputArray dst)
        {
            using (InputArray iaSrcs = srcs.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveVConcat2(iaSrcs, oaDst);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveVConcat2(IntPtr src, IntPtr dst);

        /// <summary>
        /// Swaps two matrices
        /// </summary>
        /// <param name="m1">The Mat to be swapped</param>
        /// <param name="m2">The Mat to be swapped</param>
        public static void Swap(Mat m1, Mat m2)
        {
            cveSwapMat(m1, m2);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveSwapMat(IntPtr mat1, IntPtr mat2);

        /// <summary>
        /// Swaps two matrices
        /// </summary>
        /// <param name="m1">The UMat to be swapped</param>
        /// <param name="m2">The UMat to be swapped</param>
        public static void Swap(UMat m1, UMat m2)
        {
            cveSwapUMat(m1, m2);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveSwapUMat(IntPtr mat1, IntPtr mat2);

        #region OpenCL

        /// <summary>
        /// Check if we have OpenCL
        /// </summary>
        public static bool HaveOpenCL
        {
            get
            {
                return cveHaveOpenCL();
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private extern static bool cveHaveOpenCL();

        /// <summary>
        /// Get or set if OpenCL should be used
        /// </summary>
        public static bool UseOpenCL
        {
            get
            {
                return cveUseOpenCL();
            }
            set
            {
                cveSetUseOpenCL(value);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private extern static bool cveUseOpenCL();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveSetUseOpenCL([MarshalAs(CvInvoke.BoolMarshalType)] bool flag);

        /// <summary>
        /// Finishes OpenCL queue.
        /// </summary>
        public static void OclFinish()
        {
            cveOclFinish();
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveOclFinish();

        /// <summary>
        /// Get the OpenCL platform summary as a string
        /// </summary>
        /// <returns>An OpenCL platform summary</returns>
        public static String OclGetPlatformsSummary()
        {
            if (!HaveOpenCL)
                return "OpenCL not available.";

            Ocl.Device defaultDevice = Ocl.Device.Default;

            StringBuilder builder = new StringBuilder();
            using (VectorOfOclPlatformInfo oclPlatformsInfo = Ocl.OclInvoke.GetPlatformsInfo())
            {
                if (oclPlatformsInfo.Size > 0)
                {
                    for (int i = 0; i < oclPlatformsInfo.Size; i++)
                    {
                        Ocl.PlatformInfo platformInfo = oclPlatformsInfo[i];
                        builder.Append(String.Format("Platform {0}: {1}{2}", i, platformInfo.ToString(), Environment.NewLine));

                        for (int j = 0; j < platformInfo.DeviceNumber; j++)
                        {
                            Ocl.Device device = platformInfo.GetDevice(j);
                            builder.Append(String.Format("   Device {0} {2}: {1} {3}", j, device.ToString(), device.NativeDevicePointer.Equals(defaultDevice.NativeDevicePointer) ? "(Default)" : String.Empty, Environment.NewLine));
                        }
                    }
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Set the default opencl device
        /// </summary>
        /// <param name="deviceName">The name of the opencl device</param>
        public static void OclSetDefaultDevice(String deviceName)
        {

            using (VectorOfOclPlatformInfo oclPlatformInfos = Ocl.OclInvoke.GetPlatformsInfo())
            {
                if (oclPlatformInfos.Size > 0)
                {
                    for (int i = 0; i < oclPlatformInfos.Size; i++)
                    {
                        Ocl.PlatformInfo platformInfo = oclPlatformInfos[i];

                        for (int j = 0; j < platformInfo.DeviceNumber; j++)
                        {
                            Ocl.Device device = platformInfo.GetDevice(j);
                            if (device.Name.Equals(deviceName))
                            {
                                Ocl.Device.Default.Set(device.NativeDevicePointer);
                                return;
                            }
                        }
                    }
                }
            }
            throw new Exception(String.Format("OpenCL device with name '{0}' is not found.", deviceName));
        }

        /// <summary>
        /// Gets a value indicating whether this device have open CL compatible gpu device.
        /// </summary>
        /// <value><c>true</c> if have open CL compatible gpu device; otherwise, <c>false</c>.</value>
        public static bool HaveOpenCLCompatibleGpuDevice
        {
            get
            {
                if (HaveOpenCL)
                    using (VectorOfOclPlatformInfo oclPlatformInfos = Ocl.OclInvoke.GetPlatformsInfo())
                    {
                        if (oclPlatformInfos.Size > 0)
                        {
                            for (int i = 0; i < oclPlatformInfos.Size; i++)
                            {
                                Ocl.PlatformInfo platformInfo = oclPlatformInfos[i];

                                for (int j = 0; j < platformInfo.DeviceNumber; j++)
                                {
                                    Ocl.Device device = platformInfo.GetDevice(j);
                                    if (device.Type == Ocl.DeviceType.Gpu)
                                        return true;
                                }
                            }
                        }
                    }
                return false;
            }
        }

        #endregion

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGetRangeAll(ref Emgu.CV.Structure.Range range);

        #region Clustering
        /// <summary>
        /// Implements k-means algorithm that finds centers of cluster_count clusters and groups the input samples around the clusters. On output labels(i) contains a cluster index for sample stored in the i-th row of samples matrix
        /// </summary>
        /// <param name="data">Floating-point matrix of input samples, one row per sample</param>
        /// <param name="bestLabels">Output integer vector storing cluster indices for every sample</param>
        /// <param name="termcrit">Specifies maximum number of iterations and/or accuracy (distance the centers move by between the subsequent iterations)</param>
        /// <param name="attempts">The number of attempts. Use 2 if not sure</param>
        /// <param name="flags">Flags, use 0 if not sure</param>
        /// <param name="centers">Pointer to array of centers, use IntPtr.Zero if not sure</param>
        /// <param name="k">Number of clusters to split the set by.</param>
        /// <returns>The function returns the compactness measure. The best (minimum) value is chosen and the corresponding labels and the compactness value are returned by the function. </returns>
        public static double Kmeans(
           IInputArray data,
           int k,
           IOutputArray bestLabels,
           MCvTermCriteria termcrit,
           int attempts,
           CvEnum.KMeansInitType flags,
           IOutputArray centers = null)
        {
            using (InputArray iaData = data.GetInputArray())
            using (OutputArray oaBestLabels = bestLabels.GetOutputArray())
            using (OutputArray oaCenters = centers == null ? OutputArray.GetEmpty() : centers.GetOutputArray())
                return cveKmeans(iaData, k, oaBestLabels, ref termcrit, attempts, flags, oaCenters);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveKmeans(
           IntPtr data,
           int k,
           IntPtr bestLabels,
           ref MCvTermCriteria termcrit,
           int attempts,
           CvEnum.KMeansInitType flags,
           IntPtr centers);
        #endregion
    }
}
