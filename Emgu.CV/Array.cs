using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using zlib;

namespace Emgu.CV
{
    ///<summary>
    ///The Array class that wrap around CvArr in OpenCV
    ///</summary>
    public abstract class CvArray<D> : UnmanagedObject, IXmlSerializable, ISerializable
    {
        /// <summary>
        /// The pinned GcHandle to _array;
        /// </summary>
        protected GCHandle _dataHandle;

        ///<summary> The pointer to the internal structure </summary>
        public new IntPtr Ptr { get { return _ptr; } set { _ptr = value; } }

        #region properties
        ///<summary> 
        /// The width of the Array 
        ///</summary>
        public abstract int Width { get; }
        
        ///<summary> 
        /// The height of the Array 
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// The number of rows for this array
        /// </summary>
        public int Rows { get { return Height; } }

        /// <summary>
        /// The number of cols for this array
        /// </summary>
        public int Cols { get { return Width; } }

        /// <summary>
        /// Get or Set an Array of bytes that represent the data in this array
        /// </summary>
        /// <remarks> Should only be used for serialization &amp; deserialization</remarks>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Byte[] Bytes
        {
            get
            {
                int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(D)) * ManagedArray.Length;
                Byte[] res = new Byte[size];
                Marshal.Copy(_dataHandle.AddrOfPinnedObject(), res, 0, size);
                return res;
            }
            set
            {
                int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(D)) * ManagedArray.Length;
                Marshal.Copy(value, 0, _dataHandle.AddrOfPinnedObject(), size);
            }
        }

        /// <summary>
        /// Get the underneath managed array
        /// </summary>
        public abstract System.Array ManagedArray { get; }

        /// <summary>
        /// Allocate data for the array
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        protected abstract void AllocateData(int rows, int cols);

        ///<summary>
        ///The binary data in compressed format
        ///</summary>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Byte[] CompressedBytes
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //GZipStream  compressedStream = new GZipStream(ms , CompressionMode.Compress);
                    using (zlib.ZOutputStream compressedStream = new zlib.ZOutputStream(ms, zlib.zlibConst.Z_BEST_COMPRESSION))
                    {
                        Byte[] data = Bytes;
                        compressedStream.Write(data, 0, data.Length);
                        compressedStream.Flush();
                    }
                    return ms.ToArray();
                }
            }
            set
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (zlib.ZOutputStream compressedStream = new zlib.ZOutputStream(ms))
                    {
                        compressedStream.Write(value, 0, value.Length);
                        Bytes = ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// sum of diagonal elements of the matrix 
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MCvScalar Trace
        {
            get
            {
                return CvInvoke.cvTrace(Ptr);
            }
        }

        ///<summary> 
        ///The norm of this Array 
        ///</summary>
        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double Norm
        {
            get
            {
                return CvInvoke.cvNorm(Ptr, IntPtr.Zero, 4, IntPtr.Zero);
            }
        }
        #endregion

        /// <summary>
        /// Calculates and returns the Euclidean dot product of two arrays.
        /// src1•src2 = sumI(src1(I)*src2(I))
        /// In case of multiple channel arrays the results for all channels are accumulated. In particular, cvDotProduct(a,a), where a is a complex vector, will return ||a||2. The function can process multi-dimensional arrays, row by row, layer by layer and so on.
        /// </summary>
        /// <param name="src2">The other Array to apply dot product with</param>
        /// <returns>src1•src2</returns>
        public double DotProduct(CvArray<D> src2)
        {
            return CvInvoke.cvDotProduct(Ptr, src2.Ptr);
        }

        #region Coping and filling
        ///<summary>
        /// Copy the current image to another one 
        /// </summary>
        /// <param name="dest"> The destination Array</param>
        public void Copy(CvArray<D> dest)
        {
            CvInvoke.cvCopy(Ptr, dest.Ptr, IntPtr.Zero);
        }

        ///<summary> 
        ///Set the element of the Array to <paramref name="val"/>
        ///</summary>
        ///<param name="val"> The value to be set for each element of the Array </param>
        public void SetValue(MCvScalar val)
        {
            CvInvoke.cvSet( _ptr, val, IntPtr.Zero);
        }

        ///<summary> 
        ///Set the element of the Array to <paramref name="val"/>
        ///</summary>
        ///<param name="val"> The value to be set for each element of the Array </param>
        public void SetValue(double val)
        {
            SetValue(new MCvScalar(val, val, val, val));
        }

        ///<summary>
        ///Set the element of the Array to <paramref name="val"/>, using the specific <paramref name="mask"/>
        ///</summary>
        ///<param name="val">The value to be set</param>
        ///<param name="mask">The mask for the operation</param>
        public void SetValue(MCvScalar val, CvArray<Byte> mask)
        {
            CvInvoke.cvSet( _ptr, val, mask.Ptr);
        }

        ///<summary>
        ///Set the element of the Array to <paramref name="val"/>, using the specific <paramref name="mask"/>
        ///</summary>
        ///<param name="val">The value to be set</param>
        ///<param name="mask">The mask for the operation</param>
        public void SetValue(double val, CvArray<Byte> mask)
        {
            SetValue(new MCvScalar(val, val, val, val), mask);
        }

        /// <summary>
        /// Inplace fills Array with uniformly distributed random numbers
        /// </summary>
        /// <param name="seed">Seed for the random number generator</param>
        /// <param name="floorValue">the inclusive lower boundary of random numbers range</param>
        /// <param name="ceilingValue">the exclusive upper boundary of random numbers range</param>
        public void _RandUniform(UInt64 seed, MCvScalar floorValue, MCvScalar ceilingValue)
        {
            CvInvoke.cvRandArr(ref seed, Ptr, CvEnum.RAND_TYPE.CV_RAND_UNI, floorValue, ceilingValue);
        }

        /// <summary>
        /// Inplace fills Array with uniformly distributed random numbers
        /// </summary>
        /// <param name="floorValue">the inclusive lower boundary of random numbers range</param>
        /// <param name="ceilingValue">the exclusive upper boundary of random numbers range</param>
        public void _RandUniform(MCvScalar floorValue, MCvScalar ceilingValue)
        {
            _RandUniform((UInt64) DateTime.Now.Ticks, floorValue, ceilingValue);
        }

        /// <summary>
        /// Inplace fills Array with normally distributed random numbers
        /// </summary>
        /// <param name="seed">Seed for the random number generator</param>
        /// <param name="mean">the mean value of random numbers</param>
        /// <param name="std"> the standard deviation of random numbers</param>
        public void _RandNormal(UInt64 seed, MCvScalar mean, MCvScalar std)
        {
            CvInvoke.cvRandArr(ref seed, Ptr, CvEnum.RAND_TYPE.CV_RAND_NORMAL, mean, std);
        }

        /// <summary>
        /// Inplace fills Array with normally distributed random numbers
        /// </summary>
        /// <param name="mean">the mean value of random numbers</param>
        /// <param name="std"> the standard deviation of random numbers</param>
        public void _RandNormal(MCvScalar mean, MCvScalar std)
        {
            _RandNormal( (UInt64) DateTime.Now.Ticks, mean, std);
        }

        /// <summary>
        /// Initializs scaled identity matrix
        /// </summary>
        /// <param name="value"></param>
        public void _SetIdentity(MCvScalar value)
        {
            CvInvoke.cvSetIdentity(Ptr, value);
        }
        #endregion

        /// <summary>
        /// Inplace multiply elements of the Array by <paramref name="scale"/>
        /// </summary>
        /// <param name="scale">The scale to be multiplyed</param>
        public void _Mul(double scale)
        {
            CvInvoke.cvConvertScale(Ptr, Ptr, scale, 0.0);
        }

        /// <summary>
        /// Inplace elementwise multiply the current Array with <paramref name="src2"/>
        /// </summary>
        /// <param name="src2">The other array to be elementwise multiplied with</param>
        public void _Mul(CvArray<D> src2)
        {
            CvInvoke.cvMul(Ptr, src2.Ptr, Ptr, 1.0);
        }

        #region UnmanagedObject
        /// <summary>
        /// Free the _dataHandle if it is set
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            if (_dataHandle.IsAllocated)
            {
                _dataHandle.Free();
            };
        }
        #endregion

        #region Comparison
        ///<summary>
        ///Inplace compute the elementwise minimum value 
        ///</summary>
        public void _Min(double val)
        {
            CvInvoke.cvMinS(Ptr, val, Ptr);
        }

        /// <summary>
        /// Inplace elementwise minimize the current Array with <paramref name="src2"/>
        /// </summary>
        /// <param name="src2">The other array to be elementwise minimized with this array</param>
        public void _Min(CvArray<D> src2)
        {
            CvInvoke.cvMin(Ptr, src2.Ptr, Ptr);
        }

        /// <summary>
        /// Inplace compute the elementwise maximum value with <paramref name="val"/>
        /// </summary>
        /// <param name="val">The value to be compare with</param>
        public void _Max(double val)
        {
            CvInvoke.cvMaxS(Ptr, val, Ptr);
        }

        /// <summary>
        /// Inplace elementwise maximize the current Array with <paramref name="src2"/>
        /// </summary>
        /// <param name="src2">The other array to be elementwise maximized with this array</param>
        public void _Max(CvArray<D> src2)
        {
            CvInvoke.cvMax(Ptr, src2.Ptr, Ptr);
        }

        ///<summary>
        ///Determine if the size (width and height) of <i>this</i> Array
        ///equals the size of <paramref name="src2"/>
        ///</summary>
        ///<param name="src2"> The other Array to compare size with</param>
        ///<returns> True if the two Array has the same size</returns>
        public bool EqualSize<D2>(CvArray<D2> src2)
        {
            return (Width == src2.Width && Height == src2.Height);
        }
        #endregion

        #region Inplace Logic Operators
        /// <summary>
        /// Inplace And operation with <paramref name="src2"/>
        /// </summary>
        /// <param name="src2">The other array to perform And operation</param>
        public void _And(CvArray<D> src2)
        {
            CvInvoke.cvAnd(Ptr, src2.Ptr, Ptr, IntPtr.Zero);
        }

        /// <summary>
        /// Inplace Or operation with <paramref name="src2"/>
        /// </summary>
        /// <param name="src2">The other array to perform And operation</param>
        public void _Or(CvArray<D> src2)
        {
            CvInvoke.cvOr(Ptr, src2.Ptr, Ptr, IntPtr.Zero);
        }

        ///<summary> 
        ///Inplace compute the complement for all Array Elements
        ///</summary>
        public void _Not()
        {
            CvInvoke.cvNot(Ptr, Ptr);
        }

        #endregion 
    
        #region IXmlSerializable Members

        /// <summary>
        /// Get the xml schema
        /// </summary>
        /// <returns>the xml schema</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Function to call when deserializing this object from XML
        /// </summary>
        /// <param name="reader">The xml reader</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            #region read the size of the matrix and assign storage
            reader.MoveToFirstAttribute();
            int rows = reader.ReadContentAsInt();
            reader.MoveToNextAttribute();
            int cols = reader.ReadContentAsInt();
            System.Xml.XmlNodeType type = reader.MoveToContent();
            AllocateData(rows, cols);
            #endregion

            #region decode the data from Xml and assign the value to the matrix
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(D)) *  ManagedArray.Length;
            Byte[] bytes = new Byte[size];
            reader.ReadToFollowing("Bytes");
            reader.ReadElementContentAsBase64(bytes, 0, bytes.Length);
            Bytes = bytes;
            #endregion
        }

        /// <summary>
        /// Function to call when serializing this object to XML 
        /// </summary>
        /// <param name="writer">The xml writer</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Rows", Rows.ToString());
            writer.WriteAttributeString("Cols", Cols.ToString());
            writer.WriteStartElement("Bytes");
            Byte[] bytes = Bytes;
            writer.WriteBase64(bytes, 0, bytes.Length);
            writer.WriteEndElement();
        }
        #endregion

        #region ISerializable Members
        /// <summary>
        /// A function used for runtime serilization of the object
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Rows", Rows);
            info.AddValue("Cols", Cols);
            info.AddValue("CompressedBinary", CompressedBytes);
        }

        /// <summary>
        /// A function used for runtime deserailization of the object
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected void DeserializeObjectData(SerializationInfo info, StreamingContext context)
        {
            int rows = (int)info.GetValue("Rows", typeof(int));
            int cols = (int)info.GetValue("Cols", typeof(int));
            AllocateData(rows, cols);

            CompressedBytes = (Byte[])info.GetValue("CompressedBinary", typeof(Byte[]));
        }
        #endregion
    }
}
