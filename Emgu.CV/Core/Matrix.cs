//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary> 
    /// A Matrix is a wrapper to cvMat of OpenCV. 
    /// </summary>
    /// <typeparam name="TDepth">Depth of this matrix (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
    [Serializable]
    public class Matrix<TDepth> : CvArray<TDepth>, IEquatable<Matrix<TDepth>>, ICloneable
        where TDepth : new()
    {
        private TDepth[,] _array;

        private void AllocateHeader()
        {
            if (_ptr == IntPtr.Zero)
            {
                _ptr = Marshal.AllocHGlobal(StructSize.MCvMat);
                GC.AddMemoryPressure(StructSize.MCvMat);
            }
        }

        #region Constructors
        /// <summary>
        /// The default constructor which allows Data to be set later on
        /// </summary>
        protected Matrix()
        {
        }

        /// <summary>
        /// Create a Matrix (only header is allocated) using the Pinned/Unmanaged <paramref name="data"/>. The <paramref name="data"/> is not freed by the disposed function of this class 
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of cols</param>
        /// <param name="data">The Pinned/Unmanaged data, the data must not be release before the Matrix is Disposed</param>
        /// <param name="step">The step (row stride in bytes)</param>
        /// <remarks>The caller is responsible for allocating and freeing the block of memory specified by the data parameter, however, the memory should not be released until the related Matrix is released. </remarks>
        public Matrix(int rows, int cols, IntPtr data, int step)
           : this(rows, cols, 1, data, step)
        {
        }

        /// <summary>
        /// Create a Matrix (only header is allocated) using the Pinned/Unmanaged <paramref name="data"/>. The <paramref name="data"/> is not freed by the disposed function of this class 
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of cols</param>
        /// <param name="channels">The number of channels</param>
        /// <param name="data">The Pinned/Unmanaged data, the data must not be release before the Matrix is Disposed</param>
        /// <param name="step">The step (row stride in bytes)</param>
        /// <remarks>The caller is responsible for allocating and freeing the block of memory specified by the data parameter, however, the memory should not be released until the related Matrix is released. </remarks>
        public Matrix(int rows, int cols, int channels, IntPtr data, int step)
        {
            AllocateHeader();
            CvInvoke.cvInitMatHeader(_ptr, rows, cols, CvInvoke.MakeType(CvInvoke.GetDepthType(typeof(TDepth)), channels), data, step);
        }

        /// <summary>
        /// Create a Matrix (only header is allocated) using the Pinned/Unmanaged <paramref name="data"/>. The <paramref name="data"/> is not freed by the disposed function of this class 
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of cols</param>
        /// <param name="data">The Pinned/Unmanaged data, the data must not be release before the Matrix is Disposed</param>
        /// <remarks>The caller is responsible for allocating and freeing the block of memory specified by the data parameter, however, the memory should not be released until the related Matrix is released. </remarks>
        public Matrix(int rows, int cols, IntPtr data)
           : this(rows, cols, data, 0)
        {
        }

        /*
        /// <summary>
        /// Create a Matrix from the existing CvMat. The user is responsible for releasing the CvMat. 
        /// </summary>
        /// <param name="ptr">Pointer to the CvMat structure </param>
        public Matrix(IntPtr ptr)
        {
            _ptr = ptr;
        }*/

        /// <summary>
        /// Create a matrix of the specific size
        /// </summary>
        /// <param name="rows">The number of rows (<b>height</b>)</param>
        /// <param name="cols">The number of cols (<b>width</b>)</param>
        public Matrix(int rows, int cols)
           : this(rows, cols, 1)
        {
        }

        /// <summary>
        /// Create a matrix of the specific size
        /// </summary>
        /// <param name="size">The size of the matrix</param>
        public Matrix(Size size)
           : this(size.Height, size.Width)
        {
        }

        /// <summary>
        /// Create a matrix of the specific size and channels
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of cols</param>
        /// <param name="channels">The number of channels</param>
        public Matrix(int rows, int cols, int channels)
        {
            AllocateData(rows, cols, channels);
        }

        /// <summary> 
        /// Create a matrix using the specific data. 
        /// </summary>
        /// <param name="data">The data will be used as the Matrix data storage. You need to make sure that the data object live as long as this Matrix object</param>
        /// <remarks>The data will be used as the Matrix data storage. You need to make sure that the data object live as long as this Matrix object</remarks>
        public Matrix(TDepth[,] data)
        {
            Data = data;
        }

        /// <summary>
        /// Create a matrix using the specific <paramref name="data"/>
        /// </summary>
        /// <param name="data">the data for this matrix</param>
        public Matrix(TDepth[] data)
        {
            TDepth[,] mat = new TDepth[data.Length, 1];
            Buffer.BlockCopy(data, 0, mat, 0, data.Length * SizeOfElement);
            /*
            GCHandle hdl1 = GCHandle.Alloc(data, GCHandleType.Pinned);
            GCHandle hdl2 = GCHandle.Alloc(mat, GCHandleType.Pinned);
            CvToolbox.Memcpy(hdl2.AddrOfPinnedObject(), hdl1.AddrOfPinnedObject(), data.Length * SizeOfElement);
            hdl1.Free();
            hdl2.Free();*/
            Data = mat;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get the underneath managed array
        /// </summary>
        public override Array ManagedArray
        {
            get
            {
                return Data;
            }
            set
            {
                TDepth[,] data = value as TDepth[,];
                if (data == null)
                    throw new InvalidCastException(String.Format("Cannot convert ManagedArray to type of {0}[,].", typeof(TDepth).ToString()));
                Data = data;
            }
        }

        /// <summary>
        /// Get or Set the data for this matrix
        /// </summary>
        public TDepth[,] Data
        {
            get
            {
                if (_array != null)
                    return _array;
                else
                {
                    Size s = Size;
                    TDepth[,] data = new TDepth[s.Height, s.Width];
                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    using (Matrix<TDepth> tmp = new Matrix<TDepth>(s.Height, s.Width, dataHandle.AddrOfPinnedObject()))
                    {
                        CvInvoke.cvCopy(_ptr, tmp._ptr, IntPtr.Zero);
                    }
                    dataHandle.Free();
                    return data;
                }
            }
            set
            {
                Debug.Assert(value != null, "The Array cannot be null");

                AllocateHeader();

                if (_dataHandle.IsAllocated)
                    _dataHandle.Free(); //free the data handle
                Debug.Assert(!_dataHandle.IsAllocated, "Handle should be freed");

                _array = value;
                _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);

                CvInvoke.cvInitMatHeader(_ptr, _array.GetLength(0), _array.GetLength(1), CvInvoke.MakeType(CvInvoke.GetDepthType(typeof(TDepth)), 1), _dataHandle.AddrOfPinnedObject(), 0x7fffffff);
            }
        }

        /// <summary>
        /// Get the number of channels for this matrix
        /// </summary>
        public override int NumberOfChannels
        {
            get
            {
                return MCvMat.NumberOfChannels;
            }
        }

        /// <summary>
        /// The MCvMat structure format  
        /// </summary>
        public MCvMat MCvMat
        {
            get
            {
                return (MCvMat)Marshal.PtrToStructure(Ptr, typeof(MCvMat));
            }
        }

        /// <summary>
        /// Returns determinant of the square matrix
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double Det
        {
            get
            {
                return CvInvoke.Determinant(this);
            }
        }

        /// <summary>
        /// Return the sum of the elements in this matrix
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double Sum
        {
            get
            {
                return CvInvoke.Sum(this).V0;
            }
        }
        #endregion

        #region copy and clone
        /// <summary>
        /// Return a matrix of the same size with all elements equals 0
        /// </summary>
        /// <returns>A matrix of the same size with all elements equals 0</returns>
        public Matrix<TDepth> CopyBlank()
        {
            return new Matrix<TDepth>(Rows, Cols, NumberOfChannels);
        }

        /// <summary>
        /// Make a copy of this matrix
        /// </summary>
        /// <returns>A copy if this matrix</returns>
        public virtual Matrix<TDepth> Clone()
        {
            Matrix<TDepth> mat = new Matrix<TDepth>(Rows, Cols, NumberOfChannels);
            CvInvoke.cvCopy(Ptr, mat.Ptr, IntPtr.Zero);
            return mat;
        }
        #endregion

        /// <summary>
        /// Get reshaped matrix which also share the same data with the current matrix
        /// </summary>
        /// <param name="newChannels">the new number of channles</param>
        /// <param name="newRows">The new number of rows</param>
        /// <returns>A reshaped matrix which also share the same data with the current matrix</returns>
        public Matrix<TDepth> Reshape(int newChannels, int newRows)
        {
            Matrix<TDepth> res = new Matrix<TDepth>();
            res._array = _array;
            res.AllocateHeader();
            CvInvoke.cvReshape(Ptr, res.Ptr, newChannels, newRows);
            return res;
        }

        /// <summary>
        /// Convert this matrix to different depth
        /// </summary>
        /// <typeparam name="TOtherDepth">The depth type to convert to</typeparam>
        /// <param name="scale">the scaling factor to apply during conversion (defaults to 1.0 -- no scaling)</param>
        /// <param name="shift">the shift factor to apply during conversion (defaults to 0.0 -- no shifting)</param>
        /// <returns>Matrix of different depth</returns>
        public Matrix<TOtherDepth> Convert<TOtherDepth>(double scale = 1.0, double shift = 0.0)
           where TOtherDepth : new()
        {
            Matrix<TOtherDepth> res = new Matrix<TOtherDepth>(Rows, Cols, NumberOfChannels);
            CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, shift);
            return res;
        }

        /// <summary> Returns the transpose of this matrix</summary>
        /// <returns>The transpose of this matrix</returns>
        public Matrix<TDepth> Transpose()
        {
            Matrix<TDepth> res = new Matrix<TDepth>(Cols, Rows);
            CvInvoke.Transpose(this, res);
            return res;
        }

        /// <summary>
        /// Get or Set the value in the specific <paramref name="row"/> and <paramref name="col"/>
        /// </summary>
        /// <param name="row">the row of the element</param>
        /// <param name="col">the col of the element</param>
        /// <returns>The element on the specific <paramref name="row"/> and <paramref name="col"/></returns>
        public TDepth this[int row, int col]
        {
            get
            {
                return (TDepth)System.Convert.ChangeType(CvInvoke.cvGetReal2D(Ptr, row, col), typeof(TDepth));
            }
            set
            {
                CvInvoke.cvSet2D(Ptr, row, col, new MCvScalar(System.Convert.ToDouble(value)));
            }
        }

        /// <summary>
        /// Allocate data for the array
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        /// <param name="numberOfChannels">The number of channels for this matrix</param>
        protected override void AllocateData(int rows, int cols, int numberOfChannels)
        {
            Data = new TDepth[rows, cols * numberOfChannels];
            if (numberOfChannels > 1)
                CvInvoke.cvReshape(_ptr, _ptr, numberOfChannels, 0);
        }

        #region Accessing Elements and sub-Arrays
        /// <summary>
        /// Get a submatrix corresponding to a specified rectangle
        /// </summary>
        /// <param name="rect">the rectangle area of the sub-matrix</param>
        /// <returns>A submatrix corresponding to a specified rectangle</returns>
        public Matrix<TDepth> GetSubRect(Rectangle rect)
        {
            Matrix<TDepth> subMat = new Matrix<TDepth>();
            subMat._array = _array;
            subMat.AllocateHeader();
            CvInvoke.cvGetSubRect(_ptr, subMat.Ptr, rect);
            return subMat;
        }

        /// <summary>
        /// Get the specific row of the matrix
        /// </summary>
        /// <param name="row">the index of the row to be reterived</param>
        /// <returns>the specific row of the matrix</returns>
        public Matrix<TDepth> GetRow(int row)
        {
            return GetRows(row, row + 1, 1);
        }

        /// <summary>
        /// Return the matrix corresponding to a specified row span of the input array
        /// </summary>
        /// <param name="startRow">Zero-based index of the starting row (inclusive) of the span</param>
        /// <param name="endRow">Zero-based index of the ending row (exclusive) of the span</param>
        /// <param name="deltaRow">Index step in the row span. That is, the function extracts every delta_row-th row from start_row and up to (but not including) end_row</param>
        /// <returns>A matrix corresponding to a specified row span of the input array</returns>
        public Matrix<TDepth> GetRows(int startRow, int endRow, int deltaRow)
        {
            Matrix<TDepth> subMat = new Matrix<TDepth>();
            subMat._array = _array;
            subMat.AllocateHeader();
            subMat._ptr = CvInvoke.cvGetRows(_ptr, subMat.Ptr, startRow, endRow, deltaRow);
            return subMat;
        }

        /// <summary>
        /// Get the specific column of the matrix
        /// </summary>
        /// <param name="col">the index of the column to be reterived</param>
        /// <returns>the specific column of the matrix</returns>
        public Matrix<TDepth> GetCol(int col)
        {
            return GetCols(col, col + 1);
        }

        /// <summary>
        /// Get the Matrix, corresponding to a specified column span of the input array
        /// </summary>
        /// <param name="endCol">Zero-based index of the ending column (exclusive) of the span</param>
        /// <param name="startCol">Zero-based index of the selected column</param>
        /// <returns>the specific column span of the matrix</returns>
        public Matrix<TDepth> GetCols(int startCol, int endCol)
        {
            Matrix<TDepth> subMat = new Matrix<TDepth>();
            subMat._array = _array;
            subMat.AllocateHeader();
            subMat._ptr = CvInvoke.cvGetCols(_ptr, subMat.Ptr, startCol, endCol);
            return subMat;
        }

        /// <summary>
        /// Return the specific diagonal elements of this matrix
        /// </summary>
        /// <param name="diag">Array diagonal. Zero corresponds to the main diagonal, -1 corresponds to the diagonal above the main etc., 1 corresponds to the diagonal below the main etc</param>
        /// <returns>The specific diagonal elements of this matrix</returns>
        public Matrix<TDepth> GetDiag(int diag)
        {
            Matrix<TDepth> subMat = new Matrix<TDepth>();
            subMat._array = _array;
            subMat.AllocateHeader();
            subMat._ptr = CvInvoke.cvGetDiag(_ptr, subMat.Ptr, diag);
            return subMat;
        }

        /// <summary>
        /// Return the main diagonal element of this matrix
        /// </summary>
        /// <returns>The main diagonal element of this matrix</returns>
        public Matrix<TDepth> GetDiag()
        {
            return GetDiag(0);
        }
        #endregion

        #region Removing rows or columns
        /// <summary>
        /// Return the matrix without a specified row span of the input array
        /// </summary>
        /// <param name="startRow">Zero-based index of the starting row (inclusive) of the span</param>
        /// <param name="endRow">Zero-based index of the ending row (exclusive) of the span</param>
        /// <returns>The matrix without a specified row span of the input array</returns>
        public Matrix<TDepth> RemoveRows(int startRow, int endRow)
        {
            if (startRow == 0)
                return GetRows(endRow, Rows, 1);
            if (endRow == Rows)
                return GetRows(0, startRow, 1);

            using (Matrix<TDepth> upper = GetRows(0, startRow, 1))
            using (Matrix<TDepth> lower = GetRows(endRow, Rows, 1))
                return upper.ConcateVertical(lower);
        }

        /// <summary>
        /// Return the matrix without a specified column span of the input array
        /// </summary>
        /// <param name="startCol">Zero-based index of the starting column (inclusive) of the span</param>
        /// <param name="endCol">Zero-based index of the ending column (exclusive) of the span</param>
        /// <returns>The matrix without a specified column span of the input array</returns>
        public Matrix<TDepth> RemoveCols(int startCol, int endCol)
        {
            if (startCol == 0)
                return GetCols(endCol, Cols);
            if (endCol == Cols)
                return GetCols(0, startCol);

            using (Matrix<TDepth> upper = GetCols(0, startCol))
            using (Matrix<TDepth> lower = GetCols(endCol, Cols))
                return upper.ConcateHorizontal(lower);
        }
        #endregion

        #region Matrix convatenation
        /// <summary>
        /// Concate the current matrix with another matrix vertically. If this matrix is n1 x m and <paramref name="otherMatrix"/> is n2 x m, the resulting matrix is (n1+n2) x m.
        /// </summary>
        /// <param name="otherMatrix">The other matrix to concate</param>
        /// <returns>A new matrix that is the vertical concatening of this matrix and <paramref name="otherMatrix"/></returns>
        public Matrix<TDepth> ConcateVertical(Matrix<TDepth> otherMatrix)
        {
            Debug.Assert(Cols == otherMatrix.Cols, "The number of columns must be the same when concatening matrices verticly.");
            Matrix<TDepth> res = new Matrix<TDepth>(Rows + otherMatrix.Rows, Cols);
            using (Matrix<TDepth> subUppper = res.GetRows(0, Rows, 1))
                CopyTo(subUppper);
            using (Matrix<TDepth> subLower = res.GetRows(Rows, res.Rows, 1))
                otherMatrix.CopyTo(subLower);
            return res;
        }

        /// <summary>
        /// Concate the current matrix with another matrix horizontally. If this matrix is n x m1 and <paramref name="otherMatrix"/> is n x m2, the resulting matrix is n x (m1 + m2).
        /// </summary>
        /// <param name="otherMatrix">The other matrix to concate</param>
        /// <returns>A matrix that is the horizontal concatening of this matrix and <paramref name="otherMatrix"/></returns>
        public Matrix<TDepth> ConcateHorizontal(Matrix<TDepth> otherMatrix)
        {
            Debug.Assert(Rows == otherMatrix.Rows, "The number of rows must be the same when concatening matrices horizontally.");
            Matrix<TDepth> res = new Matrix<TDepth>(Rows, Cols + otherMatrix.Cols);
            using (Matrix<TDepth> subLeft = res.GetCols(0, Cols))
                CopyTo(subLeft);
            using (Matrix<TDepth> subRight = res.GetCols(Cols, res.Cols))
                otherMatrix.CopyTo(subRight);
            return res;
        }
        #endregion

        /// <summary>
        /// Returns the min / max locations and values for the matrix
        /// </summary>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="minLocation">The minimum location</param>
        /// <param name="maxLocation">The maximum location</param>
        /// <param name="mask">The optional mask</param>
        public void MinMax(out double minValue, out double maxValue, out Point minLocation, out Point maxLocation, IInputArray mask = null)
        {
            //minValue = 0; maxValue = 0;
            minLocation = new Point(); maxLocation = new Point();
            int[] minArr = new int[2], maxArr = new int[2];
            CvInvoke.MinMaxIdx(this, out minValue, out maxValue, minArr, maxArr, mask);
            minLocation.X = minArr[1]; minLocation.Y = minArr[0];
            maxLocation.X = maxArr[1]; maxLocation.Y = maxArr[0];
            //CvInvoke.cvMinMaxLoc(Ptr, ref minValue, ref maxValue, ref minLocation, ref maxLocation, IntPtr.Zero);
        }

        #region Addition
        /// <summary> Elementwise add another matrix with the current matrix </summary>
        /// <param name="mat2">The matrix to be added to the current matrix</param>
        /// <returns> The result of elementwise adding mat2 to the current matrix</returns>
        public Matrix<TDepth> Add(Matrix<TDepth> mat2)
        {
            Matrix<TDepth> res = CopyBlank();
            CvInvoke.Add(this, mat2, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise add a color <paramref name="val"/> to the current matrix</summary>
        /// <param name="val">The value to be added to the current matrix</param>
        /// <returns> The result of elementwise adding <paramref name="val"/> from the current matrix</returns>
        public Matrix<TDepth> Add(TDepth val)
        {
            Matrix<TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(System.Convert.ToDouble(val)))
            {
                CvInvoke.Add(this, ia, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }
        #endregion

        #region Subtraction
        /// <summary> Elementwise subtract another matrix from the current matrix </summary>
        /// <param name="mat2"> The matrix to be subtracted to the current matrix</param>
        /// <returns> The result of elementwise subtracting mat2 from the current matrix</returns>
        public Matrix<TDepth> Sub(Matrix<TDepth> mat2)
        {
            Matrix<TDepth> res = CopyBlank();
            CvInvoke.Subtract(this, mat2, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise subtract a color <paramref name="val"/> to the current matrix</summary>
        /// <param name="val"> The value to be subtracted from the current matrix</param>
        /// <returns> The result of elementwise subtracting <paramref name="val"/> from the current matrix</returns>
        public Matrix<TDepth> Sub(TDepth val)
        {
            Matrix<TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(System.Convert.ToDouble(val)))
            {
                CvInvoke.Subtract(this, ia, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }

        /// <summary>
        /// result = val - this
        /// </summary>
        /// <param name="val">The value which subtract this matrix</param>
        /// <returns>val - this</returns>
        public Matrix<TDepth> SubR(TDepth val)
        {
            Matrix<TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(System.Convert.ToDouble(val)))
            {
                CvInvoke.Subtract(ia, this, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }
        #endregion

        #region Multiplication
        /// <summary> Multiply the current matrix with <paramref name="scale"/></summary>
        /// <param name="scale">The scale to be multiplied</param>
        /// <returns> The scaled matrix </returns>
        public Matrix<TDepth> Mul(double scale)
        {
            Matrix<TDepth> res = CopyBlank();
            CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, 0.0);
            return res;
        }

        /// <summary> Multiply the current matrix with <paramref name="mat2"/></summary>
        /// <param name="mat2">The matrix to be multiplied</param>
        /// <returns> Result matrix of the multiplication </returns>
        public Matrix<TDepth> Mul(Matrix<TDepth> mat2)
        {
            Matrix<TDepth> res = new Matrix<TDepth>(Rows, mat2.Cols);
            CvInvoke.Gemm(this, mat2, 1.0, null, 0.0, res, Emgu.CV.CvEnum.GemmType.Default);
            return res;
        }
        #endregion

        #region Operator overload
        /// <summary>
        /// Elementwise add <paramref name="mat1"/> with <paramref name="mat2"/>
        /// </summary>
        /// <param name="mat1">The Matrix to be added</param>
        /// <param name="mat2">The Matrix to be added</param>
        /// <returns>The elementwise sum of the two matrices</returns>
        public static Matrix<TDepth> operator +(Matrix<TDepth> mat1, Matrix<TDepth> mat2)
        {
            return mat1.Add(mat2);
        }

        /// <summary>
        /// Elementwise add <paramref name="mat1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="mat1">The Matrix to be added</param>
        /// <param name="val">The value to be added</param>
        /// <returns>The matrix plus the value</returns>
        public static Matrix<TDepth> operator +(Matrix<TDepth> mat1, double val)
        {
            return mat1.Add((TDepth)System.Convert.ChangeType(val, typeof(TDepth)));
        }

        /// <summary>
        /// <paramref name="val"/> + <paramref name="mat1"/>
        /// </summary>
        /// <param name="mat1">The Matrix to be added</param>
        /// <param name="val">The value to be added</param>
        /// <returns>The matrix plus the value</returns>
        public static Matrix<TDepth> operator +(double val, Matrix<TDepth> mat1)
        {
            return mat1.Add((TDepth)System.Convert.ChangeType(val, typeof(TDepth)));
        }

        /// <summary>
        /// <paramref name="val"/> - <paramref name="mat1"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be subtracted</param>
        /// <param name="val">The value to be subtracted</param>
        /// <returns><paramref name="val"/> - <paramref name="mat1"/></returns>
        public static Matrix<TDepth> operator -(double val, Matrix<TDepth> mat1)
        {
            return mat1.SubR((TDepth)System.Convert.ChangeType(val, typeof(TDepth)));
        }

        /// <summary>
        /// <paramref name="mat1"/> - <paramref name="mat2"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be subtracted</param>
        /// <param name="mat2">The matrix to subtract</param>
        /// <returns><paramref name="mat1"/> - <paramref name="mat2"/></returns>
        public static Matrix<TDepth> operator -(Matrix<TDepth> mat1, Matrix<TDepth> mat2)
        {
            return mat1.Sub(mat2);
        }

        /// <summary>
        /// <paramref name="mat1"/> - <paramref name="val"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be subtracted</param>
        /// <param name="val">The value to be subtracted</param>
        /// <returns><paramref name="mat1"/> - <paramref name="val"/></returns>
        public static Matrix<TDepth> operator -(Matrix<TDepth> mat1, double val)
        {
            return mat1.Sub((TDepth)System.Convert.ChangeType(val, typeof(TDepth)));
        }

        /// <summary>
        /// <paramref name="mat1"/> * <paramref name="val"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be multiplied</param>
        /// <param name="val">The value to be multiplied</param>
        /// <returns><paramref name="mat1"/> * <paramref name="val"/></returns>
        public static Matrix<TDepth> operator *(Matrix<TDepth> mat1, double val)
        {
            return mat1.Mul(val);
        }

        /// <summary>
        ///  <paramref name="val"/> * <paramref name="mat1"/> 
        /// </summary>
        /// <param name="mat1">The matrix to be multiplied</param>
        /// <param name="val">The value to be multiplied</param>
        /// <returns> <paramref name="val"/> * <paramref name="mat1"/> </returns>
        public static Matrix<TDepth> operator *(double val, Matrix<TDepth> mat1)
        {
            return mat1.Mul(val);
        }

        /// <summary>
        /// <paramref name="mat1"/> / <paramref name="val"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be divided</param>
        /// <param name="val">The value to be divided</param>
        /// <returns><paramref name="mat1"/> / <paramref name="val"/></returns>
        public static Matrix<TDepth> operator /(Matrix<TDepth> mat1, double val)
        {
            return mat1.Mul(1.0 / val);
        }

        /// <summary>
        /// <paramref name="mat1"/> * <paramref name="mat2"/> 
        /// </summary>
        /// <param name="mat1">The Matrix to be multiplied</param>
        /// <param name="mat2">The Matrix to be multiplied</param>
        /// <returns><paramref name="mat1"/> * <paramref name="mat2"/></returns>
        public static Matrix<TDepth> operator *(Matrix<TDepth> mat1, Matrix<TDepth> mat2)
        {
            return mat1.Mul(mat2);
        }
        #endregion

        #region Implement ISerializable interface
        /// <summary>
        /// Constructor used to deserialize runtime serialized object
        /// </summary>
        /// <param name="info">The serialization info</param>
        /// <param name="context">The streaming context</param>
        public Matrix(SerializationInfo info, StreamingContext context)
        {
            DeserializeObjectData(info, context);
        }
        #endregion

        #region UnmanagedObject
        /// <summary>
        /// Release the matrix and all the memory associate with it
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_ptr);
                GC.RemoveMemoryPressure(StructSize.MCvMat);
                _ptr = IntPtr.Zero;
            }

            _array = null;
        }
        #endregion

        #region Comparison
        /// <summary>
        /// This function compare the current matrix with <paramref name="mat2"/> and returns the comparison mask
        /// </summary>
        /// <param name="mat2">The other matrix to compare with</param>
        /// <param name="type">Comparison type</param>
        /// <returns>The comparison mask</returns>
        public Matrix<Byte> Cmp(Matrix<TDepth> mat2, Emgu.CV.CvEnum.CmpType type)
        {
            Matrix<Byte> res = new Matrix<Byte>(Rows, Cols);
            CvInvoke.Compare(this, mat2, res, type);
            return res;
        }

        /// <summary>
        /// Get all channels for the multi channel matrix
        /// </summary>
        /// <returns>Each individual channel of this matrix</returns>
        public Matrix<TDepth>[] Split()
        {
            int channelCount = NumberOfChannels;

            Matrix<TDepth>[] channels = new Matrix<TDepth>[channelCount];
            using (VectorOfMat vm = new VectorOfMat())
            {
                for (int i = 0; i < channelCount; i++)
                {
                    channels[i] = new Matrix<TDepth>(Rows, Cols);
                    vm.Push(channels[i].Mat);
                }
                CvInvoke.Split(this, vm);
            }
            return channels;
        }

        /// <summary>
        /// Return true if every element of this matrix equals elements in <paramref name="mat2"/>
        /// </summary>
        /// <param name="mat2">The other matrix to compare with</param>
        /// <returns>true if every element of this matrix equals elements in <paramref name="mat2"/></returns>
        public bool Equals(Matrix<TDepth> mat2)
        {
            if (!Size.Equals(mat2.Size)) return false;
            int numberOfChannels = NumberOfChannels;
            if (numberOfChannels != mat2.NumberOfChannels) return false;

            using (Matrix<TDepth> xor = new Matrix<TDepth>(Rows, Cols, numberOfChannels))
            {
                CvInvoke.BitwiseXor(this, mat2, xor, null);

                if (numberOfChannels == 1)
                {
                    return CvInvoke.CountNonZero(xor) == 0;
                }
                else
                {  //comapre channel by channel
                    Matrix<TDepth>[] channels = xor.Split();
                    try
                    {
                        for (int i = 0; i < numberOfChannels; i++)
                            if (CvInvoke.CountNonZero(channels[i]) != 0)
                                return false;

                        return true;
                    }
                    finally
                    {
                        foreach (Matrix<TDepth> channel in channels)
                            channel.Dispose();
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        /// <summary> 
        /// Get the size of the array
        /// </summary>
        public override System.Drawing.Size Size
        {
            get
            {
                //TODO: this override should not be necessary if cvGetSize is working correctly, need to check when this will be fixed.
                MCvMat cvMat = MCvMat;
                return new Size(cvMat.Width, cvMat.Height);

            }
        }
    }
}
