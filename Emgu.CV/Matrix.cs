using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Emgu.CV
{
    /// <summary> 
    /// The Matrix class that wrap around CvMat in OpenCV 
    /// </summary>
    public class Matrix<D> : Array, IEquatable<Matrix<D>> where D : new()
    {
        private D[,] _array;

        /// <summary>
        /// The pinned GcHandle to _array;
        /// </summary>
        private GCHandle _dataHandle;

        /// <summary>
        /// Indicate if _dataHandle is set
        /// </summary>
        private bool _handleSet = false;

        /// <summary>
        /// The default constructor which allows Data to be set later on
        /// </summary>
        protected Matrix()
        {
        }

        /// <summary>
        /// Create a matrix of the specific rows and columns
        /// </summary>
        /// <param name="rows">The number of rows (<b>height</b>)</param>
        /// <param name="cols">The number of cols (<b>width</b>)</param>
        public Matrix(int rows, int cols)
            :this( new D[rows, cols])
        {
        }

        ///<summary> Create a matrix using the specific <paramref>data</paramref></summary>
        public Matrix(D[,] data)
        {
            Data = data;
        }

        #region Properties
        ///<summary> Get the depth representation for openCV</summary>
        protected static CvEnum.MAT_DEPTH CvDepth
        {
            get
            {
                if (typeof(D) == typeof(float))
                    return CvEnum.MAT_DEPTH.CV_32F;
                else if (typeof(D) == typeof(Byte))
                    return CvEnum.MAT_DEPTH.CV_8U;
                else
                {
                    throw new Emgu.Exception(Emgu.ExceptionHeader.CriticalException, "Unsupported matrix depth");
                }
            }
        }

        ///<summary> The Width (Number of rows) of the Matrix </summary>
        public override int Width { get { return CvMat.width; } }

        ///<summary> The Height (Number of columns) of the Matrix </summary>
        public override int Height { get { return CvMat.height; } }

        /// <summary>
        /// The number of rows for this matrix
        /// </summary>
        public int Rows { get { return CvMat.height; } }

        /// <summary>
        /// The number of cols for this matrix
        /// </summary>
        public int Cols { get { return CvMat.width; } }
        #endregion

        /// <summary>
        /// Get or Set the data for this matrix
        /// </summary>
        public D[,] Data
        {
            get
            {
                return _array;
            }
            set
            {
                Debug.Assert(value != null, "The Array cannot be null");
                
                FreeUnmanagedObjects();
                Debug.Assert(!_handleSet, "Handle should should be free");

                _array = value;
                _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                _handleSet = true;
                _ptr = CvInvoke.cvMat(_array.GetLength(0), _array.GetLength(1), CvDepth, _dataHandle.AddrOfPinnedObject());
            }
        }

        ///<summary> Returns the transpose of this matrix</summary>
        public Matrix<D> Transpose()
        {
            Matrix<D> res = new Matrix<D>(Cols, Rows);
            CvInvoke.cvTranspose(_ptr, res._ptr);
            return res;
        }

        /// <summary>
        /// The MCvMat structure format  
        /// </summary>
        public MCvMat CvMat 
        { 
            get 
            { 
                return (MCvMat)Marshal.PtrToStructure(Ptr, typeof(MCvMat)); 
            } 
        }

        /// <summary>
        /// Get or Set the value in the specific <paramref name="row"/> and <paramref name="col"/>
        /// </summary>
        /// <param name="row">the row of the element</param>
        /// <param name="col">the col of the element</param>
        /// <returns></returns>
        public D this[int row, int col]
        {
            get
            {
                return _array[row, col];
            }
            set
            {
                _array[row, col] = value;
            }
        }

        /// <summary>
        /// The function cvDet returns determinant of the square matrix
        /// </summary>
        public double Det
        {
            get
            {
                return CvInvoke.cvDet(Ptr);
            }
        }

        /// <summary>
        /// Release the matrix and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            if (_ptr != IntPtr.Zero)
            {
                Marshal.Release(_ptr);
                _ptr = IntPtr.Zero;
            }

            if (_handleSet)
            {
                _dataHandle.Free();
                _handleSet = false;
            };
        }

        /// <summary>
        /// Return the sum of the elements in this matrix
        /// </summary>
        public double Sum
        {
            get
            {
                return CvInvoke.cvSum(Ptr).v0;
            }
        }

        /// <summary>
        /// This function compare the current image with <paramref name="mat2"/> and returns the comparison mask
        /// </summary>
        /// <param name="mat2">the other matrix to compare with</param>
        /// <param name="type">comparison type</param>
        /// <returns>The comparison mask</returns>
        public Matrix<Byte> Cmp(Matrix<D> mat2, Emgu.CV.CvEnum.CMP_TYPE type)
        {
            Matrix<Byte> res = new Matrix<Byte>(Rows , Cols);
            CvInvoke.cvCmp(Ptr, mat2.Ptr, res.Ptr, type);
            return res;
        }

        /// <summary>
        /// Return true if every element of this matrix equals elements in <paramref name="mat2"/>
        /// </summary>
        /// <param name="mat2">The other matrix to compare with</param>
        /// <returns>true if every element of this matrix equals elements in <paramref name="mat2"/></returns>
        public bool Equals(Matrix<D> mat2)
        {
            if (!EqualSize(mat2)) return false;

            using (Matrix<Byte> neqMask = Cmp(mat2, Emgu.CV.CvEnum.CMP_TYPE.CV_CMP_NE))
            {
                return (neqMask.Sum == 0.0);
            }
        }
    }
}
